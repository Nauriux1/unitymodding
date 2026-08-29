using System;
using System.Collections.Generic;
using MoveClasses;
using UnityEngine;

// Token: 0x02000024 RID: 36
public class SimpleRig : MonoBehaviour
{
	// Token: 0x06000168 RID: 360 RVA: 0x00007EA6 File Offset: 0x000060A6
	private void Awake()
	{
		this.Initialize();
	}

	// Token: 0x06000169 RID: 361 RVA: 0x0000777A File Offset: 0x0000597A
	protected virtual void Initialize()
	{
	}

	// Token: 0x0600016A RID: 362 RVA: 0x0000777A File Offset: 0x0000597A
	public virtual void CalculateOffset()
	{
	}

	// Token: 0x0600016B RID: 363 RVA: 0x0000777A File Offset: 0x0000597A
	public virtual void CalculatePosition()
	{
	}

	// Token: 0x0600016C RID: 364 RVA: 0x0000777A File Offset: 0x0000597A
	public virtual void CalculateTargetPosition()
	{
	}

	// Token: 0x0600016D RID: 365 RVA: 0x00007EB0 File Offset: 0x000060B0
	public virtual bool TransformsHaveChanged()
	{
		if ((this.target != null && this.target.hasChanged) || (this.hint != null && this.hint.hasChanged))
		{
			if (this.target != null)
			{
				this.target.hasChanged = false;
			}
			if (this.hint != null)
			{
				this.hint.hasChanged = false;
			}
			return true;
		}
		return false;
	}

	// Token: 0x0600016E RID: 366 RVA: 0x00007F2C File Offset: 0x0000612C
	public virtual List<JointMove> GetJointMoves()
	{
		List<JointMove> list = new List<JointMove>();
		if (this.boneBase != null)
		{
			JointMove jointMove = new JointMove
			{
				joint = this.jointTypeBase,
				targetRotation = new NullableVector3(this.boneBase.localEulerAngles)
			};
			jointMove.targetRotation = new NullableVector3(null, null, null);
			jointMove.targetRotation.x = new float?(this.boneBase.localEulerAngles.x);
			jointMove.targetRotation.y = new float?(this.boneBase.localEulerAngles.y);
			jointMove.targetRotation.z = new float?(this.boneBase.localEulerAngles.z);
			if (jointMove.targetRotation.x.Value > 180f)
			{
				jointMove.targetRotation.x = new float?(jointMove.targetRotation.x.Value - 360f);
			}
			if (jointMove.targetRotation.y.Value > 180f)
			{
				jointMove.targetRotation.y = new float?(jointMove.targetRotation.y.Value - 360f);
			}
			if (jointMove.targetRotation.z.Value > 180f)
			{
				jointMove.targetRotation.z = new float?(jointMove.targetRotation.z.Value - 360f);
			}
			list.Add(jointMove);
		}
		return list;
	}

	// Token: 0x04000090 RID: 144
	public int priority;

	// Token: 0x04000091 RID: 145
	public Transform boneBase;

	// Token: 0x04000092 RID: 146
	public JointType jointTypeBase;

	// Token: 0x04000093 RID: 147
	public Transform target;

	// Token: 0x04000094 RID: 148
	public Transform hint;

	// Token: 0x04000095 RID: 149
	public JointChainDirection jointChainDirection;

	// Token: 0x04000096 RID: 150
	public Vector3 positionOffset;

	// Token: 0x04000097 RID: 151
	public Quaternion rotationOffset;

	// Token: 0x04000098 RID: 152
	public Transform targetPositionHint;

	// Token: 0x04000099 RID: 153
	public bool targetCanRotate;

	// Token: 0x0400009A RID: 154
	public bool isHand;

	// Token: 0x0400009B RID: 155
	public List<SimpleRig> animatedChildRigs = new List<SimpleRig>();

	// Token: 0x0400009C RID: 156
	public List<SimpleRig> targetChildRigs = new List<SimpleRig>();
}
