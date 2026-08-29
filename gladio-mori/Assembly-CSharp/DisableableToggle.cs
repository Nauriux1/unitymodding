using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001BB RID: 443
public class DisableableToggle : MonoBehaviour, IDisableableGameSetting
{
	// Token: 0x06000D59 RID: 3417 RVA: 0x00043D0C File Offset: 0x00041F0C
	public void DisableGameSetting()
	{
		if (this.toggle != null)
		{
			this.toggle.interactable = false;
		}
	}

	// Token: 0x040009A5 RID: 2469
	public Toggle toggle;
}
