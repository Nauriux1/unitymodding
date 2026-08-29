using System;
using UnityEngine;

// Token: 0x02000023 RID: 35
public class SimpleRelativePositionRig : SimpleRig
{
	// Token: 0x06000165 RID: 357 RVA: 0x00007E20 File Offset: 0x00006020
	protected override void Initialize()
	{
		this.boneBase.parent = this.target;
		base.Initialize();
	}

	// Token: 0x06000166 RID: 358 RVA: 0x00007E3C File Offset: 0x0000603C
	public override void CalculateOffset()
	{
		this.positionOffset = this.boneBase.InverseTransformPoint(this.boneBase.position) - this.boneBase.InverseTransformPoint(this.target.position);
		this.rotationOffset = Quaternion.Inverse(this.target.rotation) * this.boneBase.rotation;
	}
}
