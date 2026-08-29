using System;
using ProtoBuf;

// Token: 0x02000142 RID: 322
[ProtoContract]
[Serializable]
public struct DamageOrigin
{
	// Token: 0x04000700 RID: 1792
	[ProtoMember(1)]
	public EnvironmentSoundType EnvironmentSoundType;
}
