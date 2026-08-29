using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoveClasses;
using PlayerHelpers;
using UnityEngine;
using Utils;

// Token: 0x02000198 RID: 408
public class GenerateActiveRigidbody : MonoBehaviour
{
	// Token: 0x06000CA3 RID: 3235 RVA: 0x0003DA1B File Offset: 0x0003BC1B
	private void Start()
	{
		this.count = 0;
		this.mainObject = base.gameObject;
	}

	// Token: 0x06000CA4 RID: 3236 RVA: 0x0000777A File Offset: 0x0000597A
	private void Update()
	{
	}

	// Token: 0x06000CA5 RID: 3237 RVA: 0x0003DA30 File Offset: 0x0003BC30
	public void DOSHIT()
	{
		this.mainObject = base.gameObject;
		this.PrepareForGeneration();
		this.ClearActiveRagdoll();
		this.GenerateRigidbodyTree(base.gameObject);
		this.GenerateJointsTree(base.gameObject);
		this.UpdateRigidbodies();
		this.GenerateWeaponDamageableIDs();
		this.HandlePaintables();
		this.HandleAnimationMesh();
		this.StartGenerateCuttableGameObjects(base.gameObject);
		this.FillBluntDamageDealerScripts();
		this.FinishUpRagdoll();
	}

	// Token: 0x06000CA6 RID: 3238 RVA: 0x0003DA9D File Offset: 0x0003BC9D
	public void UpdateRigidbodiesAction()
	{
		this.mainObject = base.gameObject;
		this.PrepareForGeneration();
		this.UpdateRigidbodies();
	}

	// Token: 0x06000CA7 RID: 3239 RVA: 0x0003DAB8 File Offset: 0x0003BCB8
	public void ClearActiveRagdoll()
	{
		this.mainObject = base.gameObject;
		this.PrepareForGeneration();
		this.playerHealth.weaponDamageableBodyParts = new WeaponDamageableBodyPart[Enum.GetNames(typeof(JointType)).Length];
		this.DeleteAll(base.gameObject);
		this.DeleteScripts();
	}

	// Token: 0x06000CA8 RID: 3240 RVA: 0x0003DB0A File Offset: 0x0003BD0A
	public void CreateFighterJoints()
	{
		this.mainObject = base.gameObject;
		this.PrepareForGeneration();
		this.GenerateFighterJoint(base.gameObject);
	}

	// Token: 0x06000CA9 RID: 3241 RVA: 0x0003DB2A File Offset: 0x0003BD2A
	public void PrepareForGeneration()
	{
		this.playerHealth = base.GetComponent<PlayerHealth>();
		this.parentPaintable = null;
	}

	// Token: 0x06000CAA RID: 3242 RVA: 0x0003DB40 File Offset: 0x0003BD40
	private void DeleteScripts()
	{
		foreach (WeaponDamageableBodyPart obj in UnityEngine.Object.FindObjectsOfType<WeaponDamageableBodyPart>(true).ToList<WeaponDamageableBodyPart>())
		{
			UnityEngine.Object.DestroyImmediate(obj);
		}
		foreach (JointStrength obj2 in UnityEngine.Object.FindObjectsOfType<JointStrength>(true).ToList<JointStrength>())
		{
			UnityEngine.Object.DestroyImmediate(obj2);
		}
		foreach (BladePaintable obj3 in UnityEngine.Object.FindObjectsOfType<BladePaintable>(true).ToList<BladePaintable>())
		{
			UnityEngine.Object.DestroyImmediate(obj3);
		}
		foreach (Paintable obj4 in UnityEngine.Object.FindObjectsOfType<Paintable>(true).ToList<Paintable>())
		{
			UnityEngine.Object.DestroyImmediate(obj4);
		}
		foreach (PaintableChild obj5 in UnityEngine.Object.FindObjectsOfType<PaintableChild>(true).ToList<PaintableChild>())
		{
			UnityEngine.Object.DestroyImmediate(obj5);
		}
		foreach (CuttableGameObject obj6 in UnityEngine.Object.FindObjectsOfType<CuttableGameObject>(true).ToList<CuttableGameObject>())
		{
			UnityEngine.Object.DestroyImmediate(obj6);
		}
		foreach (BluntDamageDealerGameObject obj7 in UnityEngine.Object.FindObjectsOfType<BluntDamageDealerGameObject>(true).ToList<BluntDamageDealerGameObject>())
		{
			UnityEngine.Object.DestroyImmediate(obj7);
		}
	}

	// Token: 0x06000CAB RID: 3243 RVA: 0x0003DD30 File Offset: 0x0003BF30
	private void DeleteAll(GameObject gameObject)
	{
		foreach (object obj in gameObject.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.name == "PlayerModelAnimation")
			{
				this.animationObject = transform.gameObject;
			}
			else if (!(gameObject.name == "ActualBoxer") || !(transform.name != "Armature"))
			{
				if (!transform.name.Replace("_", "").Any(new Func<char, bool>(char.IsLower)))
				{
					this.DeleteAllComponents(transform.gameObject);
				}
				this.DeleteAll(transform.gameObject);
			}
		}
	}

	// Token: 0x06000CAC RID: 3244 RVA: 0x0003DE0C File Offset: 0x0003C00C
	private void DeleteAllComponents(GameObject gameObject)
	{
		foreach (ConfigurableJoint obj in gameObject.GetComponents<ConfigurableJoint>().ToList<ConfigurableJoint>())
		{
			UnityEngine.Object.DestroyImmediate(obj);
		}
		foreach (ConfigurableJointScript obj2 in gameObject.GetComponents<ConfigurableJointScript>().ToList<ConfigurableJointScript>())
		{
			UnityEngine.Object.DestroyImmediate(obj2);
		}
		foreach (Rigidbody obj3 in gameObject.GetComponents<Rigidbody>().ToList<Rigidbody>())
		{
			UnityEngine.Object.DestroyImmediate(obj3);
		}
	}

	// Token: 0x06000CAD RID: 3245 RVA: 0x0003DEEC File Offset: 0x0003C0EC
	private void GenerateRigidbodyTree(GameObject gameObject)
	{
		foreach (object obj in gameObject.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.name == "PlayerModelAnimation")
			{
				this.animationObject = transform.gameObject;
			}
			else if (!(gameObject.name == "ActualBoxer") || !(transform.name != "Armature"))
			{
				if (!transform.name.Replace("_", "").Any(new Func<char, bool>(char.IsLower)))
				{
					this.GenerateRigidbodies(transform.gameObject);
				}
				else if (transform.name.ToLower().Contains("lethal"))
				{
					this.HandleOrgans(transform.gameObject);
				}
				if (transform.name.ToLower().Contains("mesh") || transform.name.ToLower().Contains("ball"))
				{
					this.HandleMesh(transform.gameObject);
				}
				this.GenerateRigidbodyTree(transform.gameObject);
			}
		}
	}

	// Token: 0x06000CAE RID: 3246 RVA: 0x0003E03C File Offset: 0x0003C23C
	private void UpdateRigidbodies()
	{
		foreach (Rigidbody rigidbody in this.mainObject.GetComponentsInChildren<Rigidbody>().ToList<Rigidbody>())
		{
			rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
			rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		}
	}

	// Token: 0x06000CAF RID: 3247 RVA: 0x0003E0A0 File Offset: 0x0003C2A0
	private void GenerateFighterJoint(GameObject gameObject)
	{
		foreach (object obj in gameObject.transform)
		{
			Transform transform = (Transform)obj;
			if (!(transform.name == "PlayerModelPhysics"))
			{
				if (transform.name == "PlayerModelAnimation")
				{
					this.animationObject = transform.gameObject;
					this.playerAnimator = transform.gameObject.GetComponent<PlayerAnimator>();
					this.playerAnimator.FighterJoints = new List<FighterJoint>();
				}
				if (!(gameObject.name == "ActualBoxer") || !(transform.name != "Armature"))
				{
					if (!transform.name.Replace("_", "").Any(new Func<char, bool>(char.IsLower)))
					{
						this.GenerateFighterJoints(transform.gameObject);
					}
					this.GenerateFighterJoint(transform.gameObject);
				}
			}
		}
	}

	// Token: 0x06000CB0 RID: 3248 RVA: 0x0003E1B0 File Offset: 0x0003C3B0
	private void GenerateFighterJoints(GameObject fighterJointObject)
	{
		if (this.playerAnimator != null)
		{
			this.playerAnimator.FighterJoints.Add(new FighterJoint
			{
				jointType = (JointType)Enum.Parse(typeof(JointType), fighterJointObject.name, true),
				joint = fighterJointObject
			});
		}
	}

	// Token: 0x06000CB1 RID: 3249 RVA: 0x0003E208 File Offset: 0x0003C408
	private void GenerateJointsTree(GameObject gameObject)
	{
		foreach (object obj in gameObject.transform)
		{
			Transform transform = (Transform)obj;
			if (!(transform.name == "PlayerModelAnimation") && (!(gameObject.name == "ActualBoxer") || !(transform.name != "Armature")))
			{
				if (!transform.name.Replace("_", "").Any(new Func<char, bool>(char.IsLower)) && transform.gameObject.activeInHierarchy)
				{
					this.GenerateJoints(transform.gameObject);
				}
				this.GenerateJointsTree(transform.gameObject);
			}
		}
	}

	// Token: 0x06000CB2 RID: 3250 RVA: 0x0003E2E4 File Offset: 0x0003C4E4
	public void HandleMesh(GameObject meshObject)
	{
		MeshFilter component = meshObject.GetComponent<MeshFilter>();
		MeshRenderer component2 = meshObject.GetComponent<MeshRenderer>();
		if (meshObject.activeInHierarchy && component != null && component2 != null && !meshObject.name.Contains("MeshHand") && !meshObject.name.Contains("BallWrist"))
		{
			BladePaintable bladePaintable = meshObject.AddComponent<BladePaintable>();
			if (meshObject.name == "MeshHip")
			{
				this.parentPaintable = meshObject.AddComponent<Paintable>();
				this.parentPaintable.bladePaintable = bladePaintable;
				this.parentPaintable.playerHealth = this.playerHealth;
				using (IEnumerator enumerator = meshObject.transform.parent.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						WeaponDamageableBodyPart component3 = ((Transform)obj).GetComponent<WeaponDamageableBodyPart>();
						if (component3 != null)
						{
							bladePaintable.weaponDamageableBodyPartsForActivatingPainter.Add(component3);
						}
					}
					return;
				}
			}
			PaintableChild paintableChild = meshObject.AddComponent<PaintableChild>();
			bladePaintable.paintable = paintableChild;
			paintableChild.bladePaintable = bladePaintable;
		}
	}

	// Token: 0x06000CB3 RID: 3251 RVA: 0x0003E414 File Offset: 0x0003C614
	public void HandleAnimationMesh()
	{
		Renderer[] componentsInChildren = this.animationMeshRoot.GetComponentsInChildren<Renderer>();
		this.playerHealth.shareMaterialRenderers.Clear();
		foreach (Renderer renderer in componentsInChildren)
		{
			if ((renderer.name.ToLower().Contains("mesh") || renderer.name.ToLower().Contains("ball")) && !renderer.transform.parent.name.ToLower().Contains("wrist"))
			{
				this.playerHealth.shareMaterialRenderers.Add(renderer);
			}
		}
	}

	// Token: 0x06000CB4 RID: 3252 RVA: 0x0003E4B0 File Offset: 0x0003C6B0
	private void GenerateWeaponDamageableIDs()
	{
		int num = 0;
		List<WeaponDamageablePart> list = UnityEngine.Object.FindObjectsOfType<WeaponDamageablePart>().ToList<WeaponDamageablePart>();
		foreach (WeaponDamageablePart weaponDamageablePart in list)
		{
			if (weaponDamageablePart.id > num)
			{
				num = weaponDamageablePart.id;
			}
		}
		num++;
		foreach (WeaponDamageablePart weaponDamageablePart2 in list)
		{
			if (weaponDamageablePart2.id < 0)
			{
				weaponDamageablePart2.id = num;
				num++;
			}
		}
	}

	// Token: 0x06000CB5 RID: 3253 RVA: 0x0003E568 File Offset: 0x0003C768
	public void HandlePaintables()
	{
		foreach (PaintableChild paintableChild in UnityEngine.Object.FindObjectsOfType<PaintableChild>(true).ToList<PaintableChild>())
		{
			paintableChild.parentPaintable = this.parentPaintable;
			this.parentPaintable.children.Add(paintableChild);
		}
	}

	// Token: 0x06000CB6 RID: 3254 RVA: 0x0000777A File Offset: 0x0000597A
	public void HandleOrgans(GameObject organ)
	{
	}

	// Token: 0x06000CB7 RID: 3255 RVA: 0x0003E5D8 File Offset: 0x0003C7D8
	private void GenerateRigidbodies(GameObject gameObject)
	{
		Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
		if (rigidbody == null)
		{
			rigidbody = gameObject.AddComponent<Rigidbody>();
			rigidbody.mass = 20f;
		}
		if (gameObject.name == "HIP")
		{
			ConfigurableJoint configurableJoint = gameObject.AddComponent<ConfigurableJoint>();
			this.playerHealth.ballHolderjoint = configurableJoint;
			configurableJoint.connectedBody = this.hipSphere.GetComponent<Rigidbody>();
			configurableJoint.autoConfigureConnectedAnchor = true;
			configurableJoint.anchor = gameObject.transform.localPosition;
			ConfigurableJointScript configurableJointScript = gameObject.AddComponent<ConfigurableJointScript>();
			configurableJointScript.hj = configurableJoint;
			configurableJointScript.target = Generic.FindChildObject(this.animationObject.transform, "HIP", null).transform;
			configurableJointScript.firstRotation = "x";
			configurableJointScript.secondRotation = "y";
			configurableJointScript.thirdRotation = "z";
			configurableJointScript.invertFirst = true;
			configurableJointScript.invertSecond = true;
			configurableJointScript.invertThird = true;
			configurableJoint.xMotion = ConfigurableJointMotion.Free;
			configurableJoint.zMotion = ConfigurableJointMotion.Free;
			configurableJoint.yMotion = ConfigurableJointMotion.Free;
			configurableJoint.angularYMotion = ConfigurableJointMotion.Free;
			configurableJoint.angularZMotion = ConfigurableJointMotion.Free;
			configurableJoint.angularXMotion = ConfigurableJointMotion.Free;
			configurableJoint.angularXDrive = PlayerJointHelpers.GetHipJointDriveX(false);
			configurableJoint.angularYZDrive = PlayerJointHelpers.GetHipJointDriveYZ(false);
			configurableJoint.lowAngularXLimit = new SoftJointLimit
			{
				bounciness = 0f,
				contactDistance = 0f,
				limit = -90f
			};
			configurableJoint.highAngularXLimit = new SoftJointLimit
			{
				bounciness = 0f,
				contactDistance = 0f,
				limit = 90f
			};
			configurableJoint.angularZLimit = new SoftJointLimit
			{
				bounciness = 0f,
				contactDistance = 0f,
				limit = 90f
			};
			configurableJoint.angularYLimit = new SoftJointLimit
			{
				bounciness = 0f,
				contactDistance = 0f,
				limit = 90f
			};
			JointStrength jointStrength = gameObject.AddComponent<JointStrength>();
			jointStrength.joint = configurableJoint;
			jointStrength.jointName = gameObject.name;
			jointStrength.strengthPercentsForDamageInstances = new List<float>
			{
				20f,
				10f,
				5f,
				1f
			};
			configurableJointScript.jointStrength = jointStrength;
		}
		if (this.rollerBladeLeft != null && gameObject.name.Contains("KNEE") && gameObject.name.Contains("LEFT"))
		{
			this.rollerBladeLeft.connectedBody = rigidbody;
		}
		else if (this.rollerBladeRight != null && gameObject.name.Contains("KNEE") && gameObject.name.Contains("RIGHT"))
		{
			this.rollerBladeRight.connectedBody = rigidbody;
		}
		float dragForJointType = PlayerJointHelpers.GetDragForJointType(PlayerJointHelpers.GetJointTypeForJointName(gameObject.name));
		rigidbody.drag = dragForJointType;
		if (gameObject.name.Contains("KNEE") || gameObject.name.Contains("SHOULDER"))
		{
			rigidbody.mass = 10f;
		}
		else if (gameObject.name.Contains("ANKLE") || gameObject.name.Contains("ELBOW"))
		{
			rigidbody.mass = 5f;
		}
		else if (gameObject.name.Contains("WRIST"))
		{
			rigidbody.mass = 2.5f;
			if (this.handLeft != null && this.handRight != null)
			{
				if (gameObject.name.Contains("LEFT"))
				{
					this.handLeft.bodypartRigidbody = rigidbody;
				}
				else
				{
					this.handRight.bodypartRigidbody = rigidbody;
				}
			}
		}
		WeaponDamageableBodyPart weaponDamageableBodyPart = gameObject.GetComponent<WeaponDamageableBodyPart>();
		if (weaponDamageableBodyPart == null && !gameObject.name.Contains("WRIST"))
		{
			weaponDamageableBodyPart = gameObject.AddComponent<WeaponDamageableBodyPart>();
		}
		this.FillWeaponDamageableBodyPart(weaponDamageableBodyPart);
		this.TryAddBluntDamageDealerGameObjectScript(gameObject);
	}

	// Token: 0x06000CB8 RID: 3256 RVA: 0x0003E9C4 File Offset: 0x0003CBC4
	private void FillWeaponDamageableBodyPart(WeaponDamageableBodyPart part)
	{
		if (part == null)
		{
			return;
		}
		JointType bodyPart = JointType.HIP;
		string name = part.gameObject.name;
		foreach (object obj in Enum.GetValues(typeof(JointType)))
		{
			JointType jointType = (JointType)obj;
			if (name == jointType.ToString())
			{
				bodyPart = jointType;
				break;
			}
		}
		part.childWeaponDamageableParts = new List<WeaponDamageablePart>();
		part.bodyPart = bodyPart;
		part.player = this.playerHealth;
		foreach (object obj2 in part.gameObject.transform)
		{
			WeaponDamageablePart[] components = ((Transform)obj2).gameObject.GetComponents<WeaponDamageablePart>();
			if (components.Length != 0)
			{
				part.childWeaponDamageableParts.AddRange(components);
			}
		}
		this.playerHealth.weaponDamageableBodyParts[(int)part.bodyPart] = part;
	}

	// Token: 0x06000CB9 RID: 3257 RVA: 0x0003EAEC File Offset: 0x0003CCEC
	private void GenerateJoints(GameObject gameObject)
	{
		try
		{
			using (IEnumerator enumerator = gameObject.transform.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Transform child = (Transform)enumerator.Current;
					if (!(child.name == "PlayerModelAnimation") && (!(gameObject.name == "ActualBoxer") || !(child.name != "Armature")) && !child.name.Replace("_", "").Any(new Func<char, bool>(char.IsLower)) && child.gameObject.activeInHierarchy)
					{
						ConfigurableJoint configurableJoint = (from x in gameObject.GetComponents<ConfigurableJoint>().ToList<ConfigurableJoint>()
						where x.connectedBody.gameObject.name == child.name
						select x).FirstOrDefault<ConfigurableJoint>();
						if (configurableJoint == null)
						{
							configurableJoint = gameObject.AddComponent<ConfigurableJoint>();
							configurableJoint.connectedBody = child.GetComponent<Rigidbody>();
							configurableJoint.autoConfigureConnectedAnchor = true;
							configurableJoint.anchor = child.localPosition;
						}
						ConfigurableJointScript configurableJointScript = (from x in gameObject.GetComponents<ConfigurableJointScript>().ToList<ConfigurableJointScript>()
						where x.target.name == child.name
						select x).FirstOrDefault<ConfigurableJointScript>();
						if (configurableJointScript == null)
						{
							configurableJointScript = gameObject.AddComponent<ConfigurableJointScript>();
							configurableJointScript.hj = configurableJoint;
							configurableJointScript.target = Generic.FindChildObject(this.animationObject.transform, child.name, null).transform;
							configurableJointScript.firstRotation = "x";
							configurableJointScript.secondRotation = "y";
							configurableJointScript.thirdRotation = "z";
							if (child.name.Contains("ELBOW"))
							{
								configurableJointScript.firstRotation = "y";
								configurableJointScript.secondRotation = "x";
							}
						}
						configurableJoint.xMotion = ConfigurableJointMotion.Locked;
						configurableJoint.zMotion = ConfigurableJointMotion.Locked;
						configurableJoint.yMotion = ConfigurableJointMotion.Locked;
						if (gameObject.name == "HIP" && !child.name.Contains("SPINE"))
						{
							configurableJoint.axis = new Vector3(1f, 0f, 0f);
							configurableJoint.secondaryAxis = new Vector3(0f, 1f, 0f);
							if (child.name.Contains("HIP_JOINT"))
							{
								configurableJoint.angularYMotion = ConfigurableJointMotion.Limited;
								configurableJoint.angularZMotion = ConfigurableJointMotion.Limited;
								configurableJoint.angularXMotion = ConfigurableJointMotion.Limited;
								configurableJoint.lowAngularXLimit = new SoftJointLimit
								{
									bounciness = 0f,
									contactDistance = 0f,
									limit = -120f
								};
								configurableJoint.highAngularXLimit = new SoftJointLimit
								{
									bounciness = 0f,
									contactDistance = 0f,
									limit = 120f
								};
								configurableJoint.angularZLimit = new SoftJointLimit
								{
									bounciness = 0f,
									contactDistance = 0f,
									limit = 120f
								};
								configurableJoint.angularYLimit = new SoftJointLimit
								{
									bounciness = 0f,
									contactDistance = 0f,
									limit = 120f
								};
							}
						}
						else if (child.name.Contains("SPINE") || child.name.Contains("NECK"))
						{
							configurableJoint.angularYMotion = ConfigurableJointMotion.Limited;
							configurableJoint.angularZMotion = ConfigurableJointMotion.Limited;
							configurableJoint.angularXMotion = ConfigurableJointMotion.Limited;
							configurableJoint.lowAngularXLimit = new SoftJointLimit
							{
								bounciness = 0f,
								contactDistance = 0f,
								limit = -20f
							};
							configurableJoint.highAngularXLimit = new SoftJointLimit
							{
								bounciness = 0f,
								contactDistance = 0f,
								limit = 45f
							};
							configurableJoint.angularYLimit = new SoftJointLimit
							{
								bounciness = 0f,
								contactDistance = 0f,
								limit = 45f
							};
							configurableJoint.angularZLimit = new SoftJointLimit
							{
								bounciness = 0f,
								contactDistance = 0f,
								limit = 45f
							};
						}
						else if (child.name.Contains("WRIST"))
						{
							configurableJoint.angularYMotion = ConfigurableJointMotion.Limited;
							configurableJoint.angularZMotion = ConfigurableJointMotion.Limited;
							configurableJoint.angularXMotion = ConfigurableJointMotion.Limited;
							configurableJoint.lowAngularXLimit = new SoftJointLimit
							{
								bounciness = 0f,
								contactDistance = 0f,
								limit = -180f
							};
							configurableJoint.highAngularXLimit = new SoftJointLimit
							{
								bounciness = 0f,
								contactDistance = 0f,
								limit = 180f
							};
							configurableJoint.angularYLimit = new SoftJointLimit
							{
								bounciness = 0f,
								contactDistance = 0f,
								limit = 120f
							};
							configurableJoint.angularZLimit = new SoftJointLimit
							{
								bounciness = 0f,
								contactDistance = 0f,
								limit = 120f
							};
						}
						else if (child.name.Contains("KNEE") || child.name.Contains("ELBOW"))
						{
							configurableJoint.angularYMotion = ConfigurableJointMotion.Locked;
							configurableJoint.angularZMotion = ConfigurableJointMotion.Locked;
							configurableJoint.angularXMotion = ConfigurableJointMotion.Limited;
							configurableJoint.highAngularXLimit = new SoftJointLimit
							{
								bounciness = 0f,
								contactDistance = 0f,
								limit = 160f
							};
							if (child.name.Contains("ELBOW"))
							{
								configurableJoint.axis = new Vector3(0f, 1f, 0f);
								if (child.name.Contains("RIGHT"))
								{
									configurableJoint.lowAngularXLimit = new SoftJointLimit
									{
										bounciness = 0f,
										contactDistance = 0f,
										limit = -160f
									};
									configurableJoint.highAngularXLimit = new SoftJointLimit
									{
										bounciness = 0f,
										contactDistance = 0f,
										limit = 0f
									};
								}
							}
						}
						List<float> strengthPercentsForDamageInstances = new List<float>
						{
							10f,
							5f,
							2.5f,
							1f
						};
						if (child.name.Contains("HIP_JOINT"))
						{
							strengthPercentsForDamageInstances = new List<float>
							{
								18f,
								13.5f,
								10f,
								1f
							};
						}
						else if (child.name.Contains("KNEE"))
						{
							strengthPercentsForDamageInstances = new List<float>
							{
								9f,
								4.5f,
								2.25f,
								1f
							};
						}
						else if (child.name.Contains("SPINE1"))
						{
							strengthPercentsForDamageInstances = new List<float>
							{
								20f,
								10f,
								5f,
								1f
							};
						}
						else if (child.name.Contains("SPINE"))
						{
							strengthPercentsForDamageInstances = new List<float>
							{
								20f,
								10f,
								5f,
								1f
							};
						}
						else if (!child.name.Contains("NECK") && !child.name.Contains("SHOULDER") && !child.name.Contains("ELBOW"))
						{
							child.name.Contains("WRIST");
						}
						JointType jointTypeForJointName = PlayerJointHelpers.GetJointTypeForJointName(child.name);
						float maxJointSpringForJointType = PlayerJointHelpers.GetMaxJointSpringForJointType(jointTypeForJointName, false);
						float maxForceForJointType = PlayerJointHelpers.GetMaxForceForJointType(jointTypeForJointName, false);
						float damperMultiplierForJointType = PlayerJointHelpers.GetDamperMultiplierForJointType(jointTypeForJointName, false);
						float positionDamper = maxJointSpringForJointType * damperMultiplierForJointType;
						configurableJoint.angularXDrive = new JointDrive
						{
							positionSpring = maxJointSpringForJointType,
							positionDamper = positionDamper,
							maximumForce = maxForceForJointType
						};
						configurableJoint.angularYZDrive = new JointDrive
						{
							positionSpring = maxJointSpringForJointType,
							positionDamper = positionDamper,
							maximumForce = maxForceForJointType
						};
						JointStrength jointStrength = gameObject.AddComponent<JointStrength>();
						jointStrength.joint = configurableJoint;
						jointStrength.jointName = child.name;
						jointStrength.strengthPercentsForDamageInstances = strengthPercentsForDamageInstances;
						configurableJointScript.jointStrength = jointStrength;
					}
				}
			}
		}
		catch (Exception message)
		{
			Debug.Log(gameObject.name);
			Debug.LogError(message);
		}
	}

	// Token: 0x06000CBA RID: 3258 RVA: 0x0000777A File Offset: 0x0000597A
	public void FinishUpRagdoll()
	{
	}

	// Token: 0x06000CBB RID: 3259 RVA: 0x0003F474 File Offset: 0x0003D674
	private void StartGenerateCuttableGameObjects(GameObject gameObject)
	{
		this.tempCuttableGameObjects = new List<CuttableGameObject>();
		this.GenerateCuttableGameObjectTree(gameObject);
		this.PopulateIgnoreLists();
		this.PopulateCuttableParentsAndChildren();
		this.PopulateCuttableSections();
		this.PopulateCuttableColliders();
		this.CuttableSectionInstantKills();
		this.GenerateParentCuttableSections();
		this.PopulateParentCuttableColliders();
		this.CuttableSectionConfigurableJoints();
		this.LinkCuttableMeshToCuttableSection();
	}

	// Token: 0x06000CBC RID: 3260 RVA: 0x0003F4CC File Offset: 0x0003D6CC
	private void LinkCuttableMeshToCuttableSection()
	{
		foreach (CuttableGameObject cuttableGameObject in this.tempCuttableGameObjects)
		{
			foreach (CuttableMesh cuttableMesh in cuttableGameObject.cuttableMeshList)
			{
				cuttableMesh.SetCuttableSectionIndex(cuttableGameObject.cuttableSections);
			}
		}
	}

	// Token: 0x06000CBD RID: 3261 RVA: 0x0003F560 File Offset: 0x0003D760
	private void CuttableSectionConfigurableJoints()
	{
		foreach (CuttableGameObject cuttableGameObject in this.tempCuttableGameObjects)
		{
			foreach (CuttableSection cuttableSection in cuttableGameObject.cuttableSections)
			{
				cuttableSection.configurableJointScripts = new List<ConfigurableJointScript>();
				if (cuttableSection.artery != null && cuttableSection.joint != null)
				{
					cuttableSection.configurableJointScripts = cuttableSection.joint.connectedBody.gameObject.GetComponentsInChildren<ConfigurableJointScript>().ToList<ConfigurableJointScript>();
				}
			}
		}
	}

	// Token: 0x06000CBE RID: 3262 RVA: 0x0003F630 File Offset: 0x0003D830
	private void PopulateParentCuttableColliders()
	{
		foreach (CuttableGameObject cuttableGameObject in this.tempCuttableGameObjects)
		{
			if (cuttableGameObject.parentCuttableGameObject != null)
			{
				List<CuttableCollider> list = cuttableGameObject.cuttableColliders.ToList<CuttableCollider>();
				foreach (CuttableCollider cuttableCollider in cuttableGameObject.parentCuttableGameObject.cuttableColliders)
				{
					if (!cuttableCollider.parentCollider)
					{
						CuttableCollider item = cuttableCollider;
						item.parentCollider = true;
						list.Add(item);
					}
				}
				cuttableGameObject.cuttableColliders = list.ToArray();
			}
		}
	}

	// Token: 0x06000CBF RID: 3263 RVA: 0x0003F6E8 File Offset: 0x0003D8E8
	private void GenerateParentCuttableSections()
	{
		foreach (CuttableGameObject cuttableGameObject in this.tempCuttableGameObjects)
		{
			if (cuttableGameObject.parentCuttableGameObject != null)
			{
				CuttableSection item = new CuttableSection
				{
					parentSection = true,
					position = cuttableGameObject.transform.InverseTransformPoint(cuttableGameObject.parentCuttableGameObject.transform.position)
				};
				cuttableGameObject.cuttableSections.Insert(0, item);
			}
		}
	}

	// Token: 0x06000CC0 RID: 3264 RVA: 0x0003F780 File Offset: 0x0003D980
	private void CuttableSectionInstantKills()
	{
		JointType[] source = new JointType[]
		{
			JointType.SPINE1,
			JointType.SPINE2,
			JointType.NECK
		};
		foreach (CuttableGameObject cuttableGameObject in this.tempCuttableGameObjects)
		{
			foreach (CuttableSection cuttableSection in cuttableGameObject.cuttableSections)
			{
				if (cuttableSection.joint != null && cuttableSection.cuttableGameObject != null)
				{
					if (source.Contains(cuttableSection.cuttableGameObject.bodyPart))
					{
						cuttableSection.instantKill = true;
						cuttableSection.deathReason = DeathReason.Spine;
					}
				}
				else if (cuttableGameObject.bodyPart == JointType.NECK && !cuttableSection.parentSection)
				{
					cuttableSection.instantKill = true;
					cuttableSection.deathReason = DeathReason.Brain;
				}
			}
		}
	}

	// Token: 0x06000CC1 RID: 3265 RVA: 0x0003F888 File Offset: 0x0003DA88
	private void PopulateCuttableColliders()
	{
		foreach (CuttableGameObject cuttableGameObject in this.tempCuttableGameObjects)
		{
			List<CuttableCollider> list = new List<CuttableCollider>();
			if (cuttableGameObject.cuttableColliders != null)
			{
				list = cuttableGameObject.cuttableColliders.ToList<CuttableCollider>();
			}
			foreach (object obj in cuttableGameObject.transform.transform)
			{
				Transform transform = (Transform)obj;
				if (transform.name.ToLower().Contains("collider"))
				{
					foreach (CapsuleCollider capsuleCollider in transform.GetComponents<CapsuleCollider>())
					{
						if (capsuleCollider != null)
						{
							Vector3 center = capsuleCollider.center;
							float num = capsuleCollider.height / 2f - capsuleCollider.radius;
							Vector3 point = center;
							Vector3 point2 = center;
							if (capsuleCollider.direction == 0)
							{
								point.x += num;
								point2.x -= num;
							}
							else if (capsuleCollider.direction == 1)
							{
								point.y += num;
								point2.y -= num;
							}
							else if (capsuleCollider.direction == 2)
							{
								point.z += num;
								point2.z -= num;
							}
							Vector3 point3 = transform.transform.localToWorldMatrix.MultiplyPoint3x4(point);
							Vector3 point4 = transform.transform.localToWorldMatrix.MultiplyPoint3x4(point2);
							Vector3 p = cuttableGameObject.transform.worldToLocalMatrix.MultiplyPoint3x4(point3);
							Vector3 p2 = cuttableGameObject.transform.worldToLocalMatrix.MultiplyPoint3x4(point4);
							CuttableCollider cuttableCollider = new CuttableCollider
							{
								p0 = p,
								p1 = p2,
								radius = capsuleCollider.radius,
								colliderType = ColliderType.Capsule
							};
							CuttableCollider item = cuttableCollider;
							list.Add(item);
						}
					}
					foreach (SphereCollider sphereCollider in transform.GetComponents<SphereCollider>())
					{
						Vector3 center2 = sphereCollider.center;
						Vector3 point5 = transform.transform.localToWorldMatrix.MultiplyPoint3x4(center2);
						Vector3 vector = cuttableGameObject.transform.worldToLocalMatrix.MultiplyPoint3x4(point5);
						CuttableCollider cuttableCollider = new CuttableCollider
						{
							p0 = vector,
							p1 = vector,
							radius = sphereCollider.radius,
							colliderType = ColliderType.Sphere
						};
						CuttableCollider item2 = cuttableCollider;
						if (!(cuttableGameObject.gameObject.name == "NECK") || vector.magnitude <= 1E-05f)
						{
							list.Add(item2);
							if (vector.magnitude < 1E-05f && cuttableGameObject.parentCuttableGameObject != null)
							{
								Vector3 vector2 = cuttableGameObject.parentCuttableGameObject.transform.worldToLocalMatrix.MultiplyPoint3x4(point5);
								cuttableCollider = new CuttableCollider
								{
									p0 = vector2,
									p1 = vector2,
									radius = sphereCollider.radius,
									colliderType = ColliderType.Sphere
								};
								CuttableCollider item3 = cuttableCollider;
								List<CuttableCollider> list2 = cuttableGameObject.parentCuttableGameObject.cuttableColliders.ToList<CuttableCollider>();
								list2.Add(item3);
								cuttableGameObject.parentCuttableGameObject.cuttableColliders = list2.ToArray();
							}
						}
					}
				}
			}
			if (cuttableGameObject.bodyPart == JointType.SPINE2)
			{
				foreach (CuttableSection cuttableSection in cuttableGameObject.cuttableSections)
				{
					if (cuttableSection.cuttableGameObject != null && cuttableSection.cuttableGameObject.bodyPart == JointType.NECK)
					{
						CuttableCollider cuttableCollider = new CuttableCollider
						{
							p0 = default(Vector3),
							p1 = cuttableSection.cuttableGameObject.gameObject.transform.localPosition,
							radius = 0.025f,
							colliderType = ColliderType.Capsule
						};
						CuttableCollider item4 = cuttableCollider;
						list.Add(item4);
					}
				}
			}
			cuttableGameObject.cuttableColliders = list.ToArray();
			foreach (CuttableMesh cuttableMesh in cuttableGameObject.cuttableMeshList)
			{
				cuttableMesh.ignoreInCheck = true;
			}
		}
	}

	// Token: 0x06000CC2 RID: 3266 RVA: 0x0003FD64 File Offset: 0x0003DF64
	private void PopulateCuttableSections()
	{
		foreach (CuttableGameObject cuttableGameObject in this.tempCuttableGameObjects)
		{
			foreach (object obj in cuttableGameObject.transform.transform)
			{
				Transform transform = (Transform)obj;
				if (transform.name == "CuttableSections")
				{
					foreach (object obj2 in transform.transform.transform)
					{
						Transform gameObjectTransform = (Transform)obj2;
						CuttableSection cuttableSection = new CuttableSection
						{
							cuttableGameObject = null,
							joint = null,
							hand = null,
							isEquipment = false,
							gameObjectTransform = gameObjectTransform
						};
						if (cuttableGameObject.gameObject.name.Contains("KNEE"))
						{
							List<WeaponDamageablePart> list = cuttableGameObject.gameObject.transform.parent.GetComponentsInChildren<WeaponDamageablePart>().ToList<WeaponDamageablePart>();
							list = (from x in list
							where x.bloodVessel
							select x).ToList<WeaponDamageablePart>();
							if (list.Count == 1)
							{
								cuttableSection.artery = list[0];
							}
						}
						cuttableGameObject.cuttableSections.Add(cuttableSection);
					}
				}
			}
		}
		this.ReorderCuttableSections();
	}

	// Token: 0x06000CC3 RID: 3267 RVA: 0x0003FF4C File Offset: 0x0003E14C
	private void ReorderCuttableSections()
	{
		foreach (CuttableGameObject cuttableGameObject in this.tempCuttableGameObjects)
		{
			for (int i = 0; i < cuttableGameObject.cuttableSections.Count; i++)
			{
				CuttableSection cuttableSection = cuttableGameObject.cuttableSections[i];
				if (cuttableSection.joint != null && cuttableSection.joint.connectedBody.gameObject.name == "NECK")
				{
					cuttableGameObject.cuttableSections.Remove(cuttableSection);
					cuttableGameObject.cuttableSections.Insert(0, cuttableSection);
					break;
				}
			}
		}
	}

	// Token: 0x06000CC4 RID: 3268 RVA: 0x00040008 File Offset: 0x0003E208
	private void PopulateCuttableParentsAndChildren()
	{
		foreach (CuttableGameObject cuttableGameObject in this.tempCuttableGameObjects)
		{
			cuttableGameObject.parentCuttableGameObject = cuttableGameObject.transform.parent.GetComponent<CuttableGameObject>();
			foreach (CuttableSection cuttableSection in cuttableGameObject.cuttableSections)
			{
				if (cuttableSection.joint != null)
				{
					cuttableSection.cuttableGameObject = cuttableSection.joint.connectedBody.gameObject.GetComponent<CuttableGameObject>();
				}
			}
		}
	}

	// Token: 0x06000CC5 RID: 3269 RVA: 0x000400CC File Offset: 0x0003E2CC
	private void PopulateIgnoreLists()
	{
		foreach (CuttableGameObject cuttableGameObject in this.tempCuttableGameObjects)
		{
			if (cuttableGameObject.gameObject.name == "HIP")
			{
				cuttableGameObject.cuttableGameObjectsToIgnoreCollisions.Add(this.FindCuttableGameObjectByName("SPINE2"));
			}
			else if (cuttableGameObject.gameObject.name == "SPINE1")
			{
				cuttableGameObject.cuttableGameObjectsToIgnoreCollisions.Add(this.FindCuttableGameObjectByName("HIP_JOINT_LEFT"));
				cuttableGameObject.cuttableGameObjectsToIgnoreCollisions.Add(this.FindCuttableGameObjectByName("HIP_JOINT_RIGHT"));
				cuttableGameObject.cuttableGameObjectsToIgnoreCollisions.Add(this.FindCuttableGameObjectByName("SCAPULA_LEFT"));
				cuttableGameObject.cuttableGameObjectsToIgnoreCollisions.Add(this.FindCuttableGameObjectByName("SCAPULA_RIGHT"));
			}
			else if (cuttableGameObject.gameObject.name == "SPINE2")
			{
				cuttableGameObject.cuttableGameObjectsToIgnoreCollisions.Add(this.FindCuttableGameObjectByName("SHOULDER_LEFT"));
				cuttableGameObject.cuttableGameObjectsToIgnoreCollisions.Add(this.FindCuttableGameObjectByName("SHOULDER_RIGHT"));
			}
			else if (cuttableGameObject.gameObject.name == "HIP_JOINT_LEFT")
			{
				cuttableGameObject.cuttableGameObjectsToIgnoreCollisions.Add(this.FindCuttableGameObjectByName("HIP_JOINT_RIGHT"));
				cuttableGameObject.cuttableGameObjectsToIgnoreCollisions.Add(this.FindCuttableGameObjectByName("SPINE1"));
			}
			else if (cuttableGameObject.gameObject.name == "HIP_JOINT_RIGHT")
			{
				cuttableGameObject.cuttableGameObjectsToIgnoreCollisions.Add(this.FindCuttableGameObjectByName("HIP_JOINT_LEFT"));
				cuttableGameObject.cuttableGameObjectsToIgnoreCollisions.Add(this.FindCuttableGameObjectByName("SPINE1"));
			}
			else if (cuttableGameObject.gameObject.name == "SCAPULA_RIGHT")
			{
				cuttableGameObject.cuttableGameObjectsToIgnoreCollisions.Add(this.FindCuttableGameObjectByName("SCAPULA_LEFT"));
			}
			else if (cuttableGameObject.gameObject.name == "SCAPULA_LEFT")
			{
				cuttableGameObject.cuttableGameObjectsToIgnoreCollisions.Add(this.FindCuttableGameObjectByName("SCAPULA_RIGHT"));
			}
			else if (cuttableGameObject.gameObject.name == "SHOULDER_RIGHT")
			{
				cuttableGameObject.cuttableGameObjectsToIgnoreCollisions.Add(this.FindCuttableGameObjectByName("SPINE2"));
			}
			else if (cuttableGameObject.gameObject.name == "SHOULDER_LEFT")
			{
				cuttableGameObject.cuttableGameObjectsToIgnoreCollisions.Add(this.FindCuttableGameObjectByName("SPINE2"));
			}
		}
	}

	// Token: 0x06000CC6 RID: 3270 RVA: 0x00040368 File Offset: 0x0003E568
	private CuttableGameObject FindCuttableGameObjectByName(string gameObjectName)
	{
		foreach (CuttableGameObject cuttableGameObject in this.tempCuttableGameObjects)
		{
			if (cuttableGameObject.gameObject.name == gameObjectName)
			{
				return cuttableGameObject;
			}
		}
		throw new Exception(gameObjectName + " cuttabe NOT FOUND");
	}

	// Token: 0x06000CC7 RID: 3271 RVA: 0x000403E0 File Offset: 0x0003E5E0
	private void GenerateCuttableGameObjectTree(GameObject gameObject)
	{
		foreach (object obj in gameObject.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.name == "PlayerModelAnimation")
			{
				this.animationObject = transform.gameObject;
			}
			else if (!(gameObject.name == "ActualBoxer") || !(transform.name != "Armature"))
			{
				if (!transform.name.Replace("_", "").Any(new Func<char, bool>(char.IsLower)))
				{
					this.AddCuttableGameObject(transform.gameObject);
				}
				this.GenerateCuttableGameObjectTree(transform.gameObject);
			}
		}
	}

	// Token: 0x06000CC8 RID: 3272 RVA: 0x000404BC File Offset: 0x0003E6BC
	private void AddCuttableGameObject(GameObject gameObject)
	{
		if (gameObject.name.Contains("WRIST"))
		{
			return;
		}
		WeaponDamageableBodyPart component = gameObject.GetComponent<WeaponDamageableBodyPart>();
		CuttableGameObject component2 = gameObject.transform.parent.GetComponent<CuttableGameObject>();
		component.cuttableGameObjects = new List<CuttableGameObject>();
		CuttableGameObject cuttableGameObject = gameObject.AddComponent<CuttableGameObject>();
		cuttableGameObject.playerHealth = this.playerHealth;
		cuttableGameObject.bodyPart = (JointType)Enum.Parse(typeof(JointType), gameObject.name, true);
		this.tempCuttableGameObjects.Add(cuttableGameObject);
		cuttableGameObject.cuttableMeshList = new List<CuttableMesh>();
		cuttableGameObject.objectsToDisable = new List<GameObject>();
		cuttableGameObject.cuttableGameObjectsToIgnoreCollisions = new List<CuttableGameObject>();
		cuttableGameObject.localCollidersForOthersToIgnore = new List<Collider>();
		cuttableGameObject.localCollidersToIgnoreWhenChildOfCutSection = new List<Collider>();
		component.cuttableGameObjects.Add(cuttableGameObject);
		if (component2 != null)
		{
			component.cuttableGameObjects.Add(component2);
		}
		cuttableGameObject.cuttableRigidbody = gameObject.GetComponent<Rigidbody>();
		cuttableGameObject.cuttableSections = new List<CuttableSection>();
		ConfigurableJoint[] components = gameObject.GetComponents<ConfigurableJoint>();
		for (int i = 0; i < components.Length; i++)
		{
			if (!(components[i].connectedBody.gameObject.name == "HipSphere"))
			{
				CuttableSection cuttableSection = new CuttableSection();
				cuttableSection.joint = components[i];
				Hand[] componentsInChildren = components[i].connectedBody.GetComponentsInChildren<Hand>();
				if (componentsInChildren.Length == 1)
				{
					cuttableSection.hand = componentsInChildren[0];
				}
				List<WeaponDamageablePart> list = components[i].connectedBody.GetComponentsInChildren<WeaponDamageablePart>().ToList<WeaponDamageablePart>();
				list = (from x in list
				where x.bloodVessel
				select x).ToList<WeaponDamageablePart>();
				if (list.Count == 0)
				{
					if (gameObject.name.Contains("ELBOW") || gameObject.name.Contains("KNEE"))
					{
						list = gameObject.transform.parent.GetComponentsInChildren<WeaponDamageablePart>().ToList<WeaponDamageablePart>();
						list = (from x in list
						where x.bloodVessel
						select x).ToList<WeaponDamageablePart>();
					}
					else if (gameObject.name.Contains("SHOULDER") || gameObject.name.Contains("HIP_JOINT"))
					{
						list = gameObject.GetComponentsInChildren<WeaponDamageablePart>().ToList<WeaponDamageablePart>();
						list = (from x in list
						where x.bloodVessel
						select x).ToList<WeaponDamageablePart>();
					}
				}
				if (list.Count == 1)
				{
					cuttableSection.artery = list[0];
				}
				cuttableGameObject.cuttableSections.Add(cuttableSection);
			}
		}
		foreach (object obj in gameObject.transform.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.gameObject.activeInHierarchy)
			{
				if (transform.name.ToLower().Contains("mesh") || transform.name.ToLower().Contains("ball"))
				{
					CuttableMesh cuttableMesh = new CuttableMesh();
					cuttableMesh.meshFilter = transform.GetComponent<MeshFilter>();
					cuttableMesh.renderer = transform.GetComponent<MeshRenderer>();
					if (this.meshesToBeIgnoredInCutCheck.Contains(transform.name))
					{
						cuttableMesh.ignoreInCheck = true;
					}
					if (cuttableMesh.meshFilter != null && cuttableMesh.renderer != null)
					{
						cuttableGameObject.cuttableMeshList.Add(cuttableMesh);
						if (transform.name.ToLower().Contains("ball") && !transform.name.ToLower().Contains("foot"))
						{
							CuttableMesh cuttableMesh2 = new CuttableMesh();
							cuttableMesh2.meshFilter = cuttableMesh.meshFilter;
							cuttableMesh2.renderer = cuttableMesh.renderer;
							cuttableMesh2.ignoreInCheck = cuttableMesh.ignoreInCheck;
							if (component2 != null)
							{
								component2.cuttableMeshList.Add(cuttableMesh2);
							}
						}
					}
				}
				if (transform.name.ToLower().Contains("collider"))
				{
					cuttableGameObject.objectsToDisable.Add(transform.gameObject);
					if (this.cuttablesWithIgnoreColliders.Contains(gameObject.name))
					{
						Collider component3 = transform.GetComponent<Collider>();
						cuttableGameObject.localCollidersForOthersToIgnore.Add(component3);
					}
					SphereCollider component4 = transform.GetComponent<SphereCollider>();
					if (component4 != null)
					{
						cuttableGameObject.localCollidersToIgnoreWhenChildOfCutSection.Add(component4);
					}
				}
			}
		}
	}

	// Token: 0x06000CC9 RID: 3273 RVA: 0x00040964 File Offset: 0x0003EB64
	public void FillBluntDamageDealerScripts()
	{
		foreach (BluntDamageDealerGameObject bluntDamageDealerGameObject in UnityEngine.Object.FindObjectsOfType<BluntDamageDealerGameObject>(true).ToList<BluntDamageDealerGameObject>())
		{
			this.FillBluntDamageCenterOfMassLine(bluntDamageDealerGameObject, bluntDamageDealerGameObject.bodyPart);
		}
	}

	// Token: 0x06000CCA RID: 3274 RVA: 0x000409C4 File Offset: 0x0003EBC4
	public void TryAddBluntDamageDealerGameObjectScript(GameObject gameObject)
	{
		JointType jointType = JointType.HIP;
		Enum.TryParse<JointType>(gameObject.name, out jointType);
		if (new JointType[]
		{
			JointType.ELBOW_LEFT,
			JointType.ELBOW_RIGHT,
			JointType.KNEE_LEFT,
			JointType.KNEE_RIGHT,
			JointType.WRIST_LEFT,
			JointType.WRIST_RIGHT,
			JointType.NECK
		}.Contains(jointType))
		{
			this.AddBluntDamageDealerGameObjectScript(gameObject, jointType);
		}
	}

	// Token: 0x06000CCB RID: 3275 RVA: 0x00040A04 File Offset: 0x0003EC04
	public void AddBluntDamageDealerGameObjectScript(GameObject gameObject, JointType bodyPart)
	{
		BluntDamageDealerGameObject bluntDamageDealerGameObject = gameObject.AddComponent<BluntDamageDealerGameObject>();
		bluntDamageDealerGameObject.bodyPart = bodyPart;
		bluntDamageDealerGameObject.bluntDamageDealer = new BluntDamageDealer();
		bluntDamageDealerGameObject.rb = gameObject.GetComponent<Rigidbody>();
		bluntDamageDealerGameObject.playerHealth = this.playerHealth;
		bluntDamageDealerGameObject.bluntDamageDealer.bluntDamageType = BluntDamageType.BodyPart;
		if (bodyPart == JointType.WRIST_LEFT || bodyPart == JointType.WRIST_RIGHT)
		{
			bluntDamageDealerGameObject.bluntDamageDealer.overrideMass = true;
			bluntDamageDealerGameObject.bluntDamageDealer.overrideMassToUse = 5f;
			return;
		}
		if (bodyPart == JointType.KNEE_LEFT || bodyPart == JointType.KNEE_RIGHT)
		{
			bluntDamageDealerGameObject.bluntDamageDealer.bluntDamageType = BluntDamageType.BodyPartLeg;
		}
	}

	// Token: 0x06000CCC RID: 3276 RVA: 0x00040A8C File Offset: 0x0003EC8C
	public void FillBluntDamageCenterOfMassLine(BluntDamageDealerGameObject bluntDamageDealerGameObject, JointType bodyPart)
	{
		bluntDamageDealerGameObject.bluntDamageDealer.centerOfMassLineLocalPoints = new List<Vector3>();
		if (bodyPart == JointType.ELBOW_LEFT || bodyPart == JointType.ELBOW_RIGHT)
		{
			bluntDamageDealerGameObject.bluntDamageDealer.centerOfMassLineLocalPoints.Add(default(Vector3));
			ConfigurableJoint component = bluntDamageDealerGameObject.gameObject.GetComponent<ConfigurableJoint>();
			bluntDamageDealerGameObject.bluntDamageDealer.centerOfMassLineLocalPoints.Add(component.connectedBody.transform.localPosition);
			return;
		}
		if (bodyPart == JointType.KNEE_LEFT || bodyPart == JointType.KNEE_RIGHT)
		{
			bluntDamageDealerGameObject.bluntDamageDealer.centerOfMassLineLocalPoints.Add(default(Vector3));
			Transform transform = bluntDamageDealerGameObject.gameObject.transform.Find("FootBallCollider");
			bluntDamageDealerGameObject.bluntDamageDealer.centerOfMassLineLocalPoints.Add(transform.localPosition);
		}
	}

	// Token: 0x04000918 RID: 2328
	private GameObject mainObject;

	// Token: 0x04000919 RID: 2329
	private GameObject physicalObject;

	// Token: 0x0400091A RID: 2330
	private GameObject animationObject;

	// Token: 0x0400091B RID: 2331
	private PlayerAnimator playerAnimator;

	// Token: 0x0400091C RID: 2332
	private PlayerHealth playerHealth;

	// Token: 0x0400091D RID: 2333
	public int count;

	// Token: 0x0400091E RID: 2334
	public GameObject hipSphere;

	// Token: 0x0400091F RID: 2335
	public ConfigurableJoint balanceBall;

	// Token: 0x04000920 RID: 2336
	public ConfigurableJoint rollerBladeLeft;

	// Token: 0x04000921 RID: 2337
	public ConfigurableJoint rollerBladeRight;

	// Token: 0x04000922 RID: 2338
	public Hand handRight;

	// Token: 0x04000923 RID: 2339
	public Hand handLeft;

	// Token: 0x04000924 RID: 2340
	public float jointDampenerModifier = 0.09f;

	// Token: 0x04000925 RID: 2341
	public float jointDampenerModifierWrists = 0.05f;

	// Token: 0x04000926 RID: 2342
	public GameObject animationMeshRoot;

	// Token: 0x04000927 RID: 2343
	private Paintable parentPaintable;

	// Token: 0x04000928 RID: 2344
	private List<CuttableGameObject> tempCuttableGameObjects;

	// Token: 0x04000929 RID: 2345
	private string[] cuttablesWithIgnoreColliders = new string[]
	{
		"SPINE1",
		"SPINE2",
		"HIP_JOINT_LEFT",
		"HIP_JOINT_RIGHT",
		"SCAPULA_LEFT",
		"SCAPULA_RIGHT",
		"SHOULDER_LEFT",
		"SHOULDER_RIGHT"
	};

	// Token: 0x0400092A RID: 2346
	private string[] meshesToBeIgnoredInCutCheck = new string[]
	{
		"MeshHip",
		"MeshStomach",
		"MeshChest"
	};
}
