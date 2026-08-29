using System;
using Dissonance.Datastructures;
using JetBrains.Annotations;
using Mirror;

namespace Dissonance.Integrations.MirrorIgnorance
{
	// Token: 0x0200030E RID: 782
	internal static class DissonanceNetworkMessageExtensions
	{
		// Token: 0x06001771 RID: 6001 RVA: 0x00076B0C File Offset: 0x00074D0C
		public static void Serialize([NotNull] this NetworkWriter writer, DissonanceNetworkMessage value)
		{
			writer.WriteUShort((ushort)value.Data.Count);
			writer.WriteBytes(value.Data.Array, value.Data.Offset, value.Data.Count);
			DissonanceNetworkMessageExtensions.SerializationBuffers.Put(value.Data.Array);
		}

		// Token: 0x06001772 RID: 6002 RVA: 0x00076B6C File Offset: 0x00074D6C
		public static DissonanceNetworkMessage Deserialize([NotNull] this NetworkReader reader)
		{
			byte[] array = DissonanceNetworkMessageExtensions.SerializationBuffers.Get();
			ushort num = reader.ReadUShort();
			for (int i = 0; i < (int)num; i++)
			{
				array[i] = reader.ReadByte();
			}
			return new DissonanceNetworkMessage(new ArraySegment<byte>(array, 0, (int)num));
		}

		// Token: 0x0400115A RID: 4442
		internal const int BufferLength = 1024;

		// Token: 0x0400115B RID: 4443
		internal static readonly ConcurrentPool<byte[]> SerializationBuffers = new ConcurrentPool<byte[]>(8, () => new byte[1024]);
	}
}
