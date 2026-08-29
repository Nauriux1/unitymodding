using System;
using System.Collections.Generic;
using Mirror;
using MoveClasses;
using UnityEngine;

// Token: 0x0200006B RID: 107
public class CuttableGameObject : MonoBehaviour
{
	// Token: 0x06000301 RID: 769 RVA: 0x0000FC44 File Offset: 0x0000DE44
	private void Awake()
	{
		this.historyPositionTracker = new HistoryPositionTracker(base.gameObject);
	}

	// Token: 0x06000302 RID: 770 RVA: 0x0000FC57 File Offset: 0x0000DE57
	private void Update()
	{
		this.historyPositionTracker.UpdateHistory();
	}

	// Token: 0x06000303 RID: 771 RVA: 0x0000FC64 File Offset: 0x0000DE64
	public void InitCuttableGameObject()
	{
		if (!this.init)
		{
			this.cuttableRigidbody = base.GetComponent<Rigidbody>();
			this.cuttableSections = new List<CuttableSection>();
			ConfigurableJoint[] components = base.GetComponents<ConfigurableJoint>();
			for (int i = 0; i < components.Length; i++)
			{
				CuttableSection cuttableSection = new CuttableSection();
				cuttableSection.joint = components[i];
				cuttableSection.hand = components[i].gameObject.GetComponentInChildren<Hand>();
				this.cuttableSections.Add(cuttableSection);
			}
		}
	}

	// Token: 0x06000304 RID: 772 RVA: 0x0000FCD3 File Offset: 0x0000DED3
	public void AddEquipment(Transform transform)
	{
		this.cuttableSections.Add(new CuttableSection
		{
			gameObjectTransform = transform,
			isEquipment = true
		});
	}

	// Token: 0x06000305 RID: 773 RVA: 0x0000FCF4 File Offset: 0x0000DEF4
	public void ClearEquipment()
	{
		for (int i = this.cuttableSections.Count - 1; i > -1; i--)
		{
			if (this.cuttableSections[i].isEquipment)
			{
				this.cuttableSections.RemoveAt(i);
			}
		}
	}

	// Token: 0x06000306 RID: 774 RVA: 0x0000FD38 File Offset: 0x0000DF38
	public void UpdateCuttableMeshes()
	{
		for (int i = this.cuttableMeshList.Count - 1; i > -1; i--)
		{
			if (!this.cuttableMeshList[i].renderer.enabled)
			{
				this.cuttableMeshList.RemoveAt(i);
			}
		}
	}

	// Token: 0x06000307 RID: 775 RVA: 0x0000FD81 File Offset: 0x0000DF81
	public void AddActiveCutItem(CutItem cutItem)
	{
		this.activeCutItems.Add(cutItem);
	}

	// Token: 0x06000308 RID: 776 RVA: 0x0000FD90 File Offset: 0x0000DF90
	public void RemoveActiveCutItem(CutItem cutItem)
	{
		for (int i = 0; i < this.activeCutItems.Count; i++)
		{
			if (this.activeCutItems[i] == cutItem)
			{
				this.activeCutItems.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x06000309 RID: 777 RVA: 0x0000FDD0 File Offset: 0x0000DFD0
	public CutItem GetCutItemForWeapon(Weapon weapon)
	{
		for (int i = 0; i < this.activeCutItems.Count; i++)
		{
			if (this.activeCutItems[i].weapon == weapon)
			{
				return this.activeCutItems[i];
			}
		}
		return null;
	}

	// Token: 0x0600030A RID: 778 RVA: 0x0000FE1C File Offset: 0x0000E01C
	public void DisableCutItems(bool disableLinked = false, bool horizontalCut = false)
	{
		for (int i = 0; i < this.activeCutItems.Count; i++)
		{
			if (disableLinked || horizontalCut)
			{
				this.activeCutItems[i].disabledFully = true;
			}
			else
			{
				this.activeCutItems[i].needsToBeReset = true;
			}
		}
		if (disableLinked)
		{
			if (this.parentCuttableGameObject != null)
			{
				this.parentCuttableGameObject.DisableCutItems(false, horizontalCut);
			}
			if (this.cuttableSections != null)
			{
				for (int j = 0; j < this.cuttableSections.Count; j++)
				{
					if (this.cuttableSections[j].cuttableGameObject != null)
					{
						this.cuttableSections[j].cuttableGameObject.DisableCutItems(false, horizontalCut);
					}
				}
			}
		}
	}

	// Token: 0x0600030B RID: 779 RVA: 0x0000FED7 File Offset: 0x0000E0D7
	public void DoFullCut(Plane plane, uint newCuttableObjectNetID)
	{
		this.pendingCuttableObjectNetID = newCuttableObjectNetID;
		CutManager.singleton.AddCutItem(this, plane);
	}

	// Token: 0x0600030C RID: 780 RVA: 0x0000FEED File Offset: 0x0000E0ED
	public void ServerInformClientsToCut(Plane plane, CuttableGameObject newCuttableGameObject)
	{
		this.cuttableMultiplayerHandler.DoFullCut((int)this.bodyPart, plane, newCuttableGameObject.networkIdentity.netId);
	}

	// Token: 0x0600030D RID: 781 RVA: 0x0000FF0C File Offset: 0x0000E10C
	public void Activate()
	{
		if (!NetworkClient.active || NetworkServer.active)
		{
			this.cuttableRigidbody.isKinematic = false;
		}
	}

	// Token: 0x0600030E RID: 782 RVA: 0x0000FF28 File Offset: 0x0000E128
	public void Disable()
	{
		this.cuttableRigidbody.isKinematic = true;
	}

	// Token: 0x040001ED RID: 493
	public PlayerHealth playerHealth;

	// Token: 0x040001EE RID: 494
	public JointType bodyPart;

	// Token: 0x040001EF RID: 495
	public CuttableGameObject parentCuttableGameObject;

	// Token: 0x040001F0 RID: 496
	public Rigidbody cuttableRigidbody;

	// Token: 0x040001F1 RID: 497
	public List<CuttableMesh> cuttableMeshList;

	// Token: 0x040001F2 RID: 498
	public List<CuttableSection> cuttableSections;

	// Token: 0x040001F3 RID: 499
	public CuttableCollider[] cuttableColliders;

	// Token: 0x040001F4 RID: 500
	public List<GameObject> objectsToDisable;

	// Token: 0x040001F5 RID: 501
	public bool cutDone;

	// Token: 0x040001F6 RID: 502
	public CuttableMultiplayerHandler cuttableMultiplayerHandler;

	// Token: 0x040001F7 RID: 503
	private bool init;

	// Token: 0x040001F8 RID: 504
	public HistoryPositionTracker historyPositionTracker;

	// Token: 0x040001F9 RID: 505
	public List<CuttableGameObject> cuttableGameObjectsToIgnoreCollisions;

	// Token: 0x040001FA RID: 506
	public List<Collider> localCollidersForOthersToIgnore;

	// Token: 0x040001FB RID: 507
	public List<Collider> localCollidersToIgnoreWhenChildOfCutSection;

	// Token: 0x040001FC RID: 508
	public List<CutItem> activeCutItems = new List<CutItem>(8);

	// Token: 0x040001FD RID: 509
	public NetworkIdentity networkIdentity;

	// Token: 0x040001FE RID: 510
	public uint pendingCuttableObjectNetID;

	// Token: 0x040001FF RID: 511
	public MultiplayerTransform multiplayerTransform;

	// Token: 0x04000200 RID: 512
	public bool doNotUpdatePositionOnFullCut;
}
