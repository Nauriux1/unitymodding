using System;
using System.Collections.Generic;
using MoveClasses;
using UnityEngine;

// Token: 0x02000057 RID: 87
public interface IRoomPlayer
{
	// Token: 0x17000091 RID: 145
	// (get) Token: 0x0600024F RID: 591
	// (set) Token: 0x06000250 RID: 592
	MoveSet selectedMoveSet { get; set; }

	// Token: 0x17000092 RID: 146
	// (get) Token: 0x06000251 RID: 593
	// (set) Token: 0x06000252 RID: 594
	List<EquippedEquipment> selectedEquipment { get; set; }

	// Token: 0x17000093 RID: 147
	// (get) Token: 0x06000253 RID: 595
	// (set) Token: 0x06000254 RID: 596
	PlayerHealth playerHealth { get; set; }

	// Token: 0x17000094 RID: 148
	// (get) Token: 0x06000255 RID: 597
	bool playerReadyState { get; }

	// Token: 0x06000256 RID: 598
	void SetReady();

	// Token: 0x06000257 RID: 599
	void SetMoveSet(MoveSet newMoveSet);

	// Token: 0x06000258 RID: 600
	MoveSet GetMoveSet();

	// Token: 0x06000259 RID: 601
	void SetEquipment(List<EquippedEquipment> newEquipment);

	// Token: 0x0600025A RID: 602
	List<EquippedEquipment> GetSelectedEquipment();

	// Token: 0x0600025B RID: 603
	void GoBack();

	// Token: 0x0600025C RID: 604
	Camera GetCamera();

	// Token: 0x17000095 RID: 149
	// (get) Token: 0x0600025D RID: 605
	// (set) Token: 0x0600025E RID: 606
	PlayerCanvasController playerCanvasContoller { get; set; }

	// Token: 0x17000096 RID: 150
	// (get) Token: 0x0600025F RID: 607
	// (set) Token: 0x06000260 RID: 608
	bool ai { get; set; }

	// Token: 0x06000261 RID: 609
	void SetSpectator(bool value);

	// Token: 0x06000262 RID: 610
	bool GetSpectator();

	// Token: 0x06000263 RID: 611
	bool ApplyTempPlayerValues();

	// Token: 0x06000264 RID: 612
	void UpdatePreviewVisuals();

	// Token: 0x06000265 RID: 613
	void SetEquipmentStartingHold(EquippedEquipment equippedEquipment);
}
