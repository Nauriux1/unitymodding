using System;
using Steamworks;
using Steamworks.Data;

// Token: 0x0200001D RID: 29
public class FizzySocketManager : SocketManager
{
	// Token: 0x0600014B RID: 331 RVA: 0x0000779B File Offset: 0x0000599B
	public override void OnMessage(Connection connection, NetIdentity identity, IntPtr data, int size, long messageNum, long recvTime, int channel)
	{
		this.ForwardMessage(connection, data, size);
	}

	// Token: 0x04000089 RID: 137
	public Action<Connection, IntPtr, int> ForwardMessage;
}
