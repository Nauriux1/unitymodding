using System;
using MoveClasses;

// Token: 0x020000FA RID: 250
[Serializable]
public class TutorialTask
{
	// Token: 0x040005BC RID: 1468
	public TutorialTaskType taskType;

	// Token: 0x040005BD RID: 1469
	public bool done;

	// Token: 0x040005BE RID: 1470
	public Move move;

	// Token: 0x040005BF RID: 1471
	public TutorialTaskRow row;

	// Token: 0x040005C0 RID: 1472
	public float? startTime;

	// Token: 0x040005C1 RID: 1473
	public PlayerHealth target;

	// Token: 0x040005C2 RID: 1474
	public string inputString = "";

	// Token: 0x040005C3 RID: 1475
	public float floatValue;

	// Token: 0x040005C4 RID: 1476
	public bool positive;
}
