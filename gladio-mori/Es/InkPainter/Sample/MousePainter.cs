using System;
using UnityEngine;

namespace Es.InkPainter.Sample
{
	// Token: 0x02000304 RID: 772
	public class MousePainter : MonoBehaviour
	{
		// Token: 0x06001743 RID: 5955 RVA: 0x00075FB8 File Offset: 0x000741B8
		private void Update()
		{
			if (Input.GetMouseButton(0))
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				bool flag = true;
				RaycastHit hitInfo;
				if (Physics.Raycast(ray, out hitInfo))
				{
					InkCanvas component = hitInfo.transform.GetComponent<InkCanvas>();
					if (component != null)
					{
						switch (this.useMethodType)
						{
						case MousePainter.UseMethodType.RaycastHitInfo:
							flag = (this.erase ? component.Erase(this.brush, hitInfo, null) : component.Paint(this.brush, hitInfo, null));
							break;
						case MousePainter.UseMethodType.WorldPoint:
							flag = (this.erase ? component.Erase(this.brush, hitInfo.point, null, null) : component.Paint(this.brush, hitInfo.point, null, null));
							break;
						case MousePainter.UseMethodType.NearestSurfacePoint:
							flag = (this.erase ? component.EraseNearestTriangleSurface(this.brush, hitInfo.point, null, null) : component.PaintNearestTriangleSurface(this.brush, hitInfo.point, null, null));
							break;
						case MousePainter.UseMethodType.DirectUV:
							if (!(hitInfo.collider is MeshCollider))
							{
								Debug.LogWarning("Raycast may be unexpected if you do not use MeshCollider.");
							}
							flag = (this.erase ? component.EraseUVDirect(this.brush, hitInfo.textureCoord, null) : component.PaintUVDirect(this.brush, hitInfo.textureCoord, null));
							break;
						}
					}
					if (!flag)
					{
						Debug.LogError("Failed to paint.");
					}
				}
			}
		}

		// Token: 0x06001744 RID: 5956 RVA: 0x00076120 File Offset: 0x00074320
		public void OnGUI()
		{
			if (GUILayout.Button("Reset", Array.Empty<GUILayoutOption>()))
			{
				InkCanvas[] array = UnityEngine.Object.FindObjectsOfType<InkCanvas>();
				for (int i = 0; i < array.Length; i++)
				{
					array[i].ResetPaint();
				}
			}
		}

		// Token: 0x04001132 RID: 4402
		[SerializeField]
		private Brush brush;

		// Token: 0x04001133 RID: 4403
		[SerializeField]
		private MousePainter.UseMethodType useMethodType;

		// Token: 0x04001134 RID: 4404
		[SerializeField]
		private bool erase;

		// Token: 0x02000305 RID: 773
		[Serializable]
		private enum UseMethodType
		{
			// Token: 0x04001136 RID: 4406
			RaycastHitInfo,
			// Token: 0x04001137 RID: 4407
			WorldPoint,
			// Token: 0x04001138 RID: 4408
			NearestSurfacePoint,
			// Token: 0x04001139 RID: 4409
			DirectUV
		}
	}
}
