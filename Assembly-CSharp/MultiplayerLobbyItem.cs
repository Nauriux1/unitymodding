using System;
using UnityEngine;

// Token: 0x02000126 RID: 294
public class MultiplayerLobbyItem
{
	// Token: 0x17000144 RID: 324
	// (get) Token: 0x06000925 RID: 2341 RVA: 0x0002C413 File Offset: 0x0002A613
	// (set) Token: 0x06000926 RID: 2342 RVA: 0x0002C41B File Offset: 0x0002A61B
	public string address { get; set; }

	// Token: 0x17000145 RID: 325
	// (get) Token: 0x06000927 RID: 2343 RVA: 0x0002C424 File Offset: 0x0002A624
	// (set) Token: 0x06000928 RID: 2344 RVA: 0x0002C42C File Offset: 0x0002A62C
	public string name { get; set; }

	// Token: 0x17000146 RID: 326
	// (get) Token: 0x06000929 RID: 2345 RVA: 0x0002C435 File Offset: 0x0002A635
	// (set) Token: 0x0600092A RID: 2346 RVA: 0x0002C43D File Offset: 0x0002A63D
	public int currentPlayers { get; set; }

	// Token: 0x17000147 RID: 327
	// (get) Token: 0x0600092B RID: 2347 RVA: 0x0002C446 File Offset: 0x0002A646
	// (set) Token: 0x0600092C RID: 2348 RVA: 0x0002C44E File Offset: 0x0002A64E
	public int maxPlayers { get; set; }

	// Token: 0x17000148 RID: 328
	// (get) Token: 0x0600092D RID: 2349 RVA: 0x0002C457 File Offset: 0x0002A657
	// (set) Token: 0x0600092E RID: 2350 RVA: 0x0002C45F File Offset: 0x0002A65F
	public ulong lobbyID { get; set; }

	// Token: 0x17000149 RID: 329
	// (get) Token: 0x0600092F RID: 2351 RVA: 0x0002C468 File Offset: 0x0002A668
	// (set) Token: 0x06000930 RID: 2352 RVA: 0x0002C470 File Offset: 0x0002A670
	public object actualLobby { get; set; }

	// Token: 0x1700014A RID: 330
	// (get) Token: 0x06000931 RID: 2353 RVA: 0x0002C479 File Offset: 0x0002A679
	// (set) Token: 0x06000932 RID: 2354 RVA: 0x0002C481 File Offset: 0x0002A681
	public int ping { get; set; }

	// Token: 0x1700014B RID: 331
	// (get) Token: 0x06000933 RID: 2355 RVA: 0x0002C48A File Offset: 0x0002A68A
	// (set) Token: 0x06000934 RID: 2356 RVA: 0x0002C492 File Offset: 0x0002A692
	public string lobbyLocationString { get; set; }

	// Token: 0x1700014C RID: 332
	// (get) Token: 0x06000935 RID: 2357 RVA: 0x0002C49B File Offset: 0x0002A69B
	// (set) Token: 0x06000936 RID: 2358 RVA: 0x0002C4A3 File Offset: 0x0002A6A3
	public string lobbyTimeScaleString { get; set; }

	// Token: 0x1700014D RID: 333
	// (get) Token: 0x06000937 RID: 2359 RVA: 0x0002C4AC File Offset: 0x0002A6AC
	// (set) Token: 0x06000938 RID: 2360 RVA: 0x0002C4B4 File Offset: 0x0002A6B4
	public GameObject gameObjectOnList { get; set; }

	// Token: 0x1700014E RID: 334
	// (get) Token: 0x06000939 RID: 2361 RVA: 0x0002C4BD File Offset: 0x0002A6BD
	// (set) Token: 0x0600093A RID: 2362 RVA: 0x0002C4C5 File Offset: 0x0002A6C5
	public LobbyStatus lobbyStatus { get; set; }

	// Token: 0x1700014F RID: 335
	// (get) Token: 0x0600093B RID: 2363 RVA: 0x0002C4CE File Offset: 0x0002A6CE
	// (set) Token: 0x0600093C RID: 2364 RVA: 0x0002C4D6 File Offset: 0x0002A6D6
	public GameTypes gameType { get; set; }

	// Token: 0x17000150 RID: 336
	// (get) Token: 0x0600093D RID: 2365 RVA: 0x0002C4DF File Offset: 0x0002A6DF
	// (set) Token: 0x0600093E RID: 2366 RVA: 0x0002C4E7 File Offset: 0x0002A6E7
	public int points { get; set; }

	// Token: 0x17000151 RID: 337
	// (get) Token: 0x0600093F RID: 2367 RVA: 0x0002C4F0 File Offset: 0x0002A6F0
	// (set) Token: 0x06000940 RID: 2368 RVA: 0x0002C4F8 File Offset: 0x0002A6F8
	public bool dismemberment { get; set; }

	// Token: 0x17000152 RID: 338
	// (get) Token: 0x06000941 RID: 2369 RVA: 0x0002C501 File Offset: 0x0002A701
	// (set) Token: 0x06000942 RID: 2370 RVA: 0x0002C509 File Offset: 0x0002A709
	public bool stamina { get; set; }
}
