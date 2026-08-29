using System;
using UnityEngine;
using Utils;

// Token: 0x02000029 RID: 41
public class TwoHandPositionRig : SimpleRig
{
	// Token: 0x0600017C RID: 380 RVA: 0x00007B22 File Offset: 0x00005D22
	protected override void Initialize()
	{
		base.Initialize();
	}

	// Token: 0x0600017D RID: 381 RVA: 0x00008A60 File Offset: 0x00006C60
	public void SetActive(bool active)
	{
		base.gameObject.SetActive(active);
		if (active)
		{
			float directionDotProductBetweenTwoTransforms = Generic.GetDirectionDotProductBetweenTwoTransforms(this.boneBase, this.boneBase2);
			float directionDotProductBetweenTwoTransforms2 = Generic.GetDirectionDotProductBetweenTwoTransforms(this.boneBase2, this.boneBase);
			float d = (float)((directionDotProductBetweenTwoTransforms > 0f) ? 1 : -1);
			float d2 = (float)((directionDotProductBetweenTwoTransforms2 > 0f) ? -1 : 1);
			this.CalculateTargetPosition();
			this.boneBase.parent = this.target;
			this.boneBase2.parent = this.target;
			this.boneBase.localPosition = new Vector3(0f, this.boneBase.localPosition.y, 0f);
			this.boneBase2.localPosition = new Vector3(0f, this.boneBase2.localPosition.y, 0f);
			this.boneBase.rotation = Quaternion.LookRotation(this.target.up * d, this.target.right * d);
			this.boneBase2.rotation = Quaternion.LookRotation(this.target.up * d2, this.target.right * d2 * -1f);
			return;
		}
		this.boneBase.parent = this.target.parent;
		this.boneBase2.parent = this.target.parent;
	}

	// Token: 0x0600017E RID: 382 RVA: 0x00008BD4 File Offset: 0x00006DD4
	public override void CalculateTargetPosition()
	{
		this.target.position = (this.boneBase.position + this.boneBase2.position) / 2f;
		Vector3 normalized = (this.boneBase2.position - this.boneBase.position).normalized;
		Quaternion rhs = Quaternion.Euler(90f, 0f, 0f);
		this.target.rotation = Quaternion.LookRotation(normalized, this.boneBase2.right) * rhs;
	}

	// Token: 0x040000B3 RID: 179
	public Transform boneBase2;
}
