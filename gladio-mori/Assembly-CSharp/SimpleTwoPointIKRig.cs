using System;
using System.Collections.Generic;
using MoveClasses;
using UnityEngine;

// Token: 0x02000028 RID: 40
public class SimpleTwoPointIKRig : SimpleRig
{
	// Token: 0x06000176 RID: 374 RVA: 0x00008760 File Offset: 0x00006960
	protected override void Initialize()
	{
		this.jointList = new IKJoint[3];
		this.totalLength = 0f;
		if (this.boneBase != null && this.boneTip != null)
		{
			this.jointList[0] = new IKJoint
			{
				transform = this.boneBase,
				length = (this.boneTip.position - this.boneBase.position).magnitude
			};
			this.jointList[1] = new IKJoint
			{
				transform = this.boneBase,
				length = 0f
			};
			this.totalLength = this.jointList[0].length + this.jointList[1].length;
		}
		switch (this.jointChainDirection)
		{
		case JointChainDirection.X:
			this.rightToForward = Quaternion.Euler(90f, -90f, 0f);
			break;
		case JointChainDirection.Y:
			this.rightToForward = Quaternion.Euler(90f, 0f, 0f);
			break;
		case JointChainDirection.negativeX:
			this.rightToForward = Quaternion.Euler(90f, 90f, 0f);
			break;
		case JointChainDirection.negativeY:
			this.rightToForward = Quaternion.Euler(-90f, 0f, 0f);
			break;
		}
		base.Initialize();
	}

	// Token: 0x06000177 RID: 375 RVA: 0x000088C4 File Offset: 0x00006AC4
	public override void CalculateOffset()
	{
		this.rotationOffset = Quaternion.Inverse(Quaternion.LookRotation(this.boneTip.position - this.boneBase.position, Vector3.up) * this.rightToForward);
	}

	// Token: 0x06000178 RID: 376 RVA: 0x00008904 File Offset: 0x00006B04
	public override void CalculatePosition()
	{
		Vector3 upwards = this.baseHint.forward * -1f + this.hint.forward * -1f;
		Quaternion lhs = Quaternion.LookRotation(this.hint.position - this.boneBase.position, upwards);
		this.boneTip.rotation = this.target.rotation;
		this.boneBase.rotation = lhs * this.rightToForward * this.rotationOffset;
	}

	// Token: 0x06000179 RID: 377 RVA: 0x0000899C File Offset: 0x00006B9C
	public override void CalculateTargetPosition()
	{
		this.target.rotation = this.boneTip.rotation;
		this.target.position = this.boneTip.position;
		this.target.Translate(this.boneTip.position - this.hint.position, Space.World);
	}

	// Token: 0x0600017A RID: 378 RVA: 0x000089FC File Offset: 0x00006BFC
	public override List<JointMove> GetJointMoves()
	{
		List<JointMove> jointMoves = base.GetJointMoves();
		if (this.boneTip != null)
		{
			jointMoves.Add(new JointMove
			{
				joint = this.jointTypeTip,
				targetRotation = new NullableVector3(this.boneTip.localEulerAngles)
			});
		}
		return jointMoves;
	}

	// Token: 0x040000AD RID: 173
	public Transform baseHint;

	// Token: 0x040000AE RID: 174
	public Transform boneTip;

	// Token: 0x040000AF RID: 175
	public JointType jointTypeTip;

	// Token: 0x040000B0 RID: 176
	private IKJoint[] jointList = new IKJoint[3];

	// Token: 0x040000B1 RID: 177
	public float totalLength;

	// Token: 0x040000B2 RID: 178
	private Quaternion rightToForward;
}
