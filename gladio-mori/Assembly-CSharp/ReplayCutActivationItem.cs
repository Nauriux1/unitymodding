using System;
using System.Collections.Generic;

// Token: 0x020000D4 RID: 212
public class ReplayCutActivationItem
{
	// Token: 0x040004F6 RID: 1270
	public CutItem CutItem;

	// Token: 0x040004F7 RID: 1271
	public CuttableJobSection[] cutSections;

	// Token: 0x040004F8 RID: 1272
	public List<CuttableMesh> allCuttableMeshs;

	// Token: 0x040004F9 RID: 1273
	public List<CuttableMesh> originalCuttableMeshs;

	// Token: 0x040004FA RID: 1274
	public List<CuttableMesh> newCuttableMeshs;

	// Token: 0x040004FB RID: 1275
	public List<WeaponDamageableArteryCut> arteryCuts;
}
