using System;
using Unity.Burst;
using UnityEngine;

// Token: 0x0200023F RID: 575
[BurstCompile]
public struct NullableJobVector3
{
	// Token: 0x04000C87 RID: 3207
	public bool hasValue;

	// Token: 0x04000C88 RID: 3208
	public Vector3 vector3;

	// Token: 0x04000C89 RID: 3209
	public float percentage;
}
