using System;
using Unity.Burst;
using UnityEngine;

// Token: 0x02000073 RID: 115
[BurstCompile]
public struct DoCutJobOutValues
{
	// Token: 0x04000234 RID: 564
	public Vector3 cutCenterPosition;

	// Token: 0x04000235 RID: 565
	public Vector3 cutDirection;

	// Token: 0x04000236 RID: 566
	public bool horizontalCut;
}
