using System;
using UnityEngine;

// Token: 0x0200005E RID: 94
public class PlayerInputAiManagerContinuousAttack : PlayerInputAIManager
{
	// Token: 0x060002AA RID: 682 RVA: 0x0000CA24 File Offset: 0x0000AC24
	public override void SetParameters(CustomAiObject customAiObject)
	{
		ContinuousAttackAiObject continuousAttackAiObject = (ContinuousAttackAiObject)customAiObject;
		this.continuousAction = continuousAttackAiObject.ActionName;
		base.SetParameters(customAiObject);
	}

	// Token: 0x060002AB RID: 683 RVA: 0x0000777A File Offset: 0x0000597A
	public override void DoBlock()
	{
	}

	// Token: 0x060002AC RID: 684 RVA: 0x0000CA4C File Offset: 0x0000AC4C
	public override void DoAttack()
	{
		if (!this.attacking && this.targetEnemy != null)
		{
			if (this.firstAttackAttemptTime == null)
			{
				this.firstAttackAttemptTime = new float?(Time.time);
			}
			if (Time.time > this.firstAttackAttemptTime.Value + this.waitBeforeAttacking)
			{
				AiAnimation aiAnimation = base.GetAiAnimation(this.continuousAction);
				base.PerformAttack(aiAnimation);
			}
		}
		if (this.attacking && this.targetEnemy == null && !this.ended)
		{
			this.ended = true;
			base.Invoke("EndAttacking", 1f);
		}
	}

	// Token: 0x060002AD RID: 685 RVA: 0x0000CAEE File Offset: 0x0000ACEE
	private void EndAttacking()
	{
		base.StopAttack();
	}

	// Token: 0x04000194 RID: 404
	private string continuousAction;

	// Token: 0x04000195 RID: 405
	public float waitBeforeAttacking = 1f;

	// Token: 0x04000196 RID: 406
	private float? firstAttackAttemptTime;

	// Token: 0x04000197 RID: 407
	private bool ended;
}
