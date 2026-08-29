using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using MoveClasses;
using Unity.Collections;
using UnityEngine;
using Utils;

// Token: 0x02000159 RID: 345
public class PlayerHealth : MonoBehaviour
{
	// Token: 0x1700015F RID: 351
	// (get) Token: 0x06000ADE RID: 2782
	// (set) Token: 0x06000ADF RID: 2783
	public string playerName { get; set; }

	// Token: 0x17000160 RID: 352
	// (get) Token: 0x06000AE0 RID: 2784
	// (set) Token: 0x06000AE1 RID: 2785
	public List<EquipmentPositionOnPlayer> equipmentPositionsOnPlayer { get; set; }

	// Token: 0x17000161 RID: 353
	// (get) Token: 0x06000AE2 RID: 2786
	// (set) Token: 0x06000AE3 RID: 2787
	public MultiplayerRoomPlayer multiplayerRoomPlayer { get; set; }

	// Token: 0x17000162 RID: 354
	// (get) Token: 0x06000AE4 RID: 2788
	// (set) Token: 0x06000AE5 RID: 2789
	public IInputManager multiplayerInputManager { get; set; }

	// Token: 0x17000163 RID: 355
	// (get) Token: 0x06000AE6 RID: 2790
	// (set) Token: 0x06000AE7 RID: 2791
	public IPlayerInputManager playerInputManager { get; set; }

	// Token: 0x17000164 RID: 356
	// (get) Token: 0x06000AE8 RID: 2792
	// (set) Token: 0x06000AE9 RID: 2793
	public Camera playerCamera { get; set; }

	// Token: 0x17000165 RID: 357
	// (get) Token: 0x06000AEA RID: 2794
	// (set) Token: 0x06000AEB RID: 2795
	public CameraSmoothFollowControllable cameraSmoothFollow { get; set; }

	// Token: 0x06000AEC RID: 2796
	private void Awake()
	{
		this.maxBloodAmount = this.bloodAmount;
		if (NetworkManager.singleton == null || !NetworkManager.singleton.isNetworkActive)
		{
			this.HidePlayerName();
		}
		if (this.onlyPhysicalByDefault)
		{
			this.OnlyPhysical();
		}
		this.IgnoreCollisionPairs();
		this.InitBluntDamage();
	}

	// Token: 0x06000AED RID: 2797
	private void Start()
	{
		this.InitializeMultiplayer();
		if (NetworkManager.singleton != null && NetworkManager.singleton.mode == NetworkManagerMode.ClientOnly)
		{
			this.disableLocalLogic = true;
		}
		this.alive = true;
		this.InitTexture();
		if (!this.physicalPlayer.activeInHierarchy)
		{
			return;
		}
		GameObject gameObject = GameObject.Find("GameMaster");
		if (gameObject != null)
		{
			this.gameMaster = gameObject.GetComponent<GameMaster>();
		}
		if (this.gameMaster != null)
		{
			this.gameMaster.RegisterPlayer(this);
		}
		if (ReplayManager.singleton != null)
		{
			ReplayManager.singleton.AddPlayerHealthToRecording(this);
		}
		if (GeneralManager.singleton != null)
		{
			GeneralManager.singleton.RegisterPlayerHealth(this);
		}
	}

	// Token: 0x06000AEE RID: 2798
	private void Update()
	{
		this.UpdateBloodVignette();
		if (this.playerNameTextMesh != null && this.playerNameTextMesh.gameObject.activeInHierarchy)
		{
			this.playerNameTextMesh.gameObject.transform.LookAt(Camera.main.transform);
			this.playerNameTextMesh.gameObject.transform.Rotate(new Vector3(0f, 180f, 0f));
		}
		this.CheckForFallingDeath();
	}

	// Token: 0x06000AEF RID: 2799
	private void CheckForFallingDeath()
	{
		if (this.disableLocalLogic || !this.physicalPlayer.activeInHierarchy)
		{
			return;
		}
		if (this.cameraPositionPoint.transform.position.y < -10f)
		{
			this.Die(DeathReason.Fall);
		}
	}

	// Token: 0x06000AF0 RID: 2800
	public void Die(DeathReason reason)
	{
		if ((this.ai || this.dummyTarget) && this.alive)
		{
			this.HandleDeath(reason);
		}
	}

	// Token: 0x06000AF1 RID: 2801
	private void HandleDeath(DeathReason reason)
	{
		if (this.disableLocalLogic)
		{
			return;
		}
		this.HandleClientDeath(reason);
		if (this.navigationObstacle != null && this.navigationObstacle.activeInHierarchy)
		{
			this.navigationObstacle.SetActive(false);
		}
		base.Invoke("HandleHandsOnDeath", 0.4f);
		if (this.playerAnimator != null)
		{
			if (this.playerAnimator.FighterJoints != null)
			{
				foreach (FighterJoint fighterJoint in this.playerAnimator.FighterJoints)
				{
					if (fighterJoint.jointStrength != null)
					{
						fighterJoint.jointStrength.SetStrengthPercent(1f);
					}
				}
			}
			this.playerAnimator.PlayerDied();
		}
		if (this.ballHolderjoint != null)
		{
			JointDrive jointDrive = new JointDrive
			{
				maximumForce = this.ballHolderjoint.angularXDrive.maximumForce,
				positionDamper = this.ballHolderjoint.angularXDrive.positionDamper,
				positionSpring = 0f
			};
			this.ballHolderjoint.angularXDrive = jointDrive;
			this.ballHolderjoint.angularYZDrive = jointDrive;
			this.ballHolderjoint.connectedBody.gameObject.SetActive(false);
			UnityEngine.Object.Destroy(this.ballHolderjoint);
		}
		if (this.gameMaster != null)
		{
			this.gameMaster.InformPlayerDeath(this, this.deathReason, null);
		}
		if (this.playerInputManager != null)
		{
			this.playerInputManager.HandlePlayerDeath();
		}
		this.SetupFreeCamera();
	}

	// Token: 0x06000AF2 RID: 2802
	public void HandleClientDeath(DeathReason reason)
	{
		this.alive = false;
		this.deathReason = reason;
		this.localUnscaledDeathTime = Time.unscaledTime;
		if (this.bleedableOrgans != null)
		{
			foreach (WeaponDamageablePart weaponDamageablePart in this.bleedableOrgans)
			{
				weaponDamageablePart.StopDestroyEffect();
			}
		}
		if (ReplayManager.singleton != null)
		{
			ReplayManager.singleton.RecordPlayerDeath(this, this.deathReason);
		}
		if (this.playerCameraEffects != null)
		{
			this.maxBloodlossVignetteValue = this.playerCameraEffects.vignetteValue;
		}
		this.HidePlayerName();
	}

	// Token: 0x06000AF3 RID: 2803
	private void HandleHandsOnDeath()
	{
		if (this.leftHand != null && this.rightHand != null)
		{
			if (this.leftHand.currentlyGrabbedItem == this.rightHand.currentlyGrabbedItem)
			{
				if (this.leftHand.IsPrimaryHoldingHand())
				{
					this.rightHand.SetHandState(HandState.NoHold);
					this.leftHand.SetHandState(HandState.NoHold);
					return;
				}
				this.leftHand.SetHandState(HandState.NoHold);
				this.rightHand.SetHandState(HandState.NoHold);
				return;
			}
			else
			{
				this.leftHand.SetHandState(HandState.NoHold);
				this.rightHand.SetHandState(HandState.NoHold);
			}
		}
	}

	// Token: 0x06000AF4 RID: 2804
	public void SetPlayerName(string newPlayerName)
	{
		this.playerName = newPlayerName;
		if (this.playerNameTextMesh != null)
		{
			this.playerNameTextMesh.text = newPlayerName;
		}
		if (ReplayManager.singleton != null)
		{
			ReplayManager.singleton.UpdatePlayerInfo(this);
		}
	}

	// Token: 0x06000AF5 RID: 2805
	public void HidePlayerName()
	{
		if (this.playerNameTextMesh != null)
		{
			this.playerNameTextMesh.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000AF6 RID: 2806
	private void IgnoreCollisionPairs()
	{
		foreach (IgnoreColliderPair ignoreColliderPair in this.ignoreColliderPairs)
		{
			Physics.IgnoreCollision(ignoreColliderPair.collider1, ignoreColliderPair.collider2, true);
		}
	}

	// Token: 0x06000AF7 RID: 2807
	public EquipmentPositionOnPlayer FindEquipmentPositionOnPlayer(EquipmentPosition equipmentPositionType)
	{
		if (this.equipmentPositionsOnPlayer == null || this.equipmentPositionsOnPlayer.Count == 0)
		{
			this.FindBodyparts();
		}
		return (from x in this.equipmentPositionsOnPlayer
		where equipmentPositionType == x.equipmentPosition && x.physics
		select x).FirstOrDefault<EquipmentPositionOnPlayer>();
	}

	// Token: 0x06000AF8 RID: 2808
	private void FindBodyparts()
	{
		this.equipmentPositionsOnPlayer = new List<EquipmentPositionOnPlayer>();
		this.ignoreCollisions = new List<IgnoreCollision>();
		this.ignoreCollisions = this.physicalPlayer.GetComponentsInChildren<IgnoreCollision>().ToList<IgnoreCollision>();
		foreach (Hand hand in base.gameObject.GetComponentsInChildren<Hand>())
		{
			EquipmentPositionOnPlayer equipmentPositionOnPlayer = new EquipmentPositionOnPlayer();
			equipmentPositionOnPlayer.equipmentPosition = hand.equipmentPosition;
			equipmentPositionOnPlayer.hand = hand;
			equipmentPositionOnPlayer.physics = true;
			equipmentPositionOnPlayer.spawnPosition = hand.spawnPosition;
			equipmentPositionOnPlayer.bodySide = hand.handSide;
			this.equipmentPositionsOnPlayer.Add(equipmentPositionOnPlayer);
			if (equipmentPositionOnPlayer.bodySide == BodySide.Left)
			{
				this.leftHand = hand;
			}
			else
			{
				this.rightHand = hand;
			}
		}
		GameObject gameObject = Generic.FindChildObject(this.playerAnimator.gameObject.transform, "GripPosition_Left", null);
		GameObject gameObject2 = Generic.FindChildObject(this.playerAnimator.gameObject.transform, "GripPosition_Right", null);
		this.equipmentPositionsOnPlayer.Add(new EquipmentPositionOnPlayer
		{
			physics = false,
			equipmentPosition = EquipmentPosition.HandLeft,
			spawnPosition = gameObject.transform,
			bodySide = BodySide.Left
		});
		this.equipmentPositionsOnPlayer.Add(new EquipmentPositionOnPlayer
		{
			physics = false,
			equipmentPosition = EquipmentPosition.HandRight,
			spawnPosition = gameObject2.transform,
			bodySide = BodySide.Right
		});
		GameObject gameObject3 = Generic.FindChildObject(this.physicalPlayer.transform, "NECK", null);
		this.equipmentPositionsOnPlayer.Add(new EquipmentPositionOnPlayer
		{
			physics = true,
			equipmentPosition = EquipmentPosition.Helmet,
			spawnPosition = gameObject3.transform,
			bodySide = BodySide.Center
		});
		GameObject gameObject4 = Generic.FindChildObject(this.playerAnimator.gameObject.transform, "NECK", null);
		this.equipmentPositionsOnPlayer.Add(new EquipmentPositionOnPlayer
		{
			physics = false,
			equipmentPosition = EquipmentPosition.Helmet,
			spawnPosition = gameObject4.transform,
			bodySide = BodySide.Center
		});
		GameObject gameObject5 = Generic.FindChildObject(this.physicalPlayer.transform, "HIP_JOINT_RIGHT", null);
		this.equipmentPositionsOnPlayer.Add(new EquipmentPositionOnPlayer
		{
			physics = true,
			equipmentPosition = EquipmentPosition.ThighRight,
			spawnPosition = gameObject5.transform,
			bodySide = BodySide.Right
		});
		GameObject gameObject6 = Generic.FindChildObject(this.playerAnimator.gameObject.transform, "HIP_JOINT_RIGHT", null);
		this.equipmentPositionsOnPlayer.Add(new EquipmentPositionOnPlayer
		{
			physics = false,
			equipmentPosition = EquipmentPosition.ThighRight,
			spawnPosition = gameObject6.transform,
			bodySide = BodySide.Right
		});
		GameObject gameObject7 = Generic.FindChildObject(this.physicalPlayer.transform, "HIP_JOINT_LEFT", null);
		this.equipmentPositionsOnPlayer.Add(new EquipmentPositionOnPlayer
		{
			physics = true,
			equipmentPosition = EquipmentPosition.ThighLeft,
			spawnPosition = gameObject7.transform,
			bodySide = BodySide.Left
		});
		GameObject gameObject8 = Generic.FindChildObject(this.playerAnimator.gameObject.transform, "HIP_JOINT_LEFT", null);
		this.equipmentPositionsOnPlayer.Add(new EquipmentPositionOnPlayer
		{
			physics = false,
			equipmentPosition = EquipmentPosition.ThighLeft,
			spawnPosition = gameObject8.transform,
			bodySide = BodySide.Left
		});
		GameObject gameObject9 = Generic.FindChildObject(this.physicalPlayer.transform, "KNEE_RIGHT", null);
		this.equipmentPositionsOnPlayer.Add(new EquipmentPositionOnPlayer
		{
			physics = true,
			equipmentPosition = EquipmentPosition.LegRight,
			spawnPosition = gameObject9.transform,
			bodySide = BodySide.Right
		});
		GameObject gameObject10 = Generic.FindChildObject(this.playerAnimator.gameObject.transform, "KNEE_RIGHT", null);
		this.equipmentPositionsOnPlayer.Add(new EquipmentPositionOnPlayer
		{
			physics = false,
			equipmentPosition = EquipmentPosition.LegRight,
			spawnPosition = gameObject10.transform,
			bodySide = BodySide.Right
		});
		GameObject gameObject11 = Generic.FindChildObject(this.physicalPlayer.transform, "KNEE_LEFT", null);
		this.equipmentPositionsOnPlayer.Add(new EquipmentPositionOnPlayer
		{
			physics = true,
			equipmentPosition = EquipmentPosition.LegLeft,
			spawnPosition = gameObject11.transform,
			bodySide = BodySide.Left
		});
		GameObject gameObject12 = Generic.FindChildObject(this.playerAnimator.gameObject.transform, "KNEE_LEFT", null);
		this.equipmentPositionsOnPlayer.Add(new EquipmentPositionOnPlayer
		{
			physics = false,
			equipmentPosition = EquipmentPosition.LegLeft,
			spawnPosition = gameObject12.transform,
			bodySide = BodySide.Left
		});
		GameObject gameObject13 = Generic.FindChildObject(this.physicalPlayer.transform, "SPINE2", null);
		this.equipmentPositionsOnPlayer.Add(new EquipmentPositionOnPlayer
		{
			physics = true,
			equipmentPosition = EquipmentPosition.Chest,
			spawnPosition = gameObject13.transform,
			bodySide = BodySide.Center
		});
		GameObject gameObject14 = Generic.FindChildObject(this.playerAnimator.gameObject.transform, "SPINE2", null);
		this.equipmentPositionsOnPlayer.Add(new EquipmentPositionOnPlayer
		{
			physics = false,
			equipmentPosition = EquipmentPosition.Chest,
			spawnPosition = gameObject14.transform,
			bodySide = BodySide.Center
		});
		GameObject gameObject15 = Generic.FindChildObject(this.physicalPlayer.transform, "SPINE1", null);
		this.equipmentPositionsOnPlayer.Add(new EquipmentPositionOnPlayer
		{
			physics = true,
			equipmentPosition = EquipmentPosition.Stomach,
			spawnPosition = gameObject15.transform,
			bodySide = BodySide.Center
		});
		GameObject gameObject16 = Generic.FindChildObject(this.playerAnimator.gameObject.transform, "SPINE1", null);
		this.equipmentPositionsOnPlayer.Add(new EquipmentPositionOnPlayer
		{
			physics = false,
			equipmentPosition = EquipmentPosition.Stomach,
			spawnPosition = gameObject16.transform,
			bodySide = BodySide.Center
		});
		GameObject gameObject17 = Generic.FindChildObject(this.physicalPlayer.transform, "HIP", null);
		this.equipmentPositionsOnPlayer.Add(new EquipmentPositionOnPlayer
		{
			physics = true,
			equipmentPosition = EquipmentPosition.Hip,
			spawnPosition = gameObject17.transform,
			bodySide = BodySide.Center
		});
		GameObject gameObject18 = Generic.FindChildObject(this.playerAnimator.gameObject.transform, "HIP", null);
		this.equipmentPositionsOnPlayer.Add(new EquipmentPositionOnPlayer
		{
			physics = false,
			equipmentPosition = EquipmentPosition.Hip,
			spawnPosition = gameObject18.transform,
			bodySide = BodySide.Center
		});
		GameObject gameObject19 = Generic.FindChildObject(this.physicalPlayer.transform, "ELBOW_RIGHT", null);
		this.equipmentPositionsOnPlayer.Add(new EquipmentPositionOnPlayer
		{
			physics = true,
			equipmentPosition = EquipmentPosition.ArmRight,
			spawnPosition = gameObject19.transform,
			bodySide = BodySide.Right
		});
		GameObject gameObject20 = Generic.FindChildObject(this.playerAnimator.gameObject.transform, "ELBOW_RIGHT", null);
		this.equipmentPositionsOnPlayer.Add(new EquipmentPositionOnPlayer
		{
			physics = false,
			equipmentPosition = EquipmentPosition.ArmRight,
			spawnPosition = gameObject20.transform,
			bodySide = BodySide.Right
		});
		GameObject gameObject21 = Generic.FindChildObject(this.physicalPlayer.transform, "ELBOW_LEFT", null);
		this.equipmentPositionsOnPlayer.Add(new EquipmentPositionOnPlayer
		{
			physics = true,
			equipmentPosition = EquipmentPosition.ArmLeft,
			spawnPosition = gameObject21.transform,
			bodySide = BodySide.Left
		});
		GameObject gameObject22 = Generic.FindChildObject(this.playerAnimator.gameObject.transform, "ELBOW_LEFT", null);
		this.equipmentPositionsOnPlayer.Add(new EquipmentPositionOnPlayer
		{
			physics = false,
			equipmentPosition = EquipmentPosition.ArmLeft,
			spawnPosition = gameObject22.transform,
			bodySide = BodySide.Left
		});
		GameObject gameObject23 = Generic.FindChildObject(this.physicalPlayer.transform, "SHOULDER_RIGHT", null);
		this.equipmentPositionsOnPlayer.Add(new EquipmentPositionOnPlayer
		{
			physics = true,
			equipmentPosition = EquipmentPosition.BicepRight,
			spawnPosition = gameObject23.transform,
			bodySide = BodySide.Right
		});
		GameObject gameObject24 = Generic.FindChildObject(this.playerAnimator.gameObject.transform, "SHOULDER_RIGHT", null);
		this.equipmentPositionsOnPlayer.Add(new EquipmentPositionOnPlayer
		{
			physics = false,
			equipmentPosition = EquipmentPosition.BicepRight,
			spawnPosition = gameObject24.transform,
			bodySide = BodySide.Right
		});
		GameObject gameObject25 = Generic.FindChildObject(this.physicalPlayer.transform, "SHOULDER_LEFT", null);
		this.equipmentPositionsOnPlayer.Add(new EquipmentPositionOnPlayer
		{
			physics = true,
			equipmentPosition = EquipmentPosition.BicepLeft,
			spawnPosition = gameObject25.transform,
			bodySide = BodySide.Left
		});
		GameObject gameObject26 = Generic.FindChildObject(this.playerAnimator.gameObject.transform, "SHOULDER_LEFT", null);
		this.equipmentPositionsOnPlayer.Add(new EquipmentPositionOnPlayer
		{
			physics = false,
			equipmentPosition = EquipmentPosition.BicepLeft,
			spawnPosition = gameObject26.transform,
			bodySide = BodySide.Left
		});
		for (int i = 0; i < this.equipmentPositionsOnPlayer.Count; i++)
		{
			EquipmentPositionOnPlayer equipmentPositionOnPlayer2 = this.equipmentPositionsOnPlayer[i];
			if (!(equipmentPositionOnPlayer2.hand != null) && equipmentPositionOnPlayer2.physics)
			{
				CuttableGameObject component = equipmentPositionOnPlayer2.spawnPosition.GetComponent<CuttableGameObject>();
				if (component != null)
				{
					equipmentPositionOnPlayer2.cuttableGameObject = component;
				}
			}
		}
	}

	// Token: 0x06000AF9 RID: 2809
	private void HandleChildren(GameObject gameObject)
	{
		foreach (object obj in gameObject.transform)
		{
			Transform transform = (Transform)obj;
			if (!(transform.name == "TargetBoxer") && (!(gameObject.name == "ActualBoxer") || !(transform.name != "Armature")))
			{
				this.SetSettings(gameObject);
				this.HandleChildren(transform.gameObject);
			}
		}
	}

	// Token: 0x06000AFA RID: 2810
	private void SetSettings(GameObject gameObject)
	{
		Rigidbody component = gameObject.GetComponent<Rigidbody>();
		if (component != null)
		{
			component.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		}
	}

	// Token: 0x06000AFB RID: 2811
	public void UpdateBallMovementCamera(Camera newCamera)
	{
		BallMovement[] componentsInChildren = base.gameObject.GetComponentsInChildren<BallMovement>();
		if (componentsInChildren != null)
		{
			BallMovement[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetCamera(newCamera);
			}
		}
	}

	// Token: 0x06000AFC RID: 2812
	public CameraSmoothFollowControllable SetupSmoothCameraFollow(Camera camera = null)
	{
		if (camera == null)
		{
			camera = Camera.main;
		}
		CameraSmoothFollowControllable cameraSmoothFollowControllable = camera.gameObject.GetComponent<CameraSmoothFollowControllable>();
		if (cameraSmoothFollowControllable == null)
		{
			cameraSmoothFollowControllable = camera.gameObject.AddComponent<CameraSmoothFollowControllable>();
		}
		cameraSmoothFollowControllable.SetTarget(this.cameraPoint, this.cameraPositionPoint);
		this.cameraSmoothFollow = cameraSmoothFollowControllable;
		this.SetupPlayerCameraEffects(camera);
		this.UpdateBallMovementCamera(camera);
		this.playerCamera = camera;
		return cameraSmoothFollowControllable;
	}

	// Token: 0x06000AFD RID: 2813
	public void SetupPlayerCameraEffects(Camera camera)
	{
		if (this.playerCameraEffects == null)
		{
			this.playerCameraEffects = camera.gameObject.AddComponent<PlayerCameraEffects>();
		}
	}

	// Token: 0x06000AFE RID: 2814
	public void SetupFreeCamera()
	{
		if (this.playerCamera != null && this.multiplayerInputManager != null)
		{
			this.multiplayerInputManager.SetupBasicCameraControls();
		}
	}

	// Token: 0x06000AFF RID: 2815
	public void SetEquipment(List<EquippedEquipment> equippedEquipment, bool multiplayerSpawn = false)
	{
		equippedEquipment = MoveSetHelpers.VerifyEquippedEquipmentList(equippedEquipment);
		this.currentlyEquippedEquipment = equippedEquipment;
		List<Armour> list = new List<Armour>();
		List<Armour> list2 = new List<Armour>();
		if (this.equipmentPositionsOnPlayer == null || this.equipmentPositionsOnPlayer.Count == 0)
		{
			this.FindBodyparts();
		}
		this.ClearEquippedEquipment();
		if (equippedEquipment != null)
		{
			using (List<EquippedEquipment>.Enumerator enumerator = equippedEquipment.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					EquippedEquipment equipment = enumerator.Current;
					GameObject gameObject = (from x in this.equipmentList
					where x.name == equipment.equipment.equipmentType.ToString()
					select x).FirstOrDefault<GameObject>();
					EquipmentPositionOnPlayer equipmentPositionOnPlayer = (from x in this.equipmentPositionsOnPlayer
					where equipment.position == x.equipmentPosition && x.physics
					select x).FirstOrDefault<EquipmentPositionOnPlayer>();
					if (equipmentPositionOnPlayer != null)
					{
						Hand hand = equipmentPositionOnPlayer.hand;
						GameObject gameObject4 = equipmentPositionOnPlayer.spawnPosition.gameObject;
						Vector3 position = gameObject4.transform.position;
						Vector3 eulerAngles = gameObject4.transform.eulerAngles;
						if (gameObject != null && this.physicalPlayer.activeInHierarchy)
						{
							GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject, equipmentPositionOnPlayer.spawnPosition);
							if (multiplayerSpawn && gameObject2.GetComponent<Rigidbody>() != null)
							{
								NetworkServer.Spawn(gameObject2, null);
							}
							gameObject2.transform.localPosition = default(Vector3);
							Handle componentInChildren = gameObject2.GetComponentInChildren<Handle>();
							if (componentInChildren != null)
							{
								if (equipmentPositionOnPlayer.bodySide == BodySide.Right)
								{
									gameObject2.transform.localRotation = Quaternion.Euler(new Vector3(componentInChildren.holdPosition.localRotation.eulerAngles.x, -componentInChildren.holdPosition.localRotation.eulerAngles.y, componentInChildren.holdPosition.localRotation.eulerAngles.z));
								}
								else if (equipmentPositionOnPlayer.bodySide == BodySide.Left)
								{
									gameObject2.transform.localRotation = Quaternion.Euler(componentInChildren.holdPosition.localRotation.eulerAngles);
								}
								float num = componentInChildren.holdPosition.localPosition.z;
								if (ValidationHelpers.ValidateEquipmentStartHoldPosition(equipment.equipmentStartHoldPosition, componentInChildren))
								{
									num += equipment.equipmentStartHoldPosition;
								}
								if (equipment.equipmentStartHoldType == EquipmentStartHandleRotation.Reverse)
								{
									gameObject2.transform.rotation = gameObject2.transform.rotation * Quaternion.Euler(0f, 180f, 0f);
									num *= -1f;
								}
								gameObject2.transform.localPosition += new Vector3(componentInChildren.holdPosition.localPosition.y * (float)((equipmentPositionOnPlayer.bodySide == BodySide.Right) ? 1 : -1), num, componentInChildren.holdPosition.localPosition.x) * gameObject2.transform.localScale.x;
							}
							Armour component = gameObject2.GetComponent<Armour>();
							if (component != null)
							{
								if (equipmentPositionOnPlayer.bodySide == BodySide.Right)
								{
									gameObject2.transform.localRotation = Quaternion.Euler(new Vector3(component.wearPosition.localRotation.eulerAngles.x, -component.wearPosition.localRotation.eulerAngles.y, component.wearPosition.localRotation.eulerAngles.z));
									gameObject2.transform.localPosition += new Vector3(component.wearPosition.localPosition.x * -1f, component.wearPosition.localPosition.y, component.wearPosition.localPosition.z) * gameObject2.transform.localScale.x;
								}
								else if (equipmentPositionOnPlayer.bodySide == BodySide.Left)
								{
									gameObject2.transform.localRotation = Quaternion.Euler(component.wearPosition.localRotation.eulerAngles);
									gameObject2.transform.localPosition += new Vector3(component.wearPosition.localPosition.x, component.wearPosition.localPosition.y, component.wearPosition.localPosition.z) * gameObject2.transform.localScale.x;
								}
								else
								{
									gameObject2.transform.localPosition += new Vector3(component.wearPosition.localPosition.x, component.wearPosition.localPosition.y, component.wearPosition.localPosition.z) * gameObject2.transform.localScale.x;
								}
								if (equipmentPositionOnPlayer.equipmentPosition == EquipmentPosition.Chest)
								{
									list.Add(component);
								}
								else if (equipmentPositionOnPlayer.equipmentPosition == EquipmentPosition.BicepLeft || equipmentPositionOnPlayer.equipmentPosition == EquipmentPosition.BicepRight)
								{
									list2.Add(component);
								}
								else if (equipmentPositionOnPlayer.equipmentPosition == EquipmentPosition.ArmLeft)
								{
									if (this.leftHand != null)
									{
										this.leftHand.RegisterColliderAsHandCollider(component.colliders);
									}
								}
								else if (equipmentPositionOnPlayer.equipmentPosition == EquipmentPosition.ArmRight && this.rightHand != null)
								{
									this.rightHand.RegisterColliderAsHandCollider(component.colliders);
								}
							}
							this.currentlyEquippedEquipmentList.Add(gameObject2);
							if (hand != null)
							{
								hand.SetGrabbedItem(gameObject2, equipment.equipmentStartHoldPosition);
							}
							if (equipmentPositionOnPlayer != null && equipmentPositionOnPlayer.cuttableGameObject != null)
							{
								equipmentPositionOnPlayer.cuttableGameObject.AddEquipment(gameObject2.transform);
							}
						}
					}
					EquipmentPositionOnPlayer equipmentPositionOnPlayer2 = (from x in this.equipmentPositionsOnPlayer
					where equipment.position == x.equipmentPosition && !x.physics
					select x).FirstOrDefault<EquipmentPositionOnPlayer>();
					if (gameObject != null && equipmentPositionOnPlayer2 != null)
					{
						GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(gameObject, equipmentPositionOnPlayer2.spawnPosition);
						Handle componentInChildren2 = gameObject3.GetComponentInChildren<Handle>();
						Equipment componentInChildren3 = gameObject3.GetComponentInChildren<Equipment>();
						if (componentInChildren3 != null)
						{
							componentInChildren3.SetAnimationOnly();
						}
						foreach (Rigidbody rigidbody in gameObject3.GetComponentsInChildren<Rigidbody>().ToList<Rigidbody>())
						{
							rigidbody.isKinematic = true;
							rigidbody.interpolation = RigidbodyInterpolation.None;
						}
						foreach (Collider collider in gameObject3.GetComponentsInChildren<Collider>().ToList<Collider>())
						{
							collider.enabled = false;
						}
						gameObject3.transform.localPosition = default(Vector3);
						if (componentInChildren2 != null)
						{
							if (equipmentPositionOnPlayer2.bodySide == BodySide.Right)
							{
								gameObject3.transform.localRotation = Quaternion.Euler(new Vector3(componentInChildren2.holdPosition.localRotation.eulerAngles.x, -componentInChildren2.holdPosition.localRotation.eulerAngles.y, componentInChildren2.holdPosition.localRotation.eulerAngles.z));
							}
							else if (equipmentPositionOnPlayer2.bodySide == BodySide.Left)
							{
								gameObject3.transform.localRotation = Quaternion.Euler(componentInChildren2.holdPosition.localRotation.eulerAngles);
							}
							float num2 = componentInChildren2.holdPosition.localPosition.z;
							if (ValidationHelpers.ValidateEquipmentStartHoldPosition(equipment.equipmentStartHoldPosition, componentInChildren2))
							{
								num2 += equipment.equipmentStartHoldPosition;
							}
							if (equipment.equipmentStartHoldType == EquipmentStartHandleRotation.Reverse)
							{
								gameObject3.transform.rotation = gameObject3.transform.rotation * Quaternion.Euler(0f, 180f, 0f);
								num2 *= -1f;
							}
							gameObject3.transform.localPosition += new Vector3(componentInChildren2.holdPosition.localPosition.y * (float)((equipmentPositionOnPlayer2.bodySide == BodySide.Right) ? 1 : -1), num2, componentInChildren2.holdPosition.localPosition.x) * gameObject3.transform.localScale.x;
						}
						Armour component2 = gameObject3.GetComponent<Armour>();
						if (component2 != null)
						{
							if (equipmentPositionOnPlayer.bodySide == BodySide.Right)
							{
								gameObject3.transform.localRotation = Quaternion.Euler(new Vector3(component2.wearPosition.localRotation.eulerAngles.x, -component2.wearPosition.localRotation.eulerAngles.y, component2.wearPosition.localRotation.eulerAngles.z));
								gameObject3.transform.localPosition += new Vector3(component2.wearPosition.localPosition.x * -1f, component2.wearPosition.localPosition.y, component2.wearPosition.localPosition.z) * gameObject3.transform.localScale.x;
							}
							else if (equipmentPositionOnPlayer.bodySide == BodySide.Left)
							{
								gameObject3.transform.localRotation = Quaternion.Euler(component2.wearPosition.localRotation.eulerAngles);
								gameObject3.transform.localPosition += new Vector3(component2.wearPosition.localPosition.x, component2.wearPosition.localPosition.y, component2.wearPosition.localPosition.z) * gameObject3.transform.localScale.x;
							}
							else
							{
								gameObject3.transform.localPosition += new Vector3(component2.wearPosition.localPosition.x, component2.wearPosition.localPosition.y, component2.wearPosition.localPosition.z) * gameObject3.transform.localScale.x;
							}
						}
						this.currentlyEquippedEquipmentList.Add(gameObject3);
					}
				}
			}
		}
		foreach (IgnoreCollision ignoreCollision in this.ignoreCollisions)
		{
			ignoreCollision.IgnoreArmour(list, list2);
		}
	}

	// Token: 0x06000B00 RID: 2816
	public void ClearEquippedEquipment()
	{
		if (this.equipmentPositionsOnPlayer != null)
		{
			for (int i = 0; i < this.equipmentPositionsOnPlayer.Count; i++)
			{
				if (this.equipmentPositionsOnPlayer[i].cuttableGameObject != null)
				{
					this.equipmentPositionsOnPlayer[i].cuttableGameObject.ClearEquipment();
				}
			}
		}
		if (this.currentlyEquippedEquipmentList != null)
		{
			for (int j = 0; j < this.currentlyEquippedEquipmentList.Count; j++)
			{
				UnityEngine.Object.Destroy(this.currentlyEquippedEquipmentList[j]);
			}
			this.currentlyEquippedEquipmentList.Clear();
		}
	}

	// Token: 0x06000B01 RID: 2817
	public void OnlyPhysical()
	{
		this.playerAnimator.gameObject.transform.position = new Vector3(this.playerAnimator.gameObject.transform.position.x, -100f, this.playerAnimator.gameObject.transform.position.y);
		List<MeshRenderer> list = this.playerAnimator.transform.GetComponentsInChildren<MeshRenderer>().ToList<MeshRenderer>();
		if (list != null)
		{
			foreach (MeshRenderer meshRenderer in list)
			{
				meshRenderer.enabled = false;
			}
		}
		List<Equipment> list2 = this.playerAnimator.transform.GetComponentsInChildren<Equipment>().ToList<Equipment>();
		if (list2 != null)
		{
			foreach (Equipment equipment in list2)
			{
				equipment.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06000B02 RID: 2818
	public void OnlyAnimation()
	{
		this.physicalPlayer.SetActive(false);
	}

	// Token: 0x06000B03 RID: 2819
	public void SetAnimatedPlayerVisible(bool visible)
	{
		if (this.playerAnimator != null)
		{
			this.playerAnimator.gameObject.SetActive(visible);
		}
	}

	// Token: 0x06000B04 RID: 2820
	public void StartBleeding()
	{
		if (!this.bleeding)
		{
			this.bleeding = true;
			base.StartCoroutine(this.DoBleed());
		}
	}

	// Token: 0x06000B05 RID: 2821
	private IEnumerator DoBleed()
	{
		for (;;)
		{
			this.bloodAmount -= this.bleedAmount * (double)this.bleedTick;
			if (this.bloodAmount < 0.0)
			{
				this.Die(DeathReason.Bleedout);
				base.StopCoroutine(this.DoBleed());
			}
			yield return new WaitForSeconds(this.bleedTick);
		}
		yield break;
	}

	// Token: 0x06000B06 RID: 2822
	public void UpdateBloodVignette()
	{
		if (this.bleeding && this.playerCameraEffects != null)
		{
			if (this.alive)
			{
				this.SetVignetteValue(Mathf.Lerp(this.playerCameraEffects.vignetteValue, 1f - (float)(this.bloodAmount / this.maxBloodAmount), Time.deltaTime * 10f));
				return;
			}
			if (Time.unscaledTime < this.localUnscaledDeathTime + this.fadeOutTime)
			{
				float t = (Time.unscaledTime - this.localUnscaledDeathTime) / this.fadeOutTime;
				this.SetVignetteValue(Mathf.Lerp(this.maxBloodlossVignetteValue, 0f, t));
				return;
			}
			this.SetVignetteValue(0f);
		}
	}

	// Token: 0x06000B07 RID: 2823
	private void SetVignetteValue(float value)
	{
		if (this.playerCameraEffects != null)
		{
			this.playerCameraEffects.vignetteValue = value;
		}
	}

	// Token: 0x06000B08 RID: 2824
	public void SetupPlayerForMoveEditor()
	{
		this.GenerateCollidersForMoveEditorChildren(base.gameObject);
		GameObject gameObject = Generic.FindChildObject(base.gameObject.transform, "MeshMoveEditorCollider", null);
		if (gameObject != null)
		{
			gameObject.SetActive(true);
		}
	}

	// Token: 0x06000B09 RID: 2825
	private void GenerateCollidersForMoveEditorChildren(GameObject currentGameObject)
	{
		foreach (object obj in currentGameObject.transform)
		{
			Transform transform = (Transform)obj;
			if (!(transform.name == "PlayerModelPhysics") && !(transform.GetComponent<Equipment>() != null))
			{
				this.GenerateColliders(transform.gameObject);
				this.GenerateCollidersForMoveEditorChildren(transform.gameObject);
			}
		}
	}

	// Token: 0x06000B0A RID: 2826
	private void GenerateColliders(GameObject currentGameObject)
	{
		if (currentGameObject.GetComponent<MeshFilter>() != null)
		{
			currentGameObject.AddComponent<MeshCollider>();
		}
	}

	// Token: 0x06000B0B RID: 2827
	public void InitMaterial()
	{
		if (this.materialInitialized)
		{
			return;
		}
		Renderer component = this.playerTextureObject.GetComponent<Renderer>();
		component.material = new Material(component.material);
		this.SetSharedMaterial();
		this.materialInitialized = true;
	}

	// Token: 0x06000B0C RID: 2828
	private void InitTexture()
	{
		this.InitMaterial();
		if (NetworkManager.singleton == null || !NetworkManager.singleton.isNetworkActive)
		{
			this.LoadLocalCustomPlayerTexture();
		}
	}

	// Token: 0x06000B0D RID: 2829
	public void LoadLocalCustomPlayerTexture()
	{
		if (this.ai || this.dummyTarget)
		{
			return;
		}
		if (ReplayManager.singleton != null && (ReplayManager.singleton.replayMode == ReplayMode.Replay || ReplayManager.singleton.replayMode == ReplayMode.StartReplayAfterLoad))
		{
			return;
		}
		this.SetPlayerTexture(SettingsHelper.GetCustomPlayerTexture());
	}

	// Token: 0x06000B0E RID: 2830
	public void SetPlayerTexture(Texture2D newTexture)
	{
		if (newTexture != this.playerTexture)
		{
			this.playerTexture = newTexture;
			this.UpdatePlayerTexture();
			this.SetSharedMaterial();
		}
	}

	// Token: 0x06000B0F RID: 2831
	public void UpdatePlayerTexture()
	{
		this.InitMaterial();
		if (this.playerTextureObject == null)
		{
			return;
		}
		Renderer component = this.playerTextureObject.GetComponent<Renderer>();
		if ((SettingsHelper.GetAllowCustomPlayerTextures() == AllowCustomTextureOptionsType.Disable && NetworkHelpers.CurrentlyInMultiplayer()) || (SettingsHelper.GetAllowCustomPlayerTextures() == AllowCustomTextureOptionsType.AllowForSelf && this.multiplayerRoomPlayer != null && this.multiplayerRoomPlayer != MultiplayerRoomPlayer.localMultiplayerRoomPlayer))
		{
			component.sharedMaterial.SetTexture("_MainTexture", null);
			return;
		}
		component.sharedMaterial.SetTexture("_MainTexture", this.playerTexture);
	}

	// Token: 0x06000B10 RID: 2832
	public void SetBloodColour(Color colour)
	{
		Renderer component = this.playerTextureObject.GetComponent<Renderer>();
		if (component != null)
		{
			component.sharedMaterial.SetColor("_BloodColour", colour);
		}
	}

	// Token: 0x06000B11 RID: 2833
	private void SetSharedMaterial()
	{
		if (this.playerTextureObject != null && this.shareMaterialRenderers != null)
		{
			Renderer component = this.playerTextureObject.GetComponent<Renderer>();
			foreach (Renderer renderer in this.shareMaterialRenderers)
			{
				renderer.sharedMaterial = component.sharedMaterial;
			}
		}
	}

	// Token: 0x06000B12 RID: 2834
	public void RegisterMultiplayerRoomPlayer(MultiplayerRoomPlayer newMultiplayerRoomPlayer)
	{
		this.multiplayerRoomPlayer = newMultiplayerRoomPlayer;
	}

	// Token: 0x06000B13 RID: 2835
	public void InitializeMultiplayer()
	{
		if (NetworkClient.active && this.multiplayerInputManager == null)
		{
			NetworkIdentity component = base.GetComponent<NetworkIdentity>();
			if (component != null && component.netId > 0U)
			{
				PlayerMultiplayerInputManager[] array = UnityEngine.Object.FindObjectsOfType<PlayerMultiplayerInputManager>();
				for (int i = 0; i < array.Length; i++)
				{
					array[i].InitPlayerHealth();
				}
			}
		}
	}

	// Token: 0x06000B14 RID: 2836
	public void SetStamina(FixedList128Bytes<float> newStaminas)
	{
		this.staminaLegs = newStaminas[0];
		this.staminaCore = newStaminas[1];
		this.staminaArms = newStaminas[2];
	}

	// Token: 0x06000B15 RID: 2837
	public void InitBluntDamage()
	{
		if (!this.bluntDamageInitialized)
		{
			this.bluntDamageInstances = new List<BluntDamageInstance>(16);
			this.bodyPartHealths = BluntDamageHelpers.GetNewBodyPartHealthArray();
			this.bluntDamageInitialized = true;
		}
	}

	// Token: 0x06000B16 RID: 2838
	public void AddBluntDamageInstance(BluntDamageInstance bluntDamageInstance)
	{
		if (this.bluntDamageInitialized)
		{
			this.bluntDamageInstances.Add(bluntDamageInstance);
		}
	}

	// Token: 0x06000B17 RID: 2839
	public BodyPartHealth GetBodyPartHealthByBodyPart(JointType bodyPart)
	{
		return this.bodyPartHealths[(int)bodyPart];
	}

	// Token: 0x06000B18 RID: 2840
	public BodyPartHealth SetBodyPartHealthByBodyPart(BodyPartHealth bodyPartHealth)
	{
		this.bodyPartHealthsChanged = true;
		this.bodyPartHealths[(int)bodyPartHealth.bodyPart] = bodyPartHealth;
		return bodyPartHealth;
	}

	// Token: 0x06000B19 RID: 2841
	public void UpdateBodyPartHealths(FixedList512Bytes<BodyPartHealth> newBodyPartHealths)
	{
		if (this.bodyPartHealthsChanged)
		{
			for (int i = 0; i < newBodyPartHealths.Length; i++)
			{
				BodyPartHealth value = this.bodyPartHealths[i];
				value.temporaryHealth = newBodyPartHealths[i].temporaryHealth;
				this.bodyPartHealths[i] = value;
			}
			return;
		}
		this.bodyPartHealths = newBodyPartHealths;
	}

	// Token: 0x0400077E RID: 1918
	public bool dummyTarget;

	// Token: 0x0400077F RID: 1919
	public bool ai;

	// Token: 0x04000780 RID: 1920
	public bool alive = true;

	// Token: 0x04000781 RID: 1921
	public int playerNum = 1;

	// Token: 0x04000782 RID: 1922
	public double bloodAmount = 2.0;

	// Token: 0x04000783 RID: 1923
	private double maxBloodAmount = 2.0;

	// Token: 0x04000784 RID: 1924
	public bool bleeding;

	// Token: 0x04000785 RID: 1925
	public double bleedAmount;

	// Token: 0x04000786 RID: 1926
	public float bleedTick = 0.1f;

	// Token: 0x04000788 RID: 1928
	public ConfigurableJoint ballHolderjoint;

	// Token: 0x04000789 RID: 1929
	private GameMaster gameMaster;

	// Token: 0x0400078A RID: 1930
	public GameObject cameraPoint;

	// Token: 0x0400078B RID: 1931
	public GameObject cameraPositionPoint;

	// Token: 0x0400078C RID: 1932
	public List<GameObject> equipmentList;

	// Token: 0x0400078D RID: 1933
	public GameObject physicalPlayer;

	// Token: 0x0400078E RID: 1934
	public List<GameObject> currentlyEquippedEquipmentList = new List<GameObject>();

	// Token: 0x0400078F RID: 1935
	public Hand leftHand;

	// Token: 0x04000790 RID: 1936
	public Hand rightHand;

	// Token: 0x04000791 RID: 1937
	public PlayerAnimator playerAnimator;

	// Token: 0x04000793 RID: 1939
	public List<IgnoreCollision> ignoreCollisions;

	// Token: 0x04000794 RID: 1940
	public TextMesh playerNameTextMesh;

	// Token: 0x04000795 RID: 1941
	public GameObject bloodParticlePrefab;

	// Token: 0x04000799 RID: 1945
	public PlayerHealthMultiplayer playerHealthMultiplayer;

	// Token: 0x0400079C RID: 1948
	public DeathReason deathReason;

	// Token: 0x0400079D RID: 1949
	public bool rollingFeet;

	// Token: 0x0400079E RID: 1950
	public bool onlyPhysicalByDefault;

	// Token: 0x0400079F RID: 1951
	public List<WeaponDamageablePart> bleedableOrgans = new List<WeaponDamageablePart>();

	// Token: 0x040007A0 RID: 1952
	public WeaponDamageableBodyPart[] weaponDamageableBodyParts;

	// Token: 0x040007A1 RID: 1953
	public List<IgnoreColliderPair> ignoreColliderPairs = new List<IgnoreColliderPair>();

	// Token: 0x040007A2 RID: 1954
	public GameObject navigationObstacle;

	// Token: 0x040007A3 RID: 1955
	public bool disableLocalLogic;

	// Token: 0x040007A4 RID: 1956
	private float localUnscaledDeathTime;

	// Token: 0x040007A5 RID: 1957
	private float maxBloodlossVignetteValue;

	// Token: 0x040007A6 RID: 1958
	public List<EquippedEquipment> currentlyEquippedEquipment = new List<EquippedEquipment>();

	// Token: 0x040007A7 RID: 1959
	public PlayerCameraEffects playerCameraEffects;

	// Token: 0x040007A8 RID: 1960
	private float fadeOutTime = 1f;

	// Token: 0x040007A9 RID: 1961
	[Header("CustomPlayerTexture")]
	public Texture2D playerTexture;

	// Token: 0x040007AA RID: 1962
	public GameObject playerTextureObject;

	// Token: 0x040007AB RID: 1963
	public List<Renderer> shareMaterialRenderers = new List<Renderer>();

	// Token: 0x040007AC RID: 1964
	public bool materialInitialized;

	// Token: 0x040007AD RID: 1965
	public float staminaArms = 1f;

	// Token: 0x040007AE RID: 1966
	public float staminaCore = 1f;

	// Token: 0x040007AF RID: 1967
	public float staminaLegs = 1f;

	// Token: 0x040007B0 RID: 1968
	public List<BluntDamageInstance> bluntDamageInstances;

	// Token: 0x040007B1 RID: 1969
	public FixedList512Bytes<BodyPartHealth> bodyPartHealths;

	// Token: 0x040007B2 RID: 1970
	public bool bluntDamageInitialized;

	// Token: 0x040007B3 RID: 1971
	public bool bodyPartHealthsChanged;
}
