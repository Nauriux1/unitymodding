using System;
using MoveClasses;

// Token: 0x02000112 RID: 274
public class ChangeMoveActionCommand : ICommand
{
	// Token: 0x060008B4 RID: 2228 RVA: 0x0002AB82 File Offset: 0x00028D82
	public ChangeMoveActionCommand(Stance newStance, Move newMove, string playerInput)
	{
		this.stance = newStance;
		this.move = newMove;
		this.oldPlayerInput = this.move.playerInput;
		this.newPlayerInput = playerInput;
	}

	// Token: 0x060008B5 RID: 2229 RVA: 0x0002ABB0 File Offset: 0x00028DB0
	public void Execute()
	{
		this.move.playerInput = this.newPlayerInput;
		this.UpdateVisuals();
	}

	// Token: 0x060008B6 RID: 2230 RVA: 0x0002ABC9 File Offset: 0x00028DC9
	public void Undo()
	{
		this.move.playerInput = this.oldPlayerInput;
		this.UpdateVisuals();
	}

	// Token: 0x060008B7 RID: 2231 RVA: 0x0002ABE2 File Offset: 0x00028DE2
	private void UpdateVisuals()
	{
		if (MoveSetEditor.singleton.CheckCurrentView(this.stance, null, false))
		{
			MoveSetEditor.singleton.UpdateMoveMenu(true);
		}
	}

	// Token: 0x0400060D RID: 1549
	private Stance stance;

	// Token: 0x0400060E RID: 1550
	private Move move;

	// Token: 0x0400060F RID: 1551
	private string oldPlayerInput;

	// Token: 0x04000610 RID: 1552
	private string newPlayerInput;
}
