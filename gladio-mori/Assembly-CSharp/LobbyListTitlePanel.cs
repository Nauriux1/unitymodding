using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000216 RID: 534
public class LobbyListTitlePanel : MonoBehaviour
{
	// Token: 0x06001033 RID: 4147 RVA: 0x0005415D File Offset: 0x0005235D
	public void Awake()
	{
		this.titlePanels = base.transform.GetComponentsInChildren<LobbyListTitleItemPanel>().ToList<LobbyListTitleItemPanel>();
	}

	// Token: 0x06001034 RID: 4148 RVA: 0x00054178 File Offset: 0x00052378
	public void SetSortColumn(string columnName, bool noUpdate = false)
	{
		LobbyListTitleItemPanel x2 = (from x in this.titlePanels
		where x.columnName == columnName
		select x).FirstOrDefault<LobbyListTitleItemPanel>();
		if (x2 == this.selectedTitle)
		{
			if (this.sortType == TableColumnSortType.Ascending)
			{
				this.sortType = TableColumnSortType.Descending;
			}
			else
			{
				this.sortType = TableColumnSortType.Ascending;
			}
		}
		else
		{
			this.sortType = TableColumnSortType.Ascending;
		}
		this.selectedTitle = x2;
		foreach (LobbyListTitleItemPanel lobbyListTitleItemPanel in this.titlePanels)
		{
			lobbyListTitleItemPanel.SetSortIcon(null);
		}
		if (this.selectedTitle != null)
		{
			this.selectedTitle.SetSortIcon(new TableColumnSortType?(this.sortType));
		}
		if (!noUpdate)
		{
			this.multiplayerMenuManager.RenderLobbyList();
		}
	}

	// Token: 0x06001035 RID: 4149 RVA: 0x00054264 File Offset: 0x00052464
	public string GetSortColumnName()
	{
		if (this.selectedTitle != null)
		{
			return this.selectedTitle.columnName;
		}
		return "";
	}

	// Token: 0x04000B9C RID: 2972
	public TableColumnSortType sortType;

	// Token: 0x04000B9D RID: 2973
	public LobbyListTitleItemPanel selectedTitle;

	// Token: 0x04000B9E RID: 2974
	public List<LobbyListTitleItemPanel> titlePanels;

	// Token: 0x04000B9F RID: 2975
	public MultiplayerMenuManager multiplayerMenuManager;
}
