using System;
using System.Collections.Generic;
using MoveClasses;
using UnityEngine;

// Token: 0x02000169 RID: 361
public class PreviewImageCharacter
{
	// Token: 0x17000176 RID: 374
	// (get) Token: 0x06000BA0 RID: 2976 RVA: 0x000381E3 File Offset: 0x000363E3
	// (set) Token: 0x06000BA1 RID: 2977 RVA: 0x000381EB File Offset: 0x000363EB
	public MoveSet moveSet { get; set; }

	// Token: 0x04000829 RID: 2089
	public List<EquippedEquipment> equippedEquipment = new List<EquippedEquipment>();

	// Token: 0x0400082A RID: 2090
	public bool ai = true;

	// Token: 0x0400082B RID: 2091
	public Texture2D customTexture;
}
