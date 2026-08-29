using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mirror;
using SimpleFileBrowser;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityUIExtensionMethods;
using Utils;

// Token: 0x020000AF RID: 175
public class PlayerOptionsManager : MonoBehaviour
{
	// Token: 0x060005FD RID: 1533 RVA: 0x0001D764 File Offset: 0x0001B964
	private void Awake()
	{
		this.InitializePlayerOptions();
		this.LoadUserControls();
	}

	// Token: 0x060005FE RID: 1534 RVA: 0x0001D772 File Offset: 0x0001B972
	private void InitializePlayerOptions()
	{
		if (PlayerOptionsManager.singleton != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		PlayerOptionsManager.singleton = this;
	}

	// Token: 0x060005FF RID: 1535 RVA: 0x0001D794 File Offset: 0x0001B994
	private void Start()
	{
		if (GeneralManager.singleton != null)
		{
			GeneralManager.singleton.UpdatePlayerOptionsVisible();
		}
		EventSystem.current.SetSelectedGameObject(this.cancelButton.gameObject);
		this.InitializeLocalization();
		this.SetVisibleSettingsPanel(PlayerOptionsManager.SettingPanel.Gameplay);
		this.LoadSettings();
		this.gameplaySettingsButton.onClick.AddListener(delegate()
		{
			this.SetVisibleSettingsPanel(PlayerOptionsManager.SettingPanel.Gameplay);
		});
		this.cameraSettingsButton.onClick.AddListener(delegate()
		{
			this.SetVisibleSettingsPanel(PlayerOptionsManager.SettingPanel.Camera);
		});
		this.graphicSettingsButton.onClick.AddListener(delegate()
		{
			this.SetVisibleSettingsPanel(PlayerOptionsManager.SettingPanel.Graphics);
		});
		this.keybindingSettingsButton.onClick.AddListener(delegate()
		{
			this.SetVisibleSettingsPanel(PlayerOptionsManager.SettingPanel.Keybindings);
		});
		this.soundsSettingsButton.onClick.AddListener(delegate()
		{
			this.SetVisibleSettingsPanel(PlayerOptionsManager.SettingPanel.Sound);
		});
		this.multiplayerSettingsButton.onClick.AddListener(delegate()
		{
			this.SetVisibleSettingsPanel(PlayerOptionsManager.SettingPanel.Multiplayer);
		});
		this.creditsSettingsButton.onClick.AddListener(delegate()
		{
			this.SetVisibleSettingsPanel(PlayerOptionsManager.SettingPanel.Credits);
		});
		this.resetCameraButton.onClick.AddListener(delegate()
		{
			this.ResetCameraConfirm();
		});
		this.resetKeyBindsButton.onClick.AddListener(delegate()
		{
			this.ResetKeybindsConfirm();
		});
		this.saveButton.onClick.AddListener(delegate()
		{
			this.SaveAll();
		});
		this.banListAdd.onClick.AddListener(delegate()
		{
			this.AddToBanList();
		});
		this.cancelButton.onClick.AddListener(delegate()
		{
			this.NavigateBack();
		});
		this.cameraSmoothFollows = UnityEngine.Object.FindObjectsOfType<CameraSmoothFollow>().ToList<CameraSmoothFollow>();
		this.allCameraEditTransparentImages.AddRange(this.cameraEditTransparentImages);
		foreach (object obj in this.cameraSettingsHolder)
		{
			Image component = ((Transform)obj).GetComponent<Image>();
			if (component != null)
			{
				this.allCameraEditTransparentImages.Add(component);
			}
		}
	}

	// Token: 0x06000600 RID: 1536 RVA: 0x0001D9B4 File Offset: 0x0001BBB4
	private IEnumerable InitializeLocalization()
	{
		yield return LocalizationSettings.InitializationOperation;
		yield break;
	}

	// Token: 0x06000601 RID: 1537 RVA: 0x0001D9C0 File Offset: 0x0001BBC0
	private void LoadSettings()
	{
		this.GenerateKeybindingSettings();
		this.GenerateBanList();
		this.resolutions = (from x in Screen.resolutions
		group x by new
		{
			x.width,
			x.height
		} into x
		select x.First<Resolution>() into x
		select new ResolutionOption
		{
			width = x.width,
			height = x.height
		}).ToList<ResolutionOption>();
		if (SettingsHelper.GetCustomNameSetting())
		{
			this.nameInput.transform.parent.gameObject.SetActive(true);
			this.nameInput.text = SettingsHelper.GetPlayerName();
		}
		else
		{
			this.nameInput.transform.parent.gameObject.SetActive(false);
		}
		this.customPlayerTextureImageSelect.Setup(SettingsHelper.GetCustomPlayerTextureSavePath());
		this.showFPSToggle.isOn = SettingsHelper.GetShowFPS();
		this.recordReplayToggle.isOn = SettingsHelper.GetRecordReplay();
		List<Dropdown.OptionData> list = new List<Dropdown.OptionData>();
		foreach (object obj in Enum.GetValues(typeof(ReplayTexturesOverrideType)))
		{
			ReplayTexturesOverrideType replayTexturesOverrideType = (ReplayTexturesOverrideType)obj;
			list.Add(new OptionDataWithValue
			{
				text = replayTexturesOverrideType.GetDescription(),
				stringValue = replayTexturesOverrideType.ToString()
			});
		}
		this.replayTexturesOverrideDropdown.options = list;
		this.replayTexturesOverrideDropdown.value = (int)SettingsHelper.GetReplayTexturesOverrideType();
		this.replayTexturesOverrideDropdown.RefreshShownValue();
		List<Dropdown.OptionData> list2 = new List<Dropdown.OptionData>();
		foreach (object obj2 in Enum.GetValues(typeof(BloodColourType)))
		{
			BloodColourType bloodColourType = (BloodColourType)obj2;
			list2.Add(new OptionDataWithValue
			{
				text = bloodColourType.GetDescription(),
				stringValue = bloodColourType.ToString()
			});
		}
		this.bloodColourDropdown.options = list2;
		this.bloodColourDropdown.value = (int)SettingsHelper.GetBloodColourType();
		this.bloodColourDropdown.RefreshShownValue();
		List<Dropdown.OptionData> list3 = new List<Dropdown.OptionData>();
		foreach (object obj3 in Enum.GetValues(typeof(AllowCustomTextureOptionsType)))
		{
			AllowCustomTextureOptionsType allowCustomTextureOptionsType = (AllowCustomTextureOptionsType)obj3;
			list3.Add(new OptionDataWithValue
			{
				text = allowCustomTextureOptionsType.GetDescription(),
				stringValue = allowCustomTextureOptionsType.ToString()
			});
		}
		this.allowCustomTexturesDropdown.options = list3;
		this.allowCustomTexturesDropdown.value = (int)SettingsHelper.GetAllowCustomPlayerTextures();
		this.allowCustomTexturesDropdown.RefreshShownValue();
		float mouseSensitivity = SettingsHelper.GetMouseSensitivity();
		this.mouseSensitivitySlider.Setup(0.01f, 2f, mouseSensitivity, false);
		this.disableMouseTurning.isOn = SettingsHelper.GetDisableMouseTurning();
		this.controllerSensitivitySlider.Setup(0.01f, 600f, SettingsHelper.GetControllerSensitivity(), false);
		this.InvertCameraY.isOn = SettingsHelper.GetInvertCameraY();
		this.timeScaleAffectCameraTurnSpeedToggle.isOn = SettingsHelper.GetTimeScaleAffactsCameraTurnSpeed();
		this.showAttackDirectionToggle.isOn = SettingsHelper.GetShowAttackDirection();
		List<Dropdown.OptionData> list4 = new List<Dropdown.OptionData>();
		foreach (object obj4 in Enum.GetValues(typeof(PlayerTurnType)))
		{
			PlayerTurnType playerTurnType = (PlayerTurnType)obj4;
			list4.Add(new OptionDataWithValue
			{
				text = playerTurnType.GetDescription(),
				stringValue = playerTurnType.ToString()
			});
		}
		this.turnTypeDropdown.options = list4;
		this.turnTypeDropdown.value = (int)SettingsHelper.GetPlayerTurnType();
		this.turnTypeDropdown.RefreshShownValue();
		List<Dropdown.OptionData> list5 = new List<Dropdown.OptionData>();
		for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++)
		{
			Locale locale = LocalizationSettings.AvailableLocales.Locales[i];
			if (SettingsHelper.AvailableLocales.Contains(locale.Formatter.ToString()) && !locale.LocaleName.ToLower().Contains("pseudo"))
			{
				list5.Add(new OptionDataWithValue
				{
					text = locale.name,
					stringValue = locale.Formatter.ToString()
				});
			}
		}
		this.languageDropdown.options = list5;
		int value = this.languageDropdown.options.IndexOf((from x in this.languageDropdown.options
		where x.GetStringValue() == LocalizationSettings.SelectedLocale.Formatter.ToString()
		select x).FirstOrDefault<Dropdown.OptionData>());
		this.languageDropdown.value = value;
		this.languageDropdown.RefreshShownValue();
		DefaultMovesetSettings defaultMovesetSettings = SettingsHelper.GetDefaultMovesetSettings();
		this.invertVerticalAttack.isOn = defaultMovesetSettings.invertVerticalAttacks;
		this.invertHorizontalAttack.isOn = defaultMovesetSettings.invertHorizontalAttacks;
		this.invertVerticalBlock.isOn = defaultMovesetSettings.invertVerticalBlocks;
		this.invertHorizontalBlock.isOn = defaultMovesetSettings.invertHorizontalBlocks;
		this.LoadCameraSettings();
		this.cameraPositionOffsetXSlider.ValueChanged += delegate(object <p0>, EventArgs <p1>)
		{
			this.UpdateCameraPositionOffset(this.cameraPositionOffsetXSlider.value, "x");
		};
		this.cameraPositionOffsetYSlider.ValueChanged += delegate(object <p0>, EventArgs <p1>)
		{
			this.UpdateCameraPositionOffset(this.cameraPositionOffsetYSlider.value, "y");
		};
		this.cameraPositionOffsetZSlider.ValueChanged += delegate(object <p0>, EventArgs <p1>)
		{
			this.UpdateCameraPositionOffset(this.cameraPositionOffsetZSlider.value, "z");
		};
		this.cameraTargetOffsetXSlider.ValueChanged += delegate(object <p0>, EventArgs <p1>)
		{
			this.UpdateCameraTargetOffset(this.cameraTargetOffsetXSlider.value, "x");
		};
		this.cameraTargetOffsetYSlider.ValueChanged += delegate(object <p0>, EventArgs <p1>)
		{
			this.UpdateCameraTargetOffset(this.cameraTargetOffsetYSlider.value, "y");
		};
		this.cameraTargetOffsetZSlider.ValueChanged += delegate(object <p0>, EventArgs <p1>)
		{
			this.UpdateCameraTargetOffset(this.cameraTargetOffsetZSlider.value, "z");
		};
		this.cameraFOV.ValueChanged += delegate(object <p0>, EventArgs <p1>)
		{
			this.UpdateCameraFov(this.cameraFOV.value);
		};
		this.bloomToggle.isOn = SettingsHelper.GetBloom();
		this.depthOfFieldToggle.isOn = SettingsHelper.GetDepthOfField();
		if (this.resolutions != null && this.resolutions.Count > 0)
		{
			int num = 0;
			foreach (ResolutionOption resolutionOption in this.resolutions)
			{
				this.resolutionDropdown.options.Add(new Dropdown.OptionData
				{
					text = resolutionOption.ToString()
				});
				if (Screen.width == resolutionOption.width && Screen.height == resolutionOption.height)
				{
					this.resolutionDropdown.value = num;
				}
				num++;
			}
		}
		this.resolutionDropdown.RefreshShownValue();
		int value2 = 0;
		OperatingSystemFamily operatingSystemFamily = SystemInfo.operatingSystemFamily;
		if (operatingSystemFamily == OperatingSystemFamily.Windows)
		{
			this.windowModeDropdown.options.Add(new OptionDataWithValue
			{
				stringValue = string.Format("{0}", FullScreenMode.ExclusiveFullScreen),
				text = LocalizationHelpers.GetLocalizedTextForWindowMode(FullScreenMode.ExclusiveFullScreen)
			});
		}
		this.windowModeDropdown.options.Add(new OptionDataWithValue
		{
			stringValue = string.Format("{0}", FullScreenMode.FullScreenWindow),
			text = LocalizationHelpers.GetLocalizedTextForWindowMode(FullScreenMode.FullScreenWindow)
		});
		if (operatingSystemFamily == OperatingSystemFamily.MacOSX)
		{
			this.windowModeDropdown.options.Add(new OptionDataWithValue
			{
				stringValue = string.Format("{0}", FullScreenMode.MaximizedWindow),
				text = LocalizationHelpers.GetLocalizedTextForWindowMode(FullScreenMode.MaximizedWindow)
			});
		}
		this.windowModeDropdown.options.Add(new OptionDataWithValue
		{
			stringValue = string.Format("{0}", FullScreenMode.Windowed),
			text = LocalizationHelpers.GetLocalizedTextForWindowMode(FullScreenMode.Windowed)
		});
		Dropdown.OptionData optionData = (from x in this.windowModeDropdown.options
		where x.GetStringValue() == string.Format("{0}", Screen.fullScreenMode)
		select x).FirstOrDefault<Dropdown.OptionData>();
		if (optionData != null)
		{
			value2 = this.windowModeDropdown.options.IndexOf(optionData);
		}
		this.windowModeDropdown.value = value2;
		this.windowModeDropdown.RefreshShownValue();
		this.vSyncToggle.isOn = (QualitySettings.vSyncCount > 0);
		this.vSyncToggle.onValueChanged.AddListener(delegate(bool <p0>)
		{
			this.VSyncChanged(this.vSyncToggle.isOn);
		});
		this.fpsLimitSlider.minValue = 30f;
		this.fpsLimitSlider.maxValue = 400f;
		this.fpsLimitSlider.wholeNumbers = true;
		this.fpsLimitSlider.onValueChanged.AddListener(delegate(float <p0>)
		{
			this.UpdateFpsLimitText();
		});
		this.fpsLimitSlider.value = (float)Application.targetFrameRate;
		this.UpdateVsyncSliderStatus();
		this.UpdateFpsLimitText();
		this.postProcessingAADropdown.options.Add(new Dropdown.OptionData
		{
			text = LocalizationHelpers.GetLocalizedTextForAntiAliasing(AntialiasingMode.None)
		});
		this.postProcessingAADropdown.options.Add(new Dropdown.OptionData
		{
			text = LocalizationHelpers.GetLocalizedTextForAntiAliasing(AntialiasingMode.FastApproximateAntialiasing)
		});
		this.postProcessingAADropdown.options.Add(new Dropdown.OptionData
		{
			text = LocalizationHelpers.GetLocalizedTextForAntiAliasing(AntialiasingMode.SubpixelMorphologicalAntiAliasing)
		});
		this.postProcessingAADropdown.value = PlayerPrefs.GetInt("PostProcessingAA", 1);
		this.postProcessingAADropdown.RefreshShownValue();
		this.msaaDropdown.options.Add(new Dropdown.OptionData
		{
			text = LocalizationHelpers.LocalizedText("txt_none", Array.Empty<object>())
		});
		this.msaaDropdown.options.Add(new Dropdown.OptionData
		{
			text = "2x"
		});
		this.msaaDropdown.options.Add(new Dropdown.OptionData
		{
			text = "4x"
		});
		this.msaaDropdown.options.Add(new Dropdown.OptionData
		{
			text = "8x"
		});
		int @int = PlayerPrefs.GetInt("MSAA", 1);
		int value3 = 0;
		if (@int != 2)
		{
			if (@int != 4)
			{
				if (@int == 8)
				{
					value3 = 3;
				}
			}
			else
			{
				value3 = 2;
			}
		}
		else
		{
			value3 = 1;
		}
		this.msaaDropdown.value = value3;
		this.msaaDropdown.RefreshShownValue();
		this.shadowQualityDropdown.options.Add(new Dropdown.OptionData
		{
			text = LocalizationHelpers.LocalizedText("txt_very_low", Array.Empty<object>())
		});
		this.shadowQualityDropdown.options.Add(new Dropdown.OptionData
		{
			text = LocalizationHelpers.LocalizedText("txt_low", Array.Empty<object>())
		});
		this.shadowQualityDropdown.options.Add(new Dropdown.OptionData
		{
			text = LocalizationHelpers.LocalizedText("txt_medium", Array.Empty<object>())
		});
		this.shadowQualityDropdown.options.Add(new Dropdown.OptionData
		{
			text = LocalizationHelpers.LocalizedText("txt_high", Array.Empty<object>())
		});
		this.shadowQualityDropdown.options.Add(new Dropdown.OptionData
		{
			text = LocalizationHelpers.LocalizedText("txt_very_high", Array.Empty<object>())
		});
		int shadowQuality = SettingsHelper.GetShadowQuality();
		this.shadowQualityDropdown.value = shadowQuality;
		this.shadowQualityDropdown.RefreshShownValue();
		this.bloodQualityDropdown.options.Add(new Dropdown.OptionData
		{
			text = LocalizationHelpers.LocalizedText("txt_very_low", Array.Empty<object>())
		});
		this.bloodQualityDropdown.options.Add(new Dropdown.OptionData
		{
			text = LocalizationHelpers.LocalizedText("txt_low", Array.Empty<object>())
		});
		this.bloodQualityDropdown.options.Add(new Dropdown.OptionData
		{
			text = LocalizationHelpers.LocalizedText("txt_medium", Array.Empty<object>())
		});
		this.bloodQualityDropdown.options.Add(new Dropdown.OptionData
		{
			text = LocalizationHelpers.LocalizedText("txt_high", Array.Empty<object>())
		});
		this.bloodQualityDropdown.options.Add(new Dropdown.OptionData
		{
			text = LocalizationHelpers.LocalizedText("txt_very_high", Array.Empty<object>())
		});
		int bloodQuality = SettingsHelper.GetBloodQuality();
		this.bloodQualityDropdown.value = bloodQuality;
		this.bloodQualityDropdown.RefreshShownValue();
		this.masterVolumeSlider.minValue = 0.0001f;
		this.masterVolumeSlider.maxValue = 1f;
		this.masterVolumeSlider.wholeNumbers = false;
		float num2;
		this.audioMixer.GetFloat("MasterVolume", out num2);
		this.masterVolumeSlider.value = Mathf.Pow(10f, num2 / 20f);
		this.effectsVolumeSlider.minValue = 0.0001f;
		this.effectsVolumeSlider.maxValue = 1f;
		this.effectsVolumeSlider.wholeNumbers = false;
		float num3;
		this.audioMixer.GetFloat("EffectsVolume", out num3);
		this.effectsVolumeSlider.value = Mathf.Pow(10f, num3 / 20f);
		this.musicVolumeSlider.minValue = 0.0001f;
		this.musicVolumeSlider.maxValue = 1f;
		this.musicVolumeSlider.wholeNumbers = false;
		float num4;
		this.audioMixer.GetFloat("MusicVolume", out num4);
		this.musicVolumeSlider.value = Mathf.Pow(10f, num4 / 20f);
		this.voiceChatVolumeSlider.minValue = 0.0001f;
		this.voiceChatVolumeSlider.maxValue = 1f;
		this.voiceChatVolumeSlider.wholeNumbers = false;
		float num5;
		this.audioMixer.GetFloat("VoiceChatVolume", out num5);
		this.voiceChatVolumeSlider.value = Mathf.Pow(10f, num5 / 20f);
		this.masterVolumeSlider.onValueChanged.AddListener(delegate(float <p0>)
		{
			this.UpdateMasterVolume(this.masterVolumeSlider.value);
		});
		this.musicVolumeSlider.onValueChanged.AddListener(delegate(float <p0>)
		{
			this.UpdateMusicVolume(this.musicVolumeSlider.value);
		});
		this.effectsVolumeSlider.onValueChanged.AddListener(delegate(float <p0>)
		{
			this.UpdateEffectsVolume(this.effectsVolumeSlider.value);
		});
		this.voiceChatVolumeSlider.onValueChanged.AddListener(delegate(float <p0>)
		{
			this.UpdateVoiceChatVolume(this.voiceChatVolumeSlider.value);
		});
		this.UpdateMasterVolumeText();
		this.UpdateMusicVolumeText();
		this.UpdateEffectsVolumeText();
		this.UpdateVoiceChatVolumeText();
		List<Dropdown.OptionData> list6 = new List<Dropdown.OptionData>();
		foreach (object obj5 in Enum.GetValues(typeof(ChatOption)))
		{
			ChatOption chatOption = (ChatOption)obj5;
			list6.Add(new OptionDataWithValue
			{
				text = chatOption.GetDescription(),
				stringValue = chatOption.ToString()
			});
		}
		this.chatDropdown.options = list6;
		this.chatDropdown.value = (int)SettingsHelper.GetChatOption();
		this.chatDropdown.RefreshShownValue();
		List<Dropdown.OptionData> list7 = new List<Dropdown.OptionData>();
		foreach (object obj6 in Enum.GetValues(typeof(GladioMoriServerType)))
		{
			GladioMoriServerType gladioMoriServerType = (GladioMoriServerType)obj6;
			if (gladioMoriServerType != GladioMoriServerType.None)
			{
				list7.Add(new OptionDataWithValue
				{
					text = gladioMoriServerType.GetDescription(),
					stringValue = gladioMoriServerType.ToString()
				});
			}
		}
		this.banListTypeDropdown.options = list7;
		this.banListTypeDropdown.value = 1;
		this.banListTypeDropdown.RefreshShownValue();
		this.creditsText.text = this.creditsTextAsset.text;
	}

	// Token: 0x06000602 RID: 1538 RVA: 0x0001E96C File Offset: 0x0001CB6C
	private void LoadCameraSettings()
	{
		this.cameraSettings = Generic.DeepClone<PlayerCameraSettings>(SettingsHelper.GetCameraSettings());
		this.cameraPositionOffsetXSlider.Setup(-5f, 5f, this.cameraSettings.cameraPositionOffset.x, false);
		this.cameraPositionOffsetYSlider.Setup(-5f, 5f, this.cameraSettings.cameraPositionOffset.y, false);
		this.cameraPositionOffsetZSlider.Setup(-5f, 5f, this.cameraSettings.cameraPositionOffset.z, false);
		this.cameraTargetOffsetXSlider.Setup(-5f, 5f, this.cameraSettings.cameraTargetOffset.x, false);
		this.cameraTargetOffsetYSlider.Setup(-5f, 5f, this.cameraSettings.cameraTargetOffset.y, false);
		this.cameraTargetOffsetZSlider.Setup(-5f, 5f, this.cameraSettings.cameraTargetOffset.z, false);
		this.cameraFOV.Setup(45f, 135f, (float)this.cameraSettings.cameraFov, true);
	}

	// Token: 0x06000603 RID: 1539 RVA: 0x0001EA8F File Offset: 0x0001CC8F
	private void UpdateCameraFov(float value)
	{
		this.cameraSettings.cameraFov = (int)value;
		this.PreviewCameraSettings();
	}

	// Token: 0x06000604 RID: 1540 RVA: 0x0001EAA4 File Offset: 0x0001CCA4
	private void UpdateCameraPositionOffset(float value, string axis)
	{
		if (axis == "x")
		{
			this.cameraSettings.cameraPositionOffset = new Vector3(value, this.cameraSettings.cameraPositionOffset.y, this.cameraSettings.cameraPositionOffset.z);
		}
		else if (axis == "y")
		{
			this.cameraSettings.cameraPositionOffset = new Vector3(this.cameraSettings.cameraPositionOffset.x, value, this.cameraSettings.cameraPositionOffset.z);
		}
		else if (axis == "z")
		{
			this.cameraSettings.cameraPositionOffset = new Vector3(this.cameraSettings.cameraPositionOffset.x, this.cameraSettings.cameraPositionOffset.y, value);
		}
		this.PreviewCameraSettings();
	}

	// Token: 0x06000605 RID: 1541 RVA: 0x0001EB78 File Offset: 0x0001CD78
	private void UpdateCameraTargetOffset(float value, string axis)
	{
		if (axis == "x")
		{
			this.cameraSettings.cameraTargetOffset = new Vector3(value, this.cameraSettings.cameraTargetOffset.y, this.cameraSettings.cameraTargetOffset.z);
		}
		else if (axis == "y")
		{
			this.cameraSettings.cameraTargetOffset = new Vector3(this.cameraSettings.cameraTargetOffset.x, value, this.cameraSettings.cameraTargetOffset.z);
		}
		else if (axis == "z")
		{
			this.cameraSettings.cameraTargetOffset = new Vector3(this.cameraSettings.cameraTargetOffset.x, this.cameraSettings.cameraTargetOffset.y, value);
		}
		this.PreviewCameraSettings();
	}

	// Token: 0x06000606 RID: 1542 RVA: 0x0001EC4C File Offset: 0x0001CE4C
	private void PreviewCameraSettings()
	{
		if (this.cameraSmoothFollows != null)
		{
			foreach (CameraSmoothFollow cameraSmoothFollow in this.cameraSmoothFollows)
			{
				cameraSmoothFollow.SetCameraSettings(this.cameraSettings, true, this.cameraTargetPreviewPrefab);
			}
		}
	}

	// Token: 0x06000607 RID: 1543 RVA: 0x0001ECB4 File Offset: 0x0001CEB4
	private void ResetCameraConfirm()
	{
		BasicConfirmDialog component = UnityEngine.Object.Instantiate<GameObject>(this.confirmDialogPrefab).GetComponent<BasicConfirmDialog>();
		component.SetText("", LocalizationHelpers.LocalizedText("confirm_reset_camera_settings", Array.Empty<object>()), false);
		component.okButton.onClick.AddListener(new UnityAction(this.ResetCamera));
		component.cancelButton.Select();
	}

	// Token: 0x06000608 RID: 1544 RVA: 0x0001ED12 File Offset: 0x0001CF12
	private void ResetCamera()
	{
		SettingsHelper.ResetCameraSettings();
		this.LoadCameraSettings();
		this.PreviewCameraSettings();
	}

	// Token: 0x06000609 RID: 1545 RVA: 0x0001ED25 File Offset: 0x0001CF25
	public void UpdateMasterVolume(float volume)
	{
		this.audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20f);
		this.PlayTestSoundEffect();
		this.UpdateMasterVolumeText();
	}

	// Token: 0x0600060A RID: 1546 RVA: 0x0001ED50 File Offset: 0x0001CF50
	public void UpdateMasterVolumeText()
	{
		this.masterVolumeText.text = Convert.ToInt32(this.masterVolumeSlider.value * 100f).ToString();
	}

	// Token: 0x0600060B RID: 1547 RVA: 0x0001ED86 File Offset: 0x0001CF86
	public void UpdateMusicVolume(float volume)
	{
		this.audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20f);
		this.UpdateMusicVolumeText();
	}

	// Token: 0x0600060C RID: 1548 RVA: 0x0001EDAC File Offset: 0x0001CFAC
	public void UpdateMusicVolumeText()
	{
		this.musicVolumeText.text = Convert.ToInt32(this.musicVolumeSlider.value * 100f).ToString();
	}

	// Token: 0x0600060D RID: 1549 RVA: 0x0001EDE2 File Offset: 0x0001CFE2
	public void UpdateVoiceChatVolume(float volume)
	{
		this.audioMixer.SetFloat("VoiceChatVolume", Mathf.Log10(volume) * 20f);
		this.UpdateVoiceChatVolumeText();
	}

	// Token: 0x0600060E RID: 1550 RVA: 0x0001EE08 File Offset: 0x0001D008
	public void UpdateVoiceChatVolumeText()
	{
		this.voiceChatVolumeText.text = Convert.ToInt32(this.voiceChatVolumeSlider.value * 100f).ToString();
	}

	// Token: 0x0600060F RID: 1551 RVA: 0x0001EE3E File Offset: 0x0001D03E
	public void UpdateEffectsVolume(float volume)
	{
		this.audioMixer.SetFloat("EffectsVolume", Mathf.Log10(volume) * 20f);
		this.PlayTestSoundEffect();
		this.UpdateEffectsVolumeText();
	}

	// Token: 0x06000610 RID: 1552 RVA: 0x0001EE6C File Offset: 0x0001D06C
	private void PlayTestSoundEffect()
	{
		if (SoundManager.singleton != null && this.lastTestSoundPlayed + this.testAudio.length <= Time.time)
		{
			this.lastTestSoundPlayed = Time.time;
			SoundManager.singleton.PlaySound(this.testAudio, default(Vector3), 1f, false);
		}
	}

	// Token: 0x06000611 RID: 1553 RVA: 0x0001EECC File Offset: 0x0001D0CC
	public void UpdateEffectsVolumeText()
	{
		this.effectsVolumeText.text = Convert.ToInt32(this.effectsVolumeSlider.value * 100f).ToString();
	}

	// Token: 0x06000612 RID: 1554 RVA: 0x0001EF02 File Offset: 0x0001D102
	public void VSyncChanged(bool vSyncOn)
	{
		this.UpdateVsyncSliderStatus();
		this.UpdateFpsLimitText();
	}

	// Token: 0x06000613 RID: 1555 RVA: 0x0001EF10 File Offset: 0x0001D110
	public void UpdateFpsLimitText()
	{
		if (this.vSyncToggle.isOn)
		{
			this.fpsLimitText.text = LocalizationHelpers.LocalizedText("txt_disabled", Array.Empty<object>());
			return;
		}
		this.fpsLimitText.text = ((int)this.fpsLimitSlider.value).ToString();
	}

	// Token: 0x06000614 RID: 1556 RVA: 0x0001EF64 File Offset: 0x0001D164
	public void UpdateVsyncSliderStatus()
	{
		if (this.vSyncToggle.isOn)
		{
			this.fpsLimitSlider.interactable = false;
			return;
		}
		this.fpsLimitSlider.interactable = true;
	}

	// Token: 0x06000615 RID: 1557 RVA: 0x0001EF8C File Offset: 0x0001D18C
	private void Update()
	{
		if (this.userControls.Generic.Back.WasPerformedThisFrame())
		{
			this.NavigateBack();
		}
	}

	// Token: 0x06000616 RID: 1558 RVA: 0x0001EFBC File Offset: 0x0001D1BC
	public void SaveAll()
	{
		try
		{
			if (SettingsHelper.GetCustomNameSetting())
			{
				PlayerPrefs.SetString("UserName", this.nameInput.text);
			}
			if (this.customPlayerTextureImageSelect.valueChanged)
			{
				SettingsHelper.SaveCustomPlayerTexture(this.customPlayerTextureImageSelect.path);
				if (NetworkClient.active)
				{
					GeneralManager.DisplayInfoMessage(LocalizationHelpers.LocalizedText("alert_texture_changes_during_multiplayer", Array.Empty<object>()), 3f);
				}
			}
			SettingsHelper.SetShowFPS(this.showFPSToggle.isOn);
			SettingsHelper.SetRecordReplay(this.recordReplayToggle.isOn);
			SettingsHelper.SetReplayTexturesOverrideType((ReplayTexturesOverrideType)this.replayTexturesOverrideDropdown.value);
			SettingsHelper.SetBloodColourType((BloodColourType)this.bloodColourDropdown.value);
			SettingsHelper.SetAllowCustomPlayerTextures((AllowCustomTextureOptionsType)this.allowCustomTexturesDropdown.value);
			SettingsHelper.SetMouseSensitivity(this.mouseSensitivitySlider.value);
			SettingsHelper.SetDisableMouseTurning(this.disableMouseTurning.isOn);
			SettingsHelper.SetControllerSensitivity(this.controllerSensitivitySlider.value);
			SettingsHelper.SetInvertCameraY(this.InvertCameraY.isOn);
			SettingsHelper.SetTimeScaleAffactsCameraTurnSpeed(this.timeScaleAffectCameraTurnSpeedToggle.isOn);
			SettingsHelper.SetShowAttackDirection(this.showAttackDirectionToggle.isOn);
			SettingsHelper.SetPlayerTurnType((PlayerTurnType)this.turnTypeDropdown.value);
			OptionDataWithValue selectedLocaleOption = (OptionDataWithValue)this.languageDropdown.options[this.languageDropdown.value];
			Locale locale = (from x in LocalizationSettings.AvailableLocales.Locales
			where x.Formatter.ToString() == selectedLocaleOption.stringValue
			select x).FirstOrDefault<Locale>();
			if (locale != LocalizationSettings.SelectedLocale)
			{
				LocalizationSettings.SelectedLocale = locale;
				SettingsHelper.SetLocale(locale.Formatter.ToString());
			}
			SettingsHelper.SetDefaultMovesetSettings(new DefaultMovesetSettings
			{
				invertVerticalAttacks = this.invertVerticalAttack.isOn,
				invertHorizontalAttacks = this.invertHorizontalAttack.isOn,
				invertVerticalBlocks = this.invertVerticalBlock.isOn,
				invertHorizontalBlocks = this.invertHorizontalBlock.isOn
			});
			SettingsHelper.SetCameraSettings(this.cameraSettings);
			PlayerPrefs.SetInt("Bloom", this.bloomToggle.isOn ? 1 : 0);
			PlayerPrefs.SetInt("DepthOfField", this.depthOfFieldToggle.isOn ? 1 : 0);
			ResolutionOption resolutionOption = this.resolutions[this.resolutionDropdown.value];
			FullScreenMode fullScreenMode = (FullScreenMode)Enum.Parse(typeof(FullScreenMode), this.windowModeDropdown.options[this.windowModeDropdown.value].GetStringValue());
			Debug.Log("Save screen settings");
			if (Screen.width != resolutionOption.width || Screen.height != resolutionOption.height || Screen.fullScreenMode != fullScreenMode)
			{
				Debug.Log(string.Format("CurrentResolution:{0}x{1}@{2}Hz {3}", new object[]
				{
					Screen.width,
					Screen.height,
					Screen.currentResolution.refreshRate,
					Screen.fullScreenMode
				}));
				Debug.Log(string.Format("NewResolution:{0}x{1}@{2}Hz {3}", new object[]
				{
					resolutionOption.width,
					resolutionOption.height,
					resolutionOption.refreshRate,
					fullScreenMode
				}));
				Application.targetFrameRate = -1;
				Screen.SetResolution(resolutionOption.width, resolutionOption.height, fullScreenMode, 0);
			}
			PlayerPrefs.SetInt("VSync", this.vSyncToggle.isOn ? 1 : 0);
			PlayerPrefs.SetInt("FpsLimit", (int)this.fpsLimitSlider.value);
			PlayerPrefs.SetInt("PostProcessingAA", this.postProcessingAADropdown.value);
			int value = 1;
			switch (this.msaaDropdown.value)
			{
			case 1:
				value = 2;
				break;
			case 2:
				value = 4;
				break;
			case 3:
				value = 8;
				break;
			}
			PlayerPrefs.SetInt("MSAA", value);
			PlayerPrefs.SetInt("ShadowQuality", this.shadowQualityDropdown.value);
			PlayerPrefs.SetInt("BloodQuality", this.bloodQualityDropdown.value);
			SettingsHelper.SaveUserControls(this.userControls);
			SettingsHelper.SetChatOption((ChatOption)this.chatDropdown.value);
			SettingsHelper.SetMasterVolume(this.masterVolumeSlider.value);
			SettingsHelper.SetEffectsVolume(this.effectsVolumeSlider.value);
			SettingsHelper.SetMusicVolume(this.musicVolumeSlider.value);
			SettingsHelper.SetVoiceChatVolume(this.voiceChatVolumeSlider.value);
			PlayerPrefs.Save();
			SettingsHelper.SavePlayerSettings();
			SettingsHelper.SaveBanList();
			SettingsHelper.LoadAllSettings();
			UnityEngine.Object.Instantiate<GameObject>(this.infoDialogPrefab).GetComponent<BasicInfoDialog>().SetText(LocalizationHelpers.LocalizedText("txt_saved", Array.Empty<object>()), 1f, false);
			this.GenerateKeybindingSettings();
			this.GenerateBanList();
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
	}

	// Token: 0x06000617 RID: 1559 RVA: 0x0001F48C File Offset: 0x0001D68C
	public void NavigateBack()
	{
		if (!GeneralManager.AllowBackNavigation(null))
		{
			return;
		}
		if (FileBrowser.IsOpen)
		{
			FileBrowser.HideDialog(false);
			return;
		}
		PlayerOptionsManager.singleton = null;
		if (GeneralManager.singleton != null)
		{
			GeneralManager.singleton.UpdatePlayerOptionsVisible();
		}
		SettingsHelper.LoadCameraSettings();
		SettingsHelper.LoadAudioSettings();
		SettingsHelper.LoadBanList();
		if (SceneManager.GetActiveScene().name == "Options")
		{
			SceneManager.LoadScene("MainMenu");
			return;
		}
		this.SetOptionsTransparency(false);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06000618 RID: 1560 RVA: 0x0001F514 File Offset: 0x0001D714
	public void SetVisibleSettingsPanel(PlayerOptionsManager.SettingPanel panelType)
	{
		foreach (object obj in this.settingsPanelHolder.transform)
		{
			((Transform)obj).gameObject.SetActive(false);
		}
		UIHelpers.SetButtonColor(this.gameplaySettingsButton, ButtonState.Basic, null, null);
		UIHelpers.SetButtonColor(this.cameraSettingsButton, ButtonState.Basic, null, null);
		UIHelpers.SetButtonColor(this.graphicSettingsButton, ButtonState.Basic, null, null);
		UIHelpers.SetButtonColor(this.keybindingSettingsButton, ButtonState.Basic, null, null);
		UIHelpers.SetButtonColor(this.soundsSettingsButton, ButtonState.Basic, null, null);
		UIHelpers.SetButtonColor(this.multiplayerSettingsButton, ButtonState.Basic, null, null);
		UIHelpers.SetButtonColor(this.creditsSettingsButton, ButtonState.Basic, null, null);
		this.SetOptionsTransparency(false);
		if (panelType == PlayerOptionsManager.SettingPanel.Gameplay)
		{
			this.gameplaySettingsPanel.SetActive(true);
			UIHelpers.SetButtonColor(this.gameplaySettingsButton, ButtonState.Selected, null, null);
			return;
		}
		if (panelType == PlayerOptionsManager.SettingPanel.Camera)
		{
			this.cameraSettingsPanel.SetActive(true);
			this.PreviewCameraSettings();
			UIHelpers.SetButtonColor(this.cameraSettingsButton, ButtonState.Selected, null, null);
			if (SceneManager.GetActiveScene().name.ToLower().Contains("map_"))
			{
				this.SetOptionsTransparency(true);
				return;
			}
		}
		else
		{
			if (panelType == PlayerOptionsManager.SettingPanel.Graphics)
			{
				this.graphicsSettingsPanel.SetActive(true);
				UIHelpers.SetButtonColor(this.graphicSettingsButton, ButtonState.Selected, null, null);
				return;
			}
			if (panelType == PlayerOptionsManager.SettingPanel.Keybindings)
			{
				this.keybindingSettingsPanel.SetActive(true);
				UIHelpers.SetButtonColor(this.keybindingSettingsButton, ButtonState.Selected, null, null);
				return;
			}
			if (panelType == PlayerOptionsManager.SettingPanel.Multiplayer)
			{
				this.multiplayerSettingsPanel.SetActive(true);
				UIHelpers.SetButtonColor(this.multiplayerSettingsButton, ButtonState.Selected, null, null);
				return;
			}
			if (panelType == PlayerOptionsManager.SettingPanel.Credits)
			{
				this.creditsSettingsPanel.SetActive(true);
				UIHelpers.SetButtonColor(this.creditsSettingsButton, ButtonState.Selected, null, null);
				return;
			}
			this.soundsSettingsPanel.SetActive(true);
			UIHelpers.SetButtonColor(this.soundsSettingsButton, ButtonState.Selected, null, null);
		}
	}

	// Token: 0x06000619 RID: 1561 RVA: 0x0001F6E0 File Offset: 0x0001D8E0
	private void SetOptionsTransparency(bool transparent = false)
	{
		if (GameMenu.singleton != null)
		{
			GameMenu.singleton.HideMenuTemporarily(transparent);
		}
		float a = 1f;
		if (transparent)
		{
			a = 0.4f;
		}
		if (this.allCameraEditTransparentImages != null)
		{
			foreach (Image image in this.allCameraEditTransparentImages)
			{
				Color color = image.color;
				color.a = a;
				image.color = color;
			}
		}
	}

	// Token: 0x0600061A RID: 1562 RVA: 0x0001F770 File Offset: 0x0001D970
	public void GenerateKeybindingSettings()
	{
		if (this.userControls == null)
		{
			return;
		}
		this.keybindOptionRows = new List<KeybindOptionRow>();
		foreach (object obj in this.keybindingSettingsContainer.transform)
		{
			UnityEngine.Object.Destroy(((Transform)obj).gameObject);
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.titleRowPrefab);
		gameObject.GetComponentInChildren<Text>().text = LocalizationHelpers.LocalizedText("option_binding_title_playeractions", Array.Empty<object>());
		gameObject.transform.SetParent(this.keybindingSettingsContainer.transform);
		foreach (PropertyInfo propertyInfo in this.userControls.PlayerActionMap.GetType().GetProperties())
		{
			if (propertyInfo.PropertyType == typeof(InputAction))
			{
				InputAction inputAction = (InputAction)propertyInfo.GetValue(this.userControls.PlayerActionMap);
				if (!(inputAction.name == "Turn_Mouse_Vertical") && !(inputAction.name == "Turn_Mouse_Horizontal"))
				{
					GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.keybindRowPrefab);
					KeybindOptionRow component = gameObject2.GetComponent<KeybindOptionRow>();
					this.keybindOptionRows.Add(component);
					gameObject2.transform.SetParent(this.keybindingSettingsContainer.transform);
					component.SetInputAction(inputAction);
					this.BindKeybindOptionRowButtons(inputAction);
				}
			}
		}
		GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(this.titleRowPrefab);
		gameObject3.GetComponentInChildren<Text>().text = LocalizationHelpers.LocalizedText("option_binding_title_general", Array.Empty<object>());
		gameObject3.transform.SetParent(this.keybindingSettingsContainer.transform);
		foreach (PropertyInfo propertyInfo2 in this.userControls.General.GetType().GetProperties())
		{
			if (propertyInfo2.PropertyType == typeof(InputAction))
			{
				InputAction inputAction2 = (InputAction)propertyInfo2.GetValue(this.userControls.General);
				GameObject gameObject4 = UnityEngine.Object.Instantiate<GameObject>(this.keybindRowPrefab);
				KeybindOptionRow component2 = gameObject4.GetComponent<KeybindOptionRow>();
				this.keybindOptionRows.Add(component2);
				gameObject4.transform.SetParent(this.keybindingSettingsContainer.transform);
				component2.SetInputAction(inputAction2);
				this.BindKeybindOptionRowButtons(inputAction2);
			}
		}
		GameObject gameObject5 = UnityEngine.Object.Instantiate<GameObject>(this.titleRowPrefab);
		gameObject5.GetComponentInChildren<Text>().text = LocalizationHelpers.LocalizedText("btn_move_editor", Array.Empty<object>());
		gameObject5.transform.SetParent(this.keybindingSettingsContainer.transform);
		foreach (PropertyInfo propertyInfo3 in this.userControls.MoveEditorMap.GetType().GetProperties())
		{
			if (propertyInfo3.PropertyType == typeof(InputAction))
			{
				InputAction inputAction3 = (InputAction)propertyInfo3.GetValue(this.userControls.MoveEditorMap);
				if (!SettingsHelper.skippableMoveEditorActions.Contains(inputAction3.name))
				{
					GameObject gameObject6 = UnityEngine.Object.Instantiate<GameObject>(this.keybindRowPrefab);
					KeybindOptionRow component3 = gameObject6.GetComponent<KeybindOptionRow>();
					this.keybindOptionRows.Add(component3);
					gameObject6.transform.SetParent(this.keybindingSettingsContainer.transform);
					component3.SetInputAction(inputAction3);
					this.BindKeybindOptionRowButtons(inputAction3);
				}
			}
		}
		GameObject gameObject7 = UnityEngine.Object.Instantiate<GameObject>(this.titleRowPrefab);
		gameObject7.GetComponentInChildren<Text>().text = LocalizationHelpers.LocalizedText("option_binding_title_replay_spectator", Array.Empty<object>());
		gameObject7.transform.SetParent(this.keybindingSettingsContainer.transform);
		foreach (PropertyInfo propertyInfo4 in this.userControls.ReplayMap.GetType().GetProperties())
		{
			if (propertyInfo4.PropertyType == typeof(InputAction))
			{
				InputAction inputAction4 = (InputAction)propertyInfo4.GetValue(this.userControls.ReplayMap);
				GameObject gameObject8 = UnityEngine.Object.Instantiate<GameObject>(this.keybindRowPrefab);
				KeybindOptionRow component4 = gameObject8.GetComponent<KeybindOptionRow>();
				this.keybindOptionRows.Add(component4);
				gameObject8.transform.SetParent(this.keybindingSettingsContainer.transform);
				component4.SetInputAction(inputAction4);
				this.BindKeybindOptionRowButtons(inputAction4);
			}
		}
	}

	// Token: 0x0600061B RID: 1563 RVA: 0x0001FBC8 File Offset: 0x0001DDC8
	public void BindKeybindOptionRowButtons(InputAction inputAction)
	{
		if (inputAction != null)
		{
			KeybindOptionRow keybindOptionRow = (from x in this.keybindOptionRows
			where x.keybindName == inputAction.name
			select x).FirstOrDefault<KeybindOptionRow>();
			if (keybindOptionRow != null)
			{
				using (List<KeybindOptionSingle>.Enumerator enumerator = keybindOptionRow.keybindOptionSingles.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeybindOptionSingle keybindOptionSingle = enumerator.Current;
						keybindOptionSingle.listenKeybindButton.onClick.RemoveAllListeners();
						keybindOptionSingle.deleteKeybindButton.onClick.RemoveAllListeners();
						keybindOptionSingle.listenKeybindButton.onClick.AddListener(delegate()
						{
							this.ListenKeyForInputAction(inputAction, keybindOptionSingle.bindInt);
						});
						keybindOptionSingle.deleteKeybindButton.onClick.AddListener(delegate()
						{
							this.DeleteKeyForInputAction(inputAction, keybindOptionSingle.bindInt);
						});
					}
				}
				keybindOptionRow.addKeybindButton.onClick.RemoveAllListeners();
				keybindOptionRow.addKeybindButton.onClick.AddListener(delegate()
				{
					this.AddBindingForInputAction(inputAction);
				});
			}
		}
	}

	// Token: 0x0600061C RID: 1564 RVA: 0x0001FD0C File Offset: 0x0001DF0C
	public void UpdateInputActionDisplay(InputAction inputAction, bool recreateKeybinds = false)
	{
		if (inputAction != null)
		{
			KeybindOptionRow keybindOptionRow = (from x in this.keybindOptionRows
			where x.keybindName == inputAction.name
			select x).FirstOrDefault<KeybindOptionRow>();
			if (keybindOptionRow != null)
			{
				keybindOptionRow.UpdateBindingDisplays(recreateKeybinds);
			}
		}
	}

	// Token: 0x0600061D RID: 1565 RVA: 0x0001FD5C File Offset: 0x0001DF5C
	public void ListenKeyForInputAction(InputAction inputAction, int bindingIndex)
	{
		if (inputAction != null)
		{
			Debug.Log("Perform rebind for " + inputAction.name);
			this.listenKeyForInputAction = inputAction;
			this.ShowRebindInfoPanel();
			this.rebindingOperation = inputAction.PerformInteractiveRebinding(bindingIndex).WithExpectedControlType<ButtonControl>().OnMatchWaitForAnother(0.1f).OnComplete(delegate(InputActionRebindingExtensions.RebindingOperation x)
			{
				this.RebindComplete();
			}).Start();
		}
	}

	// Token: 0x0600061E RID: 1566 RVA: 0x0001FDC0 File Offset: 0x0001DFC0
	public void ShowRebindInfoPanel()
	{
		if (this.rebindInfoPanel == null)
		{
			this.rebindInfoPanel = UnityEngine.Object.Instantiate<GameObject>(this.infoDialogPrefab).GetComponent<BasicInfoDialog>();
			this.rebindInfoPanel.SetText(LocalizationHelpers.LocalizedText("txt_press_key_to_bind", Array.Empty<object>()), -1f, false);
		}
	}

	// Token: 0x0600061F RID: 1567 RVA: 0x0001FE11 File Offset: 0x0001E011
	public void HideRebindInfoPanel()
	{
		if (this.rebindInfoPanel != null)
		{
			this.rebindInfoPanel.DestroyPanel();
		}
	}

	// Token: 0x06000620 RID: 1568 RVA: 0x0001FE2C File Offset: 0x0001E02C
	public void DeleteKeyForInputAction(InputAction inputAction, int bindingIndex)
	{
		if (inputAction != null)
		{
			Debug.Log("Delete keybind for " + inputAction.name);
			inputAction.ChangeBinding(bindingIndex).Erase();
			this.UpdateInputActionDisplay(inputAction, true);
			this.BindKeybindOptionRowButtons(inputAction);
		}
	}

	// Token: 0x06000621 RID: 1569 RVA: 0x0001FE6F File Offset: 0x0001E06F
	public void AddBindingForInputAction(InputAction inputAction)
	{
		if (inputAction != null)
		{
			Debug.Log("Add keybind for " + inputAction.name);
			inputAction.AddBinding("", null, null, null);
			this.UpdateInputActionDisplay(inputAction, true);
			this.BindKeybindOptionRowButtons(inputAction);
		}
	}

	// Token: 0x06000622 RID: 1570 RVA: 0x0001FEA7 File Offset: 0x0001E0A7
	private void RebindComplete()
	{
		Debug.Log("Rebind complete");
		if (this.rebindingOperation != null)
		{
			this.UpdateInputActionDisplay(this.rebindingOperation.action, false);
			this.rebindingOperation.Dispose();
		}
		this.CancelListenKeyForInputAction();
		this.HideRebindInfoPanel();
	}

	// Token: 0x06000623 RID: 1571 RVA: 0x0001FEE4 File Offset: 0x0001E0E4
	private void CancelListenKeyForInputAction()
	{
		this.listenKeyForInputAction = null;
	}

	// Token: 0x06000624 RID: 1572 RVA: 0x0001FEF0 File Offset: 0x0001E0F0
	private void ResetKeybindsConfirm()
	{
		BasicConfirmDialog component = UnityEngine.Object.Instantiate<GameObject>(this.confirmDialogPrefab).GetComponent<BasicConfirmDialog>();
		component.SetText("", LocalizationHelpers.LocalizedText("confirm_reset_keybinds", Array.Empty<object>()), false);
		component.okButton.onClick.AddListener(new UnityAction(this.ResetKeyBinds));
		component.cancelButton.Select();
	}

	// Token: 0x06000625 RID: 1573 RVA: 0x0001FF4E File Offset: 0x0001E14E
	private void ResetKeyBinds()
	{
		SettingsHelper.SaveUserControls(null);
		this.LoadUserControls();
		this.GenerateKeybindingSettings();
		SettingsHelper.LoadInputs();
	}

	// Token: 0x06000626 RID: 1574 RVA: 0x0001FF68 File Offset: 0x0001E168
	private void LoadUserControls()
	{
		this.DisposeUserControls();
		this.userControls = SettingsHelper.GetUserControls();
		this.userControls.Generic.Enable();
	}

	// Token: 0x06000627 RID: 1575 RVA: 0x0001FF9C File Offset: 0x0001E19C
	public void AddToBanList()
	{
		BanItem banItem = new BanItem();
		banItem.address = this.banListAddress.text;
		banItem.name = this.banListName.text;
		banItem.type = this.banListTypeDropdown.value + GladioMoriServerType.DirectIp;
		if (!string.IsNullOrEmpty(banItem.address))
		{
			SettingsHelper.AddItemToBanList(banItem);
			this.AddItemToBanListUI(banItem);
			this.ClearBanListInputs();
		}
	}

	// Token: 0x06000628 RID: 1576 RVA: 0x00020004 File Offset: 0x0001E204
	public void ClearBanListInputs()
	{
		this.banListAddress.text = "";
		this.banListName.text = "";
	}

	// Token: 0x06000629 RID: 1577 RVA: 0x00020028 File Offset: 0x0001E228
	public void GenerateBanList()
	{
		foreach (object obj in this.banListItemsHolder)
		{
			UnityEngine.Object.Destroy(((Transform)obj).gameObject);
		}
		foreach (BanItem banItem in SettingsHelper.banList.banItems)
		{
			this.AddItemToBanListUI(banItem);
		}
	}

	// Token: 0x0600062A RID: 1578 RVA: 0x000200CC File Offset: 0x0001E2CC
	public void AddItemToBanListUI(BanItem banItem)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.banListItemPrefab);
		BanListItemRow banListItemRow = gameObject.GetComponent<BanListItemRow>();
		gameObject.transform.SetParent(this.banListItemsHolder.transform);
		banListItemRow.SetBanItem(banItem);
		banListItemRow.removeButton.onClick.AddListener(delegate()
		{
			this.RemoveItemFromBanList(banListItemRow);
		});
	}

	// Token: 0x0600062B RID: 1579 RVA: 0x00020141 File Offset: 0x0001E341
	public void RemoveItemFromBanList(BanListItemRow banItemRow)
	{
		SettingsHelper.RemoveItemFromBanList(banItemRow.banItem);
		UnityEngine.Object.Destroy(banItemRow.gameObject);
	}

	// Token: 0x0600062C RID: 1580 RVA: 0x00020159 File Offset: 0x0001E359
	private void OnDestroy()
	{
		this.DisposeUserControls();
	}

	// Token: 0x0600062D RID: 1581 RVA: 0x00020161 File Offset: 0x0001E361
	public void DisposeUserControls()
	{
		if (this.userControls != null)
		{
			this.userControls.Disable();
			this.userControls.Dispose();
		}
	}

	// Token: 0x040003DA RID: 986
	public static PlayerOptionsManager singleton;

	// Token: 0x040003DB RID: 987
	[Header("Main buttons")]
	public Button gameplaySettingsButton;

	// Token: 0x040003DC RID: 988
	public Button cameraSettingsButton;

	// Token: 0x040003DD RID: 989
	public Button graphicSettingsButton;

	// Token: 0x040003DE RID: 990
	public Button keybindingSettingsButton;

	// Token: 0x040003DF RID: 991
	public Button soundsSettingsButton;

	// Token: 0x040003E0 RID: 992
	public Button multiplayerSettingsButton;

	// Token: 0x040003E1 RID: 993
	public Button creditsSettingsButton;

	// Token: 0x040003E2 RID: 994
	[Header("Main panels")]
	public GameObject settingsPanelHolder;

	// Token: 0x040003E3 RID: 995
	public GameObject gameplaySettingsPanel;

	// Token: 0x040003E4 RID: 996
	public GameObject cameraSettingsPanel;

	// Token: 0x040003E5 RID: 997
	public GameObject graphicsSettingsPanel;

	// Token: 0x040003E6 RID: 998
	public GameObject keybindingSettingsPanel;

	// Token: 0x040003E7 RID: 999
	public GameObject soundsSettingsPanel;

	// Token: 0x040003E8 RID: 1000
	public GameObject multiplayerSettingsPanel;

	// Token: 0x040003E9 RID: 1001
	public GameObject creditsSettingsPanel;

	// Token: 0x040003EA RID: 1002
	[Header("Gameplay settings")]
	public InputField nameInput;

	// Token: 0x040003EB RID: 1003
	public Dropdown languageDropdown;

	// Token: 0x040003EC RID: 1004
	public Toggle showFPSToggle;

	// Token: 0x040003ED RID: 1005
	public Dropdown allowCustomTexturesDropdown;

	// Token: 0x040003EE RID: 1006
	public SliderAndTextSelect mouseSensitivitySlider;

	// Token: 0x040003EF RID: 1007
	public Toggle disableMouseTurning;

	// Token: 0x040003F0 RID: 1008
	public SliderAndTextSelect controllerSensitivitySlider;

	// Token: 0x040003F1 RID: 1009
	public Dropdown turnTypeDropdown;

	// Token: 0x040003F2 RID: 1010
	public Toggle recordReplayToggle;

	// Token: 0x040003F3 RID: 1011
	public Dropdown replayTexturesOverrideDropdown;

	// Token: 0x040003F4 RID: 1012
	public Dropdown bloodColourDropdown;

	// Token: 0x040003F5 RID: 1013
	public Toggle InvertCameraY;

	// Token: 0x040003F6 RID: 1014
	public Toggle timeScaleAffectCameraTurnSpeedToggle;

	// Token: 0x040003F7 RID: 1015
	public Toggle showAttackDirectionToggle;

	// Token: 0x040003F8 RID: 1016
	public ImageSelect customPlayerTextureImageSelect;

	// Token: 0x040003F9 RID: 1017
	public Toggle invertVerticalAttack;

	// Token: 0x040003FA RID: 1018
	public Toggle invertHorizontalAttack;

	// Token: 0x040003FB RID: 1019
	public Toggle invertVerticalBlock;

	// Token: 0x040003FC RID: 1020
	public Toggle invertHorizontalBlock;

	// Token: 0x040003FD RID: 1021
	[Header("Camera settings")]
	public SliderAndTextSelect cameraPositionOffsetXSlider;

	// Token: 0x040003FE RID: 1022
	public SliderAndTextSelect cameraPositionOffsetYSlider;

	// Token: 0x040003FF RID: 1023
	public SliderAndTextSelect cameraPositionOffsetZSlider;

	// Token: 0x04000400 RID: 1024
	public SliderAndTextSelect cameraTargetOffsetXSlider;

	// Token: 0x04000401 RID: 1025
	public SliderAndTextSelect cameraTargetOffsetYSlider;

	// Token: 0x04000402 RID: 1026
	public SliderAndTextSelect cameraTargetOffsetZSlider;

	// Token: 0x04000403 RID: 1027
	public SliderAndTextSelect cameraFOV;

	// Token: 0x04000404 RID: 1028
	public GameObject cameraTargetPreviewPrefab;

	// Token: 0x04000405 RID: 1029
	public PlayerCameraSettings cameraSettings;

	// Token: 0x04000406 RID: 1030
	private List<CameraSmoothFollow> cameraSmoothFollows;

	// Token: 0x04000407 RID: 1031
	public List<Image> cameraEditTransparentImages = new List<Image>();

	// Token: 0x04000408 RID: 1032
	private List<Image> allCameraEditTransparentImages = new List<Image>();

	// Token: 0x04000409 RID: 1033
	public Transform cameraSettingsHolder;

	// Token: 0x0400040A RID: 1034
	public Button resetCameraButton;

	// Token: 0x0400040B RID: 1035
	[Header("Graphic settings")]
	public Dropdown resolutionDropdown;

	// Token: 0x0400040C RID: 1036
	public Dropdown windowModeDropdown;

	// Token: 0x0400040D RID: 1037
	public Toggle vSyncToggle;

	// Token: 0x0400040E RID: 1038
	public Slider fpsLimitSlider;

	// Token: 0x0400040F RID: 1039
	public Text fpsLimitText;

	// Token: 0x04000410 RID: 1040
	public Dropdown postProcessingAADropdown;

	// Token: 0x04000411 RID: 1041
	public Dropdown msaaDropdown;

	// Token: 0x04000412 RID: 1042
	public Dropdown shadowQualityDropdown;

	// Token: 0x04000413 RID: 1043
	public Dropdown bloodQualityDropdown;

	// Token: 0x04000414 RID: 1044
	public Toggle bloomToggle;

	// Token: 0x04000415 RID: 1045
	public Toggle depthOfFieldToggle;

	// Token: 0x04000416 RID: 1046
	[Header("Keybinding settings")]
	public UserControls userControls;

	// Token: 0x04000417 RID: 1047
	public GameObject keybindingSettingsContainer;

	// Token: 0x04000418 RID: 1048
	public GameObject keybindRowPrefab;

	// Token: 0x04000419 RID: 1049
	public GameObject titleRowPrefab;

	// Token: 0x0400041A RID: 1050
	public Button resetKeyBindsButton;

	// Token: 0x0400041B RID: 1051
	[Header("Audio settings")]
	public Slider masterVolumeSlider;

	// Token: 0x0400041C RID: 1052
	public Slider effectsVolumeSlider;

	// Token: 0x0400041D RID: 1053
	public Slider musicVolumeSlider;

	// Token: 0x0400041E RID: 1054
	public Slider voiceChatVolumeSlider;

	// Token: 0x0400041F RID: 1055
	public Text masterVolumeText;

	// Token: 0x04000420 RID: 1056
	public Text effectsVolumeText;

	// Token: 0x04000421 RID: 1057
	public Text musicVolumeText;

	// Token: 0x04000422 RID: 1058
	public Text voiceChatVolumeText;

	// Token: 0x04000423 RID: 1059
	public AudioMixer audioMixer;

	// Token: 0x04000424 RID: 1060
	public AudioClip testAudio;

	// Token: 0x04000425 RID: 1061
	[Header("Multiplayer settings")]
	public Dropdown chatDropdown;

	// Token: 0x04000426 RID: 1062
	public InputField banListAddress;

	// Token: 0x04000427 RID: 1063
	public InputField banListName;

	// Token: 0x04000428 RID: 1064
	public Dropdown banListTypeDropdown;

	// Token: 0x04000429 RID: 1065
	public Button banListAdd;

	// Token: 0x0400042A RID: 1066
	public Transform banListItemsHolder;

	// Token: 0x0400042B RID: 1067
	public GameObject banListItemPrefab;

	// Token: 0x0400042C RID: 1068
	[Header("Credits settings")]
	public TMP_Text creditsText;

	// Token: 0x0400042D RID: 1069
	public TextAsset creditsTextAsset;

	// Token: 0x0400042E RID: 1070
	[Header("Misc")]
	public Button saveButton;

	// Token: 0x0400042F RID: 1071
	public Button cancelButton;

	// Token: 0x04000430 RID: 1072
	public GameObject confirmDialogPrefab;

	// Token: 0x04000431 RID: 1073
	public GameObject infoDialogPrefab;

	// Token: 0x04000432 RID: 1074
	private List<ResolutionOption> resolutions;

	// Token: 0x04000433 RID: 1075
	private IDisposable m_EventListener;

	// Token: 0x04000434 RID: 1076
	private float lastTestSoundPlayed;

	// Token: 0x04000435 RID: 1077
	private List<KeybindOptionRow> keybindOptionRows = new List<KeybindOptionRow>();

	// Token: 0x04000436 RID: 1078
	private InputAction listenKeyForInputAction;

	// Token: 0x04000437 RID: 1079
	private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

	// Token: 0x04000438 RID: 1080
	private BasicInfoDialog rebindInfoPanel;

	// Token: 0x020000B0 RID: 176
	public enum SettingPanel
	{
		// Token: 0x0400043A RID: 1082
		Gameplay,
		// Token: 0x0400043B RID: 1083
		Camera,
		// Token: 0x0400043C RID: 1084
		Graphics,
		// Token: 0x0400043D RID: 1085
		Keybindings,
		// Token: 0x0400043E RID: 1086
		Sound,
		// Token: 0x0400043F RID: 1087
		Credits,
		// Token: 0x04000440 RID: 1088
		Multiplayer
	}
}
