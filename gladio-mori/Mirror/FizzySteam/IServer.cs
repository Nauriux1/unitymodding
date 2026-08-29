using System;

namespace Mirror.FizzySteam
{
	// Token: 0x020002C5 RID: 709
	public interface IServer
	{
		// Token: 0x060015A9 RID: 5545
		void ReceiveData();

		// Token: 0x060015AA RID: 5546
		void Send(int connectionId, byte[] data, int channelId);

		// Token: 0x060015AB RID: 5547
		void Disconnect(int connectionId);

		// Token: 0x060015AC RID: 5548
		void FlushData();

		// Token: 0x060015AD RID: 5549
		string ServerGetClientAddress(int connectionId);

		// Token: 0x060015AE RID: 5550
		void Shutdown();
	}
}
