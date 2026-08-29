using System;
using UnityEngine;

// Token: 0x02000180 RID: 384
public class CalculateTimeToCollision : MonoBehaviour
{
	// Token: 0x06000C35 RID: 3125 RVA: 0x0003A467 File Offset: 0x00038667
	private void OnCollisionEnter(Collision collision)
	{
		if (!this.collided)
		{
			this.collided = true;
			Debug.Log(string.Format("COLLISION TIME:{0}", Time.timeSinceLevelLoad));
		}
	}

	// Token: 0x0400089F RID: 2207
	private bool collided;
}
