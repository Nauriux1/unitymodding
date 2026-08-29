using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Utils;

// Token: 0x0200024C RID: 588
[Serializable]
public class BluntDamageDealer
{
	// Token: 0x0600111C RID: 4380
	public void Init(Rigidbody _rigidbody, Equipment _equipment, Transform _transform = null)
	{
		this.rb = _rigidbody;
		this.equipment = _equipment;
		if (this.equipment != null)
		{
			this.transform = this.equipment.transform;
		}
		else
		{
			this.transform = _transform;
		}
		if (this.centerOfMassLine != null && this.centerOfMassLine.Count > 0)
		{
			foreach (Transform transform in this.centerOfMassLine)
			{
				this.centerOfMassLineLocalPoints.Add(transform.localPosition);
			}
		}
		this.hitHistory = new FixedSizeQueue<BluntHitHistory>(3);
		this.velocityHistoryList = new FixedSizeQueue<HistoryVelocity>(50);
	}

	// Token: 0x0600111D RID: 4381
	public void UpdateHistory()
	{
		HistoryVelocity item = new HistoryVelocity
		{
			angularVelocity = this.rb.angularVelocity,
			velocity = this.rb.velocity,
			worldToLocalMatrix = this.rb.transform.worldToLocalMatrix
		};
		this.velocityHistoryList.Add(item);
		this.latestHistory = item;
	}

	// Token: 0x0600111E RID: 4382
	public float GetWeaponMass(Vector3 velocity, Vector3 contactPointLocal, float hitPointAlongWeight)
	{
		return 999f;
	}

	// Token: 0x0600111F RID: 4383
	private float GetDefaultMassForWeapon()
	{
		if (this.overrideMass)
		{
			return this.overrideMassToUse;
		}
		return this.rb.mass;
	}

	// Token: 0x06001120 RID: 4384
	private float CalculateTotalAddedBodyWeight(BluntDamageHandPositions handPositions, List<Hand> hands, Vector3 velocity, Vector3 contactPointLocal)
	{
		float y = this.CalculateBodyWeightFromTwoHandedHandPositions(handPositions, hands);
		return math.max(this.CalculateBodyWeightTowardsCollision(velocity, contactPointLocal, handPositions.localHandPosition), y);
	}

	// Token: 0x06001121 RID: 4385
	private float CalculateBodyWeightFromTwoHandedHandPositions(BluntDamageHandPositions handPositions, List<Hand> hands)
	{
		if (!handPositions.bothHandsOnWeapon)
		{
			return this.addedHandWeightMin;
		}
		float x = Vector3.Distance(hands[0].transform.position, hands[1].transform.position);
		return math.clamp(math.remap(this.minHandDistanceForBodyWeight, this.maxHandDistanceForBodyWeight, this.addedHandWeightMin, this.addedHandWeightMax, x), this.addedHandWeightMin, this.addedHandWeightMax);
	}

	// Token: 0x06001122 RID: 4386
	private float CalculateWeaponWeightFromHandPositions(BluntDamageHandPositions handPositions, float hitPointAlongWeight)
	{
		float num = 0f;
		float num2 = this.GetDefaultMassForWeapon() / 2f;
		float closestHandPosition = handPositions.closestHandPosition;
		float centerOfMassAlongTheWeightLine = this.GetCenterOfMassAlongTheWeightLine();
		float num3 = (hitPointAlongWeight + closestHandPosition) / 2f;
		bool flag = closestHandPosition > num3;
		if (closestHandPosition < -1f)
		{
			return 2f;
		}
		if (num3 < centerOfMassAlongTheWeightLine)
		{
			float num4 = num3;
			if (!flag)
			{
				num4 = Mathf.Abs(centerOfMassAlongTheWeightLine - num3);
			}
			num += num2 * (num4 / centerOfMassAlongTheWeightLine);
		}
		else if (flag)
		{
			num += num2;
		}
		if (centerOfMassAlongTheWeightLine < num3)
		{
			float num5 = 1f - centerOfMassAlongTheWeightLine;
			float num6 = num3 - centerOfMassAlongTheWeightLine;
			if (!flag)
			{
				num6 = Mathf.Abs(1f - num3);
			}
			num += num2 * (num6 / num5);
		}
		else if (!flag)
		{
			num += num2;
		}
		return num;
	}

	// Token: 0x06001123 RID: 4387
	private float GetCenterOfMassAlongTheWeightLine()
	{
		if (this.centerOfMassPositionOnTheWeightLine == null)
		{
			float value = 0f;
			this.ClosestCenterOfMassPoint(this.rb.centerOfMass, out value);
			this.centerOfMassPositionOnTheWeightLine = new float?(value);
		}
		return this.centerOfMassPositionOnTheWeightLine.Value;
	}

	// Token: 0x06001124 RID: 4388
	private BluntDamageHandPositions GetHandPositionsRelativeToWeightLine(List<Hand> grabbingHands, float hitPointAlongWeight)
	{
		BluntDamageHandPositions result = new BluntDamageHandPositions
		{
			hand1Position = 0f,
			hand2Position = 0f,
			bothHandsOnWeapon = false,
			closestHandPosition = 0f,
			hitBetweenHands = false
		};
		float num = 0f;
		Vector3 vector = this.transform.worldToLocalMatrix.MultiplyPoint3x4(grabbingHands[0].transform.position);
		this.ClosestCenterOfMassPoint(vector, out num);
		float num2 = Mathf.Abs(hitPointAlongWeight - num);
		result.hand1Position = num;
		result.closestHandPosition = num;
		result.localHandPosition = vector;
		if (grabbingHands.Count == 2)
		{
			float num3 = 0f;
			Vector3 vector2 = this.transform.worldToLocalMatrix.MultiplyPoint3x4(grabbingHands[1].transform.position);
			this.ClosestCenterOfMassPoint(vector2, out num3);
			if (hitPointAlongWeight >= Mathf.Min(num, num3) && hitPointAlongWeight <= Mathf.Max(num, num3))
			{
				result.hitBetweenHands = true;
			}
			result.bothHandsOnWeapon = true;
			result.hand2Position = num3;
			result.localHandPosition = (vector + vector2) / 2f;
			if (Mathf.Abs(hitPointAlongWeight - num) < num2)
			{
				result.closestHandPosition = num3;
			}
		}
		return result;
	}

	// Token: 0x06001125 RID: 4389
	private float CalculateBodyWeightTowardsCollision(Vector3 velocity, Vector3 contactPointLocal, Vector3 handPosition)
	{
		Vector3 normalized = (contactPointLocal - handPosition).normalized;
		return math.min(Vector3.Dot(velocity.normalized, normalized) * this.addedHandWeightMax, this.addedHandWeightMax);
	}

	// Token: 0x06001126 RID: 4390
	private float CalculateWeaponWeightWithoutWeightLine(float massToUse, Vector3 velocity, Vector3 contactPointLocal)
	{
		Vector3 normalized = (contactPointLocal - this.rb.centerOfMass).normalized;
		return Vector3.Dot(velocity.normalized, normalized) * massToUse;
	}

	// Token: 0x06001127 RID: 4391
	public HistoryVelocity GetHistoryVelocity()
	{
		return this.latestHistory;
	}

	// Token: 0x06001128 RID: 4392
	public Vector3 CalculateLocalPointVelocityFromLatestHistory(Vector3 localPoint)
	{
		Vector3 rhs = localPoint - this.rb.centerOfMass;
		HistoryVelocity historyVelocity = this.latestHistory;
		Vector3 localVelocity = historyVelocity.GetLocalVelocity();
		Vector3 b = Vector3.Cross(historyVelocity.GetLocalAngularVelocity(), rhs);
		return localVelocity + b;
	}

	// Token: 0x06001129 RID: 4393
	public Vector3 ClosestCenterOfMassPoint(Vector3 localSpacePoint, out float t)
	{
		t = 0f;
		if (this.centerOfMassLineLocalPoints != null && this.centerOfMassLineLocalPoints.Count == 2)
		{
			return Generic.GetClosestPointOnLine(this.centerOfMassLineLocalPoints[0], this.centerOfMassLineLocalPoints[1], localSpacePoint, true, out t);
		}
		return this.rb.centerOfMass;
	}

	// Token: 0x0600112A RID: 4394
	public bool CanBeHit(WeaponDamageableBodyPart weaponDamageableBodyPart)
	{
		float num = Time.time - 0.25f;
		for (int i = 0; i < this.hitHistory.Count; i++)
		{
			BluntHitHistory bluntHitHistory = this.hitHistory.Get(i);
			if (bluntHitHistory.weaponDamageableBodyPart == weaponDamageableBodyPart && bluntHitHistory.hitTime > num)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600112B RID: 4395
	public void AddToHitHistory(WeaponDamageableBodyPart weaponDamageableBodyPart)
	{
		BluntHitHistory item = new BluntHitHistory
		{
			weaponDamageableBodyPart = weaponDamageableBodyPart,
			hitTime = Time.time
		};
		this.hitHistory.Add(item);
	}

	// Token: 0x0600112C RID: 4396
	public BluntDamageTypeValues GetBluntDamageTypeValues()
	{
		if (!this.bluntDamageTypeValuesFetched)
		{
			this.bluntDamageTypeValues = BluntDamageHelpers.GetBluntDamageTypeValues(this.bluntDamageType);
			this.bluntDamageTypeValuesFetched = true;
		}
		return this.bluntDamageTypeValues;
	}

	// Token: 0x0600112D RID: 4397
	public float GetPermanentDamageMultiplier(bool armourHit)
	{
		return 50f;
	}

	// Token: 0x0600112E RID: 4398
	public float GetTemporaryDamageMultiplier(bool armourHit)
	{
		return 50f;
	}

	// Token: 0x0600112F RID: 4399
	public float GetTemporaryMaxDamage(bool armourHit)
	{
		return 999f;
	}

	// Token: 0x06001130 RID: 4400
	public float GetPermanentMaxDamage(bool armourHit)
	{
		return 999f;
	}

	// Token: 0x06001131 RID: 4401
	public float GetTemporaryResistancePenetration()
	{
		return this.GetBluntDamageTypeValues().temporaryDamageResistancePenetration;
	}

	// Token: 0x06001132 RID: 4402
	public float GetPermanentResistancePenetration()
	{
		return this.GetBluntDamageTypeValues().permanentDamageResistancePenetration;
	}

	// Token: 0x06001133 RID: 4403
	public float CalculateHistoryMultiplier(Vector3 localContactPoint, Vector3 comToContact)
	{
		return 1f;
	}

	// Token: 0x06001134 RID: 4404
	public float CalculateLocalPointVelocityFromHistory(Vector3 localPoint, Vector3 comToContact)
	{
		float fixedDeltaTime = Time.fixedDeltaTime;
		Vector3 rhs = localPoint - this.rb.centerOfMass;
		float num = 0f;
		for (int i = this.velocityHistoryList.Count - 1; i > -1; i--)
		{
			HistoryVelocity historyVelocity = this.velocityHistoryList.Get(i);
			Vector3 localVelocity = historyVelocity.GetLocalVelocity();
			Vector3 b = Vector3.Cross(historyVelocity.GetLocalAngularVelocity(), rhs);
			float num2 = Vector3.Dot(localVelocity + b, comToContact);
			if (num2 <= 0f)
			{
				break;
			}
			num += num2 * fixedDeltaTime;
			if (num > this.maxDistanceTravelled)
			{
				break;
			}
		}
		return num;
	}

	// Token: 0x06001135 RID: 4405
	public bool IsHeld()
	{
		return this.equipment != null && this.equipment.EquipmentIsHeld();
	}

	// Token: 0x06001136 RID: 4406
	public List<Hand> GetGrabbingHands()
	{
		if (this.equipment != null)
		{
			return this.equipment.GetGrabbingHands();
		}
		return null;
	}

	// Token: 0x04000CCA RID: 3274
	public Equipment equipment;

	// Token: 0x04000CCB RID: 3275
	public Rigidbody rb;

	// Token: 0x04000CCC RID: 3276
	public BluntDamageType bluntDamageType;

	// Token: 0x04000CCD RID: 3277
	public List<BluntDamageDealerCollider> bluntDamageDealerColliders = new List<BluntDamageDealerCollider>();

	// Token: 0x04000CCE RID: 3278
	public List<Transform> centerOfMassLine;

	// Token: 0x04000CCF RID: 3279
	public List<Vector3> centerOfMassLineLocalPoints;

	// Token: 0x04000CD0 RID: 3280
	private FixedSizeQueue<BluntHitHistory> hitHistory;

	// Token: 0x04000CD1 RID: 3281
	private FixedSizeQueue<HistoryVelocity> velocityHistoryList;

	// Token: 0x04000CD2 RID: 3282
	private HistoryVelocity latestHistory;

	// Token: 0x04000CD3 RID: 3283
	public Transform transform;

	// Token: 0x04000CD4 RID: 3284
	private float maxMassForBluntDamageCalculations = 3.5f;

	// Token: 0x04000CD5 RID: 3285
	public bool overrideMass;

	// Token: 0x04000CD6 RID: 3286
	public float overrideMassToUse = 1f;

	// Token: 0x04000CD7 RID: 3287
	private float addedHandWeightMin = 0.25f;

	// Token: 0x04000CD8 RID: 3288
	private float addedHandWeightMax = 2.5f;

	// Token: 0x04000CD9 RID: 3289
	private float minHandDistanceForBodyWeight = 0.15f;

	// Token: 0x04000CDA RID: 3290
	private float maxHandDistanceForBodyWeight = 1f;

	// Token: 0x04000CDB RID: 3291
	private float? centerOfMassPositionOnTheWeightLine;

	// Token: 0x04000CDC RID: 3292
	private bool bluntDamageTypeValuesFetched;

	// Token: 0x04000CDD RID: 3293
	private BluntDamageTypeValues bluntDamageTypeValues;

	// Token: 0x04000CDE RID: 3294
	private float minRequiredDistanceTravelled = 0.04f;

	// Token: 0x04000CDF RID: 3295
	private float maxDistanceTravelled = 0.5f;
}
