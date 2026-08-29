using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000179 RID: 377
public class Paintable : MonoBehaviour, IPaintable
{
	// Token: 0x1700017D RID: 381
	// (get) Token: 0x06000C06 RID: 3078 RVA: 0x00039528 File Offset: 0x00037728
	// (set) Token: 0x06000C07 RID: 3079 RVA: 0x00039530 File Offset: 0x00037730
	public float extendsIslandOffset { get; set; } = 1f;

	// Token: 0x06000C08 RID: 3080 RVA: 0x00039539 File Offset: 0x00037739
	public RenderTexture getMask()
	{
		return this.maskRenderTexture;
	}

	// Token: 0x06000C09 RID: 3081 RVA: 0x00039541 File Offset: 0x00037741
	public RenderTexture getUVIslands()
	{
		return this.uvIslandsRenderTexture;
	}

	// Token: 0x06000C0A RID: 3082 RVA: 0x00039549 File Offset: 0x00037749
	public RenderTexture getExtend()
	{
		return this.extendIslandsRenderTexture;
	}

	// Token: 0x06000C0B RID: 3083 RVA: 0x00039551 File Offset: 0x00037751
	public RenderTexture getSupport()
	{
		return this.supportTexture;
	}

	// Token: 0x06000C0C RID: 3084 RVA: 0x00039559 File Offset: 0x00037759
	public Renderer getRenderer()
	{
		return this.rend;
	}

	// Token: 0x06000C0D RID: 3085 RVA: 0x00039564 File Offset: 0x00037764
	private void Start()
	{
		if (this.playerHealth != null)
		{
			this.playerHealth.InitMaterial();
		}
		else
		{
			Renderer component = base.gameObject.GetComponent<Renderer>();
			if (component != null)
			{
				component.material = new Material(component.material);
			}
		}
		this.maskRenderTexture = new RenderTexture(this.TEXTURE_SIZE, this.TEXTURE_SIZE, 0);
		this.maskRenderTexture.filterMode = FilterMode.Bilinear;
		this.extendIslandsRenderTexture = new RenderTexture(this.TEXTURE_SIZE, this.TEXTURE_SIZE, 0);
		this.extendIslandsRenderTexture.filterMode = FilterMode.Bilinear;
		this.uvIslandsRenderTexture = new RenderTexture(this.TEXTURE_SIZE, this.TEXTURE_SIZE, 0, RenderTextureFormat.R8);
		this.uvIslandsRenderTexture.filterMode = FilterMode.Bilinear;
		this.supportTexture = new RenderTexture(this.TEXTURE_SIZE, this.TEXTURE_SIZE, 0);
		this.supportTexture.filterMode = FilterMode.Bilinear;
		this.maskRenderTexture.Create();
		this.extendIslandsRenderTexture.Create();
		this.uvIslandsRenderTexture.Create();
		this.supportTexture.Create();
		this.initiated = true;
		this.rend = base.GetComponent<Renderer>();
		this.rend.sharedMaterial.SetTexture(this.maskTextureID, this.extendIslandsRenderTexture);
		foreach (PaintableChild paintableChild in this.children)
		{
			paintableChild.Initialize();
		}
		Singleton<PaintManager>.instance.initTextures(this);
		this.bladePaintable.paintable = this;
	}

	// Token: 0x06000C0E RID: 3086 RVA: 0x00039700 File Offset: 0x00037900
	private void OnDestroy()
	{
		if (this.initiated)
		{
			this.maskRenderTexture.Release();
			this.uvIslandsRenderTexture.Release();
			this.extendIslandsRenderTexture.Release();
			this.supportTexture.Release();
			UnityEngine.Object.Destroy(this.maskRenderTexture);
			UnityEngine.Object.Destroy(this.uvIslandsRenderTexture);
			UnityEngine.Object.Destroy(this.extendIslandsRenderTexture);
			UnityEngine.Object.Destroy(this.supportTexture);
		}
	}

	// Token: 0x0400086B RID: 2155
	public int TEXTURE_SIZE = 1024;

	// Token: 0x0400086D RID: 2157
	public RenderTexture extendIslandsRenderTexture;

	// Token: 0x0400086E RID: 2158
	public RenderTexture uvIslandsRenderTexture;

	// Token: 0x0400086F RID: 2159
	public RenderTexture maskRenderTexture;

	// Token: 0x04000870 RID: 2160
	public RenderTexture supportTexture;

	// Token: 0x04000871 RID: 2161
	private Renderer rend;

	// Token: 0x04000872 RID: 2162
	private int maskTextureID = Shader.PropertyToID("_MaskTexture");

	// Token: 0x04000873 RID: 2163
	public List<PaintableChild> children = new List<PaintableChild>();

	// Token: 0x04000874 RID: 2164
	public BladePaintable bladePaintable;

	// Token: 0x04000875 RID: 2165
	public PlayerHealth playerHealth;

	// Token: 0x04000876 RID: 2166
	private bool initiated;
}
