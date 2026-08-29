using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000ED RID: 237
public class AlternativeMaterials : MonoBehaviour
{
	// Token: 0x0600080B RID: 2059 RVA: 0x0002828D File Offset: 0x0002648D
	private void Awake()
	{
		if (PlayerPrefs.GetString("AlternativeShaders", "0") == "1")
		{
			this.UpdateMaterials();
		}
	}

	// Token: 0x0600080C RID: 2060 RVA: 0x0000777A File Offset: 0x0000597A
	private void Start()
	{
	}

	// Token: 0x0600080D RID: 2061 RVA: 0x0000777A File Offset: 0x0000597A
	private void Update()
	{
	}

	// Token: 0x0600080E RID: 2062 RVA: 0x000282B0 File Offset: 0x000264B0
	public void UpdateMaterials()
	{
		foreach (AlternativeMaterials.GameobjectMaterialPair gameobjectMaterialPair in this.materials)
		{
			gameobjectMaterialPair.materialGameObject.GetComponent<MeshRenderer>().material = gameobjectMaterialPair.material;
		}
	}

	// Token: 0x0400058C RID: 1420
	public List<AlternativeMaterials.GameobjectMaterialPair> materials;

	// Token: 0x020000EE RID: 238
	[Serializable]
	public class GameobjectMaterialPair
	{
		// Token: 0x0400058D RID: 1421
		public GameObject materialGameObject;

		// Token: 0x0400058E RID: 1422
		public Material material;
	}
}
