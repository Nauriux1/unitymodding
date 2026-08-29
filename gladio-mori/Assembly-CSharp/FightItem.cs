using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

// Token: 0x0200016A RID: 362
[CreateAssetMenu(fileName = "FightItem", menuName = "Singleplayer/FightItem", order = 1)]
public class FightItem : ScriptableObject
{
	// Token: 0x17000177 RID: 375
	// (get) Token: 0x06000BA3 RID: 2979 RVA: 0x00038210 File Offset: 0x00036410
	public string fightName
	{
		get
		{
			if (!string.IsNullOrWhiteSpace(this.fightTitle))
			{
				return LocalizationHelpers.LocalizedText(this.fightTitle, Array.Empty<object>());
			}
			if (this.fightTitleParameters != null && this.fightTitleParameters.Count > 0)
			{
				return Generic.GetLocalizedList(this.fightTitleParameters);
			}
			return "";
		}
	}

	// Token: 0x0400082C RID: 2092
	public string fightTitle;

	// Token: 0x0400082D RID: 2093
	public List<string> fightTitleParameters;

	// Token: 0x0400082E RID: 2094
	public string scene;

	// Token: 0x0400082F RID: 2095
	public List<FightOpponent> fightOpponents;

	// Token: 0x04000830 RID: 2096
	[NonSerialized]
	public Texture2D previewImage;
}
