using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Mirror;
using MoveClasses;
using PlayerHelpers;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020000EC RID: 236
public class StaminaManager : MonoBehaviour
{
	// Token: 0x060007EC RID: 2028 RVA: 0x00027337 File Offset: 0x00025537
	private void Start()
	{
		this.InitializeStaminaManager();
	}

	// Token: 0x060007ED RID: 2029 RVA: 0x00027340 File Offset: 0x00025540
	private void InitializeStaminaManager()
	{
		if (StaminaManager.singleton != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		StaminaManager.singleton = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.SetDefaultValues();
		SceneManager.sceneUnloaded += this.OnSceneChanged;
		Debug.Log("Stamina manager has been setup");
	}

	// Token: 0x060007EE RID: 2030 RVA: 0x00027398 File Offset: 0x00025598
	public static void RegisterPlayerHealths(List<PlayerHealth> players)
	{
		if (StaminaManager.singleton != null)
		{
			StaminaManager.singleton.RegisterPlayerList(players);
			return;
		}
		Debug.LogError("Stamina manager is missing!");
	}

	// Token: 0x060007EF RID: 2031 RVA: 0x000273BD File Offset: 0x000255BD
	public static void FindAndRegisterPlayerHealths()
	{
		if (StaminaManager.singleton != null)
		{
			StaminaManager.singleton.FindPlayers();
			return;
		}
		Debug.LogError("Stamina manager is missing!");
	}

	// Token: 0x060007F0 RID: 2032 RVA: 0x000273E4 File Offset: 0x000255E4
	private void SetDefaultValues()
	{
		this.GetLegacy();
		this.staminaRegenRate = 0.2f;
		this.staminaEffectStart = 0.7f;
		this.minStaminaMultiplier = 0.1f;
		this.staminaDrainMultiplier = 0.25f;
		this.editableStaminaJointMaximumForces.Clear();
		this.editableJointStaminaDrainMultiplier.Clear();
		this.editableJointPreventStaminaRegenThreshold.Clear();
		this.editableMinChangeToDrainStamina.Clear();
		foreach (object obj in Enum.GetValues(typeof(JointType)))
		{
			JointType jointType = (JointType)obj;
			this.editableStaminaJointMaximumForces.Add(PlayerJointHelpers.GetMaxForceForJointType(jointType, false));
			this.editableJointStaminaDrainMultiplier.Add(PlayerJointHelpers.GetStaminaMultiplierForJointType(jointType));
			this.editableJointPreventStaminaRegenThreshold.Add(PlayerJointHelpers.GetStaminaRegenThresholdForJointType(jointType));
			this.editableMinChangeToDrainStamina.Add(PlayerJointHelpers.GetMinChangeToDrainStaminaForJointType(jointType));
		}
	}

	// Token: 0x060007F1 RID: 2033 RVA: 0x000274E4 File Offset: 0x000256E4
	private void OnSceneChanged(Scene scene1)
	{
		this.CleanUp();
	}

	// Token: 0x060007F2 RID: 2034 RVA: 0x000274EC File Offset: 0x000256EC
	public void CleanUp()
	{
		this.staminaItems.Clear();
		this.DisposeNativeArrays();
		this.CleanUpLegacy();
	}

	// Token: 0x060007F3 RID: 2035 RVA: 0x00027508 File Offset: 0x00025708
	public void FindPlayers()
	{
		PlayerHealth[] source = UnityEngine.Object.FindObjectsOfType<PlayerHealth>();
		this.RegisterPlayerList(source.ToList<PlayerHealth>());
	}

	// Token: 0x060007F4 RID: 2036 RVA: 0x00027527 File Offset: 0x00025727
	private void FixedUpdate()
	{
		this.HandleJob();
	}

	// Token: 0x060007F5 RID: 2037 RVA: 0x0002752F File Offset: 0x0002572F
	private void OnDestroy()
	{
		this.DisposeNativeArrays();
	}

	// Token: 0x060007F6 RID: 2038 RVA: 0x00027538 File Offset: 0x00025738
	public void RegisterPlayerList(List<PlayerHealth> players)
	{
		foreach (PlayerHealth player in players)
		{
			this.RegisterPlayer(player);
		}
		this.InitializeNativeArrays();
		this.SetInitialStrengths();
	}

	// Token: 0x060007F7 RID: 2039 RVA: 0x00027594 File Offset: 0x00025794
	private void RegisterPlayer(PlayerHealth player)
	{
		player.InitBluntDamage();
		StaminaItem item = new StaminaItem
		{
			playerHealth = player,
			fighterJoints = player.playerAnimator.GetFighterJointsPublic()
		};
		this.staminaItems.Add(item);
	}

	// Token: 0x060007F8 RID: 2040 RVA: 0x000275D4 File Offset: 0x000257D4
	public void UpdateStaminaManagerActive()
	{
		if ((NetworkManager.singleton != null && NetworkManager.singleton.mode == NetworkManagerMode.ClientOnly) || (IGameSettingsManager.singleton != null && !IGameSettingsManager.singleton.UseStamina))
		{
			this.staminaSystemActive = false;
		}
		else
		{
			this.staminaSystemActive = true;
		}
		if (IGameSettingsManager.singleton != null && IGameSettingsManager.singleton.GameType == GameTypes.Legacy)
		{
			this.staminaSystemActive = false;
		}
	}

	// Token: 0x060007F9 RID: 2041 RVA: 0x0002763C File Offset: 0x0002583C
	private void InitializeNativeArrays()
	{
		this.UpdateStaminaManagerActive();
		this.DisposeNativeArrays();
		this.legacyMode = this.GetLegacy();
		this.staminaJobItems = new NativeArray<StaminaJobItem>(this.staminaItems.Count, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		this.staminaJointMaximumForces = new NativeArray<float>(Enum.GetNames(typeof(JointType)).Length, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		this.staminaJointMaximumSprings = new NativeArray<float>(Enum.GetNames(typeof(JointType)).Length, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		this.jointStaminaDrainMultiplier = new NativeArray<float>(Enum.GetNames(typeof(JointType)).Length, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		this.jointPreventStaminaRegenThreshold = new NativeArray<float>(Enum.GetNames(typeof(JointType)).Length, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		this.jointMinChangeToDrainStamina = new NativeArray<float>(Enum.GetNames(typeof(JointType)).Length, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		for (int i = 0; i < this.staminaItems.Count; i++)
		{
			StaminaItem staminaItem = this.staminaItems[i];
			StaminaJobItem value = default(StaminaJobItem);
			if (staminaItem.playerHealth.ai)
			{
				value.ai = true;
			}
			for (int j = 0; j < staminaItem.fighterJoints.Count; j++)
			{
				quaternion quaternion = staminaItem.fighterJoints[j].physicsJoint.transform.localRotation;
				value.oldRotations.Add(quaternion);
				quaternion = staminaItem.fighterJoints[j].physicsJoint.transform.localRotation;
				value.currentRotations.Add(quaternion);
				quaternion = staminaItem.fighterJoints[j].joint.transform.localRotation;
				value.targetRotations.Add(quaternion);
				quaternion = staminaItem.fighterJoints[j].joint.transform.localRotation;
				value.oldTargetRotations.Add(quaternion);
				JointType jointType = staminaItem.fighterJoints[j].jointType;
				value.jointTypes.Add(jointType);
				float num = PlayerJointHelpers.GetMaxForceForJointType(staminaItem.fighterJoints[j].jointType, true);
				value.calculatedJointMaxForce.Add(num);
				num = PlayerJointHelpers.GetMaxJointSpringForJointType(staminaItem.fighterJoints[j].jointType, this.legacyMode);
				value.calculatedJointSpring.Add(num);
			}
			int totalStaminaCount = PlayerJointHelpers.GetTotalStaminaCount();
			for (int k = 0; k < totalStaminaCount; k++)
			{
				float num = 1f;
				value.currentStaminas.Add(num);
				num = 1f;
				value.currentStaminaMultipliers.Add(num);
				bool flag = false;
				value.preventStaminaRegenList.Add(flag);
			}
			value.bodyPartHealths = staminaItem.playerHealth.bodyPartHealths;
			this.staminaJobItems[i] = value;
		}
		foreach (object obj in Enum.GetValues(typeof(JointType)))
		{
			JointType jointType2 = (JointType)obj;
			this.staminaJointMaximumForces[(int)jointType2] = PlayerJointHelpers.GetMaxForceForJointType(jointType2, false);
			this.staminaJointMaximumSprings[(int)jointType2] = PlayerJointHelpers.GetMaxJointSpringForJointType(jointType2, this.legacyMode);
			this.jointStaminaDrainMultiplier[(int)jointType2] = PlayerJointHelpers.GetStaminaMultiplierForJointType(jointType2);
			this.jointPreventStaminaRegenThreshold[(int)jointType2] = PlayerJointHelpers.GetStaminaRegenThresholdForJointType(jointType2);
			this.jointMinChangeToDrainStamina[(int)jointType2] = PlayerJointHelpers.GetMinChangeToDrainStaminaForJointType(jointType2);
		}
	}

	// Token: 0x060007FA RID: 2042 RVA: 0x000279E4 File Offset: 0x00025BE4
	private void LoadEditedJointValues()
	{
		for (int i = 0; i < this.editableStaminaJointMaximumForces.Count; i++)
		{
			this.jointStaminaDrainMultiplier[i] = this.editableJointStaminaDrainMultiplier[i];
			this.jointPreventStaminaRegenThreshold[i] = this.editableJointPreventStaminaRegenThreshold[i];
			this.jointMinChangeToDrainStamina[i] = this.editableMinChangeToDrainStamina[i];
		}
	}

	// Token: 0x060007FB RID: 2043 RVA: 0x00027A50 File Offset: 0x00025C50
	private void DisposeNativeArrays()
	{
		this.CompleteJob();
		if (this.staminaJointMaximumForces.IsCreated)
		{
			this.staminaJointMaximumForces.Dispose();
		}
		if (this.staminaJointMaximumSprings.IsCreated)
		{
			this.staminaJointMaximumSprings.Dispose();
		}
		if (this.staminaJobItems.IsCreated)
		{
			this.staminaJobItems.Dispose();
		}
		if (this.jointStaminaDrainMultiplier.IsCreated)
		{
			this.jointStaminaDrainMultiplier.Dispose();
		}
		if (this.jointPreventStaminaRegenThreshold.IsCreated)
		{
			this.jointPreventStaminaRegenThreshold.Dispose();
		}
		if (this.jointMinChangeToDrainStamina.IsCreated)
		{
			this.jointMinChangeToDrainStamina.Dispose();
		}
	}

	// Token: 0x060007FC RID: 2044 RVA: 0x00027AF3 File Offset: 0x00025CF3
	private void HandleJob()
	{
		this.HandleExistingJob();
		this.ScheduleNextJob();
	}

	// Token: 0x060007FD RID: 2045 RVA: 0x00027B01 File Offset: 0x00025D01
	public void HandleExistingJob()
	{
		if (this.runningAJob)
		{
			this.CompleteJob();
		}
	}

	// Token: 0x060007FE RID: 2046 RVA: 0x00027B11 File Offset: 0x00025D11
	private void CompleteJob()
	{
		this.runningAJob = false;
		this.jobHandle.Complete();
	}

	// Token: 0x060007FF RID: 2047 RVA: 0x00027B28 File Offset: 0x00025D28
	private void ScheduleNextJob()
	{
		this.UpdateStaminaJobItems();
		if (this.staminaJobItems.Length > 0)
		{
			StaminaJob jobData = new StaminaJob(this.staminaSystemActive, this.staminaJobItems, this.staminaJointMaximumForces, this.staminaJointMaximumSprings, Time.fixedDeltaTime, this.staminaRegenRate, this.staminaEffectStart, this.minStaminaMultiplier, this.staminaDrainMultiplier, this.jointStaminaDrainMultiplier, this.jointPreventStaminaRegenThreshold, this.jointMinChangeToDrainStamina, this.targetRotationMultiplier, this.legacyMode);
			this.jobHandle = jobData.Schedule(default(JobHandle));
			this.runningAJob = true;
		}
	}

	// Token: 0x06000800 RID: 2048 RVA: 0x00027BC0 File Offset: 0x00025DC0
	private void UpdateStaminaJobItems()
	{
		for (int i = 0; i < this.staminaItems.Count; i++)
		{
			StaminaItem staminaItem = this.staminaItems[i];
			StaminaJobItem staminaJobItem = this.staminaJobItems[i];
			for (int j = 0; j < staminaItem.fighterJoints.Count; j++)
			{
				staminaJobItem.currentRotations[j] = staminaItem.fighterJoints[j].physicsJoint.transform.localRotation;
				staminaJobItem.targetRotations[j] = staminaItem.fighterJoints[j].joint.transform.localRotation;
				if (staminaItem.fighterJoints[j].jointStrength != null && !this.disableChanges)
				{
					staminaItem.fighterJoints[j].jointStrength.SetStaminaForce(staminaJobItem.calculatedJointMaxForce[j], staminaJobItem.calculatedJointSpring[j]);
				}
			}
			if (staminaItem.playerHealth.alive == staminaJobItem.dead)
			{
				staminaJobItem.dead = !staminaItem.playerHealth.alive;
			}
			staminaItem.playerHealth.SetStamina(staminaJobItem.currentStaminas);
			staminaItem.playerHealth.UpdateBodyPartHealths(staminaJobItem.bodyPartHealths);
			if (staminaItem.playerHealth.bodyPartHealthsChanged)
			{
				staminaJobItem.bodyPartHealths = staminaItem.playerHealth.bodyPartHealths;
				staminaItem.playerHealth.bodyPartHealthsChanged = false;
			}
			if (staminaItem.playerHealth.bluntDamageInstances.Count > 0)
			{
				int num = staminaItem.playerHealth.bluntDamageInstances.Count - 1;
				while (num > -1 && staminaJobItem.bluntDamageInstances.Capacity != staminaJobItem.bluntDamageInstances.Length)
				{
					BluntDamageInstance bluntDamageInstance = staminaItem.playerHealth.bluntDamageInstances[num];
					staminaJobItem.bluntDamageInstances.Add(bluntDamageInstance);
					staminaItem.playerHealth.bluntDamageInstances.RemoveAt(num);
					num--;
				}
			}
			this.staminaJobItems[i] = staminaJobItem;
		}
	}

	// Token: 0x06000801 RID: 2049 RVA: 0x00027DCE File Offset: 0x00025FCE
	public void RefreshStaminaManagerActive()
	{
		this.UpdateStaminaManagerActive();
		if (!this.staminaSystemActive)
		{
			this.ResetStaminaToFull();
		}
	}

	// Token: 0x06000802 RID: 2050 RVA: 0x00027DE4 File Offset: 0x00025FE4
	public void ResetStaminaToFull()
	{
		this.CompleteJob();
		bool legacy = this.GetLegacy();
		for (int i = 0; i < this.staminaItems.Count; i++)
		{
			StaminaItem staminaItem = this.staminaItems[i];
			StaminaJobItem staminaJobItem = this.staminaJobItems[i];
			if (staminaItem.playerHealth.alive)
			{
				for (int j = 0; j < staminaItem.fighterJoints.Count; j++)
				{
					staminaJobItem.calculatedJointMaxForce[j] = PlayerJointHelpers.GetMaxForceForJointType(staminaItem.fighterJoints[j].jointType, legacy);
					staminaJobItem.calculatedJointSpring[j] = PlayerJointHelpers.GetMaxJointSpringForJointType(staminaItem.fighterJoints[j].jointType, legacy);
					if (staminaItem.fighterJoints[j].jointStrength != null)
					{
						staminaItem.fighterJoints[j].jointStrength.SetStaminaForce(staminaJobItem.calculatedJointMaxForce[j], staminaJobItem.calculatedJointSpring[j]);
					}
				}
				for (int k = 0; k < staminaJobItem.currentStaminas.Length; k++)
				{
					staminaJobItem.currentStaminas[k] = 1f;
					staminaJobItem.currentStaminaMultipliers[k] = 1f;
				}
				staminaItem.playerHealth.SetStamina(staminaJobItem.currentStaminas);
				this.staminaJobItems[i] = staminaJobItem;
			}
		}
	}

	// Token: 0x06000803 RID: 2051 RVA: 0x00027F5C File Offset: 0x0002615C
	public bool GetLegacy()
	{
		bool result = false;
		if (IGameSettingsManager.singleton == null || IGameSettingsManager.singleton.GameType == GameTypes.Legacy)
		{
			result = true;
		}
		return result;
	}

	// Token: 0x06000804 RID: 2052 RVA: 0x00027F84 File Offset: 0x00026184
	public void SetInitialStrengths()
	{
		bool legacy = this.GetLegacy();
		for (int i = 0; i < this.staminaItems.Count; i++)
		{
			StaminaItem staminaItem = this.staminaItems[i];
			if (staminaItem.playerHealth.alive)
			{
				for (int j = 0; j < staminaItem.fighterJoints.Count; j++)
				{
					float maxJointSpringForJointType = PlayerJointHelpers.GetMaxJointSpringForJointType(staminaItem.fighterJoints[j].jointType, legacy);
					float damperMultiplierForJointType = PlayerJointHelpers.GetDamperMultiplierForJointType(staminaItem.fighterJoints[j].jointType, legacy);
					float newTotalMaxDamper = maxJointSpringForJointType * damperMultiplierForJointType;
					float maxForceForJointType = PlayerJointHelpers.GetMaxForceForJointType(staminaItem.fighterJoints[j].jointType, legacy);
					if (staminaItem.fighterJoints[j].jointStrength != null)
					{
						staminaItem.fighterJoints[j].jointStrength.SetInitialValues(maxJointSpringForJointType, newTotalMaxDamper, maxForceForJointType);
					}
					else if (staminaItem.playerHealth.ballHolderjoint != null && staminaItem.fighterJoints[j].jointType == JointType.HIP)
					{
						staminaItem.playerHealth.ballHolderjoint.angularXDrive = PlayerJointHelpers.GetHipJointDriveX(legacy);
						staminaItem.playerHealth.ballHolderjoint.angularYZDrive = PlayerJointHelpers.GetHipJointDriveYZ(legacy);
					}
				}
			}
		}
		this.SetLegacyPhysics(legacy);
	}

	// Token: 0x06000805 RID: 2053 RVA: 0x000280D0 File Offset: 0x000262D0
	public void SetLegacyPhysics(bool legacy)
	{
		this.UpdatePhysicsMaterialLegacyItemValues();
		if (legacy || this.legacyHasBeenInitialized)
		{
			this.InitLegacy();
			for (int i = 0; i < this.legacyItems.Count; i++)
			{
				this.legacyItems[i].SetLegacy(legacy);
			}
		}
	}

	// Token: 0x06000806 RID: 2054 RVA: 0x0002811C File Offset: 0x0002631C
	public void InitLegacy()
	{
		if (!this.legacyHasBeenInitialized)
		{
			this.legacyHasBeenInitialized = true;
			MonoBehaviour[] array = UnityEngine.Object.FindObjectsOfType<MonoBehaviour>();
			for (int i = 0; i < array.Length; i++)
			{
				ILegacy legacy = array[i] as ILegacy;
				if (legacy != null && legacy.LegacyItemExists())
				{
					this.legacyItems.Add(legacy);
				}
			}
		}
	}

	// Token: 0x06000807 RID: 2055 RVA: 0x0002816C File Offset: 0x0002636C
	public void ResetLegacy()
	{
		this.CleanUpLegacy();
		this.InitLegacy();
		bool legacy = this.GetLegacy();
		this.SetLegacyPhysics(legacy);
	}

	// Token: 0x06000808 RID: 2056 RVA: 0x00028193 File Offset: 0x00026393
	public void CleanUpLegacy()
	{
		this.legacyHasBeenInitialized = false;
		this.legacyItems.Clear();
	}

	// Token: 0x06000809 RID: 2057 RVA: 0x000281A8 File Offset: 0x000263A8
	public void UpdatePhysicsMaterialLegacyItemValues()
	{
		bool legacy = this.GetLegacy();
		for (int i = 0; i < this.physicsMaterialLegacyItems.Count; i++)
		{
			this.physicsMaterialLegacyItems[i].SetPhysicsMaterialValues(legacy);
		}
	}

	// Token: 0x04000572 RID: 1394
	public static StaminaManager singleton;

	// Token: 0x04000573 RID: 1395
	public List<StaminaItem> staminaItems = new List<StaminaItem>();

	// Token: 0x04000574 RID: 1396
	public bool staminaSystemActive = true;

	// Token: 0x04000575 RID: 1397
	public bool disableChanges;

	// Token: 0x04000576 RID: 1398
	public bool legacyMode;

	// Token: 0x04000577 RID: 1399
	public NativeArray<StaminaJobItem> staminaJobItems;

	// Token: 0x04000578 RID: 1400
	private NativeArray<float> staminaJointMaximumForces;

	// Token: 0x04000579 RID: 1401
	private NativeArray<float> staminaJointMaximumSprings;

	// Token: 0x0400057A RID: 1402
	private NativeArray<float> jointStaminaDrainMultiplier;

	// Token: 0x0400057B RID: 1403
	private NativeArray<float> jointPreventStaminaRegenThreshold;

	// Token: 0x0400057C RID: 1404
	private NativeArray<float> jointMinChangeToDrainStamina;

	// Token: 0x0400057D RID: 1405
	public List<float> editableStaminaJointMaximumForces = new List<float>();

	// Token: 0x0400057E RID: 1406
	public List<float> editableJointStaminaDrainMultiplier = new List<float>();

	// Token: 0x0400057F RID: 1407
	public List<float> editableJointPreventStaminaRegenThreshold = new List<float>();

	// Token: 0x04000580 RID: 1408
	public List<float> editableMinChangeToDrainStamina = new List<float>();

	// Token: 0x04000581 RID: 1409
	private bool runningAJob;

	// Token: 0x04000582 RID: 1410
	public JobHandle jobHandle;

	// Token: 0x04000583 RID: 1411
	public float staminaRegenRate = 0.2f;

	// Token: 0x04000584 RID: 1412
	public float staminaEffectStart = 0.7f;

	// Token: 0x04000585 RID: 1413
	public float minStaminaMultiplier = 0.05f;

	// Token: 0x04000586 RID: 1414
	public float staminaDrainMultiplier = 0.05f;

	// Token: 0x04000587 RID: 1415
	public float targetRotationMultiplier = 0.2f;

	// Token: 0x04000588 RID: 1416
	private bool legacyHasBeenInitialized;

	// Token: 0x04000589 RID: 1417
	private Stopwatch stopwatch = new Stopwatch();

	// Token: 0x0400058A RID: 1418
	private List<ILegacy> legacyItems = new List<ILegacy>();

	// Token: 0x0400058B RID: 1419
	public List<PhysicsMaterialLegacyItem> physicsMaterialLegacyItems = new List<PhysicsMaterialLegacyItem>();
}
