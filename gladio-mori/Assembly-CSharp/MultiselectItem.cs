using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000207 RID: 519
public class MultiselectItem : MonoBehaviour
{
	// Token: 0x06000FF6 RID: 4086 RVA: 0x000536E5 File Offset: 0x000518E5
	public void SetText(string text)
	{
		this.titleText.text = text;
	}

	// Token: 0x04000B6F RID: 2927
	public Text titleText;

	// Token: 0x04000B70 RID: 2928
	public Toggle checkBox;

	// Token: 0x04000B71 RID: 2929
	public object value;
}
