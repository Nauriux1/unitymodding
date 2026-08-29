using System;
using UnityEngine;

// Token: 0x02000204 RID: 516
public class SelectedRigTarget
{
	// Token: 0x170001AB RID: 427
	// (get) Token: 0x06000FEE RID: 4078 RVA: 0x0005368A File Offset: 0x0005188A
	// (set) Token: 0x06000FEF RID: 4079 RVA: 0x00053692 File Offset: 0x00051892
	public GameObject GameObject { get; set; }

	// Token: 0x170001AC RID: 428
	// (get) Token: 0x06000FF0 RID: 4080 RVA: 0x0005369B File Offset: 0x0005189B
	// (set) Token: 0x06000FF1 RID: 4081 RVA: 0x000536A3 File Offset: 0x000518A3
	public SimpleRig Rig { get; set; }

	// Token: 0x170001AD RID: 429
	// (get) Token: 0x06000FF2 RID: 4082 RVA: 0x000536AC File Offset: 0x000518AC
	// (set) Token: 0x06000FF3 RID: 4083 RVA: 0x000536B4 File Offset: 0x000518B4
	public bool isHint { get; set; }

	// Token: 0x170001AE RID: 430
	// (get) Token: 0x06000FF4 RID: 4084 RVA: 0x000536BD File Offset: 0x000518BD
	public bool canRotate
	{
		get
		{
			return this.Rig != null && this.Rig.targetCanRotate && !this.isHint;
		}
	}
}
