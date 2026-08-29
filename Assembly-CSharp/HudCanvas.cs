using System;
using UnityEngine;
using UnityEngine.UI;
using Utils;

// Token: 0x02000144 RID: 324
public class HudCanvas : MonoBehaviour
{
	// Token: 0x06000A0E RID: 2574 RVA: 0x0002F790 File Offset: 0x0002D990
	private void Start()
	{
		this.LoadSettings();
	}

	// Token: 0x06000A0F RID: 2575 RVA: 0x0002F798 File Offset: 0x0002D998
	public void SetupCamera(Camera camera)
	{
		this.hudCamera = camera;
		this.UpdateCanvasSize();
	}

	// Token: 0x06000A10 RID: 2576 RVA: 0x0002F7A7 File Offset: 0x0002D9A7
	private void Update()
	{
		this.UpdateHudAttackState();
	}

	// Token: 0x06000A11 RID: 2577 RVA: 0x0002F7B0 File Offset: 0x0002D9B0
	private void UpdateHudAttackState()
	{
		if (this.forceDisableAttackDirection || !this.showAttackDirectionOption)
		{
			return;
		}
		AttackDirection attackDirection = this.playerInputManager.GetAttackDirection();
		this.HideAll();
		if (this.playerInputManager.lastMouseActionTime + 5f > Time.unscaledTime)
		{
			if (attackDirection == AttackDirection.Up)
			{
				this.upAttackImage.color = this.onColor;
				return;
			}
			if (attackDirection == AttackDirection.Down)
			{
				this.downAttackImage.color = this.onColor;
				return;
			}
			if (attackDirection == AttackDirection.Right)
			{
				this.rightAttackImage.color = this.onColor;
				return;
			}
			this.leftAttackImage.color = this.onColor;
		}
	}

	// Token: 0x06000A12 RID: 2578 RVA: 0x0002F84A File Offset: 0x0002DA4A
	public void SetForceDisableAttackDirection(bool value)
	{
		this.forceDisableAttackDirection = value;
		if (this.forceDisableAttackDirection)
		{
			this.HideAll();
		}
	}

	// Token: 0x06000A13 RID: 2579 RVA: 0x0002F864 File Offset: 0x0002DA64
	private void HideAll()
	{
		this.upAttackImage.color = this.offColor;
		this.downAttackImage.color = this.offColor;
		this.leftAttackImage.color = this.offColor;
		this.rightAttackImage.color = this.offColor;
	}

	// Token: 0x06000A14 RID: 2580 RVA: 0x0002F8B5 File Offset: 0x0002DAB5
	public void LoadSettings()
	{
		this.UpdateCanvasSize();
		this.showAttackDirectionOption = SettingsHelper.GetShowAttackDirection();
		if (!this.showAttackDirectionOption)
		{
			this.HideAll();
		}
	}

	// Token: 0x06000A15 RID: 2581 RVA: 0x0002F8D8 File Offset: 0x0002DAD8
	private void UpdateCanvasSize()
	{
		if (this.hudCamera != null)
		{
			this.hudPanel.anchorMin = new Vector2(this.hudCamera.rect.xMin, this.hudCamera.rect.yMin);
			this.hudPanel.anchorMax = new Vector2(this.hudCamera.rect.xMax, this.hudCamera.rect.yMax);
		}
		float num = this.hudPanel.rect.width;
		if (this.hudPanel.rect.height < num)
		{
			num = this.hudPanel.rect.height;
		}
		float num2 = num / 4f;
		this.upAttackImage.rectTransform.anchoredPosition = new Vector2(0f, num2);
		this.downAttackImage.rectTransform.anchoredPosition = new Vector2(0f, num2 * -1f);
		this.leftAttackImage.rectTransform.anchoredPosition = new Vector2(num2 * -1f, 0f);
		this.rightAttackImage.rectTransform.anchoredPosition = new Vector2(num2, 0f);
	}

	// Token: 0x04000703 RID: 1795
	public Canvas hudCanvas;

	// Token: 0x04000704 RID: 1796
	public Camera hudCamera;

	// Token: 0x04000705 RID: 1797
	public RectTransform hudPanel;

	// Token: 0x04000706 RID: 1798
	public IPlayerInputManager playerInputManager;

	// Token: 0x04000707 RID: 1799
	public Image upAttackImage;

	// Token: 0x04000708 RID: 1800
	public Image downAttackImage;

	// Token: 0x04000709 RID: 1801
	public Image leftAttackImage;

	// Token: 0x0400070A RID: 1802
	public Image rightAttackImage;

	// Token: 0x0400070B RID: 1803
	public Color onColor;

	// Token: 0x0400070C RID: 1804
	public Color offColor;

	// Token: 0x0400070D RID: 1805
	private bool forceDisableAttackDirection;

	// Token: 0x0400070E RID: 1806
	private bool showAttackDirectionOption = true;
}
