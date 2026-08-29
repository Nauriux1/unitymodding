using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

// Token: 0x02000148 RID: 328
public class JointStrength : MonoBehaviour
{
	// Token: 0x06000A2C RID: 2604 RVA: 0x00030124 File Offset: 0x0002E324
	private void Start()
	{
		this.instancesOfDamage = 0;
		this.currentPercent = 100f;
		if (this.joint == null)
		{
			this.joint = base.GetComponent<ConfigurableJoint>();
		}
		if (this.joint != null)
		{
			this.SetInitialValues(this.joint.angularXDrive.positionSpring, this.joint.angularXDrive.positionDamper, this.joint.angularXDrive.maximumForce);
		}
	}

	// Token: 0x06000A2D RID: 2605 RVA: 0x000301AA File Offset: 0x0002E3AA
	private void Update()
	{
		if (this.updateStrengthPercent)
		{
			this.SetStrengthPercent(this.currentPercent);
			this.updateStrengthPercent = false;
		}
	}

	// Token: 0x06000A2E RID: 2606 RVA: 0x000301C7 File Offset: 0x0002E3C7
	public void RegisterMuscle(WeaponDamageablePart muscle)
	{
		this.muscleList.Add(muscle);
	}

	// Token: 0x06000A2F RID: 2607 RVA: 0x000301D8 File Offset: 0x0002E3D8
	public void SetStrengthPercent(float powerPercent)
	{
		this.currentPercent = powerPercent;
		float power = this.totalMaxPower * (powerPercent / 100f);
		float damper = this.totalMaxDamper * (powerPercent / 100f);
		this.maxDamper = damper;
		this.maxPower = power;
		this.SetStrength(power, damper, null);
	}

	// Token: 0x06000A30 RID: 2608 RVA: 0x0003022C File Offset: 0x0002E42C
	public void SetStrength(float power, float damper, float? maximumForce = null)
	{
		if (this.joint != null)
		{
			float maximumForce2 = this.joint.angularXDrive.maximumForce;
			if (maximumForce != null)
			{
				maximumForce2 = maximumForce.Value;
			}
			this.joint.angularXDrive = new JointDrive
			{
				positionDamper = damper,
				positionSpring = power,
				maximumForce = maximumForce2
			};
			this.joint.angularYZDrive = new JointDrive
			{
				positionDamper = damper,
				positionSpring = power,
				maximumForce = maximumForce2
			};
		}
	}

	// Token: 0x06000A31 RID: 2609 RVA: 0x000302C4 File Offset: 0x0002E4C4
	public void DealMuscleDamage()
	{
		if (this.strengthPercentsForDamageInstances.Count > this.instancesOfDamage)
		{
			float strengthPercent = this.strengthPercentsForDamageInstances[this.instancesOfDamage];
			this.SetStrengthPercent(strengthPercent);
			this.instancesOfDamage++;
		}
	}

	// Token: 0x06000A32 RID: 2610 RVA: 0x0003030C File Offset: 0x0002E50C
	public void SetStaminaForce(float maxForce, float positionSpring)
	{
		if (this.joint != null && !Generic.FloatEquals(maxForce, this.joint.angularXDrive.maximumForce))
		{
			float positionSpring2 = Mathf.Min(positionSpring, this.maxPower);
			this.joint.angularXDrive = new JointDrive
			{
				positionDamper = this.joint.angularXDrive.positionDamper,
				positionSpring = positionSpring2,
				maximumForce = maxForce
			};
			this.joint.angularYZDrive = new JointDrive
			{
				positionDamper = this.joint.angularYZDrive.positionDamper,
				positionSpring = positionSpring2,
				maximumForce = maxForce
			};
		}
	}

	// Token: 0x06000A33 RID: 2611 RVA: 0x000303D1 File Offset: 0x0002E5D1
	public void SetInitialValues(float newTotalMaxPower, float newTotalMaxDamper, float newTotalMaximumForce)
	{
		this.totalMaxPower = newTotalMaxPower;
		this.maxPower = this.totalMaxPower;
		this.totalMaxDamper = newTotalMaxDamper;
		this.maxDamper = this.totalMaxDamper;
		this.SetStrength(this.maxPower, this.maxDamper, new float?(newTotalMaximumForce));
	}

	// Token: 0x0400072B RID: 1835
	public ConfigurableJoint joint;

	// Token: 0x0400072C RID: 1836
	public float totalMaxPower;

	// Token: 0x0400072D RID: 1837
	public float maxPower;

	// Token: 0x0400072E RID: 1838
	public string jointName;

	// Token: 0x0400072F RID: 1839
	public float totalMaxDamper;

	// Token: 0x04000730 RID: 1840
	public float maxDamper;

	// Token: 0x04000731 RID: 1841
	public float currentPercent;

	// Token: 0x04000732 RID: 1842
	public List<WeaponDamageablePart> muscleList = new List<WeaponDamageablePart>();

	// Token: 0x04000733 RID: 1843
	public bool updateStrengthPercent;

	// Token: 0x04000734 RID: 1844
	public int instancesOfDamage;

	// Token: 0x04000735 RID: 1845
	public List<float> strengthPercentsForDamageInstances = new List<float>
	{
		10f,
		5f,
		2.5f,
		1f
	};
}
