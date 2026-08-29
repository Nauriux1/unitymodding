using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Token: 0x02000224 RID: 548
public class KeybindOptionRow : MonoBehaviour
{
	// Token: 0x060010A6 RID: 4262 RVA: 0x0000777A File Offset: 0x0000597A
	private void Start()
	{
	}

	// Token: 0x060010A7 RID: 4263 RVA: 0x0000777A File Offset: 0x0000597A
	private void Update()
	{
	}

	// Token: 0x060010A8 RID: 4264 RVA: 0x00056062 File Offset: 0x00054262
	public void SetInputAction(InputAction newInputAction)
	{
		this.inputAction = newInputAction;
		this.SetNameText(this.inputAction.name);
		this.GenerateBindings();
	}

	// Token: 0x060010A9 RID: 4265 RVA: 0x00056082 File Offset: 0x00054282
	public void SetNameText(string newName)
	{
		this.keybindNameText.text = LocalizationHelpers.GetLocalizedTextForInputAction(newName, true);
		this.keybindName = newName;
	}

	// Token: 0x060010AA RID: 4266 RVA: 0x000560A0 File Offset: 0x000542A0
	public void GenerateBindings()
	{
		foreach (object obj in this.bindingsHolder.transform)
		{
			UnityEngine.Object.Destroy(((Transform)obj).gameObject);
		}
		this.keybindOptionSingles = new List<KeybindOptionSingle>();
		int num = 0;
		try
		{
			foreach (InputBinding inputBinding in this.inputAction.bindings)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.keybindOptionSinglePrefab);
				KeybindOptionSingle component = gameObject.GetComponent<KeybindOptionSingle>();
				gameObject.transform.SetParent(this.bindingsHolder.transform);
				component.SetInputAction(this.inputAction, num);
				this.keybindOptionSingles.Add(component);
				num++;
			}
		}
		catch (Exception)
		{
		}
		int num2 = 50 * (num / 2 + num % 2);
		if (num2 < 50)
		{
			num2 = 50;
		}
		num2 += 10;
		base.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(500f, (float)num2);
	}

	// Token: 0x060010AB RID: 4267 RVA: 0x000561E0 File Offset: 0x000543E0
	public void UpdateBindingDisplays(bool recreateBindings = false)
	{
		if (recreateBindings)
		{
			this.GenerateBindings();
			return;
		}
		foreach (KeybindOptionSingle keybindOptionSingle in this.keybindOptionSingles)
		{
			keybindOptionSingle.UpdateKeybindText();
		}
	}

	// Token: 0x04000C0B RID: 3083
	public Text keybindNameText;

	// Token: 0x04000C0C RID: 3084
	public Button addKeybindButton;

	// Token: 0x04000C0D RID: 3085
	public List<KeybindOptionSingle> keybindOptionSingles = new List<KeybindOptionSingle>();

	// Token: 0x04000C0E RID: 3086
	public InputAction inputAction;

	// Token: 0x04000C0F RID: 3087
	public string keybindName;

	// Token: 0x04000C10 RID: 3088
	public GameObject bindingsHolder;

	// Token: 0x04000C11 RID: 3089
	public GameObject keybindOptionSinglePrefab;
}
