using System;
using MoveClasses;

// Token: 0x02000202 RID: 514
public class SingleMoveEditorListItem
{
	// Token: 0x170001A5 RID: 421
	// (get) Token: 0x06000FDF RID: 4063 RVA: 0x000535DC File Offset: 0x000517DC
	// (set) Token: 0x06000FE0 RID: 4064 RVA: 0x000535E4 File Offset: 0x000517E4
	public JointMove SingleMove { get; set; }

	// Token: 0x170001A6 RID: 422
	// (get) Token: 0x06000FE1 RID: 4065 RVA: 0x000535ED File Offset: 0x000517ED
	// (set) Token: 0x06000FE2 RID: 4066 RVA: 0x000535F5 File Offset: 0x000517F5
	public SingleMoveEditor SingleMoveEditor { get; set; }

	// Token: 0x06000FE3 RID: 4067 RVA: 0x00053600 File Offset: 0x00051800
	public void UpdateEditor()
	{
		if (this.SingleMoveEditor != null && this.SingleMove != null)
		{
			this.SingleMoveEditor.executionTime.SetTextWithoutNotify(this.SingleMove.executionTime.ToString());
		}
	}
}
