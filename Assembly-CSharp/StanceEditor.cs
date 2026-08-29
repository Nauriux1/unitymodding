using System;
using MoveClasses;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200020E RID: 526
public class StanceEditor : MonoBehaviour
{
	// Token: 0x0600100F RID: 4111 RVA: 0x0000777A File Offset: 0x0000597A
	private void Update()
	{
	}

	// Token: 0x04000B81 RID: 2945
	public Button editButton;

	// Token: 0x04000B82 RID: 2946
	public Button copyButton;

	// Token: 0x04000B83 RID: 2947
	public Button deleteButton;

	// Token: 0x04000B84 RID: 2948
	public InputField nameInputField;

	// Token: 0x04000B85 RID: 2949
	public Toggle defaultToggle;

	// Token: 0x04000B86 RID: 2950
	public Stance stance;
}
