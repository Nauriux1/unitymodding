using System;
using BasicUI;
using UnityEngine;
using UnityEngine.UI;
using Utils;

// Token: 0x020001AD RID: 429
public class BasicInputField : MonoBehaviour
{
	// Token: 0x06000D34 RID: 3380 RVA: 0x00042DC4 File Offset: 0x00040FC4
	private void Awake()
	{
		this.inputField = base.gameObject.GetComponent<InputField>();
		foreach (Text text in base.gameObject.GetComponentsInChildren<Text>())
		{
			if (text.gameObject.name == "Placeholder")
			{
				this.placeholder = text;
			}
			else
			{
				this.text = text;
			}
		}
		if (this.inputField != null)
		{
			this.inputField.image.sprite = null;
			Sprite sprite = Resources.Load<Sprite>("Icons/UI/Panel");
			if (sprite != null)
			{
				this.inputField.image.sprite = sprite;
				this.inputField.image.type = Image.Type.Sliced;
				this.inputField.image.pixelsPerUnitMultiplier = 1f;
			}
			UIHelpers.SetInputFieldColor(this.inputField, UISettings.BasicButtonColor);
		}
		if (this.text != null)
		{
			this.text.color = UISettings.BasicTextColor;
			UIHelpers.SetTextFont(this.text, this.fontType);
			this.text.rectTransform.offsetMin = new Vector2(5f, 2f);
			this.text.rectTransform.offsetMax = new Vector2(-5f, -2f);
			this.text.alignment = this.textAnchor;
		}
		if (this.placeholder != null)
		{
			Color basicTextColor = UISettings.BasicTextColor;
			basicTextColor.a = 0.5f;
			this.placeholder.color = basicTextColor;
			UIHelpers.SetTextFont(this.placeholder, this.fontType);
			this.placeholder.rectTransform.offsetMin = new Vector2(5f, 2f);
			this.placeholder.rectTransform.offsetMax = new Vector2(-5f, -2f);
			this.placeholder.alignment = this.textAnchor;
		}
	}

	// Token: 0x0400097F RID: 2431
	private InputField inputField;

	// Token: 0x04000980 RID: 2432
	private Text text;

	// Token: 0x04000981 RID: 2433
	private Text placeholder;

	// Token: 0x04000982 RID: 2434
	public FontType fontType = FontType.Options;

	// Token: 0x04000983 RID: 2435
	public TextAnchor textAnchor = TextAnchor.MiddleLeft;
}
