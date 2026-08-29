using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Es.InkPainter
{
	// Token: 0x020002F6 RID: 758
	public class MeshOperator
	{
		// Token: 0x06001713 RID: 5907 RVA: 0x00074CC8 File Offset: 0x00072EC8
		public MeshOperator(Mesh mesh)
		{
			if (mesh == null)
			{
				throw new ArgumentNullException("mesh");
			}
			this.mesh = mesh;
			this.meshTriangles = this.mesh.triangles;
			this.meshVertices = this.mesh.vertices;
			this.meshUV = this.mesh.uv;
		}

		// Token: 0x06001714 RID: 5908 RVA: 0x00074D2C File Offset: 0x00072F2C
		public bool LocalPointToUV(Vector3 localPoint, Matrix4x4 matrixMVP, out Vector2 uv)
		{
			for (int i = 0; i < this.meshTriangles.Length; i += 3)
			{
				int num = i;
				int num2 = i + 1;
				int num3 = i + 2;
				Vector3 t = this.meshVertices[this.meshTriangles[num]];
				Vector3 t2 = this.meshVertices[this.meshTriangles[num2]];
				Vector3 t3 = this.meshVertices[this.meshTriangles[num3]];
				if (Math.ExistPointInPlane(localPoint, t, t2, t3) && (Math.ExistPointOnTriangleEdge(localPoint, t, t2, t3) || Math.ExistPointInTriangle(localPoint, t, t2, t3)))
				{
					Vector2 t1UV = this.meshUV[this.meshTriangles[num]];
					Vector2 t2UV = this.meshUV[this.meshTriangles[num2]];
					Vector2 t3UV = this.meshUV[this.meshTriangles[num3]];
					uv = Math.TextureCoordinateCalculation(localPoint, t, t1UV, t2, t2UV, t3, t3UV, matrixMVP);
					return true;
				}
			}
			uv = default(Vector3);
			return false;
		}

		// Token: 0x06001715 RID: 5909 RVA: 0x00074E40 File Offset: 0x00073040
		public Vector3 NearestLocalSurfacePoint(Vector3 localPoint)
		{
			Vector3[] nearestVerticesTriangle = Math.GetNearestVerticesTriangle(localPoint, this.meshVertices, this.meshTriangles);
			List<Vector3> list = new List<Vector3>();
			for (int i = 0; i < nearestVerticesTriangle.Length; i += 3)
			{
				int num = i;
				int num2 = i + 1;
				int num3 = i + 2;
				list.Add(Math.TriangleSpaceProjection(localPoint, nearestVerticesTriangle[num], nearestVerticesTriangle[num2], nearestVerticesTriangle[num3]));
			}
			return (from t in list
			orderby Vector3.Distance(localPoint, t)
			select t).First<Vector3>();
		}

		// Token: 0x040010E4 RID: 4324
		private Mesh mesh;

		// Token: 0x040010E5 RID: 4325
		private int[] meshTriangles;

		// Token: 0x040010E6 RID: 4326
		private Vector3[] meshVertices;

		// Token: 0x040010E7 RID: 4327
		private Vector2[] meshUV;
	}
}
