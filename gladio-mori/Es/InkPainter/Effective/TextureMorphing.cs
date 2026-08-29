using System;
using UnityEngine;

namespace Es.InkPainter.Effective
{
	// Token: 0x020002FF RID: 767
	public static class TextureMorphing
	{
		// Token: 0x0600172E RID: 5934 RVA: 0x000759A4 File Offset: 0x00073BA4
		public static void Lerp(Texture src, RenderTexture dst, float lerpCoef)
		{
			if (TextureMorphing.morphingMaterial == null)
			{
				TextureMorphing.InitMorphingMaterial();
			}
			TextureMorphing.SetMorphingProperty(src, dst, lerpCoef);
			RenderTexture temporary = RenderTexture.GetTemporary(src.width, src.height);
			Graphics.Blit(src, temporary, TextureMorphing.morphingMaterial);
			Graphics.Blit(temporary, dst);
			RenderTexture.ReleaseTemporary(temporary);
		}

		// Token: 0x0600172F RID: 5935 RVA: 0x000759F6 File Offset: 0x00073BF6
		private static void InitMorphingMaterial()
		{
			TextureMorphing.morphingMaterial = new Material(Resources.Load<Material>("Es.InkPainter.Effective.TextureMorphing"));
		}

		// Token: 0x06001730 RID: 5936 RVA: 0x00075A0C File Offset: 0x00073C0C
		private static void SetMorphingProperty(Texture src, RenderTexture dst, float lerpCoef)
		{
			TextureMorphing.morphingMaterial.SetTexture(Shader.PropertyToID("_SrcTex"), src);
			TextureMorphing.morphingMaterial.SetTexture(Shader.PropertyToID("_DstTex"), dst);
			TextureMorphing.morphingMaterial.SetFloat(Shader.PropertyToID("_LerpCoef"), lerpCoef);
		}

		// Token: 0x0400111C RID: 4380
		private const string TEXTURE_MORPHING_MATERIAL = "Es.InkPainter.Effective.TextureMorphing";

		// Token: 0x0400111D RID: 4381
		private const string LERP_COEFFICIENT = "_LerpCoef";

		// Token: 0x0400111E RID: 4382
		private const string SRC_TEX = "_SrcTex";

		// Token: 0x0400111F RID: 4383
		private const string DST_TEX = "_DstTex";

		// Token: 0x04001120 RID: 4384
		private static Material morphingMaterial;
	}
}
