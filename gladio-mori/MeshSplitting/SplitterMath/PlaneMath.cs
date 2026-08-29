using System;
using UnityEngine;

namespace MeshSplitting.SplitterMath
{
	// Token: 0x020002D7 RID: 727
	public class PlaneMath
	{
		// Token: 0x06001638 RID: 5688 RVA: 0x0006DEA6 File Offset: 0x0006C0A6
		public PlaneMath()
		{
			this.Point = Vector3.zero;
			this.Normal = Vector3.up;
		}

		// Token: 0x06001639 RID: 5689 RVA: 0x0006DEC4 File Offset: 0x0006C0C4
		public PlaneMath(PlaneMath plane)
		{
			this.Point = plane.Point;
			this.Normal = plane.Normal;
		}

		// Token: 0x0600163A RID: 5690 RVA: 0x0006DEE4 File Offset: 0x0006C0E4
		public PlaneMath(Transform transform)
		{
			this.Point = transform.position;
			this.Normal = transform.up;
		}

		// Token: 0x0600163B RID: 5691 RVA: 0x0006DF04 File Offset: 0x0006C104
		public PlaneMath(Vector3 point, Vector3 normal)
		{
			this.Point = point;
			this.Normal = normal;
		}

		// Token: 0x0600163C RID: 5692 RVA: 0x0006DF1A File Offset: 0x0006C11A
		public float LineIntersect(Vector3 lineStart, Vector3 lineEnd)
		{
			return Vector3.Dot(this.Normal, this.Point - lineStart) / Vector3.Dot(this.Normal, lineEnd - lineStart);
		}

		// Token: 0x0600163D RID: 5693 RVA: 0x0006DF46 File Offset: 0x0006C146
		public float PointSide(Vector3 point)
		{
			return Vector3.Dot(this.Normal, point - this.Point);
		}

		// Token: 0x0600163E RID: 5694 RVA: 0x0006DF60 File Offset: 0x0006C160
		public float PointSideNormalized(Vector3 point)
		{
			return Vector3.Dot(this.Normal, (point - this.Point).normalized);
		}

		// Token: 0x04001005 RID: 4101
		public Vector3 Point;

		// Token: 0x04001006 RID: 4102
		public Vector3 Normal;
	}
}
