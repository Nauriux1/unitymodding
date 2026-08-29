using System;
using BasicUI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001AF RID: 431
public class BasicOverlayPanel : MonoBehaviour
{
	// Token: 0x06000D38 RID: 3384 RVA: 0x00043054 File Offset: 0x00041254
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
			this.panelImage.color = UISettings.BasicOverlayPanelColor;
			Sprite sprite = Resources.Load<Sprite>("Icons/UI/Panel");
			if (sprite != null && this.panelImage != null)
			{
				this.panelImage.sprite = sprite;
				this.panelImage.pixelsPerUnitMultiplier = 1f;
			}
		}
	}

	// Token: 0x04000985 RID: 2437
	private Image panelImage;
}
