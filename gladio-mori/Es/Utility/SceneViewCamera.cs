using System;
using UnityEngine;

namespace Es.Utility
{
	// Token: 0x020002E4 RID: 740
	[RequireComponent(typeof(Camera))]
	public class SceneViewCamera : MonoBehaviour
	{
		// Token: 0x060016A3 RID: 5795 RVA: 0x00072AF6 File Offset: 0x00070CF6
		private void Update()
		{
			this.MouseUpdate();
		}

		// Token: 0x060016A4 RID: 5796 RVA: 0x00072B00 File Offset: 0x00070D00
		private void MouseUpdate()
		{
			float axis = Input.GetAxis("Mouse ScrollWheel");
			if (axis != 0f)
			{
				this.MouseWheel(axis);
			}
			if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
			{
				this.preMousePos = Input.mousePosition;
			}
			this.MouseDrag(Input.mousePosition);
		}

		// Token: 0x060016A5 RID: 5797 RVA: 0x00072B55 File Offset: 0x00070D55
		private void MouseWheel(float delta)
		{
			base.transform.position += base.transform.forward * delta * this.wheelSpeed;
		}

		// Token: 0x060016A6 RID: 5798 RVA: 0x00072B8C File Offset: 0x00070D8C
		private void MouseDrag(Vector3 mousePos)
		{
			Vector3 vector = mousePos - this.preMousePos;
			if (vector.magnitude < 1E-05f)
			{
				return;
			}
			if (Input.GetMouseButton(2))
			{
				base.transform.Translate(-vector * Time.deltaTime * this.moveSpeed);
			}
			else if (Input.GetMouseButton(1))
			{
				this.CameraRotate(new Vector2(-vector.y, vector.x) * this.rotateSpeed);
			}
			this.preMousePos = mousePos;
		}

		// Token: 0x060016A7 RID: 5799 RVA: 0x00072C18 File Offset: 0x00070E18
		public void CameraRotate(Vector2 angle)
		{
			base.transform.RotateAround(base.transform.position, base.transform.right, angle.x);
			base.transform.RotateAround(base.transform.position, Vector3.up, angle.y);
		}

		// Token: 0x0400107E RID: 4222
		[SerializeField]
		[Range(0.1f, 100f)]
		private float wheelSpeed = 1f;

		// Token: 0x0400107F RID: 4223
		[SerializeField]
		[Range(0.1f, 100f)]
		private float moveSpeed = 0.3f;

		// Token: 0x04001080 RID: 4224
		[SerializeField]
		[Range(0.1f, 1f)]
		private float rotateSpeed = 0.3f;

		// Token: 0x04001081 RID: 4225
		private Vector3 preMousePos;
	}
}
