using System;
using MoveClasses;
using ProtoBuf;

// Token: 0x020000D8 RID: 216
[ProtoContract]
[Serializable]
public class DE
{
	// Token: 0x1700013E RID: 318
	// (get) Token: 0x060007A3 RID: 1955 RVA: 0x00025F59 File Offset: 0x00024159
	// (set) Token: 0x060007A4 RID: 1956 RVA: 0x00025F61 File Offset: 0x00024161
	[ProtoMember(1)]
	public int tick { get; set; }

	// Token: 0x1700013F RID: 319
	// (get) Token: 0x060007A5 RID: 1957 RVA: 0x00025F6A File Offset: 0x0002416A
	// (set) Token: 0x060007A6 RID: 1958 RVA: 0x00025F72 File Offset: 0x00024172
	[ProtoMember(2)]
	public DeathReason deathReason { get; set; }
}
