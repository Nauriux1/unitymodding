using System;
using System.Linq;
using MoveClasses;

// Token: 0x0200011C RID: 284
public class SetEquipmentCommand : ICommand
{
	// Token: 0x060008DD RID: 2269 RVA: 0x0002B1F8 File Offset: 0x000293F8
	public SetEquipmentCommand(MoveSet newMoveSet, EquippedEquipment newEquippedEquipment)
	{
		this.moveSet = newMoveSet;
		this.equippedEquipment = newEquippedEquipment;
		this.oldEquippedEquipment = (from x in newMoveSet.defaultEquipment
		where x.position == newEquippedEquipment.position
		select x).FirstOrDefault<EquippedEquipment>();
	}

	// Token: 0x060008DE RID: 2270 RVA: 0x0002B250 File Offset: 0x00029450
	public void Execute()
	{
		foreach (EquippedEquipment item in (from x in this.moveSet.defaultEquipment
		where x.position == this.equippedEquipment.position
		select x).ToList<EquippedEquipment>())
		{
			this.moveSet.defaultEquipment.Remove(item);
		}
		if (this.equippedEquipment != null && this.equippedEquipment.equipment != null)
		{
			this.moveSet.defaultEquipment.Add(this.equippedEquipment);
		}
		this.UpdateVisuals();
	}

	// Token: 0x060008DF RID: 2271 RVA: 0x0002B2FC File Offset: 0x000294FC
	public void Undo()
	{
		if (this.equippedEquipment != null && this.equippedEquipment.equipment != null)
		{
			this.moveSet.defaultEquipment.Remove(this.equippedEquipment);
		}
		if (this.oldEquippedEquipment != null)
		{
			this.moveSet.defaultEquipment.Add(this.oldEquippedEquipment);
		}
		this.UpdateVisuals();
	}

	// Token: 0x060008E0 RID: 2272 RVA: 0x0002B359 File Offset: 0x00029559
	private void UpdateVisuals()
	{
		if (MoveSetEditor.singleton.CheckCurrentView(null, null, true))
		{
			MoveSetEditor.singleton.equipmentPanel.UpdateEquipmentUIAfterEquipmentChange(new EquipmentPosition?(this.equippedEquipment.position));
		}
	}

	// Token: 0x0400062C RID: 1580
	private MoveSet moveSet;

	// Token: 0x0400062D RID: 1581
	private EquippedEquipment equippedEquipment;

	// Token: 0x0400062E RID: 1582
	private EquippedEquipment oldEquippedEquipment;
}
