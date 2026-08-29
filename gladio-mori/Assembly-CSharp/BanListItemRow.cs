using System;
using UnityEngine;
using UnityEngine.UI;
using Utils;

// Token: 0x0200021F RID: 543
public class BanListItemRow : MonoBehaviour
{
	// Token: 0x06001092 RID: 4242 RVA: 0x00055B7C File Offset: 0x00053D7C
	public void SetBanItem(BanItem item)
	{
		this.addressText.text = item.address;
		this.nameText.text = item.name;
		this.typeText.text = item.type.GetDescription();
		this.banItem = item;
	}

	// Token: 0x04000BEF RID: 3055
	public Text addressText;

	// Token: 0x04000BF0 RID: 3056
	public Text nameText;

	// Token: 0x04000BF1 RID: 3057
	public Text typeText;

	// Token: 0x04000BF2 RID: 3058
	public Button removeButton;

	// Token: 0x04000BF3 RID: 3059
	public BanItem banItem;
}
