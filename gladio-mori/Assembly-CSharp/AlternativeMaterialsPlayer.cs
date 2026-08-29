using System;
using UnityEngine;
using Utils;

// Token: 0x020000EF RID: 239
public class AlternativeMaterialsPlayer : MonoBehaviour
{
	// Token: 0x06000811 RID: 2065 RVA: 0x00028314 File Offset: 0x00026514
	private void Awake()
	{
		if (PlayerPrefs.GetString("AlternativeShaders", "0") == "1")
		{
			this.UpdateMaterials();
		}
	}

	// Token: 0x06000812 RID: 2066 RVA: 0x0000777A File Offset: 0x0000597A
	private void Start()
	{
	}

	// Token: 0x06000813 RID: 2067 RVA: 0x0000777A File Offset: 0x0000597A
	private void Update()
	{
	}

	// Token: 0x06000814 RID: 2068 RVA: 0x00028338 File Offset: 0x00026538
	public void UpdateMaterials()
	{
		foreach (GameObject gameObject in Generic.FindChildObjectsWithComponent(base.gameObject, typeof(MeshRenderer)))
		{
			if (gameObject.name.ToLower().Contains("ball") || gameObject.name.ToLower().Contains("motor"))
			{
				gameObject.GetComponent<MeshRenderer>().material = this.ballMaterial;
			}
			else
			{
				gameObject.GetComponent<MeshRenderer>().material = this.playerMaterial;
			}
		}
	}

	// Token: 0x0400058F RID: 1423
	public Material playerMaterial;

	// Token: 0x04000590 RID: 1424
	public Material ballMaterial;

	// Token: 0x020000F0 RID: 240
	[Serializable]
	public class GameobjectMaterialPair
	{
		// Token: 0x04000591 RID: 1425
		public GameObject materialGameObject;

		// Token: 0x04000592 RID: 1426
		public Material material;
	}
}
