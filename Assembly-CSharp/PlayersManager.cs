using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using Utils;

// Token: 0x020000B9 RID: 185
public class PlayersManager : MonoBehaviour
{
	// Token: 0x06000666 RID: 1638 RVA: 0x0002053C File Offset: 0x0001E73C
	private void Awake()
	{
		GameObject gameObject = GameObject.Find("Main Camera");
		if (gameObject != null)
		{
			gameObject.SetActive(false);
			LowResCamera component = gameObject.GetComponent<LowResCamera>();
			if (component != null && component.cameraCanvas != null)
			{
				component.cameraCanvas.gameObject.SetActive(false);
			}
		}
		this.players = new List<GamePlayer>();
		this.findSpawnPoints();
		List<LobbyPlayer> list = (List<LobbyPlayer>)SceneManagerWithParameters.GetParameter("lobbyPlayers");
		int num = 0;
		bool flag = false;
		if ((from x in list
		where !x.ai && x.playerExists
		select x).Count<LobbyPlayer>() > 1)
		{
			flag = true;
		}
		foreach (LobbyPlayer lobbyPlayer in list)
		{
			if (lobbyPlayer.playerExists)
			{
				global::PlayerInputManager component2 = UnityEngine.Object.Instantiate<GameObject>(this.inputManager).GetComponent<global::PlayerInputManager>();
				GameObject gameObject2 = this.spawnPoints[this.players.Count];
				GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(this.playerPrefab, gameObject2.transform);
				GameObject gameObject4 = gameObject3;
				gameObject4.name += (num + 1).ToString();
				if (lobbyPlayer.device != null && flag)
				{
					InputUser inputUser = InputUser.PerformPairingWithDevice(lobbyPlayer.device, default(InputUser), InputUserPairingOptions.None);
					if (lobbyPlayer.device.name == "Keyboard" && Mouse.current != null)
					{
						InputUser.PerformPairingWithDevice(Mouse.current, inputUser, InputUserPairingOptions.None);
					}
					component2.ConnectToUser(inputUser);
				}
				GamePlayer gamePlayer = new GamePlayer
				{
					playerGameObject = gameObject3,
					playerInputManager = component2,
					playerHealth = gameObject3.GetComponent<PlayerHealth>()
				};
				gamePlayer.playerHealth.playerAnimator.SetMoveSet(lobbyPlayer.selectedMoveSet, false, false);
				gamePlayer.playerHealth.playerAnimator.SetBasicMoveSetBindings(SettingsHelper.GetDefaultMovesetSettings());
				gamePlayer.playerHealth.SetEquipment(lobbyPlayer.selectedEquipment, false);
				gamePlayer.playerHealth.playerNum = num + 1;
				if (string.IsNullOrEmpty(gamePlayer.playerHealth.playerName))
				{
					gamePlayer.playerHealth.playerName = LocalizationHelpers.LocalizedText("txt_local_playername", new object[]
					{
						gamePlayer.playerHealth.playerNum
					});
				}
				gamePlayer.playerHealth.OnlyPhysical();
				this.players.Add(gamePlayer);
				num++;
			}
			else if (lobbyPlayer.ai)
			{
				this.createAI(lobbyPlayer);
			}
		}
		int num2 = (from x in this.players
		where !x.playerHealth.ai
		select x).Count<GamePlayer>();
		num = 0;
		foreach (GamePlayer gamePlayer2 in from x in this.players
		where !x.playerHealth.ai
		select x)
		{
			GameObject gameObject5 = new GameObject("Camera" + (num + 1).ToString());
			if (num == 0)
			{
				gameObject5.tag = "MainCamera";
			}
			Camera camera = gameObject5.AddComponent<Camera>();
			CameraSmoothFollowControllable cameraSmoothFollowControllable = gameObject5.AddComponent<CameraSmoothFollowControllable>();
			gameObject5.AddComponent<LowResCamera>();
			gameObject5.AddComponent<CameraSettings>();
			gamePlayer2.playerHealth.cameraSmoothFollow = cameraSmoothFollowControllable;
			gamePlayer2.playerHealth.SetupPlayerCameraEffects(camera);
			if (num == 0)
			{
				gameObject5.AddComponent<AudioListener>();
			}
			if (gamePlayer2.playerHealth != null)
			{
				cameraSmoothFollowControllable.SetTarget(gamePlayer2.playerHealth.cameraPoint, gamePlayer2.playerHealth.cameraPositionPoint);
			}
			gamePlayer2.camera = camera;
			Rect rect = default(Rect);
			rect.height = 1f;
			rect.width = 1f;
			rect.x = 0f;
			rect.y = 0f;
			if (num2 > 1)
			{
				rect.width = 0.5f;
			}
			if (num2 == 4 || (num2 == 3 && num > 0))
			{
				rect.height = 0.5f;
			}
			if (num2 == 2 && num == 1)
			{
				rect.x = 0.5f;
			}
			if (num2 == 3 && num > 0)
			{
				rect.x = 0.5f;
				if (num == 1)
				{
					rect.y = 0.5f;
				}
			}
			if (num2 == 4)
			{
				if (num == 0 || num == 1)
				{
					rect.y = 0.5f;
				}
				if (num == 1 || num == 3)
				{
					rect.x = 0.5f;
				}
			}
			gamePlayer2.camera.rect = rect;
			gamePlayer2.playerHealth.UpdateBallMovementCamera(camera);
			gamePlayer2.playerInputManager.ConnectToPlayerCharacter(gamePlayer2.playerGameObject);
			num++;
		}
		if ((from x in list
		where x.ai
		select x).Count<LobbyPlayer>() > 1)
		{
			base.gameObject.AddComponent<AiManager>();
		}
		this.CreateSplitScreenCanvas(num2);
		this.RegisterPlayersForStaminaManager();
	}

	// Token: 0x06000667 RID: 1639 RVA: 0x00020A7C File Offset: 0x0001EC7C
	private void createAI(LobbyPlayer lobbyPlayer)
	{
		if (this.spawnPoints.Count > this.players.Count)
		{
			GameObject gameObject = this.spawnPoints[this.players.Count];
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.playerPrefab, gameObject.transform);
			Type playerInputManagerAi = Generic.GetPlayerInputManagerAi(lobbyPlayer.customAiObject);
			PlayerInputAIManager playerInputAIManager = (PlayerInputAIManager)gameObject2.AddComponent(playerInputManagerAi);
			GameObject gameObject3 = gameObject2;
			gameObject3.name += " ai";
			if (lobbyPlayer.customAiObject != null)
			{
				playerInputAIManager.SetParameters(lobbyPlayer.customAiObject);
			}
			GamePlayer gamePlayer = new GamePlayer
			{
				playerGameObject = gameObject2,
				playerHealth = gameObject2.GetComponent<PlayerHealth>()
			};
			gamePlayer.playerHealth.playerAnimator.SetMoveSet(lobbyPlayer.selectedMoveSet, false, false);
			gamePlayer.playerHealth.SetEquipment(lobbyPlayer.selectedEquipment, false);
			gamePlayer.playerHealth.playerNum = this.players.Count + 1;
			if (string.IsNullOrEmpty(gamePlayer.playerHealth.playerName))
			{
				gamePlayer.playerHealth.playerName = LocalizationHelpers.LocalizedText("txt_artificial_intelligence_short", Array.Empty<object>());
			}
			if (!string.IsNullOrWhiteSpace(lobbyPlayer.playerName))
			{
				gamePlayer.playerHealth.playerName = lobbyPlayer.playerName;
			}
			gamePlayer.playerHealth.OnlyPhysical();
			gamePlayer.playerHealth.ai = true;
			playerInputAIManager.ConnectToPlayerCharacter(gameObject2);
			this.players.Add(gamePlayer);
		}
	}

	// Token: 0x06000668 RID: 1640 RVA: 0x00020BF0 File Offset: 0x0001EDF0
	private void CreateSplitScreenCanvas(int playerCount)
	{
		if (playerCount > 1)
		{
			SplitScreenCanvas component = UnityEngine.Object.Instantiate<GameObject>(this.splitScreenCanvasPrefab).GetComponent<SplitScreenCanvas>();
			if (component != null)
			{
				component.SetPlayerCount(playerCount);
			}
		}
	}

	// Token: 0x06000669 RID: 1641 RVA: 0x0000777A File Offset: 0x0000597A
	private void Update()
	{
	}

	// Token: 0x0600066A RID: 1642 RVA: 0x00020C24 File Offset: 0x0001EE24
	private void findSpawnPoints()
	{
		if (this.spawnPointHolder == null)
		{
			this.spawnPointHolder = GameObject.Find("SpawnPoints");
		}
		this.spawnPoints = new List<GameObject>();
		for (int i = 0; i < this.spawnPointHolder.transform.childCount; i++)
		{
			Transform child = this.spawnPointHolder.transform.GetChild(i);
			this.spawnPoints.Add(child.gameObject);
		}
	}

	// Token: 0x0600066B RID: 1643 RVA: 0x00020C98 File Offset: 0x0001EE98
	private void RegisterPlayersForStaminaManager()
	{
		if (SceneManagerWithParameters.currentScene == "MoveEditorTestMoveSet")
		{
			StaminaManager.FindAndRegisterPlayerHealths();
			return;
		}
		StaminaManager.RegisterPlayerHealths((from x in this.players
		select x.playerHealth).ToList<PlayerHealth>());
	}

	// Token: 0x04000455 RID: 1109
	public GameObject playerPrefab;

	// Token: 0x04000456 RID: 1110
	public GameObject inputManager;

	// Token: 0x04000457 RID: 1111
	public GameObject spawnPointHolder;

	// Token: 0x04000458 RID: 1112
	public List<GameObject> spawnPoints;

	// Token: 0x04000459 RID: 1113
	public List<GamePlayer> players;

	// Token: 0x0400045A RID: 1114
	public GameObject splitScreenCanvasPrefab;
}
