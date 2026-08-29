using System;
using System.Collections.Generic;
using MoveClasses;
using Newtonsoft.Json;

// Token: 0x02000176 RID: 374
public class SingleplayerRun
{
	// Token: 0x06000BFB RID: 3067 RVA: 0x000394E4 File Offset: 0x000376E4
	public void ResetRounds()
	{
		this.roundWins = 0;
		this.roundLosses = 0;
	}

	// Token: 0x06000BFC RID: 3068 RVA: 0x000394F4 File Offset: 0x000376F4
	public void ResetRun()
	{
		this.fightIndex = 0;
		this.ResetRounds();
	}

	// Token: 0x04000862 RID: 2146
	public int fightIndex;

	// Token: 0x04000863 RID: 2147
	public int roundWins;

	// Token: 0x04000864 RID: 2148
	public int roundLosses;

	// Token: 0x04000865 RID: 2149
	public float timescale = 0.5f;

	// Token: 0x04000866 RID: 2150
	public SinglePlayerDifficultyType difficultyType = SinglePlayerDifficultyType.Normal;

	// Token: 0x04000867 RID: 2151
	public MoveSet moveSet;

	// Token: 0x04000868 RID: 2152
	public List<EquippedEquipment> equippedEquipment = new List<EquippedEquipment>();

	// Token: 0x04000869 RID: 2153
	[JsonIgnore]
	public bool loadedSave;
}
