using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020000FE RID: 254
public class EventSystemManager : MonoBehaviour
{
	// Token: 0x0600085A RID: 2138 RVA: 0x000299D6 File Offset: 0x00027BD6
	private void Start()
	{
		this.InitEventSystemManager();
	}

	// Token: 0x0600085B RID: 2139 RVA: 0x000299DE File Offset: 0x00027BDE
	private void InitEventSystemManager()
	{
		if (EventSystemManager.singletonEventSystemManager != null)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		EventSystemManager.singletonEventSystemManager = this;
		this.eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
	}

	// Token: 0x0600085C RID: 2140 RVA: 0x00029A0F File Offset: 0x00027C0F
	public void DisableEventSystem()
	{
		if (this.eventSystem != null)
		{
			this.eventSystem.sendNavigationEvents = false;
		}
	}

	// Token: 0x0600085D RID: 2141 RVA: 0x00029A2B File Offset: 0x00027C2B
	public void EnableEventSystem()
	{
		if (this.eventSystem != null)
		{
			this.eventSystem.sendNavigationEvents = true;
		}
	}

	// Token: 0x040005CC RID: 1484
	public EventSystem eventSystem;

	// Token: 0x040005CD RID: 1485
	public static EventSystemManager singletonEventSystemManager;
}
