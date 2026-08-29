using System;
using System.Collections.Generic;
using MoveClasses;

// Token: 0x02000091 RID: 145
public class InputHolder
{
	// Token: 0x170000F5 RID: 245
	// (get) Token: 0x06000507 RID: 1287 RVA: 0x00017FFC File Offset: 0x000161FC
	// (set) Token: 0x06000508 RID: 1288 RVA: 0x00018004 File Offset: 0x00016204
	public float Vertical { get; set; }

	// Token: 0x170000F6 RID: 246
	// (get) Token: 0x06000509 RID: 1289 RVA: 0x0001800D File Offset: 0x0001620D
	// (set) Token: 0x0600050A RID: 1290 RVA: 0x00018015 File Offset: 0x00016215
	public float Horizontal { get; set; }

	// Token: 0x170000F7 RID: 247
	// (get) Token: 0x0600050B RID: 1291 RVA: 0x0001801E File Offset: 0x0001621E
	// (set) Token: 0x0600050C RID: 1292 RVA: 0x00018026 File Offset: 0x00016226
	public float Rotation { get; set; }

	// Token: 0x170000F8 RID: 248
	// (get) Token: 0x0600050D RID: 1293 RVA: 0x0001802F File Offset: 0x0001622F
	// (set) Token: 0x0600050E RID: 1294 RVA: 0x00018037 File Offset: 0x00016237
	public List<PlayerAction> playerActions { get; set; } = new List<PlayerAction>();
}
