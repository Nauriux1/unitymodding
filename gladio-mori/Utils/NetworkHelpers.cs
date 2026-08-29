using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mirror;
using ProtoBuf;
using UnityEngine;

namespace Utils
{
	// Token: 0x0200027A RID: 634
	public static class NetworkHelpers
	{
		// Token: 0x06001297 RID: 4759 RVA: 0x00061214 File Offset: 0x0005F414
		public static List<NetworkJsonMessage> CreateNetworkJsonMessages(string message)
		{
			List<NetworkJsonMessage> list = new List<NetworkJsonMessage>();
			string guid = Guid.NewGuid().ToString();
			List<string> list2 = new List<string>();
			for (int i = 0; i < message.Length; i += NetworkHelpers.chunkSize)
			{
				if (i + NetworkHelpers.chunkSize < message.Length)
				{
					list2.Add(message.Substring(i, NetworkHelpers.chunkSize));
				}
				else
				{
					list2.Add(message.Substring(i));
				}
			}
			int num = 0;
			foreach (string m in list2)
			{
				list.Add(new NetworkJsonMessage
				{
					p = num,
					tp = list2.Count<string>(),
					m = m,
					guid = guid
				});
				num++;
			}
			return list;
		}

		// Token: 0x06001298 RID: 4760 RVA: 0x00061300 File Offset: 0x0005F500
		public static RecompiledJsonMessage RecompileJsonMessage(List<NetworkJsonMessage> jsonMessages)
		{
			RecompiledJsonMessage recompiledJsonMessage = new RecompiledJsonMessage();
			recompiledJsonMessage.Status = 0;
			if (jsonMessages == null)
			{
				recompiledJsonMessage.Status = 1;
			}
			else
			{
				int num = 0;
				int num2 = 0;
				foreach (NetworkJsonMessage networkJsonMessage in from x in jsonMessages
				orderby x.p
				select x)
				{
					if (networkJsonMessage.p != num2)
					{
						recompiledJsonMessage.Status = 1;
						break;
					}
					num = networkJsonMessage.tp;
					RecompiledJsonMessage recompiledJsonMessage2 = recompiledJsonMessage;
					recompiledJsonMessage2.Message += networkJsonMessage.m;
					num2++;
				}
				if (num2 != num)
				{
					recompiledJsonMessage.Status = 1;
				}
			}
			return recompiledJsonMessage;
		}

		// Token: 0x06001299 RID: 4761 RVA: 0x000613CC File Offset: 0x0005F5CC
		public static string SerializeToString_PB<T>(this T obj)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Serializer.Serialize<T>(memoryStream, obj);
				result = Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
			}
			return result;
		}

		// Token: 0x0600129A RID: 4762 RVA: 0x00061418 File Offset: 0x0005F618
		public static T DeserializeFromString_PB<T>(this string txt)
		{
			T result;
			using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(txt)))
			{
				result = Serializer.Deserialize<T>(memoryStream);
			}
			return result;
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x0600129B RID: 4763 RVA: 0x00061458 File Offset: 0x0005F658
		public static int maxPlayerCustomTextureMessageParts
		{
			get
			{
				return SettingsHelper.customPlayerTextureMaxBytes / NetworkHelpers.byteChunkSize + 1;
			}
		}

		// Token: 0x0600129C RID: 4764 RVA: 0x00061468 File Offset: 0x0005F668
		public static List<NetworkByteMessage> CreateNetworkByteMessage(byte[] bytes)
		{
			List<NetworkByteMessage> list = new List<NetworkByteMessage>();
			int id = UnityEngine.Random.Range(1, int.MaxValue);
			List<byte[]> list2 = new List<byte[]>();
			for (int i = 0; i < bytes.Length; i += NetworkHelpers.byteChunkSize)
			{
				if (i + NetworkHelpers.byteChunkSize < bytes.Length)
				{
					byte[] array = new byte[NetworkHelpers.byteChunkSize];
					Array.Copy(bytes, i, array, 0, NetworkHelpers.byteChunkSize);
					list2.Add(array);
				}
				else
				{
					int num = bytes.Length - i;
					byte[] array2 = new byte[num];
					Array.Copy(bytes, i, array2, 0, num);
					list2.Add(array2);
				}
			}
			int num2 = 0;
			foreach (byte[] m in list2)
			{
				list.Add(new NetworkByteMessage
				{
					id = id,
					p = num2,
					tp = list2.Count<byte[]>(),
					m = m
				});
				num2++;
			}
			return list;
		}

		// Token: 0x0600129D RID: 4765 RVA: 0x00061578 File Offset: 0x0005F778
		public static RecompiledByteMessage RecompileByteMessage(List<NetworkByteMessage> byteMessages)
		{
			RecompiledByteMessage recompiledByteMessage = default(RecompiledByteMessage);
			recompiledByteMessage.Status = 0;
			if (byteMessages == null || byteMessages.Count == 0)
			{
				recompiledByteMessage.Status = 1;
			}
			else
			{
				int num = (byteMessages.Count - 1) * NetworkHelpers.byteChunkSize + byteMessages.Last<NetworkByteMessage>().m.Length;
				recompiledByteMessage.Message = new byte[num];
				int num2 = 0;
				int num3 = 0;
				foreach (NetworkByteMessage networkByteMessage in from x in byteMessages
				orderby x.p
				select x)
				{
					if (networkByteMessage.p != num3)
					{
						recompiledByteMessage.Status = 1;
						break;
					}
					num2 = networkByteMessage.tp;
					networkByteMessage.m.CopyTo(recompiledByteMessage.Message, num3 * NetworkHelpers.byteChunkSize);
					num3++;
				}
				if (num3 != num2)
				{
					recompiledByteMessage.Status = 1;
				}
			}
			return recompiledByteMessage;
		}

		// Token: 0x0600129E RID: 4766 RVA: 0x00061684 File Offset: 0x0005F884
		public static bool CurrentlyInMultiplayer()
		{
			return NetworkServer.active || NetworkClient.active;
		}

		// Token: 0x04000DFF RID: 3583
		public static int chunkSize = 10000;

		// Token: 0x04000E00 RID: 3584
		public static int byteChunkSize = 30720;
	}
}
