using System;
using UnityEngine;

// Token: 0x0200017C RID: 380
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	// Token: 0x1700017F RID: 383
	// (get) Token: 0x06000C22 RID: 3106 RVA: 0x00039FF0 File Offset: 0x000381F0
	public static T instance
	{
		get
		{
			if (Singleton<T>._instance == null)
			{
				Singleton<T>._instance = UnityEngine.Object.FindObjectOfType<T>();
				if (Singleton<T>._instance == null)
				{
					Singleton<T>._instance = new GameObject
					{
						name = typeof(T).ToString()
					}.AddComponent<T>();
				}
			}
			return Singleton<T>._instance;
		}
	}

	// Token: 0x17000180 RID: 384
	// (get) Token: 0x06000C23 RID: 3107 RVA: 0x0003A054 File Offset: 0x00038254
	public static bool isInstanceAlive
	{
		get
		{
			return Singleton<T>._instance != null;
		}
	}

	// Token: 0x06000C24 RID: 3108 RVA: 0x0003A068 File Offset: 0x00038268
	public virtual void Awake()
	{
		if (Singleton<T>._instance != null)
		{
			if (Singleton<T>.verbose)
			{
				Debug.Log("SingleAccessPoint, Destroy duplicate instance " + base.name + " of " + Singleton<T>.instance.name);
			}
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		Singleton<T>._instance = base.GetComponent<T>();
		if (Singleton<T>.keepAlive)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		if (Singleton<T>._instance == null)
		{
			if (Singleton<T>.verbose)
			{
				Debug.LogError("SingleAccessPoint<" + typeof(T).Name + "> Instance null in Awake");
			}
			return;
		}
		if (Singleton<T>.verbose)
		{
			Debug.Log("SingleAccessPoint instance found " + Singleton<T>.instance.GetType().Name);
		}
	}

	// Token: 0x04000893 RID: 2195
	public static bool verbose = false;

	// Token: 0x04000894 RID: 2196
	public static bool keepAlive = true;

	// Token: 0x04000895 RID: 2197
	private static T _instance = default(T);
}
