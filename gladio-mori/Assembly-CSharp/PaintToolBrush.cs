using System;
using PaintIn3D;
using UnityEngine;

// Token: 0x02000134 RID: 308
[Serializable]
public class PaintToolBrush : PaintToolItem
{
	// Token: 0x06000999 RID: 2457 RVA: 0x0002DAD8 File Offset: 0x0002BCD8
	public override void Initialize()
	{
		this.sizeInput.Setup(0.01f, 10f, 1f, false);
		this.sizeInput.ValueChanged += delegate(object <p0>, EventArgs <p1>)
		{
			this.SetSize(this.sizeInput.value);
		};
		this.hardnessInput.Setup(0.01f, 100f, 5f, false);
		this.hardnessInput.ValueChanged += delegate(object <p0>, EventArgs <p1>)
		{
			this.SetHardness(this.hardnessInput.value);
		};
		base.Initialize();
		this.SetSize(this.sizeInput.value);
		this.SetHardness(this.hardnessInput.value);
	}

	// Token: 0x0600099A RID: 2458 RVA: 0x0002DB71 File Offset: 0x0002BD71
	public override void SetColor(Color color)
	{
		base.SetColor(color);
		this.paintSphere.Color = color;
	}

	// Token: 0x0600099B RID: 2459 RVA: 0x0002DB86 File Offset: 0x0002BD86
	public void SetSize(float size)
	{
		this.paintSphere.Radius = size / 10f;
	}

	// Token: 0x0600099C RID: 2460 RVA: 0x0002DB9A File Offset: 0x0002BD9A
	public void SetHardness(float hardness)
	{
		this.paintSphere.Hardness = hardness;
	}

	// Token: 0x040006BC RID: 1724
	public CwPaintSphere paintSphere;

	// Token: 0x040006BD RID: 1725
	public SliderAndTextSelect sizeInput;

	// Token: 0x040006BE RID: 1726
	public SliderAndTextSelect hardnessInput;
}
