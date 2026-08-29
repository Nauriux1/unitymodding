using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200005B RID: 91
public class AiManager : MonoBehaviour
{
	// Token: 0x0600029F RID: 671 RVA: 0x0000C831 File Offset: 0x0000AA31
	private void Awake()
	{
		AiManager.singleton = this;
	}

	// Token: 0x060002A0 RID: 672 RVA: 0x0000C83C File Offset: 0x0000AA3C
	private void Start()
	{
		this.aiPlayers = UnityEngine.Object.FindObjectsOfType<PlayerInputAIManager>().ToList<PlayerInputAIManager>();
		this.targetPlayers = (from x in UnityEngine.Object.FindObjectsOfType<PlayerHealth>()
		where !x.ai
		select x).ToList<PlayerHealth>();
		this.CheckForSingleTarget();
	}

	// Token: 0x060002A1 RID: 673 RVA: 0x0000C893 File Offset: 0x0000AA93
	private void Update()
	{
		this.CheckPriority();
	}

	// Token: 0x060002A2 RID: 674 RVA: 0x0000C89C File Offset: 0x0000AA9C
	private void CheckForSingleTarget()
	{
		this.target = null;
		IEnumerable<PlayerHealth> source = from x in this.targetPlayers
		where x.alive
		select x;
		if (source.Count<PlayerHealth>() == 1)
		{
			this.target = source.FirstOrDefault<PlayerHealth>();
		}
	}

	// Token: 0x060002A3 RID: 675 RVA: 0x0000C8F0 File Offset: 0x0000AAF0
	private void CheckPriority()
	{
		this.elapsedFromLastPriorityUpdate += Time.deltaTime;
		if (this.elapsedFromLastPriorityUpdate > this.priorityUpdateFrequency)
		{
			this.elapsedFromLastPriorityUpdate -= this.priorityUpdateFrequency;
			AiManager.priorityAi = null;
			if (this.target != null)
			{
				float num = 10000f;
				foreach (PlayerInputAIManager playerInputAIManager in this.aiPlayers)
				{
					float num2 = Vector3.Distance(playerInputAIManager.playerHealth.cameraPositionPoint.transform.position, this.target.cameraPositionPoint.transform.position);
					if (num2 < num)
					{
						AiManager.priorityAi = playerInputAIManager;
						num = num2;
					}
				}
			}
		}
	}

	// Token: 0x04000189 RID: 393
	private List<PlayerInputAIManager> aiPlayers = new List<PlayerInputAIManager>();

	// Token: 0x0400018A RID: 394
	private List<PlayerHealth> targetPlayers = new List<PlayerHealth>();

	// Token: 0x0400018B RID: 395
	private PlayerHealth target;

	// Token: 0x0400018C RID: 396
	public static PlayerInputAIManager priorityAi;

	// Token: 0x0400018D RID: 397
	public static AiManager singleton;

	// Token: 0x0400018E RID: 398
	private float priorityUpdateFrequency = 0.2f;

	// Token: 0x0400018F RID: 399
	private float elapsedFromLastPriorityUpdate;
}
