using System;
using UnityEngine;
using UnityEngine.UI;
using Utils;

// Token: 0x0200012A RID: 298
public class CustomTextureButtonItem : MonoBehaviour
{
	// Token: 0x0600094F RID: 2383 RVA: 0x0002C66C File Offset: 0x0002A86C
	public void SetCustomTextureItem(CustomTextureItem item)
	{
		this.customTextureItem = item;
		if (this.customTextureItem != null)
		{
			this.rawImage.texture = this.customTextureItem.texture2D;
			this.nameText.text = this.customTextureItem.textureName;
			if (item.type == CustomTextureType.CommunityTexture)
			{
				this.creditsText.text = LocalizationHelpers.LocalizedText("txt_created_by_short", new object[]
				{
					this.customTextureItem.textureCredits
				});
				return;
			}
			this.creditsText.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000950 RID: 2384 RVA: 0x0002C6F8 File Offset: 0x0002A8F8
	public void SetStatus(bool selected)
	{
		if (selected)
		{
			UIHelpers.SetButtonColor(this.button, ButtonState.Selected, null, null);
			return;
		}
		UIHelpers.SetButtonColor(this.button, ButtonState.Basic, null, null);
	}

	// Token: 0x06000951 RID: 2385 RVA: 0x0002C71C File Offset: 0x0002A91C
	public void UpdatePreviewImage()
	{
		if (this.customTextureItem == null || this.customTextureItem.fightItem == null)
		{
			return;
		}
		this.rawImage.texture = this.customTextureItem.fightItem.previewImage;
		base.gameObject.SetActive(true);
	}

	// Token: 0x04000679 RID: 1657
	public Button button;

	// Token: 0x0400067A RID: 1658
	public CustomTextureItem customTextureItem;

	// Token: 0x0400067B RID: 1659
	public RawImage rawImage;

	// Token: 0x0400067C RID: 1660
	public Text nameText;

	// Token: 0x0400067D RID: 1661
	public Text creditsText;
}
