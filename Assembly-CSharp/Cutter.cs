using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

// Token: 0x02000181 RID: 385
public class Cutter : MonoBehaviour
{
	// Token: 0x06000C37 RID: 3127 RVA: 0x0000777A File Offset: 0x0000597A
	private void Start()
	{
	}

	// Token: 0x06000C38 RID: 3128 RVA: 0x0000777A File Offset: 0x0000597A
	private void Update()
	{
	}

	// Token: 0x06000C39 RID: 3129 RVA: 0x0003A494 File Offset: 0x00038694
	private void GetMaterial()
	{
		if (this.objectToCut != null)
		{
			Renderer component = this.objectCurrentlyBeingCut.GetComponent<Renderer>();
			if (component != null)
			{
				this.material = component.material;
			}
		}
	}

	// Token: 0x06000C3A RID: 3130 RVA: 0x0003A4D0 File Offset: 0x000386D0
	public void DoTheCut()
	{
		this.cutGameObjects = new List<CutObjectInfo>();
		this.cutJoints = new List<CutJoint>();
		this.point = base.gameObject.transform.position;
		this.normal = base.gameObject.transform.up;
		this.cutPlane = new Plane(this.normal, this.point);
		if (this.doCut && this.objectToCut != null)
		{
			this.HandleGameObject(this.objectToCut);
			foreach (object obj in this.objectToCut.transform)
			{
				Transform transform = (Transform)obj;
				this.HandleGameObject(transform.gameObject);
			}
			ConfigurableJoint component = this.objectToCut.transform.parent.GetComponent<ConfigurableJoint>();
			if (component != null)
			{
				this.HandleJoint(component, true);
			}
			this.HandleCutObjects();
			this.doCut = false;
		}
	}

	// Token: 0x06000C3B RID: 3131 RVA: 0x0003A5E8 File Offset: 0x000387E8
	private void HandleCutObjects()
	{
		bool flag = false;
		CutJoint cutJoint = (from x in this.cutJoints
		where x.isParentJoint
		select x).FirstOrDefault<CutJoint>();
		if (cutJoint != null)
		{
			flag = cutJoint.side;
		}
		GameObject gameObject = new GameObject("generatedCutObject");
		gameObject.transform.position = this.objectToCut.transform.position;
		gameObject.transform.localScale = this.objectToCut.transform.localScale;
		gameObject.transform.rotation = this.objectToCut.transform.rotation;
		Rigidbody component = this.objectToCut.GetComponent<Rigidbody>();
		Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
		if (component != null)
		{
			rigidbody.mass = component.mass;
			rigidbody.angularDrag = component.angularDrag;
			rigidbody.drag = component.drag;
		}
		foreach (CutObjectInfo cutObjectInfo in this.cutGameObjects)
		{
			if (cutObjectInfo.side == flag)
			{
				cutObjectInfo.GameObject.transform.parent = this.objectToCut.transform;
			}
			else
			{
				cutObjectInfo.GameObject.transform.parent = gameObject.transform;
			}
		}
		foreach (CutJoint cutJoint2 in this.cutJoints)
		{
			if (!cutJoint2.isParentJoint && cutJoint2.side != flag)
			{
				if (cutJoint2.joint.connectedBody.gameObject == this.objectToCut)
				{
					cutJoint2.joint.connectedBody = rigidbody;
					gameObject.transform.parent = cutJoint2.joint.gameObject.transform;
				}
				else
				{
					ConfigurableJoint configurableJoint = gameObject.AddComponent<ConfigurableJoint>();
					configurableJoint.autoConfigureConnectedAnchor = false;
					configurableJoint.connectedBody = cutJoint2.joint.connectedBody;
					configurableJoint.connectedBody.gameObject.transform.parent = gameObject.transform;
					configurableJoint.anchor = cutJoint2.joint.anchor;
					configurableJoint.axis = cutJoint2.joint.axis;
					configurableJoint.secondaryAxis = cutJoint2.joint.secondaryAxis;
					configurableJoint.connectedAnchor = cutJoint2.joint.connectedAnchor;
					configurableJoint.xMotion = cutJoint2.joint.xMotion;
					configurableJoint.yMotion = cutJoint2.joint.yMotion;
					configurableJoint.zMotion = cutJoint2.joint.zMotion;
					configurableJoint.xDrive = cutJoint2.joint.xDrive;
					configurableJoint.yDrive = cutJoint2.joint.yDrive;
					configurableJoint.zDrive = cutJoint2.joint.zDrive;
					UnityEngine.Object.Destroy(cutJoint2.joint);
				}
			}
		}
	}

	// Token: 0x06000C3C RID: 3132 RVA: 0x0003A90C File Offset: 0x00038B0C
	private void HandleGameObject(GameObject objectToHandle)
	{
		MeshRenderer component = objectToHandle.GetComponent<MeshRenderer>();
		if (component != null && component.enabled)
		{
			this.CutMesh(objectToHandle);
		}
		ConfigurableJoint component2 = objectToHandle.GetComponent<ConfigurableJoint>();
		if (component2 != null)
		{
			this.HandleJoint(component2, false);
		}
	}

	// Token: 0x06000C3D RID: 3133 RVA: 0x0003A950 File Offset: 0x00038B50
	private void HandleJoint(ConfigurableJoint joint, bool isParentJoint = false)
	{
		if (joint.connectedBody.gameObject == this.objectToCut || joint.gameObject == this.objectToCut)
		{
			bool side = this.cutPlane.GetSide(joint.gameObject.transform.TransformPoint(joint.anchor));
			this.cutJoints.Add(new CutJoint
			{
				joint = joint,
				side = side,
				isParentJoint = isParentJoint
			});
		}
	}

	// Token: 0x06000C3E RID: 3134 RVA: 0x0003A9D0 File Offset: 0x00038BD0
	private void CutMesh(GameObject cutObject)
	{
		List<VertexPairForCap> list = new List<VertexPairForCap>();
		this.objectCurrentlyBeingCut = cutObject;
		this.upMesh = new MeshInfo
		{
			name = "Up",
			side = true
		};
		this.downMesh = new MeshInfo
		{
			name = "Down",
			side = false
		};
		this.originalMesh = new MeshInfo();
		MeshFilter component = cutObject.GetComponent<MeshFilter>();
		Vector3[] vertices = component.mesh.vertices;
		Debug.Log(string.Format("VertCount: {0}", vertices.Length));
		this.originalMesh.vertices = component.mesh.vertices.ToList<Vector3>();
		this.originalMesh.gameObject = cutObject;
		component.mesh.GetUVs(0, this.originalMesh.oldUvs);
		foreach (Vector3 vector in this.originalMesh.vertices)
		{
			this.originalMesh.verticesOnWorld.Add(this.originalMesh.gameObject.transform.localToWorldMatrix.MultiplyPoint3x4(vector));
		}
		int[] triangles = component.mesh.triangles;
		bool flag = false;
		for (int i = 0; i < triangles.Length; i += 3)
		{
			float num = this.CompareLineToPlane(vertices[triangles[i]], vertices[triangles[i + 1]]);
			float num2 = this.CompareLineToPlane(vertices[triangles[i + 1]], vertices[triangles[i + 2]]);
			float num3 = this.CompareLineToPlane(vertices[triangles[i + 2]], vertices[triangles[i]]);
			if ((num > 0f && num < 1f) || (num2 > 0f && num2 < 1f) || (num3 > 0f && num3 < 1f))
			{
				flag = true;
				Vector3 vector2 = Generic.PointOnPlaneBetweenTwoPoints(this.cutPlane, this.originalMesh.verticesOnWorld[triangles[i]], this.originalMesh.verticesOnWorld[triangles[i + 1]]);
				Vector3 vector3 = Generic.PointOnPlaneBetweenTwoPoints(this.cutPlane, this.originalMesh.verticesOnWorld[triangles[i + 1]], this.originalMesh.verticesOnWorld[triangles[i + 2]]);
				Vector3 vector4 = Generic.PointOnPlaneBetweenTwoPoints(this.cutPlane, this.originalMesh.verticesOnWorld[triangles[i + 2]], this.originalMesh.verticesOnWorld[triangles[i]]);
				Vector3[] array = new Vector3[]
				{
					vector2,
					vector3,
					vector4
				};
				Vector2[] array2 = new Vector2[]
				{
					Vector2.zero,
					Vector2.zero,
					Vector2.zero
				};
				int[] array3 = new int[]
				{
					-1,
					-1,
					-1
				};
				int[] array4 = new int[6];
				bool[] array5 = new bool[3];
				bool flag2 = false;
				if (float.IsNaN(vector2.x) || float.IsInfinity(vector2.x) || Vector3.Distance(vector2, this.originalMesh.verticesOnWorld[triangles[i]]) > Vector3.Distance(this.originalMesh.verticesOnWorld[triangles[i]], this.originalMesh.verticesOnWorld[triangles[i + 1]]) || Vector3.Distance(vector2, this.originalMesh.verticesOnWorld[triangles[i + 1]]) > Vector3.Distance(this.originalMesh.verticesOnWorld[triangles[i]], this.originalMesh.verticesOnWorld[triangles[i + 1]]))
				{
					Debug.DrawRay(vector2, this.cutPlane.normal, Color.black);
					flag2 = (this.PointSideValue(vertices[triangles[i]]) < 0f);
					array3[1] = triangles[i + 2];
					array4[0] = triangles[i];
					array4[1] = triangles[i + 1];
				}
				else
				{
					array5[0] = true;
					float num4 = Vector3.Distance(this.originalMesh.verticesOnWorld[triangles[i]], this.originalMesh.verticesOnWorld[triangles[i + 1]]);
					float t = Vector3.Distance(this.originalMesh.verticesOnWorld[triangles[i]], vector2) / num4;
					array2[0] = Vector2.Lerp(this.originalMesh.oldUvs[triangles[i]], this.originalMesh.oldUvs[triangles[i + 1]], t);
				}
				if (float.IsNaN(vector3.x) || float.IsInfinity(vector3.x) || Vector3.Distance(vector3, this.originalMesh.verticesOnWorld[triangles[i + 1]]) > Vector3.Distance(this.originalMesh.verticesOnWorld[triangles[i + 1]], this.originalMesh.verticesOnWorld[triangles[i + 2]]) || Vector3.Distance(vector3, this.originalMesh.verticesOnWorld[triangles[i + 2]]) > Vector3.Distance(this.originalMesh.verticesOnWorld[triangles[i + 1]], this.originalMesh.verticesOnWorld[triangles[i + 2]]))
				{
					Debug.DrawRay(vector3, this.cutPlane.normal, Color.black);
					flag2 = (this.PointSideValue(vertices[triangles[i + 1]]) < 0f);
					array3[1] = triangles[i];
					array4[0] = triangles[i + 1];
					array4[1] = triangles[i + 2];
				}
				else
				{
					array5[1] = true;
					float num5 = Vector3.Distance(this.originalMesh.verticesOnWorld[triangles[i + 1]], this.originalMesh.verticesOnWorld[triangles[i + 2]]);
					float t2 = Vector3.Distance(this.originalMesh.verticesOnWorld[triangles[i + 1]], vector3) / num5;
					array2[1] = Vector2.Lerp(this.originalMesh.oldUvs[triangles[i + 1]], this.originalMesh.oldUvs[triangles[i + 2]], t2);
				}
				if (float.IsNaN(vector4.x) || float.IsInfinity(vector4.x) || Vector3.Distance(vector4, this.originalMesh.verticesOnWorld[triangles[i + 2]]) > Vector3.Distance(this.originalMesh.verticesOnWorld[triangles[i + 2]], this.originalMesh.verticesOnWorld[triangles[i]]) || Vector3.Distance(vector4, this.originalMesh.verticesOnWorld[triangles[i]]) > Vector3.Distance(this.originalMesh.verticesOnWorld[triangles[i + 2]], this.originalMesh.verticesOnWorld[triangles[i]]))
				{
					Debug.DrawRay(vector4, this.cutPlane.normal, Color.black);
					flag2 = (this.PointSideValue(vertices[triangles[i + 2]]) < 0f);
					array3[1] = triangles[i + 1];
					array4[0] = triangles[i + 2];
					array4[1] = triangles[i];
				}
				else
				{
					array5[2] = true;
					float num6 = Vector3.Distance(this.originalMesh.verticesOnWorld[triangles[i + 2]], this.originalMesh.verticesOnWorld[triangles[i]]);
					float t3 = Vector3.Distance(this.originalMesh.verticesOnWorld[triangles[i + 2]], vector4) / num6;
					array2[2] = Vector2.Lerp(this.originalMesh.oldUvs[triangles[i + 2]], this.originalMesh.oldUvs[triangles[i]], t3);
				}
				Debug.Log(string.Format("First break: {0} > {1}", Vector3.Distance(vector2, this.originalMesh.verticesOnWorld[triangles[i]]), Vector3.Distance(this.originalMesh.verticesOnWorld[triangles[i]], this.originalMesh.verticesOnWorld[triangles[i + 1]])));
				Debug.Log(string.Format(" OR: {0} > {1}", Vector3.Distance(vector2, this.originalMesh.verticesOnWorld[triangles[i + 1]]), Vector3.Distance(this.originalMesh.verticesOnWorld[triangles[i]], this.originalMesh.verticesOnWorld[triangles[i + 1]])));
				Debug.Log(string.Format("Second break: {0} > {1}", Vector3.Distance(vector3, this.originalMesh.verticesOnWorld[triangles[i + 1]]), Vector3.Distance(this.originalMesh.verticesOnWorld[triangles[i + 1]], this.originalMesh.verticesOnWorld[triangles[i + 2]])));
				Debug.Log(string.Format(" OR: {0} > {1}", Vector3.Distance(vector3, this.originalMesh.verticesOnWorld[triangles[i + 2]]), Vector3.Distance(this.originalMesh.verticesOnWorld[triangles[i + 1]], this.originalMesh.verticesOnWorld[triangles[i + 2]])));
				Debug.Log(string.Format("Second break: {0} > {1}", Vector3.Distance(vector4, this.originalMesh.verticesOnWorld[triangles[i + 2]]), Vector3.Distance(this.originalMesh.verticesOnWorld[triangles[i + 2]], this.originalMesh.verticesOnWorld[triangles[i]])));
				Debug.Log(string.Format(" OR: {0} > {1}", Vector3.Distance(vector4, this.originalMesh.verticesOnWorld[triangles[i]]), Vector3.Distance(this.originalMesh.verticesOnWorld[triangles[i + 2]], this.originalMesh.verticesOnWorld[triangles[i]])));
				int num7 = 0;
				if (array5[1] && array5[2])
				{
					num7 = 1;
				}
				else if (array5[2] && array5[0])
				{
					num7 = 2;
				}
				int num8 = num7 % 3;
				int num9 = (1 + num7) % 3;
				int num10 = (2 + num7) % 3;
				int num11 = this.AddNewVertex(array[num8], array2[num8]);
				int num12 = this.AddNewVertex(array[num9], array2[num9]);
				array3[0] = num11;
				array3[1] = triangles[i + num9];
				array3[2] = num12;
				array4[0] = triangles[i + num8];
				array4[1] = num11;
				array4[2] = num12;
				array4[3] = triangles[i + num8];
				array4[4] = num12;
				array4[5] = triangles[i + num10];
				this.DrawTri(array3, Color.blue);
				this.DrawTri(new int[]
				{
					array4[0],
					array4[1],
					array4[2]
				}, Color.blue);
				this.DrawTri(new int[]
				{
					array4[3],
					array4[4],
					array4[5]
				}, Color.blue);
				if (flag2)
				{
					list.Add(new VertexPairForCap
					{
						vertex1 = num11,
						vertex2 = num12
					});
					this.upMesh.oldMeshTriangles.Add(array3[0]);
					this.upMesh.oldMeshTriangles.Add(array3[1]);
					this.upMesh.oldMeshTriangles.Add(array3[2]);
					this.downMesh.oldMeshTriangles.Add(array4[0]);
					this.downMesh.oldMeshTriangles.Add(array4[1]);
					this.downMesh.oldMeshTriangles.Add(array4[2]);
					this.downMesh.oldMeshTriangles.Add(array4[3]);
					this.downMesh.oldMeshTriangles.Add(array4[4]);
					this.downMesh.oldMeshTriangles.Add(array4[5]);
				}
				else
				{
					list.Add(new VertexPairForCap
					{
						vertex1 = num12,
						vertex2 = num11
					});
					this.downMesh.oldMeshTriangles.Add(array3[0]);
					this.downMesh.oldMeshTriangles.Add(array3[1]);
					this.downMesh.oldMeshTriangles.Add(array3[2]);
					this.upMesh.oldMeshTriangles.Add(array4[0]);
					this.upMesh.oldMeshTriangles.Add(array4[1]);
					this.upMesh.oldMeshTriangles.Add(array4[2]);
					this.upMesh.oldMeshTriangles.Add(array4[3]);
					this.upMesh.oldMeshTriangles.Add(array4[4]);
					this.upMesh.oldMeshTriangles.Add(array4[5]);
				}
			}
			else
			{
				float num13 = this.PointSideValue(vertices[triangles[i]]);
				Vector3 start = cutObject.transform.localToWorldMatrix.MultiplyPoint3x4((vertices[triangles[i]] + vertices[triangles[i + 1]] + vertices[triangles[i + 2]]) / 3f);
				if (num13 > 0f)
				{
					this.upMesh.oldMeshTriangles.Add(triangles[i]);
					this.upMesh.oldMeshTriangles.Add(triangles[i + 1]);
					this.upMesh.oldMeshTriangles.Add(triangles[i + 2]);
					Debug.DrawRay(start, base.gameObject.transform.up, Color.cyan);
				}
				else
				{
					this.downMesh.oldMeshTriangles.Add(triangles[i]);
					this.downMesh.oldMeshTriangles.Add(triangles[i + 1]);
					this.downMesh.oldMeshTriangles.Add(triangles[i + 2]);
					Debug.DrawRay(start, -base.gameObject.transform.up, Color.magenta);
				}
			}
		}
		if (flag)
		{
			if (this.doCut)
			{
				this.GetMaterial();
				this.AddCaps(list);
				this.HandleNewMesh(this.upMesh, this.originalMesh);
				this.HandleNewMesh(this.downMesh, this.originalMesh);
				this.CreateMesh(this.upMesh, this.originalMesh);
				this.CreateMesh(this.downMesh, this.originalMesh);
				cutObject.SetActive(false);
				return;
			}
		}
		else
		{
			this.cutGameObjects.Add(new CutObjectInfo
			{
				GameObject = cutObject,
				side = (this.upMesh.oldMeshTriangles.Count > 0)
			});
		}
	}

	// Token: 0x06000C3F RID: 3135 RVA: 0x0003B8AC File Offset: 0x00039AAC
	private int AddNewVertex(Vector3 worldPos, Vector2 uv)
	{
		Vector3 item = this.originalMesh.gameObject.transform.worldToLocalMatrix.MultiplyPoint3x4(worldPos);
		this.originalMesh.newVertices.Add(item);
		this.originalMesh.newUvs.Add(uv);
		return this.originalMesh.vertices.Count + this.originalMesh.newVertices.Count - 1;
	}

	// Token: 0x06000C40 RID: 3136 RVA: 0x0003B920 File Offset: 0x00039B20
	private void AddCaps(List<VertexPairForCap> vertexPairForCaps)
	{
		SubMesh subMesh = new SubMesh
		{
			material = this.innerMaterial
		};
		SubMesh subMesh2 = new SubMesh
		{
			material = this.innerMaterial
		};
		Vector3 vector = Vector3.zero;
		for (int i = 0; i < this.originalMesh.newVertices.Count; i++)
		{
			vector += this.originalMesh.newVertices[i];
		}
		vector /= (float)this.originalMesh.newVertices.Count;
		this.originalMesh.newVertices.Add(vector);
		this.originalMesh.newUvs.Add(Vector2.zero);
		int item = this.originalMesh.vertices.Count + this.originalMesh.newVertices.Count - 1;
		for (int j = 0; j < vertexPairForCaps.Count; j++)
		{
			subMesh.triangles.Add(item);
			subMesh.triangles.Add(vertexPairForCaps[j].vertex1);
			subMesh.triangles.Add(vertexPairForCaps[j].vertex2);
			subMesh2.triangles.Add(item);
			subMesh2.triangles.Add(vertexPairForCaps[j].vertex2);
			subMesh2.triangles.Add(vertexPairForCaps[j].vertex1);
		}
		this.upMesh.oldSubMeshes.Add(subMesh);
		this.downMesh.oldSubMeshes.Add(subMesh2);
	}

	// Token: 0x06000C41 RID: 3137 RVA: 0x0003BAA4 File Offset: 0x00039CA4
	private void CreateMesh(MeshInfo newMeshInfo, MeshInfo oldMesh)
	{
		Mesh mesh = new Mesh();
		List<Material> list = new List<Material>();
		mesh.vertices = newMeshInfo.vertices.ToArray();
		mesh.subMeshCount = newMeshInfo.subMeshes.Count + 1;
		mesh.SetTriangles(newMeshInfo.triangles.ToArray(), 0);
		mesh.SetUVs(0, newMeshInfo.uvs);
		list.Add(this.material);
		int num = 1;
		foreach (SubMesh subMesh in newMeshInfo.subMeshes)
		{
			mesh.SetTriangles(subMesh.triangles, num);
			list.Add(subMesh.material);
			num++;
		}
		GameObject gameObject = new GameObject("generatedMesh" + newMeshInfo.name, new Type[]
		{
			typeof(MeshFilter),
			typeof(MeshRenderer)
		});
		gameObject.transform.position = oldMesh.gameObject.transform.position;
		gameObject.transform.localScale = oldMesh.gameObject.transform.localScale;
		gameObject.transform.rotation = oldMesh.gameObject.transform.rotation;
		gameObject.GetComponent<MeshFilter>().mesh = mesh;
		this.cutGameObjects.Add(new CutObjectInfo
		{
			GameObject = gameObject,
			side = newMeshInfo.side
		});
		gameObject.GetComponent<MeshRenderer>().materials = list.ToArray();
	}

	// Token: 0x06000C42 RID: 3138 RVA: 0x0003BC38 File Offset: 0x00039E38
	private void HandleNewMesh(MeshInfo newMeshInfo, MeshInfo oldMesh)
	{
		int num = 0;
		foreach (int num2 in newMeshInfo.oldMeshTriangles)
		{
			if (num2 >= oldMesh.vertices.Count)
			{
				int index = num2 - this.originalMesh.vertices.Count;
				newMeshInfo.vertices.Add(this.originalMesh.newVertices[index]);
				newMeshInfo.uvs.Add(oldMesh.newUvs[index]);
				newMeshInfo.triangles.Add(num);
				num++;
			}
			else
			{
				Debug.Log(oldMesh.vertices[num2]);
				newMeshInfo.vertices.Add(oldMesh.vertices[num2]);
				newMeshInfo.triangles.Add(num);
				newMeshInfo.uvs.Add(oldMesh.oldUvs[num2]);
				num++;
			}
		}
		foreach (SubMesh subMesh in newMeshInfo.oldSubMeshes)
		{
			SubMesh subMesh2 = new SubMesh
			{
				material = this.innerMaterial
			};
			foreach (int num3 in subMesh.triangles)
			{
				if (num3 >= oldMesh.vertices.Count)
				{
					int index2 = num3 - this.originalMesh.vertices.Count;
					newMeshInfo.vertices.Add(this.originalMesh.newVertices[index2]);
					newMeshInfo.uvs.Add(this.originalMesh.newUvs[index2]);
					subMesh2.triangles.Add(num);
					num++;
				}
				else
				{
					Debug.Log(oldMesh.vertices[num3]);
					newMeshInfo.vertices.Add(oldMesh.vertices[num3]);
					subMesh2.triangles.Add(num);
					newMeshInfo.uvs.Add(oldMesh.oldUvs[num3]);
					num++;
				}
			}
			newMeshInfo.subMeshes.Add(subMesh2);
		}
	}

	// Token: 0x06000C43 RID: 3139 RVA: 0x0003BED8 File Offset: 0x0003A0D8
	private float CompareLineToPlane(Vector3 lineStart, Vector3 lineEnd)
	{
		lineStart = this.objectCurrentlyBeingCut.transform.localToWorldMatrix.MultiplyPoint3x4(lineStart);
		lineEnd = this.objectCurrentlyBeingCut.transform.localToWorldMatrix.MultiplyPoint3x4(lineEnd);
		float num = Vector3.Dot(this.normal, this.point - lineStart) / Vector3.Dot(this.normal, lineEnd - lineStart);
		if (num > 0f && num < 1f)
		{
			Debug.DrawLine(lineStart, lineEnd, Color.red);
		}
		else
		{
			Debug.DrawLine(lineStart, lineEnd, Color.green);
		}
		return num;
	}

	// Token: 0x06000C44 RID: 3140 RVA: 0x0003BF74 File Offset: 0x0003A174
	public float PointSideValue(Vector3 meshPoint)
	{
		meshPoint = this.objectCurrentlyBeingCut.transform.localToWorldMatrix.MultiplyPoint3x4(meshPoint);
		return Vector3.Dot(this.normal, meshPoint - this.point);
	}

	// Token: 0x06000C45 RID: 3141 RVA: 0x0003BFB4 File Offset: 0x0003A1B4
	public void DrawTri(int[] tri, Color color)
	{
		Debug.DrawLine(this.GetVertWorldPos(tri[0]), this.GetVertWorldPos(tri[1]), color);
		Debug.DrawLine(this.GetVertWorldPos(tri[1]), this.GetVertWorldPos(tri[2]), color);
		Debug.DrawLine(this.GetVertWorldPos(tri[2]), this.GetVertWorldPos(tri[0]), color);
	}

	// Token: 0x06000C46 RID: 3142 RVA: 0x0003C00C File Offset: 0x0003A20C
	public Vector3 GetVertWorldPos(int vert)
	{
		if (vert >= this.originalMesh.vertices.Count)
		{
			return this.originalMesh.gameObject.transform.localToWorldMatrix.MultiplyPoint3x4(this.originalMesh.newVertices[vert - this.originalMesh.vertices.Count]);
		}
		return this.originalMesh.verticesOnWorld[vert];
	}

	// Token: 0x040008A0 RID: 2208
	public GameObject objectToCut;

	// Token: 0x040008A1 RID: 2209
	private Vector3 point;

	// Token: 0x040008A2 RID: 2210
	private Vector3 normal;

	// Token: 0x040008A3 RID: 2211
	private Plane cutPlane;

	// Token: 0x040008A4 RID: 2212
	private MeshInfo upMesh;

	// Token: 0x040008A5 RID: 2213
	private MeshInfo downMesh;

	// Token: 0x040008A6 RID: 2214
	private MeshInfo originalMesh;

	// Token: 0x040008A7 RID: 2215
	public Material material;

	// Token: 0x040008A8 RID: 2216
	public Material innerMaterial;

	// Token: 0x040008A9 RID: 2217
	public bool doCut;

	// Token: 0x040008AA RID: 2218
	public List<CutObjectInfo> cutGameObjects;

	// Token: 0x040008AB RID: 2219
	public List<CutJoint> cutJoints;

	// Token: 0x040008AC RID: 2220
	private GameObject objectCurrentlyBeingCut;
}
