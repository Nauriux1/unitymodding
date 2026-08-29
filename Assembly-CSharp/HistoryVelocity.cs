using System;
using UnityEngine;

// Token: 0x02000048 RID: 72
public struct HistoryVelocity
{
	// Token: 0x06000212 RID: 530 RVA: 0x0000BFAA File Offset: 0x0000A1AA
	public Vector3 GetLocalVelocity()
	{
		return this.worldToLocalMatrix.MultiplyVector(this.velocity);
	}

	// Token: 0x06000213 RID: 531 RVA: 0x0000BFBD File Offset: 0x0000A1BD
	public Vector3 GetLocalAngularVelocity()
	{
		return this.worldToLocalMatrix.MultiplyVector(this.angularVelocity);
	}

	// Token: 0x04000162 RID: 354
	public Vector3 velocity;

	// Token: 0x04000163 RID: 355
	public Vector3 angularVelocity;

	// Token: 0x04000164 RID: 356
	public Matrix4x4 worldToLocalMatrix;
}
