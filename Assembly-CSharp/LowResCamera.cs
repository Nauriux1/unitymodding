using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200003B RID: 59
public class LowResCamera : MonoBehaviour
{
	// Token: 0x060001DC RID: 476 RVA: 0x0000AF9C File Offset: 0x0000919C
	private void Awake()
	{
		if (PlayerPrefs.GetString("LowRes", "0") == "1")
		{
			this.settingsCamera = base.GetComponent<Camera>();
			int @int = PlayerPrefs.GetInt("RenderTextureMultiplier", 2);
			this.renderTexture = new RenderTexture(192 * @int, 108 * @int, 32);
			this.renderTexture.wrapMode = TextureWrapMode.Clamp;
			this.renderTexture.filterMode = FilterMode.Point;
			this.renderTexture.antiAliasing = PlayerPrefs.GetInt("AntiAliasing", 2);
			this.renderTexture.Create();
			this.settingsCamera.targetTexture = this.renderTexture;
			this.cameraCanvas = new GameObject
			{
				name = "CameraCanvas(" + base.name + ")"
			}.AddComponent<Canvas>();
			this.cameraCanvas.renderMode = RenderMode.ScreenSpaceCamera;
			this.cameraCanvas.sortingOrder = -1;
			this.rawImage = new GameObject
			{
				name = "rawImage",
				transform = 
				{
					parent = this.cameraCanvas.transform
				}
			}.AddComponent<RawImage>();
			this.rawImage.transform.position = new Vector3(0f, 0f, -100f);
			this.rawImage.rectTransform.anchorMin = new Vector2(0f, 0f);
			this.rawImage.rectTransform.anchorMax = new Vector2(1f, 1f);
			this.rawImage.rectTransform.offsetMin = new Vector2(0f, 0f);
			this.rawImage.rectTransform.offsetMax = new Vector2(1f, 1f);
			this.rawImage.texture = this.renderTexture;
		}
	}

	// Token: 0x04000128 RID: 296
	private Camera settingsCamera;

	// Token: 0x04000129 RID: 297
	private RenderTexture renderTexture;

	// Token: 0x0400012A RID: 298
	public Canvas cameraCanvas;

	// Token: 0x0400012B RID: 299
	private RawImage rawImage;
}
