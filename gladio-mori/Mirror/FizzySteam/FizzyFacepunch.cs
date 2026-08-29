using System;
using Steamworks;
using UnityEngine;

namespace Mirror.FizzySteam
{
	// Token: 0x020002C3 RID: 707
	[HelpURL("https://github.com/Chykary/FizzyFacepunch")]
	public class FizzyFacepunch : Transport
	{
		// Token: 0x0600158A RID: 5514 RVA: 0x0006BA4B File Offset: 0x00069C4B
		private void Awake()
		{
			if (!this.InitFacepunch)
			{
				return;
			}
			if (!this.InitialiseSteamworks(this.SteamAppID))
			{
				return;
			}
			Debug.Log("SteamWorks initialised");
			this.FetchSteamID();
		}

		// Token: 0x0600158B RID: 5515 RVA: 0x0006BA75 File Offset: 0x00069C75
		public override void ClientEarlyUpdate()
		{
			if (base.enabled)
			{
				IClient client = FizzyFacepunch.client;
				if (client == null)
				{
					return;
				}
				client.ReceiveData();
			}
		}

		// Token: 0x0600158C RID: 5516 RVA: 0x0006BA8E File Offset: 0x00069C8E
		public override void ServerEarlyUpdate()
		{
			if (base.enabled)
			{
				IServer server = FizzyFacepunch.server;
				if (server == null)
				{
					return;
				}
				server.ReceiveData();
			}
		}

		// Token: 0x0600158D RID: 5517 RVA: 0x0006BAA7 File Offset: 0x00069CA7
		public override void ClientLateUpdate()
		{
			if (base.enabled)
			{
				IClient client = FizzyFacepunch.client;
				if (client == null)
				{
					return;
				}
				client.FlushData();
			}
		}

		// Token: 0x0600158E RID: 5518 RVA: 0x0006BAC0 File Offset: 0x00069CC0
		public override void ServerLateUpdate()
		{
			if (base.enabled)
			{
				IServer server = FizzyFacepunch.server;
				if (server == null)
				{
					return;
				}
				server.FlushData();
			}
		}

		// Token: 0x0600158F RID: 5519 RVA: 0x0006BAD9 File Offset: 0x00069CD9
		public override bool ClientConnected()
		{
			return this.ClientActive() && FizzyFacepunch.client.Connected;
		}

		// Token: 0x06001590 RID: 5520 RVA: 0x0006BAF0 File Offset: 0x00069CF0
		public override void ClientConnect(string address)
		{
			if (!SteamClient.IsValid)
			{
				Debug.LogError("SteamWorks not initialized. Client could not be started.");
				this.OnClientDisconnected();
				return;
			}
			if (address == this.SteamUserID.ToString())
			{
				Debug.Log("You can't connect to yourself.");
				return;
			}
			this.FetchSteamID();
			if (this.ServerActive())
			{
				Debug.LogError("Transport already running as server!");
				return;
			}
			if (this.ClientActive() && !FizzyFacepunch.client.Error)
			{
				Debug.LogError("Client already running!");
				return;
			}
			if (this.UseNextGenSteamNetworking)
			{
				Debug.Log("Starting client [SteamSockets], target address " + address + ".");
				FizzyFacepunch.client = NextClient.CreateClient(this, address);
				return;
			}
			Debug.Log(string.Format("Starting client [DEPRECATED SteamNetworking], target address {0}. Relay enabled: {1}", address, this.AllowSteamRelay));
			SteamNetworking.AllowP2PPacketRelay(this.AllowSteamRelay);
			FizzyFacepunch.client = LegacyClient.CreateClient(this, address);
		}

		// Token: 0x06001591 RID: 5521 RVA: 0x0006BBCD File Offset: 0x00069DCD
		public override void ClientConnect(Uri uri)
		{
			if (uri.Scheme != "steam")
			{
				throw new ArgumentException(string.Format("Invalid url {0}, use {1}://SteamID instead", uri, "steam"), "uri");
			}
			this.ClientConnect(uri.Host);
		}

		// Token: 0x06001592 RID: 5522 RVA: 0x0006BC08 File Offset: 0x00069E08
		public override void ClientSend(ArraySegment<byte> segment, int channelId)
		{
			byte[] array = new byte[segment.Count];
			Array.Copy(segment.Array, segment.Offset, array, 0, segment.Count);
			FizzyFacepunch.client.Send(array, channelId);
		}

		// Token: 0x06001593 RID: 5523 RVA: 0x0006BC4A File Offset: 0x00069E4A
		public override void ClientDisconnect()
		{
			if (this.ClientActive())
			{
				this.Shutdown();
			}
		}

		// Token: 0x06001594 RID: 5524 RVA: 0x0006BC5A File Offset: 0x00069E5A
		public bool ClientActive()
		{
			return FizzyFacepunch.client != null;
		}

		// Token: 0x06001595 RID: 5525 RVA: 0x0006BC64 File Offset: 0x00069E64
		public override bool ServerActive()
		{
			return FizzyFacepunch.server != null;
		}

		// Token: 0x06001596 RID: 5526 RVA: 0x0006BC70 File Offset: 0x00069E70
		public override void ServerStart()
		{
			if (!SteamClient.IsValid)
			{
				Debug.LogError("SteamWorks not initialized. Server could not be started.");
				return;
			}
			this.FetchSteamID();
			if (this.ClientActive())
			{
				Debug.LogError("Transport already running as client!");
				return;
			}
			if (this.ServerActive())
			{
				Debug.LogError("Server already started!");
				return;
			}
			if (this.UseNextGenSteamNetworking)
			{
				Debug.Log("Starting server [SteamSockets].");
				FizzyFacepunch.server = NextServer.CreateServer(this, NetworkManager.singleton.maxConnections);
				return;
			}
			Debug.Log(string.Format("Starting server [DEPRECATED SteamNetworking]. Relay enabled: {0}", this.AllowSteamRelay));
			SteamNetworking.AllowP2PPacketRelay(this.AllowSteamRelay);
			FizzyFacepunch.server = LegacyServer.CreateServer(this, NetworkManager.singleton.maxConnections);
		}

		// Token: 0x06001597 RID: 5527 RVA: 0x0006BD20 File Offset: 0x00069F20
		public override Uri ServerUri()
		{
			return new UriBuilder
			{
				Scheme = "steam",
				Host = SteamClient.SteamId.Value.ToString()
			}.Uri;
		}

		// Token: 0x06001598 RID: 5528 RVA: 0x0006BD5C File Offset: 0x00069F5C
		public override void ServerSend(int connectionId, ArraySegment<byte> segment, int channelId)
		{
			if (this.ServerActive())
			{
				byte[] array = new byte[segment.Count];
				Array.Copy(segment.Array, segment.Offset, array, 0, segment.Count);
				FizzyFacepunch.server.Send(connectionId, array, channelId);
			}
		}

		// Token: 0x06001599 RID: 5529 RVA: 0x0006BDA7 File Offset: 0x00069FA7
		public override void ServerDisconnect(int connectionId)
		{
			if (this.ServerActive())
			{
				FizzyFacepunch.server.Disconnect(connectionId);
			}
		}

		// Token: 0x0600159A RID: 5530 RVA: 0x0006BDBC File Offset: 0x00069FBC
		public override string ServerGetClientAddress(int connectionId)
		{
			if (!this.ServerActive())
			{
				return string.Empty;
			}
			return FizzyFacepunch.server.ServerGetClientAddress(connectionId);
		}

		// Token: 0x0600159B RID: 5531 RVA: 0x0006BDD7 File Offset: 0x00069FD7
		public override void ServerStop()
		{
			if (this.ServerActive())
			{
				this.Shutdown();
			}
		}

		// Token: 0x0600159C RID: 5532 RVA: 0x0006BDE8 File Offset: 0x00069FE8
		public override void Shutdown()
		{
			if (FizzyFacepunch.server != null)
			{
				FizzyFacepunch.server.Shutdown();
				FizzyFacepunch.server = null;
				Debug.Log("Transport shut down - was server.");
			}
			if (FizzyFacepunch.client != null)
			{
				FizzyFacepunch.client.Disconnect();
				FizzyFacepunch.client = null;
				Debug.Log("Transport shut down - was client.");
			}
		}

		// Token: 0x0600159D RID: 5533 RVA: 0x0006BE38 File Offset: 0x0006A038
		public override int GetMaxPacketSize(int channelId)
		{
			if (channelId >= this.Channels.Length)
			{
				Debug.LogError("Channel Id exceeded configured channels! Please configure more channels.");
				return 1200;
			}
			P2PSend p2PSend = this.Channels[channelId];
			if (p2PSend <= P2PSend.UnreliableNoDelay)
			{
				return 1200;
			}
			if (p2PSend - P2PSend.Reliable > 1)
			{
				throw new NotSupportedException();
			}
			return 1048576;
		}

		// Token: 0x0600159E RID: 5534 RVA: 0x0006BE88 File Offset: 0x0006A088
		public override bool Available()
		{
			bool result;
			try
			{
				result = SteamClient.IsValid;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600159F RID: 5535 RVA: 0x0006BEB4 File Offset: 0x0006A0B4
		private void FetchSteamID()
		{
			if (SteamClient.IsValid)
			{
				if (this.UseNextGenSteamNetworking)
				{
					SteamNetworkingUtils.InitRelayNetworkAccess();
				}
				this.SteamUserID = SteamClient.SteamId;
			}
		}

		// Token: 0x060015A0 RID: 5536 RVA: 0x0006BEDC File Offset: 0x0006A0DC
		private bool InitialiseSteamworks(uint appid)
		{
			try
			{
				SteamClient.Init(appid, true);
			}
			catch (Exception ex)
			{
				Debug.LogError("Could be one of the following: Steam is closed, Can't find steam_api dlls or Don't have permission to open appid. Exception: " + ex.Message);
				return false;
			}
			return true;
		}

		// Token: 0x060015A1 RID: 5537 RVA: 0x0006BF20 File Offset: 0x0006A120
		private void OnDestroy()
		{
			this.Shutdown();
		}

		// Token: 0x04000FB3 RID: 4019
		private const string STEAM_SCHEME = "steam";

		// Token: 0x04000FB4 RID: 4020
		private static IClient client;

		// Token: 0x04000FB5 RID: 4021
		private static IServer server;

		// Token: 0x04000FB6 RID: 4022
		[SerializeField]
		public P2PSend[] Channels = new P2PSend[]
		{
			P2PSend.Reliable,
			P2PSend.UnreliableNoDelay
		};

		// Token: 0x04000FB7 RID: 4023
		[Tooltip("Timeout for connecting in seconds.")]
		public int Timeout = 25;

		// Token: 0x04000FB8 RID: 4024
		[Tooltip("The Steam ID for your application.")]
		public uint SteamAppID = 480U;

		// Token: 0x04000FB9 RID: 4025
		[Tooltip("Allow or disallow P2P connections to fall back to being relayed through the Steam servers if a direct connection or NAT-traversal cannot be established.")]
		public bool AllowSteamRelay = true;

		// Token: 0x04000FBA RID: 4026
		[Tooltip("Use SteamSockets instead of the (deprecated) SteamNetworking. This will always use Relay.")]
		public bool UseNextGenSteamNetworking = true;

		// Token: 0x04000FBB RID: 4027
		[Tooltip("Check this if you want the transport to initialise Facepunch.")]
		public bool InitFacepunch = true;

		// Token: 0x04000FBC RID: 4028
		[Header("Info")]
		[Tooltip("This will display your Steam User ID when you start or connect to a server.")]
		public ulong SteamUserID;
	}
}
