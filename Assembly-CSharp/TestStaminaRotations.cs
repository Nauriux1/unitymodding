using System;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x02000194 RID: 404
public class TestStaminaRotations : MonoBehaviour
{
	// Token: 0x06000C8B RID: 3211 RVA: 0x0000777A File Offset: 0x0000597A
	private void Start()
	{
	}

	// Token: 0x06000C8C RID: 3212 RVA: 0x0003D034 File Offset: 0x0003B234
	private void Update()
	{
		this.CalculateDotProducts(this.rotationOld.localRotation, this.rotationNew.localRotation, this.rotationTarget.localRotation);
		this.CalculateDotProductDirection();
		this.CalculateDotProductTurn(this.rotationOld.localRotation, this.rotationNew.localRotation, this.rotationTarget.localRotation);
		this.CalculateTurnTowards(this.rotationOld.localRotation, this.rotationNew.localRotation, this.rotationTarget.localRotation);
		this.CalculateAngleBasedTurn(this.rotationOld.localRotation, this.rotationNew.localRotation, this.rotationTarget.localRotation);
		this.CalculateRotationTowardsTargetInDegrees(this.rotationOld.localRotation, this.rotationNew.localRotation, this.rotationTarget.localRotation);
	}

	// Token: 0x06000C8D RID: 3213 RVA: 0x0003D110 File Offset: 0x0003B310
	public void CalculateDotProducts(Quaternion start, Quaternion end, Quaternion target)
	{
		this.degreesStartToEnd = Quaternion.Angle(start, end);
		this.degreesStartToTarget = Quaternion.Angle(start, target);
		this.degreesEndToTarget = Quaternion.Angle(end, target);
		this.degreesTowardsTarget = math.max(this.degreesStartToTarget - this.degreesEndToTarget, 0f);
		this.normalizedDegreesTowardsTarget = this.degreesTowardsTarget / 180f;
		float dot = math.dot(start, target);
		this.safeDegreesStartToTarget = this.DotToDegrees(dot);
		float dot2 = math.dot(end, target);
		this.safeDegreesEndToTarget = this.DotToDegrees(dot2);
		this.safeDegreesTowardsTarget = math.max(this.safeDegreesStartToTarget - this.safeDegreesEndToTarget, 0f);
		this.safeNormalizedDegreesTowardsTarget = this.safeDegreesTowardsTarget / 180f;
		this.q1q2 = Quaternion.Dot(start, end);
		this.q1q3 = Quaternion.Dot(start, target);
		this.q2q3 = Quaternion.Dot(end, target);
		Vector3 vector = start * Vector3.up;
		Vector3 vector2 = end * Vector3.up;
		Vector3 vector3 = target * Vector3.up;
		this.vectorDot1 = Vector3.Dot(vector, vector2);
		this.vectorDot2 = Vector3.Dot(vector, vector3);
		this.vectorDot3 = Vector3.Dot(vector2, vector3);
		this.degrees1 = this.QuaternionDotToDegrees(this.q1q2);
		this.degrees2 = this.QuaternionDotToDegrees(this.q1q3);
		this.degrees3 = this.QuaternionDotToDegrees(this.q2q3);
		this.normalizedDegrees1 = this.NormalizeDegrees(this.degrees1);
		this.normalizedDegrees2 = this.NormalizeDegrees(this.degrees2);
		this.normalizedDegrees3 = this.NormalizeDegrees(this.degrees3);
		Debug.DrawRay(default(Vector3), vector * 100f, Color.green);
		Debug.DrawRay(default(Vector3), vector2 * 100f, Color.blue);
		Debug.DrawRay(default(Vector3), vector3 * 100f, Color.red);
	}

	// Token: 0x06000C8E RID: 3214 RVA: 0x0003D31F File Offset: 0x0003B51F
	public float DotToDegrees(float dot)
	{
		return math.acos(math.min(math.abs(dot), 1f)) * 2f * 57.29578f;
	}

	// Token: 0x06000C8F RID: 3215 RVA: 0x0003D342 File Offset: 0x0003B542
	public float NormalizeDegrees(float degrees)
	{
		return degrees / 360f;
	}

	// Token: 0x06000C90 RID: 3216 RVA: 0x0003D34B File Offset: 0x0003B54B
	public float QuaternionDotToDegrees(float dot)
	{
		dot = Mathf.Clamp(dot, -1f, 1f);
		return 2f * Mathf.Acos(dot) * 57.29578f;
	}

	// Token: 0x06000C91 RID: 3217 RVA: 0x0003D374 File Offset: 0x0003B574
	public float CalculateAngleBasedTurn(Quaternion q1, Quaternion q2, Quaternion q3)
	{
		float num = Mathf.Acos(Quaternion.Dot(Quaternion.Inverse(q1) * q2, Quaternion.identity)) * 57.29578f;
		float num2 = Mathf.Sign(Quaternion.Dot(q3, q2) - Quaternion.Dot(q3, q1));
		this.angleBasedTurn = num * num2;
		return this.angleBasedTurn;
	}

	// Token: 0x06000C92 RID: 3218 RVA: 0x0003D3C8 File Offset: 0x0003B5C8
	public float CalculateTurnTowards(Quaternion q1, Quaternion q2, Quaternion q3)
	{
		Quaternion a = Quaternion.Inverse(q1) * q2;
		this.turnTowardsAmount = Quaternion.Dot(a, q3);
		return this.turnTowardsAmount;
	}

	// Token: 0x06000C93 RID: 3219 RVA: 0x0003D3F8 File Offset: 0x0003B5F8
	public float CalculateDotProductTurn(Quaternion q1, Quaternion q2, Quaternion q3)
	{
		float num = Quaternion.Dot(q1, q3);
		float num2 = Quaternion.Dot(q2, q3);
		this.turnAmount = num2 - num;
		return this.turnAmount;
	}

	// Token: 0x06000C94 RID: 3220 RVA: 0x0003D424 File Offset: 0x0003B624
	public float CalculateRotationTowardsTargetInDegrees(Quaternion q1, Quaternion q2, Quaternion q3)
	{
		float num = Quaternion.Angle(q1, q3);
		float num2 = Quaternion.Angle(q2, q3);
		this.angleChange = num - num2;
		return this.angleChange;
	}

	// Token: 0x06000C95 RID: 3221 RVA: 0x0003D450 File Offset: 0x0003B650
	private void CalculateDotProductDirection()
	{
		float num = Vector3.Dot(this.rotationOld.up, this.rotationTarget.up);
		float num2 = Vector3.Dot(this.rotationNew.up, this.rotationTarget.up);
		this.directionDot = num2 - num;
	}

	// Token: 0x06000C96 RID: 3222 RVA: 0x0003D4A0 File Offset: 0x0003B6A0
	private void OnGUI()
	{
		float num = 0f;
		GUI.Label(new Rect(10f, num += 12f, 1000f, 40f), "DegreesDifference1:" + this.degreesStartToEnd.ToString("0.00") + " ");
		GUI.Label(new Rect(10f, num += 12f, 1000f, 40f), "DegreesDifference2:" + this.degreesStartToTarget.ToString("0.00") + " ");
		GUI.Label(new Rect(10f, num += 12f, 1000f, 40f), "DegreesDifference3:" + this.degreesEndToTarget.ToString("0.00") + " ");
		num += 12f;
		GUI.Label(new Rect(10f, num += 12f, 1000f, 40f), string.Concat(new string[]
		{
			"degreesTowardsTarget:",
			this.degreesTowardsTarget.ToString("0.00"),
			" == (",
			this.safeDegreesTowardsTarget.ToString("0.00"),
			")"
		}));
		GUI.Label(new Rect(10f, num += 12f, 1000f, 40f), string.Concat(new string[]
		{
			"normalizedDegreesTowardsTarget:",
			this.normalizedDegreesTowardsTarget.ToString("0.00"),
			" == (",
			this.safeNormalizedDegreesTowardsTarget.ToString("0.00"),
			") "
		}));
		num += 12f;
		GUI.Label(new Rect(10f, num += 12f, 1000f, 40f), "safeDegreesStartToTarget:" + this.safeDegreesStartToTarget.ToString("0.00") + " ");
		GUI.Label(new Rect(10f, num += 12f, 1000f, 40f), "safeDegreesEndToTarget:" + this.safeDegreesEndToTarget.ToString("0.00") + " ");
		num += 12f;
		GUI.Label(new Rect(10f, num += 12f, 1000f, 40f), "safeDegreesTowardsTarget:" + this.safeDegreesTowardsTarget.ToString("0.00") + " ");
		GUI.Label(new Rect(10f, num + 12f, 1000f, 40f), "safeNormalizedDegreesTowardsTarget:" + this.safeNormalizedDegreesTowardsTarget.ToString("0.00") + " ");
	}

	// Token: 0x040008F4 RID: 2292
	public Transform rotationOld;

	// Token: 0x040008F5 RID: 2293
	public Transform rotationNew;

	// Token: 0x040008F6 RID: 2294
	public Transform rotationTarget;

	// Token: 0x040008F7 RID: 2295
	private float q1q2;

	// Token: 0x040008F8 RID: 2296
	private float q1q3;

	// Token: 0x040008F9 RID: 2297
	private float q2q3;

	// Token: 0x040008FA RID: 2298
	private float vectorDot1;

	// Token: 0x040008FB RID: 2299
	private float vectorDot2;

	// Token: 0x040008FC RID: 2300
	private float vectorDot3;

	// Token: 0x040008FD RID: 2301
	private float degrees1;

	// Token: 0x040008FE RID: 2302
	private float degrees2;

	// Token: 0x040008FF RID: 2303
	private float degrees3;

	// Token: 0x04000900 RID: 2304
	private float degreesStartToEnd;

	// Token: 0x04000901 RID: 2305
	private float degreesStartToTarget;

	// Token: 0x04000902 RID: 2306
	private float degreesEndToTarget;

	// Token: 0x04000903 RID: 2307
	private float degreesTowardsTarget;

	// Token: 0x04000904 RID: 2308
	private float normalizedDegreesTowardsTarget;

	// Token: 0x04000905 RID: 2309
	private float safeDegreesStartToTarget;

	// Token: 0x04000906 RID: 2310
	private float safeDegreesEndToTarget;

	// Token: 0x04000907 RID: 2311
	private float safeDegreesTowardsTarget;

	// Token: 0x04000908 RID: 2312
	private float safeNormalizedDegreesTowardsTarget;

	// Token: 0x04000909 RID: 2313
	private float normalizedDegrees1;

	// Token: 0x0400090A RID: 2314
	private float normalizedDegrees2;

	// Token: 0x0400090B RID: 2315
	private float normalizedDegrees3;

	// Token: 0x0400090C RID: 2316
	private float angleBasedTurn;

	// Token: 0x0400090D RID: 2317
	private float turnTowardsAmount;

	// Token: 0x0400090E RID: 2318
	public float turnAmount;

	// Token: 0x0400090F RID: 2319
	private float angleChange;

	// Token: 0x04000910 RID: 2320
	private float directionDot;
}
