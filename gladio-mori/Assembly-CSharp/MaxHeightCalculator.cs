using System;
using UnityEngine;

// Token: 0x0200018C RID: 396
public class MaxHeightCalculator : MonoBehaviour
{
	// Token: 0x06000C6A RID: 3178 RVA: 0x0000777A File Offset: 0x0000597A
	private void Start()
	{
	}

	// Token: 0x06000C6B RID: 3179 RVA: 0x0003C68C File Offset: 0x0003A88C
	private void Update()
	{
		if (base.transform.position.y > this.height)
		{
			this.height = base.transform.position.y;
		}
		if (this.reset)
		{
			this.reset = false;
			this.height = 0f;
		}
	}

	// Token: 0x040008D2 RID: 2258
	public bool reset;

	// Token: 0x040008D3 RID: 2259
	public float height;
}
