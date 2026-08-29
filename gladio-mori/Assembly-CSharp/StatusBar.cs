using System;
using UnityEngine;
using UnityEngine.UI;
using Utils;

// Token: 0x02000146 RID: 326
public class StatusBar : MonoBehaviour
{
	// Token: 0x06000A22 RID: 2594 RVA: 0x0002FD0F File Offset: 0x0002DF0F
	public void SetValue(float value)
	{
		if (!Generic.FloatEquals(value, this.slider.value))
		{
			this.slider.value = value;
			this.UpdateColor();
		}
	}

	// Token: 0x06000A23 RID: 2595 RVA: 0x0002FD38 File Offset: 0x0002DF38
	public void UpdateColor()
	{
		if (this.slider.value > this.transitionStart)
		{
			this.fillImage.color = this.fullColor;
			return;
		}
		if (this.slider.value > this.transitionMid)
		{
			this.fillImage.color = Color.Lerp(this.midColor, this.effectStartColor, (this.slider.value - this.transitionMid) / this.transitionMid);
			return;
		}
		this.fillImage.color = Color.Lerp(this.emptyColor, this.midColor, this.slider.value / this.transitionMid);
	}

	// Token: 0x04000721 RID: 1825
	public Slider slider;

	// Token: 0x04000722 RID: 1826
	public Image fillImage;

	// Token: 0x04000723 RID: 1827
	public Color fullColor;

	// Token: 0x04000724 RID: 1828
	public Color effectStartColor;

	// Token: 0x04000725 RID: 1829
	public Color midColor;

	// Token: 0x04000726 RID: 1830
	public Color emptyColor;

	// Token: 0x04000727 RID: 1831
	public float transitionStart = 0.7f;

	// Token: 0x04000728 RID: 1832
	public float transitionMid = 0.35f;
}
