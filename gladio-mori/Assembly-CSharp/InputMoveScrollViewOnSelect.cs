using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

// Token: 0x020001BC RID: 444
public class InputMoveScrollViewOnSelect : MonoBehaviour, ISelectHandler, IEventSystemHandler
{
	// Token: 0x06000D5B RID: 3419 RVA: 0x00043D28 File Offset: 0x00041F28
	private void Awake()
	{
		this.rectTransform = base.gameObject.GetComponent<RectTransform>();
	}

	// Token: 0x06000D5C RID: 3420 RVA: 0x00043D3B File Offset: 0x00041F3B
	void ISelectHandler.OnSelect(BaseEventData eventData)
	{
		if (this.horizontal)
		{
			UIHelpers.SnapHorizontalScrollViewTo(this.rectTransform, this.scrollRect);
			return;
		}
		UIHelpers.SnapScrollViewTo(this.rectTransform, this.scrollRect);
	}

	// Token: 0x040009A6 RID: 2470
	public ScrollRect scrollRect;

	// Token: 0x040009A7 RID: 2471
	public RectTransform rectTransform;

	// Token: 0x040009A8 RID: 2472
	public bool horizontal;
}
