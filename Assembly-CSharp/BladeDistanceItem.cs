using System;
using UnityEngine;

// Token: 0x020000A9 RID: 169
public struct BladeDistanceItem
{
	// Token: 0x040003B1 RID: 945
	public Vector3 closestProtectedPoint;

	// Token: 0x040003B2 RID: 946
	public Vector3 closestWeaponPoint;

	// Token: 0x040003B3 RID: 947
	public float positionOnProtectedLine;

	// Token: 0x040003B4 RID: 948
	public Vector3 vectorFromProtectedPointToWeaponPoint;

	// Token: 0x040003B5 RID: 949
	public float distanceBetweenPoints;
}
