using System;
using UnityEngine;

namespace MoveClasses
{
	// Token: 0x020002AD RID: 685
	[Serializable]
	public class FighterJoint
	{
		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06001412 RID: 5138 RVA: 0x00065AEB File Offset: 0x00063CEB
		// (set) Token: 0x06001413 RID: 5139 RVA: 0x00065AF3 File Offset: 0x00063CF3
		public GameObject joint { get; set; }

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06001414 RID: 5140 RVA: 0x00065AFC File Offset: 0x00063CFC
		// (set) Token: 0x06001415 RID: 5141 RVA: 0x00065B04 File Offset: 0x00063D04
		public GameObject physicsJoint { get; set; }

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06001416 RID: 5142 RVA: 0x00065B0D File Offset: 0x00063D0D
		// (set) Token: 0x06001417 RID: 5143 RVA: 0x00065B15 File Offset: 0x00063D15
		public JointType jointType { get; set; }

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06001418 RID: 5144 RVA: 0x00065B1E File Offset: 0x00063D1E
		// (set) Token: 0x06001419 RID: 5145 RVA: 0x00065B26 File Offset: 0x00063D26
		public Hand hand { get; set; }

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x0600141A RID: 5146 RVA: 0x00065B2F File Offset: 0x00063D2F
		// (set) Token: 0x0600141B RID: 5147 RVA: 0x00065B37 File Offset: 0x00063D37
		public JointStrength jointStrength { get; set; }
	}
}
