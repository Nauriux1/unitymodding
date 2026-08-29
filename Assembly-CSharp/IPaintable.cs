using System;
using UnityEngine;

// Token: 0x02000178 RID: 376
public interface IPaintable
{
	// Token: 0x1700017C RID: 380
	// (get) Token: 0x06000BFF RID: 3071
	// (set) Token: 0x06000C00 RID: 3072
	float extendsIslandOffset { get; set; }

	// Token: 0x06000C01 RID: 3073
	RenderTexture getMask();

	// Token: 0x06000C02 RID: 3074
	RenderTexture getUVIslands();

	// Token: 0x06000C03 RID: 3075
	RenderTexture getExtend();

	// Token: 0x06000C04 RID: 3076
	RenderTexture getSupport();

	// Token: 0x06000C05 RID: 3077
	Renderer getRenderer();
}
