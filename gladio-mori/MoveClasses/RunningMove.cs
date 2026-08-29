using System;

namespace MoveClasses
{
	// Token: 0x020002A5 RID: 677
	public class RunningMove
	{
		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06001384 RID: 4996 RVA: 0x00064F46 File Offset: 0x00063146
		// (set) Token: 0x06001385 RID: 4997 RVA: 0x00064F4E File Offset: 0x0006314E
		public Move move { get; set; }

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06001386 RID: 4998 RVA: 0x00064F57 File Offset: 0x00063157
		// (set) Token: 0x06001387 RID: 4999 RVA: 0x00064F5F File Offset: 0x0006315F
		public double time { get; set; }

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06001388 RID: 5000 RVA: 0x00064F68 File Offset: 0x00063168
		// (set) Token: 0x06001389 RID: 5001 RVA: 0x00064F70 File Offset: 0x00063170
		public double executedTime { get; set; }

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x0600138A RID: 5002 RVA: 0x00064F79 File Offset: 0x00063179
		// (set) Token: 0x0600138B RID: 5003 RVA: 0x00064F81 File Offset: 0x00063181
		public bool preview { get; set; }

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x0600138C RID: 5004 RVA: 0x00064F8A File Offset: 0x0006318A
		// (set) Token: 0x0600138D RID: 5005 RVA: 0x00064F92 File Offset: 0x00063192
		public bool playOnlyActive { get; set; }
	}
}
