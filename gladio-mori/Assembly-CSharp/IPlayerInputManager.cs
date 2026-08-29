using System;

// Token: 0x02000056 RID: 86
public interface IPlayerInputManager
{
	// Token: 0x0600024A RID: 586
	void HandlePlayerDeath();

	// Token: 0x0600024B RID: 587
	AttackDirection GetAttackDirection();

	// Token: 0x0600024C RID: 588
	void UpdateTargetRotation();

	// Token: 0x1700008F RID: 143
	// (get) Token: 0x0600024D RID: 589
	bool targetRotationInUse { get; }

	// Token: 0x17000090 RID: 144
	// (get) Token: 0x0600024E RID: 590
	float lastMouseActionTime { get; }
}
