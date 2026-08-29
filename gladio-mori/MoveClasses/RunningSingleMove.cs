using System;
using UnityEngine;

namespace MoveClasses
{
	// Token: 0x020002A6 RID: 678
	[Serializable]
	public class RunningSingleMove
	{
		// Token: 0x17000202 RID: 514
		// (get) Token: 0x0600138F RID: 5007 RVA: 0x00064F9B File Offset: 0x0006319B
		// (set) Token: 0x06001390 RID: 5008 RVA: 0x00064FA3 File Offset: 0x000631A3
		public Move move { get; set; }

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06001391 RID: 5009 RVA: 0x00064FAC File Offset: 0x000631AC
		// (set) Token: 0x06001392 RID: 5010 RVA: 0x00064FB4 File Offset: 0x000631B4
		public JointMove singleMove { get; set; }

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06001393 RID: 5011 RVA: 0x00064FBD File Offset: 0x000631BD
		// (set) Token: 0x06001394 RID: 5012 RVA: 0x00064FC5 File Offset: 0x000631C5
		public double removeTime { get; set; }

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06001395 RID: 5013 RVA: 0x00064FCE File Offset: 0x000631CE
		// (set) Token: 0x06001396 RID: 5014 RVA: 0x00064FD6 File Offset: 0x000631D6
		public double executeAtTime { get; set; }

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06001397 RID: 5015 RVA: 0x00064FDF File Offset: 0x000631DF
		// (set) Token: 0x06001398 RID: 5016 RVA: 0x00064FE7 File Offset: 0x000631E7
		public double moveSetExcecutionStartTime { get; set; }

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06001399 RID: 5017 RVA: 0x00064FF0 File Offset: 0x000631F0
		// (set) Token: 0x0600139A RID: 5018 RVA: 0x00064FF8 File Offset: 0x000631F8
		public bool passive { get; set; }

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x0600139B RID: 5019 RVA: 0x00065001 File Offset: 0x00063201
		// (set) Token: 0x0600139C RID: 5020 RVA: 0x00065009 File Offset: 0x00063209
		public bool preview { get; set; }

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x0600139D RID: 5021 RVA: 0x00065012 File Offset: 0x00063212
		// (set) Token: 0x0600139E RID: 5022 RVA: 0x0006501A File Offset: 0x0006321A
		public bool playOnlyActive { get; set; }

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x0600139F RID: 5023 RVA: 0x00065023 File Offset: 0x00063223
		// (set) Token: 0x060013A0 RID: 5024 RVA: 0x0006502B File Offset: 0x0006322B
		public bool removeAfterRunningOnce { get; set; }

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x060013A1 RID: 5025 RVA: 0x00065034 File Offset: 0x00063234
		// (set) Token: 0x060013A2 RID: 5026 RVA: 0x0006503C File Offset: 0x0006323C
		public bool remove { get; set; }

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x060013A3 RID: 5027 RVA: 0x00065045 File Offset: 0x00063245
		// (set) Token: 0x060013A4 RID: 5028 RVA: 0x0006504D File Offset: 0x0006324D
		public double? previewPercentage { get; set; }

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x060013A5 RID: 5029 RVA: 0x00065056 File Offset: 0x00063256
		// (set) Token: 0x060013A6 RID: 5030 RVA: 0x0006505E File Offset: 0x0006325E
		public bool previewOnTheTick { get; set; }

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x060013A7 RID: 5031 RVA: 0x00065067 File Offset: 0x00063267
		// (set) Token: 0x060013A8 RID: 5032 RVA: 0x0006506F File Offset: 0x0006326F
		public Quaternion? tempQuaternion { get; set; }

		// Token: 0x060013A9 RID: 5033 RVA: 0x00065078 File Offset: 0x00063278
		public void Clear()
		{
			this.move = null;
			this.singleMove = null;
			this.removeTime = 0.0;
			this.executeAtTime = 0.0;
			this.moveSetExcecutionStartTime = 0.0;
			this.passive = false;
			this.preview = false;
			this.playOnlyActive = false;
			this.removeAfterRunningOnce = false;
			this.remove = false;
			this.previewPercentage = null;
			this.previewOnTheTick = false;
			this.tempQuaternion = null;
		}
	}
}
