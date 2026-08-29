using System;
using Unity.Burst;
using UnityEngine;

// Token: 0x02000070 RID: 112
[BurstCompile]
public struct CuttableMeshJobItem
{
	// Token: 0x04000218 RID: 536
	public Matrix4x4 meshLocalToWorldMatrix;

	// Token: 0x04000219 RID: 537
	public int meshTriCounts;

	// Token: 0x0400021A RID: 538
	public int meshVertCounts;

	// Token: 0x0400021B RID: 539
	public bool ignoreInCheck;

	// Token: 0x0400021C RID: 540
	public int cuttableSectionIndex;
}
