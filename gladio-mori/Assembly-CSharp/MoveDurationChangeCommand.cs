using System;
using MoveClasses;

// Token: 0x0200010B RID: 267
public class MoveDurationChangeCommand : ICommand
{
	// Token: 0x06000894 RID: 2196 RVA: 0x0002A33B File Offset: 0x0002853B
	public MoveDurationChangeCommand(Stance commandStance, Move commandMove, float value)
	{
		this.newValue = value;
		this.oldValue = commandMove.duration;
		this.move = commandMove;
		this.stance = commandStance;
	}

	// Token: 0x06000895 RID: 2197 RVA: 0x0002A364 File Offset: 0x00028564
	public void Execute()
	{
		this.move.duration = this.newValue;
		this.UpdateVisuals();
	}

	// Token: 0x06000896 RID: 2198 RVA: 0x0002A37D File Offset: 0x0002857D
	public void Undo()
	{
		this.move.duration = this.oldValue;
		this.UpdateVisuals();
	}

	// Token: 0x06000897 RID: 2199 RVA: 0x0002A396 File Offset: 0x00028596
	private void UpdateVisuals()
	{
		if (MoveSetEditor.singleton.CheckCurrentView(this.stance, this.move, false))
		{
			MoveSetEditor.singleton.UpdateDurationUI();
		}
	}

	// Token: 0x040005F3 RID: 1523
	private Stance stance;

	// Token: 0x040005F4 RID: 1524
	private Move move;

	// Token: 0x040005F5 RID: 1525
	private float newValue;

	// Token: 0x040005F6 RID: 1526
	private float oldValue;
}
