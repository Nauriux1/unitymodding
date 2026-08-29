using System;
using System.ComponentModel;
using Newtonsoft.Json;
using ProtoBuf;
using UnityEngine;

// Token: 0x020000D7 RID: 215
[ProtoContract]
[Serializable]
public class RT
{
	// Token: 0x1700013C RID: 316
	// (get) Token: 0x0600079E RID: 1950 RVA: 0x00025DD4 File Offset: 0x00023FD4
	// (set) Token: 0x0600079F RID: 1951 RVA: 0x00025E20 File Offset: 0x00024020
	[JsonIgnore]
	public Vector3? position
	{
		get
		{
			if (this.p != null && this.p.Length == 3)
			{
				return new Vector3?(new Vector3(this.p[0], this.p[1], this.p[2]));
			}
			return null;
		}
		set
		{
			if (value != null)
			{
				this.p = new float[]
				{
					value.Value.x,
					value.Value.y,
					value.Value.z
				};
				return;
			}
			this.p = null;
		}
	}

	// Token: 0x1700013D RID: 317
	// (get) Token: 0x060007A0 RID: 1952 RVA: 0x00025E78 File Offset: 0x00024078
	// (set) Token: 0x060007A1 RID: 1953 RVA: 0x00025F04 File Offset: 0x00024104
	[JsonIgnore]
	public Quaternion? rotation
	{
		get
		{
			if (this.r != null && this.r.Length == 4)
			{
				return new Quaternion?(new Quaternion(this.r[0], this.r[1], this.r[2], this.r[3]));
			}
			if (this.r != null && this.r.Length == 3)
			{
				return new Quaternion?(Quaternion.Euler(this.r[0], this.r[1], this.r[2]));
			}
			return null;
		}
		set
		{
			if (value != null)
			{
				Vector3 eulerAngles = value.Value.eulerAngles;
				this.r = new float[]
				{
					eulerAngles.x,
					eulerAngles.y,
					eulerAngles.z
				};
				return;
			}
			this.r = null;
		}
	}

	// Token: 0x0400050D RID: 1293
	[ProtoMember(1, IsRequired = false)]
	[DefaultValue(null)]
	public float[] p;

	// Token: 0x0400050E RID: 1294
	[ProtoMember(2)]
	public float[] r;
}
