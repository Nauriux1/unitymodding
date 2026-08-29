using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

// Token: 0x0200014A RID: 330
public class MultiplayerPlayerSetup : NetworkBehaviour
{
	// Token: 0x06000A45 RID: 2629 RVA: 0x0003090B File Offset: 0x0002EB0B
	private void Start()
	{
		if (base.isClientOnly)
		{
			this.DisablePhysics();
		}
	}

	// Token: 0x06000A46 RID: 2630 RVA: 0x0003091C File Offset: 0x0002EB1C
	public void DisablePhysics()
	{
		foreach (Rigidbody rigidbody in this.rigidbodies)
		{
			rigidbody.isKinematic = true;
			rigidbody.interpolation = RigidbodyInterpolation.None;
		}
	}

	// Token: 0x06000A47 RID: 2631 RVA: 0x00030974 File Offset: 0x0002EB74
	public void FillList()
	{
		this.rigidbodies = base.gameObject.GetComponentsInChildren<Rigidbody>().ToList<Rigidbody>();
	}

	// Token: 0x06000A49 RID: 2633 RVA: 0x0000C7D7 File Offset: 0x0000A9D7
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x0400073D RID: 1853
	public List<Rigidbody> rigidbodies;
}
