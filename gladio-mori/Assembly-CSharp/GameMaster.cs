using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using MoveClasses;
using UnityEngine;

// Token: 0x02000077 RID: 119
public class GameMaster : MonoBehaviour
{
	// Token: 0x06000339 RID: 825 RVA: 0x0001108F File Offset: 0x0000F28F
	private void Start()
	{
		this.gameMenu = UnityEngine.Object.FindObjectOfType<GameMenu>();
		this.registeredPlayers = new List<PlayerGameStateInfo>();
		GameMaster.singleton = this;
		this.SetTimeScaleFromSettings();
	}

	// Token: 0x0600033A RID: 826 RVA: 0x000110B4 File Offset: 0x0000F2B4
	private void SetTimeScaleFromSettings()
	{
		float timeScale = 1f;
		if (IGameSettingsManager.singleton != null && IGameSettingsManager.singleton.TimeScaleMin > 0f)
		{
			timeScale = IGameSettingsManager.singleton.TimeScaleMin;
		}
		this.SetTimeScale(timeScale);
	}

	// Token: 0x0600033B RID: 827 RVA: 0x000110F1 File Offset: 0x0000F2F1
	private void SetNormalTimeScale()
	{
		this.SetTimeScale(1f);
	}

	// Token: 0x0600033C RID: 828 RVA: 0x000110FE File Offset: 0x0000F2FE
	private void SetTimeScale(float newTimeScale)
	{
		this.currentGameSpeed = newTimeScale;
		GeneralManager.singleton.SetTimeScale(newTimeScale);
		if (this.multiplayerGameMaster != null)
		{
			this.multiplayerGameMaster.NetworktimeScale = newTimeScale;
		}
	}

	// Token: 0x0600033D RID: 829 RVA: 0x0001112C File Offset: 0x0000F32C
	public void SetPlayOrPause(bool play)
	{
		if (!NetworkClient.active)
		{
			if (play || this.gameIsOver)
			{
				Time.timeScale = this.currentGameSpeed;
				return;
			}
			Time.timeScale = 0f;
		}
	}

	// Token: 0x0600033E RID: 830 RVA: 0x00011156 File Offset: 0x0000F356
	private void OnDestroy()
	{
		this.SetNormalTimeScale();
	}

	// Token: 0x0600033F RID: 831 RVA: 0x0001115E File Offset: 0x0000F35E
	public void RegisterPlayer(PlayerHealth registerPlayer)
	{
		this.registeredPlayers.Add(new PlayerGameStateInfo
		{
			player = registerPlayer
		});
		this.alivePlayers++;
	}

	// Token: 0x06000340 RID: 832 RVA: 0x00011188 File Offset: 0x0000F388
	public void InformPlayerDeath(PlayerHealth player, DeathReason newDeathReason = DeathReason.Unknown, PlayerHealth newKiller = null)
	{
		PlayerGameStateInfo playerGameStateInfo = (from x in this.registeredPlayers
		where x.player == player
		select x).FirstOrDefault<PlayerGameStateInfo>();
		if (playerGameStateInfo == null)
		{
			return;
		}
		this.alivePlayers--;
		playerGameStateInfo.deathTime = new float?(Time.time);
		playerGameStateInfo.deathReason = newDeathReason;
		playerGameStateInfo.killer = newKiller;
		if (playerGameStateInfo.player.multiplayerInputManager != null && playerGameStateInfo.player.multiplayerInputManager.multiplayerRoomPlayer != null)
		{
			playerGameStateInfo.player.multiplayerInputManager.multiplayerRoomPlayer.NetworkdeathTime = new float?(Time.time);
			playerGameStateInfo.player.multiplayerInputManager.multiplayerRoomPlayer.NetworkplayerDeathReason = newDeathReason;
		}
		if (this.alivePlayers > 1)
		{
			if ((from x in this.registeredPlayers
			where !x.player.ai && x.player.alive
			select x).Count<PlayerGameStateInfo>() != 0)
			{
				return;
			}
		}
		base.StartCoroutine(this.GameOverTimer());
	}

	// Token: 0x06000341 RID: 833 RVA: 0x00011290 File Offset: 0x0000F490
	private IEnumerator GameOverTimer()
	{
		yield return new WaitForSecondsRealtime(5f);
		this.GameOver(false);
		yield break;
	}

	// Token: 0x06000342 RID: 834 RVA: 0x000112A0 File Offset: 0x0000F4A0
	public void GameOver(bool forceLoss = false)
	{
		if (this.gameIsOver || this.testScene)
		{
			return;
		}
		this.gameIsOver = true;
		if (ReplayManager.singleton != null)
		{
			ReplayManager.singleton.PauseRecording();
		}
		WinScreenInfo winScreenInfo = new WinScreenInfo();
		PlayerGameStateInfo playerGameStateInfo = (from x in this.registeredPlayers
		where x.player.alive
		select x).FirstOrDefault<PlayerGameStateInfo>();
		if (forceLoss)
		{
			winScreenInfo.gameEndResultType = GameEndResultType.Loss;
		}
		else if (playerGameStateInfo != null)
		{
			string winningPlayerName = string.Format("Player {0}", playerGameStateInfo.player.playerNum);
			if (!string.IsNullOrEmpty(playerGameStateInfo.player.playerName))
			{
				winningPlayerName = playerGameStateInfo.player.playerName;
			}
			winScreenInfo.gameEndResultType = GameEndResultType.Win;
			winScreenInfo.winningPlayerName = winningPlayerName;
		}
		else
		{
			winScreenInfo.gameEndResultType = GameEndResultType.Draw;
		}
		winScreenInfo.playerList = new List<WinScreenPlayerInfo>();
		foreach (PlayerGameStateInfo playerGameStateInfo2 in this.registeredPlayers)
		{
			winScreenInfo.playerList.Add(new WinScreenPlayerInfo
			{
				deathTime = playerGameStateInfo2.deathTime,
				playerName = playerGameStateInfo2.player.playerName,
				deathReason = playerGameStateInfo2.deathReason
			});
		}
		if (SingleplayerManager.singleton != null)
		{
			bool win = false;
			if (!forceLoss && playerGameStateInfo != null && !playerGameStateInfo.player.ai)
			{
				win = true;
			}
			SingleplayerManager.singleton.HandleFightResult(winScreenInfo.gameEndResultType, win);
		}
		if (this.multiplayerGameMaster != null)
		{
			Debug.Log("GameMaster:GameOver");
			this.multiplayerGameMaster.GameOver(winScreenInfo);
			return;
		}
		if (this.gameMenu != null)
		{
			this.gameMenu.ShowWinScreenInfo(winScreenInfo);
		}
	}

	// Token: 0x06000343 RID: 835 RVA: 0x00011470 File Offset: 0x0000F670
	public void KillPlayers()
	{
		foreach (PlayerGameStateInfo playerGameStateInfo in this.registeredPlayers)
		{
			if (playerGameStateInfo.player != null && !playerGameStateInfo.player.ai)
			{
				playerGameStateInfo.player.Die(DeathReason.Heart);
			}
		}
	}

	// Token: 0x0400023F RID: 575
	public List<PlayerGameStateInfo> registeredPlayers = new List<PlayerGameStateInfo>();

	// Token: 0x04000240 RID: 576
	public int alivePlayers;

	// Token: 0x04000241 RID: 577
	public GameMenu gameMenu;

	// Token: 0x04000242 RID: 578
	public MultiplayerGameMaster multiplayerGameMaster;

	// Token: 0x04000243 RID: 579
	public bool testScene;

	// Token: 0x04000244 RID: 580
	public static GameMaster singleton;

	// Token: 0x04000245 RID: 581
	private float currentGameSpeed = 1f;

	// Token: 0x04000246 RID: 582
	public bool gameIsOver;
}
