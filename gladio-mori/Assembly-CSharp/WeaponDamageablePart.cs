using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using MoveClasses;
using UnityEngine;

// Token: 0x02000160 RID: 352
public class WeaponDamageablePart : MonoBehaviour, IWeaponDamageable
{
	// Token: 0x1700016B RID: 363
	// (get) Token: 0x06000B3F RID: 2879 RVA: 0x00036433 File Offset: 0x00034633
	// (set) Token: 0x06000B40 RID: 2880 RVA: 0x0003643B File Offset: 0x0003463B
	public List<BladePaintable> bladePaintables { get; set; }

	// Token: 0x1700016C RID: 364
	// (get) Token: 0x06000B41 RID: 2881 RVA: 0x00036444 File Offset: 0x00034644
	public bool isMuscle
	{
		get
		{
			return !this.lethal && !this.bloodVessel && this.organType != OrganType.Bone;
		}
	}

	// Token: 0x1700016D RID: 365
	// (get) Token: 0x06000B42 RID: 2882 RVA: 0x00036464 File Offset: 0x00034664
	public bool isBone
	{
		get
		{
			return this.organType == OrganType.Bone;
		}
	}

	// Token: 0x06000B43 RID: 2883 RVA: 0x0003646F File Offset: 0x0003466F
	private void Awake()
	{
		if (this.protectingBone != null)
		{
			this.protectingBone.protectedOrgans.Add(this);
		}
	}

	// Token: 0x06000B44 RID: 2884 RVA: 0x00036490 File Offset: 0x00034690
	private void Start()
	{
		if (NetworkManager.singleton != null && NetworkManager.singleton.mode == NetworkManagerMode.ClientOnly)
		{
			this.disableLocalLogic = true;
		}
		this.InitializeParticleSystem();
		this.FindJointStrengths();
		Component[] componentsInParent = base.GetComponentsInParent(typeof(PlayerHealth));
		if (componentsInParent != null && componentsInParent.Length != 0)
		{
			this.player = (PlayerHealth)componentsInParent[0];
		}
		if (this.player != null && this.bloodVessel)
		{
			this.player.bleedableOrgans.Add(this);
		}
	}

	// Token: 0x06000B45 RID: 2885 RVA: 0x00036517 File Offset: 0x00034717
	public void InitializeParticleSystem()
	{
		if (this.bloodFlowParticles != null)
		{
			this.bloodFlowParticles.SetActive(false);
			this.bloodFlowParticleSystem = this.bloodFlowParticles.GetComponent<ParticleSystem>();
		}
	}

	// Token: 0x06000B46 RID: 2886 RVA: 0x00036544 File Offset: 0x00034744
	private void Update()
	{
		if (this.destroyPendingProtectingBone)
		{
			this.destroyPendingProtectingBone = false;
			this.pendingDamageOrigin = null;
		}
	}

	// Token: 0x06000B47 RID: 2887 RVA: 0x00036564 File Offset: 0x00034764
	private void FindJointStrengths()
	{
		if (!this.isMuscle)
		{
			return;
		}
		if (this.jointStrengthGameObject == null)
		{
			this.jointStrengthGameObject = base.gameObject;
		}
		this.jointStrengths = new List<JointStrength>();
		if (this.jointStrengths.Count == 0)
		{
			this.jointStrengths = this.jointStrengthGameObject.transform.GetComponents<JointStrength>().ToList<JointStrength>();
			if (this.jointStrengths.Count == 0 && this.jointStrengthGameObject.transform.parent != null)
			{
				this.jointStrengths = this.jointStrengthGameObject.transform.parent.GetComponents<JointStrength>().ToList<JointStrength>();
			}
			if (this.jointStrengths.Count > 0 && this.jointStrengthLinkedGameObject != null)
			{
				this.jointStrengths = (from x in this.jointStrengths
				where x.jointName == this.jointStrengthLinkedGameObject.name
				select x).ToList<JointStrength>();
			}
			if (this.jointStrengths.Count > 0)
			{
				foreach (JointStrength jointStrength in this.jointStrengths)
				{
					jointStrength.RegisterMuscle(this);
				}
			}
		}
	}

	// Token: 0x06000B48 RID: 2888 RVA: 0x000366A0 File Offset: 0x000348A0
	public void CheckPendingDestruction()
	{
		if (this.destroyPendingProtectingBone && this.protectingBone.destroyed)
		{
			this.Destory(this.pendingDamageOrigin, this.pendingPlayEffects);
		}
	}

	// Token: 0x06000B49 RID: 2889 RVA: 0x000366CC File Offset: 0x000348CC
	public void Destory(DamageOrigin? damageOrigin = null, bool playEffects = true)
	{
		if (this.disableLocalLogic)
		{
			return;
		}
		if (this.destroyed)
		{
			return;
		}
		if (this.protectingBone != null && !this.protectingBone.destroyed)
		{
			this.pendingDamageOrigin = damageOrigin;
			this.destroyPendingProtectingBone = true;
			this.pendingPlayEffects = playEffects;
			return;
		}
		if (damageOrigin == null)
		{
			damageOrigin = new DamageOrigin?(default(DamageOrigin));
		}
		this.destroyed = true;
		if (this.player != null)
		{
			if (this.lethal)
			{
				this.player.Die(this.deathReason);
			}
			else if (this.bloodVessel)
			{
				this.ActivateBleed();
			}
		}
		if (this.isBone)
		{
			base.gameObject.layer = 11;
		}
		if (playEffects)
		{
			this.PlayDestroyVisuals(damageOrigin);
		}
		if (this.isMuscle && this.jointStrengths != null && this.jointStrengths.Count > 0)
		{
			foreach (JointStrength jointStrength in this.jointStrengths)
			{
				jointStrength.DealMuscleDamage();
			}
		}
		if (this.weaponDamageablePartMultiplayerHandler != null && playEffects)
		{
			this.weaponDamageablePartMultiplayerHandler.Destroyed(this.id, damageOrigin.Value);
		}
		if (this.protectedOrgans.Count > 0)
		{
			foreach (WeaponDamageablePart weaponDamageablePart in this.protectedOrgans)
			{
				weaponDamageablePart.CheckPendingDestruction();
			}
		}
	}

	// Token: 0x06000B4A RID: 2890 RVA: 0x00036868 File Offset: 0x00034A68
	public void PlayDestroyVisuals(DamageOrigin? damageOrigin = null)
	{
		if (SoundManager.singleton != null)
		{
			SoundManager.singleton.PlaySoundForOrganDestoyed(base.transform.position, this.organType, damageOrigin);
		}
		if (this.bloodFlowParticles != null && (damageOrigin == null || damageOrigin.Value.EnvironmentSoundType == EnvironmentSoundType.None))
		{
			this.bloodFlowParticles.SetActive(true);
			if (this.player != null && (this.player.alive || !this.bloodVessel))
			{
				this.bloodFlowParticleSystem.Play();
			}
		}
		if (ReplayManager.singleton != null && this.player != null)
		{
			ReplayManager.singleton.RecordDamageablePart(this.id, this.player.gameObject, false, damageOrigin);
		}
	}

	// Token: 0x06000B4B RID: 2891 RVA: 0x00036935 File Offset: 0x00034B35
	public void StopDestroyEffect()
	{
		if (this.weaponDamageablePartMultiplayerHandler != null)
		{
			this.weaponDamageablePartMultiplayerHandler.StopDestroyVisuals(this.id);
		}
		this.StopDestroyVisuals();
	}

	// Token: 0x06000B4C RID: 2892 RVA: 0x0003695C File Offset: 0x00034B5C
	public void StopDestroyVisuals()
	{
		if (this.bloodFlowParticles != null)
		{
			this.bloodFlowParticleSystem.Stop();
		}
		if (ReplayManager.singleton != null && this.player != null)
		{
			ReplayManager.singleton.RecordDamageablePart(this.id, this.player.gameObject, true, null);
		}
	}

	// Token: 0x06000B4D RID: 2893 RVA: 0x000369C2 File Offset: 0x00034BC2
	public void ResetDestroyVisuals()
	{
		if (this.bloodFlowParticles != null)
		{
			this.bloodFlowParticleSystem.Stop();
			this.bloodFlowParticleSystem.Clear();
		}
	}

	// Token: 0x06000B4E RID: 2894 RVA: 0x000369E8 File Offset: 0x00034BE8
	public void SimulateDestroyVisuals(float time)
	{
		if (this.bloodFlowParticles != null)
		{
			ParticleSystem.CollisionModule collision = this.bloodFlowParticleSystem.collision;
			collision.enabled = false;
			this.bloodFlowParticles.SetActive(true);
			this.bloodFlowParticleSystem.Simulate(time, false, true);
			this.bloodFlowParticleSystem.Play();
			collision.enabled = true;
		}
	}

	// Token: 0x06000B4F RID: 2895 RVA: 0x00036A44 File Offset: 0x00034C44
	public void ActivateBleed()
	{
		if (this.bloodVessel && !this.bleedActive)
		{
			this.bleedActive = true;
			this.player.bleedAmount += this.bleedPerSecond;
			this.player.StartBleeding();
		}
	}

	// Token: 0x06000B50 RID: 2896 RVA: 0x0000C7D7 File Offset: 0x0000A9D7
	public bool IsOrgan()
	{
		return true;
	}

	// Token: 0x06000B51 RID: 2897 RVA: 0x00036A80 File Offset: 0x00034C80
	public bool IsBone()
	{
		return this.isBone;
	}

	// Token: 0x06000B52 RID: 2898 RVA: 0x00036A88 File Offset: 0x00034C88
	public List<CuttableGameObject> GetCuttableGameObjects()
	{
		return null;
	}

	// Token: 0x06000B53 RID: 2899 RVA: 0x00036A8C File Offset: 0x00034C8C
	public void TryToSetEffectPosition(GameObject newParent, Vector3 newPosition, Vector3 direction, JointType newJointType)
	{
		if (this.bloodFlowParticles == null)
		{
			return;
		}
		if (this.currentJointType == null || this.currentJointType.Value > newJointType)
		{
			this.bloodFlowParticles.transform.SetParent(newParent.transform);
			this.bloodFlowParticles.transform.localPosition = newPosition;
			this.bloodFlowParticles.transform.LookAt(this.bloodFlowParticles.transform.position + direction);
			this.currentJointType = new JointType?(newJointType);
		}
	}

	// Token: 0x06000B54 RID: 2900 RVA: 0x00036B20 File Offset: 0x00034D20
	public void TryToSetEffectPosition(GameObject newParent, Vector3 newPosition, Quaternion rotation, JointType? newJointType, bool force)
	{
		if (this.bloodFlowParticles == null)
		{
			return;
		}
		if (this.currentJointType == null || (newJointType != null && this.currentJointType.Value > newJointType.Value) || force)
		{
			this.bloodFlowParticles.transform.SetParent(newParent.transform);
			this.bloodFlowParticles.transform.localPosition = newPosition;
			this.bloodFlowParticles.transform.localRotation = rotation;
			this.currentJointType = newJointType;
		}
	}

	// Token: 0x040007CD RID: 1997
	public PlayerHealth player;

	// Token: 0x040007CF RID: 1999
	public bool lethal;

	// Token: 0x040007D0 RID: 2000
	public bool bloodVessel;

	// Token: 0x040007D1 RID: 2001
	public double bleedPerSecond = 0.0666;

	// Token: 0x040007D2 RID: 2002
	public bool destroyed;

	// Token: 0x040007D3 RID: 2003
	public GameObject bloodFlowParticles;

	// Token: 0x040007D4 RID: 2004
	public ParticleSystem bloodFlowParticleSystem;

	// Token: 0x040007D5 RID: 2005
	public DeathReason deathReason;

	// Token: 0x040007D6 RID: 2006
	public OrganType organType;

	// Token: 0x040007D7 RID: 2007
	public int id;

	// Token: 0x040007D8 RID: 2008
	public WeaponDamageablePart protectingBone;

	// Token: 0x040007D9 RID: 2009
	public List<WeaponDamageablePart> protectedOrgans = new List<WeaponDamageablePart>();

	// Token: 0x040007DA RID: 2010
	public bool destroyPendingProtectingBone;

	// Token: 0x040007DB RID: 2011
	private DamageOrigin? pendingDamageOrigin;

	// Token: 0x040007DC RID: 2012
	private bool pendingPlayEffects = true;

	// Token: 0x040007DD RID: 2013
	public bool disableLocalLogic;

	// Token: 0x040007DE RID: 2014
	public List<JointStrength> jointStrengths;

	// Token: 0x040007DF RID: 2015
	public WeaponDamageablePartMultiplayerHandler weaponDamageablePartMultiplayerHandler;

	// Token: 0x040007E0 RID: 2016
	public GameObject jointStrengthGameObject;

	// Token: 0x040007E1 RID: 2017
	public GameObject jointStrengthLinkedGameObject;

	// Token: 0x040007E2 RID: 2018
	private bool bleedActive;

	// Token: 0x040007E3 RID: 2019
	public JointType? currentJointType;
}
