using System;
using UnityEngine;

// Token: 0x0200018F RID: 399
public class SetVelocity : MonoBehaviour
{
	// Token: 0x06000C7A RID: 3194 RVA: 0x0000777A File Offset: 0x0000597A
	private void Start()
	{
	}

	// Token: 0x06000C7B RID: 3195 RVA: 0x0000777A File Offset: 0x0000597A
	private void Update()
	{
	}

	// Token: 0x06000C7C RID: 3196 RVA: 0x0003CB5F File Offset: 0x0003AD5F
	private void FixedUpdate()
	{
		if (this.velocityToSet.magnitude > 0.05f)
		{
			this.objectRigidbody.velocity = this.velocityToSet;
		}
	}

	// Token: 0x040008DF RID: 2271
	public Rigidbody objectRigidbody;

	// Token: 0x040008E0 RID: 2272
	public Vector3 velocityToSet;
}
