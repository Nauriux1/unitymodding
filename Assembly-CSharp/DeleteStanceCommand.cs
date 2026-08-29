using System;
using System.Collections.Generic;
using MoveClasses;

// Token: 0x02000116 RID: 278
public class DeleteStanceCommand : ICommand
{
	// Token: 0x060008C5 RID: 2245 RVA: 0x0002AE38 File Offset: 0x00029038
	public DeleteStanceCommand(MoveSet newMoveSet, Stance newStance)
	{
		this.moveSet = newMoveSet;
		this.stance = newStance;
	}

	// Token: 0x060008C6 RID: 2246 RVA: 0x0002AE4E File Offset: 0x0002904E
	public void Execute()
	{
		this.moveSet.stanceList.Remove(this.stance);
		this.UpdateVisuals();
	}

	// Token: 0x060008C7 RID: 2247 RVA: 0x0002AE6D File Offset: 0x0002906D
	public void Undo()
	{
		if (this.moveSet.stanceList == null)
		{
			this.moveSet.stanceList = new List<Stance>();
		}
		this.moveSet.stanceList.Add(this.stance);
		this.UpdateVisuals();
	}

	// Token: 0x060008C8 RID: 2248 RVA: 0x0002AE1C File Offset: 0x0002901C
	private void UpdateVisuals()
	{
		if (MoveSetEditor.singleton.CheckCurrentView(null, null, false))
		{
			MoveSetEditor.singleton.UpdateMoveMenu(true);
		}
	}

	// Token: 0x04000619 RID: 1561
	private MoveSet moveSet;

	// Token: 0x0400061A RID: 1562
	private Stance stance;
}
