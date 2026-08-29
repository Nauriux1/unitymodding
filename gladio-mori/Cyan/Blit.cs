using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Cyan
{
	// Token: 0x02000266 RID: 614
	public class Blit : ScriptableRendererFeature
	{
		// Token: 0x060011D9 RID: 4569 RVA: 0x0005B5F8 File Offset: 0x000597F8
		public override void Create()
		{
			int max = (this.settings.blitMaterial != null) ? (this.settings.blitMaterial.passCount - 1) : 1;
			this.settings.blitMaterialPassIndex = Mathf.Clamp(this.settings.blitMaterialPassIndex, -1, max);
			this.blitPass = new Blit.BlitPass(this.settings.Event, this.settings, base.name);
			if (this.settings.graphicsFormat == GraphicsFormat.None)
			{
				this.settings.graphicsFormat = SystemInfo.GetGraphicsFormat(DefaultFormat.LDR);
			}
		}

		// Token: 0x060011DA RID: 4570 RVA: 0x0005B68C File Offset: 0x0005988C
		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			if (renderingData.cameraData.cameraType == CameraType.Reflection)
			{
				return;
			}
			if (renderingData.cameraData.isPreviewCamera)
			{
				return;
			}
			if (!this.settings.canShowInSceneView && renderingData.cameraData.isSceneViewCamera)
			{
				return;
			}
			if (this.settings.blitMaterial == null)
			{
				Debug.LogWarningFormat("Missing Blit Material. {0} blit pass will not execute. Check for missing reference in the assigned renderer.", new object[]
				{
					base.GetType().Name
				});
				return;
			}
			this.blitPass.Setup(renderer);
			renderer.EnqueuePass(this.blitPass);
		}

		// Token: 0x04000D71 RID: 3441
		public Blit.BlitSettings settings = new Blit.BlitSettings();

		// Token: 0x04000D72 RID: 3442
		public Blit.BlitPass blitPass;

		// Token: 0x02000267 RID: 615
		public class BlitPass : ScriptableRenderPass
		{
			// Token: 0x170001C4 RID: 452
			// (get) Token: 0x060011DC RID: 4572 RVA: 0x0005B72F File Offset: 0x0005992F
			// (set) Token: 0x060011DD RID: 4573 RVA: 0x0005B737 File Offset: 0x00059937
			public FilterMode filterMode { get; set; }

			// Token: 0x170001C5 RID: 453
			// (get) Token: 0x060011DE RID: 4574 RVA: 0x0005B740 File Offset: 0x00059940
			// (set) Token: 0x060011DF RID: 4575 RVA: 0x0005B748 File Offset: 0x00059948
			private RenderTargetIdentifier source { get; set; }

			// Token: 0x170001C6 RID: 454
			// (get) Token: 0x060011E0 RID: 4576 RVA: 0x0005B751 File Offset: 0x00059951
			// (set) Token: 0x060011E1 RID: 4577 RVA: 0x0005B759 File Offset: 0x00059959
			private RenderTargetIdentifier destination { get; set; }

			// Token: 0x060011E2 RID: 4578 RVA: 0x0005B764 File Offset: 0x00059964
			public BlitPass(RenderPassEvent renderPassEvent, Blit.BlitSettings settings, string tag)
			{
				base.renderPassEvent = renderPassEvent;
				this.settings = settings;
				this.blitMaterial = settings.blitMaterial;
				this.m_ProfilerTag = tag;
				this.m_TemporaryColorTexture.Init("_TemporaryColorTexture");
				if (settings.dstType == Cyan.Blit.Target.TextureID)
				{
					this.m_DestinationTexture.Init(settings.dstTextureId);
				}
			}

			// Token: 0x060011E3 RID: 4579 RVA: 0x0005B7C2 File Offset: 0x000599C2
			public void Setup(ScriptableRenderer renderer)
			{
				if (this.settings.requireDepthNormals)
				{
					base.ConfigureInput(ScriptableRenderPassInput.Normal);
				}
			}

			// Token: 0x060011E4 RID: 4580 RVA: 0x0005B7D8 File Offset: 0x000599D8
			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				CommandBuffer commandBuffer = CommandBufferPool.Get(this.m_ProfilerTag);
				RenderTextureDescriptor cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
				cameraTargetDescriptor.depthBufferBits = 0;
				ScriptableRenderer renderer = renderingData.cameraData.renderer;
				if (this.settings.srcType == Cyan.Blit.Target.CameraColor)
				{
					this.source = renderer.cameraColorTarget;
				}
				else if (this.settings.srcType == Cyan.Blit.Target.TextureID)
				{
					this.source = new RenderTargetIdentifier(this.settings.srcTextureId);
				}
				else if (this.settings.srcType == Cyan.Blit.Target.RenderTextureObject)
				{
					this.source = new RenderTargetIdentifier(this.settings.srcTextureObject);
				}
				if (this.settings.dstType == Cyan.Blit.Target.CameraColor)
				{
					this.destination = renderer.cameraColorTarget;
				}
				else if (this.settings.dstType == Cyan.Blit.Target.TextureID)
				{
					this.destination = new RenderTargetIdentifier(this.settings.dstTextureId);
				}
				else if (this.settings.dstType == Cyan.Blit.Target.RenderTextureObject)
				{
					this.destination = new RenderTargetIdentifier(this.settings.dstTextureObject);
				}
				if (this.settings.setInverseViewMatrix)
				{
					Shader.SetGlobalMatrix("_InverseView", renderingData.cameraData.camera.cameraToWorldMatrix);
				}
				if (this.settings.dstType == Cyan.Blit.Target.TextureID)
				{
					if (this.settings.overrideGraphicsFormat)
					{
						cameraTargetDescriptor.graphicsFormat = this.settings.graphicsFormat;
					}
					commandBuffer.GetTemporaryRT(this.m_DestinationTexture.id, cameraTargetDescriptor, this.filterMode);
				}
				if (this.source == this.destination || (this.settings.srcType == this.settings.dstType && this.settings.srcType == Cyan.Blit.Target.CameraColor))
				{
					commandBuffer.GetTemporaryRT(this.m_TemporaryColorTexture.id, cameraTargetDescriptor, this.filterMode);
					base.Blit(commandBuffer, this.source, this.m_TemporaryColorTexture.Identifier(), this.blitMaterial, this.settings.blitMaterialPassIndex);
					base.Blit(commandBuffer, this.m_TemporaryColorTexture.Identifier(), this.destination, null, 0);
				}
				else
				{
					base.Blit(commandBuffer, this.source, this.destination, this.blitMaterial, this.settings.blitMaterialPassIndex);
				}
				context.ExecuteCommandBuffer(commandBuffer);
				CommandBufferPool.Release(commandBuffer);
			}

			// Token: 0x060011E5 RID: 4581 RVA: 0x0005BA10 File Offset: 0x00059C10
			public override void FrameCleanup(CommandBuffer cmd)
			{
				if (this.settings.dstType == Cyan.Blit.Target.TextureID)
				{
					cmd.ReleaseTemporaryRT(this.m_DestinationTexture.id);
				}
				if (this.source == this.destination || (this.settings.srcType == this.settings.dstType && this.settings.srcType == Cyan.Blit.Target.CameraColor))
				{
					cmd.ReleaseTemporaryRT(this.m_TemporaryColorTexture.id);
				}
			}

			// Token: 0x04000D73 RID: 3443
			public Material blitMaterial;

			// Token: 0x04000D75 RID: 3445
			private Blit.BlitSettings settings;

			// Token: 0x04000D78 RID: 3448
			private RenderTargetHandle m_TemporaryColorTexture;

			// Token: 0x04000D79 RID: 3449
			private RenderTargetHandle m_DestinationTexture;

			// Token: 0x04000D7A RID: 3450
			private string m_ProfilerTag;
		}

		// Token: 0x02000268 RID: 616
		[Serializable]
		public class BlitSettings
		{
			// Token: 0x04000D7B RID: 3451
			public RenderPassEvent Event = RenderPassEvent.AfterRenderingOpaques;

			// Token: 0x04000D7C RID: 3452
			public Material blitMaterial;

			// Token: 0x04000D7D RID: 3453
			public int blitMaterialPassIndex;

			// Token: 0x04000D7E RID: 3454
			public bool setInverseViewMatrix;

			// Token: 0x04000D7F RID: 3455
			public bool requireDepthNormals;

			// Token: 0x04000D80 RID: 3456
			public Blit.Target srcType;

			// Token: 0x04000D81 RID: 3457
			public string srcTextureId = "_CameraColorTexture";

			// Token: 0x04000D82 RID: 3458
			public RenderTexture srcTextureObject;

			// Token: 0x04000D83 RID: 3459
			public Blit.Target dstType;

			// Token: 0x04000D84 RID: 3460
			public string dstTextureId = "_BlitPassTexture";

			// Token: 0x04000D85 RID: 3461
			public RenderTexture dstTextureObject;

			// Token: 0x04000D86 RID: 3462
			public bool overrideGraphicsFormat;

			// Token: 0x04000D87 RID: 3463
			public GraphicsFormat graphicsFormat;

			// Token: 0x04000D88 RID: 3464
			public bool canShowInSceneView = true;
		}

		// Token: 0x02000269 RID: 617
		public enum Target
		{
			// Token: 0x04000D8A RID: 3466
			CameraColor,
			// Token: 0x04000D8B RID: 3467
			TextureID,
			// Token: 0x04000D8C RID: 3468
			RenderTextureObject
		}
	}
}
