using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

// Token: 0x0200012B RID: 299
[Serializable]
public class CustomTextureItem
{
	// Token: 0x06000953 RID: 2387 RVA: 0x0002C76C File Offset: 0x0002A96C
	public CustomTextureItem CreateDeepClone()
	{
		CustomTextureItem customTextureItem = Generic.DeepClone<CustomTextureItem>(this);
		customTextureItem.texture2D = this.texture2D;
		return customTextureItem;
	}

	// Token: 0x06000954 RID: 2388 RVA: 0x0002C780 File Offset: 0x0002A980
	public void TurnIntoCopy()
	{
		this.textureName += LocalizationHelpers.LocalizedText("txt_append_to_copied_name", Array.Empty<object>());
		this.type = CustomTextureType.Default;
		this.path = "";
		this.fileName = "";
	}

	// Token: 0x06000955 RID: 2389 RVA: 0x0002C7BF File Offset: 0x0002A9BF
	public string GetFileNameFromTextureName()
	{
		return this.textureName + ".jpg";
	}

	// Token: 0x06000956 RID: 2390 RVA: 0x0002C7D1 File Offset: 0x0002A9D1
	public FightItem CreateFightItem()
	{
		this.fightItem = new FightItem
		{
			fightOpponents = new List<FightOpponent>
			{
				new FightOpponent
				{
					customTexture = this.texture2D
				}
			}
		};
		return this.fightItem;
	}

	// Token: 0x06000957 RID: 2391 RVA: 0x0002C806 File Offset: 0x0002AA06
	public void FilterNameForProfanity()
	{
		if (GeneralManager.singleton != null && !string.IsNullOrEmpty(this.textureName))
		{
			this.textureName = GeneralManager.singleton.FilterBadWords(this.textureName, true);
		}
	}

	// Token: 0x0400067E RID: 1662
	public string path;

	// Token: 0x0400067F RID: 1663
	public string fileName;

	// Token: 0x04000680 RID: 1664
	public string textureName;

	// Token: 0x04000681 RID: 1665
	public string textureCredits;

	// Token: 0x04000682 RID: 1666
	public CustomTextureType type;

	// Token: 0x04000683 RID: 1667
	[NonSerialized]
	public FightItem fightItem;

	// Token: 0x04000684 RID: 1668
	[NonSerialized]
	public Texture2D texture2D;
}
