using System;
using UnityEngine;

// Token: 0x02000197 RID: 407
public class DontDestroyOnLoad : MonoBehaviour
{
	// Token: 0x06000CA1 RID: 3233 RVA: 0x0003A160 File Offset: 0x00038360
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}
}
