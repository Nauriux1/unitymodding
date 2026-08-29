using System;
using UnityEngine;

namespace Es.InkPainter.Effective
{
	// Token: 0x020002FC RID: 764
	[DisallowMultipleComponent]
	[RequireComponent(typeof(InkCanvas))]
	public class HeightFluid : MonoBehaviour
	{
		// Token: 0x0600171F RID: 5919 RVA: 0x000750FC File Offset: 0x000732FC
		private void Init(InkCanvas canvas)
		{
			foreach (InkCanvas.PaintSet paintSet in canvas.PaintDatas)
			{
				RenderTexture paintHeightTexture = canvas.GetPaintHeightTexture(paintSet.material.name);
				if (paintHeightTexture != null)
				{
					this.SingleColorFill(paintHeightTexture, Vector4.zero);
				}
				canvas.OnPaintStart += delegate(InkCanvas own, Brush brush)
				{
					if (this.lastPaintedColor != brush.Color)
					{
						this.lastPaintedColor = brush.Color;
						this.StopFluid();
					}
				};
			}
		}

		// Token: 0x06001720 RID: 5920 RVA: 0x00075188 File Offset: 0x00073388
		private void SingleColorFill(RenderTexture texture, Color color)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(texture.width, texture.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
			this.singleColorFill.SetVector("_Color", color);
			Graphics.Blit(texture, temporary, this.singleColorFill);
			Graphics.Blit(temporary, texture);
			RenderTexture.ReleaseTemporary(temporary);
		}

		// Token: 0x06001721 RID: 5921 RVA: 0x000751DC File Offset: 0x000733DC
		private void InvertAlpha(RenderTexture texture)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(texture.width, texture.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
			Graphics.Blit(texture, temporary, this.invertAlpha);
			Graphics.Blit(temporary, texture);
			RenderTexture.ReleaseTemporary(temporary);
		}

		// Token: 0x06001722 RID: 5922 RVA: 0x00075218 File Offset: 0x00073418
		private void EnabledFluid(InkCanvas canvas, Brush brush)
		{
			this.enabledFluid = true;
			this.lastPaintedTime = Time.time;
			brush.ColorBlending = Brush.ColorBlendType.AlphaOnly;
			brush.NormalBlending = Brush.NormalBlendType.UseBrush;
			brush.HeightBlending = Brush.HeightBlendType.ColorRGB_HeightA;
		}

		// Token: 0x06001723 RID: 5923 RVA: 0x00075244 File Offset: 0x00073444
		private void StopFluid()
		{
			foreach (InkCanvas.PaintSet paintSet in this.canvas.PaintDatas)
			{
				string name = paintSet.material.name;
				RenderTexture paintHeightTexture = this.canvas.GetPaintHeightTexture(name);
				if (paintHeightTexture != null)
				{
					this.InvertAlpha(paintHeightTexture);
				}
			}
		}

		// Token: 0x06001724 RID: 5924 RVA: 0x000752BC File Offset: 0x000734BC
		private void Awake()
		{
			this.heightFluid = new Material(Resources.Load<Material>("Es.InkPainter.Fluid.HeightDrip"));
			this.height2Normal = new Material(Resources.Load<Material>("Es.InkPainter.Fluid.HeightToNormal"));
			this.height2Color = new Material(Resources.Load<Material>("Es.InkPainter.Fluid.HeightToColor"));
			this.singleColorFill = new Material(Resources.Load<Material>("Es.InkPainter.Fluid.SingleColorFill"));
			this.invertAlpha = new Material(Resources.Load<Material>("Es.InkPainter.Fluid.InvertAlpha"));
			this.canvas = base.GetComponent<InkCanvas>();
			this.canvas.OnInitializedAfter += this.Init;
			this.canvas.OnPaintStart += this.EnabledFluid;
		}

		// Token: 0x06001725 RID: 5925 RVA: 0x0007536C File Offset: 0x0007356C
		private void OnWillRenderObject()
		{
			if (this.performanceOptimize && this.enabledFluid && Time.time - this.lastPaintedTime > this.fluidProcessStopTime)
			{
				this.StopFluid();
				this.enabledFluid = false;
			}
			if (!this.enabledFluid)
			{
				return;
			}
			foreach (InkCanvas.PaintSet paintSet in this.canvas.PaintDatas)
			{
				string name = paintSet.material.name;
				RenderTexture renderTexture = this.canvas.GetPaintHeightTexture(name);
				if (renderTexture == null)
				{
					RenderTexture renderTexture2 = new RenderTexture(this.createTextureSize, this.createTextureSize, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
					this.SingleColorFill(renderTexture2, Vector4.zero);
					this.canvas.SetPaintHeightTexture(name, renderTexture2);
					renderTexture = renderTexture2;
					paintSet.material.SetFloat("_Parallax", 0f);
				}
				RenderTexture temporary = RenderTexture.GetTemporary(renderTexture.width, renderTexture.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
				this.heightFluid.SetFloat("_ScaleFactor", this.flowingForce);
				this.heightFluid.SetFloat("_Viscosity", this.easeOfDripping);
				this.heightFluid.SetFloat("_HorizontalSpread", this.horizontalSpread);
				this.heightFluid.SetFloat("_InfluenceOfNormal", this.influenceOfNormal);
				this.heightFluid.SetVector("_FlowDirection", this.flowDirection.normalized);
				this.heightFluid.SetVector("_FixedColor", this.lastPaintedColor);
				foreach (string keyword in this.heightFluid.shaderKeywords)
				{
					this.heightFluid.DisableKeyword(keyword);
				}
				HeightFluid.ColorSynthesis colorSynthesis = this.colorSynthesis;
				if (colorSynthesis != HeightFluid.ColorSynthesis.Add)
				{
					if (colorSynthesis != HeightFluid.ColorSynthesis.Overwrite)
					{
					}
					this.heightFluid.EnableKeyword("COLOR_SYNTHESIS_OVERWRITE");
				}
				else
				{
					this.heightFluid.EnableKeyword("COLOR_SYNTHESIS_ADD");
				}
				if (this.canvas.GetNormalTexture(name) != null)
				{
					this.heightFluid.SetTexture("_NormalMap", this.canvas.GetNormalTexture(name));
				}
				Graphics.Blit(renderTexture, temporary, this.heightFluid);
				Graphics.Blit(temporary, renderTexture);
				RenderTexture.ReleaseTemporary(temporary);
				if (this.useMainTextureFluid)
				{
					RenderTexture renderTexture3 = this.canvas.GetPaintMainTexture(name);
					if (renderTexture3 == null)
					{
						RenderTexture renderTexture4 = new RenderTexture(this.createTextureSize, this.createTextureSize, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
						if (this.canvas.GetMainTexture(name) != null)
						{
							Graphics.Blit(this.canvas.GetMainTexture(name), renderTexture4);
						}
						this.canvas.SetPaintMainTexture(name, renderTexture4);
						renderTexture3 = renderTexture4;
					}
					RenderTexture temporary2 = RenderTexture.GetTemporary(renderTexture3.width, renderTexture3.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
					this.height2Color.SetTexture("_ColorMap", renderTexture3);
					this.height2Color.SetTexture("_BaseColor", this.canvas.GetMainTexture(name));
					this.height2Color.SetFloat("_Alpha", this.alpha);
					this.height2Color.SetFloat("_Border", this.AdhesionBorder);
					Graphics.Blit(renderTexture, temporary2, this.height2Color);
					Graphics.Blit(temporary2, renderTexture3);
					RenderTexture.ReleaseTemporary(temporary2);
				}
				if (this.useNormalMapFluid)
				{
					RenderTexture renderTexture5 = this.canvas.GetPaintNormalTexture(name);
					if (renderTexture5 == null)
					{
						RenderTexture renderTexture6 = new RenderTexture(this.createTextureSize, this.createTextureSize, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
						this.SingleColorFill(renderTexture6, Vector4.one * 0.5f);
						paintSet.material.EnableKeyword("_NORMALMAP");
						if (this.canvas.GetNormalTexture(name) != null)
						{
							Graphics.Blit(this.canvas.GetNormalTexture(name), renderTexture6);
						}
						this.canvas.SetPaintNormalTexture(name, renderTexture6);
						renderTexture5 = renderTexture6;
					}
					RenderTexture temporary3 = RenderTexture.GetTemporary(renderTexture5.width, renderTexture5.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
					this.height2Normal.SetTexture("_BumpMap", renderTexture5);
					this.height2Normal.SetFloat("_NormalScaleFactor", this.normalScaleFactor);
					this.height2Normal.SetFloat("_Border", this.AdhesionBorder);
					Graphics.Blit(renderTexture, temporary3, this.height2Normal);
					Graphics.Blit(temporary3, renderTexture5);
					RenderTexture.ReleaseTemporary(temporary3);
				}
			}
		}

		// Token: 0x040010FA RID: 4346
		[SerializeField]
		private bool useMainTextureFluid = true;

		// Token: 0x040010FB RID: 4347
		[SerializeField]
		private bool useNormalMapFluid = true;

		// Token: 0x040010FC RID: 4348
		[SerializeField]
		private int createTextureSize = 1024;

		// Token: 0x040010FD RID: 4349
		[SerializeField]
		private HeightFluid.ColorSynthesis colorSynthesis = HeightFluid.ColorSynthesis.Overwrite;

		// Token: 0x040010FE RID: 4350
		[SerializeField]
		[Range(0f, 1f)]
		private float alpha = 1f;

		// Token: 0x040010FF RID: 4351
		[SerializeField]
		private Vector2 flowDirection = Vector2.up;

		// Token: 0x04001100 RID: 4352
		[SerializeField]
		[Range(0f, 1f)]
		private float flowingForce = 1f;

		// Token: 0x04001101 RID: 4353
		[SerializeField]
		[Range(0.1f, 10f)]
		private float easeOfDripping = 1f;

		// Token: 0x04001102 RID: 4354
		[SerializeField]
		[Range(1f, 0f)]
		private float influenceOfNormal = 1f;

		// Token: 0x04001103 RID: 4355
		[SerializeField]
		[Range(0.01f, 1f)]
		private float horizontalSpread = 0.01f;

		// Token: 0x04001104 RID: 4356
		[SerializeField]
		private float normalScaleFactor = 1f;

		// Token: 0x04001105 RID: 4357
		[SerializeField]
		[Range(0.001f, 0.999f)]
		private float AdhesionBorder = 0.01f;

		// Token: 0x04001106 RID: 4358
		[SerializeField]
		private bool performanceOptimize = true;

		// Token: 0x04001107 RID: 4359
		[SerializeField]
		[Range(0.01f, 10f)]
		private float fluidProcessStopTime = 5f;

		// Token: 0x04001108 RID: 4360
		private bool enabledFluid;

		// Token: 0x04001109 RID: 4361
		private float lastPaintedTime;

		// Token: 0x0400110A RID: 4362
		private Material heightFluid;

		// Token: 0x0400110B RID: 4363
		private Material height2Normal;

		// Token: 0x0400110C RID: 4364
		private Material height2Color;

		// Token: 0x0400110D RID: 4365
		private Material singleColorFill;

		// Token: 0x0400110E RID: 4366
		private Material invertAlpha;

		// Token: 0x0400110F RID: 4367
		private InkCanvas canvas;

		// Token: 0x04001110 RID: 4368
		private Color lastPaintedColor;

		// Token: 0x04001111 RID: 4369
		private const string COLOR_SYNTHESIS_ADD = "COLOR_SYNTHESIS_ADD";

		// Token: 0x04001112 RID: 4370
		private const string COLOR_SYNTHESIS_OVERWRITE = "COLOR_SYNTHESIS_OVERWRITE";

		// Token: 0x020002FD RID: 765
		private enum ColorSynthesis
		{
			// Token: 0x04001114 RID: 4372
			Add,
			// Token: 0x04001115 RID: 4373
			Overwrite
		}
	}
}
