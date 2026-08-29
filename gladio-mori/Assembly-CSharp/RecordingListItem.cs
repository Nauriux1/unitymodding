using System;

// Token: 0x020000CE RID: 206
[Serializable]
public class RecordingListItem
{
	// Token: 0x17000111 RID: 273
	// (get) Token: 0x0600073C RID: 1852 RVA: 0x00025558 File Offset: 0x00023758
	// (set) Token: 0x0600073D RID: 1853 RVA: 0x00025560 File Offset: 0x00023760
	public string name { get; set; }

	// Token: 0x17000112 RID: 274
	// (get) Token: 0x0600073E RID: 1854 RVA: 0x00025569 File Offset: 0x00023769
	// (set) Token: 0x0600073F RID: 1855 RVA: 0x00025571 File Offset: 0x00023771
	public string map { get; set; }

	// Token: 0x17000113 RID: 275
	// (get) Token: 0x06000740 RID: 1856 RVA: 0x0002557A File Offset: 0x0002377A
	// (set) Token: 0x06000741 RID: 1857 RVA: 0x00025582 File Offset: 0x00023782
	public int ticks { get; set; }
}
