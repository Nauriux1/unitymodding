using System;
using Mirror;
using UnityEngine;

// Token: 0x02000253 RID: 595
public class MultiplayerRigidbody : NetworkBehaviour
{
	// Token: 0x0600117B RID: 4475 RVA: 0x00059850 File Offset: 0x00057A50
	private void Start()
	{
		if (base.isClientOnly)
		{
			Rigidbody component = base.GetComponent<Rigidbody>();
			if (component != null)
			{
				component.isKinematic = true;
				component.interpolation = RigidbodyInterpolation.None;
			}
		}
	}

	// Token: 0x0600117C RID: 4476 RVA: 0x0000777A File Offset: 0x0000597A
	private void Update()
	{
	}

	// Token: 0x0600117E RID: 4478 RVA: 0x0000C7D7 File Offset: 0x0000A9D7
	public override bool Weaved()
	{
		return true;
	}
}
