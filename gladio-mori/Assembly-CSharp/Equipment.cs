using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200024F RID: 591
public class Equipment : MonoBehaviour, ILegacy, IBluntDamageDealer
{
	// Token: 0x06001142 RID: 4418 RVA: 0x00058C8C File Offset: 0x00056E8C
	private void Awake()
	{
		this.historyPositionTracker = new HistoryPositionTracker(base.gameObject);
		this.bluntDamageDealer.Init(this.GetRigidbody(), this, null);
	}

	// Token: 0x06001143 RID: 4419 RVA: 0x00058CB2 File Offset: 0x00056EB2
	protected virtual void Start()
	{
		this.UpdateCenterOfMass();
		if (ReplayManager.singleton != null && !this.animationOnly)
		{
			ReplayManager.singleton.AddEquipmentToRecording(this);
		}
	}

	// Token: 0x06001144 RID: 4420 RVA: 0x00058CDC File Offset: 0x00056EDC
	private void OnCollisionEnter(Collision collision)
	{
		if (SoundManager.singleton != null)
		{
			SoundManager.singleton.PlaySoundForCollision(collision, base.gameObject, null);
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

	// Token: 0x06001145 RID: 4421 RVA: 0x00058DAC File Offset: 0x00056FAC
	public void SetAnimationOnly()
	{
		this.animationOnly = true;
	}

	// Token: 0x06001146 RID: 4422 RVA: 0x00058DB8 File Offset: 0x00056FB8
	public void UpdateCenterOfMass()
	{
		if (this.usingLegacy)
		{
			if (this.useLegacyCom)
			{
				this.Rigidbody.centerOfMass = this.legacyCom;
				return;
			}
		}
		else if (this.CenterOfMass != null)
		{
			this.Rigidbody.centerOfMass = this.CenterOfMass.localPosition;
		}
	}

	// Token: 0x06001147 RID: 4423 RVA: 0x00058E0B File Offset: 0x0005700B
	private void Update()
	{
		this.historyPositionTracker.UpdateHistory();
		this.bluntDamageDealer.UpdateHistory();
	}

	// Token: 0x06001148 RID: 4424 RVA: 0x00058E23 File Offset: 0x00057023
	public HistoryPositionItem GetPreviousHistoryPosition()
	{
		return this.historyPositionTracker.GetPreviousHistoryPosition();
	}

	// Token: 0x06001149 RID: 4425 RVA: 0x00058E30 File Offset: 0x00057030
	public Rigidbody GetRigidbody()
	{
		return this.Rigidbody;
	}

	// Token: 0x170001B8 RID: 440
	// (get) Token: 0x0600114A RID: 4426 RVA: 0x00058E38 File Offset: 0x00057038
	public Rigidbody Rigidbody
	{
		get
		{
			Rigidbody result;
			if ((result = this._rigidbody) == null)
			{
				result = (this._rigidbody = base.gameObject.GetComponent<Rigidbody>());
			}
			return result;
		}
	}

	// Token: 0x170001B9 RID: 441
	// (get) Token: 0x0600114B RID: 4427 RVA: 0x00058E63 File Offset: 0x00057063
	// (set) Token: 0x0600114C RID: 4428 RVA: 0x00058E6B File Offset: 0x0005706B
	public bool legacyInitialized { get; set; }

	// Token: 0x0600114D RID: 4429 RVA: 0x00058E74 File Offset: 0x00057074
	public void SetLegacy(bool legacy)
	{
		this.InitLegacy();
		if (legacy)
		{
			this.usingLegacy = true;
			this.Rigidbody.drag = this.legacyDrag;
			if (this.useLegacyMass)
			{
				this.Rigidbody.mass = this.legacyMass;
			}
		}
		else
		{
			this.usingLegacy = false;
			this.Rigidbody.drag = this.normalDrag;
			this.Rigidbody.mass = this.normalMass;
		}
		this.UpdateCenterOfMass();
	}

	// Token: 0x0600114E RID: 4430 RVA: 0x00058EEC File Offset: 0x000570EC
	public void InitLegacy()
	{
		if (!this.legacyInitialized)
		{
			this.normalDrag = this.Rigidbody.drag;
			this.normalMass = this.Rigidbody.mass;
			this.legacyInitialized = true;
		}
	}

	// Token: 0x0600114F RID: 4431 RVA: 0x0002F596 File Offset: 0x0002D796
	public bool LegacyItemExists()
	{
		return base.enabled;
	}

	// Token: 0x06001150 RID: 4432 RVA: 0x00058F1F File Offset: 0x0005711F
	public void SetPlayerHealth(PlayerHealth newPlayerHealth)
	{
		this.playerHealth = newPlayerHealth;
	}

	// Token: 0x06001151 RID: 4433 RVA: 0x00058F28 File Offset: 0x00057128
	public PlayerHealth GetPlayerHealth()
	{
		return this.playerHealth;
	}

	// Token: 0x06001152 RID: 4434 RVA: 0x00058F30 File Offset: 0x00057130
	public BluntDamageDealer GetBluntDamageDealer()
	{
		return this.bluntDamageDealer;
	}

	// Token: 0x06001153 RID: 4435 RVA: 0x00058F38 File Offset: 0x00057138
	public bool EquipmentIsHeld()
	{
		return this.handle != null && this.handle.EquipmentIsHeld();
	}

	// Token: 0x06001154 RID: 4436 RVA: 0x00058F55 File Offset: 0x00057155
	public List<Hand> GetGrabbingHands()
	{
		if (this.handle != null)
		{
			return this.handle.GetGrabbingHands();
		}
		return null;
	}

	// Token: 0x04000CE9 RID: 3305
	public Handle handle;

	// Token: 0x04000CEA RID: 3306
	public PlayerHealth playerHealth;

	// Token: 0x04000CEB RID: 3307
	public Transform CenterOfMass;

	// Token: 0x04000CEC RID: 3308
	public bool IsPainter = true;

	// Token: 0x04000CED RID: 3309
	private bool animationOnly;

	// Token: 0x04000CEE RID: 3310
	private List<ContactPoint> contacts = new List<ContactPoint>(128);

	// Token: 0x04000CEF RID: 3311
	public HistoryPositionTracker historyPositionTracker;

	// Token: 0x04000CF0 RID: 3312
	private Rigidbody _rigidbody;

	// Token: 0x04000CF2 RID: 3314
	private float legacyDrag;

	// Token: 0x04000CF3 RID: 3315
	private float normalDrag;

	// Token: 0x04000CF4 RID: 3316
	[Header("Legacy")]
	public bool useLegacyMass;

	// Token: 0x04000CF5 RID: 3317
	public float legacyMass = 1f;

	// Token: 0x04000CF6 RID: 3318
	private float normalMass = 1f;

	// Token: 0x04000CF7 RID: 3319
	public bool useLegacyCom;

	// Token: 0x04000CF8 RID: 3320
	public Vector3 legacyCom;

	// Token: 0x04000CF9 RID: 3321
	private bool usingLegacy;

	// Token: 0x04000CFA RID: 3322
	public BluntDamageDealer bluntDamageDealer;
}
