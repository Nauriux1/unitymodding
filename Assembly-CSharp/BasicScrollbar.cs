using System;
using UnityEngine;
using UnityEngine.UI;
using Utils;

// Token: 0x020001B1 RID: 433
public class BasicScrollbar : MonoBehaviour
{
	// Token: 0x06000D3C RID: 3388 RVA: 0x000431A1 File Offset: 0x000413A1
	private void Awake()
	{
		this.scrollbar = base.gameObject.GetComponent<Scrollbar>();
		if (this.scrollbar != null)
		{
			UIHelpers.SetScrollbarColor(this.scrollbar);
		}
	}

	// Token: 0x04000987 RID: 2439
	private Scrollbar scrollbar;
}
