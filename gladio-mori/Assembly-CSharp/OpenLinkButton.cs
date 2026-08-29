using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000041 RID: 65
public class OpenLinkButton : MonoBehaviour
{
	// Token: 0x060001F3 RID: 499 RVA: 0x0000B84C File Offset: 0x00009A4C
	private void Awake()
	{
		this.button = base.GetComponent<Button>();
		if (this.button != null)
		{
			this.button.onClick.AddListener(delegate()
			{
				this.OpenLink();
			});
		}
	}

	// Token: 0x060001F4 RID: 500 RVA: 0x0000B884 File Offset: 0x00009A84
	private void OpenLink()
	{
		if (this.storeLink)
		{
			if (SteamManager.steamManager != null)
			{
				SteamManager.steamManager.OpenAppStore();
				return;
			}
		}
		else if (!string.IsNullOrEmpty(this.link))
		{
			Application.OpenURL(this.link);
		}
	}

	// Token: 0x04000144 RID: 324
	private Button button;

	// Token: 0x04000145 RID: 325
	public string link;

	// Token: 0x04000146 RID: 326
	public bool storeLink;
}
