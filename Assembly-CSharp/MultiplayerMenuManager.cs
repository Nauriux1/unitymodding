using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using kcp2k;
using Mirror;
using Mirror.FizzySteam;
using UnityEngine;
using UnityEngine.UI;
using Utils;

// Token: 0x02000218 RID: 536
public class MultiplayerMenuManager : MonoBehaviour
{
	// Token: 0x06001039 RID: 4153 RVA: 0x00054298 File Offset: 0x00052498
	private void Start()
	{
		MultiplayerMenuManager.singleton = this;
		this.multiplayerLobbyItems = new List<MultiplayerLobbyItem>();
		this.UpdateRoomManagerTransport();
		this.hostButton.onClick.AddListener(delegate()
		{
			this.StartHost();
		});
		this.joinButton.onClick.AddListener(delegate()
		{
			this.JoinGame();
		});
		if (this.backButton != null)
		{
			this.backButton.onClick.AddListener(delegate()
			{
				this.GoBack();
			});
		}
		if (this.enterCodeButton != null)
		{
			this.enterCodeButton.onClick.AddListener(delegate()
			{
				this.DisplayJoinLobbyWithCodeDialog();
			});
		}
		this.SetupMultiplayerPanels();
		if (this.searchButton != null)
		{
			this.searchButton.onClick.AddListener(delegate()
			{
				this.SearchLobbies();
			});
		}
		if (this.lobbyListTitlePanel != null)
		{
			this.lobbyListTitlePanel.multiplayerMenuManager = this;
			this.lobbyListTitlePanel.SetSortColumn("ping", true);
		}
		this.GetServerList();
	}

	// Token: 0x0600103A RID: 4154 RVA: 0x000543AC File Offset: 0x000525AC
	public void UpdateRoomManagerTransport()
	{
		this.roomManager = UnityEngine.Object.FindObjectOfType<MultiplayerRoomManager>();
		if (this.setKcpTransport)
		{
			KcpTransport kcpTransport = UnityEngine.Object.FindObjectOfType<KcpTransport>();
			if (kcpTransport != null)
			{
				this.roomManager.SetTransport(kcpTransport);
			}
			else
			{
				Debug.LogError("Could not find kcpTransport");
			}
		}
		else if (this.setLatencyTestTransport)
		{
			LatencySimulation latencySimulation = UnityEngine.Object.FindObjectOfType<LatencySimulation>();
			if (latencySimulation != null)
			{
				this.roomManager.SetTransport(latencySimulation);
			}
			else
			{
				Debug.LogError("Could not find latencyTransport");
			}
		}
		else
		{
			FizzyFacepunch fizzyFacepunch = UnityEngine.Object.FindObjectOfType<FizzyFacepunch>();
			if (fizzyFacepunch != null)
			{
				this.roomManager.SetTransport(fizzyFacepunch);
			}
			else
			{
				Debug.LogError("Could not find kcpTransport");
			}
		}
		this.currentTransport = this.roomManager.GetTransport();
	}

	// Token: 0x0600103B RID: 4155 RVA: 0x0000777A File Offset: 0x0000597A
	private void Update()
	{
	}

	// Token: 0x0600103C RID: 4156 RVA: 0x00054460 File Offset: 0x00052660
	private void StartHost()
	{
		if (this.connecting)
		{
			return;
		}
		MultiplayerMenuManager.DoConnectInfo(true);
		if (this.currentTransport.GetType() == typeof(KcpTransport))
		{
			KcpTransport kcpTransport = (KcpTransport)this.currentTransport;
			kcpTransport.Port = 7777;
			if (!string.IsNullOrEmpty(this.portInputField.text))
			{
				ushort port = 0;
				if (ushort.TryParse(this.portInputField.text, out port))
				{
					kcpTransport.Port = port;
				}
			}
			this.roomManager.StartHost();
			return;
		}
		if (this.currentTransport.GetType() == typeof(LatencySimulation))
		{
			KcpTransport kcpTransport2 = (KcpTransport)((LatencySimulation)this.currentTransport).wrap;
			kcpTransport2.Port = 7777;
			if (!string.IsNullOrEmpty(this.portInputField.text))
			{
				ushort port2 = 0;
				if (ushort.TryParse(this.portInputField.text, out port2))
				{
					kcpTransport2.Port = port2;
				}
			}
			this.roomManager.StartHost();
			return;
		}
		if (SteamManager.steamManager != null)
		{
			SteamManager.steamManager.HostLobby();
		}
	}

	// Token: 0x0600103D RID: 4157 RVA: 0x0005457C File Offset: 0x0005277C
	public void JoinGame()
	{
		if (this.connecting)
		{
			return;
		}
		MultiplayerMenuManager.DoConnectInfo(false);
		if (this.currentTransport.GetType() == typeof(KcpTransport))
		{
			KcpTransport kcpTransport = (KcpTransport)this.currentTransport;
			string str = "localhost";
			int num = 7777;
			if (!string.IsNullOrEmpty(this.ipInputField.text))
			{
				str = this.ipInputField.text;
			}
			if (!string.IsNullOrEmpty(this.portInputField.text))
			{
				int num2 = 0;
				if (int.TryParse(this.portInputField.text, out num2))
				{
					num = num2;
				}
			}
			Uri uri = new Uri("kcp://" + str + ":" + num.ToString(), UriKind.Absolute);
			this.roomManager.StartClient(uri);
			return;
		}
		if (this.currentTransport.GetType() == typeof(LatencySimulation))
		{
			KcpTransport kcpTransport2 = (KcpTransport)((LatencySimulation)this.currentTransport).wrap;
			string str2 = "localhost";
			int num3 = 7777;
			if (!string.IsNullOrEmpty(this.ipInputField.text))
			{
				str2 = this.ipInputField.text;
			}
			if (!string.IsNullOrEmpty(this.portInputField.text))
			{
				int num4 = 0;
				if (int.TryParse(this.portInputField.text, out num4))
				{
					num3 = num4;
				}
			}
			Uri uri2 = new Uri("kcp://" + str2 + ":" + num3.ToString(), UriKind.Absolute);
			this.roomManager.StartClient(uri2);
			return;
		}
		if (this.currentTransport.GetType() == typeof(FizzyFacepunch))
		{
			if (this.selectedLobby != null)
			{
				if (SteamManager.steamManager != null)
				{
					SteamManager.steamManager.JoinLobby(this.selectedLobby.lobbyItem.actualLobby);
					return;
				}
			}
			else
			{
				MultiplayerMenuManager.EndConnectInfo();
			}
		}
	}

	// Token: 0x0600103E RID: 4158 RVA: 0x00054756 File Offset: 0x00052956
	public void SearchLobbies()
	{
		this.ClearLobbyList();
		this.GetServerList();
	}

	// Token: 0x0600103F RID: 4159 RVA: 0x00054764 File Offset: 0x00052964
	public void GetServerList()
	{
		MultiplayerMenuManager.<GetServerList>d__34 <GetServerList>d__;
		<GetServerList>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<GetServerList>d__.<>4__this = this;
		<GetServerList>d__.<>1__state = -1;
		<GetServerList>d__.<>t__builder.Start<MultiplayerMenuManager.<GetServerList>d__34>(ref <GetServerList>d__);
	}

	// Token: 0x06001040 RID: 4160 RVA: 0x0005479B File Offset: 0x0005299B
	private void DisplayLoadingIcon(bool display)
	{
		if (this.loadingIcon != null)
		{
			this.loadingIcon.gameObject.SetActive(display);
		}
	}

	// Token: 0x06001041 RID: 4161 RVA: 0x000547BC File Offset: 0x000529BC
	public void ClearLobbyList()
	{
		if (this.lobbyListHolder == null)
		{
			return;
		}
		foreach (object obj in this.lobbyListHolder.transform)
		{
			UnityEngine.Object.Destroy(((Transform)obj).gameObject);
		}
	}

	// Token: 0x06001042 RID: 4162 RVA: 0x0005482C File Offset: 0x00052A2C
	private void SortLobbyList()
	{
		if (this.multiplayerLobbyItems != null && this.multiplayerLobbyItems.Count > 0)
		{
			string sortColumnName;
			uint num;
			if (this.lobbyListTitlePanel.sortType == TableColumnSortType.Ascending)
			{
				sortColumnName = this.lobbyListTitlePanel.GetSortColumnName();
				num = <PrivateImplementationDetails>.ComputeStringHash(sortColumnName);
				if (num <= 2670667422U)
				{
					if (num <= 1772797293U)
					{
						if (num != 1577794527U)
						{
							if (num == 1772797293U)
							{
								if (sortColumnName == "dismemberment")
								{
									this.multiplayerLobbyItems = (from x in this.multiplayerLobbyItems
									orderby x.dismemberment, x.ping
									select x).ToList<MultiplayerLobbyItem>();
									return;
								}
							}
						}
						else if (sortColumnName == "gametype")
						{
							this.multiplayerLobbyItems = (from x in this.multiplayerLobbyItems
							orderby x.gameType, x.ping
							select x).ToList<MultiplayerLobbyItem>();
							return;
						}
					}
					else if (num != 2369371622U)
					{
						if (num == 2670667422U)
						{
							if (sortColumnName == "timescale")
							{
								this.multiplayerLobbyItems = (from x in this.multiplayerLobbyItems
								orderby x.lobbyTimeScaleString, x.ping
								select x).ToList<MultiplayerLobbyItem>();
								return;
							}
						}
					}
					else if (sortColumnName == "name")
					{
						this.multiplayerLobbyItems = (from x in this.multiplayerLobbyItems
						orderby x.name, x.ping
						select x).ToList<MultiplayerLobbyItem>();
						return;
					}
				}
				else if (num <= 3125508079U)
				{
					if (num != 2836311561U)
					{
						if (num == 3125508079U)
						{
							if (sortColumnName == "status")
							{
								this.multiplayerLobbyItems = (from x in this.multiplayerLobbyItems
								orderby x.lobbyStatus, x.ping
								select x).ToList<MultiplayerLobbyItem>();
								return;
							}
						}
					}
					else if (sortColumnName == "players")
					{
						this.multiplayerLobbyItems = (from x in this.multiplayerLobbyItems
						orderby x.currentPlayers, x.ping
						select x).ToList<MultiplayerLobbyItem>();
						return;
					}
				}
				else if (num != 3163908038U)
				{
					if (num == 3875003702U)
					{
						if (sortColumnName == "stamina")
						{
							this.multiplayerLobbyItems = (from x in this.multiplayerLobbyItems
							orderby x.stamina, x.ping
							select x).ToList<MultiplayerLobbyItem>();
							return;
						}
					}
				}
				else if (sortColumnName == "points")
				{
					this.multiplayerLobbyItems = (from x in this.multiplayerLobbyItems
					orderby x.points, x.ping
					select x).ToList<MultiplayerLobbyItem>();
					return;
				}
				this.multiplayerLobbyItems = (from x in this.multiplayerLobbyItems
				orderby x.ping
				select x).ToList<MultiplayerLobbyItem>();
				return;
			}
			sortColumnName = this.lobbyListTitlePanel.GetSortColumnName();
			num = <PrivateImplementationDetails>.ComputeStringHash(sortColumnName);
			if (num <= 2670667422U)
			{
				if (num <= 1772797293U)
				{
					if (num != 1577794527U)
					{
						if (num == 1772797293U)
						{
							if (sortColumnName == "dismemberment")
							{
								this.multiplayerLobbyItems = (from x in this.multiplayerLobbyItems
								orderby x.dismemberment descending, x.ping
								select x).ToList<MultiplayerLobbyItem>();
								return;
							}
						}
					}
					else if (sortColumnName == "gametype")
					{
						this.multiplayerLobbyItems = (from x in this.multiplayerLobbyItems
						orderby x.gameType descending, x.ping
						select x).ToList<MultiplayerLobbyItem>();
						return;
					}
				}
				else if (num != 2369371622U)
				{
					if (num == 2670667422U)
					{
						if (sortColumnName == "timescale")
						{
							this.multiplayerLobbyItems = (from x in this.multiplayerLobbyItems
							orderby x.lobbyTimeScaleString descending, x.ping
							select x).ToList<MultiplayerLobbyItem>();
							return;
						}
					}
				}
				else if (sortColumnName == "name")
				{
					this.multiplayerLobbyItems = (from x in this.multiplayerLobbyItems
					orderby x.name descending, x.ping
					select x).ToList<MultiplayerLobbyItem>();
					return;
				}
			}
			else if (num <= 3125508079U)
			{
				if (num != 2836311561U)
				{
					if (num == 3125508079U)
					{
						if (sortColumnName == "status")
						{
							this.multiplayerLobbyItems = (from x in this.multiplayerLobbyItems
							orderby x.lobbyStatus descending, x.ping
							select x).ToList<MultiplayerLobbyItem>();
							return;
						}
					}
				}
				else if (sortColumnName == "players")
				{
					this.multiplayerLobbyItems = (from x in this.multiplayerLobbyItems
					orderby x.currentPlayers descending, x.ping
					select x).ToList<MultiplayerLobbyItem>();
					return;
				}
			}
			else if (num != 3163908038U)
			{
				if (num == 3875003702U)
				{
					if (sortColumnName == "stamina")
					{
						this.multiplayerLobbyItems = (from x in this.multiplayerLobbyItems
						orderby x.stamina descending, x.ping
						select x).ToList<MultiplayerLobbyItem>();
						return;
					}
				}
			}
			else if (sortColumnName == "points")
			{
				this.multiplayerLobbyItems = (from x in this.multiplayerLobbyItems
				orderby x.points descending, x.ping
				select x).ToList<MultiplayerLobbyItem>();
				return;
			}
			this.multiplayerLobbyItems = (from x in this.multiplayerLobbyItems
			orderby x.ping descending
			select x).ToList<MultiplayerLobbyItem>();
		}
	}

	// Token: 0x06001043 RID: 4163 RVA: 0x000550D8 File Offset: 0x000532D8
	public void RenderLobbyList()
	{
		if (this.lobbyListHolder == null)
		{
			return;
		}
		this.ClearLobbyList();
		this.SortLobbyList();
		if (this.multiplayerLobbyItems != null && this.multiplayerLobbyItems.Count > 0)
		{
			foreach (MultiplayerLobbyItem multiplayerLobbyItem in this.multiplayerLobbyItems)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.lobbyListItemPrefab, this.lobbyListHolder.transform);
				LobbyListItemPanel component = gameObject.GetComponent<LobbyListItemPanel>();
				if (component != null)
				{
					multiplayerLobbyItem.gameObjectOnList = gameObject;
					component.SetLobbyItem(multiplayerLobbyItem);
				}
			}
		}
		this.UpdateNavigation();
	}

	// Token: 0x06001044 RID: 4164 RVA: 0x00055190 File Offset: 0x00053390
	public void RemoveLobbyFromList(ulong lobbyID)
	{
		if (this.multiplayerLobbyItems != null && this.multiplayerLobbyItems.Count > 0)
		{
			MultiplayerLobbyItem multiplayerLobbyItem = (from x in this.multiplayerLobbyItems
			where x.lobbyID == lobbyID
			select x).FirstOrDefault<MultiplayerLobbyItem>();
			if (multiplayerLobbyItem != null)
			{
				if (multiplayerLobbyItem.gameObjectOnList != null)
				{
					UnityEngine.Object.Destroy(multiplayerLobbyItem.gameObjectOnList);
				}
				this.multiplayerLobbyItems.Remove(multiplayerLobbyItem);
			}
		}
		this.SetSelectedLobbyItem(null);
	}

	// Token: 0x06001045 RID: 4165 RVA: 0x00055210 File Offset: 0x00053410
	public void UpdateNavigation()
	{
		Selectable target = this.searchButton;
		Selectable target2 = this.hostButton;
		if (this.selectedLobby != null)
		{
			target = this.selectedLobby;
			target2 = this.selectedLobby;
		}
		else if (this.multiplayerLobbyItems.Count > 0)
		{
			LobbyListItemPanel component = this.multiplayerLobbyItems.First<MultiplayerLobbyItem>().gameObjectOnList.GetComponent<LobbyListItemPanel>();
			if (component != null)
			{
				target = component;
				target2 = component;
			}
		}
		UIHelpers.SetUpNavitagionForSelectable(this.backButtonWithoutLogic, target);
		UIHelpers.SetUpNavitagionForSelectable(this.hostButton, target);
		UIHelpers.SetUpNavitagionForSelectable(this.joinButton, target);
		UIHelpers.SetUpNavitagionForSelectable(this.enterCodeButton, target);
		UIHelpers.SetUpNavitagionForSelectable(this.directConnectButton, target);
		UIHelpers.SetDownNavitagionForSelectable(this.searchInputField, target2);
		UIHelpers.SetDownNavitagionForSelectable(this.searchButton, target2);
	}

	// Token: 0x06001046 RID: 4166 RVA: 0x000552D0 File Offset: 0x000534D0
	private void DisplayJoinLobbyWithCodeDialog()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.textConfirmDialogPrefab);
		BasicTextConfirmDialog dialog = gameObject.GetComponent<BasicTextConfirmDialog>();
		dialog.textInputField.contentType = InputField.ContentType.IntegerNumber;
		dialog.SetText("", LocalizationHelpers.LocalizedText("txt_enter_code", new object[0]), false);
		dialog.okButton.onClick.RemoveAllListeners();
		dialog.textInputField.text = "";
		string systemCopyBuffer = GUIUtility.systemCopyBuffer;
		if (!string.IsNullOrWhiteSpace(systemCopyBuffer) && systemCopyBuffer.Length <= 20 && systemCopyBuffer.All(new Func<char, bool>(char.IsDigit)))
		{
			dialog.textInputField.text = systemCopyBuffer;
		}
		dialog.okButton.onClick.AddListener(delegate()
		{
			this.JoinLobbyWithCode(dialog);
		});
		dialog.textInputField.Select();
	}

	// Token: 0x06001047 RID: 4167 RVA: 0x000553CC File Offset: 0x000535CC
	private void JoinLobbyWithCode(BasicTextConfirmDialog dialog)
	{
		string text = dialog.textInputField.text;
		if (!string.IsNullOrWhiteSpace(text))
		{
			MultiplayerMenuManager.DoConnectInfo(false);
			SteamManager.steamManager.JoinLobbyByID(text);
		}
		dialog.onClick();
	}

	// Token: 0x06001048 RID: 4168 RVA: 0x00055404 File Offset: 0x00053604
	private void GoBack()
	{
		if (this.multiplayerSettingsPanel != null && this.multiplayerSettingsPanel.activeInHierarchy)
		{
			this.HideMultiplayerPanel();
			return;
		}
		SceneManagerWithParameters.LoadScene("MainMenu", null, false, false);
	}

	// Token: 0x06001049 RID: 4169 RVA: 0x00055438 File Offset: 0x00053638
	public void SetSelectedLobbyItem(LobbyListItemPanel lobbyPanel)
	{
		if (this.selectedLobby != null)
		{
			this.selectedLobby.SetSelected(false);
		}
		this.selectedLobby = lobbyPanel;
		if (this.selectedLobby != null)
		{
			this.selectedLobby.SetSelected(true);
		}
		this.UpdateNavigation();
	}

	// Token: 0x0600104A RID: 4170 RVA: 0x00055488 File Offset: 0x00053688
	private void SetupMultiplayerPanels()
	{
		if (this.multiplayerButtonsPanel != null && this.multiplayerSettingsPanel != null)
		{
			this.hostPanelButton.onClick.AddListener(delegate()
			{
				this.DisplayMultiplayerPanel(true);
			});
			this.joinPanelButton.onClick.AddListener(delegate()
			{
				this.DisplayMultiplayerPanel(false);
			});
		}
	}

	// Token: 0x0600104B RID: 4171 RVA: 0x000554EC File Offset: 0x000536EC
	private void DisplayMultiplayerPanel(bool isHost)
	{
		this.multiplayerButtonsPanel.SetActive(false);
		this.multiplayerSettingsPanel.SetActive(true);
		if (isHost)
		{
			this.ipInputField.gameObject.SetActive(false);
			this.hostButton.gameObject.SetActive(true);
			this.joinButton.gameObject.SetActive(false);
			return;
		}
		this.ipInputField.gameObject.SetActive(true);
		this.hostButton.gameObject.SetActive(false);
		this.joinButton.gameObject.SetActive(true);
	}

	// Token: 0x0600104C RID: 4172 RVA: 0x0005557B File Offset: 0x0005377B
	private void HideMultiplayerPanel()
	{
		this.multiplayerButtonsPanel.SetActive(true);
		this.multiplayerSettingsPanel.SetActive(false);
	}

	// Token: 0x0600104D RID: 4173 RVA: 0x00055598 File Offset: 0x00053798
	private void DoConnectInfoPrivate(bool host = false)
	{
		this.connecting = true;
		this.HideConnectInfoDialog();
		string text;
		if (host)
		{
			text = LocalizationHelpers.LocalizedText("alert_server_creating_game", Array.Empty<object>());
		}
		else
		{
			text = LocalizationHelpers.LocalizedText("alert_server_joining_game", Array.Empty<object>());
		}
		this.connectInfoDialog = GeneralManager.CreateAlertDialog(text, -1f, false);
	}

	// Token: 0x0600104E RID: 4174 RVA: 0x000555EF File Offset: 0x000537EF
	private void EndConnectInfoPrivate()
	{
		this.connecting = false;
		this.HideConnectInfoDialog();
	}

	// Token: 0x0600104F RID: 4175 RVA: 0x000555FE File Offset: 0x000537FE
	private void HideConnectInfoDialog()
	{
		if (this.connectInfoDialog != null)
		{
			this.connectInfoDialog.DestroyPanel();
			this.connectInfoDialog = null;
		}
	}

	// Token: 0x06001050 RID: 4176 RVA: 0x00055620 File Offset: 0x00053820
	private List<MultiplayerLobbyItem> GetTestLobbyList()
	{
		List<MultiplayerLobbyItem> list = new List<MultiplayerLobbyItem>();
		for (int i = 0; i < 100; i++)
		{
			MultiplayerLobbyItem item = new MultiplayerLobbyItem
			{
				name = string.Format("Game {0}", UnityEngine.Random.Range(0, 2000)),
				ping = UnityEngine.Random.Range(1, 400),
				currentPlayers = UnityEngine.Random.Range(0, 4),
				lobbyTimeScaleString = UnityEngine.Random.Range(0f, 1f).ToString("0.00") + "x",
				lobbyStatus = (LobbyStatus)UnityEngine.Random.Range(0, 2),
				gameType = (GameTypes)UnityEngine.Random.Range(0, 2),
				points = UnityEngine.Random.Range(0, 100),
				dismemberment = (UnityEngine.Random.Range(0, 2) == 0),
				stamina = (UnityEngine.Random.Range(0, 2) == 0)
			};
			list.Add(item);
		}
		return list;
	}

	// Token: 0x06001051 RID: 4177 RVA: 0x00055706 File Offset: 0x00053906
	public static void DoConnectInfo(bool host = false)
	{
		if (MultiplayerMenuManager.singleton != null)
		{
			MultiplayerMenuManager.singleton.DoConnectInfoPrivate(host);
		}
	}

	// Token: 0x06001052 RID: 4178 RVA: 0x00055720 File Offset: 0x00053920
	public static void EndConnectInfo()
	{
		if (MultiplayerMenuManager.singleton != null)
		{
			MultiplayerMenuManager.singleton.EndConnectInfoPrivate();
		}
	}

	// Token: 0x04000BA1 RID: 2977
	public Button hostButton;

	// Token: 0x04000BA2 RID: 2978
	public Button joinButton;

	// Token: 0x04000BA3 RID: 2979
	public Button backButton;

	// Token: 0x04000BA4 RID: 2980
	public Button backButtonWithoutLogic;

	// Token: 0x04000BA5 RID: 2981
	public Button enterCodeButton;

	// Token: 0x04000BA6 RID: 2982
	public Button directConnectButton;

	// Token: 0x04000BA7 RID: 2983
	public InputField ipInputField;

	// Token: 0x04000BA8 RID: 2984
	public InputField portInputField;

	// Token: 0x04000BA9 RID: 2985
	private MultiplayerRoomManager roomManager;

	// Token: 0x04000BAA RID: 2986
	private Transport currentTransport;

	// Token: 0x04000BAB RID: 2987
	public bool setKcpTransport;

	// Token: 0x04000BAC RID: 2988
	public bool setLatencyTestTransport;

	// Token: 0x04000BAD RID: 2989
	[Header("lobbyListStuff")]
	public List<MultiplayerLobbyItem> multiplayerLobbyItems;

	// Token: 0x04000BAE RID: 2990
	public GameObject lobbyListHolder;

	// Token: 0x04000BAF RID: 2991
	public GameObject lobbyListItemPrefab;

	// Token: 0x04000BB0 RID: 2992
	public InputField searchInputField;

	// Token: 0x04000BB1 RID: 2993
	public Button searchButton;

	// Token: 0x04000BB2 RID: 2994
	public LobbyListTitlePanel lobbyListTitlePanel;

	// Token: 0x04000BB3 RID: 2995
	public ScrollRect lobbyListScrollRect;

	// Token: 0x04000BB4 RID: 2996
	public static MultiplayerMenuManager singleton;

	// Token: 0x04000BB5 RID: 2997
	public LobbyListItemPanel selectedLobby;

	// Token: 0x04000BB6 RID: 2998
	public RectTransform loadingIcon;

	// Token: 0x04000BB7 RID: 2999
	[Header("Multiplayer Panel Buttons")]
	public Button hostPanelButton;

	// Token: 0x04000BB8 RID: 3000
	public Button joinPanelButton;

	// Token: 0x04000BB9 RID: 3001
	public GameObject multiplayerButtonsPanel;

	// Token: 0x04000BBA RID: 3002
	public GameObject multiplayerSettingsPanel;

	// Token: 0x04000BBB RID: 3003
	public GameObject textConfirmDialogPrefab;

	// Token: 0x04000BBC RID: 3004
	private bool searchingServers;

	// Token: 0x04000BBD RID: 3005
	private BasicInfoDialog connectInfoDialog;

	// Token: 0x04000BBE RID: 3006
	private bool connecting;
}
