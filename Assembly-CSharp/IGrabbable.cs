using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000052 RID: 82
public interface IGrabbable
{
	// Token: 0x06000230 RID: 560
	Weapon GetWeapon();

	// Token: 0x06000231 RID: 561
	Rigidbody GetRigidbody();

	// Token: 0x06000232 RID: 562
	Vector3? GetHoldPosition(Vector3 currentHandPosition, Hand hand = null, bool force = false, float? equipmentStartHoldPosition = null);

	// Token: 0x06000233 RID: 563
	Transform GetHoldTransform();

	// Token: 0x06000234 RID: 564
	void SetGrabbingHand(Hand hand);

	// Token: 0x06000235 RID: 565
	void RemoveGrabbingHand(Hand hand);

	// Token: 0x06000236 RID: 566
	Quaternion GetStartRotation();

	// Token: 0x06000237 RID: 567
	Quaternion GetStartRotationGlobal();

	// Token: 0x06000238 RID: 568
	Vector3 GetPhysicalHoldRotation();

	// Token: 0x06000239 RID: 569
	List<Hand> GetGrabbingHands();

	// Token: 0x0600023A RID: 570
	List<Collider> GetHandleColliders();

	// Token: 0x1700008B RID: 139
	// (get) Token: 0x0600023B RID: 571
	bool IsTwoHanded { get; }

	// Token: 0x1700008C RID: 140
	// (get) Token: 0x0600023C RID: 572
	float handleLength { get; }

	// Token: 0x0600023D RID: 573
	Vector3 GetHandlePosition();

	// Token: 0x0600023E RID: 574
	void CheckAsleep();
}
