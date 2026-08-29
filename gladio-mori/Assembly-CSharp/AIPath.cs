using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x020000AC RID: 172
public class AIPath
{
	// Token: 0x170000FC RID: 252
	// (get) Token: 0x060005D1 RID: 1489 RVA: 0x0001C4C4 File Offset: 0x0001A6C4
	public bool validPath
	{
		get
		{
			bool result = true;
			if (this.pathEndPart != null && this.pathEndPart.status != NavMeshPathStatus.PathComplete)
			{
				result = false;
			}
			if (this.pathStartPart != null && this.pathStartPart.status != NavMeshPathStatus.PathComplete)
			{
				result = false;
			}
			if (this.objectAvoidanceResult != null)
			{
				result = false;
			}
			return result;
		}
	}

	// Token: 0x060005D2 RID: 1490 RVA: 0x0001C50C File Offset: 0x0001A70C
	public void BuildFullPath()
	{
		List<Vector3> list = new List<Vector3>();
		this.pathLength = 0f;
		for (int i = 0; i < this.pathStartPart.corners.Length - 1; i++)
		{
			this.pathLength += Vector3.Distance(this.pathStartPart.corners[i], this.pathStartPart.corners[i + 1]);
			list.Add(this.pathStartPart.corners[i]);
		}
		for (int j = 0; j < this.pathEndPart.corners.Length - 1; j++)
		{
			this.pathLength += Vector3.Distance(this.pathEndPart.corners[j], this.pathEndPart.corners[j + 1]);
			if (j == 0)
			{
				list.Add(this.pathEndPart.corners[j]);
			}
			list.Add(this.pathEndPart.corners[j + 1]);
		}
		this.fullPath = list.ToArray();
	}

	// Token: 0x040003BD RID: 957
	public NavMeshPath pathEndPart;

	// Token: 0x040003BE RID: 958
	public NavMeshPath pathStartPart;

	// Token: 0x040003BF RID: 959
	public Vector3[] fullPath;

	// Token: 0x040003C0 RID: 960
	public ObjectAvoidanceResult objectAvoidanceResult;

	// Token: 0x040003C1 RID: 961
	public float pathLength;
}
