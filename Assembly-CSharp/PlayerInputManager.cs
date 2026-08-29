using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mirror;
using MoveClasses;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using Utils;

// Token: 0x020000AE RID: 174
public class PlayerInputManager : MonoBehaviour, IDisableableInputManager, IPlayerInputManager
{
	// Token: 0x170000FD RID: 253
	// (get) Token: 0x060005D6 RID: 1494 RVA: 0x0001C638 File Offset: 0x0001A838
	// (set) Token: 0x060005D7 RID: 1495 RVA: 0x0001C640 File Offset: 0x0001A840
	public RotatePlayer rotatePlayer { get; set; }

	// Token: 0x060005D8 RID: 1496 RVA: 0x0001C64C File Offset: 0x0001A84C
	private void Awake()
	{
		if (IGameSettingsManager.singleton != null)
		{
			this.rollingFeet = IGameSettingsManager.singleton.GetRollingFeet();
		}
		this.LoadUserControl();
		if (this.rollingFeet)
		{
			this.BindRollMovement();
		}
		this.lastMouseDirectionY = new ExponentialMovingAverage(this.maxMouseDirectionHistory);
		this.lastMouseDirectionX = new ExponentialMovingAverage(this.maxMouseDirectionHistory);
		this.LoadSettings();
		this.userControls.Enable();
	}

	// Token: 0x060005D9 RID: 1497 RVA: 0x0001C6B7 File Offset: 0x0001A8B7
	public void LoadSettings()
	{
		this.mouseSensitivity = SettingsHelper.GetMouseSensitivity();
		this.playerTurnType = SettingsHelper.GetPlayerTurnType();
		if (this.rotatePlayer != null)
		{
			this.rotatePlayer.SetUseTargetRotation(this.playerTurnType == PlayerTurnType.TurnCamera);
		}
	}

	// Token: 0x060005DA RID: 1498 RVA: 0x0001C6F1 File Offset: 0x0001A8F1
	public void HandlePlayerDeath()
	{
		this.forceDisableControls = true;
		if (this.hudCanvas != null)
		{
			this.hudCanvas.SetForceDisableAttackDirection(true);
		}
		this.DisableInputManager();
	}

	// Token: 0x060005DB RID: 1499 RVA: 0x0001C71A File Offset: 0x0001A91A
	private void Update()
	{
		this.UpdateMouseDirectionHistory();
	}

	// Token: 0x060005DC RID: 1500 RVA: 0x0001C724 File Offset: 0x0001A924
	public void LoadUserControl()
	{
		if (this.userControls != null)
		{
			this.userControls.Disable();
			this.userControls.Dispose();
		}
		this.userControls = SettingsHelper.GetUserControls();
		foreach (PropertyInfo propertyInfo in this.userControls.PlayerActionMap.GetType().GetProperties())
		{
			if (propertyInfo.PropertyType == typeof(InputAction))
			{
				InputAction inputAction = (InputAction)propertyInfo.GetValue(this.userControls.PlayerActionMap);
				if ((!this.rollingFeet || !inputAction.name.Contains("Move_")) && !inputAction.name.Contains("Directional_") && !inputAction.name.Contains("Turn"))
				{
					if (inputAction.type == InputActionType.Value)
					{
						inputAction.ApplyBindingOverride(new InputBinding
						{
							overrideInteractions = "Press"
						});
						inputAction.performed += this.Action_performed;
					}
					else
					{
						inputAction.started += this.Action_performed;
					}
					inputAction.canceled += this.Action_performed;
				}
			}
		}
		this.BindTurnMovement();
		this.BindMouseTurn();
		this.BindDirectionalActions();
		InputUser inputUser = this.inputUser;
		if (this.inputUser.valid)
		{
			this.ConnectToUser(this.inputUser);
			return;
		}
		if (this.userControlsEnabled)
		{
			this.userControls.Enable();
		}
	}

	// Token: 0x060005DD RID: 1501 RVA: 0x0001C8A8 File Offset: 0x0001AAA8
	public void BindTurnMovement()
	{
		if (this.userControls != null)
		{
			this.userControls.PlayerActionMap.Turn_Left.performed += this.Turn_Left_performed;
			this.userControls.PlayerActionMap.Turn_Left.canceled += this.Turn_Left_performed;
			this.userControls.PlayerActionMap.Turn_Right.performed += this.Turn_Right_performed;
			this.userControls.PlayerActionMap.Turn_Right.canceled += this.Turn_Right_performed;
			this.userControls.PlayerActionMap.Turn_Up.performed += this.Turn_Up_performed;
			this.userControls.PlayerActionMap.Turn_Up.canceled += this.Turn_Up_performed;
			this.userControls.PlayerActionMap.Turn_Down.performed += this.Turn_Down_performed;
			this.userControls.PlayerActionMap.Turn_Down.canceled += this.Turn_Down_performed;
		}
	}

	// Token: 0x060005DE RID: 1502 RVA: 0x0001C9E0 File Offset: 0x0001ABE0
	public void BindMouseTurn()
	{
		if (this.userControls != null && !SettingsHelper.GetDisableMouseTurning())
		{
			this.userControls.PlayerActionMap.Turn_Mouse_Vertical.performed += this.Camera_Turn_Vertical_performed;
			this.userControls.PlayerActionMap.Turn_Mouse_Vertical.canceled += this.Camera_Turn_Vertical_performed;
			this.userControls.PlayerActionMap.Turn_Mouse_Horizontal.performed += this.Camera_Turn_Horizontal_performed;
			this.userControls.PlayerActionMap.Turn_Mouse_Horizontal.canceled += this.Camera_Turn_Horizontal_performed;
		}
	}

	// Token: 0x060005DF RID: 1503 RVA: 0x0001CA94 File Offset: 0x0001AC94
	public void BindDirectionalActions()
	{
		if (this.userControls != null)
		{
			this.userControls.PlayerActionMap.Directional_Action1.performed += this.Directional_Action_performed;
			this.userControls.PlayerActionMap.Directional_Action1.canceled += this.Directional_Action_performed;
			this.userControls.PlayerActionMap.Directional_Action2.performed += this.Directional_Action_performed;
			this.userControls.PlayerActionMap.Directional_Action2.canceled += this.Directional_Action_performed;
		}
	}

	// Token: 0x060005E0 RID: 1504 RVA: 0x0001CB3C File Offset: 0x0001AD3C
	public void BindRollMovement()
	{
		if (this.userControls != null)
		{
			this.userControls.PlayerActionMap.Move_Forward.performed += this.Move_Forward_performed;
			this.userControls.PlayerActionMap.Move_Forward.canceled += this.Move_Forward_performed;
			this.userControls.PlayerActionMap.Move_Back.performed += this.Move_Back_performed;
			this.userControls.PlayerActionMap.Move_Back.canceled += this.Move_Back_performed;
			this.userControls.PlayerActionMap.Move_Left.performed += this.Move_Left_performed;
			this.userControls.PlayerActionMap.Move_Left.canceled += this.Move_Left_performed;
			this.userControls.PlayerActionMap.Move_Right.performed += this.Move_Right_performed;
			this.userControls.PlayerActionMap.Move_Right.canceled += this.Move_Right_performed;
		}
	}

	// Token: 0x060005E1 RID: 1505 RVA: 0x0001CC74 File Offset: 0x0001AE74
	public void ConnectToUser(InputUser newUser)
	{
		this.inputUser = newUser;
		this.inputUser.AssociateActionsWithUser(this.userControls);
		if (this.userControlsEnabled)
		{
			this.userControls.Enable();
		}
	}

	// Token: 0x060005E2 RID: 1506 RVA: 0x0001CCA4 File Offset: 0x0001AEA4
	public void ConnectToPlayerCharacter(GameObject newPlayerCharacter)
	{
		this.playerCharacter = newPlayerCharacter;
		this.ballMovements = Generic.FindComponentsInChildObjects<BallMovement>(this.playerCharacter);
		this.rotatePlayer = Generic.FindComponentsInChildObjects<RotatePlayer>(this.playerCharacter).FirstOrDefault<RotatePlayer>();
		this.playerAnimator = Generic.FindComponentsInChildObjects<PlayerAnimator>(this.playerCharacter).FirstOrDefault<PlayerAnimator>();
		this.playerAnimator.player.playerInputManager = this;
		this.cameraSmoothFollowControllable = this.playerAnimator.player.cameraSmoothFollow;
		if (this.cameraSmoothFollowControllable != null)
		{
			this.cameraSmoothFollowControllable.SetStartRotationOffset();
			this.cameraSmoothFollowControllable.playerInputManager = this;
			this.rotatePlayer.useTargetRotation = true;
			this.UpdateTargetRotation();
			this.hudCanvas = UnityEngine.Object.Instantiate<GameObject>(this.hudCanvasPrefab).GetComponent<HudCanvas>();
			this.hudCanvas.playerInputManager = this;
			this.hudCanvas.SetupCamera(this.cameraSmoothFollowControllable.objectCamera);
			if (this.staminaHudCanvas == null)
			{
				this.staminaHudCanvas = UnityEngine.Object.Instantiate<GameObject>(this.staminaHudCanvasPrefab).GetComponent<StaminaHudCanvas>();
				this.staminaHudCanvas.playerInputManager = this;
			}
			this.staminaHudCanvas.playerHealth = this.playerAnimator.player;
			this.staminaHudCanvas.SetupCamera(this.cameraSmoothFollowControllable.objectCamera);
		}
	}

	// Token: 0x060005E3 RID: 1507 RVA: 0x0001CDEA File Offset: 0x0001AFEA
	public void ConnectToMoveSetEditor(MoveSetEditor newMoveSetEditor)
	{
		this.moveSetEditor = newMoveSetEditor;
	}

	// Token: 0x060005E4 RID: 1508 RVA: 0x0001CDF4 File Offset: 0x0001AFF4
	private void Move_Forward_performed(InputAction.CallbackContext obj)
	{
		float verticalSpeed = 0f;
		if (!obj.canceled)
		{
			verticalSpeed = obj.ReadValue<float>();
		}
		if (this.ballMovements != null)
		{
			foreach (BallMovement ballMovement in this.ballMovements)
			{
				ballMovement.SetVerticalSpeed(verticalSpeed);
			}
		}
	}

	// Token: 0x060005E5 RID: 1509 RVA: 0x0001CE64 File Offset: 0x0001B064
	private void Move_Back_performed(InputAction.CallbackContext obj)
	{
		float num = 0f;
		if (!obj.canceled)
		{
			num = obj.ReadValue<float>();
		}
		if (this.ballMovements != null)
		{
			foreach (BallMovement ballMovement in this.ballMovements)
			{
				ballMovement.SetVerticalSpeed(num * -1f);
			}
		}
	}

	// Token: 0x060005E6 RID: 1510 RVA: 0x0001CEDC File Offset: 0x0001B0DC
	private void Move_Left_performed(InputAction.CallbackContext obj)
	{
		float num = 0f;
		if (!obj.canceled)
		{
			num = obj.ReadValue<float>();
		}
		if (this.ballMovements != null)
		{
			foreach (BallMovement ballMovement in this.ballMovements)
			{
				ballMovement.SetHorizontalSpeed(num * -1f);
			}
		}
	}

	// Token: 0x060005E7 RID: 1511 RVA: 0x0001CF54 File Offset: 0x0001B154
	private void Move_Right_performed(InputAction.CallbackContext obj)
	{
		float horizontalSpeed = 0f;
		if (!obj.canceled)
		{
			horizontalSpeed = obj.ReadValue<float>();
		}
		if (this.ballMovements != null)
		{
			foreach (BallMovement ballMovement in this.ballMovements)
			{
				ballMovement.SetHorizontalSpeed(horizontalSpeed);
			}
		}
	}

	// Token: 0x060005E8 RID: 1512 RVA: 0x0001CFC4 File Offset: 0x0001B1C4
	private void Turn_Left_performed(InputAction.CallbackContext obj)
	{
		float num = 0f;
		if (!obj.canceled)
		{
			num = obj.ReadValue<float>();
		}
		this.UpdateInputType(false);
		if (this.rotatePlayer != null)
		{
			if (this.rotatePlayer.useTargetRotation)
			{
				this.cameraSmoothFollowControllable.SetRotationInputLeft(num * -1f);
				return;
			}
			this.rotatePlayer.SetRotationInputLeft(num * -1f);
		}
	}

	// Token: 0x060005E9 RID: 1513 RVA: 0x0001D030 File Offset: 0x0001B230
	private void Turn_Right_performed(InputAction.CallbackContext obj)
	{
		float rotationInputRight = 0f;
		if (!obj.canceled)
		{
			rotationInputRight = obj.ReadValue<float>();
		}
		this.UpdateInputType(false);
		if (this.rotatePlayer != null)
		{
			if (this.rotatePlayer.useTargetRotation)
			{
				this.cameraSmoothFollowControllable.SetRotationInputRight(rotationInputRight);
				return;
			}
			this.rotatePlayer.SetRotationInputRight(rotationInputRight);
		}
	}

	// Token: 0x060005EA RID: 1514 RVA: 0x0001D090 File Offset: 0x0001B290
	private void Turn_Up_performed(InputAction.CallbackContext obj)
	{
		float moveCameraVerticalTurn = 0f;
		if (!obj.canceled)
		{
			moveCameraVerticalTurn = obj.ReadValue<float>();
		}
		if (this.cameraSmoothFollowControllable != null)
		{
			this.cameraSmoothFollowControllable.SetMoveCameraVerticalTurn(moveCameraVerticalTurn);
		}
		this.UpdateInputType(false);
	}

	// Token: 0x060005EB RID: 1515 RVA: 0x0001D0D8 File Offset: 0x0001B2D8
	private void Turn_Down_performed(InputAction.CallbackContext obj)
	{
		float num = 0f;
		if (!obj.canceled)
		{
			num = obj.ReadValue<float>();
		}
		if (this.cameraSmoothFollowControllable != null)
		{
			this.cameraSmoothFollowControllable.SetMoveCameraVerticalTurn(num * -1f);
		}
		this.UpdateInputType(false);
	}

	// Token: 0x060005EC RID: 1516 RVA: 0x0001D124 File Offset: 0x0001B324
	private void Camera_Turn_Horizontal_performed(InputAction.CallbackContext obj)
	{
		float num = 0f;
		if (!obj.canceled)
		{
			num = obj.ReadValue<float>();
		}
		if (this.cameraSmoothFollowControllable != null && obj.control.device.name.Contains("Mouse"))
		{
			this.cameraSmoothFollowControllable.TurnCameraHorizontal(num * this.mouseSensitivity);
		}
		this.UpdateTargetRotation();
		this.UpdateInputType(true);
	}

	// Token: 0x060005ED RID: 1517 RVA: 0x0001D194 File Offset: 0x0001B394
	private void Camera_Turn_Vertical_performed(InputAction.CallbackContext obj)
	{
		float num = 0f;
		if (!obj.canceled)
		{
			num = obj.ReadValue<float>();
		}
		if (this.cameraSmoothFollowControllable != null && obj.control.device.name.Contains("Mouse"))
		{
			this.cameraSmoothFollowControllable.TurnCameraVertical(num * -1f * this.mouseSensitivity);
		}
		this.UpdateInputType(true);
	}

	// Token: 0x060005EE RID: 1518 RVA: 0x0001D204 File Offset: 0x0001B404
	private void Action_performed(InputAction.CallbackContext obj)
	{
		float value = 0f;
		if (!obj.canceled)
		{
			value = obj.ReadValue<float>();
		}
		if (this.playerAnimator != null)
		{
			this.playerAnimator.ActivatePlayerAction(new PlayerAction
			{
				name = obj.action.name,
				type = (obj.started ? ActionType.Start : (obj.performed ? ActionType.Start : ActionType.End)),
				value = value
			});
			return;
		}
		if (this.moveSetEditor != null)
		{
			this.moveSetEditor.ActivatePlayerAction(new PlayerAction
			{
				name = obj.action.name,
				type = (obj.started ? ActionType.Start : (obj.performed ? ActionType.Performed : ActionType.End)),
				value = value
			});
		}
	}

	// Token: 0x060005EF RID: 1519 RVA: 0x0001D2D8 File Offset: 0x0001B4D8
	private void Directional_Action_performed(InputAction.CallbackContext obj)
	{
		float value = 0f;
		if (!obj.canceled)
		{
			value = obj.ReadValue<float>();
		}
		int num = 0;
		if (obj.action.name.Contains("2"))
		{
			num = 1;
		}
		InputAction inputAction;
		if (obj.phase == InputActionPhase.Canceled)
		{
			inputAction = this.currentDirectionalAction[num];
		}
		else
		{
			if (this.currentDirectionalAction[num] != null)
			{
				return;
			}
			inputAction = this.GetInputActionForDirection(num);
		}
		if (inputAction == null)
		{
			return;
		}
		if (this.playerAnimator != null)
		{
			this.playerAnimator.ActivatePlayerAction(new PlayerAction
			{
				name = inputAction.name,
				type = (obj.started ? ActionType.Start : (obj.performed ? ActionType.Start : ActionType.End)),
				value = value
			});
		}
		else if (this.moveSetEditor != null)
		{
			this.moveSetEditor.ActivatePlayerAction(new PlayerAction
			{
				name = inputAction.name,
				type = (obj.started ? ActionType.Start : (obj.performed ? ActionType.Performed : ActionType.End)),
				value = value
			});
		}
		if (obj.phase == InputActionPhase.Canceled)
		{
			this.currentDirectionalAction[num] = null;
			return;
		}
		this.currentDirectionalAction[num] = inputAction;
	}

	// Token: 0x060005F0 RID: 1520 RVA: 0x0001D408 File Offset: 0x0001B608
	private InputAction GetInputActionForDirection(int inputNum)
	{
		InputAction result = this.userControls.PlayerActionMap.Action5;
		AttackDirection attackDirection = this.GetAttackDirection();
		if (attackDirection == AttackDirection.Up)
		{
			if (inputNum == 1)
			{
				result = this.userControls.PlayerActionMap.Action7;
			}
			else
			{
				result = this.userControls.PlayerActionMap.Action5;
			}
		}
		else if (attackDirection == AttackDirection.Down)
		{
			if (inputNum == 1)
			{
				result = this.userControls.PlayerActionMap.Action2;
			}
			else
			{
				result = this.userControls.PlayerActionMap.Action8;
			}
		}
		else if (attackDirection == AttackDirection.Right)
		{
			if (inputNum == 1)
			{
				result = this.userControls.PlayerActionMap.Action6;
			}
			else
			{
				result = this.userControls.PlayerActionMap.Action3;
			}
		}
		else if (inputNum == 1)
		{
			result = this.userControls.PlayerActionMap.Action4;
		}
		else
		{
			result = this.userControls.PlayerActionMap.Action1;
		}
		return result;
	}

	// Token: 0x170000FE RID: 254
	// (get) Token: 0x060005F1 RID: 1521 RVA: 0x0001D500 File Offset: 0x0001B700
	public float lastMouseActionTime
	{
		get
		{
			return this._lastMouseActionTime;
		}
	}

	// Token: 0x060005F2 RID: 1522 RVA: 0x0001D508 File Offset: 0x0001B708
	private void UpdateMouseDirectionHistory()
	{
		if (this.userControls.PlayerActionMap.Turn_Mouse_Vertical.WasPerformedThisFrame() || this.userControls.PlayerActionMap.Turn_Mouse_Horizontal.WasPerformedThisFrame())
		{
			this._lastMouseActionTime = Time.unscaledTime;
			this.lastMouseDirectionY.Add((double)this.userControls.PlayerActionMap.Turn_Mouse_Vertical.ReadValue<float>());
			this.lastMouseDirectionX.Add((double)this.userControls.PlayerActionMap.Turn_Mouse_Horizontal.ReadValue<float>());
		}
	}

	// Token: 0x170000FF RID: 255
	// (get) Token: 0x060005F3 RID: 1523 RVA: 0x0001D59C File Offset: 0x0001B79C
	private Vector2 totalMouseDirection
	{
		get
		{
			return new Vector2((float)this.lastMouseDirectionX.Value, (float)this.lastMouseDirectionY.Value);
		}
	}

	// Token: 0x060005F4 RID: 1524 RVA: 0x0001D5BC File Offset: 0x0001B7BC
	public AttackDirection GetAttackDirection()
	{
		if (Mathf.Abs(this.totalMouseDirection.y) > Mathf.Abs(this.totalMouseDirection.x))
		{
			if (this.totalMouseDirection.y >= 0f)
			{
				return AttackDirection.Up;
			}
			return AttackDirection.Down;
		}
		else
		{
			if (this.totalMouseDirection.x >= 0f)
			{
				return AttackDirection.Right;
			}
			return AttackDirection.Left;
		}
	}

	// Token: 0x060005F5 RID: 1525 RVA: 0x0001D618 File Offset: 0x0001B818
	private void UpdateInputType(bool mouse)
	{
		if (this.rotatePlayer != null)
		{
			bool flag = this.playerTurnType != PlayerTurnType.TurnPlayer || mouse;
			if (flag != this.rotatePlayer.useTargetRotation)
			{
				this.rotatePlayer.SetUseTargetRotation(flag);
				if (this.cameraSmoothFollowControllable != null)
				{
					this.cameraSmoothFollowControllable.RecalculateRotationOffset();
				}
			}
		}
	}

	// Token: 0x060005F6 RID: 1526 RVA: 0x0001D674 File Offset: 0x0001B874
	public void UpdateTargetRotation()
	{
		if (this.cameraSmoothFollowControllable != null)
		{
			float num = this.cameraSmoothFollowControllable.rotationOffset.y % 360f;
			if (num < 0f)
			{
				num += 360f;
			}
			this.rotatePlayer.targetRotation = num;
		}
	}

	// Token: 0x17000100 RID: 256
	// (get) Token: 0x060005F7 RID: 1527 RVA: 0x0001D6C2 File Offset: 0x0001B8C2
	public bool targetRotationInUse
	{
		get
		{
			return this.rotatePlayer.useTargetRotation;
		}
	}

	// Token: 0x060005F8 RID: 1528 RVA: 0x0001D6CF File Offset: 0x0001B8CF
	public void DisableInputManager()
	{
		if (this.userControls != null)
		{
			this.userControlsEnabled = false;
			this.userControls.Disable();
		}
	}

	// Token: 0x060005F9 RID: 1529 RVA: 0x0001D6EB File Offset: 0x0001B8EB
	public void EnableInputManager()
	{
		if (this.userControls != null && !this.forceDisableControls)
		{
			this.userControlsEnabled = true;
			this.userControls.Enable();
		}
	}

	// Token: 0x060005FA RID: 1530 RVA: 0x0001D70F File Offset: 0x0001B90F
	private void OnDestroy()
	{
		this.DisposeUserControls();
	}

	// Token: 0x060005FB RID: 1531 RVA: 0x0001D717 File Offset: 0x0001B917
	public void DisposeUserControls()
	{
		if (this.userControls != null)
		{
			this.userControls.Disable();
			this.userControls.Dispose();
		}
	}

	// Token: 0x040003C4 RID: 964
	public UserControls userControls;

	// Token: 0x040003C5 RID: 965
	public InputUser inputUser;

	// Token: 0x040003C6 RID: 966
	private List<BallMovement> ballMovements;

	// Token: 0x040003C7 RID: 967
	private GameObject playerCharacter;

	// Token: 0x040003C9 RID: 969
	public PlayerAnimator playerAnimator;

	// Token: 0x040003CA RID: 970
	private MoveSetEditor moveSetEditor;

	// Token: 0x040003CB RID: 971
	private CameraSmoothFollowControllable cameraSmoothFollowControllable;

	// Token: 0x040003CC RID: 972
	public bool rollingFeet;

	// Token: 0x040003CD RID: 973
	public int maxMouseDirectionHistory = 6;

	// Token: 0x040003CE RID: 974
	public HudCanvas hudCanvas;

	// Token: 0x040003CF RID: 975
	public GameObject hudCanvasPrefab;

	// Token: 0x040003D0 RID: 976
	public StaminaHudCanvas staminaHudCanvas;

	// Token: 0x040003D1 RID: 977
	public GameObject staminaHudCanvasPrefab;

	// Token: 0x040003D2 RID: 978
	public PlayerTurnType playerTurnType;

	// Token: 0x040003D3 RID: 979
	public float mouseSensitivity = 0.5f;

	// Token: 0x040003D4 RID: 980
	private InputAction[] currentDirectionalAction = new InputAction[2];

	// Token: 0x040003D5 RID: 981
	public ExponentialMovingAverage lastMouseDirectionX;

	// Token: 0x040003D6 RID: 982
	public ExponentialMovingAverage lastMouseDirectionY;

	// Token: 0x040003D7 RID: 983
	private float _lastMouseActionTime;

	// Token: 0x040003D8 RID: 984
	public bool forceDisableControls;

	// Token: 0x040003D9 RID: 985
	public bool userControlsEnabled = true;
}
