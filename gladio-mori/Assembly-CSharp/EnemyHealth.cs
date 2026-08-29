using System;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x02000043 RID: 67
public class EnemyHealth : MonoBehaviour
{
	// Token: 0x060001FA RID: 506 RVA: 0x0000B959 File Offset: 0x00009B59
	private void Start()
	{
		this.currentHealth = this.startingHealth;
		this.pathfinder = base.GetComponent<NavMeshAgent>();
		this.playerHead = GameObject.FindWithTag("PlayerHead").transform;
		this.OnStart();
	}

	// Token: 0x060001FB RID: 507 RVA: 0x0000777A File Offset: 0x0000597A
	public virtual void OnStart()
	{
	}

	// Token: 0x060001FC RID: 508 RVA: 0x0000B98E File Offset: 0x00009B8E
	private void Update()
	{
		this.OnUpdate();
	}

	// Token: 0x060001FD RID: 509 RVA: 0x0000777A File Offset: 0x0000597A
	public virtual void OnUpdate()
	{
	}

	// Token: 0x060001FE RID: 510 RVA: 0x0000B998 File Offset: 0x00009B98
	public void Damage(int damage, RaycastHit HitPoint, RaycastHit? OutHit, Ray? HitRay)
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

	// Token: 0x060001FF RID: 511 RVA: 0x0000BA40 File Offset: 0x00009C40
	private void Defeated()
	{
		this.alive = false;
		base.GetComponent<Rigidbody>().useGravity = true;
		if (this.pathfinder != null)
		{
			this.pathfinder.enabled = false;
		}
	}

	// Token: 0x06000200 RID: 512 RVA: 0x0000BA6F File Offset: 0x00009C6F
	public void MeleeDamage(int damage)
	{
		this.currentHealth -= damage;
		if (this.currentHealth <= 0)
		{
			this.Defeated();
		}
	}

	// Token: 0x06000201 RID: 513 RVA: 0x0000BA8E File Offset: 0x00009C8E
	public void SetCollidingBlade(GameObject blade)
	{
		this.collidingBlade = blade;
	}

	// Token: 0x0400014C RID: 332
	public int startingHealth = 3;

	// Token: 0x0400014D RID: 333
	public GameObject outBloodParticles;

	// Token: 0x0400014E RID: 334
	public GameObject inBloodParticles;

	// Token: 0x0400014F RID: 335
	public bool alive = true;

	// Token: 0x04000150 RID: 336
	public Transform playerHead;

	// Token: 0x04000151 RID: 337
	public int currentHealth;

	// Token: 0x04000152 RID: 338
	public GameObject collidingBlade;

	// Token: 0x04000153 RID: 339
	public NavMeshAgent pathfinder;
}
