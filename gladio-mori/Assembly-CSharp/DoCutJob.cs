using System;
using MoveClasses;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x02000072 RID: 114
[BurstCompile]
public struct DoCutJob : IJob
{
	// Token: 0x06000320 RID: 800 RVA: 0x0001029C File Offset: 0x0000E49C
	public DoCutJob(NativeArray<CuttableJobSection> _cuttableSections, NativeArray<int> _tris, NativeArray<Vector3> _vertices, NativeArray<CuttableMeshJobItem> _cuttableMeshJobItems, NativeArray<Vector2> _uvs, NativeArray<Vector3> _normals, NativeList<int> _downTris, NativeList<Vector3> _downVertices, NativeList<Vector2> _downUvs, NativeList<Vector3> _downNormals, NativeList<int> _upTris, NativeList<Vector3> _upVertices, NativeList<Vector2> _upUvs, NativeList<Vector3> _upNormals, Matrix4x4 _localToWorld, Matrix4x4 _worldToLocal, Plane _cutPlane, NativeArray<DoCutJobOutValues> _doCutJobOutValues, JointType _bodyPart)
	{
		this.cuttableSections = _cuttableSections;
		this.tris = _tris;
		this.vertices = _vertices;
		this.uvs = _uvs;
		this.normals = _normals;
		this.cuttableMeshJobItems = _cuttableMeshJobItems;
		this.localToWorldMatrix = _localToWorld;
		this.worldToLocalMatrix = _worldToLocal;
		this.cutPlane = _cutPlane;
		this.right = default(Vector3);
		this.forward = default(Vector3);
		this.downTris = _downTris;
		this.downVertices = _downVertices;
		this.downUvs = _downUvs;
		this.downNormals = _downNormals;
		this.upTris = _upTris;
		this.upVertices = _upVertices;
		this.upUvs = _upUvs;
		this.upNormals = _upNormals;
		this.doCutJobOutValues = _doCutJobOutValues;
		this.bodyPart = _bodyPart;
	}

	// Token: 0x06000321 RID: 801 RVA: 0x00010358 File Offset: 0x0000E558
	public void Execute()
	{
		this.UpdatePlaneForCut();
		this.CutSections();
		NativeList<int> nativeList = new NativeList<int>(Allocator.Temp);
		NativeList<int> nativeList2 = new NativeList<int>(Allocator.Temp);
		NativeList<Vector3> tranformedVerts = new NativeList<Vector3>(this.vertices.Length + 300, Allocator.Temp);
		NativeList<Vector2> transformedUVs = new NativeList<Vector2>(this.uvs.Length + 300, Allocator.Temp);
		transformedUVs.AddRange(this.uvs);
		NativeList<Vector3> tranformedNormals = new NativeList<Vector3>(this.normals.Length + 300, Allocator.Temp);
		JobHelpers.GetTransformedVerts(tranformedVerts, this.worldToLocalMatrix, this.cuttableMeshJobItems, this.vertices);
		JobHelpers.GetTransformedNormals(tranformedNormals, this.worldToLocalMatrix, this.cuttableMeshJobItems, this.normals);
		this.CutMesh(nativeList, nativeList2, tranformedVerts, transformedUVs, tranformedNormals);
		this.CreateMesh(tranformedVerts, transformedUVs, tranformedNormals, nativeList, this.downTris, this.downVertices, this.downUvs, this.downNormals);
		this.CreateMesh(tranformedVerts, transformedUVs, tranformedNormals, nativeList2, this.upTris, this.upVertices, this.upUvs, this.upNormals);
		this.SetOutCutDirection();
		this.SetHorizontalCut();
		nativeList.Dispose();
		nativeList2.Dispose();
		tranformedVerts.Dispose();
		transformedUVs.Dispose();
		tranformedNormals.Dispose();
	}

	// Token: 0x06000322 RID: 802 RVA: 0x000104A4 File Offset: 0x0000E6A4
	private void UpdatePlaneForCut()
	{
		this.cutPlane = JobHelpers.CheckPlaneDirection(this.cutPlane);
		ConversionPlane conversionPlane = JobHelpers.PlaneToConversionPlane(this.cutPlane);
		this.right = conversionPlane.right;
		this.forward = conversionPlane.forward;
	}

	// Token: 0x06000323 RID: 803 RVA: 0x000104E8 File Offset: 0x0000E6E8
	public Vector2 ConvertToPlane2D(Vector3 point)
	{
		Vector3 lhs = this.cutPlane.ClosestPointOnPlane(point);
		float x = Vector3.Dot(lhs, this.right);
		float y = Vector3.Dot(lhs, this.forward);
		return new Vector2(x, y);
	}

	// Token: 0x06000324 RID: 804 RVA: 0x00010524 File Offset: 0x0000E724
	public void CutSections()
	{
		if (this.cuttableSections.Length == 0)
		{
			return;
		}
		bool side = this.cutPlane.GetSide(new Vector3(0f, 0f, 0f));
		for (int i = 0; i < this.cuttableSections.Length; i++)
		{
			bool side2 = this.cutPlane.GetSide(this.cuttableSections[i].position);
			if (side != side2)
			{
				CuttableJobSection value = this.cuttableSections[i];
				value.isCut = true;
				this.cuttableSections[i] = value;
			}
		}
	}

	// Token: 0x06000325 RID: 805 RVA: 0x000105B8 File Offset: 0x0000E7B8
	public bool CuttableSectionIsCut(int cuttableSectionIndex)
	{
		return cuttableSectionIndex < 0 || this.cuttableSections[cuttableSectionIndex].isCut;
	}

	// Token: 0x06000326 RID: 806 RVA: 0x000105D4 File Offset: 0x0000E7D4
	public void CutMesh(NativeList<int> oldDownTris, NativeList<int> oldUpTris, NativeList<Vector3> tranformedVerts, NativeList<Vector2> transformedUVs, NativeList<Vector3> tranformedNormals)
	{
		int num = 0;
		int num2 = 0;
		int num3 = this.cuttableMeshJobItems[num].meshTriCounts;
		bool flag = this.CuttableSectionIsCut(this.cuttableMeshJobItems[num].cuttableSectionIndex);
		for (int i = 0; i < this.tris.Length; i += 3)
		{
			if (i == num3)
			{
				num++;
				num2 += this.cuttableMeshJobItems[num - 1].meshVertCounts;
				num3 += this.cuttableMeshJobItems[num].meshTriCounts;
				flag = this.CuttableSectionIsCut(this.cuttableMeshJobItems[num].cuttableSectionIndex);
			}
			if (flag)
			{
				int num4 = this.tris[i] + num2;
				int num5 = this.tris[i + 1] + num2;
				int num6 = this.tris[i + 2] + num2;
				bool side = this.cutPlane.GetSide(tranformedVerts[num4]);
				bool side2 = this.cutPlane.GetSide(tranformedVerts[num5]);
				bool side3 = this.cutPlane.GetSide(tranformedVerts[num6]);
				if (side == side2 && side3 == side)
				{
					if (side)
					{
						this.AddTriToList(oldUpTris, num4, num5, num6);
					}
					else
					{
						this.AddTriToList(oldDownTris, num4, num5, num6);
					}
				}
				else
				{
					this.SplitTri(tranformedVerts, transformedUVs, tranformedNormals, oldDownTris, oldUpTris, num4, num5, num6, side, side2, side3);
				}
			}
		}
		this.GenerateEndCaps(oldDownTris, oldUpTris, tranformedVerts, transformedUVs, tranformedNormals);
	}

	// Token: 0x06000327 RID: 807 RVA: 0x0001074D File Offset: 0x0000E94D
	public void AddTriToList(NativeList<int> list, int vert0, int vert1, int vert2)
	{
		list.Add(vert0);
		list.Add(vert1);
		list.Add(vert2);
	}

	// Token: 0x06000328 RID: 808 RVA: 0x0001076C File Offset: 0x0000E96C
	public void SplitTri(NativeList<Vector3> tranformedVerts, NativeList<Vector2> transformedUVs, NativeList<Vector3> tranformedNormals, NativeList<int> oldDownTris, NativeList<int> oldUpTris, int tri0, int tri1, int tri2, bool side0, bool side1, bool side2)
	{
		bool flag;
		if (side0 != side1 && side0 != side2)
		{
			flag = side0;
		}
		else if (side1 != side0 && side1 != side2)
		{
			flag = side1;
			int num = tri0;
			tri0 = tri1;
			tri1 = tri2;
			tri2 = num;
		}
		else
		{
			flag = side2;
			int num2 = tri0;
			tri0 = tri2;
			tri2 = tri1;
			tri1 = num2;
		}
		NullableJobVector3 intersectionPlaneAndLineSegment = JobHelpers.GetIntersectionPlaneAndLineSegment(this.cutPlane, tranformedVerts[tri0], tranformedVerts[tri1]);
		NullableJobVector3 intersectionPlaneAndLineSegment2 = JobHelpers.GetIntersectionPlaneAndLineSegment(this.cutPlane, tranformedVerts[tri0], tranformedVerts[tri2]);
		int num3;
		int num4;
		if (flag)
		{
			tranformedVerts.Add(intersectionPlaneAndLineSegment.vector3);
			tranformedVerts.Add(intersectionPlaneAndLineSegment2.vector3);
			num3 = tranformedVerts.Length - 2;
			num4 = tranformedVerts.Length - 1;
			this.AddUv(transformedUVs, tri0, tri1, intersectionPlaneAndLineSegment.percentage);
			this.AddUv(transformedUVs, tri0, tri2, intersectionPlaneAndLineSegment2.percentage);
			this.AddNormal(tranformedNormals, tri0, tri1, intersectionPlaneAndLineSegment.percentage);
			this.AddNormal(tranformedNormals, tri0, tri2, intersectionPlaneAndLineSegment2.percentage);
		}
		else
		{
			tranformedVerts.Add(intersectionPlaneAndLineSegment2.vector3);
			tranformedVerts.Add(intersectionPlaneAndLineSegment.vector3);
			num3 = tranformedVerts.Length - 1;
			num4 = tranformedVerts.Length - 2;
			this.AddUv(transformedUVs, tri0, tri2, intersectionPlaneAndLineSegment2.percentage);
			this.AddUv(transformedUVs, tri0, tri1, intersectionPlaneAndLineSegment.percentage);
			this.AddNormal(tranformedNormals, tri0, tri2, intersectionPlaneAndLineSegment2.percentage);
			this.AddNormal(tranformedNormals, tri0, tri1, intersectionPlaneAndLineSegment.percentage);
		}
		if (flag)
		{
			this.AddTriToList(oldUpTris, tri0, num3, num4);
			this.AddTriToList(oldDownTris, tri2, num4, tri1);
			this.AddTriToList(oldDownTris, tri1, num4, num3);
			return;
		}
		this.AddTriToList(oldDownTris, tri0, num3, num4);
		this.AddTriToList(oldUpTris, tri2, num4, tri1);
		this.AddTriToList(oldUpTris, tri1, num4, num3);
	}

	// Token: 0x06000329 RID: 809 RVA: 0x0001093C File Offset: 0x0000EB3C
	public void GenerateEndCaps(NativeList<int> oldDownTris, NativeList<int> oldUpTris, NativeList<Vector3> tranformedVerts, NativeList<Vector2> transformedUVs, NativeList<Vector3> tranformedNormals)
	{
		Vector2 vector = new Vector2(0f, 0f);
		Vector3 vector2 = default(Vector3);
		int num = 0;
		for (int i = this.vertices.Length; i < tranformedVerts.Length; i++)
		{
			vector2 += tranformedVerts[i];
			num++;
		}
		if (num > 0)
		{
			vector2 /= (float)num;
			tranformedVerts.Add(vector2);
			transformedUVs.Add(vector);
			Vector3 vector3 = this.cutPlane.normal;
			tranformedNormals.Add(vector3);
			tranformedVerts.Add(vector2);
			transformedUVs.Add(vector);
			vector3 = -this.cutPlane.normal;
			tranformedNormals.Add(vector3);
			int num2 = tranformedVerts.Length - 1;
			for (int j = this.vertices.Length; j < num2; j += 2)
			{
				vector3 = tranformedVerts[j];
				tranformedVerts.Add(vector3);
				vector3 = tranformedVerts[j + 1];
				tranformedVerts.Add(vector3);
				transformedUVs.Add(vector);
				transformedUVs.Add(vector);
				vector3 = this.cutPlane.normal;
				tranformedNormals.Add(vector3);
				vector3 = this.cutPlane.normal;
				tranformedNormals.Add(vector3);
				int num3 = tranformedVerts.Length - 2;
				int num4 = tranformedVerts.Length - 1;
				this.AddTriToList(oldDownTris, num2 - 1, num3, num4);
				vector3 = tranformedVerts[j];
				tranformedVerts.Add(vector3);
				vector3 = tranformedVerts[j + 1];
				tranformedVerts.Add(vector3);
				transformedUVs.Add(vector);
				transformedUVs.Add(vector);
				vector3 = -this.cutPlane.normal;
				tranformedNormals.Add(vector3);
				vector3 = -this.cutPlane.normal;
				tranformedNormals.Add(vector3);
				num3 = tranformedVerts.Length - 2;
				num4 = tranformedVerts.Length - 1;
				this.AddTriToList(oldUpTris, num2, num4, num3);
			}
		}
		this.SetOutCutPosition(vector2);
	}

	// Token: 0x0600032A RID: 810 RVA: 0x00010B50 File Offset: 0x0000ED50
	public void AddUv(NativeList<Vector2> transformedUVs, int vert0, int vert1, float percentage)
	{
		Vector2 a = transformedUVs[vert1] - transformedUVs[vert0];
		Vector2 vector = transformedUVs[vert0] + percentage * a;
		transformedUVs.Add(vector);
	}

	// Token: 0x0600032B RID: 811 RVA: 0x00010B94 File Offset: 0x0000ED94
	public void AddNormal(NativeList<Vector3> transformedNormals, int vert0, int vert1, float percentage)
	{
		Vector3 vector = math.lerp(transformedNormals[vert0], transformedNormals[vert1], percentage);
		transformedNormals.Add(vector);
	}

	// Token: 0x0600032C RID: 812 RVA: 0x00010BD4 File Offset: 0x0000EDD4
	public void CreateMesh(NativeList<Vector3> tranformedVerts, NativeList<Vector2> transformedUVs, NativeList<Vector3> tranformedNormals, NativeList<int> oldTris, NativeList<int> newTris, NativeList<Vector3> newVertices, NativeList<Vector2> newUvs, NativeList<Vector3> newNormals)
	{
		NativeParallelHashMap<int, int> oldIndexToNewIndex = new NativeParallelHashMap<int, int>(oldTris.Length, Allocator.Temp);
		for (int i = 0; i < oldTris.Length; i += 3)
		{
			this.AddVertex(tranformedVerts, transformedUVs, tranformedNormals, oldTris, newTris, newVertices, newUvs, newNormals, oldIndexToNewIndex, i);
			this.AddVertex(tranformedVerts, transformedUVs, tranformedNormals, oldTris, newTris, newVertices, newUvs, newNormals, oldIndexToNewIndex, i + 1);
			this.AddVertex(tranformedVerts, transformedUVs, tranformedNormals, oldTris, newTris, newVertices, newUvs, newNormals, oldIndexToNewIndex, i + 2);
		}
		if (oldIndexToNewIndex.IsCreated)
		{
			oldIndexToNewIndex.Dispose();
		}
	}

	// Token: 0x0600032D RID: 813 RVA: 0x00010C60 File Offset: 0x0000EE60
	public int AddVertex(NativeList<Vector3> tranformedVerts, NativeList<Vector2> transformedUVs, NativeList<Vector3> tranformedNormals, NativeList<int> oldTris, NativeList<int> newTris, NativeList<Vector3> newVertices, NativeList<Vector2> newUvs, NativeList<Vector3> newNormals, NativeParallelHashMap<int, int> oldIndexToNewIndex, int i)
	{
		int item = 0;
		int num;
		if (oldIndexToNewIndex.TryGetValue(oldTris[i], out num))
		{
			item = num;
		}
		else
		{
			item = newVertices.Length;
			Vector3 vector = tranformedVerts[oldTris[i]];
			newVertices.Add(vector);
			Vector2 vector2 = transformedUVs[oldTris[i]];
			newUvs.Add(vector2);
			vector = tranformedNormals[oldTris[i]];
			newNormals.Add(vector);
			oldIndexToNewIndex.Add(oldTris[i], item);
		}
		newTris.Add(item);
		return i;
	}

	// Token: 0x0600032E RID: 814 RVA: 0x00010CF8 File Offset: 0x0000EEF8
	public void SetOutCutPosition(Vector3 center)
	{
		DoCutJobOutValues value = this.doCutJobOutValues[0];
		value.cutCenterPosition = center;
		this.doCutJobOutValues[0] = value;
	}

	// Token: 0x0600032F RID: 815 RVA: 0x00010D28 File Offset: 0x0000EF28
	public void SetOutCutDirection()
	{
		DoCutJobOutValues value = this.doCutJobOutValues[0];
		value.cutDirection = this.localToWorldMatrix.inverse.transpose.MultiplyVector(this.cutPlane.normal).normalized;
		this.doCutJobOutValues[0] = value;
	}

	// Token: 0x06000330 RID: 816 RVA: 0x00010D84 File Offset: 0x0000EF84
	public void SetHorizontalCut()
	{
		if (this.bodyPart != JointType.SPINE2 && this.bodyPart != JointType.NECK)
		{
			return;
		}
		DoCutJobOutValues value = this.doCutJobOutValues[0];
		value.horizontalCut = false;
		if (this.cuttableSections.Length >= 2 && this.cuttableSections[0].isParent)
		{
			float num = math.abs(this.cutPlane.distance);
			if (this.bodyPart == JointType.SPINE2)
			{
				num = math.abs(this.cutPlane.GetDistanceToPoint(this.cuttableSections[1].position));
			}
			float num2 = 0.04f;
			if (num < num2 && math.abs(Vector3.Dot(this.cuttableSections[1].position.normalized, this.cutPlane.normal.normalized)) > 0.7f)
			{
				value.horizontalCut = true;
			}
		}
		this.doCutJobOutValues[0] = value;
	}

	// Token: 0x0400021F RID: 543
	private NativeArray<CuttableJobSection> cuttableSections;

	// Token: 0x04000220 RID: 544
	[ReadOnly]
	private NativeArray<int> tris;

	// Token: 0x04000221 RID: 545
	[ReadOnly]
	private NativeArray<Vector3> vertices;

	// Token: 0x04000222 RID: 546
	[ReadOnly]
	private NativeArray<Vector2> uvs;

	// Token: 0x04000223 RID: 547
	[ReadOnly]
	private NativeArray<Vector3> normals;

	// Token: 0x04000224 RID: 548
	[ReadOnly]
	public NativeArray<CuttableMeshJobItem> cuttableMeshJobItems;

	// Token: 0x04000225 RID: 549
	private NativeList<int> downTris;

	// Token: 0x04000226 RID: 550
	private NativeList<Vector3> downVertices;

	// Token: 0x04000227 RID: 551
	private NativeList<Vector2> downUvs;

	// Token: 0x04000228 RID: 552
	private NativeList<Vector3> downNormals;

	// Token: 0x04000229 RID: 553
	private NativeList<int> upTris;

	// Token: 0x0400022A RID: 554
	private NativeList<Vector3> upVertices;

	// Token: 0x0400022B RID: 555
	private NativeList<Vector2> upUvs;

	// Token: 0x0400022C RID: 556
	private NativeList<Vector3> upNormals;

	// Token: 0x0400022D RID: 557
	[ReadOnly]
	private Matrix4x4 localToWorldMatrix;

	// Token: 0x0400022E RID: 558
	[ReadOnly]
	private Matrix4x4 worldToLocalMatrix;

	// Token: 0x0400022F RID: 559
	[ReadOnly]
	private JointType bodyPart;

	// Token: 0x04000230 RID: 560
	public Plane cutPlane;

	// Token: 0x04000231 RID: 561
	private Vector3 right;

	// Token: 0x04000232 RID: 562
	private Vector3 forward;

	// Token: 0x04000233 RID: 563
	private NativeArray<DoCutJobOutValues> doCutJobOutValues;
}
