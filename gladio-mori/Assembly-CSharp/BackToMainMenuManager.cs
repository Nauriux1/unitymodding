using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

// Token: 0x020000FC RID: 252
public class BackToMainMenuManager : MonoBehaviour
{
	// Token: 0x0600084F RID: 2127 RVA: 0x000298B0 File Offset: 0x00027AB0
	private void Start()
	{
		this.userControls = SettingsHelper.GetUserControls();
		this.userControls.Generic.Enable();
	}

	// Token: 0x06000850 RID: 2128 RVA: 0x000298DC File Offset: 0x00027ADC
	private void Update()
	{
		if (this.userControls.Generic.Back.WasPerformedThisFrame())
		{
			this.NavigateToMainMenu();
		}
	}

	// Token: 0x06000851 RID: 2129 RVA: 0x00029909 File Offset: 0x00027B09
	private void NavigateToMainMenu()
	{
		if (!GeneralManager.AllowBackNavigation(null))
		{
			return;
		}
		SceneManager.LoadScene("MainMenu");
	}

	// Token: 0x06000852 RID: 2130 RVA: 0x0002991E File Offset: 0x00027B1E
	private void OnDestroy()
	{
		this.DisposeUserControls();
	}

	// Token: 0x06000853 RID: 2131 RVA: 0x00029926 File Offset: 0x00027B26
	public void DisposeUserControls()
	{
		if (this.userControls != null)
		{
			this.userControls.Disable();
			this.userControls.Dispose();
		}
	}

	// Token: 0x040005C9 RID: 1481
	public UserControls userControls;
}
