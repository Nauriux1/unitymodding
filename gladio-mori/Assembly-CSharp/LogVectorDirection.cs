using System;
using UnityEngine;

// Token: 0x0200018B RID: 395
public class LogVectorDirection : MonoBehaviour
{
	// Token: 0x06000C67 RID: 3175 RVA: 0x0000777A File Offset: 0x0000597A
	private void Start()
	{
	}

	// Token: 0x06000C68 RID: 3176 RVA: 0x0003C5D0 File Offset: 0x0003A7D0
	private void Update()
	{
		Vector3 normalized = (this.targetObject.position - this.startObject.position).normalized;
		Color color = Color.blue;
		float num = Vector3.Angle(this.baseObject.up, normalized);
		if (num > 90f)
		{
			color = Color.red;
			Vector3 dir = Vector3.RotateTowards(normalized, this.baseObject.up, (num - 90f) * 0.017453292f, 1f);
			Debug.DrawRay(this.startObject.position, dir, Color.green);
		}
		Debug.Log(string.Format("{0}", num));
		Debug.DrawRay(this.startObject.position, normalized, color);
	}

	// Token: 0x040008CF RID: 2255
	public Transform baseObject;

	// Token: 0x040008D0 RID: 2256
	public Transform startObject;

	// Token: 0x040008D1 RID: 2257
	public Transform targetObject;
}
