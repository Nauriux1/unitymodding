using System;
using System.Runtime.InteropServices;
using Steamworks;
using Steamworks.Data;
using UnityEngine;

// Token: 0x0200001E RID: 30
public abstract class NextCommon
{
	// Token: 0x0600014D RID: 333 RVA: 0x000077B4 File Offset: 0x000059B4
	protected Result SendSocket(Connection conn, byte[] data, int channelId)
	{
		Array.Resize<byte>(ref data, data.Length + 1);
		data[data.Length - 1] = (byte)channelId;
		GCHandle gchandle = GCHandle.Alloc(data, GCHandleType.Pinned);
		IntPtr ptr = gchandle.AddrOfPinnedObject();
		SendType sendType = (channelId == 1) ? SendType.Unreliable : SendType.Reliable;
		Result result = conn.SendMessage(ptr, data.Length, sendType);
		if (result != Result.OK)
		{
			Debug.LogWarning(string.Format("Send issue: {0}", result));
		}
		gchandle.Free();
		return result;
	}

	// Token: 0x0600014E RID: 334 RVA: 0x00007820 File Offset: 0x00005A20
	protected ValueTuple<byte[], int> ProcessMessage(IntPtr ptrs, int size)
	{
		byte[] array = new byte[size];
		Marshal.Copy(ptrs, array, 0, size);
		int item = (int)array[array.Length - 1];
		Array.Resize<byte>(ref array, array.Length - 1);
		return new ValueTuple<byte[], int>(array, item);
	}

	// Token: 0x0400008A RID: 138
	protected const int MAX_MESSAGES = 256;
}
