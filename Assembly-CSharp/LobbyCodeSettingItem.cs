using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000213 RID: 531
public class LobbyCodeSettingItem : MonoBehaviour, INavigationListOption
{
	// Token: 0x06001022 RID: 4130 RVA: 0x00053CBF File Offset: 0x00051EBF
	private void Start()
	{
		this.copyButton.onClick.AddListener(delegate()
		{
			this.CopyLobbyCodeToClipBoard();
		});
	}

	// Token: 0x06001023 RID: 4131 RVA: 0x00053CE0 File Offset: 0x00051EE0
	public void CopyLobbyCodeToClipBoard()
	{
		if (SteamManager.steamManager != null && SteamManager.steamManager.currentLobby.Id != 0UL)
		{
			GUIUtility.systemCopyBuffer = SteamManager.steamManager.currentLobby.Id.Value.ToString();
			GeneralManager.DisplayInfoMessage(LocalizationHelpers.LocalizedText("txt_copied", new object[0]), 1f);
			return;
		}
		Debug.Log("Failed to copy lobby code");
	}

	// Token: 0x06001024 RID: 4132 RVA: 0x00053D56 File Offset: 0x00051F56
	public Selectable GetLeftSideNavigation()
	{
		return this.copyButton;
	}

	// Token: 0x06001025 RID: 4133 RVA: 0x00053D56 File Offset: 0x00051F56
	public Selectable GetRightSideNavigation()
	{
		return this.copyButton;
	}

	// Token: 0x06001026 RID: 4134 RVA: 0x00053D60 File Offset: 0x00051F60
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
		Navigation navigation = this.copyButton.navigation;
		navigation.selectOnUp = selectOnUp;
		navigation.selectOnDown = selectable;
		navigation.selectOnLeft = null;
		navigation.selectOnRight = rightNavigation;
		this.copyButton.navigation = navigation;
	}

	// Token: 0x04000B8D RID: 2957
	public Button copyButton;
}
