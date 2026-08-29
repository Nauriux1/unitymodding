using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000250 RID: 592
public class Handle : MonoBehaviour, IGrabbable
{
	// Token: 0x170001BA RID: 442
	// (get) Token: 0x06001156 RID: 4438 RVA: 0x00058FA7 File Offset: 0x000571A7
	public bool IsTwoHanded
	{
		get
		{
			return this._isTwoHanded;
		}
	}

	// Token: 0x170001BB RID: 443
	// (get) Token: 0x06001157 RID: 4439 RVA: 0x00058FB0 File Offset: 0x000571B0
	public float handleLength
	{
		get
		{
			if (this._handleLength <= 0f)
			{
				CapsuleCollider component = base.gameObject.GetComponent<CapsuleCollider>();
				this._handleLength = component.height / 2f - this.removeFromHandleLengthForHandsToFit;
			}
			return this._handleLength;
		}
	}

	// Token: 0x06001158 RID: 4440 RVA: 0x00058FF5 File Offset: 0x000571F5
	public Rigidbody GetRigidbody()
	{
		return this.weaponRigidbody;
	}

	// Token: 0x06001159 RID: 4441 RVA: 0x00059000 File Offset: 0x00057200
	public List<Collider> GetHandleColliders()
	{
		if (this.handleColliders.Count == 0)
		{
			Collider component = base.gameObject.GetComponent<Collider>();
			if (component != null)
			{
				this.handleColliders.Add(component);
			}
		}
		return this.handleColliders;
	}

	// Token: 0x0600115A RID: 4442 RVA: 0x00059044 File Offset: 0x00057244
	public Vector3? GetHoldPosition(Vector3 currentHandPosition, Hand hand = null, bool force = false, float? equipmentStartHoldPosition = null)
	{
		if (this.holdPosition != null && equipmentStartHoldPosition != null)
		{
			Vector3 localPosition = this.holdPosition.transform.localPosition;
			localPosition.z += equipmentStartHoldPosition.Value;
			Vector3 position = base.transform.parent.TransformPoint(localPosition);
			Vector3 handPosition = base.transform.InverseTransformPoint(position);
			return this.GetClosestPositionOnHandle(handPosition, hand, false);
		}
		Vector3 handPosition2 = base.transform.InverseTransformPoint(currentHandPosition);
		return this.GetClosestPositionOnHandle(handPosition2, hand, force);
	}

	// Token: 0x0600115B RID: 4443 RVA: 0x000590CC File Offset: 0x000572CC
	private Vector3? GetClosestPositionOnHandle(Vector3 handPosition, Hand hand = null, bool force = false)
	{
		handPosition.x = 0f;
		handPosition.y = 0f;
		float? otherHandPosition = null;
		if (this.grabbingHands.Count > 0)
		{
			for (int i = 0; i < this.grabbingHands.Count; i++)
			{
				Hand hand2 = this.grabbingHands[i];
				if (hand2 != hand)
				{
					Vector3 vector = base.transform.InverseTransformPoint(hand2.spawnPosition.position);
					otherHandPosition = new float?(vector.z);
				}
			}
		}
		float handPositionUpperLimit = this.GetHandPositionUpperLimit(handPosition.z, otherHandPosition);
		float handPositionLowerLimit = this.GetHandPositionLowerLimit(handPosition.z, otherHandPosition);
		if (Mathf.Abs(handPositionUpperLimit - handPositionLowerLimit) < this.removeFromHandleLengthForHandsToFit * 2f || handPositionUpperLimit < handPositionLowerLimit)
		{
			if (!force)
			{
				return null;
			}
		}
		else
		{
			float num = Mathf.Abs(handPositionUpperLimit - handPosition.z);
			float num2 = Mathf.Abs(handPositionLowerLimit - handPosition.z);
			bool flag = num < num2;
			if ((flag ? num : num2) < this.removeFromHandleLengthForHandsToFit || handPosition.z > handPositionUpperLimit || handPosition.z < handPositionLowerLimit)
			{
				if (flag)
				{
					handPosition.z = handPositionUpperLimit - this.removeFromHandleLengthForHandsToFit;
				}
				else
				{
					handPosition.z = handPositionLowerLimit + this.removeFromHandleLengthForHandsToFit;
				}
			}
		}
		Vector3 position = base.transform.TransformPoint(handPosition);
		return new Vector3?(base.transform.parent.InverseTransformPoint(position));
	}

	// Token: 0x0600115C RID: 4444 RVA: 0x0005923C File Offset: 0x0005743C
	private float GetHandPositionUpperLimit(float newHandPosition, float? otherHandPosition)
	{
		float num = this.handleLength + this.removeFromHandleLengthForHandsToFit;
		if (otherHandPosition != null)
		{
			float? num2 = otherHandPosition;
			float num3 = num;
			if (num2.GetValueOrDefault() < num3 & num2 != null)
			{
				num2 = otherHandPosition;
				if (num2.GetValueOrDefault() > newHandPosition & num2 != null)
				{
					num = otherHandPosition.Value - this.removeFromHandleLengthForHandsToFit;
				}
			}
		}
		foreach (float num4 in this.nonGrabbableHandlePositions)
		{
			if (num4 < num && num4 > newHandPosition)
			{
				num = num4;
			}
		}
		return num;
	}

	// Token: 0x0600115D RID: 4445 RVA: 0x000592F0 File Offset: 0x000574F0
	private float GetHandPositionLowerLimit(float newHandPosition, float? otherHandPosition)
	{
		float num = (this.handleLength + this.removeFromHandleLengthForHandsToFit) * -1f;
		if (otherHandPosition != null)
		{
			float? num2 = otherHandPosition;
			float num3 = num;
			if (num2.GetValueOrDefault() > num3 & num2 != null)
			{
				num2 = otherHandPosition;
				if (num2.GetValueOrDefault() < newHandPosition & num2 != null)
				{
					num = otherHandPosition.Value + this.removeFromHandleLengthForHandsToFit;
				}
			}
		}
		foreach (float num4 in this.nonGrabbableHandlePositions)
		{
			if (num4 > num && num4 < newHandPosition)
			{
				num = num4;
			}
		}
		return num;
	}

	// Token: 0x0600115E RID: 4446 RVA: 0x000593A8 File Offset: 0x000575A8
	public Vector3 GetHandlePosition()
	{
		return base.gameObject.transform.localPosition;
	}

	// Token: 0x0600115F RID: 4447 RVA: 0x000593BA File Offset: 0x000575BA
	public void SetGrabbingHand(Hand hand)
	{
		this.grabbingHands.Add(hand);
		this.GetEquipment().SetPlayerHealth(hand.playerHealth);
	}

	// Token: 0x06001160 RID: 4448 RVA: 0x000593D9 File Offset: 0x000575D9
	public void RemoveGrabbingHand(Hand hand)
	{
		this.grabbingHands.Remove(hand);
	}

	// Token: 0x06001161 RID: 4449 RVA: 0x000593E8 File Offset: 0x000575E8
	public void CheckAsleep()
	{
		if (this.weapon != null)
		{
			this.weapon.CheckAsleep();
		}
	}

	// Token: 0x06001162 RID: 4450 RVA: 0x00059403 File Offset: 0x00057603
	public Quaternion GetStartRotation()
	{
		if (this.startRotation != null)
		{
			return this.startRotation.Value;
		}
		return base.transform.localRotation;
	}

	// Token: 0x06001163 RID: 4451 RVA: 0x00059429 File Offset: 0x00057629
	public Quaternion GetStartRotationGlobal()
	{
		if (this.startRotationGlobal != null)
		{
			return this.startRotationGlobal.Value;
		}
		return base.transform.rotation;
	}

	// Token: 0x06001164 RID: 4452 RVA: 0x0005944F File Offset: 0x0005764F
	private void Awake()
	{
		this.FindWeaponComponent();
		this.FindEquipmentComponent();
		this.startRotation = new Quaternion?(base.transform.localRotation);
		this.startRotationGlobal = new Quaternion?(base.transform.rotation);
		this.InitNonGrabbableHandlePositions();
	}

	// Token: 0x06001165 RID: 4453 RVA: 0x00059490 File Offset: 0x00057690
	private void InitNonGrabbableHandlePositions()
	{
		foreach (Transform transform in this.nonGrabbableTransforms)
		{
			Vector3 vector = base.transform.InverseTransformPoint(transform.position);
			this.nonGrabbableHandlePositions.Add(vector.z);
		}
	}

	// Token: 0x06001166 RID: 4454 RVA: 0x00059500 File Offset: 0x00057700
	public Transform GetHoldTransform()
	{
		return this.holdPosition;
	}

	// Token: 0x06001167 RID: 4455 RVA: 0x00059508 File Offset: 0x00057708
	public Vector3 GetPhysicalHoldRotation()
	{
		Vector3 vector = this.physicalHoldRotation;
		return this.physicalHoldRotation;
	}

	// Token: 0x06001168 RID: 4456 RVA: 0x00059522 File Offset: 0x00057722
	public List<Hand> GetGrabbingHands()
	{
		return this.grabbingHands;
	}

	// Token: 0x06001169 RID: 4457 RVA: 0x0005952C File Offset: 0x0005772C
	public float StartHoldPositionLimit(bool lowerLimit = false)
	{
		float num = this.handleLength;
		if (lowerLimit)
		{
			num *= -1f;
		}
		Vector3 localPosition = this.holdPosition.transform.localPosition;
		Vector3 position = base.transform.parent.TransformPoint(localPosition);
		Vector3 vector = base.transform.InverseTransformPoint(position);
		return num - vector.z;
	}

	// Token: 0x0600116A RID: 4458 RVA: 0x00059585 File Offset: 0x00057785
	private void FindWeaponComponent()
	{
		if (this.weapon == null)
		{
			this.weapon = this.weaponRigidbody.gameObject.GetComponent<Weapon>();
		}
	}

	// Token: 0x0600116B RID: 4459 RVA: 0x000595AB File Offset: 0x000577AB
	public Weapon GetWeapon()
	{
		this.FindWeaponComponent();
		return this.weapon;
	}

	// Token: 0x0600116C RID: 4460 RVA: 0x000595BC File Offset: 0x000577BC
	private void FindEquipmentComponent()
	{
		if (this.equipment == null)
		{
			this.equipment = this.weaponRigidbody.gameObject.GetComponent<Equipment>();
		}
		if (this.equipment != null && this.equipment.handle == null)
		{
			this.equipment.handle = this;
		}
	}

	// Token: 0x0600116D RID: 4461 RVA: 0x0005961A File Offset: 0x0005781A
	public Equipment GetEquipment()
	{
		this.FindEquipmentComponent();
		return this.equipment;
	}

	// Token: 0x0600116E RID: 4462 RVA: 0x00059628 File Offset: 0x00057828
	public bool EquipmentIsHeld()
	{
		return this.grabbingHands != null && this.grabbingHands.Count > 0;
	}

	// Token: 0x04000CFB RID: 3323
	public Weapon weapon;

	// Token: 0x04000CFC RID: 3324
	public Equipment equipment;

	// Token: 0x04000CFD RID: 3325
	public Rigidbody weaponRigidbody;

	// Token: 0x04000CFE RID: 3326
	public Transform holdPosition;

	// Token: 0x04000CFF RID: 3327
	public List<Hand> grabbingHands = new List<Hand>();

	// Token: 0x04000D00 RID: 3328
	public Quaternion? startRotation;

	// Token: 0x04000D01 RID: 3329
	public Quaternion? startRotationGlobal;

	// Token: 0x04000D02 RID: 3330
	public Vector3 physicalHoldRotation;

	// Token: 0x04000D03 RID: 3331
	public List<Collider> handleColliders = new List<Collider>();

	// Token: 0x04000D04 RID: 3332
	public List<Transform> nonGrabbableTransforms = new List<Transform>();

	// Token: 0x04000D05 RID: 3333
	public List<float> nonGrabbableHandlePositions = new List<float>();

	// Token: 0x04000D06 RID: 3334
	public bool _isTwoHanded;

	// Token: 0x04000D07 RID: 3335
	private float _handleLength;

	// Token: 0x04000D08 RID: 3336
	private float removeFromHandleLengthForHandsToFit = 0.07f;
}
