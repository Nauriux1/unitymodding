using System;
using UnityEngine;

// Token: 0x02000189 RID: 393
public class JumpHeightCalculator : MonoBehaviour
{
	// Token: 0x06000C61 RID: 3169 RVA: 0x0003C2EB File Offset: 0x0003A4EB
	private void Start()
	{
		this.rigidbody = base.GetComponent<Rigidbody>();
	}

	// Token: 0x06000C62 RID: 3170 RVA: 0x0003C2FC File Offset: 0x0003A4FC
	private void Update()
	{
		if (this.reset)
		{
			this.startposition = new Vector3(Mathf.Abs(base.transform.position.x), Mathf.Abs(base.transform.position.y), Mathf.Abs(base.transform.position.z));
			this.maxPosition = default(Vector3);
			this.maxPositionChange = default(Vector3);
			this.currentVelocity = 0f;
			this.maxVelocity = 0f;
		}
		if (Mathf.Abs(base.transform.position.y) > Mathf.Abs(this.maxPosition.y))
		{
			this.maxPosition.y = Mathf.Abs(base.transform.position.y);
			this.maxPositionChange.y = this.maxPosition.y - this.startposition.y;
		}
		if (Mathf.Abs(base.transform.position.x) > Mathf.Abs(this.maxPosition.x))
		{
			this.maxPosition.x = Mathf.Abs(base.transform.position.x);
			this.maxPositionChange.x = this.maxPosition.x - this.startposition.x;
		}
		if (Mathf.Abs(base.transform.position.z) > Mathf.Abs(this.maxPosition.z))
		{
			this.maxPosition.z = Mathf.Abs(base.transform.position.z);
			this.maxPositionChange.z = this.maxPosition.z - this.startposition.z;
		}
		if (this.rigidbody != null)
		{
			this.currentVelocity = this.rigidbody.velocity.magnitude;
			if (this.currentVelocity > this.maxVelocity)
			{
				this.maxVelocity = this.currentVelocity;
			}
		}
	}

	// Token: 0x040008C7 RID: 2247
	public Vector3 startposition;

	// Token: 0x040008C8 RID: 2248
	public Vector3 maxPosition;

	// Token: 0x040008C9 RID: 2249
	public Vector3 maxPositionChange;

	// Token: 0x040008CA RID: 2250
	public bool reset;

	// Token: 0x040008CB RID: 2251
	private Rigidbody rigidbody;

	// Token: 0x040008CC RID: 2252
	public float currentVelocity;

	// Token: 0x040008CD RID: 2253
	public float maxVelocity;
}
