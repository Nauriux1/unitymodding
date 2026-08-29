using System;
using UnityEngine;

// Token: 0x02000044 RID: 68
public class Stinger : EnemyHealth
{
	// Token: 0x06000203 RID: 515 RVA: 0x0000BAB0 File Offset: 0x00009CB0
	private void Start()
	{
		this.currentHealth = this.startingHealth;
		this.playerHead = GameObject.FindWithTag("PlayerHead").transform;
		this.thisRigidBody = base.GetComponent<Rigidbody>();
		this.defaultDrag = this.thisRigidBody.drag;
		this.defaultAngularDrag = this.thisRigidBody.angularDrag;
	}

	// Token: 0x06000204 RID: 516 RVA: 0x0000BB0C File Offset: 0x00009D0C
	public new void Damage(int damage, RaycastHit HitPoint, RaycastHit? OutHit, Ray? HitRay)
	{
		if (OutHit != null && this.outBloodParticles != null)
		{
			Quaternion.FromToRotation(Vector3.forward, OutHit.Value.normal);
			Quaternion.LookRotation(OutHit.Value.point - HitPoint.point, HitPoint.transform.forward);
		}
		if (this.inBloodParticles != null)
		{
			Quaternion.FromToRotation(Vector3.up, HitPoint.normal);
		}
		this.currentHealth -= damage;
		if (this.currentHealth <= 0)
		{
			this.Defeated();
		}
	}

	// Token: 0x06000205 RID: 517 RVA: 0x0000BBB4 File Offset: 0x00009DB4
	private void Defeated()
	{
		base.GetComponent<Rigidbody>().useGravity = true;
		this.alive = false;
	}

	// Token: 0x06000206 RID: 518 RVA: 0x0000BBC9 File Offset: 0x00009DC9
	public new void MeleeDamage(int damage)
	{
		this.currentHealth -= damage;
		if (this.currentHealth <= 0)
		{
			this.Defeated();
		}
	}

	// Token: 0x06000207 RID: 519 RVA: 0x0000BBE8 File Offset: 0x00009DE8
	public new void SetCollidingBlade(GameObject blade)
	{
		this.thisRigidBody.drag = ((blade != null) ? 20f : this.defaultDrag);
		this.thisRigidBody.angularDrag = ((blade != null) ? 20f : this.defaultAngularDrag);
		this.collidingBlade = blade;
	}

	// Token: 0x06000208 RID: 520 RVA: 0x0000BC40 File Offset: 0x00009E40
	private void FixedUpdate()
	{
		float num = Vector3.Magnitude(this.thisRigidBody.velocity);
		if (this.alive)
		{
			base.gameObject.transform.LookAt(new Vector3(this.playerHead.position.x, this.playerHead.position.y, this.playerHead.position.z));
			if (num < this.maximumSpeed)
			{
				float num2 = num / this.maximumSpeed;
				float num3 = 1f - num2;
				Vector3 lhs = this.playerHead.position - base.transform.position;
				Vector3 velocity = this.thisRigidBody.velocity;
				if (Vector3.Dot(lhs, velocity) < 0f)
				{
					num3 = 1f;
				}
				this.thisRigidBody.AddForce((this.playerHead.transform.position - base.transform.position).normalized * (this.speedForce * num3) * Time.smoothDeltaTime);
			}
			if (num > this.maximumSpeed)
			{
				Debug.Log("braking");
				float d = num - this.maximumSpeed;
				Vector3 a = this.thisRigidBody.velocity.normalized * d;
				this.thisRigidBody.AddForce(-a);
				return;
			}
		}
		else if (num < 1f && this.collidingBlade != null && this.joint == null)
		{
			this.joint = base.gameObject.AddComponent<FixedJoint>();
			this.joint.connectedBody = this.collidingBlade.GetComponent<Rigidbody>();
			this.joint.breakForce = 4f;
			this.joint.breakTorque = 4f;
			this.thisRigidBody.drag = this.defaultDrag;
			this.thisRigidBody.angularDrag = this.defaultAngularDrag;
			this.thisRigidBody.mass = 0.01f;
		}
	}

	// Token: 0x04000154 RID: 340
	public Rigidbody thisRigidBody;

	// Token: 0x04000155 RID: 341
	public float defaultDrag;

	// Token: 0x04000156 RID: 342
	public float defaultAngularDrag;

	// Token: 0x04000157 RID: 343
	private float maximumSpeed = 15f;

	// Token: 0x04000158 RID: 344
	private float speedForce = 300f;

	// Token: 0x04000159 RID: 345
	public Joint joint;
}
