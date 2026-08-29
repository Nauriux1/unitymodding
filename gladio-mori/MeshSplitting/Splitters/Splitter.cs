using System;
using MeshSplitting.Splitables;
using UnityEngine;

namespace MeshSplitting.Splitters
{
	// Token: 0x020002D5 RID: 725
	[AddComponentMenu("Mesh Splitting/Splitter")]
	[RequireComponent(typeof(Collider))]
	public class Splitter : MonoBehaviour
	{
		// Token: 0x06001631 RID: 5681 RVA: 0x0006DDE5 File Offset: 0x0006BFE5
		protected virtual void Awake()
		{
			this._transform = base.GetComponent<Transform>();
			base.GetComponent<Collider>().isTrigger = true;
		}

		// Token: 0x06001632 RID: 5682 RVA: 0x0006DE00 File Offset: 0x0006C000
		private void OnTriggerEnter(Collider other)
		{
			MonoBehaviour[] components = other.GetComponents<MonoBehaviour>();
			for (int i = 0; i < components.Length; i++)
			{
				ISplitable splitable = components[i] as ISplitable;
				if (splitable != null)
				{
					this.SplitObject(splitable, other.gameObject);
					return;
				}
			}
		}

		// Token: 0x06001633 RID: 5683 RVA: 0x0006DE3C File Offset: 0x0006C03C
		protected virtual void SplitObject(ISplitable splitable, GameObject go)
		{
			splitable.Split(this._transform);
		}

		// Token: 0x04001002 RID: 4098
		protected Transform _transform;
	}
}
