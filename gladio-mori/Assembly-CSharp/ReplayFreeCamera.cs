using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

// Token: 0x020000C0 RID: 192
public class ReplayFreeCamera : MonoBehaviour, IDisableableInputManager
{
	// Token: 0x060006A5 RID: 1701 RVA: 0x00021BF4 File Offset: 0x0001FDF4
	private void Start()
	{
		this.SetupUserControls(true);
		this.SetCameraSettings();
	}

	// Token: 0x060006A6 RID: 1702 RVA: 0x00021C03 File Offset: 0x0001FE03
	public void SetCameraSettings()
	{
		this.cameraTurnSpeedMultiplier = SettingsHelper.GetControllerSensitivity();
	}

	// Token: 0x060006A7 RID: 1703 RVA: 0x00021C10 File Offset: 0x0001FE10
	public void SetupUserControls(bool withDelay = false)
	{
		if (this.userControls != null)
		{
			this.userControls.Dispose();
		}
		this.userControls = SettingsHelper.GetUserControls();
		this.userControls.Disable();
		this.userControls.PlayerActionMap.Move_Forward.performed += this.Camera_Move_Forward_performed;
		this.userControls.PlayerActionMap.Move_Forward.canceled += this.Camera_Move_Forward_performed;
		this.userControls.PlayerActionMap.Move_Back.performed += this.Camera_Move_Back_performed;
		this.userControls.PlayerActionMap.Move_Back.canceled += this.Camera_Move_Back_performed;
		this.userControls.ReplayMap.Move_Up.performed += this.Camera_Move_Up_performed;
		this.userControls.ReplayMap.Move_Up.canceled += this.Camera_Move_Up_performed;
		this.userControls.ReplayMap.Move_Down.performed += this.Camera_Move_Down_performed;
		this.userControls.ReplayMap.Move_Down.canceled += this.Camera_Move_Down_performed;
		this.userControls.PlayerActionMap.Move_Right.performed += this.Camera_Move_Right_performed;
		this.userControls.PlayerActionMap.Move_Right.canceled += this.Camera_Move_Right_performed;
		this.userControls.PlayerActionMap.Move_Left.performed += this.Camera_Move_Left_performed;
		this.userControls.PlayerActionMap.Move_Left.canceled += this.Camera_Move_Left_performed;
		this.userControls.PlayerActionMap.Turn_Right.performed += this.Camera_Turn_Right_performed;
		this.userControls.PlayerActionMap.Turn_Right.canceled += this.Camera_Turn_Right_performed;
		this.userControls.PlayerActionMap.Turn_Left.performed += this.Camera_Turn_Left_performed;
		this.userControls.PlayerActionMap.Turn_Left.canceled += this.Camera_Turn_Left_performed;
		this.userControls.PlayerActionMap.Turn_Up.performed += this.Camera_Turn_Up_performed;
		this.userControls.PlayerActionMap.Turn_Up.canceled += this.Camera_Turn_Up_performed;
		this.userControls.PlayerActionMap.Turn_Down.performed += this.Camera_Turn_Down_performed;
		this.userControls.PlayerActionMap.Turn_Down.canceled += this.Camera_Turn_Down_performed;
		this.userControls.PlayerActionMap.Turn_Mouse_Vertical.performed += this.Camera_Turn_Vertical_performed;
		this.userControls.PlayerActionMap.Turn_Mouse_Vertical.canceled += this.Camera_Turn_Vertical_performed;
		this.userControls.PlayerActionMap.Turn_Mouse_Horizontal.performed += this.Camera_Turn_Horizontal_performed;
		this.userControls.PlayerActionMap.Turn_Mouse_Horizontal.canceled += this.Camera_Turn_Horizontal_performed;
		this.mouseSensitivity = SettingsHelper.GetMouseSensitivity();
		if (GeneralManager.InputSystemDisabled())
		{
			this.DisableInputManager();
		}
		if (this.userControlsEnabled)
		{
			if (withDelay)
			{
				this.StartEnableWithDelay();
				return;
			}
			this.userControls.Enable();
		}
	}

	// Token: 0x060006A8 RID: 1704 RVA: 0x00021FDB File Offset: 0x000201DB
	private void OnDestroy()
	{
		this.DisposeUserControls();
	}

	// Token: 0x060006A9 RID: 1705 RVA: 0x00021FE3 File Offset: 0x000201E3
	public void DisposeUserControls()
	{
		if (this.userControls != null)
		{
			this.userControls.Disable();
			this.userControls.Dispose();
		}
	}

	// Token: 0x060006AA RID: 1706 RVA: 0x00022004 File Offset: 0x00020204
	private void Update()
	{
		if (!ReplayManager.ToolsVisible && this.FreeCameraActive)
		{
			if (!Mathf.Approximately(0f, this.forwardSpeed))
			{
				base.gameObject.transform.Translate(Vector3.forward * this.cameraMoveSpeedMultiplier * this.forwardSpeed * Time.unscaledDeltaTime);
			}
			if (!Mathf.Approximately(0f, this.horizontalSpeed))
			{
				base.gameObject.transform.Translate(Vector3.right * this.cameraMoveSpeedMultiplier * this.horizontalSpeed * Time.unscaledDeltaTime);
			}
			if (!Mathf.Approximately(0f, this.verticalSpeed))
			{
				base.gameObject.transform.Translate(Vector3.up * this.cameraMoveSpeedMultiplier * this.verticalSpeed * Time.unscaledDeltaTime);
			}
			if (!Mathf.Approximately(0f, this.horizontalTurnSpeed))
			{
				base.gameObject.transform.Rotate(0f, this.horizontalTurnSpeed * this.cameraTurnSpeedMultiplier * Time.unscaledDeltaTime, 0f, Space.World);
			}
			if (!Mathf.Approximately(0f, this.verticalTurnSpeed))
			{
				base.gameObject.transform.Rotate(this.verticalTurnSpeed * this.cameraTurnSpeedMultiplier * Time.unscaledDeltaTime, 0f, 0f, Space.Self);
			}
		}
		this.AttemptToEnableWithDelay();
	}

	// Token: 0x060006AB RID: 1707 RVA: 0x00022181 File Offset: 0x00020381
	public void TurnCameraHorizontal(float turnValue)
	{
		if (this.FreeCameraActive)
		{
			base.gameObject.transform.Rotate(0f, turnValue, 0f, Space.World);
		}
	}

	// Token: 0x060006AC RID: 1708 RVA: 0x000221A7 File Offset: 0x000203A7
	public void TurnCameraVertical(float turnValue)
	{
		if (this.FreeCameraActive)
		{
			base.gameObject.transform.Rotate(turnValue, 0f, 0f, Space.Self);
		}
	}

	// Token: 0x17000104 RID: 260
	// (get) Token: 0x060006AD RID: 1709 RVA: 0x000221CD File Offset: 0x000203CD
	private bool FreeCameraActive
	{
		get
		{
			return !ReplayManager.ToolsVisible && ReplayCameraControls.CurrentCameraMode() == CameraMode.Free && GameMenu.GameMenuCurrentlyHidden;
		}
	}

	// Token: 0x060006AE RID: 1710 RVA: 0x000221E4 File Offset: 0x000203E4
	private void Camera_Move_Forward_performed(InputAction.CallbackContext obj)
	{
		float num = 0f;
		if (!obj.canceled)
		{
			num = obj.ReadValue<float>();
		}
		this.forwardSpeed = num;
	}

	// Token: 0x060006AF RID: 1711 RVA: 0x00022210 File Offset: 0x00020410
	private void Camera_Move_Back_performed(InputAction.CallbackContext obj)
	{
		float num = 0f;
		if (!obj.canceled)
		{
			num = obj.ReadValue<float>();
		}
		this.forwardSpeed = num * -1f;
	}

	// Token: 0x060006B0 RID: 1712 RVA: 0x00022244 File Offset: 0x00020444
	private void Camera_Move_Right_performed(InputAction.CallbackContext obj)
	{
		float num = 0f;
		if (!obj.canceled)
		{
			num = obj.ReadValue<float>();
		}
		this.horizontalSpeed = num;
	}

	// Token: 0x060006B1 RID: 1713 RVA: 0x00022270 File Offset: 0x00020470
	private void Camera_Move_Left_performed(InputAction.CallbackContext obj)
	{
		float num = 0f;
		if (!obj.canceled)
		{
			num = obj.ReadValue<float>();
		}
		this.horizontalSpeed = num * -1f;
	}

	// Token: 0x060006B2 RID: 1714 RVA: 0x000222A4 File Offset: 0x000204A4
	private void Camera_Move_Up_performed(InputAction.CallbackContext obj)
	{
		float num = 0f;
		if (!obj.canceled)
		{
			num = obj.ReadValue<float>();
		}
		this.verticalSpeed = num;
	}

	// Token: 0x060006B3 RID: 1715 RVA: 0x000222D0 File Offset: 0x000204D0
	private void Camera_Move_Down_performed(InputAction.CallbackContext obj)
	{
		float num = 0f;
		if (!obj.canceled)
		{
			num = obj.ReadValue<float>();
		}
		this.verticalSpeed = num * -1f;
	}

	// Token: 0x060006B4 RID: 1716 RVA: 0x00022304 File Offset: 0x00020504
	private void Camera_Turn_Horizontal_performed(InputAction.CallbackContext obj)
	{
		float num = 0f;
		if (!obj.canceled)
		{
			num = obj.ReadValue<float>();
		}
		if (obj.control.device.name.Contains("Mouse"))
		{
			this.TurnCameraHorizontal(num * this.mouseSensitivity);
			return;
		}
		this.horizontalTurnSpeed = num;
	}

	// Token: 0x060006B5 RID: 1717 RVA: 0x0002235C File Offset: 0x0002055C
	private void Camera_Turn_Vertical_performed(InputAction.CallbackContext obj)
	{
		float num = 0f;
		if (!obj.canceled)
		{
			num = obj.ReadValue<float>();
		}
		if (obj.control.device.name.Contains("Mouse"))
		{
			this.TurnCameraVertical(num * -1f * this.mouseSensitivity);
			return;
		}
		this.verticalTurnSpeed = num * -1f;
	}

	// Token: 0x060006B6 RID: 1718 RVA: 0x000223C0 File Offset: 0x000205C0
	private void Camera_Turn_Left_performed(InputAction.CallbackContext obj)
	{
		float num = 0f;
		if (!obj.canceled)
		{
			num = obj.ReadValue<float>();
		}
		this.horizontalTurnSpeed = num * -1f;
	}

	// Token: 0x060006B7 RID: 1719 RVA: 0x000223F4 File Offset: 0x000205F4
	private void Camera_Turn_Right_performed(InputAction.CallbackContext obj)
	{
		float num = 0f;
		if (!obj.canceled)
		{
			num = obj.ReadValue<float>();
		}
		this.horizontalTurnSpeed = num;
	}

	// Token: 0x060006B8 RID: 1720 RVA: 0x00022420 File Offset: 0x00020620
	private void Camera_Turn_Up_performed(InputAction.CallbackContext obj)
	{
		float num = 0f;
		if (!obj.canceled)
		{
			num = obj.ReadValue<float>();
		}
		this.verticalTurnSpeed = num * -1f;
	}

	// Token: 0x060006B9 RID: 1721 RVA: 0x00022454 File Offset: 0x00020654
	private void Camera_Turn_Down_performed(InputAction.CallbackContext obj)
	{
		float num = 0f;
		if (!obj.canceled)
		{
			num = obj.ReadValue<float>();
		}
		this.verticalTurnSpeed = num;
	}

	// Token: 0x060006BA RID: 1722 RVA: 0x0002247F File Offset: 0x0002067F
	public void DisableInputManager()
	{
		if (this.userControls != null)
		{
			this.userControlsEnabled = false;
			this.userControls.Disable();
		}
	}

	// Token: 0x060006BB RID: 1723 RVA: 0x0002249B File Offset: 0x0002069B
	public void EnableInputManager()
	{
		if (this.userControls != null)
		{
			this.userControlsEnabled = true;
			this.userControls.Enable();
		}
	}

	// Token: 0x060006BC RID: 1724 RVA: 0x000224B7 File Offset: 0x000206B7
	private void StartEnableWithDelay()
	{
		this.enableWithDelay = true;
		this.enableWithDelayTime = Time.unscaledTime;
	}

	// Token: 0x060006BD RID: 1725 RVA: 0x000224CC File Offset: 0x000206CC
	private void AttemptToEnableWithDelay()
	{
		if (this.enableWithDelay)
		{
			if (Time.unscaledTime > this.maxWait + this.enableWithDelayTime)
			{
				this.EnableWithDelay();
				return;
			}
			if (Time.unscaledTime < this.minWait + this.enableWithDelayTime)
			{
				return;
			}
			if (GeneralManager.singleton != null && !GeneralManager.singleton.AnyPlayerActionMapInputActive())
			{
				this.EnableWithDelay();
			}
		}
	}

	// Token: 0x060006BE RID: 1726 RVA: 0x00022530 File Offset: 0x00020730
	private void EnableWithDelay()
	{
		this.EnableInputManager();
		this.enableWithDelay = false;
	}

	// Token: 0x04000482 RID: 1154
	public UserControls userControls;

	// Token: 0x04000483 RID: 1155
	private float forwardSpeed;

	// Token: 0x04000484 RID: 1156
	private float horizontalSpeed;

	// Token: 0x04000485 RID: 1157
	private float verticalSpeed;

	// Token: 0x04000486 RID: 1158
	private float horizontalTurnSpeed;

	// Token: 0x04000487 RID: 1159
	private float verticalTurnSpeed;

	// Token: 0x04000488 RID: 1160
	private float cameraMoveSpeedMultiplier = 5f;

	// Token: 0x04000489 RID: 1161
	private float cameraTurnSpeedMultiplier = 150f;

	// Token: 0x0400048A RID: 1162
	public float mouseSensitivity = 0.5f;

	// Token: 0x0400048B RID: 1163
	public bool userControlsEnabled = true;

	// Token: 0x0400048C RID: 1164
	private bool enableWithDelay;

	// Token: 0x0400048D RID: 1165
	private float enableWithDelayTime;

	// Token: 0x0400048E RID: 1166
	private float minWait = 0.2f;

	// Token: 0x0400048F RID: 1167
	private float maxWait = 2.5f;
}
