using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Mirror;
using Steamworks;
using Steamworks.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

// Token: 0x02000094 RID: 148
public class SteamManager : MonoBehaviour
{
	// Token: 0x0600051A RID: 1306 RVA: 0x0001818C File Offset: 0x0001638C
	private void Awake()
	{
		if (Application.isEditor && !this.forceInitSteam)
		{
			this.initSteam = false;
		}
		if (SteamManager.steamManager != null)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		SteamManager.steamManager = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (this.initSteam)
		{
			this.InitSteam();
		}
	}

	// Token: 0x0600051B RID: 1307 RVA: 0x000181E3 File Offset: 0x000163E3
	private void Update()
	{
		if (SteamClient.IsValid)
		{
			SteamClient.RunCallbacks();
		}
	}

	// Token: 0x0600051C RID: 1308 RVA: 0x000181F4 File Offset: 0x000163F4
	private bool InitSteam()
	{
		try
		{
			SteamClient.Init(this.appid, false);
			SteamNetworkingUtils.InitRelayNetworkAccess();
			SteamNetworkingUtils.SendBufferSize = 7361536;
		}
		catch (Exception message)
		{
			Debug.LogError(message);
			Debug.LogError("Could be one of the following: Steam is closed, Can't find steam_api dlls or Don't have permission to open appid");
			return false;
		}
		Debug.Log("Steam connection init");
		this.FetchSteamID();
		this.SubscribeToCallbacks();
		return true;
	}

	// Token: 0x0600051D RID: 1309 RVA: 0x0001825C File Offset: 0x0001645C
	private void SubscribeToCallbacks()
	{
		SteamMatchmaking.OnLobbyCreated += this.OnLobbyCreated;
		SteamMatchmaking.OnLobbyGameCreated += this.OnLobbyGameCreated;
		SteamFriends.OnGameLobbyJoinRequested += this.OnGameLobbyJoinRequested;
		SteamFriends.OnGameRichPresenceJoinRequested += this.OnGameRichPresenceJoinRequested;
		SteamMatchmaking.OnLobbyEntered += this.OnLobbyEntered;
		SteamFriends.OnGameOverlayActivated += this.OnGameOverlayActivated;
	}

	// Token: 0x0600051E RID: 1310 RVA: 0x000182D0 File Offset: 0x000164D0
	private void UnsubscribeToCallbacks()
	{
		SteamMatchmaking.OnLobbyCreated -= this.OnLobbyCreated;
		SteamMatchmaking.OnLobbyGameCreated -= this.OnLobbyGameCreated;
		SteamFriends.OnGameLobbyJoinRequested -= this.OnGameLobbyJoinRequested;
		SteamFriends.OnGameRichPresenceJoinRequested -= this.OnGameRichPresenceJoinRequested;
		SteamMatchmaking.OnLobbyEntered -= this.OnLobbyEntered;
		SteamFriends.OnGameOverlayActivated -= this.OnGameOverlayActivated;
	}

	// Token: 0x0600051F RID: 1311 RVA: 0x00018344 File Offset: 0x00016544
	private void OnGameOverlayActivated(bool active)
	{
		if (active)
		{
			Debug.Log("Steam GameOverlayActivated");
			foreach (InputDevice device in InputSystem.devices)
			{
				InputSystem.ResetDevice(device, false);
			}
		}
	}

	// Token: 0x06000520 RID: 1312 RVA: 0x000183A4 File Offset: 0x000165A4
	private void FetchSteamID()
	{
		if (SteamClient.IsValid)
		{
			Debug.Log("Fetch steam id and name");
			this.steamUserID = SteamClient.SteamId;
			this.steamUserName = SteamClient.Name;
			if (!SettingsHelper.GetCustomNameSetting())
			{
				PlayerPrefs.SetString("UserName", this.steamUserName);
			}
		}
	}

	// Token: 0x06000521 RID: 1313 RVA: 0x000183F4 File Offset: 0x000165F4
	public string GetLocale()
	{
		string result = "";
		this.CheckSteamIniti();
		if (SteamClient.IsValid)
		{
			result = this.SteamAPILanguageCodeToLanguageCode(SteamApps.GameLanguage);
		}
		return result;
	}

	// Token: 0x06000522 RID: 1314 RVA: 0x00018424 File Offset: 0x00016624
	public string SteamAPILanguageCodeToLanguageCode(string apiCode)
	{
		string result = "";
		if (SteamManager.steamLanguages.TryGetValue(apiCode, out result))
		{
			return result;
		}
		return "";
	}

	// Token: 0x06000523 RID: 1315 RVA: 0x0001844D File Offset: 0x0001664D
	private void CheckSteamIniti()
	{
		if (this.initSteam && !SteamClient.IsValid)
		{
			this.InitSteam();
		}
	}

	// Token: 0x06000524 RID: 1316 RVA: 0x00018468 File Offset: 0x00016668
	public string GetSteamUserIDAsString()
	{
		this.CheckSteamIniti();
		string result = null;
		if (SteamClient.IsValid)
		{
			result = this.steamUserID.ToString();
		}
		return result;
	}

	// Token: 0x06000525 RID: 1317 RVA: 0x00018494 File Offset: 0x00016694
	public Task<List<MultiplayerLobbyItem>> GetLobbyList(string searchString = "")
	{
		SteamManager.<GetLobbyList>d__18 <GetLobbyList>d__;
		<GetLobbyList>d__.<>t__builder = AsyncTaskMethodBuilder<List<MultiplayerLobbyItem>>.Create();
		<GetLobbyList>d__.searchString = searchString;
		<GetLobbyList>d__.<>1__state = -1;
		<GetLobbyList>d__.<>t__builder.Start<SteamManager.<GetLobbyList>d__18>(ref <GetLobbyList>d__);
		return <GetLobbyList>d__.<>t__builder.Task;
	}

	// Token: 0x06000526 RID: 1318 RVA: 0x000184D7 File Offset: 0x000166D7
	private void OnApplicationQuit()
	{
		Debug.Log("Steam Shutdown");
		this.LeaveLobby();
		this.UnsubscribeToCallbacks();
		SteamClient.Shutdown();
	}

	// Token: 0x06000527 RID: 1319 RVA: 0x000184F4 File Offset: 0x000166F4
	public void HostLobby()
	{
		if (SteamClient.IsValid)
		{
			SteamMatchmaking.CreateLobbyAsync(4);
		}
	}

	// Token: 0x06000528 RID: 1320 RVA: 0x00018504 File Offset: 0x00016704
	public void LeaveLobby()
	{
		if (this.currentLobby.Id != 0UL)
		{
			this.currentLobby.Leave();
			this.currentLobby = default(Lobby);
		}
	}

	// Token: 0x06000529 RID: 1321 RVA: 0x00018530 File Offset: 0x00016730
	private void OnLobbyCreated(Result result, Lobby lobby)
	{
		if (result == Result.OK)
		{
			Debug.Log("OnLobbyCreated");
			this.currentLobby = lobby;
			string value = this.steamUserName + "'s Game";
			this.currentLobby.SetData("lobbyName", value);
			this.SetLobbyData("gameVersion", Application.version ?? "");
			this.SetLobbyData("lobbyLocation", (SteamNetworkingUtils.LocalPingLocation != null) ? SteamNetworkingUtils.LocalPingLocation.ToString() : "");
			this.SetLobbyData("lobbyState", 0.ToString());
			this.SetLobbyData("gameType", 0.ToString());
			this.SetLobbyData("points", 30.ToString());
			this.SetLobbyData("stamina", true.ToString());
			this.SetLobbyData("dismemberment", true.ToString());
			GameSettingsManagerMultiplayer.SetLobbyTimeScale();
			if (NetworkManager.singleton != null)
			{
				this.SetLobbyPrivacyType(LobbyPrivacyType.friendsOnlyLobby);
				NetworkManager.singleton.StartHost();
				this.SetJoinable(true);
				return;
			}
		}
		else
		{
			this.FailedToCreateLobby();
		}
	}

	// Token: 0x0600052A RID: 1322 RVA: 0x00018664 File Offset: 0x00016864
	private void LogLobbyData(Lobby lobby)
	{
		if (lobby.Id != 0UL)
		{
			foreach (KeyValuePair<string, string> keyValuePair in lobby.Data)
			{
				Debug.Log(keyValuePair.Key + ":" + keyValuePair.Value);
			}
		}
	}

	// Token: 0x0600052B RID: 1323 RVA: 0x000186D8 File Offset: 0x000168D8
	public bool SetLobbyData(string key, bool value)
	{
		return this.currentLobby.Id != 0UL && SteamClient.IsValid && this.currentLobby.IsOwnedBy(SteamClient.SteamId) && this.currentLobby.SetData(key, value.ToString());
	}

	// Token: 0x0600052C RID: 1324 RVA: 0x00018728 File Offset: 0x00016928
	public bool SetLobbyData(string key, string value)
	{
		return this.currentLobby.Id != 0UL && SteamClient.IsValid && this.currentLobby.IsOwnedBy(SteamClient.SteamId) && this.currentLobby.SetData(key, value.ToString());
	}

	// Token: 0x0600052D RID: 1325 RVA: 0x00018774 File Offset: 0x00016974
	public string GetLobbyDataString(string key)
	{
		if (this.currentLobby.Id != 0UL && SteamClient.IsValid)
		{
			return this.currentLobby.GetData(key);
		}
		return "";
	}

	// Token: 0x0600052E RID: 1326 RVA: 0x000187A4 File Offset: 0x000169A4
	public LobbyPrivacyType GetLobbyPrivacyType()
	{
		if (this.currentLobby.Id != 0UL && SteamClient.IsValid && this.currentLobby.IsOwnedBy(SteamClient.SteamId))
		{
			return (LobbyPrivacyType)Convert.ToInt32(this.currentLobby.GetData("lobbyPrivacy"));
		}
		return LobbyPrivacyType.privateLobby;
	}

	// Token: 0x0600052F RID: 1327 RVA: 0x000187F4 File Offset: 0x000169F4
	public bool SetLobbyPrivacyType(LobbyPrivacyType lobbyPrivacyType)
	{
		if (this.currentLobby.Id != 0UL && SteamClient.IsValid && this.currentLobby.IsOwnedBy(SteamClient.SteamId))
		{
			bool flag;
			if (lobbyPrivacyType == LobbyPrivacyType.publicLobby)
			{
				flag = this.currentLobby.SetPublic();
			}
			else if (lobbyPrivacyType == LobbyPrivacyType.friendsOnlyLobby)
			{
				flag = this.currentLobby.SetFriendsOnly();
			}
			else
			{
				flag = this.currentLobby.SetPrivate();
			}
			if (flag)
			{
				string key = "lobbyPrivacy";
				int num = (int)lobbyPrivacyType;
				this.SetLobbyData(key, num.ToString());
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000530 RID: 1328 RVA: 0x0001887C File Offset: 0x00016A7C
	public bool SetJoinable(bool joinable)
	{
		if (this.currentLobby.Id != 0UL && SteamClient.IsValid && this.currentLobby.IsOwnedBy(SteamClient.SteamId) && this.currentLobby.SetJoinable(joinable))
		{
			this.SetLobbyData("lobbyJoinable", joinable);
			return true;
		}
		return false;
	}

	// Token: 0x06000531 RID: 1329 RVA: 0x000188D2 File Offset: 0x00016AD2
	private void OnLobbyGameCreated(Lobby lobby, uint ip, ushort port, SteamId steamId)
	{
		Debug.Log("OnLobbyGameCreated");
	}

	// Token: 0x06000532 RID: 1330 RVA: 0x000188DE File Offset: 0x00016ADE
	private void OnGameLobbyJoinRequested(Lobby lobby, SteamId steamId)
	{
		if (SteamClient.IsValid)
		{
			this.LeaveLobby();
			Debug.Log("OnGameLobbyJoinRequested");
			lobby.Join();
		}
	}

	// Token: 0x06000533 RID: 1331 RVA: 0x000188FF File Offset: 0x00016AFF
	private void OnGameRichPresenceJoinRequested(Friend friend, string stringValue)
	{
		if (SteamClient.IsValid)
		{
			Debug.Log("OnGameRichPresenceJoinRequested");
		}
	}

	// Token: 0x06000534 RID: 1332 RVA: 0x00018914 File Offset: 0x00016B14
	public void JoinLobby(object lobbyObject)
	{
		if (SteamClient.IsValid && lobbyObject != null && lobbyObject.GetType() == typeof(Lobby))
		{
			((Lobby)lobbyObject).Join();
			return;
		}
		this.FailedToConnectToLobby((Lobby)lobbyObject);
	}

	// Token: 0x06000535 RID: 1333 RVA: 0x00018960 File Offset: 0x00016B60
	private void OnLobbyEntered(Lobby lobby)
	{
		this.currentLobby = lobby;
		if (SteamClient.IsValid && lobby.Owner.Id != 0UL)
		{
			Debug.Log("OnLobbyEntered");
			if (NetworkManager.singleton != null)
			{
				if (NetworkManager.singleton.isNetworkActive)
				{
					return;
				}
				Debug.Log("Attempt connecting to lobby");
				NetworkManager.singleton.networkAddress = lobby.Owner.Id.Value.ToString();
				NetworkManager.singleton.StartClient();
				return;
			}
		}
		this.FailedToConnectToLobby(lobby);
	}

	// Token: 0x06000536 RID: 1334 RVA: 0x000189F1 File Offset: 0x00016BF1
	private void FailedToConnectToLobby(Lobby lobby)
	{
		this.LeaveLobby();
		MultiplayerMenuManager.EndConnectInfo();
		if (MultiplayerMenuManager.singleton != null)
		{
			GeneralManager.DisplayJoinErrorMessage(ConnectionEndedType.FailedToConnect);
			MultiplayerMenuManager.singleton.RemoveLobbyFromList(lobby.Id);
		}
	}

	// Token: 0x06000537 RID: 1335 RVA: 0x00018A27 File Offset: 0x00016C27
	private void FailedToCreateLobby()
	{
		MultiplayerMenuManager.EndConnectInfo();
		GeneralManager.DisplayJoinErrorMessage(ConnectionEndedType.FailedToCreate);
	}

	// Token: 0x06000538 RID: 1336 RVA: 0x00018A34 File Offset: 0x00016C34
	public void OpenAppStore()
	{
		if (SteamClient.IsValid)
		{
			SteamFriends.OpenStoreOverlay(2689120);
		}
	}

	// Token: 0x06000539 RID: 1337 RVA: 0x00018A4C File Offset: 0x00016C4C
	public void JoinLobbyByID(string lobbyIDString)
	{
		SteamManager.<JoinLobbyByID>d__38 <JoinLobbyByID>d__;
		<JoinLobbyByID>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<JoinLobbyByID>d__.lobbyIDString = lobbyIDString;
		<JoinLobbyByID>d__.<>1__state = -1;
		<JoinLobbyByID>d__.<>t__builder.Start<SteamManager.<JoinLobbyByID>d__38>(ref <JoinLobbyByID>d__);
	}

	// Token: 0x04000314 RID: 788
	public static SteamManager steamManager;

	// Token: 0x04000315 RID: 789
	public uint appid = 2689120U;

	// Token: 0x04000316 RID: 790
	[Header("Info")]
	[Tooltip("This will display your Steam User ID when you start or connect to a server.")]
	public ulong steamUserID;

	// Token: 0x04000317 RID: 791
	public string steamUserName;

	// Token: 0x04000318 RID: 792
	public bool initSteam;

	// Token: 0x04000319 RID: 793
	public bool forceInitSteam;

	// Token: 0x0400031A RID: 794
	public Lobby currentLobby;

	// Token: 0x0400031B RID: 795
	public static Dictionary<string, string> steamLanguages = new Dictionary<string, string>
	{
		{
			"english",
			"en"
		},
		{
			"schinese",
			"zh"
		},
		{
			"french",
			"fr"
		},
		{
			"german",
			"de"
		},
		{
			"italian",
			"it"
		},
		{
			"japanese",
			"ja"
		},
		{
			"koreana",
			"ko"
		},
		{
			"polish",
			"pl"
		},
		{
			"brazilian",
			"pt-BR"
		},
		{
			"russian",
			"ru"
		},
		{
			"spanish",
			"es"
		},
		{
			"finnish",
			"fi"
		}
	};
}
