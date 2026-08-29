using System;
using System.Collections.Generic;
using UnityEngine;

namespace MeshSplitting.MeshTools
{
	// Token: 0x020002DE RID: 734
	public class MeshContainer
	{
		// Token: 0x06001658 RID: 5720 RVA: 0x0006EA09 File Offset: 0x0006CC09
		public MeshContainer(MeshFilter meshFilter)
		{
			this.Mesh = meshFilter.mesh;
			this.transform = meshFilter.GetComponent<Transform>();
			this.isAnimated = false;
		}

		// Token: 0x06001659 RID: 5721 RVA: 0x0006EA30 File Offset: 0x0006CC30
		public MeshContainer(SkinnedMeshRenderer skinnedRenderer)
		{
			this.Mesh = skinnedRenderer.sharedMesh;
			this.transform = skinnedRenderer.GetComponent<Transform>();
			this.bones = skinnedRenderer.bones;
			this.isAnimated = true;
		}

		// Token: 0x0600165A RID: 5722 RVA: 0x0006EA64 File Offset: 0x0006CC64
		public void MeshInitialize()
		{
			this.vertexCount = this.Mesh.vertexCount;
			this.vertices = this.Mesh.vertices;
			this.wsVerts = this.Mesh.vertices;
			this.triangles = this.Mesh.triangles;
			this.normals = this.Mesh.normals;
			this.tangents = this.Mesh.tangents;
			this.uv = this.Mesh.uv;
			this.uv2 = this.Mesh.uv2;
			this.colors = this.Mesh.colors;
			this.boneWeights = this.Mesh.boneWeights;
			this.bindPoses = this.Mesh.bindposes;
			int capacity = this.vertexCount / 2;
			if (this.wsVerts.Length != 0)
			{
				this.wsVertsNew = new List<Vector3>(capacity);
			}
			if (this.vertices.Length != 0)
			{
				this.verticesNew = new List<Vector3>(capacity);
			}
			if (this.normals.Length != 0)
			{
				this.normalsNew = new List<Vector3>(capacity);
			}
			if (this.tangents.Length != 0)
			{
				this.tangentsNew = new List<Vector4>(capacity);
			}
			if (this.uv.Length != 0)
			{
				this.uvNew = new List<Vector2>(capacity);
			}
			if (this.uv2.Length != 0)
			{
				this.uv2New = new List<Vector2>(capacity);
			}
			if (this.colors.Length != 0)
			{
				this.colorsNew = new List<Color>(capacity);
			}
			if (this.boneWeights.Length != 0)
			{
				this.boneWeightsNew = new List<BoneWeight>(capacity);
			}
			int capacity2 = (int)((float)this.triangles.Length * 1.5f);
			this.trisUp = new List<int>(capacity2);
			this.trisDown = new List<int>(capacity2);
		}

		// Token: 0x0600165B RID: 5723 RVA: 0x0006EC06 File Offset: 0x0006CE06
		public void CalculateWorldSpace()
		{
			if (!this.isAnimated)
			{
				this.CalculateWorldSpaceStatic();
				return;
			}
			this.CalculateWorldSpaceAnimated();
		}

		// Token: 0x0600165C RID: 5724 RVA: 0x0006EC20 File Offset: 0x0006CE20
		private void CalculateWorldSpaceStatic()
		{
			Matrix4x4 localToWorldMatrix = this.transform.localToWorldMatrix;
			int num = this.wsVerts.Length;
			for (int i = 0; i < num; i++)
			{
				this.wsVerts[i] = localToWorldMatrix.MultiplyPoint3x4(this.wsVerts[i]);
			}
		}

		// Token: 0x0600165D RID: 5725 RVA: 0x0006EC70 File Offset: 0x0006CE70
		private void CalculateWorldSpaceAnimated()
		{
			int num = this.bones.Length;
			Matrix4x4[] array = new Matrix4x4[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = this.bones[i].localToWorldMatrix * this.bindPoses[i];
			}
			Matrix4x4 identity = Matrix4x4.identity;
			int num2 = this.wsVerts.Length;
			for (int j = 0; j < num2; j++)
			{
				BoneWeight boneWeight = this.boneWeights[j];
				float weight = boneWeight.weight0;
				float weight2 = boneWeight.weight1;
				float weight3 = boneWeight.weight2;
				float weight4 = boneWeight.weight3;
				Matrix4x4 matrix4x = array[boneWeight.boneIndex0];
				Matrix4x4 matrix4x2 = array[boneWeight.boneIndex1];
				Matrix4x4 matrix4x3 = array[boneWeight.boneIndex2];
				Matrix4x4 matrix4x4 = array[boneWeight.boneIndex3];
				for (int k = 0; k < 3; k++)
				{
					for (int l = 0; l < 4; l++)
					{
						int index = k + l * 4;
						identity[index] = matrix4x[index] * weight + matrix4x2[index] * weight2 + matrix4x3[index] * weight3 + matrix4x4[index] * weight4;
					}
				}
				this.wsVerts[j] = identity.MultiplyPoint3x4(this.wsVerts[j]);
			}
		}

		// Token: 0x0600165E RID: 5726 RVA: 0x0006EDE0 File Offset: 0x0006CFE0
		public bool IsMeshSplit()
		{
			return this.HasMeshUpper() && this.HasMeshLower();
		}

		// Token: 0x0600165F RID: 5727 RVA: 0x0006EDF2 File Offset: 0x0006CFF2
		public bool HasMeshUpper()
		{
			return this.trisUp.Count > 0;
		}

		// Token: 0x06001660 RID: 5728 RVA: 0x0006EE02 File Offset: 0x0006D002
		public bool HasMeshLower()
		{
			return this.trisDown.Count > 0;
		}

		// Token: 0x06001661 RID: 5729 RVA: 0x0006EE12 File Offset: 0x0006D012
		public Mesh CreateMeshUpper()
		{
			return this.CreateMesh(this.trisUp);
		}

		// Token: 0x06001662 RID: 5730 RVA: 0x0006EE20 File Offset: 0x0006D020
		public Mesh CreateMeshLower()
		{
			return this.CreateMesh(this.trisDown);
		}

		// Token: 0x06001663 RID: 5731 RVA: 0x0006EE30 File Offset: 0x0006D030
		private Mesh CreateMesh(List<int> tris)
		{
			int count = tris.Count;
			int[] array = new int[count];
			int num = this.vertexCount + this.verticesNew.Count;
			int[] array2 = new int[num];
			for (int i = 0; i < num; i++)
			{
				array2[i] = -1;
			}
			int num2 = 0;
			for (int j = 0; j < count; j++)
			{
				int num3 = tris[j];
				if (array2[num3] == -1)
				{
					array2[num3] = num2++;
				}
				array[j] = array2[num3];
			}
			Vector3[] array3 = new Vector3[num2];
			Vector3[] array4 = (this.normals.Length != 0) ? new Vector3[num2] : this.normals;
			Vector4[] array5 = (this.tangents.Length != 0) ? new Vector4[num2] : this.tangents;
			Vector2[] array6 = (this.uv.Length != 0) ? new Vector2[num2] : this.uv;
			Vector2[] array7 = (this.uv2.Length != 0) ? new Vector2[num2] : this.uv2;
			Color[] array8 = (this.colors.Length != 0) ? new Color[num2] : this.colors;
			BoneWeight[] array9 = (this.boneWeights.Length != 0) ? new BoneWeight[num2] : this.boneWeights;
			num2 = 0;
			for (int k = 0; k < count; k++)
			{
				int num4 = tris[k];
				if (array2[num4] >= num2)
				{
					if (num4 < this.vertexCount)
					{
						array3[num2] = this.vertices[num4];
						if (this.normalsNew != null)
						{
							array4[num2] = this.normals[num4];
						}
						if (this.tangentsNew != null)
						{
							array5[num2] = this.tangents[num4];
						}
						if (this.uvNew != null)
						{
							array6[num2] = this.uv[num4];
						}
						if (this.uv2New != null)
						{
							array7[num2] = this.uv2[num4];
						}
						if (this.colorsNew != null)
						{
							array8[num2] = this.colors[num4];
						}
						if (this.boneWeightsNew != null)
						{
							array9[num2] = this.boneWeights[num4];
						}
					}
					else
					{
						num4 -= this.vertexCount;
						array3[num2] = this.verticesNew[num4];
						if (this.normalsNew != null)
						{
							array4[num2] = this.normalsNew[num4];
						}
						if (this.tangentsNew != null)
						{
							array5[num2] = this.tangentsNew[num4];
						}
						if (this.uvNew != null)
						{
							array6[num2] = this.uvNew[num4];
						}
						if (this.uv2New != null)
						{
							array7[num2] = this.uv2New[num4];
						}
						if (this.colorsNew != null)
						{
							array8[num2] = this.colorsNew[num4];
						}
						if (this.boneWeightsNew != null)
						{
							array9[num2] = this.boneWeightsNew[num4];
						}
					}
					num2++;
				}
			}
			Mesh mesh = new Mesh();
			mesh.vertices = array3;
			mesh.normals = array4;
			mesh.tangents = array5;
			mesh.uv = array6;
			mesh.uv2 = array7;
			mesh.colors = array8;
			mesh.boneWeights = array9;
			mesh.triangles = array;
			mesh.bindposes = this.bindPoses;
			mesh.RecalculateBounds();
			return mesh;
		}

		// Token: 0x06001664 RID: 5732 RVA: 0x0006F194 File Offset: 0x0006D394
		public int AddLerpVertex(int from, int to, float t)
		{
			int result = this.vertexCount + this.verticesNew.Count;
			this.verticesNew.Add(Vector3.Lerp(this.vertices[from], this.vertices[to], t));
			this.wsVertsNew.Add(Vector3.Lerp(this.wsVerts[from], this.wsVerts[to], t));
			if (this.normalsNew != null)
			{
				this.normalsNew.Add(Vector3.Lerp(this.normals[from], this.normals[to], t));
			}
			if (this.tangentsNew != null)
			{
				this.tangentsNew.Add(Vector4.Lerp(this.tangents[from], this.tangents[to], t));
			}
			if (this.uvNew != null)
			{
				this.uvNew.Add(Vector2.Lerp(this.uv[from], this.uv[to], t));
			}
			if (this.uv2New != null)
			{
				this.uv2New.Add(Vector2.Lerp(this.uv2[from], this.uv2[to], t));
			}
			if (this.colorsNew != null)
			{
				this.colorsNew.Add(Color.Lerp(this.colors[from], this.colors[to], t));
			}
			if (this.boneWeightsNew != null)
			{
				this.boneWeightsNew.Add((t >= 0.5f) ? this.boneWeights[to] : this.boneWeights[from]);
			}
			return result;
		}

		// Token: 0x06001665 RID: 5733 RVA: 0x0006F330 File Offset: 0x0006D530
		public int AddCapVertex(int refIndex, Vector3 normal)
		{
			if (this.uvNew == null)
			{
				return this.AddCapVertex(refIndex, normal, MeshContainer.Vector2Zero);
			}
			if (refIndex >= this.vertexCount)
			{
				return this.AddCapVertex(refIndex, normal, this.uvNew[refIndex - this.vertexCount]);
			}
			return this.AddCapVertex(refIndex, normal, this.uv[refIndex]);
		}

		// Token: 0x06001666 RID: 5734 RVA: 0x0006F38C File Offset: 0x0006D58C
		public int AddCapVertex(int refIndex, Vector3 normal, Vector2 capUV)
		{
			int result = this.vertexCount + this.verticesNew.Count;
			bool flag = true;
			if (refIndex >= this.vertexCount)
			{
				refIndex -= this.vertexCount;
				flag = false;
			}
			if (flag)
			{
				this.verticesNew.Add(this.vertices[refIndex]);
				if (this.uv2New != null)
				{
					this.uv2New.Add(this.uv2[refIndex]);
				}
				if (this.colorsNew != null)
				{
					this.colorsNew.Add(this.colors[refIndex]);
				}
				if (this.boneWeightsNew != null)
				{
					this.boneWeightsNew.Add(this.boneWeights[refIndex]);
				}
			}
			else
			{
				this.verticesNew.Add(this.verticesNew[refIndex]);
				if (this.uv2New != null)
				{
					this.uv2New.Add(this.uv2New[refIndex]);
				}
				if (this.colorsNew != null)
				{
					this.colorsNew.Add(this.colorsNew[refIndex]);
				}
				if (this.boneWeightsNew != null)
				{
					this.boneWeightsNew.Add(this.boneWeightsNew[refIndex]);
				}
			}
			if (this.normalsNew != null)
			{
				this.normalsNew.Add(normal);
			}
			if (this.uvNew != null)
			{
				this.uvNew.Add(capUV);
			}
			if (this.tangentsNew != null)
			{
				Vector4 vector4Zero = MeshContainer.Vector4Zero;
				Vector3 vector = Vector3.Cross(normal, MeshContainer.Vector3Fwd);
				Vector3 vector2 = Vector3.Cross(normal, MeshContainer.Vector3Up);
				if (vector.sqrMagnitude > vector2.sqrMagnitude)
				{
					vector4Zero.x = vector.x;
					vector4Zero.y = vector.y;
					vector4Zero.z = vector.z;
				}
				else
				{
					vector4Zero.x = vector2.x;
					vector4Zero.y = vector2.y;
					vector4Zero.z = vector2.z;
				}
				this.tangentsNew.Add(vector4Zero.normalized);
			}
			return result;
		}

		// Token: 0x04001019 RID: 4121
		public Mesh Mesh;

		// Token: 0x0400101A RID: 4122
		public Transform transform;

		// Token: 0x0400101B RID: 4123
		public Transform[] bones;

		// Token: 0x0400101C RID: 4124
		public bool isAnimated;

		// Token: 0x0400101D RID: 4125
		public int vertexCount;

		// Token: 0x0400101E RID: 4126
		public Vector3[] wsVerts;

		// Token: 0x0400101F RID: 4127
		public Vector3[] vertices;

		// Token: 0x04001020 RID: 4128
		public Vector3[] normals;

		// Token: 0x04001021 RID: 4129
		public Vector4[] tangents;

		// Token: 0x04001022 RID: 4130
		public Vector2[] uv;

		// Token: 0x04001023 RID: 4131
		public Vector2[] uv2;

		// Token: 0x04001024 RID: 4132
		public Color[] colors;

		// Token: 0x04001025 RID: 4133
		public BoneWeight[] boneWeights;

		// Token: 0x04001026 RID: 4134
		public int[] triangles;

		// Token: 0x04001027 RID: 4135
		public Matrix4x4[] bindPoses;

		// Token: 0x04001028 RID: 4136
		public List<Vector3> wsVertsNew;

		// Token: 0x04001029 RID: 4137
		public List<Vector3> verticesNew;

		// Token: 0x0400102A RID: 4138
		public List<Vector3> normalsNew;

		// Token: 0x0400102B RID: 4139
		public List<Vector4> tangentsNew;

		// Token: 0x0400102C RID: 4140
		public List<Vector2> uvNew;

		// Token: 0x0400102D RID: 4141
		public List<Vector2> uv2New;

		// Token: 0x0400102E RID: 4142
		public List<Color> colorsNew;

		// Token: 0x0400102F RID: 4143
		public List<BoneWeight> boneWeightsNew;

		// Token: 0x04001030 RID: 4144
		public List<int> trisUp;

		// Token: 0x04001031 RID: 4145
		public List<int> trisDown;

		// Token: 0x04001032 RID: 4146
		private static Vector2 Vector2Zero = Vector2.zero;

		// Token: 0x04001033 RID: 4147
		private static Vector3 Vector3Up = Vector3.up;

		// Token: 0x04001034 RID: 4148
		private static Vector3 Vector3Fwd = Vector3.forward;

		// Token: 0x04001035 RID: 4149
		private static Vector4 Vector4Zero = Vector4.zero;
	}
}
