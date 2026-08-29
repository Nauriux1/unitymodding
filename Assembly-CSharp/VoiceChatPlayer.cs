using System;
using Dissonance;
using Mirror;

// Token: 0x02000098 RID: 152
public class VoiceChatPlayer
{
	// Token: 0x06000551 RID: 1361 RVA: 0x000193B0 File Offset: 0x000175B0
	public void UpdatePlayerName()
	{
		if (this.multiplayerRoomPlayer != null && this.voiceChatPlayerPanel != null)
		{
			this.voiceChatPlayerPanel.SetPlayerName(this.multiplayerRoomPlayer.playerName);
		}
	}

	// Token: 0x0400032F RID: 815
	public VoicePlayerState voicePlayerState;

	// Token: 0x04000330 RID: 816
	public VoiceChatPlayerPanel voiceChatPlayerPanel;

	// Token: 0x04000331 RID: 817
	public MultiplayerRoomPlayer multiplayerRoomPlayer;
}
