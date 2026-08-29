using System;
using System.Collections.Generic;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

// Token: 0x02000071 RID: 113
public class CuttableMultiplayerHandler : NetworkBehaviour
{
	// Token: 0x06000314 RID: 788 RVA: 0x00010034 File Offset: 0x0000E234
	public void DoFullCut(int id, Plane plane, uint newCuttableObjectNetID)
	{
		if (base.isServer)
		{
			this.fullCuts.Add(new FullCut
			{
				id = id,
				plane = plane,
				newCuttableObjectNetID = newCuttableObjectNetID
			});
			this.DoFullCutOnClient(id, plane, newCuttableObjectNetID);
		}
	}

	// Token: 0x06000315 RID: 789 RVA: 0x00010080 File Offset: 0x0000E280
	[ClientRpc]
	private void DoFullCutOnClient(int id, Plane plane, uint newCuttableObjectNetID)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(id);
		writer.WritePlane(plane);
		writer.WriteUInt(newCuttableObjectNetID);
		this.SendRPCInternal("System.Void CuttableMultiplayerHandler::DoFullCutOnClient(System.Int32,UnityEngine.Plane,System.UInt32)", 1246205311, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000316 RID: 790 RVA: 0x000100D0 File Offset: 0x0000E2D0
	public void SendOldCutsToClient(NetworkConnectionToClient target)
	{
		for (int i = 0; i < this.fullCuts.Count; i++)
		{
			FullCut fullCut = this.fullCuts[i];
			this.DoFullCutOnTarget(target, fullCut.id, fullCut.plane, fullCut.newCuttableObjectNetID);
		}
	}

	// Token: 0x06000317 RID: 791 RVA: 0x0001011C File Offset: 0x0000E31C
	[TargetRpc]
	public void DoFullCutOnTarget(NetworkConnectionToClient target, int id, Plane plane, uint newCuttableObjectNetID)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(id);
		writer.WritePlane(plane);
		writer.WriteUInt(newCuttableObjectNetID);
		this.SendTargetRPCInternal(target, "System.Void CuttableMultiplayerHandler::DoFullCutOnTarget(Mirror.NetworkConnectionToClient,System.Int32,UnityEngine.Plane,System.UInt32)", -2057673540, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000318 RID: 792 RVA: 0x0001016C File Offset: 0x0000E36C
	private void DoTheClientCut(int id, Plane plane, uint newCuttableObjectNetID, bool doNotUpdatePositionOnCut = false)
	{
		if (base.isServer)
		{
			return;
		}
		if (id < this.cuttableGameObjects.Length)
		{
			CuttableGameObject cuttableGameObject = this.cuttableGameObjects[id];
			if (cuttableGameObject != null)
			{
				if (doNotUpdatePositionOnCut)
				{
					cuttableGameObject.doNotUpdatePositionOnFullCut = true;
				}
				cuttableGameObject.DoFullCut(plane, newCuttableObjectNetID);
			}
		}
	}

	// Token: 0x0600031A RID: 794 RVA: 0x0000C7D7 File Offset: 0x0000A9D7
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x0600031B RID: 795 RVA: 0x000101C7 File Offset: 0x0000E3C7
	protected void UserCode_DoFullCutOnClient__Int32__Plane__UInt32(int id, Plane plane, uint newCuttableObjectNetID)
	{
		this.DoTheClientCut(id, plane, newCuttableObjectNetID, false);
	}

	// Token: 0x0600031C RID: 796 RVA: 0x000101D3 File Offset: 0x0000E3D3
	protected static void InvokeUserCode_DoFullCutOnClient__Int32__Plane__UInt32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC DoFullCutOnClient called on server.");
			return;
		}
		((CuttableMultiplayerHandler)obj).UserCode_DoFullCutOnClient__Int32__Plane__UInt32(reader.ReadInt(), reader.ReadPlane(), reader.ReadUInt());
	}

	// Token: 0x0600031D RID: 797 RVA: 0x00010208 File Offset: 0x0000E408
	protected void UserCode_DoFullCutOnTarget__NetworkConnectionToClient__Int32__Plane__UInt32(NetworkConnectionToClient target, int id, Plane plane, uint newCuttableObjectNetID)
	{
		this.DoTheClientCut(id, plane, newCuttableObjectNetID, true);
	}

	// Token: 0x0600031E RID: 798 RVA: 0x00010215 File Offset: 0x0000E415
	protected static void InvokeUserCode_DoFullCutOnTarget__NetworkConnectionToClient__Int32__Plane__UInt32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC DoFullCutOnTarget called on server.");
			return;
		}
		((CuttableMultiplayerHandler)obj).UserCode_DoFullCutOnTarget__NetworkConnectionToClient__Int32__Plane__UInt32(null, reader.ReadInt(), reader.ReadPlane(), reader.ReadUInt());
	}

	// Token: 0x0600031F RID: 799 RVA: 0x0001024C File Offset: 0x0000E44C
	static CuttableMultiplayerHandler()
	{
		RemoteProcedureCalls.RegisterRpc(typeof(CuttableMultiplayerHandler), "System.Void CuttableMultiplayerHandler::DoFullCutOnClient(System.Int32,UnityEngine.Plane,System.UInt32)", new RemoteCallDelegate(CuttableMultiplayerHandler.InvokeUserCode_DoFullCutOnClient__Int32__Plane__UInt32));
		RemoteProcedureCalls.RegisterRpc(typeof(CuttableMultiplayerHandler), "System.Void CuttableMultiplayerHandler::DoFullCutOnTarget(Mirror.NetworkConnectionToClient,System.Int32,UnityEngine.Plane,System.UInt32)", new RemoteCallDelegate(CuttableMultiplayerHandler.InvokeUserCode_DoFullCutOnTarget__NetworkConnectionToClient__Int32__Plane__UInt32));
	}

	// Token: 0x0400021D RID: 541
	public CuttableGameObject[] cuttableGameObjects;

	// Token: 0x0400021E RID: 542
	public List<FullCut> fullCuts = new List<FullCut>(16);
}
