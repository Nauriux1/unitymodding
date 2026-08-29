using System;
using UnityEngine;

// Token: 0x0200005A RID: 90
public class GamePlayer
{
	// Token: 0x170000A8 RID: 168
	// (get) Token: 0x06000296 RID: 662 RVA: 0x0000C7ED File Offset: 0x0000A9ED
	// (set) Token: 0x06000297 RID: 663 RVA: 0x0000C7F5 File Offset: 0x0000A9F5
	public GameObject playerGameObject { get; set; }

	// Token: 0x170000A9 RID: 169
	// (get) Token: 0x06000298 RID: 664 RVA: 0x0000C7FE File Offset: 0x0000A9FE
	// (set) Token: 0x06000299 RID: 665 RVA: 0x0000C806 File Offset: 0x0000AA06
	public PlayerHealth playerHealth { get; set; }

	// Token: 0x170000AA RID: 170
	// (get) Token: 0x0600029A RID: 666 RVA: 0x0000C80F File Offset: 0x0000AA0F
	// (set) Token: 0x0600029B RID: 667 RVA: 0x0000C817 File Offset: 0x0000AA17
	public Camera camera { get; set; }

	// Token: 0x170000AB RID: 171
	// (get) Token: 0x0600029C RID: 668 RVA: 0x0000C820 File Offset: 0x0000AA20
	// (set) Token: 0x0600029D RID: 669 RVA: 0x0000C828 File Offset: 0x0000AA28
	public PlayerInputManager playerInputManager { get; set; }
}
