using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

// Token: 0x0200025A RID: 602
public class BladePainter : MonoBehaviour
{
	// Token: 0x0600119E RID: 4510 RVA: 0x0005A3E8 File Offset: 0x000585E8
	private void Start()
	{
		this.oldPosition0 = this.position0.position;
		this.oldPosition1 = this.position1.position;
		this.olderPosition0 = this.position0.position;
		this.olderPosition1 = this.position1.position;
		if (this.paintAllAlwaysTEST)
		{
			this.bladePaintables = UnityEngine.Object.FindObjectsOfType<BladePaintable>().ToList<BladePaintable>();
		}
	}

	// Token: 0x0600119F RID: 4511 RVA: 0x0005A451 File Offset: 0x00058651
	private void FixedUpdate()
	{
		this.DoDraw();
	}

	// Token: 0x060011A0 RID: 4512 RVA: 0x0005A459 File Offset: 0x00058659
	public void AddBladePaintable(List<BladePaintable> newBladePaintables = null)
	{
		if (newBladePaintables != null)
		{
			this.bladePaintables.AddRange(newBladePaintables);
		}
	}

	// Token: 0x060011A1 RID: 4513 RVA: 0x0005A46C File Offset: 0x0005866C
	public void RemoveBladePaintable(List<BladePaintable> removeBladePaintables = null)
	{
		if (removeBladePaintables != null)
		{
			foreach (BladePaintable item in removeBladePaintables)
			{
				this.bladePaintables.Remove(item);
			}
		}
	}

	// Token: 0x060011A2 RID: 4514 RVA: 0x0005A4C4 File Offset: 0x000586C4
	private bool DrawThisFrame()
	{
		return !(this.blade != null) || this.blade.BladeIsAwake();
	}

	// Token: 0x060011A3 RID: 4515 RVA: 0x0005A4E4 File Offset: 0x000586E4
	private void DoDraw()
	{
		if (this.DrawThisFrame())
		{
			if (this.bladePaintables.Count > 0)
			{
				this.DrawWithOlderPos();
			}
			this.olderPosition0 = this.oldPosition0;
			this.olderPosition1 = this.oldPosition1;
			this.oldPosition0 = this.position0.position;
			this.oldPosition1 = this.position1.position;
		}
	}

	// Token: 0x060011A4 RID: 4516 RVA: 0x0005A548 File Offset: 0x00058748
	private void DrawWithOlderPos()
	{
		Vector3 normalized = (this.olderPosition0 - this.position0.position).normalized;
		float num = Vector3.Dot(this.position0.up, normalized);
		Vector3 normalized2 = (this.olderPosition1 - this.position1.position).normalized;
		float num2 = Vector3.Dot(this.position1.up, normalized2);
		float num3 = this.drawerWidth;
		float num4 = this.drawerWidth;
		if (num < 0f)
		{
			num3 *= -1f;
		}
		if (num2 < 0f)
		{
			num4 *= -1f;
		}
		Vector3 point = new Vector3(this.position0.localPosition.x, this.position0.localPosition.y - num3, this.position0.localPosition.z);
		Vector3 point2 = new Vector3(this.position1.localPosition.x, this.position1.localPosition.y - num4, this.position1.localPosition.z);
		new Vector3(this.position0.localPosition.x, this.position0.localPosition.y + num3, this.position0.localPosition.z);
		new Vector3(this.position1.localPosition.x, this.position1.localPosition.y + num4, this.position1.localPosition.z);
		Vector3 p = base.transform.localToWorldMatrix.MultiplyPoint3x4(point);
		Vector3 vector = base.transform.localToWorldMatrix.MultiplyPoint3x4(point2);
		Vector3 vector2 = this.olderPosition0 + this.position0.up * num3;
		Vector3 p2 = this.olderPosition1 + this.position0.up * num4;
		this.DrawTriangle(p, vector, vector2, this.bladePaintables);
		this.DrawTriangle(vector2, p2, vector, this.bladePaintables);
	}

	// Token: 0x060011A5 RID: 4517 RVA: 0x0005A760 File Offset: 0x00058960
	public void DrawTriangle(Vector3 p0, Vector3 p1, Vector3 p2, List<BladePaintable> bladePaintables)
	{
		for (int i = 0; i < bladePaintables.Count; i++)
		{
			bladePaintables[i].AddDrawableTriangle(new BladeTriangle(p0, p1, p2));
		}
	}

	// Token: 0x060011A6 RID: 4518 RVA: 0x0005A794 File Offset: 0x00058994
	public bool IsTriangle()
	{
		float num = Vector3.Distance(this.position0.position, this.position1.position);
		float num2 = Vector3.Distance(this.position1.position, this.oldPosition0);
		float num3 = Vector3.Distance(this.oldPosition0, this.position0.position);
		float num4 = num;
		float num5;
		if (num2 > num4)
		{
			num5 = num4;
			num4 = num2;
		}
		else
		{
			num5 = num2;
		}
		float num6;
		if (num3 > num4)
		{
			num6 = num4;
			num4 = num3;
		}
		else
		{
			num6 = num3;
		}
		return !Generic.FloatEquals(num5 + num6, num4);
	}

	// Token: 0x04000D3D RID: 3389
	public float drawerWidth = 0.03f;

	// Token: 0x04000D3E RID: 3390
	public Transform position0;

	// Token: 0x04000D3F RID: 3391
	public Transform position1;

	// Token: 0x04000D40 RID: 3392
	private List<BladePaintable> bladePaintables = new List<BladePaintable>(32);

	// Token: 0x04000D41 RID: 3393
	private bool paintAllAlwaysTEST;

	// Token: 0x04000D42 RID: 3394
	private Vector3 oldPosition0;

	// Token: 0x04000D43 RID: 3395
	private Vector3 oldPosition1;

	// Token: 0x04000D44 RID: 3396
	private Vector3 olderPosition0;

	// Token: 0x04000D45 RID: 3397
	private Vector3 olderPosition1;

	// Token: 0x04000D46 RID: 3398
	public Blade blade;
}
