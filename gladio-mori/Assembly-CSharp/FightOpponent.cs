using System;
using System.Collections.Generic;
using MoveClasses;
using UnityEngine;

// Token: 0x0200016B RID: 363
[CreateAssetMenu(fileName = "FightOpponent", menuName = "Singleplayer/FightOpponent", order = 1)]
public class FightOpponent : ScriptableObject
{
	// Token: 0x17000178 RID: 376
	// (get) Token: 0x06000BA5 RID: 2981 RVA: 0x00038262 File Offset: 0x00036462
	public string translatedName
	{
		get
		{
			if (!string.IsNullOrEmpty(this.enemyName))
			{
				return LocalizationHelpers.LocalizedText(this.enemyName, Array.Empty<object>());
			}
			return "";
		}
	}

	// Token: 0x04000831 RID: 2097
	public string enemyName;

	// Token: 0x04000832 RID: 2098
	public string defaultMovesetName;

	// Token: 0x04000833 RID: 2099
	public List<EquippedEquipment> equippedEquipment = new List<EquippedEquipment>();

	// Token: 0x04000834 RID: 2100
	public CustomAiObject customAi;

	// Token: 0x04000835 RID: 2101
	public Texture2D customTexture;
}
