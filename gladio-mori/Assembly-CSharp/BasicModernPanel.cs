using System;
using BasicUI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001AE RID: 430
public class BasicModernPanel : MonoBehaviour
{
	// Token: 0x06000D36 RID: 3382 RVA: 0x00042FC8 File Offset: 0x000411C8
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
			this.panelImage.color = UISettings.BasicScrollviewColor;
			Color color = this.panelImage.color;
			color.a = 0.95f;
			this.panelImage.color = color;
		}
	}

	// Token: 0x04000984 RID: 2436
	private Image panelImage;
}
