using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

// Token: 0x02000256 RID: 598
public class Weapon : Equipment
{
	// Token: 0x0600118A RID: 4490 RVA: 0x00059B47 File Offset: 0x00057D47
	protected override void Start()
	{
		if (NetworkManager.singleton != null && NetworkManager.singleton.mode == NetworkManagerMode.ClientOnly)
		{
			this.disableLocalLogic = true;
		}
		this.CreateWeaponSectionsList();
		this.CreateWeaponEdgeSectionsList();
		base.Start();
	}

	// Token: 0x0600118B RID: 4491 RVA: 0x00059B7C File Offset: 0x00057D7C
	public void IgnoreCollision(Collider[] colliders, bool ignore = true)
	{
		if (colliders != null && this.bladeColliders != null)
		{
			foreach (Collider collider in colliders)
			{
				foreach (Collider collider2 in this.bladeColliders)
				{
					Physics.IgnoreCollision(collider, collider2, ignore);
				}
			}
		}
	}

	// Token: 0x0600118C RID: 4492 RVA: 0x00059BF4 File Offset: 0x00057DF4
	public virtual void OnCollisionEnter(Collision collision)
	{
		if (this.disableLocalLogic)
		{
			return;
		}
		this.contacts.Clear();
		collision.GetContacts(this.contacts);
		bool flag = false;
		foreach (ContactPoint contactPoint in this.contacts)
		{
			if (contactPoint.thisCollider.gameObject.CompareTag(this.bladeColliderTagName))
			{
				flag = true;
				break;
			}
		}
		IWeaponDamageable component;
		if (collision.collider.gameObject.layer == 17)
		{
			component = collision.transform.GetComponent<IWeaponDamageable>();
		}
		else
		{
			component = collision.collider.transform.GetComponent<IWeaponDamageable>();
			if (component == null && collision.collider.transform.parent != null)
			{
				component = collision.collider.transform.parent.GetComponent<IWeaponDamageable>();
			}
		}
		if (flag && component != null && component.IsOrgan())
		{
			if (component.IsBone())
			{
				if (this.bladeTrigger.CheckBoneBreak(component, collision.collider, collision.relativeVelocity.magnitude))
				{
					component.Destory(null, true);
				}
			}
			else
			{
				component.Destory(null, true);
			}
		}
		if (component != null && !component.IsOrgan())
		{
			BluntDamageHelpers.HandleBluntDamage(collision, this.contacts, (WeaponDamageableBodyPart)component, this);
		}
		if (SoundManager.singleton != null)
		{
			SoundManager.singleton.PlaySoundForCollision(collision, base.gameObject, null);
		}
	}

	// Token: 0x0600118D RID: 4493 RVA: 0x00059D8C File Offset: 0x00057F8C
	private void CreateWeaponSectionsList()
	{
		if (this.weaponSections == null)
		{
			this.weaponSections = new List<WeaponSection>();
			if (this.bladeTrigger != null)
			{
				foreach (BladePainter bladePainter in this.bladeTrigger.GetAllBladePainters())
				{
					this.weaponSections.Add(new WeaponSection
					{
						point0 = bladePainter.position0,
						point1 = bladePainter.position1
					});
				}
				return;
			}
			if (this.bluntDamageDealer != null && this.bluntDamageDealer.centerOfMassLine != null && this.bluntDamageDealer.centerOfMassLine.Count == 2)
			{
				this.weaponSections.Add(new WeaponSection
				{
					point0 = this.bluntDamageDealer.centerOfMassLine[0],
					point1 = this.bluntDamageDealer.centerOfMassLine[1]
				});
			}
		}
	}

	// Token: 0x0600118E RID: 4494 RVA: 0x00059E6A File Offset: 0x0005806A
	public List<WeaponSection> GetWeaponSections()
	{
		this.CreateWeaponSectionsList();
		return this.weaponSections;
	}

	// Token: 0x0600118F RID: 4495 RVA: 0x00059E78 File Offset: 0x00058078
	private void CreateWeaponEdgeSectionsList()
	{
		if (this.weaponEdgeSections == null || this.weaponEdgeSections.Count == 0)
		{
			this.weaponEdgeSections = new List<WeaponEdgeSection>();
			List<WeaponSection> list = this.GetWeaponSections();
			if (list != null && list.Count > 0)
			{
				foreach (WeaponSection weaponSection in list)
				{
					this.weaponEdgeSections.Add(new WeaponEdgeSection
					{
						points = new List<Transform>
						{
							weaponSection.point0,
							weaponSection.point1
						}
					});
				}
			}
		}
	}

	// Token: 0x06001190 RID: 4496 RVA: 0x00059F28 File Offset: 0x00058128
	public List<WeaponEdgeSection> GetWeaponEdgeSections()
	{
		this.CreateWeaponEdgeSectionsList();
		return this.weaponEdgeSections;
	}

	// Token: 0x06001191 RID: 4497 RVA: 0x00059F38 File Offset: 0x00058138
	public void CheckAsleep()
	{
		bool flag = false;
		if (base.transform.parent != null)
		{
			flag = true;
		}
		else if (ReplayManager.singleton != null && (ReplayManager.singleton.replayMode == ReplayMode.Replay || ReplayManager.singleton.replayMode == ReplayMode.StartReplayAfterLoad))
		{
			flag = true;
		}
		this.SetAsleep(!flag);
	}

	// Token: 0x06001192 RID: 4498 RVA: 0x00059F91 File Offset: 0x00058191
	private void SetAsleep(bool value)
	{
		if (value != this.weaponIsAsleep)
		{
			this.weaponIsAsleep = value;
			if (this.bladeTrigger != null)
			{
				this.bladeTrigger.SetAsleep(this.weaponIsAsleep);
			}
		}
	}

	// Token: 0x04000D1E RID: 3358
	[Header("Weapon")]
	public Blade bladeTrigger;

	// Token: 0x04000D1F RID: 3359
	public List<Collider> bladeColliders;

	// Token: 0x04000D20 RID: 3360
	public bool disableLocalLogic;

	// Token: 0x04000D21 RID: 3361
	public Transform bladePoint;

	// Token: 0x04000D22 RID: 3362
	public float weaponMaxDistance = 1.8f;

	// Token: 0x04000D23 RID: 3363
	public float weaponMinDistance = 1.4f;

	// Token: 0x04000D24 RID: 3364
	private List<ContactPoint> contacts = new List<ContactPoint>(128);

	// Token: 0x04000D25 RID: 3365
	private string bladeColliderTagName = "BladeCollider";

	// Token: 0x04000D26 RID: 3366
	private List<WeaponSection> weaponSections;

	// Token: 0x04000D27 RID: 3367
	public List<WeaponEdgeSection> weaponEdgeSections;

	// Token: 0x04000D28 RID: 3368
	private bool weaponIsAsleep;
}
