using System;
using Newtonsoft.Json;
using ProtoBuf;
using UnityEngine;

// Token: 0x020000D2 RID: 210
[ProtoContract]
[Serializable]
public class RS
{
	// Token: 0x17000125 RID: 293
	// (get) Token: 0x06000768 RID: 1896 RVA: 0x00025788 File Offset: 0x00023988
	// (set) Token: 0x06000769 RID: 1897 RVA: 0x00025790 File Offset: 0x00023990
	[ProtoMember(1)]
	public int tick { get; set; }

	// Token: 0x17000126 RID: 294
	// (get) Token: 0x0600076A RID: 1898 RVA: 0x00025799 File Offset: 0x00023999
	// (set) Token: 0x0600076B RID: 1899 RVA: 0x000257A1 File Offset: 0x000239A1
	[ProtoMember(2)]
	public CollisionSoundType cst { get; set; }

	// Token: 0x17000127 RID: 295
	// (get) Token: 0x0600076C RID: 1900 RVA: 0x000257AC File Offset: 0x000239AC
	// (set) Token: 0x0600076D RID: 1901 RVA: 0x000257F3 File Offset: 0x000239F3
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

	// Token: 0x17000128 RID: 296
	// (get) Token: 0x0600076E RID: 1902 RVA: 0x0002581C File Offset: 0x00023A1C
	// (set) Token: 0x0600076F RID: 1903 RVA: 0x00025824 File Offset: 0x00023A24
	[ProtoMember(4)]
	public float v { get; set; }

	// Token: 0x040004E4 RID: 1252
	[ProtoMember(3)]
	public float[] p;
}
