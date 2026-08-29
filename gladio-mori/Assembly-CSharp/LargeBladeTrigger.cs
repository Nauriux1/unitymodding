using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

// Token: 0x02000251 RID: 593
public class LargeBladeTrigger : MonoBehaviour
{
	// Token: 0x06001170 RID: 4464 RVA: 0x00059684 File Offset: 0x00057884
	private void Start()
	{
		if (this.weapon == null)
		{
			this.weapon = base.transform.parent.GetComponent<Weapon>();
		}
		this.penetratingColliders = new List<Collider>(32);
		this.penetratingGameObjects = new List<GameObject>(32);
		if (NetworkManager.singleton != null && NetworkManager.singleton.mode == NetworkManagerMode.ClientOnly)
		{
			this.disableLocalLogic = true;
		}
	}

	// Token: 0x06001171 RID: 4465 RVA: 0x000596F0 File Offset: 0x000578F0
	private void FixedUpdate()
	{
		if (this.disableLocalLogic)
		{
			return;
		}
		bool flag = false;
		if (this.penetratingGameObjects.Count > 0)
		{
			flag = true;
		}
		if (!flag)
		{
			this.weapon.IgnoreCollision(this.penetratingColliders.ToArray(), false);
			this.penetratingColliders.Clear();
		}
	}

	// Token: 0x06001172 RID: 4466 RVA: 0x0005973D File Offset: 0x0005793D
	public virtual void OnTriggerEnter(Collider collision)
	{
		this.HandleTriggerEnter(collision);
	}

	// Token: 0x06001173 RID: 4467 RVA: 0x00059746 File Offset: 0x00057946
	public virtual void OnTriggerExit(Collider collision)
	{
		this.HandleTriggerExit(collision);
	}

	// Token: 0x06001174 RID: 4468 RVA: 0x00059750 File Offset: 0x00057950
	public void HandleTriggerEnter(Collider collision)
	{
		if (this.disableLocalLogic)
		{
			return;
		}
		IWeaponDamageable component = collision.transform.GetComponent<IWeaponDamageable>();
		if (component == null && collision.transform.parent != null)
		{
			component = collision.transform.parent.GetComponent<IWeaponDamageable>();
		}
		if (component != null && !component.IsOrgan())
		{
			this.bladePenetrating = true;
			Collider[] components = collision.GetComponents<Collider>();
			this.weapon.IgnoreCollision(components, true);
			this.penetratingColliders.AddRange(components);
			this.penetratingGameObjects.Add(collision.gameObject);
		}
	}

	// Token: 0x06001175 RID: 4469 RVA: 0x000597DC File Offset: 0x000579DC
	public void HandleTriggerExit(Collider collision)
	{
		if (this.disableLocalLogic)
		{
			return;
		}
		bool componentInParent = collision.transform.GetComponentInParent<IWeaponDamageable>() != null;
		this.bladePenetrating = false;
		if (componentInParent)
		{
			this.penetratingGameObjects.Remove(collision.gameObject);
		}
	}

	// Token: 0x04000D09 RID: 3337
	public Weapon weapon;

	// Token: 0x04000D0A RID: 3338
	public bool bladePenetrating;

	// Token: 0x04000D0B RID: 3339
	private List<Collider> penetratingColliders;

	// Token: 0x04000D0C RID: 3340
	private List<GameObject> penetratingGameObjects;

	// Token: 0x04000D0D RID: 3341
	public bool disableLocalLogic;
}
