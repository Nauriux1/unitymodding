using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200012E RID: 302
public class PaintEditorColorPicker : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IBeginDragHandler, IDragHandler
{
	// Token: 0x0600095F RID: 2399 RVA: 0x0002CC19 File Offset: 0x0002AE19
	private void Start()
	{
		this.colorChart = this.GenerateHSVTexture();
		this.colorChart.Apply();
		this.colorChartRawImage.texture = this.colorChart;
		this.SetColor(Color.red);
	}

	// Token: 0x06000960 RID: 2400 RVA: 0x0002CC4E File Offset: 0x0002AE4E
	public void OnBeginDrag(PointerEventData eventData)
	{
		this.UpdateColor(eventData);
	}

	// Token: 0x06000961 RID: 2401 RVA: 0x0002CC4E File Offset: 0x0002AE4E
	public void OnDrag(PointerEventData eventData)
	{
		this.UpdateColor(eventData);
	}

	// Token: 0x06000962 RID: 2402 RVA: 0x0002CC4E File Offset: 0x0002AE4E
	public void OnPointerDown(PointerEventData eventData)
	{
		this.UpdateColor(eventData);
	}

	// Token: 0x06000963 RID: 2403 RVA: 0x0002CC58 File Offset: 0x0002AE58
	private void UpdateColor(PointerEventData eventData)
	{
		RectTransformUtility.ScreenPointToLocalPointInRectangle(this.colorChartTransform, eventData.position, Camera.current, out this.localColorPosition);
		int x = Mathf.Clamp((int)(this.localColorPosition.x * ((float)this.colorChart.width / this.colorChartTransform.rect.width)), 0, this.colorChart.width - 1);
		int y = Mathf.Clamp((int)(this.localColorPosition.y * ((float)this.colorChart.height / this.colorChartTransform.rect.height)), 0, this.colorChart.height - 1);
		Color pixel = this.colorChart.GetPixel(x, y);
		this.SetColor(pixel);
	}

	// Token: 0x06000964 RID: 2404 RVA: 0x0002CD19 File Offset: 0x0002AF19
	public void SetColor(Color color)
	{
		this.currentColor.color = color;
		PaintEditorManager.SetColor(color);
	}

	// Token: 0x06000965 RID: 2405 RVA: 0x0002CD30 File Offset: 0x0002AF30
	private Texture2D GenerateHSVTexture()
	{
		int num = this.colorChartResolution;
		int num2 = this.colorChartResolution + this.grayScaleWidth;
		Texture2D texture2D = new Texture2D(num2, num, TextureFormat.RGBA32, false);
		texture2D.wrapMode = TextureWrapMode.Clamp;
		float s = 0f;
		float s2 = 1f;
		for (int i = 0; i < num; i++)
		{
			float l = (float)i / (float)(num - 1);
			for (int j = 0; j < num2; j++)
			{
				if (j < this.colorChartResolution)
				{
					float h = (float)j / (float)(num2 - 1);
					Color color = this.HSLToRGB(h, s2, l);
					texture2D.SetPixel(j, i, color);
				}
				else
				{
					Color color2 = this.HSLToRGB(0f, s, l);
					texture2D.SetPixel(j, i, color2);
				}
			}
		}
		return texture2D;
	}

	// Token: 0x06000966 RID: 2406 RVA: 0x0002CDE8 File Offset: 0x0002AFE8
	private Color HSLToRGB(float h, float s, float l)
	{
		float b;
		float g;
		float r;
		if (s == 0f)
		{
			b = l;
			g = l;
			r = l;
		}
		else
		{
			float num = (l < 0.5f) ? (l * (1f + s)) : (l + s - l * s);
			float p = 2f * l - num;
			r = this.HueToRGB(p, num, h + 0.33333334f);
			g = this.HueToRGB(p, num, h);
			b = this.HueToRGB(p, num, h - 0.33333334f);
		}
		return new Color(r, g, b);
	}

	// Token: 0x06000967 RID: 2407 RVA: 0x0002CE60 File Offset: 0x0002B060
	private float HueToRGB(float p, float q, float t)
	{
		if (t < 0f)
		{
			t += 1f;
		}
		if (t > 1f)
		{
			t -= 1f;
		}
		if (t < 0.16666667f)
		{
			return p + (q - p) * 6f * t;
		}
		if (t < 0.5f)
		{
			return q;
		}
		if (t < 0.6666667f)
		{
			return p + (q - p) * (0.6666667f - t) * 6f;
		}
		return p;
	}

	// Token: 0x04000693 RID: 1683
	public Texture2D colorChart;

	// Token: 0x04000694 RID: 1684
	public RectTransform colorChartTransform;

	// Token: 0x04000695 RID: 1685
	public RawImage colorChartRawImage;

	// Token: 0x04000696 RID: 1686
	public Image currentColor;

	// Token: 0x04000697 RID: 1687
	private Vector2 localColorPosition;

	// Token: 0x04000698 RID: 1688
	private int colorChartResolution = 256;

	// Token: 0x04000699 RID: 1689
	private int grayScaleWidth = 10;
}
