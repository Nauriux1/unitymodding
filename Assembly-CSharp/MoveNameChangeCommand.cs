using System;
using MoveClasses;

// Token: 0x02000106 RID: 262
public class MoveNameChangeCommand : ICommand
{
	// Token: 0x06000880 RID: 2176 RVA: 0x00029F26 File Offset: 0x00028126
	public MoveNameChangeCommand(Stance commandStance, Move commandMove, string text)
	{
		this.newText = text;
		this.oldText = commandMove.name;
		this.move = commandMove;
		this.stance = commandStance;
	}

	// Token: 0x06000881 RID: 2177 RVA: 0x00029F4F File Offset: 0x0002814F
	public void Execute()
	{
		this.move.name = this.newText;
		this.move.FilterNameForProfanity();
		this.UpdateVisuals();
	}

	// Token: 0x06000882 RID: 2178 RVA: 0x00029F73 File Offset: 0x00028173
	public void Undo()
	{
		this.move.name = this.oldText;
		this.UpdateVisuals();
	}

	// Token: 0x06000883 RID: 2179 RVA: 0x00029F8C File Offset: 0x0002818C
	private void UpdateVisuals()
	{
		if (MoveSetEditor.singleton.CheckCurrentView(this.stance, this.move, false))
		{
			MoveSetEditor.singleton.selectedMoveNameEditor.SetTextWithoutNotify(this.move.name);
		}
	}

	// Token: 0x040005DE RID: 1502
	private Stance stance;

	// Token: 0x040005DF RID: 1503
	private Move move;

	// Token: 0x040005E0 RID: 1504
	private string newText;

	// Token: 0x040005E1 RID: 1505
	private string oldText;
}
