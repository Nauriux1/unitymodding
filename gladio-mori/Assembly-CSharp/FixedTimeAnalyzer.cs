using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020000F1 RID: 241
public class FixedTimeAnalyzer : MonoBehaviour
{
	// Token: 0x06000817 RID: 2071 RVA: 0x000283E8 File Offset: 0x000265E8
	private void Start()
	{
		this.style.alignment = TextAnchor.UpperRight;
		this.style.fontSize = 24;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06000818 RID: 2072 RVA: 0x00028410 File Offset: 0x00026610
	private void FixedUpdate()
	{
		this.fixedDeltaTimes.Add(Time.fixedDeltaTime);
		if (this.fixedDeltaTimes.Count > 10)
		{
			this.fixedDeltaTimes.RemoveAt(0);
		}
		this.slowest = (from x in this.fixedDeltaTimes
		orderby x
		select x).FirstOrDefault<float>();
	}

	// Token: 0x06000819 RID: 2073 RVA: 0x0002847D File Offset: 0x0002667D
	private void OnGUI()
	{
		GUI.Label(new Rect((float)(Screen.width - 100), 30f, 100f, 0f), string.Format("{0}", this.slowest), this.style);
	}

	// Token: 0x04000593 RID: 1427
	private GUIStyle style = new GUIStyle();

	// Token: 0x04000594 RID: 1428
	private List<float> fixedDeltaTimes = new List<float>();

	// Token: 0x04000595 RID: 1429
	private float slowest;
}
