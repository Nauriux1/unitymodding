using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Token: 0x0200001A RID: 26
public class NormalRenderPassFeature : ScriptableRendererFeature
{
	// Token: 0x06000142 RID: 322 RVA: 0x0000773B File Offset: 0x0000593B
	public override void Create()
	{
		this.m_ScriptablePass = new NormalRenderPassFeature.CustomRenderPass();
		this.m_ScriptablePass.renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
		this.m_ScriptablePass.ConfigureInput(ScriptableRenderPassInput.Normal);
	}

	// Token: 0x06000143 RID: 323 RVA: 0x00007764 File Offset: 0x00005964
	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		renderer.EnqueuePass(this.m_ScriptablePass);
	}

	// Token: 0x04000087 RID: 135
	private NormalRenderPassFeature.CustomRenderPass m_ScriptablePass;

	// Token: 0x0200001B RID: 27
	private class CustomRenderPass : ScriptableRenderPass
	{
		// Token: 0x06000145 RID: 325 RVA: 0x0000777A File Offset: 0x0000597A
		public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
		{
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000777A File Offset: 0x0000597A
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0000777A File Offset: 0x0000597A
		public override void OnCameraCleanup(CommandBuffer cmd)
		{
		}
	}
}
