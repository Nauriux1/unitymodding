using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

// Token: 0x02000037 RID: 55
public class CameraFollowPlayers : MonoBehaviour
{
	// Token: 0x060001AF RID: 431 RVA: 0x00009EC0 File Offset: 0x000080C0
	private void Start()
	{
		this.UpdateCameraStuff();
	}

	// Token: 0x060001B0 RID: 432 RVA: 0x00009EC8 File Offset: 0x000080C8
	private void UpdateCameraStuff()
	{
		this.objectCamera = base.gameObject.GetComponent<Camera>();
		if (this.objectCamera != null)
		{
			float num = this.objectCamera.fieldOfView * 0.017453292f;
			float num2 = 2f * Mathf.Atan(Mathf.Tan(num / 2f) * this.objectCamera.aspect);
			this.horizontalFov = 57.29578f * num2;
		}
	}

	// Token: 0x060001B1 RID: 433 RVA: 0x00009F38 File Offset: 0x00008138
	private void LateUpdate()
	{
		if (this.targets.Count == 0)
		{
			return;
		}
		this.center = this.GetCenter();
		this.GetLongestDistancePair();
		Vector3 cameraTargetPos = this.GetCameraTargetPos();
		this.SetCameraPosition(cameraTargetPos);
		Quaternion cameraTargetRotation = this.GetCameraTargetRotation();
		this.RotateCamera(cameraTargetRotation);
	}

	// Token: 0x060001B2 RID: 434 RVA: 0x00009F84 File Offset: 0x00008184
	private void SetCameraPosition(Vector3 targetPositionForCamera)
	{
		if (this.forcePosition)
		{
			this.forcePosition = false;
			base.gameObject.transform.position = targetPositionForCamera;
			return;
		}
		base.gameObject.transform.position = Vector3.Lerp(base.gameObject.transform.position, targetPositionForCamera, this.smoothSpeed * Time.deltaTime);
	}

	// Token: 0x060001B3 RID: 435 RVA: 0x00009FE4 File Offset: 0x000081E4
	private void RotateCamera(Quaternion rotation)
	{
		if (this.forceRotation)
		{
			this.forceRotation = false;
			base.transform.rotation = rotation;
			return;
		}
		base.transform.rotation = Quaternion.Lerp(base.transform.rotation, rotation, this.smoothSpeed * Time.deltaTime);
	}

	// Token: 0x060001B4 RID: 436 RVA: 0x0000A035 File Offset: 0x00008235
	private Quaternion GetCameraTargetRotation()
	{
		return Quaternion.LookRotation(new Vector3(this.center.x, 1f, this.center.z) - base.transform.position, Vector3.up);
	}

	// Token: 0x060001B5 RID: 437 RVA: 0x0000A074 File Offset: 0x00008274
	private Vector3 GetCameraTargetPos()
	{
		Vector3 cameraPoint = base.gameObject.transform.position;
		if (this.longestDistanceTarget1 != null)
		{
			float cameraDistance = this.GetCameraDistance(this.longestDistance + 4f, this.horizontalFov);
			Vector3 axis = new Vector3(this.longestDistanceTarget1.cameraPositionPoint.transform.position.x, 0f, this.longestDistanceTarget1.cameraPositionPoint.transform.position.z) - new Vector3(this.center.x, 0f, this.center.z);
			Vector3 vector = Quaternion.Euler(0f, -90f, 0f) * axis.normalized;
			vector = Quaternion.AngleAxis(-30f, axis) * vector;
			cameraPoint = new Vector3(this.center.x, 1f, this.center.z) + vector * cameraDistance;
		}
		return this.CheckCameraPositionForCollisions(this.center, cameraPoint);
	}

	// Token: 0x060001B6 RID: 438 RVA: 0x0000A190 File Offset: 0x00008390
	public Vector3 CheckCameraPositionForCollisions(Vector3 lookAtTarget, Vector3 cameraPoint)
	{
		Vector3 result = cameraPoint;
		float num = Vector3.Distance(lookAtTarget, cameraPoint) + this.cameraDistanceFromWall;
		Vector3 direction = cameraPoint - lookAtTarget;
		int num2 = Physics.RaycastNonAlloc(lookAtTarget, direction, this.hits, num);
		float num3 = num;
		if (this.hits.Length != 0)
		{
			for (int i = 0; i < num2; i++)
			{
				RaycastHit raycastHit = this.hits[i];
				if (raycastHit.transform.gameObject.layer == 16)
				{
					float num4 = Vector3.Distance(raycastHit.point, lookAtTarget);
					if (num4 < num3)
					{
						num3 = num4;
						result = Vector3.MoveTowards(raycastHit.point, lookAtTarget, this.cameraDistanceFromWall);
					}
				}
			}
		}
		return result;
	}

	// Token: 0x060001B7 RID: 439 RVA: 0x0000A234 File Offset: 0x00008434
	private float GetCameraDistance(float distance, float fov)
	{
		float aAngle = (180f - fov) / 2f;
		this.triangleSide = this.GetTriangleSide(distance, aAngle, fov);
		float triangleMedian = this.GetTriangleMedian(distance, this.triangleSide, this.triangleSide);
		if (triangleMedian < this.minDistance)
		{
			triangleMedian = this.minDistance;
		}
		return triangleMedian;
	}

	// Token: 0x060001B8 RID: 440 RVA: 0x0000A283 File Offset: 0x00008483
	private float GetTriangleMedian(float a, float b, float c)
	{
		return Mathf.Sqrt(2f * b * b + 2f * c * c - a * a) / 2f;
	}

	// Token: 0x060001B9 RID: 441 RVA: 0x0000A2A8 File Offset: 0x000084A8
	private float GetTriangleSide(float cLength, float aAngle, float cAngle)
	{
		return cLength * Mathf.Sin(aAngle * 0.017453292f) / Mathf.Sin(cAngle * 0.017453292f);
	}

	// Token: 0x060001BA RID: 442 RVA: 0x0000A2C8 File Offset: 0x000084C8
	private Vector3 GetCenter()
	{
		Vector3 vector = new Vector3(0f, 0f, 0f);
		foreach (PlayerHealth playerHealth in this.targets)
		{
			vector += playerHealth.cameraPositionPoint.transform.position;
		}
		vector /= (float)this.targets.Count;
		return vector;
	}

	// Token: 0x060001BB RID: 443 RVA: 0x0000A358 File Offset: 0x00008558
	private void GetLongestDistancePair()
	{
		this.aliveTargets.Clear();
		for (int i = 0; i < this.targets.Count; i++)
		{
			if (this.targets[i].alive)
			{
				this.aliveTargets.Add(this.targets[i]);
			}
		}
		this.longestDistanceTarget1 = null;
		this.longestDistanceTarget2 = null;
		this.longestDistance = 0f;
		if (this.aliveTargets.Count >= 2)
		{
			for (int j = 0; j < this.aliveTargets.Count; j++)
			{
				PlayerHealth playerHealth = this.aliveTargets[j];
				for (int k = j; k < this.aliveTargets.Count; k++)
				{
					PlayerHealth playerHealth2 = this.aliveTargets[k];
					float num = Vector3.Distance(playerHealth.cameraPositionPoint.transform.position, playerHealth2.cameraPositionPoint.transform.position);
					if (num > this.longestDistance)
					{
						this.longestDistanceTarget1 = playerHealth;
						this.longestDistanceTarget2 = playerHealth2;
						this.longestDistance = num;
					}
				}
			}
		}
	}

	// Token: 0x060001BC RID: 444 RVA: 0x0000A470 File Offset: 0x00008670
	public void UpdateTargets()
	{
		this.targets.Clear();
		if (ReplayManager.singleton != null && ReplayManager.singleton.replayMode == ReplayMode.Replay && ReplayManager.singleton.recordingPlayers != null)
		{
			using (List<PlayerHealth>.Enumerator enumerator = ReplayManager.singleton.recordingPlayers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PlayerHealth item = enumerator.Current;
					this.targets.Add(item);
				}
				return;
			}
		}
		if (NetworkClient.active && GeneralManager.singleton != null)
		{
			foreach (PlayerHealth item2 in GeneralManager.singleton.registeredPlayerHealths)
			{
				this.targets.Add(item2);
			}
		}
	}

	// Token: 0x060001BD RID: 445 RVA: 0x0000A55C File Offset: 0x0000875C
	private void OnEnable()
	{
		this.forcePosition = true;
		this.forceRotation = true;
	}

	// Token: 0x040000F0 RID: 240
	public float smoothSpeed = 3f;

	// Token: 0x040000F1 RID: 241
	public float minDistance = 3f;

	// Token: 0x040000F2 RID: 242
	public List<PlayerHealth> targets = new List<PlayerHealth>(16);

	// Token: 0x040000F3 RID: 243
	private Vector3 center;

	// Token: 0x040000F4 RID: 244
	private PlayerHealth longestDistanceTarget1;

	// Token: 0x040000F5 RID: 245
	private PlayerHealth longestDistanceTarget2;

	// Token: 0x040000F6 RID: 246
	private float longestDistance;

	// Token: 0x040000F7 RID: 247
	private Camera objectCamera;

	// Token: 0x040000F8 RID: 248
	public float horizontalFov = 80f;

	// Token: 0x040000F9 RID: 249
	private bool forcePosition;

	// Token: 0x040000FA RID: 250
	private bool forceRotation;

	// Token: 0x040000FB RID: 251
	private float cameraDistanceFromWall = 0.1f;

	// Token: 0x040000FC RID: 252
	private RaycastHit[] hits = new RaycastHit[8];

	// Token: 0x040000FD RID: 253
	private float triangleSide;

	// Token: 0x040000FE RID: 254
	private List<PlayerHealth> aliveTargets = new List<PlayerHealth>(16);
}
