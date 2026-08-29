using System;
using System.Collections.Generic;
using System.Linq;
using MoveClasses;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;
using Utils;

// Token: 0x020000F4 RID: 244
public class TutorialManager : MonoBehaviour
{
	// Token: 0x06000823 RID: 2083 RVA: 0x00028594 File Offset: 0x00026794
	private void Start()
	{
		this.userControls = SettingsHelper.GetUserControls();
		this.userControls.Generic.Enable();
		Cursor.lockState = CursorLockMode.Locked;
		this.GetMoveSet();
		this.GenerateUI();
		this.UpdatePreviousYRotation();
	}

	// Token: 0x06000824 RID: 2084 RVA: 0x000285D8 File Offset: 0x000267D8
	private void Update()
	{
		if (this.CheckActiveMoves())
		{
			if (this.CheckGroupTasksDone())
			{
				this.CheckAllTasksDone();
			}
			else
			{
				this.UpdateUI();
			}
		}
		this.CheckPlayerPosition();
		this.CheckPlayerAlive();
		if (this.userControls.Generic.OpenMenu.WasPerformedThisFrame())
		{
			this.LeaveConfirm();
		}
	}

	// Token: 0x06000825 RID: 2085 RVA: 0x00028630 File Offset: 0x00026830
	private void CheckPlayerPosition()
	{
		if (this.playerHealth != null)
		{
			float num = this.maxDistanceFromOrigin;
			if (this.currentTaskGroup >= 3)
			{
				num = this.maxDistanceFromOriginWithTarget;
			}
			if (Vector3.Distance(default(Vector3), this.playerHealth.cameraPositionPoint.transform.position) > num)
			{
				float num2 = this.playerHealth.cameraPositionPoint.transform.position.y + 0.2f;
				if (num2 < 1f)
				{
					num2 = 1.2f;
				}
				this.playerHealth.cameraPositionPoint.transform.position = new Vector3(0f, num2, 0f);
			}
			if (this.targetPlayerHealth != null && Vector3.Distance(this.playerHealth.cameraPositionPoint.transform.position, this.targetPlayerHealth.cameraPositionPoint.transform.position) > this.maxDistanceFromTarget)
			{
				Vector3 position = this.PositionForKillTarget();
				position.y += 1f;
				this.targetPlayerHealth.cameraPositionPoint.transform.position = position;
			}
		}
	}

	// Token: 0x06000826 RID: 2086 RVA: 0x00028751 File Offset: 0x00026951
	private void CheckPlayerAlive()
	{
		if (this.playerHealth != null && !this.playerHealth.alive && !this.playerDeathRegistered)
		{
			this.playerDeathRegistered = true;
			base.Invoke("PlayerDied", 2f);
		}
	}

	// Token: 0x06000827 RID: 2087 RVA: 0x00028790 File Offset: 0x00026990
	private void SetupPlayer()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.playerPrefab, new Vector3(0f, 0.5f, 0f), default(Quaternion));
		this.playerHealth = gameObject.GetComponent<PlayerHealth>();
		this.playerHealth.playerAnimator.SetMoveSet(this.tutorialMoveSet, false, false);
		this.playerHealth.SetEquipment(this.tutorialMoveSet.defaultEquipment, false);
		this.playerHealth.playerNum = 1;
		this.playerHealth.playerName = LocalizationHelpers.LocalizedText("txt_local_playername", new object[]
		{
			this.playerHealth.playerNum
		});
		this.playerHealth.OnlyPhysical();
		this.SetupCamera();
		this.SetupInputManager();
		StaminaManager.RegisterPlayerHealths(new List<PlayerHealth>
		{
			this.playerHealth
		});
	}

	// Token: 0x06000828 RID: 2088 RVA: 0x00028868 File Offset: 0x00026A68
	private void SetupCamera()
	{
		CameraSmoothFollowControllable cameraSmoothFollowControllable = Camera.main.gameObject.AddComponent<CameraSmoothFollowControllable>();
		this.playerHealth.cameraSmoothFollow = cameraSmoothFollowControllable;
		Camera.main.gameObject.AddComponent<LowResCamera>();
		Camera.main.gameObject.AddComponent<CameraSettings>();
		if (this.playerHealth != null)
		{
			cameraSmoothFollowControllable.SetTarget(this.playerHealth.cameraPoint, this.playerHealth.cameraPositionPoint);
		}
	}

	// Token: 0x06000829 RID: 2089 RVA: 0x000288DC File Offset: 0x00026ADC
	private void SetupInputManager()
	{
		InputUser.listenForUnpairedDeviceActivity++;
		InputUser.onUnpairedDeviceUsed += this.HandleDetectedDevice;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.inputManagerPrefab);
		this.inputManager = gameObject.GetComponent<global::PlayerInputManager>();
		this.inputManager.ConnectToPlayerCharacter(this.playerHealth.gameObject);
	}

	// Token: 0x0600082A RID: 2090 RVA: 0x00028934 File Offset: 0x00026B34
	private void GetMoveSet()
	{
		this.currentTaskGroup = 0;
		this.tutorialTaskGroups = new List<TutorialTaskGroup>();
		this.tutorialMoveSet = MoveSetHelpers.GetTutorialMoveSet();
		this.SetupPlayer();
		TutorialTaskGroup tutorialTaskGroup = new TutorialTaskGroup();
		this.tutorialTaskGroups.Add(tutorialTaskGroup);
		tutorialTaskGroup.tutorialTasks = new List<TutorialTask>();
		tutorialTaskGroup.title = LocalizationHelpers.LocalizedText("tutorial_task_title_move", Array.Empty<object>());
		tutorialTaskGroup.number = 0;
		string[] source = new string[]
		{
			"Move_Forward",
			"Move_Back",
			"Move_Left",
			"Move_Right"
		};
		foreach (Move move in this.tutorialMoveSet.stanceList[0].moveList)
		{
			if (!string.IsNullOrEmpty(move.name) && source.Contains(move.playerInput))
			{
				TutorialTask tutorialTask = new TutorialTask();
				tutorialTaskGroup.tutorialTasks.Add(tutorialTask);
				tutorialTask.move = move;
			}
		}
		TutorialTask tutorialTask2 = new TutorialTask();
		tutorialTaskGroup.tutorialTasks.Add(tutorialTask2);
		tutorialTask2.taskType = TutorialTaskType.Turn;
		tutorialTask2.inputString = "Turn_Left";
		TutorialTask tutorialTask3 = new TutorialTask();
		tutorialTaskGroup.tutorialTasks.Add(tutorialTask3);
		tutorialTask3.taskType = TutorialTaskType.Turn;
		tutorialTask3.inputString = "Turn_Right";
		tutorialTask3.positive = true;
		tutorialTaskGroup.mouseTip = LocalizationHelpers.LocalizedText("text_tutorial_tip_mouse_turn", Array.Empty<object>());
		TutorialTaskGroup tutorialTaskGroup2 = new TutorialTaskGroup();
		this.tutorialTaskGroups.Add(tutorialTaskGroup2);
		tutorialTaskGroup2.tutorialTasks = new List<TutorialTask>();
		tutorialTaskGroup2.title = LocalizationHelpers.LocalizedText("tutorial_task_title_attack", Array.Empty<object>());
		tutorialTaskGroup2.number = 0;
		string[] source2 = new string[]
		{
			"Action3",
			"Action8",
			"Action1",
			"Action5",
			"Action2",
			"Action4",
			"Action6"
		};
		foreach (Move move2 in from x in this.tutorialMoveSet.stanceList[0].moveList
		orderby x.name
		select x)
		{
			if (!string.IsNullOrEmpty(move2.playerInput) && source2.Contains(move2.playerInput))
			{
				TutorialTask tutorialTask4 = new TutorialTask();
				tutorialTaskGroup2.tutorialTasks.Add(tutorialTask4);
				tutorialTask4.move = move2;
			}
		}
		tutorialTaskGroup2.mouseTip = LocalizationHelpers.LocalizedText("text_tutorial_tip_mouse_attack", new object[]
		{
			this.GetTextForInput("Directional_Action1", Mouse.current),
			this.GetTextForInput("Directional_Action2", Mouse.current)
		});
		TutorialTaskGroup tutorialTaskGroup3 = new TutorialTaskGroup();
		this.tutorialTaskGroups.Add(tutorialTaskGroup3);
		tutorialTaskGroup3.tutorialTasks = new List<TutorialTask>();
		tutorialTaskGroup3.title = LocalizationHelpers.LocalizedText("tutorial_task_title_block", Array.Empty<object>());
		tutorialTaskGroup3.number = 0;
		tutorialTaskGroup3.inputString = "Action10";
		string[] source3 = new string[]
		{
			"Action3",
			"Action8",
			"Action1",
			"Action5"
		};
		foreach (Move move3 in this.tutorialMoveSet.stanceList[1].moveList)
		{
			if (!string.IsNullOrEmpty(move3.playerInput) && source3.Contains(move3.playerInput))
			{
				TutorialTask tutorialTask5 = new TutorialTask();
				tutorialTaskGroup3.tutorialTasks.Add(tutorialTask5);
				tutorialTask5.move = move3;
			}
		}
		tutorialTaskGroup3.mouseTip = LocalizationHelpers.LocalizedText("text_tutorial_tip_mouse_block", new object[]
		{
			this.GetTextForInput(tutorialTaskGroup3.inputString, Keyboard.current),
			this.GetTextForInput("Directional_Action1", Mouse.current)
		});
		TutorialTaskGroup tutorialTaskGroup4 = new TutorialTaskGroup();
		this.tutorialTaskGroups.Add(tutorialTaskGroup4);
		tutorialTaskGroup4.tutorialTasks = new List<TutorialTask>();
		tutorialTaskGroup4.title = LocalizationHelpers.LocalizedText("tutorial_task_title_final", Array.Empty<object>());
		tutorialTaskGroup4.number = 0;
		TutorialTask tutorialTask6 = new TutorialTask();
		tutorialTaskGroup4.tutorialTasks.Add(tutorialTask6);
		tutorialTask6.taskType = TutorialTaskType.Kill;
	}

	// Token: 0x0600082B RID: 2091 RVA: 0x00028DB0 File Offset: 0x00026FB0
	private void GenerateUI()
	{
		foreach (object obj in this.tutorialUIListHolder.transform)
		{
			UnityEngine.Object.Destroy(((Transform)obj).gameObject);
		}
		foreach (TutorialTask tutorialTask in this.tutorialTaskGroups[this.currentTaskGroup].tutorialTasks)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.tutorialTaskPanelPrefab, this.tutorialUIListHolder.transform);
			tutorialTask.row = gameObject.GetComponent<TutorialTaskRow>();
			tutorialTask.row.task = tutorialTask;
			if (tutorialTask.taskType == TutorialTaskType.Kill)
			{
				this.CreateTargetForKillTask(tutorialTask);
			}
		}
		this.tutorialPanel.sizeDelta = new Vector2(this.tutorialPanel.sizeDelta.x, (float)((this.tutorialTaskGroups[this.currentTaskGroup].tutorialTasks.Count + 1) * 50));
		this.taskTitle.text = this.tutorialTaskGroups[this.currentTaskGroup].title;
		this.UpdateUI();
	}

	// Token: 0x0600082C RID: 2092 RVA: 0x00028F04 File Offset: 0x00027104
	private void UpdateUI()
	{
		foreach (TutorialTask tutorialTask in this.tutorialTaskGroups[this.currentTaskGroup].tutorialTasks)
		{
			string text = "";
			if (tutorialTask.taskType == TutorialTaskType.Move)
			{
				string text2 = "";
				if (!string.IsNullOrEmpty(this.tutorialTaskGroups[this.currentTaskGroup].inputString))
				{
					text2 = LocalizationHelpers.LocalizedText("tutorial_task_hold_input", new object[]
					{
						this.GetTextForInput(this.tutorialTaskGroups[this.currentTaskGroup].inputString, null)
					});
				}
				text += LocalizationHelpers.LocalizedText("tutorial_task_press_input_to_do_action", new object[]
				{
					this.GetTextForInput(tutorialTask.move.playerInput, null),
					tutorialTask.move.name,
					text2
				});
			}
			else if (tutorialTask.taskType == TutorialTaskType.Turn)
			{
				string text3 = "";
				if (tutorialTask.inputString.Contains("Left"))
				{
					text3 += LocalizationHelpers.LocalizedText("Turn_Left", Array.Empty<object>());
				}
				else
				{
					text3 += LocalizationHelpers.LocalizedText("Turn_Right", Array.Empty<object>());
				}
				text += LocalizationHelpers.LocalizedText("tutorial_task_press_input_to_do_action", new object[]
				{
					this.GetTextForInput(tutorialTask.inputString, null),
					text3,
					""
				});
			}
			else if (tutorialTask.taskType == TutorialTaskType.Kill)
			{
				text = LocalizationHelpers.LocalizedText("tutorial_task_type_kill", Array.Empty<object>());
			}
			tutorialTask.row.UpdateTaskText(text);
		}
		this.mouseTipText.text = this.tutorialTaskGroups[this.currentTaskGroup].mouseTip;
	}

	// Token: 0x0600082D RID: 2093 RVA: 0x000290F0 File Offset: 0x000272F0
	private string GetTextForInput(string inputString, InputDevice device = null)
	{
		if (device == null)
		{
			device = this.currentDevice;
		}
		string text = "";
		InputAction inputAction = this.inputManager.userControls.FindAction("PlayerActionMap/" + inputString, false);
		if (inputAction != null)
		{
			List<InputControl> list = null;
			if (device != null)
			{
				list = (from x in inputAction.controls
				where x.device == device
				select x).ToList<InputControl>();
			}
			if (list == null)
			{
				list = inputAction.controls.ToList<InputControl>();
			}
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (i > 0)
					{
						text += LocalizationHelpers.LocalizedText("tutorial_list_or_separator", Array.Empty<object>());
					}
					text += list[i].displayName;
				}
			}
		}
		return text;
	}

	// Token: 0x0600082E RID: 2094 RVA: 0x000291D0 File Offset: 0x000273D0
	private void CreateTargetForKillTask(TutorialTask task)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.playerPrefab, this.PositionForKillTarget(), this.RotationForKillTarget());
		task.target = gameObject.GetComponent<PlayerHealth>();
		task.target.dummyTarget = true;
		this.targetPlayerHealth = task.target;
		task.target.OnlyPhysical();
	}

	// Token: 0x0600082F RID: 2095 RVA: 0x00029224 File Offset: 0x00027424
	private Vector3 PositionForKillTarget()
	{
		Vector3 result = new Vector3(0f, 0f, 5f);
		result = this.playerHealth.cameraPositionPoint.transform.position + this.playerHealth.cameraPoint.transform.forward * 4f;
		result.y = 0.5f;
		return result;
	}

	// Token: 0x06000830 RID: 2096 RVA: 0x0002928E File Offset: 0x0002748E
	private Quaternion RotationForKillTarget()
	{
		return Quaternion.LookRotation(this.playerHealth.cameraPoint.transform.forward * -1f);
	}

	// Token: 0x06000831 RID: 2097 RVA: 0x000292B4 File Offset: 0x000274B4
	private bool CheckActiveMoves()
	{
		bool result = false;
		using (List<TutorialTask>.Enumerator enumerator = this.tutorialTaskGroups[this.currentTaskGroup].tutorialTasks.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				TutorialTask task = enumerator.Current;
				if (!task.done)
				{
					if (task.taskType == TutorialTaskType.Move)
					{
						if ((from x in this.playerHealth.playerAnimator.RunningSingleMoves
						where x.move.guid == task.move.guid
						select x).FirstOrDefault<RunningSingleMove>() != null)
						{
							if (task.startTime == null)
							{
								task.startTime = new float?(Time.time);
							}
							else
							{
								float time = Time.time;
								float? num = task.startTime + 0.5f;
								if (time > num.GetValueOrDefault() & num != null)
								{
									task.done = true;
									result = true;
								}
							}
						}
						else
						{
							task.startTime = null;
						}
					}
					else if (task.taskType == TutorialTaskType.Turn)
					{
						float num2 = this.playerHealth.cameraPoint.transform.rotation.eulerAngles.y - this.previousYRotation;
						if (num2 > 180f)
						{
							num2 -= 360f;
						}
						if (num2 < -180f)
						{
							num2 += 360f;
						}
						if (!task.positive)
						{
							num2 *= -1f;
						}
						if (num2 > 0f)
						{
							task.floatValue += num2;
							if (task.floatValue > 45f)
							{
								task.done = true;
								result = true;
							}
						}
					}
					else if (task.taskType == TutorialTaskType.Kill && !task.target.alive)
					{
						task.done = true;
						result = true;
					}
				}
			}
		}
		this.UpdatePreviousYRotation();
		return result;
	}

	// Token: 0x06000832 RID: 2098 RVA: 0x00029514 File Offset: 0x00027714
	private void UpdatePreviousYRotation()
	{
		if (this.playerHealth != null)
		{
			this.previousYRotation = this.playerHealth.cameraPoint.transform.rotation.eulerAngles.y;
		}
	}

	// Token: 0x06000833 RID: 2099 RVA: 0x00029558 File Offset: 0x00027758
	private bool CheckGroupTasksDone()
	{
		return (from x in this.tutorialTaskGroups[this.currentTaskGroup].tutorialTasks
		where !x.done
		select x).FirstOrDefault<TutorialTask>() == null;
	}

	// Token: 0x06000834 RID: 2100 RVA: 0x000295A8 File Offset: 0x000277A8
	private void CheckAllTasksDone()
	{
		if (this.currentTaskGroup < this.tutorialTaskGroups.Count - 1)
		{
			this.currentTaskGroup++;
			this.GenerateUI();
			return;
		}
		this.UpdateUI();
		base.Invoke("TutorialDone", 2f);
	}

	// Token: 0x06000835 RID: 2101 RVA: 0x000295F8 File Offset: 0x000277F8
	private void TutorialDone()
	{
		BasicConfirmDialog basicConfirmDialog = GeneralManager.CreateConfirmDialog(LocalizationHelpers.LocalizedText("confirm_txt_tutorial_done", Array.Empty<object>()), LocalizationHelpers.LocalizedText("confirm_title_tutorial_done", Array.Empty<object>()), true);
		basicConfirmDialog.okButton.onClick.AddListener(delegate()
		{
			this.LeaveTutorial();
		});
		basicConfirmDialog.okButton.Select();
	}

	// Token: 0x06000836 RID: 2102 RVA: 0x00029650 File Offset: 0x00027850
	private void PlayerDied()
	{
		BasicConfirmDialog basicConfirmDialog = GeneralManager.CreateConfirmDialog(LocalizationHelpers.LocalizedText("confirm_txt_tutorial_fail", Array.Empty<object>()), LocalizationHelpers.LocalizedText("confirm_title_tutorial_fail", Array.Empty<object>()), true);
		basicConfirmDialog.okButton.onClick.AddListener(delegate()
		{
			this.ReloadTutorial();
		});
		basicConfirmDialog.okButton.Select();
	}

	// Token: 0x06000837 RID: 2103 RVA: 0x000296A8 File Offset: 0x000278A8
	private void LeaveConfirm()
	{
		if (UnityEngine.Object.FindObjectsOfType<BasicConfirmDialog>().Length == 0)
		{
			BasicConfirmDialog basicConfirmDialog = GeneralManager.CreateConfirmDialog(LocalizationHelpers.LocalizedText("confirm_txt_tutorial_skip", Array.Empty<object>()), LocalizationHelpers.LocalizedText("confirm_title_tutorial_skip", Array.Empty<object>()), false);
			basicConfirmDialog.okButton.onClick.AddListener(delegate()
			{
				this.LeaveTutorial();
			});
			basicConfirmDialog.okButton.Select();
		}
	}

	// Token: 0x06000838 RID: 2104 RVA: 0x00029707 File Offset: 0x00027907
	private void ReloadTutorial()
	{
		SceneManagerWithParameters.ReloadScene();
	}

	// Token: 0x06000839 RID: 2105 RVA: 0x0000C66F File Offset: 0x0000A86F
	private void LeaveTutorial()
	{
		SceneManagerWithParameters.LoadScene("MainMenu", null, false, false);
	}

	// Token: 0x0600083A RID: 2106 RVA: 0x0002970E File Offset: 0x0002790E
	private void HandleDetectedDevice(InputControl control, InputEventPtr eventPtr)
	{
		this.currentDevice = control.device;
		if (control.device.name == "Mouse")
		{
			this.currentDevice = Keyboard.current;
		}
		this.UpdateUI();
	}

	// Token: 0x0600083B RID: 2107 RVA: 0x00029744 File Offset: 0x00027944
	private void OnDestroy()
	{
		if (InputUser.listenForUnpairedDeviceActivity > 0)
		{
			InputUser.listenForUnpairedDeviceActivity--;
			InputUser.onUnpairedDeviceUsed -= this.HandleDetectedDevice;
		}
		this.DisposeUserControls();
	}

	// Token: 0x0600083C RID: 2108 RVA: 0x00029771 File Offset: 0x00027971
	public void DisposeUserControls()
	{
		if (this.userControls != null)
		{
			this.userControls.Disable();
			this.userControls.Dispose();
		}
	}

	// Token: 0x0400059C RID: 1436
	public GameObject playerPrefab;

	// Token: 0x0400059D RID: 1437
	public MoveSet tutorialMoveSet;

	// Token: 0x0400059E RID: 1438
	public Text taskTitle;

	// Token: 0x0400059F RID: 1439
	public RectTransform tutorialPanel;

	// Token: 0x040005A0 RID: 1440
	public GameObject tutorialUIListHolder;

	// Token: 0x040005A1 RID: 1441
	public GameObject tutorialTaskPanelPrefab;

	// Token: 0x040005A2 RID: 1442
	public GameObject inputManagerPrefab;

	// Token: 0x040005A3 RID: 1443
	public PlayerHealth playerHealth;

	// Token: 0x040005A4 RID: 1444
	public PlayerHealth targetPlayerHealth;

	// Token: 0x040005A5 RID: 1445
	public global::PlayerInputManager inputManager;

	// Token: 0x040005A6 RID: 1446
	public List<TutorialTaskGroup> tutorialTaskGroups = new List<TutorialTaskGroup>();

	// Token: 0x040005A7 RID: 1447
	public int currentTaskGroup;

	// Token: 0x040005A8 RID: 1448
	public Text mouseTipText;

	// Token: 0x040005A9 RID: 1449
	public UserControls userControls;

	// Token: 0x040005AA RID: 1450
	private float maxDistanceFromOrigin = 50f;

	// Token: 0x040005AB RID: 1451
	private float maxDistanceFromOriginWithTarget = 80f;

	// Token: 0x040005AC RID: 1452
	private float maxDistanceFromTarget = 10f;

	// Token: 0x040005AD RID: 1453
	private bool playerDeathRegistered;

	// Token: 0x040005AE RID: 1454
	public float previousYRotation;

	// Token: 0x040005AF RID: 1455
	private InputDevice currentDevice;
}
