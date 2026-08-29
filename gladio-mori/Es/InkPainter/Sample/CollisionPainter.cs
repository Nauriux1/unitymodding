using System;
using UnityEngine;

namespace Es.InkPainter.Sample
{
	// Token: 0x02000301 RID: 769
	[RequireComponent(typeof(Collider), typeof(MeshRenderer))]
	public class CollisionPainter : MonoBehaviour
	{
		// Token: 0x06001735 RID: 5941 RVA: 0x00075C43 File Offset: 0x00073E43
		public void Awake()
		{
			base.GetComponent<MeshRenderer>().material.color = this.brush.Color;
		}

		// Token: 0x06001736 RID: 5942 RVA: 0x00075C60 File Offset: 0x00073E60
		public void FixedUpdate()
		{
			this.waitCount++;
		}

		// Token: 0x06001737 RID: 5943 RVA: 0x00075C70 File Offset: 0x00073E70
		public void OnCollisionStay(Collision collision)
		{
			if (this.waitCount < this.wait)
			{
				return;
			}
			this.waitCount = 0;
			foreach (ContactPoint contactPoint in collision.contacts)
			{
				InkCanvas component = contactPoint.otherCollider.GetComponent<InkCanvas>();
				if (component != null)
				{
					component.Paint(this.brush, contactPoint.point, null, null);
				}
			}
		}

		// Token: 0x06001738 RID: 5944 RVA: 0x00075CDC File Offset: 0x00073EDC
		public void OnCollisionEnter(Collision collision)
		{
			if (this.waitCount < this.wait)
			{
				return;
			}
			this.waitCount = 0;
			foreach (ContactPoint contactPoint in collision.contacts)
			{
				InkCanvas component = contactPoint.otherCollider.GetComponent<InkCanvas>();
				if (component != null)
				{
					component.Paint(this.brush, contactPoint.point, null, null);
				}
			}
		}

		// Token: 0x04001126 RID: 4390
		[SerializeField]
		private Brush brush;

		// Token: 0x04001127 RID: 4391
		[SerializeField]
		private int wait = 3;

		// Token: 0x04001128 RID: 4392
		private int waitCount;
	}
}
