using System;
using MoveClasses;

// Token: 0x02000110 RID: 272
public class MirrorStanceCommand : ICommand
{
	// Token: 0x060008AC RID: 2220 RVA: 0x0002A9C4 File Offset: 0x00028BC4
	public MirrorStanceCommand(Stance newStance)
	{
		this.stance = newStance;
	}

	// Token: 0x060008AD RID: 2221 RVA: 0x0002A9D4 File Offset: 0x00028BD4
	public void Execute()
	{
		if (this.stance.moveList != null && this.stance.moveList.Count > 0)
		{
			foreach (Move move in this.stance.moveList)
			{
				MoveSetEditor.singleton.MirrorMove(move);
			}
		}
		this.UpdateVisuals();
	}

	// Token: 0x060008AE RID: 2222 RVA: 0x0002AA58 File Offset: 0x00028C58
	public void Undo()
	{
		if (this.stance.moveList != null && this.stance.moveList.Count > 0)
		{
			foreach (Move move in this.stance.moveList)
			{
				MoveSetEditor.singleton.MirrorMove(move);
			}
		}
		this.UpdateVisuals();
	}

	// Token: 0x060008AF RID: 2223 RVA: 0x0002AADC File Offset: 0x00028CDC
	private void UpdateVisuals()
	{
		MoveSetEditor.singleton.CheckCurrentView(this.stance, null, false);
	}

	// Token: 0x0400060A RID: 1546
	private Stance stance;
}
