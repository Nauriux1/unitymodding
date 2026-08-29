using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200003D RID: 61
public class TransparentPlayer : MonoBehaviour
{
	// Token: 0x060001E2 RID: 482 RVA: 0x0000B1B2 File Offset: 0x000093B2
	private void Awake()
	{
		this.GenerateTransparentMesh();
	}

	// Token: 0x060001E3 RID: 483 RVA: 0x0000777A File Offset: 0x0000597A
	private void Start()
	{
	}

	// Token: 0x060001E4 RID: 484 RVA: 0x0000777A File Offset: 0x0000597A
	private void Update()
	{
	}

	// Token: 0x060001E5 RID: 485 RVA: 0x0000B1BC File Offset: 0x000093BC
	private void GenerateTransparentMesh()
	{
		this.stopWatch.Start();
		if (this.playerHealth != null)
		{
			foreach (MeshRenderer meshRenderer in this.playerHealth.physicalPlayer.GetComponentsInChildren<MeshRenderer>())
			{
				if (meshRenderer.enabled)
				{
					MeshFilter component = meshRenderer.gameObject.GetComponent<MeshFilter>();
					if (component != null)
					{
						meshRenderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
						GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.transparentPrefab, meshRenderer.transform.parent);
						gameObject.transform.position = meshRenderer.transform.position;
						gameObject.transform.rotation = meshRenderer.transform.rotation;
						gameObject.transform.localScale = meshRenderer.transform.localScale;
						gameObject.layer = 18;
						gameObject.GetComponent<MeshFilter>().mesh = component.mesh;
					}
				}
			}
		}
		this.stopWatch.Stop();
		Debug.Log(string.Format("Handle animation ticks: {0}", this.stopWatch.ElapsedTicks));
		Debug.Log(string.Format("Handle animation ms: {0}", this.stopWatch.ElapsedMilliseconds));
	}

	// Token: 0x0400012D RID: 301
	public PlayerHealth playerHealth;

	// Token: 0x0400012E RID: 302
	public GameObject transparentPrefab;

	// Token: 0x0400012F RID: 303
	private Stopwatch stopWatch = new Stopwatch();
}
