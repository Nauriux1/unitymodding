using System;
using UnityEngine;

// Token: 0x02000103 RID: 259
public class LightMapReMapper : MonoBehaviour
{
	// Token: 0x06000876 RID: 2166 RVA: 0x00029DBF File Offset: 0x00027FBF
	private void Awake()
	{
		this.reMapRenderer.lightmapScaleOffset = this.scaleOffset;
	}

	// Token: 0x040005D8 RID: 1496
	public Vector4 scaleOffset;

	// Token: 0x040005D9 RID: 1497
	public Renderer reMapRenderer;
}
