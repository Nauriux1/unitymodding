using System;
using UnityEngine;

// Token: 0x0200017F RID: 383
public class CalculateImpactForce : MonoBehaviour
{
	// Token: 0x06000C30 RID: 3120 RVA: 0x0003A285 File Offset: 0x00038485
	private void Start()
	{
		this.thisCollider = base.GetComponent<Collider>();
	}

	// Token: 0x06000C31 RID: 3121 RVA: 0x0000777A File Offset: 0x0000597A
	private void Update()
	{
	}

	// Token: 0x06000C32 RID: 3122 RVA: 0x0003A294 File Offset: 0x00038494
	public virtual void OnTriggerEnter(Collider collision)
	{
		if (collision.attachedRigidbody != null && !this.collisionDone && this.parentRigidbody != null)
		{
			this.collisionDone = true;
			Vector3 relativePointVelocity = collision.attachedRigidbody.GetRelativePointVelocity(this.parentRigidbody.transform.position);
			float magnitude = (this.parentRigidbody.GetRelativePointVelocity(collision.attachedRigidbody.transform.position) + relativePointVelocity).magnitude;
			Debug.Log(string.Format("CollisionMagnitude1:{0}", magnitude));
			Vector3 vector = collision.ClosestPoint(this.parentRigidbody.transform.position);
			Vector3 vector2 = this.thisCollider.ClosestPoint(collision.attachedRigidbody.transform.position);
			Vector3 vector3 = (vector + vector2) / 2f;
			Vector3 pointVelocity = collision.attachedRigidbody.GetPointVelocity(vector3);
			Vector3 pointVelocity2 = this.parentRigidbody.GetPointVelocity(vector3);
			float magnitude2 = (pointVelocity - pointVelocity2).magnitude;
			Debug.Log(string.Format("collisionMagnitude2:{0}", magnitude2));
			Debug.DrawRay(vector, pointVelocity, Color.blue, 1000f);
			Debug.DrawRay(vector2, pointVelocity2, Color.red, 1000f);
			Debug.DrawRay(vector3, pointVelocity - pointVelocity2, Color.green, 1000f);
		}
	}

	// Token: 0x06000C33 RID: 3123 RVA: 0x0003A3F8 File Offset: 0x000385F8
	public void OnCollisionEnter(Collision collision)
	{
		if (this.doCollision)
		{
			this.doCollision = false;
			Debug.Log(string.Format("Unity collision inpulse:{0}", collision.impulse.magnitude));
			Debug.Log(string.Format("Unity collision velocity:{0}", collision.relativeVelocity.magnitude));
		}
	}

	// Token: 0x0400089B RID: 2203
	public Rigidbody parentRigidbody;

	// Token: 0x0400089C RID: 2204
	public Collider thisCollider;

	// Token: 0x0400089D RID: 2205
	public bool collisionDone;

	// Token: 0x0400089E RID: 2206
	private bool doCollision = true;
}
