using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200006A RID: 106
public class CutManager : MonoBehaviour
{
	// Token: 0x060002E9 RID: 745 RVA: 0x0000F66D File Offset: 0x0000D86D
	private void Start()
	{
		this.InitializeCutManager();
	}

	// Token: 0x060002EA RID: 746 RVA: 0x0000F678 File Offset: 0x0000D878
	private void InitializeCutManager()
	{
		if (CutManager.singleton != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		CutManager.singleton = this;
		this.forceCutManagerOnForTesting = false;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.InitializeNativeArrays();
		this.InitCuttableGameObjects();
		SceneManager.sceneUnloaded += this.OnSceneUnloaded;
		SceneManager.activeSceneChanged += this.OnSceneChanged;
		Debug.Log("Cut manager has been setup");
	}

	// Token: 0x170000AC RID: 172
	// (get) Token: 0x060002EB RID: 747 RVA: 0x0000F6F0 File Offset: 0x0000D8F0
	public static bool cutManagerActive
	{
		get
		{
			if (CutManager._cutManagerActive == null)
			{
				CutManager._cutManagerActive = new bool?(!NetworkClient.active || NetworkServer.active);
				if (ReplayManager.singleton != null && (ReplayManager.singleton.replayMode == ReplayMode.Replay || ReplayManager.singleton.replayMode == ReplayMode.StartReplayAfterLoad))
				{
					CutManager._cutManagerActive = new bool?(false);
				}
				if (IGameSettingsManager.singleton != null && !IGameSettingsManager.singleton.UseDismemberment)
				{
					CutManager._cutManagerActive = new bool?(false);
				}
				if (CutManager.singleton != null && CutManager.singleton.forceCutManagerOnForTesting)
				{
					CutManager._cutManagerActive = new bool?(true);
				}
			}
			return CutManager._cutManagerActive.Value;
		}
	}

	// Token: 0x060002EC RID: 748 RVA: 0x0000F7A3 File Offset: 0x0000D9A3
	public static void ResetCutManagerActive()
	{
		CutManager._cutManagerActive = null;
	}

	// Token: 0x060002ED RID: 749 RVA: 0x0000F7B0 File Offset: 0x0000D9B0
	private void OnSceneChanged(Scene scene1, Scene scene2)
	{
		this.InitCuttableGameObjects();
	}

	// Token: 0x060002EE RID: 750 RVA: 0x0000F7B8 File Offset: 0x0000D9B8
	private void OnSceneUnloaded(Scene scene1)
	{
		CutManager.ResetCutManagerActive();
		this.DisposeNativeArrays();
		this.cutItems.Clear();
		this.cutItemsToRemove.Clear();
	}

	// Token: 0x060002EF RID: 751 RVA: 0x0000F7DB File Offset: 0x0000D9DB
	private void Update()
	{
		this.HandleJob();
		this.HandleCutItemRemoval();
	}

	// Token: 0x060002F0 RID: 752 RVA: 0x0000F7E9 File Offset: 0x0000D9E9
	private void OnDestroy()
	{
		this.DisposeNativeArrays();
		SceneManager.sceneUnloaded -= this.OnSceneUnloaded;
		SceneManager.activeSceneChanged -= this.OnSceneChanged;
	}

	// Token: 0x060002F1 RID: 753 RVA: 0x0000F814 File Offset: 0x0000DA14
	public CutItem AddCutItem(CuttableGameObject cuttableGameObject, Weapon weapon)
	{
		CutItem cutItem = cuttableGameObject.GetCutItemForWeapon(weapon);
		if (cutItem != null)
		{
			if (cutItem.colliderCount == 0)
			{
				this.RemoveFromCutItemListWithoutDisposing(cutItem);
			}
		}
		else
		{
			cutItem = new CutItem
			{
				cuttableGameObject = cuttableGameObject,
				weapon = weapon
			};
			cutItem.InitCutItem(false);
			this.cutItems.Add(cutItem);
			cuttableGameObject.AddActiveCutItem(cutItem);
		}
		cutItem.colliderCount++;
		return cutItem;
	}

	// Token: 0x060002F2 RID: 754 RVA: 0x0000F87C File Offset: 0x0000DA7C
	public CutItem AddCutItem(CuttableGameObject cuttableGameObject, Plane cutPlane)
	{
		CutItem cutItem = new CutItem
		{
			cuttableGameObject = cuttableGameObject,
			fullCutPlane = cutPlane
		};
		cutItem.InitCutItem(true);
		this.cutItems.Add(cutItem);
		this.RemoveCutItem(cutItem);
		return cutItem;
	}

	// Token: 0x060002F3 RID: 755 RVA: 0x0000F8B8 File Offset: 0x0000DAB8
	public void RemoveCutItem(CutItem cutItem)
	{
		cutItem.colliderCount--;
		if (cutItem.colliderCount <= 0)
		{
			this.cutItemsToRemove.Add(cutItem);
		}
	}

	// Token: 0x060002F4 RID: 756 RVA: 0x0000F8E0 File Offset: 0x0000DAE0
	private void ActuallyRemoveCutItem(CutItem cutItem)
	{
		int num = this.cutItems.IndexOf(cutItem);
		if (num > -1)
		{
			cutItem.cuttableGameObject.RemoveActiveCutItem(cutItem);
			cutItem.DisposeNativeArrays();
			this.cutItems.RemoveAt(num);
		}
	}

	// Token: 0x060002F5 RID: 757 RVA: 0x0000F91C File Offset: 0x0000DB1C
	private void HandleCutItemRemoval()
	{
		if (this.cutItemsToRemove.Count > 0)
		{
			for (int i = this.cutItemsToRemove.Count - 1; i >= 0; i--)
			{
				if (this.cutItemsToRemove[i].CanBeRemoved())
				{
					this.ActuallyRemoveCutItem(this.cutItemsToRemove[i]);
					this.cutItemsToRemove.RemoveAt(i);
				}
				else
				{
					this.cutItemsToRemove[i].queuedForRemoval = true;
					this.cutItemsToRemove[i].queuedForRemovalLoops++;
				}
			}
		}
	}

	// Token: 0x060002F6 RID: 758 RVA: 0x0000F9B0 File Offset: 0x0000DBB0
	private void RemoveFromCutItemListWithoutDisposing(CutItem cutItem)
	{
		for (int i = this.cutItemsToRemove.Count - 1; i >= 0; i--)
		{
			if (this.cutItemsToRemove[i] == cutItem)
			{
				this.cutItemsToRemove.RemoveAt(i);
			}
		}
	}

	// Token: 0x060002F7 RID: 759 RVA: 0x0000F9F0 File Offset: 0x0000DBF0
	private void InitializeNativeArrays()
	{
		this.DisposeNativeArrays();
	}

	// Token: 0x060002F8 RID: 760 RVA: 0x0000F9F8 File Offset: 0x0000DBF8
	private void DisposeNativeArrays()
	{
		if (this.cutItems.Count > 0)
		{
			for (int i = 0; i < this.cutItems.Count; i++)
			{
				this.cutItems[i].DisposeNativeArrays();
			}
		}
	}

	// Token: 0x060002F9 RID: 761 RVA: 0x0000FA3A File Offset: 0x0000DC3A
	private void HandleJob()
	{
		this.ScheduleNextJob();
	}

	// Token: 0x060002FA RID: 762 RVA: 0x0000FA44 File Offset: 0x0000DC44
	private void ScheduleNextJob()
	{
		if (this.cutItems.Count > 0)
		{
			for (int i = 0; i < this.cutItems.Count; i++)
			{
				this.cutItems[i].HandleCutItem();
			}
		}
	}

	// Token: 0x060002FB RID: 763 RVA: 0x0000FA88 File Offset: 0x0000DC88
	private void InitCuttableGameObjects()
	{
		if (this.pool_cuttableGameObject == null)
		{
			this.pool_cuttableGameObject = new List<CuttableGameObject>(64);
		}
		else
		{
			this.pool_cuttableGameObject.Clear();
		}
		if (CutManager.cutManagerActive)
		{
			for (int i = 0; i < 16; i++)
			{
				this.pool_cuttableGameObject.Add(this.CreateNewCuttableGameObject());
			}
		}
	}

	// Token: 0x060002FC RID: 764 RVA: 0x0000FADC File Offset: 0x0000DCDC
	public CuttableGameObject GetCuttableGameObjectFromPool(uint poolObjectNetID)
	{
		CuttableGameObject cuttableGameObject = null;
		if (NetworkClient.active && !NetworkServer.active)
		{
			if (NetworkClient.spawned.ContainsKey(poolObjectNetID))
			{
				cuttableGameObject = NetworkClient.spawned[poolObjectNetID].gameObject.GetComponent<CuttableGameObject>();
				cuttableGameObject.cuttableRigidbody.isKinematic = true;
				cuttableGameObject.cuttableRigidbody.interpolation = RigidbodyInterpolation.None;
			}
		}
		else
		{
			if (this.pool_cuttableGameObject.Count > 0)
			{
				int index = this.pool_cuttableGameObject.Count - 1;
				cuttableGameObject = this.pool_cuttableGameObject[index];
				this.pool_cuttableGameObject.RemoveAt(index);
			}
			if (cuttableGameObject == null)
			{
				cuttableGameObject = this.CreateNewCuttableGameObject();
			}
			cuttableGameObject.cuttableRigidbody.isKinematic = false;
		}
		return cuttableGameObject;
	}

	// Token: 0x060002FD RID: 765 RVA: 0x0000FB8B File Offset: 0x0000DD8B
	private void ReturnCuttableGameObjectToPool(CuttableGameObject cuttableGameObject)
	{
		this.pool_cuttableGameObject.Add(cuttableGameObject);
	}

	// Token: 0x060002FE RID: 766 RVA: 0x0000FB9C File Offset: 0x0000DD9C
	public CuttableGameObject CreateNewCuttableGameObject()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.cuttableGameObjectPrefab, new Vector3(-100f, -100f, -100f), default(Quaternion));
		CuttableGameObject component = gameObject.GetComponent<CuttableGameObject>();
		if (NetworkServer.active)
		{
			NetworkServer.Spawn(gameObject, null);
		}
		component.cuttableRigidbody.isKinematic = true;
		return component;
	}

	// Token: 0x060002FF RID: 767 RVA: 0x0000FBF4 File Offset: 0x0000DDF4
	public CuttableMesh GetNewCuttableMeshObject()
	{
		GameObject gameObject = new GameObject();
		MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		return new CuttableMesh
		{
			meshFilter = meshFilter,
			renderer = renderer
		};
	}

	// Token: 0x040001E6 RID: 486
	public static CutManager singleton;

	// Token: 0x040001E7 RID: 487
	public bool forceCutManagerOnForTesting;

	// Token: 0x040001E8 RID: 488
	private static bool? _cutManagerActive;

	// Token: 0x040001E9 RID: 489
	public List<CutItem> cutItems = new List<CutItem>();

	// Token: 0x040001EA RID: 490
	public List<CutItem> cutItemsToRemove = new List<CutItem>();

	// Token: 0x040001EB RID: 491
	public GameObject cuttableGameObjectPrefab;

	// Token: 0x040001EC RID: 492
	public List<CuttableGameObject> pool_cuttableGameObject;
}
