using System;
using UnityEngine;

// Token: 0x02000226 RID: 550
public class QuitOnClick : MonoBehaviour
{
	// Token: 0x060010B3 RID: 4275 RVA: 0x0005628D File Offset: 0x0005448D
	public void Quit()
	{
		if (SteamManager.steamManager != null)
		{
			UnityEngine.Object.Destroy(SteamManager.steamManager.gameObject);
		}
		Application.Quit();
	}
}
