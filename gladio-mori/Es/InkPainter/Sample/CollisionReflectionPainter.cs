using System;
using System.Linq;
using Es.InkPainter.Effective;
using UnityEngine;

namespace Es.InkPainter.Sample
{
	// Token: 0x02000302 RID: 770
	[RequireComponent(typeof(Collider))]
	public class CollisionReflectionPainter : MonoBehaviour
	{
		// Token: 0x0600173A RID: 5946 RVA: 0x00075D58 File Offset: 0x00073F58
		public void OnGUI()
		{
			if (this.debugMode)
			{
				GUI.Box(new Rect(0f, 0f, 200f, 200f), "ReflectionImage");
				GUI.DrawTexture(new Rect(0f, 0f, 200f, 200f), this.debug);
			}
		}

		// Token: 0x0600173B RID: 5947 RVA: 0x00075DB4 File Offset: 0x00073FB4
		private void Awake()
		{
			this.rt = RenderTexture.GetTemporary(Screen.width, Screen.height, 16);
			if (this.debugMode)
			{
				this.debug = new RenderTexture(this.brush.BrushTexture.width, this.brush.BrushTexture.height, 16);
			}
			this.cam.targetTexture = this.rt;
		}

		// Token: 0x0600173C RID: 5948 RVA: 0x00075E1E File Offset: 0x0007401E
		public void OnDestroy()
		{
			RenderTexture.ReleaseTemporary(this.rt);
		}

		// Token: 0x0600173D RID: 5949 RVA: 0x00075E2C File Offset: 0x0007402C
		public void OnCollisionStay(Collision collision)
		{
			if (this.cam == null)
			{
				return;
			}
			if (!collision.contacts.Any((ContactPoint p) => p.otherCollider.GetComponent<InkCanvas>() != null))
			{
				return;
			}
			this.cam.transform.position = base.transform.position + this.offset;
			ContactPoint contactPoint = collision.contacts.First((ContactPoint p) => p.otherCollider.GetComponent<InkCanvas>() != null);
			InkCanvas component = contactPoint.otherCollider.GetComponent<InkCanvas>();
			RenderTexture temporary = RenderTexture.GetTemporary(this.brush.BrushTexture.width, this.brush.BrushTexture.height);
			GrabArea.Clip(this.brush.BrushTexture, this.brush.Scale, this.rt, Vector3.one * 0.5f, this.brush.RotateAngle, GrabArea.GrabTextureWrapMode.Clamp, temporary, true);
			ReverseUV.Vertical(temporary, temporary);
			if (this.debugMode)
			{
				Graphics.Blit(temporary, this.debug);
			}
			Texture brushTexture = this.brush.BrushTexture;
			this.brush.BrushTexture = temporary;
			component.Paint(this.brush, contactPoint.point, null, null);
			RenderTexture.ReleaseTemporary(temporary);
			this.brush.BrushTexture = brushTexture;
		}

		// Token: 0x04001129 RID: 4393
		[SerializeField]
		private Brush brush;

		// Token: 0x0400112A RID: 4394
		[SerializeField]
		private Camera cam;

		// Token: 0x0400112B RID: 4395
		[SerializeField]
		private Vector3 offset;

		// Token: 0x0400112C RID: 4396
		[SerializeField]
		private bool debugMode;

		// Token: 0x0400112D RID: 4397
		private RenderTexture rt;

		// Token: 0x0400112E RID: 4398
		private RenderTexture debug;
	}
}
