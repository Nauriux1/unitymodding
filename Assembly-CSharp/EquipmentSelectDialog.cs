using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using MoveClasses;
using UnityEngine;
using UnityEngine.UI;
using Utils;

// Token: 0x020001C7 RID: 455
public class EquipmentSelectDialog : MonoBehaviour
{
	// Token: 0x06000D8B RID: 3467 RVA: 0x00044E98 File Offset: 0x00043098
	private void Start()
	{
		this.closeButton.onClick.AddListener(delegate()
		{
			this.Close();
		});
		this.DisplayEquipmentOptions(null);
		this.UpdateButtonPaths();
	}

	// Token: 0x06000D8C RID: 3468 RVA: 0x00044ED8 File Offset: 0x000430D8
	public void DisplayEquipmentOptions(EquipmentPosition? equipmentPosition = null)
	{
		foreach (object obj in this.equipmentSelectPanelItemsHolder.transform)
		{
			UnityEngine.Object.Destroy(((Transform)obj).gameObject);
		}
		this.equimentSelectButtons = new List<EquipmentButtonItem>();
		List<EquipmentTypeItem> equipmentTypeItems = EquipmentTypeItem.GetEquipmentTypeItems();
		EquipmentTypeItem equipmentTypeItem = null;
		if (equipmentPosition != null)
		{
			this.createButtonForEquipment(null, equipmentPosition, equipmentTypeItem == null);
		}
		foreach (EquipmentTypeItem equipmentTypeItem2 in equipmentTypeItems)
		{
			if (equipmentPosition == null || equipmentTypeItem2.equipmentPositions.Contains(equipmentPosition.Value))
			{
				this.createButtonForEquipment(equipmentTypeItem2, equipmentPosition, equipmentTypeItem != null && equipmentTypeItem2.equipmentType == equipmentTypeItem.equipmentType);
			}
		}
		RectTransform component = this.equipmentSelectPanelScrollView.GetComponent<RectTransform>();
		component.sizeDelta = new Vector2(233f, 233f);
		if (this.equimentSelectButtons != null && this.equimentSelectButtons.Count > 0)
		{
			if (this.equimentSelectButtons.Count > 9)
			{
				component.sizeDelta = new Vector2(253f, 233f);
			}
			this.equimentSelectButtons.First<EquipmentButtonItem>().button.Select();
		}
	}

	// Token: 0x06000D8D RID: 3469 RVA: 0x00045048 File Offset: 0x00043248
	public Button createButtonForEquipment(EquipmentTypeItem item, EquipmentPosition? equipmentPosition = null, bool selectedItem = false)
	{
		int num = this.equimentSelectButtons.Count<EquipmentButtonItem>();
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.buttonPrefab);
		Button component = gameObject.GetComponent<Button>();
		Text componentInChildren = gameObject.transform.Find("Text").GetComponentInChildren<Text>();
		RawImage component2 = gameObject.transform.Find("RawImage").GetComponent<RawImage>();
		EquipmentButtonItem equipmentButtonItem = new EquipmentButtonItem
		{
			button = component,
			equipmentTypeItem = item
		};
		UIHelpers.SetButtonColor(component, ButtonState.Basic, null, null);
		component.onClick.AddListener(delegate()
		{
			this.SelectEquipment(equipmentButtonItem);
		});
		if (item != null)
		{
			componentInChildren.text = item.equipmentType.ToString();
		}
		else
		{
			componentInChildren.text = "None";
		}
		UIHelpers.UpdateEquipmentButtonVisuals(equipmentButtonItem, new bool?(NetworkServer.activeHost));
		Texture2D texture2D = Resources.Load<Texture2D>("Icons/Equipment/" + componentInChildren.text);
		if (texture2D != null && component2 != null)
		{
			component2.texture = texture2D;
			component2.gameObject.SetActive(true);
			componentInChildren.gameObject.SetActive(false);
			if (equipmentPosition != null && equipmentPosition.ToString().ToLower().Contains("right"))
			{
				component2.rectTransform.localScale = new Vector3(-1f, 1f, 1f);
			}
			else
			{
				component2.rectTransform.localScale = new Vector3(1f, 1f, 1f);
			}
		}
		gameObject.transform.SetParent(this.equipmentSelectPanelItemsHolder.transform);
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
		RectTransform component3 = gameObject.GetComponent<RectTransform>();
		component3.anchorMin = new Vector2(0f, 1f);
		component3.anchorMax = new Vector2(0f, 1f);
		component3.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 75f);
		component3.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 75f);
		component3.localScale = new Vector3(1f, 1f, 1f);
		component3.anchoredPosition = new Vector3(component3.rect.width / 2f, (component3.rect.height / 2f + component3.rect.height * (float)num + (float)(num * 2)) * -1f, 0f);
		gameObject.transform.SetSiblingIndex(num);
		gameObject.AddComponent<InputMoveScrollViewOnSelect>().scrollRect = this.equipmentSelectScrollRect;
		this.equimentSelectButtons.Add(equipmentButtonItem);
		return component;
	}

	// Token: 0x06000D8E RID: 3470 RVA: 0x0004532E File Offset: 0x0004352E
	public void SelectEquipment(EquipmentButtonItem equipmentButtonItem)
	{
		if (GameSettingsManagerMultiplayer.singleton != null)
		{
			GameSettingsManagerMultiplayer.singleton.ToggleDisabledEquipmentType(equipmentButtonItem.equipmentTypeItem.equipmentType);
		}
		UIHelpers.UpdateEquipmentButtonVisuals(equipmentButtonItem, new bool?(NetworkServer.activeHost));
	}

	// Token: 0x06000D8F RID: 3471 RVA: 0x00045364 File Offset: 0x00043564
	public void UpdateAllButtonsUI()
	{
		foreach (EquipmentButtonItem equipmentButtonItem in this.equimentSelectButtons)
		{
			UIHelpers.UpdateEquipmentButtonVisuals(equipmentButtonItem, new bool?(NetworkServer.activeHost));
		}
	}

	// Token: 0x06000D90 RID: 3472 RVA: 0x000117D2 File Offset: 0x0000F9D2
	public void Close()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06000D91 RID: 3473 RVA: 0x000453C0 File Offset: 0x000435C0
	public void UpdateButtonPaths()
	{
		int num = 0;
		foreach (EquipmentButtonItem equipmentButtonItem in this.equimentSelectButtons)
		{
			Navigation navigation = default(Navigation);
			navigation.mode = Navigation.Mode.Explicit;
			Button selectOnRight = null;
			Button selectOnLeft = null;
			Button button = null;
			Button button2 = null;
			int num2 = num % 3;
			if ((num2 == 0 || num2 == 1) && num < this.equimentSelectButtons.Count - 1)
			{
				selectOnRight = this.equimentSelectButtons[num + 1].button;
			}
			if ((num2 == 1 || num2 == 2) && num - 1 >= 0)
			{
				selectOnLeft = this.equimentSelectButtons[num - 1].button;
			}
			if (num - 3 >= 0)
			{
				button = this.equimentSelectButtons[num - 3].button;
			}
			if (num < this.equimentSelectButtons.Count - 3)
			{
				button2 = this.equimentSelectButtons[num + 3].button;
			}
			if (button2 == null && num2 > 0 && num < this.equimentSelectButtons.Count - 2)
			{
				button2 = this.equimentSelectButtons[num + 2].button;
			}
			if (button2 == null && num2 > 1 && num < this.equimentSelectButtons.Count - 1)
			{
				button2 = this.equimentSelectButtons[num + 1].button;
			}
			if (button2 == null)
			{
				button2 = this.closeButton;
			}
			if (button == null)
			{
				button = this.closeButton;
			}
			navigation.selectOnUp = button;
			navigation.selectOnDown = button2;
			navigation.selectOnLeft = selectOnLeft;
			navigation.selectOnRight = selectOnRight;
			equipmentButtonItem.button.navigation = navigation;
			num++;
		}
		Navigation navigation2 = default(Navigation);
		navigation2.mode = Navigation.Mode.Explicit;
		Button button3 = this.equimentSelectButtons[2].button;
		Button button4 = this.equimentSelectButtons.Last<EquipmentButtonItem>().button;
		navigation2.selectOnUp = button4;
		navigation2.selectOnDown = button3;
		navigation2.selectOnLeft = null;
		navigation2.selectOnRight = null;
		this.closeButton.navigation = navigation2;
	}

	// Token: 0x040009D0 RID: 2512
	public Button closeButton;

	// Token: 0x040009D1 RID: 2513
	public GameObject equipmentSelectPanelScrollView;

	// Token: 0x040009D2 RID: 2514
	public GameObject equipmentSelectPanelItemsHolder;

	// Token: 0x040009D3 RID: 2515
	public ScrollRect equipmentSelectScrollRect;

	// Token: 0x040009D4 RID: 2516
	public GameObject buttonPrefab;

	// Token: 0x040009D5 RID: 2517
	public List<EquipmentButtonItem> equimentSelectButtons;
}
