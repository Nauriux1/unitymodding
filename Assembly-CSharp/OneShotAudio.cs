using System;
using UnityEngine;

// Token: 0x020000E3 RID: 227
public class OneShotAudio
{
	// Token: 0x060007D2 RID: 2002 RVA: 0x00026B41 File Offset: 0x00024D41
	public void Disable()
	{
		this.gameObject.SetActive(false);
	}

	// Token: 0x060007D3 RID: 2003 RVA: 0x00026B4F File Offset: 0x00024D4F
	public void Enable()
	{
		this.gameObject.SetActive(true);
	}

	// Token: 0x0400053C RID: 1340
	public GameObject gameObject;

	// Token: 0x0400053D RID: 1341
	public AudioSource audioSource;

	// Token: 0x0400053E RID: 1342
	public float removeAtTime;
}
