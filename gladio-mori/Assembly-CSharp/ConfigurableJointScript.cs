using System;
using UnityEngine;

// Token: 0x0200003F RID: 63
public class ConfigurableJointScript : MonoBehaviour
{
	// Token: 0x060001EA RID: 490 RVA: 0x0000777A File Offset: 0x0000597A
	private void Start()
	{
	}

	// Token: 0x060001EB RID: 491 RVA: 0x0000B448 File Offset: 0x00009648
	private void Update()
	{
		if (this.hj != null && this.target != null)
		{
			Quaternion quaternion = default(Quaternion);
			string a = this.firstRotation;
			if (!(a == "x"))
			{
				if (!(a == "y"))
				{
					if (!(a == "z"))
					{
						quaternion.x = this.target.transform.localRotation.x;
					}
					else
					{
						quaternion.x = this.target.transform.localRotation.z;
					}
				}
				else
				{
					quaternion.x = this.target.transform.localRotation.y;
				}
			}
			else
			{
				quaternion.x = this.target.transform.localRotation.x;
			}
			a = this.secondRotation;
			if (!(a == "x"))
			{
				if (!(a == "y"))
				{
					if (!(a == "z"))
					{
						quaternion.y = this.target.transform.localRotation.x;
					}
					else
					{
						quaternion.y = this.target.transform.localRotation.z;
					}
				}
				else
				{
					quaternion.y = this.target.transform.localRotation.y;
				}
			}
			else
			{
				quaternion.y = this.target.transform.localRotation.x;
			}
			a = this.thirdRotation;
			if (!(a == "x"))
			{
				if (!(a == "y"))
				{
					if (!(a == "z"))
					{
						quaternion.z = this.target.transform.localRotation.x;
					}
					else
					{
						quaternion.z = this.target.transform.localRotation.z;
					}
				}
				else
				{
					quaternion.z = this.target.transform.localRotation.y;
				}
			}
			else
			{
				quaternion.z = this.target.transform.localRotation.x;
			}
			if (this.invertFirst)
			{
				quaternion.x = -quaternion.x;
			}
			if (this.invertSecond)
			{
				quaternion.y = -quaternion.y;
			}
			if (this.invertThird)
			{
				quaternion.z = -quaternion.z;
			}
			quaternion.w = this.target.transform.localRotation.w;
			this.hj.targetRotation = quaternion;
		}
	}

	// Token: 0x060001EC RID: 492 RVA: 0x0000B6DB File Offset: 0x000098DB
	public void DisableConfigurableJointScript()
	{
		base.enabled = false;
	}

	// Token: 0x04000136 RID: 310
	public ConfigurableJoint hj;

	// Token: 0x04000137 RID: 311
	public Transform target;

	// Token: 0x04000138 RID: 312
	public string firstRotation = "x";

	// Token: 0x04000139 RID: 313
	public bool invertFirst;

	// Token: 0x0400013A RID: 314
	public string secondRotation = "y";

	// Token: 0x0400013B RID: 315
	public bool invertSecond;

	// Token: 0x0400013C RID: 316
	public string thirdRotation = "z";

	// Token: 0x0400013D RID: 317
	public bool invertThird;

	// Token: 0x0400013E RID: 318
	public JointStrength jointStrength;
}
