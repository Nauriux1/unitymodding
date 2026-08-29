using System;
using System.Collections.Generic;
using System.Linq;
using BasicUI;
using Mirror;
using MoveClasses;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

// Token: 0x020001DB RID: 475
public class PlayerCanvasController : MonoBehaviour
{
	// Token: 0x17000198 RID: 408
	// (get) Token: 0x06000E2A RID: 3626 RVA: 0x00047810 File Offset: 0x00045A10
	public bool singleplayerLobby
	{
		get
		{
			return SceneManager.GetActiveScene().name == "LobbySingleplayer";
		}
	}

	// Token: 0x06000E2B RID: 3627 RVA: 0x0004783C File Offset: 0x00045A3C
	private void Awake()
	{
		this.moveSetEditorSelect = false;
		if (SceneManager.GetActiveScene().name == "LobbyMoveEditor")
		{
			this.moveSetEditorSelect = true;
		}
		this.removePlayerButton.gameObject.SetActive(false);
		this.readyButton = this.readyButtonGameObject.GetComponent<Button>();
		this.spectatorToggle.gameObject.SetActive(false);
		this.userControls = SettingsHelper.GetUserControls();
		this.userControls.Generic.Enable();
		this.applyButton.gameObject.SetActive(false);
		if (this.backButton != null)
		{
			this.backButton.onClick.AddListener(delegate()
			{
				this.BackButtonClicked();
			});
			UIHelpers.SetButtonColor(this.backButton, ButtonState.Basic, null, null);
		}
		if (this.EquipmentEditorPrefab != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.EquipmentEditorPrefab, this.panelsHolder.transform);
			this.equipmentPanel = gameObject.GetComponent<EquipmentPanel>();
			this.equipmentPanel.gameObject.SetActive(false);
			this.equipmentPanel.playerCanvasContoller = this;
		}
		if (this.GameSettingsPrefab != null)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.GameSettingsPrefab, this.panelsHolder.transform);
			this.gameSettingsPanel = gameObject2.GetComponent<GameSettingsPanel>();
			this.gameSettingsPanel.gameObject.SetActive(false);
		}
		if (this.moveSetEditorSelect)
		{
			this.selectMoveSetButton.onClick.AddListener(delegate()
			{
				this.SelectMoveSetButtonClicked();
			});
			UIHelpers.SetButtonColor(this.selectMoveSetButton, ButtonState.Basic, null, null);
			this.newMoveSetButton.onClick.AddListener(delegate()
			{
				this.NewMoveSetButtonClicked();
			});
			UIHelpers.SetButtonColor(this.newMoveSetButton, ButtonState.Basic, null, null);
			this.deleteMoveSetButton.onClick.AddListener(delegate()
			{
				this.DeleteMoveSetButtonClicked();
			});
			UIHelpers.SetButtonColor(this.deleteMoveSetButton, ButtonState.NotReady, null, null);
			this.copyMoveSetButton.onClick.AddListener(delegate()
			{
				this.CopyMoveSetButtonClicked();
			});
			UIHelpers.SetButtonColor(this.copyMoveSetButton, ButtonState.Basic, null, null);
			this.copyToClipboardMoveSetButton.onClick.AddListener(delegate()
			{
				this.CopyToClipboardMoveSetButtonClicked();
			});
			UIHelpers.SetButtonColor(this.copyToClipboardMoveSetButton, ButtonState.Basic, null, null);
			this.pasteFromClipboardMoveSetButton.onClick.AddListener(delegate()
			{
				this.PasteFromClipboardMoveSetButtonClicked();
			});
			UIHelpers.SetButtonColor(this.pasteFromClipboardMoveSetButton, ButtonState.Basic, null, null);
			this.readyButton.gameObject.SetActive(false);
			this.equipmentButton.gameObject.SetActive(false);
			this.moveSetButton.gameObject.SetActive(false);
			this.gameSettingsButton.gameObject.SetActive(false);
			this.selectMoveSetButton.gameObject.SetActive(true);
			this.newMoveSetButton.gameObject.SetActive(true);
			this.deleteMoveSetButton.gameObject.SetActive(true);
			this.copyMoveSetButton.gameObject.SetActive(false);
			this.copyToClipboardMoveSetButton.gameObject.SetActive(true);
			this.pasteFromClipboardMoveSetButton.gameObject.SetActive(true);
			this.lobbyPlayer = new LobbyPlayer();
		}
		else
		{
			this.readyButton.onClick.AddListener(delegate()
			{
				this.ReadyClicked();
			});
			UIHelpers.SetButtonColor(this.readyButton, ButtonState.NotReady, null, null);
			this.equipmentButton.onClick.AddListener(delegate()
			{
				this.EquipmentButtonClicked();
			});
			UIHelpers.SetButtonColor(this.equipmentButton, ButtonState.Basic, null, null);
			this.moveSetButton.onClick.AddListener(delegate()
			{
				this.MoveSetButtonClicked();
			});
			UIHelpers.SetButtonColor(this.moveSetButton, ButtonState.Basic, null, null);
			if (NetworkManager.singleton != null && NetworkManager.singleton.isNetworkActive)
			{
				this.gameSettingsButton.onClick.AddListener(delegate()
				{
					this.GameSettingsButtonClicked();
				});
				UIHelpers.SetButtonColor(this.gameSettingsButton, ButtonState.Basic, null, null);
				this.spectatorToggle.gameObject.SetActive(true);
				this.spectatorToggle.onValueChanged.AddListener(delegate(bool <p0>)
				{
					this.SpectatorChanged();
				});
			}
			else
			{
				this.gameSettingsButton.gameObject.SetActive(false);
			}
			if (GameMenu.singleton != null)
			{
				this.applyButton.onClick.AddListener(delegate()
				{
					this.ApplyClicked();
				});
				GameMenu.singleton.HideMenuTemporarily(true);
				this.readyButton.gameObject.SetActive(false);
				this.gameSettingsButton.gameObject.SetActive(false);
				this.applyButton.gameObject.SetActive(true);
			}
			this.selectMoveSetButton.gameObject.SetActive(false);
			this.newMoveSetButton.gameObject.SetActive(false);
			this.deleteMoveSetButton.gameObject.SetActive(false);
			this.copyMoveSetButton.gameObject.SetActive(false);
			this.copyToClipboardMoveSetButton.gameObject.SetActive(false);
			this.pasteFromClipboardMoveSetButton.gameObject.SetActive(false);
		}
		this.UpdateEquipmentPoints();
	}

	// Token: 0x06000E2C RID: 3628 RVA: 0x00047D2F File Offset: 0x00045F2F
	private void Start()
	{
		if (this.moveSetButtonList == null || this.moveSetButtonList.Count == 0)
		{
			if (this.lobbyPlayer != null)
			{
				this.DoInitializations();
				return;
			}
			this.FetchMultiplayerRoomPlayer();
		}
	}

	// Token: 0x06000E2D RID: 3629 RVA: 0x00047D5C File Offset: 0x00045F5C
	private void Update()
	{
		if (!this.moveSetEditorSelect && NetworkManager.singleton != null && NetworkManager.singleton.isNetworkActive && GameMenu.singleton == null && this.userControls.Generic.Back.WasPerformedThisFrame())
		{
			this.BackButtonClicked();
		}
	}

	// Token: 0x06000E2E RID: 3630 RVA: 0x00047DB8 File Offset: 0x00045FB8
	public void RegisterLobbyItems(LobbyPlayer newLobbyPlayer)
	{
		this.lobbyPlayer = newLobbyPlayer;
		this.lobbyPlayer.playerCanvasContoller = this;
		if (this.equipmentPanel != null)
		{
			this.equipmentPanel.lobbyPlayer = this.lobbyPlayer;
			this.equipmentPanel.playerHealth = this.lobbyPlayer.playerHealth;
		}
		this.DoInitializations();
	}

	// Token: 0x06000E2F RID: 3631 RVA: 0x00047E14 File Offset: 0x00046014
	public void RegisterLobbyItems(LobbyLocalManager newManager, LobbyLocalPlayer newLobbyPlayer)
	{
		this.lobbyManager = newManager;
		this.lobbyPlayer = newLobbyPlayer;
		this.lobbyPlayer.playerCanvasContoller = this;
		if (!newLobbyPlayer.ai)
		{
			this.removePlayerButton.gameObject.SetActive(true);
			this.removePlayerButton.onClick.RemoveAllListeners();
			this.removePlayerButton.onClick.AddListener(delegate()
			{
				newLobbyPlayer.UnregisterPlayer(true);
			});
		}
		if (this.equipmentPanel != null)
		{
			this.equipmentPanel.lobbyPlayer = this.lobbyPlayer;
			this.equipmentPanel.playerHealth = this.lobbyPlayer.playerHealth;
		}
		this.backButton.gameObject.SetActive(false);
		if (newLobbyPlayer.ai)
		{
			this.readyButtonGameObject.SetActive(false);
		}
	}

	// Token: 0x06000E30 RID: 3632 RVA: 0x00047EF8 File Offset: 0x000460F8
	public void RegisterLobbyItems(MultiplayerRoomPlayer newLobbyPlayer)
	{
		newLobbyPlayer.UpdateTempMultiplayerPlayerValues();
		this.lobbyPlayer = newLobbyPlayer;
		this.lobbyPlayer.playerCanvasContoller = this;
		if (this.equipmentPanel != null)
		{
			this.equipmentPanel.lobbyPlayer = this.lobbyPlayer;
			this.equipmentPanel.playerHealth = this.lobbyPlayer.playerHealth;
		}
		this.DoInitializations();
	}

	// Token: 0x06000E31 RID: 3633 RVA: 0x00047F59 File Offset: 0x00046159
	private void FetchMultiplayerRoomPlayer()
	{
		if (this.lobbyPlayer == null && MultiplayerRoomPlayer.localMultiplayerRoomPlayer != null && GameMenu.singleton != null)
		{
			this.RegisterLobbyItems(MultiplayerRoomPlayer.localMultiplayerRoomPlayer);
		}
	}

	// Token: 0x06000E32 RID: 3634 RVA: 0x00047F88 File Offset: 0x00046188
	private void ReadyClicked()
	{
		if (this.lobbyPlayer != null && this.lobbyPlayer.selectedMoveSet != null)
		{
			this.lobbyPlayer.SetReady();
			this.UpdateReadyButtonColor();
			if (this.lobbyManager != null)
			{
				this.lobbyManager.CheckReady();
				return;
			}
			if (SingleplayerManager.singleton != null)
			{
				SingleplayerManager.singleton.CheckReady();
			}
		}
	}

	// Token: 0x06000E33 RID: 3635 RVA: 0x00047FEC File Offset: 0x000461EC
	private void ApplyClicked()
	{
		if (this.lobbyPlayer != null)
		{
			this.lobbyPlayer.ApplyTempPlayerValues();
		}
	}

	// Token: 0x06000E34 RID: 3636 RVA: 0x00048004 File Offset: 0x00046204
	public void UpdateReadyButtonColor()
	{
		if (this.lobbyPlayer != null)
		{
			Button component = this.readyButtonGameObject.GetComponent<Button>();
			if (this.lobbyPlayer.playerReadyState)
			{
				UIHelpers.SetButtonColor(component, ButtonState.Ready, null, null);
				return;
			}
			UIHelpers.SetButtonColor(component, ButtonState.NotReady, null, null);
		}
	}

	// Token: 0x06000E35 RID: 3637 RVA: 0x00048045 File Offset: 0x00046245
	private void EquipmentButtonClicked()
	{
		this.SetVisiblePanel(this.equipmentPanel.gameObject);
	}

	// Token: 0x06000E36 RID: 3638 RVA: 0x00048058 File Offset: 0x00046258
	public void GameSettingsButtonClicked()
	{
		this.SetVisiblePanel(this.gameSettingsPanel.gameObject);
	}

	// Token: 0x06000E37 RID: 3639 RVA: 0x0004806B File Offset: 0x0004626B
	private void MoveSetButtonClicked()
	{
		this.SetVisiblePanel(this.moveSetPanelGameObject);
	}

	// Token: 0x06000E38 RID: 3640 RVA: 0x0004807C File Offset: 0x0004627C
	private void SelectMoveSetButtonClicked()
	{
		if (this.lobbyPlayer != null && this.lobbyPlayer.selectedMoveSet != null)
		{
			MoveSet moveSet = Generic.DeepClone<MoveSet>(this.lobbyPlayer.selectedMoveSet);
			if (moveSet.defaultMoveset)
			{
				moveSet.TurnIntoCopy();
			}
			SceneManagerWithParameters.LoadScene("MoveEditor", new Dictionary<string, object>
			{
				{
					"MoveSet",
					moveSet
				}
			}, false, false);
		}
	}

	// Token: 0x06000E39 RID: 3641 RVA: 0x000480DA File Offset: 0x000462DA
	private void NewMoveSetButtonClicked()
	{
		if (this.lobbyPlayer != null && this.lobbyPlayer.selectedMoveSet != null)
		{
			SceneManagerWithParameters.LoadScene("MoveEditor", new Dictionary<string, object>
			{
				{
					"MoveSet",
					MoveSetHelpers.CreateNewMoveSet()
				}
			}, false, false);
		}
	}

	// Token: 0x06000E3A RID: 3642 RVA: 0x00048114 File Offset: 0x00046314
	private void CopyMoveSetButtonClicked()
	{
		if (this.lobbyPlayer != null && this.lobbyPlayer.selectedMoveSet != null)
		{
			this.lobbyPlayer.selectedMoveSet.TurnIntoCopy();
			SceneManagerWithParameters.LoadScene("MoveEditor", new Dictionary<string, object>
			{
				{
					"MoveSet",
					this.lobbyPlayer.selectedMoveSet
				}
			}, false, false);
		}
	}

	// Token: 0x06000E3B RID: 3643 RVA: 0x00048170 File Offset: 0x00046370
	private void CopyToClipboardMoveSetButtonClicked()
	{
		if (this.lobbyPlayer != null && this.lobbyPlayer.selectedMoveSet != null)
		{
			GUIUtility.systemCopyBuffer = this.lobbyPlayer.selectedMoveSet.GetJsonString();
			UnityEngine.Object.Instantiate<GameObject>(this.infoDialogPrefab).GetComponent<BasicInfoDialog>().SetText(LocalizationHelpers.LocalizedText("txt_copied", Array.Empty<object>()), 1f, false);
		}
	}

	// Token: 0x06000E3C RID: 3644 RVA: 0x000481D4 File Offset: 0x000463D4
	private void PasteFromClipboardMoveSetButtonClicked()
	{
		try
		{
			string systemCopyBuffer = GUIUtility.systemCopyBuffer;
			MoveSet moveSet = JsonConvert.DeserializeObject<MoveSet>(systemCopyBuffer);
			moveSet.CreateNewGuid();
			moveSet.FilterMoveSetForProfanity();
			while ((from x in MoveSetHelpers.MoveSets
			where x.name.ToLower() == moveSet.name.ToLower()
			select x).FirstOrDefault<MoveSet>() != null)
			{
				MoveSet moveSet2 = moveSet;
				moveSet2.name += LocalizationHelpers.LocalizedText("txt_append_to_copied_name", Array.Empty<object>());
			}
			MoveSetHelpers.SaveMoveSetJson(moveSet);
			this.GenerateMoveSetButtons(true);
			this.lobbyPlayer.selectedMoveSet = null;
			UnityEngine.Object.Instantiate<GameObject>(this.infoDialogPrefab).GetComponent<BasicInfoDialog>().SetText(LocalizationHelpers.LocalizedText("txt_new_move_added", Array.Empty<object>()), 1f, false);
		}
		catch (Exception message)
		{
			Debug.LogError(message);
			UnityEngine.Object.Instantiate<GameObject>(this.infoDialogPrefab).GetComponent<BasicInfoDialog>().SetText(LocalizationHelpers.LocalizedText("txt_could_not_create_moveset_from_clipboard", Array.Empty<object>()), 1f, false);
		}
	}

	// Token: 0x06000E3D RID: 3645 RVA: 0x000482E0 File Offset: 0x000464E0
	public void DisplayInfoMessage(string text)
	{
		BasicInfoDialog component = UnityEngine.Object.Instantiate<GameObject>(this.infoDialogPrefab).GetComponent<BasicInfoDialog>();
		component.SetCanvasCamera(this.lobbyPlayer.GetCamera());
		component.SetText(text, 1f, false);
	}

	// Token: 0x06000E3E RID: 3646 RVA: 0x00048310 File Offset: 0x00046510
	private void DeleteMoveSetButtonClicked()
	{
		if (this.lobbyPlayer != null && this.lobbyPlayer.selectedMoveSet != null)
		{
			BasicConfirmDialog component = UnityEngine.Object.Instantiate<GameObject>(this.confirmDialogPrefab).GetComponent<BasicConfirmDialog>();
			component.SetText(LocalizationHelpers.LocalizedText("confirm_txt_delete_moveset", new object[]
			{
				this.lobbyPlayer.selectedMoveSet.name
			}), null, false);
			component.okButton.onClick.AddListener(new UnityAction(this.DeleteMoveSet));
			component.cancelButton.Select();
		}
	}

	// Token: 0x06000E3F RID: 3647 RVA: 0x00048395 File Offset: 0x00046595
	private void DeleteMoveSet()
	{
		if (this.lobbyPlayer != null && this.lobbyPlayer.selectedMoveSet != null)
		{
			MoveSetHelpers.DeleteMoveSet(this.lobbyPlayer.selectedMoveSet);
			this.lobbyPlayer.selectedMoveSet = null;
			this.GenerateMoveSetButtons(true);
		}
	}

	// Token: 0x06000E40 RID: 3648 RVA: 0x000483D0 File Offset: 0x000465D0
	public void BackButtonClicked()
	{
		if (GameMenu.singleton != null)
		{
			if (GameMenu.singleton != null)
			{
				GameMenu.singleton.HideMenuTemporarily(false);
				if (typeof(MultiplayerRoomPlayer) == this.lobbyPlayer.GetType())
				{
					((MultiplayerRoomPlayer)this.lobbyPlayer).ClearTempMultiplayerPlayerValues();
				}
			}
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (SingleplayerManager.singleton != null)
		{
			SceneManager.LoadScene("SingleplayerGameSettings");
			return;
		}
		if (this.lobbyPlayer != null)
		{
			this.lobbyPlayer.GoBack();
		}
	}

	// Token: 0x06000E41 RID: 3649 RVA: 0x00048468 File Offset: 0x00046668
	public void SetVisiblePanel(GameObject panel)
	{
		this.moveSetPanelGameObject.SetActive(false);
		this.equipmentPanel.gameObject.SetActive(false);
		this.gameSettingsPanel.gameObject.SetActive(false);
		panel.SetActive(true);
		this.UpdateButtonPaths();
		this.UpdateOtherCanvasButtonPaths();
	}

	// Token: 0x06000E42 RID: 3650 RVA: 0x000484B8 File Offset: 0x000466B8
	private void UpdateOtherCanvasButtonPaths()
	{
		if (this.leftPlayerCanvas != null)
		{
			this.leftPlayerCanvas.UpdateButtonPaths();
		}
		if (this.rightPlayerCanvas != null)
		{
			this.rightPlayerCanvas.UpdateButtonPaths();
		}
		if (this.upPlayerCanvas != null)
		{
			this.upPlayerCanvas.UpdateButtonPaths();
		}
		if (this.downPlayerCanvas != null)
		{
			this.downPlayerCanvas.UpdateButtonPaths();
		}
	}

	// Token: 0x06000E43 RID: 3651 RVA: 0x00048529 File Offset: 0x00046729
	public void DoInitializations()
	{
		this.CreatePlayerCharacterForMoveSelect();
		this.GenerateMoveSetButtons(false);
		this.UpdateSpectatorUI();
	}

	// Token: 0x06000E44 RID: 3652 RVA: 0x0004853E File Offset: 0x0004673E
	public void HidePressAnyButtonText()
	{
		this.pressAnyButtonTextPanel.SetActive(false);
	}

	// Token: 0x06000E45 RID: 3653 RVA: 0x0004854C File Offset: 0x0004674C
	public void ShowPressAnyButtonText()
	{
		this.pressAnyButtonTextPanel.SetActive(true);
	}

	// Token: 0x06000E46 RID: 3654 RVA: 0x0004855A File Offset: 0x0004675A
	public void ShowPressAnyButtonText(string playerTitle)
	{
		this.pressToJoinPlayerTitle.text = playerTitle;
		this.pressAnyButtonTextPanel.SetActive(true);
	}

	// Token: 0x06000E47 RID: 3655 RVA: 0x00048574 File Offset: 0x00046774
	public void MoveSetButtonClicked(Button button, MoveSet moveSet)
	{
		this.SetButtonAsSelected(button);
		if (!moveSet.loaded)
		{
			moveSet = MoveSetHelpers.GetFullMoveSet(moveSet, NetworkManager.singleton != null && NetworkManager.singleton.isNetworkActive);
		}
		if (this.lobbyPlayer != null && moveSet != null)
		{
			this.lobbyPlayer.SetMoveSet(moveSet);
		}
		if (this.equipmentPanel != null)
		{
			this.equipmentPanel.UpdateEquipmentInfo(true, true);
		}
		this.UpdateEquipmentPoints();
	}

	// Token: 0x06000E48 RID: 3656 RVA: 0x000485EA File Offset: 0x000467EA
	public void SetButtonAsSelected(Button button)
	{
		this.ClearMoveSetButtonColors();
		UIHelpers.SetButtonColor(button, ButtonState.Selected, null, null);
	}

	// Token: 0x06000E49 RID: 3657 RVA: 0x000485FC File Offset: 0x000467FC
	public void SetSelectedMoveUI(Button button)
	{
		this.SetButtonAsSelected(button);
		UIHelpers.SnapScrollViewTo(button.GetComponent<RectTransform>(), this.moveSetsButtonScrollRect);
		if (this.lobbyPlayer != null)
		{
			this.lobbyPlayer.UpdatePreviewVisuals();
		}
		if (this.equipmentPanel != null)
		{
			this.equipmentPanel.UpdateEquipmentInfo(true, true);
		}
		this.UpdateEquipmentPoints();
	}

	// Token: 0x06000E4A RID: 3658 RVA: 0x00048658 File Offset: 0x00046858
	public void ClearMoveSetButtonColors()
	{
		foreach (Button button in this.moveSetButtonList)
		{
			UIHelpers.SetButtonColor(button, ButtonState.Basic, null, null);
		}
	}

	// Token: 0x06000E4B RID: 3659 RVA: 0x000486AC File Offset: 0x000468AC
	public void GenerateMoveSetButtons(bool reload = false)
	{
		this.moveSetButtonList = new List<Button>();
		int num = 0;
		foreach (object obj in this.moveSetsButtonPanel.transform)
		{
			UnityEngine.Object.Destroy(((Transform)obj).gameObject);
		}
		if (reload)
		{
			MoveSetHelpers.ClearLoadedMoveSets();
		}
		Button button2 = null;
		MoveSet moveSet = null;
		bool flag = false;
		using (List<MoveSet>.Enumerator enumerator2 = MoveSetHelpers.FilterMovesets(MoveSetHelpers.LoadMoveSetsJson(this.moveSetEditorSelect), this.lobbyPlayer != null && this.lobbyPlayer.ai).GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				MoveSet move = enumerator2.Current;
				GameObject original = this.moveSetButtonPrefab;
				if (move.communityMoveset)
				{
					original = this.communityMovesetButtonPrefab;
				}
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
				gameObject.AddComponent<InputMoveScrollViewOnSelect>().scrollRect = this.moveSetsButtonScrollRect;
				Button button = gameObject.GetComponent<Button>();
				Text componentInChildren = gameObject.GetComponentInChildren<Text>();
				if (num == 0)
				{
					button2 = button;
					moveSet = move;
				}
				else
				{
					UIHelpers.SetButtonColor(button, ButtonState.Basic, null, null);
				}
				button.onClick.AddListener(delegate()
				{
					this.MoveSetButtonClicked(button, move);
				});
				componentInChildren.text = move.name;
				if (move.communityMoveset)
				{
					gameObject.GetComponentsInChildren<Text>()[1].text = LocalizationHelpers.LocalizedText("txt_community_moveset", new object[]
					{
						move.GetCreatorName()
					});
				}
				gameObject.transform.SetParent(this.moveSetsButtonPanel.transform);
				gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
				RectTransform component = gameObject.GetComponent<RectTransform>();
				component.anchorMin = new Vector2(0f, 1f);
				component.anchorMax = new Vector2(0f, 1f);
				component.localScale = new Vector3(1f, 1f, 1f);
				component.anchoredPosition = new Vector3(component.rect.width / 2f, (component.rect.height / 2f + component.rect.height * (float)num + (float)(num * 2)) * -1f, 0f);
				gameObject.transform.SetSiblingIndex(num);
				if (num == 0)
				{
					this.buttonToActivate = gameObject;
				}
				num++;
				this.moveSetButtonList.Add(button);
				if (this.lobbyPlayer != null && this.lobbyPlayer.GetMoveSet() != null && this.lobbyPlayer.GetMoveSet().Equals(move))
				{
					this.SetSelectedMoveUI(button);
					flag = true;
				}
			}
		}
		if (!flag)
		{
			this.MoveSetButtonClicked(button2, moveSet);
		}
		this.UpdateButtonPaths();
	}

	// Token: 0x06000E4C RID: 3660 RVA: 0x00048A20 File Offset: 0x00046C20
	public void UpdateButtonPaths()
	{
		Button button = this.moveSetButtonList.Last<Button>();
		Button button2 = this.moveSetButton;
		Button button3 = this.moveSetButton;
		INavigationListOption navigationListOption = null;
		if (this.moveSetEditorSelect)
		{
			button2 = this.selectMoveSetButton;
		}
		if (this.backButton.gameObject.activeInHierarchy)
		{
			button2 = this.backButton;
		}
		if (this.removePlayerButton.gameObject.activeInHierarchy)
		{
			button2 = this.removePlayerButton;
		}
		int num = 0;
		if (this.moveSetPanelGameObject != null && this.moveSetPanelGameObject.gameObject.activeInHierarchy)
		{
			using (List<Button>.Enumerator enumerator = this.moveSetButtonList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Selectable selectable = enumerator.Current;
					Navigation navigation = default(Navigation);
					navigation.mode = Navigation.Mode.Explicit;
					Button selectOnUp = null;
					if (num != 0)
					{
						selectOnUp = this.moveSetButtonList[num - 1];
					}
					else if (this.upPlayerCanvas != null)
					{
						selectOnUp = this.upPlayerCanvas.moveSetButton;
					}
					Button selectOnDown;
					if (num < this.moveSetButtonList.Count - 1)
					{
						selectOnDown = this.moveSetButtonList[num + 1];
					}
					else
					{
						selectOnDown = button2;
					}
					navigation.selectOnUp = selectOnUp;
					navigation.selectOnDown = selectOnDown;
					navigation.selectOnRight = button3;
					if (this.leftPlayerCanvas != null)
					{
						if (this.leftPlayerCanvas.readyButton.gameObject.activeInHierarchy)
						{
							navigation.selectOnLeft = this.leftPlayerCanvas.readyButton;
						}
						else
						{
							navigation.selectOnLeft = this.leftPlayerCanvas.equipmentButton;
						}
					}
					selectable.navigation = navigation;
					num++;
				}
				goto IL_38D;
			}
		}
		if (this.equipmentPanel != null && this.equipmentPanel.gameObject.activeInHierarchy)
		{
			button = this.equipmentPanel.BottomCenter;
			Navigation navigation2 = this.equipmentPanel.BottomCenter.navigation;
			navigation2.selectOnDown = button2;
			this.equipmentPanel.BottomCenter.navigation = navigation2;
			Navigation navigation3 = this.equipmentPanel.BottomLeft.navigation;
			navigation3.selectOnDown = button2;
			this.equipmentPanel.BottomLeft.navigation = navigation3;
			Navigation navigation4 = this.equipmentPanel.BottomRight.navigation;
			navigation4.selectOnDown = button2;
			this.equipmentPanel.BottomRight.navigation = navigation4;
			Navigation navigation5 = this.equipmentPanel.RightButton.navigation;
			navigation5.selectOnRight = this.equipmentButton;
			this.equipmentPanel.RightButton.navigation = navigation5;
			if (this.leftPlayerCanvas != null)
			{
				Navigation navigation6 = this.equipmentPanel.LeftButton.navigation;
				if (this.leftPlayerCanvas != null)
				{
					if (this.leftPlayerCanvas.readyButton.gameObject.activeInHierarchy)
					{
						navigation6.selectOnLeft = this.leftPlayerCanvas.readyButton;
					}
					else
					{
						navigation6.selectOnLeft = this.leftPlayerCanvas.equipmentButton;
					}
				}
				this.equipmentPanel.LeftButton.navigation = navigation6;
			}
			if (this.upPlayerCanvas != null)
			{
				Navigation navigation7 = this.equipmentPanel.TopButton.navigation;
				navigation7.selectOnUp = this.upPlayerCanvas.moveSetButton;
				this.equipmentPanel.TopButton.navigation = navigation7;
			}
		}
		else if (this.gameSettingsPanel != null && this.gameSettingsPanel.gameObject.activeInHierarchy)
		{
			this.gameSettingsPanel.rightNavigation = button3;
			this.gameSettingsPanel.downNavigation = button2;
			navigationListOption = this.gameSettingsPanel.UpdateNavigation();
		}
		IL_38D:
		if (this.moveSetEditorSelect)
		{
			Navigation navigation8 = default(Navigation);
			navigation8.mode = Navigation.Mode.Explicit;
			navigation8.selectOnUp = button;
			navigation8.selectOnLeft = this.newMoveSetButton;
			this.selectMoveSetButton.navigation = navigation8;
			Navigation navigation9 = default(Navigation);
			navigation9.mode = Navigation.Mode.Explicit;
			navigation9.selectOnUp = button;
			navigation9.selectOnRight = this.selectMoveSetButton;
			navigation9.selectOnLeft = this.deleteMoveSetButton;
			this.newMoveSetButton.navigation = navigation9;
			Navigation navigation10 = default(Navigation);
			navigation10.mode = Navigation.Mode.Explicit;
			navigation10.selectOnUp = button;
			navigation10.selectOnRight = this.newMoveSetButton;
			navigation10.selectOnLeft = this.backButton;
			this.deleteMoveSetButton.navigation = navigation10;
			Navigation navigation11 = default(Navigation);
			navigation11.mode = Navigation.Mode.Explicit;
			navigation11.selectOnUp = button;
			navigation11.selectOnRight = this.deleteMoveSetButton;
			this.deleteMoveSetButton.navigation = navigation11;
			return;
		}
		Button button4 = this.readyButton;
		if (this.applyButton.gameObject.activeInHierarchy)
		{
			button4 = this.applyButton;
		}
		Navigation navigation12 = default(Navigation);
		navigation12.mode = Navigation.Mode.Explicit;
		navigation12.selectOnUp = button;
		navigation12.selectOnLeft = this.equipmentButton;
		Navigation navigation13 = default(Navigation);
		navigation13.mode = Navigation.Mode.Explicit;
		navigation13.selectOnUp = button;
		navigation13.selectOnRight = button4;
		navigation13.selectOnLeft = this.moveSetButton;
		Navigation navigation14 = default(Navigation);
		navigation14.mode = Navigation.Mode.Explicit;
		navigation14.selectOnUp = button;
		navigation14.selectOnRight = this.equipmentButton;
		navigation14.selectOnLeft = button;
		if (this.rightPlayerCanvas != null)
		{
			navigation12.selectOnRight = this.rightPlayerCanvas.moveSetButton;
			if (!this.readyButton.gameObject.activeInHierarchy)
			{
				navigation13.selectOnRight = this.rightPlayerCanvas.moveSetButton;
			}
		}
		if (this.downPlayerCanvas != null)
		{
			navigation14.selectOnDown = this.downPlayerCanvas.moveSetButtonList.FirstOrDefault<Button>();
			if (this.downPlayerCanvas.equipmentPanel != null && this.downPlayerCanvas.equipmentPanel.gameObject.activeInHierarchy)
			{
				navigation14.selectOnDown = this.downPlayerCanvas.equipmentPanel.TopButton;
			}
			navigation13.selectOnDown = this.downPlayerCanvas.equipmentButton;
			navigation12.selectOnDown = this.downPlayerCanvas.readyButton;
		}
		if (this.upPlayerCanvas != null)
		{
			navigation13.selectOnUp = this.upPlayerCanvas.equipmentButton;
			navigation12.selectOnUp = this.upPlayerCanvas.readyButton;
		}
		Navigation navigation15 = default(Navigation);
		if (this.backButton.gameObject.activeInHierarchy)
		{
			navigation15.mode = Navigation.Mode.Explicit;
			navigation15.selectOnUp = button;
			navigation15.selectOnRight = this.moveSetButton;
			navigation14.selectOnLeft = this.backButton;
		}
		Navigation navigation16 = default(Navigation);
		if (this.removePlayerButton.gameObject.activeInHierarchy)
		{
			navigation16.mode = Navigation.Mode.Explicit;
			navigation16.selectOnUp = button;
			navigation16.selectOnRight = this.moveSetButton;
			if (this.downPlayerCanvas != null)
			{
				navigation16.selectOnDown = this.downPlayerCanvas.moveSetButtonList.FirstOrDefault<Button>();
			}
			navigation14.selectOnLeft = this.removePlayerButton;
		}
		if (this.gameSettingsButton.gameObject.activeInHierarchy)
		{
			Navigation navigation17 = default(Navigation);
			navigation17.mode = Navigation.Mode.Explicit;
			navigation17.selectOnUp = button;
			navigation17.selectOnRight = this.moveSetButton;
			navigation17.selectOnLeft = this.backButton;
			navigation15.selectOnRight = this.gameSettingsButton;
			navigation14.selectOnLeft = this.gameSettingsButton;
			if (navigationListOption != null)
			{
				Selectable leftSideNavigation = navigationListOption.GetLeftSideNavigation();
				Selectable rightSideNavigation = navigationListOption.GetRightSideNavigation();
				navigation15.selectOnUp = leftSideNavigation;
				navigation17.selectOnUp = rightSideNavigation;
				navigation12.selectOnUp = rightSideNavigation;
				navigation13.selectOnUp = rightSideNavigation;
			}
			this.gameSettingsButton.navigation = navigation17;
		}
		this.removePlayerButton.navigation = navigation16;
		this.backButton.navigation = navigation15;
		button4.navigation = navigation12;
		this.equipmentButton.navigation = navigation13;
		this.moveSetButton.navigation = navigation14;
	}

	// Token: 0x06000E4D RID: 3661 RVA: 0x000491EC File Offset: 0x000473EC
	public void CreatePlayerCharacterForMoveSelect()
	{
		if (this.lobbyPlayer != null && this.playerCharacterPrefab != null)
		{
			Camera camera = this.lobbyPlayer.GetCamera();
			if (camera == null)
			{
				camera = Camera.main;
			}
			if (MultiplayerRoomPlayer.tempEditMode)
			{
				this.tempLobbyCamera = UnityEngine.Object.Instantiate<GameObject>(this.tempLobbyCameraPrefab, new Vector3(100f, 100f, 100f), default(Quaternion));
				camera = this.tempLobbyCamera.GetComponent<Camera>();
				this.tempLobbyBackgroundImage.enabled = true;
			}
			int num = 0;
			if (this.lobbyManager != null)
			{
				num = 2;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.playerCharacterPrefab, new Vector3(camera.transform.position.x + (float)num, camera.transform.position.y - 1f, camera.transform.position.z + 4f), camera.transform.rotation);
			if (this.tempLobbyCamera)
			{
				this.tempPlayerCharacter = gameObject;
			}
			this.lobbyPlayer.playerHealth = gameObject.GetComponent<PlayerHealth>();
			this.lobbyPlayer.playerHealth.ai = this.lobbyPlayer.ai;
			this.lobbyPlayer.playerHealth.LoadLocalCustomPlayerTexture();
			float y = gameObject.transform.position.y;
			if (NetworkClient.active && !MultiplayerRoomPlayer.tempEditMode)
			{
				y = -100f;
			}
			gameObject.transform.position = new Vector3(gameObject.transform.position.x + this.lobbyPlayer.playerHealth.playerAnimator.transform.localPosition.x, y, gameObject.transform.position.z);
			gameObject.transform.Rotate(new Vector3(0f, 180f, 0f));
			this.lobbyPlayer.playerHealth.OnlyAnimation();
			if (this.lobbyManager != null)
			{
				this.lobbyPlayer.playerHealth.playerAnimator.gameObject.transform.Rotate(new Vector3(0f, 22f, 0f));
			}
			if (this.equipmentPanel.playerHealth == null)
			{
				this.equipmentPanel.playerHealth = this.lobbyPlayer.playerHealth;
			}
		}
	}

	// Token: 0x06000E4E RID: 3662 RVA: 0x0004944C File Offset: 0x0004764C
	public void UpdateEquipmentPoints()
	{
		string text = "";
		string text2 = "";
		if (this.EquipmentPointsText != null)
		{
			if (this.lobbyPlayer != null)
			{
				text = string.Format("{0}", GameSettingsHelper.CountEquippedEquipmentPoints(this.lobbyPlayer.GetSelectedEquipment()));
			}
			if (IGameSettingsManager.singleton != null && IGameSettingsManager.singleton.EquipmentPoints > 0 && SceneManager.GetActiveScene().name != "LobbyMoveEditor")
			{
				if (this.lobbyPlayer != null && !GameSettingsHelper.CheckCanPlayerReadyByEquipmentPoints(this.lobbyPlayer.GetSelectedEquipment()))
				{
					text = string.Concat(new string[]
					{
						"<color=#",
						ColorUtility.ToHtmlStringRGBA(UISettings.BasicButtonNotReadyColor),
						">",
						text,
						"</color>"
					});
				}
				text2 = string.Format("/{0}", IGameSettingsManager.singleton.EquipmentPoints);
			}
			this.EquipmentPointsText.text = LocalizationHelpers.LocalizedText("txt_points", new object[]
			{
				text,
				text2
			});
		}
	}

	// Token: 0x06000E4F RID: 3663 RVA: 0x0004955D File Offset: 0x0004775D
	public void UpdateSpectatorUI()
	{
		if (this.lobbyPlayer != null)
		{
			this.spectatorToggle.SetIsOnWithoutNotify(this.lobbyPlayer.GetSpectator());
		}
	}

	// Token: 0x06000E50 RID: 3664 RVA: 0x0004957D File Offset: 0x0004777D
	private void SpectatorChanged()
	{
		if (this.lobbyPlayer != null)
		{
			this.lobbyPlayer.SetSpectator(this.spectatorToggle.isOn);
		}
	}

	// Token: 0x06000E51 RID: 3665 RVA: 0x0004959D File Offset: 0x0004779D
	private void OnDestroy()
	{
		if (this.tempLobbyCamera != null)
		{
			UnityEngine.Object.Destroy(this.tempLobbyCamera);
		}
		if (this.tempPlayerCharacter != null)
		{
			UnityEngine.Object.Destroy(this.tempPlayerCharacter);
		}
		this.DisposeUserControls();
	}

	// Token: 0x06000E52 RID: 3666 RVA: 0x000495D7 File Offset: 0x000477D7
	public void DisposeUserControls()
	{
		if (this.userControls != null)
		{
			this.userControls.Disable();
			this.userControls.Dispose();
		}
	}

	// Token: 0x04000A25 RID: 2597
	public GameObject pressAnyButtonTextPanel;

	// Token: 0x04000A26 RID: 2598
	public GameObject moveSetsButtonPanel;

	// Token: 0x04000A27 RID: 2599
	public ScrollRect moveSetsButtonScrollRect;

	// Token: 0x04000A28 RID: 2600
	public GameObject moveSetButtonPrefab;

	// Token: 0x04000A29 RID: 2601
	public GameObject communityMovesetButtonPrefab;

	// Token: 0x04000A2A RID: 2602
	public GameObject buttonToActivate;

	// Token: 0x04000A2B RID: 2603
	private List<Button> moveSetButtonList = new List<Button>();

	// Token: 0x04000A2C RID: 2604
	public GameObject readyButtonGameObject;

	// Token: 0x04000A2D RID: 2605
	public Button applyButton;

	// Token: 0x04000A2E RID: 2606
	public Button readyButton;

	// Token: 0x04000A2F RID: 2607
	public Button equipmentButton;

	// Token: 0x04000A30 RID: 2608
	public Button moveSetButton;

	// Token: 0x04000A31 RID: 2609
	public Button gameSettingsButton;

	// Token: 0x04000A32 RID: 2610
	public Button selectMoveSetButton;

	// Token: 0x04000A33 RID: 2611
	public Button newMoveSetButton;

	// Token: 0x04000A34 RID: 2612
	public Button deleteMoveSetButton;

	// Token: 0x04000A35 RID: 2613
	public Button copyMoveSetButton;

	// Token: 0x04000A36 RID: 2614
	public Button copyToClipboardMoveSetButton;

	// Token: 0x04000A37 RID: 2615
	public Button pasteFromClipboardMoveSetButton;

	// Token: 0x04000A38 RID: 2616
	public Button backButton;

	// Token: 0x04000A39 RID: 2617
	public Button removePlayerButton;

	// Token: 0x04000A3A RID: 2618
	public Text pressToJoinPlayerTitle;

	// Token: 0x04000A3B RID: 2619
	public Text EquipmentPointsText;

	// Token: 0x04000A3C RID: 2620
	public LobbyLocalManager lobbyManager;

	// Token: 0x04000A3D RID: 2621
	public IRoomPlayer lobbyPlayer;

	// Token: 0x04000A3E RID: 2622
	public GameObject EquipmentEditorPrefab;

	// Token: 0x04000A3F RID: 2623
	public GameObject GameSettingsPrefab;

	// Token: 0x04000A40 RID: 2624
	public EquipmentPanel equipmentPanel;

	// Token: 0x04000A41 RID: 2625
	public GameSettingsPanel gameSettingsPanel;

	// Token: 0x04000A42 RID: 2626
	public GameObject moveSetPanelGameObject;

	// Token: 0x04000A43 RID: 2627
	public GameObject panelsHolder;

	// Token: 0x04000A44 RID: 2628
	public GameObject playerCharacterPrefab;

	// Token: 0x04000A45 RID: 2629
	public bool moveSetEditorSelect;

	// Token: 0x04000A46 RID: 2630
	public GameObject confirmDialogPrefab;

	// Token: 0x04000A47 RID: 2631
	public GameObject infoDialogPrefab;

	// Token: 0x04000A48 RID: 2632
	public UserControls userControls;

	// Token: 0x04000A49 RID: 2633
	public PlayerCanvasController rightPlayerCanvas;

	// Token: 0x04000A4A RID: 2634
	public PlayerCanvasController leftPlayerCanvas;

	// Token: 0x04000A4B RID: 2635
	public PlayerCanvasController upPlayerCanvas;

	// Token: 0x04000A4C RID: 2636
	public PlayerCanvasController downPlayerCanvas;

	// Token: 0x04000A4D RID: 2637
	public Toggle spectatorToggle;

	// Token: 0x04000A4E RID: 2638
	public RawImage tempLobbyBackgroundImage;

	// Token: 0x04000A4F RID: 2639
	public GameObject tempLobbyCameraPrefab;

	// Token: 0x04000A50 RID: 2640
	public GameObject tempLobbyCamera;

	// Token: 0x04000A51 RID: 2641
	public GameObject tempPlayerCharacter;
}
