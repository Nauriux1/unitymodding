using System;
using System.Collections.Generic;
using Es.InkPainter;
using TriTriIntersectionHelpers;
using UnityEngine;

// Token: 0x0200025E RID: 606
public class SimplePaintTest : MonoBehaviour
{
	// Token: 0x060011AD RID: 4525 RVA: 0x0005ABD6 File Offset: 0x00058DD6
	private void Start()
	{
		this.CopyTexture();
		this.cam = Camera.main;
	}

	// Token: 0x060011AE RID: 4526 RVA: 0x0005ABEC File Offset: 0x00058DEC
	private void Update()
	{
		if (Input.GetMouseButton(0))
		{
			Debug.Log("click");
			Vector3 mousePosition = Input.mousePosition;
			Ray ray = this.cam.ScreenPointToRay(mousePosition);
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit, 100f))
			{
				Debug.Log("hit");
				Debug.DrawRay(ray.origin, raycastHit.point - ray.origin, Color.red);
				base.transform.position = raycastHit.point;
				Paintable component = raycastHit.collider.GetComponent<Paintable>();
				if (component != null)
				{
					Debug.Log("paint");
					Singleton<PaintManager>.instance.paint(component, raycastHit.point, this.radius, this.hardness, this.strength, new Color?(this.paintColor));
				}
			}
		}
		Input.GetMouseButton(0);
	}

	// Token: 0x060011AF RID: 4527 RVA: 0x0005ACCC File Offset: 0x00058ECC
	private void DrawPoints(MeshFilter meshFilter)
	{
		if (this.drawPoints.Count > 0)
		{
			Texture2D texture2D = meshFilter.transform.GetComponent<Renderer>().material.mainTexture as Texture2D;
			foreach (Vector2 vector in this.drawPoints)
			{
				vector.x *= (float)texture2D.width;
				vector.y *= (float)texture2D.height;
				Color color = new Color(0f, 0f, 0f);
				texture2D.SetPixel((int)vector.x, (int)vector.y, color);
				int num = (int)vector.x;
				int num2 = (int)vector.y;
				for (int i = -this.size; i <= this.size; i++)
				{
					for (int j = -this.size; j <= this.size; j++)
					{
						int x = i + num;
						int y = j + num2;
						texture2D.SetPixel(x, y, color);
					}
				}
			}
			this.drawPoints.Clear();
			texture2D.Apply();
		}
	}

	// Token: 0x060011B0 RID: 4528 RVA: 0x0005AE0C File Offset: 0x0005900C
	private void CopyTexture()
	{
		Texture2D texture2D = this.startRenderer.material.mainTexture as Texture2D;
		int width = texture2D.width;
		int width2 = texture2D.width;
		this.outArray = new Texture2DArray(width, width2, 1, texture2D.format, true);
		this.outTex = new Texture2D(width, width2, texture2D.format, true);
		for (int i = 0; i < texture2D.mipmapCount; i++)
		{
			int srcWidth = width >> i;
			int srcHeight = width2 >> i;
			Graphics.CopyTexture(texture2D, 0, i, 0, 0, srcWidth, srcHeight, this.outArray, 0, i, 0, 0);
		}
		for (int j = 0; j < this.outArray.mipmapCount; j++)
		{
			int srcWidth2 = width >> j;
			int srcHeight2 = width2 >> j;
			Graphics.CopyTexture(this.outArray, 0, j, 0, 0, srcWidth2, srcHeight2, this.outTex, 0, j, 0, 0);
		}
		this.startRenderer.material.mainTexture = this.outTex;
	}

	// Token: 0x060011B1 RID: 4529 RVA: 0x0005AF00 File Offset: 0x00059100
	public void DrawTriangleOnMesh(Vector3 p0, Vector3 p1, Vector3 p2)
	{
		Mesh mesh = this.startMeshFilter.mesh;
		this.tris = mesh.triangles;
		this.vectors = mesh.vertices;
		this.uvs = mesh.uv;
		Vector3 u = this.startMeshFilter.transform.worldToLocalMatrix.MultiplyPoint3x4(p0);
		Vector3 u2 = this.startMeshFilter.transform.worldToLocalMatrix.MultiplyPoint3x4(p1);
		Vector3 u3 = this.startMeshFilter.transform.worldToLocalMatrix.MultiplyPoint3x4(p2);
		Matrix4x4 transformMatrix = this.cam.projectionMatrix * this.cam.worldToCameraMatrix * this.startMeshFilter.transform.localToWorldMatrix;
		for (int i = 0; i < this.tris.Length; i += 3)
		{
			int num = this.tris[i];
			int num2 = this.tris[i + 1];
			int num3 = this.tris[i + 2];
			Vector3 vector = this.vectors[num];
			Vector3 vector2 = this.vectors[num2];
			Vector3 vector3 = this.vectors[num3];
			IntersectionInfo intersectionInfo = this.CheckTriangle(vector, vector2, vector3, u, u2, u3);
			if (intersectionInfo.intersects)
			{
				Vector3 start = this.startMeshFilter.transform.localToWorldMatrix.MultiplyPoint3x4(intersectionInfo.intersectionPoint1);
				Vector3 end = this.startMeshFilter.transform.localToWorldMatrix.MultiplyPoint3x4(intersectionInfo.intersectionPoint2);
				Debug.DrawLine(start, end, Color.black);
				Vector2 t1UV = this.uvs[num];
				Vector2 t2UV = this.uvs[num2];
				Vector2 t3UV = this.uvs[num3];
				Vector2 item = Es.InkPainter.Math.TextureCoordinateCalculation(intersectionInfo.intersectionPoint1, vector, t1UV, vector2, t2UV, vector3, t3UV, transformMatrix);
				Vector2 item2 = Es.InkPainter.Math.TextureCoordinateCalculation(intersectionInfo.intersectionPoint2, vector, t1UV, vector2, t2UV, vector3, t3UV, transformMatrix);
				this.drawPoints.Add(item);
				this.drawPoints.Add(item2);
			}
		}
		this.DrawPoints(this.startMeshFilter);
	}

	// Token: 0x060011B2 RID: 4530 RVA: 0x0005B125 File Offset: 0x00059325
	private IntersectionInfo CheckTriangle(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 u0, Vector3 u1, Vector3 u2)
	{
		return TriTriIntersection.TrisIntersect(v1, v2, v3, u0, u1, u2);
	}

	// Token: 0x04000D50 RID: 3408
	[SerializeField]
	private Renderer startRenderer;

	// Token: 0x04000D51 RID: 3409
	[SerializeField]
	private MeshFilter startMeshFilter;

	// Token: 0x04000D52 RID: 3410
	[SerializeField]
	private Texture2DArray outArray;

	// Token: 0x04000D53 RID: 3411
	[SerializeField]
	private Texture2D outTex;

	// Token: 0x04000D54 RID: 3412
	[SerializeField]
	private Transform transform0;

	// Token: 0x04000D55 RID: 3413
	[SerializeField]
	private Transform transform1;

	// Token: 0x04000D56 RID: 3414
	[SerializeField]
	private Transform transform2;

	// Token: 0x04000D57 RID: 3415
	[SerializeField]
	private Transform transform3;

	// Token: 0x04000D58 RID: 3416
	[SerializeField]
	private Transform transform4;

	// Token: 0x04000D59 RID: 3417
	[SerializeField]
	private Transform transform5;

	// Token: 0x04000D5A RID: 3418
	public Camera cam;

	// Token: 0x04000D5B RID: 3419
	private List<Vector2> drawPoints = new List<Vector2>();

	// Token: 0x04000D5C RID: 3420
	[Space]
	public bool mouseSingleClick;

	// Token: 0x04000D5D RID: 3421
	[Space]
	public Color paintColor;

	// Token: 0x04000D5E RID: 3422
	public float radius = 1f;

	// Token: 0x04000D5F RID: 3423
	public float strength = 1f;

	// Token: 0x04000D60 RID: 3424
	public float hardness = 1f;

	// Token: 0x04000D61 RID: 3425
	private int size = 2;

	// Token: 0x04000D62 RID: 3426
	private int[] tris;

	// Token: 0x04000D63 RID: 3427
	private Vector3[] vectors;

	// Token: 0x04000D64 RID: 3428
	private Vector2[] uvs;
}
