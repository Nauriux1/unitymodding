using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000227 RID: 551
public class SelectOnInput : MonoBehaviour
{
	// Token: 0x060010B5 RID: 4277 RVA: 0x0000777A File Offset: 0x0000597A
	private void Start()
	{
	}

	// Token: 0x060010B6 RID: 4278 RVA: 0x0000777A File Offset: 0x0000597A
	private void Update()
	{
	}

	// Token: 0x060010B7 RID: 4279 RVA: 0x000562B0 File Offset: 0x000544B0
	private void OnDisable()
	{
		this.buttonSelected = false;
	}

	// Token: 0x04000C18 RID: 3096
	public EventSystem eventSystem;

	// Token: 0x04000C19 RID: 3097
	public GameObject selectedObject;

	// Token: 0x04000C1A RID: 3098
	private bool buttonSelected;
}
