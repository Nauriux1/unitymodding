using System;
using System.Collections.Generic;
using System.Linq;
using MoveClasses;
using UnityEngine;
using Utils;

// Token: 0x02000150 RID: 336
public class PlayerAnimator : MonoBehaviour
{
	// Token: 0x06000A82 RID: 2690 RVA: 0x00031071 File Offset: 0x0002F271
	private void Awake()
	{
		this.moveSet = null;
		this.InitRunningSingleMoves();
		this.spawnTime = Time.time;
		this.InitCancellablePlayerActions();
	}

	// Token: 0x06000A83 RID: 2691 RVA: 0x00031091 File Offset: 0x0002F291
	private void Start()
	{
		this.InitializePlayerAnimator();
	}

	// Token: 0x06000A84 RID: 2692 RVA: 0x0003109C File Offset: 0x0002F29C
	private void InitializePlayerAnimator()
	{
		if (this.initialized)
		{
			return;
		}
		this.player = (PlayerHealth)base.GetComponentsInParent(typeof(PlayerHealth))[0];
		this.GenerateFighterJoint(base.gameObject);
		using (List<FighterJoint>.Enumerator enumerator = this.FighterJoints.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				FighterJoint joint = enumerator.Current;
				try
				{
					GameObject physicsJoint = Generic.FindChildObject(this.physicalPlayer.transform, joint.joint.name, null);
					joint.physicsJoint = physicsJoint;
					if (joint.physicsJoint != null)
					{
						JointStrength jointStrength = (from x in joint.physicsJoint.GetComponents<JointStrength>()
						where x.jointName == joint.joint.name
						select x).FirstOrDefault<JointStrength>();
						if (jointStrength == null)
						{
							jointStrength = (from x in joint.physicsJoint.transform.parent.GetComponents<JointStrength>()
							where x.jointName == joint.joint.name
							select x).FirstOrDefault<JointStrength>();
						}
						joint.jointStrength = jointStrength;
						if (joint.physicsJoint.name.Contains("WRIST"))
						{
							joint.hand = joint.physicsJoint.GetComponentInChildren<Hand>();
						}
					}
				}
				catch (Exception message)
				{
					Debug.LogError(message);
				}
			}
		}
		this.initialized = true;
	}

	// Token: 0x1700015D RID: 349
	// (get) Token: 0x06000A85 RID: 2693 RVA: 0x00031248 File Offset: 0x0002F448
	private bool activeAnimationStarted
	{
		get
		{
			return !this.activeAnimationRunningPrevious && this.activeAnimationRunning;
		}
	}

	// Token: 0x1700015E RID: 350
	// (get) Token: 0x06000A86 RID: 2694 RVA: 0x0003125D File Offset: 0x0002F45D
	private bool activeAnimationEnded
	{
		get
		{
			return this.activeAnimationRunningPrevious && !this.activeAnimationRunning;
		}
	}

	// Token: 0x06000A87 RID: 2695 RVA: 0x00031272 File Offset: 0x0002F472
	public void FixedUpdate()
	{
		if (this.animateAtFixedUpdate)
		{
			this.HandleAnimation();
		}
	}

	// Token: 0x06000A88 RID: 2696 RVA: 0x00031284 File Offset: 0x0002F484
	public void HandleAnimation()
	{
		this.ClearAnimatedJoints();
		this.activeAnimationRunning = this.ActiveAnimationRunning();
		if (this.activeAnimationEnded && !this.skipPassiveExtension)
		{
			float num = Time.time - this.lastActiveAnimationStarted;
			for (int i = 0; i < this.RunningSingleMoves.Count; i++)
			{
				RunningSingleMove runningSingleMove = this.RunningSingleMoves[i];
				if (runningSingleMove.move.inputType == inputType.Passive)
				{
					runningSingleMove.executeAtTime += (double)num;
					runningSingleMove.removeTime += (double)num;
					runningSingleMove.remove = false;
				}
			}
		}
		this.skipPassiveExtension = false;
		if (this.animate && this.RunningSingleMoves != null)
		{
			PlayerAnimator.RunningSingleMovesQuickSort(this.RunningSingleMoves);
			for (int j = 0; j < this.RunningSingleMoves.Count; j++)
			{
				RunningSingleMove runningSingleMove2 = this.RunningSingleMoves[j];
				if ((runningSingleMove2.executeAtTime <= (double)Time.time && runningSingleMove2.move.inputType != inputType.Passive) || runningSingleMove2.previewOnTheTick)
				{
					this.ProcessRunningSingleMove(runningSingleMove2);
				}
			}
			this.timeToUseForPassiveMoves = Time.time;
			if (this.activeAnimationRunning && !this.activeAnimationStarted && !this.playingMovePreview)
			{
				this.timeToUseForPassiveMoves = this.lastActiveAnimationStarted;
			}
			for (int k = 0; k < this.RunningSingleMoves.Count; k++)
			{
				RunningSingleMove runningSingleMove3 = this.RunningSingleMoves[k];
				if ((runningSingleMove3.executeAtTime <= (double)this.timeToUseForPassiveMoves && runningSingleMove3.move.inputType == inputType.Passive) || runningSingleMove3.previewOnTheTick)
				{
					this.ProcessRunningSingleMove(runningSingleMove3);
				}
			}
		}
		for (int l = 0; l < this.RunningSingleMoves.Count; l++)
		{
			RunningSingleMove runningSingleMove4 = this.RunningSingleMoves[l];
			if (runningSingleMove4.remove && runningSingleMove4.move.inputType == inputType.Continuous)
			{
				runningSingleMove4.executeAtTime += (double)runningSingleMove4.move.duration;
				runningSingleMove4.removeTime += (double)runningSingleMove4.move.duration;
				runningSingleMove4.remove = false;
			}
		}
		this.tempRunningSingleMoves.Clear();
		for (int m = 0; m < this.RunningSingleMoves.Count; m++)
		{
			RunningSingleMove runningSingleMove5 = this.RunningSingleMoves[m];
			if ((!runningSingleMove5.remove || runningSingleMove5.move.inputType == inputType.Passive) && !runningSingleMove5.preview)
			{
				this.tempRunningSingleMoves.Add(runningSingleMove5);
			}
			else
			{
				this.ReturnRunningSingleMoveToPool(runningSingleMove5, false);
			}
		}
		this.TempRunningMovesToRunningSingleMoves();
		if (!this.activeAnimationRunning)
		{
			for (int n = 0; n < this.RunningSingleMoves.Count; n++)
			{
				RunningSingleMove runningSingleMove6 = this.RunningSingleMoves[n];
				if (runningSingleMove6.remove && runningSingleMove6.move.inputType == inputType.Passive)
				{
					runningSingleMove6.executeAtTime += (double)runningSingleMove6.move.duration;
					runningSingleMove6.removeTime += (double)runningSingleMove6.move.duration;
					runningSingleMove6.remove = false;
				}
			}
		}
		if (this.activeAnimationStarted)
		{
			this.lastActiveAnimationStarted = Time.time;
		}
		this.activeAnimationRunningPrevious = this.activeAnimationRunning;
	}

	// Token: 0x06000A89 RID: 2697 RVA: 0x000315B4 File Offset: 0x0002F7B4
	public void ProcessRunningSingleMove(RunningSingleMove singleMove)
	{
		bool flag = false;
		if (this.playingMovePreview && singleMove.previewPercentage == null)
		{
			flag = true;
			if (singleMove.singleMove.inPreviewList)
			{
				RunningSingleMove previousPreviewSingleMove = this.GetPreviousPreviewSingleMove(singleMove.singleMove);
				if (previousPreviewSingleMove != null)
				{
					singleMove.previewPercentage = previousPreviewSingleMove.previewPercentage;
					this.previousPreviewSingleMoves.Remove(previousPreviewSingleMove);
					this.ReturnRunningSingleMoveToPool(previousPreviewSingleMove, true);
				}
				else
				{
					singleMove.previewPercentage = null;
				}
			}
		}
		FighterJoint fighterJoint = this.GetFighterJoint(singleMove.singleMove.joint);
		if (fighterJoint != null)
		{
			if (this.CurrentlyHighestPriorityAnimationForJoint(singleMove))
			{
				RunningSingleMove nextRunningSingleMove = this.GetNextRunningSingleMove(singleMove);
				if (singleMove.move.inputType == inputType.Continuous && singleMove.singleMove.lastMoveForJoint && singleMove.singleMove.NextMove != null)
				{
					this.tempRunningSingleMoveForProcessing.Clear();
					this.tempRunningSingleMoveForProcessing.singleMove = singleMove.singleMove.NextMove;
					this.tempRunningSingleMoveForProcessing.executeAtTime = singleMove.executeAtTime + ((double)singleMove.move.duration - singleMove.singleMove.executionTime) + singleMove.singleMove.NextMove.executionTime;
					this.tempRunningSingleMoveForProcessing.removeTime = 0.0;
					this.tempRunningSingleMoveForProcessing.removeAfterRunningOnce = singleMove.removeAfterRunningOnce;
					this.tempRunningSingleMoveForProcessing.move = singleMove.move;
					this.tempRunningSingleMoveForProcessing.preview = singleMove.preview;
					nextRunningSingleMove = this.tempRunningSingleMoveForProcessing;
				}
				this.newTargetRotation.SetValues(singleMove.singleMove.targetRotation.x, singleMove.singleMove.targetRotation.y, singleMove.singleMove.targetRotation.z);
				if (nextRunningSingleMove != null)
				{
					double num = singleMove.executeAtTime;
					double num2 = nextRunningSingleMove.executeAtTime;
					if (singleMove.move.inputType != inputType.Passive || nextRunningSingleMove.move.inputType != inputType.Passive)
					{
						if (singleMove.move.inputType == inputType.Passive || (singleMove.move.inputType == inputType.HoldDown && singleMove.singleMove.lastMoveForJoint))
						{
							num = nextRunningSingleMove.executeAtTime - nextRunningSingleMove.singleMove.executionTime;
						}
						if (nextRunningSingleMove.move.inputType == inputType.Passive)
						{
							num2 = singleMove.removeTime;
						}
					}
					double value = ((double)Time.time - singleMove.moveSetExcecutionStartTime - num) / (num2 - num);
					if (singleMove.move.inputType == inputType.HoldDown && singleMove.singleMove.lastMoveForJoint && nextRunningSingleMove.move.layer < singleMove.move.layer)
					{
						value = 0.0;
					}
					if (this.playingMovePreview && singleMove.previewPercentage != null)
					{
						value = singleMove.previewPercentage.Value;
					}
					Vector3 euler = singleMove.singleMove.targetRotation.ConvertToVector3();
					Vector3 euler2 = nextRunningSingleMove.singleMove.targetRotation.ConvertToVector3();
					Quaternion a = Quaternion.Euler(euler);
					Quaternion b = Quaternion.Euler(euler2);
					if (!this.playingMovePreview)
					{
						if (singleMove.move.inputType == inputType.Passive && nextRunningSingleMove.move.inputType != inputType.Passive && singleMove.tempQuaternion != null)
						{
							a = singleMove.tempQuaternion.Value;
						}
						else if (singleMove.move.inputType != inputType.Passive && nextRunningSingleMove.move.inputType == inputType.Passive && nextRunningSingleMove.tempQuaternion != null)
						{
							b = nextRunningSingleMove.tempQuaternion.Value;
						}
					}
					Quaternion value2 = default(Quaternion);
					if (singleMove.move.inputType == inputType.Passive && nextRunningSingleMove.move.inputType == inputType.Passive && singleMove.tempQuaternion != null && this.activeAnimationRunning && !this.playingMovePreview)
					{
						value2 = singleMove.tempQuaternion.Value;
					}
					else
					{
						value2 = Quaternion.Slerp(a, b, Convert.ToSingle(value));
					}
					this.newTargetRotation.SetValues(value2.eulerAngles);
					if (!this.activeAnimationRunning)
					{
						if (singleMove.move.inputType == inputType.Passive && nextRunningSingleMove.move.inputType == inputType.Passive && singleMove.singleMove != nextRunningSingleMove.singleMove)
						{
							singleMove.tempQuaternion = new Quaternion?(value2);
						}
						else
						{
							singleMove.tempQuaternion = null;
						}
					}
					if (this.playingMovePreview)
					{
						singleMove.previewPercentage = new double?(value);
					}
				}
				Vector3 vector = this.GetNewRotation(fighterJoint.joint.transform.localEulerAngles, this.newTargetRotation, null);
				if (fighterJoint.jointType == JointType.HIP)
				{
					vector = Generic.ClampRotation(vector, new float?(MoveSetHelpers.hipRotationMax), null, new float?(MoveSetHelpers.hipRotationMax));
				}
				fighterJoint.joint.transform.localEulerAngles = vector;
				if (fighterJoint.jointType == JointType.SCAPULA_LEFT || fighterJoint.jointType == JointType.SCAPULA_RIGHT)
				{
					Vector3 vector2 = fighterJoint.joint.transform.GetChild(0).transform.position - fighterJoint.joint.transform.position;
					float num3 = Vector3.Angle(fighterJoint.joint.transform.parent.up, vector2);
					Debug.DrawRay(fighterJoint.joint.transform.position, vector2, Color.red);
					if (num3 > 90f)
					{
						Vector3 axis = Vector3.Cross(vector2, fighterJoint.joint.transform.parent.up);
						Quaternion lhs = Quaternion.AngleAxis(num3 - 90f, axis);
						fighterJoint.joint.transform.rotation = lhs * fighterJoint.joint.transform.rotation;
					}
				}
				this.SetAnimatedJoint(singleMove.move.layer, fighterJoint.jointType);
				if (fighterJoint.hand != null && singleMove.singleMove.handState != null)
				{
					fighterJoint.hand.SetHandState(singleMove.singleMove.handState.Value);
				}
				if (singleMove.removeAfterRunningOnce)
				{
					singleMove.remove = true;
				}
			}
			if (this.playingMovePreview && !singleMove.singleMove.temp && flag)
			{
				singleMove.singleMove.inPreviewList = true;
				this.previousPreviewSingleMoves.Add(singleMove);
			}
		}
		if (singleMove.removeTime <= (double)Time.time && (!singleMove.singleMove.lastMoveForJoint || singleMove.move.inputType != inputType.HoldDown))
		{
			singleMove.remove = true;
		}
	}

	// Token: 0x06000A8A RID: 2698 RVA: 0x00031C38 File Offset: 0x0002FE38
	public Vector3 GetNewRotation(Vector3 CurrentRotation, NullableVector3 NewRotation, float? axisValue = null)
	{
		if (NewRotation.x != null)
		{
			CurrentRotation.x = NewRotation.x.Value;
			if (axisValue != null)
			{
				CurrentRotation.x *= axisValue.Value;
			}
		}
		if (NewRotation.y != null)
		{
			CurrentRotation.y = NewRotation.y.Value;
			if (axisValue != null)
			{
				CurrentRotation.y *= axisValue.Value;
			}
		}
		if (NewRotation.z != null)
		{
			CurrentRotation.z = NewRotation.z.Value;
			if (axisValue != null)
			{
				CurrentRotation.z *= axisValue.Value;
			}
		}
		return CurrentRotation;
	}

	// Token: 0x06000A8B RID: 2699 RVA: 0x00031D10 File Offset: 0x0002FF10
	public void PlayMove(Move move, bool movePreviewActive = false, bool playOnlyActive = false, float runningTime = 0f, bool runOnce = false)
	{
		this.GenerateTempMoves(move);
		float num = Time.time;
		if (runningTime != 0f)
		{
			num -= runningTime;
		}
		this.tempJointMoves.Clear();
		if (movePreviewActive)
		{
			for (int i = 0; i < move.jointMoveList.Count; i++)
			{
				JointMove jointMove = move.jointMoveList[i];
				if (jointMove.temp)
				{
					this.tempJointMoves.Add(jointMove);
				}
			}
			this.RunningSingleMoves.Clear();
			Move passiveMove = this.GetPassiveMove();
			if (passiveMove != null && move.inputType != inputType.Passive)
			{
				List<JointType> list = new List<JointType>();
				using (IEnumerator<JointMove> enumerator = (from x in passiveMove.jointMoveList
				orderby x.executionTime
				select x).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						JointMove singleMove = enumerator.Current;
						JointMove jointMove2 = (from x in this.tempJointMoves
						where x.joint == singleMove.joint
						select x).FirstOrDefault<JointMove>();
						if ((from x in list
						where x == singleMove.joint
						select x).Count<JointType>() <= 0 && (jointMove2 == null || singleMove.temp || singleMove.joint != jointMove2.joint || !Generic.DoubleEquals(singleMove.executionTime, jointMove2.executionTime)))
						{
							list.Add(singleMove.joint);
							RunningSingleMove runningSingleMoveFromPool = this.GetRunningSingleMoveFromPool();
							runningSingleMoveFromPool.singleMove = singleMove;
							runningSingleMoveFromPool.executeAtTime = (double)num + singleMove.executionTime;
							runningSingleMoveFromPool.removeTime = (double)(num + passiveMove.duration);
							runningSingleMoveFromPool.removeAfterRunningOnce = runOnce;
							runningSingleMoveFromPool.move = passiveMove;
							runningSingleMoveFromPool.preview = movePreviewActive;
							this.RunningSingleMoves.Add(runningSingleMoveFromPool);
						}
					}
				}
				list.Clear();
				using (IEnumerator<JointMove> enumerator = (from x in passiveMove.jointMoveList
				orderby x.executionTime
				select x).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						JointMove singleMove = enumerator.Current;
						JointMove jointMove3 = (from x in this.tempJointMoves
						where x.joint == singleMove.joint
						select x).FirstOrDefault<JointMove>();
						if ((from x in list
						where x == singleMove.joint
						select x).Count<JointType>() <= 0 && (jointMove3 == null || singleMove.temp || singleMove.joint != jointMove3.joint || !Generic.DoubleEquals(singleMove.executionTime, jointMove3.executionTime)))
						{
							list.Add(singleMove.joint);
							RunningSingleMove runningSingleMoveFromPool2 = this.GetRunningSingleMoveFromPool();
							runningSingleMoveFromPool2.singleMove = singleMove;
							runningSingleMoveFromPool2.executeAtTime = (double)num + singleMove.executionTime + (double)passiveMove.duration;
							runningSingleMoveFromPool2.removeTime = (double)(num + passiveMove.duration + passiveMove.duration);
							runningSingleMoveFromPool2.removeAfterRunningOnce = runOnce;
							runningSingleMoveFromPool2.move = passiveMove;
							runningSingleMoveFromPool2.preview = movePreviewActive;
							this.RunningSingleMoves.Add(runningSingleMoveFromPool2);
						}
					}
				}
			}
			this.PrepareMoveForUse(move);
		}
		if (move.inputType == inputType.Passive && this.PassiveAnimationRunning())
		{
			return;
		}
		if (move.jointMoveList != null)
		{
			using (List<JointMove>.Enumerator enumerator2 = move.jointMoveList.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					JointMove singleMove = enumerator2.Current;
					double? previewPercentage = null;
					if (movePreviewActive)
					{
						JointMove jointMove4 = (from x in this.tempJointMoves
						where x.joint == singleMove.joint
						select x).FirstOrDefault<JointMove>();
						if (jointMove4 != null && !singleMove.temp && singleMove.joint == jointMove4.joint && Generic.DoubleEquals(singleMove.executionTime, jointMove4.executionTime))
						{
							continue;
						}
						if (Generic.DoubleEquals(singleMove.executionTime, (double)runningTime))
						{
							previewPercentage = new double?(0.0);
						}
					}
					double num2 = (double)(num + move.duration);
					if (move.inputType == inputType.Passive)
					{
						JointMove nextMoveForJointFromList = this.GetNextMoveForJointFromList(singleMove, move.jointMoveList);
						if (nextMoveForJointFromList != null)
						{
							num2 = (double)num + nextMoveForJointFromList.executionTime;
						}
					}
					if (move.inputType == inputType.Continuous && singleMove.NextMove != null)
					{
						num2 = (double)num + singleMove.NextMove.executionTime;
						if (singleMove.lastMoveForJoint)
						{
							num2 += (double)move.duration;
						}
					}
					RunningSingleMove runningSingleMoveFromPool3 = this.GetRunningSingleMoveFromPool();
					runningSingleMoveFromPool3.singleMove = singleMove;
					runningSingleMoveFromPool3.executeAtTime = (double)num + singleMove.executionTime;
					runningSingleMoveFromPool3.removeTime = num2;
					runningSingleMoveFromPool3.removeAfterRunningOnce = runOnce;
					runningSingleMoveFromPool3.move = move;
					runningSingleMoveFromPool3.preview = movePreviewActive;
					runningSingleMoveFromPool3.previewPercentage = previewPercentage;
					runningSingleMoveFromPool3.previewOnTheTick = (previewPercentage != null);
					this.RunningSingleMoves.Add(runningSingleMoveFromPool3);
				}
			}
		}
		this.CleanUpTempMoves(move);
	}

	// Token: 0x06000A8C RID: 2700 RVA: 0x000322BC File Offset: 0x000304BC
	private void GenerateTempMoves(Move move)
	{
		if (move.inputType == inputType.PlayAtStart)
		{
			foreach (FighterJoint fighterJoint in this.FighterJoints)
			{
				if (!MoveSetHelpers.MoveForJointExistsInList(move, fighterJoint, new double?(0.0)))
				{
					JointMove item = new JointMove
					{
						joint = fighterJoint.jointType,
						targetRotation = new NullableVector3(new float?(0f), new float?(0f), new float?(0f)),
						executionTime = 0.0,
						tempGenerated = true,
						inPreviewList = true
					};
					move.jointMoveList.Add(item);
				}
			}
		}
	}

	// Token: 0x06000A8D RID: 2701 RVA: 0x00032398 File Offset: 0x00030598
	private void CleanUpTempMoves(Move move)
	{
		if (move.inputType == inputType.PlayAtStart)
		{
			for (int i = move.jointMoveList.Count - 1; i > -1; i--)
			{
				if (move.jointMoveList[i].tempGenerated)
				{
					move.jointMoveList.RemoveAt(i);
				}
			}
		}
	}

	// Token: 0x06000A8E RID: 2702 RVA: 0x000323E5 File Offset: 0x000305E5
	public void SetEquipment()
	{
		if (this.moveSet.defaultEquipment != null)
		{
			this.player.SetEquipment(this.moveSet.defaultEquipment, false);
		}
	}

	// Token: 0x06000A8F RID: 2703 RVA: 0x0003240B File Offset: 0x0003060B
	public void ResetMoveSet()
	{
		this.currentStance = null;
		this.stanceLog = new List<Stance>();
	}

	// Token: 0x06000A90 RID: 2704 RVA: 0x00032420 File Offset: 0x00030620
	public void SetMoveSet(MoveSet newMoveSet, bool skipPlayAtStartMoves = false, bool instantlyAnimate = false)
	{
		this.ResetMoveSet();
		if (newMoveSet == null || newMoveSet.stanceList == null)
		{
			return;
		}
		this.moveSet = newMoveSet;
		Stance stance = (from x in this.moveSet.stanceList
		where x.isDefault
		select x).FirstOrDefault<Stance>();
		if (stance == null)
		{
			stance = this.moveSet.stanceList.FirstOrDefault<Stance>();
		}
		foreach (Stance stance2 in this.moveSet.stanceList)
		{
			foreach (Move move in stance2.moveList)
			{
				this.PrepareMoveForUse(move);
			}
		}
		this.SetStance(stance, false, false, stanceChangeType.Default);
		if (!skipPlayAtStartMoves)
		{
			foreach (Move move2 in from x in this.moveList
			where x.inputType == inputType.PlayAtStart
			select x)
			{
				this.PlayMove(move2, false, false, 0f, false);
			}
		}
		if (instantlyAnimate)
		{
			this.InitializePlayerAnimator();
			this.HandleAnimation();
		}
	}

	// Token: 0x06000A91 RID: 2705 RVA: 0x000325A0 File Offset: 0x000307A0
	public void SetBasicMoveSetBindings(DefaultMovesetSettings defaultMovesetSettings)
	{
		if (this.moveSet != null && this.moveSet.defaultMoveset && this.player != null && !this.player.ai)
		{
			foreach (Stance stance in this.moveSet.stanceList)
			{
				foreach (Move move in stance.moveList)
				{
					string unlocalizedName = move.unlocalizedName;
					uint num = <PrivateImplementationDetails>.ComputeStringHash(unlocalizedName);
					if (num <= 1950868109U)
					{
						if (num <= 440624785U)
						{
							if (num != 192858581U)
							{
								if (num == 440624785U)
								{
									if (unlocalizedName == "moveset_action_attack_low")
									{
										move.playerInput = MoveSetHelpers.GetInputActionForBasicMove("Action8", defaultMovesetSettings.invertVerticalAttacks);
									}
								}
							}
							else if (unlocalizedName == "moveset_action_attack_right")
							{
								move.playerInput = MoveSetHelpers.GetInputActionForBasicMove("Action3", defaultMovesetSettings.invertHorizontalAttacks);
							}
						}
						else if (num != 1889567438U)
						{
							if (num == 1950868109U)
							{
								if (unlocalizedName == "moveset_action_attack_high")
								{
									move.playerInput = MoveSetHelpers.GetInputActionForBasicMove("Action5", defaultMovesetSettings.invertVerticalAttacks);
								}
							}
						}
						else if (unlocalizedName == "moveset_action_block_high")
						{
							move.playerInput = MoveSetHelpers.GetInputActionForBasicMove("Action5", defaultMovesetSettings.invertVerticalBlocks);
						}
					}
					else if (num <= 2593096276U)
					{
						if (num != 2064633888U)
						{
							if (num == 2593096276U)
							{
								if (unlocalizedName == "moveset_action_block_low")
								{
									move.playerInput = MoveSetHelpers.GetInputActionForBasicMove("Action8", defaultMovesetSettings.invertVerticalBlocks);
								}
							}
						}
						else if (unlocalizedName == "moveset_action_attack_left")
						{
							move.playerInput = MoveSetHelpers.GetInputActionForBasicMove("Action1", defaultMovesetSettings.invertHorizontalAttacks);
						}
					}
					else if (num != 4100414971U)
					{
						if (num == 4146455492U)
						{
							if (unlocalizedName == "moveset_action_block_right")
							{
								move.playerInput = MoveSetHelpers.GetInputActionForBasicMove("Action3", defaultMovesetSettings.invertHorizontalBlocks);
							}
						}
					}
					else if (unlocalizedName == "moveset_action_block_left")
					{
						move.playerInput = MoveSetHelpers.GetInputActionForBasicMove("Action1", defaultMovesetSettings.invertHorizontalBlocks);
					}
				}
			}
		}
	}

	// Token: 0x06000A92 RID: 2706 RVA: 0x0003288C File Offset: 0x00030A8C
	public void PrepareMoveForUse(Move move)
	{
		if (move.jointMoveList != null)
		{
			move.SortSingleMoves();
			foreach (JointMove singleMove in move.jointMoveList)
			{
				this.GetNextMoveForSingleMove(singleMove, move);
			}
		}
	}

	// Token: 0x06000A93 RID: 2707 RVA: 0x000328F0 File Offset: 0x00030AF0
	public void GetNextMoveForSingleMove(JointMove singleMove, Move move)
	{
		singleMove.lastMoveForJoint = false;
		singleMove.NextMove = null;
		JointMove nextMoveForJointFromList = this.GetNextMoveForJointFromList(singleMove, move.jointMoveList);
		if (nextMoveForJointFromList != null)
		{
			singleMove.NextMove = nextMoveForJointFromList;
			return;
		}
		if (move.inputType == inputType.Continuous)
		{
			JointMove jointMove = (from x in move.jointMoveList
			where x.joint == singleMove.joint
			orderby x.executionTime
			select x).FirstOrDefault<JointMove>();
			if (jointMove != null)
			{
				singleMove.NextMove = jointMove;
			}
		}
		singleMove.lastMoveForJoint = true;
	}

	// Token: 0x06000A94 RID: 2708 RVA: 0x000329AC File Offset: 0x00030BAC
	private void GenerateCurrentMoveList()
	{
		this.moveList.Clear();
		this.PopulateMoveList(this.currentStance, true);
		for (int i = this.stanceLog.Count - 1; i > -1; i--)
		{
			Stance stance = this.stanceLog[i];
			this.PopulateMoveList(stance, false);
		}
	}

	// Token: 0x06000A95 RID: 2709 RVA: 0x00032A00 File Offset: 0x00030C00
	public void PopulateMoveList(Stance stance, bool forceAdd = false)
	{
		for (int i = 0; i < stance.moveList.Count; i++)
		{
			Move move = stance.moveList[i];
			if (forceAdd || !this.MoveForPlayerInputExistsInMoveList(move))
			{
				this.moveList.Add(move);
			}
		}
	}

	// Token: 0x06000A96 RID: 2710 RVA: 0x00032A48 File Offset: 0x00030C48
	private bool MoveForPlayerInputExistsInMoveList(Move move)
	{
		for (int i = 0; i < this.moveList.Count; i++)
		{
			if (this.moveList[i].playerInput == move.playerInput)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000A97 RID: 2711 RVA: 0x00032A8C File Offset: 0x00030C8C
	public void SetStance(Stance stance, bool goBack = false, bool reset = false, stanceChangeType stanceChangeType = stanceChangeType.Default)
	{
		if (reset)
		{
			this.stanceLog.Clear();
		}
		this.skipPassiveExtension = true;
		if (stance == null)
		{
			stance = this.stanceLog.LastOrDefault<Stance>();
			if (stance != null)
			{
				goBack = true;
			}
		}
		if (stance != null)
		{
			if (goBack)
			{
				int num = this.stanceLog.LastIndexOf(stance);
				if (num < 0)
				{
					return;
				}
				this.currentStance = stance;
				this.stanceLog.RemoveRange(num, this.stanceLog.Count<Stance>() - num);
			}
			else
			{
				if (stanceChangeType != stanceChangeType.Replace)
				{
					if (this.stanceLog.Count >= 20)
					{
						Debug.Log("Stance count exceeded");
						return;
					}
					if (this.currentStance != null)
					{
						this.stanceLog.Add(this.currentStance);
					}
				}
				this.currentStance = stance;
			}
		}
		if (this.currentStance != null)
		{
			this.GenerateCurrentMoveList();
		}
		Move passiveMove = this.GetPassiveMove();
		if (passiveMove != null)
		{
			this.ClearPassiveRunningSingleMoves();
			foreach (FighterJoint fighterJoint in this.FighterJoints)
			{
				if (!MoveSetHelpers.MoveForJointExistsInList(passiveMove, fighterJoint, null) && !this.playingMovePreview)
				{
					passiveMove.jointMoveList.Add(new JointMove
					{
						joint = fighterJoint.jointType,
						targetRotation = new NullableVector3(new float?(0f), new float?(0f), new float?(0f))
					});
				}
			}
			if (this.activeAnimationRunning)
			{
				float runningTime = Time.time - this.lastActiveAnimationStarted;
				this.PlayMove(passiveMove, false, false, runningTime, false);
				return;
			}
			this.PlayMove(passiveMove, false, false, 0f, false);
		}
	}

	// Token: 0x06000A98 RID: 2712 RVA: 0x00032C30 File Offset: 0x00030E30
	public Move GetPassiveMove()
	{
		Move move = (from x in this.moveList
		where x.inputType == inputType.Passive
		select x).FirstOrDefault<Move>();
		if (move == null)
		{
			if ((from x in this.RunningSingleMoves
			where x.move.inputType == inputType.Passive
			select x).FirstOrDefault<RunningSingleMove>() == null)
			{
				Stance stance = (from x in this.moveSet.stanceList
				where x.isDefault
				select x).FirstOrDefault<Stance>();
				if (stance != null)
				{
					move = (from x in stance.moveList
					where x.inputType == inputType.Passive
					select x).FirstOrDefault<Move>();
				}
			}
		}
		return move;
	}

	// Token: 0x06000A99 RID: 2713 RVA: 0x00032D0D File Offset: 0x00030F0D
	public void LoadEquipment()
	{
		this.moveSet.defaultEquipment = new List<EquippedEquipment>();
		this.SetEquipment();
	}

	// Token: 0x06000A9A RID: 2714 RVA: 0x00032D25 File Offset: 0x00030F25
	public void LoadDefaultMoveSet()
	{
		this.moveSet = MoveSetHelpers.GetTutorialMoveSet();
		this.SetMoveSet(this.moveSet, false, false);
	}

	// Token: 0x06000A9B RID: 2715 RVA: 0x00032D40 File Offset: 0x00030F40
	private void GenerateFighterJoint(GameObject gameObject)
	{
		foreach (object obj in gameObject.transform)
		{
			Transform transform = (Transform)obj;
			if (!(transform.name == "PlayerModelPhysics") && (!(gameObject.name == "ActualBoxer") || !(transform.name != "Armature")))
			{
				if (!transform.name.Replace("_", "").Any(new Func<char, bool>(char.IsLower)) && transform.gameObject.activeInHierarchy)
				{
					this.GenerateFighterJoints(transform.gameObject);
				}
				this.GenerateFighterJoint(transform.gameObject);
			}
		}
	}

	// Token: 0x06000A9C RID: 2716 RVA: 0x00032E1C File Offset: 0x0003101C
	private void GenerateFighterJoints(GameObject fighterJointObject)
	{
		if (this.FighterJoints == null)
		{
			this.FighterJoints = new List<FighterJoint>();
		}
		FighterJoint item = new FighterJoint
		{
			jointType = (JointType)Enum.Parse(typeof(JointType), fighterJointObject.name, true),
			joint = fighterJointObject
		};
		this.FighterJoints.Add(item);
	}

	// Token: 0x06000A9D RID: 2717 RVA: 0x00032E78 File Offset: 0x00031078
	public void ActivatePlayerAction(PlayerAction playerAction)
	{
		if (this.player != null && !this.player.alive)
		{
			return;
		}
		if (playerAction.type == ActionType.Start)
		{
			Move moveForPlayerAction = this.GetMoveForPlayerAction(playerAction);
			if (moveForPlayerAction != null)
			{
				if (moveForPlayerAction.stanceChange)
				{
					Stance stance = null;
					if (!string.IsNullOrEmpty(moveForPlayerAction.stanceGuid))
					{
						stance = this.GetStanceForMove(moveForPlayerAction);
					}
					if (moveForPlayerAction.inputType == inputType.HoldDown && moveForPlayerAction.stanceChangeType != stanceChangeType.Replace)
					{
						CancellablePlayerAction cancellablePlayerActionFromPool = this.GetCancellablePlayerActionFromPool();
						cancellablePlayerActionFromPool.name = playerAction.name;
						cancellablePlayerActionFromPool.stance = this.currentStance;
						this.cancellablePlayerActions.Add(cancellablePlayerActionFromPool);
					}
					this.SetStance(stance, false, false, moveForPlayerAction.stanceChangeType);
					return;
				}
				if (moveForPlayerAction.inputType == inputType.HoldDown)
				{
					CancellablePlayerAction cancellablePlayerActionFromPool2 = this.GetCancellablePlayerActionFromPool();
					cancellablePlayerActionFromPool2.name = playerAction.name;
					cancellablePlayerActionFromPool2.move = moveForPlayerAction;
					this.cancellablePlayerActions.Add(cancellablePlayerActionFromPool2);
				}
				else if (moveForPlayerAction.inputType == inputType.Continuous)
				{
					CancellablePlayerAction cancellablePlayerActionFromPool3 = this.GetCancellablePlayerActionFromPool();
					cancellablePlayerActionFromPool3.name = playerAction.name;
					cancellablePlayerActionFromPool3.move = moveForPlayerAction;
					this.cancellablePlayerActions.Add(cancellablePlayerActionFromPool3);
				}
				this.PlayMove(moveForPlayerAction, false, false, 0f, false);
				return;
			}
		}
		else
		{
			this.FillCurrentlyCancellingActionsByPlayerAction(playerAction);
			foreach (CancellablePlayerAction cancellablePlayerAction in this.currentlyCancellingActions)
			{
				if (cancellablePlayerAction != null)
				{
					int index = this.cancellablePlayerActions.IndexOf(cancellablePlayerAction);
					if (cancellablePlayerAction.stance != null)
					{
						this.SetStance(cancellablePlayerAction.stance, true, false, stanceChangeType.Default);
					}
					else
					{
						this.CancelMove(cancellablePlayerAction.move.guid);
					}
					this.cancellablePlayerActions.RemoveAt(index);
					this.ReturnCancellablePlayerActionToPool(cancellablePlayerAction);
				}
			}
		}
	}

	// Token: 0x06000A9E RID: 2718 RVA: 0x00033038 File Offset: 0x00031238
	private void CancelPlayerActionsOnStanceBack(int index)
	{
		for (int i = this.cancellablePlayerActions.Count; i > index + 1; i--)
		{
			CancellablePlayerAction cancellablePlayerAction = this.cancellablePlayerActions[i - 1];
			if (cancellablePlayerAction.move != null && !cancellablePlayerAction.move.stanceChange)
			{
				this.CancelMove(cancellablePlayerAction.move.guid);
			}
			this.cancellablePlayerActions.RemoveAt(i - 1);
		}
	}

	// Token: 0x06000A9F RID: 2719 RVA: 0x000330A0 File Offset: 0x000312A0
	public void CancelMove(string moveGuid)
	{
		for (int i = this.RunningSingleMoves.Count - 1; i >= 0; i--)
		{
			RunningSingleMove runningSingleMove = this.RunningSingleMoves[i];
			if (runningSingleMove.move.guid == moveGuid)
			{
				this.RunningSingleMoves.RemoveAt(i);
				this.ReturnRunningSingleMoveToPool(runningSingleMove, false);
			}
		}
	}

	// Token: 0x06000AA0 RID: 2720 RVA: 0x000330FC File Offset: 0x000312FC
	public void ClearPreviewHistory()
	{
		for (int i = 0; i < this.previousPreviewSingleMoves.Count; i++)
		{
			RunningSingleMove runningSingleMove = this.previousPreviewSingleMoves[i];
			runningSingleMove.singleMove.inPreviewList = false;
			this.ReturnRunningSingleMoveToPool(runningSingleMove, true);
		}
		this.previousPreviewSingleMoves.Clear();
		(from x in this.RunningSingleMoves
		where !x.previewOnTheTick
		select x).ToList<RunningSingleMove>().ForEach(delegate(RunningSingleMove x)
		{
			x.previewPercentage = null;
		});
	}

	// Token: 0x06000AA1 RID: 2721 RVA: 0x0003319E File Offset: 0x0003139E
	public void PlayerDied()
	{
		this.ClearRunningSingleMoves();
	}

	// Token: 0x06000AA2 RID: 2722 RVA: 0x000331A8 File Offset: 0x000313A8
	public void ClearRunningSingleMoves()
	{
		for (int i = 0; i < this.RunningSingleMoves.Count; i++)
		{
			RunningSingleMove runningSingleMove = this.RunningSingleMoves[i];
			this.ReturnRunningSingleMoveToPool(runningSingleMove, false);
		}
		this.RunningSingleMoves.Clear();
	}

	// Token: 0x06000AA3 RID: 2723 RVA: 0x000331EC File Offset: 0x000313EC
	private void ClearPassiveRunningSingleMoves()
	{
		for (int i = this.RunningSingleMoves.Count - 1; i > -1; i--)
		{
			RunningSingleMove runningSingleMove = this.RunningSingleMoves[i];
			if (runningSingleMove.move.inputType == inputType.Passive)
			{
				this.RunningSingleMoves.RemoveAt(i);
				this.ReturnRunningSingleMoveToPool(runningSingleMove, false);
			}
		}
	}

	// Token: 0x06000AA4 RID: 2724 RVA: 0x00033240 File Offset: 0x00031440
	public JointMove GetNextMoveForJointFromList(JointMove singleMove, List<JointMove> jointMoveList)
	{
		for (int i = 0; i < jointMoveList.Count; i++)
		{
			JointMove jointMove = jointMoveList[i];
			if (jointMove.joint == singleMove.joint && jointMove.executionTime > singleMove.executionTime)
			{
				return jointMove;
			}
		}
		return null;
	}

	// Token: 0x06000AA5 RID: 2725 RVA: 0x00033288 File Offset: 0x00031488
	private bool ActiveAnimationRunning()
	{
		for (int i = 0; i < this.RunningSingleMoves.Count; i++)
		{
			RunningSingleMove runningSingleMove = this.RunningSingleMoves[i];
			if (runningSingleMove.move.inputType != inputType.Passive && runningSingleMove.move.inputType != inputType.PlayAtStart)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000AA6 RID: 2726 RVA: 0x000332D8 File Offset: 0x000314D8
	private bool PassiveAnimationRunning()
	{
		for (int i = 0; i < this.RunningSingleMoves.Count; i++)
		{
			if (this.RunningSingleMoves[i].move.inputType == inputType.Passive)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000AA7 RID: 2727 RVA: 0x00033318 File Offset: 0x00031518
	private FighterJoint GetFighterJoint(JointType joint)
	{
		for (int i = 0; i < this.FighterJoints.Count; i++)
		{
			FighterJoint fighterJoint = this.FighterJoints[i];
			if (fighterJoint.jointType == joint)
			{
				return fighterJoint;
			}
		}
		return null;
	}

	// Token: 0x06000AA8 RID: 2728 RVA: 0x00033354 File Offset: 0x00031554
	public FighterJoint GetFighterJointPublic(JointType joint)
	{
		this.InitializePlayerAnimator();
		return this.GetFighterJoint(joint);
	}

	// Token: 0x06000AA9 RID: 2729 RVA: 0x00033363 File Offset: 0x00031563
	public List<FighterJoint> GetFighterJointsPublic()
	{
		this.InitializePlayerAnimator();
		return this.FighterJoints;
	}

	// Token: 0x06000AAA RID: 2730 RVA: 0x00033374 File Offset: 0x00031574
	private RunningSingleMove GetPreviousPreviewSingleMove(JointMove singleMove)
	{
		for (int i = 0; i < this.previousPreviewSingleMoves.Count; i++)
		{
			RunningSingleMove runningSingleMove = this.previousPreviewSingleMoves[i];
			if (runningSingleMove.singleMove == singleMove || (singleMove.tempGenerated && runningSingleMove.singleMove.tempGenerated && singleMove.joint == runningSingleMove.singleMove.joint && Generic.DoubleEquals(singleMove.executionTime, runningSingleMove.singleMove.executionTime)))
			{
				return runningSingleMove;
			}
		}
		return null;
	}

	// Token: 0x06000AAB RID: 2731 RVA: 0x000333F0 File Offset: 0x000315F0
	private RunningSingleMove GetNextRunningSingleMove(RunningSingleMove singleMove)
	{
		RunningSingleMove runningSingleMove = null;
		for (int i = 0; i < this.RunningSingleMoves.Count; i++)
		{
			RunningSingleMove runningSingleMove2 = this.RunningSingleMoves[i];
			if (runningSingleMove2.singleMove.joint == singleMove.singleMove.joint && (runningSingleMove2.executeAtTime > singleMove.executeAtTime || (runningSingleMove2.move.inputType == inputType.Passive && singleMove.move.inputType != inputType.Passive)))
			{
				if (runningSingleMove == null)
				{
					runningSingleMove = runningSingleMove2;
				}
				else
				{
					this.CompareMoves(runningSingleMove, runningSingleMove2, singleMove);
					if (this.compareMoveValue > 0)
					{
						runningSingleMove = runningSingleMove2;
					}
				}
			}
		}
		return runningSingleMove;
	}

	// Token: 0x06000AAC RID: 2732 RVA: 0x00033480 File Offset: 0x00031680
	private void CompareMoves(RunningSingleMove compareSingleMove1, RunningSingleMove compareSingleMove2, RunningSingleMove singleMove)
	{
		this.compareMoveValue = 0;
		if (compareSingleMove2.move.layer != compareSingleMove1.move.layer)
		{
			if (compareSingleMove2.move.layer > compareSingleMove1.move.layer)
			{
				this.compareMoveValue = 1;
				return;
			}
		}
		else
		{
			this.compareBool1 = (compareSingleMove1.move.guid == singleMove.move.guid && compareSingleMove1.move.inputType != inputType.Passive);
			this.compareBool2 = (compareSingleMove2.move.guid == singleMove.move.guid && compareSingleMove2.move.inputType != inputType.Passive);
			if (this.compareBool2 && !this.compareBool1)
			{
				this.compareMoveValue = 1;
				return;
			}
			if (this.compareBool2 == this.compareBool1)
			{
				if (compareSingleMove2.executeAtTime < compareSingleMove1.executeAtTime)
				{
					this.compareMoveValue = 1;
					return;
				}
				if (compareSingleMove2.executeAtTime == compareSingleMove1.executeAtTime && compareSingleMove2.move.layer > compareSingleMove1.move.layer)
				{
					this.compareMoveValue = 1;
				}
			}
		}
	}

	// Token: 0x06000AAD RID: 2733 RVA: 0x000335A8 File Offset: 0x000317A8
	private void TempRunningMovesToRunningSingleMoves()
	{
		this.RunningSingleMoves.Clear();
		for (int i = 0; i < this.tempRunningSingleMoves.Count; i++)
		{
			this.RunningSingleMoves.Add(this.tempRunningSingleMoves[i]);
		}
	}

	// Token: 0x06000AAE RID: 2734 RVA: 0x000335F0 File Offset: 0x000317F0
	private void InitRunningSingleMoves()
	{
		this.pool_runningSingleMoves = new List<RunningSingleMove>(1024);
		this.RunningSingleMoves = new List<RunningSingleMove>(1024);
		for (int i = 0; i < 512; i++)
		{
			this.pool_runningSingleMoves.Add(new RunningSingleMove());
		}
	}

	// Token: 0x06000AAF RID: 2735 RVA: 0x00033640 File Offset: 0x00031840
	private RunningSingleMove GetRunningSingleMoveFromPool()
	{
		RunningSingleMove runningSingleMove = null;
		if (this.pool_runningSingleMoves.Count > 0)
		{
			int index = this.pool_runningSingleMoves.Count - 1;
			runningSingleMove = this.pool_runningSingleMoves[index];
			this.pool_runningSingleMoves.RemoveAt(index);
			runningSingleMove.Clear();
		}
		if (runningSingleMove == null)
		{
			runningSingleMove = new RunningSingleMove();
		}
		return runningSingleMove;
	}

	// Token: 0x06000AB0 RID: 2736 RVA: 0x00033694 File Offset: 0x00031894
	private void ReturnRunningSingleMoveToPool(RunningSingleMove runningSingleMove, bool force = false)
	{
		if (this.playingMovePreview && !force && runningSingleMove.singleMove.inPreviewList)
		{
			return;
		}
		this.pool_runningSingleMoves.Add(runningSingleMove);
	}

	// Token: 0x06000AB1 RID: 2737 RVA: 0x000336BC File Offset: 0x000318BC
	public static void exchangeRunningMoves(List<RunningSingleMove> data, int m, int n)
	{
		RunningSingleMove value = data[m];
		data[m] = data[n];
		data[n] = value;
	}

	// Token: 0x06000AB2 RID: 2738 RVA: 0x000336E8 File Offset: 0x000318E8
	public static void RunningSingleMovesQuickSort(List<RunningSingleMove> data, int l, int r)
	{
		int num = l;
		int num2 = r;
		RunningSingleMove runningSingleMove = data[(l + r) / 2];
		for (;;)
		{
			if (PlayerAnimator.CompareRunningSingleMoves(runningSingleMove, data[num]) <= 0)
			{
				while (PlayerAnimator.CompareRunningSingleMoves(data[num2], runningSingleMove) > 0)
				{
					num2--;
				}
				if (num <= num2)
				{
					PlayerAnimator.exchangeRunningMoves(data, num, num2);
					num++;
					num2--;
				}
				if (num > num2)
				{
					break;
				}
			}
			else
			{
				num++;
			}
		}
		if (l < num2)
		{
			PlayerAnimator.RunningSingleMovesQuickSort(data, l, num2);
		}
		if (num < r)
		{
			PlayerAnimator.RunningSingleMovesQuickSort(data, num, r);
		}
	}

	// Token: 0x06000AB3 RID: 2739 RVA: 0x00033764 File Offset: 0x00031964
	private static int CompareRunningSingleMoves(RunningSingleMove x, RunningSingleMove y)
	{
		int num = y.move.layer.CompareTo(x.move.layer);
		if (num != 0)
		{
			return num;
		}
		return y.executeAtTime.CompareTo(x.executeAtTime);
	}

	// Token: 0x06000AB4 RID: 2740 RVA: 0x000337A9 File Offset: 0x000319A9
	public static void RunningSingleMovesQuickSort(List<RunningSingleMove> data)
	{
		if (data.Count > 0)
		{
			PlayerAnimator.RunningSingleMovesQuickSort(data, 0, data.Count - 1);
		}
	}

	// Token: 0x06000AB5 RID: 2741 RVA: 0x000337C4 File Offset: 0x000319C4
	private void SetAnimatedJoint(int layerValue, JointType jointTypeValue)
	{
		RunningJointAnimations runningJointAnimations = null;
		for (int i = 0; i < this.animatedJoints.Count; i++)
		{
			RunningJointAnimations runningJointAnimations2 = this.animatedJoints[i];
			if (runningJointAnimations2.jointType == jointTypeValue)
			{
				runningJointAnimations = runningJointAnimations2;
				break;
			}
		}
		if (runningJointAnimations == null)
		{
			runningJointAnimations = new RunningJointAnimations
			{
				jointType = jointTypeValue,
				layer = layerValue
			};
			this.animatedJoints.Add(runningJointAnimations);
			return;
		}
		if (runningJointAnimations.layer < layerValue)
		{
			runningJointAnimations.layer = layerValue;
		}
	}

	// Token: 0x06000AB6 RID: 2742 RVA: 0x00033838 File Offset: 0x00031A38
	private void ClearAnimatedJoints()
	{
		for (int i = 0; i < this.animatedJoints.Count; i++)
		{
			this.animatedJoints[i].layer = -1;
		}
	}

	// Token: 0x06000AB7 RID: 2743 RVA: 0x00033870 File Offset: 0x00031A70
	private bool CurrentlyHighestPriorityAnimationForJoint(RunningSingleMove runningSingleMove)
	{
		for (int i = 0; i < this.animatedJoints.Count; i++)
		{
			RunningJointAnimations runningJointAnimations = this.animatedJoints[i];
			if (runningJointAnimations.jointType == runningSingleMove.singleMove.joint && runningJointAnimations.layer >= runningSingleMove.move.layer)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000AB8 RID: 2744 RVA: 0x000338CC File Offset: 0x00031ACC
	public Move GetMoveForPlayerAction(PlayerAction playerAction)
	{
		for (int i = 0; i < this.moveList.Count; i++)
		{
			Move move = this.moveList[i];
			if (move.playerInput == playerAction.name && move.inputType != inputType.Passive && move.inputType != inputType.PlayAtStart)
			{
				return move;
			}
		}
		return null;
	}

	// Token: 0x06000AB9 RID: 2745 RVA: 0x00033924 File Offset: 0x00031B24
	private void InitCancellablePlayerActions()
	{
		this.pool_cancellablePlayerActions = new List<CancellablePlayerAction>(64);
		this.cancellablePlayerActions = new List<CancellablePlayerAction>(64);
		this.currentlyCancellingActions = new List<CancellablePlayerAction>(32);
		for (int i = 0; i < 32; i++)
		{
			this.pool_cancellablePlayerActions.Add(new CancellablePlayerAction());
		}
	}

	// Token: 0x06000ABA RID: 2746 RVA: 0x00033978 File Offset: 0x00031B78
	private CancellablePlayerAction GetCancellablePlayerActionFromPool()
	{
		CancellablePlayerAction cancellablePlayerAction = null;
		if (this.pool_cancellablePlayerActions.Count > 0)
		{
			int index = this.pool_cancellablePlayerActions.Count - 1;
			cancellablePlayerAction = this.pool_cancellablePlayerActions[index];
			this.pool_cancellablePlayerActions.RemoveAt(index);
			cancellablePlayerAction.Clear();
		}
		if (cancellablePlayerAction == null)
		{
			cancellablePlayerAction = new CancellablePlayerAction();
		}
		return cancellablePlayerAction;
	}

	// Token: 0x06000ABB RID: 2747 RVA: 0x000339CC File Offset: 0x00031BCC
	private void ReturnCancellablePlayerActionToPool(CancellablePlayerAction runningSingleMove)
	{
		this.pool_cancellablePlayerActions.Add(runningSingleMove);
	}

	// Token: 0x06000ABC RID: 2748 RVA: 0x000339DC File Offset: 0x00031BDC
	private void FillCurrentlyCancellingActionsByPlayerAction(PlayerAction playerAction)
	{
		this.currentlyCancellingActions.Clear();
		for (int i = 0; i < this.cancellablePlayerActions.Count; i++)
		{
			CancellablePlayerAction cancellablePlayerAction = this.cancellablePlayerActions[i];
			if (cancellablePlayerAction.name == playerAction.name)
			{
				this.currentlyCancellingActions.Add(cancellablePlayerAction);
			}
		}
	}

	// Token: 0x06000ABD RID: 2749 RVA: 0x00033A38 File Offset: 0x00031C38
	private Stance GetStanceForMove(Move move)
	{
		for (int i = 0; i < this.moveSet.stanceList.Count; i++)
		{
			Stance stance = this.moveSet.stanceList[i];
			if (stance.guid == move.stanceGuid)
			{
				return stance;
			}
		}
		return null;
	}

	// Token: 0x0400074D RID: 1869
	public List<FighterJoint> FighterJoints;

	// Token: 0x0400074E RID: 1870
	public List<RunningSingleMove> RunningSingleMoves;

	// Token: 0x0400074F RID: 1871
	public List<Move> moveList = new List<Move>(64);

	// Token: 0x04000750 RID: 1872
	[NonSerialized]
	public MoveSet moveSet;

	// Token: 0x04000751 RID: 1873
	public Stance currentStance;

	// Token: 0x04000752 RID: 1874
	public bool animate = true;

	// Token: 0x04000753 RID: 1875
	public bool takeInput = true;

	// Token: 0x04000754 RID: 1876
	public PlayerHealth player;

	// Token: 0x04000755 RID: 1877
	public bool playingMovePreview;

	// Token: 0x04000756 RID: 1878
	public GameObject physicalPlayer;

	// Token: 0x04000757 RID: 1879
	private float spawnTime;

	// Token: 0x04000758 RID: 1880
	public bool animateAtFixedUpdate = true;

	// Token: 0x04000759 RID: 1881
	private bool initialized;

	// Token: 0x0400075A RID: 1882
	private bool activeAnimationRunningPrevious;

	// Token: 0x0400075B RID: 1883
	private bool activeAnimationRunning;

	// Token: 0x0400075C RID: 1884
	private float lastActiveAnimationStarted;

	// Token: 0x0400075D RID: 1885
	private bool skipPassiveExtension;

	// Token: 0x0400075E RID: 1886
	private float timeToUseForPassiveMoves;

	// Token: 0x0400075F RID: 1887
	public List<RunningJointAnimations> animatedJoints = new List<RunningJointAnimations>();

	// Token: 0x04000760 RID: 1888
	public List<RunningSingleMove> tempRunningSingleMoves = new List<RunningSingleMove>(1024);

	// Token: 0x04000761 RID: 1889
	private List<RunningSingleMove> previousPreviewSingleMoves = new List<RunningSingleMove>();

	// Token: 0x04000762 RID: 1890
	private RunningSingleMove tempRunningSingleMoveForProcessing = new RunningSingleMove();

	// Token: 0x04000763 RID: 1891
	private NullableVector3 newTargetRotation = new NullableVector3(null, null, null);

	// Token: 0x04000764 RID: 1892
	private List<JointMove> tempJointMoves = new List<JointMove>(128);

	// Token: 0x04000765 RID: 1893
	public List<Stance> stanceLog = new List<Stance>(32);

	// Token: 0x04000766 RID: 1894
	private List<CancellablePlayerAction> cancellablePlayerActions;

	// Token: 0x04000767 RID: 1895
	private List<CancellablePlayerAction> currentlyCancellingActions;

	// Token: 0x04000768 RID: 1896
	private int compareMoveValue;

	// Token: 0x04000769 RID: 1897
	private bool compareBool1;

	// Token: 0x0400076A RID: 1898
	private bool compareBool2;

	// Token: 0x0400076B RID: 1899
	public List<RunningSingleMove> pool_runningSingleMoves;

	// Token: 0x0400076C RID: 1900
	public List<CancellablePlayerAction> pool_cancellablePlayerActions;
}
