using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200022B RID: 555
public class ParticleSpawner : MonoBehaviour
{
	// Token: 0x060010C9 RID: 4297 RVA: 0x00056800 File Offset: 0x00054A00
	private void Start()
	{
		this.particleSpawner = base.GetComponent<ParticleSystem>();
		this.collisionEvents = new List<ParticleCollisionEvent>();
		this.particleDisplayer = ParticleDisplayer.singleton;
	}

	// Token: 0x060010CA RID: 4298 RVA: 0x0000777A File Offset: 0x0000597A
	private void Update()
	{
	}

	// Token: 0x060010CB RID: 4299 RVA: 0x00056824 File Offset: 0x00054A24
	private void OnParticleCollision(GameObject other)
	{
		int num = this.particleSpawner.GetCollisionEvents(other, this.collisionEvents);
		for (int i = 0; i < num; i++)
		{
			this.particleDisplayer.AddBloodSpatter(this.collisionEvents[i]);
		}
	}

	// Token: 0x04000C30 RID: 3120
	public ParticleSystem particleSpawner;

	// Token: 0x04000C31 RID: 3121
	public List<ParticleCollisionEvent> collisionEvents;

	// Token: 0x04000C32 RID: 3122
	public ParticleDisplayer particleDisplayer;
}
