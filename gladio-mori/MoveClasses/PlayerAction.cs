using System;

namespace MoveClasses
{
	// Token: 0x020002A3 RID: 675
	public class PlayerAction
	{
		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06001375 RID: 4981 RVA: 0x00064EC5 File Offset: 0x000630C5
		// (set) Token: 0x06001376 RID: 4982 RVA: 0x00064ECD File Offset: 0x000630CD
		public string name { get; set; }

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06001377 RID: 4983 RVA: 0x00064ED6 File Offset: 0x000630D6
		// (set) Token: 0x06001378 RID: 4984 RVA: 0x00064EDE File Offset: 0x000630DE
		public ActionType type { get; set; }

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06001379 RID: 4985 RVA: 0x00064EE7 File Offset: 0x000630E7
		// (set) Token: 0x0600137A RID: 4986 RVA: 0x00064EEF File Offset: 0x000630EF
		public float value { get; set; }
	}
}
