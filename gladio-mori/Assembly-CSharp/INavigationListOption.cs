using System;
using UnityEngine.UI;

// Token: 0x02000055 RID: 85
public interface INavigationListOption
{
	// Token: 0x06000247 RID: 583
	Selectable GetLeftSideNavigation();

	// Token: 0x06000248 RID: 584
	Selectable GetRightSideNavigation();

	// Token: 0x06000249 RID: 585
	void SetNavigation(INavigationListOption upItem, INavigationListOption downItem, Selectable downNavigation, Selectable rightNavigation);
}
