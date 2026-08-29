using System;
using System.Collections.Generic;
using Dissonance;
using Mirror;
using UnityEngine;

// Token: 0x02000097 RID: 151
public class VoiceChatManager : MonoBehaviour
{
	// Token: 0x06000540 RID: 1344 RVA: 0x00018FF6 File Offset: 0x000171F6
	private void Awake()
	{
		this.InitializeVoiceChatManager();
	}

	// Token: 0x06000541 RID: 1345 RVA: 0x00018FFE File Offset: 0x000171FE
	public void InitializeVoiceChatManager()
	{
		if (VoiceChatManager.singleton != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		VoiceChatManager.singleton = this;
		this.SubscribeToEvents();
	}

	// Token: 0x06000542 RID: 1346 RVA: 0x00019025 File Offset: 0x00017225
	private void OnDestroy()
	{
		this.UnsubscribeToEvents();
	}

	// Token: 0x06000543 RID: 1347 RVA: 0x00019030 File Offset: 0x00017230
	public void SubscribeToEvents()
	{
		this.dissonanceComms.OnPlayerJoinedSession += this.HandlePlayerJoinedEvent;
		this.dissonanceComms.OnPlayerLeftSession += this.HandlePlayerLeftEvent;
		this.dissonanceComms.OnPlayerStartedSpeaking += this.HandlePlayerSpeakingStarted;
		this.dissonanceComms.OnPlayerStoppedSpeaking += this.HandlePlayerSpeakingStopped;
	}

	// Token: 0x06000544 RID: 1348 RVA: 0x0001909C File Offset: 0x0001729C
	public void UnsubscribeToEvents()
	{
		this.dissonanceComms.OnPlayerJoinedSession -= this.HandlePlayerJoinedEvent;
		this.dissonanceComms.OnPlayerLeftSession -= this.HandlePlayerLeftEvent;
		this.dissonanceComms.OnPlayerStartedSpeaking -= this.HandlePlayerSpeakingStarted;
		this.dissonanceComms.OnPlayerStoppedSpeaking -= this.HandlePlayerSpeakingStopped;
	}

	// Token: 0x06000545 RID: 1349 RVA: 0x00019105 File Offset: 0x00017305
	private void HandlePlayerJoinedEvent(VoicePlayerState player)
	{
		this.AddVoiceChatPlayer(player);
		this.PairVoiceChatPlayerAndRoomPlayer();
	}

	// Token: 0x06000546 RID: 1350 RVA: 0x00019114 File Offset: 0x00017314
	private void HandlePlayerLeftEvent(VoicePlayerState player)
	{
		VoiceChatPlayer voiceChatPlayerForVoicePlayerState = this.GetVoiceChatPlayerForVoicePlayerState(player);
		if (voiceChatPlayerForVoicePlayerState != null)
		{
			this.RemoveVoiceChatPlayer(voiceChatPlayerForVoicePlayerState);
		}
	}

	// Token: 0x06000547 RID: 1351 RVA: 0x00019134 File Offset: 0x00017334
	private void HandlePlayerSpeakingStarted(VoicePlayerState player)
	{
		if (!player.IsLocalPlayer && !player.IsLocallyMuted)
		{
			VoiceChatPlayer voiceChatPlayerForVoicePlayerState = this.GetVoiceChatPlayerForVoicePlayerState(player);
			if (voiceChatPlayerForVoicePlayerState != null)
			{
				voiceChatPlayerForVoicePlayerState.voiceChatPlayerPanel.Show();
			}
		}
	}

	// Token: 0x06000548 RID: 1352 RVA: 0x00019168 File Offset: 0x00017368
	private void HandlePlayerSpeakingStopped(VoicePlayerState player)
	{
		if (!player.IsLocalPlayer)
		{
			VoiceChatPlayer voiceChatPlayerForVoicePlayerState = this.GetVoiceChatPlayerForVoicePlayerState(player);
			if (voiceChatPlayerForVoicePlayerState != null)
			{
				voiceChatPlayerForVoicePlayerState.voiceChatPlayerPanel.Hide();
			}
		}
	}

	// Token: 0x06000549 RID: 1353 RVA: 0x00019193 File Offset: 0x00017393
	public void ActivateVoiceChat()
	{
		if (this.dissonanceComms != null && this.dissonanceComms.IsNetworkInitialized)
		{
			this.voiceBroadcastTrigger.IsMuted = false;
			this.localPlayerVoicePanel.SetActive(true);
		}
	}

	// Token: 0x0600054A RID: 1354 RVA: 0x000191C8 File Offset: 0x000173C8
	public void DeactivateVoiceChat()
	{
		this.voiceBroadcastTrigger.IsMuted = true;
		this.localPlayerVoicePanel.SetActive(false);
	}

	// Token: 0x0600054B RID: 1355 RVA: 0x000191E4 File Offset: 0x000173E4
	public VoiceChatPlayer GetVoiceChatPlayerForVoicePlayerState(VoicePlayerState voicePlayerState)
	{
		for (int i = 0; i < this.voiceChatPlayers.Count; i++)
		{
			VoiceChatPlayer voiceChatPlayer = this.voiceChatPlayers[i];
			if (voiceChatPlayer.voicePlayerState == voicePlayerState)
			{
				return voiceChatPlayer;
			}
		}
		return null;
	}

	// Token: 0x0600054C RID: 1356 RVA: 0x00019220 File Offset: 0x00017420
	private void AddVoiceChatPlayer(VoicePlayerState voicePlayerState)
	{
		VoiceChatPlayer voiceChatPlayer = new VoiceChatPlayer();
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.playerVoicePanelPrefab, this.playerVoicePanelHolder);
		voiceChatPlayer.voiceChatPlayerPanel = gameObject.GetComponent<VoiceChatPlayerPanel>();
		voiceChatPlayer.voiceChatPlayerPanel.SetVoicePlayerState(voicePlayerState);
		voiceChatPlayer.voicePlayerState = voicePlayerState;
		voiceChatPlayer.voiceChatPlayerPanel.Hide();
		this.voiceChatPlayers.Add(voiceChatPlayer);
	}

	// Token: 0x0600054D RID: 1357 RVA: 0x0001927B File Offset: 0x0001747B
	private void RemoveVoiceChatPlayer(VoiceChatPlayer voiceChatPlayer)
	{
		UnityEngine.Object.Destroy(voiceChatPlayer.voiceChatPlayerPanel.gameObject);
		this.voiceChatPlayers.Remove(voiceChatPlayer);
	}

	// Token: 0x0600054E RID: 1358 RVA: 0x0001929C File Offset: 0x0001749C
	public void PairVoiceChatPlayerAndRoomPlayer()
	{
		if (NetworkManager.singleton != null)
		{
			MultiplayerRoomManager multiplayerRoomManager = (MultiplayerRoomManager)NetworkManager.singleton;
			for (int i = 0; i < this.voiceChatPlayers.Count; i++)
			{
				VoiceChatPlayer voiceChatPlayer = this.voiceChatPlayers[i];
				if (voiceChatPlayer.multiplayerRoomPlayer == null)
				{
					for (int j = 0; j < multiplayerRoomManager.roomSlots.Count; j++)
					{
						if (multiplayerRoomManager.roomSlots[j] != null)
						{
							MultiplayerRoomPlayer multiplayerRoomPlayer = (MultiplayerRoomPlayer)multiplayerRoomManager.roomSlots[j];
							if (multiplayerRoomPlayer.VoiceChatId == voiceChatPlayer.voicePlayerState.Name)
							{
								voiceChatPlayer.multiplayerRoomPlayer = multiplayerRoomPlayer;
								voiceChatPlayer.UpdatePlayerName();
								multiplayerRoomPlayer.voiceChatPlayer = voiceChatPlayer;
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x0600054F RID: 1359 RVA: 0x00019368 File Offset: 0x00017568
	public void UpdateVoiceChatPlayerNames()
	{
		for (int i = 0; i < this.voiceChatPlayers.Count; i++)
		{
			this.voiceChatPlayers[i].UpdatePlayerName();
		}
	}

	// Token: 0x04000327 RID: 807
	public static VoiceChatManager singleton;

	// Token: 0x04000328 RID: 808
	public DissonanceComms dissonanceComms;

	// Token: 0x04000329 RID: 809
	public VoiceBroadcastTrigger voiceBroadcastTrigger;

	// Token: 0x0400032A RID: 810
	public VoiceReceiptTrigger voiceReceiptTrigger;

	// Token: 0x0400032B RID: 811
	public GameObject playerVoicePanelPrefab;

	// Token: 0x0400032C RID: 812
	public Transform playerVoicePanelHolder;

	// Token: 0x0400032D RID: 813
	public GameObject localPlayerVoicePanel;

	// Token: 0x0400032E RID: 814
	public List<VoiceChatPlayer> voiceChatPlayers = new List<VoiceChatPlayer>(8);
}
