using System;
using MoveClasses;

// Token: 0x02000119 RID: 281
public class StanceNameChangeCommand : ICommand
{
	// Token: 0x060008D1 RID: 2257 RVA: 0x0002B091 File Offset: 0x00029291
	public StanceNameChangeCommand(MoveSet newMoveSet, Stance newStance, string text)
	{
		this.newText = text;
		this.stance = newStance;
		this.oldText = newStance.name;
		this.moveSet = newMoveSet;
	}

	// Token: 0x060008D2 RID: 2258 RVA: 0x0002B0BA File Offset: 0x000292BA
	public void Execute()
	{
		this.stance.name = this.newText;
		this.stance.FilterNameForProfanity();
		this.UpdateVisuals();
	}

	// Token: 0x060008D3 RID: 2259 RVA: 0x0002B0DE File Offset: 0x000292DE
	public void Undo()
	{
		this.stance.name = this.oldText;
		this.UpdateVisuals();
	}

	// Token: 0x060008D4 RID: 2260 RVA: 0x0002AE1C File Offset: 0x0002901C
	private void UpdateVisuals()
	{
		if (MoveSetEditor.singleton.CheckCurrentView(null, null, false))
		{
			MoveSetEditor.singleton.UpdateMoveMenu(true);
		}
	}

	// Token: 0x04000622 RID: 1570
	private MoveSet moveSet;

	// Token: 0x04000623 RID: 1571
	private Stance stance;

	// Token: 0x04000624 RID: 1572
	private string newText;

	// Token: 0x04000625 RID: 1573
	private string oldText;
}
