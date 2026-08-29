using System;
using MoveClasses;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200020D RID: 525
public class SingleMoveEditor : MonoBehaviour
{
	// Token: 0x0600100D RID: 4109 RVA: 0x0005395C File Offset: 0x00051B5C
	public void FillOptions()
	{
		this.selectActionDropdown.options.Clear();
		this.selectActionDropdown.options.Add(new Dropdown.OptionData
		{
			text = ""
		});
		foreach (object obj in Enum.GetValues(typeof(HandState)))
		{
			HandState handState = (HandState)obj;
			this.selectActionDropdown.options.Add(new OptionDataWithValue
			{
				text = handState.GetDescription(),
				stringValue = handState.ToString()
			});
		}
		this.selectActionDropdown.value = 0;
		this.selectActionDropdown.captionText.text = "";
	}

	// Token: 0x04000B7C RID: 2940
	public Text jointText;

	// Token: 0x04000B7D RID: 2941
	public Dropdown selectActionDropdown;

	// Token: 0x04000B7E RID: 2942
	public JointMove move;

	// Token: 0x04000B7F RID: 2943
	public InputField executionTime;

	// Token: 0x04000B80 RID: 2944
	public Button removeSingleMoveButton;
}
