using System;
using System.ComponentModel;

namespace MoveClasses
{
	// Token: 0x020002B6 RID: 694
	public enum DeathReason
	{
		// Token: 0x04000F29 RID: 3881
		[Description("")]
		Unknown,
		// Token: 0x04000F2A RID: 3882
		[LocalizedDescription("txt_deathreason_brain")]
		Brain,
		// Token: 0x04000F2B RID: 3883
		[LocalizedDescription("txt_deathreason_heart")]
		Heart,
		// Token: 0x04000F2C RID: 3884
		[LocalizedDescription("txt_deathreason_blood_loss")]
		Bleedout,
		// Token: 0x04000F2D RID: 3885
		[LocalizedDescription("txt_deathreason_spine")]
		Spine,
		// Token: 0x04000F2E RID: 3886
		[LocalizedDescription("txt_deathreason_lung")]
		Lung,
		// Token: 0x04000F2F RID: 3887
		[LocalizedDescription("txt_deathreason_aorta")]
		Aorta,
		// Token: 0x04000F30 RID: 3888
		[LocalizedDescription("txt_deathreason_liver")]
		Liver,
		// Token: 0x04000F31 RID: 3889
		[LocalizedDescription("txt_deathreason_kidney")]
		Kidney,
		// Token: 0x04000F32 RID: 3890
		[LocalizedDescription("txt_deathreason_fall")]
		Fall
	}
}
