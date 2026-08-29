using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Utils;

// Token: 0x020001CC RID: 460
public class ButtonOptionSelect : MonoBehaviour, INavigationListOption, IDisableableGameSetting
{
	// Token: 0x14000003 RID: 3
	// (add) Token: 0x06000DAB RID: 3499 RVA: 0x00045A4C File Offset: 0x00043C4C
	// (remove) Token: 0x06000DAC RID: 3500 RVA: 0x00045A84 File Offset: 0x00043C84
	public event EventHandler ValueChanged;

	// Token: 0x06000DAD RID: 3501 RVA: 0x00045AB9 File Offset: 0x00043CB9
	private void Start()
	{
		this.nextButton.onClick.AddListener(delegate()
		{
			this.ChangeValue(1);
		});
		this.previousButton.onClick.AddListener(delegate()
		{
			this.ChangeValue(-1);
		});
	}

	// Token: 0x06000DAE RID: 3502 RVA: 0x0000777A File Offset: 0x0000597A
	private void Update()
	{
	}

	// Token: 0x06000DAF RID: 3503 RVA: 0x00045AF4 File Offset: 0x00043CF4
	public void ChangeValue(int valueChange)
	{
		this.value += valueChange;
		if (this.value < 0)
		{
			this.value = this.buttonOptions.Count - 1;
		}
		else if (this.value >= this.buttonOptions.Count)
		{
			this.value = 0;
		}
		this.ValueChanged(this, EventArgs.Empty);
		this.RefreshShownValue();
	}

	// Token: 0x17000192 RID: 402
	// (get) Token: 0x06000DB0 RID: 3504 RVA: 0x00045B5E File Offset: 0x00043D5E
	public ButtonOption GetCurrentValue
	{
		get
		{
			if (this.value < 0 || this.value >= this.buttonOptions.Count)
			{
				return null;
			}
			return this.buttonOptions[this.value];
		}
	}

	// Token: 0x06000DB1 RID: 3505 RVA: 0x00045B8F File Offset: 0x00043D8F
	public void SetCurrentValue(int newValue)
	{
		this.value = newValue;
		this.RefreshShownValue();
	}

	// Token: 0x06000DB2 RID: 3506 RVA: 0x00045BA0 File Offset: 0x00043DA0
	public void SetCurrentValue(ButtonOption option)
	{
		if (!Generic.FloatEquals(0f, option.optionFloatValue))
		{
			ButtonOption buttonOption = (from x in this.buttonOptions
			where Generic.FloatEquals(x.optionFloatValue, option.optionFloatValue)
			select x).FirstOrDefault<ButtonOption>();
			if (buttonOption != null)
			{
				this.value = this.buttonOptions.IndexOf(buttonOption);
			}
		}
		this.RefreshShownValue();
	}

	// Token: 0x06000DB3 RID: 3507 RVA: 0x00045C0C File Offset: 0x00043E0C
	public void SetCurrentIntValue(int newValue)
	{
		ButtonOption buttonOption = (from x in this.buttonOptions
		where x.optionIntValue == newValue
		select x).FirstOrDefault<ButtonOption>();
		if (buttonOption != null)
		{
			this.value = this.buttonOptions.IndexOf(buttonOption);
		}
		this.RefreshShownValue();
	}

	// Token: 0x06000DB4 RID: 3508 RVA: 0x00045C5E File Offset: 0x00043E5E
	public void RefreshShownValue()
	{
		if (this.GetCurrentValue != null)
		{
			this.currentOptionText.text = this.GetCurrentValue.optionText;
		}
	}

	// Token: 0x06000DB5 RID: 3509 RVA: 0x00045C7E File Offset: 0x00043E7E
	public Selectable GetLeftSideNavigation()
	{
		return this.previousButton;
	}

	// Token: 0x06000DB6 RID: 3510 RVA: 0x00045C86 File Offset: 0x00043E86
	public Selectable GetRightSideNavigation()
	{
		return this.nextButton;
	}

	// Token: 0x06000DB7 RID: 3511 RVA: 0x00045C90 File Offset: 0x00043E90
	public void SetNavigation(INavigationListOption upItem, INavigationListOption downItem, Selectable downNavigation, Selectable rightNavigation)
	{
		Selectable selectOnUp = (upItem != null) ? upItem.GetLeftSideNavigation() : null;
		Selectable selectOnUp2 = (upItem != null) ? upItem.GetRightSideNavigation() : null;
		Selectable selectable = (downItem != null) ? downItem.GetLeftSideNavigation() : null;
		Selectable selectable2 = (downItem != null) ? downItem.GetRightSideNavigation() : null;
		if (selectable == null)
		{
			selectable = downNavigation;
		}
		if (selectable2 == null)
		{
			selectable2 = downNavigation;
		}
		Navigation navigation = this.previousButton.navigation;
		navigation.mode = Navigation.Mode.Explicit;
		navigation.selectOnUp = selectOnUp;
		navigation.selectOnDown = selectable;
		navigation.selectOnLeft = null;
		navigation.selectOnRight = this.nextButton;
		this.previousButton.navigation = navigation;
		Navigation navigation2 = this.nextButton.navigation;
		navigation2.mode = Navigation.Mode.Explicit;
		navigation2.selectOnUp = selectOnUp2;
		navigation2.selectOnDown = selectable2;
		navigation2.selectOnLeft = this.previousButton;
		navigation2.selectOnRight = rightNavigation;
		this.nextButton.navigation = navigation2;
	}

	// Token: 0x06000DB8 RID: 3512 RVA: 0x00045D76 File Offset: 0x00043F76
	public void DisableGameSetting()
	{
		this.previousButton.gameObject.SetActive(false);
		this.nextButton.gameObject.SetActive(false);
	}

	// Token: 0x040009E2 RID: 2530
	public Button nextButton;

	// Token: 0x040009E3 RID: 2531
	public Button previousButton;

	// Token: 0x040009E4 RID: 2532
	public InputField currentOptionText;

	// Token: 0x040009E5 RID: 2533
	public List<ButtonOption> buttonOptions = new List<ButtonOption>();

	// Token: 0x040009E6 RID: 2534
	public int value;
}
