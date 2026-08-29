using System;
using System.Collections.Generic;
using MoveClasses;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;

// Token: 0x0200002E RID: 46
public static class BluntDamageHelpers
{
	// Token: 0x06000187 RID: 391 RVA: 0x00008E94 File Offset: 0x00007094
	public static void HandleBluntDamage(Collision collision, List<ContactPoint> contacts, WeaponDamageableBodyPart weaponDamageableBodyPart, IBluntDamageDealer damageDealer)
	{
		BluntDamageDealer bluntDamageDealer = damageDealer.GetBluntDamageDealer();
		if (weaponDamageableBodyPart.player == damageDealer.GetPlayerHealth())
		{
			return;
		}
		if (!bluntDamageDealer.CanBeHit(weaponDamageableBodyPart))
		{
			return;
		}
		Vector3 vector = BluntDamageHelpers.FindContactPoint(contacts);
		damageDealer.GetRigidbody();
		Vector3 contactPointLocal = bluntDamageDealer.GetHistoryVelocity().worldToLocalMatrix.MultiplyPoint3x4(vector);
		Vector3 directionMultiplier;
		float num = BluntDamageHelpers.CalculateKineticEnergyMultiplier(bluntDamageDealer, contactPointLocal, out directionMultiplier);
		if (num > 0f)
		{
			float volume = 0f;
			bool armourHit = BluntDamageHelpers.IsArmourHit(collision);
			float temporaryDamage = BluntDamageHelpers.GetTemporaryDamage(bluntDamageDealer, weaponDamageableBodyPart, num, armourHit);
			float permanentDamage = BluntDamageHelpers.GetPermanentDamage(bluntDamageDealer, weaponDamageableBodyPart, num, armourHit);
			bool bloodied = false;
			if (weaponDamageableBodyPart.player != null)
			{
				BodyPartHealth bodyPartHealthByBodyPart = weaponDamageableBodyPart.player.GetBodyPartHealthByBodyPart(weaponDamageableBodyPart.bodyPart);
				bodyPartHealthByBodyPart.permanentHealth -= permanentDamage;
				BluntDamageInstance bluntDamageInstance = default(BluntDamageInstance);
				bluntDamageInstance.bodyPart = weaponDamageableBodyPart.bodyPart;
				bluntDamageInstance.temporaryDamage = temporaryDamage;
				bluntDamageInstance.permanentDamage = permanentDamage;
				weaponDamageableBodyPart.player.AddBluntDamageInstance(bluntDamageInstance);
				weaponDamageableBodyPart.player.SetBodyPartHealthByBodyPart(bodyPartHealthByBodyPart);
				if (bodyPartHealthByBodyPart.permanentHealth <= 0f)
				{
					BluntDamageHelpers.HandleBodyPartDestroyed(weaponDamageableBodyPart, vector);
				}
				if (bodyPartHealthByBodyPart.permanentHealth <= 0.5f)
				{
					bloodied = true;
				}
			}
			if (temporaryDamage > 0.0001f)
			{
				volume = Math.Min(num, 1f);
			}
			BluntDamageHelpers.HandleBluntDamageEffects(weaponDamageableBodyPart, vector, permanentDamage, bloodied, volume);
			bluntDamageDealer.AddToHitHistory(weaponDamageableBodyPart);
			BluntDamageHelpers.HandlePhysicsForce(weaponDamageableBodyPart, vector, directionMultiplier);
		}
	}

	// Token: 0x06000188 RID: 392 RVA: 0x00008FFC File Offset: 0x000071FC
	public static void HandleBluntDamageEffects(WeaponDamageableBodyPart weaponDamageableBodyPart, Vector3 contactPointWorld, float damage, bool bloodied, float volume)
	{
		float num = 0f;
		if (bloodied)
		{
			num = damage * 0.5f;
		}
		BluntDamageEffect bluntDamageEffect = new BluntDamageEffect
		{
			BodyPart = weaponDamageableBodyPart.bodyPart,
			Damage = damage,
			Position = weaponDamageableBodyPart.transform.worldToLocalMatrix.MultiplyPoint3x4(contactPointWorld),
			BloodDamage = num,
			Volume = volume
		};
		if (weaponDamageableBodyPart.player != null && weaponDamageableBodyPart.player.playerHealthMultiplayer != null)
		{
			weaponDamageableBodyPart.player.playerHealthMultiplayer.BluntHitServer(bluntDamageEffect);
		}
		BluntDamageHelpers.RecordBluntDamageEffect(weaponDamageableBodyPart, bluntDamageEffect);
		BluntDamageHelpers.HandleBluntDamagePainting(weaponDamageableBodyPart, contactPointWorld, damage);
		if (num > BluntDamageHelpers.bloodDamagePaintThreshold)
		{
			BluntDamageHelpers.HandleBluntDamageBloodPainting(weaponDamageableBodyPart, contactPointWorld, num);
		}
		BluntDamageHelpers.HandleBluntDamageSound(contactPointWorld, volume, bloodied);
	}

	// Token: 0x06000189 RID: 393 RVA: 0x000090BF File Offset: 0x000072BF
	public static void RecordBluntDamageEffect(WeaponDamageableBodyPart weaponDamageableBodyPart, BluntDamageEffect bluntDamageEffect)
	{
		if (ReplayManager.singleton != null && weaponDamageableBodyPart.player != null)
		{
			ReplayManager.singleton.RecordBluntDamage(weaponDamageableBodyPart.player.gameObject, bluntDamageEffect);
		}
	}

	// Token: 0x0600018A RID: 394 RVA: 0x000090F4 File Offset: 0x000072F4
	public static void HandleBluntDamageEffects(PlayerHealth playerHealth, BluntDamageEffect bluntDamageEffect)
	{
		WeaponDamageableBodyPart weaponDamageableBodyPart = playerHealth.weaponDamageableBodyParts[(int)bluntDamageEffect.BodyPart];
		if (weaponDamageableBodyPart != null)
		{
			Vector3 vector = weaponDamageableBodyPart.transform.localToWorldMatrix.MultiplyPoint3x4(bluntDamageEffect.Position);
			bool bloodied = false;
			Vector4 contactPoint = vector;
			contactPoint.w = bluntDamageEffect.Damage;
			BluntDamageHelpers.HandleBluntDamagePainting(weaponDamageableBodyPart, contactPoint);
			if (bluntDamageEffect.BloodDamage > BluntDamageHelpers.bloodDamagePaintThreshold)
			{
				contactPoint.w = bluntDamageEffect.BloodDamage;
				BluntDamageHelpers.HandleBluntDamageBloodPainting(weaponDamageableBodyPart, contactPoint);
				bloodied = true;
			}
			BluntDamageHelpers.HandleBluntDamageSound(vector, bluntDamageEffect.Volume, bloodied);
		}
	}

	// Token: 0x0600018B RID: 395 RVA: 0x00009180 File Offset: 0x00007380
	public static void HandleBluntDamagePainting(WeaponDamageableBodyPart weaponDamageableBodyPart, Vector3 contactPoint, float damage)
	{
		Vector4 contactPoint2 = contactPoint;
		contactPoint2.w = damage;
		BluntDamageHelpers.HandleBluntDamagePainting(weaponDamageableBodyPart, contactPoint2);
	}

	// Token: 0x0600018C RID: 396 RVA: 0x000091A4 File Offset: 0x000073A4
	public static void HandleBluntDamagePainting(WeaponDamageableBodyPart weaponDamageableBodyPart, Vector4 contactPoint)
	{
		for (int i = 0; i < weaponDamageableBodyPart.bladePaintables.Count; i++)
		{
			weaponDamageableBodyPart.bladePaintables[i].AddDrawableSphere(contactPoint);
		}
	}

	// Token: 0x0600018D RID: 397 RVA: 0x000091DC File Offset: 0x000073DC
	public static void HandleBluntDamageBloodPainting(WeaponDamageableBodyPart weaponDamageableBodyPart, Vector3 contactPoint, float bloodDamage)
	{
		Vector4 contactPoint2 = contactPoint;
		contactPoint2.w = bloodDamage;
		BluntDamageHelpers.HandleBluntDamageBloodPainting(weaponDamageableBodyPart, contactPoint2);
	}

	// Token: 0x0600018E RID: 398 RVA: 0x00009200 File Offset: 0x00007400
	public static void HandleBluntDamageBloodPainting(WeaponDamageableBodyPart weaponDamageableBodyPart, Vector4 contactPoint)
	{
		for (int i = 0; i < weaponDamageableBodyPart.bladePaintables.Count; i++)
		{
			weaponDamageableBodyPart.bladePaintables[i].AddDrawableSphereGreen(contactPoint);
		}
	}

	// Token: 0x0600018F RID: 399 RVA: 0x00009238 File Offset: 0x00007438
	public static void HandleBluntDamageSound(Vector3 worldPosition, float volume, bool bloodied)
	{
		if (SoundManager.singleton == null)
		{
			return;
		}
		GeneralSoundType generalSoundType = GeneralSoundType.BluntHit;
		if (bloodied)
		{
			generalSoundType = GeneralSoundType.BluntHitWet;
		}
		SoundManager.singleton.PlayGeneralSound(worldPosition, volume, generalSoundType);
	}

	// Token: 0x06000190 RID: 400 RVA: 0x00009268 File Offset: 0x00007468
	public static void HandleBodyPartDestroyed(WeaponDamageableBodyPart weaponDamageableBodyPart, Vector3 contactPointWorld)
	{
		if (weaponDamageableBodyPart.disableLocalLogic)
		{
			return;
		}
		float num = 100000f;
		WeaponDamageablePart weaponDamageablePart = null;
		WeaponDamageablePart weaponDamageablePart2 = null;
		for (int i = 0; i < weaponDamageableBodyPart.childWeaponDamageableParts.Count; i++)
		{
			WeaponDamageablePart weaponDamageablePart3 = weaponDamageableBodyPart.childWeaponDamageableParts[i];
			if (weaponDamageablePart3.isBone && !weaponDamageablePart3.lethal)
			{
				weaponDamageablePart3.Destory(null, true);
			}
			else if (weaponDamageablePart3.isMuscle)
			{
				weaponDamageablePart3.Destory(null, false);
			}
			else if (weaponDamageablePart3.bloodVessel)
			{
				weaponDamageablePart2 = weaponDamageablePart3;
			}
			else if (weaponDamageablePart3.lethal)
			{
				float num2 = Vector3.Distance(contactPointWorld, weaponDamageablePart3.transform.position);
				if (weaponDamageablePart == null || num2 < num)
				{
					weaponDamageablePart = weaponDamageablePart3;
					num = num2;
				}
			}
		}
		if (weaponDamageablePart != null)
		{
			weaponDamageablePart.Destory(null, true);
			return;
		}
		if (weaponDamageablePart2 != null)
		{
			weaponDamageablePart2.Destory(null, true);
		}
	}

	// Token: 0x06000191 RID: 401 RVA: 0x0000936C File Offset: 0x0000756C
	public static Vector3 FindContactPoint(List<ContactPoint> contacts)
	{
		return contacts[0].point;
	}

	// Token: 0x06000192 RID: 402 RVA: 0x00009388 File Offset: 0x00007588
	public static FixedList512Bytes<BodyPartHealth> GetNewBodyPartHealthArray()
	{
		FixedList512Bytes<BodyPartHealth> result = default(FixedList512Bytes<BodyPartHealth>);
		int num = Enum.GetNames(typeof(JointType)).Length;
		for (int i = 0; i < num; i++)
		{
			BodyPartHealth bodyPartHealth = new BodyPartHealth
			{
				bodyPart = (JointType)i,
				permanentHealth = 1f,
				temporaryHealth = 1f
			};
			result.Add(bodyPartHealth);
		}
		return result;
	}

	// Token: 0x06000193 RID: 403 RVA: 0x000093F0 File Offset: 0x000075F0
	[BurstCompile]
	public static float RegenRateForBodyPart(JointType bodyPart)
	{
		return 0.1f;
	}

	// Token: 0x06000194 RID: 404 RVA: 0x000093F8 File Offset: 0x000075F8
	[BurstCompile]
	public static bool BodyPartAffectsJoint(JointType bodyPart, JointType jointType)
	{
		if (bodyPart == JointType.NECK)
		{
			return true;
		}
		if (jointType == JointType.HIP)
		{
			return false;
		}
		switch (bodyPart)
		{
		case JointType.SPINE1:
			if (jointType == JointType.SPINE1 || jointType == JointType.HIP_JOINT_LEFT || jointType == JointType.HIP_JOINT_RIGHT)
			{
				return true;
			}
			break;
		case JointType.SPINE2:
			if (jointType == JointType.SPINE2 || jointType == JointType.NECK || jointType == JointType.SCAPULA_LEFT || jointType == JointType.SCAPULA_RIGHT)
			{
				return true;
			}
			break;
		case JointType.SCAPULA_RIGHT:
			if (jointType == JointType.SPINE2 || jointType == JointType.SCAPULA_LEFT || jointType == JointType.SCAPULA_RIGHT)
			{
				return true;
			}
			break;
		case JointType.SCAPULA_LEFT:
			if (jointType == JointType.SPINE2 || jointType == JointType.SCAPULA_LEFT || jointType == JointType.SCAPULA_RIGHT)
			{
				return true;
			}
			break;
		case JointType.SHOULDER_RIGHT:
			if (jointType == JointType.ELBOW_RIGHT || jointType == JointType.SHOULDER_RIGHT || jointType == JointType.SCAPULA_RIGHT)
			{
				return true;
			}
			break;
		case JointType.SHOULDER_LEFT:
			if (jointType == JointType.ELBOW_LEFT || jointType == JointType.SHOULDER_LEFT || jointType == JointType.SCAPULA_LEFT)
			{
				return true;
			}
			break;
		case JointType.ELBOW_RIGHT:
			if (jointType == JointType.ELBOW_RIGHT || jointType == JointType.WRIST_RIGHT)
			{
				return true;
			}
			break;
		case JointType.ELBOW_LEFT:
			if (jointType == JointType.ELBOW_LEFT || jointType == JointType.WRIST_LEFT)
			{
				return true;
			}
			break;
		case JointType.HIP_JOINT_RIGHT:
			if (jointType == JointType.HIP_JOINT_RIGHT || jointType == JointType.KNEE_RIGHT)
			{
				return true;
			}
			break;
		case JointType.HIP_JOINT_LEFT:
			if (jointType == JointType.HIP_JOINT_LEFT || jointType == JointType.KNEE_LEFT)
			{
				return true;
			}
			break;
		case JointType.KNEE_RIGHT:
			if (jointType == JointType.HIP_JOINT_RIGHT || jointType == JointType.KNEE_RIGHT)
			{
				return true;
			}
			break;
		case JointType.KNEE_LEFT:
			if (jointType == JointType.HIP_JOINT_LEFT || jointType == JointType.KNEE_LEFT)
			{
				return true;
			}
			break;
		}
		return false;
	}

	// Token: 0x06000195 RID: 405 RVA: 0x00009504 File Offset: 0x00007704
	public static float CalculateKineticEnergyMultiplier(BluntDamageDealer bluntDamageDealer, Vector3 contactPointLocal, out Vector3 collisionDirection)
	{
		float t = 0f;
		Vector3 vector = bluntDamageDealer.ClosestCenterOfMassPoint(contactPointLocal, out t);
		Vector3 normalized = (contactPointLocal - vector).normalized;
		float num = BluntDamageHelpers.NormalizeToRange(BluntDamageHelpers.CalculateKineticEnergy(bluntDamageDealer, contactPointLocal, normalized, vector, t, out collisionDirection), 10f, 150f);
		float num2 = bluntDamageDealer.CalculateHistoryMultiplier(contactPointLocal, normalized);
		float num3 = num * num2;
		collisionDirection = bluntDamageDealer.transform.localToWorldMatrix.MultiplyVector(collisionDirection.normalized) * Math.Min(num3, 1f);
		return num3;
	}

	// Token: 0x06000196 RID: 406 RVA: 0x0000958C File Offset: 0x0000778C
	public static float CalculateKineticEnergy(BluntDamageDealer bluntDamageDealer, Vector3 contactPointLocal, Vector3 comToContact, Vector3 centerOfMassPoint, float t, out Vector3 collisionDirection)
	{
		Vector3 vector = bluntDamageDealer.CalculateLocalPointVelocityFromLatestHistory(contactPointLocal);
		float num = BluntDamageHelpers.CalculateMassBehindHit(bluntDamageDealer, vector, contactPointLocal, t);
		float num2 = Vector3.Dot(vector, comToContact);
		float result = 0.5f * num * (num2 * num2);
		collisionDirection = vector;
		return result;
	}

	// Token: 0x06000197 RID: 407 RVA: 0x000095C7 File Offset: 0x000077C7
	public static float CalculateMassBehindHit(BluntDamageDealer bluntDamageDealer, Vector3 velocity, Vector3 contactPointLocal, float t)
	{
		return bluntDamageDealer.GetWeaponMass(velocity, contactPointLocal, t);
	}

	// Token: 0x06000198 RID: 408 RVA: 0x000095D2 File Offset: 0x000077D2
	public static bool IsArmourHit(Collision collision)
	{
		return collision.collider.gameObject.layer == 17;
	}

	// Token: 0x06000199 RID: 409 RVA: 0x000095EC File Offset: 0x000077EC
	public static float GetTemporaryDamage(BluntDamageDealer bluntDamageDealer, WeaponDamageableBodyPart weaponDamageableBodyPart, float hitKineticEnergyMultiplier, bool armourHit)
	{
		float temporaryDamageMultiplier = bluntDamageDealer.GetTemporaryDamageMultiplier(armourHit);
		float bodyPartTemporaryResistance = BluntDamageHelpers.GetBodyPartTemporaryResistance(weaponDamageableBodyPart.bodyPart);
		return BluntDamageHelpers.CalculateDamage(hitKineticEnergyMultiplier, temporaryDamageMultiplier, bluntDamageDealer.GetTemporaryMaxDamage(armourHit), bluntDamageDealer.GetTemporaryResistancePenetration(), bodyPartTemporaryResistance);
	}

	// Token: 0x0600019A RID: 410 RVA: 0x00009624 File Offset: 0x00007824
	public static float GetPermanentDamage(BluntDamageDealer bluntDamageDealer, WeaponDamageableBodyPart weaponDamageableBodyPart, float hitKineticEnergyMultiplier, bool armourHit)
	{
		float permanentDamageMultiplier = bluntDamageDealer.GetPermanentDamageMultiplier(armourHit);
		float bodyPartPermanentResistance = BluntDamageHelpers.GetBodyPartPermanentResistance(weaponDamageableBodyPart.bodyPart);
		return BluntDamageHelpers.CalculateDamage(hitKineticEnergyMultiplier, permanentDamageMultiplier, bluntDamageDealer.GetPermanentMaxDamage(armourHit), bluntDamageDealer.GetPermanentResistancePenetration(), bodyPartPermanentResistance);
	}

	// Token: 0x0600019B RID: 411 RVA: 0x0000965C File Offset: 0x0000785C
	public static float CalculateDamage(float hitKineticEnergyMultiplier, float multiplier, float maxDamage, float resistancePenetration, float bodyPartResistance)
	{
		float num = bodyPartResistance * (1f - resistancePenetration);
		return Math.Min(hitKineticEnergyMultiplier * multiplier * (1f - num), maxDamage);
	}

	// Token: 0x0600019C RID: 412 RVA: 0x00009688 File Offset: 0x00007888
	public static BluntDamageTypeValues GetBluntDamageTypeValues(BluntDamageType type)
	{
		BluntDamageTypeValues result;
		BluntDamageHelpers.valuesForBluntDamageTypes.TryGetValue(type, out result);
		return result;
	}

	// Token: 0x0600019D RID: 413 RVA: 0x000096A4 File Offset: 0x000078A4
	public static float PermanentDamageMultiplierForBluntDamageType(BluntDamageTypeValues bluntDamageTypeValues, bool armourHit)
	{
		float result;
		if (armourHit)
		{
			result = bluntDamageTypeValues.permanentDamageMultiplierArmoured;
		}
		else
		{
			result = bluntDamageTypeValues.permanentDamageMultiplier;
		}
		return result;
	}

	// Token: 0x0600019E RID: 414 RVA: 0x000096CC File Offset: 0x000078CC
	public static float TemporaryDamageMultiplierForBluntDamageType(BluntDamageTypeValues bluntDamageTypeValues, bool armourHit)
	{
		float result;
		if (armourHit)
		{
			result = bluntDamageTypeValues.temporaryDamageMultiplierArmoured;
		}
		else
		{
			result = bluntDamageTypeValues.temporaryDamageMultiplier;
		}
		return result;
	}

	// Token: 0x0600019F RID: 415 RVA: 0x000096F3 File Offset: 0x000078F3
	[BurstCompile]
	public static float NormalizeToRange(float value, float min, float max)
	{
		return (value - min) / (max - min);
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x000096FC File Offset: 0x000078FC
	[BurstCompile]
	public static float NormalizeToRangeAndClamp(float value, float min, float max)
	{
		return Mathf.Clamp01((value - min) / (max - min));
	}

	// Token: 0x060001A1 RID: 417 RVA: 0x0000970A File Offset: 0x0000790A
	public static void HandlePhysicsForce(WeaponDamageableBodyPart weaponDamageableBodyPart, Vector3 position, Vector3 directionMultiplier)
	{
		if (weaponDamageableBodyPart.Rigidbody != null)
		{
			weaponDamageableBodyPart.Rigidbody.AddForceAtPosition(directionMultiplier * BluntDamageHelpers.maxAddedPhysicsForce, position, ForceMode.Impulse);
		}
	}

	// Token: 0x060001A2 RID: 418 RVA: 0x00009732 File Offset: 0x00007932
	public static float GetBodyPartTemporaryResistance(JointType bodyPart)
	{
		return BluntDamageHelpers.bluntDamageableBodypartValues[(int)bodyPart].temporaryDamageResistance;
	}

	// Token: 0x060001A3 RID: 419 RVA: 0x00009744 File Offset: 0x00007944
	public static float GetBodyPartPermanentResistance(JointType bodyPart)
	{
		return BluntDamageHelpers.bluntDamageableBodypartValues[(int)bodyPart].permanentDamageResistance;
	}

	// Token: 0x040000C9 RID: 201
	public static float bloodDamagePaintThreshold = 0.001f;

	// Token: 0x040000CA RID: 202
	public static Dictionary<BluntDamageType, BluntDamageTypeValues> valuesForBluntDamageTypes = new Dictionary<BluntDamageType, BluntDamageTypeValues>
	{
		{
			BluntDamageType.Default,
			new BluntDamageTypeValues
			{
				overrideMaxPermanentDamage = true,
				overriderMaxTemporaryDamage = true,
				overrideMaxPermanentDamageValue = 1.01f,
				overrideMaxPermanentDamageValueArmoured = 0.51f,
				overrideMaxTemporaryDamageValue = 1.01f,
				overrideMaxTemporaryDamageValueArmoured = 0.51f,
				permanentDamageMultiplier = 0.75f,
				permanentDamageMultiplierArmoured = 0.3f,
				temporaryDamageMultiplier = 0.75f,
				temporaryDamageMultiplierArmoured = 0.4f,
				permanentDamageResistancePenetration = 0.5f,
				temporaryDamageResistancePenetration = 0.2f
			}
		},
		{
			BluntDamageType.Mace,
			new BluntDamageTypeValues
			{
				permanentDamageMultiplier = 1f,
				permanentDamageMultiplierArmoured = 0.5f,
				temporaryDamageMultiplier = 1f,
				temporaryDamageMultiplierArmoured = 0.5f,
				permanentDamageResistancePenetration = 1f,
				temporaryDamageResistancePenetration = 1f
			}
		},
		{
			BluntDamageType.Axe,
			new BluntDamageTypeValues
			{
				overrideMaxPermanentDamage = true,
				overriderMaxTemporaryDamage = true,
				overrideMaxPermanentDamageValue = 0.8f,
				overrideMaxPermanentDamageValueArmoured = 0.4f,
				overrideMaxTemporaryDamageValue = 0.6f,
				overrideMaxTemporaryDamageValueArmoured = 0.4f,
				permanentDamageMultiplier = 0.4f,
				permanentDamageMultiplierArmoured = 0.25f,
				temporaryDamageMultiplier = 0.4f,
				temporaryDamageMultiplierArmoured = 0.25f,
				permanentDamageResistancePenetration = 0.6f,
				temporaryDamageResistancePenetration = 0.6f
			}
		},
		{
			BluntDamageType.Shield,
			new BluntDamageTypeValues
			{
				overrideMaxPermanentDamage = true,
				overriderMaxTemporaryDamage = true,
				overrideMaxPermanentDamageValue = 0.8f,
				overrideMaxPermanentDamageValueArmoured = 0.4f,
				overrideMaxTemporaryDamageValue = 0.6f,
				overrideMaxTemporaryDamageValueArmoured = 0.4f,
				permanentDamageMultiplier = 0.4f,
				permanentDamageMultiplierArmoured = 0.25f,
				temporaryDamageMultiplier = 0.4f,
				temporaryDamageMultiplierArmoured = 0.25f,
				permanentDamageResistancePenetration = 0.6f,
				temporaryDamageResistancePenetration = 0.6f
			}
		},
		{
			BluntDamageType.Polearm,
			new BluntDamageTypeValues
			{
				overrideMaxPermanentDamage = true,
				overriderMaxTemporaryDamage = true,
				overrideMaxPermanentDamageValue = 0.3f,
				overrideMaxPermanentDamageValueArmoured = 0.05f,
				overrideMaxTemporaryDamageValue = 0.4f,
				overrideMaxTemporaryDamageValueArmoured = 0.1f,
				permanentDamageMultiplier = 0.2f,
				permanentDamageMultiplierArmoured = 0.05f,
				temporaryDamageMultiplier = 0.25f,
				temporaryDamageMultiplierArmoured = 0.1f,
				permanentDamageResistancePenetration = 0.5f,
				temporaryDamageResistancePenetration = 0.3f
			}
		},
		{
			BluntDamageType.BodyPart,
			new BluntDamageTypeValues
			{
				overrideMaxPermanentDamage = true,
				overriderMaxTemporaryDamage = true,
				overrideMaxPermanentDamageValue = 0.35f,
				overrideMaxPermanentDamageValueArmoured = 0.15f,
				overrideMaxTemporaryDamageValue = 1.01f,
				overrideMaxTemporaryDamageValueArmoured = 0.51f,
				permanentDamageMultiplier = 0.25f,
				permanentDamageMultiplierArmoured = 0.1f,
				temporaryDamageMultiplier = 0.5f,
				temporaryDamageMultiplierArmoured = 0.3f,
				permanentDamageResistancePenetration = 0f,
				temporaryDamageResistancePenetration = 0f
			}
		},
		{
			BluntDamageType.BodyPartLeg,
			new BluntDamageTypeValues
			{
				overrideMaxPermanentDamage = true,
				overriderMaxTemporaryDamage = true,
				overrideMaxPermanentDamageValue = 0.6f,
				overrideMaxPermanentDamageValueArmoured = 0.3f,
				overrideMaxTemporaryDamageValue = 1.01f,
				overrideMaxTemporaryDamageValueArmoured = 0.51f,
				permanentDamageMultiplier = 0.4f,
				permanentDamageMultiplierArmoured = 0.15f,
				temporaryDamageMultiplier = 0.65f,
				temporaryDamageMultiplierArmoured = 0.4f,
				permanentDamageResistancePenetration = 0.3f,
				temporaryDamageResistancePenetration = 0.2f
			}
		}
	};

	// Token: 0x040000CB RID: 203
	public static float maxAddedPhysicsForce = 80f;

	// Token: 0x040000CC RID: 204
	public static BluntDamageableBodypartValues[] bluntDamageableBodypartValues = new BluntDamageableBodypartValues[]
	{
		new BluntDamageableBodypartValues
		{
			permanentDamageResistance = 0.4f,
			temporaryDamageResistance = 0f
		},
		new BluntDamageableBodypartValues
		{
			permanentDamageResistance = 0.4f,
			temporaryDamageResistance = 0f
		},
		new BluntDamageableBodypartValues
		{
			permanentDamageResistance = 0.4f,
			temporaryDamageResistance = 0f
		},
		new BluntDamageableBodypartValues
		{
			permanentDamageResistance = 0f,
			temporaryDamageResistance = 0f
		},
		new BluntDamageableBodypartValues
		{
			permanentDamageResistance = 0f,
			temporaryDamageResistance = 0f
		},
		new BluntDamageableBodypartValues
		{
			permanentDamageResistance = 0.9f,
			temporaryDamageResistance = 0.3f
		},
		new BluntDamageableBodypartValues
		{
			permanentDamageResistance = 0.9f,
			temporaryDamageResistance = 0.3f
		},
		new BluntDamageableBodypartValues
		{
			permanentDamageResistance = 0.9f,
			temporaryDamageResistance = 0.45f
		},
		new BluntDamageableBodypartValues
		{
			permanentDamageResistance = 0.9f,
			temporaryDamageResistance = 0.45f
		},
		new BluntDamageableBodypartValues
		{
			permanentDamageResistance = 0f,
			temporaryDamageResistance = 0f
		},
		new BluntDamageableBodypartValues
		{
			permanentDamageResistance = 0f,
			temporaryDamageResistance = 0f
		},
		new BluntDamageableBodypartValues
		{
			permanentDamageResistance = 0.3f,
			temporaryDamageResistance = 0f
		},
		new BluntDamageableBodypartValues
		{
			permanentDamageResistance = 0.3f,
			temporaryDamageResistance = 0f
		},
		new BluntDamageableBodypartValues
		{
			permanentDamageResistance = 0.3f,
			temporaryDamageResistance = 0f
		},
		new BluntDamageableBodypartValues
		{
			permanentDamageResistance = 0.9f,
			temporaryDamageResistance = 0.2f
		},
		new BluntDamageableBodypartValues
		{
			permanentDamageResistance = 0.9f,
			temporaryDamageResistance = 0.2f
		}
	};
}
