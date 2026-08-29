using System;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

// Token: 0x02000093 RID: 147
public class SoundManagerMultiplayer : NetworkBehaviour
{
	// Token: 0x06000511 RID: 1297 RVA: 0x00018066 File Offset: 0x00016266
	private void Awake()
	{
		this.soundManager = SoundManager.singleton;
		this.soundManager.soundManagerMultiplayer = this;
		Debug.Log("Multiplayer sound manager setup");
	}

	// Token: 0x06000512 RID: 1298 RVA: 0x00018089 File Offset: 0x00016289
	private void Start()
	{
		if (this.soundManager != null && base.isClientOnly)
		{
			this.soundManager.DisableLocalSound();
		}
	}

	// Token: 0x06000513 RID: 1299 RVA: 0x000180AC File Offset: 0x000162AC
	public void PlaySound(CollisionSoundType collisionSoundType, Vector3 position, float volume = 1f)
	{
		if (base.isServer)
		{
			this.PlaySoundOnClient(collisionSoundType, position, volume);
		}
	}

	// Token: 0x06000514 RID: 1300 RVA: 0x000180C0 File Offset: 0x000162C0
	[ClientRpc]
	private void PlaySoundOnClient(CollisionSoundType collisionSoundType, Vector3 position, float volume)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		Mirror.GeneratedNetworkCode._Write_CollisionSoundType(writer, collisionSoundType);
		writer.WriteVector3(position);
		writer.WriteFloat(volume);
		this.SendRPCInternal("System.Void SoundManagerMultiplayer::PlaySoundOnClient(CollisionSoundType,UnityEngine.Vector3,System.Single)", -232455089, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000516 RID: 1302 RVA: 0x0000C7D7 File Offset: 0x0000A9D7
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x06000517 RID: 1303 RVA: 0x0001810E File Offset: 0x0001630E
	protected void UserCode_PlaySoundOnClient__CollisionSoundType__Vector3__Single(CollisionSoundType collisionSoundType, Vector3 position, float volume)
	{
		if (this.soundManager != null && !base.isServer)
		{
			this.soundManager.PlaySound(collisionSoundType, position, volume);
		}
	}

	// Token: 0x06000518 RID: 1304 RVA: 0x00018134 File Offset: 0x00016334
	protected static void InvokeUserCode_PlaySoundOnClient__CollisionSoundType__Vector3__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC PlaySoundOnClient called on server.");
			return;
		}
		((SoundManagerMultiplayer)obj).UserCode_PlaySoundOnClient__CollisionSoundType__Vector3__Single(Mirror.GeneratedNetworkCode._Read_CollisionSoundType(reader), reader.ReadVector3(), reader.ReadFloat());
	}

	// Token: 0x06000519 RID: 1305 RVA: 0x0001816A File Offset: 0x0001636A
	static SoundManagerMultiplayer()
	{
		RemoteProcedureCalls.RegisterRpc(typeof(SoundManagerMultiplayer), "System.Void SoundManagerMultiplayer::PlaySoundOnClient(CollisionSoundType,UnityEngine.Vector3,System.Single)", new RemoteCallDelegate(SoundManagerMultiplayer.InvokeUserCode_PlaySoundOnClient__CollisionSoundType__Vector3__Single));
	}

	// Token: 0x04000313 RID: 787
	public SoundManager soundManager;
}
