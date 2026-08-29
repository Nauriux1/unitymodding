using System;
using System.Collections.Generic;
using MoveClasses;

// Token: 0x0200014D RID: 333
public class TempMultiplayerPlayerValues
{
	// Token: 0x1700015A RID: 346
	// (get) Token: 0x06000A68 RID: 2664 RVA: 0x00030DC1 File Offset: 0x0002EFC1
	// (set) Token: 0x06000A69 RID: 2665 RVA: 0x00030DC9 File Offset: 0x0002EFC9
	public MoveSet selectedMoveSet { get; set; }

	// Token: 0x1700015B RID: 347
	// (get) Token: 0x06000A6A RID: 2666 RVA: 0x00030DD2 File Offset: 0x0002EFD2
	// (set) Token: 0x06000A6B RID: 2667 RVA: 0x00030DDA File Offset: 0x0002EFDA
	public List<EquippedEquipment> selectedEquipment { get; set; }

	// Token: 0x1700015C RID: 348
	// (get) Token: 0x06000A6C RID: 2668 RVA: 0x00030DE3 File Offset: 0x0002EFE3
	// (set) Token: 0x06000A6D RID: 2669 RVA: 0x00030DEB File Offset: 0x0002EFEB
	public bool spectator { get; set; }

	// Token: 0x04000749 RID: 1865
	public bool equipmentHasBeenEdited;
}
