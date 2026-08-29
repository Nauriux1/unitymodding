using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000192 RID: 402
public class TestIgnoreCollisionsBetweenObjects : MonoBehaviour
{
	// Token: 0x06000C86 RID: 3206 RVA: 0x0000777A File Offset: 0x0000597A
	private void Start()
	{
	}

	// Token: 0x06000C87 RID: 3207 RVA: 0x0003CE88 File Offset: 0x0003B088
	private void Update()
	{
		if (this.activateIgnore)
		{
			this.activateIgnore = false;
			this.DoIgnore(this.collidersList1, this.collidersList2, true);
			this.existingIgnores.Add(new TestIgnoreCollisionsBetweenObjectsGroup
			{
				colliders1 = this.collidersList1,
				colliders2 = this.collidersList2
			});
			this.collidersList1 = new List<Collider>();
			this.collidersList2 = new List<Collider>();
		}
		if (this.reset)
		{
			this.reset = false;
			foreach (TestIgnoreCollisionsBetweenObjectsGroup testIgnoreCollisionsBetweenObjectsGroup in this.existingIgnores)
			{
				this.DoIgnore(testIgnoreCollisionsBetweenObjectsGroup.colliders1, testIgnoreCollisionsBetweenObjectsGroup.colliders2, false);
			}
			this.existingIgnores = new List<TestIgnoreCollisionsBetweenObjectsGroup>();
		}
	}

	// Token: 0x06000C88 RID: 3208 RVA: 0x0003CF64 File Offset: 0x0003B164
	private void DoIgnore(List<Collider> colliders1, List<Collider> colliders2, bool ignore)
	{
		foreach (Collider collider in colliders1)
		{
			foreach (Collider collider2 in colliders2)
			{
				Physics.IgnoreCollision(collider, collider2, ignore);
			}
		}
	}

	// Token: 0x040008ED RID: 2285
	private List<TestIgnoreCollisionsBetweenObjectsGroup> existingIgnores = new List<TestIgnoreCollisionsBetweenObjectsGroup>();

	// Token: 0x040008EE RID: 2286
	public List<Collider> collidersList1 = new List<Collider>();

	// Token: 0x040008EF RID: 2287
	public List<Collider> collidersList2 = new List<Collider>();

	// Token: 0x040008F0 RID: 2288
	public bool activateIgnore;

	// Token: 0x040008F1 RID: 2289
	public bool reset;
}
