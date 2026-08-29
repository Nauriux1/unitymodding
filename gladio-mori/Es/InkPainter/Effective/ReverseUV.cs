using System;
using UnityEngine;

namespace Es.InkPainter.Effective
{
	// Token: 0x020002FE RID: 766
	public static class ReverseUV
	{
		// Token: 0x06001728 RID: 5928 RVA: 0x000758B0 File Offset: 0x00073AB0
		public static void Horizontal(Texture src, RenderTexture dst)
		{
			if (ReverseUV.reverseUVMaterial == null)
			{
				ReverseUV.InitReverseUVMaterial();
			}
			ReverseUV.SetReverseUVProperty(0f, 1f);
			ReverseUV.Blit(src, dst);
		}

		// Token: 0x06001729 RID: 5929 RVA: 0x000758DA File Offset: 0x00073ADA
		public static void Vertical(Texture src, RenderTexture dst)
		{
			if (ReverseUV.reverseUVMaterial == null)
			{
				ReverseUV.InitReverseUVMaterial();
			}
			ReverseUV.SetReverseUVProperty(1f, 0f);
			ReverseUV.Blit(src, dst);
		}

		// Token: 0x0600172A RID: 5930 RVA: 0x00075904 File Offset: 0x00073B04
		public static void HorizontalAndVertical(Texture src, RenderTexture dst)
		{
			if (ReverseUV.reverseUVMaterial == null)
			{
				ReverseUV.InitReverseUVMaterial();
			}
			ReverseUV.SetReverseUVProperty(0f, 0f);
			ReverseUV.Blit(src, dst);
		}

		// Token: 0x0600172B RID: 5931 RVA: 0x0007592E File Offset: 0x00073B2E
		private static void InitReverseUVMaterial()
		{
			ReverseUV.reverseUVMaterial = new Material(Resources.Load<Material>("Es.InkPainter.Effective.ReverseUV"));
		}

		// Token: 0x0600172C RID: 5932 RVA: 0x00075944 File Offset: 0x00073B44
		private static void SetReverseUVProperty(float x, float y)
		{
			ReverseUV.reverseUVMaterial.SetFloat("_ReverseX", x);
			ReverseUV.reverseUVMaterial.SetFloat("_ReverseY", y);
		}

		// Token: 0x0600172D RID: 5933 RVA: 0x00075968 File Offset: 0x00073B68
		private static void Blit(Texture src, RenderTexture dst)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(src.width, src.height, 0);
			Graphics.Blit(src, temporary, ReverseUV.reverseUVMaterial);
			Graphics.Blit(temporary, dst);
			RenderTexture.ReleaseTemporary(temporary);
		}

		// Token: 0x04001116 RID: 4374
		private const string REVERSE_UV_MATERIAL = "Es.InkPainter.Effective.ReverseUV";

		// Token: 0x04001117 RID: 4375
		private const string REVERSE_X = "_ReverseX";

		// Token: 0x04001118 RID: 4376
		private const string REVERSE_Y = "_ReverseY";

		// Token: 0x04001119 RID: 4377
		private const float DEFAULT = 1f;

		// Token: 0x0400111A RID: 4378
		private const float REVERSE = 0f;

		// Token: 0x0400111B RID: 4379
		private static Material reverseUVMaterial;
	}
}
