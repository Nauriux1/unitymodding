using System;
using System.Runtime.InteropServices;
using Mirror.RemoteCalls;
using UnityEngine;

namespace Mirror.Experimental
{
	// Token: 0x020002BD RID: 701
	[AddComponentMenu("Network/Experimental/NetworkJoint")]
	[HelpURL("https://mirror-networking.com/docs/Components/NetworkRigidbody.html")]
	public class NetworkJoint : NetworkBehaviour
	{
		// Token: 0x060014FC RID: 5372 RVA: 0x00069436 File Offset: 0x00067636
		private new void OnValidate()
		{
			if (this.target == null)
			{
				this.target = base.GetComponent<ConfigurableJoint>();
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x060014FD RID: 5373 RVA: 0x00069452 File Offset: 0x00067652
		private bool IgnoreSync
		{
			get
			{
				return base.isServer || this.ClientWithAuthority;
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x060014FE RID: 5374 RVA: 0x00069464 File Offset: 0x00067664
		private bool ClientWithAuthority
		{
			get
			{
				return this.clientAuthority && base.hasAuthority;
			}
		}

		// Token: 0x060014FF RID: 5375 RVA: 0x00069476 File Offset: 0x00067676
		private void OnTargetRotationChanged(Quaternion _, Quaternion newValue)
		{
			if (this.IgnoreSync)
			{
				return;
			}
			this.target.targetRotation = newValue;
		}

		// Token: 0x06001500 RID: 5376 RVA: 0x0006948D File Offset: 0x0006768D
		internal void Update()
		{
			if (base.isServer)
			{
				this.SyncToClients();
				return;
			}
			if (this.ClientWithAuthority)
			{
				this.SendToServer();
			}
		}

		// Token: 0x06001501 RID: 5377 RVA: 0x000694AC File Offset: 0x000676AC
		[Server]
		private void SyncToClients()
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void Mirror.Experimental.NetworkJoint::SyncToClients()' called when server was not active");
				return;
			}
			Quaternion networktargetRotation = this.syncTargetRotation ? this.target.targetRotation : default(Quaternion);
			if (this.syncTargetRotation)
			{
				this.NetworktargetRotation = networktargetRotation;
				this.previousValue.targetRotation = networktargetRotation;
			}
		}

		// Token: 0x06001502 RID: 5378 RVA: 0x00069508 File Offset: 0x00067708
		[Client]
		private void SendToServer()
		{
			if (!NetworkClient.active)
			{
				Debug.LogWarning("[Client] function 'System.Void Mirror.Experimental.NetworkJoint::SendToServer()' called when client was not active");
				return;
			}
			if (!base.hasAuthority)
			{
				return;
			}
			this.SendTargetRotation();
		}

		// Token: 0x06001503 RID: 5379 RVA: 0x00069530 File Offset: 0x00067730
		[Client]
		private void SendTargetRotation()
		{
			if (!NetworkClient.active)
			{
				Debug.LogWarning("[Client] function 'System.Void Mirror.Experimental.NetworkJoint::SendTargetRotation()' called when client was not active");
				return;
			}
			float time = Time.time;
			if (time < this.previousValue.nextSyncTime)
			{
				return;
			}
			Quaternion quaternion = this.syncTargetRotation ? this.target.targetRotation : default(Quaternion);
			bool flag = this.syncTargetRotation;
			if (flag)
			{
				this.CmdSendTargetRotation(quaternion);
				this.previousValue.targetRotation = quaternion;
			}
			if (flag)
			{
				this.previousValue.nextSyncTime = time + this.syncInterval;
			}
		}

		// Token: 0x06001504 RID: 5380 RVA: 0x000695B8 File Offset: 0x000677B8
		[Command]
		private void CmdSendTargetRotation(Quaternion targetRotation)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteQuaternion(targetRotation);
			base.SendCommandInternal("System.Void Mirror.Experimental.NetworkJoint::CmdSendTargetRotation(UnityEngine.Quaternion)", -923498394, writer, 0, true);
			NetworkWriterPool.Return(writer);
		}

		// Token: 0x06001506 RID: 5382 RVA: 0x0000C7D7 File Offset: 0x0000A9D7
		public override bool Weaved()
		{
			return true;
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06001507 RID: 5383 RVA: 0x0006960C File Offset: 0x0006780C
		// (set) Token: 0x06001508 RID: 5384 RVA: 0x0006961F File Offset: 0x0006781F
		public Quaternion NetworktargetRotation
		{
			get
			{
				return this.targetRotation;
			}
			[param: In]
			set
			{
				base.GeneratedSyncVarSetter<Quaternion>(value, ref this.targetRotation, 1UL, new Action<Quaternion, Quaternion>(this.OnTargetRotationChanged));
			}
		}

		// Token: 0x06001509 RID: 5385 RVA: 0x00069644 File Offset: 0x00067844
		protected void UserCode_CmdSendTargetRotation__Quaternion(Quaternion targetRotation)
		{
			if (!this.clientAuthority)
			{
				return;
			}
			this.NetworktargetRotation = targetRotation;
			this.target.targetRotation = targetRotation;
		}

		// Token: 0x0600150A RID: 5386 RVA: 0x00069662 File Offset: 0x00067862
		protected static void InvokeUserCode_CmdSendTargetRotation__Quaternion(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSendTargetRotation called on client.");
				return;
			}
			((NetworkJoint)obj).UserCode_CmdSendTargetRotation__Quaternion(reader.ReadQuaternion());
		}

		// Token: 0x0600150B RID: 5387 RVA: 0x0006968B File Offset: 0x0006788B
		static NetworkJoint()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkJoint), "System.Void Mirror.Experimental.NetworkJoint::CmdSendTargetRotation(UnityEngine.Quaternion)", new RemoteCallDelegate(NetworkJoint.InvokeUserCode_CmdSendTargetRotation__Quaternion), true);
		}

		// Token: 0x0600150C RID: 5388 RVA: 0x000696B0 File Offset: 0x000678B0
		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteQuaternion(this.targetRotation);
				return;
			}
			writer.WriteULong(base.syncVarDirtyBits);
			if ((base.syncVarDirtyBits & 1UL) != 0UL)
			{
				writer.WriteQuaternion(this.targetRotation);
			}
		}

		// Token: 0x0600150D RID: 5389 RVA: 0x00069708 File Offset: 0x00067908
		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				base.GeneratedSyncVarDeserialize<Quaternion>(ref this.targetRotation, new Action<Quaternion, Quaternion>(this.OnTargetRotationChanged), reader.ReadQuaternion());
				return;
			}
			long num = (long)reader.ReadULong();
			if ((num & 1L) != 0L)
			{
				base.GeneratedSyncVarDeserialize<Quaternion>(ref this.targetRotation, new Action<Quaternion, Quaternion>(this.OnTargetRotationChanged), reader.ReadQuaternion());
			}
		}

		// Token: 0x04000F77 RID: 3959
		[Header("Settings")]
		[SerializeField]
		public ConfigurableJoint target;

		// Token: 0x04000F78 RID: 3960
		[Tooltip("Set to true if moves come from owner client, set to false if moves always come from server")]
		[SerializeField]
		public bool clientAuthority;

		// Token: 0x04000F79 RID: 3961
		[SerializeField]
		private bool syncTargetRotation = true;

		// Token: 0x04000F7A RID: 3962
		private readonly NetworkJoint.ClientSyncState previousValue = new NetworkJoint.ClientSyncState();

		// Token: 0x04000F7B RID: 3963
		[SyncVar(hook = "OnTargetRotationChanged")]
		private Quaternion targetRotation;

		// Token: 0x020002BE RID: 702
		public class ClientSyncState
		{
			// Token: 0x04000F7C RID: 3964
			public float nextSyncTime;

			// Token: 0x04000F7D RID: 3965
			public Quaternion targetRotation;
		}
	}
}
