using System;
using MoveClasses;

// Token: 0x02000113 RID: 275
public class ChangeTargetStanceForStanceChangeCommand : ICommand
{
	// Token: 0x060008B8 RID: 2232 RVA: 0x0002AC03 File Offset: 0x00028E03
	public ChangeTargetStanceForStanceChangeCommand(Stance newStance, Move newMove, string stanceGuid)
	{
		this.stance = newStance;
		this.move = newMove;
		this.oldStanceGuid = this.move.stanceGuid;
		this.oldInputType = this.move.inputType;
		this.newStanceGuid = stanceGuid;
	}

	// Token: 0x060008B9 RID: 2233 RVA: 0x0002AC42 File Offset: 0x00028E42
	public void Execute()
	{
		this.move.stanceGuid = this.newStanceGuid;
		if (string.IsNullOrEmpty(this.move.stanceGuid))
		{
			this.move.inputType = inputType.OnClick;
		}
		this.UpdateVisuals();
	}

	// Token: 0x060008BA RID: 2234 RVA: 0x0002AC79 File Offset: 0x00028E79
	public void Undo()
	{
		this.move.stanceGuid = this.oldStanceGuid;
		this.move.inputType = this.oldInputType;
		this.UpdateVisuals();
	}

	// Token: 0x060008BB RID: 2235 RVA: 0x0002ACA3 File Offset: 0x00028EA3
	private void UpdateVisuals()
	{
		if (MoveSetEditor.singleton.CheckCurrentView(this.stance, null, false))
		{
			MoveSetEditor.singleton.UpdateMoveMenu(true);
		}
	}

	// Token: 0x04000611 RID: 1553
	private Stance stance;

	// Token: 0x04000612 RID: 1554
	private Move move;

	// Token: 0x04000613 RID: 1555
	private string oldStanceGuid;

	// Token: 0x04000614 RID: 1556
	private string newStanceGuid;

	// Token: 0x04000615 RID: 1557
	private inputType oldInputType;
}
