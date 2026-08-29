using System;
using System.Collections;
using Es.InkPainter.Effective;
using UnityEngine;

namespace Es.InkPainter.Sample
{
	// Token: 0x02000307 RID: 775
	[RequireComponent(typeof(InkCanvas))]
	public class TextureRecovery : MonoBehaviour
	{
		// Token: 0x0600174A RID: 5962 RVA: 0x00076351 File Offset: 0x00074551
		private void Awake()
		{
			this.canvas = base.GetComponent<InkCanvas>();
			this.canvas.OnInitializedAfter += this.Init;
		}

		// Token: 0x0600174B RID: 5963 RVA: 0x00076378 File Offset: 0x00074578
		private void Init(InkCanvas canvas)
		{
			this.material = base.GetComponent<MeshRenderer>().sharedMaterial;
			this.defaultMainTexture = canvas.GetMainTexture(this.material.name);
			this.paintMainTexture = canvas.GetPaintMainTexture(this.material.name);
			this.defaultNormalMap = canvas.GetNormalTexture(this.material.name);
			this.paintNormalMap = canvas.GetPaintNormalTexture(this.material.name);
			this.defaultHeightMap = canvas.GetHeightTexture(this.material.name);
			this.paintHeightMap = canvas.GetPaintHeightTexture(this.material.name);
			base.StartCoroutine(this.TextureLerp());
		}

		// Token: 0x0600174C RID: 5964 RVA: 0x00076430 File Offset: 0x00074630
		public void FixedUpdate()
		{
			if (!this.@fixed)
			{
				return;
			}
			if (this.defaultMainTexture != null && this.paintMainTexture != null)
			{
				TextureMorphing.Lerp(this.defaultMainTexture, this.paintMainTexture, this.lerpCoefficient);
			}
			if (this.defaultNormalMap != null && this.paintNormalMap != null)
			{
				TextureMorphing.Lerp(this.defaultNormalMap, this.paintNormalMap, this.lerpCoefficient);
			}
			if (this.defaultHeightMap != null && this.paintHeightMap != null)
			{
				TextureMorphing.Lerp(this.defaultHeightMap, this.paintHeightMap, this.lerpCoefficient);
			}
		}

		// Token: 0x0600174D RID: 5965 RVA: 0x000764DF File Offset: 0x000746DF
		private IEnumerator TextureLerp()
		{
			for (;;)
			{
				if (this.@fixed)
				{
					yield return new WaitForSeconds(1f);
				}
				else
				{
					int num;
					for (int i = 0; i < 10; i = num)
					{
						yield return new WaitForSeconds(this.callTimer / 10f);
						if (this.defaultMainTexture != null && this.paintMainTexture != null)
						{
							TextureMorphing.Lerp(this.defaultMainTexture, this.paintMainTexture, this.lerpCoefficient / 10f);
						}
						if (this.defaultNormalMap != null && this.paintNormalMap != null)
						{
							TextureMorphing.Lerp(this.defaultNormalMap, this.paintNormalMap, this.lerpCoefficient / 10f);
						}
						if (this.defaultHeightMap != null && this.paintHeightMap != null)
						{
							TextureMorphing.Lerp(this.defaultHeightMap, this.paintHeightMap, this.lerpCoefficient / 10f);
						}
						num = i + 1;
					}
				}
			}
			yield break;
		}

		// Token: 0x04001140 RID: 4416
		[SerializeField]
		private float lerpCoefficient = 0.1f;

		// Token: 0x04001141 RID: 4417
		[SerializeField]
		private float callTimer = 0.1f;

		// Token: 0x04001142 RID: 4418
		[SerializeField]
		private bool @fixed;

		// Token: 0x04001143 RID: 4419
		private Material material;

		// Token: 0x04001144 RID: 4420
		private InkCanvas canvas;

		// Token: 0x04001145 RID: 4421
		private Texture defaultMainTexture;

		// Token: 0x04001146 RID: 4422
		private RenderTexture paintMainTexture;

		// Token: 0x04001147 RID: 4423
		private Texture defaultNormalMap;

		// Token: 0x04001148 RID: 4424
		private RenderTexture paintNormalMap;

		// Token: 0x04001149 RID: 4425
		private Texture defaultHeightMap;

		// Token: 0x0400114A RID: 4426
		private RenderTexture paintHeightMap;
	}
}
