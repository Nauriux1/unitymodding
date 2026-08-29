using System;
using UnityEngine;

// Token: 0x0200017A RID: 378
public class PaintableChild : MonoBehaviour, IPaintable
{
	// Token: 0x1700017E RID: 382
	// (get) Token: 0x06000C10 RID: 3088 RVA: 0x000397A6 File Offset: 0x000379A6
	// (set) Token: 0x06000C11 RID: 3089 RVA: 0x000397AE File Offset: 0x000379AE
	public float extendsIslandOffset { get; set; } = 1f;

	// Token: 0x06000C12 RID: 3090 RVA: 0x000397B7 File Offset: 0x000379B7
	public RenderTexture getMask()
	{
		return this.parentPaintable.getMask();
	}

	// Token: 0x06000C13 RID: 3091 RVA: 0x000397C4 File Offset: 0x000379C4
	public RenderTexture getUVIslands()
	{
		return this.parentPaintable.getUVIslands();
	}

	// Token: 0x06000C14 RID: 3092 RVA: 0x000397D1 File Offset: 0x000379D1
	public RenderTexture getExtend()
	{
		return this.parentPaintable.getExtend();
	}

	// Token: 0x06000C15 RID: 3093 RVA: 0x000397DE File Offset: 0x000379DE
	public RenderTexture getSupport()
	{
		return this.parentPaintable.getSupport();
	}

	// Token: 0x06000C16 RID: 3094 RVA: 0x000397EB File Offset: 0x000379EB
	public Renderer getRenderer()
	{
		return this.rend;
	}

	// Token: 0x06000C17 RID: 3095 RVA: 0x000397F4 File Offset: 0x000379F4
	public void Initialize()
	{
		this.rend = base.GetComponent<Renderer>();
		Material sharedMaterial = this.parentPaintable.getRenderer().sharedMaterial;
		if (this.rend.sharedMaterial != sharedMaterial)
		{
			this.rend.sharedMaterial = sharedMaterial;
		}
		this.bladePaintable.paintable = this;
	}

	// Token: 0x04000877 RID: 2167
	public Paintable parentPaintable;

	// Token: 0x04000879 RID: 2169
	private Renderer rend;

	// Token: 0x0400087A RID: 2170
	private int maskTextureID = Shader.PropertyToID("_MaskTexture");

	// Token: 0x0400087B RID: 2171
	public BladePaintable bladePaintable;
}
