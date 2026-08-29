using System;
using UnityEngine;

// Token: 0x02000228 RID: 552
public class SplitScreenCanvas : MonoBehaviour
{
	// Token: 0x060010B9 RID: 4281 RVA: 0x000562BC File Offset: 0x000544BC
	public void SetPlayerCount(int count)
	{
		if (count > 1)
		{
			this.verticalLine.gameObject.SetActive(true);
		}
		if (count > 3)
		{
			this.horizontalLineFull.gameObject.SetActive(true);
			return;
		}
		if (count > 2)
		{
			this.horizontalLineHalf.gameObject.SetActive(true);
		}
	}

	// Token: 0x04000C1B RID: 3099
	public Transform verticalLine;

	// Token: 0x04000C1C RID: 3100
	public Transform horizontalLineFull;

	// Token: 0x04000C1D RID: 3101
	public Transform horizontalLineHalf;
}
