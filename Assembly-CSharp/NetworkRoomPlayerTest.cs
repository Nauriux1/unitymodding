using System;
using System.Runtime.InteropServices;
using Mirror;

// Token: 0x0200014B RID: 331
public class NetworkRoomPlayerTest : NetworkRoomPlayer
{
	// Token: 0x06000A4A RID: 2634 RVA: 0x0000777A File Offset: 0x0000597A
	public override void OnStartServer()
	{
	}

	// Token: 0x06000A4B RID: 2635 RVA: 0x0000777A File Offset: 0x0000597A
	public override void OnStopServer()
	{
	}

	// Token: 0x06000A4C RID: 2636 RVA: 0x0000777A File Offset: 0x0000597A
	public override void OnStartClient()
	{
	}

	// Token: 0x06000A4D RID: 2637 RVA: 0x0000777A File Offset: 0x0000597A
	public override void OnStopClient()
	{
	}

	// Token: 0x06000A4E RID: 2638 RVA: 0x0000777A File Offset: 0x0000597A
	public override void OnStartLocalPlayer()
	{
	}

	// Token: 0x06000A4F RID: 2639 RVA: 0x0000777A File Offset: 0x0000597A
	public override void OnStartAuthority()
	{
	}

	// Token: 0x06000A50 RID: 2640 RVA: 0x0000777A File Offset: 0x0000597A
	public override void OnStopAuthority()
	{
	}

	// Token: 0x06000A51 RID: 2641 RVA: 0x0000777A File Offset: 0x0000597A
	public override void OnClientEnterRoom()
	{
	}

	// Token: 0x06000A52 RID: 2642 RVA: 0x0000777A File Offset: 0x0000597A
	public override void OnClientExitRoom()
	{
	}

	// Token: 0x06000A53 RID: 2643 RVA: 0x0000777A File Offset: 0x0000597A
	public override void IndexChanged(int oldIndex, int newIndex)
	{
	}

	// Token: 0x06000A54 RID: 2644 RVA: 0x0000777A File Offset: 0x0000597A
	public override void ReadyStateChanged(bool _, bool readyState)
	{
	}

	// Token: 0x06000A56 RID: 2646 RVA: 0x0000C7D7 File Offset: 0x0000A9D7
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x17000159 RID: 345
	// (get) Token: 0x06000A57 RID: 2647 RVA: 0x000309A0 File Offset: 0x0002EBA0
	// (set) Token: 0x06000A58 RID: 2648 RVA: 0x000309B3 File Offset: 0x0002EBB3
	public string NetworknameTest
	{
		get
		{
			return this.nameTest;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<string>(value, ref this.nameTest, 4UL, null);
		}
	}

	// Token: 0x06000A59 RID: 2649 RVA: 0x000309D0 File Offset: 0x0002EBD0
	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteString(this.nameTest);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 4UL) != 0UL)
		{
			writer.WriteString(this.nameTest);
		}
	}

	// Token: 0x06000A5A RID: 2650 RVA: 0x00030A28 File Offset: 0x0002EC28
	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			base.GeneratedSyncVarDeserialize<string>(ref this.nameTest, null, reader.ReadString());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 4L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<string>(ref this.nameTest, null, reader.ReadString());
		}
	}

	// Token: 0x0400073E RID: 1854
	[SyncVar]
	public string nameTest = "";
}
