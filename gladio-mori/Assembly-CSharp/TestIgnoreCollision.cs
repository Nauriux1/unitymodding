using System;
using UnityEngine;

// Token: 0x02000191 RID: 401
public class TestIgnoreCollision : MonoBehaviour
{
	// Token: 0x06000C81 RID: 3201 RVA: 0x0003CDE5 File Offset: 0x0003AFE5
	private void Start()
	{
		this.rb = base.transform.GetComponent<Rigidbody>();
	}

	// Token: 0x06000C82 RID: 3202 RVA: 0x0000777A File Offset: 0x0000597A
	private void Update()
	{
	}

	// Token: 0x06000C83 RID: 3203 RVA: 0x0003CDF8 File Offset: 0x0003AFF8
	private void FixedUpdate()
	{
		this.lastPosition = base.transform.position;
		this.lastVelocity = this.rb.velocity;
		this.lastAngularVelocity = this.rb.angularVelocity;
	}

	// Token: 0x06000C84 RID: 3204 RVA: 0x0003CE30 File Offset: 0x0003B030
	private void OnCollisionEnter(Collision collision)
	{
		Physics.IgnoreCollision(collision.collider, base.gameObject.GetComponent<Collider>(), true);
		base.transform.position = this.lastPosition;
		this.rb.velocity = this.lastVelocity;
		this.rb.angularVelocity = this.lastAngularVelocity;
	}

	// Token: 0x040008E9 RID: 2281
	private Vector3 lastPosition;

	// Token: 0x040008EA RID: 2282
	private Vector3 lastVelocity;

	// Token: 0x040008EB RID: 2283
	private Vector3 lastAngularVelocity;

	// Token: 0x040008EC RID: 2284
	private Rigidbody rb;
}
