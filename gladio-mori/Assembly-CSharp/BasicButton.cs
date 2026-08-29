using System;
using BasicUI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

// Token: 0x020001A5 RID: 421
public class BasicButton : MonoBehaviour, ISelectHandler, IEventSystemHandler, IDeselectHandler
{
	// Token: 0x1700018B RID: 395
	// (get) Token: 0x06000D0F RID: 3343 RVA: 0x0004230B File Offset: 0x0004050B
	private float overrideBottomValue
	{
		get
		{
			if (this.overrideBottomOffsetBool)
			{
				return this.overrideBottomOffsetFloat;
			}
			return 6f;
		}
	}

	// Token: 0x06000D10 RID: 3344 RVA: 0x00042324 File Offset: 0x00040524
	private void Awake()
	{
		this.backgroundImage = base.gameObject.GetComponent<Image>();
		this.button = base.gameObject.GetComponent<Button>();
		if (this.button != null)
		{
			Sprite buttonImage = this.GetButtonImage();
			if (buttonImage != null && this.backgroundImage != null)
			{
				this.backgroundImage.sprite = buttonImage;
				this.backgroundImage.pixelsPerUnitMultiplier = 1f;
			}
			UIHelpers.SetButtonColor(this.button, this.buttonState, this.overrideColor, this.overrideTextColor);
			this.UpdateDisabledColors();
			this.text = this.button.gameObject.GetComponentInChildren<Text>();
			if (this.text != null)
			{
				RectTransform component = this.text.gameObject.GetComponent<RectTransform>();
				if (component != null && !this.disableRectTransformOffsets)
				{
					component.offsetMin = new Vector2(4f, this.overrideBottomValue);
					component.offsetMax = new Vector2(-4f, -6f);
				}
				this.text.resizeTextForBestFit = true;
				this.text.alignByGeometry = true;
				this.text.resizeTextMinSize = 6;
				this.text.resizeTextMaxSize = 24;
				UIHelpers.SetTextFont(this.text, this.fontType);
				if (this.fontType == FontType.Basic)
				{
					this.text.fontSize = 20;
					this.text.resizeTextMaxSize = 20;
				}
				if (this.fontSize > 0)
				{
					this.text.fontSize = this.fontSize;
					this.text.resizeTextMaxSize = this.fontSize;
				}
			}
		}
		this.CheckDisableColor();
	}

	// Token: 0x06000D11 RID: 3345 RVA: 0x000424CC File Offset: 0x000406CC
	public void CheckDisableColor()
	{
		if (this.text != null && this.button != null && string.IsNullOrEmpty(this.overrideTextColor))
		{
			if (this.button.interactable)
			{
				this.text.color = UISettings.BasicTextColor;
				return;
			}
			this.text.color = UISettings.BasicDisabledTextColor;
		}
	}

	// Token: 0x06000D12 RID: 3346 RVA: 0x00042530 File Offset: 0x00040730
	public void UpdateDisabledColors()
	{
		if (this.button != null)
		{
			this.disabledColor = this.button.colors.disabledColor;
			this.disabledSelectedColor = UIHelpers.HighlightColor(this.disabledColor);
		}
	}

	// Token: 0x06000D13 RID: 3347 RVA: 0x00042578 File Offset: 0x00040778
	public void OnSelect(BaseEventData eventData)
	{
		if (this.button != null)
		{
			ColorBlock colors = this.button.colors;
			colors.disabledColor = this.disabledSelectedColor;
			this.button.colors = colors;
		}
	}

	// Token: 0x06000D14 RID: 3348 RVA: 0x000425B8 File Offset: 0x000407B8
	public void OnDeselect(BaseEventData eventData)
	{
		if (this.button != null)
		{
			ColorBlock colors = this.button.colors;
			colors.disabledColor = this.disabledColor;
			this.button.colors = colors;
		}
	}

	// Token: 0x06000D15 RID: 3349 RVA: 0x000425F8 File Offset: 0x000407F8
	public Sprite GetButtonImage()
	{
		if (this.UseNewStyle)
		{
			return UIHelpers.LoadSpriteFromResources("Icons/UI/UI_Buttons", "UI_Buttons_BlackWide");
		}
		return Resources.Load<Sprite>("Icons/UI/Button9");
	}

	// Token: 0x1700018C RID: 396
	// (get) Token: 0x06000D16 RID: 3350 RVA: 0x0004261C File Offset: 0x0004081C
	public bool UseNewStyle
	{
		get
		{
			if (this._useNewStyle == null)
			{
				if (this.forceNewStyle)
				{
					this._useNewStyle = new bool?(this.forceNewStyle);
				}
				else
				{
					this._useNewStyle = new bool?(this.backgroundImage != null && this.backgroundImage.sprite != null && this.backgroundImage.sprite.name == "UI_Buttons_BlackWide");
				}
			}
			return this._useNewStyle != null && this._useNewStyle.Value;
		}
	}

	// Token: 0x04000956 RID: 2390
	private Button button;

	// Token: 0x04000957 RID: 2391
	private Text text;

	// Token: 0x04000958 RID: 2392
	private Image backgroundImage;

	// Token: 0x04000959 RID: 2393
	public ButtonState buttonState;

	// Token: 0x0400095A RID: 2394
	public FontType fontType;

	// Token: 0x0400095B RID: 2395
	public int fontSize;

	// Token: 0x0400095C RID: 2396
	public bool selectFirst;

	// Token: 0x0400095D RID: 2397
	public string overrideColor;

	// Token: 0x0400095E RID: 2398
	public string overrideTextColor;

	// Token: 0x0400095F RID: 2399
	public bool disableRectTransformOffsets;

	// Token: 0x04000960 RID: 2400
	public bool overrideBottomOffsetBool;

	// Token: 0x04000961 RID: 2401
	public float overrideBottomOffsetFloat = 6f;

	// Token: 0x04000962 RID: 2402
	private Color disabledColor = Color.black;

	// Token: 0x04000963 RID: 2403
	private Color disabledSelectedColor = Color.black;

	// Token: 0x04000964 RID: 2404
	public bool forceNewStyle;

	// Token: 0x04000965 RID: 2405
	private bool? _useNewStyle;
}
