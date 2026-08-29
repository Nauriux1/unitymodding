using System;
using System.Collections.Generic;
using System.Linq;
using MoveClasses;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;

// Token: 0x02000083 RID: 131
public class LobbyLocalManager : MonoBehaviour
{
	// Token: 0x06000435 RID: 1077 RVA: 0x0001444C File Offset: 0x0001264C
	private void Start()
	{
		if (IGameSettingsManager.singleton != null)
		{
			this.aiAmount = IGameSettingsManager.singleton.AiAmount;
		}
		this.currentPlayerCount = 0;
		this.lobbyLocalPlayers = new List<LobbyLocalPlayer>();
		for (int i = 1; i <= 4; i++)
		{
			LobbyLocalPlayer lobbyLocalPlayer = new LobbyLocalPlayer();
			GameObject gameObject = GameObject.Find("Camera" + i.ToString());
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.canvasPrefab);
			GameObject gameObject3 = gameObject2;
			gameObject3.name += i.ToString();
			gameObject2.transform.parent = this.canvasHolder.transform;
			lobbyLocalPlayer.cameraGameObject = gameObject;
			lobbyLocalPlayer.camera = gameObject.GetComponent<Camera>();
			lobbyLocalPlayer.canvasGameObject = gameObject2;
			lobbyLocalPlayer.canvas = gameObject2.GetComponent<Canvas>();
			lobbyLocalPlayer.playerCanvasController = gameObject2.GetComponent<PlayerCanvasController>();
			if (this.aiAmount > 0 && i == 1)
			{
				this.player1CanvasGameObject = gameObject2;
				lobbyLocalPlayer.canvas.renderMode = RenderMode.ScreenSpaceOverlay;
				RectTransform component = gameObject2.transform.Find("InputsPanel").GetComponent<RectTransform>();
				component.anchorMin = new Vector2(0f, 0.5f);
				component.anchorMax = new Vector2(0.5f, 1f);
				RectTransform component2 = lobbyLocalPlayer.playerCanvasController.pressAnyButtonTextPanel.transform.GetComponent<RectTransform>();
				component2.anchorMin = new Vector2(0f, 0.5f);
				component2.anchorMax = new Vector2(0.5f, 1f);
			}
			else if (this.aiAmount > 0 && i <= this.aiAmount + 1)
			{
				lobbyLocalPlayer.ai = true;
				Transform transform = gameObject2.transform.Find("InputsPanel");
				transform.Find("PlayerTitleText").gameObject.SetActive(true);
				transform.parent = this.player1CanvasGameObject.transform;
				RectTransform component3 = transform.GetComponent<RectTransform>();
				Vector2 uiminAnchorsForPlayerNumber = this.GetUIMinAnchorsForPlayerNumber(i);
				Vector2 uimaxAnchorsForPlayerNumber = this.GetUIMaxAnchorsForPlayerNumber(i);
				component3.anchorMin = uiminAnchorsForPlayerNumber;
				component3.anchorMax = uimaxAnchorsForPlayerNumber;
				gameObject2.SetActive(false);
			}
			else
			{
				lobbyLocalPlayer.canvas.renderMode = RenderMode.ScreenSpaceOverlay;
				RectTransform component4 = gameObject2.transform.Find("InputsPanel").GetComponent<RectTransform>();
				Vector2 uiminAnchorsForPlayerNumber2 = this.GetUIMinAnchorsForPlayerNumber(i);
				Vector2 uimaxAnchorsForPlayerNumber2 = this.GetUIMaxAnchorsForPlayerNumber(i);
				component4.anchorMin = uiminAnchorsForPlayerNumber2;
				component4.anchorMax = uimaxAnchorsForPlayerNumber2;
				RectTransform component5 = lobbyLocalPlayer.playerCanvasController.pressAnyButtonTextPanel.transform.GetComponent<RectTransform>();
				component5.anchorMin = uiminAnchorsForPlayerNumber2;
				component5.anchorMax = uimaxAnchorsForPlayerNumber2;
			}
			lobbyLocalPlayer.canvas.planeDistance = 0.5f;
			lobbyLocalPlayer.camera.transform.position = new Vector3(lobbyLocalPlayer.camera.transform.position.x + (float)(100 * i), lobbyLocalPlayer.camera.transform.position.y, lobbyLocalPlayer.camera.transform.position.z);
			string playerTitle = string.Format("P{0}", (from x in this.lobbyLocalPlayers
			where !x.ai
			select x).Count<LobbyLocalPlayer>() + 1);
			lobbyLocalPlayer.playerNumber = (from x in this.lobbyLocalPlayers
			where !x.ai
			select x).Count<LobbyLocalPlayer>() + 1;
			lobbyLocalPlayer.playerCanvasController.ShowPressAnyButtonText(playerTitle);
			lobbyLocalPlayer.playerCanvasController.RegisterLobbyItems(this, lobbyLocalPlayer);
			lobbyLocalPlayer.playerCanvasController.DoInitializations();
			this.lobbyLocalPlayers.Add(lobbyLocalPlayer);
		}
		this.SetupUiNavigationForAi();
		InputUser.listenForUnpairedDeviceActivity++;
		InputUser.onUnpairedDeviceUsed += this.HandleDetectedDevice;
	}

	// Token: 0x06000436 RID: 1078 RVA: 0x000147E4 File Offset: 0x000129E4
	private void SetupUiNavigationForAi()
	{
		if (this.aiAmount > 0)
		{
			PlayerCanvasController playerCanvasController = null;
			PlayerCanvasController playerCanvasController2 = null;
			PlayerCanvasController playerCanvasController3 = null;
			PlayerCanvasController playerCanvasController4 = null;
			for (int i = 0; i < this.lobbyLocalPlayers.Count; i++)
			{
				if (i == 0)
				{
					playerCanvasController = this.lobbyLocalPlayers[i].playerCanvasContoller;
				}
				else if (this.lobbyLocalPlayers[i].ai)
				{
					if (i == 1)
					{
						playerCanvasController2 = this.lobbyLocalPlayers[i].playerCanvasContoller;
					}
					else if (i == 2)
					{
						playerCanvasController3 = this.lobbyLocalPlayers[i].playerCanvasContoller;
					}
					else if (i == 3)
					{
						playerCanvasController4 = this.lobbyLocalPlayers[i].playerCanvasContoller;
					}
				}
			}
			if (playerCanvasController != null)
			{
				playerCanvasController.rightPlayerCanvas = playerCanvasController2;
				playerCanvasController.downPlayerCanvas = playerCanvasController3;
			}
			if (playerCanvasController2 != null)
			{
				playerCanvasController2.leftPlayerCanvas = playerCanvasController;
				playerCanvasController2.downPlayerCanvas = playerCanvasController4;
			}
			if (playerCanvasController3 != null)
			{
				playerCanvasController3.upPlayerCanvas = playerCanvasController;
				playerCanvasController3.rightPlayerCanvas = playerCanvasController4;
			}
			if (playerCanvasController4 != null)
			{
				playerCanvasController4.upPlayerCanvas = playerCanvasController2;
				playerCanvasController4.leftPlayerCanvas = playerCanvasController3;
			}
			if (playerCanvasController != null)
			{
				playerCanvasController.UpdateButtonPaths();
			}
			if (playerCanvasController2 != null)
			{
				playerCanvasController2.UpdateButtonPaths();
			}
			if (playerCanvasController3 != null)
			{
				playerCanvasController3.UpdateButtonPaths();
			}
			if (playerCanvasController4 != null)
			{
				playerCanvasController4.UpdateButtonPaths();
			}
		}
	}

	// Token: 0x06000437 RID: 1079 RVA: 0x00014934 File Offset: 0x00012B34
	private Vector2 GetUIMinAnchorsForPlayerNumber(int i)
	{
		Vector2 result = new Vector2(0f, 0.5f);
		if (i == 4)
		{
			result = new Vector2(0.5f, 0f);
		}
		else if (i == 3)
		{
			result = new Vector2(0f, 0f);
		}
		else if (i == 2)
		{
			result = new Vector2(0.5f, 0.5f);
		}
		return result;
	}

	// Token: 0x06000438 RID: 1080 RVA: 0x00014998 File Offset: 0x00012B98
	private Vector2 GetUIMaxAnchorsForPlayerNumber(int i)
	{
		Vector2 result = new Vector2(0.5f, 1f);
		if (i == 4)
		{
			result = new Vector2(1f, 0.5f);
		}
		else if (i == 3)
		{
			result = new Vector2(0.5f, 0.5f);
		}
		else if (i == 2)
		{
			result = new Vector2(1f, 1f);
		}
		return result;
	}

	// Token: 0x06000439 RID: 1081 RVA: 0x0000777A File Offset: 0x0000597A
	private void Update()
	{
	}

	// Token: 0x0600043A RID: 1082 RVA: 0x000149FC File Offset: 0x00012BFC
	private void HandleDetectedDevice(InputControl control, InputEventPtr eventPtr)
	{
		if (!(control is ButtonControl))
		{
			return;
		}
		InputDevice inputDevice = control.device;
		if (control.device.name == "Mouse")
		{
			inputDevice = Keyboard.current;
		}
		using (List<LobbyLocalPlayer>.Enumerator enumerator = this.lobbyLocalPlayers.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.device == inputDevice)
				{
					return;
				}
			}
		}
		LobbyLocalPlayer lobbyLocalPlayer = null;
		foreach (LobbyLocalPlayer lobbyLocalPlayer2 in this.lobbyLocalPlayers)
		{
			if (!lobbyLocalPlayer2.ai && !lobbyLocalPlayer2.playerExists)
			{
				lobbyLocalPlayer = lobbyLocalPlayer2;
				break;
			}
		}
		if (lobbyLocalPlayer == null)
		{
			return;
		}
		this.PairUserToInputDevice(lobbyLocalPlayer, inputDevice);
		this.currentPlayerCount++;
	}

	// Token: 0x0600043B RID: 1083 RVA: 0x00014AEC File Offset: 0x00012CEC
	public void CheckReady()
	{
		bool flag = true;
		foreach (LobbyLocalPlayer lobbyLocalPlayer in this.lobbyLocalPlayers)
		{
			if (lobbyLocalPlayer.playerExists && (!lobbyLocalPlayer.readyToBegin || lobbyLocalPlayer.selectedMoveSet == null))
			{
				flag = false;
			}
		}
		if (flag)
		{
			this.StartGame();
		}
	}

	// Token: 0x0600043C RID: 1084 RVA: 0x00014B60 File Offset: 0x00012D60
	public void StartGame()
	{
		foreach (LobbyLocalPlayer lobbyLocalPlayer in this.lobbyLocalPlayers)
		{
			if (lobbyLocalPlayer.playerInput != null)
			{
				InputUser user = lobbyLocalPlayer.playerInput.user;
				lobbyLocalPlayer.playerInput.user.UnpairDevicesAndRemoveUser();
				lobbyLocalPlayer.playerInput.DeactivateInput();
			}
		}
		Debug.Log("START GAME");
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("lobbyPlayers", this.lobbyLocalPlayers.Cast<LobbyPlayer>().ToList<LobbyPlayer>());
		dictionary.Add("DoLocalMapInit", true);
		string sceneName = "map_ArenaOfBlades";
		if (IGameSettingsManager.singleton != null)
		{
			sceneName = IGameSettingsManager.singleton.SelectedMap;
		}
		SceneManagerWithParameters.LoadScene(sceneName, dictionary, false, false);
	}

	// Token: 0x0600043D RID: 1085 RVA: 0x00014C44 File Offset: 0x00012E44
	private void OnDestroy()
	{
		if (InputUser.listenForUnpairedDeviceActivity > 0)
		{
			InputUser.listenForUnpairedDeviceActivity--;
			InputUser.onUnpairedDeviceUsed -= this.HandleDetectedDevice;
		}
	}

	// Token: 0x0600043E RID: 1086 RVA: 0x00014C6C File Offset: 0x00012E6C
	public void UpdateCurrentPlayerCount()
	{
		this.currentPlayerCount = (from x in this.lobbyLocalPlayers
		where !x.ai && x.playerExists
		select x).Count<LobbyLocalPlayer>();
		for (int i = 0; i < this.lobbyLocalPlayers.Count; i++)
		{
			LobbyLocalPlayer lobbyLocalPlayer = this.lobbyLocalPlayers[i];
			lobbyLocalPlayer.readyToBegin = false;
			lobbyLocalPlayer.playerCanvasContoller.UpdateReadyButtonColor();
			if (!lobbyLocalPlayer.ai && !lobbyLocalPlayer.playerExists)
			{
				LobbyLocalPlayer lobbyLocalPlayer2 = this.FindNextLobbyLocalPlayer(i);
				if (lobbyLocalPlayer2 != null)
				{
					this.SwapLobbyLocalPlayers(lobbyLocalPlayer, lobbyLocalPlayer2);
				}
			}
		}
	}

	// Token: 0x0600043F RID: 1087 RVA: 0x00014D08 File Offset: 0x00012F08
	public LobbyLocalPlayer FindNextLobbyLocalPlayer(int currentIndex)
	{
		for (int i = 0; i < this.lobbyLocalPlayers.Count; i++)
		{
			LobbyLocalPlayer lobbyLocalPlayer = this.lobbyLocalPlayers[i];
			if (!lobbyLocalPlayer.ai && lobbyLocalPlayer.playerExists && i > currentIndex)
			{
				return lobbyLocalPlayer;
			}
		}
		return null;
	}

	// Token: 0x06000440 RID: 1088 RVA: 0x00014D50 File Offset: 0x00012F50
	public void SwapLobbyLocalPlayers(LobbyLocalPlayer player1, LobbyLocalPlayer player2)
	{
		InputDevice device = player2.device;
		player2.UnregisterPlayer(false);
		this.PairUserToInputDevice(player1, device);
		if (player2.GetMoveSet() != null)
		{
			player1.SetMoveSet(player2.GetMoveSet());
			player1.SetEquipment(MoveClassHelpers.CloneEquipmentList(player2.GetSelectedEquipment()));
			player1.playerCanvasContoller.equipmentPanel.UpdateEquipmentInfo(false, false);
		}
	}

	// Token: 0x06000441 RID: 1089 RVA: 0x00014DAC File Offset: 0x00012FAC
	public void PairUserToInputDevice(LobbyLocalPlayer selectedLobbyLocalPlayer, InputDevice deviceToUse)
	{
		selectedLobbyLocalPlayer.playerExists = true;
		selectedLobbyLocalPlayer.playerCanvasController.HidePressAnyButtonText();
		selectedLobbyLocalPlayer.device = deviceToUse;
		PlayerInput playerInput = PlayerInput.Instantiate(this.inputManager, -1, null, -1, deviceToUse);
		GameObject gameObject = selectedLobbyLocalPlayer.canvasGameObject.transform.Find("InputsPanel").Find("PlayerTitleText").gameObject;
		gameObject.SetActive(true);
		gameObject.GetComponent<Text>().text = string.Format("P{0}", selectedLobbyLocalPlayer.playerNumber);
		selectedLobbyLocalPlayer.playerInput = playerInput;
		selectedLobbyLocalPlayer.playerInput.neverAutoSwitchControlSchemes = true;
		if (selectedLobbyLocalPlayer.multiplayerEventSystem == null)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.eventSystemPrefab);
			GameObject gameObject3 = gameObject2;
			gameObject3.name += selectedLobbyLocalPlayer.playerNumber.ToString();
			selectedLobbyLocalPlayer.multiplayerEventSystem = gameObject2.GetComponent<MultiplayerEventSystem>();
			gameObject2.transform.parent = this.canvasHolder.transform;
			selectedLobbyLocalPlayer.multiplayerEventSystem.playerRoot = selectedLobbyLocalPlayer.canvasGameObject;
			selectedLobbyLocalPlayer.multiplayerEventSystem.firstSelectedGameObject = selectedLobbyLocalPlayer.playerCanvasContoller.readyButton.gameObject;
		}
		InputSystemUIInputModule component = selectedLobbyLocalPlayer.multiplayerEventSystem.gameObject.GetComponent<InputSystemUIInputModule>();
		selectedLobbyLocalPlayer.playerInput.uiInputModule = component;
		selectedLobbyLocalPlayer.playerInput.camera = selectedLobbyLocalPlayer.camera;
		selectedLobbyLocalPlayer.playerCanvasController.removePlayerButton.gameObject.SetActive(true);
		if (selectedLobbyLocalPlayer.playerCanvasController.buttonToActivate.activeInHierarchy)
		{
			selectedLobbyLocalPlayer.multiplayerEventSystem.SetSelectedGameObject(selectedLobbyLocalPlayer.playerCanvasController.buttonToActivate);
			return;
		}
		selectedLobbyLocalPlayer.multiplayerEventSystem.SetSelectedGameObject(selectedLobbyLocalPlayer.playerCanvasContoller.readyButton.gameObject);
	}

	// Token: 0x040002B3 RID: 691
	public int currentPlayerCount;

	// Token: 0x040002B4 RID: 692
	public List<LobbyLocalPlayer> lobbyLocalPlayers;

	// Token: 0x040002B5 RID: 693
	public GameObject canvasHolder;

	// Token: 0x040002B6 RID: 694
	public GameObject canvasPrefab;

	// Token: 0x040002B7 RID: 695
	public GameObject inputManager;

	// Token: 0x040002B8 RID: 696
	public GameObject eventSystemPrefab;

	// Token: 0x040002B9 RID: 697
	public int aiAmount;

	// Token: 0x040002BA RID: 698
	private GameObject player1CanvasGameObject;
}
