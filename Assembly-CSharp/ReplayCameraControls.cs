using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using Utils;

// Token: 0x020000BE RID: 190
public class ReplayCameraControls : MonoBehaviour, IDisableableInputManager
{
	// Token: 0x06000691 RID: 1681 RVA: 0x00021588 File Offset: 0x0001F788
	private void Awake()
	{
		this.InitializeReplayCameraControls();
	}

	// Token: 0x06000692 RID: 1682 RVA: 0x00021590 File Offset: 0x0001F790
	public static CameraMode CurrentCameraMode()
	{
		if (ReplayCameraControls.singleton != null)
		{
			return ReplayCameraControls.singleton.cameraMode;
		}
		return CameraMode.None;
	}

	// Token: 0x06000693 RID: 1683 RVA: 0x000215AC File Offset: 0x0001F7AC
	public void InitializeReplayCameraControls()
	{
		if (ReplayCameraControls.singleton != null)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		if (ReplayManager.singleton != null)
		{
			this.replayToolsManager = ReplayManager.singleton.replayToolsManager;
		}
		if (this.replayToolsManager == null)
		{
			this.replayToolsManager = null;
		}
		ReplayCameraControls.singleton = this;
		this.FillOptions();
		this.SetupUserControls();
		this.SetupReplayCamera();
		LocalizationSettings.SelectedLocaleChanged += this.OnSelectedLocaleChanged;
	}

	// Token: 0x06000694 RID: 1684 RVA: 0x00021628 File Offset: 0x0001F828
	private void OnSelectedLocaleChanged(Locale locale)
	{
		this.FillOptions();
	}

	// Token: 0x06000695 RID: 1685 RVA: 0x00021630 File Offset: 0x0001F830
	private void FillOptions()
	{
		if (this.replayToolsManager != null)
		{
			this.replayToolsManager.cameraModeDropdown.options.Clear();
			this.replayToolsManager.cameraModeDropdown.options.Add(new OptionDataWithValue
			{
				text = LocalizationHelpers.LocalizedText("txt_cameramode_freelook", Array.Empty<object>())
			});
			this.replayToolsManager.cameraModeDropdown.options.Add(new OptionDataWithValue
			{
				text = LocalizationHelpers.LocalizedText("txt_cameramode_follow", Array.Empty<object>())
			});
			this.replayToolsManager.cameraModeDropdown.options.Add(new OptionDataWithValue
			{
				text = LocalizationHelpers.LocalizedText("txt_cameramode_cinematic", Array.Empty<object>())
			});
			this.replayToolsManager.cameraModeDropdown.RefreshShownValue();
		}
	}

	// Token: 0x06000696 RID: 1686 RVA: 0x00021700 File Offset: 0x0001F900
	public void SetupUserControls()
	{
		if (this.userControls != null)
		{
			this.userControls.Dispose();
		}
		this.userControls = SettingsHelper.GetUserControls();
		this.userControls.ReplayMap.SetCameraMode1.performed += delegate(InputAction.CallbackContext <p0>)
		{
			this.SetCameraMode(CameraMode.Free);
		};
		this.userControls.ReplayMap.SetCameraMode2.performed += delegate(InputAction.CallbackContext <p0>)
		{
			this.SetCameraMode(CameraMode.Follow);
		};
		this.userControls.ReplayMap.SetCameraMode3.performed += delegate(InputAction.CallbackContext <p0>)
		{
			this.SetCameraMode(CameraMode.Cinematic);
		};
		this.userControls.ReplayMap.PreviousPlayer.performed += delegate(InputAction.CallbackContext <p0>)
		{
			this.SetFollowedPlayer(this.currentlyFollowedPlayer - 1);
		};
		this.userControls.ReplayMap.NextPlayer.performed += delegate(InputAction.CallbackContext <p0>)
		{
			this.SetFollowedPlayer(this.currentlyFollowedPlayer + 1);
		};
		if (this.userControlsEnabled)
		{
			this.userControls.ReplayMap.Enable();
		}
	}

	// Token: 0x06000697 RID: 1687 RVA: 0x000217FA File Offset: 0x0001F9FA
	private void OnDestroy()
	{
		LocalizationSettings.SelectedLocaleChanged -= this.OnSelectedLocaleChanged;
		this.DisposeUserControls();
	}

	// Token: 0x06000698 RID: 1688 RVA: 0x00021813 File Offset: 0x0001FA13
	public void DisposeUserControls()
	{
		if (this.userControls != null)
		{
			this.userControls.Disable();
			this.userControls.Dispose();
		}
	}

	// Token: 0x06000699 RID: 1689 RVA: 0x00021834 File Offset: 0x0001FA34
	private void SetupReplayCamera()
	{
		if (Camera.main != null)
		{
			this.cameraFreeControls = Camera.main.gameObject.GetComponent<ReplayFreeCamera>();
			if (this.cameraFreeControls == null)
			{
				this.cameraFreeControls = Camera.main.gameObject.AddComponent<ReplayFreeCamera>();
			}
			this.cameraFollowAllPlayers = Camera.main.gameObject.GetComponent<CameraFollowPlayers>();
			if (this.cameraFollowAllPlayers == null)
			{
				this.cameraFollowAllPlayers = Camera.main.gameObject.AddComponent<CameraFollowPlayers>();
			}
			if (Camera.main.gameObject.GetComponent<CameraSettings>() == null)
			{
				Camera.main.gameObject.AddComponent<CameraSettings>();
			}
			CameraSmoothFollowControllable component = Camera.main.gameObject.GetComponent<CameraSmoothFollowControllable>();
			if (component != null)
			{
				UnityEngine.Object.Destroy(component);
			}
			this.cameraSmoothFollow = Camera.main.gameObject.GetComponent<CameraSmoothFollow>();
			if (this.cameraSmoothFollow == null || this.cameraSmoothFollow.GetType() == typeof(CameraSmoothFollowControllable))
			{
				this.cameraSmoothFollow = Camera.main.gameObject.AddComponent<CameraSmoothFollow>();
				this.cameraSmoothFollow.enabled = false;
			}
			this.SetCameraMode(this.cameraMode);
		}
	}

	// Token: 0x0600069A RID: 1690 RVA: 0x00021974 File Offset: 0x0001FB74
	public void SetCameraMode(CameraMode newMode)
	{
		this.cameraMode = newMode;
		ReplayToolsManager replayToolsManager = this.replayToolsManager;
		if (replayToolsManager != null)
		{
			replayToolsManager.followPlayerDropdown.gameObject.SetActive(false);
		}
		if (this.cameraMode == CameraMode.Free)
		{
			this.cameraFreeControls.enabled = true;
			this.cameraSmoothFollow.enabled = false;
			this.cameraFollowAllPlayers.enabled = false;
		}
		else if (this.cameraMode == CameraMode.Cinematic)
		{
			this.cameraFollowAllPlayers.UpdateTargets();
			this.cameraFreeControls.enabled = false;
			this.cameraFollowAllPlayers.enabled = true;
			this.cameraSmoothFollow.enabled = false;
		}
		else
		{
			this.cameraFreeControls.enabled = false;
			this.cameraFollowAllPlayers.enabled = false;
			this.cameraSmoothFollow.enabled = true;
			this.SetFollowedPlayer(0);
			ReplayToolsManager replayToolsManager2 = this.replayToolsManager;
			if (replayToolsManager2 != null)
			{
				replayToolsManager2.followPlayerDropdown.gameObject.SetActive(true);
			}
		}
		ReplayToolsManager replayToolsManager3 = this.replayToolsManager;
		if (replayToolsManager3 == null)
		{
			return;
		}
		replayToolsManager3.cameraModeDropdown.SetValueWithoutNotify((int)this.cameraMode);
	}

	// Token: 0x0600069B RID: 1691 RVA: 0x00021A74 File Offset: 0x0001FC74
	public void SetFollowedPlayer(int followedPlayer)
	{
		List<PlayerHealth> list = null;
		if (ReplayManager.singleton != null && ReplayManager.singleton.replayMode == ReplayMode.Replay && ReplayManager.singleton.recordingPlayers != null)
		{
			list = ReplayManager.singleton.recordingPlayers;
		}
		else if (GeneralManager.singleton != null)
		{
			list = GeneralManager.singleton.registeredPlayerHealths;
		}
		if (list != null)
		{
			if (followedPlayer < 0)
			{
				followedPlayer = list.Count - 1;
			}
			else if (list.Count <= followedPlayer)
			{
				followedPlayer = 0;
			}
			this.currentlyFollowedPlayer = followedPlayer;
			if (list.Count > this.currentlyFollowedPlayer)
			{
				PlayerHealth playerHealth = list[this.currentlyFollowedPlayer];
				if (playerHealth != null)
				{
					this.cameraSmoothFollow.SetTarget(playerHealth.cameraPoint, playerHealth.cameraPositionPoint);
				}
			}
			this.UpdateFollowedPlayerUI();
		}
	}

	// Token: 0x0600069C RID: 1692 RVA: 0x00021B35 File Offset: 0x0001FD35
	private void UpdateFollowedPlayerUI()
	{
		if (this.replayToolsManager != null)
		{
			this.replayToolsManager.followPlayerDropdown.SetValueWithoutNotify(this.currentlyFollowedPlayer);
			this.replayToolsManager.followPlayerDropdown.RefreshShownValue();
		}
	}

	// Token: 0x0600069D RID: 1693 RVA: 0x00021B6B File Offset: 0x0001FD6B
	public void DisableInputManager()
	{
		if (this.userControls != null)
		{
			this.userControlsEnabled = false;
			this.userControls.Disable();
		}
	}

	// Token: 0x0600069E RID: 1694 RVA: 0x00021B87 File Offset: 0x0001FD87
	public void EnableInputManager()
	{
		if (this.userControls != null)
		{
			this.userControlsEnabled = true;
			this.userControls.Enable();
		}
	}

	// Token: 0x04000474 RID: 1140
	public CameraSmoothFollow cameraSmoothFollow;

	// Token: 0x04000475 RID: 1141
	public CameraFollowPlayers cameraFollowAllPlayers;

	// Token: 0x04000476 RID: 1142
	public ReplayFreeCamera cameraFreeControls;

	// Token: 0x04000477 RID: 1143
	public ReplayToolsManager replayToolsManager;

	// Token: 0x04000478 RID: 1144
	public CameraMode cameraMode = CameraMode.Follow;

	// Token: 0x04000479 RID: 1145
	public static ReplayCameraControls singleton;

	// Token: 0x0400047A RID: 1146
	public UserControls userControls;

	// Token: 0x0400047B RID: 1147
	private int currentlyFollowedPlayer;

	// Token: 0x0400047C RID: 1148
	public bool userControlsEnabled = true;
}
