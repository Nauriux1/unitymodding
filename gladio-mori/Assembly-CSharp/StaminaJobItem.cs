using System;
using MoveClasses;
using Unity.Collections;
using Unity.Mathematics;

// Token: 0x020000EB RID: 235
public struct StaminaJobItem
{
	// Token: 0x04000564 RID: 1380
	public FixedList128Bytes<float> calculatedJointMaxForce;

	// Token: 0x04000565 RID: 1381
	public FixedList128Bytes<float> calculatedJointSpring;

	// Token: 0x04000566 RID: 1382
	public FixedList512Bytes<quaternion> oldRotations;

	// Token: 0x04000567 RID: 1383
	[ReadOnly]
	public FixedList512Bytes<quaternion> currentRotations;

	// Token: 0x04000568 RID: 1384
	public FixedList512Bytes<quaternion> oldTargetRotations;

	// Token: 0x04000569 RID: 1385
	[ReadOnly]
	public FixedList512Bytes<quaternion> targetRotations;

	// Token: 0x0400056A RID: 1386
	[ReadOnly]
	public FixedList128Bytes<JointType> jointTypes;

	// Token: 0x0400056B RID: 1387
	public FixedList128Bytes<float> currentStaminas;

	// Token: 0x0400056C RID: 1388
	public FixedList128Bytes<float> currentStaminaMultipliers;

	// Token: 0x0400056D RID: 1389
	public FixedList64Bytes<bool> preventStaminaRegenList;

	// Token: 0x0400056E RID: 1390
	public FixedList512Bytes<BodyPartHealth> bodyPartHealths;

	// Token: 0x0400056F RID: 1391
	public FixedList512Bytes<BluntDamageInstance> bluntDamageInstances;

	// Token: 0x04000570 RID: 1392
	public bool dead;

	// Token: 0x04000571 RID: 1393
	public bool ai;
}
