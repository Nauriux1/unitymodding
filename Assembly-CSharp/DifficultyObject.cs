using System;
using UnityEngine;

// Token: 0x0200016D RID: 365
[CreateAssetMenu(fileName = "DifficultyObject", menuName = "Singleplayer/DifficultyObject", order = 2)]
public class DifficultyObject : ScriptableObject
{
	// Token: 0x04000838 RID: 2104
	public SinglePlayerDifficultyType difficulty;

	// Token: 0x04000839 RID: 2105
	public int maxPoints;

	// Token: 0x0400083A RID: 2106
	public float minTimeScale;
}
