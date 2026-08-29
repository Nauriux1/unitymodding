using System;
using System.Linq;
using Mirror;
using UnityEngine;

// Token: 0x020000F3 RID: 243
public class NetworkTransformSettings : MonoBehaviour
{
	// Token: 0x0600081E RID: 2078 RVA: 0x0000777A File Offset: 0x0000597A
	private void Awake()
	{
	}

	// Token: 0x0600081F RID: 2079 RVA: 0x0000777A File Offset: 0x0000597A
	private void Start()
	{
	}

	// Token: 0x06000820 RID: 2080 RVA: 0x000284E9 File Offset: 0x000266E9
	private void Update()
	{
		if (this.updateTransforms)
		{
			this.UpdateNetworkTransforms();
			this.updateTransforms = false;
		}
	}

	// Token: 0x06000821 RID: 2081 RVA: 0x00028500 File Offset: 0x00026700
	public void UpdateNetworkTransforms()
	{
		Debug.Log("UpdateNetworkTransforms");
		foreach (NetworkTransformReliable networkTransformReliable in UnityEngine.Object.FindObjectsOfType<NetworkTransformReliable>().ToList<NetworkTransformReliable>())
		{
			networkTransformReliable.rotationSensitivity = this.syncSensitivity;
			networkTransformReliable.positionPrecision = this.syncSensitivity;
		}
	}

	// Token: 0x04000598 RID: 1432
	public bool onlySyncOnChange = true;

	// Token: 0x04000599 RID: 1433
	public float syncSensitivity = 1E-09f;

	// Token: 0x0400059A RID: 1434
	public bool syncRotation = true;

	// Token: 0x0400059B RID: 1435
	public bool updateTransforms;
}
