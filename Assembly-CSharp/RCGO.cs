using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using ProtoBuf;
using UnityEngine;

// Token: 0x020000D6 RID: 214
[ProtoContract]
[Serializable]
public class RCGO
{
	// Token: 0x17000135 RID: 309
	// (get) Token: 0x0600078F RID: 1935 RVA: 0x00025D43 File Offset: 0x00023F43
	// (set) Token: 0x06000790 RID: 1936 RVA: 0x00025D4B File Offset: 0x00023F4B
	[ProtoMember(1)]
	public string name { get; set; }

	// Token: 0x17000136 RID: 310
	// (get) Token: 0x06000791 RID: 1937 RVA: 0x00025D54 File Offset: 0x00023F54
	// (set) Token: 0x06000792 RID: 1938 RVA: 0x00025D5C File Offset: 0x00023F5C
	[JsonIgnore]
	public GameObject gameObject { get; set; }

	// Token: 0x17000137 RID: 311
	// (get) Token: 0x06000793 RID: 1939 RVA: 0x00025D65 File Offset: 0x00023F65
	// (set) Token: 0x06000794 RID: 1940 RVA: 0x00025D6D File Offset: 0x00023F6D
	[ProtoMember(2)]
	public List<RT> recordedTicks { get; set; } = new List<RT>(ReplayManager.maxRecordingTicks);

	// Token: 0x17000138 RID: 312
	// (get) Token: 0x06000795 RID: 1941 RVA: 0x00025D76 File Offset: 0x00023F76
	// (set) Token: 0x06000796 RID: 1942 RVA: 0x00025D7E File Offset: 0x00023F7E
	[ProtoMember(3)]
	[DefaultValue(TickMode.LocalRotation)]
	public TickMode tickMode { get; set; }

	// Token: 0x17000139 RID: 313
	// (get) Token: 0x06000797 RID: 1943 RVA: 0x00025D87 File Offset: 0x00023F87
	// (set) Token: 0x06000798 RID: 1944 RVA: 0x00025D8F File Offset: 0x00023F8F
	[ProtoMember(4)]
	[DefaultValue(null)]
	public string parentName { get; set; }

	// Token: 0x1700013A RID: 314
	// (get) Token: 0x06000799 RID: 1945 RVA: 0x00025D98 File Offset: 0x00023F98
	// (set) Token: 0x0600079A RID: 1946 RVA: 0x00025DA0 File Offset: 0x00023FA0
	[JsonIgnore]
	public Quaternion lastRotation { get; set; }

	// Token: 0x1700013B RID: 315
	// (get) Token: 0x0600079B RID: 1947 RVA: 0x00025DA9 File Offset: 0x00023FA9
	// (set) Token: 0x0600079C RID: 1948 RVA: 0x00025DB1 File Offset: 0x00023FB1
	[JsonIgnore]
	public Vector3 lastPosition { get; set; }
}
