using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Mirror;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Utils
{
	// Token: 0x02000282 RID: 642
	internal class SettingsHelper
	{
		// Token: 0x060012BE RID: 4798 RVA: 0x00061AAC File Offset: 0x0005FCAC
		public static void LoadAllSettings()
		{
			SettingsHelper.initiated = true;
			SettingsHelper.LoadPlayerSettings();
			SettingsHelper.LoadTutorialSettings();
			SettingsHelper.LoadPersistentSave();
			SettingsHelper.LoadBanList();
			SettingsHelper.LoadLocale();
			SettingsHelper.LoadScreenSettings();
			SettingsHelper.LoadBloodSettings();
			SettingsHelper.LoadCameraSettings();
			SettingsHelper.LoadAudioSettings();
			SettingsHelper.LoadGeneralSettings();
			SettingsHelper.LoadInputs();
			SettingsHelper.LoadCommandLineArguments();
			SettingsHelper.LoadCustomTextures();
			SettingsHelper.LoadDefaultMovesetSettings();
			if (GeneralManager.singleton != null)
			{
				GeneralManager.singleton.UpdateBloodColor();
			}
			MoveSetHelpers.ClearLoadedMoveSets();
			MoveSetHelpers.GetDefaultMoveSets(true);
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x00061B28 File Offset: 0x0005FD28
		public static bool GetFirstLoad()
		{
			if (SettingsHelper.tutorialSettings.firstLoad)
			{
				SettingsHelper.tutorialSettings.firstLoad = false;
				SettingsHelper.SaveTutorialSettings();
				return !PlayerPrefs.HasKey("FirstLoad");
			}
			return false;
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x00061B57 File Offset: 0x0005FD57
		public static bool GetFirstMoveEditorLoad()
		{
			if (SettingsHelper.tutorialSettings.firstMoveEditorLoad)
			{
				SettingsHelper.tutorialSettings.firstMoveEditorLoad = false;
				SettingsHelper.SaveTutorialSettings();
				return true;
			}
			return false;
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x00061B78 File Offset: 0x0005FD78
		public static void PersistentSaveGameWon(SinglePlayerDifficultyType difficultyType)
		{
			SettingsHelper.persistentSave.wins++;
			if (difficultyType > SettingsHelper.persistentSave.hardestWin)
			{
				SettingsHelper.persistentSave.hardestWin = difficultyType;
			}
			SettingsHelper.SavePersistentSave();
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x00061BAC File Offset: 0x0005FDAC
		public static void LoadScreenSettings()
		{
			QualitySettings.vSyncCount = PlayerPrefs.GetInt("VSync", QualitySettings.vSyncCount);
			int targetFrameRate = PlayerPrefs.GetInt("FpsLimit", 200);
			if (QualitySettings.vSyncCount > 0)
			{
				targetFrameRate = -1;
			}
			Application.targetFrameRate = targetFrameRate;
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)GraphicsSettings.renderPipelineAsset;
			if (universalRenderPipelineAsset != null)
			{
				universalRenderPipelineAsset.msaaSampleCount = PlayerPrefs.GetInt("MSAA", 1);
				universalRenderPipelineAsset.GetType().GetField("m_MainLightShadowmapResolution", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(GraphicsSettings.currentRenderPipeline, SettingsHelper.GetMainLightShadowResolution());
			}
			foreach (CameraSettings cameraSettings in UnityEngine.Object.FindObjectsOfType<CameraSettings>().ToList<CameraSettings>())
			{
				cameraSettings.LoadSettings();
			}
			PostProcessingManager.LoadPostProcessingSettings();
			PostProcessingManagerAdditional.LoadPostProcessingSettings();
		}

		// Token: 0x060012C3 RID: 4803 RVA: 0x00061C8C File Offset: 0x0005FE8C
		public static void LoadAudioSettings()
		{
			Debug.Log("Load audio settings");
			if (MusicManager.singleton != null)
			{
				MusicManager.singleton.SetMusicVolume();
				MusicManager.singleton.SetMasterVolume();
				MusicManager.singleton.SetEffectsVolume();
				MusicManager.singleton.SetVoiceChatVolume();
			}
		}

		// Token: 0x060012C4 RID: 4804 RVA: 0x00061CD8 File Offset: 0x0005FED8
		public static void LoadGeneralSettings()
		{
			Debug.Log("Load general settings");
			if (GeneralManager.singleton != null)
			{
				GeneralManager.singleton.showFPS = SettingsHelper.GetShowFPS();
			}
			foreach (HudCanvas hudCanvas in UnityEngine.Object.FindObjectsOfType<HudCanvas>().ToList<HudCanvas>())
			{
				hudCanvas.LoadSettings();
			}
		}

		// Token: 0x060012C5 RID: 4805 RVA: 0x00061D54 File Offset: 0x0005FF54
		public static void LoadBloodSettings()
		{
			ParticleDisplayer particleDisplayer = UnityEngine.Object.FindObjectOfType<ParticleDisplayer>();
			if (particleDisplayer != null)
			{
				particleDisplayer.LoadSettings(false);
			}
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x00061D78 File Offset: 0x0005FF78
		public static void LoadCameraSettings()
		{
			PlayerCameraSettings cameraSettings = SettingsHelper.GetCameraSettings();
			foreach (CameraSmoothFollow cameraSmoothFollow in UnityEngine.Object.FindObjectsOfType<CameraSmoothFollow>().ToList<CameraSmoothFollow>())
			{
				cameraSmoothFollow.SetCameraSettings(cameraSettings, false, null);
			}
		}

		// Token: 0x060012C7 RID: 4807 RVA: 0x00061DD8 File Offset: 0x0005FFD8
		public static void LoadDefaultMovesetSettings()
		{
			DefaultMovesetSettings defaultMovesetSettings = SettingsHelper.GetDefaultMovesetSettings();
			if (NetworkClient.active)
			{
				using (List<MultiplayerRoomPlayer>.Enumerator enumerator = UnityEngine.Object.FindObjectsOfType<MultiplayerRoomPlayer>().ToList<MultiplayerRoomPlayer>().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MultiplayerRoomPlayer multiplayerRoomPlayer = enumerator.Current;
						multiplayerRoomPlayer.ClientSendUserDefaultMovesetSettings();
					}
					return;
				}
			}
			foreach (PlayerAnimator playerAnimator in UnityEngine.Object.FindObjectsOfType<PlayerAnimator>().ToList<PlayerAnimator>())
			{
				playerAnimator.SetBasicMoveSetBindings(defaultMovesetSettings);
			}
		}

		// Token: 0x060012C8 RID: 4808 RVA: 0x00061E80 File Offset: 0x00060080
		public static PlayerCameraSettings GetCameraSettings()
		{
			return SettingsHelper.playerSettings.playerCameraSettings;
		}

		// Token: 0x060012C9 RID: 4809 RVA: 0x00061E8C File Offset: 0x0006008C
		public static void SetCameraSettings(PlayerCameraSettings cameraSettings)
		{
			SettingsHelper.playerSettings.playerCameraSettings = cameraSettings;
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x00061E99 File Offset: 0x00060099
		public static void ResetCameraSettings()
		{
			SettingsHelper.playerSettings.playerCameraSettings = new PlayerCameraSettings();
			SettingsHelper.SavePlayerSettings();
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x00061EAF File Offset: 0x000600AF
		public static bool GetShowFPS()
		{
			return SettingsHelper.playerSettings.playerGenericSettings.showFPS;
		}

		// Token: 0x060012CC RID: 4812 RVA: 0x00061EC0 File Offset: 0x000600C0
		public static void SetShowFPS(bool value)
		{
			SettingsHelper.playerSettings.playerGenericSettings.showFPS = value;
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x00061ED2 File Offset: 0x000600D2
		public static bool GetRecordReplay()
		{
			return SettingsHelper.playerSettings.playerGenericSettings.recordReplay;
		}

		// Token: 0x060012CE RID: 4814 RVA: 0x00061EE3 File Offset: 0x000600E3
		public static void SetRecordReplay(bool value)
		{
			SettingsHelper.playerSettings.playerGenericSettings.recordReplay = value;
		}

		// Token: 0x060012CF RID: 4815 RVA: 0x00061EF8 File Offset: 0x000600F8
		public static void LoadLocale()
		{
			string language = SettingsHelper.GetLocale();
			if (SettingsHelper.AvailableLocales.Contains(language))
			{
				Locale locale = (from x in LocalizationSettings.AvailableLocales.Locales
				where x.Formatter.ToString() == language
				select x).FirstOrDefault<Locale>();
				if (locale == null)
				{
					locale = (from x in LocalizationSettings.AvailableLocales.Locales
					where x.Formatter.ToString() == "en"
					select x).FirstOrDefault<Locale>();
				}
				PlayerPrefs.SetString("selected-locale", language);
				if (locale != null)
				{
					LocalizationSettings.SelectedLocale = locale;
				}
			}
		}

		// Token: 0x060012D0 RID: 4816 RVA: 0x00061FA8 File Offset: 0x000601A8
		public static string GetLocale()
		{
			if (string.IsNullOrEmpty(SettingsHelper.playerSettings.playerGenericSettings.locale) && SteamManager.steamManager != null)
			{
				SettingsHelper.playerSettings.playerGenericSettings.locale = SteamManager.steamManager.GetLocale();
			}
			return SettingsHelper.playerSettings.playerGenericSettings.locale;
		}

		// Token: 0x060012D1 RID: 4817 RVA: 0x00062000 File Offset: 0x00060200
		public static void SetLocale(string value)
		{
			SettingsHelper.playerSettings.playerGenericSettings.locale = value;
		}

		// Token: 0x060012D2 RID: 4818 RVA: 0x00062012 File Offset: 0x00060212
		public static string GetPlayerName()
		{
			return ValidationHelpers.ValidatePlayerNameLength(PlayerPrefs.GetString("UserName", ""));
		}

		// Token: 0x060012D3 RID: 4819 RVA: 0x00062028 File Offset: 0x00060228
		public static bool GetBloom()
		{
			return PlayerPrefs.GetInt("Bloom", 0) == 1;
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x00062038 File Offset: 0x00060238
		public static bool GetDepthOfField()
		{
			return PlayerPrefs.GetInt("DepthOfField", 0) == 1;
		}

		// Token: 0x060012D5 RID: 4821 RVA: 0x00062048 File Offset: 0x00060248
		public static float GetMouseSensitivity()
		{
			return (float)Math.Round((double)SettingsHelper.playerSettings.playerGenericSettings.mouseFreeLookSensitivity, 2);
		}

		// Token: 0x060012D6 RID: 4822 RVA: 0x00062061 File Offset: 0x00060261
		public static void SetMouseSensitivity(float value)
		{
			SettingsHelper.playerSettings.playerGenericSettings.mouseFreeLookSensitivity = value;
		}

		// Token: 0x060012D7 RID: 4823 RVA: 0x00062073 File Offset: 0x00060273
		public static bool GetDisableMouseTurning()
		{
			return SettingsHelper.playerSettings.playerGenericSettings.disableMouseTurning;
		}

		// Token: 0x060012D8 RID: 4824 RVA: 0x00062084 File Offset: 0x00060284
		public static void SetDisableMouseTurning(bool value)
		{
			SettingsHelper.playerSettings.playerGenericSettings.disableMouseTurning = value;
		}

		// Token: 0x060012D9 RID: 4825 RVA: 0x00062096 File Offset: 0x00060296
		public static float GetControllerSensitivity()
		{
			return (float)Math.Round((double)SettingsHelper.playerSettings.playerGenericSettings.controllerFreeLookSensitivity, 2);
		}

		// Token: 0x060012DA RID: 4826 RVA: 0x000620AF File Offset: 0x000602AF
		public static void SetControllerSensitivity(float value)
		{
			SettingsHelper.playerSettings.playerGenericSettings.controllerFreeLookSensitivity = value;
		}

		// Token: 0x060012DB RID: 4827 RVA: 0x000620C1 File Offset: 0x000602C1
		public static bool GetInvertCameraY()
		{
			return SettingsHelper.playerSettings.playerGenericSettings.invertCameraY;
		}

		// Token: 0x060012DC RID: 4828 RVA: 0x000620D2 File Offset: 0x000602D2
		public static void SetInvertCameraY(bool value)
		{
			SettingsHelper.playerSettings.playerGenericSettings.invertCameraY = value;
		}

		// Token: 0x060012DD RID: 4829 RVA: 0x000620E4 File Offset: 0x000602E4
		public static bool GetTimeScaleAffactsCameraTurnSpeed()
		{
			return SettingsHelper.playerSettings.playerGenericSettings.timeScaleAffectCameraTurnSpeed;
		}

		// Token: 0x060012DE RID: 4830 RVA: 0x000620F5 File Offset: 0x000602F5
		public static void SetTimeScaleAffactsCameraTurnSpeed(bool value)
		{
			SettingsHelper.playerSettings.playerGenericSettings.timeScaleAffectCameraTurnSpeed = value;
		}

		// Token: 0x060012DF RID: 4831 RVA: 0x00062107 File Offset: 0x00060307
		public static bool GetShowAttackDirection()
		{
			return SettingsHelper.playerSettings.playerGenericSettings.showAttackDirection;
		}

		// Token: 0x060012E0 RID: 4832 RVA: 0x00062118 File Offset: 0x00060318
		public static void SetShowAttackDirection(bool value)
		{
			SettingsHelper.playerSettings.playerGenericSettings.showAttackDirection = value;
		}

		// Token: 0x060012E1 RID: 4833 RVA: 0x0006212A File Offset: 0x0006032A
		public static PlayerTurnType GetPlayerTurnType()
		{
			return SettingsHelper.playerSettings.playerGenericSettings.playerTurnType;
		}

		// Token: 0x060012E2 RID: 4834 RVA: 0x0006213B File Offset: 0x0006033B
		public static void SetPlayerTurnType(PlayerTurnType value)
		{
			SettingsHelper.playerSettings.playerGenericSettings.playerTurnType = value;
		}

		// Token: 0x060012E3 RID: 4835 RVA: 0x0006214D File Offset: 0x0006034D
		public static ReplayTexturesOverrideType GetReplayTexturesOverrideType()
		{
			return SettingsHelper.playerSettings.playerGenericSettings.replayTexturesOverrideType;
		}

		// Token: 0x060012E4 RID: 4836 RVA: 0x0006215E File Offset: 0x0006035E
		public static void SetReplayTexturesOverrideType(ReplayTexturesOverrideType value)
		{
			SettingsHelper.playerSettings.playerGenericSettings.replayTexturesOverrideType = value;
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x00062170 File Offset: 0x00060370
		public static BloodColourType GetBloodColourType()
		{
			return SettingsHelper.playerSettings.playerGenericSettings.bloodColourType;
		}

		// Token: 0x060012E6 RID: 4838 RVA: 0x00062181 File Offset: 0x00060381
		public static void SetBloodColourType(BloodColourType value)
		{
			SettingsHelper.playerSettings.playerGenericSettings.bloodColourType = value;
		}

		// Token: 0x060012E7 RID: 4839 RVA: 0x00062193 File Offset: 0x00060393
		public static ChatOption GetChatOption()
		{
			return SettingsHelper.playerSettings.playerMultiplayerSettings.chat;
		}

		// Token: 0x060012E8 RID: 4840 RVA: 0x000621A4 File Offset: 0x000603A4
		public static void SetChatOption(ChatOption value)
		{
			SettingsHelper.playerSettings.playerMultiplayerSettings.chat = value;
		}

		// Token: 0x060012E9 RID: 4841 RVA: 0x000621B6 File Offset: 0x000603B6
		public static AllowCustomTextureOptionsType GetAllowCustomPlayerTextures()
		{
			return SettingsHelper.playerSettings.playerGenericSettings.allowCustomPlayerTextures;
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x000621C7 File Offset: 0x000603C7
		public static void SetAllowCustomPlayerTextures(AllowCustomTextureOptionsType value)
		{
			SettingsHelper.playerSettings.playerGenericSettings.allowCustomPlayerTextures = value;
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x000621D9 File Offset: 0x000603D9
		public static DefaultMovesetSettings GetDefaultMovesetSettings()
		{
			return SettingsHelper.playerSettings.defaultMovesetSettings;
		}

		// Token: 0x060012EC RID: 4844 RVA: 0x000621E5 File Offset: 0x000603E5
		public static void SetDefaultMovesetSettings(DefaultMovesetSettings defaultMovesetSettings)
		{
			SettingsHelper.playerSettings.defaultMovesetSettings = defaultMovesetSettings;
		}

		// Token: 0x060012ED RID: 4845 RVA: 0x000621F4 File Offset: 0x000603F4
		public static float GetMasterVolume()
		{
			float num = SettingsHelper.playerSettings.playerAudioSettings.masterVolume;
			if (num < 0.0001f)
			{
				num = 0.0001f;
			}
			return Mathf.Log10(num) * 20f;
		}

		// Token: 0x060012EE RID: 4846 RVA: 0x0006222C File Offset: 0x0006042C
		public static float GetEffectsVolume()
		{
			float num = SettingsHelper.playerSettings.playerAudioSettings.effectsVolume;
			if (num < 0.0001f)
			{
				num = 0.0001f;
			}
			return Mathf.Log10(num) * 20f;
		}

		// Token: 0x060012EF RID: 4847 RVA: 0x00062264 File Offset: 0x00060464
		public static float GetMusicVolume()
		{
			float num = SettingsHelper.playerSettings.playerAudioSettings.musicVolume;
			if (num < 0.0001f)
			{
				num = 0.0001f;
			}
			return Mathf.Log10(num) * 20f;
		}

		// Token: 0x060012F0 RID: 4848 RVA: 0x0006229C File Offset: 0x0006049C
		public static float GetVoiceChatVolume()
		{
			float num = SettingsHelper.playerSettings.playerAudioSettings.voiceChatVolume;
			if (num < 0.0001f)
			{
				num = 0.0001f;
			}
			if (VoiceChatManager.singleton != null && VoiceChatManager.singleton.dissonanceComms != null)
			{
				if (num < 0.0002f)
				{
					VoiceChatManager.singleton.dissonanceComms.IsDeafened = true;
				}
				else
				{
					VoiceChatManager.singleton.dissonanceComms.IsDeafened = false;
				}
			}
			return Mathf.Log10(num) * 20f;
		}

		// Token: 0x060012F1 RID: 4849 RVA: 0x0006231C File Offset: 0x0006051C
		public static void SetMasterVolume(float volume)
		{
			SettingsHelper.playerSettings.playerAudioSettings.masterVolume = volume;
		}

		// Token: 0x060012F2 RID: 4850 RVA: 0x0006232E File Offset: 0x0006052E
		public static void SetEffectsVolume(float volume)
		{
			SettingsHelper.playerSettings.playerAudioSettings.effectsVolume = volume;
		}

		// Token: 0x060012F3 RID: 4851 RVA: 0x00062340 File Offset: 0x00060540
		public static void SetMusicVolume(float volume)
		{
			SettingsHelper.playerSettings.playerAudioSettings.musicVolume = volume;
		}

		// Token: 0x060012F4 RID: 4852 RVA: 0x00062352 File Offset: 0x00060552
		public static void SetVoiceChatVolume(float volume)
		{
			SettingsHelper.playerSettings.playerAudioSettings.voiceChatVolume = volume;
		}

		// Token: 0x060012F5 RID: 4853 RVA: 0x00062364 File Offset: 0x00060564
		public static Resolution GetUserResolution()
		{
			Resolution result = default(Resolution);
			int @int = PlayerPrefs.GetInt("ResolutionWidth", Display.main.systemWidth);
			int int2 = PlayerPrefs.GetInt("ResolutionHeight", Display.main.systemHeight);
			int int3 = PlayerPrefs.GetInt("RefreshRate", Screen.currentResolution.refreshRate);
			result.width = @int;
			result.height = int2;
			result.refreshRate = int3;
			return result;
		}

		// Token: 0x060012F6 RID: 4854 RVA: 0x000623D8 File Offset: 0x000605D8
		public static int GetMainLightShadowResolution()
		{
			int result = 256;
			switch (SettingsHelper.GetShadowQuality())
			{
			case 0:
				result = 256;
				break;
			case 1:
				result = 512;
				break;
			case 2:
				result = 1024;
				break;
			case 3:
				result = 2048;
				break;
			case 4:
				result = 4096;
				break;
			}
			return result;
		}

		// Token: 0x060012F7 RID: 4855 RVA: 0x00062434 File Offset: 0x00060634
		public static int GetShadowQuality()
		{
			return PlayerPrefs.GetInt("ShadowQuality", 4);
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x00062444 File Offset: 0x00060644
		public static int GetBloodParticleDisplayerMaxCount()
		{
			int result = 500;
			switch (SettingsHelper.GetBloodQuality())
			{
			case 0:
				result = 500;
				break;
			case 1:
				result = 3500;
				break;
			case 2:
				result = 7000;
				break;
			case 3:
				result = 10000;
				break;
			case 4:
				result = 15000;
				break;
			}
			return result;
		}

		// Token: 0x060012F9 RID: 4857 RVA: 0x000624A0 File Offset: 0x000606A0
		public static int GetBloodQuality()
		{
			return PlayerPrefs.GetInt("BloodQuality", 3);
		}

		// Token: 0x060012FA RID: 4858 RVA: 0x000624B0 File Offset: 0x000606B0
		public static void LoadCustomTextures()
		{
			try
			{
				SettingsHelper.SetupSaveDestination(SettingsHelper.GetTextureSavePath());
				string customPlayerTextureSavePath = SettingsHelper.GetCustomPlayerTextureSavePath();
				SettingsHelper.TryToLoadPngCustomTexture();
				SettingsHelper.customPlayerTexture = null;
				SettingsHelper.TryToLoadPngCustomTexture();
				if (File.Exists(customPlayerTextureSavePath))
				{
					SettingsHelper.customTextureBytes = File.ReadAllBytes(customPlayerTextureSavePath);
					Texture2D tex = new Texture2D(2, 2);
					tex.LoadImage(SettingsHelper.customTextureBytes);
					SettingsHelper.customPlayerTexture = tex;
				}
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
			SettingsHelper.ValidateLoadedTexture();
		}

		// Token: 0x060012FB RID: 4859 RVA: 0x00062528 File Offset: 0x00060728
		public static void TryToLoadPngCustomTexture()
		{
			if (!File.Exists(SettingsHelper.GetCustomPlayerTextureSavePath()))
			{
				string customPlayerTextureSavePathOld = SettingsHelper.GetCustomPlayerTextureSavePathOld();
				if (File.Exists(customPlayerTextureSavePathOld))
				{
					SettingsHelper.SaveCustomPlayerTexture(customPlayerTextureSavePathOld);
					string text = customPlayerTextureSavePathOld.Replace(".png", "_old_copy.png");
					File.Delete(text);
					File.Move(customPlayerTextureSavePathOld, text);
				}
			}
		}

		// Token: 0x060012FC RID: 4860 RVA: 0x00062574 File Offset: 0x00060774
		public static void ValidateLoadedTexture()
		{
			if (SettingsHelper.customPlayerTexture != null && SettingsHelper.customTextureBytes != null && !ValidationHelpers.ValidateTexture(SettingsHelper.customPlayerTexture, SettingsHelper.customTextureBytes))
			{
				SettingsHelper.customTextureBytes = null;
				SettingsHelper.customPlayerTexture = null;
				Debug.Log("Custom player texture is too big and will not be used.");
				GeneralManager.DisplayInfoMessage(LocalizationHelpers.LocalizedText("alert_custom_player_texture_failed_load", new object[]
				{
					SettingsHelper.customPlayerTextureMaxBytes / 1000,
					SettingsHelper.customPlayerTextureMaxWidthHeight
				}), 6f);
			}
		}

		// Token: 0x060012FD RID: 4861 RVA: 0x000625F8 File Offset: 0x000607F8
		public static bool SaveCustomPlayerTexture(string path)
		{
			try
			{
				Texture2D texture2D = Generic.GetImageFromPath(path);
				if (texture2D != null)
				{
					texture2D = Generic.ResizeTexture2D(texture2D, 1024);
					byte[] bytes = Generic.Texture2DToJpgEncodedByteArray(texture2D);
					File.WriteAllBytes(SettingsHelper.GetCustomPlayerTextureSavePath(), bytes);
				}
				else
				{
					SettingsHelper.DeleteFile(SettingsHelper.GetCustomPlayerTextureSavePath());
				}
				return true;
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
			return false;
		}

		// Token: 0x060012FE RID: 4862 RVA: 0x00062660 File Offset: 0x00060860
		public static void DeleteFile(string path)
		{
			try
			{
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
		}

		// Token: 0x060012FF RID: 4863 RVA: 0x00062694 File Offset: 0x00060894
		public static Texture2D GetCustomPlayerTexture()
		{
			if (SettingsHelper.customPlayerTexture != null)
			{
				return SettingsHelper.customPlayerTexture;
			}
			return null;
		}

		// Token: 0x06001300 RID: 4864 RVA: 0x000626AA File Offset: 0x000608AA
		public static byte[] GetCustomPlayerTextureBytes()
		{
			if (SettingsHelper.customTextureBytes != null && SettingsHelper.customTextureBytes.Length != 0)
			{
				return SettingsHelper.customTextureBytes;
			}
			return null;
		}

		// Token: 0x06001301 RID: 4865 RVA: 0x000626C4 File Offset: 0x000608C4
		public static void LoadInputs()
		{
			foreach (global::PlayerInputManager playerInputManager in UnityEngine.Object.FindObjectsOfType<global::PlayerInputManager>().ToList<global::PlayerInputManager>())
			{
				playerInputManager.LoadUserControl();
				playerInputManager.LoadSettings();
			}
			foreach (PlayerMultiplayerInputManager playerMultiplayerInputManager in UnityEngine.Object.FindObjectsOfType<PlayerMultiplayerInputManager>().ToList<PlayerMultiplayerInputManager>())
			{
				playerMultiplayerInputManager.LoadUserControl();
				playerMultiplayerInputManager.LoadSettings();
			}
			if (GeneralManager.singleton != null)
			{
				GeneralManager.singleton.LoadUserControl();
			}
			if (ReplayManager.singleton != null)
			{
				ReplayManager.singleton.SetupUserControls();
			}
			if (ReplayCameraControls.singleton != null)
			{
				ReplayCameraControls.singleton.SetupUserControls();
			}
			foreach (ReplayFreeCamera replayFreeCamera in UnityEngine.Object.FindObjectsOfType<ReplayFreeCamera>().ToList<ReplayFreeCamera>())
			{
				replayFreeCamera.SetupUserControls(false);
			}
		}

		// Token: 0x06001302 RID: 4866 RVA: 0x000627F0 File Offset: 0x000609F0
		public static UserControls GetUserControls()
		{
			UserControls userControls = new UserControls();
			try
			{
				string value = Generic.LoadJsonFromFile(SettingsHelper.GetKeyBindingsSavePath());
				if (!string.IsNullOrEmpty(value))
				{
					CustomInputs customInputs = JsonConvert.DeserializeObject<CustomInputs>(value);
					foreach (PropertyInfo propertyInfo in userControls.PlayerActionMap.GetType().GetProperties())
					{
						if (propertyInfo.PropertyType == typeof(InputAction))
						{
							InputAction inputAction = (InputAction)propertyInfo.GetValue(userControls.PlayerActionMap);
							CustomInputActionBind customInputActionBind = (from x in customInputs.customInputActions
							where x.inputActionName == inputAction.name
							select x).FirstOrDefault<CustomInputActionBind>();
							if (customInputActionBind != null)
							{
								for (int j = inputAction.bindings.Count; j > 0; j--)
								{
									inputAction.ChangeBinding(j - 1).Erase();
								}
								foreach (CustomKeyBind customKeyBind in customInputActionBind.customKeyBind)
								{
									inputAction.AddBinding(customKeyBind.bindingPath, null, customKeyBind.processors, null);
								}
							}
						}
					}
					foreach (PropertyInfo propertyInfo2 in userControls.General.GetType().GetProperties())
					{
						if (propertyInfo2.PropertyType == typeof(InputAction))
						{
							InputAction inputAction = (InputAction)propertyInfo2.GetValue(userControls.General);
							CustomInputActionBind customInputActionBind2 = (from x in customInputs.customInputActions
							where x.inputActionName == inputAction.name
							select x).FirstOrDefault<CustomInputActionBind>();
							if (customInputActionBind2 != null)
							{
								for (int k = inputAction.bindings.Count; k > 0; k--)
								{
									inputAction.ChangeBinding(k - 1).Erase();
								}
								foreach (CustomKeyBind customKeyBind2 in customInputActionBind2.customKeyBind)
								{
									inputAction.AddBinding(customKeyBind2.bindingPath, null, customKeyBind2.processors, null);
								}
							}
						}
					}
					foreach (PropertyInfo propertyInfo3 in userControls.ReplayMap.GetType().GetProperties())
					{
						if (propertyInfo3.PropertyType == typeof(InputAction))
						{
							InputAction inputAction = (InputAction)propertyInfo3.GetValue(userControls.ReplayMap);
							CustomInputActionBind customInputActionBind3 = (from x in customInputs.customInputActions
							where x.inputActionName == inputAction.name
							select x).FirstOrDefault<CustomInputActionBind>();
							if (customInputActionBind3 != null)
							{
								for (int l = inputAction.bindings.Count; l > 0; l--)
								{
									inputAction.ChangeBinding(l - 1).Erase();
								}
								foreach (CustomKeyBind customKeyBind3 in customInputActionBind3.customKeyBind)
								{
									inputAction.AddBinding(customKeyBind3.bindingPath, null, customKeyBind3.processors, null);
								}
							}
						}
					}
					foreach (PropertyInfo propertyInfo4 in userControls.MoveEditorMap.GetType().GetProperties())
					{
						if (propertyInfo4.PropertyType == typeof(InputAction))
						{
							InputAction inputAction = (InputAction)propertyInfo4.GetValue(userControls.MoveEditorMap);
							CustomInputActionBind customInputActionBind4 = (from x in customInputs.customInputActions
							where x.inputActionName == inputAction.name
							select x).FirstOrDefault<CustomInputActionBind>();
							if (customInputActionBind4 != null && !SettingsHelper.skippableMoveEditorActions.Contains(inputAction.name))
							{
								for (int m = inputAction.bindings.Count; m > 0; m--)
								{
									inputAction.ChangeBinding(m - 1).Erase();
								}
								foreach (CustomKeyBind customKeyBind4 in customInputActionBind4.customKeyBind)
								{
									inputAction.AddBinding(customKeyBind4.bindingPath, null, customKeyBind4.processors, null);
								}
							}
						}
					}
				}
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
			return userControls;
		}

		// Token: 0x06001303 RID: 4867 RVA: 0x00062D5C File Offset: 0x00060F5C
		public static void SaveUserControls(UserControls userControls)
		{
			string json = "";
			if (userControls != null)
			{
				CustomInputs customInputs = new CustomInputs();
				customInputs.customInputActions = new List<CustomInputActionBind>();
				foreach (PropertyInfo propertyInfo in userControls.PlayerActionMap.GetType().GetProperties())
				{
					if (propertyInfo.PropertyType == typeof(InputAction))
					{
						InputAction inputAction = (InputAction)propertyInfo.GetValue(userControls.PlayerActionMap);
						CustomInputActionBind customInputActionBind = new CustomInputActionBind();
						customInputActionBind.customKeyBind = new List<CustomKeyBind>();
						customInputActionBind.inputActionName = inputAction.name;
						customInputs.customInputActions.Add(customInputActionBind);
						int num = 0;
						foreach (InputBinding inputBinding in inputAction.bindings)
						{
							CustomKeyBind customKeyBind = new CustomKeyBind();
							if (!string.IsNullOrEmpty(inputBinding.overridePath))
							{
								customKeyBind.bindingPath = inputBinding.overridePath;
							}
							else
							{
								customKeyBind.bindingPath = inputBinding.path;
							}
							if (customKeyBind.bindingPath.ToLower().Contains("stick/"))
							{
								customKeyBind.processors = "AxisDeadzone";
							}
							customInputActionBind.customKeyBind.Add(customKeyBind);
							num++;
						}
					}
				}
				foreach (PropertyInfo propertyInfo2 in userControls.General.GetType().GetProperties())
				{
					if (propertyInfo2.PropertyType == typeof(InputAction))
					{
						InputAction inputAction2 = (InputAction)propertyInfo2.GetValue(userControls.General);
						CustomInputActionBind customInputActionBind2 = new CustomInputActionBind();
						customInputActionBind2.customKeyBind = new List<CustomKeyBind>();
						customInputActionBind2.inputActionName = inputAction2.name;
						customInputs.customInputActions.Add(customInputActionBind2);
						try
						{
							foreach (InputBinding inputBinding2 in inputAction2.bindings)
							{
								CustomKeyBind customKeyBind2 = new CustomKeyBind();
								if (!string.IsNullOrEmpty(inputBinding2.overridePath))
								{
									customKeyBind2.bindingPath = inputBinding2.overridePath;
								}
								else
								{
									customKeyBind2.bindingPath = inputBinding2.path;
								}
								if (customKeyBind2.bindingPath.ToLower().Contains("stick/"))
								{
									customKeyBind2.processors = "AxisDeadzone";
								}
								customInputActionBind2.customKeyBind.Add(customKeyBind2);
							}
						}
						catch (Exception)
						{
						}
					}
				}
				foreach (PropertyInfo propertyInfo3 in userControls.ReplayMap.GetType().GetProperties())
				{
					if (propertyInfo3.PropertyType == typeof(InputAction))
					{
						InputAction inputAction3 = (InputAction)propertyInfo3.GetValue(userControls.ReplayMap);
						CustomInputActionBind customInputActionBind3 = new CustomInputActionBind();
						customInputActionBind3.customKeyBind = new List<CustomKeyBind>();
						customInputActionBind3.inputActionName = inputAction3.name;
						customInputs.customInputActions.Add(customInputActionBind3);
						try
						{
							foreach (InputBinding inputBinding3 in inputAction3.bindings)
							{
								CustomKeyBind customKeyBind3 = new CustomKeyBind();
								if (!string.IsNullOrEmpty(inputBinding3.overridePath))
								{
									customKeyBind3.bindingPath = inputBinding3.overridePath;
								}
								else
								{
									customKeyBind3.bindingPath = inputBinding3.path;
								}
								if (customKeyBind3.bindingPath.ToLower().Contains("stick/"))
								{
									customKeyBind3.processors = "AxisDeadzone";
								}
								customInputActionBind3.customKeyBind.Add(customKeyBind3);
							}
						}
						catch (Exception)
						{
						}
					}
				}
				foreach (PropertyInfo propertyInfo4 in userControls.MoveEditorMap.GetType().GetProperties())
				{
					if (propertyInfo4.PropertyType == typeof(InputAction))
					{
						InputAction inputAction4 = (InputAction)propertyInfo4.GetValue(userControls.MoveEditorMap);
						CustomInputActionBind customInputActionBind4 = new CustomInputActionBind();
						customInputActionBind4.customKeyBind = new List<CustomKeyBind>();
						customInputActionBind4.inputActionName = inputAction4.name;
						customInputs.customInputActions.Add(customInputActionBind4);
						if (!SettingsHelper.skippableMoveEditorActions.Contains(inputAction4.name))
						{
							try
							{
								foreach (InputBinding inputBinding4 in inputAction4.bindings)
								{
									CustomKeyBind customKeyBind4 = new CustomKeyBind();
									if (!string.IsNullOrEmpty(inputBinding4.overridePath))
									{
										customKeyBind4.bindingPath = inputBinding4.overridePath;
									}
									else
									{
										customKeyBind4.bindingPath = inputBinding4.path;
									}
									if (customKeyBind4.bindingPath.ToLower().Contains("stick/"))
									{
										customKeyBind4.processors = "AxisDeadzone";
									}
									customInputActionBind4.customKeyBind.Add(customKeyBind4);
								}
							}
							catch (Exception)
							{
							}
						}
					}
				}
				json = JsonConvert.SerializeObject(customInputs, Formatting.Indented);
			}
			Generic.SaveJsonToFile(SettingsHelper.GetKeyBindingsSavePath(), json);
		}

		// Token: 0x06001304 RID: 4868 RVA: 0x000632D4 File Offset: 0x000614D4
		public static void LoadPlayerSettings()
		{
			SettingsHelper.playerSettings = new PlayerSettings();
			try
			{
				PlayerSettings playerSettings = JsonConvert.DeserializeObject<PlayerSettings>(Generic.LoadJsonFromFile(SettingsHelper.GetSettingsSavePath()));
				if (playerSettings != null)
				{
					if (playerSettings.playerGenericSettings != null)
					{
						SettingsHelper.playerSettings.playerGenericSettings = playerSettings.playerGenericSettings;
					}
					if (playerSettings.playerCameraSettings != null)
					{
						Vector3 cameraPositionOffset = playerSettings.playerCameraSettings.cameraPositionOffset;
						SettingsHelper.playerSettings.playerCameraSettings.cameraPositionOffset = playerSettings.playerCameraSettings.cameraPositionOffset;
						Vector3 cameraTargetOffset = playerSettings.playerCameraSettings.cameraTargetOffset;
						SettingsHelper.playerSettings.playerCameraSettings.cameraTargetOffset = playerSettings.playerCameraSettings.cameraTargetOffset;
						SettingsHelper.playerSettings.playerCameraSettings.cameraFov = playerSettings.playerCameraSettings.cameraFov;
					}
					if (playerSettings.playerAudioSettings != null)
					{
						SettingsHelper.playerSettings.playerAudioSettings = playerSettings.playerAudioSettings;
					}
					if (playerSettings.playerMultiplayerSettings != null)
					{
						SettingsHelper.playerSettings.playerMultiplayerSettings = playerSettings.playerMultiplayerSettings;
					}
					if (playerSettings.defaultMovesetSettings != null)
					{
						SettingsHelper.playerSettings.defaultMovesetSettings = playerSettings.defaultMovesetSettings;
					}
				}
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
			foreach (PlayerHealth playerHealth in UnityEngine.Object.FindObjectsOfType<PlayerHealth>().ToList<PlayerHealth>())
			{
				playerHealth.UpdatePlayerTexture();
			}
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x0006342C File Offset: 0x0006162C
		public static void SavePlayerSettings()
		{
			try
			{
				string json = JsonConvert.SerializeObject(SettingsHelper.playerSettings, Formatting.Indented);
				Generic.SaveJsonToFile(SettingsHelper.GetSettingsSavePath(), json);
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
		}

		// Token: 0x06001306 RID: 4870 RVA: 0x0006346C File Offset: 0x0006166C
		public static void LoadTutorialSettings()
		{
			SettingsHelper.tutorialSettings = new TutorialSettings();
			try
			{
				TutorialSettings tutorialSettings = JsonConvert.DeserializeObject<TutorialSettings>(Generic.LoadJsonFromFile(SettingsHelper.GetTutorialSettingsSavePath()));
				if (tutorialSettings != null)
				{
					SettingsHelper.tutorialSettings = tutorialSettings;
				}
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
		}

		// Token: 0x06001307 RID: 4871 RVA: 0x000634B8 File Offset: 0x000616B8
		public static void SaveTutorialSettings()
		{
			try
			{
				string json = JsonConvert.SerializeObject(SettingsHelper.tutorialSettings, Formatting.Indented);
				Generic.SaveJsonToFile(SettingsHelper.GetTutorialSettingsSavePath(), json);
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
		}

		// Token: 0x06001308 RID: 4872 RVA: 0x000634F8 File Offset: 0x000616F8
		public static void LoadPersistentSave()
		{
			SettingsHelper.persistentSave = new PersistentSave();
			try
			{
				PersistentSave persistentSave = JsonConvert.DeserializeObject<PersistentSave>(Generic.LoadJsonFromFile(SettingsHelper.GetPersistentSaveSavePath()));
				if (persistentSave != null)
				{
					SettingsHelper.persistentSave = persistentSave;
				}
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
		}

		// Token: 0x06001309 RID: 4873 RVA: 0x00063544 File Offset: 0x00061744
		public static void SavePersistentSave()
		{
			try
			{
				string json = JsonConvert.SerializeObject(SettingsHelper.persistentSave, Formatting.Indented);
				Generic.SaveJsonToFile(SettingsHelper.GetPersistentSaveSavePath(), json);
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
		}

		// Token: 0x0600130A RID: 4874 RVA: 0x00063584 File Offset: 0x00061784
		public static void LoadCommandLineArguments()
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 0; i < commandLineArgs.Length; i++)
			{
				if (commandLineArgs[i] == "-replay")
				{
					SettingsHelper.EnableReplay();
				}
				if (commandLineArgs[i] == "-netstat")
				{
					SettingsHelper.EnableNetworkStatistics();
				}
				if (commandLineArgs[i] == "-disablemovesetlocalization")
				{
					SettingsHelper.DisableMovesetLocalization();
				}
				if (commandLineArgs[i] == "-freecam")
				{
					SettingsHelper.EnableFreeCam();
				}
			}
		}

		// Token: 0x0600130B RID: 4875 RVA: 0x000635F8 File Offset: 0x000617F8
		public static bool GetCustomNameSetting()
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 0; i < commandLineArgs.Length; i++)
			{
				if (commandLineArgs[i] == "-customname")
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600130C RID: 4876 RVA: 0x0006362C File Offset: 0x0006182C
		public static void EnableReplay()
		{
			if (ReplayManager.singleton == null)
			{
				ReplayManager replayManager = UnityEngine.Object.FindObjectsOfType<ReplayManager>(true).FirstOrDefault<ReplayManager>();
				if (replayManager != null)
				{
					replayManager.gameObject.SetActive(true);
				}
			}
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x00063667 File Offset: 0x00061867
		public static void EnableNetworkStatistics()
		{
			SettingsHelper.networkDebugging = true;
			SettingsHelper.CheckNetworkDebugging();
		}

		// Token: 0x0600130E RID: 4878 RVA: 0x00063674 File Offset: 0x00061874
		public static void EnableFreeCam()
		{
			SettingsHelper.freecam = true;
		}

		// Token: 0x0600130F RID: 4879 RVA: 0x0006367C File Offset: 0x0006187C
		public static void EnableStrengthTool()
		{
			JointStrengthTool jointStrengthTool = UnityEngine.Object.FindObjectOfType<JointStrengthTool>(true);
			if (jointStrengthTool != null)
			{
				jointStrengthTool.gameObject.SetActive(true);
			}
		}

		// Token: 0x06001310 RID: 4880 RVA: 0x000636A5 File Offset: 0x000618A5
		public static void DisableMovesetLocalization()
		{
			SettingsHelper.disableMovesetLocalization = true;
		}

		// Token: 0x06001311 RID: 4881 RVA: 0x000636B0 File Offset: 0x000618B0
		public static void CheckNetworkDebugging()
		{
			NetworkStatistics networkStatistics = UnityEngine.Object.FindObjectsOfType<NetworkStatistics>(true).FirstOrDefault<NetworkStatistics>();
			if (networkStatistics != null)
			{
				networkStatistics.enabled = SettingsHelper.networkDebugging;
			}
			if (NetworkManager.singleton != null)
			{
				NetworkManager.singleton.timeInterpolationGui = SettingsHelper.networkDebugging;
			}
		}

		// Token: 0x06001312 RID: 4882 RVA: 0x000636FC File Offset: 0x000618FC
		public static string GetUserSavePath()
		{
			string text = Application.persistentDataPath;
			string text2 = null;
			if (SteamManager.steamManager != null)
			{
				text2 = SteamManager.steamManager.GetSteamUserIDAsString();
			}
			if (text2 != null)
			{
				text = text + "/" + text2;
			}
			SettingsHelper.SetupSaveDestination(text);
			return text;
		}

		// Token: 0x06001313 RID: 4883 RVA: 0x00063740 File Offset: 0x00061940
		public static string GetKeyBindingsSavePath()
		{
			return SettingsHelper.GetUserSavePath() + "/KeyBindings.json";
		}

		// Token: 0x06001314 RID: 4884 RVA: 0x00063751 File Offset: 0x00061951
		public static string GetSettingsSavePath()
		{
			return SettingsHelper.GetUserSavePath() + "/Settings.json";
		}

		// Token: 0x06001315 RID: 4885 RVA: 0x00063762 File Offset: 0x00061962
		public static string GetTutorialSettingsSavePath()
		{
			return SettingsHelper.GetUserSavePath() + "/TutorialSettings.json";
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x00063773 File Offset: 0x00061973
		public static string GetArcadeRunSettingsSavePath()
		{
			return SettingsHelper.GetUserSavePath() + "/ArcadeRun.json";
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x00063784 File Offset: 0x00061984
		public static string GetPersistentSaveSavePath()
		{
			return SettingsHelper.GetUserSavePath() + "/Persistent.json";
		}

		// Token: 0x06001318 RID: 4888 RVA: 0x00063795 File Offset: 0x00061995
		public static string GetSettingsBanListPath()
		{
			return SettingsHelper.GetUserSavePath() + "/BanList.json";
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x000637A6 File Offset: 0x000619A6
		public static string GetMultiplayerGameSettingsSavePath()
		{
			return SettingsHelper.GetUserSavePath() + "/MultiplayerGameSettings.json";
		}

		// Token: 0x0600131A RID: 4890 RVA: 0x000637B7 File Offset: 0x000619B7
		public static string GetLocalGameSettingsSavePath()
		{
			return SettingsHelper.GetUserSavePath() + "/LocalGameSettings.json";
		}

		// Token: 0x0600131B RID: 4891 RVA: 0x000637C8 File Offset: 0x000619C8
		public static string GetArcadeGameSettingsSavePath()
		{
			return SettingsHelper.GetUserSavePath() + "/ArcadeGameSettings.json";
		}

		// Token: 0x0600131C RID: 4892 RVA: 0x000637D9 File Offset: 0x000619D9
		public static string GetMoveSetEditorGameSettingsSavePath()
		{
			return SettingsHelper.GetUserSavePath() + "/MoveSetEditorGameSettings.json";
		}

		// Token: 0x0600131D RID: 4893 RVA: 0x000637EA File Offset: 0x000619EA
		public static string GetSavedTextureSavePath()
		{
			string text = SettingsHelper.GetUserSavePath() + "/SavedTextures/";
			Directory.CreateDirectory(text);
			return text;
		}

		// Token: 0x0600131E RID: 4894 RVA: 0x00063802 File Offset: 0x00061A02
		public static string GetTextureSavePath()
		{
			return SettingsHelper.GetUserSavePath() + "/Textures/";
		}

		// Token: 0x0600131F RID: 4895 RVA: 0x00063813 File Offset: 0x00061A13
		public static string GetCustomPlayerTextureSavePath()
		{
			return SettingsHelper.GetTextureSavePath() + "PlayerTexture.jpg";
		}

		// Token: 0x06001320 RID: 4896 RVA: 0x00063824 File Offset: 0x00061A24
		public static string GetCustomPlayerTextureSavePathOld()
		{
			return SettingsHelper.GetTextureSavePath() + "PlayerTexture.png";
		}

		// Token: 0x06001321 RID: 4897 RVA: 0x00063835 File Offset: 0x00061A35
		public static string GetCommunityTexturesFolder()
		{
			return Application.dataPath + "/Community/CommunityTextures/";
		}

		// Token: 0x06001322 RID: 4898 RVA: 0x00063846 File Offset: 0x00061A46
		public static string GetMoveSetsSaveFolder()
		{
			return SettingsHelper.GetUserSavePath() + "/MoveSets/";
		}

		// Token: 0x06001323 RID: 4899 RVA: 0x0006173D File Offset: 0x0005F93D
		public static void SetupSaveDestination(string destination)
		{
			Directory.CreateDirectory(destination);
		}

		// Token: 0x06001324 RID: 4900 RVA: 0x00063858 File Offset: 0x00061A58
		public static void LoadBanList()
		{
			SettingsHelper.banList = new BanList();
			try
			{
				BanList banList = JsonConvert.DeserializeObject<BanList>(Generic.LoadJsonFromFile(SettingsHelper.GetSettingsBanListPath()));
				if (banList != null && banList.banItems != null)
				{
					SettingsHelper.banList.banItems = banList.banItems;
				}
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
		}

		// Token: 0x06001325 RID: 4901 RVA: 0x000638B4 File Offset: 0x00061AB4
		public static void SaveBanList()
		{
			try
			{
				string json = JsonConvert.SerializeObject(SettingsHelper.banList, Formatting.Indented);
				Generic.SaveJsonToFile(SettingsHelper.GetSettingsBanListPath(), json);
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
		}

		// Token: 0x06001326 RID: 4902 RVA: 0x000638F4 File Offset: 0x00061AF4
		public static void AddItemToBanListAndSave(BanItem item)
		{
			SettingsHelper.banList.banItems.Add(item);
			SettingsHelper.SaveBanList();
		}

		// Token: 0x06001327 RID: 4903 RVA: 0x0006390B File Offset: 0x00061B0B
		public static void AddItemToBanList(BanItem item)
		{
			SettingsHelper.banList.banItems.Add(item);
		}

		// Token: 0x06001328 RID: 4904 RVA: 0x0006391D File Offset: 0x00061B1D
		public static void RemoveItemFromBanList(BanItem item)
		{
			SettingsHelper.banList.banItems.Remove(item);
		}

		// Token: 0x04000E13 RID: 3603
		public static string localizationTableName = "GladioMoriLocalizationTable";

		// Token: 0x04000E14 RID: 3604
		public static string localizationTableNameAssets = "LocalizedImages";

		// Token: 0x04000E15 RID: 3605
		public static bool initiated = false;

		// Token: 0x04000E16 RID: 3606
		public static PlayerSettings playerSettings = new PlayerSettings();

		// Token: 0x04000E17 RID: 3607
		public static TutorialSettings tutorialSettings = new TutorialSettings();

		// Token: 0x04000E18 RID: 3608
		public static PersistentSave persistentSave = new PersistentSave();

		// Token: 0x04000E19 RID: 3609
		public static BanList banList = new BanList();

		// Token: 0x04000E1A RID: 3610
		public static string[] AvailableLocales = new string[]
		{
			"en",
			"zh",
			"fr",
			"de",
			"it",
			"ja",
			"ko",
			"pl",
			"pt-BR",
			"ru",
			"es"
		};

		// Token: 0x04000E1B RID: 3611
		public static int customPlayerTextureMaxBytes = 150000;

		// Token: 0x04000E1C RID: 3612
		public static int customPlayerTextureMaxWidthHeight = 1024;

		// Token: 0x04000E1D RID: 3613
		public static Texture2D customPlayerTexture;

		// Token: 0x04000E1E RID: 3614
		public static byte[] customTextureBytes;

		// Token: 0x04000E1F RID: 3615
		public static string[] skippableMoveEditorActions = new string[]
		{
			"Left_Click",
			"Right_Click",
			"Drag_Select",
			"Copy",
			"Paste",
			"Back",
			"Undo",
			"Redo",
			"Save"
		};

		// Token: 0x04000E20 RID: 3616
		public static bool networkDebugging = false;

		// Token: 0x04000E21 RID: 3617
		public static bool freecam = false;

		// Token: 0x04000E22 RID: 3618
		public static bool disableMovesetLocalization = false;
	}
}
