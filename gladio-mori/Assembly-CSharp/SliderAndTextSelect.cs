using System;
using UnityEngine;
using UnityEngine.UI;
using Utils;

// Token: 0x020001D2 RID: 466
public class SliderAndTextSelect : MonoBehaviour
{
	// Token: 0x14000005 RID: 5
	// (add) Token: 0x06000DE1 RID: 3553 RVA: 0x00046248 File Offset: 0x00044448
	// (remove) Token: 0x06000DE2 RID: 3554 RVA: 0x00046280 File Offset: 0x00044480
	public event EventHandler ValueChanged;

	// Token: 0x06000DE3 RID: 3555 RVA: 0x000462B5 File Offset: 0x000444B5
	private void Start()
	{
		this.slider.onValueChanged.AddListener(delegate(float <p0>)
		{
			this.ChangeValue(this.slider.value);
		});
		this.inputField.onEndEdit.AddListener(delegate(string <p0>)
		{
			this.ChangeValueText(this.inputField.text);
		});
	}

	// Token: 0x06000DE4 RID: 3556 RVA: 0x000462F0 File Offset: 0x000444F0
	public void Setup(float newMinValue, float newMaxValue, float newValue = 0f, bool wholeNumbers = false)
	{
		this.minValue = newMinValue;
		this.maxValue = newMaxValue;
		this.slider.minValue = this.minValue;
		this.slider.maxValue = this.maxValue;
		this.slider.wholeNumbers = wholeNumbers;
		if (wholeNumbers)
		{
			this.inputField.contentType = InputField.ContentType.IntegerNumber;
		}
		this.value = newValue;
		this.RefreshShownValue();
	}

	// Token: 0x06000DE5 RID: 3557 RVA: 0x00046358 File Offset: 0x00044558
	public void ChangeValueText(string newTextValue)
	{
		float newValue = 0f;
		Generic.ConvertToRoundedFloat(newTextValue, out newValue, out newTextValue);
		this.ChangeValue(newValue);
	}

	// Token: 0x06000DE6 RID: 3558 RVA: 0x0004637D File Offset: 0x0004457D
	public void ChangeValue(float newValue)
	{
		this.value = newValue;
		this.ValidateCurrentValue();
		if (this.ValueChanged != null)
		{
			this.ValueChanged(this, EventArgs.Empty);
		}
		this.RefreshShownValue();
	}

	// Token: 0x06000DE7 RID: 3559 RVA: 0x000463AB File Offset: 0x000445AB
	public void ValidateCurrentValue()
	{
		if (this.value < this.minValue)
		{
			this.value = this.minValue;
		}
		if (this.maxValue < this.value)
		{
			this.value = this.maxValue;
		}
	}

	// Token: 0x06000DE8 RID: 3560 RVA: 0x000463E4 File Offset: 0x000445E4
	public void RefreshShownValue()
	{
		this.slider.SetValueWithoutNotify(this.value);
		if (this.slider.wholeNumbers)
		{
			this.inputField.SetTextWithoutNotify(((int)this.value).ToString());
			return;
		}
		this.inputField.SetTextWithoutNotify(this.value.ToString("F"));
	}

	// Token: 0x040009FC RID: 2556
	public Slider slider;

	// Token: 0x040009FD RID: 2557
	public InputField inputField;

	// Token: 0x040009FE RID: 2558
	public float value;

	// Token: 0x040009FF RID: 2559
	public float minValue;

	// Token: 0x04000A00 RID: 2560
	public float maxValue = 1f;
}
