using System;
using UnityEngine;

// Token: 0x02000104 RID: 260
public class AutoSize : MonoBehaviour
{
	// Token: 0x06000878 RID: 2168 RVA: 0x00029DD4 File Offset: 0x00027FD4
	private void LateUpdate()
	{
		float d = (this.Camera.transform.position - base.transform.position).magnitude * this.FixedSize * this.Camera.fieldOfView;
		base.transform.localScale = Vector3.one * d;
	}

	// Token: 0x040005DA RID: 1498
	public float FixedSize = 0.005f;

	// Token: 0x040005DB RID: 1499
	public Camera Camera;
}
