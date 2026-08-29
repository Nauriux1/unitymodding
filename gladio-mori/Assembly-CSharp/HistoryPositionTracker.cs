using System;
using UnityEngine;

// Token: 0x02000047 RID: 71
public class HistoryPositionTracker
{
	// Token: 0x0600020F RID: 527 RVA: 0x0000BF16 File Offset: 0x0000A116
	public HistoryPositionTracker(GameObject newGameObject)
	{
		this.gameObject = newGameObject;
	}

	// Token: 0x06000210 RID: 528 RVA: 0x0000BF34 File Offset: 0x0000A134
	public void UpdateHistory()
	{
		this.equipmentHistoryItems[1] = this.equipmentHistoryItems[0];
		this.equipmentHistoryItems[0] = new HistoryPositionItem
		{
			localToWorldMatrix = this.gameObject.transform.localToWorldMatrix,
			position = this.gameObject.transform.position
		};
	}

	// Token: 0x06000211 RID: 529 RVA: 0x0000BF9C File Offset: 0x0000A19C
	public HistoryPositionItem GetPreviousHistoryPosition()
	{
		return this.equipmentHistoryItems[1];
	}

	// Token: 0x04000160 RID: 352
	private HistoryPositionItem[] equipmentHistoryItems = new HistoryPositionItem[2];

	// Token: 0x04000161 RID: 353
	public GameObject gameObject;
}
