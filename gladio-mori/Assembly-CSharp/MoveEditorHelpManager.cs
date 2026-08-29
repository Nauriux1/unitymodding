using System;
using UnityEngine;
using UnityEngine.UI;
using Utils;

// Token: 0x02000122 RID: 290
public class MoveEditorHelpManager : MonoBehaviour
{
	// Token: 0x060008F2 RID: 2290 RVA: 0x0002B98B File Offset: 0x00029B8B
	private void Start()
	{
		this.openHelpButton.onClick.AddListener(delegate()
		{
			this.OpenHelp();
		});
		if (SettingsHelper.GetFirstMoveEditorLoad())
		{
			this.OpenHelp();
		}
	}

	// Token: 0x060008F3 RID: 2291 RVA: 0x0002B9B8 File Offset: 0x00029BB8
	public void OpenHelp()
	{
		if (this.existingDialog == null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.confirmDialogPrefab);
			this.existingDialog = gameObject.GetComponent<BasicConfirmDialog>();
			this.existingDialog.SetText("", LocalizationHelpers.LocalizedText("btn_help", Array.Empty<object>()), true);
			this.existingDialog.doNotDestroy = true;
		}
		else
		{
			this.existingDialog.canvas.gameObject.SetActive(true);
		}
		this.existingDialog.okButton.Select();
	}

	// Token: 0x04000641 RID: 1601
	public Button openHelpButton;

	// Token: 0x04000642 RID: 1602
	public GameObject confirmDialogPrefab;

	// Token: 0x04000643 RID: 1603
	private BasicConfirmDialog existingDialog;
}
