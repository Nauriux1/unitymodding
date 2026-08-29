using System;
using System.Collections.Generic;

// Token: 0x0200004E RID: 78
public interface IWeaponDamageable
{
	// Token: 0x06000225 RID: 549
	void Destory(DamageOrigin? damageOrigin = null, bool playEffects = true);

	// Token: 0x06000226 RID: 550
	bool IsOrgan();

	// Token: 0x06000227 RID: 551
	bool IsBone();

	// Token: 0x1700008A RID: 138
	// (get) Token: 0x06000228 RID: 552
	// (set) Token: 0x06000229 RID: 553
	List<BladePaintable> bladePaintables { get; set; }

	// Token: 0x0600022A RID: 554
	List<CuttableGameObject> GetCuttableGameObjects();
}
