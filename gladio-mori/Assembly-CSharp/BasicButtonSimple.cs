using System;
using UnityEngine;
using UnityEngine.UI;
using Utils;

// Token: 0x020001A6 RID: 422
public class BasicButtonSimple : MonoBehaviour
{
	// Token: 0x06000D18 RID: 3352 RVA: 0x000426E0 File Offset: 0x000408E0
	private void Awake()
	{
		this.backgroundImage = base.gameObject.GetComponent<Image>();
		this.button = base.gameObject.GetComponent<Button>();
		if (this.button != null)
		{
			Sprite sprite = Resources.Load<Sprite>("Icons/UI/Button9");
			if (sprite != null && this.backgroundImage != null)
			{
				this.backgroundImage.sprite = sprite;
				this.backgroundImage.pixelsPerUnitMultiplier = 1f;
			}
			UIHelpers.SetButtonColor(this.button, ButtonState.Basic, null, null);
		}
	}

	// Token: 0x04000966 RID: 2406
	private Button button;

	// Token: 0x04000967 RID: 2407
	private Image backgroundImage;
}
