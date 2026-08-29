using System;
using Steamworks;

// Token: 0x0200001C RID: 28
public class FizzyConnectionManager : ConnectionManager
{
	// Token: 0x06000149 RID: 329 RVA: 0x00007784 File Offset: 0x00005984
	public override void OnMessage(IntPtr data, int size, long messageNum, long recvTime, int channel)
	{
		this.ForwardMessage(data, size);
	}

	// Token: 0x04000088 RID: 136
	public Action<IntPtr, int> ForwardMessage;
}
