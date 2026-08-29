using System;
using BasicUI;
using UnityEngine;
using UnityEngine.UI;
using Utils;

// Token: 0x020001B2 RID: 434
public class BasicScrollView : MonoBehaviour
{
	// Token: 0x06000D3E RID: 3390 RVA: 0x000431D0 File Offset: 0x000413D0
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
			this.panelImage.color = (this.transparent ? UISettings.BasicScrollviewColor : UISettings.BasicBackgroundColor);
			if (this.chatBox)
			{
				Color color = this.panelImage.color;
				color.a = 0.95f;
				this.panelImage.color = color;
			}
		}
		Transform transform = base.gameObject.transform.Find("Viewport");
		if (transform != null)
		{
			this.viewportImage = transform.gameObject.GetComponent<Image>();
			if (this.viewportImage != null)
			{
				this.viewportImage.sprite = null;
				this.viewportImage.color = (this.transparent ? UISettings.BasicScrollviewColor : UISettings.BasicBackgroundColor);
			}
		}
		this.scrollRect = base.gameObject.GetComponent<ScrollRect>();
		if (this.scrollRect != null)
		{
			this.scrollRect.movementType = ScrollRect.MovementType.Clamped;
			this.scrollRect.scrollSensitivity = 10f;
			UIHelpers.SetScrollbarColor(this.scrollRect.horizontalScrollbar);
			UIHelpers.SetScrollbarColor(this.scrollRect.verticalScrollbar);
			this.scrollRect.horizontalScrollbarSpacing = 0f;
			this.scrollRect.verticalScrollbarSpacing = 0f;
		}
	}

	// Token: 0x04000988 RID: 2440
	private Image panelImage;

	// Token: 0x04000989 RID: 2441
	private Image viewportImage;

	// Token: 0x0400098A RID: 2442
	private ScrollRect scrollRect;

	// Token: 0x0400098B RID: 2443
	public bool transparent;

	// Token: 0x0400098C RID: 2444
	public bool chatBox;
}
