using System;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;

// Token: 0x02000061 RID: 97
[BurstCompile]
public struct BladeSectionJobItem
{
	// Token: 0x0400019C RID: 412
	public FixedList512Bytes<Vector3> bladePoints;

	// Token: 0x0400019D RID: 413
	public FixedList512Bytes<Vector2> bladePointsOnPlane;
}
