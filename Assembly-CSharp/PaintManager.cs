using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200017B RID: 379
public class PaintManager : Singleton<PaintManager>
{
	// Token: 0x06000C19 RID: 3097 RVA: 0x0003986C File Offset: 0x00037A6C
	public override void Awake()
	{
		base.Awake();
		this.paintMaterial = new Material(this.texturePaint);
		this.extendMaterial = new Material(this.extendIslands);
		this.paintTextureCoordinatesMaterial = new Material(this.paintTextureCoordinates);
		this.command = new CommandBuffer();
		this.command.name = "CommmandBuffer - " + base.gameObject.name;
	}

	// Token: 0x06000C1A RID: 3098 RVA: 0x000398E0 File Offset: 0x00037AE0
	public void initTextures(Paintable paintable)
	{
		RenderTexture mask = paintable.getMask();
		RenderTexture uvislands = paintable.getUVIslands();
		RenderTexture extend = paintable.getExtend();
		RenderTexture support = paintable.getSupport();
		Renderer renderer = paintable.getRenderer();
		this.command.SetRenderTarget(mask);
		this.command.ClearRenderTarget(true, true, Color.clear);
		this.command.SetRenderTarget(extend);
		this.command.ClearRenderTarget(true, true, Color.clear);
		this.command.SetRenderTarget(support);
		this.command.ClearRenderTarget(true, true, Color.clear);
		this.paintMaterial.SetFloat(this.prepareUVID, 1f);
		this.command.SetRenderTarget(uvislands);
		this.command.ClearRenderTarget(true, true, Color.clear);
		this.command.DrawRenderer(renderer, this.paintMaterial, 0);
		foreach (PaintableChild paintableChild in paintable.children)
		{
			this.command.DrawRenderer(paintableChild.getRenderer(), this.paintMaterial, 0);
		}
		Graphics.ExecuteCommandBuffer(this.command);
		this.command.Clear();
		this.SetPixelColor(mask, new Color(1f, 0f, 0f, 1f));
	}

	// Token: 0x06000C1B RID: 3099 RVA: 0x00039A58 File Offset: 0x00037C58
	public void paint(IPaintable paintable, Vector3 pos, float radius = 1f, float hardness = 0.5f, float strength = 0.5f, Color? color = null)
	{
		this.paint(paintable, 1, radius, hardness, strength, color);
	}

	// Token: 0x06000C1C RID: 3100 RVA: 0x00039A6C File Offset: 0x00037C6C
	public void paint(IPaintable paintable, NativeList<Vector3> pos, float radius = 1f, float hardness = 0.5f, float strength = 0.5f, Color? color = null)
	{
		int num = pos.Length;
		if (num > 1000)
		{
			Debug.Log("Too many draw points!");
			num = 1000;
		}
		for (int i = 0; i < num; i++)
		{
			this.positions[i] = pos[i];
		}
		this.paint(paintable, num, radius, hardness, strength, color);
	}

	// Token: 0x06000C1D RID: 3101 RVA: 0x00039AD0 File Offset: 0x00037CD0
	public void paint(IPaintable paintable, int length, float radius = 1f, float hardness = 0.5f, float strength = 0.5f, Color? color = null)
	{
		RenderTexture mask = paintable.getMask();
		RenderTexture uvislands = paintable.getUVIslands();
		RenderTexture extend = paintable.getExtend();
		RenderTexture support = paintable.getSupport();
		Renderer renderer = paintable.getRenderer();
		this.paintMaterial.SetFloat(this.prepareUVID, 0f);
		this.paintMaterial.SetVectorArray(this.positionID, this.positions);
		this.paintMaterial.SetInt(this.positionCountID, length);
		this.paintMaterial.SetFloat(this.hardnessID, hardness);
		this.paintMaterial.SetFloat(this.strengthID, strength);
		this.paintMaterial.SetFloat(this.radiusID, radius);
		this.paintMaterial.SetTexture(this.textureID, support);
		this.paintMaterial.SetColor(this.colorID, color ?? Color.red);
		this.extendMaterial.SetFloat(this.uvOffsetID, paintable.extendsIslandOffset);
		this.extendMaterial.SetTexture(this.uvIslandsID, uvislands);
		this.command.SetRenderTarget(mask);
		this.command.DrawRenderer(renderer, this.paintMaterial, 0);
		this.command.SetRenderTarget(support);
		this.command.Blit(mask, support);
		this.command.SetRenderTarget(extend);
		this.command.Blit(mask, extend, this.extendMaterial);
		Graphics.ExecuteCommandBuffer(this.command);
		this.command.Clear();
	}

	// Token: 0x06000C1E RID: 3102 RVA: 0x00039C68 File Offset: 0x00037E68
	public void paintTriangles(IPaintable paintable, Vector4[] pos, int length, Vector4[] spherePos, int sphereLength, Vector4[] sphereGreenPos, int sphereGreenLength, float radius = 1f, float hardness = 0.5f, float strength = 0.5f, Color? color = null)
	{
		RenderTexture mask = paintable.getMask();
		RenderTexture uvislands = paintable.getUVIslands();
		RenderTexture extend = paintable.getExtend();
		RenderTexture support = paintable.getSupport();
		Renderer renderer = paintable.getRenderer();
		this.paintMaterial.SetFloat(this.prepareUVID, 0f);
		this.paintMaterial.SetVectorArray(this.positionID, pos);
		this.paintMaterial.SetInt(this.positionCountID, length);
		this.paintMaterial.SetVectorArray(this.spherePositionID, spherePos);
		this.paintMaterial.SetInt(this.spherePositionCountID, sphereLength);
		this.paintMaterial.SetVectorArray(this.spherePositionGreenID, sphereGreenPos);
		this.paintMaterial.SetInt(this.spherePositionGreenCountID, sphereGreenLength);
		this.paintMaterial.SetFloat(this.hardnessID, hardness);
		this.paintMaterial.SetFloat(this.strengthID, strength);
		this.paintMaterial.SetFloat(this.radiusID, radius);
		this.paintMaterial.SetTexture(this.textureID, support);
		this.paintMaterial.SetColor(this.colorID, color ?? Color.red);
		this.extendMaterial.SetFloat(this.uvOffsetID, paintable.extendsIslandOffset);
		this.extendMaterial.SetTexture(this.uvIslandsID, uvislands);
		this.command.SetRenderTarget(mask);
		this.command.DrawRenderer(renderer, this.paintMaterial, 0);
		this.command.SetRenderTarget(support);
		this.command.Blit(mask, support);
		this.command.SetRenderTarget(extend);
		this.command.Blit(mask, extend, this.extendMaterial);
		Graphics.ExecuteCommandBuffer(this.command);
		this.command.Clear();
	}

	// Token: 0x06000C1F RID: 3103 RVA: 0x00039E46 File Offset: 0x00038046
	private void SetPixelColor(RenderTexture rt, Color color)
	{
		this.paintTextureCoordinatesMaterial.SetColor("_Color", color);
		Graphics.Blit(null, rt, this.paintTextureCoordinatesMaterial);
	}

	// Token: 0x06000C20 RID: 3104 RVA: 0x00039E68 File Offset: 0x00038068
	private void SetPixelColor(RenderTexture rt, int x, int y, Color color)
	{
		RenderTexture.active = rt;
		Texture2D texture2D = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
		texture2D.ReadPixels(new Rect(0f, 0f, (float)rt.width, (float)rt.height), 0, 0);
		texture2D.Apply();
		texture2D.SetPixel(x, y, color);
		texture2D.Apply();
		Graphics.Blit(texture2D, rt);
		RenderTexture.active = null;
		UnityEngine.Object.Destroy(texture2D);
	}

	// Token: 0x0400087C RID: 2172
	public Shader texturePaint;

	// Token: 0x0400087D RID: 2173
	public Shader extendIslands;

	// Token: 0x0400087E RID: 2174
	public Shader paintTextureCoordinates;

	// Token: 0x0400087F RID: 2175
	private int prepareUVID = Shader.PropertyToID("_PrepareUV");

	// Token: 0x04000880 RID: 2176
	private int positionID = Shader.PropertyToID("_PainterPositions");

	// Token: 0x04000881 RID: 2177
	private int positionCountID = Shader.PropertyToID("_PainterPositionCount");

	// Token: 0x04000882 RID: 2178
	private int spherePositionID = Shader.PropertyToID("_PainterSpherePositions");

	// Token: 0x04000883 RID: 2179
	private int spherePositionCountID = Shader.PropertyToID("_PainterSpherePositionCount");

	// Token: 0x04000884 RID: 2180
	private int spherePositionGreenID = Shader.PropertyToID("_PainterSpherePositionsGreen");

	// Token: 0x04000885 RID: 2181
	private int spherePositionGreenCountID = Shader.PropertyToID("_PainterSpherePositionGreenCount");

	// Token: 0x04000886 RID: 2182
	private int hardnessID = Shader.PropertyToID("_Hardness");

	// Token: 0x04000887 RID: 2183
	private int strengthID = Shader.PropertyToID("_Strength");

	// Token: 0x04000888 RID: 2184
	private int radiusID = Shader.PropertyToID("_Radius");

	// Token: 0x04000889 RID: 2185
	private int blendOpID = Shader.PropertyToID("_BlendOp");

	// Token: 0x0400088A RID: 2186
	private int colorID = Shader.PropertyToID("_PainterColor");

	// Token: 0x0400088B RID: 2187
	private int textureID = Shader.PropertyToID("_MainTex");

	// Token: 0x0400088C RID: 2188
	private int uvOffsetID = Shader.PropertyToID("_OffsetUV");

	// Token: 0x0400088D RID: 2189
	private int uvIslandsID = Shader.PropertyToID("_UVIslands");

	// Token: 0x0400088E RID: 2190
	private Material paintMaterial;

	// Token: 0x0400088F RID: 2191
	private Material extendMaterial;

	// Token: 0x04000890 RID: 2192
	private Material paintTextureCoordinatesMaterial;

	// Token: 0x04000891 RID: 2193
	private CommandBuffer command;

	// Token: 0x04000892 RID: 2194
	private Vector4[] positions = new Vector4[1000];
}
