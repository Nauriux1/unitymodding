using System;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

// Token: 0x02000149 RID: 329
public class HandMultiplayer : NetworkBehaviour
{
	// Token: 0x06000A35 RID: 2613 RVA: 0x00030469 File Offset: 0x0002E669
	public virtual void CurrentlyHeldItemIdChanged(uint? oldValue, uint? newValue)
	{
		this.UpdateLocalGrabbedItem();
	}

	// Token: 0x06000A36 RID: 2614 RVA: 0x00030471 File Offset: 0x0002E671
	private void UpdateLocalGrabbedItem()
	{
		if (this.currentlyHeldItemId != null)
		{
			this.SetGrabbedItemOnClient(this.currentlyHeldItemId.Value);
			return;
		}
		this.ClientRemoveGrabbedItem();
	}

	// Token: 0x06000A37 RID: 2615 RVA: 0x0000777A File Offset: 0x0000597A
	private void Awake()
	{
	}

	// Token: 0x06000A38 RID: 2616 RVA: 0x00030498 File Offset: 0x0002E698
	private void Update()
	{
		if (this.itemToGrabAtStart != null && this.itemToGrabAtStart.GetComponent<NetworkIdentity>().netId != 0U && ((MultiplayerRoomManager)NetworkManager.singleton).loadedPlayers.Count >= ((MultiplayerRoomManager)NetworkManager.singleton).numPlayers)
		{
			this.SetGrabbedItem(this.itemToGrabAtStart);
			this.itemToGrabAtStart = null;
		}
		if (this.updateLocalGrabbedItem)
		{
			this.updateLocalGrabbedItem = false;
			this.UpdateLocalGrabbedItem();
		}
	}

	// Token: 0x06000A39 RID: 2617 RVA: 0x00030512 File Offset: 0x0002E712
	public void GrabItemAtStart(GameObject grabbedItem)
	{
		this.itemToGrabAtStart = grabbedItem;
	}

	// Token: 0x06000A3A RID: 2618 RVA: 0x0003051B File Offset: 0x0002E71B
	public void RemoveGrabItemAtStart()
	{
		if (this.itemToGrabAtStart != null)
		{
			this.itemToGrabAtStart = null;
		}
	}

	// Token: 0x06000A3B RID: 2619 RVA: 0x00030534 File Offset: 0x0002E734
	public void SetGrabbedItem(GameObject grabbedItem)
	{
		if (base.isServer)
		{
			NetworkIdentity component = grabbedItem.GetComponent<NetworkIdentity>();
			if (component.netId == 0U || ((MultiplayerRoomManager)NetworkManager.singleton).loadedPlayers.Count != ((MultiplayerRoomManager)NetworkManager.singleton).numPlayers)
			{
				this.GrabItemAtStart(grabbedItem);
				return;
			}
			if (component != null)
			{
				MultiplayerTransform component2 = grabbedItem.GetComponent<MultiplayerTransform>();
				if (component2 != null)
				{
					this.currentlyHeldItem = component2;
				}
				if (component2 != null)
				{
					component2.disableChanges = true;
					component2.holdingHand = this;
				}
				this.itemTransform.target = grabbedItem.transform;
				this.NetworkcurrentlyHeldItemId = new uint?(component.netId);
				uint? num = this.otherHandMultiplayer.currentlyHeldItemId;
				uint? num2 = this.currentlyHeldItemId;
				if (num.GetValueOrDefault() == num2.GetValueOrDefault() & num != null == (num2 != null))
				{
					this.otherHandMultiplayer.ServerRemoveGrabbedItem();
				}
			}
		}
	}

	// Token: 0x06000A3C RID: 2620 RVA: 0x00030628 File Offset: 0x0002E828
	private void SetGrabbedItemOnClient(uint grabbedNetId)
	{
		if (base.isServer)
		{
			return;
		}
		if (grabbedNetId > 0U && this.hand != null && !base.isServer)
		{
			if (!NetworkClient.spawned.ContainsKey(grabbedNetId))
			{
				this.updateLocalGrabbedItem = true;
				return;
			}
			uint? num = this.otherHandMultiplayer.currentlyHeldItemId;
			uint? num2 = this.currentlyHeldItemId;
			if (num.GetValueOrDefault() == num2.GetValueOrDefault() & num != null == (num2 != null))
			{
				this.otherHandMultiplayer.ClientRemoveGrabbedItem();
			}
			GameObject gameObject = NetworkClient.spawned[grabbedNetId].gameObject;
			gameObject.transform.SetParent(this.hand.transform);
			Weapon component = gameObject.GetComponent<Weapon>();
			if (component != null)
			{
				component.CheckAsleep();
			}
			MultiplayerTransform component2 = gameObject.GetComponent<MultiplayerTransform>();
			if (component2 != null)
			{
				this.currentlyHeldItem = component2;
			}
			if (component2 != null)
			{
				component2.disableChanges = true;
				component2.holdingHand = this;
			}
			this.itemTransform.target = component2.transform;
		}
	}

	// Token: 0x06000A3D RID: 2621 RVA: 0x00030734 File Offset: 0x0002E934
	public void ServerRemoveGrabbedItem()
	{
		if (base.isServer)
		{
			this.RemoveGrabItemAtStart();
			this.NetworkcurrentlyHeldItemId = null;
		}
	}

	// Token: 0x06000A3E RID: 2622 RVA: 0x00030764 File Offset: 0x0002E964
	public void ClientRemoveGrabbedItem()
	{
		if (this.currentlyHeldItem != null && this.currentlyHeldItem.holdingHand == this)
		{
			this.currentlyHeldItem.transform.parent = null;
			Weapon component = this.currentlyHeldItem.GetComponent<Weapon>();
			if (component != null)
			{
				component.CheckAsleep();
			}
			MultiplayerTransform multiplayerTransform = this.currentlyHeldItem;
			if (multiplayerTransform != null)
			{
				multiplayerTransform.disableChanges = false;
				multiplayerTransform.holdingHand = null;
				multiplayerTransform.ResetPositionInterpolation();
			}
		}
		this.itemTransform.target = this.hand.placeholderGameObject.transform;
		this.currentlyHeldItem = null;
	}

	// Token: 0x06000A40 RID: 2624 RVA: 0x0000C7D7 File Offset: 0x0000A9D7
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x17000158 RID: 344
	// (get) Token: 0x06000A41 RID: 2625 RVA: 0x00030804 File Offset: 0x0002EA04
	// (set) Token: 0x06000A42 RID: 2626 RVA: 0x00030817 File Offset: 0x0002EA17
	public uint? NetworkcurrentlyHeldItemId
	{
		get
		{
			return this.currentlyHeldItemId;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<uint?>(value, ref this.currentlyHeldItemId, 1UL, new Action<uint?, uint?>(this.CurrentlyHeldItemIdChanged));
		}
	}

	// Token: 0x06000A43 RID: 2627 RVA: 0x00030840 File Offset: 0x0002EA40
	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteUIntNullable(this.currentlyHeldItemId);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1UL) != 0UL)
		{
			writer.WriteUIntNullable(this.currentlyHeldItemId);
		}
	}

	// Token: 0x06000A44 RID: 2628 RVA: 0x00030898 File Offset: 0x0002EA98
	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			base.GeneratedSyncVarDeserialize<uint?>(ref this.currentlyHeldItemId, new Action<uint?, uint?>(this.CurrentlyHeldItemIdChanged), reader.ReadUIntNullable());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 1L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<uint?>(ref this.currentlyHeldItemId, new Action<uint?, uint?>(this.CurrentlyHeldItemIdChanged), reader.ReadUIntNullable());
		}
	}

	// Token: 0x04000736 RID: 1846
	public MultiplayerTransform itemTransform;

	// Token: 0x04000737 RID: 1847
	public Hand hand;

	// Token: 0x04000738 RID: 1848
	public MultiplayerTransform currentlyHeldItem;

	// Token: 0x04000739 RID: 1849
	[NonSerialized]
	public GameObject itemToGrabAtStart;

	// Token: 0x0400073A RID: 1850
	public HandMultiplayer otherHandMultiplayer;

	// Token: 0x0400073B RID: 1851
	[SyncVar(hook = "CurrentlyHeldItemIdChanged")]
	private uint? currentlyHeldItemId;

	// Token: 0x0400073C RID: 1852
	private bool updateLocalGrabbedItem;
}
