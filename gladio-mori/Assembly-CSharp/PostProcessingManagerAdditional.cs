using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using Utils;

// Token: 0x020000BC RID: 188
public class PostProcessingManagerAdditional : MonoBehaviour
{
	// Token: 0x06000679 RID: 1657 RVA: 0x00020E11 File Offset: 0x0001F011
	private void Start()
	{
		this.InitializePostProcessingManager();
	}

	// Token: 0x0600067A RID: 1658 RVA: 0x00020E19 File Offset: 0x0001F019
	private void InitializePostProcessingManager()
	{
		PostProcessingManagerAdditional.singleton = this;
		this.postProcessingVolume.profile.TryGet<Bloom>(out this.postProcessingBloom);
	}

	// Token: 0x0600067B RID: 1659 RVA: 0x00020E38 File Offset: 0x0001F038
	public void SetPostProcessingForScene(string sceneName)
	{
		if (this.postProcessingBloom != null)
		{
			this.postProcessingBloom.active = SettingsHelper.GetBloom();
		}
	}

	// Token: 0x0600067C RID: 1660 RVA: 0x00020E58 File Offset: 0x0001F058
	public static void LoadPostProcessingSettings()
	{
		if (PostProcessingManagerAdditional.singleton != null)
		{
			PostProcessingManagerAdditional.singleton.SetPostProcessingForScene(SceneManager.GetActiveScene().name);
		}
	}

	// Token: 0x04000465 RID: 1125
	public static PostProcessingManagerAdditional singleton;

	// Token: 0x04000466 RID: 1126
	public Volume postProcessingVolume;

	// Token: 0x04000467 RID: 1127
	public Bloom postProcessingBloom;
}
