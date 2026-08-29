using System;

namespace Mirror
{
	// Token: 0x020002BA RID: 698
	public class NetworkMessagesSent
	{
		// Token: 0x060014F5 RID: 5365 RVA: 0x00069348 File Offset: 0x00067548
		public NetworkMessagesSent(NetworkConnectionToClient startConn, float startLastSend)
		{
			this.conn = startConn;
			this.lastSend = startLastSend;
		}

		// Token: 0x04000F6C RID: 3948
		public NetworkConnectionToClient conn;

		// Token: 0x04000F6D RID: 3949
		public int sentPackets;

		// Token: 0x04000F6E RID: 3950
		public bool canSend;

		// Token: 0x04000F6F RID: 3951
		public float lastSend;

		// Token: 0x04000F70 RID: 3952
		public bool allSent;

		// Token: 0x04000F71 RID: 3953
		public int failedSends;
	}
}
