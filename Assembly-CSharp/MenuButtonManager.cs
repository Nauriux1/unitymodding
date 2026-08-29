using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

// Token: 0x020000FF RID: 255
public class MenuButtonManager : MonoBehaviour
{
	// Token: 0x0600085F RID: 2143 RVA: 0x00029A48 File Offset: 0x00027C48
	private void Start()
	{
		this.userControls = SettingsHelper.GetUserControls();
		this.userControls.Generic.Enable();
	}

	// Token: 0x06000860 RID: 2144 RVA: 0x00029A74 File Offset: 0x00027C74
	private void Update()
	{
		if (this.userControls.Generic.Back.WasPerformedThisFrame())
		{
			this.DoBackNavigation(this.userControls.Generic.Back.activeControl.device);
		}
	}

	// Token: 0x06000861 RID: 2145 RVA: 0x00029ABE File Offset: 0x00027CBE
	private void DoBackNavigation(InputDevice device)
	{
		if (GeneralManager.AllowBackNavigation(device))
		{
			SceneManagerWithParameters.LoadPreviousScene();
		}
	}

	// Token: 0x06000862 RID: 2146 RVA: 0x00029ACD File Offset: 0x00027CCD
	private void OnDestroy()
	{
		this.DisposeUserControls();
	}

	// Token: 0x06000863 RID: 2147 RVA: 0x00029AD5 File Offset: 0x00027CD5
	public void DisposeUserControls()
	{
		if (this.userControls != null)
		{
			this.userControls.Disable();
			this.userControls.Dispose();
		}
	}

	// Token: 0x040005CE RID: 1486
	public UserControls userControls;
}
