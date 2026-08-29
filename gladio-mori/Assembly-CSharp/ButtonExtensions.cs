using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001C9 RID: 457
public static class ButtonExtensions
{
	// Token: 0x06000D96 RID: 3478 RVA: 0x00045618 File Offset: 0x00043818
	public static void AddRightClickListener(this Button button, Action callback)
	{
		EventTrigger eventTrigger = button.gameObject.GetComponent<EventTrigger>() ?? button.gameObject.AddComponent<EventTrigger>();
		EventTrigger.Entry entry = new EventTrigger.Entry
		{
			eventID = EventTriggerType.PointerClick
		};
		entry.callback.AddListener(delegate(BaseEventData data)
		{
			if (((PointerEventData)data).button == PointerEventData.InputButton.Right)
			{
				callback();
			}
		});
		eventTrigger.triggers.Add(entry);
	}
}
