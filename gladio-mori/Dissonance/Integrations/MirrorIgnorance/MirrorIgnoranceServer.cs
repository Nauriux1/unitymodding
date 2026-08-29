using System;
using System.Collections.Generic;
using Dissonance.Networking;
using Dissonance.Networking.Server;
using JetBrains.Annotations;
using Mirror;

namespace Dissonance.Integrations.MirrorIgnorance
{
	// Token: 0x02000312 RID: 786
	public class MirrorIgnoranceServer : BaseServer<MirrorIgnoranceServer, MirrorIgnoranceClient, MirrorConn>
	{
		// Token: 0x06001794 RID: 6036 RVA: 0x0007703B File Offset: 0x0007523B
		public MirrorIgnoranceServer([NotNull] MirrorIgnoranceCommsNetwork network)
		{
			if (network == null)
			{
				throw new ArgumentNullException("network");
			}
			this._network = network;
		}

		// Token: 0x06001795 RID: 6037 RVA: 0x00077069 File Offset: 0x00075269
		public override void Connect()
		{
			NetworkServer.ReplaceHandler<DissonanceNetworkMessage>(new Action<NetworkConnectionToClient, DissonanceNetworkMessage>(this.OnMessageReceived), true);
			base.Connect();
		}

		// Token: 0x06001796 RID: 6038 RVA: 0x00077084 File Offset: 0x00075284
		private void OnMessageReceived(NetworkConnection source, DissonanceNetworkMessage msg)
		{
			using (msg)
			{
				base.NetworkReceivedPacket(new MirrorConn(source), msg.Data);
			}
		}

		// Token: 0x06001797 RID: 6039 RVA: 0x000770C8 File Offset: 0x000752C8
		protected override void AddClient([NotNull] ClientInfo<MirrorConn> client)
		{
			base.AddClient(client);
			if (client.PlayerName != this._network.PlayerName)
			{
				this._addedConnections.Add(client.Connection.Connection);
			}
		}

		// Token: 0x06001798 RID: 6040 RVA: 0x000770FF File Offset: 0x000752FF
		public override void Disconnect()
		{
			base.Disconnect();
			NetworkServer.ReplaceHandler<DissonanceNetworkMessage>(new Action<NetworkConnectionToClient, DissonanceNetworkMessage>(MirrorIgnoranceCommsNetwork.NullMessageReceivedHandler), true);
		}

		// Token: 0x06001799 RID: 6041 RVA: 0x0000777A File Offset: 0x0000597A
		protected override void ReadMessages()
		{
		}

		// Token: 0x0600179A RID: 6042 RVA: 0x0007711C File Offset: 0x0007531C
		public override ServerState Update()
		{
			for (int i = this._addedConnections.Count - 1; i >= 0; i--)
			{
				if (!MirrorIgnoranceServer.IsConnected(this._addedConnections[i]))
				{
					base.ClientDisconnected(new MirrorConn(this._addedConnections[i]));
					this._addedConnections.RemoveAt(i);
				}
			}
			return base.Update();
		}

		// Token: 0x0600179B RID: 6043 RVA: 0x0007717D File Offset: 0x0007537D
		private static bool IsConnected([NotNull] NetworkConnection conn)
		{
			return NetworkServer.connections.ContainsKey(conn.connectionId);
		}

		// Token: 0x0600179C RID: 6044 RVA: 0x0007718F File Offset: 0x0007538F
		protected override void SendReliable(MirrorConn connection, ArraySegment<byte> packet)
		{
			if (!this.Send(packet, connection, 0))
			{
				base.FatalError("Failed to send reliable packet (unknown Mirror error)");
			}
		}

		// Token: 0x0600179D RID: 6045 RVA: 0x000771A7 File Offset: 0x000753A7
		protected override void SendUnreliable(MirrorConn connection, ArraySegment<byte> packet)
		{
			this.Send(packet, connection, 1);
		}

		// Token: 0x0600179E RID: 6046 RVA: 0x000771B4 File Offset: 0x000753B4
		private bool Send(ArraySegment<byte> packet, MirrorConn connection, byte channel)
		{
			if (this._network.PreprocessPacketToClient(packet, connection))
			{
				return true;
			}
			if (!MirrorIgnoranceServer.IsConnected(connection.Connection))
			{
				return true;
			}
			if (connection.Connection == null)
			{
				this.Log.Error("Cannot send to a null destination");
				return false;
			}
			connection.Connection.Send<DissonanceNetworkMessage>(new DissonanceNetworkMessage(packet), (int)channel);
			return true;
		}

		// Token: 0x04001162 RID: 4450
		[NotNull]
		private readonly MirrorIgnoranceCommsNetwork _network;

		// Token: 0x04001163 RID: 4451
		private readonly List<NetworkConnection> _addedConnections = new List<NetworkConnection>();
	}
}
