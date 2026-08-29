using System;
using Es.InkPainter;
using UnityEngine;

// Token: 0x02000254 RID: 596
public class BladeOLD : MonoBehaviour
{
	// Token: 0x0600117F RID: 4479 RVA: 0x00059883 File Offset: 0x00057A83
	private void Start()
	{
		this.weaponRigidbody = base.GetComponentInParent<Rigidbody>();
	}

	// Token: 0x06001180 RID: 4480 RVA: 0x00059894 File Offset: 0x00057A94
	private void FixedUpdate()
	{
		this.waitCount++;
		if (this.joint != null)
		{
			Collider[] array = null;
			bool flag = false;
			if (this.joint.connectedBody != null)
			{
				array = this.joint.connectedBody.GetComponents<Collider>();
				if (array != null)
				{
					foreach (Collider collider in array)
					{
						flag = this.bladeTriggerCollider.bounds.Intersects(collider.bounds);
						if (flag)
						{
							break;
						}
					}
				}
			}
			if (!flag)
			{
				this.weapon.IgnoreCollision(array, false);
				this.dragPoint.SetActive(false);
				UnityEngine.Object.Destroy(this.joint);
				this.joint = null;
			}
		}
	}

	// Token: 0x06001181 RID: 4481 RVA: 0x00059950 File Offset: 0x00057B50
	public virtual void OnTriggerEnter(Collider collision)
	{
		IWeaponDamageable componentInParent = collision.transform.GetComponentInParent<IWeaponDamageable>();
		if (componentInParent != null)
		{
			this.dragPoint.SetActive(true);
			this.bladePenetrating = true;
			this.colliderTagName = collision.gameObject.tag;
			this.weapon.IgnoreCollision(collision.GetComponents<Collider>(), true);
			componentInParent.Destory(null, true);
			this.collisionEnterPos = base.gameObject.transform.position;
			if (this.joint == null || this.joint.connectedBody == null)
			{
				Rigidbody componentInParent2 = collision.GetComponentInParent<Rigidbody>();
				this.joint = this.weaponRigidbody.gameObject.AddComponent<ConfigurableJoint>();
				this.joint.anchor = this.dragPoint.transform.localPosition;
				this.joint.connectedBody = componentInParent2;
				this.joint.angularXMotion = ConfigurableJointMotion.Free;
				this.joint.angularYMotion = ConfigurableJointMotion.Locked;
				this.joint.angularZMotion = ConfigurableJointMotion.Locked;
				this.joint.xMotion = ConfigurableJointMotion.Locked;
				this.joint.yMotion = ConfigurableJointMotion.Free;
				this.joint.zMotion = ConfigurableJointMotion.Free;
			}
		}
	}

	// Token: 0x06001182 RID: 4482 RVA: 0x00059A7B File Offset: 0x00057C7B
	private void OnTriggerStay(Collider collision)
	{
		collision.transform.GetComponentInParent<IWeaponDamageable>();
	}

	// Token: 0x06001183 RID: 4483 RVA: 0x00059A89 File Offset: 0x00057C89
	public virtual void OnTriggerExit(Collider collision)
	{
		bool componentInParent = collision.transform.GetComponentInParent<IWeaponDamageable>() != null;
		this.bladePenetrating = false;
		this.colliderTagName = "";
		if (componentInParent)
		{
			this.collisionExitPos = base.gameObject.transform.position;
		}
	}

	// Token: 0x04000D0F RID: 3343
	public ConfigurableJoint joint;

	// Token: 0x04000D10 RID: 3344
	public Rigidbody weaponRigidbody;

	// Token: 0x04000D11 RID: 3345
	public GameObject dragPoint;

	// Token: 0x04000D12 RID: 3346
	public Collider bladeTriggerCollider;

	// Token: 0x04000D13 RID: 3347
	public Weapon weapon;

	// Token: 0x04000D14 RID: 3348
	public bool bladePenetrating;

	// Token: 0x04000D15 RID: 3349
	public string colliderTagName = "";

	// Token: 0x04000D16 RID: 3350
	private Vector3 collisionEnterPos;

	// Token: 0x04000D17 RID: 3351
	private Vector3 collisionExitPos;

	// Token: 0x04000D18 RID: 3352
	public float CutPlaneSize = 30f;

	// Token: 0x04000D19 RID: 3353
	public bool cut;

	// Token: 0x04000D1A RID: 3354
	[SerializeField]
	private Brush brush;

	// Token: 0x04000D1B RID: 3355
	[SerializeField]
	private int wait = 3;

	// Token: 0x04000D1C RID: 3356
	private int waitCount;
}
