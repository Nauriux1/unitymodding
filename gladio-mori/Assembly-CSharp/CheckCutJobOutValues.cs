using System;
using Unity.Burst;
using UnityEngine;

// Token: 0x02000063 RID: 99
[BurstCompile]
public struct CheckCutJobOutValues
{
	// Token: 0x040001AD RID: 429
	public bool fullyCut;

	// Token: 0x040001AE RID: 430
	public ConversionPlane cutPlane;

	// Token: 0x040001AF RID: 431
	public bool parentCut;

	// Token: 0x040001B0 RID: 432
	public Plane parentCutPlane;

	// Token: 0x040001B1 RID: 433
	public int checkCount;

	// Token: 0x040001B2 RID: 434
	public Vector3 cutPlaneStartPoint;

	// Token: 0x040001B3 RID: 435
	public Vector3 cutPlaneEndPoint0;

	// Token: 0x040001B4 RID: 436
	public Vector3 cutPlaneEndPoint1;
}
