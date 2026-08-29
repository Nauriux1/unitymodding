using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000212 RID: 530
public class AllowedEquipmentSelectSettingItem : MonoBehaviour, INavigationListOption
{
	// Token: 0x0600101B RID: 4123 RVA: 0x00053BF4 File Offset: 0x00051DF4
	private void Start()
	{
		this.openButton.onClick.AddListener(delegate()
		{
			this.OpenEquipmentSelector();
		});
	}

	// Token: 0x0600101C RID: 4124 RVA: 0x00053C12 File Offset: 0x00051E12
	public void OpenEquipmentSelector()
	{
		UnityEngine.Object.Instantiate<GameObject>(this.equipmentSelectPrefab);
	}

	// Token: 0x0600101D RID: 4125 RVA: 0x00053C20 File Offset: 0x00051E20
	public Selectable GetLeftSideNavigation()
	{
		return this.openButton;
	}

	// Token: 0x0600101E RID: 4126 RVA: 0x00053C20 File Offset: 0x00051E20
	public Selectable GetRightSideNavigation()
	{
		return this.openButton;
	}

	// Token: 0x0600101F RID: 4127 RVA: 0x00053C28 File Offset: 0x00051E28
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
		Navigation navigation = this.openButton.navigation;
		navigation.mode = Navigation.Mode.Explicit;
		navigation.selectOnUp = selectOnUp;
		navigation.selectOnDown = selectable;
		navigation.selectOnLeft = null;
		navigation.selectOnRight = rightNavigation;
		this.openButton.navigation = navigation;
	}

	// Token: 0x04000B8B RID: 2955
	public Button openButton;

	// Token: 0x04000B8C RID: 2956
	public GameObject equipmentSelectPrefab;
}
