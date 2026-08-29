using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

// Token: 0x020001D9 RID: 473
public class MultiplayerLobbyStatusManager : MonoBehaviour
{
	// Token: 0x06000E24 RID: 3620 RVA: 0x00047645 File Offset: 0x00045845
	private void Awake()
	{
		MultiplayerLobbyStatusManager.singleton = this;
		this.playerLobbyStatusPanels = UnityEngine.Object.FindObjectsOfType<PlayerLobbyStatusPanel>().ToList<PlayerLobbyStatusPanel>();
	}

	// Token: 0x06000E25 RID: 3621 RVA: 0x0004765D File Offset: 0x0004585D
	private void Start()
	{
		this.UpdatePlayerNamesAndStatuses();
	}

	// Token: 0x06000E26 RID: 3622 RVA: 0x00047668 File Offset: 0x00045868
	public void UpdatePlayerNamesAndStatuses()
	{
		List<MultiplayerRoomPlayer> source = UnityEngine.Object.FindObjectsOfType<MultiplayerRoomPlayer>().ToList<MultiplayerRoomPlayer>();
		using (List<PlayerLobbyStatusPanel>.Enumerator enumerator = this.playerLobbyStatusPanels.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PlayerLobbyStatusPanel panel = enumerator.Current;
				MultiplayerRoomPlayer multiplayerRoomPlayer = (from x in source
				where x.index == panel.index
				select x).FirstOrDefault<MultiplayerRoomPlayer>();
				if (multiplayerRoomPlayer != null && !multiplayerRoomPlayer.disconnecting)
				{
					panel.UpdatePlayerName(multiplayerRoomPlayer.playerName);
					panel.UpdateSpectatorTextVisibility(multiplayerRoomPlayer.spectator);
					panel.UpdatePlayerStatus(multiplayerRoomPlayer.playerReadyState);
					panel.UpdatePlayerPing(multiplayerRoomPlayer.ping);
					panel.multiplayerRoomPlayer = multiplayerRoomPlayer;
					panel.UpdateMuteButton();
					if (NetworkClient.activeHost)
					{
						if (multiplayerRoomPlayer.isLocalPlayer)
						{
							panel.UpdateKickAndBanButton(false);
						}
						else
						{
							panel.UpdateKickAndBanButton(true);
						}
					}
					else
					{
						panel.UpdateKickAndBanButton(false);
					}
				}
				else
				{
					panel.UpdatePlayerName(null);
					panel.UpdatePlayerStatus(false);
					panel.UpdatePlayerPing(-1);
					panel.UpdateKickAndBanButton(false);
					panel.multiplayerRoomPlayer = null;
					panel.UpdateMuteButton();
				}
			}
		}
	}

	// Token: 0x04000A22 RID: 2594
	private List<PlayerLobbyStatusPanel> playerLobbyStatusPanels = new List<PlayerLobbyStatusPanel>();

	// Token: 0x04000A23 RID: 2595
	public static MultiplayerLobbyStatusManager singleton;
}
