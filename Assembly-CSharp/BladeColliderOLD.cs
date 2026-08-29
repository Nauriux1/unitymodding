using System;
using UnityEngine;

// Token: 0x02000255 RID: 597
public class BladeColliderOLD : MonoBehaviour
{
	// Token: 0x06001185 RID: 4485 RVA: 0x0000777A File Offset: 0x0000597A
	private void Start()
	{
	}

	// Token: 0x06001186 RID: 4486 RVA: 0x0000777A File Offset: 0x0000597A
	private void Update()
	{
	}

	// Token: 0x06001187 RID: 4487 RVA: 0x00059AE5 File Offset: 0x00057CE5
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == this.bladeTrigger.colliderTagName)
		{
			Physics.IgnoreCollision(collision.collider, base.GetComponent<Collider>(), true);
		}
	}

	// Token: 0x06001188 RID: 4488 RVA: 0x00059B16 File Offset: 0x00057D16
	private void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.tag == this.bladeTrigger.colliderTagName)
		{
			Physics.IgnoreCollision(collision.collider, base.GetComponent<Collider>(), false);
		}
	}

	// Token: 0x04000D1D RID: 3357
	public Blade bladeTrigger;
}
