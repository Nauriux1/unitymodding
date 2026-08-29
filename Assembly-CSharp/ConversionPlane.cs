using System;
using Unity.Burst;
using UnityEngine;

// Token: 0x0200023E RID: 574
[BurstCompile]
public struct ConversionPlane
{
	// Token: 0x04000C84 RID: 3204
	public Plane plane;

	// Token: 0x04000C85 RID: 3205
	public Vector3 right;

	// Token: 0x04000C86 RID: 3206
	public Vector3 forward;
}
