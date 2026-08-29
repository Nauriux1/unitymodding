using System;
using UnityEngine;

// Token: 0x0200004C RID: 76
public class HingeJointTarget : MonoBehaviour
{
	// Token: 0x06000221 RID: 545 RVA: 0x0000777A File Offset: 0x0000597A
	private void Start()
	{
	}

	// Token: 0x06000222 RID: 546 RVA: 0x0000C240 File Offset: 0x0000A440
	private void Update()
	{
		if (this.hj != null && this.target != null)
		{
			if (this.x)
			{
				JointSpring spring = this.hj.spring;
				spring.targetPosition = this.target.transform.localEulerAngles.x;
				if (spring.targetPosition > 180f)
				{
					spring.targetPosition -= 360f;
				}
				if (this.invert)
				{
					spring.targetPosition *= -1f;
				}
				spring.targetPosition = Mathf.Clamp(spring.targetPosition, this.hj.limits.min + 5f, this.hj.limits.max - 5f);
				this.hj.spring = spring;
				return;
			}
			if (this.y)
			{
				JointSpring spring2 = this.hj.spring;
				spring2.targetPosition = this.target.transform.localEulerAngles.y;
				if (spring2.targetPosition > 180f)
				{
					spring2.targetPosition -= 360f;
				}
				if (this.invert)
				{
					spring2.targetPosition *= -1f;
				}
				spring2.targetPosition = Mathf.Clamp(spring2.targetPosition, this.hj.limits.min + 5f, this.hj.limits.max - 5f);
				this.hj.spring = spring2;
				return;
			}
			if (this.z)
			{
				JointSpring spring3 = this.hj.spring;
				spring3.targetPosition = this.target.transform.localEulerAngles.z;
				if (spring3.targetPosition > 180f)
				{
					spring3.targetPosition -= 360f;
				}
				if (this.invert)
				{
					spring3.targetPosition *= -1f;
				}
				if (this.hj.useLimits)
				{
					spring3.targetPosition = Mathf.Clamp(spring3.targetPosition, this.hj.limits.min + 5f, this.hj.limits.max - 5f);
				}
				this.hj.spring = spring3;
			}
		}
	}

	// Token: 0x0400016D RID: 365
	public HingeJoint hj;

	// Token: 0x0400016E RID: 366
	public Transform target;

	// Token: 0x0400016F RID: 367
	[Tooltip("Only use one of these values at a time. Toggle invert if the rotation is backwards.")]
	public bool x;

	// Token: 0x04000170 RID: 368
	[Tooltip("Only use one of these values at a time. Toggle invert if the rotation is backwards.")]
	public bool y;

	// Token: 0x04000171 RID: 369
	[Tooltip("Only use one of these values at a time. Toggle invert if the rotation is backwards.")]
	public bool z;

	// Token: 0x04000172 RID: 370
	[Tooltip("Only use one of these values at a time. Toggle invert if the rotation is backwards.")]
	public bool invert;
}
