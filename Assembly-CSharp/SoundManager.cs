using System;
using System.Collections.Generic;
using MoveClasses;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x020000DD RID: 221
public class SoundManager : MonoBehaviour
{
	// Token: 0x060007B3 RID: 1971 RVA: 0x000261E3 File Offset: 0x000243E3
	private void Awake()
	{
		this.InitializeSoundManager();
	}

	// Token: 0x060007B4 RID: 1972 RVA: 0x000261EB File Offset: 0x000243EB
	public void DisableLocalSound()
	{
		this.localSoundDisabled = true;
	}

	// Token: 0x060007B5 RID: 1973 RVA: 0x000261F4 File Offset: 0x000243F4
	private void InitializeSoundManager()
	{
		SoundManager.singleton = this;
		this.InitExistingCollision();
		this.InitOneShotAudio();
	}

	// Token: 0x060007B6 RID: 1974 RVA: 0x00026208 File Offset: 0x00024408
	private void Update()
	{
		this.CleanActiveOneShotAudios();
		if (this.localSoundDisabled)
		{
			return;
		}
		for (int i = this.existingCollisions.Count - 1; i > -1; i--)
		{
			ExistingCollision existingCollision = this.existingCollisions[i];
			if (Time.time >= existingCollision.removeAtTime)
			{
				this.existingCollisions.RemoveAt(i);
				this.ReturnExistingCollisionToPool(existingCollision);
			}
		}
	}

	// Token: 0x060007B7 RID: 1975 RVA: 0x0002626C File Offset: 0x0002446C
	public void PlaySoundForCollision(Collision collision, GameObject collidingGameObject, SoundMaterialType? collidingGameObjectSoundMaterial = null)
	{
		if (this.localSoundDisabled)
		{
			return;
		}
		if (collision.gameObject == null)
		{
			return;
		}
		if (this.existingCollisions.Count > 0)
		{
			foreach (ExistingCollision existingCollision in this.existingCollisions)
			{
				if ((existingCollision.gameObject1 == collidingGameObject || existingCollision.gameObject1 == collision.gameObject) && (existingCollision.gameObject2 == collidingGameObject || existingCollision.gameObject2 == collision.gameObject))
				{
					return;
				}
			}
		}
		SoundMaterialType? soundMaterialType = null;
		SoundMaterialType? soundMaterialType2 = null;
		Vector3 position = default(Vector3);
		ExistingCollision existingCollisionFromPool = this.GetExistingCollisionFromPool();
		existingCollisionFromPool.gameObject1 = collision.gameObject;
		existingCollisionFromPool.gameObject2 = collidingGameObject;
		existingCollisionFromPool.removeAtTime = Time.time + 0.5f;
		this.existingCollisions.Add(existingCollisionFromPool);
		this.contacts.Clear();
		collision.GetContacts(this.contacts);
		if (this.contacts != null && this.contacts.Count > 0)
		{
			ContactPoint contactPoint = this.contacts[0];
			position = contactPoint.point;
			SoundMaterial component = contactPoint.otherCollider.gameObject.GetComponent<SoundMaterial>();
			if (component == null && contactPoint.otherCollider.transform.parent != null)
			{
				component = contactPoint.otherCollider.transform.parent.GetComponent<SoundMaterial>();
			}
			if (component != null)
			{
				soundMaterialType = new SoundMaterialType?(component.SoundMaterialType);
				if (soundMaterialType2 == null)
				{
					SoundMaterial component2 = contactPoint.thisCollider.gameObject.GetComponent<SoundMaterial>();
					if (component2 == null && contactPoint.thisCollider.transform.parent != null)
					{
						component2 = contactPoint.thisCollider.transform.parent.GetComponent<SoundMaterial>();
					}
					if (component2 != null)
					{
						if (component2.SoundMaterialType < soundMaterialType.Value)
						{
							soundMaterialType2 = soundMaterialType;
							soundMaterialType = new SoundMaterialType?(component2.SoundMaterialType);
						}
						else
						{
							soundMaterialType2 = new SoundMaterialType?(component2.SoundMaterialType);
						}
					}
				}
			}
		}
		if (soundMaterialType2 == null)
		{
			soundMaterialType2 = collidingGameObjectSoundMaterial;
		}
		if (soundMaterialType != null && soundMaterialType2 != null)
		{
			CollisionSound collisionSoundTypeAndVolume = this.GetCollisionSoundTypeAndVolume(soundMaterialType.Value, soundMaterialType2.Value, collision);
			if (collisionSoundTypeAndVolume.Volume > 0f)
			{
				this.PlaySound(collisionSoundTypeAndVolume.CollisionSoundType, position, collisionSoundTypeAndVolume.Volume);
				if (this.soundManagerMultiplayer != null)
				{
					this.soundManagerMultiplayer.PlaySound(collisionSoundTypeAndVolume.CollisionSoundType, position, collisionSoundTypeAndVolume.Volume);
				}
			}
		}
	}

	// Token: 0x060007B8 RID: 1976 RVA: 0x00026540 File Offset: 0x00024740
	public void PlaySound(AudioClip clip, Vector3 position, float volume = 1f, bool spatialBlend = true)
	{
		this.PlaySoundAtPoint(clip, position, volume, spatialBlend);
	}

	// Token: 0x060007B9 RID: 1977 RVA: 0x00026550 File Offset: 0x00024750
	public void PlaySound(CollisionSoundType collisionSoundType, Vector3 position, float volume = 1f)
	{
		CollisionSound collisionSound = this.GetCollisionSound(collisionSoundType);
		if (collisionSound != null && collisionSound.AudioClip != null)
		{
			this.PlaySoundAtPoint(collisionSound.AudioClip, position, volume, true);
			if (ReplayManager.singleton != null && ReplayManager.singleton.replayMode == ReplayMode.Record)
			{
				ReplayManager.singleton.RecordSound(collisionSoundType, position, volume);
			}
		}
	}

	// Token: 0x060007BA RID: 1978 RVA: 0x000265AC File Offset: 0x000247AC
	public void PlaySoundForOrganDestoyed(Vector3 position, OrganType organType, DamageOrigin? damageOrigin = null)
	{
		if (damageOrigin != null && damageOrigin.Value.EnvironmentSoundType != EnvironmentSoundType.None)
		{
			this.PlaySoundForEnvironment(position, damageOrigin.Value.EnvironmentSoundType);
			return;
		}
		OrganTypeSound organTypeSound = this.GetOrganTypeSound(organType);
		if (organTypeSound != null)
		{
			this.PlaySoundAtPoint(organTypeSound.AudioClip, position, 1f, true);
		}
	}

	// Token: 0x060007BB RID: 1979 RVA: 0x00026604 File Offset: 0x00024804
	public void PlaySoundForEnvironment(Vector3 position, EnvironmentSoundType environmentSoundType)
	{
		EnvironmentTypeSound environmentTypeSound = this.GetEnvironmentTypeSound(environmentSoundType);
		if (environmentTypeSound != null)
		{
			if (environmentTypeSound.EnvironmentSoundType == EnvironmentSoundType.Lava)
			{
				if ((double)this.lastLavaSoundPlayed + 0.1 > (double)Time.unscaledTime)
				{
					return;
				}
				this.lastLavaSoundPlayed = Time.unscaledTime;
			}
			this.PlaySoundAtPoint(environmentTypeSound.AudioClip, position, 1f, true);
		}
	}

	// Token: 0x060007BC RID: 1980 RVA: 0x00026660 File Offset: 0x00024860
	public void PlayGeneralSound(Vector3 position, float volume, GeneralSoundType generalSoundType)
	{
		GeneralSound generalTypeSound = this.GetGeneralTypeSound(generalSoundType);
		if (generalTypeSound == null)
		{
			return;
		}
		this.PlaySoundAtPoint(generalTypeSound.AudioClip, position, volume, true);
	}

	// Token: 0x060007BD RID: 1981 RVA: 0x00026688 File Offset: 0x00024888
	public GeneralSound GetGeneralTypeSound(GeneralSoundType generalSoundType)
	{
		for (int i = 0; i < this.generalTypeSoundList.Count; i++)
		{
			GeneralSound generalSound = this.generalTypeSoundList[i];
			if (generalSound.GeneralSoundType == generalSoundType)
			{
				return generalSound;
			}
		}
		return null;
	}

	// Token: 0x060007BE RID: 1982 RVA: 0x000266C4 File Offset: 0x000248C4
	public void PlaySoundAtPoint(AudioClip clip, Vector3 position, float volume = 1f, bool spatialBlend = true)
	{
		if (clip == null)
		{
			return;
		}
		OneShotAudio oneShotAudioFromPool = this.GetOneShotAudioFromPool();
		oneShotAudioFromPool.Enable();
		oneShotAudioFromPool.gameObject.transform.position = position;
		oneShotAudioFromPool.audioSource.clip = clip;
		oneShotAudioFromPool.audioSource.pitch = Time.timeScale;
		if (spatialBlend)
		{
			oneShotAudioFromPool.audioSource.spatialBlend = 1f;
		}
		else
		{
			oneShotAudioFromPool.audioSource.spatialBlend = 0f;
		}
		oneShotAudioFromPool.audioSource.volume = volume;
		oneShotAudioFromPool.audioSource.Play();
		oneShotAudioFromPool.removeAtTime = Time.time + clip.length * (((double)Time.timeScale < 0.009999999776482582) ? 0.01f : Time.timeScale);
	}

	// Token: 0x060007BF RID: 1983 RVA: 0x00026784 File Offset: 0x00024984
	public CollisionSound GetCollisionSoundTypeAndVolume(SoundMaterialType material1, SoundMaterialType material2, Collision collision)
	{
		float num = collision.relativeVelocity.magnitude;
		if (num > 1f)
		{
			num = 1f;
		}
		CollisionSound collisionSound = new CollisionSound
		{
			CollisionSoundType = CollisionSoundType.Default,
			Volume = num
		};
		if (material1 == SoundMaterialType.Metal)
		{
			if (material2 == SoundMaterialType.Metal)
			{
				collisionSound.CollisionSoundType = CollisionSoundType.MetalOnMetal;
			}
			else if (material2 == SoundMaterialType.Wood)
			{
				collisionSound.CollisionSoundType = CollisionSoundType.WoodOnWood;
			}
			else if (material2 == SoundMaterialType.Player)
			{
				collisionSound.CollisionSoundType = CollisionSoundType.PlayerOnPlayer;
			}
		}
		else if (material1 == SoundMaterialType.Wood)
		{
			if (material2 == SoundMaterialType.Metal)
			{
				collisionSound.CollisionSoundType = CollisionSoundType.WoodOnWood;
			}
			else if (material2 == SoundMaterialType.Wood)
			{
				collisionSound.CollisionSoundType = CollisionSoundType.WoodOnWood;
			}
			else if (material2 == SoundMaterialType.Player)
			{
				collisionSound.CollisionSoundType = CollisionSoundType.PlayerOnPlayer;
			}
		}
		else if (material1 == SoundMaterialType.Player)
		{
			if (material2 == SoundMaterialType.Metal)
			{
				collisionSound.CollisionSoundType = CollisionSoundType.PlayerOnPlayer;
			}
			else if (material2 == SoundMaterialType.Wood)
			{
				collisionSound.CollisionSoundType = CollisionSoundType.PlayerOnPlayer;
			}
			else if (material2 == SoundMaterialType.Player)
			{
				collisionSound.CollisionSoundType = CollisionSoundType.PlayerOnPlayer;
			}
		}
		return collisionSound;
	}

	// Token: 0x060007C0 RID: 1984 RVA: 0x00026840 File Offset: 0x00024A40
	private void InitExistingCollision()
	{
		this.pool_existingCollisions = new List<ExistingCollision>(32);
		this.existingCollisions = new List<ExistingCollision>(64);
		for (int i = 0; i < 16; i++)
		{
			this.pool_existingCollisions.Add(new ExistingCollision());
		}
	}

	// Token: 0x060007C1 RID: 1985 RVA: 0x00026884 File Offset: 0x00024A84
	private ExistingCollision GetExistingCollisionFromPool()
	{
		ExistingCollision existingCollision = null;
		if (this.pool_existingCollisions.Count > 0)
		{
			int index = this.pool_existingCollisions.Count - 1;
			existingCollision = this.pool_existingCollisions[index];
			this.pool_existingCollisions.RemoveAt(index);
			existingCollision.Clear();
		}
		if (existingCollision == null)
		{
			existingCollision = new ExistingCollision();
		}
		return existingCollision;
	}

	// Token: 0x060007C2 RID: 1986 RVA: 0x000268D8 File Offset: 0x00024AD8
	private void ReturnExistingCollisionToPool(ExistingCollision existingCollision)
	{
		this.pool_existingCollisions.Add(existingCollision);
	}

	// Token: 0x060007C3 RID: 1987 RVA: 0x000268E8 File Offset: 0x00024AE8
	public CollisionSound GetCollisionSound(CollisionSoundType collisionSoundType)
	{
		for (int i = 0; i < this.collisionSoundList.Count; i++)
		{
			CollisionSound collisionSound = this.collisionSoundList[i];
			if (collisionSound.CollisionSoundType == collisionSoundType)
			{
				return collisionSound;
			}
		}
		return null;
	}

	// Token: 0x060007C4 RID: 1988 RVA: 0x00026924 File Offset: 0x00024B24
	public OrganTypeSound GetOrganTypeSound(OrganType organType)
	{
		for (int i = 0; i < this.organTypeSoundList.Count; i++)
		{
			OrganTypeSound organTypeSound = this.organTypeSoundList[i];
			if (organTypeSound.OrganType == organType)
			{
				return organTypeSound;
			}
		}
		return null;
	}

	// Token: 0x060007C5 RID: 1989 RVA: 0x00026960 File Offset: 0x00024B60
	public EnvironmentTypeSound GetEnvironmentTypeSound(EnvironmentSoundType environmentSoundType)
	{
		for (int i = 0; i < this.environmentTypeSoundList.Count; i++)
		{
			EnvironmentTypeSound environmentTypeSound = this.environmentTypeSoundList[i];
			if (environmentTypeSound.EnvironmentSoundType == environmentSoundType)
			{
				return environmentTypeSound;
			}
		}
		return null;
	}

	// Token: 0x060007C6 RID: 1990 RVA: 0x0002699C File Offset: 0x00024B9C
	private void InitOneShotAudio()
	{
		this.pool_oneShotAudio = new List<OneShotAudio>(64);
		this.activeOneShotAudios = new List<OneShotAudio>(64);
		for (int i = 0; i < 64; i++)
		{
			this.pool_oneShotAudio.Add(this.CreateNewOneShotAudio());
		}
	}

	// Token: 0x060007C7 RID: 1991 RVA: 0x000269E4 File Offset: 0x00024BE4
	private OneShotAudio GetOneShotAudioFromPool()
	{
		OneShotAudio oneShotAudio = null;
		if (this.pool_oneShotAudio.Count > 0)
		{
			int index = this.pool_oneShotAudio.Count - 1;
			oneShotAudio = this.pool_oneShotAudio[index];
			this.pool_oneShotAudio.RemoveAt(index);
		}
		if (oneShotAudio == null)
		{
			oneShotAudio = this.CreateNewOneShotAudio();
		}
		this.activeOneShotAudios.Add(oneShotAudio);
		return oneShotAudio;
	}

	// Token: 0x060007C8 RID: 1992 RVA: 0x00026A40 File Offset: 0x00024C40
	private OneShotAudio CreateNewOneShotAudio()
	{
		OneShotAudio oneShotAudio = new OneShotAudio();
		oneShotAudio.gameObject = new GameObject("One shot audio");
		oneShotAudio.audioSource = (AudioSource)oneShotAudio.gameObject.AddComponent(typeof(AudioSource));
		oneShotAudio.audioSource.outputAudioMixerGroup = this.audioMixerGroupEffects;
		oneShotAudio.audioSource.playOnAwake = false;
		oneShotAudio.Disable();
		return oneShotAudio;
	}

	// Token: 0x060007C9 RID: 1993 RVA: 0x00026AA5 File Offset: 0x00024CA5
	private void ReturnOneShotAudioToPool(OneShotAudio oneShotAudio)
	{
		oneShotAudio.Disable();
		this.pool_oneShotAudio.Add(oneShotAudio);
	}

	// Token: 0x060007CA RID: 1994 RVA: 0x00026ABC File Offset: 0x00024CBC
	private void CleanActiveOneShotAudios()
	{
		for (int i = this.activeOneShotAudios.Count - 1; i > -1; i--)
		{
			OneShotAudio oneShotAudio = this.activeOneShotAudios[i];
			if (Time.time >= oneShotAudio.removeAtTime)
			{
				this.activeOneShotAudios.RemoveAt(i);
				this.ReturnOneShotAudioToPool(oneShotAudio);
			}
		}
	}

	// Token: 0x04000522 RID: 1314
	public List<CollisionSound> collisionSoundList;

	// Token: 0x04000523 RID: 1315
	public List<OrganTypeSound> organTypeSoundList;

	// Token: 0x04000524 RID: 1316
	public List<EnvironmentTypeSound> environmentTypeSoundList;

	// Token: 0x04000525 RID: 1317
	public List<GeneralSound> generalTypeSoundList;

	// Token: 0x04000526 RID: 1318
	public static SoundManager singleton;

	// Token: 0x04000527 RID: 1319
	public List<ExistingCollision> existingCollisions;

	// Token: 0x04000528 RID: 1320
	public SoundManagerMultiplayer soundManagerMultiplayer;

	// Token: 0x04000529 RID: 1321
	public bool localSoundDisabled;

	// Token: 0x0400052A RID: 1322
	public AudioMixerGroup audioMixerGroupEffects;

	// Token: 0x0400052B RID: 1323
	private List<ContactPoint> contacts = new List<ContactPoint>(128);

	// Token: 0x0400052C RID: 1324
	private float lastLavaSoundPlayed;

	// Token: 0x0400052D RID: 1325
	public List<ExistingCollision> pool_existingCollisions;

	// Token: 0x0400052E RID: 1326
	public List<OneShotAudio> pool_oneShotAudio;

	// Token: 0x0400052F RID: 1327
	public List<OneShotAudio> activeOneShotAudios;
}
