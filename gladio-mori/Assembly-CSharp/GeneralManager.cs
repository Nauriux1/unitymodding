using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using kcp2k;
using Mirror;
using Mirror.FizzySteam;
using SimpleFileBrowser;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.Users;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

// Token: 0x0200007F RID: 127
public class GeneralManager : MonoBehaviour
{
	// Token: 0x060003F5 RID: 1013 RVA: 0x00012D2A File Offset: 0x00010F2A
	private void Awake()
	{
		this.InitializeGeneralManager();
	}

	// Token: 0x060003F6 RID: 1014 RVA: 0x00012D34 File Offset: 0x00010F34
	public void InitializeGeneralManager()
	{
		if (GeneralManager.singleton != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		GeneralManager.singleton = this;
		this.LoadUserControl();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		SceneManager.activeSceneChanged += this.OnSceneChanged;
		SceneManager.sceneLoaded += this.OnSceneLoaded;
		this.InitializeInputActions();
		this.InitializeBannedWords();
		Debug.Log("General manager has been setup");
	}

	// Token: 0x060003F7 RID: 1015 RVA: 0x00012DAC File Offset: 0x00010FAC
	public void LoadUserControl()
	{
		if (this.userControls != null)
		{
			this.userControls.Disable();
			this.userControls.Dispose();
		}
		this.userControls = SettingsHelper.GetUserControls();
		if (this.userControlsEnabled)
		{
			this.userControls.General.Enable();
			this.userControls.Generic.Enable();
		}
	}

	// Token: 0x060003F8 RID: 1016 RVA: 0x00012E10 File Offset: 0x00011010
	private void OnDestroy()
	{
		SceneManager.activeSceneChanged -= this.OnSceneChanged;
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
		this.DisposeUserControls();
	}

	// Token: 0x060003F9 RID: 1017 RVA: 0x00012E3A File Offset: 0x0001103A
	public void DisposeUserControls()
	{
		if (this.userControls != null)
		{
			this.userControls.Disable();
			this.userControls.Dispose();
		}
	}

	// Token: 0x060003FA RID: 1018 RVA: 0x00012E5C File Offset: 0x0001105C
	private void OnSceneChanged(Scene scene1, Scene scene2)
	{
		if (ReplayManager.singleton != null)
		{
			ReplayManager.singleton.StopRecording(scene2.name.Contains("map_"));
		}
		if (ReplayManager.singleton != null && scene2.name.Contains("map_"))
		{
			ReplayManager.singleton.InitializeRecording();
		}
		this.registeredPlayerHealths.Clear();
		if (PostProcessingManager.singleton != null)
		{
			PostProcessingManager.singleton.SetPostProcessingForScene(scene2.name);
		}
		if (MultiplayerChat.singleton != null)
		{
			MultiplayerChat.singleton.SetAlwaysVisible(!scene2.name.Contains("map_"));
			MultiplayerChat.singleton.DeactivateInputField();
		}
		if (scene2.name != "MoveEditor" && scene2.name != "MoveEditorTestMoveSet")
		{
			CommandInvoker.ClearAll();
			if (!scene2.name.Contains("map_"))
			{
				this.SetTimeScale(1f);
			}
		}
		MoveSetHelpers.ClearLoadedMoveSets();
		this.UpdateCursorState();
	}

	// Token: 0x060003FB RID: 1019 RVA: 0x00012F6D File Offset: 0x0001116D
	private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
	{
		SceneManagerWithParameters.loadingGameScene = false;
		GeneralManager.SetVignetteValue(0f, true);
		if (scene.name.Contains("MenuMultiplayer"))
		{
			GeneralManager.CleanUp();
		}
	}

	// Token: 0x060003FC RID: 1020 RVA: 0x00012F98 File Offset: 0x00011198
	public void ShowLoadingBarForOperation(AsyncOperation sceneLoadOperation, string newScene, bool requireAllLoadedMultiplayer = false, bool forceShowLoadingBar = false, bool manuallyHideLoadingBar = false)
	{
		Debug.Log("Loading scene: " + newScene);
		if (newScene.Contains("map_") || forceShowLoadingBar)
		{
			if (this.currentLoadingScreen == null)
			{
				this.currentLoadingScreen = UnityEngine.Object.Instantiate<GameObject>(this.loadingScreenPrefab);
				this.loadingBarSlider = this.currentLoadingScreen.GetComponentInChildren<Slider>();
				this.loadingBarText = this.currentLoadingScreen.GetComponentInChildren<Text>();
				UnityEngine.Object.DontDestroyOnLoad(this.currentLoadingScreen);
			}
			this.currentLoadingScreen.SetActive(true);
			this.loadingBarSlider.maxValue = 1f;
			this.loadingBarSlider.minValue = 0f;
			this.loadingBarSlider.value = 0f;
			if (this.loadingCoroutine != null)
			{
				base.StopCoroutine(this.loadingCoroutine);
			}
			this.loadingCoroutine = base.StartCoroutine(this.ShowLoadingBar(sceneLoadOperation, requireAllLoadedMultiplayer, manuallyHideLoadingBar));
		}
	}

	// Token: 0x060003FD RID: 1021 RVA: 0x0001307A File Offset: 0x0001127A
	private IEnumerator ShowLoadingBar(AsyncOperation sceneLoadOperation, bool requireAllLoadedMultiplayer, bool manuallyHideLoadingBar = false)
	{
		bool waitingForPlayers = false;
		while (sceneLoadOperation == null || !sceneLoadOperation.isDone || requireAllLoadedMultiplayer || manuallyHideLoadingBar)
		{
			if (sceneLoadOperation == null)
			{
				sceneLoadOperation = NetworkManager.loadingSceneAsync;
			}
			else
			{
				if (requireAllLoadedMultiplayer && NetworkClient.connection == null)
				{
					Debug.Log("Connection lost while loading scene");
					this.currentLoadingScreen.SetActive(false);
					SceneManagerWithParameters.LoadScene("MainMenu", null, false, false);
					yield break;
				}
				float num = Mathf.Clamp01(sceneLoadOperation.progress / 0.9f);
				this.loadingBarSlider.value = num;
				string text = string.Format("{0}%", num * 100f);
				if (!waitingForPlayers)
				{
					this.loadingBarText.text = text;
				}
				if ((double)num > 0.9999 && requireAllLoadedMultiplayer && !waitingForPlayers)
				{
					this.loadingBarText.text = LocalizationHelpers.LocalizedText("txt_waiting_for_players", new object[]
					{
						""
					});
					waitingForPlayers = true;
					this.SetLoadingScreenTimeoutTime();
				}
			}
			if (waitingForPlayers)
			{
				if (MultiplayerRoomManager.GetMultiplayerRoomManager() != null && NetworkServer.activeHost && MultiplayerRoomManager.GetMultiplayerRoomManager().TimeUntilTimeout() < 10f)
				{
					int num2 = (int)MultiplayerRoomManager.GetMultiplayerRoomManager().TimeUntilTimeout();
					if (num2 < 0)
					{
						num2 = 0;
					}
					this.loadingBarText.text = LocalizationHelpers.LocalizedText("txt_waiting_for_players", new object[]
					{
						num2
					});
					yield return new WaitForSecondsRealtime(1f);
				}
				if (MultiplayerRoomManager.GetMultiplayerRoomManager() != null && NetworkClient.active && !NetworkServer.active && this.TimeUntilLoadingScreenTimeout() < 10f)
				{
					int num3 = (int)this.TimeUntilLoadingScreenTimeout();
					if (num3 < 0)
					{
						Debug.Log("Loading screen timed out");
						this.currentLoadingScreen.SetActive(false);
						yield break;
					}
					this.loadingBarText.text = LocalizationHelpers.LocalizedText("txt_waiting_for_players", new object[]
					{
						num3
					});
					yield return new WaitForSecondsRealtime(1f);
				}
			}
			yield return null;
		}
		this.currentLoadingScreen.SetActive(false);
		yield break;
	}

	// Token: 0x060003FE RID: 1022 RVA: 0x0001309E File Offset: 0x0001129E
	public void SetLoadingScreenTimeoutTime()
	{
		this.startedTimeout = Time.unscaledTime;
	}

	// Token: 0x060003FF RID: 1023 RVA: 0x000130AB File Offset: 0x000112AB
	public float TimeUntilLoadingScreenTimeout()
	{
		return this.startedTimeout + this.timeoutDuration - Time.unscaledTime;
	}

	// Token: 0x06000400 RID: 1024 RVA: 0x000130C0 File Offset: 0x000112C0
	public void RemoveLoadingScreen()
	{
		if (this.loadingCoroutine != null)
		{
			base.StopCoroutine(this.loadingCoroutine);
		}
		if (this.currentLoadingScreen != null)
		{
			this.currentLoadingScreen.SetActive(false);
		}
	}

	// Token: 0x06000401 RID: 1025 RVA: 0x000130F0 File Offset: 0x000112F0
	public bool CurrentSceneIsGameplayScene()
	{
		string name = SceneManager.GetActiveScene().name;
		return name.Contains("map_") || name == "MoveEditorTestMoveSet" || name == "Tutorial";
	}

	// Token: 0x06000402 RID: 1026 RVA: 0x00013138 File Offset: 0x00011338
	public void UpdateCursorState()
	{
		CursorLockMode lockState = CursorLockMode.None;
		if (this.CurrentSceneIsGameplayScene() && GameMenu.GameMenuCurrentlyHidden && !ReplayManager.ToolsVisible && !GeneralManager.ConfirmDialogOpen())
		{
			lockState = CursorLockMode.Locked;
		}
		Cursor.lockState = lockState;
	}

	// Token: 0x06000403 RID: 1027 RVA: 0x0001316C File Offset: 0x0001136C
	public void UpdateChatVisibility(bool removeFocus = false)
	{
		if (MultiplayerChat.singleton != null)
		{
			if (removeFocus)
			{
				MultiplayerChat.singleton.DeactivateInputField();
				return;
			}
			MultiplayerChat.singleton.UpdateVisibility();
		}
	}

	// Token: 0x06000404 RID: 1028 RVA: 0x00013194 File Offset: 0x00011394
	public static bool InputSystemDisabled()
	{
		bool result = true;
		if (GeneralManager.singleton != null && GeneralManager.singleton.CurrentSceneIsGameplayScene() && GameMenu.GameMenuCurrentlyHidden && (MultiplayerChat.singleton == null || !MultiplayerChat.singleton.currentlyWritingToChat) && !GeneralManager.ConfirmDialogOpen())
		{
			result = false;
		}
		return result;
	}

	// Token: 0x06000405 RID: 1029 RVA: 0x000131E8 File Offset: 0x000113E8
	public void UpdateInputSystemState()
	{
		bool flag = GeneralManager.InputSystemDisabled();
		List<IDisableableInputManager> list = new List<IDisableableInputManager>();
		list.AddRange(UnityEngine.Object.FindObjectsOfType<PlayerMultiplayerInputManager>());
		list.AddRange(UnityEngine.Object.FindObjectsOfType<global::PlayerInputManager>());
		list.AddRange(UnityEngine.Object.FindObjectsOfType<ReplayFreeCamera>());
		list.AddRange(UnityEngine.Object.FindObjectsOfType<ReplayCameraControls>());
		if (flag)
		{
			using (List<IDisableableInputManager>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IDisableableInputManager disableableInputManager = enumerator.Current;
					disableableInputManager.DisableInputManager();
				}
				goto IL_95;
			}
		}
		foreach (IDisableableInputManager disableableInputManager2 in list)
		{
			disableableInputManager2.EnableInputManager();
		}
		IL_95:
		bool enabled = true;
		if (PlayerOptionsManager.singleton != null)
		{
			enabled = false;
		}
		this.EnableUserControls(enabled);
	}

	// Token: 0x06000406 RID: 1030 RVA: 0x000132C0 File Offset: 0x000114C0
	public static bool AllowBackNavigation(InputDevice device = null)
	{
		MultiplayerEventSystem multiplayerEventSystem = null;
		EquipmentPanel[] array = UnityEngine.Object.FindObjectsOfType<EquipmentPanel>();
		if (array.Length != 0)
		{
			Func<InputDevice, bool> <>9__0;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].equipmentSelectPanel.activeInHierarchy)
				{
					if (SceneManagerWithParameters.currentScene == "LobbyLocal" && device != null)
					{
						PlayerInput[] array2 = UnityEngine.Object.FindObjectsOfType<PlayerInput>();
						bool flag = false;
						for (int j = 0; j < array2.Length; j++)
						{
							if (array2[j].devices.Count > 0)
							{
								IEnumerable<InputDevice> source = array2[j].devices;
								Func<InputDevice, bool> predicate;
								if ((predicate = <>9__0) == null)
								{
									predicate = (<>9__0 = ((InputDevice x) => x == device));
								}
								if (source.Where(predicate).FirstOrDefault<InputDevice>() != null)
								{
									MultiplayerEventSystem component = array2[j].uiInputModule.gameObject.GetComponent<MultiplayerEventSystem>();
									if (component.playerRoot == array[i].GetComponentInParent<Canvas>().gameObject)
									{
										flag = true;
										multiplayerEventSystem = component;
										break;
									}
								}
							}
						}
						if (!flag)
						{
							goto IL_115;
						}
					}
					array[i].CloseEquipmentSelectPanel(multiplayerEventSystem);
					return false;
				}
				IL_115:;
			}
		}
		if (EventSystem.current != null && EventSystem.current.isFocused && EventSystem.current.currentSelectedGameObject != null)
		{
			if (EventSystem.current.currentSelectedGameObject.GetComponent<InputField>() != null)
			{
				EventSystem.current.SetSelectedGameObject(null);
				return false;
			}
			if (EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>() != null)
			{
				EventSystem.current.SetSelectedGameObject(null);
				return false;
			}
		}
		BasicConfirmDialog[] array3 = UnityEngine.Object.FindObjectsOfType<BasicConfirmDialog>();
		if (array3.Length != 0)
		{
			int num = 0;
			if (num < array3.Length)
			{
				if (array3[num].cancelButton.isActiveAndEnabled)
				{
					array3[num].cancelButton.onClick.Invoke();
				}
				else
				{
					array3[num].onClick();
				}
				return false;
			}
		}
		EquipmentSelectDialog[] array4 = UnityEngine.Object.FindObjectsOfType<EquipmentSelectDialog>();
		if (array4.Length != 0)
		{
			array4[0].Close();
			return false;
		}
		if (GeneralManager.openDialogs.Count > 0)
		{
			GeneralManager.openDialogs[0].Close();
			return false;
		}
		if (FileBrowser.IsOpen)
		{
			FileBrowser.HideDialog(false);
			return false;
		}
		return true;
	}

	// Token: 0x06000407 RID: 1031 RVA: 0x000134F1 File Offset: 0x000116F1
	public static void DialogCreated(IDialog dialog)
	{
		GeneralManager.openDialogs.Add(dialog);
	}

	// Token: 0x06000408 RID: 1032 RVA: 0x000134FE File Offset: 0x000116FE
	public static void DialogDestroyed(IDialog dialog)
	{
		GeneralManager.openDialogs.Remove(dialog);
	}

	// Token: 0x06000409 RID: 1033 RVA: 0x0001350C File Offset: 0x0001170C
	public List<IDialog> FindOpenDialogs()
	{
		List<IDialog> list = new List<IDialog>();
		GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
		for (int i = 0; i < rootGameObjects.Length; i++)
		{
			IDialog component = rootGameObjects[i].GetComponent<IDialog>();
			if (component != null)
			{
				list.Add(component);
			}
		}
		return list;
	}

	// Token: 0x0600040A RID: 1034 RVA: 0x00013554 File Offset: 0x00011754
	public static bool CurrentyWritingInTextField()
	{
		if (EventSystem.current != null && EventSystem.current.isFocused && EventSystem.current.currentSelectedGameObject != null)
		{
			InputField component = EventSystem.current.currentSelectedGameObject.GetComponent<InputField>();
			if (component != null && component.isFocused)
			{
				return true;
			}
			TMP_InputField component2 = EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>();
			if (component2 != null && component2.isFocused)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600040B RID: 1035 RVA: 0x000135D3 File Offset: 0x000117D3
	public static bool ConfirmDialogOpen()
	{
		return GeneralManager.openConfirmDialog != null;
	}

	// Token: 0x0600040C RID: 1036 RVA: 0x000135E8 File Offset: 0x000117E8
	public static BasicConfirmDialog CreateConfirmDialog(string text, string title = null, bool onlyOneButton = false)
	{
		if (GeneralManager.singleton != null && UnityEngine.Object.FindObjectsOfType<BasicConfirmDialog>().Length == 0)
		{
			BasicConfirmDialog component = UnityEngine.Object.Instantiate<GameObject>(GeneralManager.singleton.confirmDialogPrefab).GetComponent<BasicConfirmDialog>();
			component.SetText(text, title, onlyOneButton);
			component.okButton.Select();
			GeneralManager.openConfirmDialog = component;
			GeneralManager.singleton.UpdateCursorState();
			GeneralManager.singleton.UpdateInputSystemState();
			return component;
		}
		return null;
	}

	// Token: 0x0600040D RID: 1037 RVA: 0x00013650 File Offset: 0x00011850
	public static BasicTextConfirmDialog CreateLargeConfirmDialog(string text, string title = null, bool onlyOneButton = false)
	{
		if (GeneralManager.singleton != null && UnityEngine.Object.FindObjectsOfType<BasicTextConfirmDialog>().Length == 0)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(GeneralManager.singleton.confirmLargeDialogPrefab);
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			BasicTextConfirmDialog component = gameObject.GetComponent<BasicTextConfirmDialog>();
			component.SetText("", title, onlyOneButton);
			component.SetValue(text);
			component.SetReadOnly();
			component.okButton.Select();
			GeneralManager.openConfirmDialog = component;
			GeneralManager.singleton.UpdateCursorState();
			GeneralManager.singleton.UpdateInputSystemState();
			return component;
		}
		return null;
	}

	// Token: 0x0600040E RID: 1038 RVA: 0x000136CD File Offset: 0x000118CD
	public static void DisplayInfoMessage(string text, float displayTime = 1f)
	{
		if (GeneralManager.singleton != null)
		{
			UnityEngine.Object.Instantiate<GameObject>(GeneralManager.singleton.basicInfoDialog).GetComponent<BasicInfoDialog>().SetText(text, displayTime, false);
		}
	}

	// Token: 0x0600040F RID: 1039 RVA: 0x000136F8 File Offset: 0x000118F8
	public static BasicInfoDialog CreateAlertDialog(string text, float newDisplayTime = 1f, bool dontDestroyOnLoad = false)
	{
		if (GeneralManager.singleton != null)
		{
			BasicInfoDialog component = UnityEngine.Object.Instantiate<GameObject>(GeneralManager.singleton.basicInfoDialog).GetComponent<BasicInfoDialog>();
			component.SetText(text, newDisplayTime, dontDestroyOnLoad);
			return component;
		}
		return null;
	}

	// Token: 0x06000410 RID: 1040 RVA: 0x00013728 File Offset: 0x00011928
	public void CheckMultiplayerSessionEnd()
	{
		if (this.leftMultiplayerSession && !this.leftMultiplayerSessionVoluntarily)
		{
			if (this.connectionEndedType == ConnectionEndedType.None)
			{
				this.connectionEndedType = ConnectionEndedType.ConnectionLost;
			}
			GeneralManager.CreateConfirmDialog(this.connectionEndedType.GetDescription(), null, true);
		}
		this.leftMultiplayerSession = false;
		this.leftMultiplayerSessionVoluntarily = false;
		this.connectionEndedType = ConnectionEndedType.None;
	}

	// Token: 0x06000411 RID: 1041 RVA: 0x00013781 File Offset: 0x00011981
	public static void DisplayJoinErrorMessage(ConnectionEndedType endType)
	{
		GeneralManager.CreateAlertDialog(endType.GetDescription(), 1f, false);
	}

	// Token: 0x06000412 RID: 1042 RVA: 0x0001379C File Offset: 0x0001199C
	private void InitializeInputActions()
	{
		this.inputActions = new MenuInputActions();
		this.inputActions.UI.Enable();
		this.inputActions.UI.Navigate.performed += this.NavigationPerformed;
	}

	// Token: 0x06000413 RID: 1043 RVA: 0x000137EC File Offset: 0x000119EC
	private void NavigationPerformed(InputAction.CallbackContext obj)
	{
		if (Cursor.lockState != CursorLockMode.Locked && EventSystem.current != null && !(EventSystem.current.GetType() == typeof(MultiplayerEventSystem)) && (EventSystem.current.currentSelectedGameObject == null || !EventSystem.current.currentSelectedGameObject.activeInHierarchy))
		{
			(from x in UnityEngine.Object.FindObjectsOfType<BasicButton>()
			orderby x.selectFirst descending
			select x).ToList<BasicButton>();
			BasicButton basicButton = (from x in UnityEngine.Object.FindObjectsOfType<BasicButton>()
			orderby x.selectFirst descending
			select x).FirstOrDefault<BasicButton>();
			if (basicButton != null)
			{
				EventSystem.current.SetSelectedGameObject(basicButton.gameObject);
			}
		}
	}

	// Token: 0x06000414 RID: 1044 RVA: 0x000138CC File Offset: 0x00011ACC
	private void Update()
	{
		if (this.lastFpsUpdate + 0.25f < Time.unscaledTime)
		{
			this.lastFpsUpdate = Time.unscaledTime;
			this.UpdateFPSText();
		}
		if (this.lastPingUpdate + 0.25f < Time.unscaledTime)
		{
			this.lastPingUpdate = Time.unscaledTime;
			this.UpdatePingText();
		}
		if (this.userControls.General.Chat.WasPressedThisFrame() && MultiplayerChat.singleton != null && !GeneralManager.CurrentyWritingInTextField())
		{
			MultiplayerChat.singleton.ActivateInputField();
		}
		if (this.userControls.General.Restart.WasPressedThisFrame() && GameMenu.singleton != null)
		{
			GameMenu.singleton.RestartGame();
		}
		if (this.userControls.General.SaveReplay.WasPressedThisFrame() && ReplayManager.singleton != null)
		{
			ReplayManager.singleton.SaveRecording();
		}
		if (this.userControls.General.PushToTalk.WasPressedThisFrame() && VoiceChatManager.singleton != null && !GeneralManager.CurrentyWritingInTextField())
		{
			VoiceChatManager.singleton.ActivateVoiceChat();
		}
		if (this.userControls.General.PushToTalk.WasReleasedThisFrame() && VoiceChatManager.singleton != null)
		{
			VoiceChatManager.singleton.DeactivateVoiceChat();
		}
		if (this.userControls.Generic.ControllerAlternativeClick.WasPressedThisFrame())
		{
			this.HandleControllerAlternativeClick();
		}
	}

	// Token: 0x06000415 RID: 1045 RVA: 0x00013A44 File Offset: 0x00011C44
	private void HandleControllerAlternativeClick()
	{
		if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
		{
			EventTrigger component = EventSystem.current.currentSelectedGameObject.GetComponent<EventTrigger>();
			if (component != null)
			{
				PointerEventData arg = new PointerEventData(EventSystem.current)
				{
					button = PointerEventData.InputButton.Right
				};
				foreach (EventTrigger.Entry entry in component.triggers)
				{
					if (entry.eventID == EventTriggerType.PointerClick)
					{
						entry.callback.Invoke(arg);
					}
				}
			}
		}
	}

	// Token: 0x06000416 RID: 1046 RVA: 0x00013AF4 File Offset: 0x00011CF4
	private void UpdateFPSText()
	{
		if (this.showFPS)
		{
			this.fps = (int)(1f / Time.unscaledDeltaTime);
			if (!this.fpsTextMesh.gameObject.activeInHierarchy)
			{
				this.fpsTextMesh.gameObject.SetActive(true);
			}
			this.fpsTextMesh.SetText("{0}", (float)this.fps);
			return;
		}
		if (this.fpsTextMesh.gameObject.activeInHierarchy)
		{
			this.fpsTextMesh.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000417 RID: 1047 RVA: 0x00013B7C File Offset: 0x00011D7C
	private void UpdatePingText()
	{
		if (NetworkClient.active && (NetworkClient.connectionQuality != ConnectionQuality.EXCELLENT || GeneralManager.badConnection))
		{
			if (!this.pingTextMesh.gameObject.activeInHierarchy)
			{
				this.pingTextMesh.gameObject.SetActive(true);
			}
			this.pingTextMesh.SetText("{0}", (float)((int)(NetworkTime.rtt * 1000.0)));
			return;
		}
		if (this.pingTextMesh.gameObject.activeInHierarchy)
		{
			this.pingTextMesh.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000418 RID: 1048 RVA: 0x00013C08 File Offset: 0x00011E08
	private void SetupRoomManager()
	{
		if (GeneralManager.multiplayerRoomManager == null)
		{
			GeneralManager.multiplayerRoomManager = UnityEngine.Object.FindObjectOfType<MultiplayerRoomManager>();
			if (GeneralManager.multiplayerRoomManager == null)
			{
				GeneralManager.multiplayerRoomManager = UnityEngine.Object.Instantiate<GameObject>(this.multiplayerRoomManagerPrefab).GetComponent<MultiplayerRoomManager>();
			}
			GeneralManager.kcpTransport = UnityEngine.Object.FindObjectOfType<KcpTransport>();
			GeneralManager.steamTransport = UnityEngine.Object.FindObjectOfType<FizzyFacepunch>();
			GeneralManager.steamManager = UnityEngine.Object.FindObjectOfType<SteamManager>();
		}
	}

	// Token: 0x06000419 RID: 1049 RVA: 0x00013C6C File Offset: 0x00011E6C
	private void CleanUpPrivate()
	{
		GeneralManager.badConnection = false;
		this.RemoveLoadingScreen();
		this.SetupRoomManager();
		Debug.Log("CleanUp");
		for (int i = InputUser.all.Count - 1; i >= 0; i--)
		{
			InputUser.all[i].UnpairDevicesAndRemoveUser();
		}
		if (NetworkClient.active)
		{
			NetworkClient.Disconnect();
		}
		if (MultiplayerMenuManager.singleton != null)
		{
			MultiplayerMenuManager.singleton.UpdateRoomManagerTransport();
		}
		else
		{
			GeneralManager.multiplayerRoomManager.SetTransport(GeneralManager.steamTransport);
		}
		GeneralManager.steamManager.LeaveLobby();
		if (GeneralManager.singleton != null)
		{
			GeneralManager.singleton.CheckMultiplayerSessionEnd();
		}
	}

	// Token: 0x0600041A RID: 1050 RVA: 0x00013D1A File Offset: 0x00011F1A
	public static void CleanUp()
	{
		if (GeneralManager.singleton != null)
		{
			GeneralManager.singleton.CleanUpPrivate();
		}
	}

	// Token: 0x0600041B RID: 1051 RVA: 0x00013D34 File Offset: 0x00011F34
	public void UpdatePlayerOptionsVisible()
	{
		bool alwaysHidden = PlayerOptionsManager.singleton != null;
		this.playerOptionsVisible = alwaysHidden;
		this.UpdateInputSystemState();
		if (MultiplayerChat.singleton != null)
		{
			MultiplayerChat.singleton.SetAlwaysHidden(alwaysHidden);
		}
	}

	// Token: 0x0600041C RID: 1052 RVA: 0x00013D72 File Offset: 0x00011F72
	public void EnableUserControls(bool enabled)
	{
		this.userControlsEnabled = enabled;
		if (this.userControls != null)
		{
			if (enabled)
			{
				this.userControls.Enable();
				return;
			}
			this.userControls.Disable();
		}
	}

	// Token: 0x0600041D RID: 1053 RVA: 0x00013DA0 File Offset: 0x00011FA0
	public bool AnyPlayerActionMapInputActive()
	{
		if (this.userControls != null)
		{
			foreach (PropertyInfo propertyInfo in this.userControls.PlayerActionMap.GetType().GetProperties())
			{
				if (propertyInfo.PropertyType == typeof(InputAction) && ((InputAction)propertyInfo.GetValue(this.userControls.PlayerActionMap)).IsInProgress())
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600041E RID: 1054 RVA: 0x00013E20 File Offset: 0x00012020
	public void SetTimeScale(float newTimeScale)
	{
		if ((double)newTimeScale < -0.5)
		{
			newTimeScale = 1f;
		}
		Time.timeScale = newTimeScale;
		float fixedDeltaTime = 0.005f;
		if (Generic.FloatEquals(newTimeScale, 0.25f))
		{
			fixedDeltaTime = 0.0025f;
		}
		Time.fixedDeltaTime = fixedDeltaTime;
	}

	// Token: 0x0600041F RID: 1055 RVA: 0x00013E66 File Offset: 0x00012066
	public void RegisterPlayerHealth(PlayerHealth player)
	{
		this.registeredPlayerHealths.Add(player);
	}

	// Token: 0x06000420 RID: 1056 RVA: 0x00013E74 File Offset: 0x00012074
	public void UnregisterPlayerHealth(PlayerHealth player)
	{
		this.registeredPlayerHealths.Remove(player);
	}

	// Token: 0x06000421 RID: 1057 RVA: 0x00013E84 File Offset: 0x00012084
	public static void SetVignetteValue(float value, bool force = false)
	{
		if (GeneralManager.singleton != null)
		{
			if (!GameMenu.GameMenuCurrentlyHidden)
			{
				value = 0f;
			}
			if (value > 1f)
			{
				value = 1f;
			}
			if (!force && Generic.FloatEquals(value, GeneralManager.singleton.vignetteValue))
			{
				return;
			}
			GeneralManager.singleton.vignetteValue = value;
			if (!Generic.FloatEquals(GeneralManager.singleton.vignetteValue, 0f))
			{
				GeneralManager.singleton.vignetteMaterial.SetInteger("_Active", 1);
				GeneralManager.singleton.vignetteMaterial.SetFloat("_Intensity", GeneralManager.singleton.vignetteValue);
				return;
			}
			GeneralManager.singleton.vignetteMaterial.SetInteger("_Active", 0);
			GeneralManager.singleton.vignetteMaterial.SetFloat("_Intensity", GeneralManager.singleton.vignetteValue);
		}
	}

	// Token: 0x06000422 RID: 1058 RVA: 0x00013F5C File Offset: 0x0001215C
	private static float InverseSmoothstep(float x)
	{
		return 0.5f - Mathf.Sin(Mathf.Asin(1f - 2f * x) / 3f);
	}

	// Token: 0x06000423 RID: 1059 RVA: 0x00013F84 File Offset: 0x00012184
	private void InitializeBannedWords()
	{
		this.badWordList = this.bannedWordsAsset.text.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
		this.clearWordList = this.clearedWordsAsset.text.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
		this.generatedRegexText = string.Concat(new string[]
		{
			"(?i)\\b(?!(?:",
			string.Join("|", this.clearWordList),
			")\\b)(?:",
			string.Join("|", this.badWordList),
			")\\b"
		});
	}

	// Token: 0x06000424 RID: 1060 RVA: 0x00014018 File Offset: 0x00012218
	public bool ContainsBadWords(string textToCheck)
	{
		return Regex.IsMatch(textToCheck, this.generatedRegexText);
	}

	// Token: 0x06000425 RID: 1061 RVA: 0x0001402B File Offset: 0x0001222B
	public string FilterBadWords(string textToFilter, bool fileNameSafe = false)
	{
		textToFilter = Regex.Replace(textToFilter, this.generatedRegexText, fileNameSafe ? "----" : "****");
		return textToFilter;
	}

	// Token: 0x06000426 RID: 1062 RVA: 0x0001404C File Offset: 0x0001224C
	public void UpdateBloodColor()
	{
		BloodColourType bloodColourType = SettingsHelper.GetBloodColourType();
		if (bloodColourType != this.currentBloodColour || !this.bloodColourInitialized)
		{
			this.bloodParticleMaterial.color = Generic.BloodParticleColour(bloodColourType);
			this.bloodDecalMaterial.SetColor("_BaseColor", Generic.BloodColour(bloodColourType));
			this.playerMaterial.SetColor("_BloodColour", Generic.BloodColour(bloodColourType));
			this.bloodColourInitialized = true;
			this.currentBloodColour = bloodColourType;
			foreach (PlayerHealth playerHealth in UnityEngine.Object.FindObjectsOfType<PlayerHealth>().ToList<PlayerHealth>())
			{
				playerHealth.SetBloodColour(Generic.BloodColour(bloodColourType));
			}
		}
	}

	// Token: 0x04000278 RID: 632
	public static GeneralManager singleton;

	// Token: 0x04000279 RID: 633
	public bool showFPS;

	// Token: 0x0400027A RID: 634
	public GameObject loadingScreenPrefab;

	// Token: 0x0400027B RID: 635
	public GameObject currentLoadingScreen;

	// Token: 0x0400027C RID: 636
	public Slider loadingBarSlider;

	// Token: 0x0400027D RID: 637
	public Text loadingBarText;

	// Token: 0x0400027E RID: 638
	public GameObject confirmDialogPrefab;

	// Token: 0x0400027F RID: 639
	public GameObject basicInfoDialog;

	// Token: 0x04000280 RID: 640
	public GameObject confirmLargeDialogPrefab;

	// Token: 0x04000281 RID: 641
	public Canvas fpsCanvas;

	// Token: 0x04000282 RID: 642
	public TextMeshProUGUI fpsTextMesh;

	// Token: 0x04000283 RID: 643
	public TextMeshProUGUI pingTextMesh;

	// Token: 0x04000284 RID: 644
	public UserControls userControls;

	// Token: 0x04000285 RID: 645
	public Material vignetteMaterial;

	// Token: 0x04000286 RID: 646
	private Coroutine loadingCoroutine;

	// Token: 0x04000287 RID: 647
	private float startedTimeout;

	// Token: 0x04000288 RID: 648
	private float timeoutDuration = 35f;

	// Token: 0x04000289 RID: 649
	private static List<IDialog> openDialogs = new List<IDialog>();

	// Token: 0x0400028A RID: 650
	public static BasicConfirmDialog openConfirmDialog = null;

	// Token: 0x0400028B RID: 651
	public bool leftMultiplayerSession;

	// Token: 0x0400028C RID: 652
	public bool leftMultiplayerSessionVoluntarily;

	// Token: 0x0400028D RID: 653
	public ConnectionEndedType connectionEndedType;

	// Token: 0x0400028E RID: 654
	public MenuInputActions inputActions;

	// Token: 0x0400028F RID: 655
	public EventSystem eventSystem;

	// Token: 0x04000290 RID: 656
	public static bool badConnection = false;

	// Token: 0x04000291 RID: 657
	private float lastFpsUpdate;

	// Token: 0x04000292 RID: 658
	private float lastPingUpdate = 0.1f;

	// Token: 0x04000293 RID: 659
	private int fps = 30;

	// Token: 0x04000294 RID: 660
	public static FizzyFacepunch steamTransport;

	// Token: 0x04000295 RID: 661
	public static KcpTransport kcpTransport;

	// Token: 0x04000296 RID: 662
	public static MultiplayerRoomManager multiplayerRoomManager;

	// Token: 0x04000297 RID: 663
	public static SteamManager steamManager;

	// Token: 0x04000298 RID: 664
	public GameObject multiplayerRoomManagerPrefab;

	// Token: 0x04000299 RID: 665
	public bool playerOptionsVisible;

	// Token: 0x0400029A RID: 666
	private bool userControlsEnabled = true;

	// Token: 0x0400029B RID: 667
	public List<PlayerHealth> registeredPlayerHealths = new List<PlayerHealth>(16);

	// Token: 0x0400029C RID: 668
	private float vignetteValue;

	// Token: 0x0400029D RID: 669
	public TextAsset bannedWordsAsset;

	// Token: 0x0400029E RID: 670
	public TextAsset clearedWordsAsset;

	// Token: 0x0400029F RID: 671
	private string[] badWordList;

	// Token: 0x040002A0 RID: 672
	private string[] clearWordList;

	// Token: 0x040002A1 RID: 673
	public string generatedRegexText = "";

	// Token: 0x040002A2 RID: 674
	public bool bloodColourInitialized;

	// Token: 0x040002A3 RID: 675
	private BloodColourType currentBloodColour;

	// Token: 0x040002A4 RID: 676
	public Material bloodParticleMaterial;

	// Token: 0x040002A5 RID: 677
	public Material bloodDecalMaterial;

	// Token: 0x040002A6 RID: 678
	public Material playerMaterial;
}
