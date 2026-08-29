using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000171 RID: 369
public class SingleplayerHudCanvas : MonoBehaviour
{
	// Token: 0x06000BBF RID: 3007 RVA: 0x00038824 File Offset: 0x00036A24
	private void Start()
	{
		if (IGameSettingsManager.singleton != null)
		{
			this.timeScale = IGameSettingsManager.singleton.TimeScaleMin;
		}
		this.roundText.gameObject.SetActive(false);
		this.UpdateIconVisibility();
		this.DisplayRoundText();
	}

	// Token: 0x06000BC0 RID: 3008 RVA: 0x0003885C File Offset: 0x00036A5C
	public void UpdateIconVisibility()
	{
		this.winIcon.gameObject.SetActive(false);
		this.lossIcon.gameObject.SetActive(false);
		if (SingleplayerManager.singleton != null)
		{
			if (SingleplayerManager.singleton.singleplayerRun.roundWins > 0)
			{
				this.winIcon.gameObject.SetActive(true);
			}
			if (SingleplayerManager.singleton.singleplayerRun.roundLosses > 0)
			{
				this.lossIcon.gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x06000BC1 RID: 3009 RVA: 0x000388E0 File Offset: 0x00036AE0
	public void DisplayRoundText()
	{
		int num = 1;
		if (SingleplayerManager.singleton != null)
		{
			num = SingleplayerManager.singleton.singleplayerRun.roundLosses + SingleplayerManager.singleton.singleplayerRun.roundWins + 1;
		}
		this.roundText.text = LocalizationHelpers.LocalizedText("txt_round", new object[]
		{
			num
		});
		this.roundText.gameObject.SetActive(true);
		base.Invoke("HideRoundText", this.roundTextDuration * this.timeScale);
	}

	// Token: 0x06000BC2 RID: 3010 RVA: 0x0003896C File Offset: 0x00036B6C
	public void HideRoundText()
	{
		this.roundTextFade = this.roundText.DOFade(0f, this.roundTextFadeTime * this.timeScale);
		TweenerCore<Color, Color, ColorOptions> tweenerCore = this.roundTextFade;
		tweenerCore.onKill = (TweenCallback)Delegate.Combine(tweenerCore.onKill, new TweenCallback(this.OnRoundTextHidden));
	}

	// Token: 0x06000BC3 RID: 3011 RVA: 0x000389C3 File Offset: 0x00036BC3
	public void OnRoundTextHidden()
	{
		this.roundText.gameObject.SetActive(false);
	}

	// Token: 0x0400084A RID: 2122
	public Image winIcon;

	// Token: 0x0400084B RID: 2123
	public Image lossIcon;

	// Token: 0x0400084C RID: 2124
	public Text roundText;

	// Token: 0x0400084D RID: 2125
	private float roundTextDuration = 2f;

	// Token: 0x0400084E RID: 2126
	private float roundTextFadeTime = 0.5f;

	// Token: 0x0400084F RID: 2127
	private float timeScale = 1f;

	// Token: 0x04000850 RID: 2128
	private TweenerCore<Color, Color, ColorOptions> roundTextFade;
}
