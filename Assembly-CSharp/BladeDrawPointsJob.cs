using System;
using TriTriIntersectionHelpers;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

// Token: 0x02000258 RID: 600
[BurstCompile]
public struct BladeDrawPointsJob : IJob
{
	// Token: 0x06001195 RID: 4501 RVA: 0x00059FFB File Offset: 0x000581FB
	public BladeDrawPointsJob(NativeList<BladeTriangle> bladeTriangles, NativeArray<int> tris, NativeArray<Vector3> vertices, NativeList<Vector3> drawWorldPoints, Matrix4x4 localToWorld)
	{
		this._tris = tris;
		this._vertices = vertices;
		this._bladeTriangles = bladeTriangles;
		this._drawWorldPoints = drawWorldPoints;
		this._localToWorldMatrix = localToWorld;
	}

	// Token: 0x06001196 RID: 4502 RVA: 0x0005A024 File Offset: 0x00058224
	public void Execute()
	{
		for (int i = 0; i < this._bladeTriangles.Length; i++)
		{
			BladeTriangle bladeTriangle = this._bladeTriangles[i];
			for (int j = 0; j < this._tris.Length; j += 3)
			{
				int index = this._tris[j];
				int index2 = this._tris[j + 1];
				int index3 = this._tris[j + 2];
				Vector3 p = this._vertices[index];
				Vector3 p2 = this._vertices[index2];
				Vector3 p3 = this._vertices[index3];
				IntersectionInfo intersectionInfo = TriTriIntersection.TrisIntersect(p, p2, p3, bladeTriangle.p0, bladeTriangle.p1, bladeTriangle.p2);
				if (intersectionInfo.intersects)
				{
					Vector3 vector = this._localToWorldMatrix.MultiplyPoint3x4(intersectionInfo.intersectionPoint1);
					Vector3 vector2 = this._localToWorldMatrix.MultiplyPoint3x4(intersectionInfo.intersectionPoint2);
					this._drawWorldPoints.Add(vector);
					this._drawWorldPoints.Add(vector2);
				}
			}
		}
	}

	// Token: 0x04000D2A RID: 3370
	[ReadOnly]
	private NativeArray<int> _tris;

	// Token: 0x04000D2B RID: 3371
	[ReadOnly]
	private NativeArray<Vector3> _vertices;

	// Token: 0x04000D2C RID: 3372
	[ReadOnly]
	private NativeList<BladeTriangle> _bladeTriangles;

	// Token: 0x04000D2D RID: 3373
	[ReadOnly]
	private Matrix4x4 _localToWorldMatrix;

	// Token: 0x04000D2E RID: 3374
	private NativeList<Vector3> _drawWorldPoints;
}
