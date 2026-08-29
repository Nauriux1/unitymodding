using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x020000F8 RID: 248
[Serializable]
public class TutorialTaskGroup
{
	// Token: 0x17000141 RID: 321
	// (get) Token: 0x06000849 RID: 2121 RVA: 0x00029826 File Offset: 0x00027A26
	public bool done
	{
		get
		{
			return (from x in this.tutorialTasks
			where !x.done
			select x).FirstOrDefault<TutorialTask>() == null;
		}
	}

	// Token: 0x040005B5 RID: 1461
	public int number;

	// Token: 0x040005B6 RID: 1462
	public string title = "";

	// Token: 0x040005B7 RID: 1463
	public string inputString = "";

	// Token: 0x040005B8 RID: 1464
	public string mouseTip = "";

	// Token: 0x040005B9 RID: 1465
	public List<TutorialTask> tutorialTasks = new List<TutorialTask>();
}
