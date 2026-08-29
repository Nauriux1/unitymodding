using System;
using System.Collections.Generic;
using MoveClasses;
using UnityEngine;

// Token: 0x0200024E RID: 590
public class BluntDamageDealerGameObject : MonoBehaviour, IBluntDamageDealer
{
	// Token: 0x06001139 RID: 4409 RVA: 0x00058AA4 File Offset: 0x00056CA4
	private void Awake()
	{
		this.bluntDamageDealer.Init(this.GetRigidbody(), null, base.transform);
	}

	// Token: 0x0600113A RID: 4410 RVA: 0x00058ABE File Offset: 0x00056CBE
	private void Update()
	{
		this.bluntDamageDealer.UpdateHistory();
	}

	// Token: 0x0600113B RID: 4411 RVA: 0x00058ACC File Offset: 0x00056CCC
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.gameObject.layer != 17 && collision.collider.gameObject.layer != 0)
		{
			return;
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
		if (component != null)
		{
			this.contacts.Clear();
			collision.GetContacts(this.contacts);
			if (!component.IsOrgan())
			{
				BluntDamageHelpers.HandleBluntDamage(collision, this.contacts, (WeaponDamageableBodyPart)component, this);
			}
		}
	}

	// Token: 0x0600113C RID: 4412 RVA: 0x00058B9C File Offset: 0x00056D9C
	public BluntDamageDealer GetBluntDamageDealer()
	{
		return this.bluntDamageDealer;
	}

	// Token: 0x0600113D RID: 4413 RVA: 0x00058BA4 File Offset: 0x00056DA4
	public PlayerHealth GetPlayerHealth()
	{
		return this.playerHealth;
	}

	// Token: 0x0600113E RID: 4414 RVA: 0x00058BAC File Offset: 0x00056DAC
	public Rigidbody GetRigidbody()
	{
		return this.rb;
	}

	// Token: 0x0600113F RID: 4415 RVA: 0x00058BB4 File Offset: 0x00056DB4
	private void CreateWeaponSectionsList()
	{
		if (this.weaponSections == null)
		{
			this.weaponSections = new List<WeaponSection>();
			if (this.bluntDamageDealer != null && this.bluntDamageDealer.centerOfMassLine != null && this.bluntDamageDealer.centerOfMassLine.Count == 2)
			{
				this.weaponSections.Add(new WeaponSection
				{
					point0 = this.bluntDamageDealer.centerOfMassLine[0],
					point1 = this.bluntDamageDealer.centerOfMassLine[1]
				});
				return;
			}
			this.weaponSections.Add(new WeaponSection
			{
				point0 = base.transform,
				point1 = base.transform
			});
		}
	}

	// Token: 0x06001140 RID: 4416 RVA: 0x00058C66 File Offset: 0x00056E66
	public List<WeaponSection> GetWeaponSections()
	{
		this.CreateWeaponSectionsList();
		return this.weaponSections;
	}

	// Token: 0x04000CE3 RID: 3299
	public BluntDamageDealer bluntDamageDealer;

	// Token: 0x04000CE4 RID: 3300
	public Rigidbody rb;

	// Token: 0x04000CE5 RID: 3301
	public PlayerHealth playerHealth;

	// Token: 0x04000CE6 RID: 3302
	public JointType bodyPart;

	// Token: 0x04000CE7 RID: 3303
	private List<ContactPoint> contacts = new List<ContactPoint>(128);

	// Token: 0x04000CE8 RID: 3304
	private List<WeaponSection> weaponSections;
}
