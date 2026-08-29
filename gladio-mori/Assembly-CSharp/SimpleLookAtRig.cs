using System;
using MoveClasses;
using UnityEngine;

// Token: 0x02000021 RID: 33
public class SimpleLookAtRig : SimpleRig
{
	// Token: 0x0600015C RID: 348 RVA: 0x00007B22 File Offset: 0x00005D22
	protected override void Initialize()
	{
		base.Initialize();
	}

	// Token: 0x0600015D RID: 349 RVA: 0x00007B2A File Offset: 0x00005D2A
	public override void CalculateOffset()
	{
		this.rotationOffset = Quaternion.Inverse(this.GetRotation());
	}

	// Token: 0x0600015E RID: 350 RVA: 0x00007B3D File Offset: 0x00005D3D
	public override void CalculatePosition()
	{
		this.boneBase.rotation = this.GetRotation();
	}

	// Token: 0x0600015F RID: 351 RVA: 0x00007B50 File Offset: 0x00005D50
	public override void CalculateTargetPosition()
	{
		if (this.targetPositionHint != null)
		{
			this.target.position = this.targetPositionHint.position;
			return;
		}
		this.target.position = this.boneBase.position;
		this.target.Translate(this.boneBase.forward, Space.World);
	}

	// Token: 0x06000160 RID: 352 RVA: 0x00007BB0 File Offset: 0x00005DB0
	private Quaternion GetRotation()
	{
		Quaternion rhs = default(Quaternion);
		switch (this.jointChainDirection)
		{
		case JointChainDirection.X:
			rhs = Quaternion.Euler(0f, -90f, 0f);
			break;
		case JointChainDirection.Y:
			rhs = Quaternion.Euler(90f, 0f, 0f);
			break;
		case JointChainDirection.Z:
			rhs = Quaternion.Euler(0f, 0f, 0f);
			break;
		case JointChainDirection.negativeX:
			rhs = Quaternion.Euler(0f, 90f, 0f);
			break;
		case JointChainDirection.negativeY:
			rhs = Quaternion.Euler(-90f, 0f, 0f);
			break;
		}
		Quaternion quaternion = Quaternion.LookRotation(this.target.position - this.boneBase.position, Vector3.up);
		if (this.jointTypeBase == JointType.SCAPULA_LEFT || this.jointTypeBase == JointType.SCAPULA_RIGHT)
		{
			Vector3 vector = quaternion * Vector3.forward;
			float num = Vector3.Angle(this.boneBase.parent.up, vector);
			if (num > 90f)
			{
				quaternion = Quaternion.LookRotation(Vector3.RotateTowards(vector, this.boneBase.parent.up, (num - 90f) * 0.017453292f, 1f), Vector3.up);
			}
		}
		return quaternion * rhs * this.rotationOffset;
	}
}
