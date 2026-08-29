using System;
using BasicUI;
using UnityEngine;
using UnityEngine.UI;
using Utils;

// Token: 0x020001B6 RID: 438
public class BasicText : MonoBehaviour
{
	// Token: 0x06000D46 RID: 3398 RVA: 0x0004350C File Offset: 0x0004170C
	private void Awake()
	{
		this.text = base.gameObject.GetComponent<Text>();
		if (this.text != null)
		{
			UIHelpers.SetTextFont(this.text, this.fontType);
			this.text.color = UISettings.BasicTextColor;
			this.text.resizeTextForBestFit = true;
			this.text.resizeTextMinSize = 6;
			this.text.resizeTextMaxSize = this.text.fontSize;
		}
	}

	// Token: 0x04000995 RID: 2453
	private Text text;

	// Token: 0x04000996 RID: 2454
	public FontType fontType = FontType.Options;
}
