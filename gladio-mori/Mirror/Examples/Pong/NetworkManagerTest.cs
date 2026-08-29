using System;
using UnityEngine;

namespace Mirror.Examples.Pong
{
	// Token: 0x020002BB RID: 699
	[AddComponentMenu("")]
	public class NetworkManagerTest : NetworkManager
	{
		// Token: 0x060014F6 RID: 5366 RVA: 0x00069360 File Offset: 0x00067560
		public override void OnServerAddPlayer(NetworkConnectionToClient conn)
		{
			Transform transform = (base.numPlayers == 0) ? this.leftRacketSpawn : this.rightRacketSpawn;
			GameObject player = UnityEngine.Object.Instantiate<GameObject>(this.playerPrefab, transform.position, transform.rotation);
			NetworkServer.AddPlayerForConnection(conn, player);
			if (base.numPlayers == 2)
			{
				this.ball = UnityEngine.Object.Instantiate<GameObject>(this.spawnPrefabs.Find((GameObject prefab) => prefab.name == "Ball"));
				NetworkServer.Spawn(this.ball, null);
			}
		}

		// Token: 0x060014F7 RID: 5367 RVA: 0x000693EE File Offset: 0x000675EE
		public override void OnServerDisconnect(NetworkConnectionToClient conn)
		{
			if (this.ball != null)
			{
				NetworkServer.Destroy(this.ball);
			}
			base.OnServerDisconnect(conn);
		}

		// Token: 0x04000F72 RID: 3954
		public Transform leftRacketSpawn;

		// Token: 0x04000F73 RID: 3955
		public Transform rightRacketSpawn;

		// Token: 0x04000F74 RID: 3956
		private GameObject ball;
	}
}
