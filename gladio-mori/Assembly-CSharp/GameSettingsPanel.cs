using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using Utils;

// Token: 0x020001D6 RID: 470
public class GameSettingsPanel : MonoBehaviour
{
	// Token: 0x17000197 RID: 407
	// (get) Token: 0x06000DF8 RID: 3576 RVA: 0x00046609 File Offset: 0x00044809
	private bool isLobby
	{
		get
		{
			return this.isMultiplayer && this.roomManager != null && this.roomManager.GetGladioMoriServerType() == GladioMoriServerType.Steam;
		}
	}

	// Token: 0x06000DF9 RID: 3577 RVA: 0x00046634 File Offset: 0x00044834
	private void Start()
	{
		GameSettingsPanel.singleton = this;
		this.GetMultiplayerSetup();
		this.LoadSettings();
		this.DisplaySettings();
		this.lobbyNameInputField.onEndEdit.AddListener(delegate(string <p0>)
		{
			this.LobbyNameChanged(this.lobbyNameInputField.text);
		});
		this.lobbyPrivacyTypeSelect.ValueChanged += delegate(object <p0>, EventArgs <p1>)
		{
			this.LobbyPrivacyTypeChanged(this.lobbyPrivacyTypeSelect.value);
		};
		this.allowedMovesetTypesSelect.ValueChanged += delegate(object <p0>, EventArgs <p1>)
		{
			this.AllowedMovesetTypeChanged(this.allowedMovesetTypesSelect.value);
		};
		this.gameTypeSelect.ValueChanged += delegate(object <p0>, EventArgs <p1>)
		{
			this.GameTypeChanged();
		};
		this.mapSelect.ValueChanged += delegate(object <p0>, EventArgs <p1>)
		{
			this.MapChanged(0);
		};
		this.aiAmountSelect.ValueChanged += delegate(object <p0>, EventArgs <p1>)
		{
			this.AiAmountChanged(this.aiAmountSelect.value);
		};
		this.timeScaleMinSelect.ValueChanged += delegate(object <p0>, EventArgs <p1>)
		{
			this.TimeScaleMinChanged(this.timeScaleMinSelect.value);
		};
		this.equipmentPointSelect.ValueChanged += delegate(object <p0>, EventArgs <p1>)
		{
			this.EquipmentPointsChanged(this.equipmentPointSelect.value);
		};
		this.rollingFeetToggle.onValueChanged.AddListener(delegate(bool <p0>)
		{
			this.RollingFeetChanged(this.rollingFeetToggle.isOn);
		});
		this.allowEquipmentEdit.onValueChanged.AddListener(delegate(bool <p0>)
		{
			this.AllowEquipmentEditChanged(this.allowEquipmentEdit.isOn);
		});
		this.useStaminaToggle.onValueChanged.AddListener(delegate(bool <p0>)
		{
			this.UseStaminaChanged(this.useStaminaToggle.isOn);
		});
		this.useDismembermentToggle.onValueChanged.AddListener(delegate(bool <p0>)
		{
			this.UseDismembermentChanged(this.useDismembermentToggle.isOn);
		});
		this.welcomeText.textChanged += this.WelcomeTextChanged;
		this.welcomeText.editorOpened += this.WelcomeTextOpened;
	}

	// Token: 0x06000DFA RID: 3578 RVA: 0x000467B4 File Offset: 0x000449B4
	private void OnDestroy()
	{
		this.welcomeText.textChanged -= this.WelcomeTextChanged;
		this.welcomeText.editorOpened -= this.WelcomeTextOpened;
	}

	// Token: 0x06000DFB RID: 3579 RVA: 0x000467E4 File Offset: 0x000449E4
	private void LoadSettings()
	{
		foreach (MapItem mapItem in this.mapList)
		{
			this.mapSelect.buttonOptions.Add(new ButtonOption
			{
				optionText = mapItem.mapName,
				optionValue = mapItem.mapName
			});
		}
		this.equipmentPointSelect.textValueForZeroDisplay = LocalizationSettings.StringDatabase.GetLocalizedString(SettingsHelper.localizationTableName, "gamesetting_equipment_point_infinite", null, FallbackBehavior.UseProjectSettings, Array.Empty<object>());
		for (int i = 0; i <= 3; i++)
		{
			this.aiAmountSelect.buttonOptions.Add(new ButtonOption
			{
				optionText = i.ToString()
			});
		}
		this.timeScaleMinSelect.buttonOptions.Add(new ButtonOption
		{
			optionText = "1.00x",
			optionFloatValue = -1f
		});
		this.timeScaleMinSelect.buttonOptions.Add(new ButtonOption
		{
			optionText = "0.25x",
			optionFloatValue = 0.25f
		});
		this.timeScaleMinSelect.buttonOptions.Add(new ButtonOption
		{
			optionText = "0.50x",
			optionFloatValue = 0.5f
		});
		this.timeScaleMinSelect.buttonOptions.Add(new ButtonOption
		{
			optionText = "0.75x",
			optionFloatValue = 0.75f
		});
		this.lobbyPrivacyTypeSelect.buttonOptions.Add(new ButtonOption
		{
			optionText = LobbyPrivacyType.privateLobby.GetDescription(),
			optionValue = 0.ToString()
		});
		this.lobbyPrivacyTypeSelect.buttonOptions.Add(new ButtonOption
		{
			optionText = LobbyPrivacyType.friendsOnlyLobby.GetDescription(),
			optionValue = 1.ToString()
		});
		this.lobbyPrivacyTypeSelect.buttonOptions.Add(new ButtonOption
		{
			optionText = LobbyPrivacyType.publicLobby.GetDescription(),
			optionValue = 2.ToString()
		});
		foreach (object obj in Enum.GetValues(typeof(AllowedMovesetTypes)))
		{
			AllowedMovesetTypes allowedMovesetTypes = (AllowedMovesetTypes)obj;
			this.allowedMovesetTypesSelect.buttonOptions.Add(new ButtonOption
			{
				optionText = allowedMovesetTypes.GetDescription(),
				optionIntValue = (int)allowedMovesetTypes
			});
		}
		foreach (object obj2 in Enum.GetValues(typeof(GameTypes)))
		{
			GameTypes gameTypes = (GameTypes)obj2;
			this.gameTypeSelect.buttonOptions.Add(new ButtonOption
			{
				optionText = gameTypes.GetDescription(),
				optionIntValue = (int)gameTypes
			});
		}
		if (IGameSettingsManager.singleton != null)
		{
			IGameSettingsManager.singleton.LoadGameSettings();
		}
		this.UpdateSettingValues();
		this.UpdateMapBackground();
	}

	// Token: 0x06000DFC RID: 3580 RVA: 0x00046B28 File Offset: 0x00044D28
	public void UpdateSettingValues()
	{
		string textWithoutNotify = "";
		string selectedMap = "";
		int currentValue = 0;
		int value = 0;
		float optionFloatValue = 0.25f;
		GameTypes currentIntValue = GameTypes.Classic;
		AllowedMovesetTypes currentIntValue2 = AllowedMovesetTypes.All;
		bool isOn = true;
		bool isOn2 = true;
		bool isOn3 = true;
		LobbyPrivacyType value2 = LobbyPrivacyType.privateLobby;
		if (IGameSettingsManager.singleton != null)
		{
			textWithoutNotify = this.GetLobbyName();
			this.rollingFeetToggle.isOn = IGameSettingsManager.singleton.GetRollingFeet();
			selectedMap = IGameSettingsManager.singleton.SelectedMap;
			currentValue = IGameSettingsManager.singleton.EquipmentPoints;
			value = IGameSettingsManager.singleton.AiAmount;
			optionFloatValue = IGameSettingsManager.singleton.TimeScaleMin;
			value2 = this.GetLobbyPrivacyType();
			currentIntValue2 = IGameSettingsManager.singleton.AllowedMovesetTypes;
			isOn = IGameSettingsManager.singleton.AllowEquipmentEdit;
			currentIntValue = IGameSettingsManager.singleton.GameType;
			isOn2 = IGameSettingsManager.singleton.UseStamina;
			isOn3 = IGameSettingsManager.singleton.UseDismemberment;
		}
		if (this.isMultiplayer && this.roomManager != null && (this.roomManager.mode == NetworkManagerMode.Host || this.roomManager.mode == NetworkManagerMode.ServerOnly))
		{
			selectedMap = this.roomManager.GameplayScene;
		}
		if (!string.IsNullOrWhiteSpace(selectedMap))
		{
			int value3 = this.mapList.IndexOf((from x in this.mapList
			where selectedMap.Contains(x.mapName.Replace(" ", ""))
			select x).FirstOrDefault<MapItem>());
			this.mapSelect.value = value3;
		}
		this.mapSelect.RefreshShownValue();
		this.lobbyNameInputField.SetTextWithoutNotify(textWithoutNotify);
		this.equipmentPointSelect.SetCurrentValue(currentValue);
		this.aiAmountSelect.value = value;
		this.aiAmountSelect.RefreshShownValue();
		this.timeScaleMinSelect.SetCurrentValue(new ButtonOption
		{
			optionFloatValue = optionFloatValue
		});
		this.allowedMovesetTypesSelect.SetCurrentIntValue((int)currentIntValue2);
		this.gameTypeSelect.SetCurrentIntValue((int)currentIntValue);
		this.allowEquipmentEdit.isOn = isOn;
		this.useStaminaToggle.isOn = isOn2;
		this.useDismembermentToggle.isOn = isOn3;
		this.lobbyPrivacyTypeSelect.value = (int)value2;
		this.lobbyPrivacyTypeSelect.RefreshShownValue();
		this.CheckForcedSettingValues();
	}

	// Token: 0x06000DFD RID: 3581 RVA: 0x00046D3C File Offset: 0x00044F3C
	public void GetMultiplayerSetup()
	{
		if (NetworkManager.singleton != null && NetworkManager.singleton.isNetworkActive)
		{
			this.roomManager = (MultiplayerRoomManager)NetworkManager.singleton;
			this.isMultiplayer = true;
			if (this.roomManager.mode == NetworkManagerMode.ClientOnly)
			{
				this.disableEdit = true;
			}
		}
	}

	// Token: 0x06000DFE RID: 3582 RVA: 0x00046D90 File Offset: 0x00044F90
	private void DisplaySettings()
	{
		if (this.singleplayerSettings != null)
		{
			foreach (GameObject gameObject in this.singleplayerSettings)
			{
				gameObject.SetActive(!this.isMultiplayer);
			}
		}
		if (this.multiplayerSettings != null)
		{
			foreach (GameObject gameObject2 in this.multiplayerSettings)
			{
				gameObject2.SetActive(this.isMultiplayer);
			}
		}
		if (this.lobbySettings != null)
		{
			foreach (GameObject gameObject3 in this.lobbySettings)
			{
				gameObject3.SetActive(this.isLobby);
			}
		}
		if (this.disableEdit)
		{
			this.DisableGameSettings();
		}
		this.UpdateNavigation();
	}

	// Token: 0x06000DFF RID: 3583 RVA: 0x00046EA0 File Offset: 0x000450A0
	private void DisableGameSettings()
	{
		List<IDisableableGameSetting> list = new List<IDisableableGameSetting>();
		foreach (object obj in this.settingsHolder.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.gameObject.activeInHierarchy)
			{
				IDisableableGameSetting component = transform.GetComponent<IDisableableGameSetting>();
				if (component != null)
				{
					list.Add(component);
				}
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			list[i].DisableGameSetting();
		}
	}

	// Token: 0x06000E00 RID: 3584 RVA: 0x00046F40 File Offset: 0x00045140
	private void LobbyNameChanged(string lobbyName)
	{
		if (SteamManager.steamManager != null)
		{
			lobbyName = GeneralManager.singleton.FilterBadWords(lobbyName, false);
			if (!SteamManager.steamManager.SetLobbyData("lobbyName", lobbyName))
			{
				this.lobbyNameInputField.SetTextWithoutNotify(SteamManager.steamManager.GetLobbyDataString("lobbyName"));
				return;
			}
			this.lobbyNameInputField.SetTextWithoutNotify(lobbyName);
			if (GameSettingsManagerMultiplayer.singleton != null)
			{
				GameSettingsManagerMultiplayer.singleton.LobbyName = lobbyName;
			}
		}
	}

	// Token: 0x06000E01 RID: 3585 RVA: 0x00046FBC File Offset: 0x000451BC
	private string GetLobbyName()
	{
		string text = "";
		if (SteamManager.steamManager != null)
		{
			if (GameSettingsManagerMultiplayer.singleton != null && GameSettingsManagerMultiplayer.singleton.isClientOnly)
			{
				text = GameSettingsManagerMultiplayer.singleton.LobbyName;
			}
			else
			{
				text = SteamManager.steamManager.GetLobbyDataString("lobbyName");
				if (GameSettingsManagerMultiplayer.singleton != null)
				{
					GameSettingsManagerMultiplayer.singleton.LobbyName = text;
				}
			}
		}
		return text;
	}

	// Token: 0x06000E02 RID: 3586 RVA: 0x0004702C File Offset: 0x0004522C
	public void LobbyPrivacyTypeChanged(int privacyType)
	{
		if (SteamManager.steamManager != null)
		{
			if (!SteamManager.steamManager.SetLobbyPrivacyType((LobbyPrivacyType)privacyType))
			{
				this.lobbyPrivacyTypeSelect.SetCurrentValue((int)this.GetLobbyPrivacyType());
				return;
			}
			if (GameSettingsManagerMultiplayer.singleton != null)
			{
				GameSettingsManagerMultiplayer.singleton.LobbyPrivacyType = (LobbyPrivacyType)privacyType;
			}
		}
	}

	// Token: 0x06000E03 RID: 3587 RVA: 0x0004707D File Offset: 0x0004527D
	private LobbyPrivacyType GetLobbyPrivacyType()
	{
		if (!(SteamManager.steamManager != null))
		{
			return LobbyPrivacyType.privateLobby;
		}
		if (GameSettingsManagerMultiplayer.singleton != null && GameSettingsManagerMultiplayer.singleton.isClientOnly)
		{
			return GameSettingsManagerMultiplayer.singleton.LobbyPrivacyType;
		}
		return SteamManager.steamManager.GetLobbyPrivacyType();
	}

	// Token: 0x06000E04 RID: 3588 RVA: 0x000470BC File Offset: 0x000452BC
	public void MapChanged(int mapValue)
	{
		string text = this.mapSelect.GetCurrentValue.optionValue.Replace(" ", "");
		if (!string.IsNullOrEmpty(text))
		{
			if (this.isMultiplayer)
			{
				if (this.roomManager.mode == NetworkManagerMode.ServerOnly || this.roomManager.mode == NetworkManagerMode.Host)
				{
					if (this.roomManager.mode == NetworkManagerMode.Host)
					{
						this.roomManager.SetGameplayScene("map_" + text);
					}
					if (IGameSettingsManager.singleton != null)
					{
						IGameSettingsManager.singleton.SelectedMap = "map_" + text;
					}
				}
			}
			else if (IGameSettingsManager.singleton != null)
			{
				IGameSettingsManager.singleton.SelectedMap = "map_" + text;
			}
		}
		this.UpdateMapBackground();
	}

	// Token: 0x06000E05 RID: 3589 RVA: 0x0004717C File Offset: 0x0004537C
	public void UpdateMapBackground()
	{
		if (this.mapSelect.GetCurrentValue != null)
		{
			MapItem mapItem = (from x in this.mapList
			where x.mapName == this.mapSelect.GetCurrentValue.optionValue
			select x).FirstOrDefault<MapItem>();
			if (mapItem != null)
			{
				GameObject gameObject = GameObject.Find("BackgroundRawImage");
				if (gameObject != null)
				{
					RawImage component = gameObject.GetComponent<RawImage>();
					if (component != null)
					{
						component.texture = mapItem.backgroundImage;
					}
				}
			}
		}
	}

	// Token: 0x06000E06 RID: 3590 RVA: 0x000471E8 File Offset: 0x000453E8
	public void EquipmentPointsChanged(int equipmentPointValue)
	{
		int value = this.equipmentPointSelect.value;
		if (IGameSettingsManager.singleton != null)
		{
			IGameSettingsManager.singleton.EquipmentPoints = value;
		}
	}

	// Token: 0x06000E07 RID: 3591 RVA: 0x00047213 File Offset: 0x00045413
	public void AiAmountChanged(int aiAmountValue)
	{
		IGameSettingsManager.singleton.AiAmount = aiAmountValue;
	}

	// Token: 0x06000E08 RID: 3592 RVA: 0x00047220 File Offset: 0x00045420
	public void TimeScaleMinChanged(int timeScaleMinValue)
	{
		ButtonOption getCurrentValue = this.timeScaleMinSelect.GetCurrentValue;
		if (getCurrentValue != null)
		{
			IGameSettingsManager.singleton.TimeScaleMin = getCurrentValue.optionFloatValue;
		}
	}

	// Token: 0x06000E09 RID: 3593 RVA: 0x0004724C File Offset: 0x0004544C
	public void AllowedMovesetTypeChanged(int value)
	{
		ButtonOption getCurrentValue = this.allowedMovesetTypesSelect.GetCurrentValue;
		if (getCurrentValue != null)
		{
			IGameSettingsManager.singleton.AllowedMovesetTypes = (AllowedMovesetTypes)getCurrentValue.optionIntValue;
		}
		this.CheckForcedSettingValues();
	}

	// Token: 0x06000E0A RID: 3594 RVA: 0x00047280 File Offset: 0x00045480
	public void GameTypeChanged()
	{
		ButtonOption getCurrentValue = this.gameTypeSelect.GetCurrentValue;
		if (getCurrentValue != null)
		{
			IGameSettingsManager.singleton.GameType = (GameTypes)getCurrentValue.optionIntValue;
		}
		this.CheckForcedSettingValues();
	}

	// Token: 0x06000E0B RID: 3595 RVA: 0x000472B4 File Offset: 0x000454B4
	private void WelcomeTextOpened(object sender, BasicTextConfirmDialog dialog)
	{
		if (dialog != null)
		{
			dialog.SetText("", LocalizationHelpers.LocalizedText("gamesetting_welcome_message", Array.Empty<object>()), this.disableEdit);
			dialog.SetMaxLength(2000);
			if (this.disableEdit)
			{
				dialog.SetReadOnly();
			}
			if (IGameSettingsManager.singleton != null)
			{
				dialog.SetValue(IGameSettingsManager.singleton.WelcomeMessage);
			}
			dialog.okButton.Select();
		}
	}

	// Token: 0x06000E0C RID: 3596 RVA: 0x00047325 File Offset: 0x00045525
	private void WelcomeTextChanged(object sender, string welcomeText)
	{
		if (!this.disableEdit)
		{
			welcomeText = GeneralManager.singleton.FilterBadWords(welcomeText, false);
			if (GameSettingsManagerMultiplayer.singleton != null)
			{
				GameSettingsManagerMultiplayer.singleton.WelcomeMessage = welcomeText;
			}
		}
	}

	// Token: 0x06000E0D RID: 3597 RVA: 0x00047358 File Offset: 0x00045558
	public void CheckForcedSettingValues()
	{
		if (this.disableEdit)
		{
			return;
		}
		if (IGameSettingsManager.singleton.AllowedMovesetTypes == AllowedMovesetTypes.All)
		{
			if (!this.allowEquipmentEdit.isOn)
			{
				this.allowEquipmentEdit.isOn = true;
			}
			this.allowEquipmentEdit.interactable = false;
		}
		else
		{
			this.allowEquipmentEdit.interactable = true;
		}
		if (IGameSettingsManager.singleton.GameType == GameTypes.Legacy)
		{
			if (this.useStaminaToggle.isOn)
			{
				this.useStaminaToggle.SetIsOnWithoutNotify(false);
			}
			this.useStaminaToggle.interactable = false;
			return;
		}
		this.useStaminaToggle.SetIsOnWithoutNotify(IGameSettingsManager.singleton.UseStamina);
		this.useStaminaToggle.interactable = true;
	}

	// Token: 0x06000E0E RID: 3598 RVA: 0x00047401 File Offset: 0x00045601
	public void AllowEquipmentEditChanged(bool value)
	{
		if (IGameSettingsManager.singleton != null)
		{
			IGameSettingsManager.singleton.AllowEquipmentEdit = value;
		}
	}

	// Token: 0x06000E0F RID: 3599 RVA: 0x00047415 File Offset: 0x00045615
	public void UseStaminaChanged(bool value)
	{
		if (IGameSettingsManager.singleton != null)
		{
			IGameSettingsManager.singleton.UseStamina = value;
		}
	}

	// Token: 0x06000E10 RID: 3600 RVA: 0x00038500 File Offset: 0x00036700
	public void UseDismembermentChanged(bool value)
	{
		if (IGameSettingsManager.singleton != null)
		{
			IGameSettingsManager.singleton.UseDismemberment = value;
		}
	}

	// Token: 0x06000E11 RID: 3601 RVA: 0x00047429 File Offset: 0x00045629
	public void RollingFeetChanged(bool rollingFeet)
	{
		if (IGameSettingsManager.singleton != null)
		{
			IGameSettingsManager.singleton.SetRollingFeet(rollingFeet);
		}
	}

	// Token: 0x06000E12 RID: 3602 RVA: 0x00047440 File Offset: 0x00045640
	public INavigationListOption UpdateNavigation()
	{
		List<INavigationListOption> list = new List<INavigationListOption>();
		foreach (object obj in this.settingsHolder.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.gameObject.activeInHierarchy)
			{
				INavigationListOption component = transform.GetComponent<INavigationListOption>();
				if (component != null)
				{
					list.Add(component);
				}
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			INavigationListOption upItem = null;
			INavigationListOption downItem = null;
			INavigationListOption navigationListOption = list[i];
			if (i != 0)
			{
				upItem = list[i - 1];
			}
			if (i < list.Count - 1)
			{
				downItem = list[i + 1];
			}
			navigationListOption.SetNavigation(upItem, downItem, this.downNavigation, this.rightNavigation);
		}
		return list.LastOrDefault<INavigationListOption>();
	}

	// Token: 0x04000A06 RID: 2566
	public static GameSettingsPanel singleton;

	// Token: 0x04000A07 RID: 2567
	public InputField lobbyNameInputField;

	// Token: 0x04000A08 RID: 2568
	public ButtonOptionSelect lobbyPrivacyTypeSelect;

	// Token: 0x04000A09 RID: 2569
	public ButtonOptionSelect mapSelect;

	// Token: 0x04000A0A RID: 2570
	public ButtonOptionSelect aiAmountSelect;

	// Token: 0x04000A0B RID: 2571
	public ButtonOptionSelect timeScaleMinSelect;

	// Token: 0x04000A0C RID: 2572
	public ButtonOptionSelect allowedMovesetTypesSelect;

	// Token: 0x04000A0D RID: 2573
	public Toggle allowEquipmentEdit;

	// Token: 0x04000A0E RID: 2574
	public ButtonOptionSelect gameTypeSelect;

	// Token: 0x04000A0F RID: 2575
	public Toggle useStaminaToggle;

	// Token: 0x04000A10 RID: 2576
	public Toggle useDismembermentToggle;

	// Token: 0x04000A11 RID: 2577
	public OpenTextDialogSettingItem welcomeText;

	// Token: 0x04000A12 RID: 2578
	public List<MapItem> mapList = new List<MapItem>();

	// Token: 0x04000A13 RID: 2579
	public Toggle rollingFeetToggle;

	// Token: 0x04000A14 RID: 2580
	public ButtonIntOptionSelect equipmentPointSelect;

	// Token: 0x04000A15 RID: 2581
	private MultiplayerRoomManager roomManager;

	// Token: 0x04000A16 RID: 2582
	private bool isMultiplayer;

	// Token: 0x04000A17 RID: 2583
	public GameObject settingsHolder;

	// Token: 0x04000A18 RID: 2584
	public List<GameObject> singleplayerSettings;

	// Token: 0x04000A19 RID: 2585
	public List<GameObject> multiplayerSettings;

	// Token: 0x04000A1A RID: 2586
	public List<GameObject> lobbySettings;

	// Token: 0x04000A1B RID: 2587
	public List<GameObject> alwaysHideInDemo;

	// Token: 0x04000A1C RID: 2588
	public Selectable downNavigation;

	// Token: 0x04000A1D RID: 2589
	public Selectable rightNavigation;

	// Token: 0x04000A1E RID: 2590
	public bool disableEdit;
}
