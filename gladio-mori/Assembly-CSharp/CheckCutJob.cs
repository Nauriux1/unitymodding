using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x02000062 RID: 98
[BurstCompile]
public struct CheckCutJob : IJob
{
	// Token: 0x060002B0 RID: 688 RVA: 0x0000CB14 File Offset: 0x0000AD14
	public CheckCutJob(NativeArray<CuttableJobSection> _cuttableSections, NativeArray<int> _tris, NativeArray<Vector3> _vertices, NativeArray<CuttableMeshJobItem> _cuttableMeshJobItems, Matrix4x4 _localToWorld, Matrix4x4 _worldToLocal, NativeArray<BladeSectionJobItem> _bladeStartSection, NativeArray<BladeSectionJobItem> _bladeCurrentSection, NativeArray<CheckCutJobOutValues> _checkCutResult, NativeArray<CuttableCollider> _cuttableColliders, NativeArray<BladeSectionJobInfoItem> _bladeSectionInfos, Matrix4x4 _parentLocalToWorldMatrix)
	{
		this.cuttableSections = _cuttableSections;
		this.cuttableColliders = _cuttableColliders;
		this.tris = _tris;
		this.vertices = _vertices;
		this.cuttableMeshJobItems = _cuttableMeshJobItems;
		this.localToWorldMatrix = _localToWorld;
		this.worldToLocalMatrix = _worldToLocal;
		this.parentLocalToWorldMatrix = _parentLocalToWorldMatrix;
		this.bladeStartSections = _bladeStartSection;
		this.bladeCurrentSections = _bladeCurrentSection;
		this.bladeSectionInfos = _bladeSectionInfos;
		this.checkCutResult = _checkCutResult;
		this.cutPlane = default(ConversionPlane);
		this.right = default(Vector3);
		this.forward = default(Vector3);
	}

	// Token: 0x060002B1 RID: 689 RVA: 0x0000CBA4 File Offset: 0x0000ADA4
	public void Execute()
	{
		this.cutPlane = this.GetPlaneForCut();
		NativeList<Vector2> meshPointsOnPlane = new NativeList<Vector2>(Allocator.Temp);
		NativeList<CutColliderSectionRing> nativeList = new NativeList<CutColliderSectionRing>(Allocator.Temp);
		NativeList<Vector3> nativeList2 = new NativeList<Vector3>(this.vertices.Length, Allocator.Temp);
		JobHelpers.UpdateCutSections(this.cuttableSections, this.worldToLocalMatrix);
		if (this.IsCuttingCuttableSection())
		{
			JobHelpers.GetColliderIntersectionPoints(meshPointsOnPlane, this.cuttableColliders, this.cutPlane, nativeList, this.checkCutResult[0].parentCut, this.parentLocalToWorldMatrix, this.worldToLocalMatrix);
			this.UpdateBladePointsOnPlane(this.cutPlane);
			this.ValidateBladeSections(nativeList);
			this.SetFullyCut(this.CheckForFullCut(meshPointsOnPlane));
			this.UpdateResultPlane();
		}
		this.UpdateCutPlanePointsAndIncrementCount();
		meshPointsOnPlane.Dispose();
		nativeList2.Dispose();
		nativeList.Dispose();
	}

	// Token: 0x060002B2 RID: 690 RVA: 0x0000CC7C File Offset: 0x0000AE7C
	public void SetFullyCut(bool cut)
	{
		CheckCutJobOutValues value = this.checkCutResult[0];
		value.fullyCut = cut;
		this.checkCutResult[0] = value;
	}

	// Token: 0x060002B3 RID: 691 RVA: 0x0000CCAC File Offset: 0x0000AEAC
	public void SetParentCut(bool cut)
	{
		CheckCutJobOutValues value = this.checkCutResult[0];
		value.parentCut = cut;
		this.checkCutResult[0] = value;
	}

	// Token: 0x060002B4 RID: 692 RVA: 0x0000CCDC File Offset: 0x0000AEDC
	public void UpdateResultPlane()
	{
		CheckCutJobOutValues value = this.checkCutResult[0];
		value.cutPlane = this.cutPlane;
		this.checkCutResult[0] = value;
	}

	// Token: 0x060002B5 RID: 693 RVA: 0x0000CD10 File Offset: 0x0000AF10
	public void UpdateCutPlanePointsAndIncrementCount()
	{
		CheckCutJobOutValues checkCutJobOutValues = this.checkCutResult[0];
		if (checkCutJobOutValues.checkCount != 0)
		{
			if (checkCutJobOutValues.checkCount == 1)
			{
				BladeSectionJobItem bladeSectionJobItem = this.bladeCurrentSections[0];
				Vector3 a = bladeSectionJobItem.bladePoints[0];
				bladeSectionJobItem = this.bladeCurrentSections[0];
				checkCutJobOutValues.cutPlaneStartPoint = (a + bladeSectionJobItem.bladePoints[1]) / 2f;
			}
			else if (checkCutJobOutValues.checkCount >= 2)
			{
				BladeSectionJobItem bladeSectionJobItem = this.bladeCurrentSections[0];
				checkCutJobOutValues.cutPlaneEndPoint0 = bladeSectionJobItem.bladePoints[0];
				bladeSectionJobItem = this.bladeCurrentSections[0];
				checkCutJobOutValues.cutPlaneEndPoint1 = bladeSectionJobItem.bladePoints[1];
			}
		}
		if (checkCutJobOutValues.checkCount < 3)
		{
			checkCutJobOutValues.checkCount++;
		}
		this.checkCutResult[0] = checkCutJobOutValues;
	}

	// Token: 0x060002B6 RID: 694 RVA: 0x0000CDF8 File Offset: 0x0000AFF8
	public void UpdateResultParentPlane()
	{
		CheckCutJobOutValues value = this.checkCutResult[0];
		value.parentCutPlane = this.parentLocalToWorldMatrix.inverse.TransformPlane(this.localToWorldMatrix.TransformPlane(this.cutPlane.plane));
		this.checkCutResult[0] = value;
	}

	// Token: 0x060002B7 RID: 695 RVA: 0x0000CE50 File Offset: 0x0000B050
	private ConversionPlane GetPlaneForCut()
	{
		Vector3 a = this.checkCutResult[0].cutPlaneStartPoint;
		Vector3 b = this.checkCutResult[0].cutPlaneEndPoint0;
		Vector3 c = this.checkCutResult[0].cutPlaneEndPoint1;
		if (this.checkCutResult[0].checkCount < 3)
		{
			BladeSectionJobItem bladeSectionJobItem = this.bladeStartSections[0];
			Vector3 a2 = bladeSectionJobItem.bladePoints[0];
			bladeSectionJobItem = this.bladeStartSections[0];
			a = (a2 + bladeSectionJobItem.bladePoints[1]) / 2f;
			bladeSectionJobItem = this.bladeCurrentSections[0];
			b = bladeSectionJobItem.bladePoints[0];
			bladeSectionJobItem = this.bladeCurrentSections[0];
			c = bladeSectionJobItem.bladePoints[1];
		}
		Plane plane = new Plane(a, b, c);
		plane = JobHelpers.CheckPlaneDirection(plane);
		this.cutPlane = JobHelpers.PlaneToConversionPlane(plane);
		return this.cutPlane;
	}

	// Token: 0x060002B8 RID: 696 RVA: 0x0000CF48 File Offset: 0x0000B148
	public void GetMeshPointsOnPlane(NativeList<Vector2> meshPointsOnPlane, NativeList<Vector3> localVertices)
	{
		int num = 0;
		int num2 = 0;
		CuttableMeshJobItem cuttableMeshJobItem = this.cuttableMeshJobItems[num];
		int num3 = cuttableMeshJobItem.meshTriCounts;
		for (int i = 0; i < this.tris.Length; i += 3)
		{
			if (i == num3)
			{
				num++;
				num2 += this.cuttableMeshJobItems[num - 1].meshVertCounts;
				cuttableMeshJobItem = this.cuttableMeshJobItems[num];
				num3 += cuttableMeshJobItem.meshTriCounts;
			}
			if (!cuttableMeshJobItem.ignoreInCheck)
			{
				int index = this.tris[i] + num2;
				int index2 = this.tris[i + 1] + num2;
				int index3 = this.tris[i + 2] + num2;
				bool flag = false;
				bool side = this.cutPlane.plane.GetSide(localVertices[index]);
				bool side2 = this.cutPlane.plane.GetSide(localVertices[index2]);
				if (side == side2)
				{
					flag = this.cutPlane.plane.GetSide(localVertices[index3]);
					if (flag == side)
					{
						goto IL_1EA;
					}
				}
				if (side != side2)
				{
					NullableJobVector3 intersectionPlaneAndLineSegment = JobHelpers.GetIntersectionPlaneAndLineSegment(this.cutPlane.plane, localVertices[index], localVertices[index2]);
					if (intersectionPlaneAndLineSegment.hasValue)
					{
						Vector2 vector = JobHelpers.ConvertToPlane2D(intersectionPlaneAndLineSegment.vector3, this.cutPlane);
						meshPointsOnPlane.Add(vector);
					}
				}
				if (side2 != flag)
				{
					NullableJobVector3 intersectionPlaneAndLineSegment2 = JobHelpers.GetIntersectionPlaneAndLineSegment(this.cutPlane.plane, localVertices[index2], localVertices[index3]);
					if (intersectionPlaneAndLineSegment2.hasValue)
					{
						Vector2 vector = JobHelpers.ConvertToPlane2D(intersectionPlaneAndLineSegment2.vector3, this.cutPlane);
						meshPointsOnPlane.Add(vector);
					}
				}
				if (flag != side)
				{
					NullableJobVector3 intersectionPlaneAndLineSegment3 = JobHelpers.GetIntersectionPlaneAndLineSegment(this.cutPlane.plane, localVertices[index3], localVertices[index]);
					if (intersectionPlaneAndLineSegment3.hasValue)
					{
						Vector2 vector = JobHelpers.ConvertToPlane2D(intersectionPlaneAndLineSegment3.vector3, this.cutPlane);
						meshPointsOnPlane.Add(vector);
					}
				}
			}
			IL_1EA:;
		}
	}

	// Token: 0x060002B9 RID: 697 RVA: 0x0000D158 File Offset: 0x0000B358
	public void UpdateBladePointsOnPlane(ConversionPlane plane)
	{
		for (int i = 0; i < this.bladeStartSections.Length; i++)
		{
			BladeSectionJobItem value = this.bladeStartSections[i];
			BladeSectionJobItem value2 = this.bladeCurrentSections[i];
			value.bladePointsOnPlane.Clear();
			value2.bladePointsOnPlane.Clear();
			for (int j = 0; j < this.bladeStartSections[i].bladePoints.Length; j++)
			{
				Vector2 vector = JobHelpers.ConvertToPlane2D(value.bladePoints[j], plane);
				Vector2 vector2 = JobHelpers.ConvertToPlane2D(value2.bladePoints[j], plane);
				value.bladePointsOnPlane.Add(vector);
				value2.bladePointsOnPlane.Add(vector2);
			}
			this.bladeStartSections[i] = value;
			this.bladeCurrentSections[i] = value2;
		}
	}

	// Token: 0x060002BA RID: 698 RVA: 0x0000D238 File Offset: 0x0000B438
	public bool IsCuttingCuttableSection()
	{
		this.SetParentCut(false);
		bool flag = false;
		bool flag2 = false;
		if (this.cuttableSections.Length == 0)
		{
			return true;
		}
		bool side = this.cutPlane.plane.GetSide(new Vector3(0f, 0f, 0f));
		for (int i = 0; i < this.cuttableSections.Length; i++)
		{
			bool side2 = this.cutPlane.plane.GetSide(this.cuttableSections[i].position);
			if (side != side2)
			{
				if (this.cuttableSections[i].isParent)
				{
					flag2 = true;
				}
				else
				{
					flag = true;
				}
			}
		}
		if (flag2 && !flag)
		{
			this.UpdateResultParentPlane();
			this.SetParentCut(true);
			flag = true;
		}
		return flag;
	}

	// Token: 0x060002BB RID: 699 RVA: 0x0000D2F4 File Offset: 0x0000B4F4
	public bool CheckForFullCut(NativeList<Vector2> meshPointsOnPlane)
	{
		if (meshPointsOnPlane.Length == 0)
		{
			return false;
		}
		bool result = true;
		Vector2 vector = new Vector2(0f, 1f);
		for (int i = 0; i < meshPointsOnPlane.Length; i++)
		{
			bool flag = false;
			for (int j = 0; j < this.bladeStartSections.Length; j++)
			{
				if (!this.bladeSectionInfos[j].invalidCut)
				{
					int num = 0;
					int num2 = 1;
					BladeSectionJobItem bladeSectionJobItem;
					for (;;)
					{
						int num3 = num2;
						bladeSectionJobItem = this.bladeStartSections[j];
						if (num3 >= bladeSectionJobItem.bladePointsOnPlane.Length)
						{
							break;
						}
						Vector2 rayOrigin = meshPointsOnPlane[i];
						Vector2 rayDirection = vector;
						bladeSectionJobItem = this.bladeStartSections[j];
						Vector2 segmentStart = bladeSectionJobItem.bladePointsOnPlane[num2 - 1];
						bladeSectionJobItem = this.bladeStartSections[j];
						if (this.IntersectRaySegment(rayOrigin, rayDirection, segmentStart, bladeSectionJobItem.bladePointsOnPlane[num2]))
						{
							num++;
						}
						Vector2 rayOrigin2 = meshPointsOnPlane[i];
						Vector2 rayDirection2 = vector;
						bladeSectionJobItem = this.bladeCurrentSections[j];
						Vector2 segmentStart2 = bladeSectionJobItem.bladePointsOnPlane[num2 - 1];
						bladeSectionJobItem = this.bladeCurrentSections[j];
						if (this.IntersectRaySegment(rayOrigin2, rayDirection2, segmentStart2, bladeSectionJobItem.bladePointsOnPlane[num2]))
						{
							num++;
						}
						num2++;
					}
					Vector2 rayOrigin3 = meshPointsOnPlane[i];
					Vector2 rayDirection3 = vector;
					bladeSectionJobItem = this.bladeStartSections[j];
					Vector2 segmentStart3 = bladeSectionJobItem.bladePointsOnPlane[0];
					bladeSectionJobItem = this.bladeCurrentSections[j];
					if (this.IntersectRaySegment(rayOrigin3, rayDirection3, segmentStart3, bladeSectionJobItem.bladePointsOnPlane[0]))
					{
						num++;
					}
					Vector2 rayOrigin4 = meshPointsOnPlane[i];
					Vector2 rayDirection4 = vector;
					bladeSectionJobItem = this.bladeStartSections[j];
					BladeSectionJobItem bladeSectionJobItem2 = this.bladeStartSections[j];
					Vector2 segmentStart4 = bladeSectionJobItem.bladePointsOnPlane[bladeSectionJobItem2.bladePointsOnPlane.Length - 1];
					bladeSectionJobItem = this.bladeCurrentSections[j];
					bladeSectionJobItem2 = this.bladeStartSections[j];
					if (this.IntersectRaySegment(rayOrigin4, rayDirection4, segmentStart4, bladeSectionJobItem.bladePointsOnPlane[bladeSectionJobItem2.bladePointsOnPlane.Length - 1]))
					{
						num++;
					}
					if (num % 2 != 0)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	// Token: 0x060002BC RID: 700 RVA: 0x0000D534 File Offset: 0x0000B734
	public void ValidateBladeSections(NativeList<CutColliderSectionRing> cutColliderSectionRings)
	{
		new Vector2(0f, 1f);
		for (int i = 0; i < this.bladeSectionInfos.Length; i++)
		{
			BladeSectionJobInfoItem value = this.bladeSectionInfos[i];
			BladeSectionJobItem bladeSectionJobItem = this.bladeCurrentSections[i];
			BladeSectionJobItem bladeSectionJobItem2 = this.bladeStartSections[i];
			Vector2 vector = bladeSectionJobItem.bladePointsOnPlane[0];
			Vector2 vector2 = bladeSectionJobItem.bladePointsOnPlane[bladeSectionJobItem.bladePointsOnPlane.Length - 1];
			Vector2 vector3 = bladeSectionJobItem2.bladePointsOnPlane[0];
			Vector2 vector4 = bladeSectionJobItem2.bladePointsOnPlane[bladeSectionJobItem2.bladePointsOnPlane.Length - 1];
			for (int j = 0; j < cutColliderSectionRings.Length; j++)
			{
				int num = 0;
				int num2 = 0;
				int num3 = 1;
				CutColliderSectionRing cutColliderSectionRing;
				for (;;)
				{
					int num4 = num3;
					cutColliderSectionRing = cutColliderSectionRings[j];
					if (num4 >= cutColliderSectionRing.points.Length)
					{
						break;
					}
					Vector2 rayOrigin = vector;
					Vector2 rayEnd = vector3;
					cutColliderSectionRing = cutColliderSectionRings[j];
					Vector2 segmentStart = cutColliderSectionRing.points[num3 - 1];
					cutColliderSectionRing = cutColliderSectionRings[j];
					if (this.IntersectLineSegmentToLineSegment(rayOrigin, rayEnd, segmentStart, cutColliderSectionRing.points[num3]))
					{
						num++;
					}
					Vector2 rayOrigin2 = vector2;
					Vector2 rayEnd2 = vector4;
					cutColliderSectionRing = cutColliderSectionRings[j];
					Vector2 segmentStart2 = cutColliderSectionRing.points[num3 - 1];
					cutColliderSectionRing = cutColliderSectionRings[j];
					if (this.IntersectLineSegmentToLineSegment(rayOrigin2, rayEnd2, segmentStart2, cutColliderSectionRing.points[num3]))
					{
						num2++;
					}
					num3++;
				}
				Vector2 rayOrigin3 = vector;
				Vector2 rayEnd3 = vector3;
				cutColliderSectionRing = cutColliderSectionRings[j];
				Vector2 segmentStart3 = cutColliderSectionRing.points[0];
				cutColliderSectionRing = cutColliderSectionRings[j];
				CutColliderSectionRing cutColliderSectionRing2 = cutColliderSectionRings[j];
				if (this.IntersectLineSegmentToLineSegment(rayOrigin3, rayEnd3, segmentStart3, cutColliderSectionRing.points[cutColliderSectionRing2.points.Length - 1]))
				{
					num++;
				}
				Vector2 rayOrigin4 = vector2;
				Vector2 rayEnd4 = vector4;
				cutColliderSectionRing = cutColliderSectionRings[j];
				Vector2 segmentStart4 = cutColliderSectionRing.points[0];
				cutColliderSectionRing = cutColliderSectionRings[j];
				cutColliderSectionRing2 = cutColliderSectionRings[j];
				if (this.IntersectLineSegmentToLineSegment(rayOrigin4, rayEnd4, segmentStart4, cutColliderSectionRing.points[cutColliderSectionRing2.points.Length - 1]))
				{
					num2++;
				}
				if (num != 0 || num2 != 0)
				{
					value.invalidCut = true;
					break;
				}
			}
			this.bladeSectionInfos[i] = value;
		}
	}

	// Token: 0x060002BD RID: 701 RVA: 0x0000D798 File Offset: 0x0000B998
	public bool IntersectRaySegment(Vector2 rayOrigin, Vector2 rayDirection, Vector2 segmentStart, Vector2 segmentEnd)
	{
		Vector2 vector;
		return this.RayToLineSegment(rayOrigin, rayDirection, segmentStart, segmentEnd, out vector);
	}

	// Token: 0x060002BE RID: 702 RVA: 0x0000D7B4 File Offset: 0x0000B9B4
	public bool IntersectLineSegmentToLineSegment(Vector2 rayOrigin, Vector2 rayEnd, Vector2 segmentStart, Vector2 segmentEnd)
	{
		Vector2 rayDirection = rayEnd - rayOrigin;
		Vector2 a;
		if (this.RayToLineSegment(rayOrigin, rayDirection, segmentStart, segmentEnd, out a))
		{
			float sqrMagnitude = rayDirection.sqrMagnitude;
			if ((a - rayOrigin).sqrMagnitude <= sqrMagnitude)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060002BF RID: 703 RVA: 0x0000D7F8 File Offset: 0x0000B9F8
	public bool RayToLineSegment(Vector2 rayOrigin, Vector2 rayDirection, Vector2 p1, Vector2 p2, out Vector2 intersection)
	{
		intersection = Vector2.zero;
		float x = rayDirection.x;
		float y = rayDirection.y;
		float x2 = rayOrigin.x;
		float y2 = rayOrigin.y;
		float x3 = p1.x;
		float y3 = p1.y;
		float x4 = p2.x;
		float y4 = p2.y;
		float num = x * (y4 - y3) - y * (x4 - x3);
		if (math.abs(num) < 1.1920929E-07f)
		{
			return false;
		}
		float num2 = ((y2 - y3) * (x4 - x3) - (x2 - x3) * (y4 - y3)) / num;
		float num3 = ((y2 - y3) * x - (x2 - x3) * y) / num;
		if (num2 >= 0f && num3 >= 0f && num3 <= 1f)
		{
			intersection = new Vector2(x2 + num2 * x, y2 + num2 * y);
			return true;
		}
		return false;
	}

	// Token: 0x0400019E RID: 414
	private NativeArray<CuttableJobSection> cuttableSections;

	// Token: 0x0400019F RID: 415
	[ReadOnly]
	private NativeArray<int> tris;

	// Token: 0x040001A0 RID: 416
	[ReadOnly]
	private NativeArray<Vector3> vertices;

	// Token: 0x040001A1 RID: 417
	[ReadOnly]
	public NativeArray<CuttableMeshJobItem> cuttableMeshJobItems;

	// Token: 0x040001A2 RID: 418
	[ReadOnly]
	public Matrix4x4 localToWorldMatrix;

	// Token: 0x040001A3 RID: 419
	[ReadOnly]
	public Matrix4x4 worldToLocalMatrix;

	// Token: 0x040001A4 RID: 420
	[ReadOnly]
	public Matrix4x4 parentLocalToWorldMatrix;

	// Token: 0x040001A5 RID: 421
	private NativeArray<BladeSectionJobItem> bladeStartSections;

	// Token: 0x040001A6 RID: 422
	private NativeArray<CheckCutJobOutValues> checkCutResult;

	// Token: 0x040001A7 RID: 423
	private NativeArray<BladeSectionJobItem> bladeCurrentSections;

	// Token: 0x040001A8 RID: 424
	private NativeArray<BladeSectionJobInfoItem> bladeSectionInfos;

	// Token: 0x040001A9 RID: 425
	private NativeArray<CuttableCollider> cuttableColliders;

	// Token: 0x040001AA RID: 426
	public ConversionPlane cutPlane;

	// Token: 0x040001AB RID: 427
	private Vector3 right;

	// Token: 0x040001AC RID: 428
	private Vector3 forward;
}
