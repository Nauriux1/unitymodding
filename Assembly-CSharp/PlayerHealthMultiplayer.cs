using System;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;
using Utils;

// Token: 0x0200014C RID: 332
public class PlayerHealthMultiplayer : NetworkBehaviour
{
	// Token: 0x06000A5B RID: 2651 RVA: 0x00030A83 File Offset: 0x0002EC83
	private void Update()
	{
		this.UpdateStaminaToClient();
	}

	// Token: 0x06000A5C RID: 2652 RVA: 0x00030A8C File Offset: 0x0002EC8C
	public void UpdateStaminaToClient()
	{
		if (base.isServer)
		{
			if (this.playerHealth == null || this.playerHealth.multiplayerRoomPlayer == null || this.playerHealth.multiplayerRoomPlayer.netIdentity.connectionToClient == null)
			{
				return;
			}
			this.CheckLastSendTime();
			if (this.sendIntervalCounter == this.sendIntervalMultiplier)
			{
				if (Generic.FloatEquals(this.playerHealth.staminaArms, this.previousStaminaArms) && Generic.FloatEquals(this.playerHealth.staminaCore, this.previousStaminaCore) && Generic.FloatEquals(this.playerHealth.staminaLegs, this.previousStaminaLegs))
				{
					return;
				}
				this.previousStaminaArms = this.playerHealth.staminaArms;
				this.previousStaminaCore = this.playerHealth.staminaCore;
				this.previousStaminaLegs = this.playerHealth.staminaLegs;
				this.UpdateStamina(this.playerHealth.multiplayerRoomPlayer.netIdentity.connectionToClient, this.playerHealth.staminaArms, this.playerHealth.staminaCore, this.playerHealth.staminaLegs);
			}
		}
	}

	// Token: 0x06000A5D RID: 2653 RVA: 0x00030BAC File Offset: 0x0002EDAC
	[TargetRpc]
	private void UpdateStamina(NetworkConnectionToClient target, float staminaArms, float staminaCore, float staminaLegs)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteFloat(staminaArms);
		writer.WriteFloat(staminaCore);
		writer.WriteFloat(staminaLegs);
		this.SendTargetRPCInternal(target, "System.Void PlayerHealthMultiplayer::UpdateStamina(Mirror.NetworkConnectionToClient,System.Single,System.Single,System.Single)", 385287546, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000A5E RID: 2654 RVA: 0x00030BFA File Offset: 0x0002EDFA
	protected virtual void CheckLastSendTime()
	{
		if (this.sendIntervalCounter == this.sendIntervalMultiplier)
		{
			this.sendIntervalCounter = 0;
		}
		if (AccurateInterval.Elapsed(NetworkTime.localTime, (double)NetworkServer.sendInterval, ref this.lastSendIntervalTime))
		{
			this.sendIntervalCounter++;
		}
	}

	// Token: 0x06000A5F RID: 2655 RVA: 0x00030C37 File Offset: 0x0002EE37
	public void BluntHitServer(BluntDamageEffect bluntDamageEffect)
	{
		if (base.isServer)
		{
			this.BluntHitClient(bluntDamageEffect);
		}
	}

	// Token: 0x06000A60 RID: 2656 RVA: 0x00030C48 File Offset: 0x0002EE48
	[ClientRpc]
	public void BluntHitClient(BluntDamageEffect bluntDamageEffect)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		Mirror.GeneratedNetworkCode._Write_BluntDamageEffect(writer, bluntDamageEffect);
		this.SendRPCInternal("System.Void PlayerHealthMultiplayer::BluntHitClient(BluntDamageEffect)", 605413114, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000A62 RID: 2658 RVA: 0x0000C7D7 File Offset: 0x0000A9D7
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x06000A63 RID: 2659 RVA: 0x00030CB2 File Offset: 0x0002EEB2
	protected void UserCode_UpdateStamina__NetworkConnectionToClient__Single__Single__Single(NetworkConnectionToClient target, float staminaArms, float staminaCore, float staminaLegs)
	{
		if (base.isClient)
		{
			this.playerHealth.staminaArms = staminaArms;
			this.playerHealth.staminaCore = staminaCore;
			this.playerHealth.staminaLegs = staminaLegs;
		}
	}

	// Token: 0x06000A64 RID: 2660 RVA: 0x00030CE1 File Offset: 0x0002EEE1
	protected static void InvokeUserCode_UpdateStamina__NetworkConnectionToClient__Single__Single__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC UpdateStamina called on server.");
			return;
		}
		((PlayerHealthMultiplayer)obj).UserCode_UpdateStamina__NetworkConnectionToClient__Single__Single__Single(null, reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat());
	}

	// Token: 0x06000A65 RID: 2661 RVA: 0x00030D1A File Offset: 0x0002EF1A
	protected void UserCode_BluntHitClient__BluntDamageEffect(BluntDamageEffect bluntDamageEffect)
	{
		if (base.isClientOnly)
		{
			BluntDamageHelpers.RecordBluntDamageEffect(this.playerHealth.weaponDamageableBodyParts[(int)bluntDamageEffect.BodyPart], bluntDamageEffect);
			BluntDamageHelpers.HandleBluntDamageEffects(this.playerHealth, bluntDamageEffect);
		}
	}

	// Token: 0x06000A66 RID: 2662 RVA: 0x00030D48 File Offset: 0x0002EF48
	protected static void InvokeUserCode_BluntHitClient__BluntDamageEffect(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC BluntHitClient called on server.");
			return;
		}
		((PlayerHealthMultiplayer)obj).UserCode_BluntHitClient__BluntDamageEffect(Mirror.GeneratedNetworkCode._Read_BluntDamageEffect(reader));
	}

	// Token: 0x06000A67 RID: 2663 RVA: 0x00030D74 File Offset: 0x0002EF74
	static PlayerHealthMultiplayer()
	{
		RemoteProcedureCalls.RegisterRpc(typeof(PlayerHealthMultiplayer), "System.Void PlayerHealthMultiplayer::BluntHitClient(BluntDamageEffect)", new RemoteCallDelegate(PlayerHealthMultiplayer.InvokeUserCode_BluntHitClient__BluntDamageEffect));
		RemoteProcedureCalls.RegisterRpc(typeof(PlayerHealthMultiplayer), "System.Void PlayerHealthMultiplayer::UpdateStamina(Mirror.NetworkConnectionToClient,System.Single,System.Single,System.Single)", new RemoteCallDelegate(PlayerHealthMultiplayer.InvokeUserCode_UpdateStamina__NetworkConnectionToClient__Single__Single__Single));
	}

	// Token: 0x0400073F RID: 1855
	public PlayerHealth playerHealth;

	// Token: 0x04000740 RID: 1856
	public CuttableMultiplayerHandler cuttableMultiplayerHandler;

	// Token: 0x04000741 RID: 1857
	private float previousStaminaArms = 1f;

	// Token: 0x04000742 RID: 1858
	private float previousStaminaCore = 1f;

	// Token: 0x04000743 RID: 1859
	private float previousStaminaLegs = 1f;

	// Token: 0x04000744 RID: 1860
	private int sendIntervalCounter;

	// Token: 0x04000745 RID: 1861
	private double lastSendIntervalTime;

	// Token: 0x04000746 RID: 1862
	private int sendIntervalMultiplier = 2;
}
