using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using MoveClasses;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

// Token: 0x020001E0 RID: 480
public class GameMenu : MonoBehaviour
{
	// Token: 0x17000199 RID: 409
	// (get) Token: 0x06000E76 RID: 3702 RVA: 0x00049949 File Offset: 0x00047B49
	public static bool GameMenuCurrentlyHidden
	{
		get
		{
			return (GameMenu.singleton != null && !GameMenu.singleton.menuHolderPanel.activeInHierarchy) || GameMenu.singleton == null;
		}
	}

	// Token: 0x06000E77 RID: 3703 RVA: 0x0004997C File Offset: 0x00047B7C
	private void Awake()
	{
		this.userControls = SettingsHelper.GetUserControls();
		this.userControls.Generic.Enable();
		GameMenu.singleton = this;
		this.gameOverPanel.SetActive(false);
		this.menuHolderPanel.SetActive(false);
		this.backToMoveSetEditButton.gameObject.SetActive(false);
		this.resumeButton.onClick.AddListener(delegate()
		{
			this.ShowMenu();
		});
		this.restartButton.onClick.AddListener(delegate()
		{
			this.RestartGame();
		});
		this.saveReplayButton.onClick.AddListener(delegate()
		{
			this.SaveReplay();
		});
		this.optionsButton.onClick.AddListener(delegate()
		{
			this.OpenOptions();
		});
		this.movesetButton.onClick.AddListener(delegate()
		{
			this.OpenMovesets();
		});
		this.returnToLobbyButton.onClick.AddListener(delegate()
		{
			this.ReturnToLobbyGame();
		});
		this.leaveButton.onClick.AddListener(delegate()
		{
			this.LeaveGame();
		});
		this.SetupMultiplayer();
		this.SetupWelcomeMessage();
		this.restartButton.gameObject.SetActive(false);
		this.saveReplayButton.gameObject.SetActive(false);
		this.optionsButton.gameObject.SetActive(true);
		this.returnToLobbyButton.gameObject.SetActive(false);
		this.movesetButton.gameObject.SetActive(false);
		this.continueButton.gameObject.SetActive(false);
		this.forfeitRoundButton.gameObject.SetActive(false);
		this.abandonRunButton.gameObject.SetActive(false);
		this.retryButton.gameObject.SetActive(false);
		if (NetworkManager.singleton != null && NetworkManager.singleton.isNetworkActive)
		{
			if (this.localMultiplayerRoomPlayer != null && this.localMultiplayerRoomPlayer.isServer)
			{
				this.restartButton.gameObject.SetActive(true);
				this.returnToLobbyButton.gameObject.SetActive(true);
			}
			this.movesetButton.gameObject.SetActive(true);
		}
		else if (SingleplayerManager.singleton != null)
		{
			this.continueButton.onClick.AddListener(delegate()
			{
				this.Continue();
			});
			this.retryButton.onClick.AddListener(delegate()
			{
				this.Retry();
			});
			this.forfeitRoundButton.onClick.AddListener(delegate()
			{
				this.ForfeitRound();
			});
			this.abandonRunButton.onClick.AddListener(delegate()
			{
				this.AbandonRun();
			});
			this.leaveButton.gameObject.SetActive(false);
			this.forfeitRoundButton.gameObject.SetActive(true);
			this.abandonRunButton.gameObject.SetActive(true);
		}
		else
		{
			this.restartButton.gameObject.SetActive(true);
		}
		if (SceneManagerWithParameters.currentScene == "MoveEditorTestMoveSet")
		{
			this.leaveButton.gameObject.SetActive(false);
			this.backToMoveSetEditButton.gameObject.SetActive(true);
			this.backToMoveSetEditButton.onClick.AddListener(delegate()
			{
				this.BackToMoveSetEdit();
			});
		}
		else if (ReplayManager.singleton != null && ReplayManager.singleton.replayMode == ReplayMode.Record)
		{
			this.saveReplayButton.gameObject.SetActive(true);
		}
		if (GeneralManager.singleton != null)
		{
			GeneralManager.singleton.UpdateCursorState();
			GeneralManager.singleton.UpdateChatVisibility(false);
		}
	}

	// Token: 0x06000E78 RID: 3704 RVA: 0x00049D0F File Offset: 0x00047F0F
	public void SetupMultiplayer()
	{
		this.localMultiplayerRoomPlayer = MultiplayerRoomPlayer.localMultiplayerRoomPlayer;
	}

	// Token: 0x06000E79 RID: 3705 RVA: 0x00049D1C File Offset: 0x00047F1C
	public void SetupWelcomeMessage()
	{
		this.welcomeMessageButton.gameObject.SetActive(false);
		if (this.localMultiplayerRoomPlayer != null && IGameSettingsManager.singleton != null && !string.IsNullOrWhiteSpace(IGameSettingsManager.singleton.WelcomeMessage))
		{
			this.welcomeMessageButton.gameObject.SetActive(true);
			this.welcomeMessageButton.onClick.AddListener(delegate()
			{
				IGameSettingsManager.singleton.DisplayWelcomeMessage();
			});
		}
	}

	// Token: 0x06000E7A RID: 3706 RVA: 0x00049DA0 File Offset: 0x00047FA0
	private void Update()
	{
		if (this.userControls.Generic.OpenMenu.WasPressedThisFrame())
		{
			this.ShowMenu();
		}
	}

	// Token: 0x06000E7B RID: 3707 RVA: 0x00049DD0 File Offset: 0x00047FD0
	private void ShowMenu()
	{
		if (this.menuHolderPanel.activeInHierarchy)
		{
			this.ResumeGame();
		}
		else
		{
			this.DisplayMenu(false);
		}
		if (GeneralManager.singleton != null)
		{
			GeneralManager.singleton.UpdateCursorState();
			GeneralManager.singleton.UpdateInputSystemState();
			GeneralManager.singleton.UpdateChatVisibility(true);
		}
		this.UpdatePauseState();
		if (ReplayManager.singleton)
		{
			ReplayManager.singleton.CheckTempPauseStatus();
		}
		this.UpdatePlayerList();
	}

	// Token: 0x06000E7C RID: 3708 RVA: 0x00049E47 File Offset: 0x00048047
	public void UpdatePauseState()
	{
		if (GameMaster.singleton != null)
		{
			GameMaster.singleton.SetPlayOrPause(GameMenu.GameMenuCurrentlyHidden);
		}
	}

	// Token: 0x06000E7D RID: 3709 RVA: 0x00049E68 File Offset: 0x00048068
	private void ResumeGame()
	{
		if (GeneralManager.AllowBackNavigation(null) && !(this.playerOptionsManager != null))
		{
			if (this.playerCanvasController != null)
			{
				this.playerCanvasController.BackButtonClicked();
				return;
			}
			if (!this.gameOverPanel.activeInHierarchy)
			{
				this.menuHolderPanel.SetActive(false);
			}
		}
	}

	// Token: 0x06000E7E RID: 3710 RVA: 0x00049EC0 File Offset: 0x000480C0
	private void DisplayMenu(bool force = false)
	{
		bool flag = force || GeneralManager.AllowBackNavigation(null);
		if (flag)
		{
			this.menuHolderPanel.SetActive(true);
			this.UpdateSelectedButton();
		}
	}

	// Token: 0x06000E7F RID: 3711 RVA: 0x00049EF4 File Offset: 0x000480F4
	private void UpdateSelectedButton()
	{
		if (EventSystem.current.currentSelectedGameObject == null || !EventSystem.current.currentSelectedGameObject.activeInHierarchy)
		{
			if (this.resumeButton.gameObject.activeInHierarchy)
			{
				this.resumeButton.Select();
				return;
			}
			if (this.restartButton.gameObject.activeInHierarchy)
			{
				this.restartButton.Select();
			}
		}
	}

	// Token: 0x06000E80 RID: 3712 RVA: 0x00049F60 File Offset: 0x00048160
	public void RestartGame()
	{
		if (NetworkManager.singleton != null && NetworkManager.singleton.isNetworkActive)
		{
			if (this.localMultiplayerRoomPlayer != null && this.localMultiplayerRoomPlayer.isServer)
			{
				((MultiplayerRoomManager)NetworkManager.singleton).ServerChangeScene(NetworkManager.networkSceneName);
				return;
			}
		}
		else if (SingleplayerManager.singleton == null)
		{
			SceneManagerWithParameters.ReloadScene();
		}
	}

	// Token: 0x06000E81 RID: 3713 RVA: 0x00049FC8 File Offset: 0x000481C8
	private void SaveReplay()
	{
		if (ReplayManager.singleton != null)
		{
			ReplayManager.singleton.SaveRecording();
		}
	}

	// Token: 0x06000E82 RID: 3714 RVA: 0x00049FE1 File Offset: 0x000481E1
	private void OpenMovesets()
	{
		this.playerCanvasController = UnityEngine.Object.Instantiate<GameObject>(this.movesetPanelPrefab).GetComponent<PlayerCanvasController>();
	}

	// Token: 0x06000E83 RID: 3715 RVA: 0x00049FF9 File Offset: 0x000481F9
	private void OpenOptions()
	{
		this.playerOptionsManager = UnityEngine.Object.Instantiate<GameObject>(this.optionsPanelPrefab, this.gameMenuCanvas.transform).GetComponent<PlayerOptionsManager>();
	}

	// Token: 0x06000E84 RID: 3716 RVA: 0x0004A01C File Offset: 0x0004821C
	public void HideMenuTemporarily(bool hide = true)
	{
		if (hide)
		{
			this.menuButtonHolder.SetActive(false);
			this.playerListPanel.SetActive(false);
			return;
		}
		this.menuButtonHolder.SetActive(true);
		this.UpdatePlayerList();
	}

	// Token: 0x06000E85 RID: 3717 RVA: 0x0004A04C File Offset: 0x0004824C
	private void ReturnToLobbyGame()
	{
		if (NetworkManager.singleton != null && this.localMultiplayerRoomPlayer != null && this.localMultiplayerRoomPlayer.isServer)
		{
			NetworkManager.singleton.ServerChangeScene(((MultiplayerRoomManager)NetworkManager.singleton).RoomScene);
		}
	}

	// Token: 0x06000E86 RID: 3718 RVA: 0x0004A09A File Offset: 0x0004829A
	private void LeaveGame()
	{
		if (this.localMultiplayerRoomPlayer != null)
		{
			this.localMultiplayerRoomPlayer.GoBack();
			return;
		}
		SceneManagerWithParameters.LoadScene("MainMenu", null, false, false);
	}

	// Token: 0x06000E87 RID: 3719 RVA: 0x0004A0C4 File Offset: 0x000482C4
	private void BackToMoveSetEdit()
	{
		MoveSet value = (MoveSet)SceneManagerWithParameters.GetParameter("MoveSet");
		Stance value2 = (Stance)SceneManagerWithParameters.GetParameter("SelectedStance");
		Move value3 = (Move)SceneManagerWithParameters.GetParameter("SelectedMove");
		SceneManagerWithParameters.LoadScene("MoveEditor", new Dictionary<string, object>
		{
			{
				"MoveSet",
				value
			},
			{
				"SelectedStance",
				value2
			},
			{
				"SelectedMove",
				value3
			}
		}, false, false);
	}

	// Token: 0x06000E88 RID: 3720 RVA: 0x0004A138 File Offset: 0x00048338
	public void ShowWinScreenInfo(WinScreenInfo info)
	{
		if (info.gameEndResultType == GameEndResultType.Win)
		{
			this.winnerTextGameOver.text = LocalizationHelpers.LocalizedText("title_winner", new object[]
			{
				info.winningPlayerName
			});
		}
		else if (info.gameEndResultType == GameEndResultType.Loss)
		{
			this.winnerTextGameOver.text = LocalizationHelpers.LocalizedText("title_you_lost", Array.Empty<object>());
		}
		else
		{
			this.winnerTextGameOver.text = LocalizationHelpers.LocalizedText("title_draw", Array.Empty<object>());
		}
		this.ShowGameOverPanel();
	}

	// Token: 0x06000E89 RID: 3721 RVA: 0x0004A1B8 File Offset: 0x000483B8
	public void UpdatePlayerList()
	{
		if (!this.menuHolderPanel.activeInHierarchy || !this.menuButtonHolder.activeInHierarchy)
		{
			return;
		}
		if (SceneManagerWithParameters.currentScene == "MoveEditorTestMoveSet")
		{
			this.playerListPanel.SetActive(false);
			return;
		}
		int num = 0;
		foreach (object obj in this.playerListPanel.transform)
		{
			Transform transform = (Transform)obj;
			if (num != 0)
			{
				UnityEngine.Object.Destroy(transform.gameObject);
			}
			num++;
		}
		List<MultiplayerRoomPlayer> list = UnityEngine.Object.FindObjectsOfType<MultiplayerRoomPlayer>().ToList<MultiplayerRoomPlayer>();
		if (list.Count > 0)
		{
			this.playerListPanel.SetActive(true);
			using (IEnumerator<MultiplayerRoomPlayer> enumerator2 = (from x in list
			orderby x.spectator, x.deathTime != null, x.deathTime descending
			select x).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					MultiplayerRoomPlayer multiplayerRoomPlayer = enumerator2.Current;
					if (!multiplayerRoomPlayer.disconnecting)
					{
						PlayerRowPanel component = UnityEngine.Object.Instantiate<GameObject>(this.playerListItemPrefab, this.playerListPanel.transform).GetComponent<PlayerRowPanel>();
						component.playerName.text = multiplayerRoomPlayer.playerName;
						component.ping.text = string.Format("{0}", multiplayerRoomPlayer.ping);
						if (multiplayerRoomPlayer.spectator)
						{
							component.playerDeathReason.text = LocalizationHelpers.LocalizedText("text_spectator", Array.Empty<object>());
						}
						else
						{
							component.playerDeathReason.text = multiplayerRoomPlayer.playerDeathReason.GetDescription();
						}
						component.SetMultiplayerRoomPlayer(multiplayerRoomPlayer);
					}
				}
				return;
			}
		}
		if (ReplayManager.singleton != null && ReplayManager.singleton.replayMode == ReplayMode.Replay && ReplayManager.singleton.recording != null)
		{
			this.ping.gameObject.SetActive(false);
			this.playerListPanel.SetActive(true);
			using (IEnumerator<RGO> enumerator3 = (from x in ReplayManager.singleton.recording.recRGO
			where x.isPlayer
			orderby x.deathEvent != null
			select x).ThenByDescending(delegate(RGO x)
			{
				DE deathEvent = x.deathEvent;
				if (deathEvent == null)
				{
					return null;
				}
				return new int?(deathEvent.tick);
			}).GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					RGO rgo = enumerator3.Current;
					PlayerRowPanel component2 = UnityEngine.Object.Instantiate<GameObject>(this.playerListItemPrefab, this.playerListPanel.transform).GetComponent<PlayerRowPanel>();
					component2.playerName.text = rgo.name;
					if (rgo.deathEvent != null && rgo.deathEvent.tick <= ReplayManager.singleton.currentTick)
					{
						component2.playerDeathReason.text = rgo.deathEvent.deathReason.GetDescription();
					}
					else
					{
						component2.playerDeathReason.text = "";
					}
					component2.ping.gameObject.SetActive(false);
				}
				return;
			}
		}
		this.ping.gameObject.SetActive(false);
		this.playerListPanel.SetActive(true);
		if (GameMaster.singleton != null)
		{
			foreach (PlayerGameStateInfo playerGameStateInfo in from x in GameMaster.singleton.registeredPlayers
			orderby x.deathTime != null, x.deathTime descending
			select x)
			{
				PlayerRowPanel component3 = UnityEngine.Object.Instantiate<GameObject>(this.playerListItemPrefab, this.playerListPanel.transform).GetComponent<PlayerRowPanel>();
				component3.playerName.text = playerGameStateInfo.player.playerName;
				if (playerGameStateInfo.deathTime != null)
				{
					component3.playerDeathReason.text = playerGameStateInfo.deathReason.GetDescription();
				}
				else
				{
					component3.playerDeathReason.text = "";
				}
				if (!NetworkClient.active)
				{
					component3.ping.gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x06000E8A RID: 3722 RVA: 0x0004A6C0 File Offset: 0x000488C0
	public void ShowGameOverPanel()
	{
		this.UpdateButtonVisibilityGameOver();
		this.DisplayMenu(true);
		this.gameOverPanel.SetActive(true);
		this.UpdatePlayerList();
		if (GeneralManager.singleton != null)
		{
			GeneralManager.singleton.UpdateCursorState();
			GeneralManager.singleton.UpdateInputSystemState();
			GeneralManager.singleton.UpdateChatVisibility(false);
		}
	}

	// Token: 0x06000E8B RID: 3723 RVA: 0x0004A718 File Offset: 0x00048918
	public void UpdateButtonVisibilityGameOver()
	{
		this.resumeButton.gameObject.SetActive(false);
		if (SingleplayerManager.singleton != null)
		{
			if (SingleplayerManager.singleton.singleplayerRun.roundWins < 2 && SingleplayerManager.singleton.singleplayerRun.roundLosses < 2)
			{
				this.gameOverTextPanel.SetActive(false);
			}
			this.leaveButton.gameObject.SetActive(true);
			this.forfeitRoundButton.gameObject.SetActive(false);
			if (SingleplayerManager.singleton.singleplayerRun.roundLosses > 1)
			{
				this.retryButton.gameObject.SetActive(true);
				this.continueButton.gameObject.SetActive(false);
				this.abandonRunButton.gameObject.SetActive(false);
				this.leaveButton.onClick.RemoveAllListeners();
				this.leaveButton.onClick.AddListener(delegate()
				{
					this.Continue();
				});
				this.retryButton.Select();
				return;
			}
			if (SingleplayerManager.singleton.PlayerWonTheCampaign)
			{
				this.abandonRunButton.gameObject.SetActive(false);
				this.leaveButton.gameObject.SetActive(false);
			}
			this.continueButton.gameObject.SetActive(true);
			this.continueButton.Select();
		}
	}

	// Token: 0x06000E8C RID: 3724 RVA: 0x0004A85F File Offset: 0x00048A5F
	private void OnDestroy()
	{
		Cursor.lockState = CursorLockMode.None;
		this.DisposeUserControls();
	}

	// Token: 0x06000E8D RID: 3725 RVA: 0x0004A86D File Offset: 0x00048A6D
	public void DisposeUserControls()
	{
		if (this.userControls != null)
		{
			this.userControls.Disable();
			this.userControls.Dispose();
		}
	}

	// Token: 0x06000E8E RID: 3726 RVA: 0x0004A88D File Offset: 0x00048A8D
	public void UpdateSaveReplayButtonState()
	{
		if (ReplayManager.singleton != null)
		{
			if (ReplayManager.singleton.recording != null)
			{
				this.saveReplayButton.interactable = true;
				return;
			}
			this.saveReplayButton.interactable = false;
		}
	}

	// Token: 0x06000E8F RID: 3727 RVA: 0x0004A8C1 File Offset: 0x00048AC1
	private void Continue()
	{
		SingleplayerManager.singleton.MoveToNextScene();
	}

	// Token: 0x06000E90 RID: 3728 RVA: 0x0004A8CD File Offset: 0x00048ACD
	private void Retry()
	{
		SingleplayerManager.singleton.Retry();
	}

	// Token: 0x06000E91 RID: 3729 RVA: 0x0003718A File Offset: 0x0003538A
	private void AbandonRun()
	{
		SingleplayerManager.singleton.AbandonRun();
	}

	// Token: 0x06000E92 RID: 3730 RVA: 0x0004A8D9 File Offset: 0x00048AD9
	private void ForfeitRound()
	{
		SingleplayerManager.singleton.ForfeitRound();
	}

	// Token: 0x04000A63 RID: 2659
	public GameObject menuHolderPanel;

	// Token: 0x04000A64 RID: 2660
	public GameObject gameOverPanel;

	// Token: 0x04000A65 RID: 2661
	public GameObject gameOverTextPanel;

	// Token: 0x04000A66 RID: 2662
	public Button resumeButton;

	// Token: 0x04000A67 RID: 2663
	public Button restartButton;

	// Token: 0x04000A68 RID: 2664
	public Button saveReplayButton;

	// Token: 0x04000A69 RID: 2665
	public Button movesetButton;

	// Token: 0x04000A6A RID: 2666
	public Button welcomeMessageButton;

	// Token: 0x04000A6B RID: 2667
	public Button optionsButton;

	// Token: 0x04000A6C RID: 2668
	public Button returnToLobbyButton;

	// Token: 0x04000A6D RID: 2669
	public Button leaveButton;

	// Token: 0x04000A6E RID: 2670
	public Button backToMoveSetEditButton;

	// Token: 0x04000A6F RID: 2671
	public Button continueButton;

	// Token: 0x04000A70 RID: 2672
	public Button forfeitRoundButton;

	// Token: 0x04000A71 RID: 2673
	public Button abandonRunButton;

	// Token: 0x04000A72 RID: 2674
	public Button retryButton;

	// Token: 0x04000A73 RID: 2675
	public Text winnerTextGameOver;

	// Token: 0x04000A74 RID: 2676
	public Text ping;

	// Token: 0x04000A75 RID: 2677
	public GameObject menuButtonHolder;

	// Token: 0x04000A76 RID: 2678
	public GameObject playerListPanel;

	// Token: 0x04000A77 RID: 2679
	public GameObject playerListItemPrefab;

	// Token: 0x04000A78 RID: 2680
	public GameObject optionsPanelPrefab;

	// Token: 0x04000A79 RID: 2681
	public GameObject movesetPanelPrefab;

	// Token: 0x04000A7A RID: 2682
	public GameObject gameMenuCanvas;

	// Token: 0x04000A7B RID: 2683
	public PlayerOptionsManager playerOptionsManager;

	// Token: 0x04000A7C RID: 2684
	public MultiplayerRoomPlayer localMultiplayerRoomPlayer;

	// Token: 0x04000A7D RID: 2685
	public PlayerCanvasController playerCanvasController;

	// Token: 0x04000A7E RID: 2686
	public static GameMenu singleton;

	// Token: 0x04000A7F RID: 2687
	public UserControls userControls;
}
