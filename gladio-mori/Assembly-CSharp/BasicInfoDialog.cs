using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001AB RID: 427
public class BasicInfoDialog : MonoBehaviour
{
	// Token: 0x06000D26 RID: 3366 RVA: 0x00042C44 File Offset: 0x00040E44
	private void Awake()
	{
		this.creationTime = Time.time;
	}

	// Token: 0x06000D27 RID: 3367 RVA: 0x0000777A File Offset: 0x0000597A
	private void Update()
	{
	}

	// Token: 0x06000D28 RID: 3368 RVA: 0x00042C51 File Offset: 0x00040E51
	public void SetText(string text, float newDisplayTime = 1f, bool dontDestroyOnLoad = false)
	{
		this.textField.text = text;
		this.displayTime = newDisplayTime;
		if (dontDestroyOnLoad)
		{
			UnityEngine.Object.DontDestroyOnLoad(this.canvas.gameObject);
		}
		if (this.displayTime > 0f)
		{
			this.SetupTimedDestroy();
		}
	}

	// Token: 0x06000D29 RID: 3369 RVA: 0x00042C8C File Offset: 0x00040E8C
	public void SetupTimedDestroy()
	{
		base.StartCoroutine("DestroyCoroutine");
	}

	// Token: 0x06000D2A RID: 3370 RVA: 0x00042C9C File Offset: 0x00040E9C
	public void SetCanvasCamera(Camera newCamera)
	{
		if (newCamera == null)
		{
			newCamera = Camera.main;
		}
		if (newCamera != null)
		{
			this.canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			this.canvas.planeDistance = 0.5f;
			RectTransform component = this.canvas.transform.GetChild(0).GetComponent<RectTransform>();
			Vector2 min = newCamera.rect.min;
			Vector2 max = newCamera.rect.max;
			component.anchorMin = min;
			component.anchorMax = max;
		}
	}

	// Token: 0x06000D2B RID: 3371 RVA: 0x00042D1F File Offset: 0x00040F1F
	public void DestroyPanel()
	{
		UnityEngine.Object.Destroy(this.canvas.gameObject);
	}

	// Token: 0x06000D2C RID: 3372 RVA: 0x00042D31 File Offset: 0x00040F31
	private IEnumerator DestroyCoroutine()
	{
		yield return new WaitForSecondsRealtime(this.displayTime);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04000978 RID: 2424
	public Canvas canvas;

	// Token: 0x04000979 RID: 2425
	public Text textField;

	// Token: 0x0400097A RID: 2426
	public float creationTime;

	// Token: 0x0400097B RID: 2427
	public float displayTime = 1f;
}
