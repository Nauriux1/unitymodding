using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000186 RID: 390
public class SubMesh
{
	// Token: 0x17000188 RID: 392
	// (get) Token: 0x06000C58 RID: 3160 RVA: 0x0003C169 File Offset: 0x0003A369
	// (set) Token: 0x06000C59 RID: 3161 RVA: 0x0003C171 File Offset: 0x0003A371
	public int submeshNum { get; set; }

	// Token: 0x17000189 RID: 393
	// (get) Token: 0x06000C5A RID: 3162 RVA: 0x0003C17A File Offset: 0x0003A37A
	// (set) Token: 0x06000C5B RID: 3163 RVA: 0x0003C182 File Offset: 0x0003A382
	public Material material { get; set; }

	// Token: 0x040008C2 RID: 2242
	public List<int> triangles = new List<int>();
}
