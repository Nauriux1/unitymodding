using System;
using System.Collections.Generic;
using System.Linq;
using MoveClasses;
using UnityEngine;
using UnityEngine.AI;
using Utils;

// Token: 0x0200009E RID: 158
public class PlayerInputAIManager : MonoBehaviour
{
	// Token: 0x170000F9 RID: 249
	// (get) Token: 0x0600056B RID: 1387 RVA: 0x0001970B File Offset: 0x0001790B
	private float weaponMaxDistanceWithMargin
	{
		get
		{
			return this.weaponMaxDistance + this.weaponMaxDistanceMargin;
		}
	}

	// Token: 0x170000FA RID: 250
	// (get) Token: 0x0600056C RID: 1388 RVA: 0x0001971A File Offset: 0x0001791A
	private float weaponMinDistanceWithMargin
	{
		get
		{
			return this.weaponMinDistance + this.weaponMinDistanceMargin;
		}
	}

	// Token: 0x170000FB RID: 251
	// (get) Token: 0x0600056D RID: 1389 RVA: 0x00019729 File Offset: 0x00017929
	public bool hasPriority
	{
		get
		{
			return AiManager.priorityAi == this;
		}
	}

	// Token: 0x0600056E RID: 1390 RVA: 0x0001973C File Offset: 0x0001793C
	private void Awake()
	{
		this.navigationMask = LayerMask.GetMask(new string[]
		{
			"RaycastOnly"
		});
		this.InitFriendlyPosition();
		this.currentPath = new AIPath();
		this.elapsedFromLastPathUpdate = 0f;
		if (IGameSettingsManager.singleton != null)
		{
			this.rollingFeet = IGameSettingsManager.singleton.GetRollingFeet();
		}
		this.elapsedFromLastFriendlyPositionsUpdate = UnityEngine.Random.Range(0f, this.friendlyPositionsUpdateFrequency);
		this.TryToFindPlayerHealth();
	}

	// Token: 0x0600056F RID: 1391 RVA: 0x000197B6 File Offset: 0x000179B6
	private void Start()
	{
		this.GetWeaponLength();
		this.otherPlayers = (from x in UnityEngine.Object.FindObjectsOfType<PlayerHealth>()
		where x != this.playerHealth
		select x).ToList<PlayerHealth>();
	}

	// Token: 0x06000570 RID: 1392 RVA: 0x000197DF File Offset: 0x000179DF
	public virtual void SetParameters(CustomAiObject customAiObject)
	{
		if (customAiObject.useOverrideWalkDistance)
		{
			this.overrideMaxWalkDistance = new float?(customAiObject.overrideMaxWalkDistance);
			this.overrideMinWalkDistance = new float?(customAiObject.overrideMinWalkDistance);
		}
	}

	// Token: 0x06000571 RID: 1393 RVA: 0x0001980C File Offset: 0x00017A0C
	private void GetWeaponLength()
	{
		if (this.playerHealth != null)
		{
			float num = 0f;
			float num2 = 0f;
			foreach (GameObject gameObject in this.playerHealth.currentlyEquippedEquipmentList)
			{
				Weapon component = gameObject.GetComponent<Weapon>();
				if (component != null && component.weaponMaxDistance > num)
				{
					num = component.weaponMaxDistance;
					num2 = component.weaponMinDistance;
				}
			}
			if (num <= 0.1f)
			{
				num = 0.9f;
				num2 = 0.6f;
			}
			this.weaponMaxDistance = num;
			this.weaponMinDistance = num2;
		}
	}

	// Token: 0x06000572 RID: 1394 RVA: 0x000198C4 File Offset: 0x00017AC4
	public virtual float GetMaxWalkDistance()
	{
		if (this.overrideMaxWalkDistance != null)
		{
			return this.overrideMaxWalkDistance.Value;
		}
		return this.weaponMaxDistanceWithMargin - this.walkMargin;
	}

	// Token: 0x06000573 RID: 1395 RVA: 0x000198EC File Offset: 0x00017AEC
	public virtual float GetMinWalkDistance()
	{
		if (this.overrideMinWalkDistance != null)
		{
			return this.overrideMinWalkDistance.Value;
		}
		float num = this.GetMaxWalkDistance() - 0.4f;
		if (num > this.weaponMinDistanceWithMargin)
		{
			return num;
		}
		return this.weaponMinDistanceWithMargin;
	}

	// Token: 0x06000574 RID: 1396 RVA: 0x00019930 File Offset: 0x00017B30
	private void TryToFindPlayerHealth()
	{
		if (this.playerHealth == null)
		{
			this.playerCharacter = base.gameObject;
			this.playerHealth = base.gameObject.GetComponent<PlayerHealth>();
			this.playerAnimator = Generic.FindComponentsInChildObjects<PlayerAnimator>(this.playerCharacter).FirstOrDefault<PlayerAnimator>();
			this.rotatePlayer = Generic.FindComponentsInChildObjects<RotatePlayer>(this.playerCharacter).FirstOrDefault<RotatePlayer>();
			this.ballMovements = Generic.FindComponentsInChildObjects<BallMovement>(this.playerCharacter);
			this.GetProtectionLineSegment();
			if (this.playerHealth.navigationObstacle != null)
			{
				this.playerHealth.navigationObstacle.SetActive(true);
			}
			this.FetchAnimations();
		}
	}

	// Token: 0x06000575 RID: 1397 RVA: 0x000199D8 File Offset: 0x00017BD8
	private void GetProtectionLineSegment()
	{
		if (this.playerHealth != null)
		{
			this.selfProtectionTransform1 = this.playerHealth.cameraPositionPoint.transform;
			FighterJoint fighterJointPublic = this.playerHealth.playerAnimator.GetFighterJointPublic(JointType.NECK);
			if (fighterJointPublic != null)
			{
				this.selfProtectionTransform2 = fighterJointPublic.physicsJoint.transform;
			}
		}
	}

	// Token: 0x06000576 RID: 1398 RVA: 0x00019A30 File Offset: 0x00017C30
	public void ConnectToPlayerCharacter(GameObject newPlayerCharacter)
	{
		this.playerCharacter = newPlayerCharacter;
		this.ballMovements = Generic.FindComponentsInChildObjects<BallMovement>(this.playerCharacter);
		this.rotatePlayer = Generic.FindComponentsInChildObjects<RotatePlayer>(this.playerCharacter).FirstOrDefault<RotatePlayer>();
		this.playerAnimator = Generic.FindComponentsInChildObjects<PlayerAnimator>(this.playerCharacter).FirstOrDefault<PlayerAnimator>();
		this.playerHealth = newPlayerCharacter.GetComponent<PlayerHealth>();
		this.FetchAnimations();
	}

	// Token: 0x06000577 RID: 1399 RVA: 0x00019A94 File Offset: 0x00017C94
	public void Update()
	{
		if (this.playerHealth == null || !this.playerHealth.alive)
		{
			return;
		}
		this.UpdateNavigationObstacleLocation();
		this.GetEnemyTarget();
		this.GetFriendlyPositions();
		this.UpdatePath();
		this.DoPlayerMovement();
		this.DoBlock();
		this.DoAttack();
		this.CheckForFightEnd();
		this.oldForwardMoveValue = this.forwardMoveValue;
		this.oldSideMoveValue = this.sideMoveValue;
		this.oldTurnValue = this.turnValue;
	}

	// Token: 0x06000578 RID: 1400 RVA: 0x00019B14 File Offset: 0x00017D14
	private void UpdatePath()
	{
		this.elapsedFromLastPathUpdate += Time.deltaTime;
		if (this.moveTarget == null)
		{
			return;
		}
		if (this.elapsedFromLastPathUpdate > this.pathUpdateFrequency)
		{
			this.elapsedFromLastPathUpdate -= this.pathUpdateFrequency;
			AIPath aipath = new AIPath
			{
				pathEndPart = new NavMeshPath()
			};
			NavMesh.CalculatePath(this.playerHealth.cameraPositionPoint.transform.position, this.moveTarget.position, -1, aipath.pathEndPart);
			aipath.fullPath = aipath.pathEndPart.corners;
			aipath.objectAvoidanceResult = this.CheckObjectAvoidance(aipath.pathEndPart.corners);
			if (aipath.objectAvoidanceResult != null)
			{
				AIPath aipath2 = new AIPath
				{
					pathEndPart = new NavMeshPath()
				};
				AIPath aipath3 = new AIPath
				{
					pathEndPart = new NavMeshPath()
				};
				Vector3 vector = aipath.objectAvoidanceResult.hitColliderPosition + Quaternion.Euler(0f, 90f, 0f) * aipath.objectAvoidanceResult.hitDirection * 1.2f;
				Vector3 vector2 = aipath.objectAvoidanceResult.hitColliderPosition + Quaternion.Euler(0f, -90f, 0f) * aipath.objectAvoidanceResult.hitDirection * 1.2f;
				NavMesh.CalculatePath(vector, this.moveTarget.position, -1, aipath2.pathEndPart);
				NavMesh.CalculatePath(vector2, this.moveTarget.position, -1, aipath3.pathEndPart);
				if (aipath2.validPath)
				{
					aipath2.pathStartPart = new NavMeshPath();
					NavMesh.CalculatePath(this.playerHealth.cameraPositionPoint.transform.position, vector, -1, aipath2.pathStartPart);
				}
				if (aipath3.validPath)
				{
					aipath3.pathStartPart = new NavMeshPath();
					NavMesh.CalculatePath(this.playerHealth.cameraPositionPoint.transform.position, vector2, -1, aipath3.pathStartPart);
				}
				if (aipath2.validPath)
				{
					aipath2.BuildFullPath();
					aipath2.objectAvoidanceResult = this.CheckObjectAvoidance(aipath2.fullPath);
				}
				if (aipath3.validPath)
				{
					aipath3.BuildFullPath();
					aipath3.objectAvoidanceResult = this.CheckObjectAvoidance(aipath3.fullPath);
				}
				AIPath aipath4 = null;
				if (aipath2.validPath && aipath3.validPath)
				{
					if (aipath2.pathLength < aipath3.pathLength)
					{
						aipath4 = aipath2;
					}
					else
					{
						aipath4 = aipath3;
					}
				}
				else if (aipath2.validPath)
				{
					aipath4 = aipath2;
				}
				else if (aipath3.validPath)
				{
					aipath4 = aipath3;
				}
				if (aipath4 != null)
				{
					this.DrawPath(aipath4.fullPath);
				}
				this.currentPath = aipath4;
			}
			else
			{
				this.currentPath = aipath;
			}
			this.currentPathTarget = 1;
		}
		Vector3 position = this.playerHealth.cameraPositionPoint.transform.position;
		Vector3 vector3 = this.moveTarget.position;
		if (this.currentPath != null && this.currentPath.fullPath != null && this.currentPath.fullPath.Length > this.currentPathTarget)
		{
			for (int i = this.currentPathTarget; i < this.currentPath.fullPath.Length; i++)
			{
				vector3 = this.currentPath.fullPath[i];
				position = new Vector3(position.x, vector3.y, position.z);
				if (Vector3.Distance(position, vector3) > this.corneringDistance)
				{
					this.currentPathTarget = i;
					return;
				}
			}
		}
	}

	// Token: 0x06000579 RID: 1401 RVA: 0x00019E8C File Offset: 0x0001808C
	private void DoPlayerMovement()
	{
		if (this.GetObstacleAvoidanceMovement())
		{
			if (this.CheckIfCanWalkInDirection(new Vector3(this.forcedMovement.y, 0f, this.forcedMovement.x)))
			{
				this.forwardMoveValue = this.forcedMovement.x;
				this.sideMoveValue = this.forcedMovement.y;
			}
			else
			{
				this.forwardMoveValue = 0f;
				this.sideMoveValue = 0f;
			}
		}
		else
		{
			this.sideMoveValue = 0f;
			this.forwardMoveValue = 0f;
			if (this.moveTarget != null)
			{
				float num = Vector3.Distance(this.playerHealth.cameraPositionPoint.transform.position, this.moveTarget.position);
				if (num > this.GetMaxWalkDistance() && this.CurrentPathToTargetIsValid())
				{
					this.forwardMoveValue = 1f;
				}
				else if (num < this.GetMinWalkDistance() && this.CheckIfCanWalkInDirection(new Vector3(0f, 0f, -1f)))
				{
					this.forwardMoveValue = -1f;
				}
			}
		}
		this.turnValue = this.GetTurnValue();
		if (this.oldTurnValue != this.turnValue)
		{
			this.setTurn(this.turnValue);
		}
		if (this.forwardMoveValue != this.oldForwardMoveValue)
		{
			this.setForwardMove(this.forwardMoveValue);
		}
		if (this.sideMoveValue != this.oldSideMoveValue)
		{
			this.setSideMove(this.sideMoveValue);
		}
	}

	// Token: 0x0600057A RID: 1402 RVA: 0x0001A000 File Offset: 0x00018200
	public bool CurrentPathToTargetIsValid()
	{
		RaycastHit raycastHit;
		return (this.currentPath != null && this.currentPath.validPath) || (this.moveTarget != null && !Physics.Linecast(this.playerHealth.cameraPositionPoint.transform.position, this.moveTarget.position, out raycastHit, this.navigationMask));
	}

	// Token: 0x0600057B RID: 1403 RVA: 0x0001A06C File Offset: 0x0001826C
	private bool CheckIfCanWalkInDirection(Vector3 localDirection)
	{
		Vector3 b = this.playerHealth.cameraPoint.transform.TransformDirection(localDirection);
		Vector3 sourcePosition = this.playerHealth.cameraPositionPoint.transform.position + b;
		sourcePosition.y = 0.05f;
		return NavMesh.SamplePosition(sourcePosition, out this.testHit, 0.1f, 1);
	}

	// Token: 0x0600057C RID: 1404 RVA: 0x0001A0D0 File Offset: 0x000182D0
	public virtual void DoBlock()
	{
		if (this.attacking)
		{
			return;
		}
		EnemyBlade enemyBlade = this.CalculateClosestWeaponPoint();
		bool flag = false;
		if (enemyBlade != null)
		{
			this.blockDistance = 1.5f;
			if (enemyBlade.GetWeaponMaxDistance() + this.weaponMaxDistanceMargin > this.blockDistance)
			{
				this.blockDistance = enemyBlade.GetWeaponMaxDistance() + this.weaponMaxDistanceMargin;
			}
			if (enemyBlade.GetWeaponMaxDistance() >= this.weaponMaxDistance)
			{
				this.blockDistance += 0.5f;
			}
			if (this.isBlocking && enemyBlade.currentBladeDistanceItem.distanceBetweenPoints < this.shortestBlockDistance)
			{
				this.shortestBlockDistance = enemyBlade.currentBladeDistanceItem.distanceBetweenPoints;
			}
			if (!this.CanCounterAttack(enemyBlade) && !this.CanAttackFirst(enemyBlade))
			{
				AiBlockDirection blockDirection = this.GetBlockDirection(enemyBlade);
				if (blockDirection != AiBlockDirection.None)
				{
					string text = "";
					if (blockDirection == AiBlockDirection.High)
					{
						text = this.BlockHigh;
					}
					else if (blockDirection == AiBlockDirection.Left)
					{
						text = this.BlockLeft;
					}
					else if (blockDirection == AiBlockDirection.Right)
					{
						text = this.BlockRight;
					}
					else if (blockDirection == AiBlockDirection.Center)
					{
						text = this.BlockCenter;
						if (string.IsNullOrEmpty(text))
						{
							if (this.isBlocking)
							{
								if (this.centerBlockStarted + 0.3f < Time.time)
								{
									text = this.BlockRight;
									if (this.performedBlock == text)
									{
										text = this.BlockLeft;
									}
									this.centerBlockStarted = Time.time;
								}
								else
								{
									text = this.performedBlock;
								}
							}
							else
							{
								if (this.centerBlockSide == AiBlockDirection.Left)
								{
									text = this.BlockLeft;
								}
								else
								{
									text = this.BlockRight;
								}
								this.centerBlockStarted = Time.time;
							}
							if (this.CanAttack(true))
							{
								text = null;
							}
						}
					}
					else if (blockDirection == AiBlockDirection.Low)
					{
						if (this.CanAttack(true))
						{
							text = null;
						}
						else
						{
							text = this.BlockLow;
						}
					}
					if (this.isBlocking && text == this.performedBlock && this.blockStarted + this.blockMaxDuration < Time.time && this.CanAttack(false))
					{
						text = null;
					}
					if (!string.IsNullOrEmpty(text))
					{
						flag = true;
					}
					if (!string.IsNullOrEmpty(text) && text != this.performedBlock)
					{
						if (this.isBlocking)
						{
							this.StopBlock();
						}
						this.blockStarted = Time.time;
						this.shortestBlockDistance = enemyBlade.currentBladeDistanceItem.distanceBetweenPoints;
						this.Action_performed(this.BlockStance, ActionType.Start, 1f);
						this.Action_performed(text, ActionType.Start, 1f);
						this.performedBlock = text;
					}
				}
			}
		}
		if (this.isBlocking && !flag)
		{
			this.StopBlock();
		}
		this.isBlocking = flag;
	}

	// Token: 0x0600057D RID: 1405 RVA: 0x0001A33C File Offset: 0x0001853C
	private void StopBlock()
	{
		if (!string.IsNullOrEmpty(this.performedBlock))
		{
			this.isBlocking = false;
			this.Action_performed(this.performedBlock, ActionType.End, 0f);
			this.Action_performed(this.BlockStance, ActionType.End, 0f);
			this.performedBlock = "";
		}
	}

	// Token: 0x0600057E RID: 1406 RVA: 0x0001A38C File Offset: 0x0001858C
	public virtual void DoAttack()
	{
		if (this.attackAnimations.Count == 0)
		{
			return;
		}
		if (this.targetEnemy != null && !this.isBlocking && this.CanAttack(false) && !this.attacking && string.IsNullOrEmpty(this.performedAttack))
		{
			if ((double)this.GetIsLookingAtEnemyDotProduct() < 0.96)
			{
				return;
			}
			if (this.friendInFront)
			{
				return;
			}
			List<AiAnimation> usableAttackList = this.GetUsableAttackList();
			if (usableAttackList.Count > 0)
			{
				int index = UnityEngine.Random.Range(0, usableAttackList.Count);
				AiAnimation attackToPerform = usableAttackList[index];
				this.PerformAttack(attackToPerform);
			}
		}
		if (this.attacking && this.nextAttackAllowedTime < Time.time)
		{
			this.StopAttack();
		}
	}

	// Token: 0x0600057F RID: 1407 RVA: 0x0001A43E File Offset: 0x0001863E
	public void PerformAttack(AiAnimation attackToPerform)
	{
		if (attackToPerform != null)
		{
			this.attacking = true;
			this.nextAttackAllowedTime = Time.time + attackToPerform.duration;
			this.performedAttack = attackToPerform.actionName;
			this.Action_performed(this.performedAttack, ActionType.Start, 1f);
		}
	}

	// Token: 0x06000580 RID: 1408 RVA: 0x0001A47A File Offset: 0x0001867A
	public void StopAttack()
	{
		if (this.attacking)
		{
			this.attacking = false;
			this.Action_performed(this.performedAttack, ActionType.End, 0f);
			this.performedAttack = null;
		}
	}

	// Token: 0x06000581 RID: 1409 RVA: 0x0001A4A4 File Offset: 0x000186A4
	public float? GetDistanceToEnemy()
	{
		if (this.targetEnemy != null && this.lastDistanceToEnemyCalculated != Time.frameCount)
		{
			this.distanceToEnemy = Vector3.Distance(this.playerHealth.cameraPositionPoint.transform.position, this.targetEnemy.cameraPositionPoint.transform.position);
			this.lastDistanceToEnemyCalculated = Time.frameCount;
		}
		if (this.lastDistanceToEnemyCalculated == Time.frameCount)
		{
			return new float?(this.distanceToEnemy);
		}
		return null;
	}

	// Token: 0x06000582 RID: 1410 RVA: 0x0001A530 File Offset: 0x00018730
	private float GetIsLookingAtEnemyDotProduct()
	{
		if (this.moveTarget != null && this.lastLookingAtEnemyDotProduct != Time.frameCount)
		{
			Vector3 position = this.playerHealth.cameraPositionPoint.transform.position;
			Vector3 position2 = this.moveTarget.position;
			Vector3 b = new Vector3(position.x, 0f, position.z);
			Vector3 normalized = (new Vector3(position2.x, 0f, position2.z) - b).normalized;
			this.lookingAtEnemyDotProduct = Vector3.Dot(normalized, this.playerHealth.cameraPoint.transform.forward);
			this.lastLookingAtEnemyDotProduct = Time.frameCount;
		}
		return this.lookingAtEnemyDotProduct;
	}

	// Token: 0x06000583 RID: 1411 RVA: 0x0001A5F0 File Offset: 0x000187F0
	public bool EnemyInRange(bool checkMinimumDistance = false)
	{
		float? num = this.GetDistanceToEnemy();
		if (num != null)
		{
			float? num2 = num;
			float num3 = this.weaponMaxDistanceWithMargin;
			if (num2.GetValueOrDefault() < num3 & num2 != null)
			{
				if (checkMinimumDistance)
				{
					num2 = num;
					num3 = this.weaponMinDistanceWithMargin;
					if (!(num2.GetValueOrDefault() > num3 & num2 != null))
					{
						return false;
					}
				}
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000584 RID: 1412 RVA: 0x0001A64D File Offset: 0x0001884D
	public bool CanAttack(bool checkMinimumDistance = false)
	{
		return this.nextAttackAllowedTime + this.attackDelay < Time.time && this.EnemyInRange(checkMinimumDistance);
	}

	// Token: 0x06000585 RID: 1413 RVA: 0x0001A66F File Offset: 0x0001886F
	public bool CanCounterAttack(EnemyBlade enemyBlade)
	{
		return this.isBlocking && (double)this.shortestBlockDistance + 0.05 < (double)enemyBlade.currentBladeDistanceItem.distanceBetweenPoints && this.CanAttack(false);
	}

	// Token: 0x06000586 RID: 1414 RVA: 0x0001A6A4 File Offset: 0x000188A4
	public bool CanAttackFirst(EnemyBlade enemyBlade)
	{
		float? num = this.GetDistanceToEnemy();
		if (num != null && this.CanAttack(true) && this.weaponMaxDistance >= enemyBlade.GetWeaponMaxDistance())
		{
			float? num2 = num;
			float num3 = enemyBlade.GetWeaponMaxDistance() + this.weaponMaxDistanceMargin;
			if (num2.GetValueOrDefault() > num3 & num2 != null)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000587 RID: 1415 RVA: 0x0001A700 File Offset: 0x00018900
	private List<AiAnimation> GetUsableAttackList()
	{
		this.usableAnimations.Clear();
		float? num = this.GetDistanceToEnemy();
		if (num != null)
		{
			bool flag = (double)this.GetIsLookingAtEnemyDotProduct() > 0.998;
			float? num2 = num;
			float num3 = this.weaponMaxDistanceWithMargin - 0.2f;
			bool flag2 = num2.GetValueOrDefault() > num3 & num2 != null;
			num2 = num;
			num3 = this.weaponMinDistanceWithMargin + 0.2f;
			bool flag3 = num2.GetValueOrDefault() < num3 & num2 != null;
			for (int i = 0; i < this.attackAnimations.Count; i++)
			{
				AiAnimation aiAnimation = this.attackAnimations[i];
				if ((!this.friendOnLeft || aiAnimation.attackDirection != AttackAnimationDirection.Left) && (!this.friendOnRight || aiAnimation.attackDirection != AttackAnimationDirection.Right) && (!flag2 || aiAnimation.attackDirection != AttackAnimationDirection.Default) && (flag3 || aiAnimation.attackDirection != AttackAnimationDirection.Low) && (flag || (aiAnimation.attackDirection != AttackAnimationDirection.High && aiAnimation.attackDirection != AttackAnimationDirection.Stab)))
				{
					this.usableAnimations.Add(aiAnimation);
				}
			}
		}
		return this.usableAnimations;
	}

	// Token: 0x06000588 RID: 1416 RVA: 0x0001A81C File Offset: 0x00018A1C
	public AiAnimation GetAiAnimation(string action)
	{
		AiAnimation result = null;
		for (int i = 0; i < this.attackAnimations.Count; i++)
		{
			if (this.attackAnimations[i].actionName == action)
			{
				return this.attackAnimations[i];
			}
		}
		return result;
	}

	// Token: 0x06000589 RID: 1417 RVA: 0x0001A868 File Offset: 0x00018A68
	private float GetTurnValue()
	{
		float result = 0f;
		if (this.moveTarget != null && this.CurrentPathToTargetIsValid())
		{
			Vector3 position = this.playerHealth.cameraPositionPoint.transform.position;
			Vector3 vector = this.moveTarget.position;
			if (this.forwardMoveValue > 0.1f && this.currentPath != null && this.currentPath.fullPath != null && this.currentPath.fullPath.Length > this.currentPathTarget)
			{
				vector = this.currentPath.fullPath[this.currentPathTarget];
			}
			Vector3 b = new Vector3(position.x, 0f, position.z);
			if ((double)Vector3.Dot((new Vector3(vector.x, 0f, vector.z) - b).normalized, this.playerHealth.cameraPoint.transform.forward) > 0.998)
			{
				return result;
			}
			Vector3 vector2 = vector - this.playerHealth.cameraPositionPoint.transform.position;
			result = Generic.AngleDir(this.playerHealth.cameraPoint.transform.forward, vector2.normalized, this.playerHealth.cameraPoint.transform.up);
		}
		return result;
	}

	// Token: 0x0600058A RID: 1418 RVA: 0x0001A9C4 File Offset: 0x00018BC4
	private void GetEnemyTarget()
	{
		if (this.targetEnemy != null && !this.targetEnemy.alive)
		{
			this.targetEnemy = null;
			this.moveTarget = null;
		}
		this.elapsedTimeFromLastEnemyUpdate += Time.deltaTime;
		if (this.targetEnemy == null || this.elapsedTimeFromLastEnemyUpdate > this.enemyTargetUpdateFrequency)
		{
			this.elapsedTimeFromLastEnemyUpdate -= this.enemyTargetUpdateFrequency;
			if (this.elapsedTimeFromLastEnemyUpdate < 0f)
			{
				this.elapsedTimeFromLastEnemyUpdate = 0f;
			}
			this.previousEnemy = this.targetEnemy;
			this.targetEnemy = null;
			if (this.testTarget != null)
			{
				this.moveTarget = this.testTarget.transform;
				this.targetEnemy = this.testTarget.GetComponent<PlayerHealth>();
			}
			else
			{
				float num = 99999f;
				foreach (PlayerHealth playerHealth in from x in this.otherPlayers
				where !x.ai
				select x)
				{
					if (!(playerHealth == this.playerHealth) && playerHealth.alive)
					{
						float num2 = Vector3.Distance(this.playerHealth.cameraPositionPoint.transform.position, playerHealth.cameraPositionPoint.transform.position);
						if (num2 < num)
						{
							num = num2;
							this.targetEnemy = playerHealth;
							this.moveTarget = playerHealth.cameraPositionPoint.transform;
						}
					}
				}
			}
			this.GetEnemyBlades();
		}
	}

	// Token: 0x0600058B RID: 1419 RVA: 0x0001AB68 File Offset: 0x00018D68
	private void CheckForFightEnd()
	{
		if (this.targetEnemy == null)
		{
			this.StopBlock();
		}
	}

	// Token: 0x0600058C RID: 1420 RVA: 0x0001AB80 File Offset: 0x00018D80
	private void GetEnemyBlades()
	{
		if (this.previousEnemy != this.targetEnemy || this.targetEnemy == null)
		{
			this.enemyBlades.Clear();
			if (this.targetEnemy != null)
			{
				Weapon weapon = null;
				if (this.targetEnemy.leftHand.currentlyGrabbedItem != null)
				{
					weapon = this.targetEnemy.leftHand.currentlyGrabbedItem.GetWeapon();
					if (weapon != null)
					{
						this.enemyBlades.Add(new EnemyBlade
						{
							weapon = weapon,
							weaponSections = weapon.GetWeaponSections()
						});
					}
				}
				if (this.targetEnemy.rightHand.currentlyGrabbedItem != null)
				{
					Weapon weapon2 = this.targetEnemy.rightHand.currentlyGrabbedItem.GetWeapon();
					if (weapon2 != null && weapon2 != weapon)
					{
						this.enemyBlades.Add(new EnemyBlade
						{
							weapon = weapon2,
							weaponSections = weapon2.GetWeaponSections()
						});
					}
				}
				if (this.enemyBlades.Count == 0)
				{
					BluntDamageDealerGameObject component = this.targetEnemy.leftHand.Rigidbody.gameObject.GetComponent<BluntDamageDealerGameObject>();
					this.enemyBlades.Add(new EnemyBlade
					{
						weaponSections = component.GetWeaponSections(),
						bluntDamageDealerGameObject = component
					});
					BluntDamageDealerGameObject component2 = this.targetEnemy.rightHand.Rigidbody.gameObject.GetComponent<BluntDamageDealerGameObject>();
					this.enemyBlades.Add(new EnemyBlade
					{
						weaponSections = component2.GetWeaponSections(),
						bluntDamageDealerGameObject = component2
					});
				}
			}
		}
	}

	// Token: 0x0600058D RID: 1421 RVA: 0x0001AD0C File Offset: 0x00018F0C
	private void setForwardMove(float value)
	{
		if (this.rollingFeet)
		{
			if (this.ballMovements == null)
			{
				return;
			}
			using (List<BallMovement>.Enumerator enumerator = this.ballMovements.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BallMovement ballMovement = enumerator.Current;
					ballMovement.SetVerticalSpeed(value);
				}
				return;
			}
		}
		if (Math.Abs(this.oldForwardMoveValue) > 0.1f)
		{
			this.Action_performed((this.oldForwardMoveValue > 0f) ? "Move_Forward" : "Move_Back", ActionType.End, value);
		}
		if (Math.Abs(value) > 0.1f)
		{
			if (value > 0f)
			{
				this.Action_performed("Move_Forward", ActionType.Start, value);
				return;
			}
			if (value < 0f)
			{
				this.Action_performed("Move_Back", ActionType.Start, value);
			}
		}
	}

	// Token: 0x0600058E RID: 1422 RVA: 0x0001ADDC File Offset: 0x00018FDC
	private void setSideMove(float value)
	{
		if (Math.Abs(this.oldSideMoveValue) > 0.1f)
		{
			this.Action_performed((this.oldSideMoveValue > 0f) ? "Move_Right" : "Move_Left", ActionType.End, value);
		}
		if (Math.Abs(value) > 0.1f)
		{
			if (value > 0f)
			{
				this.Action_performed("Move_Right", ActionType.Start, value);
				return;
			}
			if (value < 0f)
			{
				this.Action_performed("Move_Left", ActionType.Start, value);
			}
		}
	}

	// Token: 0x0600058F RID: 1423 RVA: 0x0001AE54 File Offset: 0x00019054
	private void setTurn(float value)
	{
		if (this.rotatePlayer != null)
		{
			this.rotatePlayer.SetRotationInput(value);
		}
	}

	// Token: 0x06000590 RID: 1424 RVA: 0x0001AE70 File Offset: 0x00019070
	private void Action_performed(string name, ActionType actionType, float value = 0f)
	{
		if (this.playerAnimator != null)
		{
			this.playerAnimator.ActivatePlayerAction(new PlayerAction
			{
				name = name,
				type = actionType,
				value = value
			});
		}
	}

	// Token: 0x06000591 RID: 1425 RVA: 0x0001AEA8 File Offset: 0x000190A8
	private void AttemptToFillWordLists()
	{
		if (PlayerInputAIManager.attackWords.Count == 0)
		{
			PlayerInputAIManager.attackWords = new List<string>();
			PlayerInputAIManager.blockWords = new List<string>();
			PlayerInputAIManager.directionHighWords = new List<string>();
			PlayerInputAIManager.directionLowWords = new List<string>();
			PlayerInputAIManager.directionLeftWords = new List<string>();
			PlayerInputAIManager.directionRightWords = new List<string>();
			PlayerInputAIManager.directionCenterWords = new List<string>();
			PlayerInputAIManager.attackWords.AddRange(LocalizationHelpers.LocalizedTextForAllLanguages("ai_trigger_word_attack", Array.Empty<object>()));
			PlayerInputAIManager.attackWords.AddRange(LocalizationHelpers.LocalizedTextForAllLanguages("ai_trigger_word_stab", Array.Empty<object>()));
			PlayerInputAIManager.attackWords.AddRange(LocalizationHelpers.LocalizedTextForAllLanguages("ai_trigger_word_strike", Array.Empty<object>()));
			PlayerInputAIManager.attackWords.AddRange(LocalizationHelpers.LocalizedTextForAllLanguages("ai_trigger_word_swing", Array.Empty<object>()));
			PlayerInputAIManager.attackWords.AddRange(LocalizationHelpers.LocalizedTextForAllLanguages("ai_trigger_word_slash", Array.Empty<object>()));
			PlayerInputAIManager.attackWords.AddRange(LocalizationHelpers.LocalizedTextForAllLanguages("moveset_action_attack_special", new object[]
			{
				""
			}));
			PlayerInputAIManager.attackWords.AddRange(LocalizationHelpers.LocalizedTextForAllLanguages("ai_trigger_word_punch", Array.Empty<object>()));
			PlayerInputAIManager.attackWords.AddRange(LocalizationHelpers.LocalizedTextForAllLanguages("ai_trigger_word_slam", Array.Empty<object>()));
			PlayerInputAIManager.attackWords.AddRange(LocalizationHelpers.LocalizedTextForAllLanguages("ai_trigger_word_kick", Array.Empty<object>()));
			PlayerInputAIManager.attackWords.AddRange(LocalizationHelpers.LocalizedTextForAllLanguages("ai_trigger_word_straight", Array.Empty<object>()));
			PlayerInputAIManager.attackWords.AddRange(LocalizationHelpers.LocalizedTextForAllLanguages("ai_trigger_word_hook", Array.Empty<object>()));
			PlayerInputAIManager.blockWords.AddRange(LocalizationHelpers.LocalizedTextForAllLanguages("ai_trigger_word_block", Array.Empty<object>()));
			PlayerInputAIManager.blockWords.AddRange(LocalizationHelpers.LocalizedTextForAllLanguages("moveset_stance_block", Array.Empty<object>()));
			PlayerInputAIManager.directionLeftWords.AddRange(LocalizationHelpers.LocalizedTextForAllLanguages("ai_trigger_word_direction_left", Array.Empty<object>()));
			PlayerInputAIManager.directionRightWords.AddRange(LocalizationHelpers.LocalizedTextForAllLanguages("ai_trigger_word_direction_right", Array.Empty<object>()));
			PlayerInputAIManager.directionLowWords.AddRange(LocalizationHelpers.LocalizedTextForAllLanguages("ai_trigger_word_direction_low", Array.Empty<object>()));
			PlayerInputAIManager.directionHighWords.AddRange(LocalizationHelpers.LocalizedTextForAllLanguages("ai_trigger_word_direction_high", Array.Empty<object>()));
			PlayerInputAIManager.directionCenterWords.AddRange(LocalizationHelpers.LocalizedTextForAllLanguages("ai_trigger_word_direction_center", Array.Empty<object>()));
			PlayerInputAIManager.directionCenterWords.AddRange(LocalizationHelpers.LocalizedTextForAllLanguages("ai_trigger_word_stab", Array.Empty<object>()));
			PlayerInputAIManager.attackWords = PlayerInputAIManager.attackWords.ConvertAll<string>((string x) => x.ToLower());
			PlayerInputAIManager.blockWords = PlayerInputAIManager.blockWords.ConvertAll<string>((string x) => x.ToLower());
			PlayerInputAIManager.directionHighWords = PlayerInputAIManager.directionHighWords.ConvertAll<string>((string x) => x.ToLower());
			PlayerInputAIManager.directionLowWords = PlayerInputAIManager.directionLowWords.ConvertAll<string>((string x) => x.ToLower());
			PlayerInputAIManager.directionLeftWords = PlayerInputAIManager.directionLeftWords.ConvertAll<string>((string x) => x.ToLower());
			PlayerInputAIManager.directionRightWords = PlayerInputAIManager.directionRightWords.ConvertAll<string>((string x) => x.ToLower());
			PlayerInputAIManager.directionCenterWords = PlayerInputAIManager.directionCenterWords.ConvertAll<string>((string x) => x.ToLower());
		}
	}

	// Token: 0x06000592 RID: 1426 RVA: 0x0001B230 File Offset: 0x00019430
	private void FetchAnimations()
	{
		this.attackAnimations = new List<AiAnimation>();
		this.AttemptToFillWordLists();
		if (this.playerAnimator != null && this.playerAnimator.moveSet != null)
		{
			if (this.playerAnimator.moveSet == null || this.playerAnimator.moveSet.stanceList == null)
			{
				return;
			}
			Stance stance = (from x in this.playerAnimator.moveSet.stanceList
			where x.isDefault
			select x).FirstOrDefault<Stance>();
			Stance blockStance = (from x in this.playerAnimator.moveSet.stanceList
			where x.unlocalizedName != null && PlayerInputAIManager.blockWords.Any((string y) => x.unlocalizedName.ToLower().Contains(y))
			select x).FirstOrDefault<Stance>();
			if (stance != null && stance.moveList != null)
			{
				if (blockStance != null)
				{
					Move move = (from x in stance.moveList
					where x.stanceGuid == blockStance.guid
					select x).FirstOrDefault<Move>();
					this.BlockStance = ((move != null) ? move.playerInput : null);
				}
				foreach (Move move2 in from x in stance.moveList
				where !string.IsNullOrEmpty(x.playerInput) && !x.stanceChange && x.inputType != inputType.Passive && x.inputType != inputType.PlayAtStart && !string.IsNullOrEmpty(x.name)
				select x)
				{
					string attackName = move2.unlocalizedName.ToLower();
					if (PlayerInputAIManager.attackWords.Any((string x) => attackName.Contains(x)))
					{
						AiAnimation aiAnimation = new AiAnimation
						{
							actionName = move2.playerInput,
							isAttack = true,
							duration = move2.duration
						};
						if (PlayerInputAIManager.directionLeftWords.Any((string x) => attackName.Contains(x)))
						{
							aiAnimation.attackDirection = AttackAnimationDirection.Left;
						}
						else if (PlayerInputAIManager.directionRightWords.Any((string x) => attackName.Contains(x)))
						{
							aiAnimation.attackDirection = AttackAnimationDirection.Right;
						}
						else if (PlayerInputAIManager.directionLowWords.Any((string x) => attackName.Contains(x)))
						{
							aiAnimation.attackDirection = AttackAnimationDirection.Low;
						}
						else if (PlayerInputAIManager.directionHighWords.Any((string x) => attackName.Contains(x)))
						{
							aiAnimation.attackDirection = AttackAnimationDirection.High;
						}
						else if (PlayerInputAIManager.directionCenterWords.Any((string x) => attackName.Contains(x)))
						{
							aiAnimation.attackDirection = AttackAnimationDirection.Stab;
						}
						if (move2.inputType == inputType.HoldDown)
						{
							JointMove jointMove = move2.jointMoveList.OrderBy((JointMove x) => x.executionTime).LastOrDefault<JointMove>();
							if (jointMove != null)
							{
								aiAnimation.duration = Convert.ToSingle(jointMove.executionTime + 0.44999998807907104);
							}
						}
						this.attackAnimations.Add(aiAnimation);
					}
				}
			}
			if (blockStance != null)
			{
				Move move3 = (from x in blockStance.moveList
				where !string.IsNullOrEmpty(x.unlocalizedName) && PlayerInputAIManager.directionLeftWords.Any((string y) => x.unlocalizedName.ToLower().Contains(y))
				select x).FirstOrDefault<Move>();
				this.BlockLeft = ((move3 != null) ? move3.playerInput : null);
				Move move4 = (from x in blockStance.moveList
				where !string.IsNullOrEmpty(x.unlocalizedName) && PlayerInputAIManager.directionCenterWords.Any((string y) => x.unlocalizedName.ToLower().Contains(y))
				select x).FirstOrDefault<Move>();
				this.BlockCenter = ((move4 != null) ? move4.playerInput : null);
				Move move5 = (from x in blockStance.moveList
				where !string.IsNullOrEmpty(x.unlocalizedName) && PlayerInputAIManager.directionRightWords.Any((string y) => x.unlocalizedName.ToLower().Contains(y))
				select x).FirstOrDefault<Move>();
				this.BlockRight = ((move5 != null) ? move5.playerInput : null);
				Move move6 = (from x in blockStance.moveList
				where !string.IsNullOrEmpty(x.unlocalizedName) && PlayerInputAIManager.directionHighWords.Any((string y) => x.unlocalizedName.ToLower().Contains(y))
				select x).FirstOrDefault<Move>();
				this.BlockHigh = ((move6 != null) ? move6.playerInput : null);
				Move move7 = (from x in blockStance.moveList
				where !string.IsNullOrEmpty(x.unlocalizedName) && PlayerInputAIManager.directionLowWords.Any((string y) => x.unlocalizedName.ToLower().Contains(y))
				select x).FirstOrDefault<Move>();
				this.BlockLow = ((move7 != null) ? move7.playerInput : null);
			}
		}
	}

	// Token: 0x06000593 RID: 1427 RVA: 0x0001B6A0 File Offset: 0x000198A0
	public void UpdateNavigationObstacleLocation()
	{
		if (this.playerHealth != null && this.playerHealth.navigationObstacle != null)
		{
			this.playerHealth.navigationObstacle.transform.position = new Vector3(this.playerHealth.cameraPositionPoint.transform.position.x, 0f, this.playerHealth.cameraPositionPoint.transform.position.z);
		}
	}

	// Token: 0x06000594 RID: 1428 RVA: 0x0001B724 File Offset: 0x00019924
	public ObjectAvoidanceResult CheckObjectAvoidance(Vector3[] path)
	{
		ObjectAvoidanceResult result = null;
		bool flag = false;
		if (this.playerHealth != null && this.playerHealth.navigationObstacle != null && this.playerHealth.navigationObstacle.activeInHierarchy)
		{
			flag = true;
			this.playerHealth.navigationObstacle.SetActive(false);
		}
		int i = 0;
		while (i < path.Length - 1)
		{
			RaycastHit raycastHit;
			if (Physics.Linecast(path[i], path[i + 1], out raycastHit, this.navigationMask))
			{
				if ((double)Vector3.Distance(this.playerHealth.cameraPositionPoint.transform.position, raycastHit.point) < 3.5)
				{
					result = new ObjectAvoidanceResult
					{
						hitpoint = raycastHit.point,
						hitDirection = (path[i + 1] - path[i]).normalized,
						hitColliderPosition = raycastHit.collider.transform.position
					};
					break;
				}
				break;
			}
			else
			{
				i++;
			}
		}
		if (flag)
		{
			this.playerHealth.navigationObstacle.SetActive(true);
		}
		return result;
	}

	// Token: 0x06000595 RID: 1429 RVA: 0x0000777A File Offset: 0x0000597A
	public void DrawPath(Vector3[] pathCorners)
	{
	}

	// Token: 0x06000596 RID: 1430 RVA: 0x0001B848 File Offset: 0x00019A48
	public float CalculatePathDistance(Vector3[] pathCorners)
	{
		float num = 0f;
		for (int i = 0; i < pathCorners.Length - 1; i++)
		{
			num += Vector3.Distance(pathCorners[i], pathCorners[i + 1]);
		}
		return num;
	}

	// Token: 0x06000597 RID: 1431 RVA: 0x0001B884 File Offset: 0x00019A84
	public bool GetObstacleAvoidanceMovement()
	{
		if (this.hasPriority)
		{
			return false;
		}
		this.elapsedFromLastForcedMovementUpdate += Time.deltaTime;
		if (this.elapsedFromLastForcedMovementUpdate > this.forcedMovementUpdateFrequency)
		{
			this.elapsedFromLastForcedMovementUpdate -= this.forcedMovementUpdateFrequency;
			this.forcedMovement = default(Vector2);
			this.useForcedMovement = false;
			float num = 2f;
			if (this.weaponMaxDistanceWithMargin > num)
			{
				num = this.weaponMaxDistanceWithMargin;
			}
			foreach (FriendlyPosition friendlyPosition in this.friendlyPositions)
			{
				if (friendlyPosition.distance < num)
				{
					this.useForcedMovement = true;
					if (-45f <= friendlyPosition.angle && friendlyPosition.angle <= 45f)
					{
						this.forcedMovement.x = this.forcedMovement.x + -1f;
					}
					else if (-135f >= friendlyPosition.angle || friendlyPosition.angle >= 135f)
					{
						this.useForcedMovement = false;
					}
					else if (friendlyPosition.angle < 0f)
					{
						this.forcedMovement.y = this.forcedMovement.y + -1f;
					}
					else
					{
						this.forcedMovement.y = this.forcedMovement.y + 1f;
					}
				}
			}
		}
		return this.useForcedMovement;
	}

	// Token: 0x06000598 RID: 1432 RVA: 0x0001B9E4 File Offset: 0x00019BE4
	private void GetFriendlyPositions()
	{
		this.elapsedFromLastFriendlyPositionsUpdate += Time.deltaTime;
		if (this.elapsedFromLastFriendlyPositionsUpdate > this.friendlyPositionsUpdateFrequency)
		{
			this.elapsedFromLastFriendlyPositionsUpdate -= this.friendlyPositionsUpdateFrequency;
			this.friendInFront = false;
			this.friendOnLeft = false;
			this.friendOnRight = false;
			this.FriendlyPositionsClear();
			if (this.otherPlayers != null && this.otherPlayers.Count > 0)
			{
				foreach (PlayerHealth playerHealth in this.otherPlayers)
				{
					if (playerHealth.ai && playerHealth.alive)
					{
						Vector3 position = this.playerHealth.cameraPositionPoint.transform.position;
						Vector3 position2 = playerHealth.cameraPositionPoint.transform.position;
						Vector3 b = new Vector3(position.x, 0f, position.z);
						Vector3 from = new Vector3(position2.x, 0f, position2.z) - b;
						float num = Vector3.SignedAngle(from, this.playerHealth.cameraPoint.transform.forward, Vector3.up);
						FriendlyPosition friendlyPositionFromPool = this.GetFriendlyPositionFromPool();
						friendlyPositionFromPool.angle = num;
						friendlyPositionFromPool.distance = from.magnitude;
						this.friendlyPositions.Add(friendlyPositionFromPool);
						if (from.magnitude < this.weaponMaxDistanceWithMargin)
						{
							if (Math.Abs(num) < 45f)
							{
								this.friendInFront = true;
								break;
							}
							if (Math.Abs(num) < 110f)
							{
								if (num > 0f)
								{
									this.friendOnLeft = true;
								}
								else
								{
									this.friendOnRight = true;
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06000599 RID: 1433 RVA: 0x0001BBC0 File Offset: 0x00019DC0
	private void InitFriendlyPosition()
	{
		this.pool_friendlyPosition = new List<FriendlyPosition>(8);
		for (int i = 0; i < 4; i++)
		{
			this.pool_friendlyPosition.Add(new FriendlyPosition());
		}
	}

	// Token: 0x0600059A RID: 1434 RVA: 0x0001BBF8 File Offset: 0x00019DF8
	private FriendlyPosition GetFriendlyPositionFromPool()
	{
		FriendlyPosition friendlyPosition = null;
		if (this.pool_friendlyPosition.Count > 0)
		{
			int index = this.pool_friendlyPosition.Count - 1;
			friendlyPosition = this.pool_friendlyPosition[index];
			this.pool_friendlyPosition.RemoveAt(index);
			friendlyPosition.Clear();
		}
		if (friendlyPosition == null)
		{
			friendlyPosition = new FriendlyPosition();
		}
		return friendlyPosition;
	}

	// Token: 0x0600059B RID: 1435 RVA: 0x0001BC4C File Offset: 0x00019E4C
	private void ReturnFriendlyPositionToPool(FriendlyPosition friendlyPosition)
	{
		this.pool_friendlyPosition.Add(friendlyPosition);
	}

	// Token: 0x0600059C RID: 1436 RVA: 0x0001BC5C File Offset: 0x00019E5C
	private void FriendlyPositionsClear()
	{
		for (int i = 0; i < this.friendlyPositions.Count; i++)
		{
			this.ReturnFriendlyPositionToPool(this.friendlyPositions[i]);
		}
		this.friendlyPositions.Clear();
	}

	// Token: 0x0600059D RID: 1437 RVA: 0x0001BC9C File Offset: 0x00019E9C
	private void UpdateProtectionPoints()
	{
		this.protectionPoint1 = this.selfProtectionTransform1.position;
		this.protectionPoint2 = this.selfProtectionTransform2.position;
	}

	// Token: 0x0600059E RID: 1438 RVA: 0x0001BCC0 File Offset: 0x00019EC0
	private EnemyBlade CalculateClosestWeaponPoint()
	{
		this.UpdateProtectionPoints();
		EnemyBlade enemyBlade = null;
		for (int i = 0; i < this.enemyBlades.Count; i++)
		{
			EnemyBlade enemyBlade2 = this.enemyBlades[i];
			enemyBlade2.CalculateClosestPointOnWeaponSections(this.protectionPoint1, this.protectionPoint2);
			if (enemyBlade == null || enemyBlade.currentBladeDistanceItem.distanceBetweenPoints > enemyBlade2.currentBladeDistanceItem.distanceBetweenPoints)
			{
				enemyBlade = enemyBlade2;
			}
		}
		return enemyBlade;
	}

	// Token: 0x0600059F RID: 1439 RVA: 0x0001BD2C File Offset: 0x00019F2C
	private AiBlockDirection GetBlockDirection(EnemyBlade enemyBlade)
	{
		AiBlockDirection result = AiBlockDirection.None;
		if (enemyBlade.currentBladeDistanceItem.distanceBetweenPoints < this.blockDistance)
		{
			Vector2 vector = new Vector2(this.playerHealth.cameraPoint.transform.forward.x, this.playerHealth.cameraPoint.transform.forward.z);
			Vector3 up = this.playerHealth.cameraPoint.transform.up;
			Vector3 right = this.playerHealth.cameraPoint.transform.right;
			Vector3 normalized = enemyBlade.currentBladeDistanceItem.vectorFromProtectedPointToWeaponPoint.normalized;
			float num = Vector2.Dot(new Vector2(normalized.x, normalized.z).normalized, vector.normalized);
			float num2 = Vector3.Dot(normalized, up);
			float num3 = Vector3.Dot(normalized, right);
			if ((double)num > 0.95 && Mathf.Abs(num2) < 0.3f && enemyBlade.currentBladeDistanceItem.distanceBetweenPoints < 1f)
			{
				result = AiBlockDirection.Center;
				if (num3 > 0f)
				{
					this.centerBlockSide = AiBlockDirection.Right;
				}
				else
				{
					this.centerBlockSide = AiBlockDirection.Left;
				}
			}
			else if ((num > 0.8f && num2 >= 0.3f) || (num2 > 0.5f && this.previousAiBlockDirection == AiBlockDirection.High))
			{
				result = AiBlockDirection.High;
			}
			else if (num2 < -0.3f)
			{
				result = AiBlockDirection.Low;
			}
			else if (num3 > 0f)
			{
				result = AiBlockDirection.Right;
			}
			else if (num3 < 0f)
			{
				result = AiBlockDirection.Left;
			}
		}
		this.previousAiBlockDirection = result;
		return result;
	}

	// Token: 0x04000342 RID: 834
	private List<BallMovement> ballMovements;

	// Token: 0x04000343 RID: 835
	private GameObject playerCharacter;

	// Token: 0x04000344 RID: 836
	private RotatePlayer rotatePlayer;

	// Token: 0x04000345 RID: 837
	private PlayerAnimator playerAnimator;

	// Token: 0x04000346 RID: 838
	public bool rollingFeet;

	// Token: 0x04000347 RID: 839
	public PlayerHealth playerHealth;

	// Token: 0x04000348 RID: 840
	private PlayerHealth previousEnemy;

	// Token: 0x04000349 RID: 841
	public PlayerHealth targetEnemy;

	// Token: 0x0400034A RID: 842
	private Transform moveTarget;

	// Token: 0x0400034B RID: 843
	private List<PlayerHealth> otherPlayers = new List<PlayerHealth>();

	// Token: 0x0400034C RID: 844
	public GameObject testTarget;

	// Token: 0x0400034D RID: 845
	private float weaponMaxDistanceMargin = 0.4f;

	// Token: 0x0400034E RID: 846
	private float weaponMinDistanceMargin = 0.3f;

	// Token: 0x0400034F RID: 847
	private float weaponMaxDistance = 2.8f;

	// Token: 0x04000350 RID: 848
	private float weaponMinDistance = 2f;

	// Token: 0x04000351 RID: 849
	private float walkMargin = 0.2f;

	// Token: 0x04000352 RID: 850
	private float? overrideMaxWalkDistance;

	// Token: 0x04000353 RID: 851
	private float? overrideMinWalkDistance;

	// Token: 0x04000354 RID: 852
	private AIPath currentPath;

	// Token: 0x04000355 RID: 853
	private float elapsedFromLastPathUpdate;

	// Token: 0x04000356 RID: 854
	private float corneringDistance = 0.5f;

	// Token: 0x04000357 RID: 855
	public float pathUpdateFrequency = 0.5f;

	// Token: 0x04000358 RID: 856
	public float blockDistance = 1.5f;

	// Token: 0x04000359 RID: 857
	public float blockMaxDuration = 3f;

	// Token: 0x0400035A RID: 858
	public float elapsedTimeFromLastEnemyUpdate;

	// Token: 0x0400035B RID: 859
	public float enemyTargetUpdateFrequency = 4f;

	// Token: 0x0400035C RID: 860
	private float forcedMovementUpdateFrequency = 0.3f;

	// Token: 0x0400035D RID: 861
	private float elapsedFromLastForcedMovementUpdate;

	// Token: 0x0400035E RID: 862
	private float friendlyPositionsUpdateFrequency = 0.2f;

	// Token: 0x0400035F RID: 863
	private float elapsedFromLastFriendlyPositionsUpdate;

	// Token: 0x04000360 RID: 864
	public List<EnemyBlade> enemyBlades = new List<EnemyBlade>();

	// Token: 0x04000361 RID: 865
	private LayerMask navigationMask;

	// Token: 0x04000362 RID: 866
	private Transform selfProtectionTransform1;

	// Token: 0x04000363 RID: 867
	private Transform selfProtectionTransform2;

	// Token: 0x04000364 RID: 868
	private float forwardMoveValue;

	// Token: 0x04000365 RID: 869
	private float sideMoveValue;

	// Token: 0x04000366 RID: 870
	private float turnValue;

	// Token: 0x04000367 RID: 871
	private float oldForwardMoveValue;

	// Token: 0x04000368 RID: 872
	private float oldSideMoveValue;

	// Token: 0x04000369 RID: 873
	private float oldTurnValue;

	// Token: 0x0400036A RID: 874
	private int currentPathTarget = 1;

	// Token: 0x0400036B RID: 875
	private NavMeshHit testHit;

	// Token: 0x0400036C RID: 876
	private string BlockHigh = "";

	// Token: 0x0400036D RID: 877
	private string BlockLeft = "";

	// Token: 0x0400036E RID: 878
	private string BlockCenter = "";

	// Token: 0x0400036F RID: 879
	private string BlockRight = "";

	// Token: 0x04000370 RID: 880
	private string BlockLow = "";

	// Token: 0x04000371 RID: 881
	private string BlockStance = "";

	// Token: 0x04000372 RID: 882
	private float shortestBlockDistance;

	// Token: 0x04000373 RID: 883
	private bool isBlocking;

	// Token: 0x04000374 RID: 884
	private string performedBlock = "";

	// Token: 0x04000375 RID: 885
	private float centerBlockStarted;

	// Token: 0x04000376 RID: 886
	private float blockStarted;

	// Token: 0x04000377 RID: 887
	public List<AiAnimation> attackAnimations = new List<AiAnimation>();

	// Token: 0x04000378 RID: 888
	private float nextAttackAllowedTime;

	// Token: 0x04000379 RID: 889
	public bool attacking;

	// Token: 0x0400037A RID: 890
	private string performedAttack;

	// Token: 0x0400037B RID: 891
	private bool friendInFront;

	// Token: 0x0400037C RID: 892
	private bool friendOnLeft;

	// Token: 0x0400037D RID: 893
	private bool friendOnRight;

	// Token: 0x0400037E RID: 894
	private float distanceToEnemy = 999f;

	// Token: 0x0400037F RID: 895
	private int lastDistanceToEnemyCalculated;

	// Token: 0x04000380 RID: 896
	private float lookingAtEnemyDotProduct = 1f;

	// Token: 0x04000381 RID: 897
	private int lastLookingAtEnemyDotProduct;

	// Token: 0x04000382 RID: 898
	public float attackDelay = 0.8f;

	// Token: 0x04000383 RID: 899
	private List<AiAnimation> usableAnimations = new List<AiAnimation>();

	// Token: 0x04000384 RID: 900
	private static List<string> attackWords = new List<string>();

	// Token: 0x04000385 RID: 901
	private static List<string> blockWords = new List<string>();

	// Token: 0x04000386 RID: 902
	private static List<string> directionHighWords = new List<string>();

	// Token: 0x04000387 RID: 903
	private static List<string> directionLowWords = new List<string>();

	// Token: 0x04000388 RID: 904
	private static List<string> directionLeftWords = new List<string>();

	// Token: 0x04000389 RID: 905
	private static List<string> directionRightWords = new List<string>();

	// Token: 0x0400038A RID: 906
	private static List<string> directionCenterWords = new List<string>();

	// Token: 0x0400038B RID: 907
	private Vector2 forcedMovement;

	// Token: 0x0400038C RID: 908
	private bool useForcedMovement;

	// Token: 0x0400038D RID: 909
	public List<FriendlyPosition> friendlyPositions = new List<FriendlyPosition>(8);

	// Token: 0x0400038E RID: 910
	public List<FriendlyPosition> pool_friendlyPosition;

	// Token: 0x0400038F RID: 911
	private Vector3 protectionPoint1;

	// Token: 0x04000390 RID: 912
	private Vector3 protectionPoint2;

	// Token: 0x04000391 RID: 913
	private AiBlockDirection centerBlockSide = AiBlockDirection.Right;

	// Token: 0x04000392 RID: 914
	private AiBlockDirection previousAiBlockDirection;
}
