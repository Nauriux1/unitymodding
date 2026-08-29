using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000220 RID: 544
public class CommunityTextureButtonItem : MonoBehaviour
{
	// Token: 0x06001094 RID: 4244 RVA: 0x00055BD0 File Offset: 0x00053DD0
	public void SetCommunityTextureItem(CommunityTextureItem item)
	{
		this.communityTextureItem = item;
		if (this.communityTextureItem != null && this.communityTextureItem.texture2D != null)
		{
			this.rawImage.texture = this.communityTextureItem.texture2D;
			this.nameText.text = this.communityTextureItem.textureName;
			this.creditsText.text = LocalizationHelpers.LocalizedText("txt_created_by_short", new object[]
			{
				this.communityTextureItem.textureCredits
			});
		}
	}

	// Token: 0x04000BF4 RID: 3060
	public Button button;

	// Token: 0x04000BF5 RID: 3061
	public CommunityTextureItem communityTextureItem;

	// Token: 0x04000BF6 RID: 3062
	public RawImage rawImage;

	// Token: 0x04000BF7 RID: 3063
	public Text nameText;

	// Token: 0x04000BF8 RID: 3064
	public Text creditsText;
}
