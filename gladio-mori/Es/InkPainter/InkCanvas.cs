using System;
using System.Collections.Generic;
using System.Linq;
using Es.InkPainter.Effective;
using UnityEngine;

namespace Es.InkPainter
{
	// Token: 0x020002E9 RID: 745
	[RequireComponent(typeof(Renderer))]
	[DisallowMultipleComponent]
	public class InkCanvas : MonoBehaviour
	{
		// Token: 0x17000297 RID: 663
		// (get) Token: 0x060016C5 RID: 5829 RVA: 0x00072E2D File Offset: 0x0007102D
		// (set) Token: 0x060016C6 RID: 5830 RVA: 0x00072E35 File Offset: 0x00071035
		public List<InkCanvas.PaintSet> PaintDatas
		{
			get
			{
				return this.paintSet;
			}
			set
			{
				this.paintSet = value;
			}
		}

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x060016C7 RID: 5831 RVA: 0x00072E40 File Offset: 0x00071040
		// (remove) Token: 0x060016C8 RID: 5832 RVA: 0x00072E78 File Offset: 0x00071078
		public event Action<InkCanvas> OnCanvasAttached;

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x060016C9 RID: 5833 RVA: 0x00072EB0 File Offset: 0x000710B0
		// (remove) Token: 0x060016CA RID: 5834 RVA: 0x00072EE8 File Offset: 0x000710E8
		public event Action<InkCanvas> OnInitializedStart;

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x060016CB RID: 5835 RVA: 0x00072F20 File Offset: 0x00071120
		// (remove) Token: 0x060016CC RID: 5836 RVA: 0x00072F58 File Offset: 0x00071158
		public event Action<InkCanvas> OnInitializedAfter;

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x060016CD RID: 5837 RVA: 0x00072F90 File Offset: 0x00071190
		// (remove) Token: 0x060016CE RID: 5838 RVA: 0x00072FC8 File Offset: 0x000711C8
		public event Action<InkCanvas, Brush> OnPaintStart;

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x060016CF RID: 5839 RVA: 0x00073000 File Offset: 0x00071200
		// (remove) Token: 0x060016D0 RID: 5840 RVA: 0x00073038 File Offset: 0x00071238
		public event Action<InkCanvas> OnPaintEnd;

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x060016D1 RID: 5841 RVA: 0x0007306D File Offset: 0x0007126D
		public MeshOperator MeshOperator
		{
			get
			{
				if (this.meshOperator == null)
				{
					Debug.LogError("To take advantage of the features must Mesh filter or Skinned mesh renderer component associated Mesh.");
				}
				return this.meshOperator;
			}
		}

		// Token: 0x060016D2 RID: 5842 RVA: 0x00073087 File Offset: 0x00071287
		private void Awake()
		{
			if (this.OnCanvasAttached != null)
			{
				this.OnCanvasAttached(this);
			}
			this.InitPropertyID();
			this.SetMaterial();
			this.SetTexture();
			this.MeshDataCache();
		}

		// Token: 0x060016D3 RID: 5843 RVA: 0x000730B5 File Offset: 0x000712B5
		private void Start()
		{
			if (this.OnInitializedStart != null)
			{
				this.OnInitializedStart(this);
			}
			this.SetRenderTexture();
			if (this.OnInitializedAfter != null)
			{
				this.OnInitializedAfter(this);
			}
		}

		// Token: 0x060016D4 RID: 5844 RVA: 0x000730E5 File Offset: 0x000712E5
		private void OnDestroy()
		{
			Debug.Log("InkCanvas has been destroyed.");
			this.ReleaseRenderTexture();
		}

		// Token: 0x060016D5 RID: 5845 RVA: 0x000730F8 File Offset: 0x000712F8
		private void OnGUI()
		{
			if (this.eraserDebug)
			{
				if (this.debugEraserMainView != null)
				{
					GUI.DrawTexture(new Rect(0f, 0f, 100f, 100f), this.debugEraserMainView);
				}
				if (this.debugEraserNormalView != null)
				{
					GUI.DrawTexture(new Rect(0f, 100f, 100f, 100f), this.debugEraserNormalView);
				}
				if (this.debugEraserHeightView != null)
				{
					GUI.DrawTexture(new Rect(0f, 200f, 100f, 100f), this.debugEraserHeightView);
				}
			}
		}

		// Token: 0x060016D6 RID: 5846 RVA: 0x000731A8 File Offset: 0x000713A8
		private void MeshDataCache()
		{
			MeshFilter component = base.GetComponent<MeshFilter>();
			SkinnedMeshRenderer component2 = base.GetComponent<SkinnedMeshRenderer>();
			if (component != null)
			{
				this.meshOperator = new MeshOperator(component.sharedMesh);
				return;
			}
			if (component2 != null)
			{
				this.meshOperator = new MeshOperator(component2.sharedMesh);
				return;
			}
			Debug.LogWarning("Sometimes if the MeshFilter or SkinnedMeshRenderer does not exist in the component part does not work correctly.");
		}

		// Token: 0x060016D7 RID: 5847 RVA: 0x00073204 File Offset: 0x00071404
		private void InitPropertyID()
		{
			foreach (InkCanvas.PaintSet paintSet in this.paintSet)
			{
				paintSet.mainTexturePropertyID = Shader.PropertyToID(paintSet.mainTextureName);
				paintSet.normalTexturePropertyID = Shader.PropertyToID(paintSet.normalTextureName);
				paintSet.heightTexturePropertyID = Shader.PropertyToID(paintSet.heightTextureName);
			}
			this.paintUVPropertyID = Shader.PropertyToID("_PaintUV");
			this.brushTexturePropertyID = Shader.PropertyToID("_Brush");
			this.brushScalePropertyID = Shader.PropertyToID("_BrushScale");
			this.brushRotatePropertyID = Shader.PropertyToID("_BrushRotate");
			this.brushColorPropertyID = Shader.PropertyToID("_ControlColor");
			this.brushNormalTexturePropertyID = Shader.PropertyToID("_BrushNormal");
			this.brushNormalBlendPropertyID = Shader.PropertyToID("_NormalBlend");
			this.brushHeightTexturePropertyID = Shader.PropertyToID("_BrushHeight");
			this.brushHeightBlendPropertyID = Shader.PropertyToID("_HeightBlend");
			this.brushHeightColorPropertyID = Shader.PropertyToID("_Color");
		}

		// Token: 0x060016D8 RID: 5848 RVA: 0x00073324 File Offset: 0x00071524
		private void SetMaterial()
		{
			if (InkCanvas.paintMainMaterial == null)
			{
				InkCanvas.paintMainMaterial = new Material(Resources.Load<Material>("Es.InkPainter.PaintMain"));
			}
			if (InkCanvas.paintNormalMaterial == null)
			{
				InkCanvas.paintNormalMaterial = new Material(Resources.Load<Material>("Es.InkPainter.PaintNormal"));
			}
			if (InkCanvas.paintHeightMaterial == null)
			{
				InkCanvas.paintHeightMaterial = new Material(Resources.Load<Material>("Es.InkPainter.PaintHeight"));
			}
			Material[] materials = base.GetComponent<Renderer>().materials;
			for (int i = 0; i < materials.Length; i++)
			{
				if (this.paintSet[i].material == null)
				{
					this.paintSet[i].material = materials[i];
				}
			}
		}

		// Token: 0x060016D9 RID: 5849 RVA: 0x000733DC File Offset: 0x000715DC
		private void SetTexture()
		{
			foreach (InkCanvas.PaintSet paintSet in this.paintSet)
			{
				if (paintSet.material.HasProperty(paintSet.mainTexturePropertyID))
				{
					paintSet.mainTexture = paintSet.material.GetTexture(paintSet.mainTexturePropertyID);
				}
				if (paintSet.material.HasProperty(paintSet.normalTexturePropertyID))
				{
					paintSet.normalTexture = paintSet.material.GetTexture(paintSet.normalTexturePropertyID);
				}
				if (paintSet.material.HasProperty(paintSet.heightTexturePropertyID))
				{
					paintSet.heightTexture = paintSet.material.GetTexture(paintSet.heightTexturePropertyID);
				}
			}
		}

		// Token: 0x060016DA RID: 5850 RVA: 0x000734AC File Offset: 0x000716AC
		private RenderTexture SetupRenderTexture(Texture baseTex, int propertyID, Material material)
		{
			RenderTexture renderTexture = new RenderTexture(baseTex.width, baseTex.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
			renderTexture.filterMode = baseTex.filterMode;
			Graphics.Blit(baseTex, renderTexture);
			material.SetTexture(propertyID, renderTexture);
			return renderTexture;
		}

		// Token: 0x060016DB RID: 5851 RVA: 0x000734EC File Offset: 0x000716EC
		private void SetRenderTexture()
		{
			foreach (InkCanvas.PaintSet paintSet in this.paintSet)
			{
				if (paintSet.useMainPaint)
				{
					if (paintSet.mainTexture != null)
					{
						paintSet.paintMainTexture = this.SetupRenderTexture(paintSet.mainTexture, paintSet.mainTexturePropertyID, paintSet.material);
					}
					else
					{
						Debug.LogWarning("To take advantage of the main texture paint must set main texture to materials.");
					}
				}
				if (paintSet.useNormalPaint)
				{
					if (paintSet.normalTexture != null)
					{
						paintSet.paintNormalTexture = this.SetupRenderTexture(paintSet.normalTexture, paintSet.normalTexturePropertyID, paintSet.material);
					}
					else
					{
						Debug.LogWarning("To take advantage of the normal map paint must set normal map to materials.");
					}
				}
				if (paintSet.useHeightPaint)
				{
					if (paintSet.heightTexture != null)
					{
						paintSet.paintHeightTexture = this.SetupRenderTexture(paintSet.heightTexture, paintSet.heightTexturePropertyID, paintSet.material);
					}
					else
					{
						Debug.LogWarning("To take advantage of the height map paint must set height map to materials.");
					}
				}
			}
		}

		// Token: 0x060016DC RID: 5852 RVA: 0x00073600 File Offset: 0x00071800
		private void ReleaseRenderTexture()
		{
			foreach (InkCanvas.PaintSet paintSet in this.paintSet)
			{
				if (RenderTexture.active != paintSet.paintMainTexture && paintSet.paintMainTexture != null && paintSet.paintMainTexture.IsCreated())
				{
					paintSet.paintMainTexture.Release();
				}
				if (RenderTexture.active != paintSet.paintNormalTexture && paintSet.paintNormalTexture != null && paintSet.paintNormalTexture.IsCreated())
				{
					paintSet.paintNormalTexture.Release();
				}
				if (RenderTexture.active != paintSet.paintHeightTexture && paintSet.paintHeightTexture != null && paintSet.paintHeightTexture.IsCreated())
				{
					paintSet.paintHeightTexture.Release();
				}
			}
		}

		// Token: 0x060016DD RID: 5853 RVA: 0x000736FC File Offset: 0x000718FC
		private void SetPaintMainData(Brush brush, Vector2 uv)
		{
			InkCanvas.paintMainMaterial.SetVector(this.paintUVPropertyID, uv);
			InkCanvas.paintMainMaterial.SetTexture(this.brushTexturePropertyID, brush.BrushTexture);
			InkCanvas.paintMainMaterial.SetFloat(this.brushScalePropertyID, brush.Scale);
			InkCanvas.paintMainMaterial.SetFloat(this.brushRotatePropertyID, brush.RotateAngle);
			InkCanvas.paintMainMaterial.SetVector(this.brushColorPropertyID, brush.Color);
			foreach (string keyword in InkCanvas.paintMainMaterial.shaderKeywords)
			{
				InkCanvas.paintMainMaterial.DisableKeyword(keyword);
			}
			switch (brush.ColorBlending)
			{
			case Brush.ColorBlendType.UseColor:
				InkCanvas.paintMainMaterial.EnableKeyword("INK_PAINTER_COLOR_BLEND_USE_CONTROL");
				return;
			case Brush.ColorBlendType.UseBrush:
				InkCanvas.paintMainMaterial.EnableKeyword("INK_PAINTER_COLOR_BLEND_USE_BRUSH");
				return;
			case Brush.ColorBlendType.Neutral:
				InkCanvas.paintMainMaterial.EnableKeyword("INK_PAINTER_COLOR_BLEND_NEUTRAL");
				return;
			case Brush.ColorBlendType.AlphaOnly:
				InkCanvas.paintMainMaterial.EnableKeyword("INK_PAINTER_COLOR_BLEND_ALPHA_ONLY");
				return;
			default:
				InkCanvas.paintMainMaterial.EnableKeyword("INK_PAINTER_COLOR_BLEND_USE_CONTROL");
				return;
			}
		}

		// Token: 0x060016DE RID: 5854 RVA: 0x00073814 File Offset: 0x00071A14
		private void SetPaintNormalData(Brush brush, Vector2 uv, bool erase)
		{
			InkCanvas.paintNormalMaterial.SetVector(this.paintUVPropertyID, uv);
			InkCanvas.paintNormalMaterial.SetTexture(this.brushTexturePropertyID, brush.BrushTexture);
			InkCanvas.paintNormalMaterial.SetTexture(this.brushNormalTexturePropertyID, brush.BrushNormalTexture);
			InkCanvas.paintNormalMaterial.SetFloat(this.brushScalePropertyID, brush.Scale);
			InkCanvas.paintNormalMaterial.SetFloat(this.brushRotatePropertyID, brush.RotateAngle);
			InkCanvas.paintNormalMaterial.SetFloat(this.brushNormalBlendPropertyID, brush.NormalBlend);
			foreach (string keyword in InkCanvas.paintNormalMaterial.shaderKeywords)
			{
				InkCanvas.paintNormalMaterial.DisableKeyword(keyword);
			}
			switch (brush.NormalBlending)
			{
			case Brush.NormalBlendType.UseBrush:
				InkCanvas.paintNormalMaterial.EnableKeyword("INK_PAINTER_NORMAL_BLEND_USE_BRUSH");
				break;
			case Brush.NormalBlendType.Add:
				InkCanvas.paintNormalMaterial.EnableKeyword("INK_PAINTER_NORMAL_BLEND_ADD");
				break;
			case Brush.NormalBlendType.Sub:
				InkCanvas.paintNormalMaterial.EnableKeyword("INK_PAINTER_NORMAL_BLEND_SUB");
				break;
			case Brush.NormalBlendType.Min:
				InkCanvas.paintNormalMaterial.EnableKeyword("INK_PAINTER_NORMAL_BLEND_MIN");
				break;
			case Brush.NormalBlendType.Max:
				InkCanvas.paintNormalMaterial.EnableKeyword("INK_PAINTER_NORMAL_BLEND_MAX");
				break;
			default:
				InkCanvas.paintNormalMaterial.EnableKeyword("INK_PAINTER_NORMAL_BLEND_USE_BRUSH");
				break;
			}
			if (erase)
			{
				InkCanvas.paintNormalMaterial.EnableKeyword("DXT5NM_COMPRESS_UNUSE");
				return;
			}
			InkCanvas.paintNormalMaterial.EnableKeyword("DXT5NM_COMPRESS_USE");
		}

		// Token: 0x060016DF RID: 5855 RVA: 0x00073978 File Offset: 0x00071B78
		private void SetPaintHeightData(Brush brush, Vector2 uv)
		{
			InkCanvas.paintHeightMaterial.SetVector(this.paintUVPropertyID, uv);
			InkCanvas.paintHeightMaterial.SetTexture(this.brushTexturePropertyID, brush.BrushTexture);
			InkCanvas.paintHeightMaterial.SetTexture(this.brushHeightTexturePropertyID, brush.BrushHeightTexture);
			InkCanvas.paintHeightMaterial.SetFloat(this.brushScalePropertyID, brush.Scale);
			InkCanvas.paintHeightMaterial.SetFloat(this.brushRotatePropertyID, brush.RotateAngle);
			InkCanvas.paintHeightMaterial.SetFloat(this.brushHeightBlendPropertyID, brush.HeightBlend);
			InkCanvas.paintHeightMaterial.SetVector(this.brushHeightColorPropertyID, brush.Color);
			foreach (string keyword in InkCanvas.paintHeightMaterial.shaderKeywords)
			{
				InkCanvas.paintHeightMaterial.DisableKeyword(keyword);
			}
			switch (brush.HeightBlending)
			{
			case Brush.HeightBlendType.UseBrush:
				InkCanvas.paintHeightMaterial.EnableKeyword("INK_PAINTER_HEIGHT_BLEND_USE_BRUSH");
				return;
			case Brush.HeightBlendType.Add:
				InkCanvas.paintHeightMaterial.EnableKeyword("INK_PAINTER_HEIGHT_BLEND_ADD");
				return;
			case Brush.HeightBlendType.Sub:
				InkCanvas.paintHeightMaterial.EnableKeyword("INK_PAINTER_HEIGHT_BLEND_SUB");
				return;
			case Brush.HeightBlendType.Min:
				InkCanvas.paintHeightMaterial.EnableKeyword("INK_PAINTER_HEIGHT_BLEND_MIN");
				return;
			case Brush.HeightBlendType.Max:
				InkCanvas.paintHeightMaterial.EnableKeyword("INK_PAINTER_HEIGHT_BLEND_MAX");
				return;
			case Brush.HeightBlendType.ColorRGB_HeightA:
				InkCanvas.paintHeightMaterial.EnableKeyword("INK_PAINTER_HEIGHT_BLEND_COLOR_RGB_HEIGHT_A");
				return;
			default:
				InkCanvas.paintHeightMaterial.EnableKeyword("INK_PAINTER_HEIGHT_BLEND_ADD");
				return;
			}
		}

		// Token: 0x060016E0 RID: 5856 RVA: 0x00073AE4 File Offset: 0x00071CE4
		private Brush GetEraser(Brush brush, InkCanvas.PaintSet paintSet, Vector2 uv, bool useMainPaint, bool useNormalPaint, bool useHeightpaint)
		{
			Brush brush2 = brush.Clone() as Brush;
			brush2.Color = Color.white;
			brush2.ColorBlending = Brush.ColorBlendType.UseBrush;
			brush2.NormalBlending = Brush.NormalBlendType.UseBrush;
			brush2.HeightBlending = Brush.HeightBlendType.UseBrush;
			brush2.NormalBlend = 1f;
			brush2.HeightBlend = 1f;
			if (useMainPaint)
			{
				RenderTexture temporary = RenderTexture.GetTemporary(brush.BrushTexture.width, brush.BrushTexture.height);
				GrabArea.Clip(brush.BrushTexture, brush.Scale, paintSet.mainTexture, uv, brush.RotateAngle, GrabArea.GrabTextureWrapMode.Clamp, temporary, true);
				brush2.BrushTexture = temporary;
			}
			if (useNormalPaint)
			{
				RenderTexture temporary2 = RenderTexture.GetTemporary(brush.BrushNormalTexture.width, brush.BrushNormalTexture.height);
				GrabArea.Clip(brush.BrushNormalTexture, brush.Scale, paintSet.normalTexture, uv, brush.RotateAngle, GrabArea.GrabTextureWrapMode.Clamp, temporary2, false);
				brush2.BrushNormalTexture = temporary2;
			}
			if (useHeightpaint)
			{
				RenderTexture temporary3 = RenderTexture.GetTemporary(brush.BrushHeightTexture.width, brush.BrushHeightTexture.height);
				GrabArea.Clip(brush.BrushHeightTexture, brush.Scale, paintSet.heightTexture, uv, brush.RotateAngle, GrabArea.GrabTextureWrapMode.Clamp, temporary3, false);
				brush2.BrushHeightTexture = temporary3;
			}
			if (this.eraserDebug)
			{
				if (this.debugEraserMainView == null && useMainPaint)
				{
					this.debugEraserMainView = new RenderTexture(brush2.BrushTexture.width, brush2.BrushTexture.height, 0);
				}
				if (this.debugEraserNormalView == null && useNormalPaint)
				{
					this.debugEraserNormalView = new RenderTexture(brush2.BrushNormalTexture.width, brush2.BrushNormalTexture.height, 0);
				}
				if (this.debugEraserHeightView == null && useHeightpaint)
				{
					this.debugEraserHeightView = new RenderTexture(brush2.BrushHeightTexture.width, brush2.BrushHeightTexture.height, 0);
				}
				if (useMainPaint)
				{
					Graphics.Blit(brush2.BrushTexture, this.debugEraserMainView);
				}
				if (useNormalPaint)
				{
					Graphics.Blit(brush2.BrushNormalTexture, this.debugEraserNormalView);
				}
				if (useHeightpaint)
				{
					Graphics.Blit(brush2.BrushHeightTexture, this.debugEraserHeightView);
				}
			}
			return brush2;
		}

		// Token: 0x060016E1 RID: 5857 RVA: 0x00073CF0 File Offset: 0x00071EF0
		private void ReleaseEraser(Brush brush, bool useMainPaint, bool useNormalPaint, bool useHeightpaint)
		{
			if (useMainPaint && brush.BrushTexture is RenderTexture)
			{
				RenderTexture.ReleaseTemporary(brush.BrushTexture as RenderTexture);
			}
			if (useNormalPaint && brush.BrushNormalTexture is RenderTexture)
			{
				RenderTexture.ReleaseTemporary(brush.BrushNormalTexture as RenderTexture);
			}
			if (useHeightpaint && brush.BrushHeightTexture is RenderTexture)
			{
				RenderTexture.ReleaseTemporary(brush.BrushHeightTexture as RenderTexture);
			}
		}

		// Token: 0x060016E2 RID: 5858 RVA: 0x00073D60 File Offset: 0x00071F60
		public bool PaintUVDirect(Brush brush, Vector2 uv, Func<InkCanvas.PaintSet, bool> materialSelector = null)
		{
			if (brush == null)
			{
				Debug.LogError("Do not set the brush.");
				this.eraseFlag = false;
				return false;
			}
			if (this.OnPaintStart != null)
			{
				brush = (brush.Clone() as Brush);
				this.OnPaintStart(this, brush);
			}
			IEnumerable<InkCanvas.PaintSet> enumerable;
			if (materialSelector != null)
			{
				enumerable = this.paintSet.Where(materialSelector);
			}
			else
			{
				IEnumerable<InkCanvas.PaintSet> enumerable2 = this.paintSet;
				enumerable = enumerable2;
			}
			foreach (InkCanvas.PaintSet paintSet in enumerable)
			{
				bool flag = paintSet.useMainPaint && brush.BrushTexture != null && paintSet.paintMainTexture != null && paintSet.paintMainTexture.IsCreated();
				bool flag2 = paintSet.useNormalPaint && brush.BrushNormalTexture != null && paintSet.paintNormalTexture != null && paintSet.paintNormalTexture.IsCreated();
				bool flag3 = paintSet.useHeightPaint && brush.BrushHeightTexture != null && paintSet.paintHeightTexture != null && paintSet.paintHeightTexture.IsCreated();
				if (this.eraseFlag)
				{
					brush = this.GetEraser(brush, paintSet, uv, flag, flag2, flag3);
				}
				if (flag)
				{
					RenderTexture temporary = RenderTexture.GetTemporary(paintSet.paintMainTexture.width, paintSet.paintMainTexture.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
					this.SetPaintMainData(brush, uv);
					Graphics.Blit(paintSet.paintMainTexture, temporary, InkCanvas.paintMainMaterial);
					Graphics.Blit(temporary, paintSet.paintMainTexture);
					RenderTexture.ReleaseTemporary(temporary);
				}
				if (flag2)
				{
					RenderTexture temporary2 = RenderTexture.GetTemporary(paintSet.paintNormalTexture.width, paintSet.paintNormalTexture.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
					this.SetPaintNormalData(brush, uv, this.eraseFlag);
					Graphics.Blit(paintSet.paintNormalTexture, temporary2, InkCanvas.paintNormalMaterial);
					Graphics.Blit(temporary2, paintSet.paintNormalTexture);
					RenderTexture.ReleaseTemporary(temporary2);
				}
				if (flag3)
				{
					RenderTexture temporary3 = RenderTexture.GetTemporary(paintSet.paintHeightTexture.width, paintSet.paintHeightTexture.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
					this.SetPaintHeightData(brush, uv);
					Graphics.Blit(paintSet.paintHeightTexture, temporary3, InkCanvas.paintHeightMaterial);
					Graphics.Blit(temporary3, paintSet.paintHeightTexture);
					RenderTexture.ReleaseTemporary(temporary3);
				}
				if (this.eraseFlag)
				{
					this.ReleaseEraser(brush, flag, flag2, flag3);
				}
			}
			if (this.OnPaintEnd != null)
			{
				this.OnPaintEnd(this);
			}
			this.eraseFlag = false;
			return true;
		}

		// Token: 0x060016E3 RID: 5859 RVA: 0x00073FE4 File Offset: 0x000721E4
		public bool PaintNearestTriangleSurface(Brush brush, Vector3 worldPos, Func<InkCanvas.PaintSet, bool> materialSelector = null, Camera renderCamera = null)
		{
			Vector3 localPoint = base.transform.worldToLocalMatrix.MultiplyPoint(worldPos);
			Vector3 point = this.MeshOperator.NearestLocalSurfacePoint(localPoint);
			return this.Paint(brush, base.transform.localToWorldMatrix.MultiplyPoint(point), materialSelector, renderCamera);
		}

		// Token: 0x060016E4 RID: 5860 RVA: 0x00074034 File Offset: 0x00072234
		public bool Paint(Brush brush, Vector3 worldPos, Func<InkCanvas.PaintSet, bool> materialSelector = null, Camera renderCamera = null)
		{
			if (renderCamera == null)
			{
				renderCamera = Camera.main;
			}
			Vector3 localPoint = base.transform.InverseTransformPoint(worldPos);
			Matrix4x4 matrixMVP = renderCamera.projectionMatrix * renderCamera.worldToCameraMatrix * base.transform.localToWorldMatrix;
			Vector2 uv;
			if (this.MeshOperator.LocalPointToUV(localPoint, matrixMVP, out uv))
			{
				return this.PaintUVDirect(brush, uv, materialSelector);
			}
			Debug.LogWarning("Could not get the point on the surface.");
			return this.PaintNearestTriangleSurface(brush, worldPos, materialSelector, renderCamera);
		}

		// Token: 0x060016E5 RID: 5861 RVA: 0x000740B4 File Offset: 0x000722B4
		public bool Paint(Brush brush, RaycastHit hitInfo, Func<InkCanvas.PaintSet, bool> materialSelector = null)
		{
			if (!(hitInfo.collider != null))
			{
				return false;
			}
			if (hitInfo.collider is MeshCollider)
			{
				return this.PaintUVDirect(brush, hitInfo.textureCoord, materialSelector);
			}
			Debug.LogWarning("If you want to paint using a RaycastHit, need set MeshCollider for object.");
			return this.PaintNearestTriangleSurface(brush, hitInfo.point, materialSelector, null);
		}

		// Token: 0x060016E6 RID: 5862 RVA: 0x0007410A File Offset: 0x0007230A
		public bool EraseUVDirect(Brush brush, Vector2 uv, Func<InkCanvas.PaintSet, bool> materialSelector = null)
		{
			this.eraseFlag = true;
			return this.PaintUVDirect(brush, uv, materialSelector);
		}

		// Token: 0x060016E7 RID: 5863 RVA: 0x0007411C File Offset: 0x0007231C
		public bool EraseNearestTriangleSurface(Brush brush, Vector3 worldPos, Func<InkCanvas.PaintSet, bool> materialSelector = null, Camera renderCamera = null)
		{
			this.eraseFlag = true;
			return this.PaintNearestTriangleSurface(brush, worldPos, materialSelector, renderCamera);
		}

		// Token: 0x060016E8 RID: 5864 RVA: 0x00074130 File Offset: 0x00072330
		public bool Erase(Brush brush, Vector3 worldPos, Func<InkCanvas.PaintSet, bool> materialSelector = null, Camera renderCamera = null)
		{
			this.eraseFlag = true;
			return this.Paint(brush, worldPos, materialSelector, renderCamera);
		}

		// Token: 0x060016E9 RID: 5865 RVA: 0x00074144 File Offset: 0x00072344
		public bool Erase(Brush brush, RaycastHit hitInfo, Func<InkCanvas.PaintSet, bool> materialSelector = null)
		{
			this.eraseFlag = true;
			return this.Paint(brush, hitInfo, materialSelector);
		}

		// Token: 0x060016EA RID: 5866 RVA: 0x00074156 File Offset: 0x00072356
		public void ResetPaint()
		{
			this.ReleaseRenderTexture();
			this.SetRenderTexture();
			if (this.OnInitializedAfter != null)
			{
				this.OnInitializedAfter(this);
			}
		}

		// Token: 0x060016EB RID: 5867 RVA: 0x00074178 File Offset: 0x00072378
		public Texture GetMainTexture(string materialName)
		{
			materialName = materialName.Replace(" (Instance)", "");
			InkCanvas.PaintSet paintSet = this.paintSet.FirstOrDefault((InkCanvas.PaintSet p) => p.material.name.Replace(" (Instance)", "") == materialName);
			if (paintSet == null)
			{
				return null;
			}
			return paintSet.mainTexture;
		}

		// Token: 0x060016EC RID: 5868 RVA: 0x000741D0 File Offset: 0x000723D0
		public RenderTexture GetPaintMainTexture(string materialName)
		{
			materialName = materialName.Replace(" (Instance)", "");
			InkCanvas.PaintSet paintSet = this.paintSet.FirstOrDefault((InkCanvas.PaintSet p) => p.material.name.Replace(" (Instance)", "") == materialName);
			if (paintSet == null)
			{
				return null;
			}
			return paintSet.paintMainTexture;
		}

		// Token: 0x060016ED RID: 5869 RVA: 0x00074228 File Offset: 0x00072428
		public void SetPaintMainTexture(string materialName, RenderTexture newTexture)
		{
			materialName = materialName.Replace(" (Instance)", "");
			InkCanvas.PaintSet paintSet = this.paintSet.FirstOrDefault((InkCanvas.PaintSet p) => p.material.name.Replace(" (Instance)", "") == materialName);
			if (paintSet == null)
			{
				Debug.LogError("Failed to set texture.");
				return;
			}
			paintSet.paintMainTexture = newTexture;
			paintSet.material.SetTexture(paintSet.mainTextureName, paintSet.paintMainTexture);
			paintSet.useMainPaint = true;
		}

		// Token: 0x060016EE RID: 5870 RVA: 0x000742A8 File Offset: 0x000724A8
		public Texture GetNormalTexture(string materialName)
		{
			materialName = materialName.Replace(" (Instance)", "");
			InkCanvas.PaintSet paintSet = this.paintSet.FirstOrDefault((InkCanvas.PaintSet p) => p.material.name.Replace(" (Instance)", "") == materialName);
			if (paintSet == null)
			{
				return null;
			}
			return paintSet.normalTexture;
		}

		// Token: 0x060016EF RID: 5871 RVA: 0x00074300 File Offset: 0x00072500
		public RenderTexture GetPaintNormalTexture(string materialName)
		{
			materialName = materialName.Replace(" (Instance)", "");
			InkCanvas.PaintSet paintSet = this.paintSet.FirstOrDefault((InkCanvas.PaintSet p) => p.material.name.Replace(" (Instance)", "") == materialName);
			if (paintSet == null)
			{
				return null;
			}
			return paintSet.paintNormalTexture;
		}

		// Token: 0x060016F0 RID: 5872 RVA: 0x00074358 File Offset: 0x00072558
		public void SetPaintNormalTexture(string materialName, RenderTexture newTexture)
		{
			materialName = materialName.Replace(" (Instance)", "");
			InkCanvas.PaintSet paintSet = this.paintSet.FirstOrDefault((InkCanvas.PaintSet p) => p.material.name.Replace(" (Instance)", "") == materialName);
			if (paintSet == null)
			{
				Debug.LogError("Failed to set texture.");
				return;
			}
			paintSet.paintNormalTexture = newTexture;
			paintSet.material.SetTexture(paintSet.normalTextureName, paintSet.paintNormalTexture);
			paintSet.useNormalPaint = true;
		}

		// Token: 0x060016F1 RID: 5873 RVA: 0x000743D8 File Offset: 0x000725D8
		public Texture GetHeightTexture(string materialName)
		{
			materialName = materialName.Replace(" (Instance)", "");
			InkCanvas.PaintSet paintSet = this.paintSet.FirstOrDefault((InkCanvas.PaintSet p) => p.material.name.Replace(" (Instance)", "") == materialName);
			if (paintSet == null)
			{
				return null;
			}
			return paintSet.heightTexture;
		}

		// Token: 0x060016F2 RID: 5874 RVA: 0x00074430 File Offset: 0x00072630
		public RenderTexture GetPaintHeightTexture(string materialName)
		{
			materialName = materialName.Replace(" (Instance)", "");
			InkCanvas.PaintSet paintSet = this.paintSet.FirstOrDefault((InkCanvas.PaintSet p) => p.material.name.Replace(" (Instance)", "") == materialName);
			if (paintSet == null)
			{
				return null;
			}
			return paintSet.paintHeightTexture;
		}

		// Token: 0x060016F3 RID: 5875 RVA: 0x00074488 File Offset: 0x00072688
		public void SetPaintHeightTexture(string materialName, RenderTexture newTexture)
		{
			materialName = materialName.Replace(" (Instance)", "");
			InkCanvas.PaintSet paintSet = this.paintSet.FirstOrDefault((InkCanvas.PaintSet p) => p.material.name.Replace(" (Instance)", "") == materialName);
			if (paintSet == null)
			{
				Debug.LogError("Failed to set texture.");
				return;
			}
			paintSet.paintHeightTexture = newTexture;
			paintSet.material.SetTexture(paintSet.heightTextureName, paintSet.paintHeightTexture);
			paintSet.useHeightPaint = true;
		}

		// Token: 0x0400109F RID: 4255
		private static Material paintMainMaterial;

		// Token: 0x040010A0 RID: 4256
		private static Material paintNormalMaterial;

		// Token: 0x040010A1 RID: 4257
		private static Material paintHeightMaterial;

		// Token: 0x040010A2 RID: 4258
		private bool eraseFlag;

		// Token: 0x040010A3 RID: 4259
		private RenderTexture debugEraserMainView;

		// Token: 0x040010A4 RID: 4260
		private RenderTexture debugEraserNormalView;

		// Token: 0x040010A5 RID: 4261
		private RenderTexture debugEraserHeightView;

		// Token: 0x040010A6 RID: 4262
		private bool eraserDebug;

		// Token: 0x040010AC RID: 4268
		[SerializeField]
		private List<InkCanvas.PaintSet> paintSet;

		// Token: 0x040010AD RID: 4269
		private int paintUVPropertyID;

		// Token: 0x040010AE RID: 4270
		private int brushTexturePropertyID;

		// Token: 0x040010AF RID: 4271
		private int brushScalePropertyID;

		// Token: 0x040010B0 RID: 4272
		private int brushRotatePropertyID;

		// Token: 0x040010B1 RID: 4273
		private int brushColorPropertyID;

		// Token: 0x040010B2 RID: 4274
		private int brushNormalTexturePropertyID;

		// Token: 0x040010B3 RID: 4275
		private int brushNormalBlendPropertyID;

		// Token: 0x040010B4 RID: 4276
		private int brushHeightTexturePropertyID;

		// Token: 0x040010B5 RID: 4277
		private int brushHeightBlendPropertyID;

		// Token: 0x040010B6 RID: 4278
		private int brushHeightColorPropertyID;

		// Token: 0x040010B7 RID: 4279
		private const string COLOR_BLEND_USE_CONTROL = "INK_PAINTER_COLOR_BLEND_USE_CONTROL";

		// Token: 0x040010B8 RID: 4280
		private const string COLOR_BLEND_USE_BRUSH = "INK_PAINTER_COLOR_BLEND_USE_BRUSH";

		// Token: 0x040010B9 RID: 4281
		private const string COLOR_BLEND_NEUTRAL = "INK_PAINTER_COLOR_BLEND_NEUTRAL";

		// Token: 0x040010BA RID: 4282
		private const string COLOR_BLEND_ALPHA_ONLY = "INK_PAINTER_COLOR_BLEND_ALPHA_ONLY";

		// Token: 0x040010BB RID: 4283
		private const string NORMAL_BLEND_USE_BRUSH = "INK_PAINTER_NORMAL_BLEND_USE_BRUSH";

		// Token: 0x040010BC RID: 4284
		private const string NORMAL_BLEND_ADD = "INK_PAINTER_NORMAL_BLEND_ADD";

		// Token: 0x040010BD RID: 4285
		private const string NORMAL_BLEND_SUB = "INK_PAINTER_NORMAL_BLEND_SUB";

		// Token: 0x040010BE RID: 4286
		private const string NORMAL_BLEND_MIN = "INK_PAINTER_NORMAL_BLEND_MIN";

		// Token: 0x040010BF RID: 4287
		private const string NORMAL_BLEND_MAX = "INK_PAINTER_NORMAL_BLEND_MAX";

		// Token: 0x040010C0 RID: 4288
		private const string DXT5NM_COMPRESS_USE = "DXT5NM_COMPRESS_USE";

		// Token: 0x040010C1 RID: 4289
		private const string DXT5NM_COMPRESS_UNUSE = "DXT5NM_COMPRESS_UNUSE";

		// Token: 0x040010C2 RID: 4290
		private const string HEIGHT_BLEND_USE_BRUSH = "INK_PAINTER_HEIGHT_BLEND_USE_BRUSH";

		// Token: 0x040010C3 RID: 4291
		private const string HEIGHT_BLEND_ADD = "INK_PAINTER_HEIGHT_BLEND_ADD";

		// Token: 0x040010C4 RID: 4292
		private const string HEIGHT_BLEND_SUB = "INK_PAINTER_HEIGHT_BLEND_SUB";

		// Token: 0x040010C5 RID: 4293
		private const string HEIGHT_BLEND_MIN = "INK_PAINTER_HEIGHT_BLEND_MIN";

		// Token: 0x040010C6 RID: 4294
		private const string HEIGHT_BLEND_MAX = "INK_PAINTER_HEIGHT_BLEND_MAX";

		// Token: 0x040010C7 RID: 4295
		private const string HEIGHT_BLEND_COLOR_RGB_HEIGHT_A = "INK_PAINTER_HEIGHT_BLEND_COLOR_RGB_HEIGHT_A";

		// Token: 0x040010C8 RID: 4296
		private MeshOperator meshOperator;

		// Token: 0x020002EA RID: 746
		[Serializable]
		public class PaintSet
		{
			// Token: 0x060016F5 RID: 5877 RVA: 0x00074508 File Offset: 0x00072708
			public PaintSet()
			{
			}

			// Token: 0x060016F6 RID: 5878 RVA: 0x00074538 File Offset: 0x00072738
			public PaintSet(string mainTextureName, string normalTextureName, string heightTextureName, bool useMainPaint, bool useNormalPaint, bool useHeightPaint)
			{
				this.mainTextureName = mainTextureName;
				this.normalTextureName = normalTextureName;
				this.heightTextureName = heightTextureName;
				this.useMainPaint = useMainPaint;
				this.useNormalPaint = useNormalPaint;
				this.useHeightPaint = useHeightPaint;
			}

			// Token: 0x060016F7 RID: 5879 RVA: 0x000745A0 File Offset: 0x000727A0
			public PaintSet(string mainTextureName, string normalTextureName, string heightTextureName, bool useMainPaint, bool useNormalPaint, bool useHeightPaint, Material material) : this(mainTextureName, normalTextureName, heightTextureName, useMainPaint, useNormalPaint, useHeightPaint)
			{
				this.material = material;
			}

			// Token: 0x040010C9 RID: 4297
			[HideInInspector]
			[NonSerialized]
			public Material material;

			// Token: 0x040010CA RID: 4298
			[SerializeField]
			[Tooltip("The property name of the main texture.")]
			public string mainTextureName = "_MainTex";

			// Token: 0x040010CB RID: 4299
			[SerializeField]
			[Tooltip("Normal map texture property name.")]
			public string normalTextureName = "_BumpMap";

			// Token: 0x040010CC RID: 4300
			[SerializeField]
			[Tooltip("The property name of the heightmap texture.")]
			public string heightTextureName = "_ParallaxMap";

			// Token: 0x040010CD RID: 4301
			[SerializeField]
			[Tooltip("Whether or not use main texture paint.")]
			public bool useMainPaint = true;

			// Token: 0x040010CE RID: 4302
			[SerializeField]
			[Tooltip("Whether or not use normal map paint (you need material on normal maps).")]
			public bool useNormalPaint;

			// Token: 0x040010CF RID: 4303
			[SerializeField]
			[Tooltip("Whether or not use heightmap painting (you need material on the heightmap).")]
			public bool useHeightPaint;

			// Token: 0x040010D0 RID: 4304
			[HideInInspector]
			[NonSerialized]
			public Texture mainTexture;

			// Token: 0x040010D1 RID: 4305
			[HideInInspector]
			[NonSerialized]
			public RenderTexture paintMainTexture;

			// Token: 0x040010D2 RID: 4306
			[HideInInspector]
			[NonSerialized]
			public Texture normalTexture;

			// Token: 0x040010D3 RID: 4307
			[HideInInspector]
			[NonSerialized]
			public RenderTexture paintNormalTexture;

			// Token: 0x040010D4 RID: 4308
			[HideInInspector]
			[NonSerialized]
			public Texture heightTexture;

			// Token: 0x040010D5 RID: 4309
			[HideInInspector]
			[NonSerialized]
			public RenderTexture paintHeightTexture;

			// Token: 0x040010D6 RID: 4310
			[HideInInspector]
			[NonSerialized]
			public int mainTexturePropertyID;

			// Token: 0x040010D7 RID: 4311
			[HideInInspector]
			[NonSerialized]
			public int normalTexturePropertyID;

			// Token: 0x040010D8 RID: 4312
			[HideInInspector]
			[NonSerialized]
			public int heightTexturePropertyID;
		}
	}
}
