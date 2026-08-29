using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200021D RID: 541
public class OpenTextDialogSettingItem : MonoBehaviour, INavigationListOption
{
	// Token: 0x14000006 RID: 6
	// (add) Token: 0x06001085 RID: 4229 RVA: 0x00055928 File Offset: 0x00053B28
	// (remove) Token: 0x06001086 RID: 4230 RVA: 0x00055960 File Offset: 0x00053B60
	public event EventHandler<BasicTextConfirmDialog> editorOpened;

	// Token: 0x14000007 RID: 7
	// (add) Token: 0x06001087 RID: 4231 RVA: 0x00055998 File Offset: 0x00053B98
	// (remove) Token: 0x06001088 RID: 4232 RVA: 0x000559D0 File Offset: 0x00053BD0
	public event EventHandler<string> textChanged;

	// Token: 0x06001089 RID: 4233 RVA: 0x00055A05 File Offset: 0x00053C05
	private void Start()
	{
		this.openButton.onClick.AddListener(delegate()
		{
			this.OpenDialog();
		});
	}

	// Token: 0x0600108A RID: 4234 RVA: 0x00055A24 File Offset: 0x00053C24
	public void OpenDialog()
	{
		BasicTextConfirmDialog confirmDialog = UnityEngine.Object.Instantiate<GameObject>(this.editDialogPrefab).GetComponent<BasicTextConfirmDialog>();
		this.editorOpened(this, confirmDialog);
		if (confirmDialog != null)
		{
			confirmDialog.okButton.onClick.RemoveAllListeners();
			confirmDialog.okButton.onClick.AddListener(delegate()
			{
				this.textChanged(this, confirmDialog.GetValue());
				confirmDialog.onClick();
			});
		}
	}

	// Token: 0x0600108B RID: 4235 RVA: 0x00055AAA File Offset: 0x00053CAA
	public Selectable GetLeftSideNavigation()
	{
		return this.openButton;
	}

	// Token: 0x0600108C RID: 4236 RVA: 0x00055AAA File Offset: 0x00053CAA
	public Selectable GetRightSideNavigation()
	{
		return this.openButton;
	}

	// Token: 0x0600108D RID: 4237 RVA: 0x00055AB4 File Offset: 0x00053CB4
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

	// Token: 0x04000BE9 RID: 3049
	public Button openButton;

	// Token: 0x04000BEA RID: 3050
	public GameObject editDialogPrefab;
}
