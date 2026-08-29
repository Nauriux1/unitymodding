using System;
using BasicUI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001B0 RID: 432
public class BasicPanel : MonoBehaviour
{
	// Token: 0x06000D3A RID: 3386 RVA: 0x000430FC File Offset: 0x000412FC
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
			this.panelImage.color = UISettings.BasicPanelColor;
			Sprite sprite = Resources.Load<Sprite>("Icons/UI/Panel");
			if (sprite != null && this.panelImage != null)
			{
				this.panelImage.sprite = sprite;
				this.panelImage.pixelsPerUnitMultiplier = 1f;
			}
		}
	}

	// Token: 0x04000986 RID: 2438
	private Image panelImage;
}
