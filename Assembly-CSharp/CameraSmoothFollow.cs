using System;
using UnityEngine;
using Utils;

// Token: 0x02000039 RID: 57
public class CameraSmoothFollow : MonoBehaviour
{
	// Token: 0x060001C2 RID: 450 RVA: 0x0000A728 File Offset: 0x00008928
	protected virtual void Awake()
	{
		this.objectCamera = base.gameObject.GetComponent<Camera>();
		this.cameraHitLayerMask = LayerMask.GetMask(new string[]
		{
			"MapPart"
		});
		if (!this.OverridePosition)
		{
			this.cameraOffset = new Vector3(0f, 1.8f, -1.8f);
			this.targetOffset = new Vector3(0f, 0.6f, 0f);
			PlayerCameraSettings cameraSettings = SettingsHelper.GetCameraSettings();
			this.SetCameraSettings(cameraSettings, false, null);
		}
	}

	// Token: 0x060001C3 RID: 451 RVA: 0x0000A7B0 File Offset: 0x000089B0
	protected virtual void Start()
	{
		if (SettingsHelper.freecam && base.gameObject.GetComponent<Camera>() == Camera.main && Camera.allCameras.Length == 1 && base.gameObject.GetComponent<DemoFreeCameraControls>() == null)
		{
			base.gameObject.AddComponent<DemoFreeCameraControls>();
		}
	}

	// Token: 0x060001C4 RID: 452 RVA: 0x0000A804 File Offset: 0x00008A04
	public virtual void SetCameraSettings(PlayerCameraSettings playerCameraSettings, bool preview = false, GameObject previewPrefab = null)
	{
		if (!this.OverridePosition)
		{
			this.cameraOffset = playerCameraSettings.cameraPositionOffset;
			this.targetOffset = playerCameraSettings.cameraTargetOffset;
			this.cameraOffsetDistance = new Vector3(0f, 0f, -Vector3.Distance(this.targetOffset, this.cameraOffset));
			if (previewPrefab != null)
			{
				this.previewTargetPrefab = previewPrefab;
			}
			this.isPreviewing = preview;
			this.CheckTargetPreviewObject();
			this.calculatedFullRotationOffset = Quaternion.LookRotation(this.targetOffset - this.cameraOffset);
			this.calculatedYRotationOffset = Quaternion.Euler(new Vector3(0f, this.calculatedFullRotationOffset.eulerAngles.y, 0f));
		}
		if (this.objectCamera != null)
		{
			this.objectCamera.fieldOfView = (float)playerCameraSettings.cameraFov;
		}
	}

	// Token: 0x060001C5 RID: 453 RVA: 0x0000A8E0 File Offset: 0x00008AE0
	protected virtual void LateUpdate()
	{
		if (this.target && this.positionTarget)
		{
			this.newRotation = this.GetNewRotation();
			this.cameraTarget = this.target.rotation * this.targetOffset + this.positionTarget.position;
			this.cameraPosition = this.newRotation * this.cameraOffsetDistance + this.cameraTarget;
			if (this.shouldRotate)
			{
				if (this.isPreviewing && this.previewTarget != null)
				{
					this.previewTarget.transform.position = this.cameraTarget;
					this.previewTarget.transform.rotation = this.target.rotation;
				}
				base.transform.position = this.GetCameraPosition(this.cameraTarget, this.cameraPosition);
				Quaternion rotation = default(Quaternion);
				Vector3 forward = this.cameraTarget - this.cameraPosition;
				if (forward.magnitude > 0f)
				{
					rotation = Quaternion.LookRotation(forward, Vector3.up);
				}
				base.transform.rotation = rotation;
				if (this.OverridePosition)
				{
					base.transform.position = new Vector3(base.transform.position.x, this.OverridePositionY, base.transform.position.z);
					return;
				}
			}
			else
			{
				base.transform.position = this.cameraPosition;
			}
		}
	}

	// Token: 0x060001C6 RID: 454 RVA: 0x0000AA66 File Offset: 0x00008C66
	protected virtual Quaternion GetNewRotation()
	{
		return this.target.rotation * this.calculatedFullRotationOffset;
	}

	// Token: 0x060001C7 RID: 455 RVA: 0x0000AA80 File Offset: 0x00008C80
	public Vector3 GetCameraPosition(Vector3 lookAtTarget, Vector3 cameraPoint)
	{
		Vector3 result = cameraPoint;
		Vector3 vector = new Vector3(this.positionTarget.position.x, this.positionTarget.position.y + this.cameraPositionToKeepInViewOffset.y, this.positionTarget.position.z);
		Vector3 direction = cameraPoint - vector;
		if (Physics.Raycast(vector, direction, out this.hit, Vector3.Distance(vector, cameraPoint), this.cameraHitLayerMask))
		{
			result = this.hit.point - direction.normalized * this.cameraDistanceFromWall;
		}
		return result;
	}

	// Token: 0x060001C8 RID: 456 RVA: 0x0000AB1F File Offset: 0x00008D1F
	public void SetTarget(GameObject newTarget, GameObject newPositionTarget)
	{
		this.target = newTarget.transform;
		this.positionTarget = newPositionTarget.transform;
	}

	// Token: 0x060001C9 RID: 457 RVA: 0x0000AB3C File Offset: 0x00008D3C
	public void CheckTargetPreviewObject()
	{
		if (this.isPreviewing)
		{
			if (this.previewTarget == null && this.previewTargetPrefab != null)
			{
				this.previewTarget = UnityEngine.Object.Instantiate<GameObject>(this.previewTargetPrefab);
			}
			if (!this.previewTarget.activeInHierarchy)
			{
				this.previewTarget.SetActive(true);
				return;
			}
		}
		else if (this.previewTarget != null && this.previewTarget.activeInHierarchy)
		{
			this.previewTarget.SetActive(false);
		}
	}

	// Token: 0x04000103 RID: 259
	public Camera objectCamera;

	// Token: 0x04000104 RID: 260
	public bool shouldRotate = true;

	// Token: 0x04000105 RID: 261
	public Transform target;

	// Token: 0x04000106 RID: 262
	public Transform positionTarget;

	// Token: 0x04000107 RID: 263
	public Vector3 cameraOffset = new Vector3(0f, 1.8f, -1.8f);

	// Token: 0x04000108 RID: 264
	public Vector3 targetOffset = new Vector3(0f, 0.6f, 0f);

	// Token: 0x04000109 RID: 265
	public float smoothSpeed = 0.05f;

	// Token: 0x0400010A RID: 266
	public float smoothRotationSpeed = 0.1f;

	// Token: 0x0400010B RID: 267
	public bool OverridePosition;

	// Token: 0x0400010C RID: 268
	public float OverridePositionY;

	// Token: 0x0400010D RID: 269
	private Vector3 cameraPosition;

	// Token: 0x0400010E RID: 270
	private Quaternion newRotation;

	// Token: 0x0400010F RID: 271
	private Vector3 cameraTarget;

	// Token: 0x04000110 RID: 272
	private Vector3 velocity = Vector3.zero;

	// Token: 0x04000111 RID: 273
	public GameObject previewTargetPrefab;

	// Token: 0x04000112 RID: 274
	public GameObject previewTarget;

	// Token: 0x04000113 RID: 275
	public bool isPreviewing;

	// Token: 0x04000114 RID: 276
	private Vector3 cameraPositionToKeepInViewOffset = new Vector3(0f, 0.8f, 0f);

	// Token: 0x04000115 RID: 277
	private Vector3 cameraOffsetDistance;

	// Token: 0x04000116 RID: 278
	public Quaternion calculatedYRotationOffset;

	// Token: 0x04000117 RID: 279
	public Quaternion calculatedFullRotationOffset;

	// Token: 0x04000118 RID: 280
	private float cameraDistanceFromWall = 0.1f;

	// Token: 0x04000119 RID: 281
	private RaycastHit[] hits = new RaycastHit[16];

	// Token: 0x0400011A RID: 282
	private LayerMask cameraHitLayerMask;

	// Token: 0x0400011B RID: 283
	private RaycastHit hit;
}
