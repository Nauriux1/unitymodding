using System;
using TMPro;
using UnityEngine.UI;

// Token: 0x020001B7 RID: 439
public class BasicTextConfirmDialog : BasicConfirmDialog
{
	// Token: 0x06000D48 RID: 3400 RVA: 0x00043596 File Offset: 0x00041796
	public void SetMaxLength(int maxLength)
	{
		if (this.tmpTextInputField != null)
		{
			this.tmpTextInputField.characterLimit = maxLength;
		}
		if (this.textInputField != null)
		{
			this.textInputField.characterLimit = maxLength;
		}
	}

	// Token: 0x06000D49 RID: 3401 RVA: 0x000435CC File Offset: 0x000417CC
	public void SetReadOnly()
	{
		if (this.tmpTextInputField != null)
		{
			this.tmpTextInputField.readOnly = true;
			this.tmpTextInputField.interactable = false;
		}
		if (this.textInputField != null)
		{
			this.textInputField.readOnly = true;
			this.textInputField.interactable = false;
		}
	}

	// Token: 0x06000D4A RID: 3402 RVA: 0x00043625 File Offset: 0x00041825
	public void SetValue(string value)
	{
		if (this.textInputField != null)
		{
			this.textInputField.SetTextWithoutNotify(value);
		}
		if (this.tmpTextInputField != null)
		{
			this.tmpTextInputField.SetTextWithoutNotify(value);
		}
	}

	// Token: 0x06000D4B RID: 3403 RVA: 0x0004365B File Offset: 0x0004185B
	public string GetValue()
	{
		if (this.tmpTextInputField != null)
		{
			return this.tmpTextInputField.text;
		}
		if (this.textInputField != null)
		{
			return this.textInputField.text;
		}
		return "";
	}

	// Token: 0x04000997 RID: 2455
	public InputField textInputField;

	// Token: 0x04000998 RID: 2456
	public TMP_InputField tmpTextInputField;
}
