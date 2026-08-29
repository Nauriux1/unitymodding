using System;
using System.Collections.Generic;
using UnityEngine;

namespace Es.InkPainter
{
	// Token: 0x020002F4 RID: 756
	public static class Math
	{
		// Token: 0x0600170A RID: 5898 RVA: 0x00074718 File Offset: 0x00072918
		public static bool ExistPointInPlane(Vector3 p, Vector3 t1, Vector3 t2, Vector3 t3)
		{
			Vector3 lhs = t2 - t1;
			Vector3 rhs = t3 - t1;
			Vector3 vector = p - t1;
			float num = Vector3.Dot(Vector3.Cross(lhs, rhs).normalized, vector.normalized);
			return -0.01f < num && num < 0.01f;
		}

		// Token: 0x0600170B RID: 5899 RVA: 0x0007476C File Offset: 0x0007296C
		public static bool ExistPointOnEdge(Vector3 p, Vector3 v1, Vector3 v2)
		{
			return 0.99f < Vector3.Dot((v2 - p).normalized, (v2 - v1).normalized);
		}

		// Token: 0x0600170C RID: 5900 RVA: 0x000747A3 File Offset: 0x000729A3
		public static bool ExistPointOnTriangleEdge(Vector3 p, Vector3 t1, Vector3 t2, Vector3 t3)
		{
			return Math.ExistPointOnEdge(p, t1, t2) || Math.ExistPointOnEdge(p, t2, t3) || Math.ExistPointOnEdge(p, t3, t1);
		}

		// Token: 0x0600170D RID: 5901 RVA: 0x000747C8 File Offset: 0x000729C8
		public static bool ExistPointInTriangle(Vector3 p, Vector3 t1, Vector3 t2, Vector3 t3)
		{
			Vector3 normalized = Vector3.Cross(t1 - t3, p - t1).normalized;
			Vector3 normalized2 = Vector3.Cross(t2 - t1, p - t2).normalized;
			Vector3 normalized3 = Vector3.Cross(t3 - t2, p - t3).normalized;
			float num = Vector3.Dot(normalized, normalized2);
			float num2 = Vector3.Dot(normalized2, normalized3);
			return 0.99f < num && 0.99f < num2;
		}

		// Token: 0x0600170E RID: 5902 RVA: 0x00074850 File Offset: 0x00072A50
		public static Vector2 TextureCoordinateCalculation(Vector3 p, Vector3 t1, Vector2 t1UV, Vector3 t2, Vector2 t2UV, Vector3 t3, Vector2 t3UV, Matrix4x4 transformMatrix)
		{
			Vector4 vector = transformMatrix * new Vector4(t1.x, t1.y, t1.z, 1f);
			Vector4 vector2 = transformMatrix * new Vector4(t2.x, t2.y, t2.z, 1f);
			Vector4 vector3 = transformMatrix * new Vector4(t3.x, t3.y, t3.z, 1f);
			Vector4 vector4 = transformMatrix * new Vector4(p.x, p.y, p.z, 1f);
			Vector2 vector5 = new Vector2(vector.x, vector.y) / vector.w;
			Vector2 vector6 = new Vector2(vector2.x, vector2.y) / vector2.w;
			Vector2 vector7 = new Vector2(vector3.x, vector3.y) / vector3.w;
			Vector2 vector8 = new Vector2(vector4.x, vector4.y) / vector4.w;
			float num = 0.5f * ((vector6.x - vector5.x) * (vector7.y - vector5.y) - (vector6.y - vector5.y) * (vector7.x - vector5.x));
			float num2 = 0.5f * ((vector7.x - vector8.x) * (vector5.y - vector8.y) - (vector7.y - vector8.y) * (vector5.x - vector8.x));
			float num3 = 0.5f * ((vector5.x - vector8.x) * (vector6.y - vector8.y) - (vector5.y - vector8.y) * (vector6.x - vector8.x));
			float num4 = num2 / num;
			float num5 = num3 / num;
			return 1f / ((1f - num4 - num5) * 1f / vector.w + num4 * 1f / vector2.w + num5 * 1f / vector3.w) * ((1f - num4 - num5) * t1UV / vector.w + num4 * t2UV / vector2.w + num5 * t3UV / vector3.w);
		}

		// Token: 0x0600170F RID: 5903 RVA: 0x00074AD8 File Offset: 0x00072CD8
		public static Vector3[] GetNearestVerticesTriangle(Vector3 p, Vector3[] vertices, int[] triangles)
		{
			List<Vector3> list = new List<Vector3>();
			int num = triangles[0];
			float num2 = Vector3.Distance(vertices[num], p);
			for (int i = 0; i < vertices.Length; i++)
			{
				float num3 = Vector3.Distance(vertices[i], p);
				if (num3 < num2)
				{
					num2 = num3;
					num = i;
				}
			}
			for (int j = 0; j < triangles.Length; j++)
			{
				if (triangles[j] == num)
				{
					int num4 = j % 3;
					int num5 = j;
					int num6 = 0;
					int num7 = 0;
					switch (num4)
					{
					case 0:
						num6 = j + 1;
						num7 = j + 2;
						break;
					case 1:
						num6 = j - 1;
						num7 = j + 1;
						break;
					case 2:
						num6 = j - 1;
						num7 = j - 2;
						break;
					}
					list.Add(vertices[triangles[num5]]);
					list.Add(vertices[triangles[num6]]);
					list.Add(vertices[triangles[num7]]);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06001710 RID: 5904 RVA: 0x00074BC8 File Offset: 0x00072DC8
		public static Vector3 TriangleSpaceProjection(Vector3 p, Vector3 t1, Vector3 t2, Vector3 t3)
		{
			Vector3 vector = (t1 + t2 + t3) / 3f;
			Vector3 vector2 = t1 - p;
			Vector3 vector3 = t2 - p;
			Vector3 vector4 = t3 - p;
			Vector3 a = t1 - vector;
			Vector3 a2 = t2 - vector;
			Vector3 a3 = t3 - vector;
			float magnitude = vector2.magnitude;
			float magnitude2 = vector3.magnitude;
			float magnitude3 = vector4.magnitude;
			float lmin = Mathf.Min(Mathf.Min(magnitude, magnitude2), magnitude3);
			Func<float, float, float> func = (float t, float u) => (t - lmin + u - lmin) / 2f;
			float d = func(magnitude2, magnitude3);
			float d2 = func(magnitude3, magnitude);
			float d3 = func(magnitude, magnitude2);
			return vector + (a * d + a2 * d2 + a3 * d3);
		}

		// Token: 0x040010E2 RID: 4322
		private const float TOLERANCE = 0.01f;
	}
}
