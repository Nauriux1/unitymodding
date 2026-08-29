using System;
using UnityEngine;

namespace MeshSplitting.SplitterMath
{
	// Token: 0x020002D8 RID: 728
	public static class SplitterHelper
	{
		// Token: 0x0600163F RID: 5695 RVA: 0x0006DF8C File Offset: 0x0006C18C
		public static bool CompareVector2(Vector2 vecA, Vector2 vecB)
		{
			return SplitterHelper.CompareVector2(ref vecA, ref vecB);
		}

		// Token: 0x06001640 RID: 5696 RVA: 0x0006DF98 File Offset: 0x0006C198
		public static bool CompareVector2(ref Vector2 vecA, ref Vector2 vecB)
		{
			float num = vecA.x - vecB.x;
			if (num < SplitterHelper.Threshold && num > -SplitterHelper.Threshold)
			{
				float num2 = vecA.y - vecB.y;
				if (num2 < SplitterHelper.Threshold && num2 > -SplitterHelper.Threshold)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001641 RID: 5697 RVA: 0x0006DFE6 File Offset: 0x0006C1E6
		public static bool CompareVector3(Vector3 vecA, Vector3 vecB)
		{
			return SplitterHelper.CompareVector3(ref vecA, ref vecB);
		}

		// Token: 0x06001642 RID: 5698 RVA: 0x0006DFF4 File Offset: 0x0006C1F4
		public static bool CompareVector3(ref Vector3 vecA, ref Vector3 vecB)
		{
			float num = vecA.x - vecB.x;
			if (num < SplitterHelper.Threshold && num > -SplitterHelper.Threshold)
			{
				float num2 = vecA.y - vecB.y;
				if (num2 < SplitterHelper.Threshold && num2 > -SplitterHelper.Threshold)
				{
					float num3 = vecA.z - vecB.z;
					if (num3 < SplitterHelper.Threshold && num3 > -SplitterHelper.Threshold)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001643 RID: 5699 RVA: 0x0006E064 File Offset: 0x0006C264
		public static float GetPlaneSide(PlaneMath plane, Vector3[] vertices)
		{
			float num = plane.PointSide(vertices[0]);
			if (num > SplitterHelper.Threshold && num < -SplitterHelper.Threshold)
			{
				num = plane.PointSide(vertices[1]);
				if (num > SplitterHelper.Threshold && num < -SplitterHelper.Threshold)
				{
					num = plane.PointSide(vertices[2]);
				}
			}
			return num;
		}

		// Token: 0x04001007 RID: 4103
		public static float Threshold = 1E-05f;
	}
}
