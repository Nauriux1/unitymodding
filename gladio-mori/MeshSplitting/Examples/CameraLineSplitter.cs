using System;
using MeshSplitting.Splitters;
using UnityEngine;

namespace MeshSplitting.Examples
{
	// Token: 0x020002E2 RID: 738
	[AddComponentMenu("Mesh Splitting/Examples/Camera Line Splitter")]
	[RequireComponent(typeof(Camera))]
	[RequireComponent(typeof(LineRenderer))]
	public class CameraLineSplitter : MonoBehaviour
	{
		// Token: 0x06001696 RID: 5782 RVA: 0x0007219C File Offset: 0x0007039C
		private void Awake()
		{
			this._transform = base.transform;
			this._lineRenderer = base.GetComponent<LineRenderer>();
			this._camera = base.GetComponent<Camera>();
			this._lineRenderer.enabled = false;
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		// Token: 0x06001697 RID: 5783 RVA: 0x000721DC File Offset: 0x000703DC
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				this._inCutMode = true;
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
			else if (Input.GetKeyUp(KeyCode.Space))
			{
				this._inCutMode = false;
				this._lineRenderer.enabled = false;
				this._hasStartPos = false;
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
			if (this._inCutMode)
			{
				if (Input.GetMouseButtonDown(0))
				{
					this._startPos = this.GetMousePosInWorld();
					this._hasStartPos = true;
				}
				else if (this._hasStartPos && Input.GetMouseButtonUp(0))
				{
					this._endPos = this.GetMousePosInWorld();
					if (this._startPos != this._endPos)
					{
						this.CreateCutPlane();
					}
					this._hasStartPos = false;
					this._lineRenderer.enabled = false;
				}
				if (this._hasStartPos)
				{
					this._lineRenderer.enabled = true;
					this._lineRenderer.SetPosition(0, this._startPos);
					this._lineRenderer.SetPosition(1, this.GetMousePosInWorld());
				}
			}
		}

		// Token: 0x06001698 RID: 5784 RVA: 0x000722E0 File Offset: 0x000704E0
		private Vector3 GetMousePosInWorld()
		{
			Ray ray = this._camera.ScreenPointToRay(Input.mousePosition);
			return ray.origin + ray.direction * this.CutPlaneDistance;
		}

		// Token: 0x06001699 RID: 5785 RVA: 0x0007231C File Offset: 0x0007051C
		private void CreateCutPlane()
		{
			Vector3 vector = Vector3.Lerp(this._startPos, this._endPos, 0.5f);
			Vector3 normalized = (this._endPos - this._startPos).normalized;
			Vector3 normalized2 = (vector - this._transform.position).normalized;
			Vector3 normalized3 = Vector3.Cross(normalized2, normalized).normalized;
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
			transform.up = normalized3;
			float num = Vector3.Angle(transform.forward, normalized2);
			transform.RotateAround(vector, normalized3, (normalized3.y < 0f) ? (-num) : num);
		}

		// Token: 0x04001063 RID: 4195
		public float CutPlaneDistance = 1f;

		// Token: 0x04001064 RID: 4196
		public float CutPlaneSize = 2f;

		// Token: 0x04001065 RID: 4197
		private LineRenderer _lineRenderer;

		// Token: 0x04001066 RID: 4198
		private Camera _camera;

		// Token: 0x04001067 RID: 4199
		private Transform _transform;

		// Token: 0x04001068 RID: 4200
		private bool _inCutMode;

		// Token: 0x04001069 RID: 4201
		private bool _hasStartPos;

		// Token: 0x0400106A RID: 4202
		private Vector3 _startPos;

		// Token: 0x0400106B RID: 4203
		private Vector3 _endPos;
	}
}
