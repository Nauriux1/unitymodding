using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

// Token: 0x02000038 RID: 56
public class CameraSettings : MonoBehaviour
{
	// Token: 0x060001BF RID: 447 RVA: 0x0000A5D1 File Offset: 0x000087D1
	private void Start()
	{
		this.settingsCamera = base.GetComponent<Camera>();
		this.additionalCameraData = this.settingsCamera.GetUniversalAdditionalCameraData();
		this.LoadSettings();
	}

	// Token: 0x060001C0 RID: 448 RVA: 0x0000A5F8 File Offset: 0x000087F8
	public void LoadSettings()
	{
		if (this.settingsCamera != null && this.additionalCameraData)
		{
			this.settingsCamera.nearClipPlane = 0.01f;
			this.additionalCameraData.renderPostProcessing = true;
			this.additionalCameraData.antialiasing = (AntialiasingMode)PlayerPrefs.GetInt("PostProcessingAA", 1);
			if (this.forceHorizontalFOV)
			{
				this.settingsCamera.fieldOfView = 2f * Mathf.Atan(Mathf.Tan(this.forceHorizontalFovValue * 0.017453292f * 0.5f) / this.settingsCamera.aspect) * 57.29578f;
			}
			if (this.settingsCamera.targetTexture != null)
			{
				this.settingsCamera.targetTexture.Release();
				this.settingsCamera.targetTexture.width = Screen.width;
				this.settingsCamera.targetTexture.height = Screen.height;
				this.settingsCamera.targetTexture.antiAliasing = PlayerPrefs.GetInt("MSAA", 1);
				this.settingsCamera.ResetAspect();
			}
		}
	}

	// Token: 0x040000FF RID: 255
	private Camera settingsCamera;

	// Token: 0x04000100 RID: 256
	private UniversalAdditionalCameraData additionalCameraData;

	// Token: 0x04000101 RID: 257
	public bool forceHorizontalFOV;

	// Token: 0x04000102 RID: 258
	public float forceHorizontalFovValue = 92f;
}
