using System;
using Unity.Burst;
using UnityEngine;

// Token: 0x02000240 RID: 576
[BurstCompile]
public struct CuttableJobSection
{
	// Token: 0x04000C8A RID: 3210
	public Vector3 originalPosition;

	// Token: 0x04000C8B RID: 3211
	public Vector3 position;

	// Token: 0x04000C8C RID: 3212
	public bool isCut;

	// Token: 0x04000C8D RID: 3213
	public bool isParent;

	// Token: 0x04000C8E RID: 3214
	public Matrix4x4 localToWorldMatrix;
}
