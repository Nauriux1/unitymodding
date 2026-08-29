using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000100 RID: 256
public class TooltipItem : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x06000865 RID: 2149 RVA: 0x00029AF5 File Offset: 0x00027CF5
	private void Start()
	{
		this.GenerateStringFromBaseText();
	}

	// Token: 0x06000866 RID: 2150 RVA: 0x00029B00 File Offset: 0x00027D00
	private void GenerateStringFromBaseText()
	{
		if (!string.IsNullOrWhiteSpace(this.localizedStringName))
		{
			object[] array = new object[this.localizedStringNamesForBaseText.Count];
			for (int i = 0; i < this.localizedStringNamesForBaseText.Count; i++)
			{
				array[i] = LocalizationHelpers.LocalizedText(this.localizedStringNamesForBaseText[i], Array.Empty<object>());
			}
			this.text = LocalizationHelpers.LocalizedText(this.localizedStringName, array);
		}
	}

	// Token: 0x06000867 RID: 2151 RVA: 0x00029B6C File Offset: 0x00027D6C
	public void OnPointerEnter(PointerEventData eventData)
	{
		TooltipManager.Show(this.text);
	}

	// Token: 0x06000868 RID: 2152 RVA: 0x00029B79 File Offset: 0x00027D79
	public void OnPointerExit(PointerEventData eventData)
	{
		TooltipManager.Hide();
	}

	// Token: 0x040005CF RID: 1487
	public string text = "";

	// Token: 0x040005D0 RID: 1488
	public string localizedStringName;

	// Token: 0x040005D1 RID: 1489
	public List<string> localizedStringNamesForBaseText = new List<string>();
}
