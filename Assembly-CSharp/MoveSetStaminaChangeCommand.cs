using System;
using MoveClasses;

// Token: 0x0200011A RID: 282
public class MoveSetStaminaChangeCommand : ICommand
{
	// Token: 0x060008D5 RID: 2261 RVA: 0x0002B0F7 File Offset: 0x000292F7
	public MoveSetStaminaChangeCommand(MoveSet newMoveSet, bool value)
	{
		this.newValue = value;
		this.oldValue = newMoveSet.stamina;
		this.moveSet = newMoveSet;
	}

	// Token: 0x060008D6 RID: 2262 RVA: 0x0002B119 File Offset: 0x00029319
	public void Execute()
	{
		this.moveSet.stamina = this.newValue;
		IGameSettingsManager.singleton.UseStamina = this.newValue;
		this.UpdateVisuals();
	}

	// Token: 0x060008D7 RID: 2263 RVA: 0x0002B142 File Offset: 0x00029342
	public void Undo()
	{
		this.moveSet.stamina = this.oldValue;
		IGameSettingsManager.singleton.UseStamina = this.oldValue;
		this.UpdateVisuals();
	}

	// Token: 0x060008D8 RID: 2264 RVA: 0x0002B16B File Offset: 0x0002936B
	private void UpdateVisuals()
	{
		MoveSetEditor.singleton.UpdateGeneralInputDisplays();
		MoveSetEditor.singleton.RefreshStaminaManager();
	}

	// Token: 0x04000626 RID: 1574
	private MoveSet moveSet;

	// Token: 0x04000627 RID: 1575
	private bool newValue;

	// Token: 0x04000628 RID: 1576
	private bool oldValue;
}
