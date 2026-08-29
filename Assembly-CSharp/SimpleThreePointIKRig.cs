using System;
using System.Collections.Generic;
using MoveClasses;
using UnityEngine;

// Token: 0x02000027 RID: 39
public class SimpleThreePointIKRig : SimpleRig
{
	// Token: 0x06000171 RID: 369 RVA: 0x000080F0 File Offset: 0x000062F0
	protected override void Initialize()
	{
		this.jointList = new IKJoint[3];
		this.totalLength = 0f;
		if (this.boneBase != null && this.boneMid != null && this.boneTip != null)
		{
			this.jointList[0] = new IKJoint
			{
				transform = this.boneBase,
				length = (this.boneMid.position - this.boneBase.position).magnitude
			};
			this.jointList[1] = new IKJoint
			{
				transform = this.boneBase,
				length = (this.boneTip.position - this.boneMid.position).magnitude
			};
			this.jointList[2] = new IKJoint
			{
				transform = this.boneBase,
				length = 0f
			};
			this.totalLength = this.jointList[0].length + this.jointList[1].length;
		}
		base.Initialize();
	}

	// Token: 0x06000172 RID: 370 RVA: 0x00008218 File Offset: 0x00006418
	public override void CalculatePosition()
	{
		float magnitude = (this.target.position - this.boneBase.position).magnitude;
		Vector3 upwards = Vector3.up;
		if (this.hint != null)
		{
			upwards = this.hint.position - this.boneBase.position;
		}
		Quaternion rhs = default(Quaternion);
		switch (this.jointChainDirection)
		{
		case JointChainDirection.X:
			rhs = Quaternion.Euler(90f, -90f, 0f);
			break;
		case JointChainDirection.Y:
			rhs = Quaternion.Euler(90f, 0f, 0f);
			break;
		case JointChainDirection.negativeX:
			rhs = Quaternion.Euler(90f, 90f, 0f);
			break;
		case JointChainDirection.negativeY:
			rhs = Quaternion.Euler(-90f, 0f, 0f);
			break;
		}
		if (magnitude > this.totalLength)
		{
			Quaternion lhs = Quaternion.LookRotation(this.target.position - this.boneBase.position, upwards);
			this.boneBase.rotation = lhs * rhs;
			this.boneMid.rotation = default(Quaternion);
		}
		else
		{
			float num = Mathf.Acos((this.jointList[0].length * this.jointList[0].length + this.jointList[1].length * this.jointList[1].length - magnitude * magnitude) / (2f * this.jointList[0].length * this.jointList[1].length));
			float num2 = Mathf.Acos((this.jointList[0].length * this.jointList[0].length + magnitude * magnitude - this.jointList[1].length * this.jointList[1].length) / (2f * this.jointList[0].length * magnitude));
			if (!float.IsNaN(num) && !float.IsNaN(num2))
			{
				Quaternion lhs2 = Quaternion.LookRotation(this.target.position - this.boneBase.position, upwards);
				float num3 = -Mathf.Abs(57.29578f * num2);
				float num4 = Mathf.Abs(57.29578f * num - 180f);
				switch (this.jointChainDirection)
				{
				case JointChainDirection.X:
					this.boneBase.rotation = lhs2 * rhs * Quaternion.Euler(0f, -num3, 0f);
					this.boneMid.localRotation = Quaternion.Euler(0f, -num4, 0f);
					break;
				case JointChainDirection.Y:
					this.boneBase.rotation = lhs2 * rhs * Quaternion.Euler(num3, 0f, 0f);
					this.boneMid.localRotation = Quaternion.Euler(num4, 0f, 0f);
					break;
				case JointChainDirection.negativeX:
					this.boneBase.rotation = lhs2 * rhs * Quaternion.Euler(0f, num3, 0f);
					this.boneMid.localRotation = Quaternion.Euler(0f, num4, 0f);
					break;
				case JointChainDirection.negativeY:
					this.boneBase.rotation = lhs2 * rhs * Quaternion.Euler(num3, 0f, 0f);
					this.boneMid.localRotation = Quaternion.Euler(num4, 0f, 0f);
					break;
				}
			}
		}
		if (this.targetCanRotate)
		{
			this.boneTip.rotation = this.target.rotation;
		}
	}

	// Token: 0x06000173 RID: 371 RVA: 0x000085E0 File Offset: 0x000067E0
	public override void CalculateTargetPosition()
	{
		this.target.position = this.boneTip.position;
		if (this.targetCanRotate)
		{
			this.target.rotation = this.boneTip.rotation;
		}
		else
		{
			this.target.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
		}
		this.hint.position = this.boneMid.position;
		this.hint.rotation = this.boneBase.rotation;
		this.hint.Translate(this.targetHintOffset, Space.Self);
		this.hint.rotation = Quaternion.Euler(0f, 0f, 0f);
	}

	// Token: 0x06000174 RID: 372 RVA: 0x0000869C File Offset: 0x0000689C
	public override List<JointMove> GetJointMoves()
	{
		List<JointMove> jointMoves = base.GetJointMoves();
		if (this.boneMid != null)
		{
			jointMoves.Add(new JointMove
			{
				joint = this.jointTypeMid,
				targetRotation = new NullableVector3(this.boneMid.localEulerAngles)
			});
		}
		if (this.boneTip != null && this.targetCanRotate)
		{
			jointMoves.Add(new JointMove
			{
				joint = this.jointTypeTip,
				targetRotation = new NullableVector3(this.boneTip.localEulerAngles)
			});
		}
		return jointMoves;
	}

	// Token: 0x040000A6 RID: 166
	public Transform boneMid;

	// Token: 0x040000A7 RID: 167
	public Transform boneTip;

	// Token: 0x040000A8 RID: 168
	public JointType jointTypeMid;

	// Token: 0x040000A9 RID: 169
	public JointType jointTypeTip;

	// Token: 0x040000AA RID: 170
	private IKJoint[] jointList = new IKJoint[3];

	// Token: 0x040000AB RID: 171
	public float totalLength;

	// Token: 0x040000AC RID: 172
	public Vector3 targetHintOffset = new Vector3(0f, 0f, 0.5f);
}
