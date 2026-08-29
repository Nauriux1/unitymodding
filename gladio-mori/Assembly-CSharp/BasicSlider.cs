using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

// Token: 0x020001B3 RID: 435
public class BasicSlider : MonoBehaviour, ISelectHandler, IEventSystemHandler, IDeselectHandler
{
	// Token: 0x06000D40 RID: 3392 RVA: 0x00043358 File Offset: 0x00041558
	private void Awake()
	{
		this.slider = base.gameObject.GetComponent<Slider>();
		if (this.slider != null)
		{
			UIHelpers.SetSliderColor(this.slider);
			this.disabledColor = this.slider.colors.disabledColor;
			this.disabledSelectedColor = UIHelpers.HighlightColor(this.disabledColor);
		}
	}

	// Token: 0x06000D41 RID: 3393 RVA: 0x000433BC File Offset: 0x000415BC
	public void OnSelect(BaseEventData eventData)
	{
		ColorBlock colors = this.slider.colors;
		colors.disabledColor = this.disabledSelectedColor;
		this.slider.colors = colors;
	}

	// Token: 0x06000D42 RID: 3394 RVA: 0x000433F0 File Offset: 0x000415F0
	public void OnDeselect(BaseEventData eventData)
	{
		ColorBlock colors = this.slider.colors;
		colors.disabledColor = this.disabledColor;
		this.slider.colors = colors;
	}

	// Token: 0x0400098D RID: 2445
	private Slider slider;

	// Token: 0x0400098E RID: 2446
	private Color disabledColor = Color.black;

	// Token: 0x0400098F RID: 2447
	private Color disabledSelectedColor = Color.black;
}
