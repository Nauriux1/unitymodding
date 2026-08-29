using System;
using System.Collections.Generic;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

// Token: 0x0200014F RID: 335
public class WeaponDamageablePartMultiplayerHandler : NetworkBehaviour
{
	// Token: 0x06000A77 RID: 2679 RVA: 0x00030EA8 File Offset: 0x0002F0A8
	public void Destroyed(int id, DamageOrigin damageOrigin)
	{
		if (base.isServer)
		{
			this.DestroyOnClient(id, damageOrigin);
		}
	}

	// Token: 0x06000A78 RID: 2680 RVA: 0x00030EBC File Offset: 0x0002F0BC
	[ClientRpc]
	private void DestroyOnClient(int id, DamageOrigin damageOrigin)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(id);
		Mirror.GeneratedNetworkCode._Write_DamageOrigin(writer, damageOrigin);
		this.SendRPCInternal("System.Void WeaponDamageablePartMultiplayerHandler::DestroyOnClient(System.Int32,DamageOrigin)", 940338887, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000A79 RID: 2681 RVA: 0x00030F00 File Offset: 0x0002F100
	public void StopDestroyVisuals(int id)
	{
		if (base.isServer)
		{
			this.StopDestroyOnClient(id);
		}
	}

	// Token: 0x06000A7A RID: 2682 RVA: 0x00030F14 File Offset: 0x0002F114
	[ClientRpc]
	private void StopDestroyOnClient(int id)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(id);
		this.SendRPCInternal("System.Void WeaponDamageablePartMultiplayerHandler::StopDestroyOnClient(System.Int32)", 414669480, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000A7C RID: 2684 RVA: 0x0000C7D7 File Offset: 0x0000A9D7
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x06000A7D RID: 2685 RVA: 0x00030F50 File Offset: 0x0002F150
	protected void UserCode_DestroyOnClient__Int32__DamageOrigin(int id, DamageOrigin damageOrigin)
	{
		if (id < this.weaponDamageableParts.Count)
		{
			WeaponDamageablePart weaponDamageablePart = this.weaponDamageableParts[id];
			if (weaponDamageablePart != null)
			{
				weaponDamageablePart.PlayDestroyVisuals(new DamageOrigin?(damageOrigin));
				weaponDamageablePart.ActivateBleed();
			}
		}
	}

	// Token: 0x06000A7E RID: 2686 RVA: 0x00030F93 File Offset: 0x0002F193
	protected static void InvokeUserCode_DestroyOnClient__Int32__DamageOrigin(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC DestroyOnClient called on server.");
			return;
		}
		((WeaponDamageablePartMultiplayerHandler)obj).UserCode_DestroyOnClient__Int32__DamageOrigin(reader.ReadInt(), Mirror.GeneratedNetworkCode._Read_DamageOrigin(reader));
	}

	// Token: 0x06000A7F RID: 2687 RVA: 0x00030FC4 File Offset: 0x0002F1C4
	protected void UserCode_StopDestroyOnClient__Int32(int id)
	{
		if (id < this.weaponDamageableParts.Count)
		{
			WeaponDamageablePart weaponDamageablePart = this.weaponDamageableParts[id];
			if (weaponDamageablePart != null)
			{
				weaponDamageablePart.StopDestroyVisuals();
			}
		}
	}

	// Token: 0x06000A80 RID: 2688 RVA: 0x00030FFB File Offset: 0x0002F1FB
	protected static void InvokeUserCode_StopDestroyOnClient__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC StopDestroyOnClient called on server.");
			return;
		}
		((WeaponDamageablePartMultiplayerHandler)obj).UserCode_StopDestroyOnClient__Int32(reader.ReadInt());
	}

	// Token: 0x06000A81 RID: 2689 RVA: 0x00031024 File Offset: 0x0002F224
	static WeaponDamageablePartMultiplayerHandler()
	{
		RemoteProcedureCalls.RegisterRpc(typeof(WeaponDamageablePartMultiplayerHandler), "System.Void WeaponDamageablePartMultiplayerHandler::DestroyOnClient(System.Int32,DamageOrigin)", new RemoteCallDelegate(WeaponDamageablePartMultiplayerHandler.InvokeUserCode_DestroyOnClient__Int32__DamageOrigin));
		RemoteProcedureCalls.RegisterRpc(typeof(WeaponDamageablePartMultiplayerHandler), "System.Void WeaponDamageablePartMultiplayerHandler::StopDestroyOnClient(System.Int32)", new RemoteCallDelegate(WeaponDamageablePartMultiplayerHandler.InvokeUserCode_StopDestroyOnClient__Int32));
	}

	// Token: 0x0400074C RID: 1868
	public List<WeaponDamageablePart> weaponDamageableParts;
}
