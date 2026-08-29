using System;
using MoveClasses;

// Token: 0x02000117 RID: 279
public class DefaultStanceChangeCommand : ICommand
{
	// Token: 0x060008C9 RID: 2249 RVA: 0x0002AEA8 File Offset: 0x000290A8
	public DefaultStanceChangeCommand(MoveSet newMoveSet, Stance newStance, bool newValue)
	{
		this.moveSet = newMoveSet;
		this.stance = newStance;
		this.value = newValue;
		if (this.value)
		{
			foreach (Stance stance in this.moveSet.stanceList)
			{
				if (stance.isDefault)
				{
					this.oldDefaultStance = stance;
					break;
				}
			}
		}
	}

	// Token: 0x060008CA RID: 2250 RVA: 0x0002AF30 File Offset: 0x00029130
	public void Execute()
	{
		if (this.value)
		{
			foreach (Stance stance in this.moveSet.stanceList)
			{
				stance.isDefault = false;
			}
		}
		this.stance.isDefault = this.value;
		this.UpdateVisuals();
	}

	// Token: 0x060008CB RID: 2251 RVA: 0x0002AFA8 File Offset: 0x000291A8
	public void Undo()
	{
		this.stance.isDefault = !this.value;
		if (this.oldDefaultStance != null)
		{
			this.oldDefaultStance.isDefault = true;
		}
		this.UpdateVisuals();
	}

	// Token: 0x060008CC RID: 2252 RVA: 0x0002AE1C File Offset: 0x0002901C
	private void UpdateVisuals()
	{
		if (MoveSetEditor.singleton.CheckCurrentView(null, null, false))
		{
			MoveSetEditor.singleton.UpdateMoveMenu(true);
		}
	}

	// Token: 0x0400061B RID: 1563
	private MoveSet moveSet;

	// Token: 0x0400061C RID: 1564
	private Stance stance;

	// Token: 0x0400061D RID: 1565
	private Stance oldDefaultStance;

	// Token: 0x0400061E RID: 1566
	private bool value;
}
