using System;
using UnityEngine;

namespace Es.InkPainter.Effective
{
	// Token: 0x020002FA RID: 762
	public static class GrabArea
	{
		// Token: 0x0600171C RID: 5916 RVA: 0x00074F94 File Offset: 0x00073194
		public static void Clip(Texture clipTexture, float clipScale, Texture grabTargetTexture, Vector2 targetUV, float rotateAngle, GrabArea.GrabTextureWrapMode wrapMode, RenderTexture dst, bool replaceAlpha = true)
		{
			if (GrabArea.grabAreaMaterial == null)
			{
				GrabArea.InitGrabAreaMaterial();
			}
			GrabArea.SetGrabAreaProperty(clipTexture, clipScale, grabTargetTexture, targetUV, rotateAngle, wrapMode, replaceAlpha);
			RenderTexture temporary = RenderTexture.GetTemporary(clipTexture.width, clipTexture.height, 0);
			Graphics.Blit(clipTexture, temporary, GrabArea.grabAreaMaterial);
			Graphics.Blit(temporary, dst);
			RenderTexture.ReleaseTemporary(temporary);
		}

		// Token: 0x0600171D RID: 5917 RVA: 0x00074FEF File Offset: 0x000731EF
		private static void InitGrabAreaMaterial()
		{
			GrabArea.grabAreaMaterial = new Material(Resources.Load<Material>("Es.InkPainter.Effective.GrabArea"));
		}

		// Token: 0x0600171E RID: 5918 RVA: 0x00075008 File Offset: 0x00073208
		private static void SetGrabAreaProperty(Texture clip, float clipScale, Texture grabTarget, Vector2 targetUV, float rotateAngle, GrabArea.GrabTextureWrapMode wrapMpde, bool replaceAlpha)
		{
			GrabArea.grabAreaMaterial.SetTexture("_ClipTex", clip);
			GrabArea.grabAreaMaterial.SetTexture("_TargetTex", grabTarget);
			GrabArea.grabAreaMaterial.SetFloat("_ClipScale", clipScale);
			GrabArea.grabAreaMaterial.SetFloat("_Rotate", rotateAngle);
			GrabArea.grabAreaMaterial.SetVector("_ClipUV", targetUV);
			foreach (string keyword in GrabArea.grabAreaMaterial.shaderKeywords)
			{
				GrabArea.grabAreaMaterial.DisableKeyword(keyword);
			}
			switch (wrapMpde)
			{
			case GrabArea.GrabTextureWrapMode.Clamp:
				GrabArea.grabAreaMaterial.EnableKeyword("WRAP_MODE_CLAMP");
				break;
			case GrabArea.GrabTextureWrapMode.Repeat:
				GrabArea.grabAreaMaterial.EnableKeyword("WRAP_MODE_REPEAT");
				break;
			case GrabArea.GrabTextureWrapMode.Clip:
				GrabArea.grabAreaMaterial.EnableKeyword("WRAP_MODE_CLIP");
				break;
			}
			if (replaceAlpha)
			{
				GrabArea.grabAreaMaterial.EnableKeyword("ALPHA_REPLACE");
				return;
			}
			GrabArea.grabAreaMaterial.EnableKeyword("ALPHA_NOT_REPLACE");
		}

		// Token: 0x040010EA RID: 4330
		private const string GRAB_AREA_MATERIAL = "Es.InkPainter.Effective.GrabArea";

		// Token: 0x040010EB RID: 4331
		private const string CLIP = "_ClipTex";

		// Token: 0x040010EC RID: 4332
		private const string TARGET = "_TargetTex";

		// Token: 0x040010ED RID: 4333
		private const string CLIP_SCALE = "_ClipScale";

		// Token: 0x040010EE RID: 4334
		private const string CLIP_UV = "_ClipUV";

		// Token: 0x040010EF RID: 4335
		private const string ROTATE = "_Rotate";

		// Token: 0x040010F0 RID: 4336
		private const string WM_CLAMP = "WRAP_MODE_CLAMP";

		// Token: 0x040010F1 RID: 4337
		private const string WM_REPEAT = "WRAP_MODE_REPEAT";

		// Token: 0x040010F2 RID: 4338
		private const string WM_CLIP = "WRAP_MODE_CLIP";

		// Token: 0x040010F3 RID: 4339
		private const string ALPHA_REPLACE = "ALPHA_REPLACE";

		// Token: 0x040010F4 RID: 4340
		private const string ALPHA_NOT_REPLACE = "ALPHA_NOT_REPLACE";

		// Token: 0x040010F5 RID: 4341
		private static Material grabAreaMaterial;

		// Token: 0x020002FB RID: 763
		public enum GrabTextureWrapMode
		{
			// Token: 0x040010F7 RID: 4343
			Clamp,
			// Token: 0x040010F8 RID: 4344
			Repeat,
			// Token: 0x040010F9 RID: 4345
			Clip
		}
	}
}
