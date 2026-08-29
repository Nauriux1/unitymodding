using System;

namespace MoveClasses
{
	// Token: 0x020002A4 RID: 676
	public class CancellablePlayerAction
	{
		// Token: 0x170001FA RID: 506
		// (get) Token: 0x0600137C RID: 4988 RVA: 0x00064EF8 File Offset: 0x000630F8
		// (set) Token: 0x0600137D RID: 4989 RVA: 0x00064F00 File Offset: 0x00063100
		public string name { get; set; }

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x0600137E RID: 4990 RVA: 0x00064F09 File Offset: 0x00063109
		// (set) Token: 0x0600137F RID: 4991 RVA: 0x00064F11 File Offset: 0x00063111
		public Stance stance { get; set; }

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06001380 RID: 4992 RVA: 0x00064F1A File Offset: 0x0006311A
		// (set) Token: 0x06001381 RID: 4993 RVA: 0x00064F22 File Offset: 0x00063122
		public Move move { get; set; }

		// Token: 0x06001382 RID: 4994 RVA: 0x00064F2B File Offset: 0x0006312B
		public void Clear()
		{
			this.name = "";
			this.stance = null;
			this.move = null;
		}
	}
}
