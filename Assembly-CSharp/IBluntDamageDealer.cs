using System;
using UnityEngine;

// Token: 0x02000035 RID: 53
public interface IBluntDamageDealer
{
	// Token: 0x060001AA RID: 426
	PlayerHealth GetPlayerHealth();

	// Token: 0x060001AB RID: 427
	Rigidbody GetRigidbody();

	// Token: 0x060001AC RID: 428
	BluntDamageDealer GetBluntDamageDealer();
}
