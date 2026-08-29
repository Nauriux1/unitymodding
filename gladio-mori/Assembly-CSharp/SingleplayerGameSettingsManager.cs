using System;
using System.Collections.Generic;
using BasicUI;
using UnityEngine;
using UnityEngine.UI;
using Utils;

// Token: 0x0200016F RID: 367
public class SingleplayerGameSettingsManager : MonoBehaviour
{
	// Token: 0x06000BAB RID: 2987 RVA: 0x000382A8 File Offset: 0x000364A8
	private void Start()
	{
		SingleplayerGameSettingsManager.singleton = this;
		this.startButton.onClick.AddListener(delegate()
		{
			this.NavigateForward();
		});
		this.backButton.onClick.AddListener(delegate()
		{
			this.NavigateBack();
		});
		this.userControls = SettingsHelper.GetUserControls();
		this.userControls.Generic.Enable();
		this.UpdateDifficultyButtonsImages();
		this.UpdateDifficultyButtonState();
		this.timeScaleMinSelect.ValueChanged += delegate(object <p0>, EventArgs <p1>)
		{
			this.TimeScaleMinChanged(this.timeScaleMinSelect.value);
		};
		this.useDismembermentToggle.onValueChanged.AddListener(delegate(bool <p0>)
		{
			this.UseDismembermentChanged(this.useDismembermentToggle.isOn);
		});
		using (List<SingleplayerDifficultyItem>.Enumerator enumerator = this.singleplayerDifficultyItems.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SingleplayerDifficultyItem singleplayerDifficultyItem = enumerator.Current;
				singleplayerDifficultyItem.button.onClick.AddListener(delegate()
				{
					this.SetDifficulty(singleplayerDifficultyItem.singlePlayerDifficultyType);
				});
			}
		}
		this.GenerateSettings();
		this.UpdateDifficultyDescription();
	}

	// Token: 0x06000BAC RID: 2988 RVA: 0x000383D0 File Offset: 0x000365D0
	public void GenerateSettings()
	{
		DifficultyObject currentDifficultyObject = SingleplayerManager.singleton.GetCurrentDifficultyObject();
		if (currentDifficultyObject != null)
		{
			this.timeScaleMinSelect.buttonOptions.Clear();
			foreach (ButtonOption buttonOption in this.timeScaleOptions)
			{
				if (buttonOption.optionFloatValue > currentDifficultyObject.minTimeScale || Generic.FloatEquals(buttonOption.optionFloatValue, currentDifficultyObject.minTimeScale))
				{
					this.timeScaleMinSelect.buttonOptions.Add(buttonOption);
				}
			}
			bool isOnWithoutNotify = true;
			float num = 0.25f;
			if (IGameSettingsManager.singleton != null)
			{
				num = IGameSettingsManager.singleton.TimeScaleMin;
				isOnWithoutNotify = IGameSettingsManager.singleton.UseDismemberment;
			}
			if (num < currentDifficultyObject.minTimeScale)
			{
				num = currentDifficultyObject.minTimeScale;
			}
			this.timeScaleMinSelect.SetCurrentValue(new ButtonOption
			{
				optionFloatValue = num
			});
			this.useDismembermentToggle.SetIsOnWithoutNotify(isOnWithoutNotify);
		}
	}

	// Token: 0x06000BAD RID: 2989 RVA: 0x000384D4 File Offset: 0x000366D4
	public void TimeScaleMinChanged(int timeScaleMinValue)
	{
		ButtonOption getCurrentValue = this.timeScaleMinSelect.GetCurrentValue;
		if (getCurrentValue != null)
		{
			IGameSettingsManager.singleton.TimeScaleMin = getCurrentValue.optionFloatValue;
		}
	}

	// Token: 0x06000BAE RID: 2990 RVA: 0x00038500 File Offset: 0x00036700
	public void UseDismembermentChanged(bool value)
	{
		if (IGameSettingsManager.singleton != null)
		{
			IGameSettingsManager.singleton.UseDismemberment = value;
		}
	}

	// Token: 0x06000BAF RID: 2991 RVA: 0x00038514 File Offset: 0x00036714
	private void Update()
	{
		if (this.userControls.Generic.Back.WasPerformedThisFrame())
		{
			this.NavigateBack();
		}
	}

	// Token: 0x06000BB0 RID: 2992 RVA: 0x00038541 File Offset: 0x00036741
	private void NavigateBack()
	{
		if (GeneralManager.AllowBackNavigation(null))
		{
			SceneManagerWithParameters.LoadScene("MainMenu", null, false, false);
		}
	}

	// Token: 0x06000BB1 RID: 2993 RVA: 0x00038558 File Offset: 0x00036758
	private void NavigateForward()
	{
		SingleplayerManager.singleton.OpenLobbySingleplayer();
	}

	// Token: 0x06000BB2 RID: 2994 RVA: 0x00038564 File Offset: 0x00036764
	private void OnDestroy()
	{
		this.DisposeUserControls();
	}

	// Token: 0x06000BB3 RID: 2995 RVA: 0x0003856C File Offset: 0x0003676C
	public void DisposeUserControls()
	{
		if (this.userControls != null)
		{
			this.userControls.Disable();
			this.userControls.Dispose();
		}
	}

	// Token: 0x06000BB4 RID: 2996 RVA: 0x0003858C File Offset: 0x0003678C
	public void UpdateDifficultyButtonsImages()
	{
		int num = 0;
		foreach (SingleplayerDifficultyItem singleplayerDifficultyItem in this.singleplayerDifficultyItems)
		{
			singleplayerDifficultyItem.SetRawImage(SingleplayerManager.singleton.difficultyFightItems[num].previewImage);
			num++;
		}
	}

	// Token: 0x06000BB5 RID: 2997 RVA: 0x000385F8 File Offset: 0x000367F8
	public void UpdateDifficultyButtonState()
	{
		int num = 0;
		foreach (SingleplayerDifficultyItem singleplayerDifficultyItem in this.singleplayerDifficultyItems)
		{
			if (SingleplayerManager.singleton.GetDifficulty() == singleplayerDifficultyItem.singlePlayerDifficultyType)
			{
				UIHelpers.SetButtonColor(this.singleplayerDifficultyItems[num].button, ButtonState.Selected, null, null);
			}
			else
			{
				UIHelpers.SetButtonColor(this.singleplayerDifficultyItems[num].button, ButtonState.Basic, UISettings._basicBackgroundColor, null);
			}
			num++;
		}
	}

	// Token: 0x06000BB6 RID: 2998 RVA: 0x00038694 File Offset: 0x00036894
	public void SetDifficulty(SinglePlayerDifficultyType newDifficulty)
	{
		SingleplayerManager.singleton.SetDifficulty(newDifficulty);
		this.UpdateDifficultyButtonState();
		this.GenerateSettings();
	}

	// Token: 0x06000BB7 RID: 2999 RVA: 0x000386B0 File Offset: 0x000368B0
	public void UpdateDifficultyDescription()
	{
		DifficultyObject currentDifficultyObject = SingleplayerManager.singleton.GetCurrentDifficultyObject();
		if (currentDifficultyObject != null)
		{
			this.difficultyDescriptionTitle.text = currentDifficultyObject.difficulty.GetDescription();
			this.difficultyDescriptionText.text = LocalizationHelpers.LocalizedText("txt_difficulty_description", new object[]
			{
				currentDifficultyObject.maxPoints.ToString(),
				currentDifficultyObject.minTimeScale.ToString("0.00") + "x"
			});
		}
	}

	// Token: 0x0400083E RID: 2110
	public static SingleplayerGameSettingsManager singleton;

	// Token: 0x0400083F RID: 2111
	public Button backButton;

	// Token: 0x04000840 RID: 2112
	public Button startButton;

	// Token: 0x04000841 RID: 2113
	public UserControls userControls;

	// Token: 0x04000842 RID: 2114
	public List<SingleplayerDifficultyItem> singleplayerDifficultyItems;

	// Token: 0x04000843 RID: 2115
	public ButtonOptionSelect timeScaleMinSelect;

	// Token: 0x04000844 RID: 2116
	public Text difficultyDescriptionTitle;

	// Token: 0x04000845 RID: 2117
	public Text difficultyDescriptionText;

	// Token: 0x04000846 RID: 2118
	public Toggle useDismembermentToggle;

	// Token: 0x04000847 RID: 2119
	private List<ButtonOption> timeScaleOptions = new List<ButtonOption>
	{
		new ButtonOption
		{
			optionText = "1.00x",
			optionFloatValue = 1f
		},
		new ButtonOption
		{
			optionText = "0.25x",
			optionFloatValue = 0.25f
		},
		new ButtonOption
		{
			optionText = "0.50x",
			optionFloatValue = 0.5f
		},
		new ButtonOption
		{
			optionText = "0.75x",
			optionFloatValue = 0.75f
		}
	};
}
