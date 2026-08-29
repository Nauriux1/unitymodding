using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using Utils;

// Token: 0x0200020A RID: 522
public class SideToolButtons : MonoBehaviour
{
	// Token: 0x06001003 RID: 4099 RVA: 0x000537AB File Offset: 0x000519AB
	private void Start()
	{
		this.activeSideTool = null;
		this.InitializeSideTools();
	}

	// Token: 0x06001004 RID: 4100 RVA: 0x000537BC File Offset: 0x000519BC
	private void InitializeSideTools()
	{
		using (List<SideToolPair>.Enumerator enumerator = this.sideTools.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SideToolPair sideTool = enumerator.Current;
				sideTool.button.onClick.AddListener(delegate()
				{
					this.DisplaySideTool(sideTool);
				});
			}
		}
	}

	// Token: 0x06001005 RID: 4101 RVA: 0x0005383C File Offset: 0x00051A3C
	private void DisplaySideTool(SideToolPair sideToolPair = null)
	{
		this.KillRunningTween();
		if (this.activeSideTool != null)
		{
			if (sideToolPair == this.activeSideTool)
			{
				sideToolPair = null;
			}
			this.HideSideTool();
		}
		if (sideToolPair != null)
		{
			this.runningTween = sideToolPair.panel.DOAnchorPosX(0f, 0.1f, false);
			UIHelpers.SetButtonColor(sideToolPair.button, ButtonState.Selected, null, null);
			this.activeSideTool = sideToolPair;
		}
	}

	// Token: 0x06001006 RID: 4102 RVA: 0x000538A0 File Offset: 0x00051AA0
	private void HideSideTool()
	{
		if (this.activeSideTool != null && this.activeSideTool.panel != null)
		{
			this.runningTween = this.activeSideTool.panel.DOAnchorPosX(270f, 0.1f, false);
			UIHelpers.SetButtonColor(this.activeSideTool.button, ButtonState.Basic, null, null);
		}
		this.activeSideTool = null;
	}

	// Token: 0x06001007 RID: 4103 RVA: 0x00053903 File Offset: 0x00051B03
	private void KillRunningTween()
	{
		if (this.runningTween != null && this.runningTween.active)
		{
			this.runningTween.Kill(false);
		}
	}

	// Token: 0x06001008 RID: 4104 RVA: 0x00053926 File Offset: 0x00051B26
	public void SetCanvasVisible(bool value)
	{
		base.gameObject.SetActive(value);
	}

	// Token: 0x04000B75 RID: 2933
	public List<SideToolPair> sideTools = new List<SideToolPair>();

	// Token: 0x04000B76 RID: 2934
	public SideToolPair activeSideTool;

	// Token: 0x04000B77 RID: 2935
	private TweenerCore<Vector2, Vector2, VectorOptions> runningTween;
}
