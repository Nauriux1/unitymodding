using System;
using MoveClasses;

// Token: 0x02000114 RID: 276
public class MirrorAllCommand : ICommand
{
	// Token: 0x060008BC RID: 2236 RVA: 0x0002ACC4 File Offset: 0x00028EC4
	public MirrorAllCommand(MoveSet newMoveSet)
	{
		this.moveSet = newMoveSet;
	}

	// Token: 0x060008BD RID: 2237 RVA: 0x0002ACD3 File Offset: 0x00028ED3
	public void Execute()
	{
		this.DoMirroring();
	}

	// Token: 0x060008BE RID: 2238 RVA: 0x0002ACD3 File Offset: 0x00028ED3
	public void Undo()
	{
		this.DoMirroring();
	}

	// Token: 0x060008BF RID: 2239 RVA: 0x0002ACDC File Offset: 0x00028EDC
	private void DoMirroring()
	{
		if (this.moveSet != null)
		{
			foreach (Stance stance in this.moveSet.stanceList)
			{
				if (stance.moveList != null && stance.moveList.Count > 0)
				{
					foreach (Move move in stance.moveList)
					{
						MoveSetEditor.singleton.MirrorMove(move);
					}
				}
			}
		}
		this.UpdateVisuals();
	}

	// Token: 0x060008C0 RID: 2240 RVA: 0x0002AD9C File Offset: 0x00028F9C
	private void UpdateVisuals()
	{
		MoveSetEditor.singleton.CheckCurrentView(null, null, false);
	}

	// Token: 0x04000616 RID: 1558
	private MoveSet moveSet;
}
