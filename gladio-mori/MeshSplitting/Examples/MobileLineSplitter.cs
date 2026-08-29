using System;
using MeshSplitting.Splitables;
using MeshSplitting.Splitters;
using UnityEngine;

namespace MeshSplitting.Examples
{
	// Token: 0x020002E3 RID: 739
	[AddComponentMenu("Mesh Splitting/Examples/Mobile Line Splitter")]
	[RequireComponent(typeof(Camera))]
	[RequireComponent(typeof(LineRenderer))]
	public class MobileLineSplitter : MonoBehaviour
	{
		// Token: 0x0600169B RID: 5787 RVA: 0x00072454 File Offset: 0x00070654
		private void Awake()
		{
			this._transform = base.GetComponent<Transform>();
			this._lineRenderer = base.GetComponent<LineRenderer>();
			this._camera = base.GetComponent<Camera>();
			this._lineRenderer.enabled = false;
			if (this.SplitablePrefabs.Length != 0)
			{
				UnityEngine.Object.Instantiate<GameObject>(this.SplitablePrefabs[0], Vector3.up * 2f, Quaternion.identity);
			}
			this._rects = new Rect[this.SplitableIcons.Length + 2];
			int width = Screen.width;
			int height = Screen.height;
			int num = width / 20;
			int num2 = height / 20;
			int i;
			for (i = 0; i < this.SplitableIcons.Length; i++)
			{
				int num3 = num * (i * 2 + 1);
				this._rects[i] = new Rect((float)num3, (float)num2, (float)(num * 2), (float)(num * 2));
			}
			this._rects[i++] = new Rect((float)(width - num2 - 50), (float)num2, 50f, (float)(height - num2 * 5));
			this._rects[i] = new Rect((float)num2, (float)(height - num2 - 50), (float)(width - num2 * 5), 50f);
		}

		// Token: 0x0600169C RID: 5788 RVA: 0x00072578 File Offset: 0x00070778
		private void Update()
		{
			if (Input.GetKey(KeyCode.Escape))
			{
				Application.Quit();
			}
			Vector3 position;
			this.CalcPosition(out position);
			this._transform.position = position;
			this._transform.LookAt(this.Target);
			if (Input.GetMouseButtonDown(0))
			{
				this._mouseDown = true;
				Vector3 mousePosition = Input.mousePosition;
				mousePosition.y = (float)Screen.height - mousePosition.y;
				for (int i = 0; i < this._rects.Length; i++)
				{
					Rect rect = this._rects[i];
					int num = (int)(mousePosition.x - rect.x);
					int num2 = (int)(mousePosition.y - rect.y);
					if (0 < num && (float)num < rect.width && 0 < num2 && (float)num2 < rect.height)
					{
						this._mouseDown = false;
						break;
					}
				}
			}
			if (Input.GetMouseButtonDown(0) && this._mouseDown)
			{
				this._startPos = Input.mousePosition;
				this._hasStartPos = true;
			}
			else if (this._hasStartPos && Input.GetMouseButtonUp(0) && this._mouseDown)
			{
				this._endPos = Input.mousePosition;
				if (Vector3.Distance(this._startPos, this._endPos) > this.MinSplitDistance)
				{
					this.CreateCutPlane();
				}
				else
				{
					Ray ray = this._camera.ScreenPointToRay(Input.mousePosition);
					RaycastHit raycastHit;
					if (Physics.Raycast(ray, out raycastHit))
					{
						Rigidbody component = raycastHit.collider.GetComponent<Rigidbody>();
						if (component != null)
						{
							component.AddForce(ray.direction * component.mass * this.ForcePush, ForceMode.Impulse);
						}
					}
				}
				this._hasStartPos = false;
				this._lineRenderer.enabled = false;
			}
			if (this._hasStartPos)
			{
				this._lineRenderer.enabled = true;
				this._lineRenderer.SetPosition(0, this.GetPosInWorld(this._startPos));
				this._lineRenderer.SetPosition(1, this.GetPosInWorld(Input.mousePosition));
			}
		}

		// Token: 0x0600169D RID: 5789 RVA: 0x00072778 File Offset: 0x00070978
		private void OnGUI()
		{
			if (this.GuiSkin != null)
			{
				GUI.skin = this.GuiSkin;
			}
			this.View.x = GUI.VerticalScrollbar(this._rects[this.SplitableIcons.Length], this.View.x, 7f, 70f, 0f);
			this.View.y = GUI.HorizontalScrollbar(this._rects[this.SplitableIcons.Length + 1], this.View.y, 36f, -180f, 180f);
			for (int i = 0; i < this.SplitableIcons.Length; i++)
			{
				if (GUI.Button(this._rects[i], this.SplitableIcons[i]))
				{
					this.CreateNewObject(i);
				}
			}
		}

		// Token: 0x0600169E RID: 5790 RVA: 0x00072850 File Offset: 0x00070A50
		private void CreateNewObject(int i)
		{
			if (this.SplitablePrefabs[i] != null)
			{
				Splitable splitable = UnityEngine.Object.FindObjectOfType(typeof(Splitable)) as Splitable;
				if (splitable != null)
				{
					if (splitable.transform.parent == null)
					{
						UnityEngine.Object.Destroy(splitable.gameObject);
					}
					else
					{
						UnityEngine.Object.Destroy(splitable.transform.parent.gameObject);
					}
				}
				UnityEngine.Object.Instantiate<GameObject>(this.SplitablePrefabs[i], Vector3.up * 2f, this.SplitablePrefabs[i].transform.rotation);
			}
		}

		// Token: 0x0600169F RID: 5791 RVA: 0x000728F4 File Offset: 0x00070AF4
		private void CalcPosition(out Vector3 position)
		{
			Vector3 point = Vector3.forward * -this.Distance;
			Quaternion rotation = Quaternion.Euler(this.View.x, this.View.y, 0f);
			position = this.Target + rotation * point;
		}

		// Token: 0x060016A0 RID: 5792 RVA: 0x0007294C File Offset: 0x00070B4C
		private Vector3 GetPosInWorld(Vector3 pos)
		{
			Ray ray = this._camera.ScreenPointToRay(pos);
			return ray.origin + ray.direction * this.CutPlaneDistance;
		}

		// Token: 0x060016A1 RID: 5793 RVA: 0x00072984 File Offset: 0x00070B84
		private void CreateCutPlane()
		{
			Vector3 posInWorld = this.GetPosInWorld(this._startPos);
			Vector3 posInWorld2 = this.GetPosInWorld(this._endPos);
			Vector3 vector = Vector3.Lerp(posInWorld, posInWorld2, 0.5f);
			Vector3 normalized = (posInWorld2 - posInWorld).normalized;
			Vector3 normalized2 = Vector3.Cross((vector - this._transform.position).normalized, normalized).normalized;
			GameObject gameObject = new GameObject("CutPlane", new Type[]
			{
				typeof(BoxCollider),
				typeof(Rigidbody),
				typeof(SplitterSingleCut)
			});
			gameObject.GetComponent<Collider>().isTrigger = true;
			Rigidbody component = gameObject.GetComponent<Rigidbody>();
			component.useGravity = false;
			component.isKinematic = true;
			Transform transform = gameObject.transform;
			transform.position = vector;
			transform.localScale = new Vector3(this.CutPlaneSize, 0.01f, this.CutPlaneSize);
			transform.rotation = this._transform.rotation;
			transform.up = normalized2;
		}

		// Token: 0x0400106C RID: 4204
		public float CutPlaneDistance = 5f;

		// Token: 0x0400106D RID: 4205
		public float CutPlaneSize = 10f;

		// Token: 0x0400106E RID: 4206
		public float MinSplitDistance = 20f;

		// Token: 0x0400106F RID: 4207
		private LineRenderer _lineRenderer;

		// Token: 0x04001070 RID: 4208
		private Camera _camera;

		// Token: 0x04001071 RID: 4209
		private Transform _transform;

		// Token: 0x04001072 RID: 4210
		private bool _hasStartPos;

		// Token: 0x04001073 RID: 4211
		private Vector3 _startPos;

		// Token: 0x04001074 RID: 4212
		private Vector3 _endPos;

		// Token: 0x04001075 RID: 4213
		public Vector2 View = new Vector2(0f, 10f);

		// Token: 0x04001076 RID: 4214
		public float Distance = 5f;

		// Token: 0x04001077 RID: 4215
		public Vector3 Target = Vector3.up;

		// Token: 0x04001078 RID: 4216
		public float ForcePush = 1f;

		// Token: 0x04001079 RID: 4217
		public GUISkin GuiSkin;

		// Token: 0x0400107A RID: 4218
		public Texture2D[] SplitableIcons;

		// Token: 0x0400107B RID: 4219
		public GameObject[] SplitablePrefabs;

		// Token: 0x0400107C RID: 4220
		private Rect[] _rects;

		// Token: 0x0400107D RID: 4221
		private bool _mouseDown;
	}
}
