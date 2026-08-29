using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using Utils;

// Token: 0x020000BB RID: 187
public class PostProcessingManager : MonoBehaviour
{
	// Token: 0x06000674 RID: 1652 RVA: 0x00020D1C File Offset: 0x0001EF1C
	private void Start()
	{
		this.InitializePostProcessingManager();
	}

	// Token: 0x06000675 RID: 1653 RVA: 0x00020D24 File Offset: 0x0001EF24
	private void InitializePostProcessingManager()
	{
		if (PostProcessingManager.singleton != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		PostProcessingManager.singleton = this;
		this.postProcessingVolume.profile.TryGet<Bloom>(out this.postProcessingBloom);
		this.postProcessingVolume.profile.TryGet<DepthOfField>(out this.postProcessingDepthOfField);
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06000676 RID: 1654 RVA: 0x00020D8C File Offset: 0x0001EF8C
	public void SetPostProcessingForScene(string sceneName)
	{
		if (sceneName.Contains("map_"))
		{
			this.postProcessingBloom.active = SettingsHelper.GetBloom();
			this.postProcessingDepthOfField.active = SettingsHelper.GetDepthOfField();
			return;
		}
		this.postProcessingBloom.active = false;
		this.postProcessingDepthOfField.active = false;
	}

	// Token: 0x06000677 RID: 1655 RVA: 0x00020DE0 File Offset: 0x0001EFE0
	public static void LoadPostProcessingSettings()
	{
		if (PostProcessingManager.singleton != null)
		{
			PostProcessingManager.singleton.SetPostProcessingForScene(SceneManager.GetActiveScene().name);
		}
	}

	// Token: 0x04000461 RID: 1121
	public static PostProcessingManager singleton;

	// Token: 0x04000462 RID: 1122
	public Volume postProcessingVolume;

	// Token: 0x04000463 RID: 1123
	public Bloom postProcessingBloom;

	// Token: 0x04000464 RID: 1124
	public DepthOfField postProcessingDepthOfField;
}
