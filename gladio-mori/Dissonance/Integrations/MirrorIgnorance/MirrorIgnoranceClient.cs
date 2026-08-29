using System;
using Dissonance.Networking;
using JetBrains.Annotations;
using Mirror;

namespace Dissonance.Integrations.MirrorIgnorance
{
	// Token: 0x0200030A RID: 778
	public class MirrorIgnoranceClient : BaseClient<MirrorIgnoranceServer, MirrorIgnoranceClient, MirrorConn>
	{
		// Token: 0x06001759 RID: 5977 RVA: 0x0007670F File Offset: 0x0007490F
		public MirrorIgnoranceClient([NotNull] MirrorIgnoranceCommsNetwork network) : base(network)
		{
			if (network == null)
			{
				throw new ArgumentNullException("network");
			}
			this._network = network;
		}

		// Token: 0x0600175A RID: 5978 RVA: 0x00076733 File Offset: 0x00074933
		public override void Connect()
		{
			if (!this._network.Mode.IsServerEnabled())
			{
				NetworkClient.ReplaceHandler<DissonanceNetworkMessage>(new Action<NetworkConnection, DissonanceNetworkMessage>(this.OnMessageReceived), true);
			}
			base.Connected();
		}

		// Token: 0x0600175B RID: 5979 RVA: 0x0007675F File Offset: 0x0007495F
		public override void Disconnect()
		{
			if (!this._network.Mode.IsServerEnabled())
			{
				NetworkClient.ReplaceHandler<DissonanceNetworkMessage>(new Action<NetworkConnection, DissonanceNetworkMessage>(MirrorIgnoranceCommsNetwork.NullMessageReceivedHandler), true);
			}
			base.Disconnect();
		}

		// Token: 0x0600175C RID: 5980 RVA: 0x0007678C File Offset: 0x0007498C
		private void OnMessageReceived(NetworkConnection source, DissonanceNetworkMessage msg)
		{
			using (msg)
			{
				base.NetworkReceivedPacket(msg.Data);
			}
		}

		// Token: 0x0600175D RID: 5981 RVA: 0x0000777A File Offset: 0x0000597A
		protected override void ReadMessages()
		{
		}

		// Token: 0x0600175E RID: 5982 RVA: 0x000767C8 File Offset: 0x000749C8
		protected override void SendReliable(ArraySegment<byte> packet)
		{
			if (!this.Send(packet, 0))
			{
				base.FatalError("Failed to send reliable packet (unknown Mirror error)");
			}
		}

		// Token: 0x0600175F RID: 5983 RVA: 0x000767DF File Offset: 0x000749DF
		protected override void SendUnreliable(ArraySegment<byte> packet)
		{
			this.Send(packet, 1);
		}

		// Token: 0x06001760 RID: 5984 RVA: 0x000767EA File Offset: 0x000749EA
		private bool Send(ArraySegment<byte> packet, byte channel)
		{
			if (this._network.PreprocessPacketToServer(packet))
			{
				return true;
			}
			NetworkClient.connection.Send<DissonanceNetworkMessage>(new DissonanceNetworkMessage(packet), (int)channel);
			return true;
		}

		// Token: 0x04001152 RID: 4434
		private readonly MirrorIgnoranceCommsNetwork _network;
	}
}
