using System;
using MoveClasses;
using UnityEngine;

// Token: 0x02000203 RID: 515
public class SelectedJoint
{
	// Token: 0x170001A7 RID: 423
	// (get) Token: 0x06000FE5 RID: 4069 RVA: 0x00053646 File Offset: 0x00051846
	// (set) Token: 0x06000FE6 RID: 4070 RVA: 0x0005364E File Offset: 0x0005184E
	public JointType JointType { get; set; }

	// Token: 0x170001A8 RID: 424
	// (get) Token: 0x06000FE7 RID: 4071 RVA: 0x00053657 File Offset: 0x00051857
	// (set) Token: 0x06000FE8 RID: 4072 RVA: 0x0005365F File Offset: 0x0005185F
	public GameObject PhysicsJoint { get; set; }

	// Token: 0x170001A9 RID: 425
	// (get) Token: 0x06000FE9 RID: 4073 RVA: 0x00053668 File Offset: 0x00051868
	// (set) Token: 0x06000FEA RID: 4074 RVA: 0x00053670 File Offset: 0x00051870
	public GameObject AnimationJoint { get; set; }

	// Token: 0x170001AA RID: 426
	// (get) Token: 0x06000FEB RID: 4075 RVA: 0x00053679 File Offset: 0x00051879
	// (set) Token: 0x06000FEC RID: 4076 RVA: 0x00053681 File Offset: 0x00051881
	public ConfigurableJointScript JointScript { get; set; }
}
