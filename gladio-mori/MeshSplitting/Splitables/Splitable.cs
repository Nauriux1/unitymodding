using System;
using MeshSplitting.MeshTools;
using MeshSplitting.SplitterMath;
using UnityEngine;

namespace MeshSplitting.Splitables
{
	// Token: 0x020002DC RID: 732
	[AddComponentMenu("Mesh Splitting/Splitable")]
	public class Splitable : MonoBehaviour, ISplitable
	{
		// Token: 0x0600164A RID: 5706 RVA: 0x0006E0CA File Offset: 0x0006C2CA
		private void Awake()
		{
			this._transform = base.GetComponent<Transform>();
		}

		// Token: 0x0600164B RID: 5707 RVA: 0x0006E0D8 File Offset: 0x0006C2D8
		private void Update()
		{
			if (this._splitMesh)
			{
				this._splitMesh = false;
				bool flag = false;
				for (int i = 0; i < this._meshContainerStatic.Length; i++)
				{
					this._meshContainerStatic[i].MeshInitialize();
					this._meshContainerStatic[i].CalculateWorldSpace();
					this._meshSplitterStatic[i].MeshSplit();
					if (this._meshContainerStatic[i].IsMeshSplit())
					{
						flag = true;
						if (this.CreateCap)
						{
							this._meshSplitterStatic[i].MeshCreateCaps();
						}
					}
				}
				for (int j = 0; j < this._meshContainerSkinned.Length; j++)
				{
					this._meshContainerSkinned[j].MeshInitialize();
					this._meshContainerSkinned[j].CalculateWorldSpace();
					this._meshSplitterSkinned[j].MeshSplit();
					if (this._meshContainerSkinned[j].IsMeshSplit())
					{
						flag = true;
						if (this.CreateCap)
						{
							this._meshSplitterSkinned[j].MeshCreateCaps();
						}
					}
				}
				if (flag)
				{
					this.CreateNewObjects();
				}
				this._isSplitting = false;
			}
		}

		// Token: 0x0600164C RID: 5708 RVA: 0x0006E1CC File Offset: 0x0006C3CC
		public void Split(Transform splitTransform)
		{
			if (!this._isSplitting)
			{
				this._isSplitting = (this._splitMesh = true);
				this._splitPlane = new PlaneMath(splitTransform);
				MeshFilter[] componentsInChildren = base.GetComponentsInChildren<MeshFilter>();
				SkinnedMeshRenderer[] componentsInChildren2 = base.GetComponentsInChildren<SkinnedMeshRenderer>();
				this._meshContainerStatic = new MeshContainer[componentsInChildren.Length];
				this._meshSplitterStatic = new IMeshSplitter[componentsInChildren.Length];
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					this._meshContainerStatic[i] = new MeshContainer(componentsInChildren[i]);
					IMeshSplitter[] meshSplitterStatic = this._meshSplitterStatic;
					int num = i;
					IMeshSplitter meshSplitter2;
					if (!this.Convex)
					{
						IMeshSplitter meshSplitter = new MeshSplitterConcave(this._meshContainerStatic[i], this._splitPlane, splitTransform.rotation);
						meshSplitter2 = meshSplitter;
					}
					else
					{
						IMeshSplitter meshSplitter = new MeshSplitterConvex(this._meshContainerStatic[i], this._splitPlane, splitTransform.rotation);
						meshSplitter2 = meshSplitter;
					}
					meshSplitterStatic[num] = meshSplitter2;
					if (this.UseCapUV)
					{
						this._meshSplitterStatic[i].SetCapUV(this.UseCapUV, this.CustomUV, this.CapUVMin, this.CapUVMax);
					}
				}
				this._meshSplitterSkinned = new IMeshSplitter[componentsInChildren2.Length];
				this._meshContainerSkinned = new MeshContainer[componentsInChildren2.Length];
				for (int j = 0; j < componentsInChildren2.Length; j++)
				{
					this._meshContainerSkinned[j] = new MeshContainer(componentsInChildren2[j]);
					IMeshSplitter[] meshSplitterSkinned = this._meshSplitterSkinned;
					int num2 = j;
					IMeshSplitter meshSplitter3;
					if (!this.Convex)
					{
						IMeshSplitter meshSplitter = new MeshSplitterConcave(this._meshContainerSkinned[j], this._splitPlane, splitTransform.rotation);
						meshSplitter3 = meshSplitter;
					}
					else
					{
						IMeshSplitter meshSplitter = new MeshSplitterConvex(this._meshContainerSkinned[j], this._splitPlane, splitTransform.rotation);
						meshSplitter3 = meshSplitter;
					}
					meshSplitterSkinned[num2] = meshSplitter3;
					if (this.UseCapUV)
					{
						this._meshSplitterSkinned[j].SetCapUV(this.UseCapUV, this.CustomUV, this.CapUVMin, this.CapUVMax);
					}
				}
			}
		}

		// Token: 0x0600164D RID: 5709 RVA: 0x0006E38C File Offset: 0x0006C58C
		private void CreateNewObjects()
		{
			Transform transform = this._transform.parent;
			if (transform == null)
			{
				transform = new GameObject("Parent: " + base.gameObject.name).transform;
				transform.position = Vector3.zero;
				transform.rotation = Quaternion.identity;
				transform.localScale = Vector3.one;
			}
			Mesh meshOnGameObject = this.GetMeshOnGameObject(base.gameObject);
			Rigidbody rigidbody = null;
			float num = 100f;
			float num2 = 1f;
			if (meshOnGameObject != null)
			{
				rigidbody = base.GetComponent<Rigidbody>();
				if (rigidbody != null)
				{
					num = rigidbody.mass;
				}
				Vector3 size = meshOnGameObject.bounds.size;
				num2 = size.x * size.y * size.z;
			}
			GameObject[] array = new GameObject[2];
			if (this.OptionalTargetObject == null)
			{
				array[0] = UnityEngine.Object.Instantiate<GameObject>(base.gameObject);
				array[0].name = base.gameObject.name;
				array[1] = base.gameObject;
			}
			else
			{
				array[0] = UnityEngine.Object.Instantiate<GameObject>(this.OptionalTargetObject);
				array[1] = UnityEngine.Object.Instantiate<GameObject>(this.OptionalTargetObject);
			}
			Animation[] componentsInChildren = array[1].GetComponentsInChildren<Animation>();
			Animation[] componentsInChildren2 = array[0].GetComponentsInChildren<Animation>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				foreach (object obj in componentsInChildren[i])
				{
					AnimationState animationState = (AnimationState)obj;
					AnimationState animationState2 = componentsInChildren2[i][animationState.name];
					animationState2.enabled = animationState.enabled;
					animationState2.weight = animationState.weight;
					animationState2.time = animationState.time;
					animationState2.speed = animationState.speed;
					animationState2.layer = animationState.layer;
					animationState2.blendMode = animationState.blendMode;
				}
			}
			for (int j = 0; j < 2; j++)
			{
				this.UpdateMeshesInChildren(j, array[j]);
				array[j].GetComponent<Transform>().parent = transform;
				Mesh meshOnGameObject2 = this.GetMeshOnGameObject(array[j]);
				if (meshOnGameObject2 != null)
				{
					MeshCollider component = array[j].GetComponent<MeshCollider>();
					if (component != null)
					{
						component.sharedMesh = meshOnGameObject2;
						component.convex = this.Convex;
						if (component.convex && meshOnGameObject2.triangles.Length > 765)
						{
							component.convex = false;
						}
						component.convex = true;
					}
					Rigidbody component2 = array[j].GetComponent<Rigidbody>();
					if (rigidbody != null && component2 != null)
					{
						Vector3 size2 = meshOnGameObject2.bounds.size;
						float num3 = size2.x * size2.y * size2.z;
						float num4 = num * (num3 / num2);
						component2.useGravity = rigidbody.useGravity;
						component2.mass = num4;
						component2.velocity = rigidbody.velocity;
						component2.angularVelocity = rigidbody.angularVelocity;
						if (this.SplitForce > 0f)
						{
							component2.AddForce(this._splitPlane.Normal * num4 * ((j == 0) ? this.SplitForce : (-this.SplitForce)), ForceMode.Impulse);
						}
					}
				}
				this.PostProcessObject(array[j]);
			}
		}

		// Token: 0x0600164E RID: 5710 RVA: 0x0006E708 File Offset: 0x0006C908
		private void UpdateMeshesInChildren(int i, GameObject go)
		{
			if (this._meshContainerStatic.Length != 0)
			{
				MeshFilter[] componentsInChildren = go.GetComponentsInChildren<MeshFilter>();
				for (int j = 0; j < this._meshContainerStatic.Length; j++)
				{
					Renderer component = componentsInChildren[j].GetComponent<Renderer>();
					if (this.ForceNoBatching)
					{
						component.materials = component.materials;
					}
					if (i == 0)
					{
						if (this._meshContainerStatic[j].HasMeshUpper() & this._meshContainerStatic[j].HasMeshLower())
						{
							componentsInChildren[j].mesh = this._meshContainerStatic[j].CreateMeshUpper();
						}
						else if (!this._meshContainerStatic[j].HasMeshUpper())
						{
							if (component != null)
							{
								UnityEngine.Object.Destroy(component);
							}
							UnityEngine.Object.Destroy(componentsInChildren[j]);
						}
					}
					else if (this._meshContainerStatic[j].HasMeshUpper() & this._meshContainerStatic[j].HasMeshLower())
					{
						componentsInChildren[j].mesh = this._meshContainerStatic[j].CreateMeshLower();
					}
					else if (!this._meshContainerStatic[j].HasMeshLower())
					{
						if (component != null)
						{
							UnityEngine.Object.Destroy(component);
						}
						UnityEngine.Object.Destroy(componentsInChildren[j]);
					}
				}
			}
			if (this._meshContainerSkinned.Length != 0)
			{
				SkinnedMeshRenderer[] componentsInChildren2 = go.GetComponentsInChildren<SkinnedMeshRenderer>();
				for (int k = 0; k < this._meshContainerSkinned.Length; k++)
				{
					if (i == 0)
					{
						if (this._meshContainerSkinned[k].HasMeshUpper() & this._meshContainerSkinned[k].HasMeshLower())
						{
							componentsInChildren2[k].sharedMesh = this._meshContainerSkinned[k].CreateMeshUpper();
						}
						else if (!this._meshContainerSkinned[k].HasMeshUpper())
						{
							UnityEngine.Object.Destroy(componentsInChildren2[k]);
						}
					}
					else if (this._meshContainerSkinned[k].HasMeshUpper() & this._meshContainerSkinned[k].HasMeshLower())
					{
						componentsInChildren2[k].sharedMesh = this._meshContainerSkinned[k].CreateMeshLower();
					}
					else if (!this._meshContainerSkinned[k].HasMeshLower())
					{
						UnityEngine.Object.Destroy(componentsInChildren2[k]);
					}
				}
			}
		}

		// Token: 0x0600164F RID: 5711 RVA: 0x0006E8F4 File Offset: 0x0006CAF4
		private Material[] GetSharedMaterials(GameObject go)
		{
			SkinnedMeshRenderer component = go.GetComponent<SkinnedMeshRenderer>();
			if (component != null)
			{
				return component.sharedMaterials;
			}
			Renderer component2 = go.GetComponent<Renderer>();
			if (component2 != null)
			{
				return component2.sharedMaterials;
			}
			return null;
		}

		// Token: 0x06001650 RID: 5712 RVA: 0x0006E930 File Offset: 0x0006CB30
		private void SetSharedMaterials(GameObject go, Material[] materials)
		{
			SkinnedMeshRenderer component = go.GetComponent<SkinnedMeshRenderer>();
			if (component != null)
			{
				component.sharedMaterials = materials;
				return;
			}
			Renderer component2 = go.GetComponent<Renderer>();
			if (component2 != null)
			{
				component2.sharedMaterials = materials;
			}
		}

		// Token: 0x06001651 RID: 5713 RVA: 0x0006E96C File Offset: 0x0006CB6C
		private void SetMeshOnGameObject(GameObject go, Mesh mesh)
		{
			SkinnedMeshRenderer component = go.GetComponent<SkinnedMeshRenderer>();
			if (component != null)
			{
				component.sharedMesh = mesh;
				return;
			}
			MeshFilter component2 = go.GetComponent<MeshFilter>();
			if (component2 != null)
			{
				component2.mesh = mesh;
			}
		}

		// Token: 0x06001652 RID: 5714 RVA: 0x0006E9A8 File Offset: 0x0006CBA8
		private Mesh GetMeshOnGameObject(GameObject go)
		{
			SkinnedMeshRenderer component = go.GetComponent<SkinnedMeshRenderer>();
			if (component != null)
			{
				return component.sharedMesh;
			}
			MeshFilter component2 = go.GetComponent<MeshFilter>();
			if (component2 != null)
			{
				return component2.mesh;
			}
			return null;
		}

		// Token: 0x06001653 RID: 5715 RVA: 0x0000777A File Offset: 0x0000597A
		protected virtual void PostProcessObject(GameObject go)
		{
		}

		// Token: 0x04001008 RID: 4104
		public GameObject OptionalTargetObject;

		// Token: 0x04001009 RID: 4105
		public bool Convex;

		// Token: 0x0400100A RID: 4106
		public float SplitForce;

		// Token: 0x0400100B RID: 4107
		public bool CreateCap = true;

		// Token: 0x0400100C RID: 4108
		public bool UseCapUV;

		// Token: 0x0400100D RID: 4109
		public bool CustomUV;

		// Token: 0x0400100E RID: 4110
		public Vector2 CapUVMin = Vector2.zero;

		// Token: 0x0400100F RID: 4111
		public Vector2 CapUVMax = Vector2.one;

		// Token: 0x04001010 RID: 4112
		public bool ForceNoBatching;

		// Token: 0x04001011 RID: 4113
		private Transform _transform;

		// Token: 0x04001012 RID: 4114
		private PlaneMath _splitPlane;

		// Token: 0x04001013 RID: 4115
		private MeshContainer[] _meshContainerStatic;

		// Token: 0x04001014 RID: 4116
		private IMeshSplitter[] _meshSplitterStatic;

		// Token: 0x04001015 RID: 4117
		private MeshContainer[] _meshContainerSkinned;

		// Token: 0x04001016 RID: 4118
		private IMeshSplitter[] _meshSplitterSkinned;

		// Token: 0x04001017 RID: 4119
		private bool _isSplitting;

		// Token: 0x04001018 RID: 4120
		private bool _splitMesh;
	}
}
