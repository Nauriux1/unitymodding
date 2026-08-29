using System;
using UnityEngine;

namespace MeshSplitting.MeshTools
{
	// Token: 0x020002DD RID: 733
	public interface IMeshSplitter
	{
		// Token: 0x06001655 RID: 5717
		void SetCapUV(bool useCapUV, bool customUV, Vector2 uvMin, Vector2 uvMax);

		// Token: 0x06001656 RID: 5718
		void MeshSplit();

		// Token: 0x06001657 RID: 5719
		void MeshCreateCaps();
	}
}
