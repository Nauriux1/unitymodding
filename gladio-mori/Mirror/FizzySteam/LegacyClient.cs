using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Steamworks;
using UnityEngine;

namespace Mirror.FizzySteam
{
	// Token: 0x020002C6 RID: 710
	public class LegacyClient : LegacyCommon, IClient
	{
		// Token: 0x17000281 RID: 641
		// (get) Token: 0x060015AF RID: 5551 RVA: 0x0006BF77 File Offset: 0x0006A177
		// (set) Token: 0x060015B0 RID: 5552 RVA: 0x0006BF7F File Offset: 0x0006A17F
		public bool Error { get; private set; }

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x060015B1 RID: 5553 RVA: 0x0006BF88 File Offset: 0x0006A188
		// (set) Token: 0x060015B2 RID: 5554 RVA: 0x0006BF90 File Offset: 0x0006A190
		public bool Connected { get; private set; }

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x060015B3 RID: 5555 RVA: 0x0006BF9C File Offset: 0x0006A19C
		// (remove) Token: 0x060015B4 RID: 5556 RVA: 0x0006BFD4 File Offset: 0x0006A1D4
		private event Action<byte[], int> OnReceivedData;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x060015B5 RID: 5557 RVA: 0x0006C00C File Offset: 0x0006A20C
		// (remove) Token: 0x060015B6 RID: 5558 RVA: 0x0006C044 File Offset: 0x0006A244
		private event Action OnConnected;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x060015B7 RID: 5559 RVA: 0x0006C07C File Offset: 0x0006A27C
		// (remove) Token: 0x060015B8 RID: 5560 RVA: 0x0006C0B4 File Offset: 0x0006A2B4
		private event Action OnDisconnected;

		// Token: 0x060015B9 RID: 5561 RVA: 0x0006C0E9 File Offset: 0x0006A2E9
		private LegacyClient(FizzyFacepunch transport) : base(transport)
		{
			this.ConnectionTimeout = TimeSpan.FromSeconds((double)Math.Max(1, transport.Timeout));
		}

		// Token: 0x060015BA RID: 5562 RVA: 0x0006C118 File Offset: 0x0006A318
		public static LegacyClient CreateClient(FizzyFacepunch transport, string host)
		{
			LegacyClient legacyClient = new LegacyClient(transport);
			legacyClient.OnConnected += delegate()
			{
				transport.OnClientConnected();
			};
			legacyClient.OnDisconnected += delegate()
			{
				transport.OnClientDisconnected();
			};
			legacyClient.OnReceivedData += delegate(byte[] data, int channel)
			{
				transport.OnClientDataReceived(new ArraySegment<byte>(data), channel);
			};
			if (SteamClient.IsValid)
			{
				legacyClient.Connect(host);
			}
			else
			{
				Debug.LogError("SteamWorks not initialized.");
				legacyClient.OnConnectionFailed(default(SteamId));
			}
			return legacyClient;
		}

		// Token: 0x060015BB RID: 5563 RVA: 0x0006C1A0 File Offset: 0x0006A3A0
		private void Connect(string host)
		{
			LegacyClient.<Connect>d__23 <Connect>d__;
			<Connect>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<Connect>d__.<>4__this = this;
			<Connect>d__.host = host;
			<Connect>d__.<>1__state = -1;
			<Connect>d__.<>t__builder.Start<LegacyClient.<Connect>d__23>(ref <Connect>d__);
		}

		// Token: 0x060015BC RID: 5564 RVA: 0x0006C1DF File Offset: 0x0006A3DF
		public void Disconnect()
		{
			Debug.Log("Sending Disconnect message");
			base.SendInternal(this.hostSteamID, LegacyCommon.InternalMessages.DISCONNECT);
			base.Dispose();
			CancellationTokenSource cancellationTokenSource = this.cancelToken;
			if (cancellationTokenSource != null)
			{
				cancellationTokenSource.Cancel();
			}
			base.WaitForClose(this.hostSteamID);
		}

		// Token: 0x060015BD RID: 5565 RVA: 0x0006C21C File Offset: 0x0006A41C
		private void SetConnectedComplete()
		{
			this.connectedComplete.SetResult(this.connectedComplete.Task);
		}

		// Token: 0x060015BE RID: 5566 RVA: 0x0006C234 File Offset: 0x0006A434
		protected override void OnReceiveData(byte[] data, SteamId clientSteamID, int channel)
		{
			if (clientSteamID != this.hostSteamID)
			{
				Debug.LogError("Received a message from an unknown");
				return;
			}
			this.OnReceivedData(data, channel);
		}

		// Token: 0x060015BF RID: 5567 RVA: 0x0006C261 File Offset: 0x0006A461
		protected override void OnNewConnection(SteamId id)
		{
			if (this.hostSteamID == id)
			{
				SteamNetworking.AcceptP2PSessionWithUser(id);
				return;
			}
			Debug.LogError("P2P Acceptance Request from unknown host ID.");
		}

		// Token: 0x060015C0 RID: 5568 RVA: 0x0006C288 File Offset: 0x0006A488
		protected override void OnReceiveInternalData(LegacyCommon.InternalMessages type, SteamId clientSteamID)
		{
			if (type != LegacyCommon.InternalMessages.ACCEPT_CONNECT)
			{
				if (type != LegacyCommon.InternalMessages.DISCONNECT)
				{
					Debug.Log("Received unknown message type");
				}
				else if (this.Connected)
				{
					this.Connected = false;
					Debug.Log("Disconnected.");
					this.OnDisconnected();
					return;
				}
			}
			else if (!this.Connected)
			{
				this.Connected = true;
				Debug.Log("Connection established.");
				this.OnConnected();
				return;
			}
		}

		// Token: 0x060015C1 RID: 5569 RVA: 0x0006C2F3 File Offset: 0x0006A4F3
		public void Send(byte[] data, int channelId)
		{
			base.Send(this.hostSteamID, data, channelId);
		}

		// Token: 0x060015C2 RID: 5570 RVA: 0x0006C303 File Offset: 0x0006A503
		protected override void OnConnectionFailed(SteamId remoteId)
		{
			this.OnDisconnected();
		}

		// Token: 0x060015C3 RID: 5571 RVA: 0x0000777A File Offset: 0x0000597A
		public void FlushData()
		{
		}

		// Token: 0x04000FC2 RID: 4034
		private TimeSpan ConnectionTimeout;

		// Token: 0x04000FC3 RID: 4035
		private SteamId hostSteamID = 0UL;

		// Token: 0x04000FC4 RID: 4036
		private TaskCompletionSource<Task> connectedComplete;

		// Token: 0x04000FC5 RID: 4037
		private CancellationTokenSource cancelToken;
	}
}
