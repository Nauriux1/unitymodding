using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace TriTriIntersectionHelpers
{
	// Token: 0x02000264 RID: 612
	public static class TriTriIntersection
	{
		// Token: 0x060011D7 RID: 4567
		[DllImport("MeshHelpers.dll", CallingConvention = CallingConvention.StdCall)]
		private static extern int tri_tri_intersect_with_isectline_floats(float v0_0, float v0_1, float v0_2, float v1_0, float v1_1, float v1_2, float v2_0, float v2_1, float v2_2, float u0_0, float u0_1, float u0_2, float u1_0, float u1_1, float u1_2, float u2_0, float u2_1, float u2_2, ref int coplanar, ref float isectpt0_0, ref float isectpt0_1, ref float isectpt0_2, ref float isectpt1_0, ref float isectpt1_1, ref float isectpt1_2);

		// Token: 0x060011D8 RID: 4568 RVA: 0x0005B4FC File Offset: 0x000596FC
		public static IntersectionInfo TrisIntersect(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 t0, Vector3 t1, Vector3 t2)
		{
			IntersectionInfo result = default(IntersectionInfo);
			result.intersects = false;
			int num = 0;
			float x = 0f;
			float y = 0f;
			float z = 0f;
			float x2 = 0f;
			float y2 = 0f;
			float z2 = 0f;
			if (TriTriIntersection.tri_tri_intersect_with_isectline_floats(p0.x, p0.y, p0.z, p1.x, p1.y, p1.z, p2.x, p2.y, p2.z, t0.x, t0.y, t0.z, t1.x, t1.y, t1.z, t2.x, t2.y, t2.z, ref num, ref x, ref y, ref z, ref x2, ref y2, ref z2) == 1)
			{
				result.intersects = true;
				result.intersectionPoint1 = new Vector3(x, y, z);
				result.intersectionPoint2 = new Vector3(x2, y2, z2);
			}
			return result;
		}
	}
}
