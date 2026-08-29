using System;
using UnityEngine;

// Token: 0x02000042 RID: 66
public class AddTorque : MonoBehaviour
{
	// Token: 0x060001F7 RID: 503 RVA: 0x0000B8C6 File Offset: 0x00009AC6
	private void Start()
	{
		this.rb = base.GetComponent<Rigidbody>();
	}

	// Token: 0x060001F8 RID: 504 RVA: 0x0000B8D4 File Offset: 0x00009AD4
	private void Update()
	{
		if (!this.direction)
		{
			this.rb.AddTorque((float)this.speedx, (float)this.speedy, (float)this.speedz);
			return;
		}
		this.rb.AddTorque((float)(this.speedx * -1), (float)(this.speedy * -1), (float)(this.speedz * -1));
	}

	// Token: 0x04000147 RID: 327
	private Rigidbody rb;

	// Token: 0x04000148 RID: 328
	public bool direction;

	// Token: 0x04000149 RID: 329
	public int speedx = 40000;

	// Token: 0x0400014A RID: 330
	public int speedy = 40000;

	// Token: 0x0400014B RID: 331
	public int speedz = 40000;
}
