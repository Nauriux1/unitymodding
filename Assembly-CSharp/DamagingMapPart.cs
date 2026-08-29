using System;
using UnityEngine;

// Token: 0x02000102 RID: 258
public class DamagingMapPart : MonoBehaviour
{
	// Token: 0x06000873 RID: 2163 RVA: 0x0000777A File Offset: 0x0000597A
	private void Start()
	{
	}

	// Token: 0x06000874 RID: 2164 RVA: 0x00029C9C File Offset: 0x00027E9C
	public virtual void OnTriggerEnter(Collider collision)
	{
		IWeaponDamageable component = collision.transform.GetComponent<IWeaponDamageable>();
		if (component == null && collision.transform.parent != null)
		{
			component = collision.transform.parent.GetComponent<IWeaponDamageable>();
		}
		if (component != null)
		{
			component.Destory(new DamageOrigin?(new DamageOrigin
			{
				EnvironmentSoundType = this.environmentSoundType
			}), true);
			if (!component.IsOrgan() && !component.IsBone())
			{
				Vector3 position = collision.ClosestPoint(collision.transform.position);
				if (this.prefabPoolParticles != null)
				{
					PrefabPoolObject prefabPoolObjectFromPool = this.prefabPoolParticles.GetPrefabPoolObjectFromPool();
					prefabPoolObjectFromPool.Enable();
					prefabPoolObjectFromPool.gameObject.transform.position = position;
					prefabPoolObjectFromPool.particleSystem.Clear();
					prefabPoolObjectFromPool.particleSystem.Play();
					prefabPoolObjectFromPool.removeAtTime = new float?(Time.time + prefabPoolObjectFromPool.particleSystem.main.startLifetime.constantMax);
				}
				if (SoundManager.singleton != null)
				{
					SoundManager.singleton.PlaySoundForEnvironment(position, this.environmentSoundType);
				}
			}
		}
	}

	// Token: 0x040005D6 RID: 1494
	public EnvironmentSoundType environmentSoundType;

	// Token: 0x040005D7 RID: 1495
	public PrefabPool prefabPoolParticles;
}
