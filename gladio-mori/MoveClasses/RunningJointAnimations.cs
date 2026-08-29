using System;

namespace MoveClasses
{
	// Token: 0x020002AE RID: 686
	[Serializable]
	public class RunningJointAnimations
	{
		// Token: 0x17000239 RID: 569
		// (get) Token: 0x0600141D RID: 5149 RVA: 0x00065B40 File Offset: 0x00063D40
		// (set) Token: 0x0600141E RID: 5150 RVA: 0x00065B48 File Offset: 0x00063D48
		public JointType jointType { get; set; }

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x0600141F RID: 5151 RVA: 0x00065B51 File Offset: 0x00063D51
		// (set) Token: 0x06001420 RID: 5152 RVA: 0x00065B59 File Offset: 0x00063D59
		public int layer { get; set; }
	}
}
