using System;
using BasicUI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001A3 RID: 419
public class BasicBackgroundMainMenu : MonoBehaviour
{
	// Token: 0x06000D0B RID: 3339 RVA: 0x00042214 File Offset: 0x00040414
	private void Awake()
	{
		this.backgroundImage = base.gameObject.GetComponent<Image>();
		if (this.backgroundImage == null)
		{
			this.backgroundImage = base.gameObject.AddComponent<Image>();
		}
		if (this.backgroundImage != null)
		{
			this.backgroundImage.color = UISettings.BasicBackgroundMainMenuColor;
			Sprite sprite = Resources.Load<Sprite>("Icons/UI/Menu_Background");
			if (sprite != null && this.backgroundImage != null)
			{
				this.backgroundImage.sprite = sprite;
				this.backgroundImage.pixelsPerUnitMultiplier = 1f;
			}
		}
	}

	// Token: 0x04000954 RID: 2388
	private Image backgroundImage;
}
