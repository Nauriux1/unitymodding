using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000249 RID: 585
[Serializable]
public class PenetratingObject
{
	// Token: 0x170001B2 RID: 434
	// (get) Token: 0x06001107 RID: 4359 RVA: 0x0005802A File Offset: 0x0005622A
	// (set) Token: 0x06001108 RID: 4360 RVA: 0x00058032 File Offset: 0x00056232
	public GameObject gameObject { get; set; }

	// Token: 0x170001B3 RID: 435
	// (get) Token: 0x06001109 RID: 4361 RVA: 0x0005803B File Offset: 0x0005623B
	// (set) Token: 0x0600110A RID: 4362 RVA: 0x00058043 File Offset: 0x00056243
	public Collider collider { get; set; }

	// Token: 0x170001B4 RID: 436
	// (get) Token: 0x0600110B RID: 4363 RVA: 0x0005804C File Offset: 0x0005624C
	// (set) Token: 0x0600110C RID: 4364 RVA: 0x00058054 File Offset: 0x00056254
	public Vector3 localPenetrationStartPoint { get; set; }

	// Token: 0x170001B5 RID: 437
	// (get) Token: 0x0600110D RID: 4365 RVA: 0x0005805D File Offset: 0x0005625D
	// (set) Token: 0x0600110E RID: 4366 RVA: 0x00058065 File Offset: 0x00056265
	public Vector3 localPenetrationEndPoint { get; set; }

	// Token: 0x170001B6 RID: 438
	// (get) Token: 0x0600110F RID: 4367 RVA: 0x0005806E File Offset: 0x0005626E
	// (set) Token: 0x06001110 RID: 4368 RVA: 0x00058076 File Offset: 0x00056276
	public bool enterSide { get; set; }

	// Token: 0x170001B7 RID: 439
	// (get) Token: 0x06001111 RID: 4369 RVA: 0x0005807F File Offset: 0x0005627F
	// (set) Token: 0x06001112 RID: 4370 RVA: 0x00058087 File Offset: 0x00056287
	public bool handled { get; set; }

	// Token: 0x04000CC3 RID: 3267
	public List<BladePaintable> bladePaintables;

	// Token: 0x04000CC4 RID: 3268
	public List<CuttableGameObject> cuttableGameObjects;

	// Token: 0x04000CC5 RID: 3269
	public List<CutItem> cutItems;
}
