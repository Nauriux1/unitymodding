using System;
using System.Collections.Generic;
using MoveClasses;

// Token: 0x02000111 RID: 273
public class DeleteMoveCommand : ICommand
{
	// Token: 0x060008B0 RID: 2224 RVA: 0x0002AAF1 File Offset: 0x00028CF1
	public DeleteMoveCommand(Stance newStance, Move newMove)
	{
		this.stance = newStance;
		this.move = newMove;
	}

	// Token: 0x060008B1 RID: 2225 RVA: 0x0002AB07 File Offset: 0x00028D07
	public void Execute()
	{
		this.stance.moveList.Remove(this.move);
		this.UpdateVisuals();
	}

	// Token: 0x060008B2 RID: 2226 RVA: 0x0002AB26 File Offset: 0x00028D26
	public void Undo()
	{
		if (this.stance.moveList == null)
		{
			this.stance.moveList = new List<Move>();
		}
		this.stance.moveList.Add(this.move);
		this.UpdateVisuals();
	}

	// Token: 0x060008B3 RID: 2227 RVA: 0x0002AB61 File Offset: 0x00028D61
	private void UpdateVisuals()
	{
		if (MoveSetEditor.singleton.CheckCurrentView(this.stance, null, false))
		{
			MoveSetEditor.singleton.UpdateMoveMenu(true);
		}
	}

	// Token: 0x0400060B RID: 1547
	private Stance stance;

	// Token: 0x0400060C RID: 1548
	private Move move;
}
