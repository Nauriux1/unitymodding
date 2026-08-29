using System;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;

// Token: 0x02000064 RID: 100
[BurstCompile]
public struct CutColliderSectionRing
{
	// Token: 0x040001B5 RID: 437
	public FixedList128Bytes<Vector2> points;
}
