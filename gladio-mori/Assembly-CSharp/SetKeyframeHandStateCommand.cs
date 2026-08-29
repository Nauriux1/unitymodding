using System;
using MoveClasses;

// Token: 0x0200010D RID: 269
public class SetKeyframeHandStateCommand : ICommand
{
	// Token: 0x0600089C RID: 2204 RVA: 0x0002A5FA File Offset: 0x000287FA
	public SetKeyframeHandStateCommand(Stance commandStance, Move commandMove, JointMove newSingleMove, HandState? newHandState)
	{
		this.move = commandMove;
		this.changedSingleMove = newSingleMove;
		this.newHandState = newHandState;
		this.oldHandState = this.changedSingleMove.handState;
		this.stance = commandStance;
	}

	// Token: 0x0600089D RID: 2205 RVA: 0x0002A630 File Offset: 0x00028830
	public void Execute()
	{
		this.SetHandState(this.newHandState);
	}

	// Token: 0x0600089E RID: 2206 RVA: 0x0002A63E File Offset: 0x0002883E
	public void Undo()
	{
		this.SetHandState(this.oldHandState);
	}

	// Token: 0x0600089F RID: 2207 RVA: 0x0002A64C File Offset: 0x0002884C
	private void SetHandState(HandState? handState)
	{
		this.changedSingleMove.handState = handState;
		this.UpdateVisuals();
	}

	// Token: 0x060008A0 RID: 2208 RVA: 0x0002A660 File Offset: 0x00028860
	private void UpdateVisuals()
	{
		if (MoveSetEditor.singleton.CheckCurrentView(this.stance, this.move, false))
		{
			MoveSetEditor.singleton.UpdateTimeLineMoves();
		}
	}

	// Token: 0x040005FD RID: 1533
	private Stance stance;

	// Token: 0x040005FE RID: 1534
	private Move move;

	// Token: 0x040005FF RID: 1535
	private JointMove changedSingleMove;

	// Token: 0x04000600 RID: 1536
	private HandState? newHandState;

	// Token: 0x04000601 RID: 1537
	private HandState? oldHandState;
}
