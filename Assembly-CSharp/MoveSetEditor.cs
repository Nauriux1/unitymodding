using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BasicUI;
using MoveClasses;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityUIExtensionMethods;
using Utils;

// Token: 0x020001E7 RID: 487
public class MoveSetEditor : MonoBehaviour
{
	// Token: 0x1700019A RID: 410
	// (get) Token: 0x06000EBE RID: 3774 RVA: 0x0004AD28 File Offset: 0x00048F28
	// (set) Token: 0x06000EBF RID: 3775 RVA: 0x0004AD30 File Offset: 0x00048F30
	public List<SingleMoveEditorListItem> singleMoveEditorLists { get; set; }

	// Token: 0x1700019B RID: 411
	// (get) Token: 0x06000EC0 RID: 3776 RVA: 0x0004AD39 File Offset: 0x00048F39
	public bool twoHandedRigAndHandSelected
	{
		get
		{
			return this.selectedRigTarget != null && this.usingTwoHandedRig && !this.selectedRigTarget.isHint && this.selectedRigTarget.Rig.isHand;
		}
	}

	// Token: 0x1700019C RID: 412
	// (get) Token: 0x06000EC1 RID: 3777 RVA: 0x0004AD6D File Offset: 0x00048F6D
	public EditorMode CurrentEditorMode
	{
		get
		{
			if (this.selectedMove != null)
			{
				return EditorMode.SingleMove;
			}
			if (this.selectedStance != null)
			{
				return EditorMode.Move;
			}
			return EditorMode.Stance;
		}
	}

	// Token: 0x1700019D RID: 413
	// (get) Token: 0x06000EC2 RID: 3778 RVA: 0x0004AD84 File Offset: 0x00048F84
	public List<Stance> potentialStanceChanges
	{
		get
		{
			List<Stance> list = new List<Stance>();
			if (this.moveSet != null && this.moveSet.stanceList != null)
			{
				list = this.moveSet.stanceList;
				if (this.selectedStance != null)
				{
					list = (from x in list
					where x.guid != this.selectedStance.guid
					select x).ToList<Stance>();
				}
			}
			return list;
		}
	}

	// Token: 0x1700019E RID: 414
	// (get) Token: 0x06000EC3 RID: 3779 RVA: 0x0002D5B0 File Offset: 0x0002B7B0
	public bool CurrentlyFocusedOnTextField
	{
		get
		{
			return EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.GetComponent<InputField>() != null;
		}
	}

	// Token: 0x06000EC4 RID: 3780 RVA: 0x0004ADDC File Offset: 0x00048FDC
	private void Awake()
	{
		MoveSetEditor.singleton = this;
		this.selectedMove = null;
		this.selectedStance = null;
		this.userControls = SettingsHelper.GetUserControls();
		this.userControls.MoveEditorMap.Enable();
		this.userControls.Generic.Enable();
		this.resolution = new Vector2(0f, 0f);
		this.moveSet = (MoveSet)SceneManagerWithParameters.GetParameter("MoveSet");
		this.selectedStance = (Stance)SceneManagerWithParameters.GetParameter("SelectedStance");
		this.selectedMove = (Move)SceneManagerWithParameters.GetParameter("SelectedMove");
		if (this.selectedMove != null)
		{
			this.reActivateRig = true;
		}
		this.equipmentEditorButton.onClick.AddListener(delegate()
		{
			this.EquipmentButtonClicked();
		});
		UIHelpers.SetButtonColor(this.equipmentEditorButton, ButtonState.Basic, null, null);
		this.moveSetEditorButton.onClick.AddListener(delegate()
		{
			this.MoveSetButtonClicked();
		});
		UIHelpers.SetButtonColor(this.moveSetEditorButton, ButtonState.Basic, null, null);
		this.swapEditModeButton.onClick.AddListener(delegate()
		{
			this.SwapEditMode();
		});
		this.twoHandedRigButton.gameObject.SetActive(false);
		this.swapRigButton.onClick.AddListener(delegate()
		{
			this.SwapRig(null, true);
		});
		this.twoHandedRigButton.onClick.AddListener(delegate()
		{
			this.SwapTwoHandedRig(null);
		});
		this.MoveCurrentTimeField.onValueChanged.AddListener(delegate(string <p0>)
		{
			this.CurrentTimeChanged(this.MoveCurrentTimeField.text, false);
		});
		if (this.EquipmentEditorPrefab != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.EquipmentEditorPrefab, this.panelsHolder.transform);
			this.equipmentPanel = gameObject.GetComponent<EquipmentPanel>();
			this.equipmentPanel.gameObject.SetActive(false);
			this.equipmentPanel.playerHealth = this.playerHealth;
			this.equipmentPanel.moveSet = this.moveSet;
		}
		if (this.playerHealth != null)
		{
			this.playerHealth.SetupPlayerForMoveEditor();
		}
		this.SetupInputManager();
		this.InitMoveDot();
		this.InitFilters();
		this.InitInputFieldListeners();
	}

	// Token: 0x06000EC5 RID: 3781 RVA: 0x0004AFFC File Offset: 0x000491FC
	private void DisableControls()
	{
		this.userControls.Generic.Disable();
	}

	// Token: 0x06000EC6 RID: 3782 RVA: 0x0004B01C File Offset: 0x0004921C
	private void EnableControls()
	{
		this.userControls.Generic.Enable();
	}

	// Token: 0x06000EC7 RID: 3783 RVA: 0x0004B03C File Offset: 0x0004923C
	private void InitInputFieldListeners()
	{
		this.selectedMoveNameEditor.onEndEdit.AddListener(delegate(string <p0>)
		{
			this.OnNameChanged(this.selectedMove, this.selectedMoveNameEditor.text);
		});
		this.selectedMoveLayerEditor.onValueChanged.AddListener(delegate(int <p0>)
		{
			this.OnLayerChanged(this.selectedMove, this.selectedMoveLayerEditor);
		});
		this.selectedMoveInputTypeEditor.onValueChanged.AddListener(delegate(int <p0>)
		{
			this.OnInputTypeChanged(this.selectedMove, this.selectedMoveInputTypeEditor);
		});
		this.handStateSelectButtonHold.onClick.AddListener(delegate()
		{
			this.SetHandHoldState(new HandState?(HandState.Hold));
		});
		this.handStateSelectButtonLooseHold.onClick.AddListener(delegate()
		{
			this.SetHandHoldState(new HandState?(HandState.LooseHold));
		});
		this.handStateSelectButtonNoHold.onClick.AddListener(delegate()
		{
			this.SetHandHoldState(new HandState?(HandState.NoHold));
		});
		this.handStateSelectButtonEmptyHold.onClick.AddListener(delegate()
		{
			this.SetHandHoldState(null);
		});
		this.buttonSetLeftHandState.onClick.AddListener(delegate()
		{
			this.ShowHandStatePanel(JointType.WRIST_LEFT, true);
		});
		this.buttonSetRightHandState.onClick.AddListener(delegate()
		{
			this.ShowHandStatePanel(JointType.WRIST_RIGHT, true);
		});
	}

	// Token: 0x06000EC8 RID: 3784 RVA: 0x0004B145 File Offset: 0x00049345
	private void InitFilters()
	{
		this.FillJointFilters();
	}

	// Token: 0x06000EC9 RID: 3785 RVA: 0x0004B150 File Offset: 0x00049350
	private void SetupInputManager()
	{
		if (this.playerInputManager == null && this.inputManager != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.inputManager);
			this.playerInputManager = gameObject.GetComponent<global::PlayerInputManager>();
			this.playerInputManager.ConnectToMoveSetEditor(this);
		}
	}

	// Token: 0x06000ECA RID: 3786 RVA: 0x0004B1A0 File Offset: 0x000493A0
	private void Start()
	{
		this.SetupPlaySelectedMoveButton();
		this.userControlsMap = SettingsHelper.GetUserControls();
		this.tempSingleMoves = new List<JointMove>();
		this.tempSingleMove = null;
		this.selectedSingleMove = null;
		this.singleMoveEditorLists = new List<SingleMoveEditorListItem>();
		this.animator = (PlayerAnimator)UnityEngine.Object.FindObjectsOfType(typeof(PlayerAnimator)).FirstOrDefault<UnityEngine.Object>();
		this.loadMoveSet(true);
		this.UpdateMoveMenu(true);
		this.activeCamera = null;
		this.activeCameraTarget = null;
		this.physicsCameraTarget = new GameObject("physicsCameraTarget").transform;
		this.animationCameraTarget = new GameObject("animationCameraTarget").transform;
		this.physicsCameraTarget.transform.position = this.focusedPhysicsCameraTarget.transform.position;
		this.animationCameraTarget.transform.position = this.focusedAnimationCameraTarget.transform.position;
		this.physicsCamera.transform.position = this.physicsCameraTarget.position;
		this.animationCamera.transform.position = this.animationCameraTarget.position;
		this.physicsCamera.transform.Translate(new Vector3(0f, 0f, this.activeZoomLevel));
		this.animationCamera.transform.Translate(new Vector3(0f, 0f, this.activeZoomLevel));
		if (this.moveSet != null)
		{
			this.moveSetNameInputField.text = this.moveSet.name;
		}
		this.moveSetNameInputField.onEndEdit.AddListener(delegate(string <p0>)
		{
			this.OnMoveSetNameChanged(this.moveSetNameInputField);
		});
		if (this.equipmentPanel != null)
		{
			this.equipmentPanel.UpdateEquipmentInfo(false, false);
		}
		this.SetupGeneralInputs();
		if (this.playerHealth != null)
		{
			StaminaManager.RegisterPlayerHealths(new List<PlayerHealth>
			{
				this.playerHealth
			});
		}
	}

	// Token: 0x06000ECB RID: 3787 RVA: 0x0004B388 File Offset: 0x00049588
	private void SetupGeneralInputs()
	{
		this.LoadGameSettings();
		this.timeScaleDropdown.options.Add(new OptionDataWithValue
		{
			text = "1.00x",
			floatValue = -1f
		});
		this.timeScaleDropdown.options.Add(new OptionDataWithValue
		{
			text = "0.75x",
			floatValue = 0.75f
		});
		this.timeScaleDropdown.options.Add(new OptionDataWithValue
		{
			text = "0.50x",
			floatValue = 0.5f
		});
		this.timeScaleDropdown.options.Add(new OptionDataWithValue
		{
			text = "0.25x",
			floatValue = 0.25f
		});
		float currentTimeScale = 1f;
		if (IGameSettingsManager.singleton != null)
		{
			currentTimeScale = IGameSettingsManager.singleton.TimeScaleMin;
		}
		Dropdown.OptionData optionData = (from x in this.timeScaleDropdown.options
		where x.GetFloatValue() == currentTimeScale
		select x).FirstOrDefault<Dropdown.OptionData>();
		if (optionData != null)
		{
			int value = this.timeScaleDropdown.options.IndexOf(optionData);
			this.timeScaleDropdown.value = value;
			this.OnTimeScaleChanged();
		}
		this.timeScaleDropdown.onValueChanged.AddListener(delegate(int <p0>)
		{
			this.OnTimeScaleChanged();
		});
		this.staminaToggle.onValueChanged.AddListener(delegate(bool <p0>)
		{
			this.OnStaminaChanged();
		});
		this.dismembermentToggle.onValueChanged.AddListener(delegate(bool <p0>)
		{
			this.OnDismembermentChanged();
		});
		foreach (object obj in Enum.GetValues(typeof(GameTypes)))
		{
			GameTypes gameTypes = (GameTypes)obj;
			this.gameTypeSelect.buttonOptions.Add(new ButtonOption
			{
				optionText = gameTypes.GetDescription(),
				optionIntValue = (int)gameTypes
			});
		}
		this.gameTypeSelect.ValueChanged += delegate(object <p0>, EventArgs <p1>)
		{
			this.GameTypeChanged();
		};
		this.UpdateGeneralInputDisplays();
	}

	// Token: 0x06000ECC RID: 3788 RVA: 0x0004B5B4 File Offset: 0x000497B4
	public void UpdateGeneralInputDisplays()
	{
		if (IGameSettingsManager.singleton != null)
		{
			this.gameTypeSelect.SetCurrentIntValue((int)IGameSettingsManager.singleton.GameType);
			this.staminaToggle.SetIsOnWithoutNotify(IGameSettingsManager.singleton.UseStamina);
			this.dismembermentToggle.SetIsOnWithoutNotify(IGameSettingsManager.singleton.UseDismemberment);
		}
		this.CheckForcedSettingValues();
	}

	// Token: 0x06000ECD RID: 3789 RVA: 0x0004B610 File Offset: 0x00049810
	private void SetupPlaySelectedMoveButton()
	{
		EventTrigger eventTrigger = this.playSelectedMoveButton.gameObject.AddComponent<EventTrigger>();
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerDown;
		entry.callback.AddListener(delegate(BaseEventData <p0>)
		{
			this.PlaySelectedMove();
		});
		eventTrigger.triggers.Add(entry);
		EventTrigger.Entry entry2 = new EventTrigger.Entry();
		entry2.eventID = EventTriggerType.PointerUp;
		entry2.callback.AddListener(delegate(BaseEventData <p0>)
		{
			this.CancelSelectedMove();
		});
		eventTrigger.triggers.Add(entry2);
	}

	// Token: 0x06000ECE RID: 3790 RVA: 0x0004B68C File Offset: 0x0004988C
	private void EquipmentButtonClicked()
	{
		this.SetVisiblePanel(this.equipmentPanel.gameObject);
		this.SetActiveCameras(true);
	}

	// Token: 0x06000ECF RID: 3791 RVA: 0x0004B6A6 File Offset: 0x000498A6
	private void MoveSetButtonClicked()
	{
		this.SetVisiblePanel(this.moveSetPanelGameObject);
		this.SetActiveCameras(false);
	}

	// Token: 0x06000ED0 RID: 3792 RVA: 0x0004B6BC File Offset: 0x000498BC
	private void SetActiveCameras(bool equipmentEditor)
	{
		if (equipmentEditor)
		{
			this.cameraEquipment.transform.position = this.animator.transform.GetChild(0).position + new Vector3(0f, 0f, -3f);
			this.animationCamera.SetActive(false);
			this.physicsCamera.SetActive(false);
			this.cameraEquipment.gameObject.SetActive(true);
			this.testMoveSetButton.gameObject.SetActive(false);
			this.equipmentEditorButton.gameObject.SetActive(false);
			return;
		}
		this.animationCamera.SetActive(true);
		this.physicsCamera.SetActive(true);
		this.cameraEquipment.gameObject.SetActive(false);
		this.testMoveSetButton.gameObject.SetActive(true);
		this.equipmentEditorButton.gameObject.SetActive(true);
	}

	// Token: 0x06000ED1 RID: 3793 RVA: 0x0004B7A8 File Offset: 0x000499A8
	public void SetVisiblePanel(GameObject panel)
	{
		this.moveSetPanelGameObject.SetActive(false);
		this.equipmentPanel.gameObject.SetActive(false);
		if (panel == this.equipmentPanel.gameObject)
		{
			this.sideToolButtons.SetCanvasVisible(false);
			this.timelineCanvas.gameObject.SetActive(false);
			this.ClearSelectedGizmoTargets();
			this.rigManager.SetActive(false);
			this.staminaHudCanvas.SetForceDisableHud(true);
		}
		else
		{
			if (StaminaManager.singleton != null)
			{
				StaminaManager.singleton.ResetLegacy();
			}
			this.sideToolButtons.SetCanvasVisible(true);
			this.timelineCanvas.gameObject.SetActive(true);
			if (this.usingIKRig)
			{
				this.rigManager.SetActive(true);
			}
			this.staminaHudCanvas.SetForceDisableHud(false);
		}
		panel.SetActive(true);
	}

	// Token: 0x06000ED2 RID: 3794 RVA: 0x0004B880 File Offset: 0x00049A80
	private void Update()
	{
		bool currentlyFocusedOnTextField = this.CurrentlyFocusedOnTextField;
		if ((this.userControls.Generic.Back.WasPerformedThisFrame() && this.listenActionForMove == null) || Keyboard.current.escapeKey.wasPressedThisFrame)
		{
			this.BackButtonPress();
		}
		if (this.userControls.MoveEditorMap.Delete.WasPerformedThisFrame())
		{
			this.DeleteSelectedSingleMoves();
		}
		if (this.userControls.MoveEditorMap.Copy.WasPerformedThisFrame())
		{
			this.CopyPerformed();
		}
		if (this.userControls.MoveEditorMap.Paste.WasPerformedThisFrame())
		{
			this.PastePerformed();
		}
		this.UpdateGizmoPosition();
		this.CalculateCameraSize();
		bool flag = this.HandleCameraMove();
		if (this.rigManager != null)
		{
			this.rigManager.HandleRigAnimations();
		}
		if (this.playingSelectedMove)
		{
			this.UpdateTimelinePlayMoveIndicator();
		}
		if (this.activeAnimationToggle.isOn && !this.dragginGizmo && this.selectedMove != null && this.selectedMove.jointMoveList != null && !this.playingSelectedMove)
		{
			this.PlayAnimation();
			if (this.reActivateRig)
			{
				this.rigManager.RecalculateTargetPosition();
				this.rigManager.SetDoAnimation(true);
				this.reActivateRig = false;
			}
		}
		else
		{
			this.animator.playingMovePreview = false;
			this.animator.ClearPreviewHistory();
		}
		this.updateAnimation = false;
		if (!this.rotatingCamera && !flag)
		{
			if (EventSystem.current.IsPointerOverGameObject())
			{
				this.activeCamera = null;
			}
			else
			{
				Vector2 vector = Mouse.current.position.ReadValue();
				if (this.cameraWidth > vector.x && vector.y > 240f)
				{
					this.activeCamera = this.physicsCamera;
					this.activeCameraTarget = this.physicsCameraTarget;
					this.activeZoomLevel = this.physicsZoomLevel;
				}
				else if (this.cameraWidth * 2f > vector.x && vector.y > 240f)
				{
					this.activeCamera = this.animationCamera;
					this.activeCameraTarget = this.animationCameraTarget;
					this.activeZoomLevel = this.animationZoomLevel;
				}
				else
				{
					this.activeCamera = null;
				}
			}
		}
		if (Mouse.current.scroll.ReadValue().y != 0f && this.activeCamera != null)
		{
			this.activeZoomLevel += Mouse.current.scroll.ReadValue().y * 0.005f;
			if (this.activeZoomLevel > 0f)
			{
				this.activeZoomLevel = 0f;
			}
			if (this.activeCamera == this.physicsCamera)
			{
				this.physicsZoomLevel = this.activeZoomLevel;
			}
			else
			{
				this.animationZoomLevel = this.activeZoomLevel;
			}
		}
		if (!this.CurrentlyFocusedOnTextField)
		{
			if (this.userControls.MoveEditorMap.Save_Move.WasPressedThisFrame())
			{
				this.CreateKeyframe();
			}
			if (this.userControls.MoveEditorMap.EditMode_Move.WasPressedThisFrame())
			{
				this.SetEditMode(EditMode.Move);
			}
			if (this.userControls.MoveEditorMap.EditMode_Rotate.WasPressedThisFrame())
			{
				this.SetEditMode(EditMode.Rotate);
			}
			if (this.userControls.MoveEditorMap.Save.WasPressedThisFrame())
			{
				this.SaveButtonPress();
			}
			if (this.userControls.MoveEditorMap.Undo.WasPressedThisFrame())
			{
				this.Undo();
			}
			if (this.userControls.MoveEditorMap.Redo.WasPressedThisFrame())
			{
				this.Redo();
			}
		}
		if (Mouse.current.leftButton.wasPressedThisFrame)
		{
			this.previousMousePosition = Mouse.current.position.ReadValue();
			if (!EventSystem.current.IsPointerOverGameObject())
			{
				this.rotatingCamera = true;
			}
			if (EventSystem.current.IsPointerOverGameObject() && this.selectedRotationLabel == null)
			{
				using (List<RaycastResult>.Enumerator enumerator = this.RaycastMouse().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						RaycastResult raycast = enumerator.Current;
						SingleMoveEditor component = raycast.gameObject.GetComponent<SingleMoveEditor>();
						if (component != null)
						{
							if (this.userControls.MoveEditorMap.Drag_Select.IsPressed())
							{
								this.ToggleSelectedSingleMove(component.move);
							}
							else
							{
								this.SetSelectedSingleMove(component.move);
							}
						}
						RotationalLabel component2 = raycast.gameObject.GetComponent<RotationalLabel>();
						if (component2)
						{
							if (component2.inputField.interactable)
							{
								this.selectedRotationLabel = component2;
							}
							else
							{
								this.selectedRotationLabel = null;
							}
						}
						if (raycast.gameObject.name.Contains("moveDot"))
						{
							MoveDot moveDot = (from x in this.moveDots
							where x.Name == raycast.gameObject.name
							select x).FirstOrDefault<MoveDot>();
							if (moveDot != null)
							{
								if (!this.JointMoveAlreadySelected(moveDot.SingleMove))
								{
									if (this.userControls.MoveEditorMap.Drag_Select.IsPressed())
									{
										this.ToggleSelectedSingleMove(moveDot.SingleMove);
									}
									else
									{
										this.SetSelectedSingleMove(moveDot.SingleMove);
									}
								}
								else
								{
									this.SwapDelectedSingleMove(moveDot.SingleMove);
								}
								this.draggingMoveDot = true;
								break;
							}
						}
						else if (raycast.gameObject.name.Contains("TimeLineMovePanel1"))
						{
							if (this.userControls.MoveEditorMap.Drag_Select.IsPressed())
							{
								this.selectionStart = new Vector2?(Mouse.current.position.ReadValue());
								break;
							}
							this.CalculateCurrentTimeFromMousePosition();
							this.draggingCurrentTime = true;
							break;
						}
					}
					goto IL_6CE;
				}
			}
			if (this.selectedRotationLabel == null)
			{
				foreach (RaycastHit raycastHit in this.RaycastPhysicsMouse())
				{
					if (raycastHit.transform.gameObject.layer != 10 && (raycastHit.transform.gameObject.name == "X" || raycastHit.transform.gameObject.name == "Y" || raycastHit.transform.gameObject.name == "Z"))
					{
						if (raycastHit.transform.parent.gameObject == this.animationRotationGizmo)
						{
							this.draggingAnimationAxis = raycastHit.transform.gameObject.name;
						}
						else
						{
							this.draggingPhysicalAxis = raycastHit.transform.gameObject.name;
						}
						this.rotatingCamera = false;
						break;
					}
				}
			}
			IL_6CE:
			this.previousMousePos = Mouse.current.position.ReadValue();
		}
		else if (Mouse.current.leftButton.isPressed && this.selectedRotationLabel != null)
		{
			this.selectedRotationLabel.AddToValue((double)(this.previousMousePos.x - Mouse.current.position.ReadValue().x));
			this.previousMousePos = Mouse.current.position.ReadValue();
		}
		else if (Mouse.current.leftButton.isPressed)
		{
			if (this.rotatingCamera && this.activeCamera != null)
			{
				Vector2 vector2 = this.previousMousePosition - Mouse.current.position.ReadValue();
				vector2.y /= (float)Screen.height;
				vector2.x /= (float)Screen.width;
				this.activeCamera.transform.position = this.activeCameraTarget.position;
				this.activeCamera.transform.Rotate(new Vector3(1f, 0f, 0f), vector2.y * 180f);
				this.activeCamera.transform.Rotate(new Vector3(0f, 1f, 0f), -vector2.x * 180f, Space.World);
				this.activeCamera.transform.Translate(new Vector3(0f, 0f, this.activeZoomLevel));
				this.previousMousePosition = Mouse.current.position.ReadValue();
			}
			else if (this.draggingMoveDot)
			{
				float timelineMousePercentage = this.GetTimelineMousePercentage();
				double num = Math.Round((double)(this.selectedMove.duration * timelineMousePercentage), 2);
				if (num != this.selectedSingleMove.executionTime)
				{
					if (this.dragKeyframesCommand == null)
					{
						List<JointMove> list = new List<JointMove>();
						foreach (JointMove item in this.selectedJointMoves)
						{
							list.Add(item);
						}
						list.Add(this.selectedSingleMove);
						this.dragKeyframesCommand = new MoveKeyframesCommand(this.selectedStance, this.selectedMove, list, this.selectedSingleMove.executionTime);
						CommandInvoker.ExecuteCommand(this.dragKeyframesCommand, true);
					}
					double difference = num - this.selectedSingleMove.executionTime;
					this.dragKeyframesCommand.AddToDifference(difference);
				}
			}
			else if (this.draggingCurrentTime)
			{
				this.CalculateCurrentTimeFromMousePosition();
			}
			else if (this.dragginGizmo)
			{
				this.CalculateGizmoChange();
			}
		}
		else if (Mouse.current.leftButton.wasReleasedThisFrame)
		{
			if (this.selectionStart != null)
			{
				this.AddAreaToSelection();
			}
			this.selectedRotationLabel = null;
			this.rotatingCamera = false;
			this.draggingMoveDot = false;
			this.HandleKeyFrameDragEnd();
			this.draggingCurrentTime = false;
			this.draggingPhysicalAxis = "";
			this.draggingAnimationAxis = "";
			this.selectionStart = null;
			this.distanceToGizmoHoldPosition = null;
		}
		this.HandleJointSelect();
		this.HandleMouseRightClick();
		if (this.focusedPhysicsCameraTarget != null && (this.focusedPhysicsCameraTarget.position - this.physicsCameraTarget.position).magnitude > 0.001f)
		{
			this.physicsCameraTarget.position = this.focusedPhysicsCameraTarget.position;
		}
		if (this.focusedAnimationCameraTarget != null && !this.usingIKRig)
		{
			this.animationCameraTarget.position = this.focusedAnimationCameraTarget.position;
		}
		this.physicsCamera.transform.position = this.physicsCameraTarget.position;
		this.physicsCamera.transform.Translate(new Vector3(0f, 0f, this.physicsZoomLevel));
		this.animationCamera.transform.position = this.animationCameraTarget.position;
		this.animationCamera.transform.Translate(new Vector3(0f, 0f, this.animationZoomLevel));
	}

	// Token: 0x06000ED3 RID: 3795 RVA: 0x0004C3DC File Offset: 0x0004A5DC
	private void PlayAnimation()
	{
		if (this.updateAnimation)
		{
			this.animator.ClearPreviewHistory();
		}
		this.animator.playingMovePreview = true;
		if (this.tempSingleMoves.Count > 0)
		{
			this.selectedMove.jointMoveList.AddRange(this.tempSingleMoves);
		}
		this.animator.PlayMove(this.selectedMove, true, true, this.timeLineSlider.value, true);
		if (this.tempSingleMoves.Count > 0)
		{
			foreach (JointMove item in this.tempSingleMoves)
			{
				this.selectedMove.jointMoveList.Remove(item);
			}
		}
		this.animator.HandleAnimation();
	}

	// Token: 0x06000ED4 RID: 3796 RVA: 0x0004C4B8 File Offset: 0x0004A6B8
	private bool HandleCameraMove()
	{
		if (Mouse.current.rightButton.isPressed && this.activeCamera != null)
		{
			Vector2 vector = Mouse.current.position.ReadValueFromPreviousFrame() - Mouse.current.position.ReadValue();
			vector.y /= (float)Screen.height;
			vector.x /= (float)Screen.width;
			if (vector.magnitude != 0f)
			{
				if (this.activeCamera.name.ToLower().Contains("physics"))
				{
					this.focusedPhysicsCameraTarget = null;
				}
				else
				{
					this.focusedAnimationCameraTarget = null;
				}
				this.activeCameraTarget.transform.Translate(new Vector3(vector.x * this.cameraMoveMultiplier, vector.y * this.cameraMoveMultiplier), this.activeCamera.transform);
			}
			return true;
		}
		if (this.userControls.MoveEditorMap.Focus_Camera.WasPressedThisFrame() && this.activeCamera != null)
		{
			if (this.usingIKRig && this.selectedRigTarget != null)
			{
				if (!this.activeCamera.name.ToLower().Contains("physics"))
				{
					this.animationCameraTarget.position = this.selectedRigTarget.GameObject.transform.position;
				}
			}
			else if (this.selectedJoint != null)
			{
				if (this.activeCamera.name.ToLower().Contains("physics"))
				{
					this.focusedPhysicsCameraTarget = this.selectedJoint.PhysicsJoint.transform;
				}
				else
				{
					this.focusedAnimationCameraTarget = this.selectedJoint.AnimationJoint.transform;
				}
			}
		}
		return false;
	}

	// Token: 0x06000ED5 RID: 3797 RVA: 0x0004C67C File Offset: 0x0004A87C
	private void HandleJointSelect()
	{
		if (!this.dragginGizmo && this.userControls.MoveEditorMap.Left_Click.WasPerformedThisFrame())
		{
			if (EventSystem.current.IsPointerOverGameObject())
			{
				return;
			}
			List<RaycastHit> list = this.RaycastPhysicsMouse();
			GameObject gameObject = null;
			foreach (RaycastHit raycastHit in list)
			{
				if (raycastHit.transform.gameObject.layer != 10)
				{
					if (raycastHit.transform.gameObject.name == "X" || raycastHit.transform.gameObject.name == "Y" || raycastHit.transform.gameObject.name == "Z")
					{
						gameObject = null;
						break;
					}
					if (this.usingIKRig)
					{
						if (raycastHit.transform.tag == "Rig")
						{
							gameObject = raycastHit.transform.gameObject;
							break;
						}
					}
					else
					{
						GameObject parentWithUpperCaseName = Generic.GetParentWithUpperCaseName(raycastHit.transform.gameObject);
						if (parentWithUpperCaseName != null && gameObject == null)
						{
							gameObject = parentWithUpperCaseName;
						}
					}
				}
			}
			if (gameObject != null && !this.dragginGizmo)
			{
				if (this.usingIKRig)
				{
					this.SetSelectedRig(gameObject);
					return;
				}
				if (Generic.GetParentWithName(gameObject, "PlayerModePhysics") != null)
				{
					this.SetSelectedJoint(gameObject, null);
					return;
				}
				this.SetSelectedJoint(null, gameObject);
			}
		}
	}

	// Token: 0x06000ED6 RID: 3798 RVA: 0x0004C818 File Offset: 0x0004AA18
	private void HandleMouseRightClick()
	{
		if (this.userControls.MoveEditorMap.Right_Click.WasPerformedThisFrame())
		{
			List<RaycastHit> list = this.RaycastPhysicsMouse();
			GameObject gameObject = null;
			foreach (RaycastHit raycastHit in list)
			{
				if (raycastHit.transform.gameObject.layer != 10)
				{
					if (this.usingIKRig)
					{
						if (raycastHit.transform.tag == "Rig")
						{
							gameObject = raycastHit.transform.gameObject;
							break;
						}
					}
					else
					{
						GameObject parentWithUpperCaseName = Generic.GetParentWithUpperCaseName(raycastHit.transform.gameObject);
						if (parentWithUpperCaseName != null && gameObject == null)
						{
							gameObject = parentWithUpperCaseName;
						}
					}
				}
			}
			if (gameObject != null && !this.dragginGizmo)
			{
				if (this.usingIKRig)
				{
					this.SetSelectedHandByRig(gameObject);
					return;
				}
				this.SetSelectedHandByMeshObject(gameObject);
			}
		}
	}

	// Token: 0x06000ED7 RID: 3799 RVA: 0x0004C918 File Offset: 0x0004AB18
	private void CalculateCurrentTimeFromMousePosition()
	{
		if (this.selectedMove != null)
		{
			float timelineMousePercentage = this.GetTimelineMousePercentage();
			this.CurrentTimeChanged(Math.Round((double)(this.selectedMove.duration * timelineMousePercentage), 2).ToString(), false);
		}
	}

	// Token: 0x1700019F RID: 415
	// (get) Token: 0x06000ED8 RID: 3800 RVA: 0x0004C957 File Offset: 0x0004AB57
	private bool dragginGizmo
	{
		get
		{
			return !string.IsNullOrEmpty(this.draggingPhysicalAxis) || !string.IsNullOrEmpty(this.draggingAnimationAxis);
		}
	}

	// Token: 0x06000ED9 RID: 3801 RVA: 0x0004C978 File Offset: 0x0004AB78
	public void CalculateGizmoChange()
	{
		if (this.usingIKRig)
		{
			GameObject gameObject = this.animationCamera;
			GameObject currentGizmoGameobject = this.animationPositionGizmo;
			Vector3 vector = default(Vector3);
			string text = this.draggingPhysicalAxis;
			if (this.rigEditMode == EditMode.Rotate)
			{
				currentGizmoGameobject = this.animationRotationGizmo;
				text = this.draggingAnimationAxis;
			}
			if (!(text == "X"))
			{
				if (!(text == "Y"))
				{
					if (text == "Z")
					{
						vector = this.selectedRigTarget.GameObject.transform.forward;
					}
				}
				else
				{
					vector = this.selectedRigTarget.GameObject.transform.up;
				}
			}
			else
			{
				vector = this.selectedRigTarget.GameObject.transform.right;
			}
			if (this.selectedRigTarget != null)
			{
				if (this.rigEditMode == EditMode.Move)
				{
					Vector3 a = default(Vector3);
					Vector3 vector2 = default(Vector3);
					Ray ray = gameObject.GetComponent<Camera>().ScreenPointToRay(Mouse.current.position.ReadValue());
					if (Generic.ClosestPointsOnTwoLines(out a, out vector2, this.selectedRigTarget.GameObject.transform.position, vector, ray.origin, ray.direction, false))
					{
						if (this.distanceToGizmoHoldPosition == null)
						{
							this.distanceToGizmoHoldPosition = new float?(Vector3.Distance(a, this.selectedRigTarget.GameObject.transform.position));
						}
						else
						{
							Vector3 localPosition = this.selectedRigTarget.GameObject.transform.localPosition;
							this.selectedRigTarget.GameObject.transform.position = a - vector * this.distanceToGizmoHoldPosition.Value;
							if (this.twoHandedRigAndHandSelected)
							{
								this.selectedRigTarget.GameObject.transform.localPosition = new Vector3(localPosition.x, this.selectedRigTarget.GameObject.transform.localPosition.y, localPosition.z);
							}
							this.UpdateGizmoPosition();
						}
					}
				}
				else
				{
					Vector3 rotationAngle = this.GetRotationAngle(gameObject, currentGizmoGameobject, text);
					this.RotateJoint(rotationAngle);
				}
			}
		}
		else
		{
			GameObject currentCameraGameobject = this.physicsCamera;
			GameObject currentGizmoGameobject2 = this.physicsRotationGizmo;
			JointMove jointMove = null;
			if (this.selectedSingleMove != null)
			{
				this.ClearSelectedSingleMoves();
				this.UpdateTimeLineMoves();
			}
			if (jointMove == null)
			{
				if (this.tempSingleMove == null)
				{
					this.CreateTempSingleMove();
				}
				jointMove = this.tempSingleMove;
			}
			string currentAxis = this.draggingPhysicalAxis;
			if (!string.IsNullOrEmpty(this.draggingAnimationAxis))
			{
				currentCameraGameobject = this.animationCamera;
				currentGizmoGameobject2 = this.animationRotationGizmo;
				currentAxis = this.draggingAnimationAxis;
			}
			Vector3 rotationAngle2 = this.GetRotationAngle(currentCameraGameobject, currentGizmoGameobject2, currentAxis);
			if (jointMove != null)
			{
				if (jointMove.targetRotation == null)
				{
					jointMove.targetRotation = new NullableVector3(null, null, null);
				}
				this.RotateJoint(rotationAngle2);
			}
		}
		this.previousMousePosition = Mouse.current.position.ReadValue();
		this.UpdateAnimation();
	}

	// Token: 0x06000EDA RID: 3802 RVA: 0x0004CC70 File Offset: 0x0004AE70
	public Vector3 GetRotationAngle(GameObject currentCameraGameobject, GameObject currentGizmoGameobject, string currentAxis)
	{
		Vector3 vector = currentCameraGameobject.GetComponent<Camera>().WorldToScreenPoint(currentGizmoGameobject.transform.position);
		vector.z = 0f;
		float num = Vector3.Angle(this.previousMousePosition, vector);
		float num2 = Vector3.Angle(Mouse.current.position.ReadValue(), vector);
		float num3 = num - num2;
		Vector2 a = new Vector2(vector.x, vector.y);
		Vector2 b = new Vector2(this.previousMousePosition.x, this.previousMousePosition.y);
		Vector2 b2 = new Vector2(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y);
		num3 = Vector2.SignedAngle(a - b, a - b2);
		Vector3 to = currentGizmoGameobject.transform.position - currentCameraGameobject.transform.position;
		Vector3 result = default(Vector3);
		if (currentAxis == "X")
		{
			if (Mathf.Abs(Vector3.Angle(currentGizmoGameobject.transform.right, to)) > 90f)
			{
				num3 *= -1f;
			}
			result = new Vector3(num3, 0f, 0f);
		}
		else if (currentAxis == "Y")
		{
			if (Mathf.Abs(Vector3.Angle(currentGizmoGameobject.transform.up, to)) > 90f)
			{
				num3 *= -1f;
			}
			result = new Vector3(0f, num3, 0f);
		}
		else if (currentAxis == "Z")
		{
			if (Mathf.Abs(Vector3.Angle(currentGizmoGameobject.transform.forward, to)) > 90f)
			{
				num3 *= -1f;
			}
			result = new Vector3(0f, 0f, num3);
		}
		return result;
	}

	// Token: 0x06000EDB RID: 3803 RVA: 0x0004CE44 File Offset: 0x0004B044
	public void UpdateGizmoPosition()
	{
		if (!this.usingIKRig)
		{
			if (this.selectedJoint != null)
			{
				this.physicsRotationGizmo.transform.position = this.selectedJoint.PhysicsJoint.transform.position;
				this.physicsRotationGizmo.transform.rotation = this.selectedJoint.PhysicsJoint.transform.rotation;
				this.animationRotationGizmo.transform.position = this.selectedJoint.AnimationJoint.transform.position;
				this.animationRotationGizmo.transform.rotation = this.selectedJoint.AnimationJoint.transform.rotation;
				return;
			}
		}
		else if (this.selectedRigTarget != null)
		{
			if (this.rigEditMode == EditMode.Rotate)
			{
				this.animationRotationGizmo.transform.position = this.selectedRigTarget.GameObject.transform.position;
				this.animationRotationGizmo.transform.rotation = this.selectedRigTarget.GameObject.transform.rotation;
				return;
			}
			this.animationPositionGizmo.transform.position = this.selectedRigTarget.GameObject.transform.position;
			this.animationPositionGizmo.transform.rotation = this.selectedRigTarget.GameObject.transform.rotation;
		}
	}

	// Token: 0x06000EDC RID: 3804 RVA: 0x0004CFA5 File Offset: 0x0004B1A5
	private void ClearSelectedGizmoTargets()
	{
		this.selectedJoint = null;
		this.selectedRigTarget = null;
		this.SetupGizmoForJoint();
	}

	// Token: 0x06000EDD RID: 3805 RVA: 0x0004CFBC File Offset: 0x0004B1BC
	public void SetupGizmoForJoint()
	{
		if (this.usingIKRig)
		{
			string axesForJoint = "xyz";
			this.SetGizmoAxisVisibility(axesForJoint, "x");
			this.SetGizmoAxisVisibility(axesForJoint, "y");
			this.SetGizmoAxisVisibility(axesForJoint, "z");
			this.physicsRotationGizmo.SetActive(false);
			this.animationRotationGizmo.SetActive(false);
			this.animationPositionGizmo.SetActive(false);
			this.CheckRigMode();
			if (this.selectedRigTarget != null && this.selectedMove != null)
			{
				if (this.rigEditMode == EditMode.Rotate && this.selectedRigTarget.GameObject)
				{
					this.animationRotationGizmo.SetActive(true);
					this.animationPositionGizmo.SetActive(false);
				}
				else
				{
					if (this.twoHandedRigAndHandSelected)
					{
						axesForJoint = "z";
						this.SetGizmoAxisVisibility(axesForJoint, "x");
						this.SetGizmoAxisVisibility(axesForJoint, "y");
						this.SetGizmoAxisVisibility(axesForJoint, "z");
					}
					this.animationRotationGizmo.SetActive(false);
					this.animationPositionGizmo.SetActive(true);
				}
			}
		}
		else
		{
			this.animationPositionGizmo.SetActive(false);
			if (this.selectedJoint != null && this.selectedMove != null)
			{
				this.physicsRotationGizmo.SetActive(true);
				this.animationRotationGizmo.SetActive(true);
				string axesForJointType = MoveClassHelpers.GetAxesForJointType(this.selectedJoint.JointType);
				this.SetGizmoAxisVisibility(axesForJointType, "x");
				this.SetGizmoAxisVisibility(axesForJointType, "y");
				this.SetGizmoAxisVisibility(axesForJointType, "z");
			}
			else
			{
				this.physicsRotationGizmo.SetActive(false);
				this.animationRotationGizmo.SetActive(false);
			}
		}
		this.UpdateSwapEditModeButton();
		this.UpdateGizmoPosition();
	}

	// Token: 0x06000EDE RID: 3806 RVA: 0x0004D154 File Offset: 0x0004B354
	public void SetGizmoAxisVisibility(string axesForJoint, string axis)
	{
		GameObject gameObject = Generic.FindChildObjectWithNameContains(this.physicsRotationGizmo.transform, axis);
		GameObject gameObject2 = Generic.FindChildObjectWithNameContains(this.animationRotationGizmo.transform, axis);
		GameObject gameObject3 = Generic.FindChildObjectWithNameContains(this.animationPositionGizmo.transform, axis);
		if (gameObject != null && gameObject2 != null && gameObject3 != null)
		{
			if (axesForJoint.ToLower().Contains(axis))
			{
				gameObject.SetActive(true);
				gameObject2.SetActive(true);
				gameObject3.SetActive(true);
				return;
			}
			gameObject.SetActive(false);
			gameObject2.SetActive(false);
			gameObject3.SetActive(false);
		}
	}

	// Token: 0x06000EDF RID: 3807 RVA: 0x0004D1EC File Offset: 0x0004B3EC
	private Vector3 GetRotationForHips(Vector3 targetCurrentRotation, Vector3 rotation)
	{
		Vector3 vector = Generic.ConvertToNegativeAndPositiveRotation(targetCurrentRotation);
		vector += rotation;
		if (vector.x > MoveSetHelpers.hipRotationMax)
		{
			rotation.x -= vector.x - MoveSetHelpers.hipRotationMax;
		}
		else if (vector.x < MoveSetHelpers.hipRotationMax * -1f)
		{
			rotation.x -= vector.x + MoveSetHelpers.hipRotationMax;
		}
		if (vector.z > MoveSetHelpers.hipRotationMax)
		{
			rotation.z -= vector.z - MoveSetHelpers.hipRotationMax;
		}
		else if (vector.z < MoveSetHelpers.hipRotationMax * -1f)
		{
			rotation.z -= vector.z + MoveSetHelpers.hipRotationMax;
		}
		return rotation;
	}

	// Token: 0x06000EE0 RID: 3808 RVA: 0x0004D2B8 File Offset: 0x0004B4B8
	public void RotateJoint(Vector3 rotation)
	{
		if (this.usingIKRig)
		{
			if (this.selectedRigTarget != null)
			{
				if (this.selectedRigTarget.Rig.jointTypeBase == JointType.HIP)
				{
					rotation = this.GetRotationForHips(this.selectedRigTarget.GameObject.transform.localEulerAngles, rotation);
				}
				this.selectedRigTarget.GameObject.transform.Rotate(rotation, Space.Self);
				if (this.selectedRigTarget.Rig.jointTypeBase == JointType.HIP)
				{
					this.selectedRigTarget.GameObject.transform.localEulerAngles = Generic.ClampRotation(this.selectedRigTarget.GameObject.transform.localEulerAngles, new float?(MoveSetHelpers.hipRotationMax), null, new float?(MoveSetHelpers.hipRotationMax));
					return;
				}
			}
		}
		else if (this.selectedJoint != null)
		{
			string axesForJointType = MoveClassHelpers.GetAxesForJointType(this.selectedJoint.JointType);
			if (axesForJointType.Length == 1 && axesForJointType == "x")
			{
				if (this.selectedSingleMove != null)
				{
					this.selectedSingleMove.targetRotation.x += rotation.x;
					this.selectedJoint.AnimationJoint.transform.localEulerAngles = this.selectedSingleMove.targetRotation.ConvertToVector3();
					this.UpdateSingleMoveEditor(this.selectedSingleMove);
					return;
				}
				if (this.tempSingleMove != null)
				{
					if (this.tempSingleMove.targetRotation.x == null)
					{
						this.tempSingleMove.targetRotation.x = new float?(this.selectedJoint.AnimationJoint.transform.localEulerAngles.x);
						if (this.selectedJoint.AnimationJoint.transform.localEulerAngles.y > 179f && this.selectedJoint.AnimationJoint.transform.localEulerAngles.z > 179f)
						{
							this.tempSingleMove.targetRotation.x = new float?(180f - this.selectedJoint.AnimationJoint.transform.localEulerAngles.x);
						}
						this.tempSingleMove.targetRotation.y = new float?(0f);
						this.tempSingleMove.targetRotation.z = new float?(0f);
					}
					this.tempSingleMove.targetRotation.x += rotation.x;
					this.selectedJoint.AnimationJoint.transform.localEulerAngles = this.tempSingleMove.targetRotation.ConvertToVector3();
					return;
				}
			}
			else
			{
				if (this.selectedJoint.JointType == JointType.HIP)
				{
					rotation = this.GetRotationForHips(this.selectedJoint.AnimationJoint.transform.localEulerAngles, rotation);
				}
				this.selectedJoint.AnimationJoint.transform.Rotate(rotation, Space.Self);
				if (this.selectedJoint.JointType == JointType.HIP)
				{
					this.selectedJoint.AnimationJoint.transform.localEulerAngles = Generic.ClampRotation(this.selectedJoint.AnimationJoint.transform.localEulerAngles, new float?(MoveSetHelpers.hipRotationMax), null, new float?(MoveSetHelpers.hipRotationMax));
				}
				if (this.selectedSingleMove != null)
				{
					if (this.selectedSingleMove.targetRotation == null)
					{
						this.selectedSingleMove.targetRotation = new NullableVector3(null, null, null);
					}
					this.selectedSingleMove.targetRotation.x = new float?(this.selectedJoint.AnimationJoint.transform.localEulerAngles.x);
					this.selectedSingleMove.targetRotation.y = new float?(this.selectedJoint.AnimationJoint.transform.localEulerAngles.y);
					this.selectedSingleMove.targetRotation.z = new float?(this.selectedJoint.AnimationJoint.transform.localEulerAngles.z);
					if (this.selectedSingleMove.targetRotation.x.Value > 180f)
					{
						this.selectedSingleMove.targetRotation.x = new float?(this.selectedSingleMove.targetRotation.x.Value - 360f);
					}
					if (this.selectedSingleMove.targetRotation.y.Value > 180f)
					{
						this.selectedSingleMove.targetRotation.y = new float?(this.selectedSingleMove.targetRotation.y.Value - 360f);
					}
					if (this.selectedSingleMove.targetRotation.z.Value > 180f)
					{
						this.selectedSingleMove.targetRotation.z = new float?(this.selectedSingleMove.targetRotation.z.Value - 360f);
					}
					this.UpdateSingleMoveEditor(this.selectedSingleMove);
					return;
				}
				this.SetTempSingleMoveRotation();
			}
		}
	}

	// Token: 0x06000EE1 RID: 3809 RVA: 0x0004D814 File Offset: 0x0004BA14
	private void SetTempSingleMoveRotation()
	{
		if (this.tempSingleMove != null)
		{
			if (this.tempSingleMove.targetRotation == null)
			{
				this.tempSingleMove.targetRotation = new NullableVector3(null, null, null);
			}
			this.tempSingleMove.targetRotation.x = new float?(this.selectedJoint.AnimationJoint.transform.localEulerAngles.x);
			this.tempSingleMove.targetRotation.y = new float?(this.selectedJoint.AnimationJoint.transform.localEulerAngles.y);
			this.tempSingleMove.targetRotation.z = new float?(this.selectedJoint.AnimationJoint.transform.localEulerAngles.z);
			float? num = this.tempSingleMove.targetRotation.x;
			float num2 = (float)180;
			if (num.GetValueOrDefault() > num2 & num != null)
			{
				this.tempSingleMove.targetRotation.x = this.tempSingleMove.targetRotation.x - (float)360;
			}
			num = this.tempSingleMove.targetRotation.y;
			num2 = (float)180;
			if (num.GetValueOrDefault() > num2 & num != null)
			{
				this.tempSingleMove.targetRotation.y = this.tempSingleMove.targetRotation.y - (float)360;
			}
			num = this.tempSingleMove.targetRotation.z;
			num2 = (float)180;
			if (num.GetValueOrDefault() > num2 & num != null)
			{
				this.tempSingleMove.targetRotation.z = this.tempSingleMove.targetRotation.z - (float)360;
			}
		}
	}

	// Token: 0x06000EE2 RID: 3810 RVA: 0x0004DA54 File Offset: 0x0004BC54
	public void UpdateSingleMoveEditor(JointMove singleMove)
	{
		SingleMoveEditorListItem singleMoveEditorListItem = (from x in this.singleMoveEditorLists
		where x.SingleMove == singleMove
		select x).FirstOrDefault<SingleMoveEditorListItem>();
		if (singleMoveEditorListItem != null)
		{
			singleMoveEditorListItem.UpdateEditor();
		}
	}

	// Token: 0x06000EE3 RID: 3811 RVA: 0x0004DA94 File Offset: 0x0004BC94
	public void UpdateAllSingleMoveEditors()
	{
		foreach (SingleMoveEditorListItem singleMoveEditorListItem in this.singleMoveEditorLists)
		{
			singleMoveEditorListItem.UpdateEditor();
		}
	}

	// Token: 0x06000EE4 RID: 3812 RVA: 0x0004DAE4 File Offset: 0x0004BCE4
	private void loadMoveSet(bool loadEquipment = false)
	{
		if (this.moveSet == null)
		{
			this.moveSet = MoveSetHelpers.CreateNewMoveSet();
		}
		this.animator.SetMoveSet(this.moveSet, false, false);
		if (this.selectedStance != null)
		{
			this.animator.SetStance(this.selectedStance, false, false, stanceChangeType.Default);
		}
		if (loadEquipment)
		{
			this.playerHealth.SetEquipment(this.moveSet.defaultEquipment, false);
		}
	}

	// Token: 0x06000EE5 RID: 3813 RVA: 0x0004DB50 File Offset: 0x0004BD50
	public void UpdateMoveMenu(bool swapTwoHandedMode = true)
	{
		if (this.latestMoveMenuUpdate == Time.frameCount)
		{
			return;
		}
		if (this.previousEditorMode != this.CurrentEditorMode)
		{
			this.latestMoveMenuUpdate = Time.frameCount;
		}
		this.previousEditorMode = this.CurrentEditorMode;
		this.DisplayTimeLineInputs(false);
		foreach (object obj in this.moveSelectPanelList.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.GetComponent<MoveEditor>() != null || transform.GetComponent<ChangeStanceEditor>() != null)
			{
				UnityEngine.Object.Destroy(transform.gameObject);
			}
		}
		foreach (object obj2 in this.stanceSelectPanelList.transform)
		{
			Transform transform2 = (Transform)obj2;
			if (transform2.GetComponent<StanceEditor>() != null)
			{
				UnityEngine.Object.Destroy(transform2.gameObject);
			}
		}
		if (this.CurrentEditorMode == EditorMode.Stance)
		{
			this.SwapRig(new bool?(false), true);
			this.stanceSelectPanel.SetActive(true);
			this.moveSelectPanel.SetActive(false);
			this.moveEditorPanel.SetActive(false);
			int num = 0;
			using (IEnumerator<Stance> enumerator2 = (from x in this.moveSet.stanceList
			orderby !x.isDefault, x.name
			select x).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					MoveSetEditor.<>c__DisplayClass139_0 CS$<>8__locals1 = new MoveSetEditor.<>c__DisplayClass139_0();
					CS$<>8__locals1.<>4__this = this;
					CS$<>8__locals1.stance = enumerator2.Current;
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.stanceEditorPrefab);
					StanceEditor stanceEditor = gameObject.GetComponent<StanceEditor>();
					gameObject.GetComponent<RectTransform>();
					stanceEditor.nameInputField.text = CS$<>8__locals1.stance.name;
					stanceEditor.defaultToggle.isOn = CS$<>8__locals1.stance.isDefault;
					stanceEditor.defaultToggle.onValueChanged.AddListener(delegate(bool <p0>)
					{
						CS$<>8__locals1.<>4__this.OnStanceDefaultChanged(CS$<>8__locals1.stance, stanceEditor.defaultToggle);
					});
					stanceEditor.editButton.onClick.AddListener(delegate()
					{
						CS$<>8__locals1.<>4__this.SelectStance(CS$<>8__locals1.stance);
					});
					gameObject.transform.SetParent(this.stanceSelectPanelList.transform, false);
					gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
					num++;
					gameObject.transform.SetSiblingIndex(num);
					stanceEditor.nameInputField.onEndEdit.AddListener(delegate(string <p0>)
					{
						CS$<>8__locals1.<>4__this.OnStanceNameChanged(CS$<>8__locals1.stance, stanceEditor.nameInputField);
					});
					stanceEditor.deleteButton.onClick.AddListener(delegate()
					{
						CS$<>8__locals1.<>4__this.DeleteStance(CS$<>8__locals1.stance);
					});
					stanceEditor.copyButton.onClick.AddListener(delegate()
					{
						CS$<>8__locals1.<>4__this.CopyStance(CS$<>8__locals1.stance);
					});
				}
				goto IL_1109;
			}
		}
		if (this.CurrentEditorMode == EditorMode.Move)
		{
			this.SwapRig(new bool?(false), true);
			this.moveSelectPanel.SetActive(true);
			this.moveEditorPanel.SetActive(false);
			this.stanceSelectPanel.SetActive(false);
			int num2 = 0;
			using (IEnumerator<Move> enumerator3 = (from x in this.selectedStance.moveList
			where !x.stanceChange
			orderby x.inputType != inputType.Passive, x.inputType != inputType.PlayAtStart, x.name
			select x).GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					MoveSetEditor.<>c__DisplayClass139_2 CS$<>8__locals3 = new MoveSetEditor.<>c__DisplayClass139_2();
					CS$<>8__locals3.<>4__this = this;
					CS$<>8__locals3.move = enumerator3.Current;
					GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.moveEditorPrefab);
					MoveEditor moveEditor = gameObject2.GetComponent<MoveEditor>();
					gameObject2.GetComponent<RectTransform>();
					moveEditor.title.text = CS$<>8__locals3.move.name;
					moveEditor.FillOptions();
					moveEditor.editButton.onClick.AddListener(delegate()
					{
						CS$<>8__locals3.<>4__this.SelectMove(CS$<>8__locals3.<>4__this.selectedStance.moveList.IndexOf(CS$<>8__locals3.move));
					});
					gameObject2.transform.SetParent(this.moveSelectPanelList.transform, false);
					gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
					num2++;
					gameObject2.transform.SetSiblingIndex(num2);
					moveEditor.selectInputDropdown.options = new List<Dropdown.OptionData>();
					moveEditor.selectInputDropdown.options.Add(new Dropdown.OptionData(""));
					foreach (InputAction inputAction in (from x in this.userControlsMap.asset.actionMaps
					where x.name == "PlayerActionMap"
					select x).FirstOrDefault<InputActionMap>().actions)
					{
						if (!inputAction.name.ToLower().Contains("turn") && !inputAction.name.ToLower().Contains("directional"))
						{
							moveEditor.selectInputDropdown.options.Add(new OptionDataWithValue
							{
								stringValue = inputAction.name,
								text = LocalizationHelpers.GetLocalizedTextForInputAction(inputAction.name, false)
							});
						}
					}
					if (!string.IsNullOrEmpty(CS$<>8__locals3.move.playerInput))
					{
						int value = moveEditor.selectInputDropdown.options.IndexOf((from x in moveEditor.selectInputDropdown.options
						where x.GetStringValue() == CS$<>8__locals3.move.playerInput
						select x).FirstOrDefault<Dropdown.OptionData>());
						moveEditor.selectInputDropdown.value = value;
						moveEditor.selectInputDropdown.captionText.text = LocalizationHelpers.GetLocalizedTextForInputAction(CS$<>8__locals3.move.playerInput, false);
					}
					moveEditor.selectInputDropdown.onValueChanged.AddListener(delegate(int <p0>)
					{
						CS$<>8__locals3.<>4__this.OnPlayerInputChanged(CS$<>8__locals3.move, moveEditor.selectInputDropdown);
					});
					moveEditor.playButton.gameObject.AddComponent<PlayMove>().move = CS$<>8__locals3.move;
					moveEditor.playButton.GetComponent<Button>();
					moveEditor.deleteButton.onClick.AddListener(delegate()
					{
						CS$<>8__locals3.<>4__this.DeleteMove(CS$<>8__locals3.<>4__this.selectedStance, CS$<>8__locals3.move);
					});
					moveEditor.copyButton.onClick.AddListener(delegate()
					{
						CS$<>8__locals3.<>4__this.CopyMove(CS$<>8__locals3.move);
					});
					moveEditor.listenInputButton.onClick.AddListener(delegate()
					{
						CS$<>8__locals3.<>4__this.ListenInputMove(CS$<>8__locals3.move, moveEditor.selectInputDropdown);
					});
					if (CS$<>8__locals3.move.inputType == inputType.Passive || CS$<>8__locals3.move.inputType == inputType.PlayAtStart)
					{
						moveEditor.playButton.gameObject.SetActive(false);
						moveEditor.listenInputButton.gameObject.SetActive(false);
						moveEditor.selectInputDropdown.gameObject.SetActive(false);
					}
				}
			}
			using (IEnumerator<Move> enumerator3 = (from x in this.selectedStance.moveList
			where x.stanceChange
			orderby x.name
			select x).GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					MoveSetEditor.<>c__DisplayClass139_4 CS$<>8__locals5 = new MoveSetEditor.<>c__DisplayClass139_4();
					CS$<>8__locals5.<>4__this = this;
					CS$<>8__locals5.move = enumerator3.Current;
					GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(this.changeStanceEditorPrefab);
					ChangeStanceEditor moveEditor = gameObject3.GetComponent<ChangeStanceEditor>();
					gameObject3.GetComponent<RectTransform>();
					gameObject3.transform.SetParent(this.moveSelectPanelList.transform, false);
					gameObject3.transform.localScale = new Vector3(1f, 1f, 1f);
					num2++;
					gameObject3.transform.SetSiblingIndex(num2);
					moveEditor.selectInputDropdown.options = new List<Dropdown.OptionData>();
					moveEditor.selectInputDropdown.options.Add(new Dropdown.OptionData(""));
					foreach (InputAction inputAction2 in (from x in this.userControlsMap.asset.actionMaps
					where x.name == "PlayerActionMap"
					select x).FirstOrDefault<InputActionMap>().actions)
					{
						if (!inputAction2.name.ToLower().Contains("turn") && !inputAction2.name.ToLower().Contains("directional"))
						{
							moveEditor.selectInputDropdown.options.Add(new OptionDataWithValue
							{
								stringValue = inputAction2.name,
								text = LocalizationHelpers.GetLocalizedTextForInputAction(inputAction2.name, false)
							});
						}
					}
					if (!string.IsNullOrEmpty(CS$<>8__locals5.move.playerInput))
					{
						int value2 = moveEditor.selectInputDropdown.options.IndexOf((from x in moveEditor.selectInputDropdown.options
						where x.GetStringValue() == CS$<>8__locals5.move.playerInput
						select x).FirstOrDefault<Dropdown.OptionData>());
						moveEditor.selectInputDropdown.value = value2;
						moveEditor.selectInputDropdown.captionText.text = LocalizationHelpers.GetLocalizedTextForInputAction(CS$<>8__locals5.move.playerInput, false);
					}
					moveEditor.selectInputDropdown.onValueChanged.AddListener(delegate(int <p0>)
					{
						CS$<>8__locals5.<>4__this.OnPlayerInputChanged(CS$<>8__locals5.move, moveEditor.selectInputDropdown);
					});
					moveEditor.selectStanceDropdown.options = new List<Dropdown.OptionData>();
					moveEditor.selectStanceDropdown.options.Add(new Dropdown.OptionData(LocalizationSettings.StringDatabase.GetLocalizedString(SettingsHelper.localizationTableName, "move_editor_back_to_previous", null, FallbackBehavior.UseProjectSettings, Array.Empty<object>())));
					foreach (Stance stance in this.potentialStanceChanges)
					{
						moveEditor.selectStanceDropdown.options.Add(new Dropdown.OptionData(stance.name));
					}
					if (!string.IsNullOrEmpty(CS$<>8__locals5.move.stanceGuid))
					{
						Stance item = (from x in this.potentialStanceChanges
						where x.guid == CS$<>8__locals5.move.stanceGuid
						select x).FirstOrDefault<Stance>();
						int value3 = this.potentialStanceChanges.IndexOf(item) + 1;
						moveEditor.selectStanceDropdown.value = value3;
					}
					moveEditor.selectStanceDropdown.onValueChanged.AddListener(delegate(int <p0>)
					{
						CS$<>8__locals5.<>4__this.OnPlayerStanceChanged(CS$<>8__locals5.move, moveEditor.selectStanceDropdown);
					});
					moveEditor.selectInputTypeDropdown.options.Clear();
					foreach (object obj3 in Enum.GetValues(typeof(inputType)))
					{
						inputType inputType = (inputType)obj3;
						if (inputType == inputType.OnClick || (inputType == inputType.HoldDown && !string.IsNullOrEmpty(CS$<>8__locals5.move.stanceGuid) && CS$<>8__locals5.move.stanceChangeType != stanceChangeType.Replace))
						{
							moveEditor.selectInputTypeDropdown.options.Add(new OptionDataWithValue
							{
								text = inputType.GetDescription(),
								stringValue = inputType.ToString()
							});
						}
					}
					Dropdown.OptionData optionData = (from x in moveEditor.selectInputTypeDropdown.options
					where x.GetStringValue() == CS$<>8__locals5.move.inputType.ToString()
					select x).FirstOrDefault<Dropdown.OptionData>();
					if (optionData != null)
					{
						int value4 = moveEditor.selectInputTypeDropdown.options.IndexOf(optionData);
						moveEditor.selectInputTypeDropdown.value = value4;
					}
					moveEditor.selectInputTypeDropdown.captionText.text = CS$<>8__locals5.move.inputType.GetDescription();
					moveEditor.selectInputTypeDropdown.RefreshShownValue();
					moveEditor.selectInputTypeDropdown.onValueChanged.AddListener(delegate(int <p0>)
					{
						CS$<>8__locals5.<>4__this.OnInputTypeChanged(CS$<>8__locals5.move, moveEditor.selectInputTypeDropdown);
					});
					moveEditor.stanceChangeTypeDropdown.options.Clear();
					foreach (object obj4 in Enum.GetValues(typeof(stanceChangeType)))
					{
						stanceChangeType stanceChangeType = (stanceChangeType)obj4;
						moveEditor.stanceChangeTypeDropdown.options.Add(new OptionDataWithValue
						{
							text = stanceChangeType.GetDescription(),
							stringValue = stanceChangeType.ToString()
						});
					}
					Dropdown.OptionData optionData2 = (from x in moveEditor.stanceChangeTypeDropdown.options
					where x.GetStringValue() == CS$<>8__locals5.move.stanceChangeType.ToString()
					select x).FirstOrDefault<Dropdown.OptionData>();
					if (optionData2 != null)
					{
						int value5 = moveEditor.stanceChangeTypeDropdown.options.IndexOf(optionData2);
						moveEditor.stanceChangeTypeDropdown.value = value5;
					}
					moveEditor.stanceChangeTypeDropdown.captionText.text = CS$<>8__locals5.move.stanceChangeType.GetDescription();
					moveEditor.stanceChangeTypeDropdown.RefreshShownValue();
					moveEditor.stanceChangeTypeDropdown.onValueChanged.AddListener(delegate(int <p0>)
					{
						CS$<>8__locals5.<>4__this.OnStanceChangeTypeChanged(CS$<>8__locals5.move, moveEditor.stanceChangeTypeDropdown);
					});
					moveEditor.deleteButton.onClick.AddListener(delegate()
					{
						CS$<>8__locals5.<>4__this.DeleteMove(CS$<>8__locals5.<>4__this.selectedStance, CS$<>8__locals5.move);
					});
					moveEditor.listenInputButton.onClick.AddListener(delegate()
					{
						CS$<>8__locals5.<>4__this.ListenInputMove(CS$<>8__locals5.move, moveEditor.selectInputDropdown);
					});
				}
			}
			this.UpdateAnimation();
		}
		else if (this.CurrentEditorMode == EditorMode.SingleMove)
		{
			this.SwapRig(new bool?(this.usingIKRig), swapTwoHandedMode);
			this.DisplayTimeLineInputs(true);
			this.UpdateDurationUI();
			this.singleMoveEditorLists = new List<SingleMoveEditorListItem>();
			this.moveSelectPanel.SetActive(false);
			this.moveEditorPanel.SetActive(true);
			this.stanceSelectPanel.SetActive(false);
			this.selectedMoveNameEditor.text = this.selectedMove.name;
			this.selectedMoveLayerEditor.options.Clear();
			for (int i = 0; i <= 10; i++)
			{
				this.selectedMoveLayerEditor.options.Add(new OptionDataWithValue
				{
					text = i.ToString(),
					stringValue = i.ToString()
				});
			}
			this.UpdateLayerShownValue();
			this.selectedMoveInputTypeEditor.options.Clear();
			this.MoveDurationField.text = this.selectedMove.duration.ToString();
			foreach (object obj5 in Enum.GetValues(typeof(inputType)))
			{
				inputType inputType2 = (inputType)obj5;
				this.selectedMoveInputTypeEditor.options.Add(new OptionDataWithValue
				{
					text = inputType2.GetDescription(),
					stringValue = inputType2.ToString()
				});
			}
			this.UpdateInputTypeShownValue();
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.singleMoveListPanel.GetComponent<RectTransform>());
		}
		IL_1109:
		this.UpdateTimeLineMoves();
		this.SetupGizmoForJoint();
		this.PrepareSelectedMoveForUse();
	}

	// Token: 0x06000EE6 RID: 3814 RVA: 0x0004ED84 File Offset: 0x0004CF84
	private void DisplayTimeLineInputs(bool displayItems)
	{
		this.timeLineInputHolder.SetActive(displayItems);
	}

	// Token: 0x06000EE7 RID: 3815 RVA: 0x0004ED92 File Offset: 0x0004CF92
	private void SelectMove(int moveIndex)
	{
		this.selectedMove = this.selectedStance.moveList[moveIndex];
		this.UpdateMoveMenu(true);
		this.CurrentTimeChanged("0", true);
	}

	// Token: 0x06000EE8 RID: 3816 RVA: 0x0004EDBE File Offset: 0x0004CFBE
	private void SelectStance(Stance stance)
	{
		this.selectedStance = stance;
		this.animator.SetStance(this.selectedStance, false, false, stanceChangeType.Default);
		this.UpdateMoveMenu(true);
	}

	// Token: 0x06000EE9 RID: 3817 RVA: 0x0004EDE4 File Offset: 0x0004CFE4
	public void BackButtonPress()
	{
		if (GeneralManager.AllowBackNavigation(null))
		{
			this.ClearSelectedGizmoTargets();
			this.tempSingleMoves = new List<JointMove>();
			this.tempSingleMove = null;
			if (this.listenActionForMove != null)
			{
				this.CancelListenActionForMove();
				return;
			}
			if (this.cameraEquipment.gameObject.activeInHierarchy)
			{
				this.MoveSetButtonClicked();
				return;
			}
			if (this.selectedMove != null)
			{
				this.selectedMove = null;
				this.ResetStance();
				this.UpdateMoveMenu(true);
				return;
			}
			if (this.selectedStance != null)
			{
				this.selectedStance = null;
				this.ResetStance();
				this.UpdateMoveMenu(true);
				return;
			}
			this.LeaveConfirm();
		}
	}

	// Token: 0x06000EEA RID: 3818 RVA: 0x0004EE7C File Offset: 0x0004D07C
	private void LeaveConfirm()
	{
		BasicConfirmDialog basicConfirmDialog = GeneralManager.CreateConfirmDialog(LocalizationHelpers.LocalizedText("confirm_txt_leave_move_editor", Array.Empty<object>()), null, false);
		if (basicConfirmDialog != null)
		{
			basicConfirmDialog.okButton.onClick.AddListener(new UnityAction(this.LeaveMoveEditor));
		}
	}

	// Token: 0x06000EEB RID: 3819 RVA: 0x0004EEC5 File Offset: 0x0004D0C5
	private void LeaveMoveEditor()
	{
		SceneManager.LoadScene("LobbyMoveEditor");
	}

	// Token: 0x06000EEC RID: 3820 RVA: 0x0004EED4 File Offset: 0x0004D0D4
	public void ResetStance()
	{
		if (this.selectedStance != null)
		{
			this.animator.SetStance(this.selectedStance, false, true, stanceChangeType.Default);
			return;
		}
		Stance stance = (from x in this.moveSet.stanceList
		where x.isDefault
		select x).FirstOrDefault<Stance>();
		if (stance != null)
		{
			this.animator.SetStance(stance, false, true, stanceChangeType.Default);
		}
	}

	// Token: 0x06000EED RID: 3821 RVA: 0x0004EF48 File Offset: 0x0004D148
	public void SaveButtonPress()
	{
		if (this.ValidateMoveSet())
		{
			this.SaveMoveSet(false);
			return;
		}
		BasicConfirmDialog component = UnityEngine.Object.Instantiate<GameObject>(this.confirmDialogPrefab).GetComponent<BasicConfirmDialog>();
		component.SetText(this.validationInfo, LocalizationHelpers.LocalizedText("confirm_title_moveset_save", Array.Empty<object>()), false);
		component.okButton.onClick.AddListener(new UnityAction(this.CleanMoveSet));
		component.cancelButton.Select();
	}

	// Token: 0x06000EEE RID: 3822 RVA: 0x0004EFB8 File Offset: 0x0004D1B8
	public void TestButtonPress()
	{
		List<LobbyPlayer> list = new List<LobbyPlayer>();
		MoveSet moveSet = this.moveSet;
		if (IGameSettingsManager.singleton != null && IGameSettingsManager.singleton.GameType == GameTypes.Classic && moveSet != null)
		{
			moveSet = MoveSetHelpers.ConvertMoveSetToClassic(moveSet);
		}
		LobbyPlayer item = new LobbyPlayer
		{
			selectedMoveSet = moveSet,
			selectedEquipment = this.moveSet.defaultEquipment,
			playerExists = true
		};
		list.Add(item);
		SceneManagerWithParameters.LoadScene("MoveEditorTestMoveSet", new Dictionary<string, object>
		{
			{
				"lobbyPlayers",
				list.Cast<LobbyPlayer>().ToList<LobbyPlayer>()
			},
			{
				"DoLocalMapInit",
				true
			},
			{
				"MoveSet",
				this.moveSet
			},
			{
				"SelectedStance",
				this.selectedStance
			},
			{
				"SelectedMove",
				this.selectedMove
			}
		}, false, false);
	}

	// Token: 0x06000EEF RID: 3823 RVA: 0x0004F088 File Offset: 0x0004D288
	public void ResetButtonPress()
	{
		SceneManagerWithParameters.LoadScene("MoveEditor", new Dictionary<string, object>
		{
			{
				"MoveSet",
				this.moveSet
			},
			{
				"SelectedStance",
				this.selectedStance
			},
			{
				"SelectedMove",
				this.selectedMove
			}
		}, false, false);
	}

	// Token: 0x06000EF0 RID: 3824 RVA: 0x0004F0DC File Offset: 0x0004D2DC
	private void SaveMoveSet(bool force = false)
	{
		if (!force && MoveSetHelpers.FileExists(this.moveSet))
		{
			this.ConfirmOverwriteSave();
			return;
		}
		try
		{
			this.moveSet.fileName = MoveSetHelpers.SaveMoveSetJson(this.moveSet);
			this.loadMoveSet(false);
			UnityEngine.Object.Instantiate<GameObject>(this.infoDialogPrefab).GetComponent<BasicInfoDialog>().SetText(LocalizationHelpers.LocalizedText("txt_saved", Array.Empty<object>()), 1f, false);
		}
		catch (Exception message)
		{
			Debug.LogError(message);
			if (Generic.CreateErrorBackupForMovesetClass(this.moveSet))
			{
				GeneralManager.CreateConfirmDialog(LocalizationHelpers.LocalizedText("alert_backup_file_created", Array.Empty<object>()), LocalizationHelpers.LocalizedText("txt_save_failed", Array.Empty<object>()), true);
			}
			else
			{
				GeneralManager.CreateConfirmDialog(LocalizationHelpers.LocalizedText("alert_backup_file_fail", Array.Empty<object>()), LocalizationHelpers.LocalizedText("txt_save_failed", Array.Empty<object>()), true);
			}
		}
	}

	// Token: 0x06000EF1 RID: 3825 RVA: 0x0004F1BC File Offset: 0x0004D3BC
	private void ConfirmOverwriteSave()
	{
		BasicConfirmDialog component = UnityEngine.Object.Instantiate<GameObject>(this.confirmDialogPrefab).GetComponent<BasicConfirmDialog>();
		component.SetText(LocalizationHelpers.LocalizedText("confirm_title_moveset_save_overwrite", Array.Empty<object>()), null, false);
		component.okButton.onClick.AddListener(delegate()
		{
			this.SaveMoveSet(true);
		});
		component.cancelButton.Select();
	}

	// Token: 0x06000EF2 RID: 3826 RVA: 0x0004F218 File Offset: 0x0004D418
	private bool ValidateMoveSet()
	{
		this.validationInfo = "";
		bool result = true;
		List<string> list = new List<string>();
		if (this.moveSet != null)
		{
			foreach (Stance stance in this.moveSet.stanceList)
			{
				using (List<Move>.Enumerator enumerator2 = stance.moveList.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Move move = enumerator2.Current;
						if (move.jointMoveList != null && move.jointMoveList.Count > 0)
						{
							List<JointMove> list2 = (from x in move.jointMoveList
							where !Enum.IsDefined(typeof(JointType), x.joint)
							select x).ToList<JointMove>();
							if (list2.Count > 0)
							{
								foreach (JointMove item in list2)
								{
									move.jointMoveList.Remove(item);
								}
							}
							if ((from x in move.jointMoveList
							where Generic.DoubleIsGreaterThan(0.0, x.executionTime) || Generic.DoubleIsGreaterThan(x.executionTime, (double)move.duration)
							select x).FirstOrDefault<JointMove>() != null)
							{
								result = false;
								list.Add(move.name);
							}
						}
					}
				}
			}
		}
		if (list.Count > 0)
		{
			this.validationInfo = LocalizationHelpers.LocalizedText("confirm_txt_validation_moveset_save", Array.Empty<object>()) + " ";
			this.validationInfo += string.Join(",", list);
		}
		return result;
	}

	// Token: 0x06000EF3 RID: 3827 RVA: 0x0004F434 File Offset: 0x0004D634
	private void CleanMoveSet()
	{
		if (this.moveSet != null)
		{
			foreach (Stance stance in this.moveSet.stanceList)
			{
				using (List<Move>.Enumerator enumerator2 = stance.moveList.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Move move = enumerator2.Current;
						if (move.jointMoveList != null)
						{
							foreach (JointMove item in (from x in move.jointMoveList
							where Generic.DoubleIsGreaterThan(0.0, x.executionTime) || Generic.DoubleIsGreaterThan(x.executionTime, (double)move.duration)
							select x).ToList<JointMove>())
							{
								move.jointMoveList.Remove(item);
							}
						}
					}
				}
			}
		}
		this.SaveMoveSet(false);
	}

	// Token: 0x06000EF4 RID: 3828 RVA: 0x0004F55C File Offset: 0x0004D75C
	public void MirrorMoveSet()
	{
		if (this.moveSet != null)
		{
			CommandInvoker.ExecuteCommand(new MirrorAllCommand(this.moveSet), false);
		}
	}

	// Token: 0x06000EF5 RID: 3829 RVA: 0x0004F577 File Offset: 0x0004D777
	public void MirrorStanceButtonClicked()
	{
		if (this.selectedStance != null)
		{
			this.MirrorStance(this.selectedStance);
		}
	}

	// Token: 0x06000EF6 RID: 3830 RVA: 0x0004F58D File Offset: 0x0004D78D
	public void MirrorMoveButtonClicked()
	{
		if (this.selectedMove != null)
		{
			CommandInvoker.ExecuteCommand(new MirrorMoveCommand(this.selectedStance, this.selectedMove), false);
		}
	}

	// Token: 0x06000EF7 RID: 3831 RVA: 0x0004F5AE File Offset: 0x0004D7AE
	public void MirrorStance(Stance stance)
	{
		if (stance != null)
		{
			CommandInvoker.ExecuteCommand(new MirrorStanceCommand(stance), false);
		}
	}

	// Token: 0x06000EF8 RID: 3832 RVA: 0x0004F5C0 File Offset: 0x0004D7C0
	public void MirrorMove(Move move)
	{
		if (move.jointMoveList != null && move.jointMoveList.Count > 0)
		{
			foreach (JointMove jointMove in move.jointMoveList)
			{
				if (jointMove.joint.ToString().Contains("LEFT"))
				{
					jointMove.joint = (JointType)Enum.Parse(typeof(JointType), jointMove.joint.ToString().Replace("LEFT", "RIGHT"));
				}
				else if (jointMove.joint.ToString().Contains("RIGHT"))
				{
					jointMove.joint = (JointType)Enum.Parse(typeof(JointType), jointMove.joint.ToString().Replace("RIGHT", "LEFT"));
				}
				jointMove.targetRotation.y = jointMove.targetRotation.y * (float)-1;
				jointMove.targetRotation.z = jointMove.targetRotation.z * (float)-1;
			}
		}
	}

	// Token: 0x06000EF9 RID: 3833 RVA: 0x0004F774 File Offset: 0x0004D974
	public void OnStanceNameChanged(Stance stance, InputField input)
	{
		CommandInvoker.ExecuteCommand(new StanceNameChangeCommand(this.moveSet, stance, input.text), false);
	}

	// Token: 0x06000EFA RID: 3834 RVA: 0x0004F78E File Offset: 0x0004D98E
	public void OnMoveSetNameChanged(InputField input)
	{
		CommandInvoker.ExecuteCommand(new MoveSetNameChangeCommand(this.moveSet, input.text), false);
	}

	// Token: 0x06000EFB RID: 3835 RVA: 0x0004F7A7 File Offset: 0x0004D9A7
	public void OnStanceDefaultChanged(Stance changedStance, Toggle input)
	{
		CommandInvoker.ExecuteCommand(new DefaultStanceChangeCommand(this.moveSet, changedStance, input.isOn), false);
	}

	// Token: 0x06000EFC RID: 3836 RVA: 0x0004F7C4 File Offset: 0x0004D9C4
	public void OnPlayerInputChanged(Move move, Dropdown dp)
	{
		string playerInput = null;
		if (dp.value != 0)
		{
			playerInput = dp.options[dp.value].GetStringValue();
		}
		CommandInvoker.ExecuteCommand(new ChangeMoveActionCommand(this.selectedStance, move, playerInput), false);
	}

	// Token: 0x06000EFD RID: 3837 RVA: 0x0004F808 File Offset: 0x0004DA08
	public void OnPlayerStanceChanged(Move move, Dropdown dp)
	{
		string stanceGuid = null;
		Stance stance = this.potentialStanceChanges.ElementAtOrDefault(dp.value - 1);
		if (stance != null)
		{
			stanceGuid = stance.guid;
		}
		CommandInvoker.ExecuteCommand(new ChangeTargetStanceForStanceChangeCommand(this.selectedStance, move, stanceGuid), false);
	}

	// Token: 0x06000EFE RID: 3838 RVA: 0x0004F848 File Offset: 0x0004DA48
	public void OnJointChanged(SingleMoveEditor singleMoveEditor, Dropdown dp)
	{
		if (dp.value != 0 && singleMoveEditor.move != null)
		{
			singleMoveEditor.move.joint = (JointType)Enum.Parse(typeof(JointType), dp.options[dp.value].GetStringValue(), true);
		}
		singleMoveEditor.move.handState = null;
	}

	// Token: 0x06000EFF RID: 3839 RVA: 0x0004F8B0 File Offset: 0x0004DAB0
	public void OnHandActionChanged(JointMove singleMove, Dropdown dp)
	{
		if (dp.value != 0)
		{
			singleMove.handState = new HandState?((HandState)Enum.Parse(typeof(HandState), dp.options[dp.value].GetStringValue(), true));
			return;
		}
		singleMove.handState = null;
	}

	// Token: 0x06000F00 RID: 3840 RVA: 0x0004F90C File Offset: 0x0004DB0C
	public void OnExecutionTimeChanged(JointMove singleMove, SingleMoveEditor singleMoveEditor)
	{
		float value;
		string text;
		if (Generic.ConvertToRoundedFloat(singleMoveEditor.executionTime.text, out value, out text))
		{
			singleMove.executionTime = Convert.ToDouble(value);
			singleMoveEditor.executionTime.text = text;
			this.UpdateTimeLineMoves();
		}
	}

	// Token: 0x06000F01 RID: 3841 RVA: 0x0004F950 File Offset: 0x0004DB50
	public void OnDurationTimeChanged(JointMove singleMove, string text)
	{
		double num;
		double.TryParse(text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out num);
	}

	// Token: 0x06000F02 RID: 3842 RVA: 0x0004F97C File Offset: 0x0004DB7C
	public void OnXChanged(JointMove singleMove, string text)
	{
		float value;
		float.TryParse(text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out value);
		if (!string.IsNullOrEmpty(text))
		{
			singleMove.targetRotation.x = new float?(value);
		}
	}

	// Token: 0x06000F03 RID: 3843 RVA: 0x0004F9C0 File Offset: 0x0004DBC0
	public void OnYChanged(JointMove singleMove, string text)
	{
		float value;
		float.TryParse(text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out value);
		if (!string.IsNullOrEmpty(text))
		{
			singleMove.targetRotation.y = new float?(value);
		}
	}

	// Token: 0x06000F04 RID: 3844 RVA: 0x0004FA04 File Offset: 0x0004DC04
	public void OnZChanged(JointMove singleMove, string text)
	{
		float value;
		float.TryParse(text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out value);
		if (!string.IsNullOrEmpty(text))
		{
			singleMove.targetRotation.z = new float?(value);
		}
	}

	// Token: 0x06000F05 RID: 3845 RVA: 0x0004FA46 File Offset: 0x0004DC46
	public void OnNameChanged(Move move, string text)
	{
		CommandInvoker.ExecuteCommand(new MoveNameChangeCommand(this.selectedStance, move, text), false);
	}

	// Token: 0x06000F06 RID: 3846 RVA: 0x0004FA5C File Offset: 0x0004DC5C
	public void UpdateLayerShownValue()
	{
		if (this.selectedMove != null)
		{
			this.selectedMoveLayerEditor.value = this.selectedMove.layer;
			this.selectedMoveLayerEditor.captionText.text = this.selectedMove.inputType.GetDescription();
			this.selectedMoveLayerEditor.RefreshShownValue();
		}
	}

	// Token: 0x06000F07 RID: 3847 RVA: 0x0004FAB8 File Offset: 0x0004DCB8
	public void OnLayerChanged(Move move, Dropdown dp)
	{
		try
		{
			if (move.layer != dp.value)
			{
				CommandInvoker.ExecuteCommand(new MoveLayerChangeCommand(this.selectedStance, move, dp.value), false);
			}
		}
		catch
		{
		}
	}

	// Token: 0x06000F08 RID: 3848 RVA: 0x0004FB00 File Offset: 0x0004DD00
	public void UpdateInputTypeShownValue()
	{
		if (this.selectedMove != null)
		{
			int value = this.selectedMoveInputTypeEditor.options.IndexOf((from x in this.selectedMoveInputTypeEditor.options
			where x.GetStringValue() == this.selectedMove.inputType.ToString()
			select x).FirstOrDefault<Dropdown.OptionData>());
			this.selectedMoveInputTypeEditor.value = value;
			this.selectedMoveInputTypeEditor.captionText.text = this.selectedMove.inputType.GetDescription();
			this.selectedMoveInputTypeEditor.RefreshShownValue();
		}
	}

	// Token: 0x06000F09 RID: 3849 RVA: 0x0004FB84 File Offset: 0x0004DD84
	public void OnInputTypeChanged(Move move, Dropdown dp)
	{
		inputType inputType = (inputType)Enum.Parse(typeof(inputType), dp.options[dp.value].GetStringValue(), true);
		if (inputType != move.inputType)
		{
			List<JointMove> list = new List<JointMove>();
			if (inputType == inputType.Passive)
			{
				foreach (FighterJoint fighterJoint in this.animator.FighterJoints)
				{
					if (!MoveSetHelpers.MoveForJointExistsInList(move, fighterJoint, null))
					{
						JointMove item = new JointMove
						{
							joint = fighterJoint.jointType,
							targetRotation = new NullableVector3(new float?(fighterJoint.joint.transform.localEulerAngles.x), new float?(fighterJoint.joint.transform.localEulerAngles.y), new float?(fighterJoint.joint.transform.localEulerAngles.z))
						};
						list.Add(item);
					}
				}
			}
			CommandInvoker.ExecuteCommand(new MoveInputTypeChangeCommand(this.selectedStance, move, inputType, list), false);
		}
	}

	// Token: 0x06000F0A RID: 3850 RVA: 0x0004FCBC File Offset: 0x0004DEBC
	public void OnStanceChangeTypeChanged(Move move, Dropdown dp)
	{
		stanceChangeType stanceChangeType = (stanceChangeType)Enum.Parse(typeof(stanceChangeType), dp.options[dp.value].GetStringValue(), true);
		if (stanceChangeType != move.stanceChangeType)
		{
			CommandInvoker.ExecuteCommand(new MoveStanceChangeTypeChangeCommand(this.selectedStance, move, stanceChangeType), false);
		}
	}

	// Token: 0x06000F0B RID: 3851 RVA: 0x0004FD14 File Offset: 0x0004DF14
	public void AddSingleMove()
	{
		if (this.selectedMove != null)
		{
			if (this.selectedMove.jointMoveList == null)
			{
				this.selectedMove.jointMoveList = new List<JointMove>();
			}
			this.selectedMove.jointMoveList.Add(new JointMove
			{
				targetRotation = new NullableVector3(null, null, null),
				notSaved = true
			});
			this.UpdateMoveMenu(true);
		}
	}

	// Token: 0x06000F0C RID: 3852 RVA: 0x0004FD8F File Offset: 0x0004DF8F
	public void RemoveSingleMove(JointMove singleMove)
	{
		if (this.selectedMove != null)
		{
			if (this.selectedMove.jointMoveList != null)
			{
				this.selectedMove.jointMoveList.Remove(singleMove);
			}
			this.ClearRig();
			this.UpdateMoveMenu(true);
		}
	}

	// Token: 0x06000F0D RID: 3853 RVA: 0x0004FDC8 File Offset: 0x0004DFC8
	public void DeleteSelectedSingleMoves()
	{
		if (this.selectedMove != null)
		{
			List<JointMove> list = new List<JointMove>();
			if (this.selectedJointMoves != null && this.selectedJointMoves.Count > 0)
			{
				foreach (JointMove item in this.selectedJointMoves)
				{
					list.Add(item);
				}
			}
			this.selectedJointMoves = new List<JointMove>();
			if (this.selectedSingleMove != null)
			{
				list.Add(this.selectedSingleMove);
				this.selectedSingleMove = null;
			}
			CommandInvoker.ExecuteCommand(new SetKeyframesCommand(this.selectedStance, this.selectedMove, null, list, null, false), false);
		}
		this.ClearRig();
		this.UpdateMoveMenu(true);
	}

	// Token: 0x06000F0E RID: 3854 RVA: 0x0004FE98 File Offset: 0x0004E098
	public void CopyPerformed()
	{
		if (this.selectedMove != null)
		{
			List<JointMove> list = new List<JointMove>(this.selectedJointMoves);
			if (this.selectedSingleMove != null)
			{
				list.Add(this.selectedSingleMove);
			}
			if (list.Count > 0)
			{
				GUIUtility.systemCopyBuffer = JsonConvert.SerializeObject(list);
				UnityEngine.Object.Instantiate<GameObject>(this.infoDialogPrefab).GetComponent<BasicInfoDialog>().SetText(LocalizationHelpers.LocalizedText("txt_copied", Array.Empty<object>()), 1f, false);
			}
		}
	}

	// Token: 0x06000F0F RID: 3855 RVA: 0x0004FF0B File Offset: 0x0004E10B
	public void PastePerformed()
	{
		if (!GeneralManager.CurrentyWritingInTextField())
		{
			if (this.CurrentEditorMode == EditorMode.SingleMove)
			{
				this.PasteSingleMovesFromClipboardPerformed();
				return;
			}
			if (this.CurrentEditorMode == EditorMode.Move)
			{
				this.PasteMoveFromClipboard();
				return;
			}
			if (this.CurrentEditorMode == EditorMode.Stance)
			{
				this.PasteStanceFromClipboard();
			}
		}
	}

	// Token: 0x06000F10 RID: 3856 RVA: 0x0004FF44 File Offset: 0x0004E144
	public void PasteSingleMovesFromClipboardPerformed()
	{
		try
		{
			if (this.selectedMove != null)
			{
				string systemCopyBuffer = GUIUtility.systemCopyBuffer;
				if (!string.IsNullOrEmpty(systemCopyBuffer))
				{
					List<JointMove> list = JsonConvert.DeserializeObject<List<JointMove>>(systemCopyBuffer);
					if (list != null && list.Count > 0)
					{
						List<JointMove> list2 = new List<JointMove>();
						List<JointMove> list3 = new List<JointMove>();
						float value = this.timeLineSlider.value;
						double num = 1000.0;
						foreach (JointMove jointMove in list)
						{
							if (jointMove.executionTime < num)
							{
								num = jointMove.executionTime;
							}
						}
						using (List<JointMove>.Enumerator enumerator = list.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								JointMove copiedMove = enumerator.Current;
								copiedMove.executionTime = Math.Round(copiedMove.executionTime - num + (double)this.timeLineSlider.value, 2);
								foreach (JointMove item in (from x in this.selectedMove.jointMoveList
								where x.joint == copiedMove.joint && Generic.DoubleEquals(x.executionTime, copiedMove.executionTime)
								select x).ToList<JointMove>())
								{
									list2.Add(item);
								}
								list3.Add(copiedMove);
							}
						}
						CommandInvoker.ExecuteCommand(new SetKeyframesCommand(this.selectedStance, this.selectedMove, list3, list2, new float?(value), false), false);
						UnityEngine.Object.Instantiate<GameObject>(this.infoDialogPrefab).GetComponent<BasicInfoDialog>().SetText(LocalizationHelpers.LocalizedText("txt_pasted", Array.Empty<object>()), 1f, false);
					}
				}
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
			UnityEngine.Object.Instantiate<GameObject>(this.infoDialogPrefab).GetComponent<BasicInfoDialog>().SetText(LocalizationHelpers.LocalizedText("txt_could_not_paste_from_clipboard", Array.Empty<object>()), 1f, false);
		}
	}

	// Token: 0x06000F11 RID: 3857 RVA: 0x0005019C File Offset: 0x0004E39C
	public void PlaySelectedMove()
	{
		if (this.selectedMove != null)
		{
			this.playingSelectedMove = true;
			this.animator.playingMovePreview = false;
			this.animator.ClearPreviewHistory();
			this.ResetStance();
			this.animator.PlayMove(this.selectedMove, false, false, 0f, false);
			this.movePlayStarted = Time.time;
			this.UpdateTimelinePlayMoveIndicator();
		}
	}

	// Token: 0x06000F12 RID: 3858 RVA: 0x000501FF File Offset: 0x0004E3FF
	public void CancelSelectedMove()
	{
		this.playingSelectedMove = false;
		this.updateAnimation = true;
		if (this.selectedMove != null)
		{
			this.animator.CancelMove(this.selectedMove.guid);
		}
		this.UpdateTimelinePlayMoveIndicator();
	}

	// Token: 0x06000F13 RID: 3859 RVA: 0x00050233 File Offset: 0x0004E433
	public void PlayActiveSelectedMove()
	{
		if (this.selectedMove != null)
		{
			this.animator.PlayMove(this.selectedMove, false, true, 0f, false);
		}
	}

	// Token: 0x06000F14 RID: 3860 RVA: 0x00050258 File Offset: 0x0004E458
	public List<RaycastResult> RaycastMouse()
	{
		PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
		{
			pointerId = -1
		};
		pointerEventData.position = Mouse.current.position.ReadValue();
		List<RaycastResult> list = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointerEventData, list);
		return list;
	}

	// Token: 0x06000F15 RID: 3861 RVA: 0x000502A0 File Offset: 0x0004E4A0
	public List<RaycastHit> RaycastPhysicsMouse()
	{
		if (this.activeCamera != null)
		{
			return (from x in Physics.RaycastAll(this.activeCamera.GetComponent<Camera>().ScreenPointToRay(Mouse.current.position.ReadValue()), float.PositiveInfinity)
			orderby x.distance
			select x).ToList<RaycastHit>();
		}
		return new List<RaycastHit>();
	}

	// Token: 0x06000F16 RID: 3862 RVA: 0x00050318 File Offset: 0x0004E518
	public void AddMove()
	{
		CommandInvoker.ExecuteCommand(new AddMoveCommand(this.selectedStance, new Move
		{
			jointMoveList = new List<JointMove>(),
			duration = 1f
		}), false);
	}

	// Token: 0x06000F17 RID: 3863 RVA: 0x00050346 File Offset: 0x0004E546
	public void AddStanceChange()
	{
		CommandInvoker.ExecuteCommand(new AddMoveCommand(this.selectedStance, new Move
		{
			stanceChange = true
		}), false);
	}

	// Token: 0x06000F18 RID: 3864 RVA: 0x00050365 File Offset: 0x0004E565
	public void AddStance()
	{
		CommandInvoker.ExecuteCommand(new AddStanceCommand(this.moveSet, new Stance
		{
			moveList = new List<Move>()
		}), false);
	}

	// Token: 0x06000F19 RID: 3865 RVA: 0x00050388 File Offset: 0x0004E588
	public void UpdateDurationUI()
	{
		if (this.selectedMove != null)
		{
			this.MoveDurationField.SetTextWithoutNotify(this.selectedMove.duration.ToString());
			this.timeLineSlider.maxValue = this.selectedMove.duration;
			this.UpdateTimeLineMoves();
		}
	}

	// Token: 0x06000F1A RID: 3866 RVA: 0x000503D8 File Offset: 0x0004E5D8
	public void OnDurationChanged()
	{
		try
		{
			string text = this.MoveDurationField.text;
			float num;
			string text2;
			if (this.selectedMove != null && Generic.ConvertToRoundedFloat(text, out num, out text2))
			{
				float num2 = num;
				if (num2 > 10f)
				{
					num2 = 10f;
				}
				else if (num2 < 0.01f)
				{
					num2 = 0.01f;
				}
				CommandInvoker.ExecuteCommand(new MoveDurationChangeCommand(this.selectedStance, this.selectedMove, num2), false);
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
	}

	// Token: 0x06000F1B RID: 3867 RVA: 0x00050458 File Offset: 0x0004E658
	public void DeleteMove(Stance stance, Move move)
	{
		CommandInvoker.ExecuteCommand(new DeleteMoveCommand(stance, move), false);
	}

	// Token: 0x06000F1C RID: 3868 RVA: 0x00050467 File Offset: 0x0004E667
	public void DeleteStance(Stance stance)
	{
		CommandInvoker.ExecuteCommand(new DeleteStanceCommand(this.moveSet, stance), false);
	}

	// Token: 0x06000F1D RID: 3869 RVA: 0x0005047B File Offset: 0x0004E67B
	public void CopyStance(Stance stance)
	{
		if (stance != null)
		{
			GUIUtility.systemCopyBuffer = stance.GetJsonCopyString();
			UnityEngine.Object.Instantiate<GameObject>(this.infoDialogPrefab).GetComponent<BasicInfoDialog>().SetText(LocalizationHelpers.LocalizedText("txt_copied", Array.Empty<object>()), 1f, false);
		}
	}

	// Token: 0x06000F1E RID: 3870 RVA: 0x000504B8 File Offset: 0x0004E6B8
	public void PasteStanceFromClipboard()
	{
		try
		{
			string systemCopyBuffer = GUIUtility.systemCopyBuffer;
			Stance stance = JsonConvert.DeserializeObject<Stance>(systemCopyBuffer);
			stance.CreateNewGuid();
			while ((from x in this.moveSet.stanceList
			where x.name == stance.name
			select x).FirstOrDefault<Stance>() != null)
			{
				Stance stance2 = stance;
				stance2.name += LocalizationHelpers.LocalizedText("txt_append_to_copied_name", Array.Empty<object>());
			}
			if (stance.moveList != null)
			{
				foreach (Move move in stance.moveList)
				{
					move.CreateNewGuid();
				}
			}
			stance.isDefault = false;
			CommandInvoker.ExecuteCommand(new AddStanceCommand(this.moveSet, stance), false);
			UnityEngine.Object.Instantiate<GameObject>(this.infoDialogPrefab).GetComponent<BasicInfoDialog>().SetText(LocalizationHelpers.LocalizedText("txt_new_stance_added", Array.Empty<object>()), 1f, false);
		}
		catch (Exception message)
		{
			Debug.LogError(message);
			UnityEngine.Object.Instantiate<GameObject>(this.infoDialogPrefab).GetComponent<BasicInfoDialog>().SetText(LocalizationHelpers.LocalizedText("txt_could_not_create_stance_from_clipboard", Array.Empty<object>()), 1f, false);
		}
	}

	// Token: 0x06000F1F RID: 3871 RVA: 0x0005062C File Offset: 0x0004E82C
	public void CopyMove(Move move)
	{
		if (move != null)
		{
			GUIUtility.systemCopyBuffer = move.GetJsonCopyString();
			UnityEngine.Object.Instantiate<GameObject>(this.infoDialogPrefab).GetComponent<BasicInfoDialog>().SetText(LocalizationHelpers.LocalizedText("txt_copied", Array.Empty<object>()), 1f, false);
		}
	}

	// Token: 0x06000F20 RID: 3872 RVA: 0x00050668 File Offset: 0x0004E868
	public void ListenInputMove(Move move, Dropdown dp)
	{
		if (move != null)
		{
			BasicInfoDialog component = UnityEngine.Object.Instantiate<GameObject>(this.infoDialogPrefab).GetComponent<BasicInfoDialog>();
			component.SetText(LocalizationHelpers.LocalizedText("txt_press_a_action_button", Array.Empty<object>()), -1f, false);
			this.listenActionForMove = move;
			this.listenActionForMoveDialog = component;
			this.listenActionForMoveDropdown = dp;
			EventSystemManager.singletonEventSystemManager.DisableEventSystem();
			this.DisableControls();
		}
	}

	// Token: 0x06000F21 RID: 3873 RVA: 0x000506C9 File Offset: 0x0004E8C9
	public void ActivatePlayerAction(PlayerAction playerAction)
	{
		if (playerAction != null && this.listenActionForMove != null)
		{
			CommandInvoker.ExecuteCommand(new ChangeMoveActionCommand(this.selectedStance, this.listenActionForMove, playerAction.name), false);
		}
		this.CancelListenActionForMove();
	}

	// Token: 0x06000F22 RID: 3874 RVA: 0x000506F9 File Offset: 0x0004E8F9
	public void CancelListenActionForMove()
	{
		this.EnableControls();
		EventSystemManager.singletonEventSystemManager.EnableEventSystem();
		this.listenActionForMove = null;
		this.listenActionForMoveDropdown = null;
		if (this.listenActionForMoveDialog != null)
		{
			this.listenActionForMoveDialog.DestroyPanel();
			this.listenActionForMoveDialog = null;
		}
	}

	// Token: 0x06000F23 RID: 3875 RVA: 0x0005073C File Offset: 0x0004E93C
	public void PasteMoveFromClipboard()
	{
		if (this.selectedStance == null)
		{
			return;
		}
		try
		{
			string systemCopyBuffer = GUIUtility.systemCopyBuffer;
			Move move = JsonConvert.DeserializeObject<Move>(systemCopyBuffer);
			move.CreateNewGuid();
			while ((from x in this.selectedStance.moveList
			where x.name == move.name
			select x).FirstOrDefault<Move>() != null)
			{
				Move move2 = move;
				move2.name += LocalizationHelpers.LocalizedText("txt_append_to_copied_name", Array.Empty<object>());
			}
			CommandInvoker.ExecuteCommand(new AddMoveCommand(this.selectedStance, move), false);
			UnityEngine.Object.Instantiate<GameObject>(this.infoDialogPrefab).GetComponent<BasicInfoDialog>().SetText(LocalizationHelpers.LocalizedText("txt_new_move_added", Array.Empty<object>()), 1f, false);
		}
		catch (Exception message)
		{
			Debug.LogError(message);
			UnityEngine.Object.Instantiate<GameObject>(this.infoDialogPrefab).GetComponent<BasicInfoDialog>().SetText(LocalizationHelpers.LocalizedText("txt_could_not_create_move_from_clipboard", Array.Empty<object>()), 1f, false);
		}
	}

	// Token: 0x06000F24 RID: 3876 RVA: 0x00050844 File Offset: 0x0004EA44
	public void SetCurrentTime(float newCurrentTime)
	{
		this.timeLineSlider.SetValueWithoutNotify(Convert.ToSingle(Math.Round((double)newCurrentTime, 2)));
		this.MoveCurrentTimeField.SetTextWithoutNotify(this.timeLineSlider.value.ToString());
		this.UpdateTimeLineMoves();
	}

	// Token: 0x06000F25 RID: 3877 RVA: 0x00050890 File Offset: 0x0004EA90
	public void UpdateCurrentTime()
	{
		this.timeLineSlider.value = Convert.ToSingle(Math.Round((double)this.timeLineSlider.value, 2));
		if (this.timeLineSlider.value.ToString() != this.MoveCurrentTimeField.text)
		{
			this.MoveCurrentTimeField.text = this.timeLineSlider.value.ToString();
			this.SetSelectedSingleMove(null);
			this.SwapTwoHandedRig(new bool?(false));
			this.ClearTempSingleMoves();
		}
	}

	// Token: 0x06000F26 RID: 3878 RVA: 0x0005091C File Offset: 0x0004EB1C
	public void CurrentTimeChanged(string textValue, bool forceValueSet = false)
	{
		try
		{
			float num;
			string text;
			if (this.selectedMove != null && Generic.ConvertToRoundedFloat(textValue, out num, out text))
			{
				if (!Generic.DoubleEquals((double)num, (double)this.timeLineSlider.value))
				{
					this.SwapTwoHandedRig(new bool?(false));
				}
				if (num <= this.selectedMove.duration)
				{
					if (num != this.timeLineSlider.value || forceValueSet)
					{
						this.timeLineSlider.value = num;
						this.SetSelectedSingleMove(null);
						this.ClearTempSingleMoves();
					}
					this.MoveCurrentTimeField.text = text;
				}
				else
				{
					this.MoveCurrentTimeField.text = text;
					this.SetSelectedSingleMove(null);
					this.ClearTempSingleMoves();
				}
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
	}

	// Token: 0x06000F27 RID: 3879 RVA: 0x000509E0 File Offset: 0x0004EBE0
	public void UpdateTimeLineMoves()
	{
		float num = 200f;
		this.ClearTimeLine();
		this.timeLineRowRectTransform.sizeDelta = new Vector2((float)(Screen.width - 20), num);
		this.timeLineJointRectTransform.sizeDelta = new Vector2((float)(Screen.width - 20 - 14), num);
		float x3 = this.timeLineJointRectTransform.sizeDelta.x;
		if (this.selectedMove != null)
		{
			if (this.selectedMove.jointMoveList != null)
			{
				bool flag = this.selectedJointTypes.Count > 0 && this.selectedJointTypes.Count < this.jointFilterItems.Count;
				int num2 = 0;
				using (List<JointMove>.Enumerator enumerator = this.selectedMove.jointMoveList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						JointMove move = enumerator.Current;
						if (!flag || this.selectedJointTypes.Contains(move.joint))
						{
							MoveDot moveDotFromPool = this.GetMoveDotFromPool();
							moveDotFromPool.Enable();
							RectTransform rectTransform = moveDotFromPool.rectTransform;
							float num3 = Convert.ToSingle((double)x3 * (move.executionTime / (double)this.selectedMove.duration));
							float num4 = -6f;
							moveDotFromPool.SetPosition(num3, num4);
							for (int i = 0; i < this.moveDots.Count; i++)
							{
								MoveDot moveDot = this.moveDots[i];
								if (moveDot.positionY <= num4 && num3 - 10f < moveDot.positionX && moveDot.positionX < num3 + 10f)
								{
									num4 = moveDot.positionY - 12f;
									moveDotFromPool.SetPosition(num3, num4);
								}
							}
							moveDotFromPool.UpdatePosition();
							moveDotFromPool.dotImage.color = Color.white;
							if (move.handState != null)
							{
								HandState? handState = move.handState;
								HandState handState2 = HandState.Hold;
								if (handState.GetValueOrDefault() == handState2 & handState != null)
								{
									moveDotFromPool.dotImage.color = UISettings.HandHoldColor;
								}
								else
								{
									handState = move.handState;
									handState2 = HandState.LooseHold;
									if (handState.GetValueOrDefault() == handState2 & handState != null)
									{
										moveDotFromPool.dotImage.color = UISettings.HandLooseHoldColor;
									}
									else
									{
										handState = move.handState;
										handState2 = HandState.NoHold;
										if (handState.GetValueOrDefault() == handState2 & handState != null)
										{
											moveDotFromPool.dotImage.color = UISettings.HandNoHoldColor;
										}
									}
								}
							}
							if (move == this.selectedSingleMove)
							{
								moveDotFromPool.dotImage.color = Color.yellow;
							}
							else if ((from x in this.selectedJointMoves
							where x == move
							select x).FirstOrDefault<JointMove>() != null)
							{
								moveDotFromPool.dotImage.color = Color.red;
							}
							moveDotFromPool.SingleMove = move;
							moveDotFromPool.tooltipItem.text = move.joint.GetDescription();
							this.moveDots.Add(moveDotFromPool);
							if (Math.Abs(num4) + 6f > num)
							{
								num = Math.Abs(num4) + 6f;
							}
							num2++;
						}
					}
				}
			}
			float x2 = Convert.ToSingle(x3 * (this.timeLineSlider.value / this.selectedMove.duration)) + 7f;
			float y = 0f;
			this.timeLineRectTransform.anchoredPosition = new Vector3(x2, y, 0f);
			this.timeLineRectTransform.sizeDelta = new Vector2(1f, num);
		}
		else
		{
			this.timeLineRectTransform.anchoredPosition = new Vector3(-1000f, 0f, 0f);
		}
		this.timeLineHolderRectTransform.sizeDelta = new Vector2(0f, num - 200f);
		this.timeLineRowRectTransform.sizeDelta = new Vector2((float)(Screen.width - 20), num);
		this.timeLineJointRectTransform.sizeDelta = new Vector2((float)(Screen.width - 20 - 14), num);
		this.UpdateHandStateUI();
		this.UpdateAnimation();
	}

	// Token: 0x06000F28 RID: 3880 RVA: 0x00050E3C File Offset: 0x0004F03C
	private void UpdateTimelinePlayMoveIndicator()
	{
		if (!this.playingSelectedMove)
		{
			this.timeLinePlayMoveRectTransform.anchoredPosition = new Vector3(-1000f, 0f, 0f);
			return;
		}
		if (this.selectedMove.duration < 0f)
		{
			return;
		}
		float x = this.timeLineJointRectTransform.sizeDelta.x;
		float y = 200f;
		float num;
		for (num = Time.time - this.movePlayStarted / this.selectedMove.duration; num > this.selectedMove.duration; num -= this.selectedMove.duration)
		{
		}
		float x2 = Convert.ToSingle(x * num) + 7f;
		float y2 = 0f;
		this.timeLinePlayMoveRectTransform.anchoredPosition = new Vector3(x2, y2, 0f);
		this.timeLinePlayMoveRectTransform.sizeDelta = new Vector2(1f, y);
	}

	// Token: 0x06000F29 RID: 3881 RVA: 0x00050F24 File Offset: 0x0004F124
	public void SetSelectedJoint(GameObject physicsJoint, GameObject animationJoint = null)
	{
		SelectedJoint joint = new SelectedJoint();
		if (animationJoint != null)
		{
			FighterJoint fighterJoint = (from x in this.animator.FighterJoints
			where x.joint.name == animationJoint.name
			select x).FirstOrDefault<FighterJoint>();
			if (fighterJoint != null)
			{
				physicsJoint = fighterJoint.physicsJoint;
				joint.JointType = fighterJoint.jointType;
			}
		}
		if (physicsJoint != null)
		{
			joint.PhysicsJoint = physicsJoint;
			foreach (ConfigurableJointScript configurableJointScript in physicsJoint.GetComponentsInParent<ConfigurableJointScript>())
			{
				if (configurableJointScript.target.name == physicsJoint.name)
				{
					joint.JointScript = configurableJointScript;
					joint.AnimationJoint = configurableJointScript.target.gameObject;
					break;
				}
			}
			if (joint.AnimationJoint != null)
			{
				FighterJoint fighterJoint2 = (from x in this.animator.FighterJoints
				where x.joint.name == joint.AnimationJoint.name
				select x).FirstOrDefault<FighterJoint>();
				if (fighterJoint2 != null)
				{
					joint.JointType = fighterJoint2.jointType;
				}
			}
		}
		if (joint.AnimationJoint != null && joint.PhysicsJoint != null)
		{
			this.selectedJoint = joint;
		}
		if (this.selectedSingleMove != null && this.selectedSingleMove.joint != this.selectedJoint.JointType)
		{
			this.selectedSingleMove = null;
		}
		if (this.tempSingleMove != null && this.tempSingleMove.joint != this.selectedJoint.JointType)
		{
			this.tempSingleMove = null;
		}
		this.UpdateTimeLineMoves();
		this.SetupGizmoForJoint();
	}

	// Token: 0x06000F2A RID: 3882 RVA: 0x000510E0 File Offset: 0x0004F2E0
	public void SetSelectedSingleMove(JointMove singleMove)
	{
		if (singleMove != null)
		{
			this.CurrentTimeChanged(singleMove.executionTime.ToString(), true);
		}
		this.selectedSingleMove = singleMove;
		if (this.selectedSingleMove != null)
		{
			FighterJoint fighterJoint = (from x in this.animator.FighterJoints
			where x.jointType == singleMove.joint
			select x).FirstOrDefault<FighterJoint>();
			if (fighterJoint != null)
			{
				this.SetSelectedJoint(fighterJoint.physicsJoint, null);
			}
		}
		this.tempSingleMove = null;
		this.selectedJointMoves = new List<JointMove>();
		this.UpdateTimeLineMoves();
	}

	// Token: 0x06000F2B RID: 3883 RVA: 0x0005117A File Offset: 0x0004F37A
	public void ClearTempSingleMoves()
	{
		this.tempSingleMoves = new List<JointMove>();
		this.tempSingleMove = null;
		this.ClearRig();
	}

	// Token: 0x06000F2C RID: 3884 RVA: 0x00051194 File Offset: 0x0004F394
	public void ToggleSelectedSingleMove(JointMove singleMove)
	{
		if (this.selectedSingleMove == singleMove)
		{
			this.selectedSingleMove = this.selectedJointMoves.FirstOrDefault<JointMove>();
		}
		else if ((from x in this.selectedJointMoves
		where x == singleMove
		select x).FirstOrDefault<JointMove>() != null)
		{
			this.selectedJointMoves.Remove(singleMove);
		}
		else
		{
			if (this.selectedSingleMove != null)
			{
				this.selectedJointMoves.Add(this.selectedSingleMove);
			}
			this.selectedSingleMove = singleMove;
		}
		this.UpdateTimeLineMoves();
	}

	// Token: 0x06000F2D RID: 3885 RVA: 0x0005122C File Offset: 0x0004F42C
	public void ClearSelectedSingleMoves()
	{
		this.selectedJointMoves.Clear();
		this.selectedSingleMove = null;
	}

	// Token: 0x06000F2E RID: 3886 RVA: 0x00051240 File Offset: 0x0004F440
	public void AddSingleMoveToSelection(JointMove singleMove)
	{
		if (singleMove == null)
		{
			return;
		}
		if (!this.JointMoveAlreadySelected(singleMove))
		{
			if (this.selectedSingleMove == null)
			{
				this.selectedSingleMove = singleMove;
				return;
			}
			this.selectedJointMoves.Add(singleMove);
		}
	}

	// Token: 0x06000F2F RID: 3887 RVA: 0x0005126C File Offset: 0x0004F46C
	public void AddAreaToSelection()
	{
		if (this.selectionStart != null)
		{
			Vector2 value = this.selectionStart.Value;
			Vector2 v = Mouse.current.position.ReadValue();
			Rect screenRect = UIHelpers.GetScreenRect(value, v);
			foreach (MoveDot moveDot in this.moveDots)
			{
				float num = (float)Screen.height - moveDot.rectTransform.position.y;
				if (screenRect.xMin < moveDot.rectTransform.position.x && moveDot.rectTransform.position.x < screenRect.xMax && screenRect.yMin < num && num < screenRect.yMax)
				{
					this.AddSingleMoveToSelection(moveDot.SingleMove);
				}
			}
		}
		this.UpdateTimeLineMoves();
	}

	// Token: 0x06000F30 RID: 3888 RVA: 0x00051368 File Offset: 0x0004F568
	public void SwapDelectedSingleMove(JointMove singleMove)
	{
		if (this.selectedSingleMove != singleMove)
		{
			if (this.selectedSingleMove != null)
			{
				this.selectedJointMoves.Add(this.selectedSingleMove);
			}
			this.selectedJointMoves.Remove(singleMove);
			this.selectedSingleMove = singleMove;
		}
		this.UpdateTimeLineMoves();
	}

	// Token: 0x06000F31 RID: 3889 RVA: 0x000513A8 File Offset: 0x0004F5A8
	public bool JointMoveAlreadySelected(JointMove singleMove)
	{
		return singleMove == this.selectedSingleMove || (from x in this.selectedJointMoves
		where x == singleMove
		select x).FirstOrDefault<JointMove>() != null;
	}

	// Token: 0x06000F32 RID: 3890 RVA: 0x000513F4 File Offset: 0x0004F5F4
	public void CalculateCameraSize()
	{
		if (this.resolution.x != (float)Screen.width || this.resolution.y != (float)Screen.height)
		{
			float num = (float)Screen.width;
			float num2 = (float)Screen.height;
			float num3 = 240f;
			this.cameraWidth = (num - 330f) / 2f;
			this.cameraHeight = num2 - num3;
			this.physicsCamera.GetComponent<Camera>().pixelRect = new Rect
			{
				width = this.cameraWidth,
				height = this.cameraHeight,
				y = num3
			};
			this.animationCamera.GetComponent<Camera>().pixelRect = new Rect
			{
				width = this.cameraWidth,
				height = this.cameraHeight,
				y = num3,
				x = this.cameraWidth
			};
			this.resolution.x = (float)Screen.width;
			this.resolution.y = (float)Screen.height;
			if (this.staminaHudCanvas != null)
			{
				this.staminaHudCanvas.SetupCamera(this.physicsCamera.GetComponent<Camera>());
			}
		}
	}

	// Token: 0x06000F33 RID: 3891 RVA: 0x00051524 File Offset: 0x0004F724
	private void HandleKeyFrameDragEnd()
	{
		if (this.dragKeyframesCommand != null)
		{
			List<JointMove> list = new List<JointMove>();
			using (List<JointMove>.Enumerator enumerator = this.dragKeyframesCommand.changedSingleMoves.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					JointMove currentTempSingleMove = enumerator.Current;
					List<JointMove> collection = (from x in this.selectedMove.jointMoveList
					where x.joint == currentTempSingleMove.joint && Generic.DoubleEquals(x.executionTime, currentTempSingleMove.executionTime)
					select x).ToList<JointMove>();
					list.AddRange(collection);
				}
			}
			for (int i = list.Count - 1; i > -1; i--)
			{
				JointMove jointMove = list[i];
				for (int j = this.dragKeyframesCommand.changedSingleMoves.Count - 1; j > -1; j--)
				{
					if (this.dragKeyframesCommand.changedSingleMoves[j] == jointMove)
					{
						list.RemoveAt(i);
						break;
					}
				}
			}
			this.dragKeyframesCommand.SetDeletedKeyframes(list);
			this.dragKeyframesCommand = null;
		}
	}

	// Token: 0x06000F34 RID: 3892 RVA: 0x00051630 File Offset: 0x0004F830
	public void CreateKeyframe()
	{
		if (this.selectedMove != null)
		{
			if (this.selectedJoint != null && this.selectedSingleMove == null && this.tempSingleMoves.Count == 0)
			{
				this.CreateTempSingleMove();
				this.SetTempSingleMoveRotation();
			}
			if (this.tempSingleMoves.Count > 0)
			{
				List<JointMove> list = new List<JointMove>();
				List<JointMove> list2 = new List<JointMove>();
				float value = 0f;
				int num = 0;
				using (List<JointMove>.Enumerator enumerator = this.tempSingleMoves.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						JointMove currentTempSingleMove = enumerator.Current;
						if (num == 0)
						{
							value = Convert.ToSingle(currentTempSingleMove.executionTime);
						}
						List<JointMove> list3 = (from x in this.selectedMove.jointMoveList
						where x.joint == currentTempSingleMove.joint && Generic.DoubleEquals(x.executionTime, currentTempSingleMove.executionTime)
						select x).ToList<JointMove>();
						list.AddRange(list3);
						if ((currentTempSingleMove.joint == JointType.WRIST_RIGHT || currentTempSingleMove.joint == JointType.WRIST_LEFT) && list3.Count > 0)
						{
							currentTempSingleMove.handState = list3.First<JointMove>().handState;
						}
						currentTempSingleMove.temp = false;
						list2.Add(currentTempSingleMove);
						num++;
					}
				}
				CommandInvoker.ExecuteCommand(new SetKeyframesCommand(this.selectedStance, this.selectedMove, list2, list, new float?(value), true), false);
			}
		}
	}

	// Token: 0x06000F35 RID: 3893 RVA: 0x000517AC File Offset: 0x0004F9AC
	public void UpdateUIAfterKeyframeChanges(List<JointMove> newSelectedMoves = null)
	{
		this.ClearSelectedSingleMoves();
		if (newSelectedMoves != null)
		{
			foreach (JointMove singleMove in newSelectedMoves)
			{
				this.AddSingleMoveToSelection(singleMove);
			}
		}
		this.UpdateTimeLineMoves();
		this.UpdateMoveMenu(false);
	}

	// Token: 0x06000F36 RID: 3894 RVA: 0x00051810 File Offset: 0x0004FA10
	public JointMove CreateTempSingleMove()
	{
		JointMove jointMove = (from x in this.tempSingleMoves
		where x.joint == this.selectedJoint.JointType
		select x).FirstOrDefault<JointMove>();
		JointMove jointMove2;
		if (jointMove != null)
		{
			this.tempSingleMove = jointMove;
			jointMove2 = jointMove;
		}
		else
		{
			jointMove2 = new JointMove
			{
				joint = this.selectedJoint.JointType,
				executionTime = (double)this.timeLineSlider.value,
				temp = true
			};
			this.tempSingleMove = jointMove2;
			this.tempSingleMoves.Add(jointMove2);
		}
		return jointMove2;
	}

	// Token: 0x06000F37 RID: 3895 RVA: 0x00051890 File Offset: 0x0004FA90
	public void AddTempSingleMove(JointMove newMove)
	{
		JointMove jointMove = (from x in this.tempSingleMoves
		where x.joint == newMove.joint
		select x).FirstOrDefault<JointMove>();
		if (jointMove != null)
		{
			this.tempSingleMoves.Remove(jointMove);
		}
		newMove.temp = true;
		newMove.executionTime = (double)this.timeLineSlider.value;
		this.tempSingleMoves.Add(newMove);
	}

	// Token: 0x06000F38 RID: 3896 RVA: 0x0005190C File Offset: 0x0004FB0C
	public void SetTempSingleMoves(List<JointMove> newTempMoves)
	{
		this.tempSingleMoves.Clear();
		if (newTempMoves != null)
		{
			foreach (JointMove item in newTempMoves)
			{
				this.tempSingleMoves.Add(item);
			}
		}
	}

	// Token: 0x06000F39 RID: 3897 RVA: 0x00051970 File Offset: 0x0004FB70
	public void PrepareSelectedMoveForUse()
	{
		if (this.animator != null && this.selectedMove != null)
		{
			this.animator.PrepareMoveForUse(this.selectedMove);
		}
	}

	// Token: 0x06000F3A RID: 3898 RVA: 0x0005199C File Offset: 0x0004FB9C
	public float GetTimelineMousePercentage()
	{
		float num = (Mouse.current.position.ReadValue().x - 6f) / (float)(Screen.width - 20 - 14);
		if (num > 1f)
		{
			num = 1f;
		}
		else if (num < 0f)
		{
			num = 0f;
		}
		return num;
	}

	// Token: 0x06000F3B RID: 3899 RVA: 0x000519F0 File Offset: 0x0004FBF0
	private void OnGUI()
	{
		new GUIStyle().fontSize = 24;
		if (this.selectionStart != null)
		{
			Rect screenRect = UIHelpers.GetScreenRect(this.selectionStart.Value, Mouse.current.position.ReadValue());
			UIHelpers.DrawScreenRect(screenRect, UISettings.SelectionBoxColor);
			UIHelpers.DrawScreenRectBorder(screenRect, 1f, UISettings.SelectionBoxBorderColor);
		}
	}

	// Token: 0x06000F3C RID: 3900 RVA: 0x00051A59 File Offset: 0x0004FC59
	private void UpdateAnimation()
	{
		this.updateAnimation = true;
		this.animator.ClearPreviewHistory();
	}

	// Token: 0x06000F3D RID: 3901 RVA: 0x00051A70 File Offset: 0x0004FC70
	public void SetSelectedRig(GameObject rigObject)
	{
		if (rigObject != null)
		{
			this.selectedRigTarget = new SelectedRigTarget
			{
				GameObject = rigObject
			};
			SimpleRig component = rigObject.GetComponent<SimpleRig>();
			if (component != null)
			{
				this.selectedRigTarget.Rig = component;
			}
			else
			{
				RigHint component2 = rigObject.GetComponent<RigHint>();
				if (component2 != null)
				{
					this.selectedRigTarget.Rig = component2.simpleRig;
					this.selectedRigTarget.isHint = true;
				}
			}
			this.SetupGizmoForJoint();
		}
	}

	// Token: 0x06000F3E RID: 3902 RVA: 0x00051AEC File Offset: 0x0004FCEC
	public void SwapRig(bool? forceRigMode = null, bool swapTwoHandedMode = true)
	{
		if (swapTwoHandedMode)
		{
			this.SwapTwoHandedRig(new bool?(false));
		}
		if (forceRigMode == null)
		{
			this.usingIKRig = !this.usingIKRig;
			this.ClearSelectedGizmoTargets();
		}
		bool value = this.usingIKRig;
		if (forceRigMode != null)
		{
			value = forceRigMode.Value;
		}
		this.rigManager.SetActive(value);
		if (value)
		{
			this.swapRigButton.transform.GetComponentInChildren<Text>().text = LocalizationSettings.StringDatabase.GetLocalizedString(SettingsHelper.localizationTableName, "btn_disable_rig", null, FallbackBehavior.UseProjectSettings, Array.Empty<object>());
			this.twoHandedRigButton.gameObject.SetActive(true);
		}
		else
		{
			this.swapRigButton.transform.GetComponentInChildren<Text>().text = LocalizationSettings.StringDatabase.GetLocalizedString(SettingsHelper.localizationTableName, "btn_enable_rig", null, FallbackBehavior.UseProjectSettings, Array.Empty<object>());
			this.twoHandedRigButton.gameObject.SetActive(false);
		}
		this.UpdateAnimation();
	}

	// Token: 0x06000F3F RID: 3903 RVA: 0x00051BEC File Offset: 0x0004FDEC
	public void SwapTwoHandedRig(bool? forceMode = null)
	{
		this.usingTwoHandedRig = !this.usingTwoHandedRig;
		if (forceMode != null)
		{
			this.usingTwoHandedRig = forceMode.Value;
		}
		this.rigManager.twoHandedRig.SetActive(this.usingTwoHandedRig);
		this.SetupGizmoForJoint();
		if (this.usingTwoHandedRig)
		{
			this.twoHandedRigButton.transform.GetComponentInChildren<Text>().text = LocalizationSettings.StringDatabase.GetLocalizedString(SettingsHelper.localizationTableName, "btn_one_handed", null, FallbackBehavior.UseProjectSettings, Array.Empty<object>());
		}
		else
		{
			this.twoHandedRigButton.transform.GetComponentInChildren<Text>().text = LocalizationSettings.StringDatabase.GetLocalizedString(SettingsHelper.localizationTableName, "btn_two_handed", null, FallbackBehavior.UseProjectSettings, Array.Empty<object>());
			if (this.selectedRigTarget != null && this.selectedRigTarget.Rig == this.rigManager.twoHandedRig)
			{
				this.ClearSelectedGizmoTargets();
			}
		}
		this.UpdateAnimation();
	}

	// Token: 0x06000F40 RID: 3904 RVA: 0x00051CE9 File Offset: 0x0004FEE9
	public void ClearRig()
	{
		this.rigManager.SetDoAnimation(false);
		this.reActivateRig = true;
		if (this.rigManager != null && this.usingIKRig)
		{
			this.rigManager.RecalculateTargetPosition();
		}
	}

	// Token: 0x06000F41 RID: 3905 RVA: 0x00051D1F File Offset: 0x0004FF1F
	public void SetEditMode(EditMode editMode)
	{
		this.rigEditMode = editMode;
		this.CheckRigMode();
		this.SetupGizmoForJoint();
	}

	// Token: 0x06000F42 RID: 3906 RVA: 0x00051D34 File Offset: 0x0004FF34
	public void CheckRigMode()
	{
		if ((this.rigEditMode == EditMode.Rotate && this.selectedRigTarget != null && !this.selectedRigTarget.canRotate) || this.twoHandedRigAndHandSelected)
		{
			this.rigEditMode = EditMode.Move;
		}
	}

	// Token: 0x06000F43 RID: 3907 RVA: 0x00051D64 File Offset: 0x0004FF64
	public void SwapEditMode()
	{
		EditMode editMode = EditMode.Move;
		if (this.rigEditMode == editMode)
		{
			editMode = EditMode.Rotate;
		}
		this.SetEditMode(editMode);
	}

	// Token: 0x06000F44 RID: 3908 RVA: 0x00051D88 File Offset: 0x0004FF88
	public void UpdateSwapEditModeButton()
	{
		if (this.rigEditMode == EditMode.Move)
		{
			this.swapEditModeButton.transform.GetComponentInChildren<Text>().text = LocalizationSettings.StringDatabase.GetLocalizedString(SettingsHelper.localizationTableName, "btn_rotate", null, FallbackBehavior.UseProjectSettings, Array.Empty<object>());
		}
		else
		{
			this.swapEditModeButton.transform.GetComponentInChildren<Text>().text = LocalizationSettings.StringDatabase.GetLocalizedString(SettingsHelper.localizationTableName, "btn_move", null, FallbackBehavior.UseProjectSettings, Array.Empty<object>());
		}
		if (!this.usingIKRig || (this.selectedRigTarget != null && !this.selectedRigTarget.canRotate))
		{
			this.swapEditModeButton.gameObject.SetActive(false);
			return;
		}
		this.swapEditModeButton.gameObject.SetActive(true);
	}

	// Token: 0x06000F45 RID: 3909 RVA: 0x00051E54 File Offset: 0x00050054
	private void ClearTimeLine()
	{
		for (int i = this.moveDots.Count - 1; i > -1; i--)
		{
			MoveDot moveDot = this.moveDots[i];
			this.moveDots.RemoveAt(i);
			this.ReturnMoveDotToPool(moveDot);
		}
	}

	// Token: 0x06000F46 RID: 3910 RVA: 0x00051E9C File Offset: 0x0005009C
	private void InitMoveDot()
	{
		if (this.timeLineRowPanel == null)
		{
			this.timeLineRowPanel = new GameObject("TimeLineMovePanel1");
			this.timeLineRowPanel.transform.SetParent(this.timeLineMovesPanel.transform);
			this.timeLineRowRectTransform = this.timeLineRowPanel.AddComponent<RectTransform>();
			this.timeLineRowRectTransform.anchoredPosition = new Vector3(-10f, 0f, 0f);
			this.timeLineRowRectTransform.localScale = new Vector3(1f, 1f, 1f);
			this.timeLineRowPanel.AddComponent<CanvasRenderer>();
			this.timeLineRowPanel.AddComponent<Image>().color = UISettings.BasicSubPanelColor;
		}
		if (this.timeLineJointPanel == null)
		{
			this.timeLineJointPanel = new GameObject("timeLineJointPanel");
			this.timeLineJointPanel.transform.SetParent(this.timeLineRowPanel.transform);
			this.timeLineJointRectTransform = this.timeLineJointPanel.AddComponent<RectTransform>();
			this.timeLineJointRectTransform.anchoredPosition = new Vector3(0f, 0f, 0f);
			this.timeLineJointRectTransform.localScale = new Vector3(1f, 1f, 1f);
			this.timeLineJointPanel.AddComponent<CanvasRenderer>();
		}
		if (this.timeLineIndicatorPanel == null)
		{
			this.timeLineIndicatorPanel = new GameObject("timeLineIndicator");
			this.timeLineIndicatorPanel.transform.SetParent(this.timeLineRowPanel.transform);
			this.timeLineRectTransform = this.timeLineIndicatorPanel.AddComponent<RectTransform>();
			this.timeLineRectTransform.anchorMin = new Vector2(0f, 0.5f);
			this.timeLineRectTransform.anchorMax = new Vector2(0f, 0.5f);
			this.timeLineRectTransform.localScale = new Vector3(1f, 1f, 1f);
			this.timeLineRectTransform.sizeDelta = new Vector2(1f, 200f);
			this.timeLineIndicatorPanel.AddComponent<CanvasRenderer>();
			Image image = this.timeLineIndicatorPanel.AddComponent<Image>();
			image.raycastTarget = false;
			image.color = Color.red;
		}
		if (this.timeLinePlayMoveIndicatorPanel == null)
		{
			this.timeLinePlayMoveIndicatorPanel = new GameObject("timeLinePlayMoveIndicator");
			this.timeLinePlayMoveIndicatorPanel.transform.SetParent(this.timeLineRowPanel.transform);
			this.timeLinePlayMoveRectTransform = this.timeLinePlayMoveIndicatorPanel.AddComponent<RectTransform>();
			this.timeLinePlayMoveRectTransform.anchorMin = new Vector2(0f, 0.5f);
			this.timeLinePlayMoveRectTransform.anchorMax = new Vector2(0f, 0.5f);
			this.timeLinePlayMoveRectTransform.localScale = new Vector3(1f, 1f, 1f);
			this.timeLinePlayMoveRectTransform.sizeDelta = new Vector2(1f, 200f);
			this.timeLinePlayMoveIndicatorPanel.AddComponent<CanvasRenderer>();
			this.timeLinePlayMoveIndicatorPanel.AddComponent<Image>().color = Color.green;
			this.UpdateTimelinePlayMoveIndicator();
		}
		this.timeLineHolderRectTransform = this.timeLineMovesPanel.transform.parent.gameObject.GetComponent<RectTransform>();
		this.pool_moveDot = new List<MoveDot>(1024);
		this.moveDots = new List<MoveDot>(1024);
		for (int i = 0; i < 512; i++)
		{
			this.pool_moveDot.Add(this.CreateNewMoveDot());
		}
	}

	// Token: 0x06000F47 RID: 3911 RVA: 0x0005221C File Offset: 0x0005041C
	private MoveDot GetMoveDotFromPool()
	{
		MoveDot moveDot = null;
		if (this.pool_moveDot.Count > 0)
		{
			int index = this.pool_moveDot.Count - 1;
			moveDot = this.pool_moveDot[index];
			this.pool_moveDot.RemoveAt(index);
		}
		if (moveDot == null)
		{
			moveDot = this.CreateNewMoveDot();
		}
		return moveDot;
	}

	// Token: 0x06000F48 RID: 3912 RVA: 0x0005226C File Offset: 0x0005046C
	private MoveDot CreateNewMoveDot()
	{
		MoveDot moveDot = new MoveDot();
		string name = "moveDot" + this.moveDots.Count.ToString() + this.pool_moveDot.Count.ToString();
		GameObject gameObject = new GameObject(name);
		gameObject.transform.SetParent(this.timeLineJointPanel.transform);
		RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
		rectTransform.anchorMin = new Vector2(0f, 1f);
		rectTransform.anchorMax = new Vector2(0f, 1f);
		moveDot.tooltipItem = gameObject.AddComponent<TooltipItem>();
		rectTransform.localScale = new Vector3(1f, 1f, 1f);
		rectTransform.sizeDelta = new Vector2(10f, 10f);
		moveDot.rectTransform = rectTransform;
		moveDot.Name = name;
		gameObject.AddComponent<CanvasRenderer>();
		moveDot.dotImage = gameObject.AddComponent<Image>();
		moveDot.Disable();
		return moveDot;
	}

	// Token: 0x06000F49 RID: 3913 RVA: 0x0005235F File Offset: 0x0005055F
	private void ReturnMoveDotToPool(MoveDot moveDot)
	{
		moveDot.Disable();
		this.pool_moveDot.Add(moveDot);
	}

	// Token: 0x06000F4A RID: 3914 RVA: 0x00052374 File Offset: 0x00050574
	private void FillJointFilters()
	{
		foreach (JointType jointType in from x in new List<JointType>((IEnumerable<JointType>)Enum.GetValues(typeof(JointType)))
		orderby x.GetDescription()
		select x)
		{
			MultiselectItem component = UnityEngine.Object.Instantiate<GameObject>(this.jointFilterItemPrefab, this.jointFilterPanel).GetComponent<MultiselectItem>();
			component.SetText(jointType.GetDescription());
			component.checkBox.isOn = false;
			component.value = jointType;
			component.checkBox.onValueChanged.AddListener(delegate(bool <p0>)
			{
				this.OnJointFilterValueChanged();
			});
			this.jointFilterItems.Add(component);
		}
		this.mainFilterPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)(80 + this.jointFilterItems.Count<MultiselectItem>() * 30));
		this.selectAllJointsFilterButton.onClick.AddListener(delegate()
		{
			this.SetAllJointFilters(true);
		});
		this.selectNoneJointsFilterButton.onClick.AddListener(delegate()
		{
			this.SetAllJointFilters(false);
		});
	}

	// Token: 0x06000F4B RID: 3915 RVA: 0x000524B0 File Offset: 0x000506B0
	private void SetAllJointFilters(bool selected)
	{
		foreach (MultiselectItem multiselectItem in this.jointFilterItems)
		{
			multiselectItem.checkBox.SetIsOnWithoutNotify(selected);
		}
		this.OnJointFilterValueChanged();
	}

	// Token: 0x06000F4C RID: 3916 RVA: 0x0005250C File Offset: 0x0005070C
	private void OnJointFilterValueChanged()
	{
		this.UpdateSelectedJointsList();
		this.UpdateTimeLineMoves();
	}

	// Token: 0x06000F4D RID: 3917 RVA: 0x0005251C File Offset: 0x0005071C
	private void UpdateSelectedJointsList()
	{
		this.selectedJointTypes.Clear();
		foreach (MultiselectItem multiselectItem in this.jointFilterItems)
		{
			if (multiselectItem.checkBox.isOn)
			{
				this.selectedJointTypes.Add((JointType)multiselectItem.value);
			}
		}
	}

	// Token: 0x06000F4E RID: 3918 RVA: 0x00052598 File Offset: 0x00050798
	private void Undo()
	{
		if (this.usingTwoHandedRig)
		{
			this.SwapTwoHandedRig(new bool?(false));
		}
		if (this.tempSingleMoves.Count > 0)
		{
			this.ClearTempSingleMoves();
			return;
		}
		CommandInvoker.UndoCommand();
	}

	// Token: 0x06000F4F RID: 3919 RVA: 0x000525C8 File Offset: 0x000507C8
	private void Redo()
	{
		CommandInvoker.RedoCommand();
	}

	// Token: 0x06000F50 RID: 3920 RVA: 0x000525D0 File Offset: 0x000507D0
	public bool CheckCurrentView(Stance stance, Move move, bool equipment = false)
	{
		if (equipment)
		{
			return true;
		}
		if (this.selectedStance == stance && this.selectedMove == move)
		{
			return true;
		}
		if (this.latestMoveMenuUpdate == Time.frameCount)
		{
			return false;
		}
		this.selectedStance = stance;
		this.selectedMove = move;
		this.ResetStance();
		this.UpdateMoveMenu(true);
		if (this.selectedMove != null)
		{
			this.CurrentTimeChanged("0", true);
		}
		return false;
	}

	// Token: 0x06000F51 RID: 3921 RVA: 0x00052638 File Offset: 0x00050838
	private void SetSelectedHandByRig(GameObject rigObject)
	{
		SimpleThreePointIKRig component = rigObject.GetComponent<SimpleThreePointIKRig>();
		if (component != null && (component.jointTypeTip == JointType.WRIST_LEFT || component.jointTypeTip == JointType.WRIST_RIGHT))
		{
			this.ShowHandStatePanel(component.jointTypeTip, false);
		}
	}

	// Token: 0x06000F52 RID: 3922 RVA: 0x00052678 File Offset: 0x00050878
	private void SetSelectedHandByMeshObject(GameObject meshObject)
	{
		if (meshObject != null)
		{
			if (meshObject.name == "WRIST_LEFT")
			{
				this.ShowHandStatePanel(JointType.WRIST_LEFT, false);
				return;
			}
			if (meshObject.name == "WRIST_RIGHT")
			{
				this.ShowHandStatePanel(JointType.WRIST_RIGHT, false);
			}
		}
	}

	// Token: 0x06000F53 RID: 3923 RVA: 0x000526C8 File Offset: 0x000508C8
	private void ShowHandStatePanel(JointType jointType, bool usedButton = false)
	{
		if (this.selectedStance != null && this.selectedMove != null)
		{
			this.selectedJointTypeForHandState = new JointType?(jointType);
			this.handStateSelectPanel.gameObject.SetActive(true);
			Vector2 vector = Mouse.current.position.ReadValue();
			if (usedButton)
			{
				vector.x -= this.handStateSelectPanel.rect.width / 2f;
				vector.y -= this.handStateSelectPanel.rect.height / 2f - 10f;
			}
			else
			{
				vector.x -= 10f;
			}
			this.handStateSelectPanel.anchoredPosition = new Vector2(vector.x, vector.y);
			HandState? currentHandStateForJointType = this.GetCurrentHandStateForJointType(this.selectedJointTypeForHandState.Value);
			UIHelpers.SetButtonColor(this.handStateSelectButtonHold, ButtonState.Basic, null, null);
			UIHelpers.SetButtonColor(this.handStateSelectButtonLooseHold, ButtonState.Basic, null, null);
			UIHelpers.SetButtonColor(this.handStateSelectButtonNoHold, ButtonState.Basic, null, null);
			UIHelpers.SetButtonColor(this.handStateSelectButtonEmptyHold, ButtonState.Basic, null, null);
			Button button = this.handStateSelectButtonEmptyHold;
			if (currentHandStateForJointType != null)
			{
				if (currentHandStateForJointType.Value == HandState.Hold)
				{
					button = this.handStateSelectButtonHold;
				}
				else if (currentHandStateForJointType.Value == HandState.LooseHold)
				{
					button = this.handStateSelectButtonLooseHold;
				}
				else if (currentHandStateForJointType.Value == HandState.NoHold)
				{
					button = this.handStateSelectButtonNoHold;
				}
			}
			UIHelpers.SetButtonColor(button, ButtonState.Selected, null, null);
		}
	}

	// Token: 0x06000F54 RID: 3924 RVA: 0x00052830 File Offset: 0x00050A30
	public void HideHandStatePanel()
	{
		this.selectedJointTypeForHandState = null;
		if (this.handStateSelectPanel.gameObject.activeInHierarchy)
		{
			this.handStateSelectPanel.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000F55 RID: 3925 RVA: 0x00052864 File Offset: 0x00050A64
	private void SetHandHoldState(HandState? handState = null)
	{
		if (this.selectedJointTypeForHandState != null && this.selectedStance != null && this.selectedMove != null)
		{
			double currentTime = Math.Round((double)this.timeLineSlider.value, 2);
			JointMove jointMove = (from x in this.selectedMove.jointMoveList
			where x.joint == this.selectedJointTypeForHandState.Value && Generic.DoubleEquals(x.executionTime, currentTime)
			select x).FirstOrDefault<JointMove>();
			if (jointMove != null)
			{
				CommandInvoker.ExecuteCommand(new SetKeyframeHandStateCommand(this.selectedStance, this.selectedMove, jointMove, handState), false);
			}
			else if (handState != null)
			{
				FighterJoint fighterJoint = (from x in this.animator.FighterJoints
				where x.jointType == this.selectedJointTypeForHandState.Value
				select x).FirstOrDefault<FighterJoint>();
				if (fighterJoint != null)
				{
					JointMove jointMove2 = new JointMove
					{
						joint = fighterJoint.jointType,
						executionTime = currentTime,
						temp = false,
						targetRotation = new NullableVector3(null, null, null),
						handState = handState
					};
					jointMove2.targetRotation.x = new float?(fighterJoint.joint.transform.localEulerAngles.x);
					jointMove2.targetRotation.y = new float?(fighterJoint.joint.transform.localEulerAngles.y);
					jointMove2.targetRotation.z = new float?(fighterJoint.joint.transform.localEulerAngles.z);
					CommandInvoker.ExecuteCommand(new SetKeyframesCommand(this.selectedStance, this.selectedMove, new List<JointMove>
					{
						jointMove2
					}, new List<JointMove>(), null, false), false);
				}
			}
		}
		this.HideHandStatePanel();
	}

	// Token: 0x06000F56 RID: 3926 RVA: 0x00052A30 File Offset: 0x00050C30
	private HandState? GetCurrentHandStateForJointType(JointType jointType)
	{
		if (this.selectedMove != null && this.selectedMove.jointMoveList != null)
		{
			JointMove jointMove = (from x in this.selectedMove.jointMoveList
			where x.joint == jointType && Generic.DoubleEquals(x.executionTime, (double)this.timeLineSlider.value)
			select x).FirstOrDefault<JointMove>();
			if (jointMove != null)
			{
				return jointMove.handState;
			}
		}
		return null;
	}

	// Token: 0x06000F57 RID: 3927 RVA: 0x00052A9C File Offset: 0x00050C9C
	private void UpdateHandStateUI()
	{
		HandState? currentHandStateForJointType = this.GetCurrentHandStateForJointType(JointType.WRIST_LEFT);
		HandState? currentHandStateForJointType2 = this.GetCurrentHandStateForJointType(JointType.WRIST_RIGHT);
		string text = "";
		string text2 = "";
		if (currentHandStateForJointType != null)
		{
			text = currentHandStateForJointType.GetDescription();
		}
		if (currentHandStateForJointType2 != null)
		{
			text2 = currentHandStateForJointType2.GetDescription();
		}
		this.currentHandStateLeft.text = text;
		this.currentHandStateRight.text = text2;
	}

	// Token: 0x06000F58 RID: 3928 RVA: 0x00052B0C File Offset: 0x00050D0C
	private void LoadGameSettings()
	{
		if (IGameSettingsManager.singleton != null && this.moveSet != null)
		{
			IGameSettingsManager.singleton.LoadGameSettings();
			IGameSettingsManager.singleton.GameType = this.moveSet.gameType;
			IGameSettingsManager.singleton.UseStamina = this.moveSet.stamina;
		}
		this.RefreshStaminaManager();
	}

	// Token: 0x06000F59 RID: 3929 RVA: 0x00052B64 File Offset: 0x00050D64
	private void OnTimeScaleChanged()
	{
		float floatValue = this.timeScaleDropdown.options[this.timeScaleDropdown.value].GetFloatValue();
		IGameSettingsManager.singleton.TimeScaleMin = floatValue;
		GeneralManager.singleton.SetTimeScale(floatValue);
	}

	// Token: 0x06000F5A RID: 3930 RVA: 0x00052BA8 File Offset: 0x00050DA8
	private void GameTypeChanged()
	{
		ButtonOption getCurrentValue = this.gameTypeSelect.GetCurrentValue;
		if (getCurrentValue != null)
		{
			CommandInvoker.ExecuteCommand(new MoveSetGameTypeChangeCommand(this.moveSet, (GameTypes)getCurrentValue.optionIntValue), false);
		}
	}

	// Token: 0x06000F5B RID: 3931 RVA: 0x00052BDB File Offset: 0x00050DDB
	private void OnDismembermentChanged()
	{
		if (IGameSettingsManager.singleton != null)
		{
			IGameSettingsManager.singleton.UseDismemberment = this.dismembermentToggle.isOn;
		}
		if (CutManager.singleton != null)
		{
			CutManager.ResetCutManagerActive();
		}
	}

	// Token: 0x06000F5C RID: 3932 RVA: 0x00052C0C File Offset: 0x00050E0C
	private void OnStaminaChanged()
	{
		bool isOn = this.staminaToggle.isOn;
		CommandInvoker.ExecuteCommand(new MoveSetStaminaChangeCommand(this.moveSet, isOn), false);
	}

	// Token: 0x06000F5D RID: 3933 RVA: 0x00052C37 File Offset: 0x00050E37
	public void RefreshStaminaManager()
	{
		if (StaminaManager.singleton != null)
		{
			StaminaManager.singleton.RefreshStaminaManagerActive();
			StaminaManager.singleton.SetInitialStrengths();
		}
		if (this.staminaHudCanvas != null)
		{
			this.staminaHudCanvas.UpdateCanvasVisibility();
		}
	}

	// Token: 0x06000F5E RID: 3934 RVA: 0x00052C74 File Offset: 0x00050E74
	public void CheckForcedSettingValues()
	{
		if (IGameSettingsManager.singleton.GameType == GameTypes.Legacy)
		{
			if (this.staminaToggle.isOn)
			{
				this.staminaToggle.SetIsOnWithoutNotify(false);
			}
			this.staminaToggle.interactable = false;
			return;
		}
		this.staminaToggle.SetIsOnWithoutNotify(IGameSettingsManager.singleton.UseStamina);
		this.staminaToggle.interactable = true;
	}

	// Token: 0x06000F5F RID: 3935 RVA: 0x00052CD5 File Offset: 0x00050ED5
	private void OnDestroy()
	{
		this.DisposeUserControls();
	}

	// Token: 0x06000F60 RID: 3936 RVA: 0x00052CDD File Offset: 0x00050EDD
	public void DisposeUserControls()
	{
		if (this.userControls != null)
		{
			this.userControls.Disable();
			this.userControls.Dispose();
		}
		if (this.userControlsMap != null)
		{
			this.userControlsMap.Disable();
			this.userControlsMap.Dispose();
		}
	}

	// Token: 0x04000AA3 RID: 2723
	public GameObject physicsFighter;

	// Token: 0x04000AA4 RID: 2724
	public MoveSet moveSet;

	// Token: 0x04000AA5 RID: 2725
	private Stance selectedStance;

	// Token: 0x04000AA6 RID: 2726
	public GameObject buttonPrefab;

	// Token: 0x04000AA7 RID: 2727
	public GameObject stanceSelectPanel;

	// Token: 0x04000AA8 RID: 2728
	public GameObject stanceSelectPanelList;

	// Token: 0x04000AA9 RID: 2729
	public GameObject moveSelectPanel;

	// Token: 0x04000AAA RID: 2730
	public GameObject moveSelectPanelList;

	// Token: 0x04000AAB RID: 2731
	public GameObject moveEditorPanel;

	// Token: 0x04000AAC RID: 2732
	public GameObject singleMoveListPanel;

	// Token: 0x04000AAD RID: 2733
	public GameObject stanceEditorPrefab;

	// Token: 0x04000AAE RID: 2734
	public GameObject moveEditorPrefab;

	// Token: 0x04000AAF RID: 2735
	public GameObject changeStanceEditorPrefab;

	// Token: 0x04000AB0 RID: 2736
	public GameObject singleMoveEditorPrefab;

	// Token: 0x04000AB1 RID: 2737
	private Move selectedMove;

	// Token: 0x04000AB2 RID: 2738
	public SelectedJoint selectedJoint;

	// Token: 0x04000AB3 RID: 2739
	public InputField selectedMoveNameEditor;

	// Token: 0x04000AB4 RID: 2740
	public Dropdown selectedMoveLayerEditor;

	// Token: 0x04000AB5 RID: 2741
	public Dropdown selectedMoveInputTypeEditor;

	// Token: 0x04000AB6 RID: 2742
	public JointMove selectedSingleMove;

	// Token: 0x04000AB7 RID: 2743
	public JointMove tempSingleMove;

	// Token: 0x04000AB8 RID: 2744
	public List<JointMove> tempSingleMoves = new List<JointMove>();

	// Token: 0x04000AB9 RID: 2745
	public PlayerAnimator animator;

	// Token: 0x04000ABA RID: 2746
	public Toggle activeAnimationToggle;

	// Token: 0x04000ABB RID: 2747
	public InputField MoveDurationField;

	// Token: 0x04000ABC RID: 2748
	public InputField MoveCurrentTimeField;

	// Token: 0x04000ABD RID: 2749
	public GameObject physicsCamera;

	// Token: 0x04000ABE RID: 2750
	public GameObject animationCamera;

	// Token: 0x04000ABF RID: 2751
	public Camera cameraEquipment;

	// Token: 0x04000AC0 RID: 2752
	private Vector2 previousMousePosition;

	// Token: 0x04000AC1 RID: 2753
	public float physicsZoomLevel = -5f;

	// Token: 0x04000AC2 RID: 2754
	public float animationZoomLevel = -5f;

	// Token: 0x04000AC3 RID: 2755
	public float activeZoomLevel = -5f;

	// Token: 0x04000AC4 RID: 2756
	public Transform focusedPhysicsCameraTarget;

	// Token: 0x04000AC5 RID: 2757
	public Transform focusedAnimationCameraTarget;

	// Token: 0x04000AC6 RID: 2758
	public Transform physicsCameraTarget;

	// Token: 0x04000AC7 RID: 2759
	public Transform animationCameraTarget;

	// Token: 0x04000AC8 RID: 2760
	public GameObject activeCamera;

	// Token: 0x04000AC9 RID: 2761
	public Transform activeCameraTarget;

	// Token: 0x04000ACA RID: 2762
	public Canvas timelineCanvas;

	// Token: 0x04000ACB RID: 2763
	public Slider timeLineSlider;

	// Token: 0x04000ACC RID: 2764
	public GameObject timeLineMovesPanel;

	// Token: 0x04000ACD RID: 2765
	public GameObject timeLineInputHolder;

	// Token: 0x04000ACE RID: 2766
	public GameObject physicsRotationGizmo;

	// Token: 0x04000ACF RID: 2767
	public GameObject animationRotationGizmo;

	// Token: 0x04000AD0 RID: 2768
	public string draggingPhysicalAxis;

	// Token: 0x04000AD1 RID: 2769
	public string draggingAnimationAxis;

	// Token: 0x04000AD2 RID: 2770
	public CanvasScaler editorCanvasScaler;

	// Token: 0x04000AD4 RID: 2772
	private UserControls userControlsMap;

	// Token: 0x04000AD5 RID: 2773
	public GameObject EquipmentEditorPrefab;

	// Token: 0x04000AD6 RID: 2774
	public EquipmentPanel equipmentPanel;

	// Token: 0x04000AD7 RID: 2775
	public GameObject panelsHolder;

	// Token: 0x04000AD8 RID: 2776
	public GameObject moveSetPanelGameObject;

	// Token: 0x04000AD9 RID: 2777
	public Button moveSetEditorButton;

	// Token: 0x04000ADA RID: 2778
	public Button equipmentEditorButton;

	// Token: 0x04000ADB RID: 2779
	public Button testMoveSetButton;

	// Token: 0x04000ADC RID: 2780
	public PlayerHealth playerHealth;

	// Token: 0x04000ADD RID: 2781
	public InputField moveSetNameInputField;

	// Token: 0x04000ADE RID: 2782
	public GameObject infoDialogPrefab;

	// Token: 0x04000ADF RID: 2783
	public GameObject confirmDialogPrefab;

	// Token: 0x04000AE0 RID: 2784
	private Vector2 resolution;

	// Token: 0x04000AE1 RID: 2785
	public UserControls userControls;

	// Token: 0x04000AE2 RID: 2786
	private global::PlayerInputManager playerInputManager;

	// Token: 0x04000AE3 RID: 2787
	public GameObject inputManager;

	// Token: 0x04000AE4 RID: 2788
	public Button playSelectedMoveButton;

	// Token: 0x04000AE5 RID: 2789
	[Header("General")]
	public Dropdown timeScaleDropdown;

	// Token: 0x04000AE6 RID: 2790
	public Toggle staminaToggle;

	// Token: 0x04000AE7 RID: 2791
	public ButtonOptionSelect gameTypeSelect;

	// Token: 0x04000AE8 RID: 2792
	public Toggle dismembermentToggle;

	// Token: 0x04000AE9 RID: 2793
	[Header("MoveSetEditor")]
	public static MoveSetEditor singleton;

	// Token: 0x04000AEA RID: 2794
	[Header("RIG")]
	public EditMode rigEditMode;

	// Token: 0x04000AEB RID: 2795
	public Button swapEditModeButton;

	// Token: 0x04000AEC RID: 2796
	public Button swapRigButton;

	// Token: 0x04000AED RID: 2797
	public Button twoHandedRigButton;

	// Token: 0x04000AEE RID: 2798
	public bool usingTwoHandedRig;

	// Token: 0x04000AEF RID: 2799
	public bool usingIKRig = true;

	// Token: 0x04000AF0 RID: 2800
	public RigManager rigManager;

	// Token: 0x04000AF1 RID: 2801
	public GameObject animationPositionGizmo;

	// Token: 0x04000AF2 RID: 2802
	public SelectedRigTarget selectedRigTarget;

	// Token: 0x04000AF3 RID: 2803
	public SideToolButtons sideToolButtons;

	// Token: 0x04000AF4 RID: 2804
	public StaminaHudCanvas staminaHudCanvas;

	// Token: 0x04000AF5 RID: 2805
	private RotationalLabel selectedRotationLabel;

	// Token: 0x04000AF6 RID: 2806
	private Vector2 previousMousePos;

	// Token: 0x04000AF7 RID: 2807
	public bool rotatingCamera;

	// Token: 0x04000AF8 RID: 2808
	public bool draggingMoveDot;

	// Token: 0x04000AF9 RID: 2809
	public MoveKeyframesCommand dragKeyframesCommand;

	// Token: 0x04000AFA RID: 2810
	public bool draggingCurrentTime;

	// Token: 0x04000AFB RID: 2811
	public bool updateAnimation;

	// Token: 0x04000AFC RID: 2812
	public Vector2? selectionStart;

	// Token: 0x04000AFD RID: 2813
	private float cameraMoveMultiplier = 10f;

	// Token: 0x04000AFE RID: 2814
	private float? distanceToGizmoHoldPosition;

	// Token: 0x04000AFF RID: 2815
	private EditorMode previousEditorMode;

	// Token: 0x04000B00 RID: 2816
	private int latestMoveMenuUpdate;

	// Token: 0x04000B01 RID: 2817
	private string validationInfo = "";

	// Token: 0x04000B02 RID: 2818
	private float movePlayStarted;

	// Token: 0x04000B03 RID: 2819
	private bool playingSelectedMove;

	// Token: 0x04000B04 RID: 2820
	private Move listenActionForMove;

	// Token: 0x04000B05 RID: 2821
	private BasicInfoDialog listenActionForMoveDialog;

	// Token: 0x04000B06 RID: 2822
	private Dropdown listenActionForMoveDropdown;

	// Token: 0x04000B07 RID: 2823
	private RectTransform timeLineHolderRectTransform;

	// Token: 0x04000B08 RID: 2824
	public List<MoveDot> moveDots;

	// Token: 0x04000B09 RID: 2825
	private GameObject timeLineRowPanel;

	// Token: 0x04000B0A RID: 2826
	private RectTransform timeLineRowRectTransform;

	// Token: 0x04000B0B RID: 2827
	private GameObject timeLineJointPanel;

	// Token: 0x04000B0C RID: 2828
	private RectTransform timeLineJointRectTransform;

	// Token: 0x04000B0D RID: 2829
	private GameObject timeLineIndicatorPanel;

	// Token: 0x04000B0E RID: 2830
	private RectTransform timeLineRectTransform;

	// Token: 0x04000B0F RID: 2831
	private GameObject timeLinePlayMoveIndicatorPanel;

	// Token: 0x04000B10 RID: 2832
	private RectTransform timeLinePlayMoveRectTransform;

	// Token: 0x04000B11 RID: 2833
	private List<JointMove> selectedJointMoves = new List<JointMove>();

	// Token: 0x04000B12 RID: 2834
	private float cameraWidth;

	// Token: 0x04000B13 RID: 2835
	private float cameraHeight;

	// Token: 0x04000B14 RID: 2836
	public bool reActivateRig;

	// Token: 0x04000B15 RID: 2837
	public List<MoveDot> pool_moveDot;

	// Token: 0x04000B16 RID: 2838
	[Header("Filters")]
	public RectTransform mainFilterPanel;

	// Token: 0x04000B17 RID: 2839
	public GameObject jointFilterItemPrefab;

	// Token: 0x04000B18 RID: 2840
	public RectTransform jointFilterPanel;

	// Token: 0x04000B19 RID: 2841
	private List<MultiselectItem> jointFilterItems = new List<MultiselectItem>();

	// Token: 0x04000B1A RID: 2842
	private List<JointType> selectedJointTypes = new List<JointType>();

	// Token: 0x04000B1B RID: 2843
	public Button selectAllJointsFilterButton;

	// Token: 0x04000B1C RID: 2844
	public Button selectNoneJointsFilterButton;

	// Token: 0x04000B1D RID: 2845
	[Header("Hand stuff")]
	public RectTransform handStateSelectPanel;

	// Token: 0x04000B1E RID: 2846
	public Button handStateSelectButtonHold;

	// Token: 0x04000B1F RID: 2847
	public Button handStateSelectButtonLooseHold;

	// Token: 0x04000B20 RID: 2848
	public Button handStateSelectButtonNoHold;

	// Token: 0x04000B21 RID: 2849
	public Button handStateSelectButtonEmptyHold;

	// Token: 0x04000B22 RID: 2850
	public Text currentHandStateRight;

	// Token: 0x04000B23 RID: 2851
	public Text currentHandStateLeft;

	// Token: 0x04000B24 RID: 2852
	public Button buttonSetRightHandState;

	// Token: 0x04000B25 RID: 2853
	public Button buttonSetLeftHandState;

	// Token: 0x04000B26 RID: 2854
	private JointType? selectedJointTypeForHandState;
}
