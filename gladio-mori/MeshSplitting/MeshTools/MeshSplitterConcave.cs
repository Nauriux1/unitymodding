using System;
using System.Collections.Generic;
using MeshSplitting.SplitterMath;
using UnityEngine;

namespace MeshSplitting.MeshTools
{
	// Token: 0x020002DF RID: 735
	public class MeshSplitterConcave : IMeshSplitter
	{
		// Token: 0x06001668 RID: 5736 RVA: 0x0006F5A0 File Offset: 0x0006D7A0
		public MeshSplitterConcave(MeshContainer meshContainer, PlaneMath splitPlane, Quaternion splitRotation)
		{
			this._mesh = meshContainer;
			this._splitPlane = splitPlane;
			this._splitRotation = splitRotation;
			this._ownRotation = meshContainer.transform.rotation;
			this._edges = new List<MeshSplitterConcave.Edge>(meshContainer.vertexCount / 10);
		}

		// Token: 0x06001669 RID: 5737 RVA: 0x0006F656 File Offset: 0x0006D856
		public void SetCapUV(bool useCapUV, bool customUV, Vector2 uvMin, Vector2 uvMax)
		{
			this.UseCapUV = useCapUV;
			this.CustomUV = customUV;
			this.CapUVMin = uvMin;
			this.CapUVMax = uvMax;
		}

		// Token: 0x0600166A RID: 5738 RVA: 0x0006F678 File Offset: 0x0006D878
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
					if (!this.lineHit[2])
					{
						this.SplitTriangle(this.triIndicies, this.lineLerp, 0);
					}
					else if (!this.lineHit[0])
					{
						this.SplitTriangle(this.triIndicies, this.lineLerp, 1);
					}
					else
					{
						this.SplitTriangle(this.triIndicies, this.lineLerp, 2);
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

		// Token: 0x0600166B RID: 5739 RVA: 0x0006F9C0 File Offset: 0x0006DBC0
		private void SplitTriangle(int[] triIndicies, float[] lineLerp, int offset)
		{
			int num = offset % 3;
			int num2 = (1 + offset) % 3;
			int num3 = (2 + offset) % 3;
			int num4 = this._mesh.AddLerpVertex(triIndicies[num], triIndicies[num2], lineLerp[num]);
			int num5 = this._mesh.AddLerpVertex(triIndicies[num2], triIndicies[num3], lineLerp[num2]);
			this.AddEdge(num4, num5);
			this.smallTri[0] = num4;
			this.smallTri[1] = triIndicies[num2];
			this.smallTri[2] = num5;
			this.bigTri[0] = triIndicies[num];
			this.bigTri[1] = num4;
			this.bigTri[2] = num5;
			this.bigTri[3] = triIndicies[num];
			this.bigTri[4] = num5;
			this.bigTri[5] = triIndicies[num3];
			if (this._splitPlane.PointSide(this._mesh.wsVerts[triIndicies[num2]]) > 0f)
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

		// Token: 0x0600166C RID: 5740 RVA: 0x0006FC48 File Offset: 0x0006DE48
		private void AddEdge(int index0, int index1)
		{
			int vertexCount = this._mesh.vertexCount;
			MeshSplitterConcave.Edge edge = new MeshSplitterConcave.Edge();
			edge.IndexLeft = index0;
			edge.IndexRight = index1;
			Vector3 vector = this._mesh.wsVertsNew[index0 - vertexCount];
			Vector3 vector2 = this._mesh.wsVertsNew[index1 - vertexCount];
			if (SplitterHelper.CompareVector3(ref vector, ref vector2))
			{
				return;
			}
			this._edges.Add(edge);
		}

		// Token: 0x0600166D RID: 5741 RVA: 0x0006FCB8 File Offset: 0x0006DEB8
		public void MeshCreateCaps()
		{
			if (this._edges.Count == 0)
			{
				return;
			}
			this.triList = new List<int>(this._mesh.vertexCount / 4);
			this.uvList = new List<Vector2>(this._mesh.vertexCount / 4);
			this.CalculateRotatedEdges();
			this.LinkEdges();
			this.CheckNormals();
			if (this.UseCapUV)
			{
				this.CreateUVs();
			}
			this.CreateTriangles();
			this.AddTrianglesToMesh();
		}

		// Token: 0x0600166E RID: 5742 RVA: 0x0006FD30 File Offset: 0x0006DF30
		private void CalculateRotatedEdges()
		{
			Quaternion rotation = Quaternion.Inverse(this._splitRotation);
			int count = this._edges.Count;
			int vertexCount = this._mesh.vertexCount;
			for (int i = 0; i < count; i++)
			{
				MeshSplitterConcave.Edge edge = this._edges[i];
				Vector3 vector = this._mesh.wsVertsNew[edge.IndexLeft - vertexCount];
				Vector3 vector2 = this._mesh.wsVertsNew[edge.IndexRight - vertexCount];
				vector = rotation * vector;
				vector2 = rotation * vector2;
				edge.Left = new Vector2(vector.x, vector.z);
				edge.Right = new Vector2(vector2.x, vector2.z);
			}
		}

		// Token: 0x0600166F RID: 5743 RVA: 0x0006FE00 File Offset: 0x0006E000
		private void LinkEdges()
		{
			int count = this._edges.Count;
			LinkedList<LinkedList<int>> linkedList = new LinkedList<LinkedList<int>>();
			for (int i = 0; i < count; i++)
			{
				LinkedList<int> linkedList2 = new LinkedList<int>();
				linkedList2.AddLast(i);
				linkedList.AddLast(linkedList2);
			}
			LinkedListNode<LinkedList<int>> linkedListNode = linkedList.First;
			LinkedListNode<LinkedList<int>> linkedListNode2 = linkedListNode.Next;
			while (linkedList.Count > 0)
			{
				Vector2 left = this._edges[linkedListNode.Value.Last.Value].Left;
				MeshSplitterConcave.Edge edge = this._edges[linkedListNode2.Value.First.Value];
				Vector2 right = edge.Right;
				Vector2 left2 = edge.Left;
				bool flag = SplitterHelper.CompareVector2(ref left, ref right);
				bool flag2 = !flag && SplitterHelper.CompareVector2(ref left, ref left2);
				if (flag || flag2)
				{
					if (flag)
					{
						this.AttachLinkedList(linkedListNode2.Value, linkedListNode.Value);
					}
					else
					{
						this.AttachLinkedListFlip(linkedListNode2.Value, linkedListNode.Value);
					}
					linkedList.Remove(linkedListNode2);
					linkedListNode2 = MeshSplitterConcave.LLCircularNext<LinkedList<int>>(linkedListNode);
					Vector2 right2 = this._edges[linkedListNode.Value.First.Value].Right;
					if (SplitterHelper.CompareVector2(ref left, ref right2))
					{
						LinkedList<int> linkedList3 = new LinkedList<int>();
						this.AttachLinkedList(linkedListNode.Value, linkedList3);
						this.linkedBorders.AddLast(linkedList3);
						linkedList.Remove(linkedListNode);
						if (linkedList.Count == 0)
						{
							break;
						}
						linkedListNode = linkedListNode2;
						linkedListNode2 = MeshSplitterConcave.LLCircularNext<LinkedList<int>>(linkedListNode);
					}
				}
				else
				{
					linkedListNode2 = MeshSplitterConcave.LLCircularNext<LinkedList<int>>(linkedListNode2);
				}
				if (linkedListNode == linkedListNode2)
				{
					if (linkedListNode == linkedList.Last)
					{
						break;
					}
					linkedListNode = MeshSplitterConcave.LLCircularNext<LinkedList<int>>(linkedListNode);
					linkedListNode2 = MeshSplitterConcave.LLCircularNext<LinkedList<int>>(linkedListNode);
				}
			}
			if (linkedList.Count > 0)
			{
				foreach (LinkedList<int> linkedList4 in linkedList)
				{
					MeshSplitterConcave.Edge edge2 = this._edges[linkedList4.First.Value];
					MeshSplitterConcave.Edge edge3 = this._edges[linkedList4.Last.Value];
					if (linkedList4.Count > 2)
					{
						MeshSplitterConcave.Edge edge4 = new MeshSplitterConcave.Edge();
						edge4.IndexLeft = edge2.IndexRight;
						edge4.Left = edge2.Right;
						edge4.IndexRight = edge3.IndexLeft;
						edge4.Right = edge3.Left;
						linkedList4.AddLast(this._edges.Count);
						this._edges.Add(edge4);
						this.linkedBorders.AddLast(linkedList4);
					}
				}
			}
		}

		// Token: 0x06001670 RID: 5744 RVA: 0x00070098 File Offset: 0x0006E298
		private void AttachLinkedList(LinkedList<int> source, LinkedList<int> destination)
		{
			foreach (int value in source)
			{
				destination.AddLast(value);
			}
		}

		// Token: 0x06001671 RID: 5745 RVA: 0x000700E8 File Offset: 0x0006E2E8
		private void AttachLinkedListFlip(LinkedList<int> source, LinkedList<int> destination)
		{
			foreach (int num in source)
			{
				this._edges[num].Flip();
				destination.AddLast(num);
			}
		}

		// Token: 0x06001672 RID: 5746 RVA: 0x00070148 File Offset: 0x0006E348
		private void CheckNormals()
		{
			foreach (LinkedList<int> linkedList in this.linkedBorders)
			{
				LinkedListNode<int> linkedListNode = linkedList.First;
				LinkedListNode<int> linkedListNode2 = linkedListNode;
				while ((linkedListNode2 = linkedListNode2.Next) != null)
				{
					if (this._edges[linkedListNode2.Value].Right.y > this._edges[linkedListNode.Value].Right.y)
					{
						linkedListNode = linkedListNode2;
					}
				}
				int value = MeshSplitterConcave.LLCircularNext<int>(linkedListNode).Value;
				Vector2 right = this._edges[linkedListNode.Value].Right;
				Vector2 left = this._edges[value].Left;
				if (!SplitterHelper.CompareVector2(ref right, ref left))
				{
					value = MeshSplitterConcave.LLCircularPrevious<int>(linkedListNode).Value;
				}
				if (this.TestInnerSide(this._edges[linkedListNode.Value], this._edges[value], true) < 0)
				{
					foreach (int index in linkedList)
					{
						this._edges[index].Flip();
					}
				}
			}
		}

		// Token: 0x06001673 RID: 5747 RVA: 0x000702C4 File Offset: 0x0006E4C4
		private void CreateUVs()
		{
			foreach (LinkedList<int> linkedList in this.linkedBorders)
			{
				Vector2 vector = new Vector2(float.MaxValue, float.MaxValue);
				Vector2 vector2 = new Vector2(float.MinValue, float.MinValue);
				LinkedListNode<int> linkedListNode = linkedList.First;
				do
				{
					Vector2 left = this._edges[linkedListNode.Value].Left;
					if (vector.x > left.x)
					{
						vector.x = left.x;
					}
					else if (vector2.x < left.x)
					{
						vector2.x = left.x;
					}
					if (vector.y > left.y)
					{
						vector.y = left.y;
					}
					else if (vector2.y < left.y)
					{
						vector2.y = left.y;
					}
				}
				while ((linkedListNode = linkedListNode.Next) != null);
				Vector2 vector3 = vector2 - vector;
				Vector2 vector4 = this.CapUVMax - this.CapUVMin;
				linkedListNode = linkedList.First;
				do
				{
					MeshSplitterConcave.Edge edge = this._edges[linkedListNode.Value];
					edge.UVLeft.Set((edge.Left.x - vector.x) / vector3.x, (edge.Left.y - vector.y) / vector3.y);
					edge.UVRight.Set((edge.Right.x - vector.x) / vector3.x, (edge.Right.y - vector.y) / vector3.y);
					if (this.CustomUV)
					{
						edge.UVLeft.Set(edge.UVLeft.x * vector4.x + this.CapUVMin.x, edge.UVLeft.y * vector4.y + this.CapUVMin.y);
						edge.UVRight.Set(edge.UVRight.x * vector4.x + this.CapUVMin.x, edge.UVRight.y * vector4.y + this.CapUVMin.y);
					}
				}
				while ((linkedListNode = linkedListNode.Next) != null);
			}
		}

		// Token: 0x06001674 RID: 5748 RVA: 0x0007055C File Offset: 0x0006E75C
		private void CreateTriangles()
		{
			if (this.linkedBorders.Count <= 0)
			{
				return;
			}
			foreach (LinkedList<int> linkedList in this.linkedBorders)
			{
				LinkedListNode<int> linkedListNode = linkedList.First;
				Vector2 right = this._edges[linkedListNode.Value].Right;
				Vector2 left = this._edges[linkedListNode.Next.Value].Left;
				bool flag = SplitterHelper.CompareVector2(ref right, ref left);
				bool flag2 = true;
				int num = 0;
				int num2 = 0;
				while (linkedList.Count > 3 && num2++ < 10000)
				{
					bool flag3 = flag2 ^ flag;
					LinkedListNode<int> linkedListNode2 = flag3 ? MeshSplitterConcave.LLCircularPrevious<int>(linkedListNode) : MeshSplitterConcave.LLCircularNext<int>(linkedListNode);
					MeshSplitterConcave.Edge edge = this._edges[linkedListNode.Value];
					MeshSplitterConcave.Edge edge2 = this._edges[linkedListNode2.Value];
					int num3 = this.TestInnerSide(edge, edge2, flag2);
					if (num3 == 0)
					{
						MeshSplitterConcave.Edge item;
						if (flag2)
						{
							item = MeshSplitterConcave.Edge.MeltEdges(edge, edge2);
						}
						else
						{
							item = MeshSplitterConcave.Edge.MeltEdges(edge2, edge);
						}
						LinkedListNode<int> linkedListNode3 = linkedList.AddAfter(linkedListNode, this._edges.Count);
						this._edges.Add(item);
						linkedList.Remove(linkedListNode);
						linkedList.Remove(linkedListNode2);
						linkedListNode = linkedListNode3;
					}
					else
					{
						if (num3 == 1)
						{
							MeshSplitterConcave.Edge edge3;
							if (flag2)
							{
								edge3 = MeshSplitterConcave.Edge.CloseEdges(edge, edge2);
							}
							else
							{
								edge3 = MeshSplitterConcave.Edge.CloseEdges(edge2, edge);
							}
							LinkedListNode<int> linkedListNode4;
							LinkedListNode<int> linkedListNode5;
							if (flag3)
							{
								linkedListNode4 = MeshSplitterConcave.LLCircularNext<int>(linkedListNode);
								linkedListNode5 = MeshSplitterConcave.LLCircularPrevious<int>(linkedListNode2);
							}
							else
							{
								linkedListNode4 = MeshSplitterConcave.LLCircularPrevious<int>(linkedListNode);
								linkedListNode5 = MeshSplitterConcave.LLCircularNext<int>(linkedListNode2);
							}
							MeshSplitterConcave.Edge other = this._edges[linkedListNode4.Value];
							MeshSplitterConcave.Edge other2 = this._edges[linkedListNode5.Value];
							LinkedListNode<int> linkedListNode6 = null;
							if (edge3.SameVectors(other))
							{
								linkedListNode6 = linkedListNode4;
							}
							else if (edge3.SameVectors(other2))
							{
								linkedListNode6 = linkedListNode5;
							}
							if (linkedListNode6 != null)
							{
								if (flag2)
								{
									this.AddTriangle(edge, edge2, edge3);
								}
								else
								{
									this.AddTriangle(edge, edge3, edge2);
								}
								linkedList.Remove(linkedListNode);
								linkedList.Remove(linkedListNode2);
								linkedListNode = MeshSplitterConcave.LLCircularNext<int>(linkedListNode6);
								linkedList.Remove(linkedListNode6);
								num = 0;
								continue;
							}
							if (!this.TestNewEdgeIntersect(edge3, linkedList))
							{
								if (flag2)
								{
									this.AddTriangle(edge, edge2, edge3);
								}
								else
								{
									this.AddTriangle(edge, edge3, edge2);
								}
								LinkedListNode<int> linkedListNode7 = linkedList.AddAfter(linkedListNode, this._edges.Count);
								edge3.Flip();
								this._edges.Add(edge3);
								linkedList.Remove(linkedListNode);
								linkedList.Remove(linkedListNode2);
								linkedListNode = linkedListNode7;
								num = 0;
								continue;
							}
							num++;
							flag2 = !flag2;
						}
						else
						{
							num++;
							flag2 = !flag2;
						}
						if (num >= 2)
						{
							linkedListNode = ((flag2 ^ flag) ? MeshSplitterConcave.LLCircularPrevious<int>(linkedListNode) : MeshSplitterConcave.LLCircularNext<int>(linkedListNode));
							num = 0;
						}
					}
				}
				if (linkedList.Count == 3)
				{
					MeshSplitterConcave.Edge edge4 = this._edges[linkedList.First.Value];
					MeshSplitterConcave.Edge edge5 = this._edges[linkedList.First.Next.Value];
					MeshSplitterConcave.Edge edge6 = this._edges[linkedList.Last.Value];
					if (flag)
					{
						this.AddTriangle(edge4, edge5, edge6);
					}
					else
					{
						this.AddTriangle(edge4, edge6, edge5);
					}
				}
			}
		}

		// Token: 0x06001675 RID: 5749 RVA: 0x000708D0 File Offset: 0x0006EAD0
		private void AddTriangle(MeshSplitterConcave.Edge edge1, MeshSplitterConcave.Edge edge2, MeshSplitterConcave.Edge edge3)
		{
			this.triList.Add(edge1.IndexRight);
			this.triList.Add(edge2.IndexRight);
			this.triList.Add(edge3.IndexRight);
			if (this.UseCapUV)
			{
				this.uvList.Add(edge1.UVRight);
				this.uvList.Add(edge2.UVRight);
				this.uvList.Add(edge3.UVRight);
			}
		}

		// Token: 0x06001676 RID: 5750 RVA: 0x0007094C File Offset: 0x0006EB4C
		private int TestInnerSide(MeshSplitterConcave.Edge currentEdge, MeshSplitterConcave.Edge nextEdge, bool cw)
		{
			float num = new PlaneMath(currentEdge.Left, currentEdge.Normal).PointSide(cw ? nextEdge.Right : nextEdge.Left);
			if (num < -1E-06f)
			{
				return 1;
			}
			if (num < 1E-06f)
			{
				return 0;
			}
			return -1;
		}

		// Token: 0x06001677 RID: 5751 RVA: 0x000709A8 File Offset: 0x0006EBA8
		private bool TestNewEdgeIntersect(MeshSplitterConcave.Edge edge, LinkedList<int> borderIndicies)
		{
			foreach (int index in borderIndicies)
			{
				if (edge.EdgeIntersect(this._edges[index]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001678 RID: 5752 RVA: 0x00070A0C File Offset: 0x0006EC0C
		private static LinkedListNode<T> LLCircularNext<T>(LinkedListNode<T> current)
		{
			if (current.Next != null)
			{
				return current.Next;
			}
			return current.List.First;
		}

		// Token: 0x06001679 RID: 5753 RVA: 0x00070A28 File Offset: 0x0006EC28
		private static LinkedListNode<T> LLCircularPrevious<T>(LinkedListNode<T> current)
		{
			if (current.Previous != null)
			{
				return current.Previous;
			}
			return current.List.Last;
		}

		// Token: 0x0600167A RID: 5754 RVA: 0x00070A44 File Offset: 0x0006EC44
		private void AddTrianglesToMesh()
		{
			Vector3 vector = Quaternion.Inverse(this._ownRotation) * this._splitPlane.Normal;
			Vector3 normal = -vector;
			int count = this.triList.Count;
			int[] array = new int[count];
			int[] array2 = new int[count];
			if (this.UseCapUV)
			{
				for (int i = 0; i < count; i++)
				{
					int refIndex = this.triList[i];
					Vector2 capUV = this.uvList[i];
					array[i] = this._mesh.AddCapVertex(refIndex, normal, capUV);
					array2[i] = this._mesh.AddCapVertex(refIndex, vector, capUV);
				}
			}
			else
			{
				for (int j = 0; j < count; j++)
				{
					int refIndex2 = this.triList[j];
					array[j] = this._mesh.AddCapVertex(refIndex2, normal);
					array2[j] = this._mesh.AddCapVertex(refIndex2, vector);
				}
			}
			for (int k = 2; k < count; k += 3)
			{
				this._mesh.trisUp.Add(array[k - 2]);
				this._mesh.trisUp.Add(array[k]);
				this._mesh.trisUp.Add(array[k - 1]);
				this._mesh.trisDown.Add(array2[k - 2]);
				this._mesh.trisDown.Add(array2[k - 1]);
				this._mesh.trisDown.Add(array2[k]);
			}
		}

		// Token: 0x04001036 RID: 4150
		public bool UseCapUV;

		// Token: 0x04001037 RID: 4151
		public bool CustomUV;

		// Token: 0x04001038 RID: 4152
		public Vector2 CapUVMin = Vector2.zero;

		// Token: 0x04001039 RID: 4153
		public Vector2 CapUVMax = Vector2.one;

		// Token: 0x0400103A RID: 4154
		protected MeshContainer _mesh;

		// Token: 0x0400103B RID: 4155
		protected PlaneMath _splitPlane;

		// Token: 0x0400103C RID: 4156
		protected Quaternion _splitRotation;

		// Token: 0x0400103D RID: 4157
		private Quaternion _ownRotation;

		// Token: 0x0400103E RID: 4158
		private List<MeshSplitterConcave.Edge> _edges;

		// Token: 0x0400103F RID: 4159
		private int[] triIndicies = new int[3];

		// Token: 0x04001040 RID: 4160
		private float[] lineLerp = new float[3];

		// Token: 0x04001041 RID: 4161
		private bool[] lineHit = new bool[3];

		// Token: 0x04001042 RID: 4162
		private Vector3[] triVertices = new Vector3[3];

		// Token: 0x04001043 RID: 4163
		private int[] smallTri = new int[3];

		// Token: 0x04001044 RID: 4164
		private int[] bigTri = new int[6];

		// Token: 0x04001045 RID: 4165
		private LinkedList<LinkedList<int>> linkedBorders = new LinkedList<LinkedList<int>>();

		// Token: 0x04001046 RID: 4166
		private List<int> triList;

		// Token: 0x04001047 RID: 4167
		private List<Vector2> uvList;

		// Token: 0x020002E0 RID: 736
		protected class Edge
		{
			// Token: 0x17000289 RID: 649
			// (get) Token: 0x0600167B RID: 5755 RVA: 0x00070BCD File Offset: 0x0006EDCD
			// (set) Token: 0x0600167C RID: 5756 RVA: 0x00070BD5 File Offset: 0x0006EDD5
			public Vector2 Left
			{
				get
				{
					return this._left;
				}
				set
				{
					this._left = value;
					this._normal = MeshSplitterConcave.Edge._zero;
				}
			}

			// Token: 0x1700028A RID: 650
			// (get) Token: 0x0600167D RID: 5757 RVA: 0x00070BE9 File Offset: 0x0006EDE9
			// (set) Token: 0x0600167E RID: 5758 RVA: 0x00070BF1 File Offset: 0x0006EDF1
			public Vector2 Right
			{
				get
				{
					return this._right;
				}
				set
				{
					this._right = value;
					this._normal = MeshSplitterConcave.Edge._zero;
				}
			}

			// Token: 0x1700028B RID: 651
			// (get) Token: 0x0600167F RID: 5759 RVA: 0x00070C05 File Offset: 0x0006EE05
			public Vector2 Normal
			{
				get
				{
					if (this._normal == MeshSplitterConcave.Edge._zero)
					{
						this.CalculateNormal();
					}
					return this._normal;
				}
			}

			// Token: 0x06001680 RID: 5760 RVA: 0x00070C28 File Offset: 0x0006EE28
			public void CalculateNormal()
			{
				Vector3 a = new Vector3(this.Right.x, this.Right.y);
				Vector3 b = new Vector3(this.Left.x, this.Left.y);
				Vector3 vector = MeshSplitterConcave.Edge._cw90 * (a - b);
				vector.Normalize();
				this._normal.Set(vector.x, vector.y);
			}

			// Token: 0x06001681 RID: 5761 RVA: 0x00070CA0 File Offset: 0x0006EEA0
			public bool EdgeIntersect(MeshSplitterConcave.Edge other)
			{
				float num = (other.Right.y - other.Left.y) * (this.Right.x - this.Left.x) - (other.Right.x - other.Left.x) * (this.Right.y - this.Left.y);
				float num2 = ((other.Right.x - other.Left.x) * (this.Left.y - other.Left.y) - (other.Right.y - other.Left.y) * (this.Left.x - other.Left.x)) / num;
				float num3 = ((this.Right.x - this.Left.x) * (this.Left.y - other.Left.y) - (this.Right.y - this.Left.y) * (this.Left.x - other.Left.x)) / num;
				return num2 > 1E-05f && num2 < 0.99999f && num3 > 1E-05f && num3 < 0.99999f;
			}

			// Token: 0x06001682 RID: 5762 RVA: 0x00070DF4 File Offset: 0x0006EFF4
			public void Flip()
			{
				int indexLeft = this.IndexLeft;
				this.IndexLeft = this.IndexRight;
				this.IndexRight = indexLeft;
				Vector2 vector = this.Left;
				this._left = this._right;
				this.Right = vector;
				vector = this.UVLeft;
				this.UVLeft = this.UVRight;
				this.UVRight = vector;
			}

			// Token: 0x06001683 RID: 5763 RVA: 0x00070E4F File Offset: 0x0006F04F
			public bool SameVectors(MeshSplitterConcave.Edge other)
			{
				return SplitterHelper.CompareVector2(ref this._left, ref other._left) && SplitterHelper.CompareVector2(ref this._right, ref other._right);
			}

			// Token: 0x06001684 RID: 5764 RVA: 0x00070E7C File Offset: 0x0006F07C
			public static MeshSplitterConcave.Edge MeltEdges(MeshSplitterConcave.Edge edgeLeft, MeshSplitterConcave.Edge edgeRight)
			{
				return new MeshSplitterConcave.Edge
				{
					IndexLeft = edgeLeft.IndexLeft,
					Left = edgeLeft.Left,
					UVLeft = edgeLeft.UVLeft,
					IndexRight = edgeRight.IndexRight,
					Right = edgeRight.Right,
					UVRight = edgeRight.UVRight
				};
			}

			// Token: 0x06001685 RID: 5765 RVA: 0x00070ED8 File Offset: 0x0006F0D8
			public static MeshSplitterConcave.Edge CloseEdges(MeshSplitterConcave.Edge edgeLeft, MeshSplitterConcave.Edge edgeRight)
			{
				return new MeshSplitterConcave.Edge
				{
					IndexLeft = edgeRight.IndexRight,
					Left = edgeRight.Right,
					UVLeft = edgeRight.UVRight,
					IndexRight = edgeLeft.IndexLeft,
					Right = edgeLeft.Left,
					UVRight = edgeLeft.UVLeft
				};
			}

			// Token: 0x04001048 RID: 4168
			private static readonly Vector2 _zero = Vector2.zero;

			// Token: 0x04001049 RID: 4169
			private static readonly Quaternion _cw90 = Quaternion.AngleAxis(90f, Vector3.forward);

			// Token: 0x0400104A RID: 4170
			private Vector2 _left = Vector2.zero;

			// Token: 0x0400104B RID: 4171
			private Vector2 _right = Vector2.zero;

			// Token: 0x0400104C RID: 4172
			private Vector2 _normal = Vector2.zero;

			// Token: 0x0400104D RID: 4173
			public int IndexLeft;

			// Token: 0x0400104E RID: 4174
			public int IndexRight;

			// Token: 0x0400104F RID: 4175
			public Vector2 UVLeft;

			// Token: 0x04001050 RID: 4176
			public Vector2 UVRight;
		}
	}
}
