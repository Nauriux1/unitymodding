using System;
using System.IO;
using MoveClasses;

// Token: 0x02000118 RID: 280
public class MoveSetNameChangeCommand : ICommand
{
	// Token: 0x060008CD RID: 2253 RVA: 0x0002AFD8 File Offset: 0x000291D8
	public MoveSetNameChangeCommand(MoveSet newMoveSet, string text)
	{
		this.newText = text;
		this.oldText = newMoveSet.name;
		this.moveSet = newMoveSet;
	}

	// Token: 0x060008CE RID: 2254 RVA: 0x0002AFFC File Offset: 0x000291FC
	public void Execute()
	{
		if (this.newText.IndexOfAny(Path.GetInvalidFileNameChars()) < 0 && this.newText.Length <= 50)
		{
			this.moveSet.name = this.newText;
			this.moveSet.FilterNameForProfanity();
		}
		this.UpdateVisuals();
	}

	// Token: 0x060008CF RID: 2255 RVA: 0x0002B04D File Offset: 0x0002924D
	public void Undo()
	{
		this.moveSet.name = this.oldText;
		this.UpdateVisuals();
	}

	// Token: 0x060008D0 RID: 2256 RVA: 0x0002B066 File Offset: 0x00029266
	private void UpdateVisuals()
	{
		if (MoveSetEditor.singleton.CheckCurrentView(null, null, false))
		{
			MoveSetEditor.singleton.moveSetNameInputField.SetTextWithoutNotify(this.moveSet.name);
		}
	}

	// Token: 0x0400061F RID: 1567
	private MoveSet moveSet;

	// Token: 0x04000620 RID: 1568
	private string newText;

	// Token: 0x04000621 RID: 1569
	private string oldText;
}
