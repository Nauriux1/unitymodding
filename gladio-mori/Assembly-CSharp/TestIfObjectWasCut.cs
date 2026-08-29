using System;
using UnityEngine;

// Token: 0x02000190 RID: 400
public class TestIfObjectWasCut : MonoBehaviour
{
	// Token: 0x06000C7E RID: 3198 RVA: 0x0003CB84 File Offset: 0x0003AD84
	private void Start()
	{
		this.cleanCut = true;
	}

	// Token: 0x06000C7F RID: 3199 RVA: 0x0003CB90 File Offset: 0x0003AD90
	private void Update()
	{
		if (this.cutTarget.GetComponent<Collider>().bounds.Contains(this.line2End.transform.position))
		{
			this.cleanCut = false;
		}
		Plane plane = new Plane(this.blade1.forward, this.line1Start.position);
		Plane plane2 = new Plane(this.blade2.forward, this.line2Start.position);
		bool side = plane.GetSide(this.cutTarget.transform.position);
		bool side2 = plane2.GetSide(this.cutTarget.transform.position);
		if (side != side2)
		{
			Debug.DrawRay(this.line1Start.position, this.blade1.forward, Color.green, 0f);
			Debug.DrawRay(this.line2Start.position, this.blade2.forward, Color.green, 0f);
		}
		else
		{
			Debug.DrawRay(this.line1Start.position, this.blade1.forward, Color.red, 0f);
			Debug.DrawRay(this.line2Start.position, this.blade2.forward, Color.red, 0f);
		}
		Vector3 dir = this.line1End.position - this.line1Start.position;
		Vector3 dir2 = this.line2End.position - this.line2Start.position;
		Debug.DrawRay(this.line1Start.position, dir, Color.red);
		Debug.DrawRay(this.line2Start.position, dir2, Color.red);
		Vector3 b = (this.line1Start.position + this.line1End.position) / 2f;
		Vector3 normalized = ((this.line2Start.position + this.line2End.position) / 2f - b).normalized;
		if (this.cleanCut)
		{
			Debug.DrawRay(this.cutTarget.transform.position, normalized, Color.green);
			return;
		}
		Debug.DrawRay(this.cutTarget.transform.position, normalized, Color.red);
	}

	// Token: 0x040008E1 RID: 2273
	public Transform blade1;

	// Token: 0x040008E2 RID: 2274
	public Transform line1Start;

	// Token: 0x040008E3 RID: 2275
	public Transform line1End;

	// Token: 0x040008E4 RID: 2276
	public Transform blade2;

	// Token: 0x040008E5 RID: 2277
	public Transform line2Start;

	// Token: 0x040008E6 RID: 2278
	public Transform line2End;

	// Token: 0x040008E7 RID: 2279
	public GameObject cutTarget;

	// Token: 0x040008E8 RID: 2280
	private bool cleanCut = true;
}
