using System;

namespace Utils
{
	// Token: 0x0200028D RID: 653
	public class PlayerGenericSettings
	{
		// Token: 0x04000E34 RID: 3636
		public string locale = "";

		// Token: 0x04000E35 RID: 3637
		public bool showFPS;

		// Token: 0x04000E36 RID: 3638
		public float mouseFreeLookSensitivity = 0.5f;

		// Token: 0x04000E37 RID: 3639
		public bool disableMouseTurning;

		// Token: 0x04000E38 RID: 3640
		public bool recordReplay = true;

		// Token: 0x04000E39 RID: 3641
		public float controllerFreeLookSensitivity = 150f;

		// Token: 0x04000E3A RID: 3642
		public PlayerTurnType playerTurnType;

		// Token: 0x04000E3B RID: 3643
		public bool invertCameraY;

		// Token: 0x04000E3C RID: 3644
		public bool timeScaleAffectCameraTurnSpeed;

		// Token: 0x04000E3D RID: 3645
		public bool showAttackDirection = true;

		// Token: 0x04000E3E RID: 3646
		public AllowCustomTextureOptionsType allowCustomPlayerTextures;

		// Token: 0x04000E3F RID: 3647
		public ReplayTexturesOverrideType replayTexturesOverrideType;

		// Token: 0x04000E40 RID: 3648
		public BloodColourType bloodColourType;
	}
}
