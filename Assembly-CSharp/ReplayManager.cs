using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Mirror;
using MoveClasses;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

// Token: 0x020000C9 RID: 201
public class ReplayManager : MonoBehaviour
{
	// Token: 0x060006DE RID: 1758 RVA: 0x00022B66 File Offset: 0x00020D66
	private void Start()
	{
		this.InitializeReplayManager();
	}

	// Token: 0x060006DF RID: 1759 RVA: 0x00022B6E File Offset: 0x00020D6E
	public void InitializeReplayManager()
	{
		if (ReplayManager.singleton != null)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		ReplayManager.singleton = this;
		this.SetupUserControls();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x060006E0 RID: 1760 RVA: 0x00022B9C File Offset: 0x00020D9C
	public void SetupUserControls()
	{
		bool flag = false;
		if (this.userControls != null && this.userControls.ReplayMap.enabled)
		{
			flag = true;
		}
		if (this.userControls != null)
		{
			this.userControls.Dispose();
		}
		this.userControls = SettingsHelper.GetUserControls();
		this.userControls.ReplayMap.Disable();
		this.userControls.ReplayMap.ToggleToolbarVisibility.performed += this.ToggleToolbarVisibility;
		this.userControls.ReplayMap.TogglePlay.performed += this.TogglePlay;
		this.userControls.ReplayMap.SetReplaySpeed1.performed += delegate(InputAction.CallbackContext <p0>)
		{
			this.ReplaySpeedChanged(0);
		};
		this.userControls.ReplayMap.SetReplaySpeed2.performed += delegate(InputAction.CallbackContext <p0>)
		{
			this.ReplaySpeedChanged(1);
		};
		this.userControls.ReplayMap.SetReplaySpeed3.performed += delegate(InputAction.CallbackContext <p0>)
		{
			this.ReplaySpeedChanged(2);
		};
		this.userControls.ReplayMap.SetReplaySpeed4.performed += delegate(InputAction.CallbackContext <p0>)
		{
			this.ReplaySpeedChanged(3);
		};
		this.userControls.ReplayMap.SetReplaySpeed5.performed += delegate(InputAction.CallbackContext <p0>)
		{
			this.ReplaySpeedChanged(4);
		};
		this.userControls.ReplayMap.SetReplaySpeed6.performed += delegate(InputAction.CallbackContext <p0>)
		{
			this.ReplaySpeedChanged(5);
		};
		if (flag)
		{
			this.userControls.ReplayMap.Enable();
		}
	}

	// Token: 0x060006E1 RID: 1761 RVA: 0x00022D34 File Offset: 0x00020F34
	public void StopRecording(bool keepRecording = false)
	{
		if (!keepRecording || this.replayMode != ReplayMode.StartReplayAfterLoad)
		{
			if (keepRecording && this.replayMode == ReplayMode.Replay)
			{
				this.replayMode = ReplayMode.StartReplayAfterLoad;
			}
			else
			{
				this.replayMode = ReplayMode.None;
			}
		}
		if (this.pendingSave)
		{
			this.pendingSave = false;
			this.SaveRecording();
		}
		if (!keepRecording)
		{
			this.recording = null;
		}
		this.userControls.ReplayMap.Disable();
	}

	// Token: 0x060006E2 RID: 1762 RVA: 0x00022D9B File Offset: 0x00020F9B
	public void PauseRecording()
	{
		this.replayMode = ReplayMode.None;
		if (this.pendingSave)
		{
			this.pendingSave = false;
			this.SaveRecording();
		}
	}

	// Token: 0x060006E3 RID: 1763 RVA: 0x00022DBC File Offset: 0x00020FBC
	public void SaveRecording()
	{
		if (this.replayMode == ReplayMode.Record)
		{
			GeneralManager.CreateAlertDialog(LocalizationHelpers.LocalizedText("txt_replay_will_be_saved", Array.Empty<object>()), 1f, false);
			this.pendingSave = true;
			return;
		}
		if (this.replayMode == ReplayMode.None && this.recording != null && this.recording.recRGO.Count > 0)
		{
			RecordingHelper.SaveRecording(RecordingHelper.GetRecordingDestination(), this.recording);
			this.recording = null;
			this.replayMode = ReplayMode.None;
			if (!GameMenu.GameMenuCurrentlyHidden && GameMenu.singleton != null)
			{
				GameMenu.singleton.UpdateSaveReplayButtonState();
			}
		}
	}

	// Token: 0x060006E4 RID: 1764 RVA: 0x00022E54 File Offset: 0x00021054
	public void PrepareRGOPlayer(RGO rgo)
	{
		if (rgo == null || rgo.playerHealth == null)
		{
			return;
		}
		byte[] array = null;
		if (NetworkHelpers.CurrentlyInMultiplayer())
		{
			if (rgo.playerHealth.multiplayerRoomPlayer != null)
			{
				if (rgo.playerHealth.multiplayerRoomPlayer.isLocalRoomPlayer)
				{
					rgo.isLocalPlayer = true;
					array = SettingsHelper.GetCustomPlayerTextureBytes();
				}
				else
				{
					array = rgo.playerHealth.multiplayerRoomPlayer.customPlayerTextureBytes;
					if (rgo.playerHealth.playerTexture != null && (array == null || array.Length == 0))
					{
						array = Generic.Texture2DToJpgEncodedByteArray(rgo.playerHealth.playerTexture);
					}
				}
			}
		}
		else if (!rgo.playerHealth.ai)
		{
			rgo.isLocalPlayer = true;
			array = SettingsHelper.GetCustomPlayerTextureBytes();
		}
		if (array != null)
		{
			rgo.customTexture = array;
		}
	}

	// Token: 0x060006E5 RID: 1765 RVA: 0x00022F14 File Offset: 0x00021114
	public void InitializeRecording()
	{
		if (SettingsHelper.GetRecordReplay() && this.replayMode != ReplayMode.StartReplayAfterLoad && this.replayMode != ReplayMode.Replay)
		{
			this.replayMode = ReplayMode.Record;
			this.recording = new Recording();
			this.recording.name = (DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss", CultureInfo.InvariantCulture) ?? "");
			this.recording.map = SceneManager.GetActiveScene().name;
			this.recording.ticks = 0;
			this.recording.tickRate = this.tickRate;
		}
	}

	// Token: 0x060006E6 RID: 1766 RVA: 0x00022FB0 File Offset: 0x000211B0
	public void AddPlayerHealthToRecording(PlayerHealth player)
	{
		if (this.replayMode == ReplayMode.Record)
		{
			RGO rgo = new RGO();
			rgo.startTick = this.recording.ticks;
			rgo.name = player.playerName;
			rgo.prefabName = "player";
			rgo.gameObject = player.gameObject;
			rgo.equippedEquipment = player.currentlyEquippedEquipment;
			rgo.playerHealth = player;
			this.HandlePlayerHealthChildren(player.gameObject, rgo);
			this.PrepareRGOPlayer(rgo);
			this.recording.recRGO.Add(rgo);
		}
	}

	// Token: 0x060006E7 RID: 1767 RVA: 0x00023038 File Offset: 0x00021238
	public void UpdatePlayerInfo(PlayerHealth player)
	{
		if (this.recording != null && this.recording.recRGO != null && this.replayMode == ReplayMode.Record)
		{
			for (int i = this.recording.recRGO.Count - 1; i > -1; i--)
			{
				RGO rgo = this.recording.recRGO[i];
				if (rgo.gameObject == player.gameObject)
				{
					rgo.name = player.playerName;
				}
			}
		}
	}

	// Token: 0x060006E8 RID: 1768 RVA: 0x000230B4 File Offset: 0x000212B4
	public void RecordPlayerDeath(PlayerHealth player, DeathReason deathReason)
	{
		if (this.recording != null && this.recording.recRGO != null && this.replayMode == ReplayMode.Record)
		{
			for (int i = this.recording.recRGO.Count - 1; i > -1; i--)
			{
				RGO rgo = this.recording.recRGO[i];
				if (rgo.gameObject == player.gameObject)
				{
					rgo.deathEvent = new DE
					{
						deathReason = deathReason,
						tick = this.recording.ticks
					};
				}
			}
		}
	}

	// Token: 0x060006E9 RID: 1769 RVA: 0x00023144 File Offset: 0x00021344
	public void AddEquipmentToRecording(Equipment equipment)
	{
		if (this.replayMode == ReplayMode.Record)
		{
			RGO rgo = new RGO();
			rgo.startTick = this.recording.ticks;
			rgo.name = equipment.gameObject.name.Replace("(Clone)", "");
			rgo.prefabName = "equipment";
			rgo.gameObject = equipment.gameObject;
			this.SetupChildForRecording(equipment.gameObject, rgo, null);
			this.recording.recRGO.Add(rgo);
		}
	}

	// Token: 0x060006EA RID: 1770 RVA: 0x000231CF File Offset: 0x000213CF
	private void Update()
	{
		if (!this.CheckTick() && this.recording != null && this.replayMode == ReplayMode.Replay && Time.timeScale != 0f)
		{
			this.PlayTick(this.currentTick, true, false);
		}
	}

	// Token: 0x060006EB RID: 1771 RVA: 0x00023204 File Offset: 0x00021404
	private void HandlePlayerHealthChildren(GameObject gameObject, RGO recordableGameObject)
	{
		foreach (object obj in gameObject.transform)
		{
			Transform transform = (Transform)obj;
			if ((!(transform.GetComponent<Rigidbody>() == null) || !(transform.name != "PlayerModelPhysics") || transform.name.Contains("Hand_")) && transform.gameObject.activeInHierarchy && !(transform.GetComponent<Equipment>() != null))
			{
				if (transform.GetComponent<Rigidbody>() != null)
				{
					this.SetupChildForRecording(transform.gameObject, recordableGameObject, null);
				}
				this.HandlePlayerHealthChildren(transform.gameObject, recordableGameObject);
			}
		}
	}

	// Token: 0x060006EC RID: 1772 RVA: 0x000232DC File Offset: 0x000214DC
	private void SetupChildForRecording(GameObject child, RGO recordableGameObject, TickMode? tickMode = null)
	{
		RCGO rcgo = new RCGO();
		rcgo.name = child.name;
		rcgo.gameObject = child;
		Equipment component = child.GetComponent<Equipment>();
		if (tickMode != null)
		{
			rcgo.tickMode = tickMode.Value;
		}
		else if (component != null || child.name == "HIP")
		{
			rcgo.tickMode = TickMode.GlobalRotationAndPosition;
		}
		else if (child.name == "HipSphere")
		{
			rcgo.tickMode = TickMode.GlobalRotation;
		}
		else
		{
			rcgo.tickMode = TickMode.LocalRotation;
		}
		if (component != null)
		{
			rcgo.parentName = ((child.transform.parent != null) ? child.transform.parent.name : null);
		}
		recordableGameObject.recordableChildGameObject.Add(rcgo);
	}

	// Token: 0x17000105 RID: 261
	// (get) Token: 0x060006ED RID: 1773 RVA: 0x000233A8 File Offset: 0x000215A8
	public double tickInterval
	{
		get
		{
			return (double)((this.tickRate < int.MaxValue) ? (1f / (float)this.tickRate) : 0f);
		}
	}

	// Token: 0x060006EE RID: 1774 RVA: 0x000233CC File Offset: 0x000215CC
	private bool CheckTick()
	{
		if (AccurateInterval.Elapsed(Time.timeAsDouble, this.tickInterval, ref this.lastFixedTickTime))
		{
			this.HandleTick();
			return true;
		}
		return false;
	}

	// Token: 0x060006EF RID: 1775 RVA: 0x000233F0 File Offset: 0x000215F0
	private void HandleTick()
	{
		if (this.replayMode == ReplayMode.Record && this.recording != null)
		{
			if (this.recording.ticks >= ReplayManager.maxRecordingTicks)
			{
				this.replayMode = ReplayMode.None;
				return;
			}
			foreach (RGO rgo in this.recording.recRGO)
			{
				foreach (RCGO rcgo in rgo.recordableChildGameObject)
				{
					if (rcgo.gameObject == null)
					{
						Debug.Log("Recordable gameObject has been destroyed");
						this.replayMode = ReplayMode.None;
						return;
					}
					RT recordedTick;
					if (rcgo.tickMode == TickMode.GlobalRotationAndPosition)
					{
						recordedTick = this.GetRecordedTick(rcgo.lastRotation, rcgo.gameObject.transform.rotation, new Vector3?(rcgo.lastPosition), new Vector3?(rcgo.gameObject.transform.position));
					}
					else if (rcgo.tickMode == TickMode.GlobalRotation)
					{
						recordedTick = this.GetRecordedTick(rcgo.lastRotation, rcgo.gameObject.transform.rotation, null, null);
					}
					else
					{
						recordedTick = this.GetRecordedTick(rcgo.lastRotation, rcgo.gameObject.transform.localRotation, null, null);
					}
					rcgo.recordedTicks.Add(recordedTick);
					if (recordedTick.position != null)
					{
						rcgo.lastPosition = recordedTick.position.Value;
					}
					if (recordedTick.rotation != null)
					{
						rcgo.lastRotation = recordedTick.rotation.Value;
					}
				}
			}
			Recording recording = this.recording;
			int ticks = recording.ticks;
			recording.ticks = ticks + 1;
			return;
		}
		else if (this.replayMode == ReplayMode.Replay && this.recording != null)
		{
			this.PlayCurrentTick();
		}
	}

	// Token: 0x060006F0 RID: 1776 RVA: 0x00023634 File Offset: 0x00021834
	private RT GetRecordedTick(Quaternion lastRotation, Quaternion newRotation, Vector3? lastPosition = null, Vector3? newPosition = null)
	{
		RT rt = new RT();
		if (ReplayManager.recordEveryTick || !Generic.IsQuaternionApproximate(lastRotation, newRotation, 1E-08f) || this.recording.ticks == 0)
		{
			rt.rotation = new Quaternion?(newRotation);
		}
		if (lastPosition != null && newPosition != null)
		{
			Vector3 vector = lastPosition.Value - newPosition.Value;
			if (ReplayManager.recordEveryTick || (double)vector.magnitude > 0.0001 || this.recording.ticks == 0)
			{
				rt.position = newPosition;
			}
		}
		return rt;
	}

	// Token: 0x060006F1 RID: 1777 RVA: 0x000236CC File Offset: 0x000218CC
	private void PlayCurrentTick()
	{
		if (this.currentTick < this.recording.ticks)
		{
			this.currentTick++;
			this.PlayTick(this.currentTick, false, false);
			this.PlayTickSound(this.currentTick);
			this.UpdateVideoSlider();
		}
	}

	// Token: 0x060006F2 RID: 1778 RVA: 0x0002371C File Offset: 0x0002191C
	public void RecordSound(CollisionSoundType collisionSoundType, Vector3 position, float volume = 1f)
	{
		if (this.replayMode == ReplayMode.Record)
		{
			this.recording.recS.Add(new RS
			{
				cst = collisionSoundType,
				position = position,
				v = volume,
				tick = this.recording.ticks
			});
		}
	}

	// Token: 0x060006F3 RID: 1779 RVA: 0x00023770 File Offset: 0x00021970
	public void RecordCut(int id, GameObject parentObject, Plane plane, CuttableGameObject newCuttableGameObject)
	{
		if (this.replayMode == ReplayMode.Record)
		{
			for (int i = 0; i < this.recording.recRGO.Count; i++)
			{
				RGO rgo = this.recording.recRGO[i];
				if (rgo.gameObject == parentObject)
				{
					RGO rgo2 = new RGO();
					rgo2.startTick = this.recording.ticks;
					rgo2.name = "fc";
					rgo2.prefabName = "fc";
					rgo2.gameObject = newCuttableGameObject.gameObject;
					this.SetupChildForRecording(newCuttableGameObject.gameObject, rgo2, new TickMode?(TickMode.GlobalRotationAndPosition));
					this.recording.recRGO.Add(rgo2);
					rgo.recordableFullCuts.Add(new RFC
					{
						id = id,
						tick = this.recording.ticks,
						plane = plane,
						RGOI = this.recording.recRGO.Count - 1
					});
					return;
				}
			}
		}
	}

	// Token: 0x060006F4 RID: 1780 RVA: 0x00023874 File Offset: 0x00021A74
	public void RecordDamageablePart(int id, GameObject player, bool stopDestory, DamageOrigin? damageOrigin = null)
	{
		if (this.replayMode == ReplayMode.Record)
		{
			int i = 0;
			while (i < this.recording.recRGO.Count)
			{
				RGO rgo = this.recording.recRGO[i];
				if (rgo.gameObject == player)
				{
					if (!stopDestory)
					{
						rgo.recordableDamageablePart.Add(new RDP
						{
							id = id,
							tick = this.recording.ticks,
							sd = 0,
							dO = damageOrigin
						});
						return;
					}
					for (int j = 0; j < rgo.recordableDamageablePart.Count; j++)
					{
						rgo.recordableDamageablePart[j].sd = this.recording.ticks;
					}
					return;
				}
				else
				{
					i++;
				}
			}
		}
	}

	// Token: 0x060006F5 RID: 1781 RVA: 0x0002393C File Offset: 0x00021B3C
	public void RecordBluntDamage(GameObject player, BluntDamageEffect bluntDamageEffect)
	{
		if (this.replayMode == ReplayMode.Record)
		{
			for (int i = 0; i < this.recording.recRGO.Count; i++)
			{
				RGO rgo = this.recording.recRGO[i];
				if (rgo.gameObject == player)
				{
					RBH item = new RBH
					{
						id = (int)bluntDamageEffect.BodyPart,
						tick = this.recording.ticks,
						dmg = bluntDamageEffect.Damage,
						position = bluntDamageEffect.Position,
						bDmg = bluntDamageEffect.BloodDamage,
						v = bluntDamageEffect.Volume
					};
					rgo.recordableBluntHits.Add(item);
					return;
				}
			}
		}
	}

	// Token: 0x060006F6 RID: 1782 RVA: 0x000239F8 File Offset: 0x00021BF8
	public void PlayTick(int tickNumber, bool onlyAnimation = false, bool findLastValues = false)
	{
		float t = 0f;
		t = (float)((Time.timeAsDouble - this.lastFixedTickTime) / this.tickInterval);
		if (this.recording.ticks >= tickNumber + 1)
		{
			foreach (RGO rgo in this.recording.recRGO)
			{
				if (tickNumber < rgo.startTick && rgo.gameObject.activeInHierarchy)
				{
					rgo.Deactivate(false);
				}
				else if (tickNumber >= rgo.startTick && !rgo.gameObject.activeInHierarchy)
				{
					rgo.Activate();
				}
				int num = tickNumber - rgo.startTick;
				if (num >= 0)
				{
					foreach (RCGO rcgo in rgo.recordableChildGameObject)
					{
						if (rcgo.gameObject != null)
						{
							RT rt = rcgo.recordedTicks[num];
							if (rt.rotation != null)
							{
								rcgo.lastRotation = rt.rotation.Value;
							}
							if (rt.position != null)
							{
								rcgo.lastPosition = rt.position.Value;
							}
							RT rt2 = null;
							if (rcgo.recordedTicks.Count > num + 1)
							{
								rt2 = rcgo.recordedTicks[num + 1];
							}
							if (findLastValues && num != 0)
							{
								rt2 = new RT
								{
									rotation = rt2.rotation,
									position = rt2.position
								};
								int i = num;
								while (i > -1)
								{
									RT rt3 = rcgo.recordedTicks[i];
									if (rt3.rotation != null)
									{
										rcgo.lastRotation = rt3.rotation.Value;
										if (rt2.rotation == null)
										{
											rt2.rotation = rt3.rotation;
											break;
										}
										break;
									}
									else
									{
										i--;
									}
								}
								if (rcgo.tickMode == TickMode.GlobalRotationAndPosition)
								{
									int j = num;
									while (j > -1)
									{
										RT rt4 = rcgo.recordedTicks[j];
										if (rt4.position != null)
										{
											rcgo.lastPosition = rt4.position.Value;
											if (rt2.position == null)
											{
												rt2.position = rt4.position;
												break;
											}
											break;
										}
										else
										{
											j--;
										}
									}
								}
							}
							if (rt2 != null)
							{
								if (rcgo.tickMode == TickMode.GlobalRotationAndPosition)
								{
									if (rt2.position != null)
									{
										rcgo.gameObject.transform.position = Vector3.Lerp(rcgo.lastPosition, rt2.position.Value, t);
									}
									else if (num == 0)
									{
										rcgo.gameObject.transform.position = rcgo.lastPosition;
									}
									if (rt2.rotation != null)
									{
										rcgo.gameObject.transform.rotation = Quaternion.Lerp(rcgo.lastRotation, rt2.rotation.Value, t);
									}
									else if (num == 0)
									{
										rcgo.gameObject.transform.rotation = rcgo.lastRotation;
									}
								}
								else if (rcgo.tickMode == TickMode.GlobalRotation)
								{
									if (rt2.rotation != null)
									{
										rcgo.gameObject.transform.rotation = Quaternion.Lerp(rcgo.lastRotation, rt2.rotation.Value, t);
									}
									else if (num == 0)
									{
										rcgo.gameObject.transform.rotation = rcgo.lastRotation;
									}
								}
								else if (rt2.rotation != null)
								{
									rcgo.gameObject.transform.localRotation = Quaternion.Lerp(rcgo.lastRotation, rt2.rotation.Value, t);
								}
								else if (num == 0)
								{
									rcgo.gameObject.transform.localRotation = rcgo.lastRotation;
								}
							}
						}
					}
				}
				if (!onlyAnimation)
				{
					for (int k = 0; k < rgo.recordableDamageablePart.Count; k++)
					{
						RDP rdp = rgo.recordableDamageablePart[k];
						if (findLastValues)
						{
							if (rdp.WeaponDamageablePart != null)
							{
								if (rdp.tick <= tickNumber && (tickNumber < rdp.sd || (rdp.sd == -1 && !rdp.WeaponDamageablePart.bloodVessel)))
								{
									rdp.WeaponDamageablePart.SimulateDestroyVisuals((float)(tickNumber - rdp.tick) / (float)this.tickRate);
								}
								else
								{
									rdp.WeaponDamageablePart.ResetDestroyVisuals();
								}
							}
						}
						else if (rdp.WeaponDamageablePart != null && Time.timeScale != 0f)
						{
							if (rdp.tick == tickNumber && (!rdp.WeaponDamageablePart.bloodVessel || (rdp.sd != tickNumber && rdp.sd != -1)))
							{
								rdp.WeaponDamageablePart.PlayDestroyVisuals(rdp.dO);
							}
							else if (rdp.sd == tickNumber)
							{
								rdp.WeaponDamageablePart.StopDestroyVisuals();
							}
						}
					}
					for (int l = 0; l < rgo.recordableBluntHits.Count; l++)
					{
						RBH rbh = rgo.recordableBluntHits[l];
						if (!findLastValues && rbh.tick == tickNumber)
						{
							BluntDamageHelpers.HandleBluntDamageEffects(rgo.playerHealth, new BluntDamageEffect
							{
								BodyPart = (JointType)rbh.id,
								Damage = rbh.dmg,
								Position = rbh.position,
								BloodDamage = rbh.bDmg,
								Volume = rbh.v
							});
						}
						if (rbh.tick > tickNumber)
						{
							break;
						}
					}
				}
			}
		}
	}

	// Token: 0x060006F7 RID: 1783 RVA: 0x00024024 File Offset: 0x00022224
	public void PlayTickSound(int tickNumber)
	{
		if (this.recording.ticks >= tickNumber + 1)
		{
			IEnumerable<RS> recS = this.recording.recS;
			Func<RS, bool> <>9__0;
			Func<RS, bool> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = ((RS x) => x.tick == tickNumber));
			}
			foreach (RS rs in recS.Where(predicate))
			{
				if (SoundManager.singleton != null)
				{
					SoundManager.singleton.PlaySound(rs.cst, rs.position, rs.v);
				}
			}
		}
	}

	// Token: 0x060006F8 RID: 1784 RVA: 0x000240E0 File Offset: 0x000222E0
	public void LoadRecording(Recording newRecording)
	{
		if (newRecording != null)
		{
			this.recording = newRecording;
			this.replayMode = ReplayMode.StartReplayAfterLoad;
			this.LoadReplayScene();
		}
	}

	// Token: 0x060006F9 RID: 1785 RVA: 0x000240FC File Offset: 0x000222FC
	public void LoadReplayScene()
	{
		if (this.recording != null && !string.IsNullOrEmpty(this.recording.map))
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("DoReplayInit", true);
			SceneManagerWithParameters.LoadScene(this.recording.map, dictionary, false, false);
		}
	}

	// Token: 0x060006FA RID: 1786 RVA: 0x00024150 File Offset: 0x00022350
	public void StartReplay()
	{
		this.recordingPlayers = new List<PlayerHealth>();
		if (this.recording != null)
		{
			using (List<RGO>.Enumerator enumerator = this.recording.recRGO.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					RGO recordableGameObject = enumerator.Current;
					if (recordableGameObject.prefabName == "player")
					{
						GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.playerPrefab);
						recordableGameObject.isPlayer = true;
						recordableGameObject.weaponDamageableParts = gameObject.GetComponentsInChildren<WeaponDamageablePart>().ToList<WeaponDamageablePart>();
						for (int i = recordableGameObject.weaponDamageableParts.Count<WeaponDamageablePart>() - 1; i > -1; i--)
						{
							WeaponDamageablePart weaponDamageablePart = recordableGameObject.weaponDamageableParts[i];
							weaponDamageablePart.disableLocalLogic = true;
							for (int j = 0; j < recordableGameObject.recordableDamageablePart.Count; j++)
							{
								RDP rdp = recordableGameObject.recordableDamageablePart[j];
								if (rdp.id == weaponDamageablePart.id)
								{
									rdp.WeaponDamageablePart = weaponDamageablePart;
								}
							}
						}
						PlayerHealth component = gameObject.GetComponent<PlayerHealth>();
						if (component != null)
						{
							recordableGameObject.playerHealth = component;
							this.recordingPlayers.Add(component);
							component.InitMaterial();
							component.playerName = recordableGameObject.name;
							component.OnlyPhysical();
							recordableGameObject.SetCustomTexture();
							if (recordableGameObject.equippedEquipment != null)
							{
								List<EquippedEquipment> equippedEquipment = (from x in recordableGameObject.equippedEquipment
								where x.position != EquipmentPosition.HandLeft && x.position != EquipmentPosition.HandRight
								select x).ToList<EquippedEquipment>();
								component.SetEquipment(equippedEquipment, false);
							}
							this.SetupGameObjectForReplay(gameObject);
						}
						recordableGameObject.gameObject = gameObject;
						using (List<RCGO>.Enumerator enumerator2 = recordableGameObject.recordableChildGameObject.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								RCGO rcgo = enumerator2.Current;
								rcgo.gameObject = Generic.FindChildObject(gameObject.transform, rcgo.name, rcgo.parentName);
							}
							continue;
						}
					}
					if (recordableGameObject.prefabName == "equipment")
					{
						PlayerHealth component2 = this.playerPrefab.GetComponent<PlayerHealth>();
						if (component2 != null)
						{
							GameObject gameObject2 = (from x in component2.equipmentList
							where x.name == recordableGameObject.name
							select x).FirstOrDefault<GameObject>();
							if (gameObject2 != null)
							{
								GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(gameObject2);
								this.SetupGameObjectForReplay(gameObject3);
								recordableGameObject.gameObject = gameObject3;
								if (recordableGameObject.recordableChildGameObject.Count > 0)
								{
									recordableGameObject.recordableChildGameObject[0].gameObject = gameObject3;
								}
							}
						}
					}
					else if (recordableGameObject.prefabName == "fc")
					{
						GameObject gameObject4 = new GameObject();
						recordableGameObject.gameObject = gameObject4;
						if (recordableGameObject.recordableChildGameObject.Count > 0)
						{
							recordableGameObject.recordableChildGameObject[0].gameObject = gameObject4;
						}
					}
				}
			}
			this.GenerateCutItems();
			this.DeActivateAll();
			this.currentTick = -1;
			this.replayMode = ReplayMode.Replay;
			this.SetupReplayTools();
			this.ReplaySpeedChanged(3);
			this.SetPlayOrPause(true, false);
		}
	}

	// Token: 0x060006FB RID: 1787 RVA: 0x000244F8 File Offset: 0x000226F8
	private void DeActivateAll()
	{
		foreach (RGO rgo in from x in this.recording.recRGO
		orderby x.startTick descending
		select x)
		{
			rgo.Deactivate(true);
		}
	}

	// Token: 0x060006FC RID: 1788 RVA: 0x0002456C File Offset: 0x0002276C
	private void GenerateCutItems()
	{
		foreach (RGO rgo in this.recording.recRGO)
		{
			CuttableGameObject[] componentsInChildren = rgo.gameObject.GetComponentsInChildren<CuttableGameObject>();
			if (rgo.recordableFullCuts != null)
			{
				foreach (RFC rfc in rgo.recordableFullCuts)
				{
					CuttableGameObject cuttableGameObject = null;
					foreach (CuttableGameObject cuttableGameObject2 in componentsInChildren)
					{
						if (cuttableGameObject2.bodyPart == (JointType)rfc.id)
						{
							cuttableGameObject = cuttableGameObject2;
							break;
						}
					}
					RGO rgo2 = this.recording.recRGO[rfc.RGOI];
					rgo2.cutActivationItem = new ReplayCutActivationItem();
					CutItem cutItem = new CutItem
					{
						cuttableGameObject = cuttableGameObject,
						fullCutPlane = rfc.plane
					};
					cutItem.ResetMaterials();
					this.FillActivationItemBeforeCut(rgo2, cutItem);
					cutItem.DoFullCutNoWait();
					if (cutItem.newCuttableGameObject != null && rgo2 != null)
					{
						cutItem.newCuttableGameObject.transform.SetParent(rgo2.gameObject.transform);
						cutItem.newCuttableGameObject.transform.localPosition = default(Vector3);
						cutItem.newCuttableGameObject.transform.localRotation = default(Quaternion);
						this.FillActivationItemCuttableSections(rgo2, cutItem);
						this.FillActivationItemOriginalMeshs(rgo2, cutItem);
					}
					cutItem.DisposeNativeArrays();
				}
			}
		}
	}

	// Token: 0x060006FD RID: 1789 RVA: 0x00024744 File Offset: 0x00022944
	private void FillActivationItemOriginalMeshs(RGO rgo, CutItem cutItem)
	{
		if (cutItem.cuttableGameObject.cuttableMeshList.Count > 0)
		{
			foreach (CuttableMesh cuttableMesh in rgo.cutActivationItem.allCuttableMeshs)
			{
				if (cutItem.CuttableMeshWasCut(cuttableMesh))
				{
					rgo.cutActivationItem.originalCuttableMeshs.Add(cuttableMesh);
				}
			}
		}
	}

	// Token: 0x060006FE RID: 1790 RVA: 0x000247C4 File Offset: 0x000229C4
	private void FillActivationItemBeforeCut(RGO rgo, CutItem cutItem)
	{
		rgo.cutActivationItem.CutItem = cutItem;
		rgo.cutActivationItem.allCuttableMeshs = new List<CuttableMesh>();
		rgo.cutActivationItem.originalCuttableMeshs = new List<CuttableMesh>();
		rgo.cutActivationItem.newCuttableMeshs = new List<CuttableMesh>();
		rgo.cutActivationItem.arteryCuts = new List<WeaponDamageableArteryCut>();
		if (cutItem.cuttableGameObject.cuttableMeshList.Count > 0)
		{
			rgo.cutActivationItem.allCuttableMeshs.AddRange(cutItem.cuttableGameObject.cuttableMeshList);
		}
	}

	// Token: 0x060006FF RID: 1791 RVA: 0x0002484C File Offset: 0x00022A4C
	private void FillActivationItemCuttableSections(RGO rgo, CutItem cutItem)
	{
		if (cutItem.cuttableGameObject.cuttableSections.Count > 0)
		{
			rgo.cutActivationItem.cutSections = cutItem.cuttableSections.ToArray();
		}
		if (cutItem.cuttableGameObject.cuttableMeshList.Count > 0)
		{
			rgo.cutActivationItem.newCuttableMeshs.AddRange(cutItem.cuttableGameObject.cuttableMeshList);
		}
		for (int i = 0; i < cutItem.cuttableGameObject.cuttableSections.Count; i++)
		{
			CuttableSection cuttableSection = cutItem.cuttableGameObject.cuttableSections[i];
			if (cutItem.cuttableSections[i].isCut && cuttableSection.artery != null)
			{
				cuttableSection.artery.InitializeParticleSystem();
				WeaponDamageableArteryCut weaponDamageableArteryCut = new WeaponDamageableArteryCut
				{
					newParent = cutItem.cuttableGameObject.gameObject.transform,
					newPosition = cutItem.doCutJobOutValues[0].cutCenterPosition,
					newBodypart = cutItem.cuttableGameObject.bodyPart,
					oldBodypart = cuttableSection.artery.currentJointType,
					oldParent = cuttableSection.artery.bloodFlowParticles.transform.parent,
					oldPosition = cuttableSection.artery.bloodFlowParticles.transform.localPosition,
					oldRotation = cuttableSection.artery.bloodFlowParticles.transform.localRotation,
					oldBloodFlow = cuttableSection.artery.destroyed,
					WeaponDamageablePart = cuttableSection.artery
				};
				cuttableSection.artery.Destory(null, true);
				cuttableSection.artery.TryToSetEffectPosition(cutItem.cuttableGameObject.gameObject, cutItem.doCutJobOutValues[0].cutCenterPosition, cutItem.doCutJobOutValues[0].cutDirection, cutItem.cuttableGameObject.bodyPart);
				weaponDamageableArteryCut.newRotation = cuttableSection.artery.bloodFlowParticles.transform.localRotation;
				rgo.cutActivationItem.arteryCuts.Add(weaponDamageableArteryCut);
			}
		}
	}

	// Token: 0x06000700 RID: 1792 RVA: 0x00024A5C File Offset: 0x00022C5C
	private void SetupGameObjectForReplay(GameObject replayGameObject)
	{
		this.SetupCuttableSectionsForTesting(replayGameObject);
		foreach (Rigidbody rigidbody in replayGameObject.GetComponentsInChildren<Rigidbody>())
		{
			rigidbody.isKinematic = true;
			rigidbody.interpolation = RigidbodyInterpolation.None;
		}
		ConfigurableJoint[] componentsInChildren2 = replayGameObject.GetComponentsInChildren<ConfigurableJoint>();
		for (int j = componentsInChildren2.Count<ConfigurableJoint>() - 1; j > -1; j--)
		{
			UnityEngine.Object.Destroy(componentsInChildren2[j]);
		}
		Blade[] componentsInChildren3 = replayGameObject.GetComponentsInChildren<Blade>();
		for (int k = componentsInChildren3.Count<Blade>() - 1; k > -1; k--)
		{
			componentsInChildren3[k].disableLocalLogic = true;
		}
		Hand[] componentsInChildren4 = replayGameObject.GetComponentsInChildren<Hand>();
		for (int l = componentsInChildren4.Count<Hand>() - 1; l > -1; l--)
		{
			componentsInChildren4[l].disableLocalLogic = true;
		}
	}

	// Token: 0x06000701 RID: 1793 RVA: 0x00024B14 File Offset: 0x00022D14
	private void SetupCuttableSectionsForTesting(GameObject replayGameObject)
	{
		if (CutManager.singleton != null && CutManager.singleton.forceCutManagerOnForTesting)
		{
			CuttableGameObject[] componentsInChildren = replayGameObject.GetComponentsInChildren<CuttableGameObject>();
			for (int i = componentsInChildren.Count<CuttableGameObject>() - 1; i > -1; i--)
			{
				CuttableGameObject cuttableGameObject = componentsInChildren[i];
				if (cuttableGameObject.cuttableSections != null)
				{
					foreach (CuttableSection cuttableSection in cuttableGameObject.cuttableSections)
					{
						if (cuttableSection.joint != null)
						{
							cuttableSection.gameObjectTransform = cuttableSection.joint.connectedBody.transform;
						}
					}
				}
			}
		}
	}

	// Token: 0x06000702 RID: 1794 RVA: 0x00024BCC File Offset: 0x00022DCC
	private void SetupReplayTools()
	{
		if (this.replayToolsManager == null && this.recording != null)
		{
			this.userControls.ReplayMap.Enable();
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.replayToolPrefab);
			this.replayToolsManager = gameObject.GetComponentInChildren<ReplayToolsManager>();
			this.replayToolsManager.backButton.onClick.AddListener(delegate()
			{
				this.ResetReplay();
			});
			this.replayToolsManager.pauseButton.onClick.AddListener(delegate()
			{
				this.SetPlayOrPause(false, false);
			});
			this.replayToolsManager.playButton.onClick.AddListener(delegate()
			{
				this.SetPlayOrPause(true, false);
			});
			this.replayToolsManager.replayPositionSlider.minValue = 0f;
			this.replayToolsManager.replayPositionSlider.maxValue = (float)this.recording.ticks;
			this.replayToolsManager.replayPositionSlider.wholeNumbers = true;
			this.replayToolsManager.replayPositionSlider.onValueChanged.AddListener(delegate(float <p0>)
			{
				this.SetCurrentTick((int)this.replayToolsManager.replayPositionSlider.value);
			});
			this.replayToolsManager.replaySpeedDropdown.onValueChanged.AddListener(delegate(int <p0>)
			{
				this.ReplaySpeedChanged(this.replayToolsManager.replaySpeedDropdown.value);
			});
			this.replayToolsManager.replaySpeedDropdown.value = 3;
			this.replayToolsManager.cameraModeDropdown.onValueChanged.AddListener(delegate(int <p0>)
			{
				if (ReplayCameraControls.singleton != null)
				{
					ReplayCameraControls.singleton.SetCameraMode((CameraMode)this.replayToolsManager.cameraModeDropdown.value);
				}
			});
			this.replayToolsManager.followPlayerDropdown.onValueChanged.AddListener(delegate(int <p0>)
			{
				if (ReplayCameraControls.singleton != null)
				{
					ReplayCameraControls.singleton.SetFollowedPlayer(this.replayToolsManager.followPlayerDropdown.value);
				}
			});
			this.replayToolsManager.followPlayerDropdown.options.Clear();
			foreach (PlayerHealth playerHealth in this.recordingPlayers)
			{
				this.replayToolsManager.followPlayerDropdown.options.Add(new Dropdown.OptionData(playerHealth.playerName));
			}
			this.replayToolsManager.followPlayerDropdown.RefreshShownValue();
			this.SetupReplayCamera();
			if (GeneralManager.singleton != null)
			{
				GeneralManager.singleton.UpdateCursorState();
			}
		}
	}

	// Token: 0x06000703 RID: 1795 RVA: 0x00024DFC File Offset: 0x00022FFC
	private void SetupReplayCamera()
	{
		if (Camera.main != null && ReplayCameraControls.singleton == null)
		{
			Camera.main.gameObject.AddComponent<ReplayCameraControls>();
		}
	}

	// Token: 0x06000704 RID: 1796 RVA: 0x00024E28 File Offset: 0x00023028
	private void ResetReplay()
	{
		if (ParticleDisplayer.singleton != null)
		{
			ParticleDisplayer.singleton.ClearParticles();
		}
		this.SetCurrentTick(0);
		this.UpdateVideoSlider();
	}

	// Token: 0x06000705 RID: 1797 RVA: 0x00024E50 File Offset: 0x00023050
	public void SetPlayOrPause(bool play, bool temp = false)
	{
		if (!temp)
		{
			this.playState = play;
		}
		if (play)
		{
			Time.timeScale = this.currentReplaySpeed;
			this.replayToolsManager.playButton.gameObject.SetActive(false);
			this.replayToolsManager.pauseButton.gameObject.SetActive(true);
			return;
		}
		this.replayToolsManager.playButton.gameObject.SetActive(true);
		this.replayToolsManager.pauseButton.gameObject.SetActive(false);
		Time.timeScale = 0f;
	}

	// Token: 0x06000706 RID: 1798 RVA: 0x00024ED8 File Offset: 0x000230D8
	public void CheckTempPauseStatus()
	{
		if (this.replayMode == ReplayMode.Replay)
		{
			if (this.draggingTimeline || (GameMenu.singleton != null && GameMenu.singleton.menuHolderPanel.activeInHierarchy))
			{
				this.SetPlayOrPause(false, true);
				return;
			}
			this.SetPlayOrPause(this.playState, true);
		}
	}

	// Token: 0x06000707 RID: 1799 RVA: 0x00024F2A File Offset: 0x0002312A
	private void SetReplaySpeed(float replaySpeed)
	{
		this.currentReplaySpeed = replaySpeed;
		if (!Mathf.Approximately(0f, Time.timeScale))
		{
			Time.timeScale = this.currentReplaySpeed;
		}
	}

	// Token: 0x06000708 RID: 1800 RVA: 0x00024F50 File Offset: 0x00023150
	private void ReplaySpeedChanged(int replayValue)
	{
		float replaySpeed;
		switch (replayValue)
		{
		case 0:
			replaySpeed = 0.25f;
			break;
		case 1:
			replaySpeed = 0.5f;
			break;
		case 2:
			replaySpeed = 0.75f;
			break;
		case 3:
			replaySpeed = 1f;
			break;
		case 4:
			replaySpeed = 1.25f;
			break;
		case 5:
			replaySpeed = 1.5f;
			break;
		default:
			replaySpeed = 1f;
			break;
		}
		this.replayToolsManager.replaySpeedDropdown.SetValueWithoutNotify(replayValue);
		this.SetReplaySpeed(replaySpeed);
	}

	// Token: 0x06000709 RID: 1801 RVA: 0x00024FD1 File Offset: 0x000231D1
	private void UpdateVideoSlider()
	{
		this.replayToolsManager.replayPositionSlider.value = (float)this.currentTick;
	}

	// Token: 0x0600070A RID: 1802 RVA: 0x00024FEA File Offset: 0x000231EA
	private void SetCurrentTick(int newTick)
	{
		this.currentTick = newTick;
		if (Time.timeScale == 0f)
		{
			this.PlayTick(this.currentTick, false, true);
		}
	}

	// Token: 0x0600070B RID: 1803 RVA: 0x0002500D File Offset: 0x0002320D
	private void ToggleToolbarVisibility(InputAction.CallbackContext obj)
	{
		this.replayToolsManager.gameObject.SetActive(!this.replayToolsManager.gameObject.activeInHierarchy);
		if (GeneralManager.singleton != null)
		{
			GeneralManager.singleton.UpdateCursorState();
		}
	}

	// Token: 0x0600070C RID: 1804 RVA: 0x00025049 File Offset: 0x00023249
	private void TogglePlay(InputAction.CallbackContext obj)
	{
		this.SetPlayOrPause(!this.playState, false);
	}

	// Token: 0x17000106 RID: 262
	// (get) Token: 0x0600070D RID: 1805 RVA: 0x0002505B File Offset: 0x0002325B
	public static bool ToolsVisible
	{
		get
		{
			return ReplayManager.singleton != null && ReplayManager.singleton.replayToolsManager != null && ReplayManager.singleton.replayToolsManager.gameObject.activeInHierarchy;
		}
	}

	// Token: 0x17000107 RID: 263
	// (get) Token: 0x0600070E RID: 1806 RVA: 0x00025095 File Offset: 0x00023295
	public static bool PlayingReplay
	{
		get
		{
			return ReplayManager.singleton != null && (ReplayManager.singleton.replayMode == ReplayMode.Replay || ReplayManager.singleton.replayMode == ReplayMode.StartReplayAfterLoad) && ReplayManager.singleton.recording != null;
		}
	}

	// Token: 0x0600070F RID: 1807 RVA: 0x000250D0 File Offset: 0x000232D0
	public bool IsLocalPlayer(PlayerHealth player)
	{
		if (this.recording == null || this.recording.recRGO == null)
		{
			return false;
		}
		for (int i = 0; i < this.recording.recRGO.Count; i++)
		{
			RGO rgo = this.recording.recRGO[i];
			if (rgo != null && rgo.playerHealth == player && rgo.isLocalPlayer)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000710 RID: 1808 RVA: 0x00025140 File Offset: 0x00023340
	private void ParseTicksFromRecording()
	{
		if (this.recording != null)
		{
			ReplayManager.recordEveryTick = false;
			foreach (RGO rgo in this.recording.recRGO)
			{
				foreach (RCGO rcgo in rgo.recordableChildGameObject)
				{
					for (int i = 0; i < rcgo.recordedTicks.Count; i++)
					{
						RT rt = rcgo.recordedTicks[i];
						if (i != 0)
						{
							rt = this.GetRecordedTick(rcgo.lastRotation, rt.rotation.Value, (rt.position != null) ? new Vector3?(rcgo.lastPosition) : null, rt.position);
						}
						if (rt.position != null)
						{
							rcgo.lastPosition = rt.position.Value;
						}
						if (rt.rotation != null)
						{
							rcgo.lastRotation = rt.rotation.Value;
						}
						rcgo.recordedTicks[i] = rt;
					}
				}
			}
			Recording recording = this.recording;
			recording.name += "(parsed)";
			this.SaveRecording();
		}
	}

	// Token: 0x06000711 RID: 1809 RVA: 0x000252FC File Offset: 0x000234FC
	private void OnDestroy()
	{
		this.DisposeUserControls();
	}

	// Token: 0x06000712 RID: 1810 RVA: 0x00025304 File Offset: 0x00023504
	public void DisposeUserControls()
	{
		if (this.userControls != null)
		{
			this.userControls.Disable();
			this.userControls.Dispose();
		}
	}

	// Token: 0x040004B0 RID: 1200
	public Recording recording;

	// Token: 0x040004B1 RID: 1201
	public ReplayMode replayMode;

	// Token: 0x040004B2 RID: 1202
	public static ReplayManager singleton;

	// Token: 0x040004B3 RID: 1203
	public GameObject playerPrefab;

	// Token: 0x040004B4 RID: 1204
	public GameObject replayToolPrefab;

	// Token: 0x040004B5 RID: 1205
	public ReplayToolsManager replayToolsManager;

	// Token: 0x040004B6 RID: 1206
	public UserControls userControls;

	// Token: 0x040004B7 RID: 1207
	public List<PlayerHealth> recordingPlayers = new List<PlayerHealth>();

	// Token: 0x040004B8 RID: 1208
	public static int maxRecordingTicks = 18000;

	// Token: 0x040004B9 RID: 1209
	public static bool recordEveryTick = false;

	// Token: 0x040004BA RID: 1210
	private bool pendingSave;

	// Token: 0x040004BB RID: 1211
	public int currentTick;

	// Token: 0x040004BC RID: 1212
	private int tickRate = 30;

	// Token: 0x040004BD RID: 1213
	public double lastFixedTickTime;

	// Token: 0x040004BE RID: 1214
	public float currentReplaySpeed = 1f;

	// Token: 0x040004BF RID: 1215
	private bool playState = true;

	// Token: 0x040004C0 RID: 1216
	public bool draggingTimeline;
}
