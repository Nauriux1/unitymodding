using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000162 RID: 354
public class EnemyPreviewItem : MonoBehaviour
{
	// Token: 0x06000B59 RID: 2905 RVA: 0x00036C0D File Offset: 0x00034E0D
	public void SetFightItem(FightItem item)
	{
		this.fightItem = item;
		this.nameText.text = this.fightItem.fightName;
		this.rawImage.texture = item.previewImage;
	}

	// Token: 0x06000B5A RID: 2906 RVA: 0x00036C3D File Offset: 0x00034E3D
	public void SetPlayerInfo()
	{
		this.nameText.transform.parent.gameObject.SetActive(false);
		if (SingleplayerManager.singleton != null)
		{
			this.rawImage.texture = SingleplayerManager.singleton.playerImage;
		}
	}

	// Token: 0x040007E4 RID: 2020
	public Text nameText;

	// Token: 0x040007E5 RID: 2021
	public FightItem fightItem;

	// Token: 0x040007E6 RID: 2022
	public RectTransform rectTransform;

	// Token: 0x040007E7 RID: 2023
	public RawImage rawImage;
}
