using System;
using Newtonsoft.Json;

// Token: 0x020001A0 RID: 416
[Serializable]
public class JointStrengthToolItem
{
	// Token: 0x1700018A RID: 394
	// (get) Token: 0x06000CFE RID: 3326 RVA: 0x00042013 File Offset: 0x00040213
	public float damperPercentage
	{
		get
		{
			return this.maxDamper / this.maxStrength;
		}
	}

	// Token: 0x04000948 RID: 2376
	public string jointName;

	// Token: 0x04000949 RID: 2377
	public float maxStrength;

	// Token: 0x0400094A RID: 2378
	public float maxDamper;

	// Token: 0x0400094B RID: 2379
	public float jointMaximumForceMultiplier;

	// Token: 0x0400094C RID: 2380
	[JsonIgnore]
	public JointStrengthToolListItem jointStrengthToolListItem;
}
