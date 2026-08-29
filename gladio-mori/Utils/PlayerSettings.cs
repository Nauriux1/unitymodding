using System;

namespace Utils
{
	// Token: 0x0200028C RID: 652
	public class PlayerSettings
	{
		// Token: 0x04000E2F RID: 3631
		public PlayerGenericSettings playerGenericSettings = new PlayerGenericSettings();

		// Token: 0x04000E30 RID: 3632
		public PlayerCameraSettings playerCameraSettings = new PlayerCameraSettings();

		// Token: 0x04000E31 RID: 3633
		public PlayerAudioSettings playerAudioSettings = new PlayerAudioSettings();

		// Token: 0x04000E32 RID: 3634
		public PlayerMultiplayerSettings playerMultiplayerSettings = new PlayerMultiplayerSettings();

		// Token: 0x04000E33 RID: 3635
		public DefaultMovesetSettings defaultMovesetSettings = new DefaultMovesetSettings();
	}
}
