using System;
using UnityEngine;

// Token: 0x02000140 RID: 320
public static class ConfigurableJointExtensions
{
	// Token: 0x06000A03 RID: 2563 RVA: 0x0002F5B8 File Offset: 0x0002D7B8
	public static void SetTargetRotationLocal(this ConfigurableJoint joint, Quaternion targetLocalRotation, Quaternion startLocalRotation)
	{
		if (joint.configuredInWorldSpace)
		{
			Debug.LogError("SetTargetRotationLocal should not be used with joints that are configured in world space. For world space joints, use SetTargetRotation.", joint);
		}
		joint.SetTargetRotationInternal(targetLocalRotation, startLocalRotation, Space.Self);
	}

	// Token: 0x06000A04 RID: 2564 RVA: 0x0002F5D6 File Offset: 0x0002D7D6
	public static void SetTargetRotation(this ConfigurableJoint joint, Quaternion targetWorldRotation, Quaternion startWorldRotation)
	{
		if (!joint.configuredInWorldSpace)
		{
			Debug.LogError("SetTargetRotation must be used with joints that are configured in world space. For local space joints, use SetTargetRotationLocal.", joint);
		}
		joint.SetTargetRotationInternal(targetWorldRotation, startWorldRotation, Space.World);
	}

	// Token: 0x06000A05 RID: 2565 RVA: 0x0002F5F4 File Offset: 0x0002D7F4
	public static void SetTestRotation(this ConfigurableJoint joint, Quaternion targetWorldRotation, Quaternion startWorldRotation)
	{
		Vector3 axis = joint.axis;
		Vector3 normalized = Vector3.Cross(joint.axis, joint.secondaryAxis).normalized;
		Vector3 normalized2 = Vector3.Cross(normalized, axis).normalized;
		Quaternion quaternion = Quaternion.LookRotation(normalized, normalized2);
		Quaternion quaternion2 = Quaternion.Inverse(quaternion);
		quaternion2 *= Quaternion.Inverse(targetWorldRotation);
		quaternion2 *= quaternion;
		joint.targetRotation = quaternion2;
	}

	// Token: 0x06000A06 RID: 2566 RVA: 0x0002F660 File Offset: 0x0002D860
	private static void SetTargetRotationInternal(this ConfigurableJoint joint, Quaternion targetRotation, Quaternion startRotation, Space space)
	{
		Vector3 axis = joint.axis;
		Vector3 normalized = Vector3.Cross(joint.axis, joint.secondaryAxis).normalized;
		Vector3 normalized2 = Vector3.Cross(normalized, axis).normalized;
		Quaternion quaternion = Quaternion.LookRotation(normalized, normalized2);
		Quaternion quaternion2 = Quaternion.Inverse(quaternion);
		if (space == Space.World)
		{
			quaternion2 *= startRotation * Quaternion.Inverse(targetRotation);
		}
		else
		{
			quaternion2 *= Quaternion.Inverse(targetRotation) * startRotation;
		}
		quaternion2 *= quaternion;
		joint.targetRotation = quaternion2;
	}
}
