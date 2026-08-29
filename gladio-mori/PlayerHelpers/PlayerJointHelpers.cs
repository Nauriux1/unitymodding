using System;
using MoveClasses;
using Unity.Burst;
using UnityEngine;

namespace PlayerHelpers
{
	// Token: 0x02000270 RID: 624
	public static class PlayerJointHelpers
	{
		// Token: 0x06001227 RID: 4647 RVA: 0x0005E994 File Offset: 0x0005CB94
		public static float GetMaxJointSpringForJointType(JointType jointType, bool legacy)
		{
			if (legacy)
			{
				return PlayerJointHelpers.GetMaxJointSpringForJointTypeLegacy(jointType);
			}
			float result = 100f;
			if (jointType == JointType.HIP)
			{
				result = 10000f;
			}
			else if (jointType == JointType.HIP_JOINT_LEFT || jointType == JointType.HIP_JOINT_RIGHT)
			{
				result = 10000f;
			}
			else if (jointType == JointType.KNEE_LEFT || jointType == JointType.KNEE_RIGHT)
			{
				result = 10000f;
			}
			else if (jointType == JointType.NECK)
			{
				result = 1000f;
			}
			else if (jointType == JointType.SPINE1)
			{
				result = 7000f;
			}
			else if (jointType == JointType.SPINE2)
			{
				result = 7000f;
			}
			else if (jointType == JointType.SCAPULA_LEFT || jointType == JointType.SCAPULA_RIGHT)
			{
				result = 4000f;
			}
			else if (jointType == JointType.SHOULDER_LEFT || jointType == JointType.SHOULDER_RIGHT)
			{
				result = 1700f;
			}
			else if (jointType == JointType.ELBOW_LEFT || jointType == JointType.ELBOW_RIGHT)
			{
				result = 1700f;
			}
			else if (jointType == JointType.WRIST_LEFT || jointType == JointType.WRIST_RIGHT)
			{
				result = 1000f;
			}
			return result;
		}

		// Token: 0x06001228 RID: 4648 RVA: 0x0005EA4C File Offset: 0x0005CC4C
		public static float GetMaxJointSpringForJointTypeLegacy(JointType jointType)
		{
			float result = 100f;
			if (jointType == JointType.HIP)
			{
				result = 10000f;
			}
			else if (jointType == JointType.HIP_JOINT_LEFT || jointType == JointType.HIP_JOINT_RIGHT)
			{
				result = 4500f;
			}
			else if (jointType == JointType.KNEE_LEFT || jointType == JointType.KNEE_RIGHT)
			{
				result = 4500f;
			}
			else if (jointType == JointType.NECK)
			{
				result = 1000f;
			}
			else if (jointType == JointType.SPINE1)
			{
				result = 7000f;
			}
			else if (jointType == JointType.SPINE2)
			{
				result = 6000f;
			}
			else if (jointType == JointType.SCAPULA_LEFT || jointType == JointType.SCAPULA_RIGHT)
			{
				result = 4000f;
			}
			else if (jointType == JointType.SHOULDER_LEFT || jointType == JointType.SHOULDER_RIGHT)
			{
				result = 2000f;
			}
			else if (jointType == JointType.ELBOW_LEFT || jointType == JointType.ELBOW_RIGHT)
			{
				result = 2000f;
			}
			else if (jointType == JointType.WRIST_LEFT || jointType == JointType.WRIST_RIGHT)
			{
				result = 1000f;
			}
			return result;
		}

		// Token: 0x06001229 RID: 4649 RVA: 0x0005EAF8 File Offset: 0x0005CCF8
		public static float GetMaxForceForJointType(JointType jointType, bool legacy)
		{
			if (legacy)
			{
				return float.MaxValue;
			}
			float result = 100f;
			if (jointType == JointType.HIP)
			{
				result = 2500f;
			}
			else if (jointType == JointType.HIP_JOINT_LEFT || jointType == JointType.HIP_JOINT_RIGHT)
			{
				result = 1500f;
			}
			else if (jointType == JointType.KNEE_LEFT || jointType == JointType.KNEE_RIGHT)
			{
				result = 1500f;
			}
			else if (jointType == JointType.NECK)
			{
				result = 150f;
			}
			else if (jointType == JointType.SPINE1)
			{
				result = 1400f;
			}
			else if (jointType == JointType.SPINE2)
			{
				result = 1400f;
			}
			else if (jointType == JointType.SCAPULA_LEFT || jointType == JointType.SCAPULA_RIGHT)
			{
				result = 800f;
			}
			else if (jointType == JointType.SHOULDER_LEFT || jointType == JointType.SHOULDER_RIGHT)
			{
				result = 646f;
			}
			else if (jointType == JointType.ELBOW_LEFT || jointType == JointType.ELBOW_RIGHT)
			{
				result = 476f;
			}
			else if (jointType == JointType.WRIST_LEFT || jointType == JointType.WRIST_RIGHT)
			{
				result = 300f;
			}
			return result;
		}

		// Token: 0x0600122A RID: 4650 RVA: 0x0005EBAC File Offset: 0x0005CDAC
		public static float GetDamperMultiplierForJointType(JointType jointType, bool legacy)
		{
			float result = 0.09f;
			if (legacy)
			{
				result = 0.09f;
				if (jointType == JointType.WRIST_LEFT || jointType == JointType.WRIST_RIGHT)
				{
					result = 0.05f;
				}
				if (jointType == JointType.HIP)
				{
					result = 0.27f;
				}
				return result;
			}
			if (jointType == JointType.WRIST_LEFT || jointType == JointType.WRIST_RIGHT)
			{
				result = 0.05f;
			}
			if (jointType == JointType.HIP)
			{
				result = 0.27f;
			}
			return result;
		}

		// Token: 0x0600122B RID: 4651 RVA: 0x0005EBFD File Offset: 0x0005CDFD
		public static float GetMinChangeToDrainStaminaForJointType(JointType jointType)
		{
			return 0.008f;
		}

		// Token: 0x0600122C RID: 4652 RVA: 0x0005EC04 File Offset: 0x0005CE04
		public static float GetStaminaRegenThresholdForJointType(JointType jointType)
		{
			return PlayerJointHelpers.GetMinChangeToDrainStaminaForJointType(jointType);
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x0005EC0C File Offset: 0x0005CE0C
		public static float GetStaminaMultiplierForJointType(JointType jointType)
		{
			return PlayerJointHelpers.GetStaminaMultiplierForStaminaIndex(PlayerJointHelpers.GetStaminaIndexForJoint(jointType));
		}

		// Token: 0x0600122E RID: 4654 RVA: 0x0005EC1C File Offset: 0x0005CE1C
		public static float GetStaminaMultiplierForStaminaIndex(int i)
		{
			float result = 1f;
			if (i == 0)
			{
				result = 0.25f;
			}
			else if (i == 1)
			{
				result = 1f;
			}
			else if (i == 2)
			{
				result = 0.18f;
			}
			return result;
		}

		// Token: 0x0600122F RID: 4655 RVA: 0x0005EC54 File Offset: 0x0005CE54
		public static int GetTotalStaminaCount()
		{
			if (PlayerJointHelpers.staminaCount != 0)
			{
				return PlayerJointHelpers.staminaCount;
			}
			foreach (object obj in Enum.GetValues(typeof(JointType)))
			{
				int num = PlayerJointHelpers.GetStaminaIndexForJoint((JointType)obj) + 1;
				if (num > PlayerJointHelpers.staminaCount)
				{
					PlayerJointHelpers.staminaCount = num;
				}
			}
			return PlayerJointHelpers.staminaCount;
		}

		// Token: 0x06001230 RID: 4656 RVA: 0x0005ECD8 File Offset: 0x0005CED8
		[BurstCompile]
		public static int GetStaminaIndexForJoint(JointType jointType)
		{
			int result = -1;
			if (jointType != JointType.HIP)
			{
				if (jointType == JointType.HIP_JOINT_LEFT || jointType == JointType.HIP_JOINT_RIGHT)
				{
					result = 0;
				}
				else if (jointType == JointType.KNEE_LEFT || jointType == JointType.KNEE_RIGHT)
				{
					result = 0;
				}
				else if (jointType != JointType.NECK)
				{
					if (jointType == JointType.SPINE1)
					{
						result = 1;
					}
					else if (jointType == JointType.SPINE2)
					{
						result = 1;
					}
					else if (jointType == JointType.SCAPULA_LEFT || jointType == JointType.SCAPULA_RIGHT)
					{
						result = 2;
					}
					else if (jointType == JointType.SHOULDER_LEFT || jointType == JointType.SHOULDER_RIGHT)
					{
						result = 2;
					}
					else if (jointType == JointType.ELBOW_LEFT || jointType == JointType.ELBOW_RIGHT)
					{
						result = 2;
					}
					else if (jointType == JointType.WRIST_LEFT || jointType == JointType.WRIST_RIGHT)
					{
						result = 2;
					}
				}
			}
			return result;
		}

		// Token: 0x06001231 RID: 4657 RVA: 0x0005ED4C File Offset: 0x0005CF4C
		public static JointType GetJointTypeForJointName(string jointName)
		{
			JointType result = JointType.HIP;
			if (!Enum.TryParse<JointType>(jointName, out result))
			{
				Debug.LogError("Joint not found for name(" + jointName + ")");
			}
			return result;
		}

		// Token: 0x06001232 RID: 4658 RVA: 0x0005ED7C File Offset: 0x0005CF7C
		public static float GetDragForJointType(JointType jointType)
		{
			float num = 0.5f;
			float num2 = 0f;
			if (jointType == JointType.HIP)
			{
				num2 = 0.11f;
			}
			else if (jointType == JointType.HIP_JOINT_LEFT || jointType == JointType.HIP_JOINT_RIGHT)
			{
				num2 = 0.075f;
			}
			else if (jointType == JointType.KNEE_LEFT || jointType == JointType.KNEE_RIGHT)
			{
				num2 = 0.08f;
			}
			else if (jointType == JointType.NECK)
			{
				num2 = 0.09f;
			}
			else if (jointType == JointType.SPINE1)
			{
				num2 = 0.07f;
			}
			else if (jointType == JointType.SPINE2)
			{
				num2 = 0.11f;
			}
			else if (jointType == JointType.SCAPULA_LEFT || jointType == JointType.SCAPULA_RIGHT)
			{
				num2 = 0.03f;
			}
			else if (jointType == JointType.SHOULDER_LEFT || jointType == JointType.SHOULDER_RIGHT)
			{
				num2 = 0.035f;
			}
			else if (jointType == JointType.ELBOW_LEFT || jointType == JointType.ELBOW_RIGHT)
			{
				num2 = 0.03f;
			}
			else if (jointType == JointType.WRIST_LEFT || jointType == JointType.WRIST_RIGHT)
			{
				num2 = 0.02f;
			}
			return num * num2;
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x0005EE30 File Offset: 0x0005D030
		public static JointDrive GetHipJointDriveX(bool legacy = false)
		{
			if (legacy)
			{
				return PlayerJointHelpers.GetHipJointDriveLegacy();
			}
			return new JointDrive
			{
				positionSpring = 10000f,
				positionDamper = 2700f,
				maximumForce = 2500f
			};
		}

		// Token: 0x06001234 RID: 4660 RVA: 0x0005EE74 File Offset: 0x0005D074
		public static JointDrive GetHipJointDriveYZ(bool legacy = false)
		{
			if (legacy)
			{
				return PlayerJointHelpers.GetHipJointDriveLegacy();
			}
			return new JointDrive
			{
				positionSpring = 10000f,
				positionDamper = 2700f,
				maximumForce = 2500f
			};
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x0005EEB8 File Offset: 0x0005D0B8
		public static JointDrive GetHipJointDriveLegacy()
		{
			return new JointDrive
			{
				positionSpring = 10000f,
				positionDamper = 2700f,
				maximumForce = float.MaxValue
			};
		}

		// Token: 0x04000DE3 RID: 3555
		private static int staminaCount;
	}
}
