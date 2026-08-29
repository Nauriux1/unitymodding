using System;
using PaintIn3D;
using UnityEngine;

// Token: 0x02000135 RID: 309
[Serializable]
public class PaintToolDecal : PaintToolItem
{
	// Token: 0x060009A0 RID: 2464 RVA: 0x0002DBD8 File Offset: 0x0002BDD8
	public override void Initialize()
	{
		this.sizeInput.Setup(0.01f, 10f, 1f, false);
		this.sizeInput.ValueChanged += delegate(object <p0>, EventArgs <p1>)
		{
			this.SetSize(this.sizeInput.value);
		};
		this.rotationInput.Setup(-180f, 180f, 0f, false);
		this.rotationInput.ValueChanged += delegate(object <p0>, EventArgs <p1>)
		{
			this.SetRotation(this.rotationInput.value);
		};
		this.imageSelect.ValueChangedEvent += delegate(object <p0>, EventArgs <p1>)
		{
			this.SetImage();
		};
		this.imageSelect.Setup("");
		base.Initialize();
		this.SetSize(this.sizeInput.value);
		this.SetRotation(this.rotationInput.value);
	}

	// Token: 0x060009A1 RID: 2465 RVA: 0x0002DC98 File Offset: 0x0002BE98
	public override void SetColor(Color color)
	{
		base.SetColor(color);
		this.paintDecal.Color = color;
	}

	// Token: 0x060009A2 RID: 2466 RVA: 0x0002DCAD File Offset: 0x0002BEAD
	public void SetSize(float size)
	{
		this.paintDecal.Radius = size / 10f;
	}

	// Token: 0x060009A3 RID: 2467 RVA: 0x0002DCC1 File Offset: 0x0002BEC1
	public void SetRotation(float rotation)
	{
		this.paintDecal.Angle = rotation;
		this.imagePreview.rotation = Quaternion.Euler(0f, 0f, rotation);
	}

	// Token: 0x060009A4 RID: 2468 RVA: 0x0002DCEA File Offset: 0x0002BEEA
	public void SetImage()
	{
		this.paintDecal.Texture = this.imageSelect.texture;
	}

	// Token: 0x040006BF RID: 1727
	public CwPaintDecal paintDecal;

	// Token: 0x040006C0 RID: 1728
	public SliderAndTextSelect sizeInput;

	// Token: 0x040006C1 RID: 1729
	public SliderAndTextSelect rotationInput;

	// Token: 0x040006C2 RID: 1730
	public ImageSelect imageSelect;

	// Token: 0x040006C3 RID: 1731
	public RectTransform imagePreview;
}
