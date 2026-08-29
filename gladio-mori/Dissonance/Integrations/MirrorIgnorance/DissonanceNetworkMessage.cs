using System;
using Dissonance.Extensions;
using Mirror;

namespace Dissonance.Integrations.MirrorIgnorance
{
	// Token: 0x02000310 RID: 784
	internal struct DissonanceNetworkMessage : NetworkMessage, IDisposable
	{
		// Token: 0x06001777 RID: 6007 RVA: 0x00076BD6 File Offset: 0x00074DD6
		public DissonanceNetworkMessage(ArraySegment<byte> packet)
		{
			this.Data = packet.CopyToSegment(DissonanceNetworkMessageExtensions.SerializationBuffers.Get(), 0);
		}

		// Token: 0x06001778 RID: 6008 RVA: 0x00076BF0 File Offset: 0x00074DF0
		public void Dispose()
		{
			byte[] array = this.Data.Array;
			if (array != null && array.Length == 1024)
			{
				DissonanceNetworkMessageExtensions.SerializationBuffers.Put(array);
				this.Data = new ArraySegment<byte>(Array.Empty<byte>(), 0, 0);
			}
		}

		// Token: 0x0400115D RID: 4445
		public ArraySegment<byte> Data;
	}
}
