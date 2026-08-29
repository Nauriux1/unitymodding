using System;
using System.Collections.Generic;
using System.Linq;
using MoveClasses;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using Utils;

// Token: 0x020001BE RID: 446
public class EquipmentPanel : MonoBehaviour
{
	// Token: 0x1700018F RID: 399
	// (get) Token: 0x06000D62 RID: 3426 RVA: 0x00043D70 File Offset: 0x00041F70
	// (set) Token: 0x06000D63 RID: 3427 RVA: 0x00043D78 File Offset: 0x00041F78
	public IRoomPlayer lobbyPlayer { get; set; }

	// Token: 0x17000190 RID: 400
	// (get) Token: 0x06000D64 RID: 3428 RVA: 0x00043D81 File Offset: 0x00041F81
	// (set) Token: 0x06000D65 RID: 3429 RVA: 0x00043D89 File Offset: 0x00041F89
	public MoveSet moveSet { get; set; }

	// Token: 0x06000D66 RID: 3430 RVA: 0x00043D94 File Offset: 0x00041F94
	private void Awake()
	{
		this.equipmentSelectPanel.SetActive(false);
		using (List<EquipmentPositionButton>.Enumerator enumerator = this.equipmentPositionButtonList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				EquipmentPositionButton equipmentPositionButton = enumerator.Current;
				equipmentPositionButton.button.onClick.AddListener(delegate()
				{
					this.SelectEquipmentForSlot(equipmentPositionButton.equipmentPosition, equipmentPositionButton.button);
				});
				equipmentPositionButton.button.AddRightClickListener(delegate
				{
					this.SelectEquipment(null, equipmentPositionButton.equipmentPosition);
				});
			}
		}
		this.clearAllButton.onClick.AddListener(delegate()
		{
			this.ClearEquipment();
		});
	}

	// Token: 0x06000D67 RID: 3431 RVA: 0x00043E58 File Offset: 0x00042058
	public void SelectEquipmentForSlot(EquipmentPosition equipmentPosition, Button button)
	{
		if (this.equipmentSelectPanel.activeInHierarchy && this.currentEquipmentPosition == equipmentPosition)
		{
			this.equipmentSelectPanel.SetActive(false);
		}
		else
		{
			this.currentEquipmentPosition = equipmentPosition;
			this.DisplayEquipmentOptions(equipmentPosition, button);
		}
		this.UpdateButtonPaths();
	}

	// Token: 0x06000D68 RID: 3432 RVA: 0x00043E94 File Offset: 0x00042094
	public void DisplayEquipmentOptions(EquipmentPosition equipmentPosition, Button button)
	{
		this.buttonForSelectedItem = null;
		foreach (object obj in this.equipmentSelectPanelItemsHolder.transform)
		{
			UnityEngine.Object.Destroy(((Transform)obj).gameObject);
		}
		this.equimentSelectButtons = new List<EquipmentButtonItem>();
		List<EquipmentTypeItem> equipmentTypeItems = EquipmentTypeItem.GetEquipmentTypeItems();
		List<EquippedEquipment> list = null;
		EquipmentTypeItem equipmentTypeItem = null;
		if (this.lobbyPlayer != null)
		{
			list = this.lobbyPlayer.GetSelectedEquipment();
		}
		if (this.moveSet != null)
		{
			list = this.moveSet.defaultEquipment;
		}
		if (list != null)
		{
			EquippedEquipment equippedEquipment = (from x in list
			where x.position == this.currentEquipmentPosition
			select x).FirstOrDefault<EquippedEquipment>();
			if (equippedEquipment != null)
			{
				equipmentTypeItem = equippedEquipment.equipment;
			}
		}
		this.createButtonForEquipment(null, equipmentPosition, equipmentTypeItem == null);
		foreach (EquipmentTypeItem equipmentTypeItem2 in equipmentTypeItems)
		{
			if (equipmentTypeItem2.equipmentPositions.Contains(equipmentPosition))
			{
				this.createButtonForEquipment(equipmentTypeItem2, equipmentPosition, equipmentTypeItem != null && equipmentTypeItem2.equipmentType == equipmentTypeItem.equipmentType);
			}
		}
		this.equipmentSelectPanel.SetActive(true);
		RectTransform component = this.equipmentSelectPanelScrollView.GetComponent<RectTransform>();
		button.GetComponent<RectTransform>();
		component.sizeDelta = new Vector2(233f, 233f);
		if (this.equimentSelectButtons != null && this.equimentSelectButtons.Count > 0)
		{
			if (this.equimentSelectButtons.Count > 9)
			{
				component.sizeDelta = new Vector2(253f, 233f);
			}
			if (this.buttonForSelectedItem != null)
			{
				this.buttonForSelectedItem.Select();
				return;
			}
			this.equimentSelectButtons.First<EquipmentButtonItem>().button.Select();
		}
	}

	// Token: 0x06000D69 RID: 3433 RVA: 0x00044074 File Offset: 0x00042274
	public Button createButtonForEquipment(EquipmentTypeItem item, EquipmentPosition equipmentPosition, bool selectedItem = false)
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
		UIHelpers.UpdateEquipmentButtonVisuals(equipmentButtonItem, null);
		component.onClick.AddListener(delegate()
		{
			this.SelectEquipment(item);
		});
		if (item != null)
		{
			componentInChildren.text = item.equipmentType.ToString();
		}
		else
		{
			componentInChildren.text = "None";
		}
		Texture2D texture2D = Resources.Load<Texture2D>("Icons/Equipment/" + componentInChildren.text);
		if (texture2D != null && component2 != null)
		{
			component2.texture = texture2D;
			component2.gameObject.SetActive(true);
			componentInChildren.gameObject.SetActive(false);
			if (equipmentPosition.ToString().ToLower().Contains("right"))
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
		if (selectedItem)
		{
			this.buttonForSelectedItem = component;
		}
		this.equimentSelectButtons.Add(equipmentButtonItem);
		return component;
	}

	// Token: 0x06000D6A RID: 3434 RVA: 0x00044365 File Offset: 0x00042565
	public void SelectEquipment(EquipmentTypeItem equipment)
	{
		this.SelectEquipment(equipment, this.currentEquipmentPosition);
	}

	// Token: 0x06000D6B RID: 3435 RVA: 0x00044374 File Offset: 0x00042574
	public void SelectEquipment(EquipmentTypeItem equipment, EquipmentPosition equipmentPosition)
	{
		List<EquippedEquipment> list = null;
		if (this.lobbyPlayer != null)
		{
			list = this.lobbyPlayer.GetSelectedEquipment();
			if (list != null && IGameSettingsManager.singleton != null && IGameSettingsManager.singleton.AllowEquipmentEdit)
			{
				foreach (EquippedEquipment item in (from x in list
				where x.position == equipmentPosition
				select x).ToList<EquippedEquipment>())
				{
					list.Remove(item);
				}
				if (equipment != null)
				{
					list.Add(new EquippedEquipment
					{
						equipment = equipment,
						position = equipmentPosition
					});
				}
			}
			this.UpdateEquipmentUIAfterEquipmentChange(new EquipmentPosition?(equipmentPosition));
		}
		if (this.moveSet != null)
		{
			CommandInvoker.ExecuteCommand(new SetEquipmentCommand(this.moveSet, new EquippedEquipment
			{
				equipment = equipment,
				position = equipmentPosition
			}), false);
		}
	}

	// Token: 0x06000D6C RID: 3436 RVA: 0x00044478 File Offset: 0x00042678
	public void UpdateEquipmentUIAfterEquipmentChange(EquipmentPosition? position = null)
	{
		this.UpdateEquipmentInfo(false, false);
		this.equipmentSelectPanel.SetActive(false);
		if (position != null)
		{
			EquipmentPositionButton equipmentPositionButton = (from x in this.equipmentPositionButtonList
			where x.equipmentPosition == position.Value
			select x).FirstOrDefault<EquipmentPositionButton>();
			if (equipmentPositionButton != null)
			{
				equipmentPositionButton.button.Select();
			}
		}
		this.UpdateButtonPaths();
	}

	// Token: 0x06000D6D RID: 3437 RVA: 0x000444E4 File Offset: 0x000426E4
	public void ClearEquipment()
	{
		if (this.lobbyPlayer != null)
		{
			this.lobbyPlayer.GetSelectedEquipment().Clear();
		}
		else if (this.moveSet != null)
		{
			CommandInvoker.ExecuteCommand(new ClearEquipmentCommand(this.moveSet), false);
		}
		this.UpdateEquipmentInfo(false, false);
	}

	// Token: 0x06000D6E RID: 3438 RVA: 0x00044524 File Offset: 0x00042724
	public void UpdateEquipmentInfo(bool onlyPreview = false, bool forceUpdateButtons = false)
	{
		List<EquippedEquipment> list = null;
		if (this.lobbyPlayer != null)
		{
			list = this.lobbyPlayer.GetSelectedEquipment();
		}
		if (this.moveSet != null)
		{
			list = this.moveSet.defaultEquipment;
		}
		if (list != null)
		{
			if (!onlyPreview || forceUpdateButtons)
			{
				this.UpdateEquipmentPositionButtons(list);
			}
			if (this.playerHealth != null)
			{
				this.playerHealth.SetEquipment(list, false);
			}
			if (this.lobbyPlayer != null && !onlyPreview)
			{
				this.lobbyPlayer.SetEquipment(list);
			}
		}
	}

	// Token: 0x06000D6F RID: 3439 RVA: 0x000445A0 File Offset: 0x000427A0
	public void UpdateEquipmentPositionButtons(List<EquippedEquipment> equipmentList)
	{
		using (List<EquipmentPositionButton>.Enumerator enumerator = this.equipmentPositionButtonList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				EquipmentPositionButton button = enumerator.Current;
				RawImage component = button.button.transform.Find("RawImage").GetComponent<RawImage>();
				Text componentInChildren = button.button.transform.Find("Text").GetComponentInChildren<Text>();
				Text componentInChildren2 = button.button.transform.Find("BottomText").GetComponentInChildren<Text>();
				if (componentInChildren2 != null)
				{
					componentInChildren2.gameObject.SetActive(false);
				}
				if (component == null || componentInChildren == null)
				{
					return;
				}
				string text = "None";
				EquippedEquipment equippedEquipment = (from x in equipmentList
				where x.position == button.equipmentPosition
				select x).FirstOrDefault<EquippedEquipment>();
				if (equippedEquipment != null)
				{
					text = equippedEquipment.equipment.equipmentType.ToString();
					if (equippedEquipment.equipment != null)
					{
						componentInChildren2.gameObject.SetActive(true);
						componentInChildren2.text = LocalizationHelpers.LocalizedText("txt_points_short", new object[]
						{
							equippedEquipment.equipment.equipmentPoints
						});
					}
				}
				if (!string.IsNullOrEmpty(text))
				{
					Texture2D texture2D = Resources.Load<Texture2D>("Icons/Equipment/" + text);
					if (texture2D != null && component != null)
					{
						component.texture = texture2D;
						component.gameObject.SetActive(true);
						componentInChildren.gameObject.SetActive(false);
						if (button.equipmentPosition.ToString().ToLower().Contains("right"))
						{
							component.rectTransform.localScale = new Vector3(-1f, 1f, 1f);
						}
					}
					else
					{
						component.gameObject.SetActive(false);
						componentInChildren.gameObject.SetActive(true);
						componentInChildren.text = text;
					}
				}
				else
				{
					component.gameObject.SetActive(false);
					componentInChildren.gameObject.SetActive(true);
				}
				button.button.interactable = IGameSettingsManager.singleton.AllowEquipmentEdit;
				this.UpdateStartHoldInputs(button, equippedEquipment);
			}
		}
		this.clearAllButton.interactable = IGameSettingsManager.singleton.AllowEquipmentEdit;
		if (this.EquipmentPointsTotalText != null && equipmentList != null)
		{
			int num = GameSettingsHelper.CountEquippedEquipmentPoints(equipmentList);
			this.EquipmentPointsTotalText.text = LocalizationHelpers.LocalizedText("txt_total_points", new object[]
			{
				num
			});
		}
		if (this.playerCanvasContoller != null)
		{
			this.playerCanvasContoller.UpdateEquipmentPoints();
		}
	}

	// Token: 0x06000D70 RID: 3440 RVA: 0x00044884 File Offset: 0x00042A84
	private void UpdateStartHoldInputs(EquipmentPositionButton button, EquippedEquipment equippedEquipment)
	{
		if (button.equipmentPosition == EquipmentPosition.HandLeft || button.equipmentPosition == EquipmentPosition.HandRight)
		{
			Toggle startingReverseHoldToggle = this.startingReverseHoldRight;
			Slider startingHoldPositionSlider = this.startingHoldPositionRight;
			if (button.equipmentPosition == EquipmentPosition.HandLeft)
			{
				startingReverseHoldToggle = this.startingReverseHoldLeft;
				startingHoldPositionSlider = this.startingHoldPositionLeft;
			}
			startingReverseHoldToggle.onValueChanged.RemoveAllListeners();
			startingHoldPositionSlider.onValueChanged.RemoveAllListeners();
			startingReverseHoldToggle.interactable = false;
			startingHoldPositionSlider.interactable = false;
			if (equippedEquipment != null)
			{
				startingReverseHoldToggle.isOn = (equippedEquipment.equipmentStartHoldType == EquipmentStartHandleRotation.Reverse);
				GameObject gameObject = (from x in this.playerHealth.equipmentList
				where x.name == equippedEquipment.equipment.equipmentType.ToString()
				select x).FirstOrDefault<GameObject>();
				if (gameObject != null)
				{
					Handle componentInChildren = gameObject.GetComponentInChildren<Handle>();
					if (componentInChildren != null)
					{
						startingHoldPositionSlider.maxValue = componentInChildren.StartHoldPositionLimit(false);
						startingHoldPositionSlider.minValue = componentInChildren.StartHoldPositionLimit(true);
						startingHoldPositionSlider.value = equippedEquipment.equipmentStartHoldPosition;
						startingHoldPositionSlider.onValueChanged.AddListener(delegate(float <p0>)
						{
							this.OnStartingHoldPositionChanged(startingHoldPositionSlider, equippedEquipment);
						});
						startingReverseHoldToggle.onValueChanged.AddListener(delegate(bool <p0>)
						{
							this.OnStartingHoldReverseChanged(startingReverseHoldToggle, equippedEquipment);
						});
						if (IGameSettingsManager.singleton.AllowEquipmentEdit)
						{
							startingHoldPositionSlider.interactable = componentInChildren.IsTwoHanded;
							startingReverseHoldToggle.interactable = true;
						}
					}
				}
			}
		}
	}

	// Token: 0x06000D71 RID: 3441 RVA: 0x00044A4C File Offset: 0x00042C4C
	public void CloseEquipmentSelectPanel(MultiplayerEventSystem eventSystem)
	{
		if (this.equipmentSelectPanel.activeInHierarchy)
		{
			this.equipmentSelectPanel.SetActive(false);
			EquipmentPositionButton equipmentPositionButton = (from x in this.equipmentPositionButtonList
			where x.equipmentPosition == this.currentEquipmentPosition
			select x).FirstOrDefault<EquipmentPositionButton>();
			if (eventSystem == null)
			{
				if (equipmentPositionButton != null)
				{
					equipmentPositionButton.button.Select();
					return;
				}
				this.equipmentPositionButtonList.First<EquipmentPositionButton>().button.Select();
				return;
			}
			else
			{
				if (equipmentPositionButton != null)
				{
					eventSystem.SetSelectedGameObject(equipmentPositionButton.button.gameObject);
					return;
				}
				eventSystem.SetSelectedGameObject(this.equipmentPositionButtonList.First<EquipmentPositionButton>().button.gameObject);
			}
		}
	}

	// Token: 0x06000D72 RID: 3442 RVA: 0x00044AF0 File Offset: 0x00042CF0
	public void UpdateAllButtonsUI()
	{
		foreach (EquipmentButtonItem equipmentButtonItem in this.equimentSelectButtons)
		{
			UIHelpers.UpdateEquipmentButtonVisuals(equipmentButtonItem, null);
		}
	}

	// Token: 0x06000D73 RID: 3443 RVA: 0x00044B4C File Offset: 0x00042D4C
	public void UpdateButtonPaths()
	{
		int num = 0;
		if (this.equipmentSelectPanel.activeInHierarchy)
		{
			foreach (EquipmentButtonItem equipmentButtonItem in this.equimentSelectButtons)
			{
				Navigation navigation = default(Navigation);
				navigation.mode = Navigation.Mode.Explicit;
				Button selectOnRight = null;
				Button selectOnLeft = null;
				Button selectOnUp = null;
				Button button = null;
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
					selectOnUp = this.equimentSelectButtons[num - 3].button;
				}
				if (num < this.equimentSelectButtons.Count - 3)
				{
					button = this.equimentSelectButtons[num + 3].button;
				}
				if (button == null && num2 > 0 && num < this.equimentSelectButtons.Count - 2)
				{
					button = this.equimentSelectButtons[num + 2].button;
				}
				if (button == null && num2 > 1 && num < this.equimentSelectButtons.Count - 1)
				{
					button = this.equimentSelectButtons[num + 1].button;
				}
				navigation.selectOnUp = selectOnUp;
				navigation.selectOnDown = button;
				navigation.selectOnLeft = selectOnLeft;
				navigation.selectOnRight = selectOnRight;
				equipmentButtonItem.button.navigation = navigation;
				num++;
			}
		}
	}

	// Token: 0x06000D74 RID: 3444 RVA: 0x00044D08 File Offset: 0x00042F08
	public void OnStartingHoldPositionChanged(Slider slider, EquippedEquipment equippedEquipment)
	{
		if (equippedEquipment != null)
		{
			equippedEquipment.equipmentStartHoldPosition = slider.value;
			if (this.lobbyPlayer != null)
			{
				this.lobbyPlayer.SetEquipmentStartingHold(equippedEquipment);
			}
			this.UpdateEquipmentInfo(true, false);
		}
	}

	// Token: 0x06000D75 RID: 3445 RVA: 0x00044D35 File Offset: 0x00042F35
	public void OnStartingHoldReverseChanged(Toggle toggle, EquippedEquipment equippedEquipment)
	{
		if (equippedEquipment != null)
		{
			equippedEquipment.equipmentStartHoldType = (toggle.isOn ? EquipmentStartHandleRotation.Reverse : EquipmentStartHandleRotation.Default);
			if (this.lobbyPlayer != null)
			{
				this.lobbyPlayer.SetEquipmentStartingHold(equippedEquipment);
			}
			this.UpdateEquipmentInfo(true, false);
		}
	}

	// Token: 0x040009AA RID: 2474
	public PlayerHealth playerHealth;

	// Token: 0x040009AB RID: 2475
	public GameObject equipmentSelectPanel;

	// Token: 0x040009AC RID: 2476
	public GameObject equipmentSelectPanelScrollView;

	// Token: 0x040009AD RID: 2477
	public GameObject equipmentSelectPanelItemsHolder;

	// Token: 0x040009AE RID: 2478
	public ScrollRect equipmentSelectScrollRect;

	// Token: 0x040009AF RID: 2479
	public GameObject buttonPrefab;

	// Token: 0x040009B0 RID: 2480
	public Text EquipmentPointsTotalText;

	// Token: 0x040009B1 RID: 2481
	public List<EquipmentPositionButton> equipmentPositionButtonList;

	// Token: 0x040009B2 RID: 2482
	public EquipmentPosition currentEquipmentPosition;

	// Token: 0x040009B3 RID: 2483
	public List<EquipmentButtonItem> equimentSelectButtons;

	// Token: 0x040009B4 RID: 2484
	public PlayerCanvasController playerCanvasContoller;

	// Token: 0x040009B6 RID: 2486
	public Button clearAllButton;

	// Token: 0x040009B7 RID: 2487
	public Toggle startingReverseHoldRight;

	// Token: 0x040009B8 RID: 2488
	public Slider startingHoldPositionRight;

	// Token: 0x040009B9 RID: 2489
	public Toggle startingReverseHoldLeft;

	// Token: 0x040009BA RID: 2490
	public Slider startingHoldPositionLeft;

	// Token: 0x040009BB RID: 2491
	public Button LeftButton;

	// Token: 0x040009BC RID: 2492
	public Button RightButton;

	// Token: 0x040009BD RID: 2493
	public Button TopButton;

	// Token: 0x040009BE RID: 2494
	public Button BottomRight;

	// Token: 0x040009BF RID: 2495
	public Button BottomCenter;

	// Token: 0x040009C0 RID: 2496
	public Button BottomLeft;

	// Token: 0x040009C1 RID: 2497
	private Button buttonForSelectedItem;
}
