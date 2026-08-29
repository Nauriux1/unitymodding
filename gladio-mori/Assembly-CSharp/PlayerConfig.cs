using System;
using Es.InkPainter;
using UnityEngine;

// Token: 0x02000158 RID: 344
public class PlayerConfig : MonoBehaviour
{
	// Token: 0x06000ADA RID: 2778 RVA: 0x00033C1D File Offset: 0x00031E1D
	private void Start()
	{
		this.ConfigureParts(base.gameObject.transform);
	}

	// Token: 0x06000ADB RID: 2779 RVA: 0x0000777A File Offset: 0x0000597A
	private void Update()
	{
	}

	// Token: 0x06000ADC RID: 2780 RVA: 0x00033C30 File Offset: 0x00031E30
	private void ConfigureParts(Transform parent)
	{
		for (int i = 0; i < parent.childCount; i++)
		{
			Transform child = parent.GetChild(i);
			if ((child.GetComponent<BoxCollider>() != null || child.GetComponent<MeshCollider>() != null || child.GetComponent<CapsuleCollider>() != null || child.GetComponent<SphereCollider>() != null) && child.GetComponent<MeshRenderer>() != null)
			{
				child.gameObject.AddComponent<InkCanvas>();
			}
			this.ConfigureParts(child);
		}
	}
}
