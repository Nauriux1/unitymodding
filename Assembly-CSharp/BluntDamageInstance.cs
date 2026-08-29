using System;
using MoveClasses;
using Unity.Burst;

// Token: 0x0200002F RID: 47
[BurstCompile]
public struct BluntDamageInstance
{
	// Token: 0x040000CD RID: 205
	public JointType bodyPart;

	// Token: 0x040000CE RID: 206
	public float temporaryDamage;

	// Token: 0x040000CF RID: 207
	public float permanentDamage;
}
