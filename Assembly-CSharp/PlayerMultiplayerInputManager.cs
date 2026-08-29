using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Mirror;
using Mirror.RemoteCalls;
using MoveClasses;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using Utils;

// Token: 0x0200008F RID: 143
public class PlayerMultiplayerInputManager : NetworkBehaviour, IInputManager, IDisableableInputManager, IPlayerInputManager
{
	// Token: 0x170000EC RID: 236
	// (get) Token: 0x060004A2 RID: 1186 RVA: 0x000160C4 File Offset: 0x000142C4
	// (set) Token: 0x060004A3 RID: 1187 RVA: 0x000160CC File Offset: 0x000142CC
	public RotatePlayer rotatePlayer { get; set; }

	// Token: 0x170000ED RID: 237
	// (get) Token: 0x060004A4 RID: 1188 RVA: 0x000160D5 File Offset: 0x000142D5
	// (set) Token: 0x060004A5 RID: 1189 RVA: 0x000160DD File Offset: 0x000142DD
	public MultiplayerRoomPlayer multiplayerRoomPlayer { get; set; }

	// Token: 0x060004A6 RID: 1190 RVA: 0x000160E6 File Offset: 0x000142E6
	public virtual void SetMultiplayerRoomPlayerIdentity(uint oldIdentity, uint newIdentity)
	{
		this.FindMultiplayerRoomPlayer();
	}

	// Token: 0x060004A7 RID: 1191 RVA: 0x000160EE File Offset: 0x000142EE
	public virtual void PlayerNameChanged(string _, string newPlayerName)
	{
		this.SetPlayerName();
	}

	// Token: 0x060004A8 RID: 1192 RVA: 0x000160F6 File Offset: 0x000142F6
	private void SetPlayerHealthIdentity(uint oldIdentity, uint newIdentity)
	{
		if (newIdentity > 0U)
		{
			this.InitPlayerHealth();
		}
	}

	// Token: 0x060004A9 RID: 1193 RVA: 0x00016104 File Offset: 0x00014304
	public void InitPlayerHealth()
	{
		if (this.playerHealthInitialized && this.playerHealth != null)
		{
			return;
		}
		if (this.playerHealth == null)
		{
			this.FindPlayerHealth();
		}
		if (this.playerHealth != null)
		{
			this.playerHealthInitialized = true;
			if (base.isLocalPlayer)
			{
				this.cameraSmoothFollowControllable = this.playerHealth.SetupSmoothCameraFollow(null);
				this.SetupLocalPlayer();
			}
			this.playerHealth.multiplayerInputManager = this;
			this.playerHealth.playerInputManager = this;
			this.UpdatePlayerHealthMultiplayerRoomPlayer();
			this.SetPlayerName();
			this.SetTexture();
			this.InstantiateLocalArmour();
		}
	}

	// Token: 0x060004AA RID: 1194 RVA: 0x000161A1 File Offset: 0x000143A1
	private void SetPlayerEquipment(List<EquippedEquipment> oldEquipment, List<EquippedEquipment> newEquipment)
	{
		if (oldEquipment == null || oldEquipment.Count <= 0)
		{
			if (this.playerHealth == null)
			{
				this.FindPlayerHealth();
			}
			if (this.playerHealth != null)
			{
				this.InstantiateLocalArmour();
			}
		}
	}

	// Token: 0x060004AB RID: 1195 RVA: 0x000161D8 File Offset: 0x000143D8
	private void FindPlayerHealth()
	{
		if (this.playerHealthIdentity != 0U && this.playerHealth == null)
		{
			foreach (NetworkIdentity networkIdentity in Resources.FindObjectsOfTypeAll<NetworkIdentity>())
			{
				if (networkIdentity.netId == this.playerHealthIdentity)
				{
					this.playerHealth = networkIdentity.gameObject.GetComponent<PlayerHealth>();
				}
			}
		}
	}

	// Token: 0x060004AC RID: 1196 RVA: 0x00016234 File Offset: 0x00014434
	private void FindMultiplayerRoomPlayer()
	{
		if (this.multiplayerRoomPlayerIdentity != 0U && this.multiplayerRoomPlayer == null)
		{
			NetworkIdentity networkIdentity = null;
			NetworkClient.spawned.TryGetValue(this.multiplayerRoomPlayerIdentity, out networkIdentity);
			if (networkIdentity != null)
			{
				this.multiplayerRoomPlayer = networkIdentity.GetComponent<MultiplayerRoomPlayer>();
				this.multiplayerRoomPlayer.RegisterPlayerMultiplayerInputManager(this);
				this.UpdatePlayerHealthMultiplayerRoomPlayer();
				this.SetTexture();
			}
		}
	}

	// Token: 0x060004AD RID: 1197 RVA: 0x0001629C File Offset: 0x0001449C
	private void UpdatePlayerHealthMultiplayerRoomPlayer()
	{
		if (this.playerHealth != null && this.multiplayerRoomPlayer != null && this.playerHealth.multiplayerRoomPlayer == null)
		{
			this.playerHealth.RegisterMultiplayerRoomPlayer(this.multiplayerRoomPlayer);
		}
	}

	// Token: 0x060004AE RID: 1198 RVA: 0x000162EC File Offset: 0x000144EC
	private void SetPlayerName()
	{
		if (this.playerHealth == null)
		{
			this.FindPlayerHealth();
		}
		if (this.playerHealth != null)
		{
			this.playerHealth.SetPlayerName(this.playerName);
			if (base.isLocalPlayer)
			{
				this.playerHealth.HidePlayerName();
			}
		}
	}

	// Token: 0x060004AF RID: 1199 RVA: 0x00016340 File Offset: 0x00014540
	private void InstantiateLocalArmour()
	{
		if (!base.isServer && !NetworkServer.active && this.playerHealth != null)
		{
			List<EquippedEquipment> list = (from x in this.equippedEquipment
			where x.position != EquipmentPosition.HandLeft && x.position != EquipmentPosition.HandRight
			select x).ToList<EquippedEquipment>();
			this.playerHealth.SetEquipment(list, false);
		}
	}

	// Token: 0x060004B0 RID: 1200 RVA: 0x000163A7 File Offset: 0x000145A7
	private void Awake()
	{
		if (IGameSettingsManager.singleton != null)
		{
			this.rollingFeet = IGameSettingsManager.singleton.GetRollingFeet();
		}
		this.lastMouseDirectionY = new ExponentialMovingAverage(this.maxMouseDirectionHistory);
		this.lastMouseDirectionX = new ExponentialMovingAverage(this.maxMouseDirectionHistory);
	}

	// Token: 0x060004B1 RID: 1201 RVA: 0x000163E2 File Offset: 0x000145E2
	private void OnDestroy()
	{
		if (this.numberOfThisPlayer > 0)
		{
			PlayerMultiplayerInputManager.numberOfPlayers--;
		}
		this.DisposeUserControls();
	}

	// Token: 0x060004B2 RID: 1202 RVA: 0x00016400 File Offset: 0x00014600
	private void Start()
	{
		if (base.isClient && base.isOwned)
		{
			if (MultiplayerRoomPlayer.localMultiplayerRoomPlayer.spectator || this.playerHealth == null)
			{
				this.SetupSpectatorCamera();
			}
			else
			{
				this.SetupBasicCharacterControls();
			}
		}
		if (base.isLocalPlayer && this.playerHealth != null)
		{
			this.cameraSmoothFollowControllable = this.playerHealth.SetupSmoothCameraFollow(null);
			this.SetPlayerName();
			this.SetupLocalPlayer();
		}
		if (base.isClient)
		{
			this.InitPlayerHealth();
		}
	}

	// Token: 0x060004B3 RID: 1203 RVA: 0x00016487 File Offset: 0x00014687
	private void SetupSpectatorCamera()
	{
		if (Camera.main.gameObject.GetComponent<ReplayCameraControls>() == null)
		{
			Camera.main.gameObject.AddComponent<ReplayCameraControls>().SetCameraMode(CameraMode.Free);
		}
	}

	// Token: 0x060004B4 RID: 1204 RVA: 0x000164B8 File Offset: 0x000146B8
	private void SetupLocalPlayer()
	{
		if (this.cameraSmoothFollowControllable != null)
		{
			this.cameraSmoothFollowControllable.SetStartRotationOffset();
			this.cameraSmoothFollowControllable.playerInputManager = this;
			this.SetUseTargetRotation(true);
			this.UpdateTargetRotation();
			if (this.hudCanvas == null)
			{
				this.hudCanvas = UnityEngine.Object.Instantiate<GameObject>(this.hudCanvasPrefab).GetComponent<HudCanvas>();
			}
			this.hudCanvas.playerInputManager = this;
			this.hudCanvas.SetupCamera(this.cameraSmoothFollowControllable.objectCamera);
			if (this.staminaHudCanvas == null)
			{
				this.staminaHudCanvas = UnityEngine.Object.Instantiate<GameObject>(this.staminaHudCanvasPrefab).GetComponent<StaminaHudCanvas>();
				this.staminaHudCanvas.playerInputManager = this;
			}
			this.staminaHudCanvas.playerHealth = this.playerHealth;
			this.staminaHudCanvas.SetupCamera(this.cameraSmoothFollowControllable.objectCamera);
			this.LoadSettings();
		}
	}

	// Token: 0x060004B5 RID: 1205 RVA: 0x000165A0 File Offset: 0x000147A0
	public void LoadUserControl()
	{
		if (this.userControls != null)
		{
			this.userControls.Disable();
			this.userControls.Dispose();
		}
		if (base.isClient && base.isOwned)
		{
			if (!this.useCameraControls)
			{
				this.SetupBasicCharacterControls();
				return;
			}
			this.SetupBasicCameraControls();
		}
	}

	// Token: 0x060004B6 RID: 1206 RVA: 0x000165F0 File Offset: 0x000147F0
	private void SetupBasicCharacterControls()
	{
		this.userControls = SettingsHelper.GetUserControls();
		this.BindTurnMovement();
		if (this.rollingFeet)
		{
			this.BindRollMovement();
		}
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
		this.BindMouseTurn();
		this.BindDirectionalActions();
		if (GeneralManager.InputSystemDisabled())
		{
			this.DisableInputManager();
		}
		if (this.userControlsEnabled)
		{
			this.userControls.Enable();
		}
	}

	// Token: 0x060004B7 RID: 1207 RVA: 0x00016750 File Offset: 0x00014950
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

	// Token: 0x060004B8 RID: 1208 RVA: 0x00016888 File Offset: 0x00014A88
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

	// Token: 0x060004B9 RID: 1209 RVA: 0x0001693C File Offset: 0x00014B3C
	public void BindDirectionalActions()
	{
		if (this.userControls != null)
		{
			this.userControls.PlayerActionMap.Directional_Action1.started += this.Directional_Action_performed;
			this.userControls.PlayerActionMap.Directional_Action1.canceled += this.Directional_Action_performed;
			this.userControls.PlayerActionMap.Directional_Action2.started += this.Directional_Action_performed;
			this.userControls.PlayerActionMap.Directional_Action2.canceled += this.Directional_Action_performed;
		}
	}

	// Token: 0x060004BA RID: 1210 RVA: 0x000169E4 File Offset: 0x00014BE4
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

	// Token: 0x060004BB RID: 1211 RVA: 0x00016B1C File Offset: 0x00014D1C
	public void SetupBasicCameraControls()
	{
		this.useCameraControls = true;
		if (this.userControls != null)
		{
			this.userControls.Dispose();
		}
		this.userControls = SettingsHelper.GetUserControls();
		this.userControls.Disable();
		this.SetupSpectatorCamera();
	}

	// Token: 0x060004BC RID: 1212 RVA: 0x00016B54 File Offset: 0x00014D54
	private void Update()
	{
		if (base.isLocalPlayer)
		{
			this.UpdateMouseDirectionHistory();
		}
	}

	// Token: 0x060004BD RID: 1213 RVA: 0x00016B64 File Offset: 0x00014D64
	[Server]
	public void AttemptToCreatePlayerCharacter()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerMultiplayerInputManager::AttemptToCreatePlayerCharacter()' called when server was not active");
			return;
		}
		this.InitializeSpawnPoints();
		if (this.multiplayerRoomPlayer != null && !this.multiplayerRoomPlayer.spectator && !this.multiplayerRoomPlayer.GetJoinedMidGame())
		{
			this.CreatePlayer();
		}
	}

	// Token: 0x060004BE RID: 1214 RVA: 0x00016BBC File Offset: 0x00014DBC
	private void InitializeSpawnPoints()
	{
		if (this.spawnPoints.Count == 0)
		{
			PlayerMultiplayerInputManager.numberOfPlayers++;
			this.numberOfThisPlayer = PlayerMultiplayerInputManager.numberOfPlayers;
			this.spawnPoints = new List<Transform>();
			foreach (object obj in GameObject.Find("SpawnPoints").transform)
			{
				Transform item = (Transform)obj;
				this.spawnPoints.Add(item);
			}
		}
	}

	// Token: 0x060004BF RID: 1215 RVA: 0x00016C54 File Offset: 0x00014E54
	[Server]
	private void CreatePlayer()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerMultiplayerInputManager::CreatePlayer()' called when server was not active");
			return;
		}
		Transform transform = null;
		if (this.spawnPoints.Count >= this.numberOfThisPlayer)
		{
			transform = this.spawnPoints[this.numberOfThisPlayer - 1];
		}
		this.playerCharacter = UnityEngine.Object.Instantiate<GameObject>(this.playerPrefab, transform.position, transform.rotation);
		NetworkServer.Spawn(this.playerCharacter, null);
		this.ballMovements = Generic.FindComponentsInChildObjects<BallMovement>(this.playerCharacter);
		this.rotatePlayer = Generic.FindComponentsInChildObjects<RotatePlayer>(this.playerCharacter).FirstOrDefault<RotatePlayer>();
		this.playerAnimator = Generic.FindComponentsInChildObjects<PlayerAnimator>(this.playerCharacter).FirstOrDefault<PlayerAnimator>();
		this.playerHealth = this.playerCharacter.GetComponent<PlayerHealth>();
		if (this.moveSet != null)
		{
			this.playerAnimator.SetMoveSet(this.moveSet, false, false);
			this.UpdateDefaultMovesetSettings();
		}
		this.playerHealth.SetEquipment(this.equippedEquipment, true);
		NetworkIdentity component = this.playerCharacter.GetComponent<NetworkIdentity>();
		this.NetworkplayerHealthIdentity = component.netId;
		this.playerHealth.OnlyPhysical();
	}

	// Token: 0x060004C0 RID: 1216 RVA: 0x00016D6C File Offset: 0x00014F6C
	[Server]
	public void UpdateDefaultMovesetSettings()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerMultiplayerInputManager::UpdateDefaultMovesetSettings()' called when server was not active");
			return;
		}
		if (this.playerAnimator != null && this.multiplayerRoomPlayer != null)
		{
			this.playerAnimator.SetBasicMoveSetBindings(this.multiplayerRoomPlayer.userDefaultMovesetSettings);
		}
	}

	// Token: 0x060004C1 RID: 1217 RVA: 0x00016DC0 File Offset: 0x00014FC0
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

	// Token: 0x060004C2 RID: 1218 RVA: 0x00016E30 File Offset: 0x00015030
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

	// Token: 0x060004C3 RID: 1219 RVA: 0x00016EA0 File Offset: 0x000150A0
	private void Move_Forward_performed(InputAction.CallbackContext obj)
	{
		float num = 0f;
		if (!obj.canceled)
		{
			num = obj.ReadValue<float>();
		}
		this.inputs.Vertical = num;
		this.CmdVertical(num);
	}

	// Token: 0x060004C4 RID: 1220 RVA: 0x00016ED8 File Offset: 0x000150D8
	private void Move_Back_performed(InputAction.CallbackContext obj)
	{
		float num = 0f;
		if (!obj.canceled)
		{
			num = obj.ReadValue<float>();
		}
		this.CmdVertical(num * -1f);
	}

	// Token: 0x060004C5 RID: 1221 RVA: 0x00016F0C File Offset: 0x0001510C
	private void Move_Left_performed(InputAction.CallbackContext obj)
	{
		float num = 0f;
		if (!obj.canceled)
		{
			num = obj.ReadValue<float>();
		}
		this.CmdHorizontal(num * -1f);
	}

	// Token: 0x060004C6 RID: 1222 RVA: 0x00016F40 File Offset: 0x00015140
	private void Move_Right_performed(InputAction.CallbackContext obj)
	{
		float value = 0f;
		if (!obj.canceled)
		{
			value = obj.ReadValue<float>();
		}
		this.CmdHorizontal(value);
	}

	// Token: 0x060004C7 RID: 1223 RVA: 0x00016F6C File Offset: 0x0001516C
	private void Turn_Left_performed(InputAction.CallbackContext obj)
	{
		float num = 0f;
		if (!obj.canceled)
		{
			num = obj.ReadValue<float>();
		}
		this.UpdateInputType(false);
		if (this.targetRotationInUse)
		{
			this.cameraSmoothFollowControllable.SetRotationInputLeft(num * -1f);
			return;
		}
		this.CmdTurnLeft(num * -1f);
	}

	// Token: 0x060004C8 RID: 1224 RVA: 0x00016FC0 File Offset: 0x000151C0
	private void Turn_Right_performed(InputAction.CallbackContext obj)
	{
		float num = 0f;
		if (!obj.canceled)
		{
			num = obj.ReadValue<float>();
		}
		this.UpdateInputType(false);
		if (this.targetRotationInUse)
		{
			this.cameraSmoothFollowControllable.SetRotationInputRight(num);
			return;
		}
		this.CmdTurnRight(num);
	}

	// Token: 0x060004C9 RID: 1225 RVA: 0x00017008 File Offset: 0x00015208
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

	// Token: 0x060004CA RID: 1226 RVA: 0x00017050 File Offset: 0x00015250
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

	// Token: 0x060004CB RID: 1227 RVA: 0x0001709B File Offset: 0x0001529B
	private void Action_performed(InputAction.CallbackContext obj)
	{
		this.CmdAction(obj.action.name, (obj.started || obj.performed) ? 1 : 0);
	}

	// Token: 0x060004CC RID: 1228 RVA: 0x000170C8 File Offset: 0x000152C8
	[Command]
	private void CmdVertical(float value)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteFloat(value);
		base.SendCommandInternal("System.Void PlayerMultiplayerInputManager::CmdVertical(System.Single)", -958195726, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x060004CD RID: 1229 RVA: 0x00017104 File Offset: 0x00015304
	[Command]
	private void CmdHorizontal(float value)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteFloat(value);
		base.SendCommandInternal("System.Void PlayerMultiplayerInputManager::CmdHorizontal(System.Single)", 983876100, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x060004CE RID: 1230 RVA: 0x00017140 File Offset: 0x00015340
	[Command]
	private void CmdTurn(float value)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteFloat(value);
		base.SendCommandInternal("System.Void PlayerMultiplayerInputManager::CmdTurn(System.Single)", -1714319605, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x060004CF RID: 1231 RVA: 0x0001717C File Offset: 0x0001537C
	[Command]
	private void CmdTurnLeft(float value)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteFloat(value);
		base.SendCommandInternal("System.Void PlayerMultiplayerInputManager::CmdTurnLeft(System.Single)", 164069252, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x060004D0 RID: 1232 RVA: 0x000171B8 File Offset: 0x000153B8
	[Command]
	private void CmdTurnRight(float value)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteFloat(value);
		base.SendCommandInternal("System.Void PlayerMultiplayerInputManager::CmdTurnRight(System.Single)", -1265236819, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x060004D1 RID: 1233 RVA: 0x000171F4 File Offset: 0x000153F4
	[Command]
	private void CmdTargetRotation(float value)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteFloat(value);
		base.SendCommandInternal("System.Void PlayerMultiplayerInputManager::CmdTargetRotation(System.Single)", -1427721319, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x060004D2 RID: 1234 RVA: 0x00017230 File Offset: 0x00015430
	[Command]
	private void CmdAction(string actionName, int actionType)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(actionName);
		writer.WriteInt(actionType);
		base.SendCommandInternal("System.Void PlayerMultiplayerInputManager::CmdAction(System.String,System.Int32)", -697348754, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x060004D3 RID: 1235 RVA: 0x00017274 File Offset: 0x00015474
	public void HandlePlayerDeath()
	{
		if (base.isServer)
		{
			this.HandlePlayerDeathOnClient(this.playerHealth.deathReason);
		}
	}

	// Token: 0x060004D4 RID: 1236 RVA: 0x00017290 File Offset: 0x00015490
	[ClientRpc]
	public void HandlePlayerDeathOnClient(DeathReason deathReason)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		Mirror.GeneratedNetworkCode._Write_MoveClasses.DeathReason(writer, deathReason);
		this.SendRPCInternal("System.Void PlayerMultiplayerInputManager::HandlePlayerDeathOnClient(MoveClasses.DeathReason)", 1289753493, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x060004D5 RID: 1237 RVA: 0x000172CA File Offset: 0x000154CA
	public void DisableInputManager()
	{
		if (this.userControls != null)
		{
			this.userControlsEnabled = false;
			this.userControls.Disable();
		}
	}

	// Token: 0x060004D6 RID: 1238 RVA: 0x000172E6 File Offset: 0x000154E6
	public void EnableInputManager()
	{
		if (this.userControls != null)
		{
			this.userControlsEnabled = true;
			this.userControls.Enable();
		}
	}

	// Token: 0x060004D7 RID: 1239 RVA: 0x00017304 File Offset: 0x00015504
	private void Directional_Action_performed(InputAction.CallbackContext obj)
	{
		if (!obj.canceled)
		{
			obj.ReadValue<float>();
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
		this.CmdAction(inputAction.name, obj.started ? 1 : 0);
		if (obj.phase == InputActionPhase.Canceled)
		{
			this.currentDirectionalAction[num] = null;
			return;
		}
		this.currentDirectionalAction[num] = inputAction;
	}

	// Token: 0x060004D8 RID: 1240 RVA: 0x000173A4 File Offset: 0x000155A4
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

	// Token: 0x170000EE RID: 238
	// (get) Token: 0x060004D9 RID: 1241 RVA: 0x0001749C File Offset: 0x0001569C
	public float lastMouseActionTime
	{
		get
		{
			return this._lastMouseActionTime;
		}
	}

	// Token: 0x060004DA RID: 1242 RVA: 0x000174A4 File Offset: 0x000156A4
	private void UpdateMouseDirectionHistory()
	{
		if (this.userControls != null && (this.userControls.PlayerActionMap.Turn_Mouse_Vertical.WasPerformedThisFrame() || this.userControls.PlayerActionMap.Turn_Mouse_Horizontal.WasPerformedThisFrame()))
		{
			this._lastMouseActionTime = Time.unscaledTime;
			this.lastMouseDirectionY.Add((double)this.userControls.PlayerActionMap.Turn_Mouse_Vertical.ReadValue<float>());
			this.lastMouseDirectionX.Add((double)this.userControls.PlayerActionMap.Turn_Mouse_Horizontal.ReadValue<float>());
		}
	}

	// Token: 0x170000EF RID: 239
	// (get) Token: 0x060004DB RID: 1243 RVA: 0x00017543 File Offset: 0x00015743
	private Vector2 totalMouseDirection
	{
		get
		{
			return new Vector2((float)this.lastMouseDirectionX.Value, (float)this.lastMouseDirectionY.Value);
		}
	}

	// Token: 0x060004DC RID: 1244 RVA: 0x00017564 File Offset: 0x00015764
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

	// Token: 0x060004DD RID: 1245 RVA: 0x000175C0 File Offset: 0x000157C0
	private void UpdateInputType(bool mouse)
	{
		bool flag = this.playerTurnType != PlayerTurnType.TurnPlayer || mouse;
		if (flag != this.useTargetRotation)
		{
			this.SetUseTargetRotation(flag);
			if (this.cameraSmoothFollowControllable != null)
			{
				this.cameraSmoothFollowControllable.RecalculateRotationOffset();
			}
		}
	}

	// Token: 0x170000F0 RID: 240
	// (get) Token: 0x060004DE RID: 1246 RVA: 0x00017603 File Offset: 0x00015803
	public bool targetRotationInUse
	{
		get
		{
			return this.useTargetRotation;
		}
	}

	// Token: 0x060004DF RID: 1247 RVA: 0x0001760B File Offset: 0x0001580B
	private void SetUseTargetRotation(bool value)
	{
		this.useTargetRotation = value;
		this.CmdUseTargetRotation(this.useTargetRotation);
	}

	// Token: 0x060004E0 RID: 1248 RVA: 0x00017620 File Offset: 0x00015820
	[Command]
	private void CmdUseTargetRotation(bool value)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteBool(value);
		base.SendCommandInternal("System.Void PlayerMultiplayerInputManager::CmdUseTargetRotation(System.Boolean)", -590050132, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x060004E1 RID: 1249 RVA: 0x0001765A File Offset: 0x0001585A
	public void LoadSettings()
	{
		if (!base.isLocalPlayer)
		{
			return;
		}
		this.mouseSensitivity = SettingsHelper.GetMouseSensitivity();
		this.playerTurnType = SettingsHelper.GetPlayerTurnType();
		this.SetUseTargetRotation(this.playerTurnType == PlayerTurnType.TurnCamera);
	}

	// Token: 0x060004E2 RID: 1250 RVA: 0x0001768C File Offset: 0x0001588C
	[Client]
	public void UpdateTargetRotation()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void PlayerMultiplayerInputManager::UpdateTargetRotation()' called when client was not active");
			return;
		}
		if (base.isLocalPlayer && this.cameraSmoothFollowControllable != null)
		{
			float num = this.cameraSmoothFollowControllable.rotationOffset.y % 360f;
			if (num < 0f)
			{
				num += 360f;
			}
			this.CmdTargetRotation(num);
		}
	}

	// Token: 0x060004E3 RID: 1251 RVA: 0x000176F4 File Offset: 0x000158F4
	public void SetTexture()
	{
		if (this.playerHealth != null && this.playerHealth.multiplayerRoomPlayer != null && this.multiplayerRoomPlayer != null && this.multiplayerRoomPlayer.customPlayerTexture != null)
		{
			this.playerHealth.SetPlayerTexture(this.multiplayerRoomPlayer.customPlayerTexture);
		}
	}

	// Token: 0x060004E4 RID: 1252 RVA: 0x00017759 File Offset: 0x00015959
	public void DisposeUserControls()
	{
		if (this.userControls != null)
		{
			this.userControls.Disable();
			this.userControls.Dispose();
		}
	}

	// Token: 0x060004E6 RID: 1254 RVA: 0x0000C7D7 File Offset: 0x0000A9D7
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x170000F1 RID: 241
	// (get) Token: 0x060004E7 RID: 1255 RVA: 0x000177E0 File Offset: 0x000159E0
	// (set) Token: 0x060004E8 RID: 1256 RVA: 0x000177F3 File Offset: 0x000159F3
	public uint NetworkplayerHealthIdentity
	{
		get
		{
			return this.playerHealthIdentity;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<uint>(value, ref this.playerHealthIdentity, 1UL, new Action<uint, uint>(this.SetPlayerHealthIdentity));
		}
	}

	// Token: 0x170000F2 RID: 242
	// (get) Token: 0x060004E9 RID: 1257 RVA: 0x00017818 File Offset: 0x00015A18
	// (set) Token: 0x060004EA RID: 1258 RVA: 0x0001782B File Offset: 0x00015A2B
	public List<EquippedEquipment> NetworkequippedEquipment
	{
		get
		{
			return this.equippedEquipment;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<List<EquippedEquipment>>(value, ref this.equippedEquipment, 2UL, new Action<List<EquippedEquipment>, List<EquippedEquipment>>(this.SetPlayerEquipment));
		}
	}

	// Token: 0x170000F3 RID: 243
	// (get) Token: 0x060004EB RID: 1259 RVA: 0x00017850 File Offset: 0x00015A50
	// (set) Token: 0x060004EC RID: 1260 RVA: 0x00017863 File Offset: 0x00015A63
	public string NetworkplayerName
	{
		get
		{
			return this.playerName;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<string>(value, ref this.playerName, 4UL, new Action<string, string>(this.PlayerNameChanged));
		}
	}

	// Token: 0x170000F4 RID: 244
	// (get) Token: 0x060004ED RID: 1261 RVA: 0x0001788C File Offset: 0x00015A8C
	// (set) Token: 0x060004EE RID: 1262 RVA: 0x0001789F File Offset: 0x00015A9F
	public uint NetworkmultiplayerRoomPlayerIdentity
	{
		get
		{
			return this.multiplayerRoomPlayerIdentity;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<uint>(value, ref this.multiplayerRoomPlayerIdentity, 8UL, new Action<uint, uint>(this.SetMultiplayerRoomPlayerIdentity));
		}
	}

	// Token: 0x060004EF RID: 1263 RVA: 0x000178C8 File Offset: 0x00015AC8
	protected void UserCode_CmdVertical__Single(float value)
	{
		if (this.rollingFeet && this.ballMovements != null)
		{
			foreach (BallMovement ballMovement in this.ballMovements)
			{
				ballMovement.SetVerticalSpeed(value);
			}
		}
	}

	// Token: 0x060004F0 RID: 1264 RVA: 0x0001792C File Offset: 0x00015B2C
	protected static void InvokeUserCode_CmdVertical__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdVertical called on client.");
			return;
		}
		((PlayerMultiplayerInputManager)obj).UserCode_CmdVertical__Single(reader.ReadFloat());
	}

	// Token: 0x060004F1 RID: 1265 RVA: 0x00017958 File Offset: 0x00015B58
	protected void UserCode_CmdHorizontal__Single(float value)
	{
		if (this.rollingFeet && this.ballMovements != null)
		{
			foreach (BallMovement ballMovement in this.ballMovements)
			{
				ballMovement.SetHorizontalSpeed(value);
			}
		}
	}

	// Token: 0x060004F2 RID: 1266 RVA: 0x000179BC File Offset: 0x00015BBC
	protected static void InvokeUserCode_CmdHorizontal__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdHorizontal called on client.");
			return;
		}
		((PlayerMultiplayerInputManager)obj).UserCode_CmdHorizontal__Single(reader.ReadFloat());
	}

	// Token: 0x060004F3 RID: 1267 RVA: 0x000179E6 File Offset: 0x00015BE6
	protected void UserCode_CmdTurn__Single(float value)
	{
		value = ValidationHelpers.ValidateFloatInput(value);
		if (this.rotatePlayer != null)
		{
			this.rotatePlayer.SetRotationInput(value);
		}
	}

	// Token: 0x060004F4 RID: 1268 RVA: 0x00017A0A File Offset: 0x00015C0A
	protected static void InvokeUserCode_CmdTurn__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdTurn called on client.");
			return;
		}
		((PlayerMultiplayerInputManager)obj).UserCode_CmdTurn__Single(reader.ReadFloat());
	}

	// Token: 0x060004F5 RID: 1269 RVA: 0x00017A34 File Offset: 0x00015C34
	protected void UserCode_CmdTurnLeft__Single(float value)
	{
		value = ValidationHelpers.ValidateFloatInput(value);
		if (this.rotatePlayer != null)
		{
			this.rotatePlayer.SetRotationInputLeft(value);
		}
	}

	// Token: 0x060004F6 RID: 1270 RVA: 0x00017A58 File Offset: 0x00015C58
	protected static void InvokeUserCode_CmdTurnLeft__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdTurnLeft called on client.");
			return;
		}
		((PlayerMultiplayerInputManager)obj).UserCode_CmdTurnLeft__Single(reader.ReadFloat());
	}

	// Token: 0x060004F7 RID: 1271 RVA: 0x00017A82 File Offset: 0x00015C82
	protected void UserCode_CmdTurnRight__Single(float value)
	{
		value = ValidationHelpers.ValidateFloatInput(value);
		if (this.rotatePlayer != null)
		{
			this.rotatePlayer.SetRotationInputRight(value);
		}
	}

	// Token: 0x060004F8 RID: 1272 RVA: 0x00017AA6 File Offset: 0x00015CA6
	protected static void InvokeUserCode_CmdTurnRight__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdTurnRight called on client.");
			return;
		}
		((PlayerMultiplayerInputManager)obj).UserCode_CmdTurnRight__Single(reader.ReadFloat());
	}

	// Token: 0x060004F9 RID: 1273 RVA: 0x00017AD0 File Offset: 0x00015CD0
	protected void UserCode_CmdTargetRotation__Single(float value)
	{
		if (this.rotatePlayer != null)
		{
			this.rotatePlayer.targetRotation = value;
		}
	}

	// Token: 0x060004FA RID: 1274 RVA: 0x00017AEC File Offset: 0x00015CEC
	protected static void InvokeUserCode_CmdTargetRotation__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdTargetRotation called on client.");
			return;
		}
		((PlayerMultiplayerInputManager)obj).UserCode_CmdTargetRotation__Single(reader.ReadFloat());
	}

	// Token: 0x060004FB RID: 1275 RVA: 0x00017B16 File Offset: 0x00015D16
	protected void UserCode_CmdAction__String__Int32(string actionName, int actionType)
	{
		if (this.playerAnimator != null)
		{
			this.playerAnimator.ActivatePlayerAction(new PlayerAction
			{
				name = actionName,
				type = ((actionType == 1) ? ActionType.Start : ActionType.End)
			});
		}
	}

	// Token: 0x060004FC RID: 1276 RVA: 0x00017B4B File Offset: 0x00015D4B
	protected static void InvokeUserCode_CmdAction__String__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdAction called on client.");
			return;
		}
		((PlayerMultiplayerInputManager)obj).UserCode_CmdAction__String__Int32(reader.ReadString(), reader.ReadInt());
	}

	// Token: 0x060004FD RID: 1277 RVA: 0x00017B7C File Offset: 0x00015D7C
	protected void UserCode_HandlePlayerDeathOnClient__DeathReason(DeathReason deathReason)
	{
		if (this.playerHealth != null)
		{
			if (base.isLocalPlayer)
			{
				this.playerHealth.SetupFreeCamera();
				if (this.hudCanvas != null)
				{
					this.hudCanvas.SetForceDisableAttackDirection(true);
				}
				if (this.staminaHudCanvas != null)
				{
					this.staminaHudCanvas.SetForceDisableHud(true);
				}
			}
			if (!base.isServer)
			{
				this.playerHealth.HandleClientDeath(deathReason);
			}
		}
	}

	// Token: 0x060004FE RID: 1278 RVA: 0x00017BF2 File Offset: 0x00015DF2
	protected static void InvokeUserCode_HandlePlayerDeathOnClient__DeathReason(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC HandlePlayerDeathOnClient called on server.");
			return;
		}
		((PlayerMultiplayerInputManager)obj).UserCode_HandlePlayerDeathOnClient__DeathReason(Mirror.GeneratedNetworkCode._Read_MoveClasses.DeathReason(reader));
	}

	// Token: 0x060004FF RID: 1279 RVA: 0x00017C1B File Offset: 0x00015E1B
	protected void UserCode_CmdUseTargetRotation__Boolean(bool value)
	{
		if (this.rotatePlayer != null)
		{
			this.rotatePlayer.SetUseTargetRotation(value);
		}
	}

	// Token: 0x06000500 RID: 1280 RVA: 0x00017C37 File Offset: 0x00015E37
	protected static void InvokeUserCode_CmdUseTargetRotation__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdUseTargetRotation called on client.");
			return;
		}
		((PlayerMultiplayerInputManager)obj).UserCode_CmdUseTargetRotation__Boolean(reader.ReadBool());
	}

	// Token: 0x06000501 RID: 1281 RVA: 0x00017C60 File Offset: 0x00015E60
	static PlayerMultiplayerInputManager()
	{
		RemoteProcedureCalls.RegisterCommand(typeof(PlayerMultiplayerInputManager), "System.Void PlayerMultiplayerInputManager::CmdVertical(System.Single)", new RemoteCallDelegate(PlayerMultiplayerInputManager.InvokeUserCode_CmdVertical__Single), true);
		RemoteProcedureCalls.RegisterCommand(typeof(PlayerMultiplayerInputManager), "System.Void PlayerMultiplayerInputManager::CmdHorizontal(System.Single)", new RemoteCallDelegate(PlayerMultiplayerInputManager.InvokeUserCode_CmdHorizontal__Single), true);
		RemoteProcedureCalls.RegisterCommand(typeof(PlayerMultiplayerInputManager), "System.Void PlayerMultiplayerInputManager::CmdTurn(System.Single)", new RemoteCallDelegate(PlayerMultiplayerInputManager.InvokeUserCode_CmdTurn__Single), true);
		RemoteProcedureCalls.RegisterCommand(typeof(PlayerMultiplayerInputManager), "System.Void PlayerMultiplayerInputManager::CmdTurnLeft(System.Single)", new RemoteCallDelegate(PlayerMultiplayerInputManager.InvokeUserCode_CmdTurnLeft__Single), true);
		RemoteProcedureCalls.RegisterCommand(typeof(PlayerMultiplayerInputManager), "System.Void PlayerMultiplayerInputManager::CmdTurnRight(System.Single)", new RemoteCallDelegate(PlayerMultiplayerInputManager.InvokeUserCode_CmdTurnRight__Single), true);
		RemoteProcedureCalls.RegisterCommand(typeof(PlayerMultiplayerInputManager), "System.Void PlayerMultiplayerInputManager::CmdTargetRotation(System.Single)", new RemoteCallDelegate(PlayerMultiplayerInputManager.InvokeUserCode_CmdTargetRotation__Single), true);
		RemoteProcedureCalls.RegisterCommand(typeof(PlayerMultiplayerInputManager), "System.Void PlayerMultiplayerInputManager::CmdAction(System.String,System.Int32)", new RemoteCallDelegate(PlayerMultiplayerInputManager.InvokeUserCode_CmdAction__String__Int32), true);
		RemoteProcedureCalls.RegisterCommand(typeof(PlayerMultiplayerInputManager), "System.Void PlayerMultiplayerInputManager::CmdUseTargetRotation(System.Boolean)", new RemoteCallDelegate(PlayerMultiplayerInputManager.InvokeUserCode_CmdUseTargetRotation__Boolean), true);
		RemoteProcedureCalls.RegisterRpc(typeof(PlayerMultiplayerInputManager), "System.Void PlayerMultiplayerInputManager::HandlePlayerDeathOnClient(MoveClasses.DeathReason)", new RemoteCallDelegate(PlayerMultiplayerInputManager.InvokeUserCode_HandlePlayerDeathOnClient__DeathReason));
	}

	// Token: 0x06000502 RID: 1282 RVA: 0x00017D98 File Offset: 0x00015F98
	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteUInt(this.playerHealthIdentity);
			Mirror.GeneratedNetworkCode._Write_System.Collections.Generic.List`1<MoveClasses.EquippedEquipment>(writer, this.equippedEquipment);
			writer.WriteString(this.playerName);
			writer.WriteUInt(this.multiplayerRoomPlayerIdentity);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1UL) != 0UL)
		{
			writer.WriteUInt(this.playerHealthIdentity);
		}
		if ((base.syncVarDirtyBits & 2UL) != 0UL)
		{
			Mirror.GeneratedNetworkCode._Write_System.Collections.Generic.List`1<MoveClasses.EquippedEquipment>(writer, this.equippedEquipment);
		}
		if ((base.syncVarDirtyBits & 4UL) != 0UL)
		{
			writer.WriteString(this.playerName);
		}
		if ((base.syncVarDirtyBits & 8UL) != 0UL)
		{
			writer.WriteUInt(this.multiplayerRoomPlayerIdentity);
		}
	}

	// Token: 0x06000503 RID: 1283 RVA: 0x00017E7C File Offset: 0x0001607C
	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			base.GeneratedSyncVarDeserialize<uint>(ref this.playerHealthIdentity, new Action<uint, uint>(this.SetPlayerHealthIdentity), reader.ReadUInt());
			base.GeneratedSyncVarDeserialize<List<EquippedEquipment>>(ref this.equippedEquipment, new Action<List<EquippedEquipment>, List<EquippedEquipment>>(this.SetPlayerEquipment), Mirror.GeneratedNetworkCode._Read_System.Collections.Generic.List`1<MoveClasses.EquippedEquipment>(reader));
			base.GeneratedSyncVarDeserialize<string>(ref this.playerName, new Action<string, string>(this.PlayerNameChanged), reader.ReadString());
			base.GeneratedSyncVarDeserialize<uint>(ref this.multiplayerRoomPlayerIdentity, new Action<uint, uint>(this.SetMultiplayerRoomPlayerIdentity), reader.ReadUInt());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 1L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<uint>(ref this.playerHealthIdentity, new Action<uint, uint>(this.SetPlayerHealthIdentity), reader.ReadUInt());
		}
		if ((num & 2L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<List<EquippedEquipment>>(ref this.equippedEquipment, new Action<List<EquippedEquipment>, List<EquippedEquipment>>(this.SetPlayerEquipment), Mirror.GeneratedNetworkCode._Read_System.Collections.Generic.List`1<MoveClasses.EquippedEquipment>(reader));
		}
		if ((num & 4L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<string>(ref this.playerName, new Action<string, string>(this.PlayerNameChanged), reader.ReadString());
		}
		if ((num & 8L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<uint>(ref this.multiplayerRoomPlayerIdentity, new Action<uint, uint>(this.SetMultiplayerRoomPlayerIdentity), reader.ReadUInt());
		}
	}

	// Token: 0x040002E6 RID: 742
	public static int numberOfPlayers;

	// Token: 0x040002E7 RID: 743
	public int numberOfThisPlayer;

	// Token: 0x040002E8 RID: 744
	public UserControls userControls;

	// Token: 0x040002E9 RID: 745
	public InputUser inputUser;

	// Token: 0x040002EA RID: 746
	private List<BallMovement> ballMovements;

	// Token: 0x040002EB RID: 747
	private GameObject playerCharacter;

	// Token: 0x040002ED RID: 749
	private PlayerAnimator playerAnimator;

	// Token: 0x040002EE RID: 750
	public ClientInputs previousValue = new ClientInputs();

	// Token: 0x040002EF RID: 751
	public InputHolder inputs = new InputHolder();

	// Token: 0x040002F0 RID: 752
	private InputHolder targetInputs = new InputHolder();

	// Token: 0x040002F1 RID: 753
	public GameObject playerPrefab;

	// Token: 0x040002F2 RID: 754
	public List<Transform> spawnPoints;

	// Token: 0x040002F3 RID: 755
	public PlayerHealth playerHealth;

	// Token: 0x040002F4 RID: 756
	[SyncVar(hook = "SetPlayerHealthIdentity")]
	public uint playerHealthIdentity;

	// Token: 0x040002F5 RID: 757
	public MoveSet moveSet;

	// Token: 0x040002F6 RID: 758
	[SyncVar(hook = "SetPlayerEquipment")]
	public List<EquippedEquipment> equippedEquipment;

	// Token: 0x040002F7 RID: 759
	[SyncVar(hook = "PlayerNameChanged")]
	public string playerName = "";

	// Token: 0x040002F8 RID: 760
	public bool rollingFeet;

	// Token: 0x040002F9 RID: 761
	private CameraSmoothFollowControllable cameraSmoothFollowControllable;

	// Token: 0x040002FA RID: 762
	public int maxMouseDirectionHistory = 6;

	// Token: 0x040002FB RID: 763
	public HudCanvas hudCanvas;

	// Token: 0x040002FC RID: 764
	public GameObject hudCanvasPrefab;

	// Token: 0x040002FD RID: 765
	public StaminaHudCanvas staminaHudCanvas;

	// Token: 0x040002FE RID: 766
	public GameObject staminaHudCanvasPrefab;

	// Token: 0x040002FF RID: 767
	[SyncVar(hook = "SetMultiplayerRoomPlayerIdentity")]
	public uint multiplayerRoomPlayerIdentity;

	// Token: 0x04000301 RID: 769
	private bool playerHealthInitialized;

	// Token: 0x04000302 RID: 770
	private bool useCameraControls;

	// Token: 0x04000303 RID: 771
	public float mouseSensitivity = 0.5f;

	// Token: 0x04000304 RID: 772
	public bool userControlsEnabled = true;

	// Token: 0x04000305 RID: 773
	private InputAction[] currentDirectionalAction = new InputAction[2];

	// Token: 0x04000306 RID: 774
	public ExponentialMovingAverage lastMouseDirectionX;

	// Token: 0x04000307 RID: 775
	public ExponentialMovingAverage lastMouseDirectionY;

	// Token: 0x04000308 RID: 776
	private float _lastMouseActionTime;

	// Token: 0x04000309 RID: 777
	private bool useTargetRotation;

	// Token: 0x0400030A RID: 778
	public PlayerTurnType playerTurnType;
}
