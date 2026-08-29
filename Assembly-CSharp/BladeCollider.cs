using System;
using Mirror;
using UnityEngine;

// Token: 0x0200024B RID: 587
public class BladeCollider : MonoBehaviour
{
	// Token: 0x06001119 RID: 4377 RVA: 0x00058107 File Offset: 0x00056307
	private void Start()
	{
		if (NetworkManager.singleton != null && NetworkManager.singleton.mode == NetworkManagerMode.ClientOnly)
		{
			this.disableLocalLogic = true;
		}
	}

	// Token: 0x0600111A RID: 4378 RVA: 0x0005812C File Offset: 0x0005632C
	public virtual void OnCollisionEnter(Collision collision)
	{
		if (this.disableLocalLogic)
		{
			return;
		}
		IWeaponDamageable component = collision.collider.transform.GetComponent<IWeaponDamageable>();
		if (component == null && collision.collider.transform.parent != null)
		{
			component = collision.collider.transform.parent.GetComponent<IWeaponDamageable>();
		}
		if (component != null && component.IsOrgan())
		{
			if (component.IsBone())
			{
				if (this.blade.CheckBoneBreak(component, collision.collider, collision.impulse.magnitude))
				{
					component.Destory(null, true);
					return;
				}
			}
			else
			{
				component.Destory(null, true);
			}
		}
	}

	// Token: 0x04000CC8 RID: 3272
	public Blade blade;

	// Token: 0x04000CC9 RID: 3273
	public bool disableLocalLogic;
}
