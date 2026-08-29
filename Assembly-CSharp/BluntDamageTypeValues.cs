using System;

// Token: 0x02000032 RID: 50
public struct BluntDamageTypeValues
{
	// Token: 0x060001A5 RID: 421 RVA: 0x00009DFE File Offset: 0x00007FFE
	public float GetOverrideMaxPermanentDamage(bool armoured)
	{
		if (armoured)
		{
			return this.overrideMaxPermanentDamageValueArmoured;
		}
		return this.overrideMaxPermanentDamageValue;
	}

	// Token: 0x060001A6 RID: 422 RVA: 0x00009E10 File Offset: 0x00008010
	public float GetOverrideMaxTemporaryDamage(bool armoured)
	{
		if (armoured)
		{
			return this.overrideMaxTemporaryDamageValueArmoured;
		}
		return this.overrideMaxTemporaryDamageValue;
	}

	// Token: 0x040000DD RID: 221
	public bool overrideMaxPermanentDamage;

	// Token: 0x040000DE RID: 222
	public bool overriderMaxTemporaryDamage;

	// Token: 0x040000DF RID: 223
	public float permanentDamageMultiplier;

	// Token: 0x040000E0 RID: 224
	public float permanentDamageMultiplierArmoured;

	// Token: 0x040000E1 RID: 225
	public float overrideMaxPermanentDamageValue;

	// Token: 0x040000E2 RID: 226
	public float overrideMaxPermanentDamageValueArmoured;

	// Token: 0x040000E3 RID: 227
	public float temporaryDamageMultiplier;

	// Token: 0x040000E4 RID: 228
	public float temporaryDamageMultiplierArmoured;

	// Token: 0x040000E5 RID: 229
	public float overrideMaxTemporaryDamageValue;

	// Token: 0x040000E6 RID: 230
	public float overrideMaxTemporaryDamageValueArmoured;

	// Token: 0x040000E7 RID: 231
	public float permanentDamageResistancePenetration;

	// Token: 0x040000E8 RID: 232
	public float temporaryDamageResistancePenetration;
}
