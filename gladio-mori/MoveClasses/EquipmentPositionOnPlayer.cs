using System;
using UnityEngine;

namespace MoveClasses
{
	// Token: 0x020002B1 RID: 689
	public class EquipmentPositionOnPlayer
	{
		// Token: 0x1700023E RID: 574
		// (get) Token: 0x0600142C RID: 5164 RVA: 0x00065C58 File Offset: 0x00063E58
		// (set) Token: 0x0600142D RID: 5165 RVA: 0x00065C60 File Offset: 0x00063E60
		public bool physics { get; set; }

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x0600142E RID: 5166 RVA: 0x00065C69 File Offset: 0x00063E69
		// (set) Token: 0x0600142F RID: 5167 RVA: 0x00065C71 File Offset: 0x00063E71
		public EquipmentPosition equipmentPosition { get; set; }

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06001430 RID: 5168 RVA: 0x00065C7A File Offset: 0x00063E7A
		// (set) Token: 0x06001431 RID: 5169 RVA: 0x00065C82 File Offset: 0x00063E82
		public Hand hand { get; set; }

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06001432 RID: 5170 RVA: 0x00065C8B File Offset: 0x00063E8B
		// (set) Token: 0x06001433 RID: 5171 RVA: 0x00065C93 File Offset: 0x00063E93
		public Transform spawnPosition { get; set; }

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06001434 RID: 5172 RVA: 0x00065C9C File Offset: 0x00063E9C
		// (set) Token: 0x06001435 RID: 5173 RVA: 0x00065CA4 File Offset: 0x00063EA4
		public BodySide bodySide { get; set; }

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06001436 RID: 5174 RVA: 0x00065CAD File Offset: 0x00063EAD
		// (set) Token: 0x06001437 RID: 5175 RVA: 0x00065CB5 File Offset: 0x00063EB5
		public CuttableGameObject cuttableGameObject { get; set; }
	}
}
