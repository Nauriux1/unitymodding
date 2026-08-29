using System;
using System.Collections.Generic;
using MoveClasses;

// Token: 0x0200010E RID: 270
public class MoveKeyframesCommand : ICommand
{
	// Token: 0x060008A1 RID: 2209 RVA: 0x0002A685 File Offset: 0x00028885
	public MoveKeyframesCommand(Stance commandStance, Move commandMove, List<JointMove> newSingleMoves, double newTime)
	{
		this.move = commandMove;
		this.changedSingleMoves = newSingleMoves;
		this.originalTime = newTime;
		this.stance = commandStance;
	}

	// Token: 0x060008A2 RID: 2210 RVA: 0x0002A6AA File Offset: 0x000288AA
	public void AddToDifference(double difference)
	{
		this.totalDifference += difference;
		this.UpdateExecutionTimes(difference);
	}

	// Token: 0x060008A3 RID: 2211 RVA: 0x0002A6C4 File Offset: 0x000288C4
	public void SetDeletedKeyframes(List<JointMove> jointMoves)
	{
		this.deletedSingleMoves = jointMoves;
		if (this.deletedSingleMoves != null && this.deletedSingleMoves.Count > 0)
		{
			foreach (JointMove item in this.deletedSingleMoves)
			{
				this.move.jointMoveList.Remove(item);
			}
			MoveSetEditor.singleton.UpdateTimeLineMoves();
			MoveSetEditor.singleton.UpdateAllSingleMoveEditors();
			MoveSetEditor.singleton.ClearRig();
		}
	}

	// Token: 0x060008A4 RID: 2212 RVA: 0x0002A760 File Offset: 0x00028960
	public void Execute()
	{
		if (this.deletedSingleMoves != null)
		{
			foreach (JointMove item in this.deletedSingleMoves)
			{
				this.move.jointMoveList.Remove(item);
			}
		}
		this.UpdateExecutionTimes(this.totalDifference);
	}

	// Token: 0x060008A5 RID: 2213 RVA: 0x0002A7D4 File Offset: 0x000289D4
	public void Undo()
	{
		if (this.deletedSingleMoves != null)
		{
			foreach (JointMove item in this.deletedSingleMoves)
			{
				this.move.jointMoveList.Add(item);
			}
		}
		this.UpdateExecutionTimes(this.totalDifference * -1.0);
	}

	// Token: 0x060008A6 RID: 2214 RVA: 0x0002A850 File Offset: 0x00028A50
	private void UpdateExecutionTimes(double difference)
	{
		foreach (JointMove jointMove in this.changedSingleMoves)
		{
			jointMove.executionTime = Math.Round(jointMove.executionTime + difference, 2);
		}
		this.UpdateVisuals();
	}

	// Token: 0x060008A7 RID: 2215 RVA: 0x0002A8B4 File Offset: 0x00028AB4
	private void UpdateVisuals()
	{
		if (MoveSetEditor.singleton.CheckCurrentView(this.stance, this.move, false))
		{
			MoveSetEditor.singleton.UpdateTimeLineMoves();
			if (this.changedSingleMoves.Count == 1 && (this.deletedSingleMoves == null || this.deletedSingleMoves.Count == 0))
			{
				MoveSetEditor.singleton.UpdateSingleMoveEditor(this.changedSingleMoves[0]);
			}
			else
			{
				MoveSetEditor.singleton.UpdateAllSingleMoveEditors();
			}
			MoveSetEditor.singleton.ClearRig();
		}
	}

	// Token: 0x04000602 RID: 1538
	private Stance stance;

	// Token: 0x04000603 RID: 1539
	private Move move;

	// Token: 0x04000604 RID: 1540
	private double originalTime;

	// Token: 0x04000605 RID: 1541
	public List<JointMove> changedSingleMoves;

	// Token: 0x04000606 RID: 1542
	private List<JointMove> deletedSingleMoves;

	// Token: 0x04000607 RID: 1543
	private double totalDifference;
}
