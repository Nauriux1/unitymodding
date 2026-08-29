using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000259 RID: 601
public class BladePaintable : MonoBehaviour
{
	// Token: 0x06001197 RID: 4503 RVA: 0x0005A138 File Offset: 0x00058338
	private void Start()
	{
		this.trianglePoints = new Vector4[999];
		this.trianglePointLength = 0;
		this.spherePoints = new Vector4[128];
		this.spherePointLength = 0;
		this.spherePointsGreen = new Vector4[128];
		this.spherePointGreenLength = 0;
		this.paintColor = new Color(0.3294117f, 0f, 0f, 1f);
		this.meshRenderer = base.GetComponent<MeshRenderer>();
		this.mesh = base.gameObject.GetComponent<MeshFilter>().mesh;
		if (this.weaponDamageableBodyPartsForActivatingPainter != null && this.weaponDamageableBodyPartsForActivatingPainter.Count > 0)
		{
			using (List<WeaponDamageableBodyPart>.Enumerator enumerator = this.weaponDamageableBodyPartsForActivatingPainter.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					WeaponDamageableBodyPart weaponDamageableBodyPart = enumerator.Current;
					weaponDamageableBodyPart.bladePaintables.Add(this);
				}
				return;
			}
		}
		Transform parent = base.transform.parent;
		if (parent == null)
		{
			return;
		}
		parent.GetComponent<WeaponDamageableBodyPart>().bladePaintables.Add(this);
	}

	// Token: 0x06001198 RID: 4504 RVA: 0x0005A24C File Offset: 0x0005844C
	private void Update()
	{
		this.DrawPoints();
	}

	// Token: 0x06001199 RID: 4505 RVA: 0x0005A254 File Offset: 0x00058454
	public void AddDrawableTriangle(BladeTriangle tri)
	{
		if (this.trianglePointLength < 996)
		{
			this.trianglePoints[this.trianglePointLength] = tri.p0;
			this.trianglePoints[this.trianglePointLength + 1] = tri.p1;
			this.trianglePoints[this.trianglePointLength + 2] = tri.p2;
			this.trianglePointLength += 3;
		}
	}

	// Token: 0x0600119A RID: 4506 RVA: 0x0005A2D4 File Offset: 0x000584D4
	public void AddDrawableSphere(Vector4 vector4)
	{
		if (this.spherePointLength < 100)
		{
			this.spherePoints[this.spherePointLength] = vector4;
			this.spherePointLength++;
		}
	}

	// Token: 0x0600119B RID: 4507 RVA: 0x0005A300 File Offset: 0x00058500
	public void AddDrawableSphereGreen(Vector4 vector4)
	{
		if (this.spherePointGreenLength < 100)
		{
			this.spherePointsGreen[this.spherePointGreenLength] = vector4;
			this.spherePointGreenLength++;
		}
	}

	// Token: 0x0600119C RID: 4508 RVA: 0x0005A32C File Offset: 0x0005852C
	private void DrawPoints()
	{
		if (this.trianglePointLength > 0 || this.spherePointLength > 0)
		{
			Singleton<PaintManager>.instance.paintTriangles(this.paintable, this.trianglePoints, this.trianglePointLength, this.spherePoints, this.spherePointLength, this.spherePointsGreen, this.spherePointGreenLength, this.radius, this.hardness, this.strength, new Color?(this.paintColor));
			this.trianglePointLength = 0;
			this.spherePointLength = 0;
			this.spherePointGreenLength = 0;
		}
	}

	// Token: 0x04000D2F RID: 3375
	private Mesh mesh;

	// Token: 0x04000D30 RID: 3376
	private MeshRenderer meshRenderer;

	// Token: 0x04000D31 RID: 3377
	public IPaintable paintable;

	// Token: 0x04000D32 RID: 3378
	public List<WeaponDamageableBodyPart> weaponDamageableBodyPartsForActivatingPainter = new List<WeaponDamageableBodyPart>();

	// Token: 0x04000D33 RID: 3379
	public int trianglePointLength;

	// Token: 0x04000D34 RID: 3380
	public Vector4[] trianglePoints;

	// Token: 0x04000D35 RID: 3381
	public int spherePointLength;

	// Token: 0x04000D36 RID: 3382
	public Vector4[] spherePoints;

	// Token: 0x04000D37 RID: 3383
	public int spherePointGreenLength;

	// Token: 0x04000D38 RID: 3384
	public Vector4[] spherePointsGreen;

	// Token: 0x04000D39 RID: 3385
	[Space]
	public Color paintColor;

	// Token: 0x04000D3A RID: 3386
	private float radius = 0.01f;

	// Token: 0x04000D3B RID: 3387
	private float strength = 0.5f;

	// Token: 0x04000D3C RID: 3388
	private float hardness = 0.5f;
}
