using System;
using UnityEngine;

// Token: 0x02000129 RID: 297
public class OppositeGravity : MonoBehaviour
{
	// Token: 0x0600094B RID: 2379 RVA: 0x0002C627 File Offset: 0x0002A827
	private void Start()
	{
		this.rb = base.GetComponent<Rigidbody>();
	}

	// Token: 0x0600094C RID: 2380 RVA: 0x0000777A File Offset: 0x0000597A
	private void Update()
	{
	}

	// Token: 0x0600094D RID: 2381 RVA: 0x0002C635 File Offset: 0x0002A835
	private void FixedUpdate()
	{
		this.rb.AddForce(base.transform.up * this.thrust);
	}

	// Token: 0x04000677 RID: 1655
	public float thrust = 60f;

	// Token: 0x04000678 RID: 1656
	public Rigidbody rb;
}
