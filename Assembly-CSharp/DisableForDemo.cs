using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000040 RID: 64
public class DisableForDemo : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x060001EE RID: 494 RVA: 0x0000B710 File Offset: 0x00009910
	private void Awake()
	{
		this.button = base.GetComponent<Button>();
		this.button.interactable = false;
		this.basicButton = base.GetComponent<BasicButton>();
		if (this.basicButton != null)
		{
			this.basicButton.CheckDisableColor();
		}
		this.demoTextObject = (UnityEngine.Object.Instantiate(Resources.Load("UI/DemoText", typeof(GameObject))) as GameObject);
		if (this.demoTextObject != null)
		{
			this.demoTextObject.SetActive(false);
			this.demoTextRectTransform = this.demoTextObject.transform.GetChild(0).GetComponent<RectTransform>();
		}
	}

	// Token: 0x060001EF RID: 495 RVA: 0x0000B7B4 File Offset: 0x000099B4
	private void Update()
	{
		if (this.track && this.demoTextRectTransform != null)
		{
			this.demoTextRectTransform.position = new Vector2(Input.mousePosition.x + 20f, Input.mousePosition.y);
		}
	}

	// Token: 0x060001F0 RID: 496 RVA: 0x0000B806 File Offset: 0x00009A06
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (this.demoTextObject != null)
		{
			this.track = true;
			this.demoTextObject.SetActive(true);
		}
	}

	// Token: 0x060001F1 RID: 497 RVA: 0x0000B829 File Offset: 0x00009A29
	public void OnPointerExit(PointerEventData eventData)
	{
		if (this.demoTextObject != null)
		{
			this.track = false;
			this.demoTextObject.SetActive(false);
		}
	}

	// Token: 0x0400013F RID: 319
	private Button button;

	// Token: 0x04000140 RID: 320
	private BasicButton basicButton;

	// Token: 0x04000141 RID: 321
	private GameObject demoTextObject;

	// Token: 0x04000142 RID: 322
	private RectTransform demoTextRectTransform;

	// Token: 0x04000143 RID: 323
	private bool track;
}
