using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200012F RID: 303
public class PaintEditorHideTool : MonoBehaviour
{
	// Token: 0x06000969 RID: 2409 RVA: 0x0002CEE7 File Offset: 0x0002B0E7
	public void InitHideTool(List<PaintEditorItem> items)
	{
		this.paintEditorItems = items;
		this.FillHideItems();
	}

	// Token: 0x0600096A RID: 2410 RVA: 0x0002CEF8 File Offset: 0x0002B0F8
	private void FillHideItems()
	{
		foreach (PaintEditorItem paintEditorItem in from x in this.paintEditorItems
		orderby x.GetDescription()
		select x)
		{
			MultiselectItem component = UnityEngine.Object.Instantiate<GameObject>(this.multiselectItemPrefab, this.multiselectFilterPanel).GetComponent<MultiselectItem>();
			component.SetText(paintEditorItem.GetDescription());
			component.checkBox.isOn = false;
			component.value = paintEditorItem;
			component.checkBox.onValueChanged.AddListener(delegate(bool <p0>)
			{
				this.OnMultiselectValueChanged();
			});
			this.multiselectItems.Add(component);
		}
		this.hideAllItemsButton.onClick.AddListener(delegate()
		{
			this.HideAllItems();
		});
		this.hideBallItemsButton.onClick.AddListener(delegate()
		{
			this.HideItemsByType(true);
		});
		this.hideNonBallItemsButton.onClick.AddListener(delegate()
		{
			this.HideItemsByType(false);
		});
	}

	// Token: 0x0600096B RID: 2411 RVA: 0x0002D014 File Offset: 0x0002B214
	private void HideAllItems()
	{
		bool? flag = null;
		foreach (MultiselectItem multiselectItem in this.multiselectItems)
		{
			if (flag == null)
			{
				flag = new bool?(!multiselectItem.checkBox.isOn);
			}
			multiselectItem.checkBox.SetIsOnWithoutNotify(flag.Value);
		}
		this.OnMultiselectValueChanged();
	}

	// Token: 0x0600096C RID: 2412 RVA: 0x0002D0A0 File Offset: 0x0002B2A0
	private void HideItemsByType(bool balls)
	{
		bool? flag = null;
		foreach (MultiselectItem multiselectItem in this.multiselectItems)
		{
			if (((PaintEditorItem)multiselectItem.value).IsBall == balls)
			{
				if (flag == null)
				{
					flag = new bool?(!multiselectItem.checkBox.isOn);
				}
				multiselectItem.checkBox.SetIsOnWithoutNotify(flag.Value);
			}
		}
		this.OnMultiselectValueChanged();
	}

	// Token: 0x0600096D RID: 2413 RVA: 0x0002D13C File Offset: 0x0002B33C
	private void OnMultiselectValueChanged()
	{
		this.UpdateSelectedItemList();
	}

	// Token: 0x0600096E RID: 2414 RVA: 0x0002D144 File Offset: 0x0002B344
	private void UpdateSelectedItemList()
	{
		this.selectedObjects.Clear();
		foreach (MultiselectItem multiselectItem in this.multiselectItems)
		{
			PaintEditorItem paintEditorItem = (PaintEditorItem)multiselectItem.value;
			if (multiselectItem.checkBox.isOn)
			{
				this.selectedObjects.Add(paintEditorItem);
			}
			this.HandleVisibility(paintEditorItem, !multiselectItem.checkBox.isOn);
		}
	}

	// Token: 0x0600096F RID: 2415 RVA: 0x0002D1D8 File Offset: 0x0002B3D8
	private void HandleVisibility(PaintEditorItem paintEditorItem, bool visible)
	{
		if (paintEditorItem.renderer.enabled == visible)
		{
			return;
		}
		paintEditorItem.renderer.enabled = visible;
		paintEditorItem.cwLocalPaintableMesh.enabled = visible;
		paintEditorItem.meshCollider.enabled = visible;
	}

	// Token: 0x0400069A RID: 1690
	private List<PaintEditorItem> paintEditorItems;

	// Token: 0x0400069B RID: 1691
	[Header("HideItems")]
	public GameObject multiselectItemPrefab;

	// Token: 0x0400069C RID: 1692
	public RectTransform multiselectFilterPanel;

	// Token: 0x0400069D RID: 1693
	private List<MultiselectItem> multiselectItems = new List<MultiselectItem>();

	// Token: 0x0400069E RID: 1694
	private List<PaintEditorItem> selectedObjects = new List<PaintEditorItem>();

	// Token: 0x0400069F RID: 1695
	public Button hideAllItemsButton;

	// Token: 0x040006A0 RID: 1696
	public Button hideBallItemsButton;

	// Token: 0x040006A1 RID: 1697
	public Button hideNonBallItemsButton;
}
