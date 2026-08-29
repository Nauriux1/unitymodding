using System;
using System.Collections.Generic;
using MoveClasses;

// Token: 0x0200010C RID: 268
public class SetKeyframesCommand : ICommand
{
	// Token: 0x06000898 RID: 2200 RVA: 0x0002A3BB File Offset: 0x000285BB
	public SetKeyframesCommand(Stance commandStance, Move commandMove, List<JointMove> addedValues, List<JointMove> deletedValues, float? newTime = null, bool wereTempMoves = false)
	{
		this.newValue = addedValues;
		this.oldValue = deletedValues;
		this.move = commandMove;
		this.time = newTime;
		this.tempMoves = wereTempMoves;
		this.stance = commandStance;
	}

	// Token: 0x06000899 RID: 2201 RVA: 0x0002A3F0 File Offset: 0x000285F0
	public void Execute()
	{
		if (this.oldValue != null)
		{
			foreach (JointMove item in this.oldValue)
			{
				this.move.jointMoveList.Remove(item);
			}
		}
		if (this.newValue != null)
		{
			foreach (JointMove jointMove in this.newValue)
			{
				jointMove.temp = false;
				this.move.jointMoveList.Add(jointMove);
			}
		}
		MoveSetEditor.singleton.ClearTempSingleMoves();
		MoveSetEditor.singleton.UpdateUIAfterKeyframeChanges(this.newValue);
		MoveSetEditor.singleton.ClearRig();
		this.UpdateVisuals();
	}

	// Token: 0x0600089A RID: 2202 RVA: 0x0002A4DC File Offset: 0x000286DC
	public void Undo()
	{
		this.UpdateVisuals();
		if (this.newValue != null)
		{
			foreach (JointMove jointMove in this.newValue)
			{
				jointMove.temp = true;
				this.move.jointMoveList.Remove(jointMove);
			}
		}
		if (this.oldValue != null)
		{
			foreach (JointMove item in this.oldValue)
			{
				this.move.jointMoveList.Add(item);
			}
		}
		MoveSetEditor.singleton.UpdateUIAfterKeyframeChanges(this.oldValue);
		MoveSetEditor.singleton.ClearRig();
		if (this.time != null)
		{
			MoveSetEditor.singleton.SetCurrentTime(this.time.Value);
		}
	}

	// Token: 0x0600089B RID: 2203 RVA: 0x0002A5E0 File Offset: 0x000287E0
	private void UpdateVisuals()
	{
		MoveSetEditor.singleton.CheckCurrentView(this.stance, this.move, false);
	}

	// Token: 0x040005F7 RID: 1527
	private Stance stance;

	// Token: 0x040005F8 RID: 1528
	private Move move;

	// Token: 0x040005F9 RID: 1529
	private float? time;

	// Token: 0x040005FA RID: 1530
	private List<JointMove> newValue;

	// Token: 0x040005FB RID: 1531
	private List<JointMove> oldValue;

	// Token: 0x040005FC RID: 1532
	private bool tempMoves;
}
