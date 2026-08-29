using System;
using System.Collections.Generic;
using MoveClasses;

// Token: 0x02000108 RID: 264
public class MoveInputTypeChangeCommand : ICommand
{
	// Token: 0x06000888 RID: 2184 RVA: 0x0002A044 File Offset: 0x00028244
	public MoveInputTypeChangeCommand(Stance commandStance, Move commandMove, inputType inputType, List<JointMove> newJointMoves)
	{
		this.newInputType = inputType;
		this.oldInputType = commandMove.inputType;
		this.move = commandMove;
		this.stance = commandStance;
		this.oldPlayerInput = commandMove.playerInput;
		if (newJointMoves.Count > 0)
		{
			this.setKeyframesCommand = new SetKeyframesCommand(this.stance, this.move, newJointMoves, null, null, false);
		}
	}

	// Token: 0x06000889 RID: 2185 RVA: 0x0002A0B4 File Offset: 0x000282B4
	public void Execute()
	{
		this.move.inputType = this.newInputType;
		if (this.move.inputType == inputType.Passive || this.move.inputType == inputType.PlayAtStart)
		{
			this.move.playerInput = "";
		}
		if (this.setKeyframesCommand != null)
		{
			this.setKeyframesCommand.Execute();
		}
		this.UpdateVisuals();
	}

	// Token: 0x0600088A RID: 2186 RVA: 0x0002A118 File Offset: 0x00028318
	public void Undo()
	{
		if (this.move.inputType == inputType.Passive || this.move.inputType == inputType.PlayAtStart)
		{
			this.move.playerInput = this.oldPlayerInput;
		}
		this.move.inputType = this.oldInputType;
		if (this.setKeyframesCommand != null)
		{
			this.setKeyframesCommand.Undo();
		}
		this.UpdateVisuals();
	}

	// Token: 0x0600088B RID: 2187 RVA: 0x0002A17C File Offset: 0x0002837C
	private void UpdateVisuals()
	{
		if (MoveSetEditor.singleton.CheckCurrentView(this.stance, this.move.stanceChange ? null : this.move, false))
		{
			if (this.move.stanceChange)
			{
				MoveSetEditor.singleton.UpdateMoveMenu(true);
				return;
			}
			MoveSetEditor.singleton.UpdateInputTypeShownValue();
			if (this.newInputType == inputType.Passive || this.oldInputType == inputType.Passive)
			{
				MoveSetEditor.singleton.ResetStance();
				MoveSetEditor.singleton.UpdateMoveMenu(true);
				MoveSetEditor.singleton.ClearRig();
			}
		}
	}

	// Token: 0x040005E6 RID: 1510
	private Stance stance;

	// Token: 0x040005E7 RID: 1511
	private Move move;

	// Token: 0x040005E8 RID: 1512
	private inputType newInputType;

	// Token: 0x040005E9 RID: 1513
	private inputType oldInputType;

	// Token: 0x040005EA RID: 1514
	private SetKeyframesCommand setKeyframesCommand;

	// Token: 0x040005EB RID: 1515
	private string oldPlayerInput;
}
