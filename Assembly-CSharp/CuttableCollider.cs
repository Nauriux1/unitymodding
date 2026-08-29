using System;
using UnityEngine;

// Token: 0x0200006E RID: 110
[Serializable]
public struct CuttableCollider
{
	// Token: 0x04000210 RID: 528
	public Vector3 p0;

	// Token: 0x04000211 RID: 529
	public Vector3 p1;

	// Token: 0x04000212 RID: 530
	public float radius;

	// Token: 0x04000213 RID: 531
	public ColliderType colliderType;

	// Token: 0x04000214 RID: 532
	public bool parentCollider;
}
