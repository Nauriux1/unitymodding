using System;
using BasicUI;
using UnityEngine;

// Token: 0x02000036 RID: 54
public class BasicCameraBackground : MonoBehaviour
{
	// Token: 0x060001AD RID: 429 RVA: 0x00009E83 File Offset: 0x00008083
	private void Awake()
	{
		this.basicCamera = base.gameObject.GetComponent<Camera>();
		if (this.basicCamera != null)
		{
			this.basicCamera.clearFlags = CameraClearFlags.Color;
			this.basicCamera.backgroundColor = UISettings.BasicBackgroundColor;
		}
	}

	// Token: 0x040000EF RID: 239
	private Camera basicCamera;
}
