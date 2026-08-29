using System;
using UnityEngine;

// Token: 0x02000188 RID: 392
public class GenerateMesh : MonoBehaviour
{
	// Token: 0x06000C5E RID: 3166 RVA: 0x0003C1A0 File Offset: 0x0003A3A0
	private void Start()
	{
		Vector3[] array = new Vector3[4];
		Vector2[] array2 = new Vector2[4];
		int[] array3 = new int[6];
		array[0] = new Vector3(0f, 1f);
		array[1] = new Vector3(1f, 1f);
		array[2] = new Vector3(0f, 0f);
		array[3] = new Vector3(1f, 0f);
		array2[0] = new Vector2(0f, 1f);
		array2[1] = new Vector2(1f, 1f);
		array2[2] = new Vector2(0f, 0f);
		array2[3] = new Vector2(1f, 0f);
		array3[0] = 0;
		array3[1] = 1;
		array3[2] = 2;
		array3[3] = 2;
		array3[4] = 1;
		array3[5] = 3;
		Mesh mesh = new Mesh();
		mesh.vertices = array;
		mesh.uv = array2;
		mesh.triangles = array3;
		GameObject gameObject = new GameObject("generatedMesh", new Type[]
		{
			typeof(MeshFilter),
			typeof(MeshRenderer)
		});
		gameObject.GetComponent<MeshFilter>().mesh = mesh;
		gameObject.GetComponent<MeshRenderer>().material = this.material;
	}

	// Token: 0x06000C5F RID: 3167 RVA: 0x0000777A File Offset: 0x0000597A
	private void Update()
	{
	}

	// Token: 0x040008C6 RID: 2246
	public Material material;
}
