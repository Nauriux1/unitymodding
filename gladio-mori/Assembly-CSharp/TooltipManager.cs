using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000101 RID: 257
public class TooltipManager : MonoBehaviour
{
	// Token: 0x0600086A RID: 2154 RVA: 0x00029B9E File Offset: 0x00027D9E
	private void Awake()
	{
		this.InitializeTooltipManager();
	}

	// Token: 0x0600086B RID: 2155 RVA: 0x00029BA6 File Offset: 0x00027DA6
	public void InitializeTooltipManager()
	{
		if (TooltipManager.singleton != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		TooltipManager.singleton = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		TooltipManager.Hide();
		Debug.Log("Tooltip manager has been setup");
	}

	// Token: 0x0600086C RID: 2156 RVA: 0x00029BE1 File Offset: 0x00027DE1
	private void Update()
	{
		this.UpdatePosition();
	}

	// Token: 0x0600086D RID: 2157 RVA: 0x00029BE9 File Offset: 0x00027DE9
	public void UpdatePosition()
	{
		if (!this.showing)
		{
			return;
		}
		this.tooltipPanel.position = new Vector2(Input.mousePosition.x + 20f, Input.mousePosition.y + 22f);
	}

	// Token: 0x0600086E RID: 2158 RVA: 0x00029C29 File Offset: 0x00027E29
	public void ShowTooltip(string text)
	{
		this.tooltipPanel.gameObject.SetActive(true);
		this.textField.text = text;
		this.showing = true;
	}

	// Token: 0x0600086F RID: 2159 RVA: 0x00029C4F File Offset: 0x00027E4F
	public void HideTooltip()
	{
		this.tooltipPanel.gameObject.SetActive(false);
		this.showing = false;
	}

	// Token: 0x06000870 RID: 2160 RVA: 0x00029C69 File Offset: 0x00027E69
	public static void Show(string text)
	{
		if (TooltipManager.singleton != null)
		{
			TooltipManager.singleton.ShowTooltip(text);
		}
	}

	// Token: 0x06000871 RID: 2161 RVA: 0x00029C83 File Offset: 0x00027E83
	public static void Hide()
	{
		if (TooltipManager.singleton != null)
		{
			TooltipManager.singleton.HideTooltip();
		}
	}

	// Token: 0x040005D2 RID: 1490
	public static TooltipManager singleton;

	// Token: 0x040005D3 RID: 1491
	public RectTransform tooltipPanel;

	// Token: 0x040005D4 RID: 1492
	public Text textField;

	// Token: 0x040005D5 RID: 1493
	public bool showing;
}
