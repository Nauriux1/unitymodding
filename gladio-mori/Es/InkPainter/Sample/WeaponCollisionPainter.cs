using System;
using UnityEngine;

namespace Es.InkPainter.Sample
{
	// Token: 0x02000309 RID: 777
	public class WeaponCollisionPainter : MonoBehaviour
	{
		// Token: 0x06001755 RID: 5973 RVA: 0x0000777A File Offset: 0x0000597A
		public void InitiateBladeBrush()
		{
		}

		// Token: 0x06001756 RID: 5974 RVA: 0x00076683 File Offset: 0x00074883
		public void FixedUpdate()
		{
			this.waitCount++;
		}

		// Token: 0x06001757 RID: 5975 RVA: 0x00076694 File Offset: 0x00074894
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

		// Token: 0x0400114F RID: 4431
		[SerializeField]
		private Brush brush;

		// Token: 0x04001150 RID: 4432
		[SerializeField]
		private int wait = 3;

		// Token: 0x04001151 RID: 4433
		private int waitCount;
	}
}
