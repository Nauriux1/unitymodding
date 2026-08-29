using System;
using UnityEngine;

// Token: 0x0200024A RID: 586
public class BladeChild : MonoBehaviour, IBlade
{
	// Token: 0x06001114 RID: 4372 RVA: 0x00058090 File Offset: 0x00056290
	private void Start()
	{
		if (this.blade == null)
		{
			this.blade = base.transform.parent.GetComponent<Blade>();
		}
	}

	// Token: 0x06001115 RID: 4373 RVA: 0x000580B6 File Offset: 0x000562B6
	public virtual void OnTriggerEnter(Collider collision)
	{
		this.blade.HandleBladeCutStart(collision, this);
	}

	// Token: 0x06001116 RID: 4374 RVA: 0x000580C5 File Offset: 0x000562C5
	public virtual void OnTriggerExit(Collider collision)
	{
		this.blade.HandleBladeCutEnd(collision, this);
	}

	// Token: 0x06001117 RID: 4375 RVA: 0x000580D4 File Offset: 0x000562D4
	public BladePainter[] GetBladePainters()
	{
		if (this.blade != null && (this.bladePainters == null || this.bladePainters.Length == 0))
		{
			return this.blade.bladePainters;
		}
		return this.bladePainters;
	}

	// Token: 0x04000CC6 RID: 3270
	public Blade blade;

	// Token: 0x04000CC7 RID: 3271
	public BladePainter[] bladePainters;
}
