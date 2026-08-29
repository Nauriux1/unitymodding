using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

// Token: 0x020000FD RID: 253
public class BackToSceneManager : MonoBehaviour
{
	// Token: 0x06000855 RID: 2133 RVA: 0x00029948 File Offset: 0x00027B48
	private void Start()
	{
		this.userControls = SettingsHelper.GetUserControls();
		this.userControls.Generic.Enable();
	}

	// Token: 0x06000856 RID: 2134 RVA: 0x00029974 File Offset: 0x00027B74
	private void Update()
	{
		if (this.userControls.Generic.Back.WasPerformedThisFrame() && GeneralManager.AllowBackNavigation(null))
		{
			SceneManager.LoadScene(this.sceneName);
		}
	}

	// Token: 0x06000857 RID: 2135 RVA: 0x000299AE File Offset: 0x00027BAE
	private void OnDestroy()
	{
		this.DisposeUserControls();
	}

	// Token: 0x06000858 RID: 2136 RVA: 0x000299B6 File Offset: 0x00027BB6
	public void DisposeUserControls()
	{
		if (this.userControls != null)
		{
			this.userControls.Disable();
			this.userControls.Dispose();
		}
	}

	// Token: 0x040005CA RID: 1482
	public UserControls userControls;

	// Token: 0x040005CB RID: 1483
	public string sceneName;
}
