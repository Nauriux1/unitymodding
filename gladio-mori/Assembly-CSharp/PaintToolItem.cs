using System;
using UnityEngine;
using UnityEngine.UI;
using Utils;

// Token: 0x02000136 RID: 310
[Serializable]
public class PaintToolItem : MonoBehaviour
{
	// Token: 0x060009A9 RID: 2473 RVA: 0x0002DD30 File Offset: 0x0002BF30
	public void ActivateTool()
	{
		UIHelpers.SetButtonColor(this.toolButton, ButtonState.Selected, null, null);
		this.toolParentGameObject.SetActive(true);
	}

	// Token: 0x060009AA RID: 2474 RVA: 0x0002DD4C File Offset: 0x0002BF4C
	public void DeactivateTool()
	{
		UIHelpers.SetButtonColor(this.toolButton, ButtonState.Basic, null, null);
		this.toolParentGameObject.SetActive(false);
	}

	// Token: 0x060009AB RID: 2475 RVA: 0x0002DD68 File Offset: 0x0002BF68
	public void TempDisable(bool disableValue)
	{
		if (this.tempDisable == disableValue)
		{
			return;
		}
		this.tempDisable = disableValue;
		this.toolChildGameObject.SetActive(!this.tempDisable);
	}

	// Token: 0x060009AC RID: 2476 RVA: 0x0000777A File Offset: 0x0000597A
	public virtual void Initialize()
	{
	}

	// Token: 0x060009AD RID: 2477 RVA: 0x0000777A File Offset: 0x0000597A
	public virtual void SetColor(Color color)
	{
	}

	// Token: 0x040006C4 RID: 1732
	public Button toolButton;

	// Token: 0x040006C5 RID: 1733
	public Canvas toolCanvas;

	// Token: 0x040006C6 RID: 1734
	public GameObject toolParentGameObject;

	// Token: 0x040006C7 RID: 1735
	public GameObject toolChildGameObject;

	// Token: 0x040006C8 RID: 1736
	public PaintToolType paintToolType;

	// Token: 0x040006C9 RID: 1737
	private bool tempDisable;
}
