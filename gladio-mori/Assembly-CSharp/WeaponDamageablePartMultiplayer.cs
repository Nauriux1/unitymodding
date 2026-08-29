using System;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

// Token: 0x0200014E RID: 334
public class WeaponDamageablePartMultiplayer : NetworkBehaviour
{
	// Token: 0x06000A6F RID: 2671 RVA: 0x0000777A File Offset: 0x0000597A
	private void Awake()
	{
	}

	// Token: 0x06000A70 RID: 2672 RVA: 0x00030DF4 File Offset: 0x0002EFF4
	public void Destroyed()
	{
		if (base.isServer)
		{
			this.DestroyOnClient();
		}
	}

	// Token: 0x06000A71 RID: 2673 RVA: 0x00030E04 File Offset: 0x0002F004
	[ClientRpc]
	private void DestroyOnClient()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		this.SendRPCInternal("System.Void WeaponDamageablePartMultiplayer::DestroyOnClient()", -94830809, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000A73 RID: 2675 RVA: 0x0000C7D7 File Offset: 0x0000A9D7
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x06000A74 RID: 2676 RVA: 0x00030E34 File Offset: 0x0002F034
	protected void UserCode_DestroyOnClient()
	{
		if (this.weaponDamageablePart != null)
		{
			this.weaponDamageablePart.PlayDestroyVisuals(null);
		}
	}

	// Token: 0x06000A75 RID: 2677 RVA: 0x00030E63 File Offset: 0x0002F063
	protected static void InvokeUserCode_DestroyOnClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC DestroyOnClient called on server.");
			return;
		}
		((WeaponDamageablePartMultiplayer)obj).UserCode_DestroyOnClient();
	}

	// Token: 0x06000A76 RID: 2678 RVA: 0x00030E86 File Offset: 0x0002F086
	static WeaponDamageablePartMultiplayer()
	{
		RemoteProcedureCalls.RegisterRpc(typeof(WeaponDamageablePartMultiplayer), "System.Void WeaponDamageablePartMultiplayer::DestroyOnClient()", new RemoteCallDelegate(WeaponDamageablePartMultiplayer.InvokeUserCode_DestroyOnClient));
	}

	// Token: 0x0400074B RID: 1867
	public WeaponDamageablePart weaponDamageablePart;
}
