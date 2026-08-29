using System;
using System.Collections;
using Steamworks;
using Steamworks.Data;
using UnityEngine;

namespace Mirror.FizzySteam
{
	// Token: 0x020002C9 RID: 713
	public abstract class LegacyCommon
	{
		// Token: 0x17000283 RID: 643
		// (get) Token: 0x060015CA RID: 5578 RVA: 0x0006C5DA File Offset: 0x0006A7DA
		private int internal_ch
		{
			get
			{
				return this.channels.Length;
			}
		}

		// Token: 0x060015CB RID: 5579 RVA: 0x0006C5E4 File Offset: 0x0006A7E4
		protected LegacyCommon(FizzyFacepunch transport)
		{
			this.channels = transport.Channels;
			SteamNetworking.OnP2PSessionRequest = (Action<SteamId>)Delegate.Combine(SteamNetworking.OnP2PSessionRequest, new Action<SteamId>(this.OnNewConnection));
			SteamNetworking.OnP2PConnectionFailed = (Action<SteamId, P2PSessionError>)Delegate.Combine(SteamNetworking.OnP2PConnectionFailed, new Action<SteamId, P2PSessionError>(this.OnConnectFail));
			this.transport = transport;
		}

		// Token: 0x060015CC RID: 5580 RVA: 0x0006C64B File Offset: 0x0006A84B
		protected void WaitForClose(SteamId cSteamID)
		{
			this.transport.StartCoroutine(this.DelayedClose(cSteamID));
		}

		// Token: 0x060015CD RID: 5581 RVA: 0x0006C660 File Offset: 0x0006A860
		private IEnumerator DelayedClose(SteamId cSteamID)
		{
			yield return null;
			this.CloseP2PSessionWithUser(cSteamID);
			yield break;
		}

		// Token: 0x060015CE RID: 5582 RVA: 0x0006C678 File Offset: 0x0006A878
		protected void Dispose()
		{
			SteamNetworking.OnP2PSessionRequest = (Action<SteamId>)Delegate.Remove(SteamNetworking.OnP2PSessionRequest, new Action<SteamId>(this.OnNewConnection));
			SteamNetworking.OnP2PConnectionFailed = (Action<SteamId, P2PSessionError>)Delegate.Remove(SteamNetworking.OnP2PConnectionFailed, new Action<SteamId, P2PSessionError>(this.OnConnectFail));
		}

		// Token: 0x060015CF RID: 5583
		protected abstract void OnNewConnection(SteamId steamID);

		// Token: 0x060015D0 RID: 5584 RVA: 0x0006C6C8 File Offset: 0x0006A8C8
		private void OnConnectFail(SteamId id, P2PSessionError err)
		{
			this.OnConnectionFailed(id);
			this.CloseP2PSessionWithUser(id);
			switch (err)
			{
			case P2PSessionError.NotRunningApp:
				throw new Exception("Connection failed: The target user is not running the same game.");
			case P2PSessionError.NoRightsToApp:
				throw new Exception("Connection failed: The local user doesn't own the app that is running.");
			case P2PSessionError.DestinationNotLoggedIn:
				throw new Exception("Connection failed: Target user isn't connected to Steam.");
			case P2PSessionError.Timeout:
				throw new Exception("Connection failed: The connection timed out because the target user didn't respond.");
			default:
				throw new Exception("Connection failed: Unknown error.");
			}
		}

		// Token: 0x060015D1 RID: 5585 RVA: 0x0006C733 File Offset: 0x0006A933
		protected bool SendInternal(SteamId target, LegacyCommon.InternalMessages type)
		{
			return SteamNetworking.SendP2PPacket(target, new byte[]
			{
				(byte)type
			}, 1, this.internal_ch, P2PSend.Reliable);
		}

		// Token: 0x060015D2 RID: 5586 RVA: 0x0006C74D File Offset: 0x0006A94D
		protected void Send(SteamId host, byte[] msgBuffer, int channel)
		{
			SteamNetworking.SendP2PPacket(host, msgBuffer, msgBuffer.Length, channel, this.channels[Mathf.Min(channel, this.channels.Length - 1)]);
		}

		// Token: 0x060015D3 RID: 5587 RVA: 0x0006C774 File Offset: 0x0006A974
		private bool Receive(out SteamId clientSteamID, out byte[] receiveBuffer, int channel)
		{
			if (SteamNetworking.IsP2PPacketAvailable(channel))
			{
				P2Packet? p2Packet = SteamNetworking.ReadP2PPacket(channel);
				if (p2Packet != null)
				{
					receiveBuffer = p2Packet.Value.Data;
					clientSteamID = p2Packet.Value.SteamId;
					return true;
				}
			}
			receiveBuffer = null;
			clientSteamID = 0UL;
			return false;
		}

		// Token: 0x060015D4 RID: 5588 RVA: 0x0006C7CC File Offset: 0x0006A9CC
		protected void CloseP2PSessionWithUser(SteamId clientSteamID)
		{
			SteamNetworking.CloseP2PSessionWithUser(clientSteamID);
		}

		// Token: 0x060015D5 RID: 5589 RVA: 0x0006C7D8 File Offset: 0x0006A9D8
		public void ReceiveData()
		{
			try
			{
				SteamId clientSteamID;
				byte[] array;
				while (this.transport.enabled && this.Receive(out clientSteamID, out array, this.internal_ch))
				{
					if (array.Length == 1)
					{
						this.OnReceiveInternalData((LegacyCommon.InternalMessages)array[0], clientSteamID);
						return;
					}
					Debug.Log("Incorrect package length on internal channel.");
				}
				for (int i = 0; i < this.channels.Length; i++)
				{
					SteamId clientSteamID2;
					byte[] data;
					while (this.transport.enabled && this.Receive(out clientSteamID2, out data, i))
					{
						this.OnReceiveData(data, clientSteamID2, i);
					}
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}

		// Token: 0x060015D6 RID: 5590
		protected abstract void OnReceiveInternalData(LegacyCommon.InternalMessages type, SteamId clientSteamID);

		// Token: 0x060015D7 RID: 5591
		protected abstract void OnReceiveData(byte[] data, SteamId clientSteamID, int channel);

		// Token: 0x060015D8 RID: 5592
		protected abstract void OnConnectionFailed(SteamId remoteId);

		// Token: 0x04000FCE RID: 4046
		private P2PSend[] channels;

		// Token: 0x04000FCF RID: 4047
		protected readonly FizzyFacepunch transport;

		// Token: 0x020002CA RID: 714
		protected enum InternalMessages : byte
		{
			// Token: 0x04000FD1 RID: 4049
			CONNECT,
			// Token: 0x04000FD2 RID: 4050
			ACCEPT_CONNECT,
			// Token: 0x04000FD3 RID: 4051
			DISCONNECT
		}
	}
}
