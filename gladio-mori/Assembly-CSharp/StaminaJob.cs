using System;
using MoveClasses;
using PlayerHelpers;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

// Token: 0x020000EA RID: 234
[BurstCompile]
public struct StaminaJob : IJob
{
	// Token: 0x060007D8 RID: 2008 RVA: 0x00026BD4 File Offset: 0x00024DD4
	public StaminaJob(bool _staminaSystemActive, NativeArray<StaminaJobItem> _staminaItems, NativeArray<float> _maximumJointForces, NativeArray<float> _maximumJointSprings, float _fixedDeltaTime, float _staminaRegenRate, float _staminaEffectStart, float _minStaminaMultiplier, float _staminaDrainMultiplier, NativeArray<float> _jointStaminaDrainMultiplier, NativeArray<float> _jointPreventStaminaRegenThreshold, NativeArray<float> _minChangeToDrainStamina, float _targetRotationMultiplier, bool _legacy)
	{
		this.staminaSystemActive = _staminaSystemActive;
		this.staminaItems = _staminaItems;
		this.maximumJointForces = _maximumJointForces;
		this.maximumJointSprings = _maximumJointSprings;
		this.fixedDeltaTime = _fixedDeltaTime;
		this.minChangeToDrainStamina = _minChangeToDrainStamina;
		this.staminaRegenRate = _staminaRegenRate;
		this.staminaEffectStart = _staminaEffectStart;
		this.minStaminaMultiplier = _minStaminaMultiplier;
		this.staminaDrainMultiplier = _staminaDrainMultiplier;
		this.jointStaminaDrainMultiplier = _jointStaminaDrainMultiplier;
		this.jointPreventStaminaRegenThreshold = _jointPreventStaminaRegenThreshold;
		this.targetRotationMultiplier = _targetRotationMultiplier;
		this.legacy = _legacy;
	}

	// Token: 0x060007D9 RID: 2009 RVA: 0x00026C50 File Offset: 0x00024E50
	public void Execute()
	{
		for (int i = 0; i < this.staminaItems.Length; i++)
		{
			StaminaJobItem staminaJobItem = this.staminaItems[i];
			if (!staminaJobItem.dead)
			{
				staminaJobItem = this.HandleBluntDamage(staminaJobItem);
				if (!staminaJobItem.ai && this.staminaSystemActive)
				{
					staminaJobItem = this.ResetStaminaJobItem(staminaJobItem);
					staminaJobItem = this.CalculateCurrentStaminaAmount(staminaJobItem);
				}
				staminaJobItem = this.CalculateJointMaxForces(staminaJobItem);
				staminaJobItem = this.RegenerateBodyPartHealth(staminaJobItem);
				this.staminaItems[i] = staminaJobItem;
			}
		}
	}

	// Token: 0x060007DA RID: 2010 RVA: 0x00026CD0 File Offset: 0x00024ED0
	private StaminaJobItem ResetStaminaJobItem(StaminaJobItem staminaJobItem)
	{
		for (int i = 0; i < staminaJobItem.preventStaminaRegenList.Length; i++)
		{
			staminaJobItem.preventStaminaRegenList[i] = false;
		}
		return staminaJobItem;
	}

	// Token: 0x060007DB RID: 2011 RVA: 0x00026D04 File Offset: 0x00024F04
	private StaminaJobItem CalculateCurrentStaminaAmount(StaminaJobItem staminaItem)
	{
		for (int i = 0; i < staminaItem.jointTypes.Length; i++)
		{
			float num = this.CalculateNormalizedChangeForSingleJoint(staminaItem.oldRotations[i], staminaItem.currentRotations[i], staminaItem.targetRotations[i], staminaItem.oldTargetRotations[i]);
			int staminaIndexForJoint = PlayerJointHelpers.GetStaminaIndexForJoint(staminaItem.jointTypes[i]);
			if (staminaIndexForJoint >= 0)
			{
				staminaItem.currentStaminas[staminaIndexForJoint] = math.max(staminaItem.currentStaminas[staminaIndexForJoint] - this.CalculateStaminaLoss(num, (int)staminaItem.jointTypes[i]), 0f);
				if (this.CalculateThresholdValue(this.jointPreventStaminaRegenThreshold[(int)staminaItem.jointTypes[i]]) < num)
				{
					staminaItem.preventStaminaRegenList[staminaIndexForJoint] = true;
				}
			}
			staminaItem.oldRotations[i] = staminaItem.currentRotations[i];
			staminaItem.oldTargetRotations[i] = staminaItem.targetRotations[i];
		}
		staminaItem = this.RegenerateStaminasAndCalculateMultipliers(staminaItem);
		return staminaItem;
	}

	// Token: 0x060007DC RID: 2012 RVA: 0x00026E28 File Offset: 0x00025028
	public float CalculateNormalizedChangeForSingleJoint(quaternion start, quaternion end, quaternion target, quaternion oldTarget)
	{
		float y = StaminaJob.DotToDegrees(math.dot(target, oldTarget)) / 180f * this.targetRotationMultiplier;
		float num = StaminaJob.DotToDegrees(math.dot(start, target));
		float num2 = StaminaJob.DotToDegrees(math.dot(end, target));
		return math.max(math.max(num - num2, 0f) / 180f, y);
	}

	// Token: 0x060007DD RID: 2013 RVA: 0x00026E84 File Offset: 0x00025084
	public StaminaJobItem RegenerateStaminasAndCalculateMultipliers(StaminaJobItem staminaItem)
	{
		for (int i = 0; i < staminaItem.currentStaminas.Length; i++)
		{
			if (!staminaItem.preventStaminaRegenList[i])
			{
				staminaItem.currentStaminas[i] = this.CalculateRegeneratedStamina(staminaItem.currentStaminas[i]);
			}
			staminaItem.currentStaminaMultipliers[i] = this.CalculateStaminaMultiplier(staminaItem.currentStaminas[i]);
		}
		return staminaItem;
	}

	// Token: 0x060007DE RID: 2014 RVA: 0x00026EF8 File Offset: 0x000250F8
	public float CalculateRegeneratedStamina(float stamina)
	{
		return math.clamp(stamina + this.fixedDeltaTime * this.staminaRegenRate, 0f, 1f);
	}

	// Token: 0x060007DF RID: 2015 RVA: 0x00026F18 File Offset: 0x00025118
	public float CalculateStaminaMultiplier(float stamina)
	{
		float result = 1f;
		if (stamina < this.staminaEffectStart)
		{
			result = StaminaJob.NormalizeToRange(math.smoothstep(0f, this.staminaEffectStart, stamina), this.minStaminaMultiplier, 1f);
		}
		return result;
	}

	// Token: 0x060007E0 RID: 2016 RVA: 0x00026F58 File Offset: 0x00025158
	private StaminaJobItem CalculateJointMaxForces(StaminaJobItem staminaItem)
	{
		for (int i = 0; i < staminaItem.jointTypes.Length; i++)
		{
			float num = StaminaJob.GetStaminaMultiplierForJoint(staminaItem, i);
			float b = 1f;
			float bluntDamageMultiplierForJoint = StaminaJob.GetBluntDamageMultiplierForJoint(staminaItem, i, out b);
			if (bluntDamageMultiplierForJoint < num)
			{
				num = bluntDamageMultiplierForJoint;
			}
			if (this.legacy)
			{
				float x = math.remap(0f, b, 0f, 1f, num);
				staminaItem.calculatedJointMaxForce[i] = this.GetCustomMappedValue(x, this.maximumJointForces[(int)staminaItem.jointTypes[i]], 0.75f);
				staminaItem.calculatedJointSpring[i] = this.maximumJointSprings[(int)staminaItem.jointTypes[i]] * StaminaJob.NormalizeToRange(num, 0.75f, 1f);
			}
			else
			{
				staminaItem.calculatedJointMaxForce[i] = this.maximumJointForces[(int)staminaItem.jointTypes[i]] * num;
				staminaItem.calculatedJointSpring[i] = this.maximumJointSprings[(int)staminaItem.jointTypes[i]] * StaminaJob.NormalizeToRange(num, 0.75f, 1f);
			}
		}
		return staminaItem;
	}

	// Token: 0x060007E1 RID: 2017 RVA: 0x0002708C File Offset: 0x0002528C
	public static float GetStaminaMultiplierForJoint(StaminaJobItem staminaItem, int index)
	{
		float result = 1f;
		int staminaIndexForJoint = PlayerJointHelpers.GetStaminaIndexForJoint(staminaItem.jointTypes[index]);
		if (staminaIndexForJoint >= 0)
		{
			result = staminaItem.currentStaminaMultipliers[staminaIndexForJoint];
		}
		return result;
	}

	// Token: 0x060007E2 RID: 2018 RVA: 0x000270C5 File Offset: 0x000252C5
	private static float DotToDegrees(float dot)
	{
		return math.acos(math.min(math.abs(dot), 1f)) * 2f * 57.29578f;
	}

	// Token: 0x060007E3 RID: 2019 RVA: 0x000270E8 File Offset: 0x000252E8
	public static float NormalizeToRange(float value, float min, float max)
	{
		return min + value * (max - min);
	}

	// Token: 0x060007E4 RID: 2020 RVA: 0x000270F1 File Offset: 0x000252F1
	public static float NormalizeDotProductValue(float value)
	{
		return 1f - (value + 1f) / 2f;
	}

	// Token: 0x060007E5 RID: 2021 RVA: 0x00027108 File Offset: 0x00025308
	public float CalculateStaminaLoss(float normalizedJointChange, int jointTypeIndex)
	{
		float result = 0f;
		if (normalizedJointChange > this.CalculateThresholdValue(this.minChangeToDrainStamina[jointTypeIndex]))
		{
			result = normalizedJointChange * this.staminaDrainMultiplier * this.jointStaminaDrainMultiplier[jointTypeIndex];
		}
		return result;
	}

	// Token: 0x060007E6 RID: 2022 RVA: 0x00027147 File Offset: 0x00025347
	public float CalculateThresholdValue(float value)
	{
		return value * (this.fixedDeltaTime * 200f);
	}

	// Token: 0x060007E7 RID: 2023 RVA: 0x00027158 File Offset: 0x00025358
	private float GetCustomMappedValue(float x, float midPointValue, float breakingPoint)
	{
		if (x <= breakingPoint)
		{
			return x / breakingPoint * midPointValue;
		}
		float x2 = (x - breakingPoint) / (1f - breakingPoint);
		float y = 10f;
		float num = 1000000f;
		return midPointValue + math.pow(x2, y) * num;
	}

	// Token: 0x060007E8 RID: 2024 RVA: 0x00027194 File Offset: 0x00025394
	private StaminaJobItem HandleBluntDamage(StaminaJobItem staminaItem)
	{
		for (int i = staminaItem.bluntDamageInstances.Length - 1; i > -1; i--)
		{
			BluntDamageInstance bluntDamageInstance = staminaItem.bluntDamageInstances[i];
			int bodyPart = (int)bluntDamageInstance.bodyPart;
			BodyPartHealth bodyPartHealth = staminaItem.bodyPartHealths[bodyPart];
			bodyPartHealth.temporaryHealth = math.max(0f, bodyPartHealth.temporaryHealth - bluntDamageInstance.temporaryDamage);
			bodyPartHealth.lowestTemporaryHealth = bodyPartHealth.temporaryHealth;
			staminaItem.bodyPartHealths[bodyPart] = bodyPartHealth;
		}
		staminaItem.bluntDamageInstances.Clear();
		return staminaItem;
	}

	// Token: 0x060007E9 RID: 2025 RVA: 0x00027224 File Offset: 0x00025424
	public StaminaJobItem RegenerateBodyPartHealth(StaminaJobItem staminaItem)
	{
		for (int i = 0; i < staminaItem.bodyPartHealths.Length; i++)
		{
			staminaItem.bodyPartHealths[i] = this.CalculateRegeneratedBodyPartHealth(staminaItem.bodyPartHealths[i]);
		}
		return staminaItem;
	}

	// Token: 0x060007EA RID: 2026 RVA: 0x0002726C File Offset: 0x0002546C
	public BodyPartHealth CalculateRegeneratedBodyPartHealth(BodyPartHealth bodyPartHealth)
	{
		float x = bodyPartHealth.temporaryHealth + this.fixedDeltaTime * BluntDamageHelpers.RegenRateForBodyPart(bodyPartHealth.bodyPart);
		bodyPartHealth.temporaryHealth = math.clamp(x, 0f, 1f);
		return bodyPartHealth;
	}

	// Token: 0x060007EB RID: 2027 RVA: 0x000272AC File Offset: 0x000254AC
	public static float GetBluntDamageMultiplierForJoint(StaminaJobItem staminaItem, int index, out float maxBluntDamageMultiplier)
	{
		float num = 1f;
		JointType jointType = staminaItem.jointTypes[index];
		maxBluntDamageMultiplier = 1f;
		for (int i = 0; i < staminaItem.bodyPartHealths.Length; i++)
		{
			BodyPartHealth bodyPartHealth = staminaItem.bodyPartHealths[i];
			if (BluntDamageHelpers.BodyPartAffectsJoint(bodyPartHealth.bodyPart, jointType))
			{
				float num2;
				if (jointType == JointType.HIP)
				{
					num2 = bodyPartHealth.TemporaryStrengthMultiplier();
				}
				else
				{
					num2 = bodyPartHealth.StrengthMultiplier();
				}
				if (num2 < num)
				{
					num = num2;
					maxBluntDamageMultiplier = bodyPartHealth.PermanentStrengthMultiplier();
				}
			}
		}
		return num;
	}

	// Token: 0x04000556 RID: 1366
	[ReadOnly]
	public bool staminaSystemActive;

	// Token: 0x04000557 RID: 1367
	public NativeArray<StaminaJobItem> staminaItems;

	// Token: 0x04000558 RID: 1368
	[ReadOnly]
	public NativeArray<float> maximumJointForces;

	// Token: 0x04000559 RID: 1369
	[ReadOnly]
	public NativeArray<float> maximumJointSprings;

	// Token: 0x0400055A RID: 1370
	[ReadOnly]
	public NativeArray<float> jointStaminaDrainMultiplier;

	// Token: 0x0400055B RID: 1371
	[ReadOnly]
	public NativeArray<float> jointPreventStaminaRegenThreshold;

	// Token: 0x0400055C RID: 1372
	[ReadOnly]
	public float fixedDeltaTime;

	// Token: 0x0400055D RID: 1373
	[ReadOnly]
	public NativeArray<float> minChangeToDrainStamina;

	// Token: 0x0400055E RID: 1374
	[ReadOnly]
	public float staminaRegenRate;

	// Token: 0x0400055F RID: 1375
	[ReadOnly]
	public float staminaEffectStart;

	// Token: 0x04000560 RID: 1376
	[ReadOnly]
	public float minStaminaMultiplier;

	// Token: 0x04000561 RID: 1377
	[ReadOnly]
	public float staminaDrainMultiplier;

	// Token: 0x04000562 RID: 1378
	[ReadOnly]
	public float targetRotationMultiplier;

	// Token: 0x04000563 RID: 1379
	[ReadOnly]
	public bool legacy;
}
