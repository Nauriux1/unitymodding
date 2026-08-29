using System;
using BasicUI;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001E3 RID: 483
public class PlayerRowPanel : MonoBehaviour
{
	// Token: 0x06000EAF RID: 3759 RVA: 0x0004A9E0 File Offset: 0x00048BE0
	public void SetMultiplayerRoomPlayer(MultiplayerRoomPlayer newRoomPlayer)
	{
		this.multiplayerRoomPlayer = newRoomPlayer;
		if (NetworkServer.active && !newRoomPlayer.isOwned)
		{
			this.kickButton.onClick.RemoveAllListeners();
			this.banButton.onClick.RemoveAllListeners();
			this.kickButton.gameObject.SetActive(true);
			this.banButton.gameObject.SetActive(true);
			this.kickButton.onClick.AddListener(delegate()
			{
				this.KickPlayer();
			});
			this.banButton.onClick.AddListener(delegate()
			{
				this.BanPlayer();
			});
		}
		if (NetworkClient.active && !newRoomPlayer.isOwned)
		{
			this.muteButton.gameObject.SetActive(true);
			this.muteButton.onClick.AddListener(delegate()
			{
				this.MutePlayer();
			});
			this.muteButtonImage.color = UISettings.BasicTextColor;
			this.UpdateMuteButtonIcon();
		}
	}

	// Token: 0x06000EB0 RID: 3760 RVA: 0x0004AAD2 File Offset: 0x00048CD2
	public void KickPlayer()
	{
		if (this.multiplayerRoomPlayer != null)
		{
			this.multiplayerRoomPlayer.KickPlayer();
		}
	}

	// Token: 0x06000EB1 RID: 3761 RVA: 0x0004AAED File Offset: 0x00048CED
	public void BanPlayer()
	{
		if (this.multiplayerRoomPlayer != null)
		{
			this.multiplayerRoomPlayer.BanPlayer();
		}
	}

	// Token: 0x06000EB2 RID: 3762 RVA: 0x0004AB08 File Offset: 0x00048D08
	public void MutePlayer()
	{
		if (this.multiplayerRoomPlayer != null)
		{
			this.multiplayerRoomPlayer.ToggleMutePlayer();
			this.UpdateMuteButtonIcon();
		}
	}

	// Token: 0x06000EB3 RID: 3763 RVA: 0x0004AB2C File Offset: 0x00048D2C
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

	// Token: 0x06000EB4 RID: 3764 RVA: 0x0004AB89 File Offset: 0x00048D89
	private void OnDestroy()
	{
		this.kickButton.onClick.RemoveAllListeners();
		this.banButton.onClick.RemoveAllListeners();
		this.muteButton.onClick.RemoveAllListeners();
	}

	// Token: 0x04000A8A RID: 2698
	public Text playerName;

	// Token: 0x04000A8B RID: 2699
	public Text playerDeathReason;

	// Token: 0x04000A8C RID: 2700
	public Text ping;

	// Token: 0x04000A8D RID: 2701
	public Button kickButton;

	// Token: 0x04000A8E RID: 2702
	public Button banButton;

	// Token: 0x04000A8F RID: 2703
	public Button muteButton;

	// Token: 0x04000A90 RID: 2704
	public Image muteButtonImage;

	// Token: 0x04000A91 RID: 2705
	public Sprite notMutedIcon;

	// Token: 0x04000A92 RID: 2706
	public Sprite mutedIcon;

	// Token: 0x04000A93 RID: 2707
	private MultiplayerRoomPlayer multiplayerRoomPlayer;
}
