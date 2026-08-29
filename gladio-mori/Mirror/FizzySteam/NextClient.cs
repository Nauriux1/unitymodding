using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Steamworks;
using Steamworks.Data;
using UnityEngine;

namespace Mirror.FizzySteam
{
	// Token: 0x020002CF RID: 719
	public class NextClient : NextCommon, IClient
	{
		// Token: 0x17000286 RID: 646
		// (get) Token: 0x060015FA RID: 5626 RVA: 0x0006CEF6 File Offset: 0x0006B0F6
		// (set) Token: 0x060015FB RID: 5627 RVA: 0x0006CEFE File Offset: 0x0006B0FE
		public bool Connected { get; private set; }

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x060015FC RID: 5628 RVA: 0x0006CF07 File Offset: 0x0006B107
		// (set) Token: 0x060015FD RID: 5629 RVA: 0x0006CF0F File Offset: 0x0006B10F
		public bool Error { get; private set; }

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x060015FE RID: 5630 RVA: 0x0006CF18 File Offset: 0x0006B118
		// (remove) Token: 0x060015FF RID: 5631 RVA: 0x0006CF50 File Offset: 0x0006B150
		private event Action<byte[], int> OnReceivedData;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06001600 RID: 5632 RVA: 0x0006CF88 File Offset: 0x0006B188
		// (remove) Token: 0x06001601 RID: 5633 RVA: 0x0006CFC0 File Offset: 0x0006B1C0
		private event Action OnConnected;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06001602 RID: 5634 RVA: 0x0006CFF8 File Offset: 0x0006B1F8
		// (remove) Token: 0x06001603 RID: 5635 RVA: 0x0006D030 File Offset: 0x0006B230
		private event Action OnDisconnected;

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06001604 RID: 5636 RVA: 0x0006D065 File Offset: 0x0006B265
		private Connection HostConnection
		{
			get
			{
				return this.HostConnectionManager.Connection;
			}
		}

		// Token: 0x06001605 RID: 5637 RVA: 0x0006D072 File Offset: 0x0006B272
		private NextClient(FizzyFacepunch transport)
		{
			this.ConnectionTimeout = TimeSpan.FromSeconds((double)Math.Max(1, transport.Timeout));
			this.BufferedData = new List<Action>();
		}

		// Token: 0x06001606 RID: 5638 RVA: 0x0006D0AC File Offset: 0x0006B2AC
		public static NextClient CreateClient(FizzyFacepunch transport, string host)
		{
			NextClient nextClient = new NextClient(transport);
			nextClient.OnConnected += delegate()
			{
				transport.OnClientConnected();
			};
			nextClient.OnDisconnected += delegate()
			{
				transport.OnClientDisconnected();
			};
			nextClient.OnReceivedData += delegate(byte[] data, int ch)
			{
				transport.OnClientDataReceived(new ArraySegment<byte>(data), ch);
			};
			if (SteamClient.IsValid)
			{
				nextClient.Connect(host);
			}
			else
			{
				Debug.LogError("SteamWorks not initialized");
				nextClient.OnConnectionFailed();
			}
			return nextClient;
		}

		// Token: 0x06001607 RID: 5639 RVA: 0x0006D12C File Offset: 0x0006B32C
		private void Connect(string host)
		{
			NextClient.<Connect>d__27 <Connect>d__;
			<Connect>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<Connect>d__.<>4__this = this;
			<Connect>d__.host = host;
			<Connect>d__.<>1__state = -1;
			<Connect>d__.<>t__builder.Start<NextClient.<Connect>d__27>(ref <Connect>d__);
		}

		// Token: 0x06001608 RID: 5640 RVA: 0x0006D16C File Offset: 0x0006B36C
		private void OnMessageReceived(IntPtr dataPtr, int size)
		{
			ValueTuple<byte[], int> valueTuple = base.ProcessMessage(dataPtr, size);
			byte[] data = valueTuple.Item1;
			int ch = valueTuple.Item2;
			if (this.Connected)
			{
				this.OnReceivedData(data, ch);
				return;
			}
			this.BufferedData.Add(delegate
			{
				this.OnReceivedData(data, ch);
			});
		}

		// Token: 0x06001609 RID: 5641 RVA: 0x0006D1E0 File Offset: 0x0006B3E0
		private void OnConnectionStatusChanged(Connection conn, ConnectionInfo info)
		{
			info.Identity.SteamId;
			if (info.State == ConnectionState.Connected)
			{
				this.Connected = true;
				this.OnConnected();
				Debug.Log("Connection established.");
				if (this.BufferedData.Count <= 0)
				{
					return;
				}
				Debug.Log(string.Format("{0} received before connection was established. Processing now.", this.BufferedData.Count));
				using (List<Action>.Enumerator enumerator = this.BufferedData.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Action action = enumerator.Current;
						action();
					}
					return;
				}
			}
			if (info.State == ConnectionState.ClosedByPeer)
			{
				this.Connected = false;
				this.OnDisconnected();
				Debug.Log("Disconnected.");
				conn.Close(false, 0, "Disconnected");
				return;
			}
			Debug.Log("Connection state changed: " + info.State.ToString());
		}

		// Token: 0x0600160A RID: 5642 RVA: 0x0006D2F8 File Offset: 0x0006B4F8
		public void Disconnect()
		{
			CancellationTokenSource cancellationTokenSource = this.cancelToken;
			if (cancellationTokenSource != null)
			{
				cancellationTokenSource.Cancel();
			}
			SteamNetworkingSockets.OnConnectionStatusChanged -= this.OnConnectionStatusChanged;
			if (this.HostConnectionManager != null)
			{
				Debug.Log("Sending Disconnect message");
				this.HostConnection.Close(false, 0, "Graceful disconnect");
				this.HostConnectionManager = null;
			}
		}

		// Token: 0x0600160B RID: 5643 RVA: 0x0006D356 File Offset: 0x0006B556
		public void ReceiveData()
		{
			this.HostConnectionManager.Receive(256);
		}

		// Token: 0x0600160C RID: 5644 RVA: 0x0006D368 File Offset: 0x0006B568
		public void Send(byte[] data, int channelId)
		{
			Result result = base.SendSocket(this.HostConnection, data, channelId);
			if (result != Result.OK)
			{
				Debug.LogError("Could not send: " + result.ToString());
			}
		}

		// Token: 0x0600160D RID: 5645 RVA: 0x0006D3A4 File Offset: 0x0006B5A4
		private void SetConnectedComplete()
		{
			this.connectedComplete.SetResult(this.connectedComplete.Task);
		}

		// Token: 0x0600160E RID: 5646 RVA: 0x0006D3BC File Offset: 0x0006B5BC
		private void OnConnectionFailed()
		{
			this.OnDisconnected();
		}

		// Token: 0x0600160F RID: 5647 RVA: 0x0006D3CC File Offset: 0x0006B5CC
		public void FlushData()
		{
			this.HostConnection.Flush();
		}

		// Token: 0x04000FE4 RID: 4068
		private TimeSpan ConnectionTimeout;

		// Token: 0x04000FE8 RID: 4072
		private CancellationTokenSource cancelToken;

		// Token: 0x04000FE9 RID: 4073
		private TaskCompletionSource<Task> connectedComplete;

		// Token: 0x04000FEA RID: 4074
		private SteamId hostSteamID = 0UL;

		// Token: 0x04000FEB RID: 4075
		private FizzyConnectionManager HostConnectionManager;

		// Token: 0x04000FEC RID: 4076
		private List<Action> BufferedData;
	}
}
