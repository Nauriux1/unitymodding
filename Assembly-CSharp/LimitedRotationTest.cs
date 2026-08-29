using System;
using UnityEngine;

// Token: 0x0200018A RID: 394
public class LimitedRotationTest : MonoBehaviour
{
	// Token: 0x06000C64 RID: 3172 RVA: 0x0000777A File Offset: 0x0000597A
	private void Start()
	{
	}

	// Token: 0x06000C65 RID: 3173 RVA: 0x0003C508 File Offset: 0x0003A708
	private void Update()
	{
		Vector3 eulerAngles = this.target.rotation.eulerAngles;
		if (eulerAngles.x > 180f)
		{
			eulerAngles.x -= 360f;
		}
		if (eulerAngles.y > 180f)
		{
			eulerAngles.y -= 360f;
		}
		if (eulerAngles.z > 180f)
		{
			eulerAngles.z -= 360f;
		}
		eulerAngles.x = Mathf.Clamp(eulerAngles.x, -10f, 10f);
		eulerAngles.z = Mathf.Clamp(eulerAngles.z, -10f, 10f);
		base.transform.eulerAngles = eulerAngles;
	}

	// Token: 0x040008CE RID: 2254
	public Transform target;
}
