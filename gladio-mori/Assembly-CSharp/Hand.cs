using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using MoveClasses;
using UnityEngine;

// Token: 0x0200013F RID: 319
public class Hand : Bodypart, ILegacy
{
	// Token: 0x060009EC RID: 2540 RVA: 0x0002EBD6 File Offset: 0x0002CDD6
	public void Awake()
	{
		this.handState = HandState.Hold;
		this.playerHealth = base.GetComponentInParent<PlayerHealth>();
	}

	// Token: 0x060009ED RID: 2541 RVA: 0x0002EBEC File Offset: 0x0002CDEC
	public override void Start()
	{
		if (base.transform.name == "Hand_Left")
		{
			this.handSide = BodySide.Left;
		}
		else
		{
			this.handSide = BodySide.Right;
		}
		if (NetworkManager.singleton != null && NetworkManager.singleton.mode == NetworkManagerMode.ClientOnly)
		{
			this.disableLocalLogic = true;
		}
		base.Start();
	}

	// Token: 0x060009EE RID: 2542 RVA: 0x0002EC48 File Offset: 0x0002CE48
	public virtual void OnTriggerEnter(Collider collision)
	{
		if (this.disableLocalLogic)
		{
			return;
		}
		if ((this.handState == HandState.Hold || this.handState == HandState.LooseHold) && !this.grabbed)
		{
			IGrabbable componentInParent = collision.transform.GetComponentInParent<IGrabbable>();
			if (componentInParent != null)
			{
				this.SetGrabbedItem(componentInParent, null);
			}
		}
	}

	// Token: 0x060009EF RID: 2543 RVA: 0x0002EC98 File Offset: 0x0002CE98
	public void SetGrabbedItem(GameObject item, float equipmentStartHoldPosition = 0f)
	{
		if (item != null)
		{
			IGrabbable componentInChildren = item.GetComponentInChildren<IGrabbable>();
			this.SetGrabbedItem(componentInChildren, new float?(equipmentStartHoldPosition));
		}
	}

	// Token: 0x060009F0 RID: 2544 RVA: 0x0002ECC4 File Offset: 0x0002CEC4
	public void SetGrabbedItem(IGrabbable grabbable, float? equipmentStartHoldPosition = null)
	{
		if (grabbable != null)
		{
			List<Hand> grabbingHands = grabbable.GetGrabbingHands();
			Hand hand = grabbingHands.FirstOrDefault<Hand>();
			Vector3? holdPosition = grabbable.GetHoldPosition(this.spawnPosition.position, this, false, equipmentStartHoldPosition);
			if (holdPosition == null)
			{
				return;
			}
			if (this.playerHealth == null || !this.playerHealth.alive)
			{
				return;
			}
			if (hand != null && this.playerHealth != null && (this.playerHealth != hand.playerHealth || !grabbable.IsTwoHanded))
			{
				return;
			}
			List<Collider> handleColliders = grabbable.GetHandleColliders();
			this.SetCollisionWithHands(handleColliders, true);
			if (this.joint != null)
			{
				this.SetJointToNoHold();
			}
			this.grabbed = true;
			if (this.joint == null || this.joint.connectedBody == null)
			{
				this.currentlyGrabbedItem = grabbable;
				this.joint = grabbable.GetRigidbody().gameObject.AddComponent<ConfigurableJoint>();
				this.joint.autoConfigureConnectedAnchor = false;
				this.joint.anchor = holdPosition.Value;
				this.joint.connectedAnchor = this.spawnPosition.localPosition;
				this.joint.connectedBody = this.bodypartRigidbody;
				this.joint.xMotion = ConfigurableJointMotion.Locked;
				this.joint.yMotion = ConfigurableJointMotion.Locked;
				this.joint.zMotion = ConfigurableJointMotion.Locked;
				if (this.handState == HandState.LooseHold)
				{
					this.SetJointToLoose();
				}
				this.joint.angularXMotion = ConfigurableJointMotion.Free;
				this.joint.angularYMotion = ConfigurableJointMotion.Free;
				this.joint.angularZMotion = ConfigurableJointMotion.Free;
				this.joint.angularXDrive = new JointDrive
				{
					positionSpring = 5000f,
					positionDamper = 1f,
					maximumForce = 3.402823E+38f
				};
				this.joint.angularYZDrive = new JointDrive
				{
					positionSpring = 5000f,
					positionDamper = 1f,
					maximumForce = 3.402823E+38f
				};
				if (grabbingHands.Count == 0)
				{
					grabbable.GetRigidbody().transform.parent = base.transform;
				}
				Quaternion quaternion = Quaternion.Euler(0f, 0f, 0f);
				Transform holdTransform = grabbable.GetHoldTransform();
				Vector3 physicalHoldRotation = grabbable.GetPhysicalHoldRotation();
				if (holdTransform != null)
				{
					if (this.handSide == BodySide.Right)
					{
						quaternion = Quaternion.Euler(new Vector3(physicalHoldRotation.x, physicalHoldRotation.y, physicalHoldRotation.z));
					}
					else
					{
						quaternion = Quaternion.Euler(new Vector3(physicalHoldRotation.x, -physicalHoldRotation.y, -physicalHoldRotation.z));
					}
				}
				bool flag = false;
				if (Vector3.Dot(base.transform.forward, -holdTransform.right) < 0f)
				{
					flag = true;
				}
				if (flag)
				{
					quaternion *= Quaternion.Euler(0f, 180f, 0f);
				}
				if (grabbingHands.Count == 0)
				{
					this.joint.SetTargetRotationLocal(base.transform.localRotation * Quaternion.Inverse(grabbable.GetRigidbody().transform.localRotation) * quaternion, grabbable.GetStartRotation());
				}
				else
				{
					this.joint.configuredInWorldSpace = true;
					Quaternion rotation = Quaternion.identity * Quaternion.Inverse(base.transform.rotation);
					Quaternion targetWorldRotation = Quaternion.identity * Quaternion.Inverse(grabbable.GetRigidbody().transform.rotation) * Quaternion.Inverse(rotation) * quaternion;
					this.joint.SetTestRotation(targetWorldRotation, grabbable.GetStartRotationGlobal());
					this.joint.angularXDrive = new JointDrive
					{
						positionSpring = 50000f,
						positionDamper = 1f,
						maximumForce = 3.402823E+38f
					};
					this.joint.angularYZDrive = new JointDrive
					{
						positionSpring = 50000f,
						positionDamper = 1f,
						maximumForce = 3.402823E+38f
					};
				}
				grabbable.SetGrabbingHand(this);
				grabbable.CheckAsleep();
				if (this.handMultiplayer != null && hand == null)
				{
					this.handMultiplayer.SetGrabbedItem(this.currentlyGrabbedItem.GetRigidbody().gameObject);
				}
			}
		}
	}

	// Token: 0x060009F1 RID: 2545 RVA: 0x0002F11D File Offset: 0x0002D31D
	public void SetHandState(HandState newHandState)
	{
		if (this.handState != newHandState)
		{
			if (newHandState == HandState.NoHold)
			{
				this.SetJointToNoHold();
			}
			else if (newHandState == HandState.LooseHold)
			{
				this.SetJointToLoose();
			}
			else if (newHandState == HandState.Hold)
			{
				this.SetJointToHold();
			}
			this.handState = newHandState;
		}
	}

	// Token: 0x060009F2 RID: 2546 RVA: 0x0002F150 File Offset: 0x0002D350
	private void SetJointToNoHold()
	{
		if (this.joint != null)
		{
			UnityEngine.Object.Destroy(this.joint);
		}
		if (this.currentlyGrabbedItem != null)
		{
			this.grabbablesToReEnable.Add(this.currentlyGrabbedItem);
			base.Invoke("ReEnableHandCollisions", 0.3f);
			this.currentlyGrabbedItem.RemoveGrabbingHand(this);
			if (this.currentlyGrabbedItem.GetGrabbingHands().Count == 0)
			{
				this.currentlyGrabbedItem.GetRigidbody().transform.parent = null;
				this.currentlyGrabbedItem.CheckAsleep();
				if (this.handMultiplayer != null)
				{
					this.handMultiplayer.ServerRemoveGrabbedItem();
				}
			}
			else if (this.IsPrimaryHoldingHand())
			{
				this.currentlyGrabbedItem.GetGrabbingHands()[0].SetAsPrimaryHand();
			}
		}
		this.grabbed = false;
		this.joint = null;
		this.currentlyGrabbedItem = null;
	}

	// Token: 0x060009F3 RID: 2547 RVA: 0x0002F230 File Offset: 0x0002D430
	private void SetJointToLoose()
	{
		if (this.joint != null && this.currentlyGrabbedItem != null)
		{
			this.joint.zMotion = ConfigurableJointMotion.Limited;
			this.joint.linearLimit = new SoftJointLimit
			{
				limit = this.currentlyGrabbedItem.handleLength
			};
			this.joint.anchor = this.currentlyGrabbedItem.GetHandlePosition();
		}
	}

	// Token: 0x060009F4 RID: 2548 RVA: 0x0002F29C File Offset: 0x0002D49C
	private void SetJointToHold()
	{
		if (this.joint != null && this.currentlyGrabbedItem != null)
		{
			this.joint.zMotion = ConfigurableJointMotion.Locked;
			Vector3? holdPosition = this.currentlyGrabbedItem.GetHoldPosition(this.spawnPosition.position, this, true, null);
			if (holdPosition != null)
			{
				this.joint.anchor = holdPosition.Value;
				return;
			}
			this.SetJointToNoHold();
		}
	}

	// Token: 0x060009F5 RID: 2549 RVA: 0x0002F310 File Offset: 0x0002D510
	private void SetCollisionWithHands(List<Collider> handleColliders, bool ignore = true)
	{
		foreach (Collider collider in handleColliders)
		{
			if (collider != null)
			{
				foreach (Collider collider2 in this.handColliders)
				{
					if (collider2 != null)
					{
						Physics.IgnoreCollision(collider, collider2, ignore);
					}
				}
			}
		}
	}

	// Token: 0x060009F6 RID: 2550 RVA: 0x0002F3AC File Offset: 0x0002D5AC
	private void CleanHandColliders()
	{
		for (int i = this.handColliders.Count - 1; i > -1; i--)
		{
			if (this.handColliders[i] == null)
			{
				this.handColliders.RemoveAt(i);
			}
		}
	}

	// Token: 0x060009F7 RID: 2551 RVA: 0x0002F3F1 File Offset: 0x0002D5F1
	private void DisableHandCollisions()
	{
		if (this.currentlyGrabbedItem != null)
		{
			this.SetCollisionWithHands(this.currentlyGrabbedItem.GetHandleColliders(), true);
		}
	}

	// Token: 0x060009F8 RID: 2552 RVA: 0x0002F410 File Offset: 0x0002D610
	private void ReEnableHandCollisions()
	{
		if (this.playerHealth.alive)
		{
			IGrabbable grabbable = this.grabbablesToReEnable.FirstOrDefault<IGrabbable>();
			if (grabbable != null)
			{
				if (grabbable != this.currentlyGrabbedItem)
				{
					this.SetCollisionWithHands(grabbable.GetHandleColliders(), false);
				}
				this.grabbablesToReEnable.RemoveAt(0);
			}
		}
	}

	// Token: 0x060009F9 RID: 2553 RVA: 0x0002F45B File Offset: 0x0002D65B
	public bool IsPrimaryHoldingHand()
	{
		return this.currentlyGrabbedItem != null && this.currentlyGrabbedItem.GetRigidbody().transform.parent == base.transform;
	}

	// Token: 0x060009FA RID: 2554 RVA: 0x0002F48C File Offset: 0x0002D68C
	public void SetAsPrimaryHand()
	{
		if (this.currentlyGrabbedItem != null)
		{
			this.currentlyGrabbedItem.GetRigidbody().transform.parent = base.transform;
			if (this.handMultiplayer != null)
			{
				this.handMultiplayer.SetGrabbedItem(this.currentlyGrabbedItem.GetRigidbody().gameObject);
			}
		}
	}

	// Token: 0x060009FB RID: 2555 RVA: 0x0002F4E5 File Offset: 0x0002D6E5
	public void RegisterColliderAsHandCollider(List<Collider> collider)
	{
		this.CleanHandColliders();
		this.handColliders.AddRange(collider);
		this.DisableHandCollisions();
	}

	// Token: 0x17000156 RID: 342
	// (get) Token: 0x060009FC RID: 2556 RVA: 0x0002F500 File Offset: 0x0002D700
	public Rigidbody Rigidbody
	{
		get
		{
			Rigidbody result;
			if ((result = this._rigidbody) == null)
			{
				result = (this._rigidbody = base.gameObject.transform.parent.GetComponent<Rigidbody>());
			}
			return result;
		}
	}

	// Token: 0x17000157 RID: 343
	// (get) Token: 0x060009FD RID: 2557 RVA: 0x0002F535 File Offset: 0x0002D735
	// (set) Token: 0x060009FE RID: 2558 RVA: 0x0002F53D File Offset: 0x0002D73D
	public bool legacyInitialized { get; set; }

	// Token: 0x060009FF RID: 2559 RVA: 0x0002F546 File Offset: 0x0002D746
	public void SetLegacy(bool legacy)
	{
		this.InitLegacy();
		if (legacy)
		{
			this.Rigidbody.drag = this.legacyDrag;
			return;
		}
		this.Rigidbody.drag = this.normalDrag;
	}

	// Token: 0x06000A00 RID: 2560 RVA: 0x0002F574 File Offset: 0x0002D774
	public void InitLegacy()
	{
		if (!this.legacyInitialized)
		{
			this.normalDrag = this.Rigidbody.drag;
			this.legacyInitialized = true;
		}
	}

	// Token: 0x06000A01 RID: 2561 RVA: 0x0002F596 File Offset: 0x0002D796
	public bool LegacyItemExists()
	{
		return base.enabled;
	}

	// Token: 0x040006EC RID: 1772
	public HandState handState = HandState.Hold;

	// Token: 0x040006ED RID: 1773
	public List<Collider> handColliders;

	// Token: 0x040006EE RID: 1774
	public Collider handTrigger;

	// Token: 0x040006EF RID: 1775
	public bool grabbed;

	// Token: 0x040006F0 RID: 1776
	public ConfigurableJoint joint;

	// Token: 0x040006F1 RID: 1777
	public BodySide handSide;

	// Token: 0x040006F2 RID: 1778
	public IGrabbable currentlyGrabbedItem;

	// Token: 0x040006F3 RID: 1779
	public bool initialized;

	// Token: 0x040006F4 RID: 1780
	public HandMultiplayer handMultiplayer;

	// Token: 0x040006F5 RID: 1781
	public GameObject placeholderGameObject;

	// Token: 0x040006F6 RID: 1782
	public PlayerHealth playerHealth;

	// Token: 0x040006F7 RID: 1783
	public bool disableLocalLogic;

	// Token: 0x040006F8 RID: 1784
	private List<IGrabbable> grabbablesToReEnable = new List<IGrabbable>();

	// Token: 0x040006F9 RID: 1785
	private Rigidbody _rigidbody;

	// Token: 0x040006FB RID: 1787
	private float legacyDrag;

	// Token: 0x040006FC RID: 1788
	private float normalDrag;
}
