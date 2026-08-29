using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Token: 0x02000225 RID: 549
public class KeybindOptionSingle : MonoBehaviour
{
	// Token: 0x060010AD RID: 4269 RVA: 0x0000777A File Offset: 0x0000597A
	private void Start()
	{
	}

	// Token: 0x060010AE RID: 4270 RVA: 0x0000777A File Offset: 0x0000597A
	private void Update()
	{
	}

	// Token: 0x060010AF RID: 4271 RVA: 0x0005624F File Offset: 0x0005444F
	public void SetInputAction(InputAction newInputAction, int newBindInt)
	{
		this.inputAction = newInputAction;
		this.bindInt = newBindInt;
		this.UpdateKeybindText();
	}

	// Token: 0x060010B0 RID: 4272 RVA: 0x00056265 File Offset: 0x00054465
	public void UpdateKeybindText()
	{
		this.SetKeybindText(this.inputAction.GetBindingDisplayString(this.bindInt, (InputBinding.DisplayStringOptions)0));
	}

	// Token: 0x060010B1 RID: 4273 RVA: 0x0005627F File Offset: 0x0005447F
	public void SetKeybindText(string newKeybind)
	{
		this.keybindText.text = newKeybind;
	}

	// Token: 0x04000C12 RID: 3090
	public Text keybindText;

	// Token: 0x04000C13 RID: 3091
	public Button listenKeybindButton;

	// Token: 0x04000C14 RID: 3092
	public Button deleteKeybindButton;

	// Token: 0x04000C15 RID: 3093
	public string keybindName;

	// Token: 0x04000C16 RID: 3094
	public InputAction inputAction;

	// Token: 0x04000C17 RID: 3095
	public int bindInt;
}
