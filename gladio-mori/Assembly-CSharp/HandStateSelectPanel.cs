using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020001E5 RID: 485
public class HandStateSelectPanel : MonoBehaviour, IPointerExitHandler, IEventSystemHandler
{
	// Token: 0x06000EBA RID: 3770 RVA: 0x0004ABD3 File Offset: 0x00048DD3
	public void OnPointerExit(PointerEventData eventData)
	{
		MoveSetEditor.singleton.HideHandStatePanel();
	}
}
