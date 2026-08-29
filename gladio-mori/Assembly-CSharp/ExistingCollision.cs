using System;
using UnityEngine;

// Token: 0x020000E2 RID: 226
public class ExistingCollision
{
	// Token: 0x060007D0 RID: 2000 RVA: 0x00026B26 File Offset: 0x00024D26
	public void Clear()
	{
		this.gameObject1 = null;
		this.gameObject2 = null;
		this.removeAtTime = 0f;
	}

	// Token: 0x04000539 RID: 1337
	public GameObject gameObject1;

	// Token: 0x0400053A RID: 1338
	public GameObject gameObject2;

	// Token: 0x0400053B RID: 1339
	public float removeAtTime;
}
