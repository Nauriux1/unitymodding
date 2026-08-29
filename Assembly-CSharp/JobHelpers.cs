using System;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;

// Token: 0x0200023D RID: 573
[BurstCompile]
public class JobHelpers
{
	// Token: 0x060010CD RID: 4301 RVA: 0x00056868 File Offset: 0x00054A68
	public static Vector2 ConvertToPlane2D(Vector3 point, ConversionPlane plane)
	{
		Vector3 lhs = plane.plane.ClosestPointOnPlane(point);
		float x = Vector3.Dot(lhs, plane.right);
		float y = Vector3.Dot(lhs, plane.forward);
		return new Vector2(x, y);
	}

	// Token: 0x060010CE RID: 4302 RVA: 0x000568A4 File Offset: 0x00054AA4
	public static ConversionPlane PlaneToConversionPlane(Plane plane)
	{
		ConversionPlane conversionPlane = default(ConversionPlane);
		conversionPlane.plane = plane;
		conversionPlane.right = Vector3.Cross(conversionPlane.plane.normal, Vector3.forward).normalized;
		if (conversionPlane.right.magnitude < 0.1f)
		{
			conversionPlane.right = Vector3.Cross(conversionPlane.plane.normal, Vector3.right).normalized;
		}
		conversionPlane.forward = Vector3.Cross(conversionPlane.right, conversionPlane.plane.normal).normalized;
		return conversionPlane;
	}

	// Token: 0x060010CF RID: 4303 RVA: 0x00056948 File Offset: 0x00054B48
	public static NullableJobVector3 GetIntersectionPlaneAndLineSegment(Plane plane, Vector3 pointA, Vector3 pointB)
	{
		NullableJobVector3 result = default(NullableJobVector3);
		result.hasValue = false;
		Vector3 vector = pointB - pointA;
		float num = Vector3.Dot(plane.normal, vector);
		if ((double)Mathf.Abs(num) > 1E-06)
		{
			float num2 = Vector3.Dot(plane.normal, plane.ClosestPointOnPlane(pointA) - pointA) / num;
			if (num2 >= 0f && num2 <= 1f)
			{
				Vector3 vector2 = pointA + num2 * vector;
				result.vector3 = vector2;
				result.hasValue = true;
				result.percentage = num2;
			}
			else
			{
				Vector3 vector3 = pointA + num2 * vector;
				result.vector3 = vector3;
				result.percentage = num2;
				result.hasValue = false;
			}
		}
		return result;
	}

	// Token: 0x060010D0 RID: 4304 RVA: 0x00056A10 File Offset: 0x00054C10
	public static void GetColliderIntersectionPoints(NativeList<Vector2> meshPointsOnPlane, NativeArray<CuttableCollider> cuttableColliders, ConversionPlane plane, NativeList<CutColliderSectionRing> colliderSections, bool parentCut, Matrix4x4 parentLocalToWorldMatrix, Matrix4x4 worldToLocalMatrix)
	{
		Matrix4x4 matrix4x = worldToLocalMatrix * parentLocalToWorldMatrix;
		for (int i = 0; i < cuttableColliders.Length; i++)
		{
			CuttableCollider cuttableCollider = cuttableColliders[i];
			if (cuttableCollider.parentCollider == parentCut)
			{
				float radius = cuttableCollider.radius;
				Vector3 vector = cuttableCollider.p0;
				Vector3 vector2 = cuttableCollider.p1;
				if (parentCut)
				{
					vector = matrix4x.MultiplyPoint3x4(vector);
					vector2 = matrix4x.MultiplyPoint3x4(vector2);
				}
				if (cuttableCollider.colliderType == ColliderType.Sphere)
				{
					JobHelpers.GetSphereIntersectionPoints(meshPointsOnPlane, vector, radius, plane, colliderSections);
				}
				else
				{
					JobHelpers.GetSphereIntersectionPoints(meshPointsOnPlane, vector, radius, plane, colliderSections);
					JobHelpers.GetSphereIntersectionPoints(meshPointsOnPlane, vector2, radius, plane, colliderSections);
					JobHelpers.GetCylinderIntersectionPoints(meshPointsOnPlane, vector, vector2, radius, plane, colliderSections);
				}
			}
		}
	}

	// Token: 0x060010D1 RID: 4305 RVA: 0x00056ABC File Offset: 0x00054CBC
	public static void GetCylinderIntersectionPoints(NativeList<Vector2> meshPointsOnPlane, Vector3 p0, Vector3 p1, float radius, ConversionPlane plane, NativeList<CutColliderSectionRing> colliderSections)
	{
		Vector3 b = p1 - p0;
		Vector3 normalized = b.normalized;
		Vector3 vector = default(Vector3);
		Vector3 vector2;
		if (Mathf.Abs(normalized.y) < 0.99f)
		{
			vector2 = Vector3.Cross(normalized, Vector3.up);
			vector = vector2.normalized;
		}
		else
		{
			vector2 = Vector3.Cross(normalized, Vector3.right);
			vector = vector2.normalized;
		}
		vector2 = Vector3.Cross(vector, normalized);
		Vector3 normalized2 = vector2.normalized;
		vector2 = vector + normalized2;
		Vector3 normalized3 = vector2.normalized;
		vector2 = vector + -normalized2;
		Vector3 normalized4 = vector2.normalized;
		FixedList128Bytes<Vector3> fixedList128Bytes = default(FixedList128Bytes<Vector3>);
		vector2 = p0 + vector * radius;
		fixedList128Bytes.Add(vector2);
		vector2 = p0 + normalized3 * radius;
		fixedList128Bytes.Add(vector2);
		vector2 = p0 + normalized2 * radius;
		fixedList128Bytes.Add(vector2);
		vector2 = p0 + -normalized4 * radius;
		fixedList128Bytes.Add(vector2);
		vector2 = p0 + -vector * radius;
		fixedList128Bytes.Add(vector2);
		vector2 = p0 + -normalized3 * radius;
		fixedList128Bytes.Add(vector2);
		vector2 = p0 + -normalized2 * radius;
		fixedList128Bytes.Add(vector2);
		vector2 = p0 + normalized4 * radius;
		fixedList128Bytes.Add(vector2);
		CutColliderSectionRing cutColliderSectionRing = default(CutColliderSectionRing);
		for (int i = 0; i < fixedList128Bytes.Length; i++)
		{
			NullableJobVector3 intersectionPlaneAndLineSegment = JobHelpers.GetIntersectionPlaneAndLineSegment(plane.plane, fixedList128Bytes[i], fixedList128Bytes[i] + b);
			if (intersectionPlaneAndLineSegment.hasValue)
			{
				Vector2 vector3 = JobHelpers.ConvertToPlane2D(intersectionPlaneAndLineSegment.vector3, plane);
				meshPointsOnPlane.Add(vector3);
				cutColliderSectionRing.points.Add(vector3);
			}
		}
		if (cutColliderSectionRing.points.Length > 0)
		{
			colliderSections.Add(cutColliderSectionRing);
		}
	}

	// Token: 0x060010D2 RID: 4306 RVA: 0x00056CC8 File Offset: 0x00054EC8
	public static void GetSphereIntersectionPoints(NativeList<Vector2> meshPointsOnPlane, Vector3 sphereCenter, float radius, ConversionPlane plane, NativeList<CutColliderSectionRing> colliderSections)
	{
		Vector3 zero = Vector3.zero;
		float distanceToPoint = plane.plane.GetDistanceToPoint(sphereCenter);
		float num = Mathf.Abs(distanceToPoint);
		if (num <= radius)
		{
			Vector3 point = sphereCenter - plane.plane.normal * distanceToPoint;
			float d = radius - num;
			Vector2 a = JobHelpers.ConvertToPlane2D(point, plane);
			Vector2 normalized = (Vector2.up + Vector2.right).normalized;
			Vector2 normalized2 = (Vector2.up + -Vector2.right).normalized;
			Vector2 vector = a + Vector2.up * d;
			Vector2 vector2 = a + -Vector2.up * d;
			Vector2 vector3 = a + Vector2.right * d;
			Vector2 vector4 = a + -Vector2.right * d;
			Vector2 vector5 = a + normalized * d;
			Vector2 vector6 = a + -normalized * d;
			Vector2 vector7 = a + normalized2 * d;
			Vector2 vector8 = a + -normalized2 * d;
			meshPointsOnPlane.Add(vector);
			meshPointsOnPlane.Add(vector2);
			meshPointsOnPlane.Add(vector3);
			meshPointsOnPlane.Add(vector4);
			meshPointsOnPlane.Add(vector5);
			meshPointsOnPlane.Add(vector6);
			meshPointsOnPlane.Add(vector7);
			meshPointsOnPlane.Add(vector8);
			CutColliderSectionRing cutColliderSectionRing = default(CutColliderSectionRing);
			cutColliderSectionRing.points.Add(vector);
			cutColliderSectionRing.points.Add(vector5);
			cutColliderSectionRing.points.Add(vector3);
			cutColliderSectionRing.points.Add(vector8);
			cutColliderSectionRing.points.Add(vector2);
			cutColliderSectionRing.points.Add(vector6);
			cutColliderSectionRing.points.Add(vector4);
			cutColliderSectionRing.points.Add(vector7);
			colliderSections.Add(cutColliderSectionRing);
		}
	}

	// Token: 0x060010D3 RID: 4307 RVA: 0x00056EC0 File Offset: 0x000550C0
	public static void GetTransformedVerts(NativeList<Vector3> tranformedVerts, Matrix4x4 worldToLocalMatrix, NativeArray<CuttableMeshJobItem> cuttableMeshJobItems, NativeArray<Vector3> vertices)
	{
		int num = 0;
		Matrix4x4 matrix4x = worldToLocalMatrix * cuttableMeshJobItems[num].meshLocalToWorldMatrix;
		int num2 = cuttableMeshJobItems[num].meshVertCounts;
		for (int i = 0; i < vertices.Length; i++)
		{
			if (i == num2)
			{
				num++;
				matrix4x = worldToLocalMatrix * cuttableMeshJobItems[num].meshLocalToWorldMatrix;
				num2 += cuttableMeshJobItems[num].meshVertCounts;
			}
			Vector3 vector = matrix4x.MultiplyPoint3x4(vertices[i]);
			tranformedVerts.Add(vector);
		}
	}

	// Token: 0x060010D4 RID: 4308 RVA: 0x00056F4C File Offset: 0x0005514C
	public static void GetTransformedNormals(NativeList<Vector3> tranformedNormals, Matrix4x4 worldToLocalMatrix, NativeArray<CuttableMeshJobItem> cuttableMeshJobItems, NativeArray<Vector3> normals)
	{
		int num = 0;
		Matrix4x4 matrix4x = worldToLocalMatrix * cuttableMeshJobItems[num].meshLocalToWorldMatrix;
		int num2 = cuttableMeshJobItems[num].meshVertCounts;
		for (int i = 0; i < normals.Length; i++)
		{
			if (i == num2)
			{
				num++;
				matrix4x = worldToLocalMatrix * cuttableMeshJobItems[num].meshLocalToWorldMatrix;
				num2 += cuttableMeshJobItems[num].meshVertCounts;
			}
			Vector3 vector = matrix4x.inverse.transpose.MultiplyVector(normals[i]);
			vector = vector.normalized;
			tranformedNormals.Add(vector);
		}
	}

	// Token: 0x060010D5 RID: 4309 RVA: 0x00056FF0 File Offset: 0x000551F0
	public static Plane CheckPlaneDirection(Plane plane)
	{
		if (plane.GetSide(new Vector3(0f, 0f, 0f)))
		{
			plane.Flip();
		}
		return plane;
	}

	// Token: 0x060010D6 RID: 4310 RVA: 0x00057018 File Offset: 0x00055218
	public static void UpdateCutSections(NativeArray<CuttableJobSection> cuttableSections, Matrix4x4 worldToLocalMatrix)
	{
		for (int i = 0; i < cuttableSections.Length; i++)
		{
			CuttableJobSection cuttableJobSection = cuttableSections[i];
			if (cuttableJobSection.isParent)
			{
				cuttableJobSection.position = worldToLocalMatrix.MultiplyPoint3x4(cuttableJobSection.localToWorldMatrix.MultiplyPoint3x4(new Vector3(0f, 0f, 0f)));
			}
			cuttableSections[i] = cuttableJobSection;
		}
	}
}
