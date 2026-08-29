using System;
using MoveClasses;

// Token: 0x02000107 RID: 263
public class MoveLayerChangeCommand : ICommand
{
	// Token: 0x06000884 RID: 2180 RVA: 0x00029FC1 File Offset: 0x000281C1
	public MoveLayerChangeCommand(Stance commandStance, Move commandMove, int layer)
	{
		this.newLayer = layer;
		this.oldLayer = commandMove.layer;
		this.move = commandMove;
		this.stance = commandStance;
	}

	// Token: 0x06000885 RID: 2181 RVA: 0x00029FEA File Offset: 0x000281EA
	public void Execute()
	{
		this.move.layer = this.newLayer;
		this.UpdateVisuals();
	}

	// Token: 0x06000886 RID: 2182 RVA: 0x0002A003 File Offset: 0x00028203
	public void Undo()
	{
		this.move.layer = this.oldLayer;
		this.UpdateVisuals();
	}

	// Token: 0x06000887 RID: 2183 RVA: 0x0002A01C File Offset: 0x0002821C
	private void UpdateVisuals()
	{
		if (MoveSetEditor.singleton.CheckCurrentView(this.stance, this.move, false))
		{
			MoveSetEditor.singleton.UpdateLayerShownValue();
		}
	}

	// Token: 0x040005E2 RID: 1506
	private Stance stance;

	// Token: 0x040005E3 RID: 1507
	private Move move;

	// Token: 0x040005E4 RID: 1508
	private int newLayer;

	// Token: 0x040005E5 RID: 1509
	private int oldLayer;
}
