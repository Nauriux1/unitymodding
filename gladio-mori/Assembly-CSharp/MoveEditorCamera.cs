using System;
using UnityEngine;

// Token: 0x02000121 RID: 289
public class MoveEditorCamera : MonoBehaviour
{
	// Token: 0x060008EF RID: 2287 RVA: 0x0002B644 File Offset: 0x00029844
	private void Start()
	{
		this.activeCamera = null;
		this.activeCameraTarget = null;
		this.physicsCamera.transform.position = this.physicsCameraTarget.position;
		this.animationCamera.transform.position = this.animationCameraTarget.position;
		this.physicsCamera.transform.Translate(new Vector3(0f, 0f, this.activeZoomLevel));
		this.animationCamera.transform.Translate(new Vector3(0f, 0f, this.activeZoomLevel));
	}

	// Token: 0x060008F0 RID: 2288 RVA: 0x0002B6E0 File Offset: 0x000298E0
	private void Update()
	{
		if (Input.mouseScrollDelta.y != 0f)
		{
			this.activeZoomLevel += Input.mouseScrollDelta.y * 0.5f;
			if (this.activeZoomLevel > 0f)
			{
				this.activeZoomLevel = 0f;
			}
			if (this.activeCamera == this.physicsCamera)
			{
				this.physicsZoomLevel = this.activeZoomLevel;
			}
			else
			{
				this.animationZoomLevel = this.activeZoomLevel;
			}
		}
		if (Input.GetMouseButtonDown(0))
		{
			this.previousMousePosition = Input.mousePosition;
			if ((float)(Screen.width / 2) > this.previousMousePosition.x)
			{
				this.activeCamera = this.physicsCamera;
				this.activeCameraTarget = this.physicsCameraTarget;
				this.activeZoomLevel = this.physicsZoomLevel;
			}
			else
			{
				this.activeCamera = this.animationCamera;
				this.activeCameraTarget = this.animationCameraTarget;
				this.activeZoomLevel = this.animationZoomLevel;
			}
			Debug.Log("CAMERA:" + this.activeCamera.name);
			return;
		}
		if (Input.GetMouseButton(0))
		{
			Vector3 vector = this.previousMousePosition - Input.mousePosition;
			vector.y /= (float)Screen.height;
			vector.x /= (float)Screen.width;
			this.activeCamera.transform.position = this.activeCameraTarget.position;
			this.activeCamera.transform.Rotate(new Vector3(1f, 0f, 0f), vector.y * 180f);
			this.activeCamera.transform.Rotate(new Vector3(0f, 1f, 0f), -vector.x * 180f, Space.World);
			this.activeCamera.transform.Translate(new Vector3(0f, 0f, this.activeZoomLevel));
			this.previousMousePosition = Input.mousePosition;
			return;
		}
		this.physicsCamera.transform.position = this.physicsCameraTarget.position;
		this.physicsCamera.transform.Translate(new Vector3(0f, 0f, this.physicsZoomLevel));
		this.animationCamera.transform.position = this.animationCameraTarget.position;
		this.animationCamera.transform.Translate(new Vector3(0f, 0f, this.animationZoomLevel));
	}

	// Token: 0x04000637 RID: 1591
	public GameObject physicsCamera;

	// Token: 0x04000638 RID: 1592
	public GameObject animationCamera;

	// Token: 0x04000639 RID: 1593
	private Vector3 previousMousePosition;

	// Token: 0x0400063A RID: 1594
	public float physicsZoomLevel = -5f;

	// Token: 0x0400063B RID: 1595
	public float animationZoomLevel = -5f;

	// Token: 0x0400063C RID: 1596
	public float activeZoomLevel = -5f;

	// Token: 0x0400063D RID: 1597
	public Transform physicsCameraTarget;

	// Token: 0x0400063E RID: 1598
	public Transform animationCameraTarget;

	// Token: 0x0400063F RID: 1599
	public GameObject activeCamera;

	// Token: 0x04000640 RID: 1600
	public Transform activeCameraTarget;
}
