using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoveClasses;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

// Token: 0x02000172 RID: 370
public class SingleplayerManager : MonoBehaviour
{
	// Token: 0x17000179 RID: 377
	// (get) Token: 0x06000BC5 RID: 3013 RVA: 0x000389FF File Offset: 0x00036BFF
	public bool PlayerWonTheCampaign
	{
		get
		{
			return this.singleplayerRun != null && this.singleplayerRun.fightIndex >= this.fightItems.Count;
		}
	}

	// Token: 0x06000BC6 RID: 3014 RVA: 0x00038A24 File Offset: 0x00036C24
	private void Awake()
	{
		this.Initialize();
	}

	// Token: 0x06000BC7 RID: 3015 RVA: 0x0000777A File Offset: 0x0000597A
	private void Start()
	{
	}

	// Token: 0x06000BC8 RID: 3016 RVA: 0x00038A2C File Offset: 0x00036C2C
	private void CreatePlayer()
	{
		this.lobbyPlayer = new LobbyLocalPlayer
		{
			playerExists = true
		};
		PlayerCanvasController playerCanvasController = UnityEngine.Object.FindObjectOfType<PlayerCanvasController>();
		if (playerCanvasController != null)
		{
			playerCanvasController.RegisterLobbyItems(this.lobbyPlayer);
		}
	}

	// Token: 0x06000BC9 RID: 3017 RVA: 0x00038A68 File Offset: 0x00036C68
	private void Initialize()
	{
		if (SingleplayerManager.singleton != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		SingleplayerManager.singleton = this;
		SceneManager.sceneLoaded += this.OnSceneLoaded;
		this.LoadSave();
		this.SetDifficulty(this.singleplayerRun.difficultyType);
		this.LoadPreviewImageGenerationsScene();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06000BCA RID: 3018 RVA: 0x00038ACD File Offset: 0x00036CCD
	public void OpenLobbySingleplayer()
	{
		SceneManagerWithParameters.LoadScene("LobbySingleplayer", null, false, false);
	}

	// Token: 0x06000BCB RID: 3019 RVA: 0x00038ADC File Offset: 0x00036CDC
	public void CheckReady()
	{
		if (this.lobbyPlayer.readyToBegin)
		{
			this.SetRunData();
			this.StartSingleplayerCampaign();
		}
	}

	// Token: 0x06000BCC RID: 3020 RVA: 0x00038AF7 File Offset: 0x00036CF7
	public void StartSingleplayerCampaign()
	{
		if (this.imagesGenerated)
		{
			this.GeneratePlayerImage();
		}
		base.StartCoroutine(this.LoadEnemyPreviewSceneOnceImagesHaveBeenCreated());
	}

	// Token: 0x06000BCD RID: 3021 RVA: 0x00038B14 File Offset: 0x00036D14
	private IEnumerator LoadEnemyPreviewSceneOnceImagesHaveBeenCreated()
	{
		while (!this.imagesGenerated)
		{
			yield return 0;
		}
		this.LoadEnemyPreviewScene();
		yield break;
	}

	// Token: 0x06000BCE RID: 3022 RVA: 0x00038B24 File Offset: 0x00036D24
	public void HandleFightResult(GameEndResultType result, bool win)
	{
		if (result == GameEndResultType.Draw)
		{
			this.forceRestart = true;
			return;
		}
		if (win)
		{
			this.singleplayerRun.roundWins++;
			if (this.singleplayerRun.roundWins > 1)
			{
				this.singleplayerRun.fightIndex++;
				this.singleplayerRun.ResetRounds();
			}
			if (this.PlayerWonTheCampaign)
			{
				this.HandleCampaignWin();
			}
			this.SaveRun();
			return;
		}
		this.singleplayerRun.roundLosses++;
		this.SaveRun();
	}

	// Token: 0x06000BCF RID: 3023 RVA: 0x00038BAD File Offset: 0x00036DAD
	private void HandleCampaignWin()
	{
		SettingsHelper.PersistentSaveGameWon(this.singleplayerRun.difficultyType);
	}

	// Token: 0x06000BD0 RID: 3024 RVA: 0x00038BBF File Offset: 0x00036DBF
	public void Retry()
	{
		this.singleplayerRun.ResetRun();
		this.SaveRun();
		this.LoadEnemyPreviewScene();
	}

	// Token: 0x06000BD1 RID: 3025 RVA: 0x00038BD8 File Offset: 0x00036DD8
	public void MoveToNextScene()
	{
		if (this.singleplayerRun.roundLosses > 1)
		{
			this.DoAbandonRun();
			return;
		}
		if (this.singleplayerRun.roundLosses != 0 || this.singleplayerRun.roundWins != 0 || this.forceRestart)
		{
			this.forceRestart = false;
			this.StartNextMatch();
			return;
		}
		this.LoadEnemyPreviewScene();
	}

	// Token: 0x06000BD2 RID: 3026 RVA: 0x00038C30 File Offset: 0x00036E30
	public void LoadEnemyPreviewScene()
	{
		SceneManagerWithParameters.LoadScene("SingleplayerEnemyPreviewScene", null, false, false);
	}

	// Token: 0x06000BD3 RID: 3027 RVA: 0x00038C40 File Offset: 0x00036E40
	public void StartNextMatch()
	{
		List<LobbyPlayer> list = new List<LobbyPlayer>();
		list.Add(this.lobbyPlayer);
		list.AddRange(this.GetOpponentsForCurrentFight());
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("lobbyPlayers", list);
		dictionary.Add("DoLocalMapInit", true);
		SceneManagerWithParameters.LoadScene(this.GetSceneForCurrentFight(), dictionary, false, false);
	}

	// Token: 0x06000BD4 RID: 3028 RVA: 0x00038C9C File Offset: 0x00036E9C
	public void OnDestroy()
	{
		if (SingleplayerManager.singleton == this)
		{
			SingleplayerManager.singleton = null;
		}
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
	}

	// Token: 0x06000BD5 RID: 3029 RVA: 0x00038CC4 File Offset: 0x00036EC4
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (mode == LoadSceneMode.Additive)
		{
			return;
		}
		if (!scene.name.ToLower().Contains("singleplayer") && !scene.name.ToLower().Contains("map_"))
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		if (scene.name == "LobbySingleplayer")
		{
			this.CreatePlayer();
			this.LoadPreviewImageGenerationsScene();
		}
		if (scene.name == "SingleplayerEnemyPreviewScene" && GeneralManager.singleton != null)
		{
			GeneralManager.singleton.RemoveLoadingScreen();
		}
		if (scene.name.ToLower().Contains("map_"))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.singleplayerHudCanvasPrefab);
		}
	}

	// Token: 0x06000BD6 RID: 3030 RVA: 0x00038D80 File Offset: 0x00036F80
	public string GetSceneForCurrentFight()
	{
		if (this.fightItems != null && this.fightItems.Count > this.singleplayerRun.fightIndex)
		{
			FightItem fightItem = this.fightItems[this.singleplayerRun.fightIndex];
			if (fightItem != null && !string.IsNullOrWhiteSpace(fightItem.scene) && SceneManagerWithParameters.IsValidScene(fightItem.scene))
			{
				return fightItem.scene;
			}
		}
		return "map_ArenaOfBlades";
	}

	// Token: 0x06000BD7 RID: 3031 RVA: 0x00038DF4 File Offset: 0x00036FF4
	public List<LobbyPlayer> GetOpponentsForCurrentFight()
	{
		List<LobbyPlayer> list = new List<LobbyPlayer>();
		if (this.fightItems != null && this.fightItems.Count > this.singleplayerRun.fightIndex)
		{
			FightItem fightItem = this.fightItems[this.singleplayerRun.fightIndex];
			if (fightItem != null && fightItem.fightOpponents != null)
			{
				int num = 0;
				foreach (FightOpponent fightOpponent in fightItem.fightOpponents)
				{
					LobbyPlayer lobbyPlayer = new LobbyPlayer();
					lobbyPlayer.ai = true;
					lobbyPlayer.SetMoveSet(SingleplayerManager.GetFightOpponentMoveset(fightOpponent));
					lobbyPlayer.SetEquipment(SingleplayerManager.GetFightOpponentEquipment(fightOpponent));
					lobbyPlayer.playerName = SingleplayerManager.GetFightOpponentName(fightItem, fightOpponent, num);
					if (fightOpponent.customAi)
					{
						lobbyPlayer.customAiObject = fightOpponent.customAi;
					}
					list.Add(lobbyPlayer);
					num++;
				}
			}
		}
		return list;
	}

	// Token: 0x06000BD8 RID: 3032 RVA: 0x00038F04 File Offset: 0x00037104
	public static string GetFightOpponentName(FightItem fightItem, FightOpponent fightOpponent, int index)
	{
		string text = "";
		if (!string.IsNullOrWhiteSpace(fightOpponent.translatedName))
		{
			text = fightOpponent.translatedName;
		}
		if (string.IsNullOrWhiteSpace(text) && fightItem.fightTitleParameters != null && fightItem.fightTitleParameters.Count > index && fightItem.fightTitleParameters.Count > index)
		{
			text = LocalizationHelpers.LocalizedText(fightItem.fightTitleParameters[index], Array.Empty<object>());
		}
		if (string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(fightItem.fightTitle))
		{
			text = LocalizationHelpers.LocalizedText(fightItem.fightTitle, Array.Empty<object>());
		}
		return text;
	}

	// Token: 0x06000BD9 RID: 3033 RVA: 0x00038F98 File Offset: 0x00037198
	public static MoveSet GetFightOpponentMoveset(FightOpponent fightOpponent)
	{
		MoveSet result = null;
		if (!string.IsNullOrEmpty(fightOpponent.defaultMovesetName))
		{
			result = MoveSetHelpers.GetMovesetByName(fightOpponent.defaultMovesetName);
		}
		return result;
	}

	// Token: 0x06000BDA RID: 3034 RVA: 0x00038FC1 File Offset: 0x000371C1
	public static List<EquippedEquipment> GetFightOpponentEquipment(FightOpponent fightOpponent)
	{
		if (fightOpponent.equippedEquipment != null && fightOpponent.equippedEquipment.Count > 0)
		{
			return fightOpponent.equippedEquipment;
		}
		return SingleplayerManager.GetFightOpponentMoveset(fightOpponent).defaultEquipment;
	}

	// Token: 0x06000BDB RID: 3035 RVA: 0x00038FEB File Offset: 0x000371EB
	public void LoadPreviewImageGenerationsScene()
	{
		SingleplayerManager.sceneLoadOperation = SceneManager.LoadSceneAsync("PreviewImageGenerationScene", LoadSceneMode.Additive);
	}

	// Token: 0x06000BDC RID: 3036 RVA: 0x00038FFD File Offset: 0x000371FD
	public void RegisterPreviewImageGenerationManager(PreviewImageGenerationManager newPreviewImageGenerationManager)
	{
		this.previewImageGenerationManager = newPreviewImageGenerationManager;
		this.GenerateDifficultyImages();
	}

	// Token: 0x06000BDD RID: 3037 RVA: 0x0003900C File Offset: 0x0003720C
	public void GenerateDifficultyImages()
	{
		this.imagesGenerated = false;
		this.previewImageGenerationManager.GenerateImagesForFightItems(this.difficultyFightItems, PreviewImageGenerationMode.Difficulty);
	}

	// Token: 0x06000BDE RID: 3038 RVA: 0x00039027 File Offset: 0x00037227
	public void GenerateEnemyImages()
	{
		this.imagesGenerated = false;
		this.previewImageGenerationManager.GenerateImagesForFightItems(this.fightItems, PreviewImageGenerationMode.Enemy);
	}

	// Token: 0x06000BDF RID: 3039 RVA: 0x00039042 File Offset: 0x00037242
	public void GeneratePlayerImage()
	{
		this.imagesGenerated = false;
		this.previewImageGenerationManager.GenerateImagesForLobbyPlayer(this.lobbyPlayer);
	}

	// Token: 0x06000BE0 RID: 3040 RVA: 0x0003905C File Offset: 0x0003725C
	public void ImagesHaveBeenGenerated(PreviewImageGenerationMode mode)
	{
		if (mode == PreviewImageGenerationMode.Difficulty)
		{
			this.UpdateSingleplayerDifficultyImages();
			this.GenerateEnemyImages();
		}
		if (mode == PreviewImageGenerationMode.Enemy)
		{
			if (!this.singleplayerRun.loadedSave)
			{
				if (GeneralManager.singleton != null)
				{
					GeneralManager.singleton.RemoveLoadingScreen();
				}
			}
			else
			{
				this.StartSingleplayerCampaign();
			}
		}
		this.imagesGenerated = true;
	}

	// Token: 0x06000BE1 RID: 3041 RVA: 0x000390B0 File Offset: 0x000372B0
	private void UpdateSingleplayerDifficultyImages()
	{
		SingleplayerGameSettingsManager singleplayerGameSettingsManager = SingleplayerGameSettingsManager.singleton;
		if (singleplayerGameSettingsManager != null)
		{
			singleplayerGameSettingsManager.UpdateDifficultyButtonsImages();
		}
	}

	// Token: 0x06000BE2 RID: 3042 RVA: 0x000390D2 File Offset: 0x000372D2
	public SinglePlayerDifficultyType GetDifficulty()
	{
		return this.singleplayerRun.difficultyType;
	}

	// Token: 0x06000BE3 RID: 3043 RVA: 0x000390DF File Offset: 0x000372DF
	public DifficultyObject GetCurrentDifficultyObject()
	{
		return this.GetDifficultyObject(this.GetDifficulty());
	}

	// Token: 0x06000BE4 RID: 3044 RVA: 0x000390F0 File Offset: 0x000372F0
	public DifficultyObject GetDifficultyObject(SinglePlayerDifficultyType difficulty)
	{
		return (from x in this.difficultyObjects
		where x.difficulty == difficulty
		select x).FirstOrDefault<DifficultyObject>();
	}

	// Token: 0x06000BE5 RID: 3045 RVA: 0x00039128 File Offset: 0x00037328
	public void SetDifficulty(SinglePlayerDifficultyType newDifficulty)
	{
		if (IGameSettingsManager.singleton != null)
		{
			IGameSettingsManager.singleton.LoadGameSettings();
		}
		this.singleplayerRun.difficultyType = newDifficulty;
		IGameSettingsManager.singleton.UseStamina = true;
		DifficultyObject difficultyObject = (from x in this.difficultyObjects
		where x.difficulty == newDifficulty
		select x).FirstOrDefault<DifficultyObject>();
		if (difficultyObject != null)
		{
			if (this.singleplayerRun.loadedSave)
			{
				IGameSettingsManager.singleton.TimeScaleMin = this.singleplayerRun.timescale;
			}
			else
			{
				IGameSettingsManager.singleton.TimeScaleMin = difficultyObject.minTimeScale;
			}
			IGameSettingsManager.singleton.EquipmentPoints = difficultyObject.maxPoints;
			IGameSettingsManager.singleton.AllowedMovesetTypes = AllowedMovesetTypes.All;
		}
		if (SingleplayerGameSettingsManager.singleton != null)
		{
			SingleplayerGameSettingsManager.singleton.UpdateDifficultyDescription();
		}
	}

	// Token: 0x06000BE6 RID: 3046 RVA: 0x000391FC File Offset: 0x000373FC
	public void SetRunData()
	{
		this.singleplayerRun.timescale = IGameSettingsManager.singleton.TimeScaleMin;
		this.singleplayerRun.moveSet = this.lobbyPlayer.GetMoveSet();
		this.singleplayerRun.equippedEquipment = this.lobbyPlayer.GetSelectedEquipment();
		this.SaveRun();
	}

	// Token: 0x06000BE7 RID: 3047 RVA: 0x00039250 File Offset: 0x00037450
	public void SaveRun()
	{
		try
		{
			string json = JsonConvert.SerializeObject(this.singleplayerRun, Formatting.None);
			if (!Generic.SaveJsonToFile(SettingsHelper.GetArcadeRunSettingsSavePath(), json))
			{
				throw new Exception("Arcade run save failed");
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
			GeneralManager.CreateAlertDialog(LocalizationHelpers.LocalizedText("txt_save_failed", Array.Empty<object>()), 1f, true);
		}
	}

	// Token: 0x06000BE8 RID: 3048 RVA: 0x000392B8 File Offset: 0x000374B8
	public void LoadSave()
	{
		this.singleplayerRun = new SingleplayerRun();
		try
		{
			SingleplayerRun singleplayerRun = JsonConvert.DeserializeObject<SingleplayerRun>(Generic.LoadJsonFromFile(SettingsHelper.GetArcadeRunSettingsSavePath()));
			if (singleplayerRun != null && singleplayerRun.roundLosses < 2)
			{
				this.singleplayerRun = singleplayerRun;
				this.singleplayerRun.loadedSave = true;
				this.CreatePlayer();
				this.lobbyPlayer.SetMoveSet(this.singleplayerRun.moveSet);
				this.lobbyPlayer.SetEquipment(this.singleplayerRun.equippedEquipment);
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
	}

	// Token: 0x06000BE9 RID: 3049 RVA: 0x0003934C File Offset: 0x0003754C
	public void DeleteSave()
	{
		Generic.DeleteFile(SettingsHelper.GetArcadeRunSettingsSavePath());
	}

	// Token: 0x06000BEA RID: 3050 RVA: 0x00039358 File Offset: 0x00037558
	public void AbandonRun()
	{
		GeneralManager.CreateConfirmDialog(LocalizationHelpers.LocalizedText("confirm_txt_abandon_run", Array.Empty<object>()), null, false).okButton.onClick.AddListener(delegate()
		{
			this.DoAbandonRun();
		});
	}

	// Token: 0x06000BEB RID: 3051 RVA: 0x0003938B File Offset: 0x0003758B
	public void DoAbandonRun()
	{
		this.DeleteSave();
		SceneManagerWithParameters.LoadScene("MainMenu", null, false, false);
	}

	// Token: 0x06000BEC RID: 3052 RVA: 0x000393A0 File Offset: 0x000375A0
	public void ForfeitRound()
	{
		GeneralManager.CreateConfirmDialog(LocalizationHelpers.LocalizedText("confirm_txt_forfeit_round", Array.Empty<object>()), null, false).okButton.onClick.AddListener(delegate()
		{
			this.DoForfeitRound();
		});
	}

	// Token: 0x06000BED RID: 3053 RVA: 0x000393D3 File Offset: 0x000375D3
	private void DoForfeitRound()
	{
		if (GameMaster.singleton != null)
		{
			GameMaster.singleton.KillPlayers();
			GameMaster.singleton.GameOver(true);
			if (GameMenu.singleton != null)
			{
				GameMenu.singleton.UpdatePauseState();
			}
		}
	}

	// Token: 0x04000851 RID: 2129
	public static SingleplayerManager singleton;

	// Token: 0x04000852 RID: 2130
	public LobbyLocalPlayer lobbyPlayer;

	// Token: 0x04000853 RID: 2131
	public Texture2D playerImage;

	// Token: 0x04000854 RID: 2132
	public SingleplayerRun singleplayerRun = new SingleplayerRun();

	// Token: 0x04000855 RID: 2133
	public List<DifficultyObject> difficultyObjects = new List<DifficultyObject>();

	// Token: 0x04000856 RID: 2134
	public List<FightItem> difficultyFightItems = new List<FightItem>();

	// Token: 0x04000857 RID: 2135
	public List<FightItem> fightItems = new List<FightItem>();

	// Token: 0x04000858 RID: 2136
	public GameObject singleplayerHudCanvasPrefab;

	// Token: 0x04000859 RID: 2137
	public bool forceRestart;

	// Token: 0x0400085A RID: 2138
	private PreviewImageGenerationManager previewImageGenerationManager;

	// Token: 0x0400085B RID: 2139
	public static AsyncOperation sceneLoadOperation;

	// Token: 0x0400085C RID: 2140
	public bool imagesGenerated;
}
