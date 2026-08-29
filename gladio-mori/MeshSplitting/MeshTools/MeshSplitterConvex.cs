using System;
using System.Collections.Generic;
using MeshSplitting.SplitterMath;
using UnityEngine;

namespace MeshSplitting.MeshTools
{
	// Token: 0x020002E1 RID: 737
	public class MeshSplitterConvex : IMeshSplitter
	{
		// Token: 0x06001688 RID: 5768 RVA: 0x00070F7C File Offset: 0x0006F17C
		public MeshSplitterConvex(MeshContainer meshContainer, PlaneMath splitPlane, Quaternion splitRotation)
		{
			this._mesh = meshContainer;
			this._splitPlane = splitPlane;
			this._splitRotation = splitRotation;
			this._ownRotation = meshContainer.transform.rotation;
			this.capInds = new List<int>(meshContainer.vertexCount / 10);
		}

		// Token: 0x06001689 RID: 5769 RVA: 0x00071027 File Offset: 0x0006F227
		public void SetCapUV(bool useCapUV, bool customUV, Vector2 uvMin, Vector2 uvMax)
		{
			this.UseCapUV = useCapUV;
			this.CustomUV = customUV;
			this.CapUVMin = uvMin;
			this.CapUVMax = uvMax;
		}

		// Token: 0x0600168A RID: 5770 RVA: 0x00071048 File Offset: 0x0006F248
		public void MeshSplit()
		{
			int num = this._mesh.triangles.Length - 2;
			for (int i = 0; i < num; i += 3)
			{
				this.triIndicies[0] = this._mesh.triangles[i];
				this.triIndicies[1] = this._mesh.triangles[1 + i];
				this.triIndicies[2] = this._mesh.triangles[2 + i];
				this.lineLerp[0] = this._splitPlane.LineIntersect(this._mesh.wsVerts[this.triIndicies[0]], this._mesh.wsVerts[this.triIndicies[1]]);
				this.lineLerp[1] = this._splitPlane.LineIntersect(this._mesh.wsVerts[this.triIndicies[1]], this._mesh.wsVerts[this.triIndicies[2]]);
				this.lineLerp[2] = this._splitPlane.LineIntersect(this._mesh.wsVerts[this.triIndicies[2]], this._mesh.wsVerts[this.triIndicies[0]]);
				this.lineHit[0] = (this.lineLerp[0] > 0f && this.lineLerp[0] < 1f);
				this.lineHit[1] = (this.lineLerp[1] > 0f && this.lineLerp[1] < 1f);
				this.lineHit[2] = (this.lineLerp[2] > 0f && this.lineLerp[2] < 1f);
				if (this.lineHit[0] || this.lineHit[1] || this.lineHit[2])
				{
					if (this.lineHit[0] && this.lineHit[1])
					{
						this.SplitTriangle(0);
					}
					else if (this.lineHit[1] && this.lineHit[2])
					{
						this.SplitTriangle(1);
					}
					else if (this.lineHit[0] && this.lineHit[2])
					{
						this.SplitTriangle(2);
					}
					else if (this.lineHit[1])
					{
						this.SplitTriangleAlternative(0);
					}
					else if (this.lineHit[2])
					{
						this.SplitTriangleAlternative(1);
					}
					else
					{
						this.SplitTriangleAlternative(2);
					}
				}
				else
				{
					this.triVertices[0] = this._mesh.wsVerts[this.triIndicies[0]];
					this.triVertices[1] = this._mesh.wsVerts[this.triIndicies[1]];
					this.triVertices[2] = this._mesh.wsVerts[this.triIndicies[2]];
					if (SplitterHelper.GetPlaneSide(this._splitPlane, this.triVertices) > 0f)
					{
						this._mesh.trisUp.Add(this.triIndicies[0]);
						this._mesh.trisUp.Add(this.triIndicies[1]);
						this._mesh.trisUp.Add(this.triIndicies[2]);
					}
					else
					{
						this._mesh.trisDown.Add(this.triIndicies[0]);
						this._mesh.trisDown.Add(this.triIndicies[1]);
						this._mesh.trisDown.Add(this.triIndicies[2]);
					}
				}
			}
		}

		// Token: 0x0600168B RID: 5771 RVA: 0x000713D0 File Offset: 0x0006F5D0
		private void SplitTriangle(int offset)
		{
			int num = offset % 3;
			int num2 = (1 + offset) % 3;
			int num3 = (2 + offset) % 3;
			int num4 = this._mesh.AddLerpVertex(this.triIndicies[num], this.triIndicies[num2], this.lineLerp[num]);
			int num5 = this._mesh.AddLerpVertex(this.triIndicies[num2], this.triIndicies[num3], this.lineLerp[num2]);
			this.AddCapIndex(num4);
			this.AddCapIndex(num5);
			this.smallTri[0] = num4;
			this.smallTri[1] = this.triIndicies[num2];
			this.smallTri[2] = num5;
			this.bigTri[0] = this.triIndicies[num];
			this.bigTri[1] = num4;
			this.bigTri[2] = num5;
			this.bigTri[3] = this.triIndicies[num];
			this.bigTri[4] = num5;
			this.bigTri[5] = this.triIndicies[num3];
			if (this._splitPlane.PointSide(this._mesh.wsVerts[this.triIndicies[num2]]) > 0f)
			{
				this._mesh.trisUp.Add(this.smallTri[0]);
				this._mesh.trisUp.Add(this.smallTri[1]);
				this._mesh.trisUp.Add(this.smallTri[2]);
				this._mesh.trisDown.Add(this.bigTri[0]);
				this._mesh.trisDown.Add(this.bigTri[1]);
				this._mesh.trisDown.Add(this.bigTri[2]);
				this._mesh.trisDown.Add(this.bigTri[3]);
				this._mesh.trisDown.Add(this.bigTri[4]);
				this._mesh.trisDown.Add(this.bigTri[5]);
				return;
			}
			this._mesh.trisDown.Add(this.smallTri[0]);
			this._mesh.trisDown.Add(this.smallTri[1]);
			this._mesh.trisDown.Add(this.smallTri[2]);
			this._mesh.trisUp.Add(this.bigTri[0]);
			this._mesh.trisUp.Add(this.bigTri[1]);
			this._mesh.trisUp.Add(this.bigTri[2]);
			this._mesh.trisUp.Add(this.bigTri[3]);
			this._mesh.trisUp.Add(this.bigTri[4]);
			this._mesh.trisUp.Add(this.bigTri[5]);
		}

		// Token: 0x0600168C RID: 5772 RVA: 0x00071694 File Offset: 0x0006F894
		private void SplitTriangleAlternative(int offset)
		{
			Debug.Log("alt tri split");
			int num = offset % 3;
			int num2 = (1 + offset) % 3;
			int num3 = (2 + offset) % 3;
			int num4 = this._mesh.AddLerpVertex(this.triIndicies[num], this.triIndicies[num2], this.lineLerp[num]);
			this.AddCapIndex(num4);
			this.smallTri[0] = this.triIndicies[num];
			this.smallTri[1] = num4;
			this.smallTri[2] = this.triIndicies[num3];
			this.bigTri[0] = num4;
			this.bigTri[1] = this.triIndicies[num2];
			this.bigTri[2] = this.triIndicies[num3];
			if (this._splitPlane.PointSide(this._mesh.wsVerts[this.triIndicies[num]]) > 0f)
			{
				this._mesh.trisUp.Add(this.smallTri[0]);
				this._mesh.trisUp.Add(this.smallTri[1]);
				this._mesh.trisUp.Add(this.smallTri[2]);
				this._mesh.trisDown.Add(this.bigTri[0]);
				this._mesh.trisDown.Add(this.bigTri[1]);
				this._mesh.trisDown.Add(this.bigTri[2]);
				return;
			}
			this._mesh.trisDown.Add(this.smallTri[0]);
			this._mesh.trisDown.Add(this.smallTri[1]);
			this._mesh.trisDown.Add(this.smallTri[2]);
			this._mesh.trisUp.Add(this.bigTri[0]);
			this._mesh.trisUp.Add(this.bigTri[1]);
			this._mesh.trisUp.Add(this.bigTri[2]);
		}

		// Token: 0x0600168D RID: 5773 RVA: 0x00071888 File Offset: 0x0006FA88
		private void AddCapIndex(int index)
		{
			int index2 = index - this._mesh.vertexCount;
			Vector3 vecB = this._mesh.verticesNew[index2];
			int count = this.capInds.Count;
			for (int i = 0; i < count; i++)
			{
				int num;
				if ((num = this.capInds[i]) >= this._mesh.vertexCount)
				{
					num -= this._mesh.vertexCount;
				}
				if (SplitterHelper.CompareVector3(this._mesh.verticesNew[num], vecB))
				{
					return;
				}
			}
			this.capInds.Add(index);
		}

		// Token: 0x0600168E RID: 5774 RVA: 0x00071920 File Offset: 0x0006FB20
		public void MeshCreateCaps()
		{
			if (this.capInds.Count == 0)
			{
				return;
			}
			this.CreateCap();
			int num = this.capsSorted.Length;
			if (this.CustomUV)
			{
				float x = this.CapUVMin.x;
				float y = this.CapUVMin.y;
				float num2 = this.CapUVMax.x - this.CapUVMin.x;
				float num3 = this.CapUVMax.y - this.CapUVMin.y;
				for (int i = 0; i < num; i++)
				{
					this.capsUV[i].x = this.capsUV[i].x * num2 + x;
					this.capsUV[i].y = this.capsUV[i].y * num3 + y;
				}
			}
			Vector3 vector = Quaternion.Inverse(this._ownRotation) * this._splitPlane.Normal;
			Vector3 normal = -vector;
			int[] array = new int[this.capsSorted.Length];
			int[] array2 = new int[this.capsSorted.Length];
			if (this.UseCapUV)
			{
				for (int j = 0; j < num; j++)
				{
					array[j] = this._mesh.AddCapVertex(this.capsSorted[j], normal, this.capsUV[j]);
					array2[j] = this._mesh.AddCapVertex(this.capsSorted[j], vector, this.capsUV[j]);
				}
			}
			else
			{
				for (int k = 0; k < num; k++)
				{
					array[k] = this._mesh.AddCapVertex(this.capsSorted[k], normal);
					array2[k] = this._mesh.AddCapVertex(this.capsSorted[k], vector);
				}
			}
			int num4 = array.Length;
			for (int l = 2; l < num4; l++)
			{
				this._mesh.trisUp.Add(array[0]);
				this._mesh.trisUp.Add(array[l - 1]);
				this._mesh.trisUp.Add(array[l]);
				this._mesh.trisDown.Add(array2[0]);
				this._mesh.trisDown.Add(array2[l]);
				this._mesh.trisDown.Add(array2[l - 1]);
			}
		}

		// Token: 0x0600168F RID: 5775 RVA: 0x00071B88 File Offset: 0x0006FD88
		private void CreateCap()
		{
			Quaternion rotation = Quaternion.Inverse(this._splitRotation);
			int count = this.capInds.Count;
			Vector3 position = this._mesh.transform.position;
			Vector2[] array = new Vector2[count];
			for (int i = 0; i < count; i++)
			{
				Vector3 vector = (this.capInds[i] < this._mesh.vertexCount) ? this._mesh.wsVerts[this.capInds[i]] : this._mesh.wsVertsNew[this.capInds[i] - this._mesh.vertexCount];
				vector = rotation * (vector - position);
				array[i] = new Vector2(vector.x, vector.z);
			}
			int[] array2 = new int[count];
			for (int j = 0; j < count; j++)
			{
				array2[j] = j;
			}
			int num = 0;
			Vector2 b = array[array2[num]];
			for (int k = 1; k < count; k++)
			{
				if (this.SortLowY(array[array2[k]], b))
				{
					num = k;
					b = array[array2[num]];
				}
			}
			if (num != 0)
			{
				this.Swap(array2, 0, num);
			}
			float num2 = 900f;
			int[] array3 = new int[count];
			Vector2 vector2 = array[array2[0]];
			for (int l = 1; l < count; l++)
			{
				Vector2 vector3 = array[array2[l]];
				float num3 = Vector2.Dot(MeshSplitterConvex.Vector2Up, (vector3 - vector2).normalized);
				if (vector2.x <= vector3.x)
				{
					array3[array2[l]] = (int)(num3 * num2);
				}
				else
				{
					array3[array2[l]] = (int)((2f - num3) * num2);
				}
				if (array3[array2[l]] < 0)
				{
					array3[array2[l]] = 0;
				}
			}
			array3[array2[0]] = -1;
			this.GnomeSort(array2, array3);
			this.SortEvenStart(array2, array3, array);
			this.SortEvenEnd(array2, array3, array);
			float num4 = 1f - SplitterHelper.Threshold;
			int num5 = array2.Length;
			int num6 = num5;
			int num7 = 0;
			int num8 = 1 % num5;
			int num9 = 2 % num5;
			for (;;)
			{
				Vector2 vector4 = array[array2[num8]] - array[array2[num7]];
				Vector2 vector5 = array[array2[num9]] - array[array2[num8]];
				if (Vector2.Dot(vector4.normalized, vector5.normalized) > num4)
				{
					array2[num8] = -1;
					num6--;
				}
				else
				{
					num7 = num8;
				}
				if (num9 == 0)
				{
					break;
				}
				num8 = num9;
				num9 = (num9 + 1) % num5;
			}
			this.capsSorted = new int[num6];
			int num10 = 0;
			int num11 = 0;
			while (num11 < num5 && num10 < num6)
			{
				int num12 = array2[num11];
				if (num12 >= 0)
				{
					this.capsSorted[num10++] = this.capInds[num12];
				}
				num11++;
			}
			if (this.UseCapUV)
			{
				this.capsUV = new Vector2[num6];
				Vector2 vector6 = new Vector2(float.MaxValue, float.MaxValue);
				Vector2 vector7 = new Vector2(float.MinValue, float.MinValue);
				for (int m = 0; m < num5; m++)
				{
					int num13 = array2[m];
					if (num13 >= 0)
					{
						Vector2 vector8 = array[num13];
						if (vector6.x > vector8.x)
						{
							vector6.x = vector8.x;
						}
						else if (vector7.x < vector8.x)
						{
							vector7.x = vector8.x;
						}
						if (vector6.y > vector8.y)
						{
							vector6.y = vector8.y;
						}
						else if (vector7.y < vector8.y)
						{
							vector7.y = vector8.y;
						}
					}
				}
				float num14 = vector7.x - vector6.x;
				float num15 = vector7.y - vector6.y;
				num10 = 0;
				int num16 = 0;
				while (num16 < num5 && num10 < num6)
				{
					int num17 = array2[num16];
					if (num17 >= 0)
					{
						Vector2 vector9 = array[num17];
						this.capsUV[num10++] = new Vector2((vector9.x - vector6.x) / num14, (vector9.y - vector6.y) / num15);
					}
					num16++;
				}
			}
		}

		// Token: 0x06001690 RID: 5776 RVA: 0x00072004 File Offset: 0x00070204
		private void Swap(int[] array, int a, int b)
		{
			int num = array[a];
			array[a] = array[b];
			array[b] = num;
		}

		// Token: 0x06001691 RID: 5777 RVA: 0x0007201F File Offset: 0x0007021F
		private bool SortLowY(Vector2 a, Vector2 b)
		{
			return a.y <= b.y && (a.y < b.y || a.x < b.x);
		}

		// Token: 0x06001692 RID: 5778 RVA: 0x00072054 File Offset: 0x00070254
		private void GnomeSort(int[] index, int[] value)
		{
			int i = 1;
			int num = index.Length;
			while (i < num)
			{
				if (value[index[i]] >= value[index[i - 1]])
				{
					i++;
				}
				else
				{
					this.Swap(index, i, i - 1);
					if (i > 1)
					{
						i--;
					}
					else
					{
						i++;
					}
				}
			}
		}

		// Token: 0x06001693 RID: 5779 RVA: 0x0007209C File Offset: 0x0007029C
		private void SortEvenStart(int[] index, int[] value, Vector2[] localVerts)
		{
			int num = 2;
			int num2 = index.Length;
			while (num < num2 && value[index[num]] == value[index[num - 1]])
			{
				Vector2 vector = localVerts[index[num - 1]];
				Vector2 vector2 = localVerts[index[num]];
				if (vector.y > vector2.y || (vector.x > vector2.x && vector.y == vector2.y))
				{
					this.Swap(index, num, num - 1);
					if (num > 2)
					{
						num--;
					}
					else
					{
						num++;
					}
				}
				else
				{
					num++;
				}
			}
		}

		// Token: 0x06001694 RID: 5780 RVA: 0x00072124 File Offset: 0x00070324
		private void SortEvenEnd(int[] index, int[] value, Vector2[] localVerts)
		{
			int num = index.Length;
			int num2 = num - 2;
			while (num2 > 0 && value[index[num2]] == value[index[num2 + 1]])
			{
				ref Vector2 ptr = localVerts[index[num2]];
				Vector2 vector = localVerts[index[num2 + 1]];
				if (ptr.y < vector.y)
				{
					this.Swap(index, num2, num2 + 1);
					if (num2 < num - 2)
					{
						num2++;
					}
					else
					{
						num2--;
					}
				}
				else
				{
					num2--;
				}
			}
		}

		// Token: 0x04001051 RID: 4177
		public bool UseCapUV;

		// Token: 0x04001052 RID: 4178
		public bool CustomUV;

		// Token: 0x04001053 RID: 4179
		public Vector2 CapUVMin = Vector2.zero;

		// Token: 0x04001054 RID: 4180
		public Vector2 CapUVMax = Vector2.one;

		// Token: 0x04001055 RID: 4181
		protected MeshContainer _mesh;

		// Token: 0x04001056 RID: 4182
		protected PlaneMath _splitPlane;

		// Token: 0x04001057 RID: 4183
		protected Quaternion _splitRotation;

		// Token: 0x04001058 RID: 4184
		private Quaternion _ownRotation;

		// Token: 0x04001059 RID: 4185
		public List<int> capInds;

		// Token: 0x0400105A RID: 4186
		private int[] triIndicies = new int[3];

		// Token: 0x0400105B RID: 4187
		private float[] lineLerp = new float[3];

		// Token: 0x0400105C RID: 4188
		private bool[] lineHit = new bool[3];

		// Token: 0x0400105D RID: 4189
		private Vector3[] triVertices = new Vector3[3];

		// Token: 0x0400105E RID: 4190
		private int[] smallTri = new int[3];

		// Token: 0x0400105F RID: 4191
		private int[] bigTri = new int[6];

		// Token: 0x04001060 RID: 4192
		private static Vector2 Vector2Up = Vector2.up;

		// Token: 0x04001061 RID: 4193
		protected int[] capsSorted;

		// Token: 0x04001062 RID: 4194
		protected Vector2[] capsUV;
	}
}
