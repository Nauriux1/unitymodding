using System;
using BasicUI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001B4 RID: 436
public class BasicSubPanel : MonoBehaviour
{
	// Token: 0x06000D44 RID: 3396 RVA: 0x00043440 File Offset: 0x00041640
	private void Awake()
	{
		this.panelImage = base.gameObject.GetComponent<Image>();
		if (this.panelImage == null)
		{
			this.panelImage = base.gameObject.AddComponent<Image>();
		}
		if (this.panelImage != null)
		{
			this.panelImage.sprite = null;
			this.panelImage.color = UISettings.BasicSubPanelColor;
			if (this.subPanelType == BasicSubPanel.SubPanelType.TableTitleRow)
			{
				this.panelImage.color = UISettings.BasicTableTitleRowColor;
			}
			Sprite sprite = Resources.Load<Sprite>("Icons/UI/Panel");
			if (sprite != null && this.panelImage != null)
			{
				this.panelImage.sprite = sprite;
				this.panelImage.type = Image.Type.Sliced;
				this.panelImage.pixelsPerUnitMultiplier = 1f;
			}
		}
	}

	// Token: 0x04000990 RID: 2448
	private Image panelImage;

	// Token: 0x04000991 RID: 2449
	public BasicSubPanel.SubPanelType subPanelType;

	// Token: 0x020001B5 RID: 437
	public enum SubPanelType
	{
		// Token: 0x04000993 RID: 2451
		None,
		// Token: 0x04000994 RID: 2452
		TableTitleRow
	}
}
