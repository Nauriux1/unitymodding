using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000049 RID: 73
public class PrefabPool : MonoBehaviour
{
	// Token: 0x06000214 RID: 532 RVA: 0x0000BFD0 File Offset: 0x0000A1D0
	private void Start()
	{
		this.InitPrefabPoolObject();
	}

	// Token: 0x06000215 RID: 533 RVA: 0x0000BFD8 File Offset: 0x0000A1D8
	private void Update()
	{
		this.CleanActivePrefabPoolObjects();
	}

	// Token: 0x06000216 RID: 534 RVA: 0x0000BFE0 File Offset: 0x0000A1E0
	private void InitPrefabPoolObject()
	{
		this.pool_prefabPoolObject = new List<PrefabPoolObject>(this.objectCount);
		this.activePrefabPoolObjects = new List<PrefabPoolObject>(this.objectCount);
		for (int i = 0; i < this.objectCount; i++)
		{
			this.pool_prefabPoolObject.Add(this.CreateNewPrefabPoolObject());
		}
	}

	// Token: 0x06000217 RID: 535 RVA: 0x0000C034 File Offset: 0x0000A234
	public PrefabPoolObject GetPrefabPoolObjectFromPool()
	{
		PrefabPoolObject prefabPoolObject = null;
		if (this.pool_prefabPoolObject.Count > 0)
		{
			int index = this.pool_prefabPoolObject.Count - 1;
			prefabPoolObject = this.pool_prefabPoolObject[index];
			this.pool_prefabPoolObject.RemoveAt(index);
		}
		if (prefabPoolObject == null)
		{
			prefabPoolObject = this.CreateNewPrefabPoolObject();
		}
		this.activePrefabPoolObjects.Add(prefabPoolObject);
		return prefabPoolObject;
	}

	// Token: 0x06000218 RID: 536 RVA: 0x0000C08F File Offset: 0x0000A28F
	private PrefabPoolObject CreateNewPrefabPoolObject()
	{
		PrefabPoolObject prefabPoolObject = new PrefabPoolObject();
		prefabPoolObject.gameObject = UnityEngine.Object.Instantiate<GameObject>(this.prefab);
		prefabPoolObject.particleSystem = prefabPoolObject.gameObject.GetComponent<ParticleSystem>();
		prefabPoolObject.Disable();
		return prefabPoolObject;
	}

	// Token: 0x06000219 RID: 537 RVA: 0x0000C0BE File Offset: 0x0000A2BE
	public void ReturnPrefabPoolObjectToPool(PrefabPoolObject prefabPoolObject)
	{
		prefabPoolObject.Disable();
		this.pool_prefabPoolObject.Add(prefabPoolObject);
	}

	// Token: 0x0600021A RID: 538 RVA: 0x0000C0D4 File Offset: 0x0000A2D4
	public void CleanActivePrefabPoolObjects()
	{
		for (int i = this.activePrefabPoolObjects.Count - 1; i > -1; i--)
		{
			PrefabPoolObject prefabPoolObject = this.activePrefabPoolObjects[i];
			float time = Time.time;
			float? removeAtTime = prefabPoolObject.removeAtTime;
			if ((time >= removeAtTime.GetValueOrDefault() & removeAtTime != null) || prefabPoolObject.remove)
			{
				this.activePrefabPoolObjects.RemoveAt(i);
				this.ReturnPrefabPoolObjectToPool(prefabPoolObject);
			}
		}
	}

	// Token: 0x04000165 RID: 357
	public GameObject prefab;

	// Token: 0x04000166 RID: 358
	public int objectCount = 64;

	// Token: 0x04000167 RID: 359
	public List<PrefabPoolObject> pool_prefabPoolObject;

	// Token: 0x04000168 RID: 360
	public List<PrefabPoolObject> activePrefabPoolObjects;
}
