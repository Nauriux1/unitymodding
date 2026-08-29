using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

// Token: 0x02000247 RID: 583
public class Blade : MonoBehaviour, IBlade, ILegacy
{
	// Token: 0x060010EE RID: 4334 RVA: 0x0005760C File Offset: 0x0005580C
	private void Start()
	{
		this.penetratingGameObjects = new List<PenetratingObject>(32);
		this.weaponRigidbody = base.GetComponentInParent<Rigidbody>();
		if (NetworkManager.singleton != null && NetworkManager.singleton.mode == NetworkManagerMode.ClientOnly)
		{
			this.disableLocalLogic = true;
		}
		this.CreateBladePainterArray();
		this.RegisterBladePainters();
	}

	// Token: 0x060010EF RID: 4335 RVA: 0x00057660 File Offset: 0x00055860
	private void CreateBladePainterArray()
	{
		if (this.bladePainters == null || this.bladePainters.Length == 0)
		{
			this.bladePainters = base.gameObject.transform.parent.GetComponentsInChildren<BladePainter>();
		}
		if (this.allBladePainters == null || this.allBladePainters.Length == 0)
		{
			this.allBladePainters = base.gameObject.transform.parent.GetComponentsInChildren<BladePainter>();
		}
	}

	// Token: 0x060010F0 RID: 4336 RVA: 0x000576C8 File Offset: 0x000558C8
	private void RegisterBladePainters()
	{
		if (this.allBladePainters != null)
		{
			BladePainter[] array = this.allBladePainters;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].blade = this;
			}
		}
	}

	// Token: 0x060010F1 RID: 4337 RVA: 0x000576FB File Offset: 0x000558FB
	public BladePainter[] GetAllBladePainters()
	{
		this.CreateBladePainterArray();
		return this.allBladePainters;
	}

	// Token: 0x060010F2 RID: 4338 RVA: 0x00057709 File Offset: 0x00055909
	public BladePainter[] GetBladePainters()
	{
		return this.bladePainters;
	}

	// Token: 0x060010F3 RID: 4339 RVA: 0x00057714 File Offset: 0x00055914
	private void FixedUpdate()
	{
		this.cutting = false;
		if (this.penetratingGameObjects.Count > 0)
		{
			this.cutting = true;
		}
		if (!this.cutting && this.joint != null)
		{
			UnityEngine.Object.Destroy(this.joint);
			this.joint = null;
		}
	}

	// Token: 0x060010F4 RID: 4340 RVA: 0x00057768 File Offset: 0x00055968
	public bool CheckBoneBreak(IWeaponDamageable damageable, Collider collision, float collisionMagnitude = -1f)
	{
		if (collision.attachedRigidbody != null)
		{
			Vector3 a = collision.ClosestPoint(base.gameObject.transform.position);
			Vector3 b = this.bladeTriggerCollider.ClosestPoint(collision.attachedRigidbody.transform.position);
			Vector3 worldPoint = (a + b) / 2f;
			if (collisionMagnitude < 0f)
			{
				Vector3 pointVelocity = collision.attachedRigidbody.GetPointVelocity(worldPoint);
				Vector3 pointVelocity2 = this.weaponRigidbody.GetPointVelocity(worldPoint);
				collisionMagnitude = (pointVelocity - pointVelocity2).magnitude;
			}
			collisionMagnitude *= collisionMagnitude;
			float num;
			if (this.IsStabbing(collision))
			{
				num = collisionMagnitude * this.stabBoneMultiplier;
			}
			else
			{
				num = collisionMagnitude * this.slashBoneMultiplier;
			}
			if (num > 9f)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060010F5 RID: 4341 RVA: 0x0005782E File Offset: 0x00055A2E
	public virtual void OnTriggerEnter(Collider collision)
	{
		this.HandleBladeCutStart(collision, this);
	}

	// Token: 0x060010F6 RID: 4342 RVA: 0x00057838 File Offset: 0x00055A38
	public void HandleBladeCutStart(Collider collision, IBlade iBlade)
	{
		this.forceNotAsleepThisFrame = true;
		GameObject gameObject = collision.gameObject;
		IWeaponDamageable component = collision.transform.GetComponent<IWeaponDamageable>();
		if (component == null && collision.transform.parent != null)
		{
			component = collision.transform.parent.GetComponent<IWeaponDamageable>();
			gameObject = collision.transform.parent.gameObject;
		}
		if (component != null)
		{
			if (component.IsOrgan())
			{
				if (this.disableLocalLogic)
				{
					return;
				}
				if (!component.IsBone())
				{
					component.Destory(null, true);
					return;
				}
				if (this.CheckBoneBreak(component, collision, -1f))
				{
					component.Destory(null, true);
					return;
				}
			}
			else
			{
				this.bladePenetrating = true;
				this.colliderTagName = collision.gameObject.tag;
				collision.GetComponents<Collider>();
				this.AddPenetratingGameObject(gameObject, collision, iBlade, component.bladePaintables, component.GetCuttableGameObjects());
				if (this.disableLocalLogic)
				{
					return;
				}
				this.collisionEnterPos = base.gameObject.transform.position;
				Rigidbody component2 = collision.GetComponent<Rigidbody>();
				if (component2 == null)
				{
					component2 = collision.transform.parent.GetComponent<Rigidbody>();
				}
				if (component2 != null)
				{
					if (this.joint == null || this.joint.connectedBody == null)
					{
						this.joint = this.weaponRigidbody.gameObject.AddComponent<ConfigurableJoint>();
						this.joint.anchor = this.dragPoint.transform.localPosition;
						this.joint.connectedBody = component2;
						this.joint.angularXMotion = ConfigurableJointMotion.Free;
						this.joint.angularYMotion = ConfigurableJointMotion.Locked;
						this.joint.angularZMotion = ConfigurableJointMotion.Locked;
						this.joint.xMotion = ConfigurableJointMotion.Locked;
						this.joint.yMotion = ConfigurableJointMotion.Free;
						this.joint.zMotion = ConfigurableJointMotion.Free;
						this.joint.enableCollision = true;
						float num = 3000f;
						float num2 = 1000f * this.slashMultiplier - this.cutResistanceIncreaseVariable;
						float num3 = 1000f * this.stabMultiplier;
						if (num2 > num)
						{
							num2 = num;
						}
						if (num3 > num)
						{
							num3 = num;
						}
						JointDrive jointDrive = new JointDrive
						{
							positionSpring = 0f,
							positionDamper = num - num2,
							maximumForce = float.MaxValue
						};
						JointDrive zDrive = new JointDrive
						{
							positionSpring = 0f,
							positionDamper = num - num3,
							maximumForce = float.MaxValue
						};
						JointDrive jointDrive2 = new JointDrive
						{
							positionSpring = 0f,
							positionDamper = num - num2,
							maximumForce = float.MaxValue
						};
						this.joint.angularXDrive = jointDrive2;
						this.joint.angularYZDrive = jointDrive2;
						this.joint.xDrive = jointDrive;
						this.joint.yDrive = jointDrive;
						this.joint.zDrive = zDrive;
						return;
					}
					this.joint.connectedBody = component2;
					return;
				}
			}
		}
		else
		{
			CuttableObject component3 = collision.transform.GetComponent<CuttableObject>();
			if (component3 == null && collision.transform.parent != null)
			{
				component3 = collision.transform.parent.GetComponent<CuttableObject>();
			}
			if (component3 != null)
			{
				this.AddPenetratingGameObject(collision.gameObject, collision, iBlade, null, null);
			}
		}
	}

	// Token: 0x060010F7 RID: 4343 RVA: 0x00057B94 File Offset: 0x00055D94
	private void AddPenetratingGameObject(GameObject penetratingGameObject, Collider collider, IBlade iBlade, List<BladePaintable> bladePaintables = null, List<CuttableGameObject> cuttableGameObjects = null)
	{
		PenetratingObject penetratingObject = new PenetratingObject
		{
			gameObject = penetratingGameObject,
			collider = collider,
			bladePaintables = bladePaintables,
			cuttableGameObjects = cuttableGameObjects,
			cutItems = new List<CutItem>()
		};
		BladePainter[] array = iBlade.GetBladePainters();
		if (array != null)
		{
			for (int i = 0; i < array.Length; i++)
			{
				array[i].AddBladePaintable(penetratingObject.bladePaintables);
			}
		}
		if (cuttableGameObjects != null && CutManager.cutManagerActive)
		{
			for (int j = 0; j < cuttableGameObjects.Count; j++)
			{
				penetratingObject.cutItems.Add(CutManager.singleton.AddCutItem(cuttableGameObjects[j], this.weapon));
			}
		}
		this.penetratingGameObjects.Add(penetratingObject);
	}

	// Token: 0x060010F8 RID: 4344 RVA: 0x00057C44 File Offset: 0x00055E44
	private void RemovePenetratingGameObject(GameObject penetratingGameObject, IBlade iBlade)
	{
		PenetratingObject penetratingObject = (from x in this.penetratingGameObjects
		where x.gameObject == penetratingGameObject
		select x).FirstOrDefault<PenetratingObject>();
		if (penetratingObject != null)
		{
			BladePainter[] array = iBlade.GetBladePainters();
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i].RemoveBladePaintable(penetratingObject.bladePaintables);
				}
			}
			if (penetratingObject.cutItems != null)
			{
				for (int j = 0; j < penetratingObject.cutItems.Count; j++)
				{
					CutManager.singleton.RemoveCutItem(penetratingObject.cutItems[j]);
				}
			}
			this.collisionExitPos = base.gameObject.transform.position;
			this.penetratingGameObjects.Remove(penetratingObject);
		}
	}

	// Token: 0x060010F9 RID: 4345 RVA: 0x00057D03 File Offset: 0x00055F03
	public virtual void OnTriggerExit(Collider collision)
	{
		this.HandleBladeCutEnd(collision, this);
	}

	// Token: 0x060010FA RID: 4346 RVA: 0x00057D10 File Offset: 0x00055F10
	public void HandleBladeCutEnd(Collider collision, IBlade iBlade)
	{
		GameObject gameObject = collision.gameObject;
		IWeaponDamageable component = collision.transform.GetComponent<IWeaponDamageable>();
		if (component == null && collision.transform.parent != null)
		{
			component = collision.transform.parent.GetComponent<IWeaponDamageable>();
			gameObject = collision.transform.parent.gameObject;
		}
		this.bladePenetrating = false;
		this.colliderTagName = "";
		if (component != null)
		{
			this.RemovePenetratingGameObject(gameObject, iBlade);
			return;
		}
		if (collision.transform.GetComponentInParent<CuttableObject>() != null)
		{
			this.RemovePenetratingGameObject(collision.gameObject, iBlade);
		}
	}

	// Token: 0x060010FB RID: 4347 RVA: 0x00057DA8 File Offset: 0x00055FA8
	public bool IsStabbing(Collider collision)
	{
		RaycastHit raycastHit;
		return this.bladeTip != null && collision.Raycast(new Ray(this.bladeTip.position, this.bladeTip.forward * 0.1f), out raycastHit, 100f);
	}

	// Token: 0x060010FC RID: 4348 RVA: 0x00057DFC File Offset: 0x00055FFC
	public bool BladeIsAwake()
	{
		if (this.bladeIsAsleep)
		{
			if (this.lastFrameAwakeChecked < Time.frameCount)
			{
				if (this.forceNotAsleepThisFrame || (base.transform.position - this.oldPosition).sqrMagnitude > this.sqrSleepDistance)
				{
					this.oldPosition = base.transform.position;
					this.calculatedAwakeValue = true;
					this.forceNotAsleepThisFrame = false;
				}
				else
				{
					this.calculatedAwakeValue = false;
				}
				this.lastFrameAwakeChecked = Time.frameCount;
			}
			return this.calculatedAwakeValue;
		}
		return true;
	}

	// Token: 0x060010FD RID: 4349 RVA: 0x00057E87 File Offset: 0x00056087
	public void SetAsleep(bool value)
	{
		if (value != this.bladeIsAsleep)
		{
			this.bladeIsAsleep = value;
		}
	}

	// Token: 0x170001B0 RID: 432
	// (get) Token: 0x060010FE RID: 4350 RVA: 0x00057E9C File Offset: 0x0005609C
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

	// Token: 0x170001B1 RID: 433
	// (get) Token: 0x060010FF RID: 4351 RVA: 0x00057ED1 File Offset: 0x000560D1
	// (set) Token: 0x06001100 RID: 4352 RVA: 0x00057ED9 File Offset: 0x000560D9
	public bool legacyInitialized { get; set; }

	// Token: 0x06001101 RID: 4353 RVA: 0x00057EE4 File Offset: 0x000560E4
	public void SetLegacy(bool legacy)
	{
		this.InitLegacy();
		if (legacy)
		{
			this.cutResistanceIncreaseVariable = 0f;
			if (this.useLegacyMultipliers)
			{
				this.slashMultiplier = this.legacySlashMultiplier;
				this.stabMultiplier = this.legacyStabMultiplier;
				return;
			}
		}
		else
		{
			this.cutResistanceIncreaseVariable = this.normalCutResistanceIncrease;
			this.slashMultiplier = this.normalSlashMultiplier;
			this.stabMultiplier = this.normalStabMultiplier;
		}
	}

	// Token: 0x06001102 RID: 4354 RVA: 0x00057F4A File Offset: 0x0005614A
	public void InitLegacy()
	{
		if (!this.legacyInitialized)
		{
			this.normalCutResistanceIncrease = this.cutResistanceIncreaseVariable;
			this.normalSlashMultiplier = this.slashMultiplier;
			this.normalStabMultiplier = this.stabMultiplier;
			this.legacyInitialized = true;
		}
	}

	// Token: 0x06001103 RID: 4355 RVA: 0x0002F596 File Offset: 0x0002D796
	public bool LegacyItemExists()
	{
		return base.enabled;
	}

	// Token: 0x04000C99 RID: 3225
	public ConfigurableJoint joint;

	// Token: 0x04000C9A RID: 3226
	public ConfigurableJoint boneJoint;

	// Token: 0x04000C9B RID: 3227
	public Rigidbody weaponRigidbody;

	// Token: 0x04000C9C RID: 3228
	public GameObject dragPoint;

	// Token: 0x04000C9D RID: 3229
	public Collider bladeTriggerCollider;

	// Token: 0x04000C9E RID: 3230
	public Weapon weapon;

	// Token: 0x04000C9F RID: 3231
	public string colliderTagName = "";

	// Token: 0x04000CA0 RID: 3232
	private Vector3 collisionEnterPos;

	// Token: 0x04000CA1 RID: 3233
	private Vector3 collisionExitPos;

	// Token: 0x04000CA2 RID: 3234
	public bool bladePenetrating;

	// Token: 0x04000CA3 RID: 3235
	public List<PenetratingObject> penetratingGameObjects;

	// Token: 0x04000CA4 RID: 3236
	public float stabMultiplier = 2f;

	// Token: 0x04000CA5 RID: 3237
	public float slashMultiplier = 1f;

	// Token: 0x04000CA6 RID: 3238
	public float stabBoneMultiplier = 2f;

	// Token: 0x04000CA7 RID: 3239
	public float slashBoneMultiplier = 2f;

	// Token: 0x04000CA8 RID: 3240
	public Transform bladeTip;

	// Token: 0x04000CA9 RID: 3241
	public bool disableLocalLogic;

	// Token: 0x04000CAA RID: 3242
	public BladePainter[] bladePainters;

	// Token: 0x04000CAB RID: 3243
	public BladePainter[] allBladePainters;

	// Token: 0x04000CAC RID: 3244
	private bool cutting;

	// Token: 0x04000CAD RID: 3245
	private float cutResistanceIncreaseVariable = 300f;

	// Token: 0x04000CAE RID: 3246
	public bool bladeIsAsleep;

	// Token: 0x04000CAF RID: 3247
	private Vector3 oldPosition;

	// Token: 0x04000CB0 RID: 3248
	private int lastFrameAwakeChecked;

	// Token: 0x04000CB1 RID: 3249
	private bool calculatedAwakeValue;

	// Token: 0x04000CB2 RID: 3250
	private bool forceNotAsleepThisFrame;

	// Token: 0x04000CB3 RID: 3251
	private float sqrSleepDistance = 0.0001f;

	// Token: 0x04000CB4 RID: 3252
	private Rigidbody _rigidbody;

	// Token: 0x04000CB6 RID: 3254
	private float normalCutResistanceIncrease = 300f;

	// Token: 0x04000CB7 RID: 3255
	[Header("Legacy")]
	public bool useLegacyMultipliers;

	// Token: 0x04000CB8 RID: 3256
	public float legacySlashMultiplier = 1f;

	// Token: 0x04000CB9 RID: 3257
	public float legacyStabMultiplier = 1f;

	// Token: 0x04000CBA RID: 3258
	private float normalSlashMultiplier = 1f;

	// Token: 0x04000CBB RID: 3259
	private float normalStabMultiplier = 1f;
}
