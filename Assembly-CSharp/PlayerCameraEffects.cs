using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200003C RID: 60
public class PlayerCameraEffects : MonoBehaviour
{
	// Token: 0x060001DE RID: 478 RVA: 0x0000B16B File Offset: 0x0000936B
	private void Awake()
	{
		RenderPipelineManager.beginCameraRendering += this.OnBeginCameraRendering;
	}

	// Token: 0x060001DF RID: 479 RVA: 0x0000B17E File Offset: 0x0000937E
	private void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
	{
		if (camera.gameObject == base.gameObject)
		{
			GeneralManager.SetVignetteValue(this.vignetteValue, false);
		}
	}

	// Token: 0x060001E0 RID: 480 RVA: 0x0000B19F File Offset: 0x0000939F
	private void OnDestroy()
	{
		RenderPipelineManager.beginCameraRendering -= this.OnBeginCameraRendering;
	}

	// Token: 0x0400012C RID: 300
	public float vignetteValue;
}
