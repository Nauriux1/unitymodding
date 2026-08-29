using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200016E RID: 366
public class SingleplayerDifficultyItem : MonoBehaviour
{
	// Token: 0x06000BA9 RID: 2985 RVA: 0x0003829A File Offset: 0x0003649A
	public void SetRawImage(Texture2D texture)
	{
		this.rawImage.texture = texture;
	}

	// Token: 0x0400083B RID: 2107
	public SinglePlayerDifficultyType singlePlayerDifficultyType;

	// Token: 0x0400083C RID: 2108
	public RawImage rawImage;

	// Token: 0x0400083D RID: 2109
	public Button button;
}
