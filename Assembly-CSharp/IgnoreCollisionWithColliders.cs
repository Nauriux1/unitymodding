using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200013D RID: 317
public class IgnoreCollisionWithColliders : MonoBehaviour
{
	// Token: 0x060009E8 RID: 2536 RVA: 0x0002EB38 File Offset: 0x0002CD38
	private void Awake()
	{
		this.localCollider = base.gameObject.GetComponent<Collider>();
		if (this.localCollider != null)
		{
			foreach (Collider collider in this.ignoreColliders)
			{
				Physics.IgnoreCollision(this.localCollider, collider, true);
			}
		}
	}

	// Token: 0x040006E6 RID: 1766
	public List<Collider> ignoreColliders = new List<Collider>();

	// Token: 0x040006E7 RID: 1767
	private Collider localCollider;
}
