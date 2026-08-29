using System;
using Es.InkPainter.Effective;
using UnityEngine;

namespace Es.InkPainter.Sample
{
	// Token: 0x02000300 RID: 768
	public class ClipPainter : MonoBehaviour
	{
		// Token: 0x06001731 RID: 5937 RVA: 0x00075A58 File Offset: 0x00073C58
		private void OnGUI()
		{
			GUI.Box(new Rect(0f, 0f, 300f, 320f), "");
			GUI.Box(new Rect(0f, 0f, 300f, 300f), "Grab Texture");
			if (this.t != null)
			{
				GUI.DrawTexture(new Rect(0f, 0f, 300f, 300f), this.t);
			}
			this.grab = GUI.Toggle(new Rect(0f, 300f, 300f, 20f), this.grab, "Grab");
		}

		// Token: 0x06001732 RID: 5938 RVA: 0x00075B0C File Offset: 0x00073D0C
		public void Awake()
		{
			this.t = new RenderTexture(this.brush.BrushTexture.width, this.brush.BrushTexture.height, 0);
		}

		// Token: 0x06001733 RID: 5939 RVA: 0x00075B3C File Offset: 0x00073D3C
		private void Update()
		{
			if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out this.hitInfo))
			{
				InkCanvas component = this.hitInfo.transform.GetComponent<InkCanvas>();
				if (component != null && !this.grab)
				{
					component.Paint(this.brush, this.hitInfo, null);
				}
				if (this.grab)
				{
					GrabArea.Clip(this.brush.BrushTexture, this.brush.Scale, this.hitInfo.transform.GetComponent<MeshRenderer>().sharedMaterial.mainTexture, this.hitInfo.textureCoord, this.brush.RotateAngle, this.wrapMode, this.t, true);
					this.brush.BrushTexture = this.t;
					this.brush.ColorBlending = Brush.ColorBlendType.UseBrush;
					this.grab = false;
				}
			}
		}

		// Token: 0x04001121 RID: 4385
		[SerializeField]
		private bool grab = true;

		// Token: 0x04001122 RID: 4386
		[SerializeField]
		private Brush brush;

		// Token: 0x04001123 RID: 4387
		[SerializeField]
		private GrabArea.GrabTextureWrapMode wrapMode = GrabArea.GrabTextureWrapMode.Repeat;

		// Token: 0x04001124 RID: 4388
		private RenderTexture t;

		// Token: 0x04001125 RID: 4389
		private RaycastHit hitInfo;
	}
}
