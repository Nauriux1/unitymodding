using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace PhysicsTest
{
	// Token: 0x0200026D RID: 621
	public class MagmarStaminaManager : MonoBehaviour
	{
		// Token: 0x0600120A RID: 4618 RVA: 0x0005BE85 File Offset: 0x0005A085
		public void Awake()
		{
			if (MagmarStaminaManager.singleton != null)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			MagmarStaminaManager.singleton = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			SceneManager.sceneLoaded += this.OnSceneLoaded;
		}

		// Token: 0x0600120B RID: 4619 RVA: 0x0005BEC2 File Offset: 0x0005A0C2
		private void OnEnable()
		{
			this.toggleMenuAction = new InputAction("ToggleMenu", InputActionType.Button, "<Keyboard>/f8", null, null, null);
			this.toggleMenuAction.performed += delegate(InputAction.CallbackContext ctx)
			{
				this.ToggleMenu();
			};
			this.toggleMenuAction.Enable();
		}

		// Token: 0x0600120C RID: 4620 RVA: 0x0005BEFF File Offset: 0x0005A0FF
		private void OnDisable()
		{
			if (this.toggleMenuAction != null)
			{
				this.toggleMenuAction.Disable();
				this.toggleMenuAction.performed -= delegate(InputAction.CallbackContext ctx)
				{
					this.ToggleMenu();
				};
			}
		}

		// Token: 0x0600120D RID: 4621 RVA: 0x0005BF2C File Offset: 0x0005A12C
		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			base.StopAllCoroutines();
			if (scene.name.ToLower().Contains("map") || scene.name.ToLower().Contains("test"))
			{
				this.shoulderJoints.Clear();
				this.elbowJoints.Clear();
				this.hipJoints.Clear();
				this.kneeJoints.Clear();
				this.spineJoints.Clear();
				this.hipStabilizerJoints.Clear();
				this.playerHealths.Clear();
				this.playerCardioStamina.Clear();
				this.jointFrameCount.Clear();
				this.originalPositionSprings.Clear();
				this.originalPositionDampers.Clear();
				this.originalMaximumForces.Clear();
				this.shoulderStamina = null;
				this.elbowStamina = null;
				this.hipStamina = null;
				this.kneeStamina = null;
				this.spineStamina = null;
				this.prevShoulderRotations = null;
				this.prevElbowRotations = null;
				this.prevHipRotations = null;
				this.prevKneeRotations = null;
				this.prevSpineRotations = null;
				this.updateInterval = 0.1f;
				if (this.useMod)
				{
					base.StartCoroutine(this.WaitForPlayerObjects());
				}
			}
		}

		// Token: 0x0600120E RID: 4622 RVA: 0x0005C05C File Offset: 0x0005A25C
		private IEnumerator WaitForPlayerObjects()
		{
			yield return new WaitForSeconds(0.5f);
			this.ChangeFloorPhysicsMaterial();
			this.SceneRigidbodies = UnityEngine.Object.FindObjectsOfType<Rigidbody>();
			foreach (PlayerHealth playerHealth in UnityEngine.Object.FindObjectsOfType<PlayerHealth>())
			{
				List<Transform> list = new List<Transform>();
				Transform transform = playerHealth.transform;
				while (transform.parent != null)
				{
					list.Add(transform.parent);
					transform = transform.parent;
				}
				GameObject gameObject;
				if (list.Count >= 3)
				{
					gameObject = list[list.Count - 3].gameObject;
					Debug.Log("Found Character: " + gameObject.name);
				}
				else
				{
					gameObject = playerHealth.gameObject;
					Debug.Log("Found Character " + playerHealth.gameObject.name);
				}
				if (!this.playerHealths.ContainsKey(gameObject.name))
				{
					this.playerHealths.Add(gameObject.name, playerHealth);
					this.playerCardioStamina.Add(gameObject.name, 100f);
					Debug.Log("Added player " + gameObject.name + " with 100 cardio stamina.");
				}
			}
			foreach (ConfigurableJoint configurableJoint in UnityEngine.Object.FindObjectsOfType<ConfigurableJoint>().ToList<ConfigurableJoint>())
			{
				if (!(configurableJoint == null) && !(configurableJoint.connectedBody == null) && !(configurableJoint.connectedBody.gameObject == null))
				{
					if (configurableJoint.connectedBody.gameObject.name == "HipSphere" && this.useCustomHipStabilizer)
					{
						Debug.Log("HipStabilizer FOUND!!!, Switching to CustomStabilizer");
						configurableJoint.angularXDrive = new JointDrive
						{
							positionSpring = 10000f,
							positionDamper = 2700f * (this.hipStabilizerDamperMultiplier / 100f),
							maximumForce = float.MaxValue
						};
						configurableJoint.angularYZDrive = new JointDrive
						{
							positionSpring = 10000f,
							positionDamper = 2700f * (this.hipStabilizerDamperMultiplier / 100f),
							maximumForce = 2500f * (this.hipStabilizerStrengthMultiplier / 100f)
						};
						this.HipStabilizer = configurableJoint;
						this.originalPositionSprings[configurableJoint] = configurableJoint.angularXDrive.positionSpring;
						this.originalPositionDampers[configurableJoint] = configurableJoint.angularXDrive.positionDamper;
						this.originalMaximumForces[configurableJoint] = configurableJoint.angularXDrive.maximumForce;
					}
					string name = configurableJoint.connectedBody.gameObject.transform.root.gameObject.name;
					string text = configurableJoint.connectedBody.gameObject.name.ToLower();
					uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
					if (num <= 2477759090U)
					{
						if (num <= 887795277U)
						{
							if (num != 391066851U)
							{
								if (num == 887795277U)
								{
									if (text == "elbow_right")
									{
										Debug.Log("ElbowRight Joint Found!!!");
										this.elbowJoints.Add(configurableJoint);
										this.originalPositionSprings[configurableJoint] = 1700f;
										this.originalPositionDampers[configurableJoint] = 150f;
										this.originalMaximumForces[configurableJoint] = 475f * (this.armStrenghtMultiplier / 100f);
									}
								}
							}
							else if (text == "shoulder_left")
							{
								Debug.Log("ShoulderLeft Joint Found!!!");
								this.shoulderJoints.Add(configurableJoint);
								this.originalPositionSprings[configurableJoint] = 1700f;
								this.originalPositionDampers[configurableJoint] = 150f;
								this.originalMaximumForces[configurableJoint] = 650f * (this.armStrenghtMultiplier / 100f);
							}
						}
						else if (num != 2460981471U)
						{
							if (num != 2472187908U)
							{
								if (num == 2477759090U)
								{
									if (text == "spine2")
									{
										Debug.Log("Spine2 Joint Found!!! Skipping stamina system.");
										this.spineJoints.Add(configurableJoint);
										configurableJoint.angularXDrive = new JointDrive
										{
											positionDamper = 650f,
											positionSpring = 7000f,
											maximumForce = 1500f * (this.spineStrenghtMultiplier / 100f)
										};
										configurableJoint.angularYZDrive = new JointDrive
										{
											positionDamper = 650f,
											positionSpring = 7000f,
											maximumForce = 1500f * (this.spineStrenghtMultiplier / 100f)
										};
									}
								}
							}
							else if (text == "hip_joint_right")
							{
								Debug.Log("HipJointRight Joint Found!!!");
								this.hipJoints.Add(configurableJoint);
								this.originalPositionSprings[configurableJoint] = 10000f;
								this.originalPositionDampers[configurableJoint] = 900f;
								this.originalMaximumForces[configurableJoint] = 1500f * (this.legStrengthMultiplier / 100f);
							}
						}
						else if (text == "spine1")
						{
							Debug.Log("Spine1 Joint Found!!! Skipping stamina system.");
							this.spineJoints.Add(configurableJoint);
							configurableJoint.angularXDrive = new JointDrive
							{
								positionDamper = 650f,
								positionSpring = 7000f,
								maximumForce = 1500f * (this.spineStrenghtMultiplier / 100f)
							};
							configurableJoint.angularYZDrive = new JointDrive
							{
								positionDamper = 650f,
								positionSpring = 7000f,
								maximumForce = 1500f * (this.spineStrenghtMultiplier / 100f)
							};
						}
					}
					else if (num <= 2864062187U)
					{
						if (num != 2670821435U)
						{
							if (num != 2712895782U)
							{
								if (num == 2864062187U)
								{
									if (text == "hipsphere")
									{
										Debug.Log("HipStabilizer Joint Found!!! Skipping stamina system.");
										this.hipStabilizerJoints.Add(configurableJoint);
									}
								}
							}
							else if (text == "knee_left")
							{
								Debug.Log("KneeLeft Joint Found!!!");
								this.kneeJoints.Add(configurableJoint);
								this.originalPositionSprings[configurableJoint] = 10000f;
								this.originalPositionDampers[configurableJoint] = 900f;
								this.originalMaximumForces[configurableJoint] = 1500f * (this.legStrengthMultiplier / 100f);
							}
						}
						else if (text == "hip_joint_left")
						{
							Debug.Log("HipJointLeft Joint Found!!!");
							this.hipJoints.Add(configurableJoint);
							this.originalPositionSprings[configurableJoint] = 10000f;
							this.originalPositionDampers[configurableJoint] = 900f;
							this.originalMaximumForces[configurableJoint] = 1500f * (this.legStrengthMultiplier / 100f);
						}
					}
					else if (num != 2984559451U)
					{
						if (num != 3169380088U)
						{
							if (num == 3369854316U)
							{
								if (text == "shoulder_right")
								{
									Debug.Log("ShoulderRight Joint Found!!!");
									this.shoulderJoints.Add(configurableJoint);
									this.originalPositionSprings[configurableJoint] = 1700f;
									this.originalPositionDampers[configurableJoint] = 150f;
									this.originalMaximumForces[configurableJoint] = 650f * (this.armStrenghtMultiplier / 100f);
								}
							}
						}
						else if (text == "elbow_left")
						{
							Debug.Log("ElbowLeft Joint Found!!!");
							this.elbowJoints.Add(configurableJoint);
							this.originalPositionSprings[configurableJoint] = 1700f;
							this.originalPositionDampers[configurableJoint] = 150f;
							this.originalMaximumForces[configurableJoint] = 475f * (this.armStrenghtMultiplier / 100f);
						}
					}
					else if (text == "knee_right")
					{
						Debug.Log("KneeRight Joint Found!!!");
						this.kneeJoints.Add(configurableJoint);
						this.originalPositionSprings[configurableJoint] = 10000f;
						this.originalPositionDampers[configurableJoint] = 900f;
						this.originalMaximumForces[configurableJoint] = 1500f * (this.legStrengthMultiplier / 100f);
					}
				}
			}
			this.shoulderStamina = new float[this.shoulderJoints.Count];
			this.elbowStamina = new float[this.elbowJoints.Count];
			this.hipStamina = new float[this.hipJoints.Count];
			this.kneeStamina = new float[this.kneeJoints.Count];
			this.spineStamina = new float[this.spineJoints.Count];
			this.prevShoulderRotations = new Quaternion[this.shoulderJoints.Count];
			this.prevElbowRotations = new Quaternion[this.elbowJoints.Count];
			this.prevHipRotations = new Quaternion[this.hipJoints.Count];
			this.prevKneeRotations = new Quaternion[this.kneeJoints.Count];
			this.prevSpineRotations = new Quaternion[this.spineJoints.Count];
			this.StartingHipStabilizerRotations = new Quaternion[this.hipStabilizerJoints.Count];
			for (int j = 0; j < this.hipStabilizerJoints.Count; j++)
			{
				this.StartingHipStabilizerRotations[j] = this.hipStabilizerJoints[j].connectedBody.transform.rotation;
			}
			for (int k = 0; k < this.shoulderJoints.Count; k++)
			{
				this.shoulderStamina[k] = 100f;
				this.prevShoulderRotations[k] = this.shoulderJoints[k].transform.localRotation * Quaternion.Inverse(this.shoulderJoints[k].targetRotation);
			}
			for (int l = 0; l < this.elbowJoints.Count; l++)
			{
				this.elbowStamina[l] = 100f;
				this.prevElbowRotations[l] = this.elbowJoints[l].transform.localRotation * Quaternion.Inverse(this.elbowJoints[l].targetRotation);
			}
			for (int m = 0; m < this.hipJoints.Count; m++)
			{
				this.hipStamina[m] = 100f;
				this.prevHipRotations[m] = this.hipJoints[m].transform.localRotation * Quaternion.Inverse(this.hipJoints[m].targetRotation);
			}
			for (int n = 0; n < this.kneeJoints.Count; n++)
			{
				this.kneeStamina[n] = 100f;
				this.prevKneeRotations[n] = this.kneeJoints[n].transform.localRotation * Quaternion.Inverse(this.kneeJoints[n].targetRotation);
			}
			for (int num2 = 0; num2 < this.spineJoints.Count; num2++)
			{
				this.spineStamina[num2] = 100f;
				this.prevSpineRotations[num2] = this.spineJoints[num2].transform.localRotation * Quaternion.Inverse(this.spineJoints[num2].targetRotation);
			}
			this.updateInterval = (this.updateInterval * (float)(this.elbowJoints.Count / 2) + this.updateInterval) / 2f;
			Debug.Log(string.Format("update interval is {0}", this.updateInterval));
			base.StartCoroutine(this.UpdateStaminaCoroutine());
			yield break;
		}

		// Token: 0x0600120F RID: 4623 RVA: 0x0005C06B File Offset: 0x0005A26B
		private IEnumerator UpdateStaminaCoroutine()
		{
			for (;;)
			{
				for (int i = 0; i < this.shoulderJoints.Count; i++)
				{
					if (this.shoulderJoints[i] != null)
					{
						this.CheckAndUpdateStamina(this.shoulderJoints[i], ref this.shoulderStamina[i], ref this.prevShoulderRotations[i]);
					}
				}
				for (int j = 0; j < this.elbowJoints.Count; j++)
				{
					if (this.elbowJoints[j] != null)
					{
						this.CheckAndUpdateStamina(this.elbowJoints[j], ref this.elbowStamina[j], ref this.prevElbowRotations[j]);
					}
				}
				for (int k = 0; k < this.hipJoints.Count; k++)
				{
					if (this.hipJoints[k] != null)
					{
						this.CheckAndUpdateStamina(this.hipJoints[k], ref this.hipStamina[k], ref this.prevHipRotations[k]);
					}
				}
				for (int l = 0; l < this.kneeJoints.Count; l++)
				{
					if (this.kneeJoints[l] != null)
					{
						this.CheckAndUpdateStamina(this.kneeJoints[l], ref this.kneeStamina[l], ref this.prevKneeRotations[l]);
					}
				}
				for (int m = 0; m < this.hipStabilizerJoints.Count; m++)
				{
					if (this.kneeJoints[m] != null)
					{
						this.HipStabilizerCalculator(this.hipStabilizerJoints[m], ref this.StartingHipStabilizerRotations[m]);
					}
				}
				yield return new WaitForSeconds(this.updateInterval);
			}
			yield break;
		}

		// Token: 0x06001210 RID: 4624 RVA: 0x0005C07C File Offset: 0x0005A27C
		private void CheckAndUpdateStamina(ConfigurableJoint joint, ref float stamina, ref Quaternion prevRotation)
		{
			if (joint == null || joint.connectedBody == null || joint.connectedBody.gameObject == null)
			{
				return;
			}
			string name = joint.connectedBody.gameObject.transform.root.gameObject.name;
			Transform transform = null;
			List<Transform> list = new List<Transform>();
			Transform transform2 = joint.transform;
			while (!transform2.name.ToLower().Contains("character"))
			{
				list.Add(transform2.parent);
				if (transform2.name.ToLower() == "hip")
				{
					transform = transform2;
				}
				transform2 = transform2.parent;
			}
			name = transform2.name;
			if (!this.playerCardioStamina.ContainsKey(name))
			{
				Debug.Log("Player " + name + " not found in playerCardioStamina dictionary.");
				return;
			}
			if (joint != null)
			{
				float num = transform.InverseTransformDirection(joint.connectedBody.angularVelocity).magnitude * 57.29578f * this.updateInterval;
				if (num > this.minMovementSpeedToDrainStamina * this.updateInterval)
				{
					if (this.playerCardioStamina.ContainsKey(name))
					{
						if (joint.connectedBody.name.ToLower().Contains("knee") || joint.connectedBody.name.ToLower().Contains("hip"))
						{
							float num2 = num / (20f * Mathf.Max(Mathf.Abs(this.playerCardioStamina[name] - 50f) / 50f, 0.1f)) * (this.cardioVascularStaminaDrainMultiplier / 500f) * (this.legsStaminaDrainMultiplier / 100f);
							Dictionary<string, float> dictionary = this.playerCardioStamina;
							string key = name;
							dictionary[key] -= num2;
							this.playerCardioStamina[name] = Mathf.Clamp(this.playerCardioStamina[name], 0.01f, 100f);
						}
						else
						{
							float num3 = num / (20f * Mathf.Max(Mathf.Abs(this.playerCardioStamina[name] - 50f) / 50f, 0.1f)) * (this.cardioVascularStaminaDrainMultiplier / 500f) * (this.armsStaminaDrainMultiplier / 100f);
							Dictionary<string, float> dictionary = this.playerCardioStamina;
							string key = name;
							dictionary[key] -= num3;
							this.playerCardioStamina[name] = Mathf.Clamp(this.playerCardioStamina[name], 0.01f, 100f);
						}
					}
					else
					{
						Debug.Log(string.Concat(new string[]
						{
							"CardioVascularStamina for ",
							name,
							" wasn't found because ",
							name,
							" is not part of the PlayerList"
						}));
					}
					if (joint.connectedBody.name.ToLower().Contains("knee") || joint.connectedBody.name.ToLower().Contains("hip"))
					{
						stamina -= num / (20f * Mathf.Max(Mathf.Abs(stamina - 50f) / 50f, 0.1f)) * (this.legsStaminaDrainMultiplier / 100f);
						stamina = Mathf.Clamp(stamina, 0.01f, 100f);
					}
					else
					{
						stamina -= num / (20f * Mathf.Max(Mathf.Abs(stamina - 50f) / 50f, 0.1f)) * (this.armsStaminaDrainMultiplier / 100f);
						stamina = Mathf.Clamp(stamina, 0.01f, 100f);
					}
					if (this.jointFrameCount.ContainsKey(joint.connectedBody.gameObject.name.ToLower()))
					{
						this.jointFrameCount[joint.connectedBody.gameObject.name.ToLower()] = 0;
					}
				}
				if (num < this.minMovementSpeedToDrainStamina * this.updateInterval)
				{
					string key;
					if (joint.connectedBody.name.ToLower().Contains("knee") || joint.connectedBody.name.ToLower().Contains("hip"))
					{
						stamina += 20f * this.updateInterval * (this.legsStaminaRegenMultiplier / 100f);
						stamina = Mathf.Clamp(stamina, 0.01f, 100f);
						Dictionary<string, float> dictionary = this.playerCardioStamina;
						key = name;
						dictionary[key] += 5f * (this.cardioVascularStaminaRegenMultiplier / 100f) * this.updateInterval;
						this.playerCardioStamina[name] = Mathf.Clamp(this.playerCardioStamina[name], 0.01f, 100f);
					}
					else
					{
						stamina += 20f * this.updateInterval * (this.armsStaminaRegenMultiplier / 100f);
						stamina = Mathf.Clamp(stamina, 0.01f, 100f);
						Dictionary<string, float> dictionary = this.playerCardioStamina;
						key = name;
						dictionary[key] += 5f * (this.cardioVascularStaminaRegenMultiplier / 100f) * this.updateInterval;
						this.playerCardioStamina[name] = Mathf.Clamp(this.playerCardioStamina[name], 0.01f, 100f);
					}
					string text = joint.connectedBody.gameObject.name.ToLower();
					if (!this.jointFrameCount.ContainsKey(text))
					{
						this.jointFrameCount[text] = 0;
					}
					Dictionary<string, int> dictionary2 = this.jointFrameCount;
					key = text;
					int num4 = dictionary2[key];
					dictionary2[key] = num4 + 1;
					if ((float)this.jointFrameCount[text] >= 2f / this.updateInterval)
					{
						stamina = 100f;
						this.jointFrameCount[text] = 0;
					}
				}
				float num5 = (float)transform2.GetComponent<PlayerHealth>().bloodAmount / 2f;
				num5 = Mathf.Clamp(num5, 0.0001f, 1f);
				float num6;
				float num7;
				float num8;
				if (this.originalPositionSprings.TryGetValue(joint, out num6) && this.originalPositionDampers.TryGetValue(joint, out num7) && this.originalMaximumForces.TryGetValue(joint, out num8))
				{
					if (num5 <= 0.4f)
					{
						joint.angularXDrive = new JointDrive
						{
							positionSpring = num6 * ((stamina + stamina + this.playerCardioStamina[name]) / 3f / 100f) * num5,
							positionDamper = num7 * ((stamina + stamina + this.playerCardioStamina[name]) / 3f / 100f) * num5,
							maximumForce = float.MaxValue
						};
						joint.angularYZDrive = new JointDrive
						{
							positionSpring = num6 * ((stamina + stamina + this.playerCardioStamina[name]) / 3f / 100f) * num5,
							positionDamper = num7 * ((stamina + stamina + this.playerCardioStamina[name]) / 3f / 100f) * num5,
							maximumForce = float.MaxValue
						};
					}
					else
					{
						joint.angularXDrive = new JointDrive
						{
							positionSpring = num6 * Mathf.Clamp((stamina + stamina + this.playerCardioStamina[name]) / 3f / 100f, 0.75f, 1f),
							positionDamper = num7,
							maximumForce = num8 * ((stamina + stamina + this.playerCardioStamina[name]) / 3f / 100f)
						};
						joint.angularYZDrive = new JointDrive
						{
							positionSpring = num6 * Mathf.Clamp((stamina + stamina + this.playerCardioStamina[name]) / 3f / 100f, 0.75f, 1f),
							positionDamper = num7,
							maximumForce = num8 * ((stamina + stamina + this.playerCardioStamina[name]) / 3f / 100f)
						};
					}
				}
				else
				{
					Debug.Log("Original position spring or damper not found for joint " + joint.connectedBody.gameObject.name);
				}
				if (num5 <= 0.3f && joint.connectedBody.gameObject.name.ToLower().Contains("knee"))
				{
					joint.angularXDrive = new JointDrive
					{
						positionSpring = 0.1f,
						positionDamper = 0.1f,
						maximumForce = 0.1f
					};
					joint.angularYZDrive = new JointDrive
					{
						positionSpring = 0.1f,
						positionDamper = 0.1f,
						maximumForce = 0.1f
					};
				}
			}
		}

		// Token: 0x06001211 RID: 4625 RVA: 0x0005C94C File Offset: 0x0005AB4C
		public void UpdateJointValues()
		{
			foreach (ConfigurableJoint configurableJoint in this.shoulderJoints)
			{
				if (configurableJoint != null && configurableJoint.angularXDrive.maximumForce == 3.4028235E+38f)
				{
					this.originalPositionSprings[configurableJoint] = configurableJoint.angularXDrive.positionSpring;
					this.originalPositionDampers[configurableJoint] = configurableJoint.angularXDrive.positionDamper;
					this.originalMaximumForces[configurableJoint] = configurableJoint.angularXDrive.maximumForce;
				}
			}
			foreach (ConfigurableJoint configurableJoint2 in this.elbowJoints)
			{
				if (configurableJoint2 != null && configurableJoint2.angularXDrive.maximumForce == 3.4028235E+38f)
				{
					this.originalPositionSprings[configurableJoint2] = configurableJoint2.angularXDrive.positionSpring;
					this.originalPositionDampers[configurableJoint2] = configurableJoint2.angularXDrive.positionDamper;
					this.originalMaximumForces[configurableJoint2] = configurableJoint2.angularXDrive.maximumForce;
				}
			}
			foreach (ConfigurableJoint configurableJoint3 in this.hipJoints)
			{
				if (configurableJoint3 != null && configurableJoint3.angularXDrive.maximumForce == 3.4028235E+38f)
				{
					this.originalPositionSprings[configurableJoint3] = configurableJoint3.angularXDrive.positionSpring;
					this.originalPositionDampers[configurableJoint3] = configurableJoint3.angularXDrive.positionDamper;
					this.originalMaximumForces[configurableJoint3] = configurableJoint3.angularXDrive.maximumForce;
				}
			}
			foreach (ConfigurableJoint configurableJoint4 in this.kneeJoints)
			{
				if (configurableJoint4 != null && configurableJoint4.angularXDrive.maximumForce == 3.4028235E+38f)
				{
					this.originalPositionSprings[configurableJoint4] = configurableJoint4.angularXDrive.positionSpring;
					this.originalPositionDampers[configurableJoint4] = configurableJoint4.angularXDrive.positionDamper;
					this.originalMaximumForces[configurableJoint4] = configurableJoint4.angularXDrive.maximumForce;
				}
			}
			foreach (ConfigurableJoint configurableJoint5 in this.spineJoints)
			{
				if (configurableJoint5 != null && configurableJoint5.angularXDrive.maximumForce == 3.4028235E+38f)
				{
					this.originalPositionSprings[configurableJoint5] = configurableJoint5.angularXDrive.positionSpring;
					this.originalPositionDampers[configurableJoint5] = configurableJoint5.angularXDrive.positionDamper;
					this.originalMaximumForces[configurableJoint5] = configurableJoint5.angularXDrive.maximumForce;
				}
			}
		}

		// Token: 0x06001212 RID: 4626 RVA: 0x0005CCC4 File Offset: 0x0005AEC4
		private void HipStabilizerCalculator(ConfigurableJoint joint, ref Quaternion startingRotation)
		{
			if (joint == null || joint.connectedBody == null || joint.connectedBody.gameObject == null)
			{
				return;
			}
			string name = joint.connectedBody.gameObject.transform.root.gameObject.name;
			List<Transform> list = new List<Transform>();
			Transform transform = joint.transform;
			while (!transform.name.ToLower().Contains("character"))
			{
				list.Add(transform.parent);
				transform = transform.parent;
			}
			string name2 = transform.name;
			float num = (float)transform.GetComponent<PlayerHealth>().bloodAmount / 2f;
			num = Mathf.Clamp(num, 0.0001f, 1f);
			if (num <= 0.5f)
			{
				joint.angularXDrive = new JointDrive
				{
					positionSpring = 10000f * this.hipStabilizerStrengthMultiplier,
					positionDamper = 2700f * this.hipStabilizerDamperMultiplier,
					maximumForce = 2500f * (num / 1.2f)
				};
				joint.angularYZDrive = new JointDrive
				{
					positionSpring = 10000f * this.hipStabilizerStrengthMultiplier,
					positionDamper = 2700f * this.hipStabilizerDamperMultiplier,
					maximumForce = 2500f * (num / 1.2f)
				};
			}
			float magnitude = joint.currentTorque.magnitude;
		}

		// Token: 0x06001213 RID: 4627 RVA: 0x0005CE30 File Offset: 0x0005B030
		private void ChangeFloorPhysicsMaterial()
		{
			foreach (GameObject gameObject in UnityEngine.Object.FindObjectsOfType<GameObject>())
			{
				if (gameObject.name.ToLower() == "arenaofblades")
				{
					foreach (Collider collider in gameObject.GetComponentsInChildren<Collider>())
					{
						if (collider.name.ToLower() == "arenaground.001")
						{
							Debug.Log(collider.name ?? "");
							collider.material = this.floorPhyMaterial;
							collider.material.staticFriction = this.floorFriction / 100f;
							collider.material.dynamicFriction = this.floorFriction / 100f;
							if (this.floorFriction > 60f)
							{
								collider.material.frictionCombine = PhysicMaterialCombine.Maximum;
							}
							if (this.floorFriction < 60f)
							{
								collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
							}
						}
					}
				}
			}
		}

		// Token: 0x06001214 RID: 4628 RVA: 0x0005CF3B File Offset: 0x0005B13B
		private void FixedUpdate()
		{
			this.SetGravity();
		}

		// Token: 0x06001215 RID: 4629 RVA: 0x0005CF44 File Offset: 0x0005B144
		private void SetGravity()
		{
			if (this.GravityModifier != 100f)
			{
				foreach (Rigidbody rigidbody in this.SceneRigidbodies)
				{
					if (rigidbody != null)
					{
						float num = this.GravityModifier / 100f;
						Vector3 force = -Physics.gravity * rigidbody.mass * (1f - num);
						rigidbody.AddForce(force, ForceMode.Force);
					}
				}
			}
		}

		// Token: 0x06001216 RID: 4630 RVA: 0x0005CFB9 File Offset: 0x0005B1B9
		private void ToggleMenu()
		{
			this.showMenu = !this.showMenu;
		}

		// Token: 0x06001217 RID: 4631 RVA: 0x0005CFCC File Offset: 0x0005B1CC
		private void OnGUI()
		{
			float num = (float)(Screen.height - 12);
			if (this.playerCardioStamina != null && this.hipStamina != null && this.kneeStamina != null)
			{
				for (int i = 0; i < this.playerCardioStamina.Count; i++)
				{
					GUI.Label(new Rect(10f, num -= 12f, 1000f, 40f), this.playerCardioStamina.Keys.ElementAt(i) + ":" + this.playerCardioStamina.Values.ElementAt(i).ToString("0.00"));
				}
				for (int j = 0; j < this.hipStamina.Count<float>(); j++)
				{
					GUI.Label(new Rect(10f, num -= 12f, 1000f, 40f), "hipJoint:" + this.hipStamina[j].ToString("0.00"));
				}
				for (int k = 0; k < this.kneeStamina.Count<float>(); k++)
				{
					GUI.Label(new Rect(10f, num -= 12f, 1000f, 40f), "kneeJoints:" + this.kneeStamina[k].ToString("0.00"));
				}
				for (int l = 0; l < this.elbowStamina.Count<float>(); l++)
				{
					GUI.Label(new Rect(10f, num -= 12f, 1000f, 40f), "elbowStamina:" + this.elbowStamina[l].ToString("0.00"));
				}
				for (int m = 0; m < this.shoulderStamina.Count<float>(); m++)
				{
					GUI.Label(new Rect(10f, num -= 12f, 1000f, 40f), "shoulderJoints:" + this.shoulderStamina[m].ToString("0.00"));
				}
				for (int n = 0; n < this.spineStamina.Count<float>(); n++)
				{
					GUI.Label(new Rect(10f, num -= 12f, 1000f, 40f), "spineStamina:" + this.spineStamina[n].ToString("0.00"));
				}
			}
			if (!this.showMenu)
			{
				return;
			}
			GUI.Box(new Rect(10f, 10f, 420f, 620f), "ImprovedCharacterPhysics Menu");
			this.useMod = GUI.Toggle(new Rect(20f, 40f, 230f, 20f), this.useMod, "Use Mod");
			this.useCustomHipStabilizer = GUI.Toggle(new Rect(20f, 70f, 230f, 20f), this.useCustomHipStabilizer, "Use Custom Hip Stabilizer");
			GUI.Label(new Rect(20f, 100f, 400f, 20f), "Hip Stabilizer Strength (Anti-Torque Exploit): " + Mathf.RoundToInt(this.hipStabilizerStrengthMultiplier).ToString());
			this.hipStabilizerStrengthMultiplier = Mathf.Round(GUI.HorizontalSlider(new Rect(20f, 120f, 400f, 20f), this.hipStabilizerStrengthMultiplier, 0f, 200f));
			GUI.Label(new Rect(20f, 140f, 400f, 20f), "Hip Stabilizer Damper: " + Mathf.RoundToInt(this.hipStabilizerDamperMultiplier).ToString());
			this.hipStabilizerDamperMultiplier = Mathf.Round(GUI.HorizontalSlider(new Rect(20f, 160f, 400f, 20f), this.hipStabilizerDamperMultiplier, 0f, 200f));
			GUI.Label(new Rect(20f, 180f, 400f, 20f), "Arm Strength: " + Mathf.RoundToInt(this.armStrenghtMultiplier).ToString());
			this.armStrenghtMultiplier = Mathf.Round(GUI.HorizontalSlider(new Rect(20f, 200f, 400f, 20f), this.armStrenghtMultiplier, 0f, 200f));
			GUI.Label(new Rect(20f, 220f, 400f, 20f), "Leg Strength: " + Mathf.RoundToInt(this.legStrengthMultiplier).ToString());
			this.legStrengthMultiplier = Mathf.Round(GUI.HorizontalSlider(new Rect(20f, 240f, 400f, 20f), this.legStrengthMultiplier, 0f, 200f));
			GUI.Label(new Rect(20f, 260f, 400f, 20f), "Spine Strength: " + Mathf.RoundToInt(this.spineStrenghtMultiplier).ToString());
			this.spineStrenghtMultiplier = Mathf.Round(GUI.HorizontalSlider(new Rect(20f, 280f, 400f, 20f), this.spineStrenghtMultiplier, 0f, 200f));
			GUI.Label(new Rect(20f, 300f, 400f, 20f), "min Movement Speed to Drain Stamina (Degrees per second): " + Mathf.RoundToInt(this.minMovementSpeedToDrainStamina).ToString());
			this.minMovementSpeedToDrainStamina = Mathf.Round(GUI.HorizontalSlider(new Rect(20f, 320f, 400f, 20f), this.minMovementSpeedToDrainStamina, 0f, 200f));
			GUI.Label(new Rect(20f, 340f, 400f, 20f), "Arms Stamina Drain: " + Mathf.RoundToInt(this.armsStaminaDrainMultiplier).ToString());
			this.armsStaminaDrainMultiplier = Mathf.Round(GUI.HorizontalSlider(new Rect(20f, 360f, 400f, 20f), this.armsStaminaDrainMultiplier, 0f, 200f));
			GUI.Label(new Rect(20f, 380f, 400f, 20f), "Arms Stamina Regen: " + Mathf.RoundToInt(this.armsStaminaRegenMultiplier).ToString());
			this.armsStaminaRegenMultiplier = Mathf.Round(GUI.HorizontalSlider(new Rect(20f, 400f, 400f, 20f), this.armsStaminaRegenMultiplier, 0f, 200f));
			GUI.Label(new Rect(20f, 420f, 400f, 20f), "Legs Stamina Drain: " + Mathf.RoundToInt(this.legsStaminaDrainMultiplier).ToString());
			this.legsStaminaDrainMultiplier = Mathf.Round(GUI.HorizontalSlider(new Rect(20f, 440f, 400f, 20f), this.legsStaminaDrainMultiplier, 0f, 200f));
			GUI.Label(new Rect(20f, 460f, 400f, 20f), "Legs Stamina Regen: " + Mathf.RoundToInt(this.legsStaminaRegenMultiplier).ToString());
			this.legsStaminaRegenMultiplier = Mathf.Round(GUI.HorizontalSlider(new Rect(20f, 480f, 400f, 20f), this.legsStaminaRegenMultiplier, 0f, 200f));
			GUI.Label(new Rect(20f, 500f, 400f, 20f), "Cardiovascular/Shared Stamina Drain: " + Mathf.RoundToInt(this.cardioVascularStaminaDrainMultiplier).ToString());
			this.cardioVascularStaminaDrainMultiplier = Mathf.Round(GUI.HorizontalSlider(new Rect(20f, 520f, 400f, 20f), this.cardioVascularStaminaDrainMultiplier, 0f, 200f));
			GUI.Label(new Rect(20f, 540f, 400f, 20f), "Cardiovascular/Shared Stamina Regen: " + Mathf.RoundToInt(this.cardioVascularStaminaRegenMultiplier).ToString());
			this.cardioVascularStaminaRegenMultiplier = Mathf.Round(GUI.HorizontalSlider(new Rect(20f, 560f, 400f, 20f), this.cardioVascularStaminaRegenMultiplier, 0f, 200f));
			GUI.Label(new Rect(20f, 580f, 400f, 20f), "Floor Friction: " + Mathf.RoundToInt(this.floorFriction).ToString());
			this.floorFriction = Mathf.Round(GUI.HorizontalSlider(new Rect(20f, 600f, 400f, 20f), this.floorFriction, 1f, 100f));
			GUI.Label(new Rect(20f, 620f, 400f, 20f), "Gravity Modifier: " + Mathf.RoundToInt(this.GravityModifier).ToString());
			this.GravityModifier = Mathf.Round(GUI.HorizontalSlider(new Rect(20f, 640f, 400f, 20f), this.GravityModifier, -200f, 200f));
		}

		// Token: 0x04000DAF RID: 3503
		public PhysicMaterial floorPhyMaterial;

		// Token: 0x04000DB0 RID: 3504
		private ConfigurableJoint HipStabilizer;

		// Token: 0x04000DB1 RID: 3505
		private Rigidbody[] SceneRigidbodies;

		// Token: 0x04000DB2 RID: 3506
		private List<ConfigurableJoint> shoulderJoints = new List<ConfigurableJoint>();

		// Token: 0x04000DB3 RID: 3507
		private List<ConfigurableJoint> elbowJoints = new List<ConfigurableJoint>();

		// Token: 0x04000DB4 RID: 3508
		private List<ConfigurableJoint> hipJoints = new List<ConfigurableJoint>();

		// Token: 0x04000DB5 RID: 3509
		private List<ConfigurableJoint> kneeJoints = new List<ConfigurableJoint>();

		// Token: 0x04000DB6 RID: 3510
		private List<ConfigurableJoint> spineJoints = new List<ConfigurableJoint>();

		// Token: 0x04000DB7 RID: 3511
		private List<ConfigurableJoint> hipStabilizerJoints = new List<ConfigurableJoint>();

		// Token: 0x04000DB8 RID: 3512
		private float[] shoulderStamina;

		// Token: 0x04000DB9 RID: 3513
		private float[] elbowStamina;

		// Token: 0x04000DBA RID: 3514
		private float[] hipStamina;

		// Token: 0x04000DBB RID: 3515
		private float[] kneeStamina;

		// Token: 0x04000DBC RID: 3516
		private float[] spineStamina;

		// Token: 0x04000DBD RID: 3517
		private Quaternion[] prevShoulderRotations;

		// Token: 0x04000DBE RID: 3518
		private Quaternion[] prevElbowRotations;

		// Token: 0x04000DBF RID: 3519
		private Quaternion[] prevHipRotations;

		// Token: 0x04000DC0 RID: 3520
		private Quaternion[] prevKneeRotations;

		// Token: 0x04000DC1 RID: 3521
		private Quaternion[] prevSpineRotations;

		// Token: 0x04000DC2 RID: 3522
		private Quaternion[] StartingHipStabilizerRotations;

		// Token: 0x04000DC3 RID: 3523
		private Dictionary<string, PlayerHealth> playerHealths = new Dictionary<string, PlayerHealth>();

		// Token: 0x04000DC4 RID: 3524
		private Dictionary<string, float> playerCardioStamina = new Dictionary<string, float>();

		// Token: 0x04000DC5 RID: 3525
		private Dictionary<string, int> jointFrameCount = new Dictionary<string, int>();

		// Token: 0x04000DC6 RID: 3526
		private Dictionary<ConfigurableJoint, float> originalPositionSprings = new Dictionary<ConfigurableJoint, float>();

		// Token: 0x04000DC7 RID: 3527
		private Dictionary<ConfigurableJoint, float> originalPositionDampers = new Dictionary<ConfigurableJoint, float>();

		// Token: 0x04000DC8 RID: 3528
		private Dictionary<ConfigurableJoint, float> originalMaximumForces = new Dictionary<ConfigurableJoint, float>();

		// Token: 0x04000DC9 RID: 3529
		private float updateInterval = 0.1f;

		// Token: 0x04000DCA RID: 3530
		private bool showMenu;

		// Token: 0x04000DCB RID: 3531
		private InputAction toggleMenuAction;

		// Token: 0x04000DCC RID: 3532
		public bool useMod = true;

		// Token: 0x04000DCD RID: 3533
		public bool useCustomHipStabilizer = true;

		// Token: 0x04000DCE RID: 3534
		public float hipStabilizerStrengthMultiplier = 100f;

		// Token: 0x04000DCF RID: 3535
		public float hipStabilizerDamperMultiplier = 100f;

		// Token: 0x04000DD0 RID: 3536
		public float armStrenghtMultiplier = 100f;

		// Token: 0x04000DD1 RID: 3537
		public float legStrengthMultiplier = 100f;

		// Token: 0x04000DD2 RID: 3538
		public float spineStrenghtMultiplier = 100f;

		// Token: 0x04000DD3 RID: 3539
		public float minMovementSpeedToDrainStamina = 110f;

		// Token: 0x04000DD4 RID: 3540
		public float armsStaminaDrainMultiplier = 50f;

		// Token: 0x04000DD5 RID: 3541
		public float armsStaminaRegenMultiplier = 100f;

		// Token: 0x04000DD6 RID: 3542
		public float legsStaminaDrainMultiplier = 120f;

		// Token: 0x04000DD7 RID: 3543
		public float legsStaminaRegenMultiplier = 100f;

		// Token: 0x04000DD8 RID: 3544
		public float cardioVascularStaminaDrainMultiplier = 80f;

		// Token: 0x04000DD9 RID: 3545
		public float cardioVascularStaminaRegenMultiplier = 40f;

		// Token: 0x04000DDA RID: 3546
		public float floorFriction = 80f;

		// Token: 0x04000DDB RID: 3547
		public float GravityModifier = 100f;

		// Token: 0x04000DDC RID: 3548
		public static MagmarStaminaManager singleton;
	}
}
