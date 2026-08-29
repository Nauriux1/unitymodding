using System;
using ProtoBuf;
using UnityEngine;

// Token: 0x020000D0 RID: 208
[ProtoContract]
[Serializable]
public class RFC
{
	// Token: 0x17000119 RID: 281
	// (get) Token: 0x0600074E RID: 1870 RVA: 0x000255EF File Offset: 0x000237EF
	// (set) Token: 0x0600074F RID: 1871 RVA: 0x000255F7 File Offset: 0x000237F7
	[ProtoMember(1)]
	public int tick { get; set; }

	// Token: 0x1700011A RID: 282
	// (get) Token: 0x06000750 RID: 1872 RVA: 0x00025600 File Offset: 0x00023800
	// (set) Token: 0x06000751 RID: 1873 RVA: 0x00025608 File Offset: 0x00023808
	[ProtoMember(2)]
	public int id { get; set; }

	// Token: 0x1700011B RID: 283
	// (get) Token: 0x06000752 RID: 1874 RVA: 0x00025611 File Offset: 0x00023811
	// (set) Token: 0x06000753 RID: 1875 RVA: 0x00025619 File Offset: 0x00023819
	[ProtoMember(3)]
	public float pd { get; set; }

	// Token: 0x1700011C RID: 284
	// (get) Token: 0x06000754 RID: 1876 RVA: 0x00025622 File Offset: 0x00023822
	// (set) Token: 0x06000755 RID: 1877 RVA: 0x0002562A File Offset: 0x0002382A
	[ProtoMember(4)]
	public float[] pn { get; set; }

	// Token: 0x1700011D RID: 285
	// (get) Token: 0x06000756 RID: 1878 RVA: 0x00025633 File Offset: 0x00023833
	// (set) Token: 0x06000757 RID: 1879 RVA: 0x0002563B File Offset: 0x0002383B
	[ProtoMember(5)]
	public int RGOI { get; set; }

	// Token: 0x1700011E RID: 286
	// (get) Token: 0x06000758 RID: 1880 RVA: 0x00025644 File Offset: 0x00023844
	// (set) Token: 0x06000759 RID: 1881 RVA: 0x00025670 File Offset: 0x00023870
	public Plane plane
	{
		get
		{
			return new Plane(new Vector3(this.pn[0], this.pn[1], this.pn[2]), this.pd);
		}
		set
		{
			this.pd = value.distance;
			this.pn = new float[]
			{
				value.normal.x,
				value.normal.y,
				value.normal.z
			};
		}
	}
}
