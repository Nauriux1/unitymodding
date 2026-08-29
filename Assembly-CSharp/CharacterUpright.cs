using System;
using UnityEngine;

// Token: 0x0200003E RID: 62
public class CharacterUpright : MonoBehaviour
{
	// Token: 0x060001E7 RID: 487 RVA: 0x0000B301 File Offset: 0x00009501
	private void Awake()
	{
		this.rigidbody = base.GetComponent<Rigidbody>();
		this.rigidbody.maxAngularVelocity = 40f;
	}

	// Token: 0x060001E8 RID: 488 RVA: 0x0000B320 File Offset: 0x00009520
	private void FixedUpdate()
	{
		if (this.keepUpright)
		{
			this.rigidbody.AddForceAtPosition(new Vector3(0f, this.uprightForce + this.additionalUpwardForce, 0f), base.transform.position + base.transform.TransformPoint(new Vector3(0f, this.uprightOffset, 0f)), ForceMode.Force);
			this.rigidbody.AddForceAtPosition(new Vector3(0f, -this.uprightForce, 0f), base.transform.position + base.transform.TransformPoint(new Vector3(0f, -this.uprightOffset, 0f)), ForceMode.Force);
		}
		if (this.dampenAngularForce > 0f)
		{
			this.rigidbody.angularVelocity *= 1f - Time.deltaTime * this.dampenAngularForce;
		}
	}

	// Token: 0x04000130 RID: 304
	protected Rigidbody rigidbody;

	// Token: 0x04000131 RID: 305
	public bool keepUpright = true;

	// Token: 0x04000132 RID: 306
	public float uprightForce = 10f;

	// Token: 0x04000133 RID: 307
	public float uprightOffset = 1.45f;

	// Token: 0x04000134 RID: 308
	public float additionalUpwardForce = 10f;

	// Token: 0x04000135 RID: 309
	public float dampenAngularForce;
}
