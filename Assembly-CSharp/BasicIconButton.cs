using System;
using UnityEngine;
using UnityEngine.UI;
using Utils;

// Token: 0x020001AA RID: 426
public class BasicIconButton : MonoBehaviour
{
	// Token: 0x06000D24 RID: 3364 RVA: 0x00042B78 File Offset: 0x00040D78
	private void Awake()
	{
		this.backgroundImage = base.gameObject.GetComponent<Image>();
		this.button = base.gameObject.GetComponent<Button>();
		if (this.button != null)
		{
			if (Resources.Load<Sprite>("Icons/UI/Button") != null && this.backgroundImage != null)
			{
				this.backgroundImage.pixelsPerUnitMultiplier = 1f;
			}
			UIHelpers.SetIconButtonColor(this.button, this.buttonState);
			Text componentInChildren = this.button.gameObject.GetComponentInChildren<Text>();
			if (componentInChildren != null)
			{
				UIHelpers.SetTextFont(componentInChildren, this.fontType);
				if (this.fontType == FontType.Basic)
				{
					componentInChildren.fontSize = 20;
				}
				if (this.fontSize > 0)
				{
					componentInChildren.fontSize = this.fontSize;
				}
			}
		}
	}

	// Token: 0x04000973 RID: 2419
	private Button button;

	// Token: 0x04000974 RID: 2420
	private Image backgroundImage;

	// Token: 0x04000975 RID: 2421
	public ButtonState buttonState;

	// Token: 0x04000976 RID: 2422
	public FontType fontType;

	// Token: 0x04000977 RID: 2423
	public int fontSize;
}
