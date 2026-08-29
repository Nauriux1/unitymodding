using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001CB RID: 459
public class ButtonIntOptionSelect : MonoBehaviour, INavigationListOption, IDisableableGameSetting
{
	// Token: 0x14000002 RID: 2
	// (add) Token: 0x06000D99 RID: 3481 RVA: 0x00045698 File Offset: 0x00043898
	// (remove) Token: 0x06000D9A RID: 3482 RVA: 0x000456D0 File Offset: 0x000438D0
	public event EventHandler ValueChanged;

	// Token: 0x06000D9B RID: 3483 RVA: 0x00045708 File Offset: 0x00043908
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
		this.nextNextButton.onClick.AddListener(delegate()
		{
			this.ChangeValue(10);
		});
		this.previousPreviousButton.onClick.AddListener(delegate()
		{
			this.ChangeValue(-10);
		});
	}

	// Token: 0x06000D9C RID: 3484 RVA: 0x0000777A File Offset: 0x0000597A
	private void Update()
	{
	}

	// Token: 0x06000D9D RID: 3485 RVA: 0x00045785 File Offset: 0x00043985
	public void ChangeValue(int valueChange)
	{
		this.value += valueChange;
		this.ValidateCurrentValue();
		this.ValueChanged(this, EventArgs.Empty);
		this.RefreshShownValue();
	}

	// Token: 0x06000D9E RID: 3486 RVA: 0x000457B2 File Offset: 0x000439B2
	public void ValidateCurrentValue()
	{
		if (this.onlyPositiveValues && this.value < 0)
		{
			this.value = 0;
		}
	}

	// Token: 0x17000191 RID: 401
	// (get) Token: 0x06000D9F RID: 3487 RVA: 0x000457CC File Offset: 0x000439CC
	public int GetCurrentValue
	{
		get
		{
			return this.value;
		}
	}

	// Token: 0x06000DA0 RID: 3488 RVA: 0x000457D4 File Offset: 0x000439D4
	public void SetCurrentValue(int newValue)
	{
		this.value = newValue;
		this.ValidateCurrentValue();
		this.RefreshShownValue();
	}

	// Token: 0x06000DA1 RID: 3489 RVA: 0x000457E9 File Offset: 0x000439E9
	public void RefreshShownValue()
	{
		if (this.value == 0 && !string.IsNullOrEmpty(this.textValueForZeroDisplay))
		{
			this.currentOptionText.text = this.textValueForZeroDisplay;
			return;
		}
		this.currentOptionText.text = this.value.ToString();
	}

	// Token: 0x06000DA2 RID: 3490 RVA: 0x00045828 File Offset: 0x00043A28
	public Selectable GetLeftSideNavigation()
	{
		return this.previousPreviousButton;
	}

	// Token: 0x06000DA3 RID: 3491 RVA: 0x00045830 File Offset: 0x00043A30
	public Selectable GetRightSideNavigation()
	{
		return this.nextNextButton;
	}

	// Token: 0x06000DA4 RID: 3492 RVA: 0x00045838 File Offset: 0x00043A38
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
		Navigation navigation = this.previousPreviousButton.navigation;
		navigation.mode = Navigation.Mode.Explicit;
		navigation.selectOnUp = selectOnUp;
		navigation.selectOnDown = selectable;
		navigation.selectOnLeft = null;
		navigation.selectOnRight = this.previousButton;
		this.previousPreviousButton.navigation = navigation;
		Navigation navigation2 = this.previousButton.navigation;
		navigation2.mode = Navigation.Mode.Explicit;
		navigation2.selectOnUp = selectOnUp;
		navigation2.selectOnDown = selectable;
		navigation2.selectOnLeft = this.previousPreviousButton;
		navigation2.selectOnRight = this.nextButton;
		this.previousButton.navigation = navigation2;
		Navigation navigation3 = this.nextButton.navigation;
		navigation3.mode = Navigation.Mode.Explicit;
		navigation3.selectOnUp = selectOnUp2;
		navigation3.selectOnDown = selectable2;
		navigation3.selectOnLeft = this.previousButton;
		navigation3.selectOnRight = this.nextNextButton;
		this.nextButton.navigation = navigation3;
		Navigation navigation4 = this.nextNextButton.navigation;
		navigation4.mode = Navigation.Mode.Explicit;
		navigation4.selectOnUp = selectOnUp2;
		navigation4.selectOnDown = selectable2;
		navigation4.selectOnLeft = this.nextButton;
		navigation4.selectOnRight = rightNavigation;
		this.nextNextButton.navigation = navigation4;
	}

	// Token: 0x06000DA5 RID: 3493 RVA: 0x000459B8 File Offset: 0x00043BB8
	public void DisableGameSetting()
	{
		this.previousButton.gameObject.SetActive(false);
		this.nextButton.gameObject.SetActive(false);
		this.previousPreviousButton.gameObject.SetActive(false);
		this.nextNextButton.gameObject.SetActive(false);
	}

	// Token: 0x040009D9 RID: 2521
	public Button nextButton;

	// Token: 0x040009DA RID: 2522
	public Button previousButton;

	// Token: 0x040009DB RID: 2523
	public Button nextNextButton;

	// Token: 0x040009DC RID: 2524
	public Button previousPreviousButton;

	// Token: 0x040009DD RID: 2525
	public InputField currentOptionText;

	// Token: 0x040009DE RID: 2526
	public int value;

	// Token: 0x040009DF RID: 2527
	public bool onlyPositiveValues = true;

	// Token: 0x040009E1 RID: 2529
	public string textValueForZeroDisplay = "";
}
