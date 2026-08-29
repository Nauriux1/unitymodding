using System;
using UnityEngine;

// Token: 0x02000252 RID: 594
public class LargeBladeTriggerChild : MonoBehaviour
{
	// Token: 0x06001177 RID: 4471 RVA: 0x0005980D File Offset: 0x00057A0D
	private void Start()
	{
		if (this.largeBladeTrigger == null)
		{
			this.largeBladeTrigger = base.transform.parent.GetComponent<LargeBladeTrigger>();
		}
	}

	// Token: 0x06001178 RID: 4472 RVA: 0x00059833 File Offset: 0x00057A33
	public virtual void OnTriggerEnter(Collider collision)
	{
		this.largeBladeTrigger.HandleTriggerEnter(collision);
	}

	// Token: 0x06001179 RID: 4473 RVA: 0x00059841 File Offset: 0x00057A41
	public virtual void OnTriggerExit(Collider collision)
	{
		this.largeBladeTrigger.HandleTriggerExit(collision);
	}

	// Token: 0x04000D0E RID: 3342
	public LargeBladeTrigger largeBladeTrigger;
}
