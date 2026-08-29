using System;
using System.Collections.Generic;
using MoveClasses;

// Token: 0x0200010F RID: 271
public class AddMoveCommand : ICommand
{
	// Token: 0x060008A8 RID: 2216 RVA: 0x0002A933 File Offset: 0x00028B33
	public AddMoveCommand(Stance newStance, Move newMove)
	{
		this.stance = newStance;
		this.move = newMove;
	}

	// Token: 0x060008A9 RID: 2217 RVA: 0x0002A949 File Offset: 0x00028B49
	public void Execute()
	{
		if (this.stance.moveList == null)
		{
			this.stance.moveList = new List<Move>();
		}
		this.stance.moveList.Add(this.move);
		this.UpdateVisuals();
	}

	// Token: 0x060008AA RID: 2218 RVA: 0x0002A984 File Offset: 0x00028B84
	public void Undo()
	{
		this.stance.moveList.Remove(this.move);
		this.UpdateVisuals();
	}

	// Token: 0x060008AB RID: 2219 RVA: 0x0002A9A3 File Offset: 0x00028BA3
	private void UpdateVisuals()
	{
		if (MoveSetEditor.singleton.CheckCurrentView(this.stance, null, false))
		{
			MoveSetEditor.singleton.UpdateMoveMenu(true);
		}
	}

	// Token: 0x04000608 RID: 1544
	private Stance stance;

	// Token: 0x04000609 RID: 1545
	private Move move;
}
