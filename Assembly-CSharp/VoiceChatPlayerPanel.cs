using System;
using Dissonance;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000099 RID: 153
public class VoiceChatPlayerPanel : MonoBehaviour
{
	// Token: 0x06000553 RID: 1363 RVA: 0x000193E4 File Offset: 0x000175E4
	public void SetPlayerName(string newPlayerName)
	{
		this.playerNameText.text = newPlayerName;
	}

	// Token: 0x06000554 RID: 1364 RVA: 0x000193F2 File Offset: 0x000175F2
	public void SetVoicePlayerState(VoicePlayerState newVoicePlayerState)
	{
		this.voicePlayerState = newVoicePlayerState;
	}

	// Token: 0x06000555 RID: 1365 RVA: 0x000193FB File Offset: 0x000175FB
	public void Show()
	{
		base.gameObject.SetActive(true);
	}

	// Token: 0x06000556 RID: 1366 RVA: 0x00019409 File Offset: 0x00017609
	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x04000332 RID: 818
	public Text playerNameText;

	// Token: 0x04000333 RID: 819
	public VoicePlayerState voicePlayerState;
}
