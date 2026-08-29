using System;
using BasicUI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000229 RID: 553
public class TutorialTaskRow : MonoBehaviour
{
	// Token: 0x060010BB RID: 4283 RVA: 0x00056309 File Offset: 0x00054509
	public void UpdateTaskText(string newText)
	{
		this.text.text = newText;
		if (this.task.done)
		{
			this.text.color = UISettings.BasicTextReadyColor;
			return;
		}
		this.text.color = UISettings.BasicTextNotReadyColor;
	}

	// Token: 0x04000C1E RID: 3102
	public Text text;

	// Token: 0x04000C1F RID: 3103
	public TutorialTask task;
}
