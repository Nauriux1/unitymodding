using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200006C RID: 108
[Serializable]
public class CuttableMesh
{
	// Token: 0x06000310 RID: 784 RVA: 0x0000FF4C File Offset: 0x0000E14C
	public void SetCuttableSectionIndex(List<CuttableSection> cuttableSections)
	{
		this.cuttableSectionIndex = -1;
		if (this.meshFilter == null)
		{
			return;
		}
		GameObject gameObject = this.meshFilter.transform.parent.gameObject;
		for (int i = 0; i < cuttableSections.Count; i++)
		{
			CuttableSection cuttableSection = cuttableSections[i];
			if (cuttableSection.joint != null && cuttableSection.joint.connectedBody != null && cuttableSection.joint.connectedBody.gameObject == gameObject)
			{
				this.cuttableSectionIndex = i;
				return;
			}
		}
	}

	// Token: 0x06000311 RID: 785 RVA: 0x0000FFDF File Offset: 0x0000E1DF
	public CuttableMesh GetCopy()
	{
		return new CuttableMesh
		{
			meshFilter = this.meshFilter,
			renderer = this.renderer,
			ignoreInCheck = this.ignoreInCheck,
			cuttableSectionIndex = this.cuttableSectionIndex
		};
	}

	// Token: 0x04000201 RID: 513
	public MeshFilter meshFilter;

	// Token: 0x04000202 RID: 514
	public MeshRenderer renderer;

	// Token: 0x04000203 RID: 515
	public bool ignoreInCheck;

	// Token: 0x04000204 RID: 516
	public int cuttableSectionIndex = -1;
}
