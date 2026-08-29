using System;
using Newtonsoft.Json;
using ProtoBuf;

// Token: 0x020000CF RID: 207
[ProtoContract]
[Serializable]
public class RDP
{
	// Token: 0x17000114 RID: 276
	// (get) Token: 0x06000743 RID: 1859 RVA: 0x0002558B File Offset: 0x0002378B
	// (set) Token: 0x06000744 RID: 1860 RVA: 0x00025593 File Offset: 0x00023793
	[ProtoMember(1)]
	public int tick { get; set; }

	// Token: 0x17000115 RID: 277
	// (get) Token: 0x06000745 RID: 1861 RVA: 0x0002559C File Offset: 0x0002379C
	// (set) Token: 0x06000746 RID: 1862 RVA: 0x000255A4 File Offset: 0x000237A4
	[ProtoMember(2)]
	public int id { get; set; }

	// Token: 0x17000116 RID: 278
	// (get) Token: 0x06000747 RID: 1863 RVA: 0x000255AD File Offset: 0x000237AD
	// (set) Token: 0x06000748 RID: 1864 RVA: 0x000255B5 File Offset: 0x000237B5
	[ProtoMember(3)]
	public int sd { get; set; } = -1;

	// Token: 0x17000117 RID: 279
	// (get) Token: 0x06000749 RID: 1865 RVA: 0x000255BE File Offset: 0x000237BE
	// (set) Token: 0x0600074A RID: 1866 RVA: 0x000255C6 File Offset: 0x000237C6
	[ProtoMember(4)]
	public DamageOrigin? dO { get; set; }

	// Token: 0x17000118 RID: 280
	// (get) Token: 0x0600074B RID: 1867 RVA: 0x000255CF File Offset: 0x000237CF
	// (set) Token: 0x0600074C RID: 1868 RVA: 0x000255D7 File Offset: 0x000237D7
	[JsonIgnore]
	public WeaponDamageablePart WeaponDamageablePart { get; set; }
}
