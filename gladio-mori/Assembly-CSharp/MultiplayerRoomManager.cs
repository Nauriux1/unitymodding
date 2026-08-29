using System;
using System.Collections;
using System.Collections.Generic;
using kcp2k;
using Mirror;
using MoveClasses;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

// Token: 0x0200008A RID: 138
public class MultiplayerRoomManager : NetworkRoomManager
{
	// Token: 0x06000461 RID: 1121 RVA: 0x00015217 File Offset: 0x00013417
	public static MultiplayerRoomManager GetMultiplayerRoomManager()
	{
		if (NetworkManager.singleton != null)
		{
			return (MultiplayerRoomManager)NetworkManager.singleton;
		}
		return null;
	}

	// Token: 0x170000E9 RID: 233
	// (get) Token: 0x06000462 RID: 1122 RVA: 0x00015232 File Offset: 0x00013432
	// (set) Token: 0x06000463 RID: 1123 RVA: 0x0001523A File Offset: 0x0001343A
	public GameMaster gameMaster { get; set; }

	// Token: 0x06000464 RID: 1124 RVA: 0x00015243 File Offset: 0x00013443
	public override void Awake()
	{
		base.Awake();
		SettingsHelper.CheckNetworkDebugging();
	}

	// Token: 0x06000465 RID: 1125 RVA: 0x00015250 File Offset: 0x00013450
	public override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06000466 RID: 1126 RVA: 0x00015258 File Offset: 0x00013458
	public override void OnServerConnect(NetworkConnectionToClient conn)
	{
		if (BanHelpers.IsBanned(conn, this.GetGladioMoriServerType()))
		{
			conn.Disconnect();
			return;
		}
		Debug.Log("User connecting(" + conn.address + ")");
		this.OnRoomServerConnect(conn);
	}

	// Token: 0x06000467 RID: 1127 RVA: 0x00015290 File Offset: 0x00013490
	public override void OnRoomStartServer()
	{
		if (IGameSettingsManager.singleton == null)
		{
			NetworkServer.Spawn(UnityEngine.Object.Instantiate<GameObject>(this.multiplayerGameSettingsManagerPrefab), null);
			if (GameSettingsManagerMultiplayer.singleton != null)
			{
				GameSettingsManagerMultiplayer.singleton.NetworkgameVersion = Application.version;
			}
		}
		string gameplayScene = "map_ArenaOfBlades";
		this.GameplayScene = gameplayScene;
	}

	// Token: 0x06000468 RID: 1128 RVA: 0x000152DE File Offset: 0x000134DE
	public override void OnRoomStopServer()
	{
		if (StaminaManager.singleton != null)
		{
			StaminaManager.singleton.CleanUp();
		}
	}

	// Token: 0x06000469 RID: 1129 RVA: 0x0000777A File Offset: 0x0000597A
	public override void OnRoomStartHost()
	{
	}

	// Token: 0x0600046A RID: 1130 RVA: 0x0000777A File Offset: 0x0000597A
	public override void OnRoomStopHost()
	{
	}

	// Token: 0x0600046B RID: 1131 RVA: 0x0000777A File Offset: 0x0000597A
	public override void OnRoomServerConnect(NetworkConnectionToClient conn)
	{
	}

	// Token: 0x0600046C RID: 1132 RVA: 0x000152F7 File Offset: 0x000134F7
	public override void OnRoomServerDisconnect(NetworkConnectionToClient conn)
	{
		this.RemoveLoadedConnection(conn);
		this.CheckIfSceneHasBeenLoadedByAll();
		this.CleanUpSentTexturesToConnection(conn);
	}

	// Token: 0x0600046D RID: 1133 RVA: 0x00015310 File Offset: 0x00013510
	public void CleanUpSentTexturesToConnection(NetworkConnectionToClient conn)
	{
		foreach (NetworkRoomPlayer networkRoomPlayer in this.roomSlots)
		{
			MultiplayerRoomPlayer multiplayerRoomPlayer = (MultiplayerRoomPlayer)networkRoomPlayer;
			if (multiplayerRoomPlayer != null)
			{
				multiplayerRoomPlayer.RemoveFromCustomTextureAlreadySent(conn);
			}
		}
	}

	// Token: 0x0600046E RID: 1134 RVA: 0x00015374 File Offset: 0x00013574
	public override void OnRoomServerSceneChanged(string sceneName)
	{
		if (sceneName != this.RoomScene)
		{
			GameObject gameObject = new GameObject("GameMaster");
			this.gameMaster = gameObject.AddComponent<GameMaster>();
			GameObject gameObject2 = GameObject.Find("Managers");
			if (gameObject2 != null)
			{
				this.gameMaster.transform.SetParent(gameObject2.transform);
			}
			GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(this.multiplayerGameMasterPrefab);
			NetworkServer.Spawn(gameObject3, null);
			MultiplayerGameMaster component = gameObject3.GetComponent<MultiplayerGameMaster>();
			if (component != null)
			{
				this.gameMaster.multiplayerGameMaster = component;
			}
			NetworkServer.Spawn(UnityEngine.Object.Instantiate<GameObject>(this.multiplayerSoundManagerPrefab), null);
			return;
		}
		GameSettingsManagerMultiplayer.SetGameStatus(LobbyStatus.inLobby);
	}

	// Token: 0x0600046F RID: 1135 RVA: 0x00015418 File Offset: 0x00013618
	public override GameObject OnRoomServerCreateRoomPlayer(NetworkConnectionToClient conn)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.roomPlayerPrefab.gameObject, Vector3.zero, Quaternion.identity);
		MultiplayerRoomPlayer component = gameObject.GetComponent<MultiplayerRoomPlayer>();
		if (!Utils.IsSceneActive(this.RoomScene))
		{
			component.SetJoinedMidGame(true);
			component.SetDefaultMoveSet();
		}
		return gameObject;
	}

	// Token: 0x06000470 RID: 1136 RVA: 0x00015460 File Offset: 0x00013660
	public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
	{
		Transform startPosition = this.GetStartPosition();
		GameObject gameObject = (startPosition != null) ? UnityEngine.Object.Instantiate<GameObject>(this.playerPrefab, startPosition.position, startPosition.rotation) : UnityEngine.Object.Instantiate<GameObject>(this.playerPrefab, Vector3.zero, Quaternion.identity);
		PlayerMultiplayerInputManager component = gameObject.GetComponent<PlayerMultiplayerInputManager>();
		MultiplayerRoomPlayer component2 = roomPlayer.GetComponent<MultiplayerRoomPlayer>();
		component.moveSet = component2.selectedMoveSet;
		component.NetworkequippedEquipment = component2.selectedEquipment;
		component.NetworkplayerName = component2.playerName;
		component.multiplayerRoomPlayer = component2;
		component2.RegisterPlayerMultiplayerInputManager(component);
		component.NetworkmultiplayerRoomPlayerIdentity = component2.netId;
		component.AttemptToCreatePlayerCharacter();
		if (component2.GetJoinedMidGame())
		{
			this.SendCutsToMidGameJoiner(conn);
		}
		return gameObject;
	}

	// Token: 0x06000471 RID: 1137 RVA: 0x0001550C File Offset: 0x0001370C
	public override void OnRoomServerAddPlayer(NetworkConnectionToClient conn)
	{
		base.OnRoomServerAddPlayer(conn);
	}

	// Token: 0x06000472 RID: 1138 RVA: 0x00015518 File Offset: 0x00013718
	public override void OnServerAddPlayer(NetworkConnectionToClient conn)
	{
		this.clientIndex++;
		base.allPlayersReady = false;
		GameObject gameObject = this.OnRoomServerCreateRoomPlayer(conn);
		if (gameObject == null)
		{
			gameObject = UnityEngine.Object.Instantiate<GameObject>(this.roomPlayerPrefab.gameObject, Vector3.zero, Quaternion.identity);
		}
		NetworkServer.AddPlayerForConnection(conn, gameObject);
		if (!Utils.IsSceneActive(this.RoomScene))
		{
			base.SceneLoadedForPlayer(conn, gameObject);
		}
	}

	// Token: 0x06000473 RID: 1139 RVA: 0x00015583 File Offset: 0x00013783
	public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
	{
		this.loadedPlayers.Add(conn);
		base.OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer);
		this.CheckIfSceneHasBeenLoadedByAll();
		return true;
	}

	// Token: 0x06000474 RID: 1140 RVA: 0x000155A2 File Offset: 0x000137A2
	public void OnSceneLoaded(MultiplayerRoomManager.SceneLoaded sceneLoaded)
	{
		if (GeneralManager.singleton != null)
		{
			GeneralManager.singleton.RemoveLoadingScreen();
		}
	}

	// Token: 0x06000475 RID: 1141 RVA: 0x000155BB File Offset: 0x000137BB
	public override void OnRoomServerPlayersReady()
	{
		base.OnRoomServerPlayersReady();
	}

	// Token: 0x06000476 RID: 1142 RVA: 0x0000777A File Offset: 0x0000597A
	public override void OnRoomServerPlayersNotReady()
	{
	}

	// Token: 0x06000477 RID: 1143 RVA: 0x0000777A File Offset: 0x0000597A
	public override void OnRoomClientEnter()
	{
	}

	// Token: 0x06000478 RID: 1144 RVA: 0x0000777A File Offset: 0x0000597A
	public override void OnRoomClientExit()
	{
	}

	// Token: 0x06000479 RID: 1145 RVA: 0x000155C3 File Offset: 0x000137C3
	public override void OnRoomClientConnect()
	{
		NetworkClient.RegisterHandler<MultiplayerRoomManager.SceneLoaded>(new Action<MultiplayerRoomManager.SceneLoaded>(this.OnSceneLoaded), true);
	}

	// Token: 0x0600047A RID: 1146 RVA: 0x000155D7 File Offset: 0x000137D7
	public override void OnRoomClientDisconnect()
	{
		if (GeneralManager.singleton != null)
		{
			GeneralManager.singleton.leftMultiplayerSession = true;
			if (NetworkClient.isConnecting)
			{
				GeneralManager.singleton.connectionEndedType = ConnectionEndedType.FailedToConnect;
			}
		}
		NetworkClient.UnregisterHandler<MultiplayerRoomManager.SceneLoaded>();
	}

	// Token: 0x0600047B RID: 1147 RVA: 0x00015609 File Offset: 0x00013809
	public override void OnRoomStartClient()
	{
		if (!Utils.IsSceneActive(this.RoomScene))
		{
			this.joinedMidGame = true;
		}
	}

	// Token: 0x0600047C RID: 1148 RVA: 0x00015620 File Offset: 0x00013820
	public override void OnRoomStopClient()
	{
		if (!string.IsNullOrWhiteSpace(this.offlineScene) && SceneManager.GetActiveScene().path == this.offlineScene)
		{
			MultiplayerMenuManager.EndConnectInfo();
			GeneralManager.CleanUp();
		}
	}

	// Token: 0x0600047D RID: 1149 RVA: 0x00015660 File Offset: 0x00013860
	public override void OnRoomClientSceneChanged()
	{
		if (SceneManager.GetActiveScene().name.Contains("map_"))
		{
			UnityEngine.Object.Instantiate(Resources.Load("UI/GameMenu", typeof(GameObject)));
		}
		else
		{
			if (MultiplayerRoomPlayer.localMultiplayerRoomPlayer != null)
			{
				MultiplayerRoomPlayer.localMultiplayerRoomPlayer.SetupPlayerMoveSetCanvas();
			}
			foreach (NetworkRoomPlayer networkRoomPlayer in this.roomSlots)
			{
				MultiplayerRoomPlayer multiplayerRoomPlayer = (MultiplayerRoomPlayer)networkRoomPlayer;
				if (multiplayerRoomPlayer != null)
				{
					multiplayerRoomPlayer.SetupPreviewCharacter();
				}
			}
		}
		this.joinedMidGame = false;
	}

	// Token: 0x0600047E RID: 1150 RVA: 0x00015714 File Offset: 0x00013914
	public override void ServerChangeScene(string newSceneName)
	{
		if (!this.CanChangeScene())
		{
			return;
		}
		this.ClearLoadedPlayers(newSceneName);
		if (newSceneName != this.RoomScene)
		{
			GameSettingsManagerMultiplayer.SetGameStatus(LobbyStatus.inGame);
			this.ResetRoomPlayersForSceneChange();
		}
		base.ServerChangeScene(newSceneName);
		if (GeneralManager.singleton != null)
		{
			GeneralManager.singleton.ShowLoadingBarForOperation(NetworkManager.loadingSceneAsync, newSceneName, true, false, false);
		}
	}

	// Token: 0x0600047F RID: 1151 RVA: 0x00015774 File Offset: 0x00013974
	private void ResetRoomPlayersForSceneChange()
	{
		foreach (NetworkRoomPlayer networkRoomPlayer in this.roomSlots)
		{
			MultiplayerRoomPlayer multiplayerRoomPlayer = (MultiplayerRoomPlayer)networkRoomPlayer;
			if (!(multiplayerRoomPlayer == null))
			{
				NetworkIdentity component = multiplayerRoomPlayer.GetComponent<NetworkIdentity>();
				if (NetworkServer.active)
				{
					multiplayerRoomPlayer.SetJoinedMidGame(false);
					multiplayerRoomPlayer.SetReadyToBegin(false);
					multiplayerRoomPlayer.ClientRpcSetRoomReadyState(false);
					multiplayerRoomPlayer.NetworkdeathTime = null;
					multiplayerRoomPlayer.NetworkplayerDeathReason = DeathReason.Unknown;
					NetworkServer.ReplacePlayerForConnection(component.connectionToClient, multiplayerRoomPlayer.gameObject, true);
				}
			}
		}
	}

	// Token: 0x06000480 RID: 1152 RVA: 0x00015820 File Offset: 0x00013A20
	private bool CanChangeScene()
	{
		foreach (NetworkRoomPlayer networkRoomPlayer in this.roomSlots)
		{
			if (networkRoomPlayer != null)
			{
				MultiplayerRoomPlayer multiplayerRoomPlayer = (MultiplayerRoomPlayer)networkRoomPlayer;
				if (multiplayerRoomPlayer.moveSetIsBeingSynced)
				{
					string[] array = new string[5];
					array[0] = "Can not change scene. Moveset is being synced. (";
					array[1] = multiplayerRoomPlayer.name;
					array[2] = ") (";
					int num = 3;
					string text;
					if (multiplayerRoomPlayer == null)
					{
						text = null;
					}
					else
					{
						NetworkConnectionToClient connectionToClient = multiplayerRoomPlayer.connectionToClient;
						text = ((connectionToClient != null) ? connectionToClient.address : null);
					}
					array[num] = text;
					array[4] = ")";
					Debug.Log(string.Concat(array));
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06000481 RID: 1153 RVA: 0x000158D8 File Offset: 0x00013AD8
	public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling)
	{
		if (GeneralManager.singleton != null)
		{
			GeneralManager.singleton.ShowLoadingBarForOperation(NetworkManager.loadingSceneAsync, newSceneName, !this.joinedMidGame, false, false);
		}
	}

	// Token: 0x06000482 RID: 1154 RVA: 0x00015902 File Offset: 0x00013B02
	public override void OnClientSceneChanged()
	{
		if (MultiplayerRoomPlayer.localMultiplayerRoomPlayer != null)
		{
			MultiplayerRoomPlayer.localMultiplayerRoomPlayer.moveSetChangeEnabled = true;
		}
		base.OnClientSceneChanged();
	}

	// Token: 0x06000483 RID: 1155 RVA: 0x00015924 File Offset: 0x00013B24
	public void SetTransport(Transport newTransport)
	{
		this.transport = newTransport;
		Transport.active = this.transport;
		NetworkStatistics networkStatistics = UnityEngine.Object.FindObjectOfType<NetworkStatistics>();
		if (networkStatistics != null && networkStatistics.isActiveAndEnabled)
		{
			GameObject gameObject = networkStatistics.gameObject;
			UnityEngine.Object.DestroyImmediate(networkStatistics);
			gameObject.AddComponent<NetworkStatistics>();
		}
		if (this.transport.GetType() == typeof(KcpTransport))
		{
			this.SetOfflineScene("Assets/Scenes/MenuMultiplayer.unity");
			return;
		}
		this.SetOfflineScene("Assets/Scenes/MenuMultiplayerSteam.unity");
	}

	// Token: 0x06000484 RID: 1156 RVA: 0x0001599F File Offset: 0x00013B9F
	public Transport GetTransport()
	{
		return this.transport;
	}

	// Token: 0x06000485 RID: 1157 RVA: 0x000159A7 File Offset: 0x00013BA7
	public GladioMoriServerType GetGladioMoriServerType()
	{
		if (!(this.transport != null))
		{
			return GladioMoriServerType.None;
		}
		if (this.transport.GetType() == typeof(KcpTransport))
		{
			return GladioMoriServerType.DirectIp;
		}
		return GladioMoriServerType.Steam;
	}

	// Token: 0x06000486 RID: 1158 RVA: 0x000159D8 File Offset: 0x00013BD8
	public void SetGameplayScene(string newScene)
	{
		this.GameplayScene = newScene;
	}

	// Token: 0x06000487 RID: 1159 RVA: 0x000159E1 File Offset: 0x00013BE1
	public void SetOfflineScene(string newScene)
	{
		this.offlineScene = newScene;
	}

	// Token: 0x06000488 RID: 1160 RVA: 0x000159EC File Offset: 0x00013BEC
	public new void ReadyStatusChanged()
	{
		int num = 0;
		int num2 = 0;
		foreach (NetworkRoomPlayer networkRoomPlayer in this.roomSlots)
		{
			if (networkRoomPlayer != null)
			{
				num++;
				if (networkRoomPlayer.readyToBegin)
				{
					if (this.VerifyPlayerInfo(networkRoomPlayer))
					{
						num2++;
					}
					else
					{
						((MultiplayerRoomPlayer)networkRoomPlayer).ClientRpcSetRoomReadyState(false);
					}
				}
			}
		}
		if (num == num2)
		{
			base.CheckReadyToBegin();
			return;
		}
		base.allPlayersReady = false;
	}

	// Token: 0x06000489 RID: 1161 RVA: 0x00015A80 File Offset: 0x00013C80
	private bool VerifyPlayerInfo(NetworkRoomPlayer networkRoomPlayer)
	{
		MultiplayerRoomPlayer multiplayerRoomPlayer = (MultiplayerRoomPlayer)networkRoomPlayer;
		if (!(multiplayerRoomPlayer != null))
		{
			return false;
		}
		if (!GameSettingsHelper.CheckCanPlayerReady(multiplayerRoomPlayer.selectedMoveSet, multiplayerRoomPlayer.selectedEquipment))
		{
			return false;
		}
		Debug.Log("PLAYER STATUS VERIFIED");
		return true;
	}

	// Token: 0x0600048A RID: 1162 RVA: 0x00015AC0 File Offset: 0x00013CC0
	public void UnReadyAll()
	{
		if (MultiplayerRoomPlayer.localMultiplayerRoomPlayer != null && MultiplayerRoomPlayer.localMultiplayerRoomPlayer.isServer)
		{
			foreach (NetworkRoomPlayer networkRoomPlayer in this.roomSlots)
			{
				if (networkRoomPlayer.readyToBegin)
				{
					networkRoomPlayer.SetReadyToBegin(false);
				}
			}
		}
	}

	// Token: 0x0600048B RID: 1163 RVA: 0x00015B34 File Offset: 0x00013D34
	public void SetDefaultEquipmentForAll()
	{
		if (MultiplayerRoomPlayer.localMultiplayerRoomPlayer != null && MultiplayerRoomPlayer.localMultiplayerRoomPlayer.isServer)
		{
			foreach (NetworkRoomPlayer networkRoomPlayer in this.roomSlots)
			{
				((MultiplayerRoomPlayer)networkRoomPlayer).SetDefaultEquipment();
			}
		}
	}

	// Token: 0x0600048C RID: 1164 RVA: 0x00015BA4 File Offset: 0x00013DA4
	public void CheckAllowedEquipmentForAll()
	{
		if (MultiplayerRoomPlayer.localMultiplayerRoomPlayer != null && MultiplayerRoomPlayer.localMultiplayerRoomPlayer.isServer)
		{
			foreach (NetworkRoomPlayer networkRoomPlayer in this.roomSlots)
			{
				((MultiplayerRoomPlayer)networkRoomPlayer).CheckAllowedEquipment();
			}
		}
	}

	// Token: 0x0600048D RID: 1165 RVA: 0x00015C14 File Offset: 0x00013E14
	public void UpdateSelectedMoveSetForAll()
	{
		if (MultiplayerRoomPlayer.localMultiplayerRoomPlayer != null && MultiplayerRoomPlayer.localMultiplayerRoomPlayer.isServer)
		{
			foreach (NetworkRoomPlayer networkRoomPlayer in this.roomSlots)
			{
				((MultiplayerRoomPlayer)networkRoomPlayer).UpdateSelectedMoveSet();
			}
		}
	}

	// Token: 0x0600048E RID: 1166 RVA: 0x00015C84 File Offset: 0x00013E84
	public void UpdatePreviewCharactersForAll()
	{
		foreach (NetworkRoomPlayer networkRoomPlayer in this.roomSlots)
		{
			((MultiplayerRoomPlayer)networkRoomPlayer).SetupPreviewCharacter();
		}
	}

	// Token: 0x0600048F RID: 1167 RVA: 0x00015CDC File Offset: 0x00013EDC
	[Server]
	public void BanPlayerConfirm(NetworkConnectionToClient conn, string playerName = "")
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void MultiplayerRoomManager::BanPlayerConfirm(Mirror.NetworkConnectionToClient,System.String)' called when server was not active");
			return;
		}
		string address = conn.address;
		BasicConfirmDialog basicConfirmDialog = GeneralManager.CreateConfirmDialog(LocalizationHelpers.LocalizedText("confirm_txt_ban_player", new object[]
		{
			playerName
		}), null, false);
		if (basicConfirmDialog != null)
		{
			basicConfirmDialog.okButton.onClick.AddListener(delegate()
			{
				this.BanPlayer(conn, address, playerName);
			});
			return;
		}
		this.BanPlayer(conn, address, playerName);
	}

	// Token: 0x06000490 RID: 1168 RVA: 0x00015D89 File Offset: 0x00013F89
	public void BanPlayer(NetworkConnectionToClient conn, string address, string playerName = "")
	{
		BanHelpers.AddConnectionToBanList(address, this.GetGladioMoriServerType(), playerName);
		conn.Disconnect();
	}

	// Token: 0x06000491 RID: 1169 RVA: 0x00015DA0 File Offset: 0x00013FA0
	private void ClearLoadedPlayers(string newScene)
	{
		this.registeredForStaminaManager = false;
		this.loadedPlayers.Clear();
		this.loadingSceneForAll = true;
		if (this.loadingTimeoutCoroutine != null)
		{
			base.StopCoroutine(this.loadingTimeoutCoroutine);
		}
		if (newScene.Contains("map_"))
		{
			this.startedLoad = Time.unscaledTime;
			this.loadingTimeoutCoroutine = base.StartCoroutine(this.LoadingTimeoutCoroutine());
		}
	}

	// Token: 0x06000492 RID: 1170 RVA: 0x00015E04 File Offset: 0x00014004
	public float TimeUntilTimeout()
	{
		return this.startedLoad + this.timeoutDuration - Time.unscaledTime;
	}

	// Token: 0x06000493 RID: 1171 RVA: 0x00015E19 File Offset: 0x00014019
	private IEnumerator LoadingTimeoutCoroutine()
	{
		while (this.loadingSceneForAll)
		{
			if (this.TimeUntilTimeout() < 0f && (NetworkManager.loadingSceneAsync == null || NetworkManager.loadingSceneAsync.isDone))
			{
				Debug.Log("Scene load timed out");
				this.HandleSceneLoadedForAllCorrectly();
				yield break;
			}
			yield return new WaitForSecondsRealtime(1f);
		}
		yield break;
	}

	// Token: 0x06000494 RID: 1172 RVA: 0x00015E28 File Offset: 0x00014028
	private void RemoveLoadedConnection(NetworkConnectionToClient conn)
	{
		this.loadedPlayers.Remove(conn);
	}

	// Token: 0x06000495 RID: 1173 RVA: 0x00015E37 File Offset: 0x00014037
	private void CheckIfSceneHasBeenLoadedByAll()
	{
		if (this.loadedPlayers.Count == base.numPlayers)
		{
			this.HandleSceneLoadedForAllCorrectly();
		}
	}

	// Token: 0x06000496 RID: 1174 RVA: 0x00015E54 File Offset: 0x00014054
	private void HandleSceneLoadedForAllCorrectly()
	{
		if (this.loadingTimeoutCoroutine != null)
		{
			base.StopCoroutine(this.loadingTimeoutCoroutine);
		}
		if (this.loadingSceneForAll)
		{
			this.loadingSceneForAll = false;
			NetworkServer.SendToAll<MultiplayerRoomManager.SceneLoaded>(new MultiplayerRoomManager.SceneLoaded
			{
				loaded = true
			}, 0, false);
		}
		this.RegisterPlayersForStaminaManager();
	}

	// Token: 0x06000497 RID: 1175 RVA: 0x00015EA4 File Offset: 0x000140A4
	public void RegisterPlayersForStaminaManager()
	{
		if (this.registeredForStaminaManager)
		{
			return;
		}
		List<PlayerHealth> list = new List<PlayerHealth>();
		for (int i = 0; i < this.roomSlots.Count; i++)
		{
			MultiplayerRoomPlayer multiplayerRoomPlayer = (MultiplayerRoomPlayer)this.roomSlots[i];
			if (multiplayerRoomPlayer != null && multiplayerRoomPlayer.playerMultiplayerInputManager != null && multiplayerRoomPlayer.playerMultiplayerInputManager.playerHealth != null)
			{
				list.Add(multiplayerRoomPlayer.playerMultiplayerInputManager.playerHealth);
			}
		}
		StaminaManager.RegisterPlayerHealths(list);
		this.registeredForStaminaManager = true;
	}

	// Token: 0x06000498 RID: 1176 RVA: 0x00015F30 File Offset: 0x00014130
	public void SendCutsToMidGameJoiner(NetworkConnectionToClient conn)
	{
		for (int i = 0; i < this.roomSlots.Count; i++)
		{
			MultiplayerRoomPlayer multiplayerRoomPlayer = (MultiplayerRoomPlayer)this.roomSlots[i];
			if (multiplayerRoomPlayer != null && multiplayerRoomPlayer.playerMultiplayerInputManager != null && multiplayerRoomPlayer.playerMultiplayerInputManager.playerHealth != null && multiplayerRoomPlayer.playerMultiplayerInputManager.playerHealth.playerHealthMultiplayer != null && multiplayerRoomPlayer.playerMultiplayerInputManager.playerHealth.playerHealthMultiplayer.cuttableMultiplayerHandler != null)
			{
				multiplayerRoomPlayer.playerMultiplayerInputManager.playerHealth.playerHealthMultiplayer.cuttableMultiplayerHandler.SendOldCutsToClient(conn);
			}
		}
	}

	// Token: 0x040002D0 RID: 720
	public GameObject multiplayerGameMasterPrefab;

	// Token: 0x040002D1 RID: 721
	public GameObject multiplayerSoundManagerPrefab;

	// Token: 0x040002D2 RID: 722
	public GameObject multiplayerGameSettingsManagerPrefab;

	// Token: 0x040002D3 RID: 723
	public List<NetworkConnectionToClient> loadedPlayers = new List<NetworkConnectionToClient>();

	// Token: 0x040002D4 RID: 724
	public bool joinedMidGame;

	// Token: 0x040002D5 RID: 725
	private Coroutine loadingTimeoutCoroutine;

	// Token: 0x040002D6 RID: 726
	private bool loadingSceneForAll;

	// Token: 0x040002D7 RID: 727
	private float startedLoad;

	// Token: 0x040002D8 RID: 728
	private float timeoutDuration = 25f;

	// Token: 0x040002D9 RID: 729
	public bool registeredForStaminaManager;

	// Token: 0x0200008B RID: 139
	public struct SceneLoaded : NetworkMessage
	{
		// Token: 0x040002DA RID: 730
		public bool loaded;
	}
}
