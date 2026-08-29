using System;
using BasicUI;
using TMPro;
using UnityEngine;
using Utils;

// Token: 0x020001B8 RID: 440
public class BasicTmpInputField : MonoBehaviour
{
	// Token: 0x06000D4D RID: 3405 RVA: 0x000436A0 File Offset: 0x000418A0
	private void Awake()
	{
		this.inputField = base.gameObject.GetComponent<TMP_InputField>();
		foreach (TMP_Text tmp_Text in base.gameObject.GetComponentsInChildren<TMP_Text>())
		{
			if (tmp_Text.gameObject.name == "Placeholder")
			{
				this.placeholder = tmp_Text;
			}
			else
			{
				this.text = tmp_Text;
			}
		}
		if (this.inputField != null)
		{
			this.inputField.image.sprite = null;
			Sprite sprite = Resources.Load<Sprite>("Icons/UI/Panel");
			if (sprite != null && !this.chatBox)
			{
				this.inputField.image.sprite = sprite;
				this.inputField.image.pixelsPerUnitMultiplier = 1f;
			}
			UIHelpers.SetInputFieldColor(this.inputField, UISettings.BasicButtonColor);
			if (this.chatBox)
			{
				Color color = this.inputField.image.color;
				color.a = 0.95f;
				this.inputField.image.color = color;
			}
		}
		if (this.text != null)
		{
			if (this.chatBox)
			{
				this.text.color = Color.white;
			}
			else
			{
				this.text.color = UISettings.BasicTextColor;
				if (!this.largeBox)
				{
					this.text.rectTransform.offsetMin = new Vector2(5f, 2f);
					this.text.rectTransform.offsetMax = new Vector2(-5f, -2f);
				}
			}
		}
		if (this.placeholder != null)
		{
			if (this.chatBox)
			{
				this.placeholder.color = Color.gray;
				return;
			}
			Color basicTextColor = UISettings.BasicTextColor;
			basicTextColor.a = 0.5f;
			this.placeholder.color = basicTextColor;
			if (!this.largeBox)
			{
				this.placeholder.rectTransform.offsetMin = new Vector2(5f, 2f);
				this.placeholder.rectTransform.offsetMax = new Vector2(-5f, -2f);
			}
		}
	}

	// Token: 0x04000999 RID: 2457
	private TMP_InputField inputField;

	// Token: 0x0400099A RID: 2458
	private TMP_Text text;

	// Token: 0x0400099B RID: 2459
	private TMP_Text placeholder;

	// Token: 0x0400099C RID: 2460
	public bool largeBox;

	// Token: 0x0400099D RID: 2461
	public bool chatBox;
}
