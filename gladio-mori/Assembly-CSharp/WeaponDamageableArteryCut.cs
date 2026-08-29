using System;
using MoveClasses;
using UnityEngine;

// Token: 0x020000D5 RID: 213
public class WeaponDamageableArteryCut
{
	// Token: 0x040004FC RID: 1276
	public WeaponDamageablePart WeaponDamageablePart;

	// Token: 0x040004FD RID: 1277
	public Transform newParent;

	// Token: 0x040004FE RID: 1278
	public Vector3 newPosition;

	// Token: 0x040004FF RID: 1279
	public Quaternion newRotation;

	// Token: 0x04000500 RID: 1280
	public JointType newBodypart;

	// Token: 0x04000501 RID: 1281
	public Transform oldParent;

	// Token: 0x04000502 RID: 1282
	public Vector3 oldPosition;

	// Token: 0x04000503 RID: 1283
	public Quaternion oldRotation;

	// Token: 0x04000504 RID: 1284
	public JointType? oldBodypart;

	// Token: 0x04000505 RID: 1285
	public bool oldBloodFlow;
}
