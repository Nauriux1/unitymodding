using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

// Token: 0x0200007C RID: 124
[JsonObject(MemberSerialization.OptIn)]
public class GameSettingsManager : MonoBehaviour, IGameSettingsManager
{
	// Token: 0x170000B4 RID: 180
	// (get) Token: 0x0600035C RID: 860 RVA: 0x00011613 File Offset: 0x0000F813
	// (set) Token: 0x0600035D RID: 861 RVA: 0x0001161B File Offset: 0x0000F81B
	[JsonProperty]
	public string SelectedMap { get; set; }

	// Token: 0x170000B5 RID: 181
	// (get) Token: 0x0600035E RID: 862 RVA: 0x00011624 File Offset: 0x0000F824
	// (set) Token: 0x0600035F RID: 863 RVA: 0x0001162C File Offset: 0x0000F82C
	[JsonProperty]
	public int EquipmentPoints { get; set; } = 30;

	// Token: 0x170000B6 RID: 182
	// (get) Token: 0x06000360 RID: 864 RVA: 0x00011635 File Offset: 0x0000F835
	// (set) Token: 0x06000361 RID: 865 RVA: 0x0001163D File Offset: 0x0000F83D
	[JsonProperty]
	public int AiAmount { get; set; } = 1;

	// Token: 0x170000B7 RID: 183
	// (get) Token: 0x06000362 RID: 866 RVA: 0x00011646 File Offset: 0x0000F846
	// (set) Token: 0x06000363 RID: 867 RVA: 0x0001164E File Offset: 0x0000F84E
	[JsonProperty]
	public float TimeScaleMin { get; set; } = 0.5f;

	// Token: 0x170000B8 RID: 184
	// (get) Token: 0x06000364 RID: 868 RVA: 0x00011657 File Offset: 0x0000F857
	// (set) Token: 0x06000365 RID: 869 RVA: 0x0001165F File Offset: 0x0000F85F
	public AllowedMovesetTypes AllowedMovesetTypes { get; set; }

	// Token: 0x170000B9 RID: 185
	// (get) Token: 0x06000366 RID: 870 RVA: 0x00011668 File Offset: 0x0000F868
	// (set) Token: 0x06000367 RID: 871 RVA: 0x00011670 File Offset: 0x0000F870
	[JsonProperty]
	public GameTypes GameType { get; set; } = GameTypes.Creative;

	// Token: 0x170000BA RID: 186
	// (get) Token: 0x06000368 RID: 872 RVA: 0x00011679 File Offset: 0x0000F879
	// (set) Token: 0x06000369 RID: 873 RVA: 0x00011681 File Offset: 0x0000F881
	public bool AllowEquipmentEdit { get; set; } = true;

	// Token: 0x170000BB RID: 187
	// (get) Token: 0x0600036A RID: 874 RVA: 0x0001168A File Offset: 0x0000F88A
	// (set) Token: 0x0600036B RID: 875 RVA: 0x000116A5 File Offset: 0x0000F8A5
	[JsonProperty]
	public bool UseStamina
	{
		get
		{
			return (this.saving || this.GameType != GameTypes.Legacy) && this._useStamina;
		}
		set
		{
			this._useStamina = value;
		}
	}

	// Token: 0x170000BC RID: 188
	// (get) Token: 0x0600036C RID: 876 RVA: 0x000116AE File Offset: 0x0000F8AE
	// (set) Token: 0x0600036D RID: 877 RVA: 0x000116B6 File Offset: 0x0000F8B6
	[JsonProperty]
	public bool UseDismemberment
	{
		get
		{
			return this._useDismemberment;
		}
		set
		{
			this._useDismemberment = value;
		}
	}

	// Token: 0x170000BD RID: 189
	// (get) Token: 0x0600036E RID: 878 RVA: 0x000116BF File Offset: 0x0000F8BF
	// (set) Token: 0x0600036F RID: 879 RVA: 0x000116C7 File Offset: 0x0000F8C7
	public string WelcomeMessage { get; set; } = "";

	// Token: 0x170000BE RID: 190
	// (get) Token: 0x06000370 RID: 880 RVA: 0x000116D0 File Offset: 0x0000F8D0
	public bool IsLocalMode
	{
		get
		{
			if (this._isLocalMode)
			{
				return true;
			}
			if (SceneManager.GetActiveScene().name == "LobbyLocalGameSettings")
			{
				this._isLocalMode = true;
				return true;
			}
			return false;
		}
	}

	// Token: 0x170000BF RID: 191
	// (get) Token: 0x06000371 RID: 881 RVA: 0x0001170C File Offset: 0x0000F90C
	public bool IsMoveEditorMode
	{
		get
		{
			if (this._isMoveEditorMode)
			{
				return true;
			}
			if (SceneManager.GetActiveScene().name == "MoveEditor")
			{
				this._isMoveEditorMode = true;
				return true;
			}
			return false;
		}
	}

	// Token: 0x170000C0 RID: 192
	// (get) Token: 0x06000372 RID: 882 RVA: 0x00011746 File Offset: 0x0000F946
	public bool IsArcadeMode
	{
		get
		{
			return SingleplayerManager.singleton != null;
		}
	}

	// Token: 0x06000373 RID: 883 RVA: 0x0000777A File Offset: 0x0000597A
	public void DisplayWelcomeMessage()
	{
	}

	// Token: 0x06000374 RID: 884 RVA: 0x00011758 File Offset: 0x0000F958
	private void Awake()
	{
		this.SelectedMap = "map_ArenaOfBlades";
		if (IGameSettingsManager.singleton == null)
		{
			IGameSettingsManager.singleton = this;
			UnityEngine.Object.DontDestroyOnLoad(this);
			SceneManager.sceneLoaded += this.OnSceneLoaded;
			if (SceneManagerWithParameters.currentScene.ToLower().Contains("moveeditor"))
			{
				this.TimeScaleMin = 1f;
				return;
			}
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000375 RID: 885 RVA: 0x000117C1 File Offset: 0x0000F9C1
	public void SetRollingFeet(bool newRollingFeetValue)
	{
		this.rollingFeet = newRollingFeetValue;
	}

	// Token: 0x06000376 RID: 886 RVA: 0x000117CA File Offset: 0x0000F9CA
	public bool GetRollingFeet()
	{
		return this.rollingFeet;
	}

	// Token: 0x06000377 RID: 887 RVA: 0x000117D2 File Offset: 0x0000F9D2
	public void DestroyGameSettingsManager()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06000378 RID: 888 RVA: 0x000117DF File Offset: 0x0000F9DF
	public void OnDestroy()
	{
		if (IGameSettingsManager.singleton == this)
		{
			this.SaveGameSettings();
		}
		if (IGameSettingsManager.singleton == this)
		{
			IGameSettingsManager.singleton = null;
		}
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
	}

	// Token: 0x06000379 RID: 889 RVA: 0x00011810 File Offset: 0x0000FA10
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (mode == LoadSceneMode.Additive)
		{
			return;
		}
		if ((!scene.name.ToLower().Contains("lobby") && !scene.name.ToLower().Contains("map_") && !scene.name.ToLower().Contains("singleplayer") && scene.name.ToLower() != "moveeditor" && scene.name.ToLower() != "moveeditortestmoveset") || scene.name.ToLower() == "lobbymoveeditor")
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0600037A RID: 890 RVA: 0x000118C0 File Offset: 0x0000FAC0
	public void SaveGameSettings()
	{
		if (this.IsLocalMode || this.IsArcadeMode || this.IsMoveEditorMode)
		{
			try
			{
				this.saving = true;
				string json = JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings
				{
					ReferenceLoopHandling = ReferenceLoopHandling.Ignore
				});
				Generic.SaveJsonToFile(this.GetSettingsSavePath(), json);
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
		}
		this.saving = false;
	}

	// Token: 0x0600037B RID: 891 RVA: 0x00011930 File Offset: 0x0000FB30
	public void LoadGameSettings()
	{
		if (this.loaded)
		{
			return;
		}
		if (this.IsLocalMode || this.IsArcadeMode || this.IsMoveEditorMode)
		{
			try
			{
				string value = Generic.LoadJsonFromFile(this.GetSettingsSavePath());
				if (!string.IsNullOrWhiteSpace(value))
				{
					JsonConvert.PopulateObject(value, this);
				}
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
		}
		this.loaded = true;
	}

	// Token: 0x0600037C RID: 892 RVA: 0x0001199C File Offset: 0x0000FB9C
	public string GetSettingsSavePath()
	{
		if (this.IsArcadeMode)
		{
			return SettingsHelper.GetArcadeGameSettingsSavePath();
		}
		if (this.IsLocalMode)
		{
			return SettingsHelper.GetLocalGameSettingsSavePath();
		}
		if (this.IsMoveEditorMode)
		{
			return SettingsHelper.GetMoveSetEditorGameSettingsSavePath();
		}
		return "";
	}

	// Token: 0x04000253 RID: 595
	private bool rollingFeet;

	// Token: 0x0400025B RID: 603
	private bool _useStamina = true;

	// Token: 0x0400025C RID: 604
	private bool _useDismemberment = true;

	// Token: 0x0400025E RID: 606
	private bool _isLocalMode;

	// Token: 0x0400025F RID: 607
	private bool _isMoveEditorMode;

	// Token: 0x04000260 RID: 608
	private bool saving;

	// Token: 0x04000261 RID: 609
	private bool loaded;
}
