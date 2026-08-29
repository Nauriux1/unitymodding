using System;
using System.Collections.Generic;
using MoveClasses;

// Token: 0x02000109 RID: 265
public class MoveStanceChangeTypeChangeCommand : ICommand
{
	// Token: 0x0600088C RID: 2188 RVA: 0x0002A208 File Offset: 0x00028408
	public MoveStanceChangeTypeChangeCommand(Stance commandStance, Move commandMove, stanceChangeType stanceChangeType)
	{
		this.newStanceChangeType = stanceChangeType;
		this.oldStanceChangeType = commandMove.stanceChangeType;
		this.move = commandMove;
		this.stance = commandStance;
		if (stanceChangeType == stanceChangeType.Replace && commandMove.inputType != inputType.OnClick)
		{
			this.moveInputTypeChangeCommand = new MoveInputTypeChangeCommand(this.stance, this.move, inputType.OnClick, new List<JointMove>());
		}
	}

	// Token: 0x0600088D RID: 2189 RVA: 0x0002A265 File Offset: 0x00028465
	public void Execute()
	{
		this.move.stanceChangeType = this.newStanceChangeType;
		if (this.moveInputTypeChangeCommand != null)
		{
			this.moveInputTypeChangeCommand.Execute();
		}
		this.UpdateVisuals();
	}

	// Token: 0x0600088E RID: 2190 RVA: 0x0002A291 File Offset: 0x00028491
	public void Undo()
	{
		this.move.stanceChangeType = this.oldStanceChangeType;
		if (this.moveInputTypeChangeCommand != null)
		{
			this.moveInputTypeChangeCommand.Undo();
		}
		this.UpdateVisuals();
	}

	// Token: 0x0600088F RID: 2191 RVA: 0x0002A2BD File Offset: 0x000284BD
	private void UpdateVisuals()
	{
		if (MoveSetEditor.singleton.CheckCurrentView(this.stance, this.move.stanceChange ? null : this.move, false))
		{
			MoveSetEditor.singleton.UpdateMoveMenu(true);
		}
	}

	// Token: 0x040005EC RID: 1516
	private Stance stance;

	// Token: 0x040005ED RID: 1517
	private Move move;

	// Token: 0x040005EE RID: 1518
	private stanceChangeType newStanceChangeType;

	// Token: 0x040005EF RID: 1519
	private stanceChangeType oldStanceChangeType;

	// Token: 0x040005F0 RID: 1520
	private MoveInputTypeChangeCommand moveInputTypeChangeCommand;
}
