using System;
using Mirror;
using MoveClasses;

// Token: 0x0200007B RID: 123
public class PlayerGameStateInfo
{
	// Token: 0x170000AF RID: 175
	// (get) Token: 0x06000351 RID: 849 RVA: 0x000115BE File Offset: 0x0000F7BE
	// (set) Token: 0x06000352 RID: 850 RVA: 0x000115C6 File Offset: 0x0000F7C6
	public PlayerHealth player { get; set; }

	// Token: 0x170000B0 RID: 176
	// (get) Token: 0x06000353 RID: 851 RVA: 0x000115CF File Offset: 0x0000F7CF
	// (set) Token: 0x06000354 RID: 852 RVA: 0x000115D7 File Offset: 0x0000F7D7
	public float? deathTime { get; set; }

	// Token: 0x170000B1 RID: 177
	// (get) Token: 0x06000355 RID: 853 RVA: 0x000115E0 File Offset: 0x0000F7E0
	// (set) Token: 0x06000356 RID: 854 RVA: 0x000115E8 File Offset: 0x0000F7E8
	public DeathReason deathReason { get; set; }

	// Token: 0x170000B2 RID: 178
	// (get) Token: 0x06000357 RID: 855 RVA: 0x000115F1 File Offset: 0x0000F7F1
	// (set) Token: 0x06000358 RID: 856 RVA: 0x000115F9 File Offset: 0x0000F7F9
	public PlayerHealth killer { get; set; }

	// Token: 0x170000B3 RID: 179
	// (get) Token: 0x06000359 RID: 857 RVA: 0x00011602 File Offset: 0x0000F802
	// (set) Token: 0x0600035A RID: 858 RVA: 0x0001160A File Offset: 0x0000F80A
	public MultiplayerRoomPlayer roomPlayer { get; set; }
}
