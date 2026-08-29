using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020001A8 RID: 424
public class BasicConfirmDialog : MonoBehaviour
{
	// Token: 0x06000D1D RID: 3357 RVA: 0x00042898 File Offset: 0x00040A98
	private void Awake()
	{
		this.cancelButton.onClick.AddListener(new UnityAction(this.onClick));
		this.okButton.onClick.AddListener(new UnityAction(this.onClick));
	}

	// Token: 0x06000D1E RID: 3358 RVA: 0x000428D4 File Offset: 0x00040AD4
	public void SetText(string text, string title = null, bool oneButton = false)
	{
		if (oneButton)
		{
			this.SetOneButton();
		}
		if (this.textField != null)
		{
			this.textField.text = text;
		}
		if (!string.IsNullOrEmpty(title))
		{
			this.titleTextField.text = title;
			return;
		}
		this.titleTextField.gameObject.SetActive(false);
	}

	// Token: 0x06000D1F RID: 3359 RVA: 0x0004292C File Offset: 0x00040B2C
	private void SetOneButton()
	{
		this.cancelButton.gameObject.SetActive(false);
		RectTransform component = this.okButton.gameObject.GetComponent<RectTransform>();
		if (component != null)
		{
			component.anchorMin = new Vector2(0.5f, 0f);
			component.anchorMax = new Vector2(0.5f, 0f);
			component.pivot = new Vector2(0.5f, 0f);
			component.anchoredPosition = new Vector2(0f, 0f);
		}
	}

	// Token: 0x06000D20 RID: 3360 RVA: 0x000429B8 File Offset: 0x00040BB8
	public void onClick()
	{
		if (this.doNotDestroy)
		{
			this.canvas.gameObject.SetActive(false);
			return;
		}
		UnityEngine.Object.Destroy(this.canvas.gameObject);
		if (GeneralManager.singleton != null)
		{
			if (GeneralManager.openConfirmDialog == this)
			{
				GeneralManager.openConfirmDialog = null;
			}
			GeneralManager.singleton.UpdateCursorState();
			GeneralManager.singleton.UpdateInputSystemState();
		}
	}

	// Token: 0x0400096B RID: 2411
	public Canvas canvas;

	// Token: 0x0400096C RID: 2412
	public Text textField;

	// Token: 0x0400096D RID: 2413
	public Text titleTextField;

	// Token: 0x0400096E RID: 2414
	public Button okButton;

	// Token: 0x0400096F RID: 2415
	public Button cancelButton;

	// Token: 0x04000970 RID: 2416
	public bool doNotDestroy;
}
