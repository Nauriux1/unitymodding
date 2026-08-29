using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000185 RID: 389
public class MeshInfo
{
	// Token: 0x17000187 RID: 391
	// (get) Token: 0x06000C55 RID: 3157 RVA: 0x0003C0D5 File Offset: 0x0003A2D5
	// (set) Token: 0x06000C56 RID: 3158 RVA: 0x0003C0DD File Offset: 0x0003A2DD
	public string name { get; set; }

	// Token: 0x040008B5 RID: 2229
	public bool side;

	// Token: 0x040008B6 RID: 2230
	public List<Vector3> vertices = new List<Vector3>();

	// Token: 0x040008B7 RID: 2231
	public List<int> oldMeshTriangles = new List<int>();

	// Token: 0x040008B8 RID: 2232
	public List<int> triangles = new List<int>();

	// Token: 0x040008B9 RID: 2233
	public List<SubMesh> subMeshes = new List<SubMesh>();

	// Token: 0x040008BA RID: 2234
	public List<SubMesh> oldSubMeshes = new List<SubMesh>();

	// Token: 0x040008BB RID: 2235
	public List<Vector3> newVertices = new List<Vector3>();

	// Token: 0x040008BC RID: 2236
	public List<Vector2> uvs = new List<Vector2>();

	// Token: 0x040008BD RID: 2237
	public List<Vector2> newUvs = new List<Vector2>();

	// Token: 0x040008BE RID: 2238
	public List<Vector2> oldUvs = new List<Vector2>();

	// Token: 0x040008BF RID: 2239
	public GameObject gameObject;

	// Token: 0x040008C0 RID: 2240
	public List<Vector3> verticesOnWorld = new List<Vector3>();
}
