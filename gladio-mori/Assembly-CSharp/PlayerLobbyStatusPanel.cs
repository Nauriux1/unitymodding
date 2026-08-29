using System;
using BasicUI;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001DF RID: 479
public class PlayerLobbyStatusPanel : MonoBehaviour
{
	// Token: 0x06000E67 RID: 3687 RVA: 0x000496BB File Offset: 0x000478BB
	private void Awake()
	{
		if (this.muteButtonImage != null)
		{
			this.muteButtonImage.color = UISettings.BasicTextColor;
		}
	}

	// Token: 0x06000E68 RID: 3688 RVA: 0x000496DB File Offset: 0x000478DB
	public void UpdatePlayerName(string name = null)
	{
		if (name == null)
		{
			name = LocalizationHelpers.LocalizedText("txt_player_slot_empty", Array.Empty<object>());
		}
		this.PlayerName.text = name;
	}

	// Token: 0x06000E69 RID: 3689 RVA: 0x000496FD File Offset: 0x000478FD
	public void UpdatePlayerStatus(bool ready)
	{
		if (ready)
		{
			this.PlayerStatus.text = LocalizationHelpers.LocalizedText("txt_player_status_ready", Array.Empty<object>());
			return;
		}
		this.PlayerStatus.text = LocalizationHelpers.LocalizedText("txt_player_status_not_ready", Array.Empty<object>());
	}

	// Token: 0x06000E6A RID: 3690 RVA: 0x00049737 File Offset: 0x00047937
	public void UpdatePlayerPing(int ping)
	{
		if (ping >= 0)
		{
			this.PlayerPing.text = string.Format("{0}", ping);
			return;
		}
		this.PlayerPing.text = "";
	}

	// Token: 0x06000E6B RID: 3691 RVA: 0x00049769 File Offset: 0x00047969
	public void UpdateSpectatorTextVisibility(bool visible)
	{
		this.SpectatorText.gameObject.SetActive(visible);
	}

	// Token: 0x06000E6C RID: 3692 RVA: 0x0004977C File Offset: 0x0004797C
	public void UpdateKickAndBanButton(bool show = false)
	{
		this.KickPlayerButton.onClick.RemoveAllListeners();
		this.BanPlayerButton.onClick.RemoveAllListeners();
		this.KickPlayerButton.gameObject.SetActive(show);
		this.BanPlayerButton.gameObject.SetActive(show);
		if (show)
		{
			this.KickPlayerButton.onClick.AddListener(delegate()
			{
				this.KickPlayer();
			});
			this.BanPlayerButton.onClick.AddListener(delegate()
			{
				this.BanPlayer();
			});
		}
	}

	// Token: 0x06000E6D RID: 3693 RVA: 0x00049806 File Offset: 0x00047A06
	public void KickPlayer()
	{
		if (this.multiplayerRoomPlayer != null)
		{
			this.multiplayerRoomPlayer.KickPlayer();
		}
	}

	// Token: 0x06000E6E RID: 3694 RVA: 0x00049821 File Offset: 0x00047A21
	public void BanPlayer()
	{
		if (this.multiplayerRoomPlayer != null)
		{
			this.multiplayerRoomPlayer.BanPlayer();
		}
	}

	// Token: 0x06000E6F RID: 3695 RVA: 0x0004983C File Offset: 0x00047A3C
	public void UpdateMuteButton()
	{
		this.muteButton.onClick.RemoveAllListeners();
		this.muteButton.gameObject.SetActive(false);
		if (this.multiplayerRoomPlayer != null && !this.multiplayerRoomPlayer.isLocalPlayer)
		{
			this.muteButton.gameObject.SetActive(true);
			this.muteButton.onClick.AddListener(delegate()
			{
				this.MutePlayer();
			});
		}
	}

	// Token: 0x06000E70 RID: 3696 RVA: 0x000498B2 File Offset: 0x00047AB2
	public void MutePlayer()
	{
		if (this.multiplayerRoomPlayer != null)
		{
			this.multiplayerRoomPlayer.ToggleMutePlayer();
			this.UpdateMuteButtonIcon();
		}
	}

	// Token: 0x06000E71 RID: 3697 RVA: 0x000498D4 File Offset: 0x00047AD4
	public void UpdateMuteButtonIcon()
	{
		if (this.multiplayerRoomPlayer != null && this.muteButton.gameObject.activeInHierarchy)
		{
			if (this.multiplayerRoomPlayer.IsPlayerMuted())
			{
				this.muteButtonImage.sprite = this.mutedIcon;
				return;
			}
			this.muteButtonImage.sprite = this.notMutedIcon;
		}
	}

	// Token: 0x04000A57 RID: 2647
	public Text PlayerName;

	// Token: 0x04000A58 RID: 2648
	public Text PlayerStatus;

	// Token: 0x04000A59 RID: 2649
	public Text PlayerPing;

	// Token: 0x04000A5A RID: 2650
	public Text SpectatorText;

	// Token: 0x04000A5B RID: 2651
	public int index;

	// Token: 0x04000A5C RID: 2652
	public Button KickPlayerButton;

	// Token: 0x04000A5D RID: 2653
	public Button BanPlayerButton;

	// Token: 0x04000A5E RID: 2654
	public Button muteButton;

	// Token: 0x04000A5F RID: 2655
	public Image muteButtonImage;

	// Token: 0x04000A60 RID: 2656
	public Sprite notMutedIcon;

	// Token: 0x04000A61 RID: 2657
	public Sprite mutedIcon;

	// Token: 0x04000A62 RID: 2658
	public MultiplayerRoomPlayer multiplayerRoomPlayer;
}
