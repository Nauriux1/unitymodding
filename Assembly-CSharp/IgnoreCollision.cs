using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000147 RID: 327
public class IgnoreCollision : MonoBehaviour
{
	// Token: 0x06000A25 RID: 2597 RVA: 0x0002FDFF File Offset: 0x0002DFFF
	private void Awake()
	{
		this.IgnoreCollisions();
	}

	// Token: 0x06000A26 RID: 2598 RVA: 0x0000777A File Offset: 0x0000597A
	private void Update()
	{
	}

	// Token: 0x06000A27 RID: 2599 RVA: 0x0002FE08 File Offset: 0x0002E008
	private void IgnoreCollisions()
	{
		if (this.ignoreColliders != null)
		{
			foreach (Collider collider in this.ignoreColliders)
			{
				foreach (Collider collider2 in this.localColliders)
				{
					Physics.IgnoreCollision(collider, collider2, true);
				}
			}
		}
	}

	// Token: 0x06000A28 RID: 2600 RVA: 0x0002FEA0 File Offset: 0x0002E0A0
	public void IgnoreWithLocalCollider(List<Collider> newColliders)
	{
		if (this.localColliders != null && newColliders != null)
		{
			foreach (Collider collider in this.localColliders)
			{
				foreach (Collider collider2 in newColliders)
				{
					Physics.IgnoreCollision(collider, collider2, true);
				}
			}
		}
	}

	// Token: 0x06000A29 RID: 2601 RVA: 0x0002FF38 File Offset: 0x0002E138
	public void IgnoreWithOutsideColliders(List<Collider> newColliders)
	{
		if (this.ignoreColliders != null && newColliders != null)
		{
			foreach (Collider collider in this.ignoreColliders)
			{
				foreach (Collider collider2 in newColliders)
				{
					Physics.IgnoreCollision(collider, collider2, true);
				}
			}
		}
	}

	// Token: 0x06000A2A RID: 2602 RVA: 0x0002FFD0 File Offset: 0x0002E1D0
	public void IgnoreArmour(List<Armour> localArmours, List<Armour> outsideArmours)
	{
		List<Collider> list = new List<Collider>();
		List<Collider> list2 = new List<Collider>();
		foreach (Armour armour in localArmours)
		{
			list.AddRange(armour.colliders);
		}
		foreach (Armour armour2 in outsideArmours)
		{
			list2.AddRange(armour2.colliders);
		}
		this.IgnoreWithLocalCollider(list2);
		this.IgnoreWithOutsideColliders(list);
		if (list != null && list2 != null)
		{
			foreach (Collider collider in list)
			{
				foreach (Collider collider2 in list2)
				{
					Physics.IgnoreCollision(collider, collider2, true);
				}
			}
		}
	}

	// Token: 0x04000729 RID: 1833
	public List<Collider> ignoreColliders = new List<Collider>();

	// Token: 0x0400072A RID: 1834
	public List<Collider> localColliders = new List<Collider>();
}
