using System;

namespace Utils
{
	// Token: 0x0200028E RID: 654
	public class DefaultMovesetSettings
	{
		// Token: 0x06001343 RID: 4931 RVA: 0x00063BC9 File Offset: 0x00061DC9
		public bool Equals(DefaultMovesetSettings compare)
		{
			return this.invertVerticalAttacks == compare.invertVerticalAttacks && this.invertHorizontalAttacks == compare.invertHorizontalAttacks && this.invertVerticalBlocks == compare.invertVerticalBlocks && this.invertHorizontalBlocks == compare.invertHorizontalBlocks;
		}

		// Token: 0x04000E41 RID: 3649
		public bool invertVerticalAttacks;

		// Token: 0x04000E42 RID: 3650
		public bool invertHorizontalAttacks;

		// Token: 0x04000E43 RID: 3651
		public bool invertVerticalBlocks;

		// Token: 0x04000E44 RID: 3652
		public bool invertHorizontalBlocks;
	}
}
