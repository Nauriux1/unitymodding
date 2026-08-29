using System;
using Steamworks;
using Steamworks.Data;
using UnityEngine;

namespace Mirror.FizzySteam
{
	// Token: 0x020002D3 RID: 723
	public class NextServer : NextCommon, IServer
	{
		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06001618 RID: 5656 RVA: 0x0006D6F4 File Offset: 0x0006B8F4
		// (remove) Token: 0x06001619 RID: 5657 RVA: 0x0006D72C File Offset: 0x0006B92C
		private event Action<int> OnConnected;

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x0600161A RID: 5658 RVA: 0x0006D764 File Offset: 0x0006B964
		// (remove) Token: 0x0600161B RID: 5659 RVA: 0x0006D79C File Offset: 0x0006B99C
		private event Action<int, byte[], int> OnReceivedData;

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x0600161C RID: 5660 RVA: 0x0006D7D4 File Offset: 0x0006B9D4
		// (remove) Token: 0x0600161D RID: 5661 RVA: 0x0006D80C File Offset: 0x0006BA0C
		private event Action<int> OnDisconnected;

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x0600161E RID: 5662 RVA: 0x0006D844 File Offset: 0x0006BA44
		// (remove) Token: 0x0600161F RID: 5663 RVA: 0x0006D87C File Offset: 0x0006BA7C
		private event Action<int, Exception> OnReceivedError;

		// Token: 0x06001620 RID: 5664 RVA: 0x0006D8B1 File Offset: 0x0006BAB1
		private NextServer(int maxConnections)
		{
			this.maxConnections = maxConnections;
			this.connToMirrorID = new BidirectionalDictionary<Connection, int>();
			this.steamIDToMirrorID = new BidirectionalDictionary<SteamId, int>();
			this.nextConnectionID = 1;
			SteamNetworkingSockets.OnConnectionStatusChanged += this.OnConnectionStatusChanged;
		}

		// Token: 0x06001621 RID: 5665 RVA: 0x0006D8F0 File Offset: 0x0006BAF0
		public static NextServer CreateServer(FizzyFacepunch transport, int maxConnections)
		{
			NextServer nextServer = new NextServer(maxConnections);
			nextServer.OnConnected += delegate(int id)
			{
				transport.OnServerConnected(id);
			};
			nextServer.OnDisconnected += delegate(int id)
			{
				transport.OnServerDisconnected(id);
			};
			nextServer.OnReceivedData += delegate(int id, byte[] data, int ch)
			{
				transport.OnServerDataReceived(id, new ArraySegment<byte>(data), ch);
			};
			nextServer.OnReceivedError += delegate(int id, Exception exception)
			{
				transport.OnServerError(id, TransportError.Unexpected, exception.Message);
			};
			if (!SteamClient.IsValid)
			{
				Debug.LogError("SteamWorks not initialized.");
			}
			nextServer.Host();
			return nextServer;
		}

		// Token: 0x06001622 RID: 5666 RVA: 0x0006D96F File Offset: 0x0006BB6F
		private void Host()
		{
			this.listenSocket = SteamNetworkingSockets.CreateRelaySocket<FizzySocketManager>(0);
			this.listenSocket.ForwardMessage = new Action<Connection, IntPtr, int>(this.OnMessageReceived);
		}

		// Token: 0x06001623 RID: 5667 RVA: 0x0006D994 File Offset: 0x0006BB94
		private void OnConnectionStatusChanged(Connection conn, ConnectionInfo info)
		{
			ulong num = info.Identity.SteamId;
			if (info.State == ConnectionState.Connecting)
			{
				if (this.connToMirrorID.Count >= this.maxConnections)
				{
					Debug.Log(string.Format("Incoming connection {0} would exceed max connection count. Rejecting.", num));
					conn.Close(false, 0, "Max Connection Count");
					return;
				}
				Result result;
				if ((result = conn.Accept()) == Result.OK)
				{
					Debug.Log(string.Format("Accepting connection {0}", num));
					return;
				}
				Debug.Log(string.Format("Connection {0} could not be accepted: {1}", num, result.ToString()));
				return;
			}
			else
			{
				if (info.State == ConnectionState.Connected)
				{
					int num2 = this.nextConnectionID;
					this.nextConnectionID = num2 + 1;
					int num3 = num2;
					this.connToMirrorID.Add(conn, num3);
					this.steamIDToMirrorID.Add(num, num3);
					this.OnConnected(num3);
					Debug.Log(string.Format("Client with SteamID {0} connected. Assigning connection id {1}", num, num3));
					return;
				}
				if (info.State == ConnectionState.ClosedByPeer)
				{
					int connId;
					if (this.connToMirrorID.TryGetValue(conn, out connId))
					{
						this.InternalDisconnect(connId, conn);
						return;
					}
				}
				else
				{
					Debug.Log(string.Format("Connection {0} state changed: {1}", num, info.State.ToString()));
				}
				return;
			}
		}

		// Token: 0x06001624 RID: 5668 RVA: 0x0006DAF4 File Offset: 0x0006BCF4
		private void InternalDisconnect(int connId, Connection socket)
		{
			this.OnDisconnected(connId);
			socket.Close(false, 0, "Graceful disconnect");
			this.connToMirrorID.Remove(connId);
			this.steamIDToMirrorID.Remove(connId);
			Debug.Log(string.Format("Client with SteamID {0} disconnected.", connId));
		}

		// Token: 0x06001625 RID: 5669 RVA: 0x0006DB4C File Offset: 0x0006BD4C
		public void Disconnect(int connectionId)
		{
			Connection connection;
			if (this.connToMirrorID.TryGetValue(connectionId, out connection))
			{
				Debug.Log(string.Format("Connection id {0} disconnected.", connectionId));
				connection.Close(false, 0, "Disconnected by server");
				this.steamIDToMirrorID.Remove(connectionId);
				this.connToMirrorID.Remove(connectionId);
				this.OnDisconnected(connectionId);
				return;
			}
			Debug.LogWarning("Trying to disconnect unknown connection id: " + connectionId.ToString());
		}

		// Token: 0x06001626 RID: 5670 RVA: 0x0006DBC8 File Offset: 0x0006BDC8
		public void FlushData()
		{
			foreach (Connection connection in this.connToMirrorID.FirstTypes)
			{
				connection.Flush();
			}
		}

		// Token: 0x06001627 RID: 5671 RVA: 0x0006DC1C File Offset: 0x0006BE1C
		public void ReceiveData()
		{
			this.listenSocket.Receive(256);
		}

		// Token: 0x06001628 RID: 5672 RVA: 0x0006DC30 File Offset: 0x0006BE30
		private void OnMessageReceived(Connection conn, IntPtr dataPtr, int size)
		{
			ValueTuple<byte[], int> valueTuple = base.ProcessMessage(dataPtr, size);
			byte[] item = valueTuple.Item1;
			int item2 = valueTuple.Item2;
			this.OnReceivedData(this.connToMirrorID[conn], item, item2);
		}

		// Token: 0x06001629 RID: 5673 RVA: 0x0006DC6C File Offset: 0x0006BE6C
		public void Send(int connectionId, byte[] data, int channelId)
		{
			Connection connection;
			if (this.connToMirrorID.TryGetValue(connectionId, out connection))
			{
				Result result = base.SendSocket(connection, data, channelId);
				if (result == Result.NoConnection || result == Result.InvalidParam)
				{
					Debug.Log(string.Format("Connection to {0} was lost.", connectionId));
					this.InternalDisconnect(connectionId, connection);
					return;
				}
				if (result != Result.OK)
				{
					Debug.LogError("Could not send: " + result.ToString());
					return;
				}
			}
			else
			{
				Debug.LogError("Trying to send on unknown connection: " + connectionId.ToString());
				this.OnReceivedError(connectionId, new Exception("ERROR Unknown Connection"));
			}
		}

		// Token: 0x0600162A RID: 5674 RVA: 0x0006DD08 File Offset: 0x0006BF08
		public string ServerGetClientAddress(int connectionId)
		{
			SteamId steamId;
			if (this.steamIDToMirrorID.TryGetValue(connectionId, out steamId))
			{
				return steamId.ToString();
			}
			Debug.LogError("Trying to get info on unknown connection: " + connectionId.ToString());
			this.OnReceivedError(connectionId, new Exception("ERROR Unknown Connection"));
			return string.Empty;
		}

		// Token: 0x0600162B RID: 5675 RVA: 0x0006DD64 File Offset: 0x0006BF64
		public void Shutdown()
		{
			if (this.listenSocket != null)
			{
				SteamNetworkingSockets.OnConnectionStatusChanged -= this.OnConnectionStatusChanged;
				this.listenSocket.Close();
			}
		}

		// Token: 0x04000FFC RID: 4092
		private BidirectionalDictionary<Connection, int> connToMirrorID;

		// Token: 0x04000FFD RID: 4093
		private BidirectionalDictionary<SteamId, int> steamIDToMirrorID;

		// Token: 0x04000FFE RID: 4094
		private int maxConnections;

		// Token: 0x04000FFF RID: 4095
		private int nextConnectionID;

		// Token: 0x04001000 RID: 4096
		private FizzySocketManager listenSocket;
	}
}
