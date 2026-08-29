using System;
using System.Runtime.InteropServices;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

namespace Dissonance.Integrations.MirrorIgnorance
{
	// Token: 0x02000311 RID: 785
	[RequireComponent(typeof(NetworkIdentity))]
	public class MirrorIgnorancePlayer : NetworkBehaviour, IDissonancePlayer
	{
		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06001779 RID: 6009 RVA: 0x00076C33 File Offset: 0x00074E33
		// (set) Token: 0x0600177A RID: 6010 RVA: 0x00076C3B File Offset: 0x00074E3B
		public bool IsTracking { get; private set; }

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x0600177B RID: 6011 RVA: 0x00076C44 File Offset: 0x00074E44
		public string PlayerId
		{
			get
			{
				return this._playerId;
			}
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x0600177C RID: 6012 RVA: 0x00076C4C File Offset: 0x00074E4C
		public Vector3 Position
		{
			get
			{
				return base.transform.position;
			}
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x0600177D RID: 6013 RVA: 0x00076C59 File Offset: 0x00074E59
		public Quaternion Rotation
		{
			get
			{
				return base.transform.rotation;
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x0600177E RID: 6014 RVA: 0x00076C66 File Offset: 0x00074E66
		public NetworkPlayerType Type
		{
			get
			{
				if (this._comms == null || this._playerId == null)
				{
					return NetworkPlayerType.Unknown;
				}
				if (!this._comms.LocalPlayerName.Equals(this._playerId))
				{
					return NetworkPlayerType.Remote;
				}
				return NetworkPlayerType.Local;
			}
		}

		// Token: 0x0600177F RID: 6015 RVA: 0x00076C9B File Offset: 0x00074E9B
		public void OnDestroy()
		{
			if (this._comms != null)
			{
				this._comms.LocalPlayerNameChanged -= this.SetPlayerName;
			}
		}

		// Token: 0x06001780 RID: 6016 RVA: 0x00076CC2 File Offset: 0x00074EC2
		public void OnEnable()
		{
			this._comms = UnityEngine.Object.FindObjectOfType<DissonanceComms>();
		}

		// Token: 0x06001781 RID: 6017 RVA: 0x00076CCF File Offset: 0x00074ECF
		public void OnDisable()
		{
			if (this.IsTracking)
			{
				this.StopTracking();
			}
		}

		// Token: 0x06001782 RID: 6018 RVA: 0x00076CE0 File Offset: 0x00074EE0
		public override void OnStartLocalPlayer()
		{
			base.OnStartLocalPlayer();
			DissonanceComms dissonanceComms = UnityEngine.Object.FindObjectOfType<DissonanceComms>();
			if (dissonanceComms == null)
			{
				throw MirrorIgnorancePlayer.Log.CreateUserErrorException("cannot find DissonanceComms component in scene", "not placing a DissonanceComms component on a game object in the scene", "https://dissonance.readthedocs.io/en/latest/Basics/Quick-Start-MirrorIgnorance/", "2D90A6C3-5F2B-4859-994C-EBBDDD4A10F4");
			}
			if (dissonanceComms.LocalPlayerName != null)
			{
				this.SetPlayerName(dissonanceComms.LocalPlayerName);
			}
			dissonanceComms.LocalPlayerNameChanged += this.SetPlayerName;
		}

		// Token: 0x06001783 RID: 6019 RVA: 0x00076D47 File Offset: 0x00074F47
		private void SetPlayerName(string playerName)
		{
			if (this.IsTracking)
			{
				this.StopTracking();
			}
			this.Network_playerId = playerName;
			this.StartTracking();
			if (base.isLocalPlayer)
			{
				this.CmdSetPlayerName(playerName);
			}
		}

		// Token: 0x06001784 RID: 6020 RVA: 0x00076D73 File Offset: 0x00074F73
		public override void OnStartClient()
		{
			base.OnStartClient();
			if (!string.IsNullOrEmpty(this.PlayerId))
			{
				this.StartTracking();
			}
		}

		// Token: 0x06001785 RID: 6021 RVA: 0x00076D90 File Offset: 0x00074F90
		[Command]
		private void CmdSetPlayerName(string playerName)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteString(playerName);
			base.SendCommandInternal("System.Void Dissonance.Integrations.MirrorIgnorance.MirrorIgnorancePlayer::CmdSetPlayerName(System.String)", 1219872396, writer, 0, true);
			NetworkWriterPool.Return(writer);
		}

		// Token: 0x06001786 RID: 6022 RVA: 0x00076DCC File Offset: 0x00074FCC
		[ClientRpc]
		private void RpcSetPlayerName(string playerName)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteString(playerName);
			this.SendRPCInternal("System.Void Dissonance.Integrations.MirrorIgnorance.MirrorIgnorancePlayer::RpcSetPlayerName(System.String)", 1527750839, writer, 0, true);
			NetworkWriterPool.Return(writer);
		}

		// Token: 0x06001787 RID: 6023 RVA: 0x00076E06 File Offset: 0x00075006
		private void StartTracking()
		{
			if (this.IsTracking)
			{
				throw MirrorIgnorancePlayer.Log.CreatePossibleBugException("Attempting to start player tracking, but tracking is already started", "31971B1F-52FD-4FCF-89E9-67A17A917921");
			}
			if (this._comms != null)
			{
				this._comms.TrackPlayerPosition(this);
				this.IsTracking = true;
			}
		}

		// Token: 0x06001788 RID: 6024 RVA: 0x00076E46 File Offset: 0x00075046
		private void StopTracking()
		{
			if (!this.IsTracking)
			{
				throw MirrorIgnorancePlayer.Log.CreatePossibleBugException("Attempting to stop player tracking, but tracking is not started", "C7CF0174-0667-4F07-88E3-800ED652142D");
			}
			if (this._comms != null)
			{
				this._comms.StopTracking(this);
				this.IsTracking = false;
			}
		}

		// Token: 0x0600178A RID: 6026 RVA: 0x00076E88 File Offset: 0x00075088
		static MirrorIgnorancePlayer()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(MirrorIgnorancePlayer), "System.Void Dissonance.Integrations.MirrorIgnorance.MirrorIgnorancePlayer::CmdSetPlayerName(System.String)", new RemoteCallDelegate(MirrorIgnorancePlayer.InvokeUserCode_CmdSetPlayerName__String), true);
			RemoteProcedureCalls.RegisterRpc(typeof(MirrorIgnorancePlayer), "System.Void Dissonance.Integrations.MirrorIgnorance.MirrorIgnorancePlayer::RpcSetPlayerName(System.String)", new RemoteCallDelegate(MirrorIgnorancePlayer.InvokeUserCode_RpcSetPlayerName__String));
		}

		// Token: 0x0600178B RID: 6027 RVA: 0x0000C7D7 File Offset: 0x0000A9D7
		public override bool Weaved()
		{
			return true;
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x0600178C RID: 6028 RVA: 0x00076EE8 File Offset: 0x000750E8
		// (set) Token: 0x0600178D RID: 6029 RVA: 0x00076EFB File Offset: 0x000750FB
		public string Network_playerId
		{
			get
			{
				return this._playerId;
			}
			[param: In]
			set
			{
				base.GeneratedSyncVarSetter<string>(value, ref this._playerId, 1UL, null);
			}
		}

		// Token: 0x0600178E RID: 6030 RVA: 0x00076F15 File Offset: 0x00075115
		protected void UserCode_CmdSetPlayerName__String(string playerName)
		{
			this.Network_playerId = playerName;
			this.RpcSetPlayerName(playerName);
		}

		// Token: 0x0600178F RID: 6031 RVA: 0x00076F25 File Offset: 0x00075125
		protected static void InvokeUserCode_CmdSetPlayerName__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetPlayerName called on client.");
				return;
			}
			((MirrorIgnorancePlayer)obj).UserCode_CmdSetPlayerName__String(reader.ReadString());
		}

		// Token: 0x06001790 RID: 6032 RVA: 0x00076F4E File Offset: 0x0007514E
		protected void UserCode_RpcSetPlayerName__String(string playerName)
		{
			if (!base.isLocalPlayer)
			{
				this.SetPlayerName(playerName);
			}
		}

		// Token: 0x06001791 RID: 6033 RVA: 0x00076F5F File Offset: 0x0007515F
		protected static void InvokeUserCode_RpcSetPlayerName__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcSetPlayerName called on server.");
				return;
			}
			((MirrorIgnorancePlayer)obj).UserCode_RpcSetPlayerName__String(reader.ReadString());
		}

		// Token: 0x06001792 RID: 6034 RVA: 0x00076F88 File Offset: 0x00075188
		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteString(this._playerId);
				return;
			}
			writer.WriteULong(base.syncVarDirtyBits);
			if ((base.syncVarDirtyBits & 1UL) != 0UL)
			{
				writer.WriteString(this._playerId);
			}
		}

		// Token: 0x06001793 RID: 6035 RVA: 0x00076FE0 File Offset: 0x000751E0
		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				base.GeneratedSyncVarDeserialize<string>(ref this._playerId, null, reader.ReadString());
				return;
			}
			long num = (long)reader.ReadULong();
			if ((num & 1L) != 0L)
			{
				base.GeneratedSyncVarDeserialize<string>(ref this._playerId, null, reader.ReadString());
			}
		}

		// Token: 0x0400115E RID: 4446
		private static readonly Log Log = Logs.Create(LogCategory.Network, "Mirror Player Component");

		// Token: 0x0400115F RID: 4447
		private DissonanceComms _comms;

		// Token: 0x04001161 RID: 4449
		[SyncVar]
		private string _playerId;
	}
}
