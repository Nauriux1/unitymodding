using System;
using MoveClasses;
using UnityEngine;

// Token: 0x02000022 RID: 34
public class SimplePositionAndRotationRig : SimpleRig
{
	// Token: 0x06000162 RID: 354 RVA: 0x00007D0C File Offset: 0x00005F0C
	public override void CalculatePosition()
	{
		this.boneBase.position = this.target.position;
		Vector3 eulerAngles = this.target.rotation.eulerAngles;
		if (this.jointTypeBase == JointType.HIP)
		{
			if (eulerAngles.x > 180f)
			{
				eulerAngles.x -= 360f;
			}
			if (eulerAngles.y > 180f)
			{
				eulerAngles.y -= 360f;
			}
			if (eulerAngles.z > 180f)
			{
				eulerAngles.z -= 360f;
			}
			eulerAngles.x = Mathf.Clamp(eulerAngles.x, -10f, 10f);
			eulerAngles.z = Mathf.Clamp(eulerAngles.z, -10f, 10f);
		}
		this.boneBase.eulerAngles = eulerAngles;
	}

	// Token: 0x06000163 RID: 355 RVA: 0x00007DF2 File Offset: 0x00005FF2
	public override void CalculateTargetPosition()
	{
		this.target.position = this.boneBase.position;
		this.target.rotation = this.boneBase.rotation;
	}
}
