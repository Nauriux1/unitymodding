using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001D4 RID: 468
public class ToggleOptionSelect : MonoBehaviour, INavigationListOption
{
	// Token: 0x06000DF1 RID: 3569 RVA: 0x0004651D File Offset: 0x0004471D
	public Selectable GetLeftSideNavigation()
	{
		return this.toggle;
	}

	// Token: 0x06000DF2 RID: 3570 RVA: 0x0004651D File Offset: 0x0004471D
	public Selectable GetRightSideNavigation()
	{
		return this.toggle;
	}

	// Token: 0x06000DF3 RID: 3571 RVA: 0x00046528 File Offset: 0x00044728
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
		Navigation navigation = this.toggle.navigation;
		navigation.mode = Navigation.Mode.Explicit;
		navigation.selectOnUp = selectOnUp;
		navigation.selectOnDown = selectable;
		navigation.selectOnLeft = null;
		navigation.selectOnRight = rightNavigation;
		this.toggle.navigation = navigation;
	}

	// Token: 0x04000A03 RID: 2563
	public Toggle toggle;
}
