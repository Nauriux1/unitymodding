using System;
using Es.InkPainter.Effective;
using UnityEngine;

namespace Es.InkPainter.Sample
{
	// Token: 0x02000306 RID: 774
	public class ReflectPainter : MonoBehaviour
	{
		// Token: 0x06001746 RID: 5958 RVA: 0x0007615A File Offset: 0x0007435A
		public void Awake()
		{
			this.rt = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
			this.brush.ColorBlending = Brush.ColorBlendType.UseBrush;
		}

		// Token: 0x06001747 RID: 5959 RVA: 0x00076180 File Offset: 0x00074380
		public void OnGUI()
		{
			if (GUILayout.Button("Reset", Array.Empty<GUILayoutOption>()))
			{
				if (this.paintObject != null)
				{
					this.paintObject.ResetPaint();
				}
				UnityEngine.Object.Destroy(this.cam);
				this.cam = null;
			}
		}

		// Token: 0x06001748 RID: 5960 RVA: 0x000761C0 File Offset: 0x000743C0
		private void Update()
		{
			if (this.cam == null && Input.GetMouseButtonDown(0))
			{
				RaycastHit raycastHit;
				if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit))
				{
					this.paintObject = raycastHit.transform.GetComponent<InkCanvas>();
					if (this.paintObject != null)
					{
						this.uv = raycastHit.textureCoord;
						GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.camPref, raycastHit.point, Quaternion.LookRotation(raycastHit.normal), raycastHit.transform);
						this.cam = gameObject.GetComponent<Camera>();
						this.cam.targetTexture = this.rt;
						gameObject.SetActive(true);
						return;
					}
				}
			}
			else if (this.cam != null)
			{
				RenderTexture temporary = RenderTexture.GetTemporary(this.brush.BrushTexture.width, this.brush.BrushTexture.height);
				GrabArea.Clip(this.brush.BrushTexture, this.brush.Scale, this.rt, Vector3.one * 0.5f, this.brush.RotateAngle, GrabArea.GrabTextureWrapMode.Clip, temporary, true);
				ReverseUV.Horizontal(temporary, temporary);
				Texture brushTexture = this.brush.BrushTexture;
				this.brush.BrushTexture = temporary;
				if (this.paintObject != null)
				{
					this.paintObject.PaintUVDirect(this.brush, this.uv, null);
				}
				RenderTexture.ReleaseTemporary(temporary);
				this.brush.BrushTexture = brushTexture;
			}
		}

		// Token: 0x0400113A RID: 4410
		[SerializeField]
		private Brush brush;

		// Token: 0x0400113B RID: 4411
		[SerializeField]
		private GameObject camPref;

		// Token: 0x0400113C RID: 4412
		private RenderTexture rt;

		// Token: 0x0400113D RID: 4413
		private Camera cam;

		// Token: 0x0400113E RID: 4414
		private Vector2 uv;

		// Token: 0x0400113F RID: 4415
		private InkCanvas paintObject;
	}
}
