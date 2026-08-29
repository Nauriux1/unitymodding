using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using BasicUI;
using Mirror.RemoteCalls;
using MoveClasses;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils;

namespace Mirror
{
	// Token: 0x020002B8 RID: 696
	public class MultiplayerRoomPlayer : NetworkRoomPlayer, IRoomPlayer
	{
		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06001442 RID: 5186 RVA: 0x0006658E File Offset: 0x0006478E
		// (set) Token: 0x06001443 RID: 5187 RVA: 0x00066596 File Offset: 0x00064796
		public PlayerCanvasController playerCanvasContoller { get; set; }

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06001444 RID: 5188 RVA: 0x0006659F File Offset: 0x0006479F
		// (set) Token: 0x06001445 RID: 5189 RVA: 0x000665A6 File Offset: 0x000647A6
		public static MultiplayerRoomPlayer localMultiplayerRoomPlayer { get; private set; }

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06001446 RID: 5190 RVA: 0x000665AE File Offset: 0x000647AE
		public bool isLocalRoomPlayer
		{
			get
			{
				return this == MultiplayerRoomPlayer.localMultiplayerRoomPlayer;
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06001447 RID: 5191 RVA: 0x000665BB File Offset: 0x000647BB
		// (set) Token: 0x06001448 RID: 5192 RVA: 0x000665C3 File Offset: 0x000647C3
		public bool ai { get; set; }

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06001449 RID: 5193 RVA: 0x000665CC File Offset: 0x000647CC
		// (set) Token: 0x0600144A RID: 5194 RVA: 0x000665D4 File Offset: 0x000647D4
		public MoveSet selectedMoveSet
		{
			get
			{
				return this._selectedMoveSet;
			}
			set
			{
				this._fullMoveSet = value;
				this.UpdateSelectedMoveSet();
			}
		}

		// Token: 0x0600144B RID: 5195 RVA: 0x000665E4 File Offset: 0x000647E4
		public void UpdateSelectedMoveSet()
		{
			if (IGameSettingsManager.singleton != null && IGameSettingsManager.singleton.GameType == GameTypes.Classic && this._fullMoveSet != null && (!this._fullMoveSet.defaultMoveset || this._fullMoveSet.communityMoveset))
			{
				this._selectedMoveSet = MoveSetHelpers.ConvertMoveSetToClassic(this._fullMoveSet);
				return;
			}
			this._selectedMoveSet = this._fullMoveSet;
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x0600144C RID: 5196 RVA: 0x00066644 File Offset: 0x00064844
		// (set) Token: 0x0600144D RID: 5197 RVA: 0x00066670 File Offset: 0x00064870
		public Move defaultPassiveMove
		{
			get
			{
				if (IGameSettingsManager.singleton != null && IGameSettingsManager.singleton.GameType == GameTypes.Classic)
				{
					return MoveSetHelpers.ConvertMoveToClassic(JsonConvert.DeserializeObject<Move>(this.defaultPassiveMoveString));
				}
				return this._defaultPassiveMove;
			}
			set
			{
				this._defaultPassiveMove = value;
			}
		}

		// Token: 0x0600144E RID: 5198 RVA: 0x0006667C File Offset: 0x0006487C
		public virtual void DefaultPassiveMoveChanged(string _, string newValue)
		{
			try
			{
				this.defaultPassiveMove = JsonConvert.DeserializeObject<Move>(this.defaultPassiveMoveString);
			}
			catch (Exception message)
			{
				Debug.LogError(message);
				this.defaultPassiveMove = null;
			}
			this.SetupPreviewCharacter();
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x0600144F RID: 5199 RVA: 0x000666C0 File Offset: 0x000648C0
		// (set) Token: 0x06001450 RID: 5200 RVA: 0x000666C8 File Offset: 0x000648C8
		public List<EquippedEquipment> selectedEquipment
		{
			get
			{
				return this._selectedEquipment;
			}
			set
			{
				this.Network_selectedEquipment = value;
			}
		}

		// Token: 0x06001451 RID: 5201 RVA: 0x000666D1 File Offset: 0x000648D1
		public virtual void SelectedEquipmentChanged(List<EquippedEquipment> _, List<EquippedEquipment> newEquipment)
		{
			this.SetupPreviewCharacter();
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06001452 RID: 5202 RVA: 0x000666D9 File Offset: 0x000648D9
		// (set) Token: 0x06001453 RID: 5203 RVA: 0x000666E1 File Offset: 0x000648E1
		public PlayerMultiplayerInputManager playerMultiplayerInputManager { get; set; }

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06001454 RID: 5204 RVA: 0x000666EA File Offset: 0x000648EA
		// (set) Token: 0x06001455 RID: 5205 RVA: 0x000666F2 File Offset: 0x000648F2
		public PlayerHealth playerHealth { get; set; }

		// Token: 0x06001456 RID: 5206 RVA: 0x000666FB File Offset: 0x000648FB
		public virtual void PlayerNameChanged(string _, string newPlayerName)
		{
			this.UpdateNameAndStatusText();
			this.SetupPreviewCharacter();
			this.UpdateVoiceChatPlayerNames();
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06001457 RID: 5207 RVA: 0x0006670F File Offset: 0x0006490F
		// (set) Token: 0x06001458 RID: 5208 RVA: 0x00066717 File Offset: 0x00064917
		public bool disconnecting { get; set; }

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06001459 RID: 5209 RVA: 0x00066720 File Offset: 0x00064920
		public bool playerReadyState
		{
			get
			{
				return this.readyToBegin;
			}
		}

		// Token: 0x0600145A RID: 5210 RVA: 0x00066728 File Offset: 0x00064928
		public virtual void PingChanged(int _, int newPing)
		{
			this.UpdateNameAndStatusText();
		}

		// Token: 0x0600145B RID: 5211 RVA: 0x00066728 File Offset: 0x00064928
		public virtual void DeathReasonChanged(DeathReason _, DeathReason newDeathReason)
		{
			this.UpdateNameAndStatusText();
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x00066730 File Offset: 0x00064930
		public void Update()
		{
			if (base.isServer && this.lastPingTime + this.pingUpdateFrequency < Time.unscaledTime)
			{
				this.lastPingTime = Time.unscaledTime;
				this.Networkping = (int)(base.connectionToClient.rtt * 1000.0);
			}
			this.TryToSendDataOvertime();
		}

		// Token: 0x0600145D RID: 5213 RVA: 0x00066788 File Offset: 0x00064988
		public void SetMoveSet(MoveSet newMoveSet)
		{
			if (MultiplayerRoomPlayer.tempEditMode)
			{
				if (this.tempMultiplayerPlayerValues != null)
				{
					this.tempMultiplayerPlayerValues.selectedMoveSet = newMoveSet;
					this.tempMultiplayerPlayerValues.selectedEquipment = GameSettingsHelper.FilterDisabledEquipmentFromList(MoveClassHelpers.CloneEquipmentList(newMoveSet.defaultEquipment));
					this.tempMultiplayerPlayerValues.equipmentHasBeenEdited = false;
					this.UpdatePreviewVisuals();
					return;
				}
			}
			else if (this.selectedMoveSet != newMoveSet && this.moveSetChangeEnabled)
			{
				this.selectedMoveSet = newMoveSet;
				this.selectedEquipment = GameSettingsHelper.FilterDisabledEquipmentFromList(MoveClassHelpers.CloneEquipmentList(newMoveSet.defaultEquipment));
				this.UpdatePreviewVisuals();
				this.SendMovesetToServer(newMoveSet, this.selectedEquipment, false);
			}
		}

		// Token: 0x0600145E RID: 5214 RVA: 0x00066820 File Offset: 0x00064A20
		public void UpdatePreviewVisuals()
		{
			if (this.playerHealth != null)
			{
				this.playerHealth.playerAnimator.SetMoveSet(this.GetMoveSet(true), true, false);
				this.UpdatePreviewEquipment();
			}
		}

		// Token: 0x0600145F RID: 5215 RVA: 0x0006684F File Offset: 0x00064A4F
		private void UpdatePreviewEquipment()
		{
			if (this.playerHealth != null)
			{
				this.playerHealth.SetEquipment(this.GetSelectedEquipment(), false);
			}
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x00066874 File Offset: 0x00064A74
		public void SendMovesetToServer(MoveSet newMoveSet, List<EquippedEquipment> newEquipment, bool sendEquipment = false)
		{
			if (this.moveSetChangeEnabled)
			{
				if (newMoveSet == null)
				{
					return;
				}
				if (newMoveSet.defaultMoveset)
				{
					this.CmdChangeMoveSetByGuid(newMoveSet.guid, !sendEquipment);
				}
				else
				{
					sendEquipment = true;
					foreach (NetworkJsonMessage value in NetworkHelpers.CreateNetworkJsonMessages(newMoveSet.SerializeToString_PB<MoveSet>()))
					{
						this.CmdChangeMoveSetChunks(JsonConvert.SerializeObject(value));
					}
				}
				if (sendEquipment)
				{
					this.CmdChangeEquipment(JsonConvert.SerializeObject(newEquipment));
				}
				this.moveSetChangeEnabled = false;
			}
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x00066910 File Offset: 0x00064B10
		[TargetRpc]
		public void EnableMoveSetChange()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			this.SendTargetRPCInternal(null, "System.Void Mirror.MultiplayerRoomPlayer::EnableMoveSetChange()", 1366039316, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x00066940 File Offset: 0x00064B40
		public void SetEquipment(List<EquippedEquipment> newEquipment)
		{
			newEquipment = GameSettingsHelper.FilterDisabledEquipmentFromList(newEquipment);
			if (MultiplayerRoomPlayer.tempEditMode)
			{
				if (this.tempMultiplayerPlayerValues != null)
				{
					this.tempMultiplayerPlayerValues.selectedEquipment = newEquipment;
					this.tempMultiplayerPlayerValues.equipmentHasBeenEdited = true;
					return;
				}
			}
			else
			{
				this.selectedEquipment = newEquipment;
				if (this.playerHealth != null)
				{
					this.playerHealth.SetEquipment(this.selectedEquipment, false);
				}
				this.CmdChangeEquipment(JsonConvert.SerializeObject(this.selectedEquipment));
			}
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x000669B8 File Offset: 0x00064BB8
		[Command]
		public void CmdChangeMoveSetChunks(string moveSetJson)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteString(moveSetJson);
			base.SendCommandInternal("System.Void Mirror.MultiplayerRoomPlayer::CmdChangeMoveSetChunks(System.String)", -1162313207, writer, 0, true);
			NetworkWriterPool.Return(writer);
		}

		// Token: 0x06001464 RID: 5220 RVA: 0x000669F4 File Offset: 0x00064BF4
		[Command]
		public void CmdChangeMoveSetByGuid(string moveSetGuid, bool updateEquipment)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteString(moveSetGuid);
			writer.WriteBool(updateEquipment);
			base.SendCommandInternal("System.Void Mirror.MultiplayerRoomPlayer::CmdChangeMoveSetByGuid(System.String,System.Boolean)", 363236526, writer, 0, true);
			NetworkWriterPool.Return(writer);
		}

		// Token: 0x06001465 RID: 5221 RVA: 0x00066A38 File Offset: 0x00064C38
		private void UpdateDefaultPassiveMoveToClients()
		{
			if (base.isServer)
			{
				Move value = null;
				if (this._fullMoveSet != null)
				{
					Stance defaultStance = this._fullMoveSet.GetDefaultStance();
					if (defaultStance != null)
					{
						value = (from x in defaultStance.moveList
						where x.inputType == inputType.Passive
						select x).FirstOrDefault<Move>();
					}
					this.NetworkdefaultPassiveMoveString = JsonConvert.SerializeObject(value);
				}
			}
		}

		// Token: 0x06001466 RID: 5222 RVA: 0x00066AA4 File Offset: 0x00064CA4
		[Command]
		public void CmdChangeEquipment(string equipmentJson)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteString(equipmentJson);
			base.SendCommandInternal("System.Void Mirror.MultiplayerRoomPlayer::CmdChangeEquipment(System.String)", 1007837426, writer, 0, true);
			NetworkWriterPool.Return(writer);
		}

		// Token: 0x06001467 RID: 5223 RVA: 0x00066AE0 File Offset: 0x00064CE0
		[Server]
		private void SetSelectedEquipment(List<EquippedEquipment> newEquipmentList)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void Mirror.MultiplayerRoomPlayer::SetSelectedEquipment(System.Collections.Generic.List`1<MoveClasses.EquippedEquipment>)' called when server was not active");
				return;
			}
			newEquipmentList = GameSettingsHelper.FilterDisabledEquipmentFromList(newEquipmentList);
			if ((!this.readyToBegin && Utils.IsSceneActive(MultiplayerRoomManager.GetMultiplayerRoomManager().RoomScene)) || GameSettingsHelper.CheckCanPlayerReadyByEquipmentPoints(newEquipmentList))
			{
				this.selectedEquipment = newEquipmentList;
				return;
			}
			Debug.Log("Player " + this.playerName + " selected equipment with too many points");
			this.selectedEquipment = new List<EquippedEquipment>();
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x00066B58 File Offset: 0x00064D58
		[Command]
		public void CmdChangePlayerName(string newPlayerName)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteString(newPlayerName);
			base.SendCommandInternal("System.Void Mirror.MultiplayerRoomPlayer::CmdChangePlayerName(System.String)", 1760975958, writer, 0, true);
			NetworkWriterPool.Return(writer);
		}

		// Token: 0x06001469 RID: 5225 RVA: 0x00066B94 File Offset: 0x00064D94
		[Command]
		public void CmdUpdatePing(int updatedPing)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteInt(updatedPing);
			base.SendCommandInternal("System.Void Mirror.MultiplayerRoomPlayer::CmdUpdatePing(System.Int32)", 537900924, writer, 0, true);
			NetworkWriterPool.Return(writer);
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x00066BD0 File Offset: 0x00064DD0
		[Command]
		public void CmdSetRoomReadyState(bool readyState)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteBool(readyState);
			base.SendCommandInternal("System.Void Mirror.MultiplayerRoomPlayer::CmdSetRoomReadyState(System.Boolean)", -494656396, writer, 0, true);
			NetworkWriterPool.Return(writer);
		}

		// Token: 0x0600146B RID: 5227 RVA: 0x00066C0C File Offset: 0x00064E0C
		[ClientRpc]
		public void ClientRpcSetRoomReadyState(bool readyState)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteBool(readyState);
			this.SendRPCInternal("System.Void Mirror.MultiplayerRoomPlayer::ClientRpcSetRoomReadyState(System.Boolean)", -1616056492, writer, 0, true);
			NetworkWriterPool.Return(writer);
		}

		// Token: 0x0600146C RID: 5228 RVA: 0x00066C46 File Offset: 0x00064E46
		public override void OnStartServer()
		{
			Debug.Log("User connected(" + base.connectionToClient.address + ")");
		}

		// Token: 0x0600146D RID: 5229 RVA: 0x00066C67 File Offset: 0x00064E67
		public override void OnStopServer()
		{
			Debug.Log("User " + this.playerName + " disconnected");
		}

		// Token: 0x0600146E RID: 5230 RVA: 0x00066C84 File Offset: 0x00064E84
		public override void OnStartClient()
		{
			if (base.isLocalPlayer)
			{
				this.CmdChangePlayerName(SettingsHelper.GetPlayerName());
				if (base.isServer)
				{
					this.openGameSettingsByDefault = true;
				}
			}
			else if (this.customPlayerTexture == null)
			{
				this.RequestCustomPlayerTexture(null);
			}
			this.UpdateNameAndStatusText();
		}

		// Token: 0x0600146F RID: 5231 RVA: 0x00066CD0 File Offset: 0x00064ED0
		public override void OnStopClient()
		{
			this.disconnecting = true;
			this.UpdateNameAndStatusText();
		}

		// Token: 0x06001470 RID: 5232 RVA: 0x00066CDF File Offset: 0x00064EDF
		public override void OnStartLocalPlayer()
		{
			if (base.isLocalPlayer)
			{
				MultiplayerRoomPlayer.localMultiplayerRoomPlayer = this;
				this.SetupPlayerMoveSetCanvas();
				this.SendCustomPlayerTexture();
				this.SetupLocalVoiceChatPlayer();
				if (GameMenu.singleton != null)
				{
					GameMenu.singleton.SetupMultiplayer();
				}
				this.ClientSendUserDefaultMovesetSettings();
			}
		}

		// Token: 0x06001471 RID: 5233 RVA: 0x00066D20 File Offset: 0x00064F20
		public void SetupPlayerMoveSetCanvas()
		{
			if (base.isLocalPlayer)
			{
				GameObject gameObject = GameObject.Find("PlayerMoveSetCanvas");
				if (gameObject != null)
				{
					this.moveSetCanvasController = gameObject.GetComponent<PlayerCanvasController>();
					this.moveSetCanvasController.RegisterLobbyItems(this);
					if (this.openGameSettingsByDefault)
					{
						this.openGameSettingsByDefault = false;
						this.moveSetCanvasController.GameSettingsButtonClicked();
					}
				}
			}
		}

		// Token: 0x06001472 RID: 5234 RVA: 0x0000777A File Offset: 0x0000597A
		public override void OnStartAuthority()
		{
		}

		// Token: 0x06001473 RID: 5235 RVA: 0x0000777A File Offset: 0x0000597A
		public override void OnStopAuthority()
		{
		}

		// Token: 0x06001474 RID: 5236 RVA: 0x0000777A File Offset: 0x0000597A
		public override void OnClientEnterRoom()
		{
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x00066728 File Offset: 0x00064928
		public override void OnClientExitRoom()
		{
			this.UpdateNameAndStatusText();
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x000666D1 File Offset: 0x000648D1
		public override void IndexChanged(int oldIndex, int newIndex)
		{
			this.SetupPreviewCharacter();
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x00066D7B File Offset: 0x00064F7B
		public override void ReadyStateChanged(bool _, bool readyState)
		{
			Debug.Log(string.Format("ReadyStateChanged:{0}", readyState));
			this.UpdateNameAndStatusText();
			if (base.isLocalPlayer && this.moveSetCanvasController != null)
			{
				this.moveSetCanvasController.UpdateReadyButtonColor();
			}
		}

		// Token: 0x06001478 RID: 5240 RVA: 0x00066DBC File Offset: 0x00064FBC
		public void UpdateReadyStateUI()
		{
			Debug.Log(string.Format("UpdateReadyStateUI:{0}", this.playerReadyState));
			this.UpdateNameAndStatusText();
			if (base.isLocalPlayer && this.moveSetCanvasController != null)
			{
				this.moveSetCanvasController.UpdateReadyButtonColor();
			}
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x00066E0A File Offset: 0x0006500A
		public void SetReady()
		{
			if (this.readyToBegin)
			{
				base.SetReadyToBegin(false);
				this.CmdSetRoomReadyState(false);
				return;
			}
			if (!this.ValidatePlayerEquipment())
			{
				return;
			}
			base.SetReadyToBegin(true);
			this.CmdSetRoomReadyState(true);
		}

		// Token: 0x0600147A RID: 5242 RVA: 0x00066E3C File Offset: 0x0006503C
		public void UpdateNameAndStatusText()
		{
			MultiplayerLobbyStatusManager singleton = MultiplayerLobbyStatusManager.singleton;
			if (singleton != null)
			{
				singleton.UpdatePlayerNamesAndStatuses();
			}
			if (GameMenu.singleton != null)
			{
				GameMenu.singleton.UpdatePlayerList();
			}
		}

		// Token: 0x0600147B RID: 5243 RVA: 0x00066E75 File Offset: 0x00065075
		public void GoBack()
		{
			if (GeneralManager.AllowBackNavigation(null))
			{
				this.LeaveConfirm();
			}
		}

		// Token: 0x0600147C RID: 5244 RVA: 0x00066E88 File Offset: 0x00065088
		private void LeaveConfirm()
		{
			BasicConfirmDialog basicConfirmDialog = GeneralManager.CreateConfirmDialog(LocalizationHelpers.LocalizedText("confirm_txt_leave_lobby", Array.Empty<object>()), null, false);
			if (basicConfirmDialog != null)
			{
				basicConfirmDialog.okButton.onClick.AddListener(new UnityAction(this.LeaveLobby));
				return;
			}
			this.LeaveLobby();
		}

		// Token: 0x0600147D RID: 5245 RVA: 0x00066ED8 File Offset: 0x000650D8
		private void LeaveLobby()
		{
			if (GeneralManager.singleton != null)
			{
				GeneralManager.singleton.leftMultiplayerSessionVoluntarily = true;
			}
			SteamManager.steamManager.LeaveLobby();
			if (base.isClientOnly)
			{
				NetworkManager.singleton.StopClient();
				return;
			}
			if (base.isServer)
			{
				NetworkManager.singleton.StopHost();
			}
		}

		// Token: 0x0600147E RID: 5246 RVA: 0x00036A88 File Offset: 0x00034C88
		public Camera GetCamera()
		{
			return null;
		}

		// Token: 0x0600147F RID: 5247 RVA: 0x00066F2C File Offset: 0x0006512C
		public override void OnDisable()
		{
			if (this.playerPreviewGameObject != null)
			{
				UnityEngine.Object.Destroy(this.playerPreviewGameObject);
			}
			if (this.playerNameCanvasGameObject != null)
			{
				UnityEngine.Object.Destroy(this.playerNameCanvasGameObject.transform.parent.gameObject);
			}
			base.OnDisable();
		}

		// Token: 0x06001480 RID: 5248 RVA: 0x00066F80 File Offset: 0x00065180
		public void SetupPreviewCharacter()
		{
			NetworkRoomManager networkRoomManager = NetworkManager.singleton as NetworkRoomManager;
			if (networkRoomManager && !Utils.IsSceneActive(networkRoomManager.RoomScene))
			{
				return;
			}
			if (this.playerHealth == null && this.playerPreviewGameObject == null && !base.isLocalPlayer)
			{
				this.playerPreviewGameObject = UnityEngine.Object.Instantiate<GameObject>(this.playerPreviewPrefab);
				this.playerHealth = this.playerPreviewGameObject.GetComponent<PlayerHealth>();
				this.playerHealth.RegisterMultiplayerRoomPlayer(this);
				this.playerHealth.OnlyAnimation();
			}
			if (this.playerNameCanvasGameObject == null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.playerNameCanvasPrefab);
				this.playerNameCanvasGameObject = gameObject.GetComponentInChildren<Text>();
			}
			if (this.playerHealth != null)
			{
				Camera main = Camera.main;
				float num = main.transform.position.x - 1.5f;
				num += Mathf.Abs(1f) * (float)this.index;
				this.playerHealth.playerAnimator.gameObject.transform.position = new Vector3(num, 0f, -6f);
				Vector3 forward = new Vector3(main.transform.position.x, this.playerHealth.playerAnimator.gameObject.transform.position.y, main.transform.position.z) - this.playerHealth.playerAnimator.gameObject.transform.position;
				if (base.isLocalPlayer)
				{
					this.playerHealth.playerAnimator.gameObject.transform.Translate(forward.normalized, Space.World);
				}
				this.playerHealth.playerAnimator.gameObject.transform.rotation = Quaternion.LookRotation(forward);
				this.playerNameCanvasGameObject.text = this.playerName;
				Vector3 v = main.WorldToScreenPoint(new Vector3(this.playerHealth.playerAnimator.gameObject.transform.position.x, 0f, this.playerHealth.playerAnimator.gameObject.transform.position.z - 0.25f), Camera.MonoOrStereoscopicEye.Mono);
				this.playerNameCanvasGameObject.rectTransform.anchoredPosition = v;
				this.playerHealth.SetEquipment(this.selectedEquipment, false);
				this.playerHealth.playerAnimator.ClearRunningSingleMoves();
				if (this.defaultPassiveMove != null && this.defaultPassiveMove.jointMoveList != null)
				{
					this.playerHealth.playerAnimator.PlayMove(this.defaultPassiveMove, false, false, 0f, false);
				}
				if (base.isLocalPlayer)
				{
					this.playerCanvasContoller.equipmentPanel.UpdateEquipmentInfo(true, true);
				}
				if (this.spectator)
				{
					this.playerHealth.SetAnimatedPlayerVisible(false);
					this.playerNameCanvasGameObject.gameObject.SetActive(false);
				}
				else
				{
					this.playerHealth.SetAnimatedPlayerVisible(true);
					this.playerNameCanvasGameObject.gameObject.SetActive(true);
				}
			}
			this.UpdateTextureForCurrentPlayerHealth();
		}

		// Token: 0x06001481 RID: 5249 RVA: 0x00067286 File Offset: 0x00065486
		public void SetJoinedMidGame(bool newValue)
		{
			this.joinedMidGame = newValue;
		}

		// Token: 0x06001482 RID: 5250 RVA: 0x0006728F File Offset: 0x0006548F
		public bool GetJoinedMidGame()
		{
			return this.joinedMidGame;
		}

		// Token: 0x06001483 RID: 5251 RVA: 0x00067297 File Offset: 0x00065497
		public virtual void SpectatorChanged(bool _, bool newSpectator)
		{
			if (base.isLocalPlayer && this.playerCanvasContoller != null)
			{
				this.playerCanvasContoller.UpdateSpectatorUI();
			}
			this.UpdateNameAndStatusText();
			this.SetupPreviewCharacter();
		}

		// Token: 0x06001484 RID: 5252 RVA: 0x000672C6 File Offset: 0x000654C6
		public void SetSpectator(bool value)
		{
			if (MultiplayerRoomPlayer.tempEditMode)
			{
				if (this.tempMultiplayerPlayerValues != null)
				{
					this.tempMultiplayerPlayerValues.spectator = value;
					return;
				}
			}
			else
			{
				this.CmdSetSpectator(value);
			}
		}

		// Token: 0x06001485 RID: 5253 RVA: 0x000672EB File Offset: 0x000654EB
		public bool GetSpectator()
		{
			if (MultiplayerRoomPlayer.tempEditMode)
			{
				return this.tempMultiplayerPlayerValues.spectator;
			}
			return this.spectator;
		}

		// Token: 0x06001486 RID: 5254 RVA: 0x00067308 File Offset: 0x00065508
		[Command]
		public void CmdSetSpectator(bool value)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteBool(value);
			base.SendCommandInternal("System.Void Mirror.MultiplayerRoomPlayer::CmdSetSpectator(System.Boolean)", -1861889548, writer, 0, true);
			NetworkWriterPool.Return(writer);
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06001487 RID: 5255 RVA: 0x00067342 File Offset: 0x00065542
		public static bool tempEditMode
		{
			get
			{
				return GameMenu.singleton != null;
			}
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06001488 RID: 5256 RVA: 0x0006734F File Offset: 0x0006554F
		// (set) Token: 0x06001489 RID: 5257 RVA: 0x00067365 File Offset: 0x00065565
		public TempMultiplayerPlayerValues tempMultiplayerPlayerValues
		{
			get
			{
				if (this._tempMultiplayerPlayerValues == null)
				{
					this.UpdateTempMultiplayerPlayerValues();
				}
				return this._tempMultiplayerPlayerValues;
			}
			set
			{
				this._tempMultiplayerPlayerValues = value;
			}
		}

		// Token: 0x0600148A RID: 5258 RVA: 0x00067370 File Offset: 0x00065570
		public void UpdateTempMultiplayerPlayerValues()
		{
			this.tempMultiplayerPlayerValues = new TempMultiplayerPlayerValues
			{
				selectedMoveSet = this._fullMoveSet,
				selectedEquipment = MoveClassHelpers.CloneEquipmentList(this.selectedEquipment),
				spectator = this.spectator
			};
			this.tempMultiplayerPlayerValues.equipmentHasBeenEdited = !GameSettingsHelper.CheckPlayerUsesDefaultEquipment(this.tempMultiplayerPlayerValues.selectedMoveSet, this.tempMultiplayerPlayerValues.selectedEquipment);
		}

		// Token: 0x0600148B RID: 5259 RVA: 0x000673DA File Offset: 0x000655DA
		public void ClearTempMultiplayerPlayerValues()
		{
			this.tempMultiplayerPlayerValues = null;
		}

		// Token: 0x0600148C RID: 5260 RVA: 0x000673E3 File Offset: 0x000655E3
		public MoveSet GetMoveSet()
		{
			return this.GetMoveSet(false);
		}

		// Token: 0x0600148D RID: 5261 RVA: 0x000673EC File Offset: 0x000655EC
		public MoveSet GetMoveSet(bool preview = false)
		{
			if (!MultiplayerRoomPlayer.tempEditMode)
			{
				return this.selectedMoveSet;
			}
			if (preview && IGameSettingsManager.singleton != null && IGameSettingsManager.singleton.GameType == GameTypes.Classic && this.tempMultiplayerPlayerValues.selectedMoveSet != null && (!this.tempMultiplayerPlayerValues.selectedMoveSet.defaultMoveset || this.tempMultiplayerPlayerValues.selectedMoveSet.communityMoveset))
			{
				return MoveSetHelpers.ConvertMoveSetToClassic(this.tempMultiplayerPlayerValues.selectedMoveSet);
			}
			return this.tempMultiplayerPlayerValues.selectedMoveSet;
		}

		// Token: 0x0600148E RID: 5262 RVA: 0x0006746A File Offset: 0x0006566A
		public List<EquippedEquipment> GetSelectedEquipment()
		{
			if (MultiplayerRoomPlayer.tempEditMode)
			{
				return this.tempMultiplayerPlayerValues.selectedEquipment;
			}
			return this.selectedEquipment;
		}

		// Token: 0x0600148F RID: 5263 RVA: 0x00067488 File Offset: 0x00065688
		public bool ApplyTempPlayerValues()
		{
			if (this.ValidatePlayerEquipment() && this.moveSetChangeEnabled)
			{
				this.selectedMoveSet = this.tempMultiplayerPlayerValues.selectedMoveSet;
				this.SendMovesetToServer(this.tempMultiplayerPlayerValues.selectedMoveSet, this.tempMultiplayerPlayerValues.selectedEquipment, this.tempMultiplayerPlayerValues.equipmentHasBeenEdited);
				if (this.spectator != this.tempMultiplayerPlayerValues.spectator)
				{
					this.CmdSetSpectator(this.tempMultiplayerPlayerValues.spectator);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06001490 RID: 5264 RVA: 0x00067504 File Offset: 0x00065704
		[Server]
		public void SetDefaultEquipment()
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void Mirror.MultiplayerRoomPlayer::SetDefaultEquipment()' called when server was not active");
				return;
			}
			if (this.selectedMoveSet != null && !GameSettingsHelper.CheckPlayerUsesDefaultEquipmentOrEditingAllowed(this.selectedMoveSet, this.selectedEquipment))
			{
				this.SetSelectedEquipment(MoveClassHelpers.CloneEquipmentList(this.selectedMoveSet.defaultEquipment));
			}
		}

		// Token: 0x06001491 RID: 5265 RVA: 0x00067558 File Offset: 0x00065758
		[Server]
		public void CheckAllowedEquipment()
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void Mirror.MultiplayerRoomPlayer::CheckAllowedEquipment()' called when server was not active");
				return;
			}
			if (this.selectedEquipment != null && this.selectedEquipment.Count > 0 && !GameSettingsHelper.CheckPlayerUsesAllowedEquipment(this.selectedEquipment))
			{
				this.SetSelectedEquipment(MoveClassHelpers.CloneEquipmentList(this.selectedEquipment));
			}
		}

		// Token: 0x06001492 RID: 5266 RVA: 0x000675B0 File Offset: 0x000657B0
		public void SetDefaultMoveSet()
		{
			if (this.selectedMoveSet == null)
			{
				List<MoveSet> defaultMoveSets = MoveSetHelpers.GetDefaultMoveSets(false);
				MoveSet moveSet = null;
				foreach (MoveSet moveSet2 in defaultMoveSets)
				{
					if (moveSet2.defaultMoveset && !moveSet2.communityMoveset)
					{
						moveSet = moveSet2;
						if (GameSettingsHelper.CheckCanPlayerReady(moveSet, moveSet.defaultEquipment))
						{
							break;
						}
					}
				}
				if (moveSet != null)
				{
					this.selectedMoveSet = moveSet;
					if (GameSettingsHelper.CheckCanPlayerReadyByEquipmentPoints(moveSet.defaultEquipment))
					{
						this.SetSelectedEquipment(MoveClassHelpers.CloneEquipmentList(moveSet.defaultEquipment));
						return;
					}
					Debug.Log("Default equipment uses too many points " + this.playerName);
					this.selectedEquipment = new List<EquippedEquipment>();
				}
			}
		}

		// Token: 0x06001493 RID: 5267 RVA: 0x00067674 File Offset: 0x00065874
		public void SetEquipmentStartingHold(EquippedEquipment equippedEquipment)
		{
			this.UpdateLocalEquipmentStartingHold(equippedEquipment.positionInt, equippedEquipment.equipmentStartHoldTypeInt, equippedEquipment.equipmentStartHoldPosition);
			if (!MultiplayerRoomPlayer.tempEditMode)
			{
				this.CmdSetEquipmentStartingHold(equippedEquipment.positionInt, equippedEquipment.equipmentStartHoldTypeInt, equippedEquipment.equipmentStartHoldPosition);
			}
		}

		// Token: 0x06001494 RID: 5268 RVA: 0x000676B0 File Offset: 0x000658B0
		[Command]
		public void CmdSetEquipmentStartingHold(int positionInt, int equipmentStartHoldTypeInt, float equipmentStartHoldPosition)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteInt(positionInt);
			writer.WriteInt(equipmentStartHoldTypeInt);
			writer.WriteFloat(equipmentStartHoldPosition);
			base.SendCommandInternal("System.Void Mirror.MultiplayerRoomPlayer::CmdSetEquipmentStartingHold(System.Int32,System.Int32,System.Single)", 1357984272, writer, 0, true);
			NetworkWriterPool.Return(writer);
		}

		// Token: 0x06001495 RID: 5269 RVA: 0x00067700 File Offset: 0x00065900
		[ClientRpc(includeOwner = false)]
		public void ClientRpcSetEquipmentStartingHold(int positionInt, int equipmentStartHoldTypeInt, float equipmentStartHoldPosition)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteInt(positionInt);
			writer.WriteInt(equipmentStartHoldTypeInt);
			writer.WriteFloat(equipmentStartHoldPosition);
			this.SendRPCInternal("System.Void Mirror.MultiplayerRoomPlayer::ClientRpcSetEquipmentStartingHold(System.Int32,System.Int32,System.Single)", -986284816, writer, 0, false);
			NetworkWriterPool.Return(writer);
		}

		// Token: 0x06001496 RID: 5270 RVA: 0x00067750 File Offset: 0x00065950
		private void UpdateLocalEquipmentStartingHold(int positionInt, int equipmentStartHoldTypeInt, float equipmentStartHoldPosition)
		{
			List<EquippedEquipment> selectedEquipment = this.GetSelectedEquipment();
			for (int i = 0; i < selectedEquipment.Count; i++)
			{
				EquippedEquipment equippedEquipment = selectedEquipment[i];
				if (equippedEquipment.positionInt == positionInt)
				{
					equippedEquipment.equipmentStartHoldTypeInt = equipmentStartHoldTypeInt;
					equippedEquipment.equipmentStartHoldPosition = equipmentStartHoldPosition;
				}
			}
			if (MultiplayerRoomPlayer.tempEditMode)
			{
				this.tempMultiplayerPlayerValues.equipmentHasBeenEdited = true;
			}
		}

		// Token: 0x06001497 RID: 5271 RVA: 0x000677A7 File Offset: 0x000659A7
		[Server]
		public void KickPlayer()
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void Mirror.MultiplayerRoomPlayer::KickPlayer()' called when server was not active");
				return;
			}
			base.connectionToClient.Disconnect();
		}

		// Token: 0x06001498 RID: 5272 RVA: 0x000677C9 File Offset: 0x000659C9
		public void BanPlayer()
		{
			if (NetworkManager.singleton != null)
			{
				((MultiplayerRoomManager)NetworkManager.singleton).BanPlayerConfirm(base.connectionToClient, this.playerName);
			}
		}

		// Token: 0x06001499 RID: 5273 RVA: 0x000677F4 File Offset: 0x000659F4
		public bool ValidatePlayerEquipment()
		{
			if (GameSettingsHelper.CheckCanPlayerReadyByEquipmentPoints(this.GetSelectedEquipment()))
			{
				return true;
			}
			if (this.playerCanvasContoller != null)
			{
				int equipmentPoints = IGameSettingsManager.singleton.EquipmentPoints;
				string text = string.Format("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGBA(UISettings.BasicButtonNotReadyColor), GameSettingsHelper.CountEquippedEquipmentPoints(this.GetSelectedEquipment()));
				this.playerCanvasContoller.DisplayInfoMessage(LocalizationHelpers.LocalizedText("txt_max_equipment_points_alert", new object[]
				{
					equipmentPoints,
					text
				}));
			}
			return false;
		}

		// Token: 0x0600149A RID: 5274 RVA: 0x00067878 File Offset: 0x00065A78
		public void SendCustomPlayerTexture()
		{
			if (this.customPlayerTexture == null)
			{
				Texture2D x = SettingsHelper.GetCustomPlayerTexture();
				if (x != null)
				{
					this.customPlayerTexture = x;
					byte[] array = SettingsHelper.GetCustomPlayerTextureBytes();
					if (ValidationHelpers.ValidateTexture(this.customPlayerTexture, array))
					{
						this.sentTextureMessageFromClientToServerPackets = 0;
						this.textureMessagesFromClientToServer = NetworkHelpers.CreateNetworkByteMessage(array);
						this.lastSendClientTextureToServer = Time.unscaledTime - (this.sendAttemptDelay - this.waitTimeBeforeSendingBigPackets);
						this.allClientTextureSent = false;
						return;
					}
					Debug.Log("Custom Player Texture not sent. Texture is too large.");
				}
			}
		}

		// Token: 0x0600149B RID: 5275 RVA: 0x000678FC File Offset: 0x00065AFC
		public void HandleReceivedPlayerTextureMessage(NetworkByteMessage byteMessage)
		{
			if (base.isServer)
			{
				if (byteMessage.p == 1)
				{
					Debug.Log(string.Format("receiving custom texture from {0}. Parts:{1}", base.connectionToClient.address, byteMessage.tp));
				}
				this.playerTextureIsBeingSynced = true;
				if (byteMessage.tp > NetworkHelpers.maxPlayerCustomTextureMessageParts && !base.isLocalPlayer)
				{
					Debug.Log(string.Concat(new string[]
					{
						"Player attempted to send a texture that was too large. The connection was forcibly closed.(",
						base.connectionToClient.address,
						")(",
						this.playerName,
						")"
					}));
					base.connectionToClient.Disconnect();
					return;
				}
			}
			if (byteMessage.id != this.previousNetworkByteMessagePlayerTexture.id)
			{
				this.previousNetworkByteMessagePlayerTexture = byteMessage;
				this.networkByteMessageListPlayerTexture = new List<NetworkByteMessage>();
				this.customPlayerTexture = null;
			}
			if (this.networkByteMessageListPlayerTexture.Count == 0 || byteMessage.p != this.networkByteMessageListPlayerTexture.Last<NetworkByteMessage>().p)
			{
				this.networkByteMessageListPlayerTexture.Add(byteMessage);
			}
			if (this.networkByteMessageListPlayerTexture.Count == this.previousNetworkByteMessagePlayerTexture.tp)
			{
				RecompiledByteMessage recompiledByteMessage = NetworkHelpers.RecompileByteMessage(this.networkByteMessageListPlayerTexture);
				if (recompiledByteMessage.Status == 0)
				{
					Texture2D tex = new Texture2D(2, 2);
					tex.LoadImage(recompiledByteMessage.Message);
					this.customPlayerTexture = tex;
					this.customPlayerTextureBytes = recompiledByteMessage.Message;
					if (base.isServer)
					{
						this.TextureFromClientReceived(base.connectionToClient);
						this.UpdateCustomPlayerTextureToClients(null);
					}
					if (base.isClient)
					{
						this.TextureFromServerReceived(null);
					}
				}
				this.UpdateTextureForCurrentPlayerHealth();
				this.playerTextureIsBeingSynced = false;
			}
		}

		// Token: 0x0600149C RID: 5276 RVA: 0x00067A94 File Offset: 0x00065C94
		[Command]
		public void CmdChangeCustomPlayerTexturesChunks(NetworkByteMessage byteMessage, NetworkConnectionToClient sender = null)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			GeneratedNetworkCode._Write_Utils.NetworkByteMessage(writer, byteMessage);
			base.SendCommandInternal("System.Void Mirror.MultiplayerRoomPlayer::CmdChangeCustomPlayerTexturesChunks(Utils.NetworkByteMessage,Mirror.NetworkConnectionToClient)", 390771297, writer, 0, true);
			NetworkWriterPool.Return(writer);
		}

		// Token: 0x0600149D RID: 5277 RVA: 0x00067AD0 File Offset: 0x00065CD0
		[Command(requiresAuthority = false)]
		public void RequestCustomPlayerTexture(NetworkConnectionToClient sender = null)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			base.SendCommandInternal("System.Void Mirror.MultiplayerRoomPlayer::RequestCustomPlayerTexture(Mirror.NetworkConnectionToClient)", 1970435139, writer, 0, false);
			NetworkWriterPool.Return(writer);
		}

		// Token: 0x0600149E RID: 5278 RVA: 0x00067B00 File Offset: 0x00065D00
		public void RemoveFromCustomTextureAlreadySent(NetworkConnectionToClient conn)
		{
			NetworkMessagesSent networkMessagesSent = this.FindNetworkMessagesSentByConnection(conn);
			if (networkMessagesSent != null)
			{
				this.customTextureNetworkMessages.Remove(networkMessagesSent);
			}
		}

		// Token: 0x0600149F RID: 5279 RVA: 0x00067B28 File Offset: 0x00065D28
		private NetworkMessagesSent FindNetworkMessagesSentByConnection(NetworkConnectionToClient conn)
		{
			foreach (NetworkMessagesSent networkMessagesSent in this.customTextureNetworkMessages)
			{
				if (networkMessagesSent.conn == conn)
				{
					return networkMessagesSent;
				}
			}
			return null;
		}

		// Token: 0x060014A0 RID: 5280 RVA: 0x00067B84 File Offset: 0x00065D84
		[Server]
		public void UpdateCustomPlayerTextureToClients(NetworkConnectionToClient target = null)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void Mirror.MultiplayerRoomPlayer::UpdateCustomPlayerTextureToClients(Mirror.NetworkConnectionToClient)' called when server was not active");
				return;
			}
			if (target != null && this.FindNetworkMessagesSentByConnection(target) != null)
			{
				return;
			}
			if (!ValidationHelpers.ValidateTexture(this.customPlayerTexture, this.customPlayerTextureBytes))
			{
				if (!base.isLocalPlayer)
				{
					Debug.Log(string.Concat(new string[]
					{
						"Player sent a texture that was too large. The connection was forcibly closed.(",
						base.connectionToClient.address,
						")(",
						this.playerName,
						")"
					}));
					base.connectionToClient.Disconnect();
				}
				return;
			}
			if (this.customPlayerTextureBytes != null && this.customPlayerTextureBytes.Length != 0 && this.customPlayerTextureBytes.Length < SettingsHelper.customPlayerTextureMaxBytes)
			{
				if (this.textureMessagesFromServerToClient == null || this.textureMessagesFromServerToClient.Count == 0)
				{
					this.textureMessagesFromServerToClient = NetworkHelpers.CreateNetworkByteMessage(this.customPlayerTextureBytes);
				}
				for (int i = 0; i < NetworkServer.connections.Count; i++)
				{
					NetworkConnectionToClient value = NetworkServer.connections.ElementAt(i).Value;
					if (value.connectionId != 0 && !value.owned.Contains(base.netIdentity) && (target == null || value.connectionId == target.connectionId))
					{
						this.customTextureNetworkMessages.Add(new NetworkMessagesSent(value, Time.unscaledTime - (this.sendAttemptDelay - this.waitTimeBeforeSendingBigPackets)));
					}
				}
			}
		}

		// Token: 0x060014A1 RID: 5281 RVA: 0x00067CE4 File Offset: 0x00065EE4
		[TargetRpc]
		public void RpcSetCustomPlayerTexture(NetworkConnectionToClient target, NetworkByteMessage byteMessage)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			GeneratedNetworkCode._Write_Utils.NetworkByteMessage(writer, byteMessage);
			this.SendTargetRPCInternal(target, "System.Void Mirror.MultiplayerRoomPlayer::RpcSetCustomPlayerTexture(Mirror.NetworkConnectionToClient,Utils.NetworkByteMessage)", 7919257, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		// Token: 0x060014A2 RID: 5282 RVA: 0x00067D20 File Offset: 0x00065F20
		public void UpdateTextureForCurrentPlayerHealth()
		{
			if (this.playerHealth != null && this.customPlayerTexture != null)
			{
				this.playerHealth.SetPlayerTexture(this.customPlayerTexture);
			}
			if (this.playerMultiplayerInputManager != null)
			{
				this.playerMultiplayerInputManager.SetTexture();
			}
		}

		// Token: 0x060014A3 RID: 5283 RVA: 0x00067D74 File Offset: 0x00065F74
		[TargetRpc]
		public void CanSendMoreToServer(NetworkConnectionToClient target)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			this.SendTargetRPCInternal(target, "System.Void Mirror.MultiplayerRoomPlayer::CanSendMoreToServer(Mirror.NetworkConnectionToClient)", 1272488510, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		// Token: 0x060014A4 RID: 5284 RVA: 0x00067DA4 File Offset: 0x00065FA4
		[Command(requiresAuthority = false)]
		public void CanSendMoreToClient(NetworkConnectionToClient sender = null)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			base.SendCommandInternal("System.Void Mirror.MultiplayerRoomPlayer::CanSendMoreToClient(Mirror.NetworkConnectionToClient)", -2099362618, writer, 0, false);
			NetworkWriterPool.Return(writer);
		}

		// Token: 0x060014A5 RID: 5285 RVA: 0x00067DD4 File Offset: 0x00065FD4
		[TargetRpc]
		public void TextureFromClientReceived(NetworkConnectionToClient target)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			this.SendTargetRPCInternal(target, "System.Void Mirror.MultiplayerRoomPlayer::TextureFromClientReceived(Mirror.NetworkConnectionToClient)", 1587609316, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		// Token: 0x060014A6 RID: 5286 RVA: 0x00067E04 File Offset: 0x00066004
		[Command(requiresAuthority = false)]
		public void TextureFromServerReceived(NetworkConnectionToClient sender = null)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			base.SendCommandInternal("System.Void Mirror.MultiplayerRoomPlayer::TextureFromServerReceived(Mirror.NetworkConnectionToClient)", -368941988, writer, 0, false);
			NetworkWriterPool.Return(writer);
		}

		// Token: 0x060014A7 RID: 5287 RVA: 0x00067E34 File Offset: 0x00066034
		private void TryToSendTextureToServer()
		{
			if (this.allClientTextureSent)
			{
				return;
			}
			if (this.textureMessagesFromClientToServer == null || this.sentTextureMessageFromClientToServerPackets == this.textureMessagesFromClientToServer.Count || !this.canSendClientTextureToServer || !NetworkClient.ready)
			{
				if (!this.canSendClientTextureToServer && this.lastSendClientTextureToServer + this.sendAttemptDelay < Time.unscaledTime)
				{
					this.canSendClientTextureToServer = true;
					if (this.sentTextureMessageFromClientToServerPackets > 0)
					{
						this.sentTextureMessageFromClientToServerPackets--;
					}
					Debug.Log("Resend custom texture");
					this.failedSends++;
					if (this.failedSends > 2)
					{
						this.allClientTextureSent = true;
					}
				}
				return;
			}
			if (MultiplayerRoomPlayer.localMultiplayerRoomPlayer == this && this.textureMessagesFromClientToServer.Count > 0)
			{
				int num = this.sentTextureMessageFromClientToServerPackets + 1;
				if (num > this.textureMessagesFromClientToServer.Count)
				{
					num = this.textureMessagesFromClientToServer.Count;
				}
				Debug.Log(string.Format("customTexture send to server:{0}/{1}", this.sentTextureMessageFromClientToServerPackets + 1, this.textureMessagesFromClientToServer.Count));
				while (this.sentTextureMessageFromClientToServerPackets < num)
				{
					this.CmdChangeCustomPlayerTexturesChunks(this.textureMessagesFromClientToServer[this.sentTextureMessageFromClientToServerPackets], null);
					this.sentTextureMessageFromClientToServerPackets++;
				}
				this.lastSendClientTextureToServer = Time.unscaledTime;
				this.canSendClientTextureToServer = false;
			}
		}

		// Token: 0x060014A8 RID: 5288 RVA: 0x00067F8C File Offset: 0x0006618C
		[Server]
		private void TryToSendTextureToClients()
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void Mirror.MultiplayerRoomPlayer::TryToSendTextureToClients()' called when server was not active");
				return;
			}
			foreach (NetworkMessagesSent networkMessagesSent in this.customTextureNetworkMessages)
			{
				if (networkMessagesSent.conn != null && networkMessagesSent.conn.isReady && !NetworkServer.isLoadingScene && !networkMessagesSent.allSent)
				{
					if (this.textureMessagesFromServerToClient == null || networkMessagesSent.sentPackets == this.textureMessagesFromServerToClient.Count || !networkMessagesSent.canSend)
					{
						if (!networkMessagesSent.canSend && networkMessagesSent.lastSend + this.sendAttemptDelay < Time.unscaledTime)
						{
							networkMessagesSent.canSend = true;
							if (networkMessagesSent.sentPackets > 0)
							{
								networkMessagesSent.sentPackets--;
							}
							Debug.Log("Resend custom texture to client");
							networkMessagesSent.failedSends++;
							if (networkMessagesSent.failedSends > 2)
							{
								networkMessagesSent.allSent = true;
							}
						}
					}
					else if (this.textureMessagesFromServerToClient.Count > 0)
					{
						int num = networkMessagesSent.sentPackets + 1;
						if (num > this.textureMessagesFromServerToClient.Count)
						{
							num = this.textureMessagesFromServerToClient.Count;
						}
						while (networkMessagesSent.sentPackets < num)
						{
							this.RpcSetCustomPlayerTexture(networkMessagesSent.conn, this.textureMessagesFromServerToClient[networkMessagesSent.sentPackets]);
							networkMessagesSent.sentPackets++;
						}
						networkMessagesSent.lastSend = Time.unscaledTime;
						networkMessagesSent.canSend = false;
					}
				}
			}
		}

		// Token: 0x060014A9 RID: 5289 RVA: 0x00068138 File Offset: 0x00066338
		public void RegisterPlayerMultiplayerInputManager(PlayerMultiplayerInputManager newPlayerMultiplayerInputManager)
		{
			this.playerMultiplayerInputManager = newPlayerMultiplayerInputManager;
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x060014AA RID: 5290 RVA: 0x00068141 File Offset: 0x00066341
		public string VoiceChatId
		{
			get
			{
				return this._voiceChatId;
			}
		}

		// Token: 0x060014AB RID: 5291 RVA: 0x00068149 File Offset: 0x00066349
		public virtual void VoiceChatIDChanged(string _, string newVoiceChatID)
		{
			if (VoiceChatManager.singleton != null)
			{
				VoiceChatManager.singleton.PairVoiceChatPlayerAndRoomPlayer();
			}
		}

		// Token: 0x060014AC RID: 5292 RVA: 0x00068164 File Offset: 0x00066364
		[Command]
		private void CmdSetVoiceChatID(string newVoiceChatID)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteString(newVoiceChatID);
			base.SendCommandInternal("System.Void Mirror.MultiplayerRoomPlayer::CmdSetVoiceChatID(System.String)", 110595599, writer, 0, true);
			NetworkWriterPool.Return(writer);
		}

		// Token: 0x060014AD RID: 5293 RVA: 0x0006819E File Offset: 0x0006639E
		private void SetupLocalVoiceChatPlayer()
		{
			if (base.isLocalPlayer && VoiceChatManager.singleton != null && VoiceChatManager.singleton.dissonanceComms != null)
			{
				this.CmdSetVoiceChatID(VoiceChatManager.singleton.dissonanceComms.LocalPlayerName);
			}
		}

		// Token: 0x060014AE RID: 5294 RVA: 0x000681DC File Offset: 0x000663DC
		private void UpdateVoiceChatPlayerNames()
		{
			if (VoiceChatManager.singleton != null)
			{
				VoiceChatManager.singleton.UpdateVoiceChatPlayerNames();
			}
		}

		// Token: 0x060014AF RID: 5295 RVA: 0x000681F5 File Offset: 0x000663F5
		public void ToggleMutePlayer()
		{
			if (this.voiceChatPlayer != null)
			{
				this.voiceChatPlayer.voicePlayerState.IsLocallyMuted = !this.voiceChatPlayer.voicePlayerState.IsLocallyMuted;
			}
		}

		// Token: 0x060014B0 RID: 5296 RVA: 0x00068222 File Offset: 0x00066422
		public bool IsPlayerMuted()
		{
			return this.voiceChatPlayer != null && this.voiceChatPlayer.voicePlayerState.IsLocallyMuted;
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x060014B1 RID: 5297 RVA: 0x0006823E File Offset: 0x0006643E
		public double dataSendTickInterval
		{
			get
			{
				return (double)((this.dataSendTickRate < int.MaxValue) ? (1f / (float)this.dataSendTickRate) : 0f);
			}
		}

		// Token: 0x060014B2 RID: 5298 RVA: 0x00068264 File Offset: 0x00066464
		private void TryToSendDataOvertime()
		{
			if ((MultiplayerRoomPlayer.localMultiplayerRoomPlayer == this || base.isServer) && AccurateInterval.Elapsed(Time.timeAsDouble, this.dataSendTickInterval, ref this.lastFixedDataSendTickTime))
			{
				this.TryToSendTextureToServer();
				if (base.isServer)
				{
					this.TryToSendTextureToClients();
				}
			}
		}

		// Token: 0x060014B3 RID: 5299 RVA: 0x000682B4 File Offset: 0x000664B4
		public void ClientSendUserDefaultMovesetSettings()
		{
			if (this == MultiplayerRoomPlayer.localMultiplayerRoomPlayer)
			{
				DefaultMovesetSettings defaultMovesetSettings = SettingsHelper.GetDefaultMovesetSettings();
				if (!this.userDefaultMovesetSettings.Equals(defaultMovesetSettings))
				{
					this.userDefaultMovesetSettings = defaultMovesetSettings;
					this.CmdSetUserDefaultMovesetSettings(this.userDefaultMovesetSettings);
				}
			}
		}

		// Token: 0x060014B4 RID: 5300 RVA: 0x000682F8 File Offset: 0x000664F8
		[Command]
		private void CmdSetUserDefaultMovesetSettings(DefaultMovesetSettings newDefaultMovesetSettings)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			GeneratedNetworkCode._Write_Utils.DefaultMovesetSettings(writer, newDefaultMovesetSettings);
			base.SendCommandInternal("System.Void Mirror.MultiplayerRoomPlayer::CmdSetUserDefaultMovesetSettings(Utils.DefaultMovesetSettings)", 947314210, writer, 0, true);
			NetworkWriterPool.Return(writer);
		}

		// Token: 0x060014B6 RID: 5302 RVA: 0x0000C7D7 File Offset: 0x0000A9D7
		public override bool Weaved()
		{
			return true;
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x060014B7 RID: 5303 RVA: 0x000683B8 File Offset: 0x000665B8
		// (set) Token: 0x060014B8 RID: 5304 RVA: 0x000683CB File Offset: 0x000665CB
		public string NetworkdefaultPassiveMoveString
		{
			get
			{
				return this.defaultPassiveMoveString;
			}
			[param: In]
			set
			{
				base.GeneratedSyncVarSetter<string>(value, ref this.defaultPassiveMoveString, 4UL, new Action<string, string>(this.DefaultPassiveMoveChanged));
			}
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x060014B9 RID: 5305 RVA: 0x000683F4 File Offset: 0x000665F4
		// (set) Token: 0x060014BA RID: 5306 RVA: 0x00068407 File Offset: 0x00066607
		public List<EquippedEquipment> Network_selectedEquipment
		{
			get
			{
				return this._selectedEquipment;
			}
			[param: In]
			set
			{
				base.GeneratedSyncVarSetter<List<EquippedEquipment>>(value, ref this._selectedEquipment, 8UL, new Action<List<EquippedEquipment>, List<EquippedEquipment>>(this.SelectedEquipmentChanged));
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x060014BB RID: 5307 RVA: 0x00068430 File Offset: 0x00066630
		// (set) Token: 0x060014BC RID: 5308 RVA: 0x00068443 File Offset: 0x00066643
		public string NetworkplayerName
		{
			get
			{
				return this.playerName;
			}
			[param: In]
			set
			{
				base.GeneratedSyncVarSetter<string>(value, ref this.playerName, 16UL, new Action<string, string>(this.PlayerNameChanged));
			}
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x060014BD RID: 5309 RVA: 0x0006846C File Offset: 0x0006666C
		// (set) Token: 0x060014BE RID: 5310 RVA: 0x0006847F File Offset: 0x0006667F
		public int Networkping
		{
			get
			{
				return this.ping;
			}
			[param: In]
			set
			{
				base.GeneratedSyncVarSetter<int>(value, ref this.ping, 32UL, new Action<int, int>(this.PingChanged));
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x060014BF RID: 5311 RVA: 0x000684A8 File Offset: 0x000666A8
		// (set) Token: 0x060014C0 RID: 5312 RVA: 0x000684BB File Offset: 0x000666BB
		public float? NetworkdeathTime
		{
			get
			{
				return this.deathTime;
			}
			[param: In]
			set
			{
				base.GeneratedSyncVarSetter<float?>(value, ref this.deathTime, 64UL, null);
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x060014C1 RID: 5313 RVA: 0x000684D8 File Offset: 0x000666D8
		// (set) Token: 0x060014C2 RID: 5314 RVA: 0x000684EB File Offset: 0x000666EB
		public DeathReason NetworkplayerDeathReason
		{
			get
			{
				return this.playerDeathReason;
			}
			[param: In]
			set
			{
				base.GeneratedSyncVarSetter<DeathReason>(value, ref this.playerDeathReason, 128UL, new Action<DeathReason, DeathReason>(this.DeathReasonChanged));
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x060014C3 RID: 5315 RVA: 0x00068514 File Offset: 0x00066714
		// (set) Token: 0x060014C4 RID: 5316 RVA: 0x00068527 File Offset: 0x00066727
		public bool Networkspectator
		{
			get
			{
				return this.spectator;
			}
			[param: In]
			set
			{
				base.GeneratedSyncVarSetter<bool>(value, ref this.spectator, 256UL, new Action<bool, bool>(this.SpectatorChanged));
			}
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x060014C5 RID: 5317 RVA: 0x00068550 File Offset: 0x00066750
		// (set) Token: 0x060014C6 RID: 5318 RVA: 0x00068563 File Offset: 0x00066763
		public string Network_voiceChatId
		{
			get
			{
				return this._voiceChatId;
			}
			[param: In]
			set
			{
				base.GeneratedSyncVarSetter<string>(value, ref this._voiceChatId, 512UL, new Action<string, string>(this.VoiceChatIDChanged));
			}
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x00068589 File Offset: 0x00066789
		protected void UserCode_EnableMoveSetChange()
		{
			this.moveSetChangeEnabled = true;
			if (MultiplayerRoomPlayer.tempEditMode)
			{
				GeneralManager.DisplayInfoMessage(LocalizationHelpers.LocalizedText("txt_changes_applied", Array.Empty<object>()), 1f);
			}
		}

		// Token: 0x060014C8 RID: 5320 RVA: 0x000685B2 File Offset: 0x000667B2
		protected static void InvokeUserCode_EnableMoveSetChange(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("TargetRPC EnableMoveSetChange called on server.");
				return;
			}
			((MultiplayerRoomPlayer)obj).UserCode_EnableMoveSetChange();
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x000685D8 File Offset: 0x000667D8
		protected void UserCode_CmdChangeMoveSetChunks__String(string moveSetJson)
		{
			if (this.readyToBegin)
			{
				base.SetReadyToBegin(false);
			}
			this.moveSetIsBeingSynced = true;
			NetworkJsonMessage networkJsonMessage = JsonConvert.DeserializeObject<NetworkJsonMessage>(moveSetJson);
			if (this.previousNetworkJsonMessage == null || networkJsonMessage.guid != this.previousNetworkJsonMessage.guid)
			{
				this.previousNetworkJsonMessage = networkJsonMessage;
				this.networkJsonMessageList = new List<NetworkJsonMessage>();
				this.selectedMoveSet = null;
			}
			if (networkJsonMessage.tp > MoveSetHelpers.maxMoveSetSize / NetworkHelpers.chunkSize + 1)
			{
				Debug.Log("Kicked player because their move set was too long");
				base.connectionToClient.Disconnect();
				return;
			}
			this.networkJsonMessageList.Add(networkJsonMessage);
			if (this.networkJsonMessageList.Count == this.previousNetworkJsonMessage.tp)
			{
				if (IGameSettingsManager.singleton != null && IGameSettingsManager.singleton.AllowedMovesetTypes == AllowedMovesetTypes.All)
				{
					RecompiledJsonMessage recompiledJsonMessage = NetworkHelpers.RecompileJsonMessage(this.networkJsonMessageList);
					if (recompiledJsonMessage.Status == 0)
					{
						this.selectedMoveSet = recompiledJsonMessage.Message.DeserializeFromString_PB<MoveSet>();
						this.UpdateDefaultPassiveMoveToClients();
					}
				}
				else
				{
					Debug.Log(this.playerName + " attempted to send custom moveset when not allowed.");
				}
				this.EnableMoveSetChange();
				this.moveSetIsBeingSynced = false;
			}
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x000686EC File Offset: 0x000668EC
		protected static void InvokeUserCode_CmdChangeMoveSetChunks__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdChangeMoveSetChunks called on client.");
				return;
			}
			((MultiplayerRoomPlayer)obj).UserCode_CmdChangeMoveSetChunks__String(reader.ReadString());
		}

		// Token: 0x060014CB RID: 5323 RVA: 0x00068718 File Offset: 0x00066918
		protected void UserCode_CmdChangeMoveSetByGuid__String__Boolean(string moveSetGuid, bool updateEquipment)
		{
			if (this.readyToBegin)
			{
				base.SetReadyToBegin(false);
			}
			MoveSet movesetByGuid = MoveSetHelpers.GetMovesetByGuid(moveSetGuid);
			if (movesetByGuid != null && GameSettingsHelper.CheckPlayerUsesAllowedMoveset(movesetByGuid))
			{
				this.selectedMoveSet = movesetByGuid;
				if (updateEquipment)
				{
					this.SetSelectedEquipment(MoveClassHelpers.CloneEquipmentList(this.selectedMoveSet.defaultEquipment));
				}
				this.UpdateDefaultPassiveMoveToClients();
			}
			this.EnableMoveSetChange();
		}

		// Token: 0x060014CC RID: 5324 RVA: 0x00068772 File Offset: 0x00066972
		protected static void InvokeUserCode_CmdChangeMoveSetByGuid__String__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdChangeMoveSetByGuid called on client.");
				return;
			}
			((MultiplayerRoomPlayer)obj).UserCode_CmdChangeMoveSetByGuid__String__Boolean(reader.ReadString(), reader.ReadBool());
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x000687A4 File Offset: 0x000669A4
		protected void UserCode_CmdChangeEquipment__String(string equipmentJson)
		{
			if (IGameSettingsManager.singleton != null && !IGameSettingsManager.singleton.AllowEquipmentEdit)
			{
				return;
			}
			if (this.readyToBegin)
			{
				base.SetReadyToBegin(false);
			}
			List<EquippedEquipment> selectedEquipment = JsonConvert.DeserializeObject<List<EquippedEquipment>>(equipmentJson);
			this.SetSelectedEquipment(selectedEquipment);
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x000687E2 File Offset: 0x000669E2
		protected static void InvokeUserCode_CmdChangeEquipment__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdChangeEquipment called on client.");
				return;
			}
			((MultiplayerRoomPlayer)obj).UserCode_CmdChangeEquipment__String(reader.ReadString());
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x0006880C File Offset: 0x00066A0C
		protected void UserCode_CmdChangePlayerName__String(string newPlayerName)
		{
			string text = ValidationHelpers.ValidatePlayerNameLength(newPlayerName);
			if (text.Length != newPlayerName.Length)
			{
				Debug.Log("Kicked player because their name was too long");
				base.connectionToClient.Disconnect();
			}
			this.NetworkplayerName = GeneralManager.singleton.FilterBadWords(text, false);
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x00068855 File Offset: 0x00066A55
		protected static void InvokeUserCode_CmdChangePlayerName__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdChangePlayerName called on client.");
				return;
			}
			((MultiplayerRoomPlayer)obj).UserCode_CmdChangePlayerName__String(reader.ReadString());
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x0006887E File Offset: 0x00066A7E
		protected void UserCode_CmdUpdatePing__Int32(int updatedPing)
		{
			this.Networkping = updatedPing;
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x00068887 File Offset: 0x00066A87
		protected static void InvokeUserCode_CmdUpdatePing__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdUpdatePing called on client.");
				return;
			}
			((MultiplayerRoomPlayer)obj).UserCode_CmdUpdatePing__Int32(reader.ReadInt());
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x000688B0 File Offset: 0x00066AB0
		protected void UserCode_CmdSetRoomReadyState__Boolean(bool readyState)
		{
			Debug.Log(string.Format("CmdSetRoomReadyState:{0}", readyState));
			if (readyState)
			{
				if (this.selectedMoveSet == null)
				{
					base.SetReadyToBegin(false);
				}
				else
				{
					base.SetReadyToBegin(readyState);
				}
			}
			else
			{
				base.SetReadyToBegin(readyState);
			}
			this.ClientRpcSetRoomReadyState(this.readyToBegin);
			MultiplayerRoomManager multiplayerRoomManager = NetworkManager.singleton as MultiplayerRoomManager;
			if (multiplayerRoomManager != null)
			{
				multiplayerRoomManager.ReadyStatusChanged();
			}
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x0006891C File Offset: 0x00066B1C
		protected static void InvokeUserCode_CmdSetRoomReadyState__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetRoomReadyState called on client.");
				return;
			}
			((MultiplayerRoomPlayer)obj).UserCode_CmdSetRoomReadyState__Boolean(reader.ReadBool());
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x00068945 File Offset: 0x00066B45
		protected void UserCode_ClientRpcSetRoomReadyState__Boolean(bool readyState)
		{
			Debug.Log(string.Format("ClientRpcSetRoomReadyState:{0}", readyState));
			base.SetReadyToBegin(readyState);
			this.UpdateReadyStateUI();
		}

		// Token: 0x060014D6 RID: 5334 RVA: 0x00068969 File Offset: 0x00066B69
		protected static void InvokeUserCode_ClientRpcSetRoomReadyState__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC ClientRpcSetRoomReadyState called on server.");
				return;
			}
			((MultiplayerRoomPlayer)obj).UserCode_ClientRpcSetRoomReadyState__Boolean(reader.ReadBool());
		}

		// Token: 0x060014D7 RID: 5335 RVA: 0x00068992 File Offset: 0x00066B92
		protected void UserCode_CmdSetSpectator__Boolean(bool value)
		{
			if (this.readyToBegin)
			{
				base.SetReadyToBegin(false);
			}
			this.Networkspectator = value;
		}

		// Token: 0x060014D8 RID: 5336 RVA: 0x000689AA File Offset: 0x00066BAA
		protected static void InvokeUserCode_CmdSetSpectator__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetSpectator called on client.");
				return;
			}
			((MultiplayerRoomPlayer)obj).UserCode_CmdSetSpectator__Boolean(reader.ReadBool());
		}

		// Token: 0x060014D9 RID: 5337 RVA: 0x000689D3 File Offset: 0x00066BD3
		protected void UserCode_CmdSetEquipmentStartingHold__Int32__Int32__Single(int positionInt, int equipmentStartHoldTypeInt, float equipmentStartHoldPosition)
		{
			if (this.readyToBegin)
			{
				base.SetReadyToBegin(false);
			}
			this.UpdateLocalEquipmentStartingHold(positionInt, equipmentStartHoldTypeInt, equipmentStartHoldPosition);
			this.ClientRpcSetEquipmentStartingHold(positionInt, equipmentStartHoldTypeInt, equipmentStartHoldPosition);
		}

		// Token: 0x060014DA RID: 5338 RVA: 0x000689F6 File Offset: 0x00066BF6
		protected static void InvokeUserCode_CmdSetEquipmentStartingHold__Int32__Int32__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetEquipmentStartingHold called on client.");
				return;
			}
			((MultiplayerRoomPlayer)obj).UserCode_CmdSetEquipmentStartingHold__Int32__Int32__Single(reader.ReadInt(), reader.ReadInt(), reader.ReadFloat());
		}

		// Token: 0x060014DB RID: 5339 RVA: 0x00068A2C File Offset: 0x00066C2C
		protected void UserCode_ClientRpcSetEquipmentStartingHold__Int32__Int32__Single(int positionInt, int equipmentStartHoldTypeInt, float equipmentStartHoldPosition)
		{
			this.UpdateLocalEquipmentStartingHold(positionInt, equipmentStartHoldTypeInt, equipmentStartHoldPosition);
			this.UpdatePreviewEquipment();
		}

		// Token: 0x060014DC RID: 5340 RVA: 0x00068A3D File Offset: 0x00066C3D
		protected static void InvokeUserCode_ClientRpcSetEquipmentStartingHold__Int32__Int32__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC ClientRpcSetEquipmentStartingHold called on server.");
				return;
			}
			((MultiplayerRoomPlayer)obj).UserCode_ClientRpcSetEquipmentStartingHold__Int32__Int32__Single(reader.ReadInt(), reader.ReadInt(), reader.ReadFloat());
		}

		// Token: 0x060014DD RID: 5341 RVA: 0x00068A73 File Offset: 0x00066C73
		protected void UserCode_CmdChangeCustomPlayerTexturesChunks__NetworkByteMessage__NetworkConnectionToClient(NetworkByteMessage byteMessage, NetworkConnectionToClient sender)
		{
			this.HandleReceivedPlayerTextureMessage(byteMessage);
			this.CanSendMoreToServer(sender);
		}

		// Token: 0x060014DE RID: 5342 RVA: 0x00068A83 File Offset: 0x00066C83
		protected static void InvokeUserCode_CmdChangeCustomPlayerTexturesChunks__NetworkByteMessage__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdChangeCustomPlayerTexturesChunks called on client.");
				return;
			}
			((MultiplayerRoomPlayer)obj).UserCode_CmdChangeCustomPlayerTexturesChunks__NetworkByteMessage__NetworkConnectionToClient(GeneratedNetworkCode._Read_Utils.NetworkByteMessage(reader), senderConnection);
		}

		// Token: 0x060014DF RID: 5343 RVA: 0x00068AAD File Offset: 0x00066CAD
		protected void UserCode_RequestCustomPlayerTexture__NetworkConnectionToClient(NetworkConnectionToClient sender)
		{
			this.UpdateCustomPlayerTextureToClients(sender);
		}

		// Token: 0x060014E0 RID: 5344 RVA: 0x00068AB6 File Offset: 0x00066CB6
		protected static void InvokeUserCode_RequestCustomPlayerTexture__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command RequestCustomPlayerTexture called on client.");
				return;
			}
			((MultiplayerRoomPlayer)obj).UserCode_RequestCustomPlayerTexture__NetworkConnectionToClient(senderConnection);
		}

		// Token: 0x060014E1 RID: 5345 RVA: 0x00068ADA File Offset: 0x00066CDA
		protected void UserCode_RpcSetCustomPlayerTexture__NetworkConnectionToClient__NetworkByteMessage(NetworkConnectionToClient target, NetworkByteMessage byteMessage)
		{
			this.HandleReceivedPlayerTextureMessage(byteMessage);
			this.CanSendMoreToClient(null);
		}

		// Token: 0x060014E2 RID: 5346 RVA: 0x00068AEA File Offset: 0x00066CEA
		protected static void InvokeUserCode_RpcSetCustomPlayerTexture__NetworkConnectionToClient__NetworkByteMessage(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("TargetRPC RpcSetCustomPlayerTexture called on server.");
				return;
			}
			((MultiplayerRoomPlayer)obj).UserCode_RpcSetCustomPlayerTexture__NetworkConnectionToClient__NetworkByteMessage(null, GeneratedNetworkCode._Read_Utils.NetworkByteMessage(reader));
		}

		// Token: 0x060014E3 RID: 5347 RVA: 0x00068B14 File Offset: 0x00066D14
		protected void UserCode_CanSendMoreToServer__NetworkConnectionToClient(NetworkConnectionToClient target)
		{
			this.failedSends = 0;
			this.canSendClientTextureToServer = true;
		}

		// Token: 0x060014E4 RID: 5348 RVA: 0x00068B24 File Offset: 0x00066D24
		protected static void InvokeUserCode_CanSendMoreToServer__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("TargetRPC CanSendMoreToServer called on server.");
				return;
			}
			((MultiplayerRoomPlayer)obj).UserCode_CanSendMoreToServer__NetworkConnectionToClient(null);
		}

		// Token: 0x060014E5 RID: 5349 RVA: 0x00068B48 File Offset: 0x00066D48
		protected void UserCode_CanSendMoreToClient__NetworkConnectionToClient(NetworkConnectionToClient sender)
		{
			NetworkMessagesSent networkMessagesSent = this.FindNetworkMessagesSentByConnection(sender);
			if (networkMessagesSent != null)
			{
				networkMessagesSent.canSend = true;
				networkMessagesSent.failedSends = 0;
			}
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x00068B6E File Offset: 0x00066D6E
		protected static void InvokeUserCode_CanSendMoreToClient__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CanSendMoreToClient called on client.");
				return;
			}
			((MultiplayerRoomPlayer)obj).UserCode_CanSendMoreToClient__NetworkConnectionToClient(senderConnection);
		}

		// Token: 0x060014E7 RID: 5351 RVA: 0x00068B92 File Offset: 0x00066D92
		protected void UserCode_TextureFromClientReceived__NetworkConnectionToClient(NetworkConnectionToClient target)
		{
			this.allClientTextureSent = true;
		}

		// Token: 0x060014E8 RID: 5352 RVA: 0x00068B9B File Offset: 0x00066D9B
		protected static void InvokeUserCode_TextureFromClientReceived__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("TargetRPC TextureFromClientReceived called on server.");
				return;
			}
			((MultiplayerRoomPlayer)obj).UserCode_TextureFromClientReceived__NetworkConnectionToClient(null);
		}

		// Token: 0x060014E9 RID: 5353 RVA: 0x00068BC0 File Offset: 0x00066DC0
		protected void UserCode_TextureFromServerReceived__NetworkConnectionToClient(NetworkConnectionToClient sender)
		{
			NetworkMessagesSent networkMessagesSent = this.FindNetworkMessagesSentByConnection(sender);
			if (networkMessagesSent != null)
			{
				networkMessagesSent.allSent = true;
			}
		}

		// Token: 0x060014EA RID: 5354 RVA: 0x00068BDF File Offset: 0x00066DDF
		protected static void InvokeUserCode_TextureFromServerReceived__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command TextureFromServerReceived called on client.");
				return;
			}
			((MultiplayerRoomPlayer)obj).UserCode_TextureFromServerReceived__NetworkConnectionToClient(senderConnection);
		}

		// Token: 0x060014EB RID: 5355 RVA: 0x00068C03 File Offset: 0x00066E03
		protected void UserCode_CmdSetVoiceChatID__String(string newVoiceChatID)
		{
			this.Network_voiceChatId = newVoiceChatID;
		}

		// Token: 0x060014EC RID: 5356 RVA: 0x00068C0C File Offset: 0x00066E0C
		protected static void InvokeUserCode_CmdSetVoiceChatID__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetVoiceChatID called on client.");
				return;
			}
			((MultiplayerRoomPlayer)obj).UserCode_CmdSetVoiceChatID__String(reader.ReadString());
		}

		// Token: 0x060014ED RID: 5357 RVA: 0x00068C35 File Offset: 0x00066E35
		protected void UserCode_CmdSetUserDefaultMovesetSettings__DefaultMovesetSettings(DefaultMovesetSettings newDefaultMovesetSettings)
		{
			this.userDefaultMovesetSettings = newDefaultMovesetSettings;
			if (this.playerMultiplayerInputManager)
			{
				this.playerMultiplayerInputManager.UpdateDefaultMovesetSettings();
			}
		}

		// Token: 0x060014EE RID: 5358 RVA: 0x00068C56 File Offset: 0x00066E56
		protected static void InvokeUserCode_CmdSetUserDefaultMovesetSettings__DefaultMovesetSettings(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetUserDefaultMovesetSettings called on client.");
				return;
			}
			((MultiplayerRoomPlayer)obj).UserCode_CmdSetUserDefaultMovesetSettings__DefaultMovesetSettings(GeneratedNetworkCode._Read_Utils.DefaultMovesetSettings(reader));
		}

		// Token: 0x060014EF RID: 5359 RVA: 0x00068C80 File Offset: 0x00066E80
		static MultiplayerRoomPlayer()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(MultiplayerRoomPlayer), "System.Void Mirror.MultiplayerRoomPlayer::CmdChangeMoveSetChunks(System.String)", new RemoteCallDelegate(MultiplayerRoomPlayer.InvokeUserCode_CmdChangeMoveSetChunks__String), true);
			RemoteProcedureCalls.RegisterCommand(typeof(MultiplayerRoomPlayer), "System.Void Mirror.MultiplayerRoomPlayer::CmdChangeMoveSetByGuid(System.String,System.Boolean)", new RemoteCallDelegate(MultiplayerRoomPlayer.InvokeUserCode_CmdChangeMoveSetByGuid__String__Boolean), true);
			RemoteProcedureCalls.RegisterCommand(typeof(MultiplayerRoomPlayer), "System.Void Mirror.MultiplayerRoomPlayer::CmdChangeEquipment(System.String)", new RemoteCallDelegate(MultiplayerRoomPlayer.InvokeUserCode_CmdChangeEquipment__String), true);
			RemoteProcedureCalls.RegisterCommand(typeof(MultiplayerRoomPlayer), "System.Void Mirror.MultiplayerRoomPlayer::CmdChangePlayerName(System.String)", new RemoteCallDelegate(MultiplayerRoomPlayer.InvokeUserCode_CmdChangePlayerName__String), true);
			RemoteProcedureCalls.RegisterCommand(typeof(MultiplayerRoomPlayer), "System.Void Mirror.MultiplayerRoomPlayer::CmdUpdatePing(System.Int32)", new RemoteCallDelegate(MultiplayerRoomPlayer.InvokeUserCode_CmdUpdatePing__Int32), true);
			RemoteProcedureCalls.RegisterCommand(typeof(MultiplayerRoomPlayer), "System.Void Mirror.MultiplayerRoomPlayer::CmdSetRoomReadyState(System.Boolean)", new RemoteCallDelegate(MultiplayerRoomPlayer.InvokeUserCode_CmdSetRoomReadyState__Boolean), true);
			RemoteProcedureCalls.RegisterCommand(typeof(MultiplayerRoomPlayer), "System.Void Mirror.MultiplayerRoomPlayer::CmdSetSpectator(System.Boolean)", new RemoteCallDelegate(MultiplayerRoomPlayer.InvokeUserCode_CmdSetSpectator__Boolean), true);
			RemoteProcedureCalls.RegisterCommand(typeof(MultiplayerRoomPlayer), "System.Void Mirror.MultiplayerRoomPlayer::CmdSetEquipmentStartingHold(System.Int32,System.Int32,System.Single)", new RemoteCallDelegate(MultiplayerRoomPlayer.InvokeUserCode_CmdSetEquipmentStartingHold__Int32__Int32__Single), true);
			RemoteProcedureCalls.RegisterCommand(typeof(MultiplayerRoomPlayer), "System.Void Mirror.MultiplayerRoomPlayer::CmdChangeCustomPlayerTexturesChunks(Utils.NetworkByteMessage,Mirror.NetworkConnectionToClient)", new RemoteCallDelegate(MultiplayerRoomPlayer.InvokeUserCode_CmdChangeCustomPlayerTexturesChunks__NetworkByteMessage__NetworkConnectionToClient), true);
			RemoteProcedureCalls.RegisterCommand(typeof(MultiplayerRoomPlayer), "System.Void Mirror.MultiplayerRoomPlayer::RequestCustomPlayerTexture(Mirror.NetworkConnectionToClient)", new RemoteCallDelegate(MultiplayerRoomPlayer.InvokeUserCode_RequestCustomPlayerTexture__NetworkConnectionToClient), false);
			RemoteProcedureCalls.RegisterCommand(typeof(MultiplayerRoomPlayer), "System.Void Mirror.MultiplayerRoomPlayer::CanSendMoreToClient(Mirror.NetworkConnectionToClient)", new RemoteCallDelegate(MultiplayerRoomPlayer.InvokeUserCode_CanSendMoreToClient__NetworkConnectionToClient), false);
			RemoteProcedureCalls.RegisterCommand(typeof(MultiplayerRoomPlayer), "System.Void Mirror.MultiplayerRoomPlayer::TextureFromServerReceived(Mirror.NetworkConnectionToClient)", new RemoteCallDelegate(MultiplayerRoomPlayer.InvokeUserCode_TextureFromServerReceived__NetworkConnectionToClient), false);
			RemoteProcedureCalls.RegisterCommand(typeof(MultiplayerRoomPlayer), "System.Void Mirror.MultiplayerRoomPlayer::CmdSetVoiceChatID(System.String)", new RemoteCallDelegate(MultiplayerRoomPlayer.InvokeUserCode_CmdSetVoiceChatID__String), true);
			RemoteProcedureCalls.RegisterCommand(typeof(MultiplayerRoomPlayer), "System.Void Mirror.MultiplayerRoomPlayer::CmdSetUserDefaultMovesetSettings(Utils.DefaultMovesetSettings)", new RemoteCallDelegate(MultiplayerRoomPlayer.InvokeUserCode_CmdSetUserDefaultMovesetSettings__DefaultMovesetSettings), true);
			RemoteProcedureCalls.RegisterRpc(typeof(MultiplayerRoomPlayer), "System.Void Mirror.MultiplayerRoomPlayer::ClientRpcSetRoomReadyState(System.Boolean)", new RemoteCallDelegate(MultiplayerRoomPlayer.InvokeUserCode_ClientRpcSetRoomReadyState__Boolean));
			RemoteProcedureCalls.RegisterRpc(typeof(MultiplayerRoomPlayer), "System.Void Mirror.MultiplayerRoomPlayer::ClientRpcSetEquipmentStartingHold(System.Int32,System.Int32,System.Single)", new RemoteCallDelegate(MultiplayerRoomPlayer.InvokeUserCode_ClientRpcSetEquipmentStartingHold__Int32__Int32__Single));
			RemoteProcedureCalls.RegisterRpc(typeof(MultiplayerRoomPlayer), "System.Void Mirror.MultiplayerRoomPlayer::EnableMoveSetChange()", new RemoteCallDelegate(MultiplayerRoomPlayer.InvokeUserCode_EnableMoveSetChange));
			RemoteProcedureCalls.RegisterRpc(typeof(MultiplayerRoomPlayer), "System.Void Mirror.MultiplayerRoomPlayer::RpcSetCustomPlayerTexture(Mirror.NetworkConnectionToClient,Utils.NetworkByteMessage)", new RemoteCallDelegate(MultiplayerRoomPlayer.InvokeUserCode_RpcSetCustomPlayerTexture__NetworkConnectionToClient__NetworkByteMessage));
			RemoteProcedureCalls.RegisterRpc(typeof(MultiplayerRoomPlayer), "System.Void Mirror.MultiplayerRoomPlayer::CanSendMoreToServer(Mirror.NetworkConnectionToClient)", new RemoteCallDelegate(MultiplayerRoomPlayer.InvokeUserCode_CanSendMoreToServer__NetworkConnectionToClient));
			RemoteProcedureCalls.RegisterRpc(typeof(MultiplayerRoomPlayer), "System.Void Mirror.MultiplayerRoomPlayer::TextureFromClientReceived(Mirror.NetworkConnectionToClient)", new RemoteCallDelegate(MultiplayerRoomPlayer.InvokeUserCode_TextureFromClientReceived__NetworkConnectionToClient));
		}

		// Token: 0x060014F0 RID: 5360 RVA: 0x00068F1C File Offset: 0x0006711C
		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteString(this.defaultPassiveMoveString);
				GeneratedNetworkCode._Write_System.Collections.Generic.List`1<MoveClasses.EquippedEquipment>(writer, this._selectedEquipment);
				writer.WriteString(this.playerName);
				writer.WriteInt(this.ping);
				writer.WriteFloatNullable(this.deathTime);
				GeneratedNetworkCode._Write_MoveClasses.DeathReason(writer, this.playerDeathReason);
				writer.WriteBool(this.spectator);
				writer.WriteString(this._voiceChatId);
				return;
			}
			writer.WriteULong(base.syncVarDirtyBits);
			if ((base.syncVarDirtyBits & 4UL) != 0UL)
			{
				writer.WriteString(this.defaultPassiveMoveString);
			}
			if ((base.syncVarDirtyBits & 8UL) != 0UL)
			{
				GeneratedNetworkCode._Write_System.Collections.Generic.List`1<MoveClasses.EquippedEquipment>(writer, this._selectedEquipment);
			}
			if ((base.syncVarDirtyBits & 16UL) != 0UL)
			{
				writer.WriteString(this.playerName);
			}
			if ((base.syncVarDirtyBits & 32UL) != 0UL)
			{
				writer.WriteInt(this.ping);
			}
			if ((base.syncVarDirtyBits & 64UL) != 0UL)
			{
				writer.WriteFloatNullable(this.deathTime);
			}
			if ((base.syncVarDirtyBits & 128UL) != 0UL)
			{
				GeneratedNetworkCode._Write_MoveClasses.DeathReason(writer, this.playerDeathReason);
			}
			if ((base.syncVarDirtyBits & 256UL) != 0UL)
			{
				writer.WriteBool(this.spectator);
			}
			if ((base.syncVarDirtyBits & 512UL) != 0UL)
			{
				writer.WriteString(this._voiceChatId);
			}
		}

		// Token: 0x060014F1 RID: 5361 RVA: 0x000690B8 File Offset: 0x000672B8
		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				base.GeneratedSyncVarDeserialize<string>(ref this.defaultPassiveMoveString, new Action<string, string>(this.DefaultPassiveMoveChanged), reader.ReadString());
				base.GeneratedSyncVarDeserialize<List<EquippedEquipment>>(ref this._selectedEquipment, new Action<List<EquippedEquipment>, List<EquippedEquipment>>(this.SelectedEquipmentChanged), GeneratedNetworkCode._Read_System.Collections.Generic.List`1<MoveClasses.EquippedEquipment>(reader));
				base.GeneratedSyncVarDeserialize<string>(ref this.playerName, new Action<string, string>(this.PlayerNameChanged), reader.ReadString());
				base.GeneratedSyncVarDeserialize<int>(ref this.ping, new Action<int, int>(this.PingChanged), reader.ReadInt());
				base.GeneratedSyncVarDeserialize<float?>(ref this.deathTime, null, reader.ReadFloatNullable());
				base.GeneratedSyncVarDeserialize<DeathReason>(ref this.playerDeathReason, new Action<DeathReason, DeathReason>(this.DeathReasonChanged), GeneratedNetworkCode._Read_MoveClasses.DeathReason(reader));
				base.GeneratedSyncVarDeserialize<bool>(ref this.spectator, new Action<bool, bool>(this.SpectatorChanged), reader.ReadBool());
				base.GeneratedSyncVarDeserialize<string>(ref this._voiceChatId, new Action<string, string>(this.VoiceChatIDChanged), reader.ReadString());
				return;
			}
			long num = (long)reader.ReadULong();
			if ((num & 4L) != 0L)
			{
				base.GeneratedSyncVarDeserialize<string>(ref this.defaultPassiveMoveString, new Action<string, string>(this.DefaultPassiveMoveChanged), reader.ReadString());
			}
			if ((num & 8L) != 0L)
			{
				base.GeneratedSyncVarDeserialize<List<EquippedEquipment>>(ref this._selectedEquipment, new Action<List<EquippedEquipment>, List<EquippedEquipment>>(this.SelectedEquipmentChanged), GeneratedNetworkCode._Read_System.Collections.Generic.List`1<MoveClasses.EquippedEquipment>(reader));
			}
			if ((num & 16L) != 0L)
			{
				base.GeneratedSyncVarDeserialize<string>(ref this.playerName, new Action<string, string>(this.PlayerNameChanged), reader.ReadString());
			}
			if ((num & 32L) != 0L)
			{
				base.GeneratedSyncVarDeserialize<int>(ref this.ping, new Action<int, int>(this.PingChanged), reader.ReadInt());
			}
			if ((num & 64L) != 0L)
			{
				base.GeneratedSyncVarDeserialize<float?>(ref this.deathTime, null, reader.ReadFloatNullable());
			}
			if ((num & 128L) != 0L)
			{
				base.GeneratedSyncVarDeserialize<DeathReason>(ref this.playerDeathReason, new Action<DeathReason, DeathReason>(this.DeathReasonChanged), GeneratedNetworkCode._Read_MoveClasses.DeathReason(reader));
			}
			if ((num & 256L) != 0L)
			{
				base.GeneratedSyncVarDeserialize<bool>(ref this.spectator, new Action<bool, bool>(this.SpectatorChanged), reader.ReadBool());
			}
			if ((num & 512L) != 0L)
			{
				base.GeneratedSyncVarDeserialize<string>(ref this._voiceChatId, new Action<string, string>(this.VoiceChatIDChanged), reader.ReadString());
			}
		}

		// Token: 0x04000F3A RID: 3898
		private PlayerCanvasController moveSetCanvasController;

		// Token: 0x04000F3C RID: 3900
		private MoveSet _selectedMoveSet;

		// Token: 0x04000F3D RID: 3901
		private MoveSet _fullMoveSet;

		// Token: 0x04000F3E RID: 3902
		[SyncVar(hook = "DefaultPassiveMoveChanged")]
		public string defaultPassiveMoveString = "";

		// Token: 0x04000F3F RID: 3903
		private Move _defaultPassiveMove;

		// Token: 0x04000F40 RID: 3904
		[SyncVar(hook = "SelectedEquipmentChanged")]
		private List<EquippedEquipment> _selectedEquipment = new List<EquippedEquipment>();

		// Token: 0x04000F43 RID: 3907
		[SyncVar(hook = "PlayerNameChanged")]
		public string playerName = "";

		// Token: 0x04000F45 RID: 3909
		[SyncVar(hook = "PingChanged")]
		public int ping;

		// Token: 0x04000F46 RID: 3910
		private float lastPingTime;

		// Token: 0x04000F47 RID: 3911
		private float pingUpdateFrequency = 2f;

		// Token: 0x04000F48 RID: 3912
		[SyncVar]
		public float? deathTime;

		// Token: 0x04000F49 RID: 3913
		[SyncVar(hook = "DeathReasonChanged")]
		public DeathReason playerDeathReason;

		// Token: 0x04000F4A RID: 3914
		public bool moveSetChangeEnabled = true;

		// Token: 0x04000F4B RID: 3915
		public bool moveSetIsBeingSynced;

		// Token: 0x04000F4C RID: 3916
		private NetworkJsonMessage previousNetworkJsonMessage;

		// Token: 0x04000F4D RID: 3917
		private List<NetworkJsonMessage> networkJsonMessageList;

		// Token: 0x04000F4E RID: 3918
		public bool openGameSettingsByDefault;

		// Token: 0x04000F4F RID: 3919
		[Header("PlayerPreview")]
		public GameObject playerPreviewPrefab;

		// Token: 0x04000F50 RID: 3920
		public GameObject playerPreviewGameObject;

		// Token: 0x04000F51 RID: 3921
		public GameObject playerNameCanvasPrefab;

		// Token: 0x04000F52 RID: 3922
		public Text playerNameCanvasGameObject;

		// Token: 0x04000F53 RID: 3923
		private bool joinedMidGame;

		// Token: 0x04000F54 RID: 3924
		[SyncVar(hook = "SpectatorChanged")]
		public bool spectator;

		// Token: 0x04000F55 RID: 3925
		private TempMultiplayerPlayerValues _tempMultiplayerPlayerValues;

		// Token: 0x04000F56 RID: 3926
		public Texture2D customPlayerTexture;

		// Token: 0x04000F57 RID: 3927
		public byte[] customPlayerTextureBytes;

		// Token: 0x04000F58 RID: 3928
		public bool playerTextureIsBeingSynced;

		// Token: 0x04000F59 RID: 3929
		private NetworkByteMessage previousNetworkByteMessagePlayerTexture;

		// Token: 0x04000F5A RID: 3930
		private List<NetworkByteMessage> networkByteMessageListPlayerTexture;

		// Token: 0x04000F5B RID: 3931
		private List<NetworkMessagesSent> customTextureNetworkMessages = new List<NetworkMessagesSent>(16);

		// Token: 0x04000F5C RID: 3932
		private float sendAttemptDelay = 5f;

		// Token: 0x04000F5D RID: 3933
		private float waitTimeBeforeSendingBigPackets = 2f;

		// Token: 0x04000F5E RID: 3934
		private List<NetworkByteMessage> textureMessagesFromClientToServer;

		// Token: 0x04000F5F RID: 3935
		private int sentTextureMessageFromClientToServerPackets;

		// Token: 0x04000F60 RID: 3936
		private bool canSendClientTextureToServer;

		// Token: 0x04000F61 RID: 3937
		private float lastSendClientTextureToServer;

		// Token: 0x04000F62 RID: 3938
		private bool allClientTextureSent = true;

		// Token: 0x04000F63 RID: 3939
		private int failedSends;

		// Token: 0x04000F64 RID: 3940
		private List<NetworkByteMessage> textureMessagesFromServerToClient;

		// Token: 0x04000F65 RID: 3941
		[SyncVar(hook = "VoiceChatIDChanged")]
		private string _voiceChatId;

		// Token: 0x04000F66 RID: 3942
		public VoiceChatPlayer voiceChatPlayer;

		// Token: 0x04000F67 RID: 3943
		private int dataSendTickRate = 60;

		// Token: 0x04000F68 RID: 3944
		public double lastFixedDataSendTickTime;

		// Token: 0x04000F69 RID: 3945
		public DefaultMovesetSettings userDefaultMovesetSettings = new DefaultMovesetSettings();
	}
}
