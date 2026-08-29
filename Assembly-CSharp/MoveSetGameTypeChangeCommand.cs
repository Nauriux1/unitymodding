using System;
using MoveClasses;

// Token: 0x0200011B RID: 283
public class MoveSetGameTypeChangeCommand : ICommand
{
	// Token: 0x060008D9 RID: 2265 RVA: 0x0002B181 File Offset: 0x00029381
	public MoveSetGameTypeChangeCommand(MoveSet newMoveSet, GameTypes value)
	{
		this.newValue = value;
		this.oldValue = newMoveSet.gameType;
		this.moveSet = newMoveSet;
	}

	// Token: 0x060008DA RID: 2266 RVA: 0x0002B1A3 File Offset: 0x000293A3
	public void Execute()
	{
		this.moveSet.gameType = this.newValue;
		IGameSettingsManager.singleton.GameType = this.newValue;
		this.UpdateVisuals();
	}

	// Token: 0x060008DB RID: 2267 RVA: 0x0002B1CC File Offset: 0x000293CC
	public void Undo()
	{
		this.moveSet.gameType = this.oldValue;
		IGameSettingsManager.singleton.GameType = this.oldValue;
		this.UpdateVisuals();
	}

	// Token: 0x060008DC RID: 2268 RVA: 0x0002B16B File Offset: 0x0002936B
	private void UpdateVisuals()
	{
		MoveSetEditor.singleton.UpdateGeneralInputDisplays();
		MoveSetEditor.singleton.RefreshStaminaManager();
	}

	// Token: 0x04000629 RID: 1577
	private MoveSet moveSet;

	// Token: 0x0400062A RID: 1578
	private GameTypes newValue;

	// Token: 0x0400062B RID: 1579
	private GameTypes oldValue;
}
