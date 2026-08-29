using System;
using MoveClasses;

// Token: 0x0200010A RID: 266
public class MirrorMoveCommand : ICommand
{
	// Token: 0x06000890 RID: 2192 RVA: 0x0002A2F3 File Offset: 0x000284F3
	public MirrorMoveCommand(Stance commandStance, Move commandMove)
	{
		this.move = commandMove;
		this.stance = commandStance;
	}

	// Token: 0x06000891 RID: 2193 RVA: 0x0002A309 File Offset: 0x00028509
	public void Execute()
	{
		MoveSetEditor.singleton.MirrorMove(this.move);
		this.UpdateVisuals();
	}

	// Token: 0x06000892 RID: 2194 RVA: 0x0002A309 File Offset: 0x00028509
	public void Undo()
	{
		MoveSetEditor.singleton.MirrorMove(this.move);
		this.UpdateVisuals();
	}

	// Token: 0x06000893 RID: 2195 RVA: 0x0002A321 File Offset: 0x00028521
	private void UpdateVisuals()
	{
		MoveSetEditor.singleton.CheckCurrentView(this.stance, this.move, false);
	}

	// Token: 0x040005F1 RID: 1521
	private Stance stance;

	// Token: 0x040005F2 RID: 1522
	private Move move;
}
