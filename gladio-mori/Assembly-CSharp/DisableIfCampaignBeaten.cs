using System;
using UnityEngine;
using Utils;

// Token: 0x02000161 RID: 353
public class DisableIfCampaignBeaten : MonoBehaviour
{
	// Token: 0x06000B57 RID: 2903 RVA: 0x00036BF2 File Offset: 0x00034DF2
	private void Start()
	{
		if (SettingsHelper.persistentSave.wins > 0)
		{
			base.gameObject.SetActive(false);
		}
	}
}
