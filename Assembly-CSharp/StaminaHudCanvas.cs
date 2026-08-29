using System;
using Mirror;
using UnityEngine;

// Token: 0x02000145 RID: 325
public class StaminaHudCanvas : MonoBehaviour
{
	// Token: 0x06000A17 RID: 2583 RVA: 0x0002FA2C File Offset: 0x0002DC2C
	private void Awake()
	{
		this.LoadSettings();
	}

	// Token: 0x06000A18 RID: 2584 RVA: 0x0002FA34 File Offset: 0x0002DC34
	public void SetupCamera(Camera camera)
	{
		this.hudCamera = camera;
		this.UpdateCanvasSize();
	}

	// Token: 0x06000A19 RID: 2585 RVA: 0x0002FA43 File Offset: 0x0002DC43
	private void Update()
	{
		if (this.staminaHudVisible)
		{
			this.UpdateHudStamina();
		}
	}

	// Token: 0x06000A1A RID: 2586 RVA: 0x0002FA54 File Offset: 0x0002DC54
	private void UpdateHudStamina()
	{
		if (this.playerHealth == null)
		{
			return;
		}
		if (AccurateInterval.Elapsed(NetworkTime.localTime, this.checkInterval, ref this.lastCheckTime))
		{
			this.prevStaminaArms = this.targetStaminaArms;
			this.prevStaminaCore = this.targetStaminaCore;
			this.prevStaminaLegs = this.targetStaminaLegs;
			this.targetStaminaArms = this.playerHealth.staminaArms;
			this.targetStaminaCore = this.playerHealth.staminaCore;
			this.targetStaminaLegs = this.playerHealth.staminaLegs;
		}
		float t = Mathf.Clamp((float)((NetworkTime.localTime - this.lastCheckTime) / this.checkInterval), 0f, 1f);
		this.sliderStaminaArms.SetValue(Mathf.Lerp(this.prevStaminaArms, this.targetStaminaArms, t));
		this.sliderStaminaCore.SetValue(Mathf.Lerp(this.prevStaminaCore, this.targetStaminaCore, t));
		this.sliderStaminaLegs.SetValue(Mathf.Lerp(this.prevStaminaLegs, this.targetStaminaLegs, t));
	}

	// Token: 0x06000A1B RID: 2587 RVA: 0x0002FB5C File Offset: 0x0002DD5C
	public void UpdateCanvasVisibility()
	{
		this.staminaHudVisible = false;
		if (StaminaManager.singleton != null)
		{
			StaminaManager.singleton.UpdateStaminaManagerActive();
		}
		if (((IGameSettingsManager.singleton != null && IGameSettingsManager.singleton.UseStamina) || (StaminaManager.singleton != null && StaminaManager.singleton.staminaSystemActive)) && !this.forceHide)
		{
			this.staminaHudVisible = true;
		}
		if (!this.staminaHudVisible)
		{
			this.HideAll();
			return;
		}
		this.ShowAll();
	}

	// Token: 0x06000A1C RID: 2588 RVA: 0x0002FBD7 File Offset: 0x0002DDD7
	public void LoadSettings()
	{
		this.UpdateCanvasSize();
		this.UpdateCanvasVisibility();
	}

	// Token: 0x06000A1D RID: 2589 RVA: 0x0002FBE5 File Offset: 0x0002DDE5
	public void SetForceDisableHud(bool value)
	{
		this.forceHide = value;
		this.UpdateCanvasVisibility();
	}

	// Token: 0x06000A1E RID: 2590 RVA: 0x0002FBF4 File Offset: 0x0002DDF4
	public void HideAll()
	{
		this.hudPanel.gameObject.SetActive(false);
	}

	// Token: 0x06000A1F RID: 2591 RVA: 0x0002FC07 File Offset: 0x0002DE07
	public void ShowAll()
	{
		this.hudPanel.gameObject.SetActive(true);
	}

	// Token: 0x06000A20 RID: 2592 RVA: 0x0002FC1C File Offset: 0x0002DE1C
	private void UpdateCanvasSize()
	{
		if (this.hudCamera != null)
		{
			this.hudPanel.anchorMin = new Vector2(this.hudCamera.rect.xMin, this.hudCamera.rect.yMin);
			this.hudPanel.anchorMax = new Vector2(this.hudCamera.rect.xMax, this.hudCamera.rect.yMax);
		}
	}

	// Token: 0x0400070F RID: 1807
	public Canvas hudCanvas;

	// Token: 0x04000710 RID: 1808
	public Camera hudCamera;

	// Token: 0x04000711 RID: 1809
	public RectTransform hudPanel;

	// Token: 0x04000712 RID: 1810
	public IPlayerInputManager playerInputManager;

	// Token: 0x04000713 RID: 1811
	public PlayerHealth playerHealth;

	// Token: 0x04000714 RID: 1812
	public StatusBar sliderStaminaArms;

	// Token: 0x04000715 RID: 1813
	public StatusBar sliderStaminaCore;

	// Token: 0x04000716 RID: 1814
	public StatusBar sliderStaminaLegs;

	// Token: 0x04000717 RID: 1815
	private double lastCheckTime;

	// Token: 0x04000718 RID: 1816
	public double checkInterval = 0.05000000074505806;

	// Token: 0x04000719 RID: 1817
	private float targetStaminaArms = 1f;

	// Token: 0x0400071A RID: 1818
	private float targetStaminaCore = 1f;

	// Token: 0x0400071B RID: 1819
	private float targetStaminaLegs = 1f;

	// Token: 0x0400071C RID: 1820
	private float prevStaminaArms = 1f;

	// Token: 0x0400071D RID: 1821
	private float prevStaminaCore = 1f;

	// Token: 0x0400071E RID: 1822
	private float prevStaminaLegs = 1f;

	// Token: 0x0400071F RID: 1823
	public bool staminaHudVisible = true;

	// Token: 0x04000720 RID: 1824
	public bool forceHide;
}
