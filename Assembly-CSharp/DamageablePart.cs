using System;
using UnityEngine;

// Token: 0x02000141 RID: 321
public class DamageablePart : MonoBehaviour
{
	// Token: 0x06000A07 RID: 2567 RVA: 0x0002F6E7 File Offset: 0x0002D8E7
	private void Start()
	{
		this.player = (PlayerHealth)base.GetComponentsInParent(typeof(PlayerHealth))[0];
	}

	// Token: 0x06000A08 RID: 2568 RVA: 0x0000777A File Offset: 0x0000597A
	private void Update()
	{
	}

	// Token: 0x06000A09 RID: 2569 RVA: 0x0002F708 File Offset: 0x0002D908
	private void OnCollisionEnter(Collision col)
	{
		DamagingPart component = col.gameObject.GetComponent<DamagingPart>();
		if (component != null)
		{
			float num = (col.impulse / Time.fixedDeltaTime).sqrMagnitude / 80000f;
			double num2 = this.damageMultiplier;
			float num3 = component.damageMultiplier;
		}
	}

	// Token: 0x040006FD RID: 1789
	private PlayerHealth player;

	// Token: 0x040006FE RID: 1790
	public double damageMultiplier = 1.0;

	// Token: 0x040006FF RID: 1791
	public double permanentDamageMultiplier = 0.10000000149011612;
}
