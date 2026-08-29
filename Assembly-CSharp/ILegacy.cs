using System;

// Token: 0x02000054 RID: 84
public interface ILegacy
{
	// Token: 0x1700008E RID: 142
	// (get) Token: 0x06000242 RID: 578
	// (set) Token: 0x06000243 RID: 579
	bool legacyInitialized { get; set; }

	// Token: 0x06000244 RID: 580
	void SetLegacy(bool legacy);

	// Token: 0x06000245 RID: 581
	void InitLegacy();

	// Token: 0x06000246 RID: 582
	bool LegacyItemExists();
}
