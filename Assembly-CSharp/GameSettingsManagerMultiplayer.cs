using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Mirror;
using MoveClasses;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

// Token: 0x0200007E RID: 126
[JsonObject(MemberSerialization.OptIn)]
public class GameSettingsManagerMultiplayer : NetworkBehaviour, IGameSettingsManager
{
	// Token: 0x170000CB RID: 203
	// (get) Token: 0x06000398 RID: 920 RVA: 0x00011A24 File Offset: 0x0000FC24
	// (set) Token: 0x06000399 RID: 921 RVA: 0x00011A2C File Offset: 0x0000FC2C
	[JsonProperty]
	public string SelectedMap
	{
		get
		{
			return this._selectedMap;
		}
		set
		{
			this.Network_selectedMap = value;
		}
	}

	// Token: 0x170000CC RID: 204
	// (get) Token: 0x0600039A RID: 922 RVA: 0x00011A35 File Offset: 0x0000FC35
	// (set) Token: 0x0600039B RID: 923 RVA: 0x00011A3D File Offset: 0x0000FC3D
	public int AiAmount { get; set; }

	// Token: 0x170000CD RID: 205
	// (get) Token: 0x0600039C RID: 924 RVA: 0x00011A46 File Offset: 0x0000FC46
	// (set) Token: 0x0600039D RID: 925 RVA: 0x00011A4E File Offset: 0x0000FC4E
	[JsonProperty]
	public int EquipmentPoints
	{
		get
		{
			return this._equipmentPoints;
		}
		set
		{
			if (value != this._equipmentPoints)
			{
				this.Network_equipmentPoints = value;
				this.ServerUnreadyAll();
				GameSettingsManagerMultiplayer.SetLobbyPoints();
			}
		}
	}

	// Token: 0x170000CE RID: 206
	// (get) Token: 0x0600039E RID: 926 RVA: 0x00011A6B File Offset: 0x0000FC6B
	// (set) Token: 0x0600039F RID: 927 RVA: 0x00011A73 File Offset: 0x0000FC73
	[JsonProperty]
	public string LobbyName
	{
		get
		{
			return this._lobbyName;
		}
		set
		{
			this.Network_lobbyName = value;
		}
	}

	// Token: 0x170000CF RID: 207
	// (get) Token: 0x060003A0 RID: 928 RVA: 0x00011A7C File Offset: 0x0000FC7C
	// (set) Token: 0x060003A1 RID: 929 RVA: 0x00011A84 File Offset: 0x0000FC84
	public LobbyPrivacyType LobbyPrivacyType
	{
		get
		{
			return this._lobbyPrivacyType;
		}
		set
		{
			this.Network_lobbyPrivacyType = value;
		}
	}

	// Token: 0x170000D0 RID: 208
	// (get) Token: 0x060003A2 RID: 930 RVA: 0x00011A8D File Offset: 0x0000FC8D
	// (set) Token: 0x060003A3 RID: 931 RVA: 0x00011A95 File Offset: 0x0000FC95
	[JsonProperty]
	public float TimeScaleMin
	{
		get
		{
			return this._timeScaleMin;
		}
		set
		{
			this.Network_timeScaleMin = value;
			GameSettingsManagerMultiplayer.SetLobbyTimeScale();
		}
	}

	// Token: 0x170000D1 RID: 209
	// (get) Token: 0x060003A4 RID: 932 RVA: 0x00011AA3 File Offset: 0x0000FCA3
	// (set) Token: 0x060003A5 RID: 933 RVA: 0x00011AAB File Offset: 0x0000FCAB
	[JsonProperty]
	public AllowedMovesetTypes AllowedMovesetTypes
	{
		get
		{
			return this._allowedMovesetTypes;
		}
		set
		{
			this.Network_allowedMovesetTypes = value;
			this.ServerUnreadyAll();
		}
	}

	// Token: 0x170000D2 RID: 210
	// (get) Token: 0x060003A6 RID: 934 RVA: 0x00011ABA File Offset: 0x0000FCBA
	// (set) Token: 0x060003A7 RID: 935 RVA: 0x00011AC4 File Offset: 0x0000FCC4
	[JsonProperty]
	public GameTypes GameType
	{
		get
		{
			return this._gameType;
		}
		set
		{
			if (value != this._gameType)
			{
				this.Network_gameType = value;
				this.ServerUnreadyAll();
				GameSettingsManagerMultiplayer.SetLobbyGameType();
				GameSettingsManagerMultiplayer.SetLobbyStamina();
			}
			if (base.isServer && MultiplayerRoomManager.GetMultiplayerRoomManager() != null)
			{
				MultiplayerRoomManager.GetMultiplayerRoomManager().UpdateSelectedMoveSetForAll();
			}
		}
	}

	// Token: 0x170000D3 RID: 211
	// (get) Token: 0x060003A8 RID: 936 RVA: 0x00011B10 File Offset: 0x0000FD10
	// (set) Token: 0x060003A9 RID: 937 RVA: 0x00011B18 File Offset: 0x0000FD18
	[JsonProperty]
	public bool AllowEquipmentEdit
	{
		get
		{
			return this._allowEquipmentEdit;
		}
		set
		{
			this.Network_allowEquipmentEdit = value;
			this.ServerUnreadyAll();
			if (base.isServer && MultiplayerRoomManager.GetMultiplayerRoomManager() != null)
			{
				MultiplayerRoomManager.GetMultiplayerRoomManager().SetDefaultEquipmentForAll();
			}
		}
	}

	// Token: 0x170000D4 RID: 212
	// (get) Token: 0x060003AA RID: 938 RVA: 0x00011B46 File Offset: 0x0000FD46
	// (set) Token: 0x060003AB RID: 939 RVA: 0x00011B61 File Offset: 0x0000FD61
	[JsonProperty]
	public bool UseStamina
	{
		get
		{
			return (this.saving || this.GameType != GameTypes.Legacy) && this._useStamina;
		}
		set
		{
			this.Network_useStamina = value;
			this.ServerUnreadyAll();
			GameSettingsManagerMultiplayer.SetLobbyStamina();
		}
	}

	// Token: 0x170000D5 RID: 213
	// (get) Token: 0x060003AC RID: 940 RVA: 0x00011B75 File Offset: 0x0000FD75
	// (set) Token: 0x060003AD RID: 941 RVA: 0x00011B7D File Offset: 0x0000FD7D
	[JsonProperty]
	public bool UseDismemberment
	{
		get
		{
			return this._useDismemberment;
		}
		set
		{
			this.Network_useDismemberment = value;
			this.ServerUnreadyAll();
			GameSettingsManagerMultiplayer.SetLobbyDismemberment();
		}
	}

	// Token: 0x170000D6 RID: 214
	// (get) Token: 0x060003AE RID: 942 RVA: 0x00011B91 File Offset: 0x0000FD91
	// (set) Token: 0x060003AF RID: 943 RVA: 0x00011B99 File Offset: 0x0000FD99
	[JsonProperty]
	public string WelcomeMessage
	{
		get
		{
			return this._welcomeMessage;
		}
		set
		{
			this.Network_welcomeMessage = value;
		}
	}

	// Token: 0x060003B0 RID: 944 RVA: 0x00011BA2 File Offset: 0x0000FDA2
	private void Awake()
	{
		if (IGameSettingsManager.singleton == null)
		{
			IGameSettingsManager.singleton = this;
			GameSettingsManagerMultiplayer.singleton = this;
			UnityEngine.Object.DontDestroyOnLoad(this);
			return;
		}
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x060003B1 RID: 945 RVA: 0x00011BC4 File Offset: 0x0000FDC4
	private void Update()
	{
		if (base.isClientOnly && NetworkClient.active && this.lastConnectionTimeoutTime + this.connectionTimeoutUpdateFrequency < Time.unscaledTime)
		{
			this.lastConnectionTimeoutTime = Time.unscaledTime;
			if (Generic.IsConnectionAlive(NetworkClient.connection.lastMessageTime, 0.5f))
			{
				GeneralManager.badConnection = false;
				return;
			}
			GeneralManager.badConnection = true;
			if (!Generic.IsConnectionAlive(NetworkClient.connection.lastMessageTime, 8f))
			{
				Debug.Log("Local connection timeout");
				this.LeaveLobby(ConnectionEndedType.ConnectionLost);
			}
		}
	}

	// Token: 0x060003B2 RID: 946 RVA: 0x00011C49 File Offset: 0x0000FE49
	public void SetRollingFeet(bool newRollingFeetValue)
	{
		this.NetworkrollingFeet = newRollingFeetValue;
	}

	// Token: 0x060003B3 RID: 947 RVA: 0x00011C52 File Offset: 0x0000FE52
	public bool GetRollingFeet()
	{
		return this.rollingFeet;
	}

	// Token: 0x060003B4 RID: 948 RVA: 0x000117D2 File Offset: 0x0000F9D2
	public void DestroyGameSettingsManager()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060003B5 RID: 949 RVA: 0x00011C5A File Offset: 0x0000FE5A
	public void OnDestroy()
	{
		if (GameSettingsManagerMultiplayer.singleton == this)
		{
			this.SaveGameSettings();
		}
		IGameSettingsManager.singleton = null;
		GameSettingsManagerMultiplayer.singleton = null;
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
	}

	// Token: 0x060003B6 RID: 950 RVA: 0x00011C8C File Offset: 0x0000FE8C
	private void SetSelectedMap(string oldMap, string newMap)
	{
		this.UpdateSettingDisplaysOnClients();
	}

	// Token: 0x060003B7 RID: 951 RVA: 0x00011C94 File Offset: 0x0000FE94
	private void SetEquipmentPoints(int oldEquipmentPoints, int newEquipmentPoints)
	{
		PlayerCanvasController playerCanvasController = UnityEngine.Object.FindObjectOfType<PlayerCanvasController>();
		if (playerCanvasController != null)
		{
			playerCanvasController.UpdateEquipmentPoints();
		}
		this.UpdateSettingDisplaysOnClients();
	}

	// Token: 0x060003B8 RID: 952 RVA: 0x00011C8C File Offset: 0x0000FE8C
	private void SetLobbyName(string oldLobbyName, string newLobbyName)
	{
		this.UpdateSettingDisplaysOnClients();
	}

	// Token: 0x060003B9 RID: 953 RVA: 0x00011C8C File Offset: 0x0000FE8C
	private void SetPrivacyType(LobbyPrivacyType oldPrivacyType, LobbyPrivacyType newPrivacyType)
	{
		this.UpdateSettingDisplaysOnClients();
	}

	// Token: 0x060003BA RID: 954 RVA: 0x00011C8C File Offset: 0x0000FE8C
	private void SetTimeScaleMin(float oldTimeScaleMin, float newTimeScaleMin)
	{
		this.UpdateSettingDisplaysOnClients();
	}

	// Token: 0x060003BB RID: 955 RVA: 0x00011CBC File Offset: 0x0000FEBC
	private void SetAllowedMoveSetTypes(AllowedMovesetTypes oldValue, AllowedMovesetTypes newValue)
	{
		this.UpdateSettingDisplaysOnClients();
		PlayerCanvasController playerCanvasController = UnityEngine.Object.FindObjectOfType<PlayerCanvasController>(true);
		if (playerCanvasController != null)
		{
			playerCanvasController.GenerateMoveSetButtons(false);
		}
	}

	// Token: 0x060003BC RID: 956 RVA: 0x00011CE6 File Offset: 0x0000FEE6
	private void SetGameType(GameTypes oldValue, GameTypes newValue)
	{
		this.UpdateSettingDisplaysOnClients();
		if (MultiplayerRoomManager.GetMultiplayerRoomManager() != null)
		{
			MultiplayerRoomManager.GetMultiplayerRoomManager().UpdatePreviewCharactersForAll();
		}
	}

	// Token: 0x060003BD RID: 957 RVA: 0x00011D08 File Offset: 0x0000FF08
	private void SetAllowEquipmentEdit(bool oldValue, bool newValue)
	{
		EquipmentPanel equipmentPanel = UnityEngine.Object.FindObjectOfType<EquipmentPanel>(true);
		if (equipmentPanel != null)
		{
			equipmentPanel.UpdateEquipmentInfo(false, false);
		}
		this.UpdateSettingDisplaysOnClients();
	}

	// Token: 0x060003BE RID: 958 RVA: 0x00011C8C File Offset: 0x0000FE8C
	private void SetUseStamina(bool oldValue, bool newValue)
	{
		this.UpdateSettingDisplaysOnClients();
	}

	// Token: 0x060003BF RID: 959 RVA: 0x00011C8C File Offset: 0x0000FE8C
	private void SetUseDismemberment(bool oldValue, bool newValue)
	{
		this.UpdateSettingDisplaysOnClients();
	}

	// Token: 0x170000D7 RID: 215
	// (get) Token: 0x060003C0 RID: 960 RVA: 0x00011D33 File Offset: 0x0000FF33
	// (set) Token: 0x060003C1 RID: 961 RVA: 0x00011D3B File Offset: 0x0000FF3B
	[JsonProperty]
	public List<EquipmentType> DisabledEquipmentTypes
	{
		get
		{
			return this._disabledEquipmentTypes;
		}
		set
		{
			this.Network_disabledEquipmentTypes = value;
			this.ServerUnreadyAll();
			this.CheckAllowedEquipmentForAll();
		}
	}

	// Token: 0x060003C2 RID: 962 RVA: 0x00011D50 File Offset: 0x0000FF50
	public void CheckAllowedEquipmentForAll()
	{
		if (base.isServer && MultiplayerRoomManager.GetMultiplayerRoomManager() != null)
		{
			MultiplayerRoomManager.GetMultiplayerRoomManager().CheckAllowedEquipmentForAll();
		}
	}

	// Token: 0x060003C3 RID: 963 RVA: 0x00011D74 File Offset: 0x0000FF74
	public void ToggleDisabledEquipmentType(EquipmentType equipmentType)
	{
		for (int i = 0; i < this.DisabledEquipmentTypes.Count; i++)
		{
			if (this.DisabledEquipmentTypes[i] == equipmentType)
			{
				this.DisabledEquipmentTypes.RemoveAt(i);
				this.DisabledEquipmentTypes = this.DisabledEquipmentTypes.ToList<EquipmentType>();
				return;
			}
		}
		this.DisabledEquipmentTypes.Add(equipmentType);
		this.DisabledEquipmentTypes = this.DisabledEquipmentTypes.ToList<EquipmentType>();
	}

	// Token: 0x060003C4 RID: 964 RVA: 0x00011DE4 File Offset: 0x0000FFE4
	private void DisabledEquipmentTypesChanged(List<EquipmentType> oldValue, List<EquipmentType> newValue)
	{
		EquipmentSelectDialog equipmentSelectDialog = UnityEngine.Object.FindObjectOfType<EquipmentSelectDialog>();
		if (equipmentSelectDialog != null)
		{
			equipmentSelectDialog.UpdateAllButtonsUI();
		}
		EquipmentPanel equipmentPanel = UnityEngine.Object.FindObjectOfType<EquipmentPanel>();
		if (equipmentPanel != null)
		{
			equipmentPanel.UpdateAllButtonsUI();
		}
	}

	// Token: 0x060003C5 RID: 965 RVA: 0x00011E1B File Offset: 0x0001001B
	private void SetWelcomeMessage(string oldValue, string newValue)
	{
		if (!string.IsNullOrEmpty(this.WelcomeMessage) && !this.welcomeMessageDisplayed && !base.isServer)
		{
			this.welcomeMessageDisplayed = true;
			this.DisplayWelcomeMessage();
		}
	}

	// Token: 0x060003C6 RID: 966 RVA: 0x00011E47 File Offset: 0x00010047
	public void DisplayWelcomeMessage()
	{
		GeneralManager.CreateLargeConfirmDialog(this.WelcomeMessage, LocalizationHelpers.LocalizedText("gamesetting_welcome_message", Array.Empty<object>()), true);
	}

	// Token: 0x060003C7 RID: 967 RVA: 0x00011E65 File Offset: 0x00010065
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (!scene.name.ToLower().Contains("lobby") && !scene.name.ToLower().Contains("map_"))
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060003C8 RID: 968 RVA: 0x00011EA2 File Offset: 0x000100A2
	public void UpdateSettingDisplaysOnClients()
	{
		if (base.isClientOnly && GameSettingsPanel.singleton != null)
		{
			GameSettingsPanel.singleton.UpdateSettingValues();
		}
	}

	// Token: 0x060003C9 RID: 969 RVA: 0x00011EC3 File Offset: 0x000100C3
	public virtual void GameVersionChanged(string _, string newGameVersion)
	{
		this.CheckGameVersion();
	}

	// Token: 0x060003CA RID: 970 RVA: 0x00011ECB File Offset: 0x000100CB
	private void CheckGameVersion()
	{
		if (this.gameVersion != Application.version)
		{
			this.LeaveLobby(ConnectionEndedType.VersionConflict);
		}
	}

	// Token: 0x060003CB RID: 971 RVA: 0x00011EE8 File Offset: 0x000100E8
	public void LeaveLobby(ConnectionEndedType endType = ConnectionEndedType.None)
	{
		if (GeneralManager.singleton != null)
		{
			GeneralManager.singleton.leftMultiplayerSession = true;
			if (endType == ConnectionEndedType.None)
			{
				GeneralManager.singleton.leftMultiplayerSessionVoluntarily = true;
			}
			else
			{
				GeneralManager.singleton.leftMultiplayerSessionVoluntarily = false;
				GeneralManager.singleton.connectionEndedType = endType;
			}
		}
		SteamManager.steamManager.LeaveLobby();
		if (base.isClientOnly)
		{
			NetworkManager.singleton.StopClient();
		}
		else if (base.isServer)
		{
			NetworkManager.singleton.StopHost();
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060003CC RID: 972 RVA: 0x00011F70 File Offset: 0x00010170
	public static void SetGameStatus(LobbyStatus newStatus)
	{
		if (GameSettingsManagerMultiplayer.singleton != null && GameSettingsManagerMultiplayer.singleton.isServer && SteamManager.steamManager != null)
		{
			SteamManager steamManager = SteamManager.steamManager;
			string key = "lobbyState";
			int num = (int)newStatus;
			steamManager.SetLobbyData(key, num.ToString());
		}
	}

	// Token: 0x060003CD RID: 973 RVA: 0x00011FBC File Offset: 0x000101BC
	public static void SetLobbyGameType()
	{
		if (GameSettingsManagerMultiplayer.singleton != null && GameSettingsManagerMultiplayer.singleton.isServer && SteamManager.steamManager != null)
		{
			SteamManager.steamManager.SetLobbyData("gameType", ((int)GameSettingsManagerMultiplayer.singleton.GameType).ToString());
		}
	}

	// Token: 0x060003CE RID: 974 RVA: 0x00012014 File Offset: 0x00010214
	public static void SetLobbyPoints()
	{
		if (GameSettingsManagerMultiplayer.singleton != null && GameSettingsManagerMultiplayer.singleton.isServer && SteamManager.steamManager != null)
		{
			SteamManager.steamManager.SetLobbyData("points", GameSettingsManagerMultiplayer.singleton.EquipmentPoints.ToString());
		}
	}

	// Token: 0x060003CF RID: 975 RVA: 0x0001206C File Offset: 0x0001026C
	public static void SetLobbyDismemberment()
	{
		if (GameSettingsManagerMultiplayer.singleton != null && GameSettingsManagerMultiplayer.singleton.isServer && SteamManager.steamManager != null)
		{
			SteamManager.steamManager.SetLobbyData("dismemberment", GameSettingsManagerMultiplayer.singleton.UseDismemberment.ToString());
		}
	}

	// Token: 0x060003D0 RID: 976 RVA: 0x000120C4 File Offset: 0x000102C4
	public static void SetLobbyStamina()
	{
		if (GameSettingsManagerMultiplayer.singleton != null && GameSettingsManagerMultiplayer.singleton.isServer && SteamManager.steamManager != null)
		{
			SteamManager.steamManager.SetLobbyData("stamina", GameSettingsManagerMultiplayer.singleton.UseStamina.ToString());
		}
	}

	// Token: 0x060003D1 RID: 977 RVA: 0x0001211C File Offset: 0x0001031C
	public static void SetLobbyTimeScale()
	{
		if (GameSettingsManagerMultiplayer.singleton != null && GameSettingsManagerMultiplayer.singleton.isServer && SteamManager.steamManager != null)
		{
			SteamManager.steamManager.SetLobbyData("lobbyTimeScale", GameSettingsHelper.GetTextForTimeScaleValue(GameSettingsManagerMultiplayer.singleton.TimeScaleMin));
			return;
		}
		if (SteamManager.steamManager != null)
		{
			SteamManager.steamManager.SetLobbyData("lobbyTimeScale", GameSettingsHelper.GetTextForTimeScaleValue(0.5f));
		}
	}

	// Token: 0x060003D2 RID: 978 RVA: 0x00012196 File Offset: 0x00010396
	public void ServerUnreadyAll()
	{
		if (base.isServer && MultiplayerRoomManager.GetMultiplayerRoomManager() != null)
		{
			MultiplayerRoomManager.GetMultiplayerRoomManager().UnReadyAll();
		}
	}

	// Token: 0x060003D3 RID: 979 RVA: 0x000121B8 File Offset: 0x000103B8
	public void SaveGameSettings()
	{
		if (!base.isServer)
		{
			return;
		}
		try
		{
			this.saving = true;
			string json = JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			});
			Generic.SaveJsonToFile(SettingsHelper.GetMultiplayerGameSettingsSavePath(), json);
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
		this.saving = false;
	}

	// Token: 0x060003D4 RID: 980 RVA: 0x00012218 File Offset: 0x00010418
	public void LoadGameSettings()
	{
		if (this.loaded)
		{
			return;
		}
		if (!base.isServer)
		{
			return;
		}
		try
		{
			string value = Generic.LoadJsonFromFile(SettingsHelper.GetMultiplayerGameSettingsSavePath());
			if (!string.IsNullOrWhiteSpace(value))
			{
				JsonConvert.PopulateObject(value, this);
			}
			if (base.isServer && MultiplayerRoomManager.GetMultiplayerRoomManager() != null && SceneManagerWithParameters.IsValidScene(this.SelectedMap))
			{
				MultiplayerRoomManager.GetMultiplayerRoomManager().SetGameplayScene(this.SelectedMap);
			}
			this.CheckAllowedEquipmentForAll();
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
		this.loaded = true;
	}

	// Token: 0x060003D6 RID: 982 RVA: 0x0000C7D7 File Offset: 0x0000A9D7
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x170000D8 RID: 216
	// (get) Token: 0x060003D7 RID: 983 RVA: 0x00012328 File Offset: 0x00010528
	// (set) Token: 0x060003D8 RID: 984 RVA: 0x0001233B File Offset: 0x0001053B
	public bool NetworkrollingFeet
	{
		get
		{
			return this.rollingFeet;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<bool>(value, ref this.rollingFeet, 1UL, null);
		}
	}

	// Token: 0x170000D9 RID: 217
	// (get) Token: 0x060003D9 RID: 985 RVA: 0x00012358 File Offset: 0x00010558
	// (set) Token: 0x060003DA RID: 986 RVA: 0x0001236B File Offset: 0x0001056B
	public string Network_selectedMap
	{
		get
		{
			return this._selectedMap;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<string>(value, ref this._selectedMap, 2UL, new Action<string, string>(this.SetSelectedMap));
		}
	}

	// Token: 0x170000DA RID: 218
	// (get) Token: 0x060003DB RID: 987 RVA: 0x00012390 File Offset: 0x00010590
	// (set) Token: 0x060003DC RID: 988 RVA: 0x000123A3 File Offset: 0x000105A3
	public int Network_equipmentPoints
	{
		get
		{
			return this._equipmentPoints;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this._equipmentPoints, 4UL, new Action<int, int>(this.SetEquipmentPoints));
		}
	}

	// Token: 0x170000DB RID: 219
	// (get) Token: 0x060003DD RID: 989 RVA: 0x000123C8 File Offset: 0x000105C8
	// (set) Token: 0x060003DE RID: 990 RVA: 0x000123DB File Offset: 0x000105DB
	public string Network_lobbyName
	{
		get
		{
			return this._lobbyName;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<string>(value, ref this._lobbyName, 8UL, new Action<string, string>(this.SetLobbyName));
		}
	}

	// Token: 0x170000DC RID: 220
	// (get) Token: 0x060003DF RID: 991 RVA: 0x00012400 File Offset: 0x00010600
	// (set) Token: 0x060003E0 RID: 992 RVA: 0x00012413 File Offset: 0x00010613
	public LobbyPrivacyType Network_lobbyPrivacyType
	{
		get
		{
			return this._lobbyPrivacyType;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<LobbyPrivacyType>(value, ref this._lobbyPrivacyType, 16UL, new Action<LobbyPrivacyType, LobbyPrivacyType>(this.SetPrivacyType));
		}
	}

	// Token: 0x170000DD RID: 221
	// (get) Token: 0x060003E1 RID: 993 RVA: 0x00012438 File Offset: 0x00010638
	// (set) Token: 0x060003E2 RID: 994 RVA: 0x0001244B File Offset: 0x0001064B
	public float Network_timeScaleMin
	{
		get
		{
			return this._timeScaleMin;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<float>(value, ref this._timeScaleMin, 32UL, new Action<float, float>(this.SetTimeScaleMin));
		}
	}

	// Token: 0x170000DE RID: 222
	// (get) Token: 0x060003E3 RID: 995 RVA: 0x00012470 File Offset: 0x00010670
	// (set) Token: 0x060003E4 RID: 996 RVA: 0x00012483 File Offset: 0x00010683
	public AllowedMovesetTypes Network_allowedMovesetTypes
	{
		get
		{
			return this._allowedMovesetTypes;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<AllowedMovesetTypes>(value, ref this._allowedMovesetTypes, 64UL, new Action<AllowedMovesetTypes, AllowedMovesetTypes>(this.SetAllowedMoveSetTypes));
		}
	}

	// Token: 0x170000DF RID: 223
	// (get) Token: 0x060003E5 RID: 997 RVA: 0x000124A8 File Offset: 0x000106A8
	// (set) Token: 0x060003E6 RID: 998 RVA: 0x000124BB File Offset: 0x000106BB
	public GameTypes Network_gameType
	{
		get
		{
			return this._gameType;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<GameTypes>(value, ref this._gameType, 128UL, new Action<GameTypes, GameTypes>(this.SetGameType));
		}
	}

	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x060003E7 RID: 999 RVA: 0x000124E0 File Offset: 0x000106E0
	// (set) Token: 0x060003E8 RID: 1000 RVA: 0x000124F3 File Offset: 0x000106F3
	public bool Network_allowEquipmentEdit
	{
		get
		{
			return this._allowEquipmentEdit;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<bool>(value, ref this._allowEquipmentEdit, 256UL, new Action<bool, bool>(this.SetAllowEquipmentEdit));
		}
	}

	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x060003E9 RID: 1001 RVA: 0x00012518 File Offset: 0x00010718
	// (set) Token: 0x060003EA RID: 1002 RVA: 0x0001252B File Offset: 0x0001072B
	public bool Network_useStamina
	{
		get
		{
			return this._useStamina;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<bool>(value, ref this._useStamina, 512UL, new Action<bool, bool>(this.SetUseStamina));
		}
	}

	// Token: 0x170000E2 RID: 226
	// (get) Token: 0x060003EB RID: 1003 RVA: 0x00012550 File Offset: 0x00010750
	// (set) Token: 0x060003EC RID: 1004 RVA: 0x00012563 File Offset: 0x00010763
	public bool Network_useDismemberment
	{
		get
		{
			return this._useDismemberment;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<bool>(value, ref this._useDismemberment, 1024UL, new Action<bool, bool>(this.SetUseDismemberment));
		}
	}

	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x060003ED RID: 1005 RVA: 0x00012588 File Offset: 0x00010788
	// (set) Token: 0x060003EE RID: 1006 RVA: 0x0001259B File Offset: 0x0001079B
	public string Network_welcomeMessage
	{
		get
		{
			return this._welcomeMessage;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<string>(value, ref this._welcomeMessage, 2048UL, new Action<string, string>(this.SetWelcomeMessage));
		}
	}

	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x060003EF RID: 1007 RVA: 0x000125C0 File Offset: 0x000107C0
	// (set) Token: 0x060003F0 RID: 1008 RVA: 0x000125D3 File Offset: 0x000107D3
	public List<EquipmentType> Network_disabledEquipmentTypes
	{
		get
		{
			return this._disabledEquipmentTypes;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<List<EquipmentType>>(value, ref this._disabledEquipmentTypes, 4096UL, new Action<List<EquipmentType>, List<EquipmentType>>(this.DisabledEquipmentTypesChanged));
		}
	}

	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x060003F1 RID: 1009 RVA: 0x000125F8 File Offset: 0x000107F8
	// (set) Token: 0x060003F2 RID: 1010 RVA: 0x0001260B File Offset: 0x0001080B
	public string NetworkgameVersion
	{
		get
		{
			return this.gameVersion;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<string>(value, ref this.gameVersion, 8192UL, new Action<string, string>(this.GameVersionChanged));
		}
	}

	// Token: 0x060003F3 RID: 1011 RVA: 0x00012634 File Offset: 0x00010834
	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteBool(this.rollingFeet);
			writer.WriteString(this._selectedMap);
			writer.WriteInt(this._equipmentPoints);
			writer.WriteString(this._lobbyName);
			Mirror.GeneratedNetworkCode._Write_LobbyPrivacyType(writer, this._lobbyPrivacyType);
			writer.WriteFloat(this._timeScaleMin);
			Mirror.GeneratedNetworkCode._Write_AllowedMovesetTypes(writer, this._allowedMovesetTypes);
			Mirror.GeneratedNetworkCode._Write_GameTypes(writer, this._gameType);
			writer.WriteBool(this._allowEquipmentEdit);
			writer.WriteBool(this._useStamina);
			writer.WriteBool(this._useDismemberment);
			writer.WriteString(this._welcomeMessage);
			Mirror.GeneratedNetworkCode._Write_System.Collections.Generic.List`1<MoveClasses.EquipmentType>(writer, this._disabledEquipmentTypes);
			writer.WriteString(this.gameVersion);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1UL) != 0UL)
		{
			writer.WriteBool(this.rollingFeet);
		}
		if ((base.syncVarDirtyBits & 2UL) != 0UL)
		{
			writer.WriteString(this._selectedMap);
		}
		if ((base.syncVarDirtyBits & 4UL) != 0UL)
		{
			writer.WriteInt(this._equipmentPoints);
		}
		if ((base.syncVarDirtyBits & 8UL) != 0UL)
		{
			writer.WriteString(this._lobbyName);
		}
		if ((base.syncVarDirtyBits & 16UL) != 0UL)
		{
			Mirror.GeneratedNetworkCode._Write_LobbyPrivacyType(writer, this._lobbyPrivacyType);
		}
		if ((base.syncVarDirtyBits & 32UL) != 0UL)
		{
			writer.WriteFloat(this._timeScaleMin);
		}
		if ((base.syncVarDirtyBits & 64UL) != 0UL)
		{
			Mirror.GeneratedNetworkCode._Write_AllowedMovesetTypes(writer, this._allowedMovesetTypes);
		}
		if ((base.syncVarDirtyBits & 128UL) != 0UL)
		{
			Mirror.GeneratedNetworkCode._Write_GameTypes(writer, this._gameType);
		}
		if ((base.syncVarDirtyBits & 256UL) != 0UL)
		{
			writer.WriteBool(this._allowEquipmentEdit);
		}
		if ((base.syncVarDirtyBits & 512UL) != 0UL)
		{
			writer.WriteBool(this._useStamina);
		}
		if ((base.syncVarDirtyBits & 1024UL) != 0UL)
		{
			writer.WriteBool(this._useDismemberment);
		}
		if ((base.syncVarDirtyBits & 2048UL) != 0UL)
		{
			writer.WriteString(this._welcomeMessage);
		}
		if ((base.syncVarDirtyBits & 4096UL) != 0UL)
		{
			Mirror.GeneratedNetworkCode._Write_System.Collections.Generic.List`1<MoveClasses.EquipmentType>(writer, this._disabledEquipmentTypes);
		}
		if ((base.syncVarDirtyBits & 8192UL) != 0UL)
		{
			writer.WriteString(this.gameVersion);
		}
	}

	// Token: 0x060003F4 RID: 1012 RVA: 0x000128E4 File Offset: 0x00010AE4
	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			base.GeneratedSyncVarDeserialize<bool>(ref this.rollingFeet, null, reader.ReadBool());
			base.GeneratedSyncVarDeserialize<string>(ref this._selectedMap, new Action<string, string>(this.SetSelectedMap), reader.ReadString());
			base.GeneratedSyncVarDeserialize<int>(ref this._equipmentPoints, new Action<int, int>(this.SetEquipmentPoints), reader.ReadInt());
			base.GeneratedSyncVarDeserialize<string>(ref this._lobbyName, new Action<string, string>(this.SetLobbyName), reader.ReadString());
			base.GeneratedSyncVarDeserialize<LobbyPrivacyType>(ref this._lobbyPrivacyType, new Action<LobbyPrivacyType, LobbyPrivacyType>(this.SetPrivacyType), Mirror.GeneratedNetworkCode._Read_LobbyPrivacyType(reader));
			base.GeneratedSyncVarDeserialize<float>(ref this._timeScaleMin, new Action<float, float>(this.SetTimeScaleMin), reader.ReadFloat());
			base.GeneratedSyncVarDeserialize<AllowedMovesetTypes>(ref this._allowedMovesetTypes, new Action<AllowedMovesetTypes, AllowedMovesetTypes>(this.SetAllowedMoveSetTypes), Mirror.GeneratedNetworkCode._Read_AllowedMovesetTypes(reader));
			base.GeneratedSyncVarDeserialize<GameTypes>(ref this._gameType, new Action<GameTypes, GameTypes>(this.SetGameType), Mirror.GeneratedNetworkCode._Read_GameTypes(reader));
			base.GeneratedSyncVarDeserialize<bool>(ref this._allowEquipmentEdit, new Action<bool, bool>(this.SetAllowEquipmentEdit), reader.ReadBool());
			base.GeneratedSyncVarDeserialize<bool>(ref this._useStamina, new Action<bool, bool>(this.SetUseStamina), reader.ReadBool());
			base.GeneratedSyncVarDeserialize<bool>(ref this._useDismemberment, new Action<bool, bool>(this.SetUseDismemberment), reader.ReadBool());
			base.GeneratedSyncVarDeserialize<string>(ref this._welcomeMessage, new Action<string, string>(this.SetWelcomeMessage), reader.ReadString());
			base.GeneratedSyncVarDeserialize<List<EquipmentType>>(ref this._disabledEquipmentTypes, new Action<List<EquipmentType>, List<EquipmentType>>(this.DisabledEquipmentTypesChanged), Mirror.GeneratedNetworkCode._Read_System.Collections.Generic.List`1<MoveClasses.EquipmentType>(reader));
			base.GeneratedSyncVarDeserialize<string>(ref this.gameVersion, new Action<string, string>(this.GameVersionChanged), reader.ReadString());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 1L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<bool>(ref this.rollingFeet, null, reader.ReadBool());
		}
		if ((num & 2L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<string>(ref this._selectedMap, new Action<string, string>(this.SetSelectedMap), reader.ReadString());
		}
		if ((num & 4L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this._equipmentPoints, new Action<int, int>(this.SetEquipmentPoints), reader.ReadInt());
		}
		if ((num & 8L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<string>(ref this._lobbyName, new Action<string, string>(this.SetLobbyName), reader.ReadString());
		}
		if ((num & 16L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<LobbyPrivacyType>(ref this._lobbyPrivacyType, new Action<LobbyPrivacyType, LobbyPrivacyType>(this.SetPrivacyType), Mirror.GeneratedNetworkCode._Read_LobbyPrivacyType(reader));
		}
		if ((num & 32L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<float>(ref this._timeScaleMin, new Action<float, float>(this.SetTimeScaleMin), reader.ReadFloat());
		}
		if ((num & 64L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<AllowedMovesetTypes>(ref this._allowedMovesetTypes, new Action<AllowedMovesetTypes, AllowedMovesetTypes>(this.SetAllowedMoveSetTypes), Mirror.GeneratedNetworkCode._Read_AllowedMovesetTypes(reader));
		}
		if ((num & 128L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<GameTypes>(ref this._gameType, new Action<GameTypes, GameTypes>(this.SetGameType), Mirror.GeneratedNetworkCode._Read_GameTypes(reader));
		}
		if ((num & 256L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<bool>(ref this._allowEquipmentEdit, new Action<bool, bool>(this.SetAllowEquipmentEdit), reader.ReadBool());
		}
		if ((num & 512L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<bool>(ref this._useStamina, new Action<bool, bool>(this.SetUseStamina), reader.ReadBool());
		}
		if ((num & 1024L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<bool>(ref this._useDismemberment, new Action<bool, bool>(this.SetUseDismemberment), reader.ReadBool());
		}
		if ((num & 2048L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<string>(ref this._welcomeMessage, new Action<string, string>(this.SetWelcomeMessage), reader.ReadString());
		}
		if ((num & 4096L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<List<EquipmentType>>(ref this._disabledEquipmentTypes, new Action<List<EquipmentType>, List<EquipmentType>>(this.DisabledEquipmentTypesChanged), Mirror.GeneratedNetworkCode._Read_System.Collections.Generic.List`1<MoveClasses.EquipmentType>(reader));
		}
		if ((num & 8192L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<string>(ref this.gameVersion, new Action<string, string>(this.GameVersionChanged), reader.ReadString());
		}
	}

	// Token: 0x04000263 RID: 611
	public static GameSettingsManagerMultiplayer singleton;

	// Token: 0x04000264 RID: 612
	[SyncVar]
	private bool rollingFeet;

	// Token: 0x04000265 RID: 613
	[SyncVar(hook = "SetSelectedMap")]
	private string _selectedMap = "";

	// Token: 0x04000267 RID: 615
	[SyncVar(hook = "SetEquipmentPoints")]
	private int _equipmentPoints = 30;

	// Token: 0x04000268 RID: 616
	[SyncVar(hook = "SetLobbyName")]
	private string _lobbyName = "";

	// Token: 0x04000269 RID: 617
	[SyncVar(hook = "SetPrivacyType")]
	private LobbyPrivacyType _lobbyPrivacyType = LobbyPrivacyType.friendsOnlyLobby;

	// Token: 0x0400026A RID: 618
	[SyncVar(hook = "SetTimeScaleMin")]
	private float _timeScaleMin = 0.5f;

	// Token: 0x0400026B RID: 619
	[SyncVar(hook = "SetAllowedMoveSetTypes")]
	private AllowedMovesetTypes _allowedMovesetTypes;

	// Token: 0x0400026C RID: 620
	[SyncVar(hook = "SetGameType")]
	private GameTypes _gameType;

	// Token: 0x0400026D RID: 621
	[SyncVar(hook = "SetAllowEquipmentEdit")]
	private bool _allowEquipmentEdit = true;

	// Token: 0x0400026E RID: 622
	[SyncVar(hook = "SetUseStamina")]
	private bool _useStamina = true;

	// Token: 0x0400026F RID: 623
	[SyncVar(hook = "SetUseDismemberment")]
	private bool _useDismemberment = true;

	// Token: 0x04000270 RID: 624
	[SyncVar(hook = "SetWelcomeMessage")]
	private string _welcomeMessage = "";

	// Token: 0x04000271 RID: 625
	private float lastConnectionTimeoutTime;

	// Token: 0x04000272 RID: 626
	private float connectionTimeoutUpdateFrequency = 0.5f;

	// Token: 0x04000273 RID: 627
	[SyncVar(hook = "DisabledEquipmentTypesChanged")]
	private List<EquipmentType> _disabledEquipmentTypes = new List<EquipmentType>();

	// Token: 0x04000274 RID: 628
	private bool welcomeMessageDisplayed;

	// Token: 0x04000275 RID: 629
	[SyncVar(hook = "GameVersionChanged")]
	public string gameVersion;

	// Token: 0x04000276 RID: 630
	private bool saving;

	// Token: 0x04000277 RID: 631
	private bool loaded;
}
