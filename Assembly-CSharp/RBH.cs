using System;
using Newtonsoft.Json;
using ProtoBuf;
using UnityEngine;

// Token: 0x020000D1 RID: 209
[ProtoContract]
[Serializable]
public class RBH
{
	// Token: 0x1700011F RID: 287
	// (get) Token: 0x0600075B RID: 1883 RVA: 0x000256C3 File Offset: 0x000238C3
	// (set) Token: 0x0600075C RID: 1884 RVA: 0x000256CB File Offset: 0x000238CB
	[ProtoMember(1)]
	public int tick { get; set; }

	// Token: 0x17000120 RID: 288
	// (get) Token: 0x0600075D RID: 1885 RVA: 0x000256D4 File Offset: 0x000238D4
	// (set) Token: 0x0600075E RID: 1886 RVA: 0x000256DC File Offset: 0x000238DC
	[ProtoMember(2)]
	public int id { get; set; }

	// Token: 0x17000121 RID: 289
	// (get) Token: 0x0600075F RID: 1887 RVA: 0x000256E5 File Offset: 0x000238E5
	// (set) Token: 0x06000760 RID: 1888 RVA: 0x000256ED File Offset: 0x000238ED
	[ProtoMember(3)]
	public float dmg { get; set; }

	// Token: 0x17000122 RID: 290
	// (get) Token: 0x06000761 RID: 1889 RVA: 0x000256F6 File Offset: 0x000238F6
	// (set) Token: 0x06000762 RID: 1890 RVA: 0x000256FE File Offset: 0x000238FE
	[ProtoMember(5)]
	public float bDmg { get; set; }

	// Token: 0x17000123 RID: 291
	// (get) Token: 0x06000763 RID: 1891 RVA: 0x00025707 File Offset: 0x00023907
	// (set) Token: 0x06000764 RID: 1892 RVA: 0x0002570F File Offset: 0x0002390F
	[ProtoMember(6)]
	public float v { get; set; }

	// Token: 0x17000124 RID: 292
	// (get) Token: 0x06000765 RID: 1893 RVA: 0x00025718 File Offset: 0x00023918
	// (set) Token: 0x06000766 RID: 1894 RVA: 0x0002575F File Offset: 0x0002395F
	[JsonIgnore]
	public Vector3 position
	{
		get
		{
			if (this.p != null && this.p.Length == 3)
			{
				return new Vector3(this.p[0], this.p[1], this.p[2]);
			}
			return default(Vector3);
		}
		set
		{
			this.p = new float[]
			{
				value.x,
				value.y,
				value.z
			};
		}
	}

	// Token: 0x040004DF RID: 1247
	[ProtoMember(4)]
	public float[] p;
}
