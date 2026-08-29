using System;
using UnityEngine;

// Token: 0x0200025B RID: 603
public struct BladeTriangle
{
	// Token: 0x060011A8 RID: 4520 RVA: 0x0005A834 File Offset: 0x00058A34
	public BladeTriangle(Vector3 v0, Vector3 v1, Vector3 v2)
	{
		this.p0 = v0;
		this.p1 = v1;
		this.p2 = v2;
	}

	// Token: 0x04000D47 RID: 3399
	public Vector3 p0;

	// Token: 0x04000D48 RID: 3400
	public Vector3 p1;

	// Token: 0x04000D49 RID: 3401
	public Vector3 p2;
}
