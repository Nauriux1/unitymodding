using System;
using UnityEngine;

// Token: 0x0200004A RID: 74
[Serializable]
public class PrefabPoolObject
{
	// Token: 0x0600021C RID: 540 RVA: 0x0000C153 File Offset: 0x0000A353
	public void Disable()
	{
		this.gameObject.SetActive(false);
	}

	// Token: 0x0600021D RID: 541 RVA: 0x0000C161 File Offset: 0x0000A361
	public void Enable()
	{
		this.gameObject.SetActive(true);
		this.removeAtTime = null;
		this.remove = false;
	}

	// Token: 0x04000169 RID: 361
	public GameObject gameObject;

	// Token: 0x0400016A RID: 362
	public ParticleSystem particleSystem;

	// Token: 0x0400016B RID: 363
	public float? removeAtTime;

	// Token: 0x0400016C RID: 364
	public bool remove;
}
