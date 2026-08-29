using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001A7 RID: 423
public class BasicCanvas : MonoBehaviour
{
	// Token: 0x06000D1A RID: 3354 RVA: 0x0004276C File Offset: 0x0004096C
	private void Awake()
	{
		this.canvasScaler = base.gameObject.GetComponent<CanvasScaler>();
		this.canvas = base.gameObject.GetComponent<Canvas>();
		if (this.canvasScaler == null)
		{
			this.canvasScaler = base.gameObject.AddComponent<CanvasScaler>();
			this.canvasScaler.enabled = false;
		}
		if (this.canvas != null)
		{
			this.canvas.pixelPerfect = true;
		}
		this.UpdateCanvas();
	}

	// Token: 0x06000D1B RID: 3355 RVA: 0x000427E8 File Offset: 0x000409E8
	public void UpdateCanvas()
	{
		if (this.canvasScaler != null)
		{
			if (this.forceCanvasScale && Screen.width < 1920)
			{
				this.canvasScaler.referenceResolution = new Vector2(1920f, 1080f);
				this.canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
				this.canvasScaler.enabled = true;
				return;
			}
			if (Screen.width < 1000)
			{
				this.canvasScaler.referenceResolution = new Vector2(1000f, 1000f);
				this.canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
				this.canvasScaler.enabled = true;
				return;
			}
			this.canvasScaler.enabled = false;
		}
	}

	// Token: 0x04000968 RID: 2408
	public bool forceCanvasScale;

	// Token: 0x04000969 RID: 2409
	private CanvasScaler canvasScaler;

	// Token: 0x0400096A RID: 2410
	private Canvas canvas;
}
