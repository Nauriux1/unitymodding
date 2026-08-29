using System;
using UnityEngine;

// Token: 0x0200009D RID: 157
public class NavigationManager : MonoBehaviour
{
	// Token: 0x06000568 RID: 1384 RVA: 0x000196D2 File Offset: 0x000178D2
	private void Awake()
	{
		this.InitializeNavigationManager();
	}

	// Token: 0x06000569 RID: 1385 RVA: 0x000196DA File Offset: 0x000178DA
	public void InitializeNavigationManager()
	{
		if (NavigationManager.singleton != null)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		NavigationManager.singleton = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		Debug.Log("Navigation manager has been setup");
	}

	// Token: 0x04000341 RID: 833
	public static NavigationManager singleton;
}
