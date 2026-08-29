using System;
using System.Runtime.InteropServices;
using Mirror;
using Mirror.RemoteCalls;
using Newtonsoft.Json;
using UnityEngine;

// Token: 0x02000086 RID: 134
public class MultiplayerGameMaster : NetworkBehaviour
{
	// Token: 0x0600044C RID: 1100 RVA: 0x00015005 File Offset: 0x00013205
	public virtual void TimeScaleChanged(float _, float newTimeScale)
	{
		if (GeneralManager.singleton != null)
		{
			GeneralManager.singleton.SetTimeScale(newTimeScale);
		}
	}

	// Token: 0x0600044D RID: 1101 RVA: 0x0000777A File Offset: 0x0000597A
	public override void OnStartServer()
	{
	}

	// Token: 0x0600044E RID: 1102 RVA: 0x0000777A File Offset: 0x0000597A
	public override void OnStopServer()
	{
	}

	// Token: 0x0600044F RID: 1103 RVA: 0x0001501F File Offset: 0x0001321F
	public override void OnStartClient()
	{
		this.gameMenu = UnityEngine.Object.FindObjectOfType<GameMenu>();
	}

	// Token: 0x06000450 RID: 1104 RVA: 0x0000777A File Offset: 0x0000597A
	public override void OnStopClient()
	{
	}

	// Token: 0x06000451 RID: 1105 RVA: 0x0000777A File Offset: 0x0000597A
	public override void OnStartLocalPlayer()
	{
	}

	// Token: 0x06000452 RID: 1106 RVA: 0x0000777A File Offset: 0x0000597A
	public override void OnStartAuthority()
	{
	}

	// Token: 0x06000453 RID: 1107 RVA: 0x0000777A File Offset: 0x0000597A
	public override void OnStopAuthority()
	{
	}

	// Token: 0x06000454 RID: 1108 RVA: 0x0001502C File Offset: 0x0001322C
	public void GameOver(WinScreenInfo info)
	{
		Debug.Log("MultiplayerGameMaster:GameOver");
		if (base.isServer)
		{
			this.GameOverClientRpc(JsonConvert.SerializeObject(info));
		}
	}

	// Token: 0x06000455 RID: 1109 RVA: 0x0001504C File Offset: 0x0001324C
	[ClientRpc]
	public void GameOverClientRpc(string info)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(info);
		this.SendRPCInternal("System.Void MultiplayerGameMaster::GameOverClientRpc(System.String)", -1960162473, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000457 RID: 1111 RVA: 0x0000C7D7 File Offset: 0x0000A9D7
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x170000E8 RID: 232
	// (get) Token: 0x06000458 RID: 1112 RVA: 0x00015090 File Offset: 0x00013290
	// (set) Token: 0x06000459 RID: 1113 RVA: 0x000150A3 File Offset: 0x000132A3
	public float NetworktimeScale
	{
		get
		{
			return this.timeScale;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<float>(value, ref this.timeScale, 1UL, new Action<float, float>(this.TimeScaleChanged));
		}
	}

	// Token: 0x0600045A RID: 1114 RVA: 0x000150C9 File Offset: 0x000132C9
	protected void UserCode_GameOverClientRpc__String(string info)
	{
		Debug.Log("MultiplayerGameMaster:GameOverClientRpc");
		if (this.gameMenu != null)
		{
			Debug.Log("Game over");
			this.gameMenu.ShowWinScreenInfo(JsonConvert.DeserializeObject<WinScreenInfo>(info));
		}
	}

	// Token: 0x0600045B RID: 1115 RVA: 0x000150FE File Offset: 0x000132FE
	protected static void InvokeUserCode_GameOverClientRpc__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC GameOverClientRpc called on server.");
			return;
		}
		((MultiplayerGameMaster)obj).UserCode_GameOverClientRpc__String(reader.ReadString());
	}

	// Token: 0x0600045C RID: 1116 RVA: 0x00015127 File Offset: 0x00013327
	static MultiplayerGameMaster()
	{
		RemoteProcedureCalls.RegisterRpc(typeof(MultiplayerGameMaster), "System.Void MultiplayerGameMaster::GameOverClientRpc(System.String)", new RemoteCallDelegate(MultiplayerGameMaster.InvokeUserCode_GameOverClientRpc__String));
	}

	// Token: 0x0600045D RID: 1117 RVA: 0x0001514C File Offset: 0x0001334C
	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteFloat(this.timeScale);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1UL) != 0UL)
		{
			writer.WriteFloat(this.timeScale);
		}
	}

	// Token: 0x0600045E RID: 1118 RVA: 0x000151A4 File Offset: 0x000133A4
	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			base.GeneratedSyncVarDeserialize<float>(ref this.timeScale, new Action<float, float>(this.TimeScaleChanged), reader.ReadFloat());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 1L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<float>(ref this.timeScale, new Action<float, float>(this.TimeScaleChanged), reader.ReadFloat());
		}
	}

	// Token: 0x040002C3 RID: 707
	public GameMenu gameMenu;

	// Token: 0x040002C4 RID: 708
	[SyncVar(hook = "TimeScaleChanged")]
	public float timeScale;
}
