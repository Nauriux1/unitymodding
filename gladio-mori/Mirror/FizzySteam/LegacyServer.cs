using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace Mirror.FizzySteam
{
	// Token: 0x020002CC RID: 716
	public class LegacyServer : LegacyCommon, IServer
	{
		// Token: 0x1400000C RID: 12
		// (add) Token: 0x060015DF RID: 5599 RVA: 0x0006C8DC File Offset: 0x0006AADC
		// (remove) Token: 0x060015E0 RID: 5600 RVA: 0x0006C914 File Offset: 0x0006AB14
		private event Action<int> OnConnected;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x060015E1 RID: 5601 RVA: 0x0006C94C File Offset: 0x0006AB4C
		// (remove) Token: 0x060015E2 RID: 5602 RVA: 0x0006C984 File Offset: 0x0006AB84
		private event Action<int, byte[], int> OnReceivedData;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x060015E3 RID: 5603 RVA: 0x0006C9BC File Offset: 0x0006ABBC
		// (remove) Token: 0x060015E4 RID: 5604 RVA: 0x0006C9F4 File Offset: 0x0006ABF4
		private event Action<int> OnDisconnected;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x060015E5 RID: 5605 RVA: 0x0006CA2C File Offset: 0x0006AC2C
		// (remove) Token: 0x060015E6 RID: 5606 RVA: 0x0006CA64 File Offset: 0x0006AC64
		private event Action<int, Exception> OnReceivedError;

		// Token: 0x060015E7 RID: 5607 RVA: 0x0006CA9C File Offset: 0x0006AC9C
		public static LegacyServer CreateServer(FizzyFacepunch transport, int maxConnections)
		{
			LegacyServer legacyServer = new LegacyServer(transport, maxConnections);
			legacyServer.OnConnected += delegate(int id)
			{
				transport.OnServerConnected(id);
			};
			legacyServer.OnDisconnected += delegate(int id)
			{
				transport.OnServerDisconnected(id);
			};
			legacyServer.OnReceivedData += delegate(int id, byte[] data, int channel)
			{
				transport.OnServerDataReceived(id, new ArraySegment<byte>(data), channel);
			};
			legacyServer.OnReceivedError += delegate(int id, Exception exception)
			{
				transport.OnServerError(id, TransportError.Unexpected, exception.Message);
			};
			SteamNetworking.OnP2PSessionRequest = delegate(SteamId steamid)
			{
				Debug.Log(string.Format("Incoming request from SteamId {0}.", steamid));
				SteamNetworking.AcceptP2PSessionWithUser(steamid);
			};
			if (!SteamClient.IsValid)
			{
				Debug.LogError("SteamWorks not initialized.");
			}
			return legacyServer;
		}

		// Token: 0x060015E8 RID: 5608 RVA: 0x0006CB3F File Offset: 0x0006AD3F
		private LegacyServer(FizzyFacepunch transport, int maxConnections) : base(transport)
		{
			this.maxConnections = maxConnections;
			this.steamToMirrorIds = new BidirectionalDictionary<SteamId, int>();
			this.nextConnectionID = 1;
		}

		// Token: 0x060015E9 RID: 5609 RVA: 0x0006CB61 File Offset: 0x0006AD61
		protected override void OnNewConnection(SteamId id)
		{
			SteamNetworking.AcceptP2PSessionWithUser(id);
		}

		// Token: 0x060015EA RID: 5610 RVA: 0x0006CB6C File Offset: 0x0006AD6C
		protected override void OnReceiveInternalData(LegacyCommon.InternalMessages type, SteamId clientSteamID)
		{
			if (type != LegacyCommon.InternalMessages.CONNECT)
			{
				if (type != LegacyCommon.InternalMessages.DISCONNECT)
				{
					Debug.Log("Received unknown message type");
					return;
				}
				int obj;
				if (this.steamToMirrorIds.TryGetValue(clientSteamID, out obj))
				{
					this.OnDisconnected(obj);
					base.CloseP2PSessionWithUser(clientSteamID);
					this.steamToMirrorIds.Remove(clientSteamID);
					Debug.Log(string.Format("Client with SteamID {0} disconnected.", clientSteamID));
					return;
				}
				this.OnReceivedError(-1, new Exception("ERROR Unknown SteamID while receiving disconnect message."));
				return;
			}
			else
			{
				if (this.steamToMirrorIds.Count >= this.maxConnections)
				{
					base.SendInternal(clientSteamID, LegacyCommon.InternalMessages.DISCONNECT);
					return;
				}
				base.SendInternal(clientSteamID, LegacyCommon.InternalMessages.ACCEPT_CONNECT);
				int num = this.nextConnectionID;
				this.nextConnectionID = num + 1;
				int num2 = num;
				this.steamToMirrorIds.Add(clientSteamID, num2);
				this.OnConnected(num2);
				Debug.Log(string.Format("Client with SteamID {0} connected. Assigning connection id {1}", clientSteamID, num2));
				return;
			}
		}

		// Token: 0x060015EB RID: 5611 RVA: 0x0006CC58 File Offset: 0x0006AE58
		protected override void OnReceiveData(byte[] data, SteamId clientSteamID, int channel)
		{
			int arg;
			if (this.steamToMirrorIds.TryGetValue(clientSteamID, out arg))
			{
				this.OnReceivedData(arg, data, channel);
				return;
			}
			base.CloseP2PSessionWithUser(clientSteamID);
			string str = "Data received from steam client thats not known ";
			SteamId steamId = clientSteamID;
			Debug.LogError(str + steamId.ToString());
			this.OnReceivedError(-1, new Exception("ERROR Unknown SteamID"));
		}

		// Token: 0x060015EC RID: 5612 RVA: 0x0006CCC0 File Offset: 0x0006AEC0
		public void Disconnect(int connectionId)
		{
			SteamId target;
			if (this.steamToMirrorIds.TryGetValue(connectionId, out target))
			{
				base.SendInternal(target, LegacyCommon.InternalMessages.DISCONNECT);
				this.steamToMirrorIds.Remove(connectionId);
				return;
			}
			Debug.LogWarning("Trying to disconnect unknown connection id: " + connectionId.ToString());
		}

		// Token: 0x060015ED RID: 5613 RVA: 0x0006CD0C File Offset: 0x0006AF0C
		public void Shutdown()
		{
			foreach (object obj in this.steamToMirrorIds)
			{
				KeyValuePair<SteamId, int> keyValuePair = (KeyValuePair<SteamId, int>)obj;
				this.Disconnect(keyValuePair.Value);
				base.WaitForClose(keyValuePair.Key);
			}
			SteamNetworking.OnP2PSessionRequest = null;
			base.Dispose();
		}

		// Token: 0x060015EE RID: 5614 RVA: 0x0006CD84 File Offset: 0x0006AF84
		public void Send(int connectionId, byte[] data, int channelId)
		{
			SteamId host;
			if (this.steamToMirrorIds.TryGetValue(connectionId, out host))
			{
				base.Send(host, data, channelId);
				return;
			}
			Debug.LogError("Trying to send on unknown connection: " + connectionId.ToString());
			this.OnReceivedError(connectionId, new Exception("ERROR Unknown Connection"));
		}

		// Token: 0x060015EF RID: 5615 RVA: 0x0006CDD8 File Offset: 0x0006AFD8
		public string ServerGetClientAddress(int connectionId)
		{
			SteamId steamId;
			if (this.steamToMirrorIds.TryGetValue(connectionId, out steamId))
			{
				return steamId.ToString();
			}
			Debug.LogError("Trying to get info on unknown connection: " + connectionId.ToString());
			this.OnReceivedError(connectionId, new Exception("ERROR Unknown Connection"));
			return string.Empty;
		}

		// Token: 0x060015F0 RID: 5616 RVA: 0x0006CE34 File Offset: 0x0006B034
		protected override void OnConnectionFailed(SteamId remoteId)
		{
			int num;
			int num3;
			if (!this.steamToMirrorIds.TryGetValue(remoteId, out num))
			{
				int num2 = this.nextConnectionID;
				this.nextConnectionID = num2 + 1;
				num3 = num2;
			}
			else
			{
				num3 = num;
			}
			int obj = num3;
			this.OnDisconnected(obj);
		}

		// Token: 0x060015F1 RID: 5617 RVA: 0x0000777A File Offset: 0x0000597A
		public void FlushData()
		{
		}

		// Token: 0x04000FDC RID: 4060
		private BidirectionalDictionary<SteamId, int> steamToMirrorIds;

		// Token: 0x04000FDD RID: 4061
		private int maxConnections;

		// Token: 0x04000FDE RID: 4062
		private int nextConnectionID;
	}
}
