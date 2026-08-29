using System;
using MeshSplitting.Splitables;
using UnityEngine;

namespace MeshSplitting.Splitters
{
	// Token: 0x020002D6 RID: 726
	[AddComponentMenu("Mesh Splitting/Splitter Single Cut")]
	public class SplitterSingleCut : Splitter
	{
		// Token: 0x06001635 RID: 5685 RVA: 0x0006DE4A File Offset: 0x0006C04A
		protected override void SplitObject(ISplitable splitable, GameObject go)
		{
			splitable.Split(this._transform);
			this._hasCut = true;
		}

		// Token: 0x06001636 RID: 5686 RVA: 0x0006DE5F File Offset: 0x0006C05F
		protected virtual void Update()
		{
			this._time -= Time.deltaTime;
			if (this._hasCut || this._time <= 0f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x04001003 RID: 4099
		private bool _hasCut;

		// Token: 0x04001004 RID: 4100
		private float _time = 0.1f;
	}
}
