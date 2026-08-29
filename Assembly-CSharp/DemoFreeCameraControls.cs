using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

// Token: 0x020000BD RID: 189
public class DemoFreeCameraControls : MonoBehaviour
{
	// Token: 0x0600067E RID: 1662 RVA: 0x00020E89 File Offset: 0x0001F089
	private void Start()
	{
		this.SetupUserControls();
		this.cameraSmoothFollow = base.gameObject.GetComponent<CameraSmoothFollow>();
		this.FindInputManager();
	}

	// Token: 0x0600067F RID: 1663 RVA: 0x00020EA8 File Offset: 0x0001F0A8
	private void FindInputManager()
	{
		if (this.cameraSmoothFollow != null)
		{
			this.playerHealth = this.cameraSmoothFollow.positionTarget.GetComponentInParent<PlayerHealth>();
			if (this.playerHealth != null && this.playerHealth.multiplayerInputManager != null)
			{
				this.disableableInputManager = (IDisableableInputManager)this.playerHealth.multiplayerInputManager;
				return;
			}
			foreach (global::PlayerInputManager playerInputManager in UnityEngine.Object.FindObjectsOfType<global::PlayerInputManager>())
			{
				if (playerInputManager.playerAnimator != null && playerInputManager.playerAnimator.player == this.playerHealth)
				{
					this.disableableInputManager = playerInputManager;
				}
			}
		}
	}

	// Token: 0x06000680 RID: 1664 RVA: 0x00020F54 File Offset: 0x0001F154
	private void SetupUserControls()
	{
		if (this.userControls != null)
		{
			this.userControls.Dispose();
		}
		this.userControls = SettingsHelper.GetUserControls();
		this.userControls.PlayerActionMap.Move_Forward.performed += this.Camera_Move_Forward_performed;
		this.userControls.PlayerActionMap.Move_Forward.canceled += this.Camera_Move_Forward_performed;
		this.userControls.PlayerActionMap.Move_Back.performed += this.Camera_Move_Back_performed;
		this.userControls.PlayerActionMap.Move_Back.canceled += this.Camera_Move_Back_performed;
		this.userControls.PlayerActionMap.Move_Right.performed += this.Camera_Move_Right_performed;
		this.userControls.PlayerActionMap.Move_Right.canceled += this.Camera_Move_Right_performed;
		this.userControls.PlayerActionMap.Move_Left.performed += this.Camera_Move_Left_performed;
		this.userControls.PlayerActionMap.Move_Left.canceled += this.Camera_Move_Left_performed;
		this.userControls.PlayerActionMap.Turn_Mouse_Vertical.performed += this.Camera_Turn_Vertical_performed;
		this.userControls.PlayerActionMap.Turn_Mouse_Vertical.canceled += this.Camera_Turn_Vertical_performed;
		this.userControls.PlayerActionMap.Turn_Mouse_Horizontal.performed += this.Camera_Turn_Horizontal_performed;
		this.userControls.PlayerActionMap.Turn_Mouse_Horizontal.canceled += this.Camera_Turn_Horizontal_performed;
		this.mouseSensitivity = SettingsHelper.GetMouseSensitivity();
		this.userControls.Enable();
	}

	// Token: 0x06000681 RID: 1665 RVA: 0x00021145 File Offset: 0x0001F345
	private void OnDestroy()
	{
		this.DisposeUserControls();
	}

	// Token: 0x06000682 RID: 1666 RVA: 0x0002114D File Offset: 0x0001F34D
	public void DisposeUserControls()
	{
		if (this.userControls != null)
		{
			this.userControls.Disable();
			this.userControls.Dispose();
		}
	}

	// Token: 0x06000683 RID: 1667 RVA: 0x00021170 File Offset: 0x0001F370
	private void Update()
	{
		if (this.playerHealth == null || !this.playerHealth.alive)
		{
			return;
		}
		if (Keyboard.current.f1Key.wasPressedThisFrame)
		{
			this.EnableFreeCameraSystem();
		}
		if (Keyboard.current.f2Key.wasPressedThisFrame)
		{
			this.EnableFreeCameraMovement();
		}
		if (this.freeCamControlsActive && !GameMenu.GameMenuCurrentlyHidden)
		{
			this.freeCamControlsActive = false;
		}
		if (this.FreeCameraActive)
		{
			if (!Mathf.Approximately(0f, this.forwardSpeed))
			{
				base.gameObject.transform.Translate(Vector3.forward * this.cameraMoveSpeedMultiplier * this.forwardSpeed * Time.unscaledDeltaTime);
			}
			if (!Mathf.Approximately(0f, this.horizontalSpeed))
			{
				base.gameObject.transform.Translate(Vector3.right * this.cameraMoveSpeedMultiplier * this.horizontalSpeed * Time.unscaledDeltaTime);
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
	}

	// Token: 0x06000684 RID: 1668 RVA: 0x000212F8 File Offset: 0x0001F4F8
	private void EnableFreeCameraSystem()
	{
		if (this.cameraSmoothFollow != null)
		{
			this.cameraSmoothFollow.enabled = !this.cameraSmoothFollow.enabled;
			this.freeCamControlsActive = !this.cameraSmoothFollow.enabled;
			this.CheckPlayerInputs();
		}
	}

	// Token: 0x06000685 RID: 1669 RVA: 0x00021346 File Offset: 0x0001F546
	private void EnableFreeCameraMovement()
	{
		this.freeCamControlsActive = !this.freeCamControlsActive;
		this.CheckPlayerInputs();
	}

	// Token: 0x06000686 RID: 1670 RVA: 0x0002135D File Offset: 0x0001F55D
	private void CheckPlayerInputs()
	{
		if (this.disableableInputManager != null)
		{
			if (this.freeCamControlsActive)
			{
				this.disableableInputManager.DisableInputManager();
				return;
			}
			this.disableableInputManager.EnableInputManager();
		}
	}

	// Token: 0x06000687 RID: 1671 RVA: 0x00021386 File Offset: 0x0001F586
	public void TurnCameraHorizontal(float turnValue)
	{
		if (this.FreeCameraActive)
		{
			base.gameObject.transform.Rotate(0f, turnValue, 0f, Space.World);
		}
	}

	// Token: 0x06000688 RID: 1672 RVA: 0x000213AC File Offset: 0x0001F5AC
	public void TurnCameraVertical(float turnValue)
	{
		if (this.FreeCameraActive)
		{
			base.gameObject.transform.Rotate(turnValue, 0f, 0f, Space.Self);
		}
	}

	// Token: 0x17000103 RID: 259
	// (get) Token: 0x06000689 RID: 1673 RVA: 0x000213D2 File Offset: 0x0001F5D2
	private bool FreeCameraActive
	{
		get
		{
			return this.freeCamControlsActive && GameMenu.GameMenuCurrentlyHidden;
		}
	}

	// Token: 0x0600068A RID: 1674 RVA: 0x000213E4 File Offset: 0x0001F5E4
	private void Camera_Move_Forward_performed(InputAction.CallbackContext obj)
	{
		float num = 0f;
		if (!obj.canceled)
		{
			num = obj.ReadValue<float>();
		}
		this.forwardSpeed = num;
	}

	// Token: 0x0600068B RID: 1675 RVA: 0x00021410 File Offset: 0x0001F610
	private void Camera_Move_Back_performed(InputAction.CallbackContext obj)
	{
		float num = 0f;
		if (!obj.canceled)
		{
			num = obj.ReadValue<float>();
		}
		this.forwardSpeed = num * -1f;
	}

	// Token: 0x0600068C RID: 1676 RVA: 0x00021444 File Offset: 0x0001F644
	private void Camera_Move_Right_performed(InputAction.CallbackContext obj)
	{
		float num = 0f;
		if (!obj.canceled)
		{
			num = obj.ReadValue<float>();
		}
		this.horizontalSpeed = num;
	}

	// Token: 0x0600068D RID: 1677 RVA: 0x00021470 File Offset: 0x0001F670
	private void Camera_Move_Left_performed(InputAction.CallbackContext obj)
	{
		float num = 0f;
		if (!obj.canceled)
		{
			num = obj.ReadValue<float>();
		}
		this.horizontalSpeed = num * -1f;
	}

	// Token: 0x0600068E RID: 1678 RVA: 0x000214A4 File Offset: 0x0001F6A4
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

	// Token: 0x0600068F RID: 1679 RVA: 0x000214FC File Offset: 0x0001F6FC
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

	// Token: 0x04000468 RID: 1128
	public UserControls userControls;

	// Token: 0x04000469 RID: 1129
	public bool freeCamControlsActive;

	// Token: 0x0400046A RID: 1130
	public CameraSmoothFollow cameraSmoothFollow;

	// Token: 0x0400046B RID: 1131
	public IDisableableInputManager disableableInputManager;

	// Token: 0x0400046C RID: 1132
	private PlayerHealth playerHealth;

	// Token: 0x0400046D RID: 1133
	private float forwardSpeed;

	// Token: 0x0400046E RID: 1134
	private float horizontalSpeed;

	// Token: 0x0400046F RID: 1135
	private float horizontalTurnSpeed;

	// Token: 0x04000470 RID: 1136
	private float verticalTurnSpeed;

	// Token: 0x04000471 RID: 1137
	private float cameraMoveSpeedMultiplier = 5f;

	// Token: 0x04000472 RID: 1138
	private float cameraTurnSpeedMultiplier = 150f;

	// Token: 0x04000473 RID: 1139
	public float mouseSensitivity = 0.5f;
}
