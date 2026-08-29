using System;
using BasicUI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001A4 RID: 420
public class BasicBackgroundMenu : MonoBehaviour
{
	// Token: 0x06000D0D RID: 3341 RVA: 0x000422B0 File Offset: 0x000404B0
	private void Awake()
	{
		this.backgroundImage = base.gameObject.GetComponent<Image>();
		if (this.backgroundImage == null)
		{
			this.backgroundImage = base.gameObject.AddComponent<Image>();
		}
		if (this.backgroundImage != null)
		{
			this.backgroundImage.color = UISettings.BasicBackgroundColor;
		}
	}

	// Token: 0x04000955 RID: 2389
	private Image backgroundImage;
}
