using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using ProtoBuf;

// Token: 0x020000CD RID: 205
[ProtoContract]
[Serializable]
public class Recording
{
	// Token: 0x17000108 RID: 264
	// (get) Token: 0x0600072A RID: 1834 RVA: 0x00025474 File Offset: 0x00023674
	// (set) Token: 0x0600072B RID: 1835 RVA: 0x0002547C File Offset: 0x0002367C
	[ProtoMember(1)]
	public string name { get; set; }

	// Token: 0x17000109 RID: 265
	// (get) Token: 0x0600072C RID: 1836 RVA: 0x00025485 File Offset: 0x00023685
	// (set) Token: 0x0600072D RID: 1837 RVA: 0x0002548D File Offset: 0x0002368D
	[ProtoMember(2)]
	public string map { get; set; }

	// Token: 0x1700010A RID: 266
	// (get) Token: 0x0600072E RID: 1838 RVA: 0x00025496 File Offset: 0x00023696
	// (set) Token: 0x0600072F RID: 1839 RVA: 0x0002549E File Offset: 0x0002369E
	[ProtoMember(3)]
	public int ticks { get; set; }

	// Token: 0x1700010B RID: 267
	// (get) Token: 0x06000730 RID: 1840 RVA: 0x000254A7 File Offset: 0x000236A7
	// (set) Token: 0x06000731 RID: 1841 RVA: 0x000254AF File Offset: 0x000236AF
	[ProtoMember(4)]
	public List<RGO> recRGO { get; set; } = new List<RGO>();

	// Token: 0x1700010C RID: 268
	// (get) Token: 0x06000732 RID: 1842 RVA: 0x000254B8 File Offset: 0x000236B8
	// (set) Token: 0x06000733 RID: 1843 RVA: 0x000254C0 File Offset: 0x000236C0
	[ProtoMember(5)]
	public List<RS> recS { get; set; } = new List<RS>();

	// Token: 0x1700010D RID: 269
	// (get) Token: 0x06000734 RID: 1844 RVA: 0x000254C9 File Offset: 0x000236C9
	// (set) Token: 0x06000735 RID: 1845 RVA: 0x000254D1 File Offset: 0x000236D1
	[ProtoMember(6)]
	[DefaultValue(4)]
	public int tickRate { get; set; }

	// Token: 0x1700010E RID: 270
	// (get) Token: 0x06000736 RID: 1846 RVA: 0x000254DA File Offset: 0x000236DA
	// (set) Token: 0x06000737 RID: 1847 RVA: 0x000254E2 File Offset: 0x000236E2
	[ProtoMember(7)]
	[DefaultValue(1)]
	public float timeScale { get; set; }

	// Token: 0x1700010F RID: 271
	// (get) Token: 0x06000738 RID: 1848 RVA: 0x000254EB File Offset: 0x000236EB
	// (set) Token: 0x06000739 RID: 1849 RVA: 0x000254F3 File Offset: 0x000236F3
	[JsonIgnore]
	public long fileLength { get; set; }

	// Token: 0x17000110 RID: 272
	// (get) Token: 0x0600073A RID: 1850 RVA: 0x000254FC File Offset: 0x000236FC
	[JsonIgnore]
	public string fileSizeString
	{
		get
		{
			return Math.Round((double)((float)this.fileLength / 1024f / 1024f), 2).ToString("0.##") + " MB";
		}
	}
}
