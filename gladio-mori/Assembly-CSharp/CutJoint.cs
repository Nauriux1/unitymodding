using System;
using UnityEngine;

// Token: 0x02000184 RID: 388
public class CutJoint
{
	// Token: 0x17000184 RID: 388
	// (get) Token: 0x06000C4E RID: 3150 RVA: 0x0003C0A2 File Offset: 0x0003A2A2
	// (set) Token: 0x06000C4F RID: 3151 RVA: 0x0003C0AA File Offset: 0x0003A2AA
	public ConfigurableJoint joint { get; set; }

	// Token: 0x17000185 RID: 389
	// (get) Token: 0x06000C50 RID: 3152 RVA: 0x0003C0B3 File Offset: 0x0003A2B3
	// (set) Token: 0x06000C51 RID: 3153 RVA: 0x0003C0BB File Offset: 0x0003A2BB
	public bool side { get; set; }

	// Token: 0x17000186 RID: 390
	// (get) Token: 0x06000C52 RID: 3154 RVA: 0x0003C0C4 File Offset: 0x0003A2C4
	// (set) Token: 0x06000C53 RID: 3155 RVA: 0x0003C0CC File Offset: 0x0003A2CC
	public bool isParentJoint { get; set; }
}
