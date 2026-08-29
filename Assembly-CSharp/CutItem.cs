using System;
using System.Collections.Generic;
using Mirror;
using MoveClasses;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

// Token: 0x02000065 RID: 101
[Serializable]
public class CutItem
{
	// Token: 0x060002C0 RID: 704 RVA: 0x0000D8D4 File Offset: 0x0000BAD4
	public void InitCutItem(bool fullCut = false)
	{
		if (this.cuttableGameObject != null && this.cuttableGameObject.cutDone)
		{
			this.disabledFully = true;
		}
		if (!fullCut)
		{
			this.UpdateWeaponStartValues();
			this.CreateCheckCutJob();
			return;
		}
		this.fullCutMode = true;
		this.FillNativeArrays();
		this.CreateDoCutJob(this.fullCutPlane, true);
	}

	// Token: 0x060002C1 RID: 705 RVA: 0x0000D92D File Offset: 0x0000BB2D
	public void UpdateWeaponStartValues()
	{
		this.weaponPositionStart = this.cuttableGameObject.transform.InverseTransformPoint(this.weapon.transform.position);
		this.weapon.GetWeaponSections();
	}

	// Token: 0x060002C2 RID: 706 RVA: 0x0000D961 File Offset: 0x0000BB61
	public bool CanBeRemoved()
	{
		return this.disabledFully || (this.queuedForRemoval && !this.checkCutJobRunning && !this.doCutJobRunning && (!this.fullCutQueued || this.fullCutHandled));
	}

	// Token: 0x060002C3 RID: 707 RVA: 0x0000D997 File Offset: 0x0000BB97
	public void HandleCutItem()
	{
		this.HandleDoCutJob();
		this.HandleJob();
		this.ScheduleCheckCutJob();
	}

	// Token: 0x060002C4 RID: 708 RVA: 0x0000D9AB File Offset: 0x0000BBAB
	public void HandleCutItemReset()
	{
		if (this.needsToBeReset)
		{
			this.FillNativeArrays();
			this.needsToBeReset = false;
			if (this.doCutJobRunning)
			{
				this.CreateDoCutJob(this.fullCutPlane, true);
			}
		}
	}

	// Token: 0x060002C5 RID: 709 RVA: 0x0000D9D8 File Offset: 0x0000BBD8
	public void HandleJob()
	{
		if (this.disabledFully)
		{
			return;
		}
		if (this.checkCutJobRunning)
		{
			this.checkCutJobRunning = false;
			this.checkCutJobHandle.Complete();
			this.UpdateCurrentBladeSection(false);
			this.UpdateMeshMatrices(this.checkCutResult[0].fullyCut);
			if (this.checkCutResult[0].fullyCut)
			{
				if (this.checkCutResult[0].parentCut)
				{
					this.disabledFully = true;
					CutManager.singleton.AddCutItem(this.cuttableGameObject.parentCuttableGameObject, this.checkCutResult[0].parentCutPlane);
					return;
				}
				this.CreateDoCutJob(this.checkCutResult[0].cutPlane.plane, true);
			}
		}
	}

	// Token: 0x060002C6 RID: 710 RVA: 0x0000DA9C File Offset: 0x0000BC9C
	public void DisposeNativeArrays()
	{
		this.checkCutJobHandle.Complete();
		this.doCutJobHandle.Complete();
		if (this.checkCutResult.IsCreated)
		{
			this.checkCutResult.Dispose();
		}
		if (this.cuttableSections.IsCreated)
		{
			this.cuttableSections.Dispose();
		}
		if (this.cuttableColliders.IsCreated)
		{
			this.cuttableColliders.Dispose();
		}
		if (this.tris.IsCreated)
		{
			this.tris.Dispose();
		}
		if (this.vertices.IsCreated)
		{
			this.vertices.Dispose();
		}
		if (this.uvs.IsCreated)
		{
			this.uvs.Dispose();
		}
		if (this.normals.IsCreated)
		{
			this.normals.Dispose();
		}
		if (this.cuttableMeshJobItems.IsCreated)
		{
			this.cuttableMeshJobItems.Dispose();
		}
		if (this.bladeSectionStart.IsCreated)
		{
			this.bladeSectionStart.Dispose();
		}
		if (this.bladeSectionCurrent.IsCreated)
		{
			this.bladeSectionCurrent.Dispose();
		}
		if (this.bladeSectionInfos.IsCreated)
		{
			this.bladeSectionInfos.Dispose();
		}
		this.DisposeDoCutJobNativeArrays();
	}

	// Token: 0x060002C7 RID: 711 RVA: 0x0000DBD0 File Offset: 0x0000BDD0
	public void UpdateCurrentBladeSection(bool getHistoryPosition = false)
	{
		List<WeaponEdgeSection> weaponEdgeSections = this.weapon.GetWeaponEdgeSections();
		if (getHistoryPosition)
		{
			HistoryPositionItem previousHistoryPosition = this.weapon.GetPreviousHistoryPosition();
			HistoryPositionItem previousHistoryPosition2 = this.cuttableGameObject.historyPositionTracker.GetPreviousHistoryPosition();
			for (int i = 0; i < weaponEdgeSections.Count; i++)
			{
				BladeSectionJobItem value = default(BladeSectionJobItem);
				for (int j = 0; j < weaponEdgeSections[i].points.Count; j++)
				{
					Vector3 point = previousHistoryPosition.localToWorldMatrix.MultiplyPoint3x4(this.weapon.transform.InverseTransformPoint(weaponEdgeSections[i].points[j].position));
					Vector3 vector = previousHistoryPosition2.localToWorldMatrix.inverse.MultiplyPoint3x4(point);
					value.bladePoints.Add(vector);
				}
				this.bladeSectionStart[i] = value;
				this.bladeSectionCurrent[i] = value;
			}
			return;
		}
		for (int k = 0; k < weaponEdgeSections.Count; k++)
		{
			BladeSectionJobItem value2 = default(BladeSectionJobItem);
			for (int l = 0; l < weaponEdgeSections[k].points.Count; l++)
			{
				Vector3 vector = this.cuttableGameObject.transform.InverseTransformPoint(weaponEdgeSections[k].points[l].position);
				value2.bladePoints.Add(vector);
			}
			this.bladeSectionCurrent[k] = value2;
		}
	}

	// Token: 0x060002C8 RID: 712 RVA: 0x0000DD50 File Offset: 0x0000BF50
	public Vector3 TransferToTestPosition(Vector3 position)
	{
		if (this.cuttableGameObject.bodyPart == JointType.NECK)
		{
			position += new Vector3(0f, 0.04f, 0f);
		}
		else if (this.cuttableGameObject.bodyPart == JointType.SPINE2)
		{
			position += new Vector3(0f, -0.04f, 0f);
		}
		return position;
	}

	// Token: 0x060002C9 RID: 713 RVA: 0x0000DDB8 File Offset: 0x0000BFB8
	public void UpdateMeshMatrices(bool updateMeshMatrices = false)
	{
		if (updateMeshMatrices)
		{
			for (int i = 0; i < this.cuttableGameObject.cuttableMeshList.Count; i++)
			{
				CuttableMeshJobItem value = this.cuttableMeshJobItems[i];
				value.meshLocalToWorldMatrix = this.cuttableGameObject.cuttableMeshList[i].meshFilter.transform.localToWorldMatrix;
				this.cuttableMeshJobItems[i] = value;
			}
		}
		this.checkCutJob.localToWorldMatrix = this.cuttableGameObject.gameObject.transform.localToWorldMatrix;
		this.checkCutJob.worldToLocalMatrix = this.cuttableGameObject.gameObject.transform.worldToLocalMatrix;
		this.checkCutJob.parentLocalToWorldMatrix = this.GetParentLocalToWorldMatrix();
	}

	// Token: 0x060002CA RID: 714 RVA: 0x0000DE78 File Offset: 0x0000C078
	public Matrix4x4 GetParentLocalToWorldMatrix()
	{
		Matrix4x4 result = default(Matrix4x4);
		if (this.cuttableGameObject.parentCuttableGameObject != null)
		{
			result = this.cuttableGameObject.parentCuttableGameObject.transform.localToWorldMatrix;
		}
		return result;
	}

	// Token: 0x060002CB RID: 715 RVA: 0x0000DEB8 File Offset: 0x0000C0B8
	public void CreateCheckCutJob()
	{
		this.FillNativeArrays();
		this.checkCutJob = new CheckCutJob(this.cuttableSections, this.tris, this.vertices, this.cuttableMeshJobItems, this.cuttableGameObject.gameObject.transform.localToWorldMatrix, this.cuttableGameObject.gameObject.transform.worldToLocalMatrix, this.bladeSectionStart, this.bladeSectionCurrent, this.checkCutResult, this.cuttableColliders, this.bladeSectionInfos, this.GetParentLocalToWorldMatrix());
	}

	// Token: 0x060002CC RID: 716 RVA: 0x0000DF3C File Offset: 0x0000C13C
	private void FillNativeArrays()
	{
		this.DisposeNativeArrays();
		if (this.cuttableGameObject.cuttableMeshList.Count > 0)
		{
			int num = 0;
			int num2 = 0;
			this.cuttableMeshJobItems = new NativeArray<CuttableMeshJobItem>(this.cuttableGameObject.cuttableMeshList.Count, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			for (int i = 0; i < this.cuttableGameObject.cuttableMeshList.Count; i++)
			{
				CuttableMeshJobItem cuttableMeshJobItem = new CuttableMeshJobItem
				{
					meshLocalToWorldMatrix = this.cuttableGameObject.cuttableMeshList[i].meshFilter.transform.localToWorldMatrix,
					meshVertCounts = this.cuttableGameObject.cuttableMeshList[i].meshFilter.mesh.vertexCount,
					meshTriCounts = this.cuttableGameObject.cuttableMeshList[i].meshFilter.mesh.triangles.Length,
					ignoreInCheck = this.cuttableGameObject.cuttableMeshList[i].ignoreInCheck,
					cuttableSectionIndex = this.cuttableGameObject.cuttableMeshList[i].cuttableSectionIndex
				};
				this.cuttableMeshJobItems[i] = cuttableMeshJobItem;
				num += cuttableMeshJobItem.meshVertCounts;
				num2 += cuttableMeshJobItem.meshTriCounts;
			}
			this.FillCuttableSections();
			this.checkCutResult = new NativeArray<CheckCutJobOutValues>(1, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			if (this.cuttableGameObject.cuttableColliders != null)
			{
				this.cuttableColliders = new NativeArray<CuttableCollider>(this.cuttableGameObject.cuttableColliders, Allocator.Persistent);
			}
			else
			{
				this.cuttableColliders = new NativeArray<CuttableCollider>(0, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			}
			this.tris = new NativeArray<int>(num2, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.vertices = new NativeArray<Vector3>(num, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.uvs = new NativeArray<Vector2>(num, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.normals = new NativeArray<Vector3>(num, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			int num3 = 0;
			int num4 = 0;
			for (int j = 0; j < this.cuttableGameObject.cuttableMeshList.Count; j++)
			{
				int meshTriCounts = this.cuttableMeshJobItems[j].meshTriCounts;
				int meshVertCounts = this.cuttableMeshJobItems[j].meshVertCounts;
				NativeArray<int>.Copy(this.cuttableGameObject.cuttableMeshList[j].meshFilter.mesh.triangles, 0, this.tris, num4, meshTriCounts);
				NativeArray<Vector3>.Copy(this.cuttableGameObject.cuttableMeshList[j].meshFilter.mesh.vertices, 0, this.vertices, num3, meshVertCounts);
				NativeArray<Vector2>.Copy(this.cuttableGameObject.cuttableMeshList[j].meshFilter.mesh.uv, 0, this.uvs, num3, meshVertCounts);
				NativeArray<Vector3>.Copy(this.cuttableGameObject.cuttableMeshList[j].meshFilter.mesh.normals, 0, this.normals, num3, meshVertCounts);
				num4 += meshTriCounts;
				num3 += meshVertCounts;
			}
			if (!this.fullCutMode)
			{
				List<WeaponEdgeSection> weaponEdgeSections = this.weapon.GetWeaponEdgeSections();
				this.bladeSectionStart = new NativeArray<BladeSectionJobItem>(weaponEdgeSections.Count, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				this.bladeSectionCurrent = new NativeArray<BladeSectionJobItem>(weaponEdgeSections.Count, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				this.bladeSectionInfos = new NativeArray<BladeSectionJobInfoItem>(weaponEdgeSections.Count, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				this.UpdateCurrentBladeSection(true);
				return;
			}
		}
		else
		{
			this.disabledFully = true;
		}
	}

	// Token: 0x060002CD RID: 717 RVA: 0x0000E288 File Offset: 0x0000C488
	private void FillCuttableSections()
	{
		this.cuttableSections = new NativeArray<CuttableJobSection>(this.cuttableGameObject.cuttableSections.Count, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		for (int i = 0; i < this.cuttableGameObject.cuttableSections.Count; i++)
		{
			CuttableJobSection value = new CuttableJobSection
			{
				isCut = false
			};
			if (this.cuttableGameObject.cuttableSections[i].joint != null)
			{
				value.position = this.cuttableGameObject.cuttableSections[i].joint.anchor;
			}
			else if (this.cuttableGameObject.cuttableSections[i].gameObjectTransform != null)
			{
				value.position = this.cuttableGameObject.cuttableSections[i].gameObjectTransform.localPosition;
			}
			else if (this.cuttableGameObject.cuttableSections[i].parentSection)
			{
				value.originalPosition = this.cuttableGameObject.cuttableSections[i].position;
				value.localToWorldMatrix = this.cuttableGameObject.parentCuttableGameObject.transform.localToWorldMatrix;
				value.isParent = true;
			}
			this.cuttableSections[i] = value;
		}
	}

	// Token: 0x060002CE RID: 718 RVA: 0x0000E3D4 File Offset: 0x0000C5D4
	public void ScheduleCheckCutJob()
	{
		if (this.queuedForRemoval && this.queuedForRemovalLoops > 1)
		{
			return;
		}
		if (this.fullCutQueued || this.disabledFully)
		{
			return;
		}
		this.checkCutJobRunning = true;
		this.checkCutJobHandle = this.checkCutJob.Schedule(default(JobHandle));
	}

	// Token: 0x060002CF RID: 719 RVA: 0x0000E428 File Offset: 0x0000C628
	public void CreateDoCutJob(Plane plane, bool schedule = true)
	{
		if (this.disabledFully)
		{
			return;
		}
		this.HandleCutItemReset();
		this.fullCutQueued = true;
		this.downTris = new NativeList<int>(Allocator.TempJob);
		this.downVertices = new NativeList<Vector3>(Allocator.TempJob);
		this.downUvs = new NativeList<Vector2>(Allocator.TempJob);
		this.downNormals = new NativeList<Vector3>(Allocator.TempJob);
		this.upTris = new NativeList<int>(Allocator.TempJob);
		this.upVertices = new NativeList<Vector3>(Allocator.TempJob);
		this.upUvs = new NativeList<Vector2>(Allocator.TempJob);
		this.upNormals = new NativeList<Vector3>(Allocator.TempJob);
		this.doCutJobOutValues = new NativeArray<DoCutJobOutValues>(1, Allocator.TempJob, NativeArrayOptions.ClearMemory);
		this.fullCutPlane = plane;
		this.doCutJob = new DoCutJob(this.cuttableSections, this.tris, this.vertices, this.cuttableMeshJobItems, this.uvs, this.normals, this.downTris, this.downVertices, this.downUvs, this.downNormals, this.upTris, this.upVertices, this.upUvs, this.upNormals, this.cuttableGameObject.gameObject.transform.localToWorldMatrix, this.cuttableGameObject.gameObject.transform.worldToLocalMatrix, plane, this.doCutJobOutValues, this.cuttableGameObject.bodyPart);
		if (schedule)
		{
			this.ScheduleDoCutJob();
		}
	}

	// Token: 0x060002D0 RID: 720 RVA: 0x0000E58C File Offset: 0x0000C78C
	public void ScheduleDoCutJob()
	{
		this.doCutJobRunning = true;
		this.doCutJobHandle = this.doCutJob.Schedule(default(JobHandle));
	}

	// Token: 0x060002D1 RID: 721 RVA: 0x0000E5BA File Offset: 0x0000C7BA
	public void ExecuteDoCutJobNoWait()
	{
		this.doCutJobRunning = true;
		this.doCutJob.Execute();
		this.HandleDoCutJob();
	}

	// Token: 0x060002D2 RID: 722 RVA: 0x0000E5D4 File Offset: 0x0000C7D4
	public void HandleDoCutJob()
	{
		if (this.doCutJobRunning)
		{
			this.doCutJobHandle.Complete();
			if (this.needsToBeReset)
			{
				this.HandleCutItemReset();
				return;
			}
			this.doCutJobRunning = false;
			if (this.disabledFully)
			{
				return;
			}
			if (!this.cuttableGameObject.cutDone)
			{
				this.newCuttableGameObject = CutManager.singleton.GetCuttableGameObjectFromPool(this.cuttableGameObject.pendingCuttableObjectNetID);
				if (this.newCuttableGameObject == null)
				{
					this.doCutJobRunning = true;
					return;
				}
				this.cuttableGameObject.DisableCutItems(true, this.doCutJobOutValues[0].horizontalCut);
				this.cuttableGameObject.cutDone = true;
				this.newCuttableGameObject.cutDone = true;
				CuttableMesh newCuttableMeshObject = CutManager.singleton.GetNewCuttableMeshObject();
				newCuttableMeshObject.meshFilter.mesh.SetVertices(this.downVertices.ToArray());
				newCuttableMeshObject.meshFilter.mesh.SetTriangles(this.downTris.ToArray(), 0);
				newCuttableMeshObject.meshFilter.mesh.SetUVs(0, this.downUvs.ToArray());
				newCuttableMeshObject.meshFilter.mesh.SetNormals(this.downNormals.ToArray());
				newCuttableMeshObject.meshFilter.mesh.RecalculateTangents();
				newCuttableMeshObject.meshFilter.gameObject.transform.SetParent(this.cuttableGameObject.transform);
				newCuttableMeshObject.meshFilter.gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
				newCuttableMeshObject.meshFilter.gameObject.transform.localRotation = default(Quaternion);
				newCuttableMeshObject.renderer.sharedMaterial = this.cuttableGameObject.cuttableMeshList[0].renderer.sharedMaterial;
				this.newCuttableGameObject.playerHealth = this.cuttableGameObject.playerHealth;
				this.PopulateCuttableGameObjectsToIgnore(this.newCuttableGameObject);
				this.SetNewCuttableObjectMeshData();
				if (!this.cuttableGameObject.doNotUpdatePositionOnFullCut)
				{
					this.newCuttableGameObject.transform.position = this.cuttableGameObject.transform.position;
					this.newCuttableGameObject.transform.rotation = this.cuttableGameObject.transform.rotation;
				}
				this.newCuttableGameObject.cuttableRigidbody.mass = this.cuttableGameObject.cuttableRigidbody.mass;
				this.newCuttableGameObject.cuttableRigidbody.angularDrag = this.cuttableGameObject.cuttableRigidbody.angularDrag;
				this.newCuttableGameObject.cuttableRigidbody.drag = this.cuttableGameObject.cuttableRigidbody.drag;
				this.HandleNewColliders(this.newCuttableGameObject, newCuttableMeshObject);
				this.DisableOldMeshes();
				this.cuttableGameObject.cuttableMeshList.Clear();
				this.cuttableGameObject.cuttableMeshList.Add(newCuttableMeshObject);
				this.HandleCuttableSections(this.newCuttableGameObject);
				this.HandleObjectsToDisableColliders();
				this.UpdateParentCuttableMeshes(newCuttableMeshObject);
				this.HandleServerMultiplayer(this.newCuttableGameObject);
				this.HandleClientMultiplayer(this.newCuttableGameObject);
				this.HandleReplay(this.newCuttableGameObject);
			}
			this.fullCutHandled = true;
			if (!this.replayCut)
			{
				this.DisposeDoCutJobNativeArrays();
			}
		}
	}

	// Token: 0x060002D3 RID: 723 RVA: 0x0000E900 File Offset: 0x0000CB00
	private void DisableOldMeshes()
	{
		for (int i = 0; i < this.cuttableGameObject.cuttableMeshList.Count; i++)
		{
			CuttableMesh cuttableMesh = this.cuttableGameObject.cuttableMeshList[i];
			if (this.CuttableMeshWasCut(cuttableMesh))
			{
				this.cuttableGameObject.cuttableMeshList[i].renderer.enabled = false;
			}
		}
	}

	// Token: 0x060002D4 RID: 724 RVA: 0x0000E95F File Offset: 0x0000CB5F
	public bool CuttableMeshWasCut(CuttableMesh cuttableMesh)
	{
		return cuttableMesh.cuttableSectionIndex < 0 || this.cuttableSections[cuttableMesh.cuttableSectionIndex].isCut;
	}

	// Token: 0x060002D5 RID: 725 RVA: 0x0000E988 File Offset: 0x0000CB88
	private void SetNewCuttableObjectMeshData()
	{
		this.newCuttableGameObject.cuttableMeshList[0].meshFilter.mesh.SetVertices(this.upVertices.ToArray());
		this.newCuttableGameObject.cuttableMeshList[0].meshFilter.mesh.SetTriangles(this.upTris.ToArray(), 0);
		this.newCuttableGameObject.cuttableMeshList[0].meshFilter.mesh.SetUVs(0, this.upUvs.ToArray());
		this.newCuttableGameObject.cuttableMeshList[0].meshFilter.mesh.SetNormals(this.upNormals.ToArray());
		this.newCuttableGameObject.cuttableMeshList[0].meshFilter.mesh.RecalculateTangents();
		this.newCuttableGameObject.cuttableMeshList[0].renderer.sharedMaterial = this.cuttableGameObject.cuttableMeshList[0].renderer.sharedMaterial;
	}

	// Token: 0x060002D6 RID: 726 RVA: 0x0000EA9C File Offset: 0x0000CC9C
	private void HandleObjectsToDisableColliders()
	{
		if (NetworkClient.active && !NetworkServer.active)
		{
			return;
		}
		if (this.replayCut)
		{
			return;
		}
		for (int i = 0; i < this.cuttableGameObject.objectsToDisable.Count; i++)
		{
			this.cuttableGameObject.objectsToDisable[i].gameObject.layer = 9;
		}
		this.cuttableGameObject.objectsToDisable.Clear();
	}

	// Token: 0x060002D7 RID: 727 RVA: 0x0000EB0C File Offset: 0x0000CD0C
	private void HandleNewColliders(CuttableGameObject newCuttableGameObject, CuttableMesh cuttableMesh)
	{
		if (!CutManager.cutManagerActive)
		{
			return;
		}
		if (this.upVertices.Length > 0)
		{
			MeshCollider meshCollider = newCuttableGameObject.gameObject.AddComponent<MeshCollider>();
			meshCollider.cookingOptions = MeshColliderCookingOptions.None;
			meshCollider.convex = true;
			meshCollider.sharedMesh = newCuttableGameObject.cuttableMeshList[0].meshFilter.mesh;
			this.HandleColliderIgnoring(meshCollider);
			newCuttableGameObject.localCollidersForOthersToIgnore.Add(meshCollider);
		}
		else
		{
			BoxCollider boxCollider = newCuttableGameObject.gameObject.AddComponent<BoxCollider>();
			boxCollider.size = new Vector3(0.015f, 0.015f, 0.015f);
			this.HandleColliderIgnoring(boxCollider);
			newCuttableGameObject.localCollidersForOthersToIgnore.Add(boxCollider);
		}
		if (this.downVertices.Length > 0)
		{
			MeshCollider meshCollider2 = cuttableMesh.renderer.gameObject.AddComponent<MeshCollider>();
			meshCollider2.cookingOptions = MeshColliderCookingOptions.None;
			meshCollider2.convex = true;
			meshCollider2.sharedMesh = cuttableMesh.meshFilter.mesh;
			this.HandleColliderIgnoring(meshCollider2);
			this.cuttableGameObject.localCollidersForOthersToIgnore.Add(meshCollider2);
			this.downObjectNewCollider = meshCollider2;
			return;
		}
		BoxCollider boxCollider2 = cuttableMesh.renderer.gameObject.AddComponent<BoxCollider>();
		boxCollider2.size = new Vector3(0.015f, 0.015f, 0.015f);
		this.HandleColliderIgnoring(boxCollider2);
		this.cuttableGameObject.localCollidersForOthersToIgnore.Add(boxCollider2);
		this.downObjectNewCollider = boxCollider2;
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x0000EC5C File Offset: 0x0000CE5C
	private void UpdateParentCuttableMeshes(CuttableMesh cuttableMesh)
	{
		if (this.cuttableGameObject.parentCuttableGameObject != null)
		{
			CuttableMesh copy = cuttableMesh.GetCopy();
			copy.SetCuttableSectionIndex(this.cuttableGameObject.parentCuttableGameObject.cuttableSections);
			this.cuttableGameObject.parentCuttableGameObject.cuttableMeshList.Add(copy);
			this.cuttableGameObject.parentCuttableGameObject.UpdateCuttableMeshes();
		}
	}

	// Token: 0x060002D9 RID: 729 RVA: 0x0000ECC0 File Offset: 0x0000CEC0
	private void PopulateCuttableGameObjectsToIgnore(CuttableGameObject newCuttableGameObject)
	{
		for (int i = 0; i < this.cuttableGameObject.cuttableGameObjectsToIgnoreCollisions.Count; i++)
		{
			newCuttableGameObject.cuttableGameObjectsToIgnoreCollisions.Add(this.cuttableGameObject.cuttableGameObjectsToIgnoreCollisions[i]);
			this.cuttableGameObject.cuttableGameObjectsToIgnoreCollisions[i].cuttableGameObjectsToIgnoreCollisions.Add(newCuttableGameObject);
		}
	}

	// Token: 0x060002DA RID: 730 RVA: 0x0000ED20 File Offset: 0x0000CF20
	private void HandleColliderIgnoring(Collider newCollider)
	{
		for (int i = 0; i < this.cuttableGameObject.cuttableGameObjectsToIgnoreCollisions.Count; i++)
		{
			CuttableGameObject cuttableGameObject = this.cuttableGameObject.cuttableGameObjectsToIgnoreCollisions[i];
			for (int j = 0; j < cuttableGameObject.localCollidersForOthersToIgnore.Count; j++)
			{
				if (cuttableGameObject.localCollidersForOthersToIgnore[j] != null)
				{
					Physics.IgnoreCollision(newCollider, cuttableGameObject.localCollidersForOthersToIgnore[j], true);
				}
			}
		}
	}

	// Token: 0x060002DB RID: 731 RVA: 0x0000ED98 File Offset: 0x0000CF98
	private void HandleCuttableSections(CuttableGameObject newCuttableGameObject)
	{
		if (this.cuttableGameObject.cuttableSections.Count > 0)
		{
			for (int i = this.cuttableGameObject.cuttableSections.Count - 1; i > -1; i--)
			{
				if (this.cuttableSections[i].isCut)
				{
					CuttableSection cuttableSection = this.cuttableGameObject.cuttableSections[i];
					if (cuttableSection.joint != null)
					{
						if (this.replayCut)
						{
							cuttableSection.gameObjectTransform = cuttableSection.joint.connectedBody.transform;
							cuttableSection.isEquipment = true;
						}
						else
						{
							this.MoveJointToNewGameObject(cuttableSection.joint, newCuttableGameObject.gameObject);
						}
					}
					if (cuttableSection.hand != null)
					{
						cuttableSection.hand.SetHandState(HandState.NoHold);
					}
					if (cuttableSection.gameObjectTransform != null && cuttableSection.isEquipment)
					{
						this.MoveGameObjectToNewGameObject(cuttableSection.gameObjectTransform, newCuttableGameObject);
					}
					if (cuttableSection.cuttableGameObject != null)
					{
						cuttableSection.cuttableGameObject.parentCuttableGameObject = newCuttableGameObject;
						cuttableSection.cuttableGameObject.UpdateCuttableMeshes();
						this.DisableNewCuttableGameObjectChildWithBottomPart(cuttableSection.cuttableGameObject);
					}
					if (cuttableSection.artery != null && !this.replayCut)
					{
						cuttableSection.artery.Destory(null, true);
						cuttableSection.artery.TryToSetEffectPosition(this.cuttableGameObject.gameObject, this.doCutJobOutValues[0].cutCenterPosition, this.doCutJobOutValues[0].cutDirection, this.cuttableGameObject.bodyPart);
					}
					if (cuttableSection.instantKill && this.cuttableGameObject.playerHealth != null)
					{
						this.cuttableGameObject.playerHealth.Die(cuttableSection.deathReason);
					}
					this.DisableConfigurableJointScripts(cuttableSection);
					if (!this.replayCut)
					{
						this.cuttableGameObject.cuttableSections.RemoveAt(i);
					}
				}
			}
		}
	}

	// Token: 0x060002DC RID: 732 RVA: 0x0000EF7C File Offset: 0x0000D17C
	private void DisableNewCuttableGameObjectChildWithBottomPart(CuttableGameObject childCuttableGameObject)
	{
		if (this.downObjectNewCollider != null && childCuttableGameObject != null && childCuttableGameObject.localCollidersToIgnoreWhenChildOfCutSection != null)
		{
			for (int i = 0; i < childCuttableGameObject.localCollidersToIgnoreWhenChildOfCutSection.Count; i++)
			{
				Physics.IgnoreCollision(this.downObjectNewCollider, childCuttableGameObject.localCollidersToIgnoreWhenChildOfCutSection[i], true);
			}
		}
	}

	// Token: 0x060002DD RID: 733 RVA: 0x0000EFD8 File Offset: 0x0000D1D8
	private void DisableConfigurableJointScripts(CuttableSection cuttableSection)
	{
		if (cuttableSection.configurableJointScripts != null)
		{
			for (int i = 0; i < cuttableSection.configurableJointScripts.Count; i++)
			{
				cuttableSection.configurableJointScripts[i].DisableConfigurableJointScript();
			}
		}
	}

	// Token: 0x060002DE RID: 734 RVA: 0x0000F014 File Offset: 0x0000D214
	private void MoveGameObjectToNewGameObject(Transform originalTransform, CuttableGameObject newCuttableGameObject)
	{
		Vector3 localPosition = originalTransform.localPosition;
		Quaternion localRotation = originalTransform.localRotation;
		originalTransform.SetParent(newCuttableGameObject.transform);
		originalTransform.localRotation = localRotation;
		originalTransform.localPosition = localPosition;
	}

	// Token: 0x060002DF RID: 735 RVA: 0x0000F04C File Offset: 0x0000D24C
	private ConfigurableJoint MoveJointToNewGameObject(ConfigurableJoint originalJoint, GameObject newGameObject)
	{
		ConfigurableJoint configurableJoint = newGameObject.AddComponent<ConfigurableJoint>();
		originalJoint.connectedBody.transform.SetParent(newGameObject.transform);
		configurableJoint.connectedBody = originalJoint.connectedBody;
		configurableJoint.xMotion = originalJoint.xMotion;
		configurableJoint.yMotion = originalJoint.yMotion;
		configurableJoint.zMotion = originalJoint.zMotion;
		configurableJoint.angularXMotion = originalJoint.angularXMotion;
		configurableJoint.angularYMotion = originalJoint.angularYMotion;
		configurableJoint.angularZMotion = originalJoint.angularZMotion;
		configurableJoint.anchor = originalJoint.anchor;
		configurableJoint.autoConfigureConnectedAnchor = false;
		configurableJoint.connectedAnchor = originalJoint.connectedAnchor;
		configurableJoint.axis = originalJoint.axis;
		configurableJoint.secondaryAxis = originalJoint.secondaryAxis;
		configurableJoint.linearLimit = originalJoint.linearLimit;
		configurableJoint.angularXLimitSpring = originalJoint.angularXLimitSpring;
		configurableJoint.angularYZLimitSpring = originalJoint.angularYZLimitSpring;
		configurableJoint.lowAngularXLimit = originalJoint.lowAngularXLimit;
		configurableJoint.highAngularXLimit = originalJoint.highAngularXLimit;
		configurableJoint.angularYLimit = originalJoint.angularYLimit;
		configurableJoint.angularZLimit = originalJoint.angularZLimit;
		configurableJoint.targetPosition = originalJoint.targetPosition;
		configurableJoint.targetVelocity = originalJoint.targetVelocity;
		configurableJoint.xDrive = originalJoint.xDrive;
		configurableJoint.yDrive = originalJoint.yDrive;
		configurableJoint.zDrive = originalJoint.zDrive;
		configurableJoint.targetRotation = originalJoint.targetRotation;
		configurableJoint.targetAngularVelocity = originalJoint.targetAngularVelocity;
		configurableJoint.rotationDriveMode = originalJoint.rotationDriveMode;
		configurableJoint.slerpDrive = originalJoint.slerpDrive;
		configurableJoint.projectionMode = originalJoint.projectionMode;
		configurableJoint.projectionDistance = originalJoint.projectionDistance;
		configurableJoint.projectionAngle = originalJoint.projectionAngle;
		configurableJoint.configuredInWorldSpace = originalJoint.configuredInWorldSpace;
		configurableJoint.swapBodies = originalJoint.swapBodies;
		configurableJoint.angularXDrive = originalJoint.angularXDrive;
		configurableJoint.angularYZDrive = originalJoint.angularYZDrive;
		if (this.cuttableGameObject.doNotUpdatePositionOnFullCut)
		{
			configurableJoint.connectedBody.gameObject.transform.localPosition = configurableJoint.anchor;
		}
		UnityEngine.Object.Destroy(originalJoint);
		return configurableJoint;
	}

	// Token: 0x060002E0 RID: 736 RVA: 0x0000F244 File Offset: 0x0000D444
	private void DisposeDoCutJobNativeArrays()
	{
		if (this.downTris.IsCreated)
		{
			this.downTris.Dispose();
		}
		if (this.downVertices.IsCreated)
		{
			this.downVertices.Dispose();
		}
		if (this.downUvs.IsCreated)
		{
			this.downUvs.Dispose();
		}
		if (this.downNormals.IsCreated)
		{
			this.downNormals.Dispose();
		}
		if (this.upTris.IsCreated)
		{
			this.upTris.Dispose();
		}
		if (this.upVertices.IsCreated)
		{
			this.upVertices.Dispose();
		}
		if (this.upUvs.IsCreated)
		{
			this.upUvs.Dispose();
		}
		if (this.upNormals.IsCreated)
		{
			this.upNormals.Dispose();
		}
		if (this.doCutJobOutValues.IsCreated)
		{
			this.doCutJobOutValues.Dispose();
		}
	}

	// Token: 0x060002E1 RID: 737 RVA: 0x0000F329 File Offset: 0x0000D529
	private void HandleServerMultiplayer(CuttableGameObject newCuttableGameObject)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		if (this.cuttableGameObject.cuttableMultiplayerHandler == null)
		{
			return;
		}
		this.cuttableGameObject.ServerInformClientsToCut(this.fullCutPlane, newCuttableGameObject);
	}

	// Token: 0x060002E2 RID: 738 RVA: 0x0000F359 File Offset: 0x0000D559
	private void HandleClientMultiplayer(CuttableGameObject newCuttableGameObject)
	{
		if (NetworkServer.active || !NetworkClient.active || this.cuttableGameObject.doNotUpdatePositionOnFullCut)
		{
			return;
		}
		if (newCuttableGameObject.multiplayerTransform != null)
		{
			newCuttableGameObject.multiplayerTransform.ResetPositionInterpolation();
		}
	}

	// Token: 0x060002E3 RID: 739 RVA: 0x0000F390 File Offset: 0x0000D590
	private void HandleReplay(CuttableGameObject newCuttableGameObject)
	{
		if (ReplayManager.singleton != null && this.cuttableGameObject.playerHealth != null)
		{
			ReplayManager.singleton.RecordCut((int)this.cuttableGameObject.bodyPart, this.cuttableGameObject.playerHealth.gameObject, this.fullCutPlane, newCuttableGameObject);
		}
	}

	// Token: 0x060002E4 RID: 740 RVA: 0x0000F3EC File Offset: 0x0000D5EC
	public void DoFullCutNoWait()
	{
		this.replayCut = true;
		this.fullCutMode = true;
		this.FillNativeArrays();
		this.CreateDoCutJob(this.fullCutPlane, false);
		this.ExecuteDoCutJobNoWait();
		if (this.newCuttableGameObject != null)
		{
			this.newCuttableGameObject.cuttableRigidbody.isKinematic = true;
			this.newCuttableGameObject.cuttableRigidbody.interpolation = RigidbodyInterpolation.None;
		}
	}

	// Token: 0x060002E5 RID: 741 RVA: 0x0000F450 File Offset: 0x0000D650
	public void UndoCuttableSections(CuttableJobSection[] cuttableJobSections)
	{
		if (this.cuttableGameObject.cuttableSections.Count > 0)
		{
			for (int i = this.cuttableGameObject.cuttableSections.Count - 1; i > -1; i--)
			{
				if (cuttableJobSections[i].isCut)
				{
					CuttableSection cuttableSection = this.cuttableGameObject.cuttableSections[i];
					if (cuttableSection.gameObjectTransform != null && cuttableSection.isEquipment)
					{
						this.MoveGameObjectToNewGameObject(cuttableSection.gameObjectTransform, this.cuttableGameObject);
					}
				}
			}
		}
	}

	// Token: 0x060002E6 RID: 742 RVA: 0x0000F4D8 File Offset: 0x0000D6D8
	public void ResetMaterials()
	{
		if (this.cuttableGameObject != null && this.cuttableGameObject.cuttableMeshList != null)
		{
			foreach (CuttableMesh cuttableMesh in this.cuttableGameObject.cuttableMeshList)
			{
				cuttableMesh.renderer.sharedMaterial = this.cuttableGameObject.playerHealth.shareMaterialRenderers[0].sharedMaterial;
			}
		}
		if (this.newCuttableGameObject != null && this.newCuttableGameObject.cuttableMeshList != null)
		{
			foreach (CuttableMesh cuttableMesh2 in this.newCuttableGameObject.cuttableMeshList)
			{
				cuttableMesh2.renderer.sharedMaterial = this.cuttableGameObject.playerHealth.shareMaterialRenderers[0].sharedMaterial;
			}
		}
	}

	// Token: 0x060002E7 RID: 743 RVA: 0x0000F5E8 File Offset: 0x0000D7E8
	public void RedoCuttableSections(CuttableJobSection[] cuttableJobSections)
	{
		if (this.cuttableGameObject.cuttableSections.Count > 0)
		{
			for (int i = this.cuttableGameObject.cuttableSections.Count - 1; i > -1; i--)
			{
				if (cuttableJobSections[i].isCut)
				{
					CuttableSection cuttableSection = this.cuttableGameObject.cuttableSections[i];
					if (cuttableSection.gameObjectTransform != null && cuttableSection.isEquipment)
					{
						this.MoveGameObjectToNewGameObject(cuttableSection.gameObjectTransform, this.newCuttableGameObject);
					}
				}
			}
		}
	}

	// Token: 0x040001B6 RID: 438
	public CuttableGameObject cuttableGameObject;

	// Token: 0x040001B7 RID: 439
	public CuttableGameObject newCuttableGameObject;

	// Token: 0x040001B8 RID: 440
	public Weapon weapon;

	// Token: 0x040001B9 RID: 441
	public Vector3 weaponPositionStart;

	// Token: 0x040001BA RID: 442
	public Quaternion weaponRotationStart;

	// Token: 0x040001BB RID: 443
	public NativeArray<CuttableJobSection> cuttableSections;

	// Token: 0x040001BC RID: 444
	public NativeArray<CuttableCollider> cuttableColliders;

	// Token: 0x040001BD RID: 445
	public NativeArray<CheckCutJobOutValues> checkCutResult;

	// Token: 0x040001BE RID: 446
	public NativeArray<int> tris;

	// Token: 0x040001BF RID: 447
	public NativeArray<Vector3> vertices;

	// Token: 0x040001C0 RID: 448
	public NativeArray<Vector2> uvs;

	// Token: 0x040001C1 RID: 449
	public NativeArray<Vector3> normals;

	// Token: 0x040001C2 RID: 450
	public NativeArray<CuttableMeshJobItem> cuttableMeshJobItems;

	// Token: 0x040001C3 RID: 451
	public NativeArray<BladeSectionJobItem> bladeSectionStart;

	// Token: 0x040001C4 RID: 452
	public NativeArray<BladeSectionJobItem> bladeSectionCurrent;

	// Token: 0x040001C5 RID: 453
	public NativeArray<BladeSectionJobInfoItem> bladeSectionInfos;

	// Token: 0x040001C6 RID: 454
	public JobHandle checkCutJobHandle;

	// Token: 0x040001C7 RID: 455
	public CheckCutJob checkCutJob;

	// Token: 0x040001C8 RID: 456
	public int colliderCount;

	// Token: 0x040001C9 RID: 457
	public bool queuedForRemoval;

	// Token: 0x040001CA RID: 458
	public int queuedForRemovalLoops;

	// Token: 0x040001CB RID: 459
	public bool fullCutQueued;

	// Token: 0x040001CC RID: 460
	public bool disabledFully;

	// Token: 0x040001CD RID: 461
	public bool needsToBeReset;

	// Token: 0x040001CE RID: 462
	public bool checkCutJobRunning;

	// Token: 0x040001CF RID: 463
	public JobHandle doCutJobHandle;

	// Token: 0x040001D0 RID: 464
	public DoCutJob doCutJob;

	// Token: 0x040001D1 RID: 465
	private NativeList<int> downTris;

	// Token: 0x040001D2 RID: 466
	private NativeList<Vector3> downVertices;

	// Token: 0x040001D3 RID: 467
	private NativeList<Vector2> downUvs;

	// Token: 0x040001D4 RID: 468
	private NativeList<Vector3> downNormals;

	// Token: 0x040001D5 RID: 469
	private NativeList<int> upTris;

	// Token: 0x040001D6 RID: 470
	private NativeList<Vector3> upVertices;

	// Token: 0x040001D7 RID: 471
	private NativeList<Vector2> upUvs;

	// Token: 0x040001D8 RID: 472
	private NativeList<Vector3> upNormals;

	// Token: 0x040001D9 RID: 473
	public NativeArray<DoCutJobOutValues> doCutJobOutValues;

	// Token: 0x040001DA RID: 474
	public bool doCutJobRunning;

	// Token: 0x040001DB RID: 475
	public bool fullCutHandled;

	// Token: 0x040001DC RID: 476
	private Collider downObjectNewCollider;

	// Token: 0x040001DD RID: 477
	public Plane fullCutPlane;

	// Token: 0x040001DE RID: 478
	public bool fullCutMode;

	// Token: 0x040001DF RID: 479
	public bool replayCut;
}
