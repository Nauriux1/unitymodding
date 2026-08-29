using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

// Token: 0x02000058 RID: 88
public class LobbyLocalPlayer : LobbyPlayer
{
	// Token: 0x17000097 RID: 151
	// (get) Token: 0x06000266 RID: 614 RVA: 0x0000C4B8 File Offset: 0x0000A6B8
	// (set) Token: 0x06000267 RID: 615 RVA: 0x0000C4C0 File Offset: 0x0000A6C0
	public GameObject cameraGameObject { get; set; }

	// Token: 0x17000098 RID: 152
	// (get) Token: 0x06000268 RID: 616 RVA: 0x0000C4C9 File Offset: 0x0000A6C9
	// (set) Token: 0x06000269 RID: 617 RVA: 0x0000C4D1 File Offset: 0x0000A6D1
	public GameObject canvasGameObject { get; set; }

	// Token: 0x17000099 RID: 153
	// (get) Token: 0x0600026A RID: 618 RVA: 0x0000C4DA File Offset: 0x0000A6DA
	// (set) Token: 0x0600026B RID: 619 RVA: 0x0000C4E2 File Offset: 0x0000A6E2
	public Canvas canvas { get; set; }

	// Token: 0x1700009A RID: 154
	// (get) Token: 0x0600026C RID: 620 RVA: 0x0000C4EB File Offset: 0x0000A6EB
	// (set) Token: 0x0600026D RID: 621 RVA: 0x0000C4F3 File Offset: 0x0000A6F3
	public MultiplayerEventSystem multiplayerEventSystem { get; set; }

	// Token: 0x1700009B RID: 155
	// (get) Token: 0x0600026E RID: 622 RVA: 0x0000C4FC File Offset: 0x0000A6FC
	// (set) Token: 0x0600026F RID: 623 RVA: 0x0000C504 File Offset: 0x0000A704
	public PlayerCanvasController playerCanvasController { get; set; }

	// Token: 0x1700009C RID: 156
	// (get) Token: 0x06000270 RID: 624 RVA: 0x0000C50D File Offset: 0x0000A70D
	// (set) Token: 0x06000271 RID: 625 RVA: 0x0000C515 File Offset: 0x0000A715
	public PlayerInput playerInput { get; set; }

	// Token: 0x06000272 RID: 626 RVA: 0x0000C520 File Offset: 0x0000A720
	public void UnregisterPlayer(bool updatePlayerCount = true)
	{
		base.playerExists = false;
		base.device = null;
		this.playerInput.user.UnpairDevicesAndRemoveUser();
		this.playerInput.DeactivateInput();
		this.playerInput.enabled = false;
		UnityEngine.Object.Destroy(this.playerInput.gameObject);
		this.playerCanvasController.removePlayerButton.gameObject.SetActive(false);
		base.playerCanvasContoller.ShowPressAnyButtonText();
		LobbyLocalManager lobbyLocalManager = UnityEngine.Object.FindObjectOfType<LobbyLocalManager>();
		if (lobbyLocalManager != null && updatePlayerCount)
		{
			lobbyLocalManager.UpdateCurrentPlayerCount();
		}
	}

	// Token: 0x04000179 RID: 377
	public int playerNumber;
}
