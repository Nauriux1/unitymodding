using System;
using System.Collections.Generic;
using MoveClasses;

// Token: 0x0200011E RID: 286
public class ClearEquipmentCommand : ICommand
{
	// Token: 0x060008E4 RID: 2276 RVA: 0x0002B3B4 File Offset: 0x000295B4
	public ClearEquipmentCommand(MoveSet newMoveSet)
	{
		this.moveSet = newMoveSet;
		this.oldEquippedEquipment = new List<EquippedEquipment>();
		foreach (EquippedEquipment item in this.moveSet.defaultEquipment)
		{
			this.oldEquippedEquipment.Add(item);
		}
	}

	// Token: 0x060008E5 RID: 2277 RVA: 0x0002B42C File Offset: 0x0002962C
	public void Execute()
	{
		this.moveSet.defaultEquipment.Clear();
		this.UpdateVisuals();
	}

	// Token: 0x060008E6 RID: 2278 RVA: 0x0002B444 File Offset: 0x00029644
	public void Undo()
	{
		this.moveSet.defaultEquipment.Clear();
		foreach (EquippedEquipment item in this.oldEquippedEquipment)
		{
			this.moveSet.defaultEquipment.Add(item);
		}
		this.UpdateVisuals();
	}

	// Token: 0x060008E7 RID: 2279 RVA: 0x0002B4B8 File Offset: 0x000296B8
	private void UpdateVisuals()
	{
		if (MoveSetEditor.singleton.CheckCurrentView(null, null, true))
		{
			MoveSetEditor.singleton.equipmentPanel.UpdateEquipmentUIAfterEquipmentChange(null);
		}
	}

	// Token: 0x04000630 RID: 1584
	private MoveSet moveSet;

	// Token: 0x04000631 RID: 1585
	private List<EquippedEquipment> oldEquippedEquipment;
}
