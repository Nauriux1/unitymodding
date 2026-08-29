using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020000C7 RID: 199
public class ReplayTimeSlider : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	// Token: 0x060006DA RID: 1754 RVA: 0x00022B1E File Offset: 0x00020D1E
	public void OnPointerDown(PointerEventData eventData)
	{
		if (ReplayManager.singleton != null)
		{
			ReplayManager.singleton.draggingTimeline = true;
			ReplayManager.singleton.CheckTempPauseStatus();
		}
	}

	// Token: 0x060006DB RID: 1755 RVA: 0x00022B42 File Offset: 0x00020D42
	public void OnPointerUp(PointerEventData eventData)
	{
		if (ReplayManager.singleton != null)
		{
			ReplayManager.singleton.draggingTimeline = false;
			ReplayManager.singleton.CheckTempPauseStatus();
		}
	}
}
