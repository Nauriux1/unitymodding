using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

// Token: 0x020001BA RID: 442
public class BasicToggle : MonoBehaviour, ISelectHandler, IEventSystemHandler, IDeselectHandler
{
	// Token: 0x06000D55 RID: 3413 RVA: 0x00043BD4 File Offset: 0x00041DD4
	private void Awake()
	{
		this.toggle = base.gameObject.GetComponent<Toggle>();
		if (this.toggle != null)
		{
			UIHelpers.SetToggleColor(this.toggle);
			this.disabledColor = this.toggle.colors.disabledColor;
			this.disabledSelectedColor = UIHelpers.HighlightColor(this.disabledColor);
			Transform transform = this.toggle.transform.Find("Label");
			if (transform != null)
			{
				Text component = transform.GetComponent<Text>();
				if (component != null)
				{
					UIHelpers.SetTextFont(component, FontType.Options);
					component.resizeTextForBestFit = true;
					component.resizeTextMinSize = 6;
					component.resizeTextMaxSize = component.fontSize;
				}
			}
		}
	}

	// Token: 0x06000D56 RID: 3414 RVA: 0x00043C88 File Offset: 0x00041E88
	public void OnSelect(BaseEventData eventData)
	{
		ColorBlock colors = this.toggle.colors;
		colors.disabledColor = this.disabledSelectedColor;
		this.toggle.colors = colors;
	}

	// Token: 0x06000D57 RID: 3415 RVA: 0x00043CBC File Offset: 0x00041EBC
	public void OnDeselect(BaseEventData eventData)
	{
		ColorBlock colors = this.toggle.colors;
		colors.disabledColor = this.disabledColor;
		this.toggle.colors = colors;
	}

	// Token: 0x040009A1 RID: 2465
	private Toggle toggle;

	// Token: 0x040009A2 RID: 2466
	private Color disabledColor = Color.black;

	// Token: 0x040009A3 RID: 2467
	private Color disabledSelectedColor = Color.black;

	// Token: 0x040009A4 RID: 2468
	private Text text;
}
