using System;
using UnityEngine;

namespace Es.InkPainter
{
	// Token: 0x020002E5 RID: 741
	[Serializable]
	public class Brush : ICloneable
	{
		// Token: 0x1700028C RID: 652
		// (get) Token: 0x060016A9 RID: 5801 RVA: 0x00072C96 File Offset: 0x00070E96
		// (set) Token: 0x060016AA RID: 5802 RVA: 0x00072C9E File Offset: 0x00070E9E
		public Texture BrushTexture
		{
			get
			{
				return this.brushTexture;
			}
			set
			{
				this.brushTexture = value;
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x060016AB RID: 5803 RVA: 0x00072CA7 File Offset: 0x00070EA7
		// (set) Token: 0x060016AC RID: 5804 RVA: 0x00072CAF File Offset: 0x00070EAF
		public Texture BrushNormalTexture
		{
			get
			{
				return this.brushNormalTexture;
			}
			set
			{
				this.brushNormalTexture = value;
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x060016AD RID: 5805 RVA: 0x00072CB8 File Offset: 0x00070EB8
		// (set) Token: 0x060016AE RID: 5806 RVA: 0x00072CC0 File Offset: 0x00070EC0
		public Texture BrushHeightTexture
		{
			get
			{
				return this.brushHeightTexture;
			}
			set
			{
				this.brushHeightTexture = value;
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x060016AF RID: 5807 RVA: 0x00072CC9 File Offset: 0x00070EC9
		// (set) Token: 0x060016B0 RID: 5808 RVA: 0x00072CD6 File Offset: 0x00070ED6
		public float Scale
		{
			get
			{
				return Mathf.Clamp01(this.brushScale);
			}
			set
			{
				this.brushScale = Mathf.Clamp01(value);
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x060016B1 RID: 5809 RVA: 0x00072CE4 File Offset: 0x00070EE4
		// (set) Token: 0x060016B2 RID: 5810 RVA: 0x00072CEC File Offset: 0x00070EEC
		public float RotateAngle
		{
			get
			{
				return this.rotateAngle;
			}
			set
			{
				this.rotateAngle = value;
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x060016B3 RID: 5811 RVA: 0x00072CF5 File Offset: 0x00070EF5
		// (set) Token: 0x060016B4 RID: 5812 RVA: 0x00072D02 File Offset: 0x00070F02
		public float NormalBlend
		{
			get
			{
				return Mathf.Clamp01(this.brushNormalBlend);
			}
			set
			{
				this.brushNormalBlend = Mathf.Clamp01(value);
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x060016B5 RID: 5813 RVA: 0x00072D10 File Offset: 0x00070F10
		// (set) Token: 0x060016B6 RID: 5814 RVA: 0x00072D1D File Offset: 0x00070F1D
		public float HeightBlend
		{
			get
			{
				return Mathf.Clamp01(this.brushHeightBlend);
			}
			set
			{
				this.brushHeightBlend = Mathf.Clamp01(value);
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x060016B7 RID: 5815 RVA: 0x00072D2B File Offset: 0x00070F2B
		// (set) Token: 0x060016B8 RID: 5816 RVA: 0x00072D33 File Offset: 0x00070F33
		public Color Color
		{
			get
			{
				return this.brushColor;
			}
			set
			{
				this.brushColor = value;
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x060016B9 RID: 5817 RVA: 0x00072D3C File Offset: 0x00070F3C
		// (set) Token: 0x060016BA RID: 5818 RVA: 0x00072D44 File Offset: 0x00070F44
		public Brush.ColorBlendType ColorBlending
		{
			get
			{
				return this.colorBlendType;
			}
			set
			{
				this.colorBlendType = value;
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x060016BB RID: 5819 RVA: 0x00072D4D File Offset: 0x00070F4D
		// (set) Token: 0x060016BC RID: 5820 RVA: 0x00072D55 File Offset: 0x00070F55
		public Brush.NormalBlendType NormalBlending
		{
			get
			{
				return this.normalBlendType;
			}
			set
			{
				this.normalBlendType = value;
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x060016BD RID: 5821 RVA: 0x00072D5E File Offset: 0x00070F5E
		// (set) Token: 0x060016BE RID: 5822 RVA: 0x00072D66 File Offset: 0x00070F66
		public Brush.HeightBlendType HeightBlending
		{
			get
			{
				return this.heightBlendType;
			}
			set
			{
				this.heightBlendType = value;
			}
		}

		// Token: 0x060016BF RID: 5823 RVA: 0x00072D6F File Offset: 0x00070F6F
		public Brush(Texture brushTex, float scale, Color color)
		{
			this.BrushTexture = brushTex;
			this.Scale = scale;
			this.Color = color;
		}

		// Token: 0x060016C0 RID: 5824 RVA: 0x00072DAD File Offset: 0x00070FAD
		public Brush(Texture brushTex, float scale, Color color, Brush.ColorBlendType colorBlending) : this(brushTex, scale, color)
		{
			this.ColorBlending = colorBlending;
		}

		// Token: 0x060016C1 RID: 5825 RVA: 0x00072DC0 File Offset: 0x00070FC0
		public Brush(Texture brushTex, float scale, Color color, Texture normalTex, float normalBlend) : this(brushTex, scale, color)
		{
			this.BrushNormalTexture = normalTex;
			this.NormalBlend = normalBlend;
		}

		// Token: 0x060016C2 RID: 5826 RVA: 0x00072DDB File Offset: 0x00070FDB
		public Brush(Texture brushTex, float scale, Color color, Texture normalTex, float normalBlend, Brush.ColorBlendType colorBlending, Brush.NormalBlendType normalBlending) : this(brushTex, scale, color, normalTex, normalBlend)
		{
			this.ColorBlending = colorBlending;
			this.NormalBlending = normalBlending;
		}

		// Token: 0x060016C3 RID: 5827 RVA: 0x00072DFA File Offset: 0x00070FFA
		public Brush(Texture brushTex, float scale, Color color, Texture normalTex, float normalBlend, Texture heightTex, float heightBlend, Brush.ColorBlendType colorBlending, Brush.NormalBlendType normalBlending, Brush.HeightBlendType heightBlending) : this(brushTex, scale, color, normalTex, normalBlend, colorBlending, normalBlending)
		{
			this.BrushHeightTexture = heightTex;
			this.HeightBlend = heightBlend;
			this.HeightBlending = heightBlending;
		}

		// Token: 0x060016C4 RID: 5828 RVA: 0x00072E25 File Offset: 0x00071025
		public object Clone()
		{
			return base.MemberwiseClone();
		}

		// Token: 0x04001082 RID: 4226
		[SerializeField]
		private Texture brushTexture;

		// Token: 0x04001083 RID: 4227
		[SerializeField]
		private Texture brushNormalTexture;

		// Token: 0x04001084 RID: 4228
		[SerializeField]
		private Texture brushHeightTexture;

		// Token: 0x04001085 RID: 4229
		[SerializeField]
		[Range(0f, 1f)]
		private float brushScale = 0.1f;

		// Token: 0x04001086 RID: 4230
		[SerializeField]
		[Range(0f, 360f)]
		private float rotateAngle;

		// Token: 0x04001087 RID: 4231
		[SerializeField]
		[Range(0f, 1f)]
		private float brushNormalBlend = 0.1f;

		// Token: 0x04001088 RID: 4232
		[SerializeField]
		[Range(0f, 1f)]
		private float brushHeightBlend = 0.1f;

		// Token: 0x04001089 RID: 4233
		[SerializeField]
		private Color brushColor;

		// Token: 0x0400108A RID: 4234
		[SerializeField]
		private Brush.ColorBlendType colorBlendType;

		// Token: 0x0400108B RID: 4235
		[SerializeField]
		private Brush.NormalBlendType normalBlendType;

		// Token: 0x0400108C RID: 4236
		[SerializeField]
		private Brush.HeightBlendType heightBlendType;

		// Token: 0x020002E6 RID: 742
		public enum ColorBlendType
		{
			// Token: 0x0400108E RID: 4238
			UseColor,
			// Token: 0x0400108F RID: 4239
			UseBrush,
			// Token: 0x04001090 RID: 4240
			Neutral,
			// Token: 0x04001091 RID: 4241
			AlphaOnly
		}

		// Token: 0x020002E7 RID: 743
		public enum NormalBlendType
		{
			// Token: 0x04001093 RID: 4243
			UseBrush,
			// Token: 0x04001094 RID: 4244
			Add,
			// Token: 0x04001095 RID: 4245
			Sub,
			// Token: 0x04001096 RID: 4246
			Min,
			// Token: 0x04001097 RID: 4247
			Max
		}

		// Token: 0x020002E8 RID: 744
		public enum HeightBlendType
		{
			// Token: 0x04001099 RID: 4249
			UseBrush,
			// Token: 0x0400109A RID: 4250
			Add,
			// Token: 0x0400109B RID: 4251
			Sub,
			// Token: 0x0400109C RID: 4252
			Min,
			// Token: 0x0400109D RID: 4253
			Max,
			// Token: 0x0400109E RID: 4254
			ColorRGB_HeightA
		}
	}
}
