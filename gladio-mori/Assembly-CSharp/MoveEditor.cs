using System;
using MoveClasses;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001E6 RID: 486
public class MoveEditor : MonoBehaviour
{
	// Token: 0x06000EBC RID: 3772 RVA: 0x0004ABE0 File Offset: 0x00048DE0
	public void FillOptions()
	{
		this.selectInputDropdown.options.Clear();
		this.selectInputDropdown.options.Add(new Dropdown.OptionData
		{
			text = ""
		});
		foreach (object obj in Enum.GetValues(typeof(playerInputKeys)))
		{
			playerInputKeys playerInputKeys = (playerInputKeys)obj;
			this.selectInputDropdown.options.Add(new Dropdown.OptionData
			{
				text = playerInputKeys.ToString()
			});
		}
		foreach (object obj2 in Enum.GetValues(typeof(playerInputAxis)))
		{
			playerInputAxis playerInputAxis = (playerInputAxis)obj2;
			this.selectInputDropdown.options.Add(new Dropdown.OptionData
			{
				text = playerInputAxis.ToString()
			});
		}
		this.selectInputDropdown.value = 0;
		this.selectInputDropdown.captionText.text = "";
	}

	// Token: 0x04000A9B RID: 2715
	public Text title;

	// Token: 0x04000A9C RID: 2716
	public Button playButton;

	// Token: 0x04000A9D RID: 2717
	public Button editButton;

	// Token: 0x04000A9E RID: 2718
	public Button copyButton;

	// Token: 0x04000A9F RID: 2719
	public Button deleteButton;

	// Token: 0x04000AA0 RID: 2720
	public Button listenInputButton;

	// Token: 0x04000AA1 RID: 2721
	public Dropdown selectInputDropdown;

	// Token: 0x04000AA2 RID: 2722
	public Move move;
}
