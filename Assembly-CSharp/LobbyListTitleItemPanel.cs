using System;
using BasicUI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000215 RID: 533
public class LobbyListTitleItemPanel : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x0600102F RID: 4143 RVA: 0x0005400C File Offset: 0x0005220C
	public void Awake()
	{
		this.lobbyListTitlePanel = base.transform.parent.GetComponent<LobbyListTitlePanel>();
		Sprite sprite = Resources.Load<Sprite>("Icons/UI/DownArrowAlt");
		if (sprite != null && this.sortIcon != null)
		{
			this.sortIcon.sprite = sprite;
			this.sortIcon.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 10f);
			this.sortIcon.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 10f);
			this.sortIcon.color = UISettings.BasicTextColor;
		}
	}

	// Token: 0x06001030 RID: 4144 RVA: 0x00054099 File Offset: 0x00052299
	public void OnPointerClick(PointerEventData eventData)
	{
		this.lobbyListTitlePanel.SetSortColumn(this.columnName, false);
	}

	// Token: 0x06001031 RID: 4145 RVA: 0x000540B0 File Offset: 0x000522B0
	public void SetSortIcon(TableColumnSortType? sortType)
	{
		if (sortType == null)
		{
			this.sortIcon.gameObject.SetActive(false);
			return;
		}
		this.sortIcon.gameObject.SetActive(true);
		TableColumnSortType? tableColumnSortType = sortType;
		TableColumnSortType tableColumnSortType2 = TableColumnSortType.Ascending;
		if (tableColumnSortType.GetValueOrDefault() == tableColumnSortType2 & tableColumnSortType != null)
		{
			this.sortIcon.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
			return;
		}
		this.sortIcon.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
	}

	// Token: 0x04000B99 RID: 2969
	public string columnName = "";

	// Token: 0x04000B9A RID: 2970
	public LobbyListTitlePanel lobbyListTitlePanel;

	// Token: 0x04000B9B RID: 2971
	public Image sortIcon;
}
