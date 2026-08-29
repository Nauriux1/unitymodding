using System;
using System.Collections.Generic;
using MoveClasses;

// Token: 0x02000115 RID: 277
public class AddStanceCommand : ICommand
{
	// Token: 0x060008C1 RID: 2241 RVA: 0x0002ADAC File Offset: 0x00028FAC
	public AddStanceCommand(MoveSet newMoveSet, Stance newStance)
	{
		this.moveSet = newMoveSet;
		this.stance = newStance;
	}

	// Token: 0x060008C2 RID: 2242 RVA: 0x0002ADC2 File Offset: 0x00028FC2
	public void Execute()
	{
		if (this.moveSet.stanceList == null)
		{
			this.moveSet.stanceList = new List<Stance>();
		}
		this.moveSet.stanceList.Add(this.stance);
		this.UpdateVisuals();
	}

	// Token: 0x060008C3 RID: 2243 RVA: 0x0002ADFD File Offset: 0x00028FFD
	public void Undo()
	{
		this.moveSet.stanceList.Remove(this.stance);
		this.UpdateVisuals();
	}

	// Token: 0x060008C4 RID: 2244 RVA: 0x0002AE1C File Offset: 0x0002901C
	private void UpdateVisuals()
	{
		if (MoveSetEditor.singleton.CheckCurrentView(null, null, false))
		{
			MoveSetEditor.singleton.UpdateMoveMenu(true);
		}
	}

	// Token: 0x04000617 RID: 1559
	private MoveSet moveSet;

	// Token: 0x04000618 RID: 1560
	private Stance stance;
}
