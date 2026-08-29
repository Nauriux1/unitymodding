using System;
using MoveClasses;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000201 RID: 513
public class MoveDot
{
	// Token: 0x170001A0 RID: 416
	// (get) Token: 0x06000FD0 RID: 4048 RVA: 0x00053522 File Offset: 0x00051722
	// (set) Token: 0x06000FD1 RID: 4049 RVA: 0x0005352A File Offset: 0x0005172A
	public string Name { get; set; }

	// Token: 0x170001A1 RID: 417
	// (get) Token: 0x06000FD2 RID: 4050 RVA: 0x00053533 File Offset: 0x00051733
	// (set) Token: 0x06000FD3 RID: 4051 RVA: 0x0005353B File Offset: 0x0005173B
	public JointMove SingleMove { get; set; }

	// Token: 0x170001A2 RID: 418
	// (get) Token: 0x06000FD4 RID: 4052 RVA: 0x00053544 File Offset: 0x00051744
	// (set) Token: 0x06000FD5 RID: 4053 RVA: 0x0005354C File Offset: 0x0005174C
	public TooltipItem tooltipItem { get; set; }

	// Token: 0x170001A3 RID: 419
	// (get) Token: 0x06000FD6 RID: 4054 RVA: 0x00053555 File Offset: 0x00051755
	// (set) Token: 0x06000FD7 RID: 4055 RVA: 0x0005355D File Offset: 0x0005175D
	public RectTransform rectTransform { get; set; }

	// Token: 0x170001A4 RID: 420
	// (get) Token: 0x06000FD8 RID: 4056 RVA: 0x00053566 File Offset: 0x00051766
	// (set) Token: 0x06000FD9 RID: 4057 RVA: 0x0005356E File Offset: 0x0005176E
	public Image dotImage { get; set; }

	// Token: 0x06000FDA RID: 4058 RVA: 0x00053577 File Offset: 0x00051777
	public void SetPosition(float newPositionX, float newPositionY)
	{
		this.positionX = newPositionX;
		this.positionY = newPositionY;
	}

	// Token: 0x06000FDB RID: 4059 RVA: 0x00053587 File Offset: 0x00051787
	public void UpdatePosition()
	{
		this.rectTransform.anchoredPosition = new Vector3(this.positionX, this.positionY, 0f);
	}

	// Token: 0x06000FDC RID: 4060 RVA: 0x000535AF File Offset: 0x000517AF
	public void Disable()
	{
		this.SingleMove = null;
		this.rectTransform.anchoredPosition = new Vector3(-1000f, 0f, 0f);
	}

	// Token: 0x06000FDD RID: 4061 RVA: 0x0000777A File Offset: 0x0000597A
	public void Enable()
	{
	}

	// Token: 0x04000B5D RID: 2909
	public float positionX;

	// Token: 0x04000B5E RID: 2910
	public float positionY;
}
