using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001D3 RID: 467
public class TextOptionSelect : MonoBehaviour, INavigationListOption, IDisableableGameSetting
{
	// Token: 0x06000DEC RID: 3564 RVA: 0x0004647E File Offset: 0x0004467E
	public Selectable GetLeftSideNavigation()
	{
		return this.inputFieldText;
	}

	// Token: 0x06000DED RID: 3565 RVA: 0x0004647E File Offset: 0x0004467E
	public Selectable GetRightSideNavigation()
	{
		return this.inputFieldText;
	}

	// Token: 0x06000DEE RID: 3566 RVA: 0x00046488 File Offset: 0x00044688
	public void SetNavigation(INavigationListOption upItem, INavigationListOption downItem, Selectable downNavigation, Selectable rightNavigation)
	{
		Selectable selectOnUp = (upItem != null) ? upItem.GetLeftSideNavigation() : null;
		if (upItem != null)
		{
			upItem.GetRightSideNavigation();
		}
		Selectable selectable = (downItem != null) ? downItem.GetLeftSideNavigation() : null;
		UnityEngine.Object exists = (downItem != null) ? downItem.GetRightSideNavigation() : null;
		if (selectable == null)
		{
			selectable = downNavigation;
		}
		exists;
		Navigation navigation = this.inputFieldText.navigation;
		navigation.selectOnUp = selectOnUp;
		navigation.selectOnDown = selectable;
		navigation.selectOnLeft = null;
		navigation.selectOnRight = rightNavigation;
		this.inputFieldText.navigation = navigation;
	}

	// Token: 0x06000DEF RID: 3567 RVA: 0x0004650F File Offset: 0x0004470F
	public void DisableGameSetting()
	{
		this.inputFieldText.interactable = false;
	}

	// Token: 0x04000A02 RID: 2562
	public InputField inputFieldText;
}
