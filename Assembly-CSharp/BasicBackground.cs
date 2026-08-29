using System;
using BasicUI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001A2 RID: 418
public class BasicBackground : MonoBehaviour
{
	// Token: 0x06000D09 RID: 3337 RVA: 0x000421AC File Offset: 0x000403AC
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
			this.backgroundImage.sprite = null;
		}
	}

	// Token: 0x04000953 RID: 2387
	private Image backgroundImage;
}
