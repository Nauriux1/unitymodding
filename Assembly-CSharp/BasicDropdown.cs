using System;
using BasicUI;
using UnityEngine;
using UnityEngine.UI;
using Utils;

// Token: 0x020001A9 RID: 425
public class BasicDropdown : MonoBehaviour
{
	// Token: 0x06000D22 RID: 3362 RVA: 0x00042A24 File Offset: 0x00040C24
	private void Awake()
	{
		this.dropdown = base.gameObject.GetComponent<Dropdown>();
		UIHelpers.SetDropdownColor(this.dropdown, UISettings.BasicButtonColor);
		if (this.dropdown != null)
		{
			UIHelpers.SetTextFont(this.dropdown.captionText, FontType.Options);
			UIHelpers.SetTextFont(this.dropdown.itemText, FontType.Options);
			this.dropdown.captionText.resizeTextForBestFit = true;
			this.dropdown.captionText.resizeTextMaxSize = this.dropdown.captionText.fontSize;
			this.dropdown.itemText.resizeTextForBestFit = true;
			this.dropdown.itemText.resizeTextMaxSize = this.dropdown.captionText.fontSize;
			if (this.dropdown.template != null)
			{
				this.scrollRect = this.dropdown.template.gameObject.GetComponent<ScrollRect>();
				if (this.scrollRect != null)
				{
					this.scrollRect.scrollSensitivity = 10f;
					this.scrollRect.movementType = ScrollRect.MovementType.Clamped;
				}
			}
			if (!this.dropdown.interactable)
			{
				Transform transform = base.gameObject.transform.Find("Arrow");
				if (transform != null)
				{
					transform.gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x04000971 RID: 2417
	private Dropdown dropdown;

	// Token: 0x04000972 RID: 2418
	private ScrollRect scrollRect;
}
