using System;
using MoveClasses;
using Unity.Burst;
using Unity.Mathematics;

// Token: 0x02000034 RID: 52
[BurstCompile]
public struct BodyPartHealth
{
	// Token: 0x060001A7 RID: 423 RVA: 0x00009E22 File Offset: 0x00008022
	public float StrengthMultiplier()
	{
		return this.TemporaryStrengthMultiplier() * this.PermanentStrengthMultiplier();
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x00009E31 File Offset: 0x00008031
	public float PermanentStrengthMultiplier()
	{
		return math.remap(0f, 1f, 0.5f, 1f, math.clamp(this.permanentHealth, 0f, 1f));
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x00009E61 File Offset: 0x00008061
	public float TemporaryStrengthMultiplier()
	{
		return math.remap(this.lowestTemporaryHealth, 1f, 0f, 1f, this.temporaryHealth);
	}

	// Token: 0x040000EB RID: 235
	public JointType bodyPart;

	// Token: 0x040000EC RID: 236
	public float permanentHealth;

	// Token: 0x040000ED RID: 237
	public float temporaryHealth;

	// Token: 0x040000EE RID: 238
	public float lowestTemporaryHealth;
}
