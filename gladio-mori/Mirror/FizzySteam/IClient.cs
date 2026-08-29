using System;

namespace Mirror.FizzySteam
{
	// Token: 0x020002C4 RID: 708
	public interface IClient
	{
		// Token: 0x1700027F RID: 639
		// (get) Token: 0x060015A3 RID: 5539
		bool Connected { get; }

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x060015A4 RID: 5540
		bool Error { get; }

		// Token: 0x060015A5 RID: 5541
		void ReceiveData();

		// Token: 0x060015A6 RID: 5542
		void Disconnect();

		// Token: 0x060015A7 RID: 5543
		void FlushData();

		// Token: 0x060015A8 RID: 5544
		void Send(byte[] data, int channelId);
	}
}
