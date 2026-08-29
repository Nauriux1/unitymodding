using System;
using System.Collections.Generic;
using Dissonance.Datastructures;
using Dissonance.Extensions;
using Dissonance.Networking;
using Mirror;
using UnityEngine;

namespace Dissonance.Integrations.MirrorIgnorance
{
	// Token: 0x0200030B RID: 779
	[HelpURL("https://placeholder-software.co.uk/dissonance/docs/Basics/Quick-Start-MirrorIgnorance/")]
	public class MirrorIgnoranceCommsNetwork : BaseCommsNetwork<MirrorIgnoranceServer, MirrorIgnoranceClient, MirrorConn, Unit, Unit>
	{
		// Token: 0x06001761 RID: 5985 RVA: 0x0007680E File Offset: 0x00074A0E
		protected override MirrorIgnoranceServer CreateServer(Unit details)
		{
			return new MirrorIgnoranceServer(this);
		}

		// Token: 0x06001762 RID: 5986 RVA: 0x00076816 File Offset: 0x00074A16
		protected override MirrorIgnoranceClient CreateClient(Unit details)
		{
			return new MirrorIgnoranceClient(this);
		}

		// Token: 0x06001763 RID: 5987 RVA: 0x00076820 File Offset: 0x00074A20
		protected override void Update()
		{
			if (base.IsInitialized)
			{
				if (NetworkManager.singleton != null && NetworkManager.singleton.isNetworkActive && (NetworkServer.active || NetworkClient.active) && (!NetworkClient.active || (NetworkClient.connection != null && NetworkClient.connection.isReady)))
				{
					bool active = NetworkServer.active;
					bool active2 = NetworkClient.active;
					if (base.Mode.IsServerEnabled() != active || base.Mode.IsClientEnabled() != active2)
					{
						if (active && active2)
						{
							base.RunAsHost(Unit.None, Unit.None);
						}
						else if (active)
						{
							base.RunAsDedicatedServer(Unit.None);
						}
						else if (active2)
						{
							base.RunAsClient(Unit.None);
						}
					}
				}
				else if (base.Mode != NetworkMode.None)
				{
					base.Stop();
					this._loopbackQueue.Clear();
				}
				for (int i = 0; i < this._loopbackQueue.Count; i++)
				{
					MirrorIgnoranceClient client = base.Client;
					if (client != null)
					{
						client.NetworkReceivedPacket(this._loopbackQueue[i]);
					}
					this._loopbackBuffers.Put(this._loopbackQueue[i].Array);
				}
				this._loopbackQueue.Clear();
			}
			base.Update();
		}

		// Token: 0x06001764 RID: 5988 RVA: 0x00076958 File Offset: 0x00074B58
		protected override void Initialize()
		{
			NetworkServer.ReplaceHandler<DissonanceNetworkMessage>(new Action<NetworkConnectionToClient, DissonanceNetworkMessage>(MirrorIgnoranceCommsNetwork.NullMessageReceivedHandler), true);
			base.Initialize();
		}

		// Token: 0x06001765 RID: 5989 RVA: 0x00076974 File Offset: 0x00074B74
		internal bool PreprocessPacketToClient(ArraySegment<byte> packet, MirrorConn destination)
		{
			if (base.Server == null)
			{
				throw this.Log.CreatePossibleBugException("server packet preprocessing running, but this peer is not a server", "8f9dc0a0-1b48-4a7f-9bb6-f767b2542ab1");
			}
			if (base.Client == null)
			{
				return false;
			}
			if (NetworkClient.connection != destination.Connection)
			{
				return false;
			}
			if (base.Client != null)
			{
				this._loopbackQueue.Add(packet.CopyToSegment(this._loopbackBuffers.Get(), 0));
			}
			return true;
		}

		// Token: 0x06001766 RID: 5990 RVA: 0x000769E0 File Offset: 0x00074BE0
		internal bool PreprocessPacketToServer(ArraySegment<byte> packet)
		{
			if (base.Client == null)
			{
				throw this.Log.CreatePossibleBugException("client packet processing running, but this peer is not a client", "dd75dce4-e85c-4bb3-96ec-3a3636cc4fbe");
			}
			if (base.Server == null)
			{
				return false;
			}
			base.Server.NetworkReceivedPacket(new MirrorConn(NetworkClient.connection), packet);
			return true;
		}

		// Token: 0x06001767 RID: 5991 RVA: 0x00076A2C File Offset: 0x00074C2C
		internal static void NullMessageReceivedHandler(NetworkConnection source, DissonanceNetworkMessage msg)
		{
			if (Logs.GetLogLevel(LogCategory.Network) <= LogLevel.Trace)
			{
				Debug.Log("Discarding Dissonance network message");
			}
			msg.Dispose();
		}

		// Token: 0x04001153 RID: 4435
		internal const byte ReliableSequencedChannel = 0;

		// Token: 0x04001154 RID: 4436
		internal const byte UnreliableChannel = 1;

		// Token: 0x04001155 RID: 4437
		private readonly ConcurrentPool<byte[]> _loopbackBuffers = new ConcurrentPool<byte[]>(8, () => new byte[1024]);

		// Token: 0x04001156 RID: 4438
		private readonly List<ArraySegment<byte>> _loopbackQueue = new List<ArraySegment<byte>>();
	}
}
