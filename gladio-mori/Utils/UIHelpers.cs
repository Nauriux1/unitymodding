using System;
using BasicUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
	// Token: 0x02000295 RID: 661
	internal class UIHelpers
	{
		// Token: 0x0600134F RID: 4943 RVA: 0x00063D48 File Offset: 0x00061F48
		public static void SetButtonColor(Button button, ButtonState state = ButtonState.Basic, string overrideColor = null, string overrideTextColor = null)
		{
			BasicButton component = button.gameObject.GetComponent<BasicButton>();
			bool flag = false;
			if (component != null)
			{
				component.buttonState = state;
				flag = component.UseNewStyle;
			}
			Color color = UIHelpers.GetColorForButtonState(state, flag);
			if (!string.IsNullOrEmpty(overrideColor))
			{
				color = UISettings.ParseColor(overrideColor, 1f);
			}
			Color color2 = UIHelpers.HighlightColor(color);
			Color color3 = UISettings.BasicDisabledColor;
			Color pressedColor = Color.Lerp(color, Color.black, 0.5f);
			if (flag)
			{
				color2 = UIHelpers.DarkenColorByValue(color, 0.5f);
				pressedColor = UIHelpers.DarkenColorByValue(color, 0.25f);
				color3 = UIHelpers.DarkenColorByValue(color, 0.2f);
				color3 = UIHelpers.SetColorAlpha(color3, 0.5f);
			}
			button.colors = new ColorBlock
			{
				normalColor = color,
				highlightedColor = color2,
				selectedColor = color2,
				pressedColor = pressedColor,
				disabledColor = color3,
				colorMultiplier = 1f,
				fadeDuration = 0f
			};
			Text componentInChildren = button.gameObject.GetComponentInChildren<Text>();
			if (componentInChildren != null)
			{
				componentInChildren.color = UISettings.BasicTextColor;
				if (!string.IsNullOrEmpty(overrideTextColor))
				{
					componentInChildren.color = UISettings.ParseColor(overrideTextColor, 1f);
				}
			}
		}

		// Token: 0x06001350 RID: 4944 RVA: 0x00063E7C File Offset: 0x0006207C
		public static void SetIconButtonColor(Button button, ButtonState state = ButtonState.Basic)
		{
			BasicButton component = button.gameObject.GetComponent<BasicButton>();
			if (component != null)
			{
				component.buttonState = state;
			}
			Color basicTextColor = UISettings.BasicTextColor;
			Color color = UIHelpers.HighlightColor(basicTextColor);
			Color pressedColor = Color.Lerp(basicTextColor, Color.black, 0.5f);
			button.colors = new ColorBlock
			{
				normalColor = basicTextColor,
				highlightedColor = color,
				selectedColor = color,
				pressedColor = pressedColor,
				colorMultiplier = 1f,
				fadeDuration = 0f
			};
			Text componentInChildren = button.gameObject.GetComponentInChildren<Text>();
			if (componentInChildren != null)
			{
				componentInChildren.color = UISettings.BasicTextColor;
			}
		}

		// Token: 0x06001351 RID: 4945 RVA: 0x00063F30 File Offset: 0x00062130
		public static void SetSliderColor(Slider slider)
		{
			Color basicButtonSelectedColor = UISettings.BasicButtonSelectedColor;
			Color color = UIHelpers.HighlightColor(basicButtonSelectedColor);
			Color disabledColor = UIHelpers.DisabledColor(basicButtonSelectedColor);
			slider.colors = new ColorBlock
			{
				normalColor = basicButtonSelectedColor,
				highlightedColor = color,
				selectedColor = color,
				pressedColor = color,
				disabledColor = disabledColor,
				colorMultiplier = 1f,
				fadeDuration = 0f
			};
			Transform transform = slider.transform.Find("Background");
			Sprite sprite = Resources.Load<Sprite>("Icons/UI/Panel");
			if (transform != null)
			{
				Image component = transform.GetComponent<Image>();
				if (component != null)
				{
					component.sprite = null;
					if (sprite != null)
					{
						component.sprite = sprite;
					}
					component.color = UISettings.BasicButtonColor;
				}
			}
			if (slider.fillRect != null)
			{
				Image component2 = slider.fillRect.GetComponent<Image>();
				if (component2 != null)
				{
					component2.sprite = null;
					if (sprite != null)
					{
						component2.sprite = sprite;
					}
					component2.color = UISettings.BasicButtonSelectedColor;
				}
			}
			if (slider.targetGraphic != null)
			{
				Image component3 = slider.targetGraphic.GetComponent<Image>();
				Sprite sprite2 = Resources.Load<Sprite>("Icons/UI/Button");
				if (sprite2 != null)
				{
					component3.sprite = sprite2;
					component3.type = Image.Type.Sliced;
					component3.pixelsPerUnitMultiplier = 1f;
				}
			}
		}

		// Token: 0x06001352 RID: 4946 RVA: 0x0006409C File Offset: 0x0006229C
		public static void SetInputFieldColor(InputField inputField, Color basicColor)
		{
			Color color = UIHelpers.HighlightColor(basicColor);
			inputField.colors = new ColorBlock
			{
				normalColor = basicColor,
				highlightedColor = color,
				selectedColor = color,
				pressedColor = color,
				colorMultiplier = 1f,
				fadeDuration = 0f
			};
		}

		// Token: 0x06001353 RID: 4947 RVA: 0x000640F8 File Offset: 0x000622F8
		public static void SetInputFieldColor(TMP_InputField inputField, Color basicColor)
		{
			Color color = UIHelpers.HighlightColor(basicColor);
			inputField.colors = new ColorBlock
			{
				normalColor = basicColor,
				highlightedColor = color,
				selectedColor = color,
				pressedColor = color,
				colorMultiplier = 1f,
				fadeDuration = 0f
			};
		}

		// Token: 0x06001354 RID: 4948 RVA: 0x00064154 File Offset: 0x00062354
		public static void SetScrollbarColor(Scrollbar scrollbar)
		{
			if (scrollbar == null)
			{
				return;
			}
			Color basicScrollbarColor = UISettings.BasicScrollbarColor;
			Color color = UIHelpers.HighlightColor(basicScrollbarColor);
			scrollbar.colors = new ColorBlock
			{
				normalColor = basicScrollbarColor,
				highlightedColor = color,
				selectedColor = color,
				pressedColor = color,
				colorMultiplier = 1f,
				fadeDuration = 0f
			};
			if (scrollbar.image != null)
			{
				scrollbar.image.sprite = null;
			}
			Image component = scrollbar.GetComponent<Image>();
			if (component != null)
			{
				component.sprite = null;
				component.type = Image.Type.Sliced;
				component.color = UISettings.BasicScrollbarBackgroundColor;
			}
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x00064204 File Offset: 0x00062404
		public static void SetToggleColor(Toggle toggle)
		{
			Color basicButtonSelectedColor = UISettings.BasicButtonSelectedColor;
			Color color = UIHelpers.HighlightColor(basicButtonSelectedColor);
			Color disabledColor = UIHelpers.DisabledColor(basicButtonSelectedColor);
			toggle.colors = new ColorBlock
			{
				normalColor = basicButtonSelectedColor,
				highlightedColor = color,
				selectedColor = color,
				pressedColor = color,
				disabledColor = disabledColor,
				colorMultiplier = 1f,
				fadeDuration = 0f
			};
			if (toggle.image != null)
			{
				toggle.image.sprite = null;
				Sprite sprite = Resources.Load<Sprite>("Icons/UI/Panel");
				if (sprite != null)
				{
					toggle.image.sprite = sprite;
					toggle.image.pixelsPerUnitMultiplier = 2f;
				}
			}
			Image image = (Image)toggle.graphic;
			if (image != null)
			{
				image.color = UISettings.BasicTextColor;
				image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 10f);
				image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 10f);
				image.sprite = null;
			}
			Transform transform = toggle.transform.Find("Label");
			if (transform != null)
			{
				Text component = transform.GetComponent<Text>();
				if (component != null)
				{
					component.color = UISettings.BasicTextColor;
				}
			}
		}

		// Token: 0x06001356 RID: 4950 RVA: 0x00064348 File Offset: 0x00062548
		public static void SetDropdownColor(Dropdown dropdown, Color basicColor)
		{
			Image image = (Image)dropdown.targetGraphic;
			Sprite sprite = Resources.Load<Sprite>("Icons/UI/Panel");
			if (sprite != null && image != null)
			{
				image.sprite = sprite;
				image.pixelsPerUnitMultiplier = 1f;
			}
			Color color = UIHelpers.HighlightColor(basicColor);
			ColorBlock colors = new ColorBlock
			{
				normalColor = basicColor,
				highlightedColor = color,
				selectedColor = color,
				pressedColor = color,
				colorMultiplier = 1f,
				fadeDuration = 0f
			};
			dropdown.colors = colors;
			dropdown.captionText.color = UISettings.BasicTextColor;
			dropdown.itemText.color = UISettings.BasicTextColor;
			Transform transform = dropdown.gameObject.transform.Find("Arrow");
			if (transform != null)
			{
				Image component = transform.GetComponent<Image>();
				Sprite sprite2 = Resources.Load<Sprite>("Icons/UI/DownArrow");
				if (sprite2 != null && component != null)
				{
					component.sprite = sprite2;
					component.pixelsPerUnitMultiplier = 1f;
					component.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 10f);
					component.color = UISettings.BasicTextColor;
				}
			}
			Transform transform2 = dropdown.gameObject.transform.Find("Template");
			if (transform2 != null)
			{
				UIHelpers.SetScrollbarColor(transform2.gameObject.transform.Find("Scrollbar").gameObject.GetComponent<Scrollbar>());
				Image component2 = transform2.GetComponent<Image>();
				if (sprite != null && component2 != null)
				{
					component2.sprite = sprite;
					component2.pixelsPerUnitMultiplier = 1f;
					component2.color = UISettings.BasicSubPanelColor;
				}
				Transform transform3 = transform2.gameObject.transform.Find("Viewport");
				if (transform3 != null)
				{
					Image component3 = transform3.GetComponent<Image>();
					if (component3 != null)
					{
						component3.sprite = null;
						component3.pixelsPerUnitMultiplier = 1f;
						component3.color = UISettings.BasicSubPanelColor;
					}
					Transform transform4 = transform3.gameObject.transform.Find("Content");
					if (transform4 != null)
					{
						Transform transform5 = transform4.gameObject.transform.Find("Item");
						if (transform5 != null)
						{
							RectTransform component4 = transform5.GetComponent<RectTransform>();
							if (component4 != null)
							{
								component4.offsetMin = new Vector2(5f, component4.offsetMin.y);
								component4.offsetMax = new Vector2(-5f, component4.offsetMax.y);
								component4.sizeDelta = new Vector2(component4.sizeDelta.x, 24f);
							}
							Toggle component5 = transform5.GetComponent<Toggle>();
							component5.colors = colors;
							Image image2 = (Image)component5.graphic;
							image2.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 10f);
							image2.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 10f);
							image2.color = UISettings.BasicTextColor;
							image2.sprite = null;
						}
					}
				}
			}
		}

		// Token: 0x06001357 RID: 4951 RVA: 0x0006465C File Offset: 0x0006285C
		public static void SetBackgroundColor(GameObject gameObject, Color basicColor)
		{
			Image component = gameObject.GetComponent<Image>();
			if (component != null)
			{
				component.color = basicColor;
			}
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x00064680 File Offset: 0x00062880
		public static Color GetColorForButtonState(ButtonState state, bool useNewStyle = false)
		{
			if (useNewStyle)
			{
				return UIHelpers.GetColorForButtonStateNewStyle(state);
			}
			Color result = UISettings.BasicButtonColor;
			switch (state)
			{
			case ButtonState.Selected:
				result = UISettings.BasicButtonSelectedColor;
				break;
			case ButtonState.Ready:
				result = UISettings.BasicButtonReadyColor;
				break;
			case ButtonState.NotReady:
				result = UISettings.BasicButtonNotReadyColor;
				break;
			}
			return result;
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x000646CC File Offset: 0x000628CC
		public static Color GetColorForButtonStateNewStyle(ButtonState state)
		{
			Color result = UISettings.BasicButtonColorNewStyle;
			switch (state)
			{
			case ButtonState.Selected:
				result = UISettings.BasicButtonSelectedColorNewStyle;
				break;
			case ButtonState.Ready:
				result = UISettings.BasicButtonReadyColorNewStyle;
				break;
			case ButtonState.NotReady:
				result = UISettings.BasicButtonNotReadyColorNewStyle;
				break;
			}
			return result;
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x0006470C File Offset: 0x0006290C
		public static Color HighlightColor(Color color)
		{
			if (color.grayscale > 0.8f)
			{
				float v = 0f;
				float num;
				float s;
				Color.RGBToHSV(color, out num, out s, out v);
				num += 0.5f;
				return Color.HSVToRGB(num, s, v);
			}
			if (color.grayscale > 0.3f)
			{
				return Color.Lerp(color, Color.white, 0.3f);
			}
			return Color.Lerp(color, Color.white, 0.15f);
		}

		// Token: 0x0600135B RID: 4955 RVA: 0x00064779 File Offset: 0x00062979
		public static Color DisabledColor(Color color)
		{
			return Color.Lerp(color, Color.black, 0.8f);
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x0006478C File Offset: 0x0006298C
		public static void SnapScrollViewTo(RectTransform target, ScrollRect scrollRect)
		{
			Canvas.ForceUpdateCanvases();
			if (UIHelpers.scrollViewChildPosition(target, scrollRect) != 0)
			{
				Vector2 v = new Vector2(0f, 0f - (scrollRect.viewport.localPosition.y + target.localPosition.y));
				scrollRect.content.localPosition = v;
			}
		}

		// Token: 0x0600135D RID: 4957 RVA: 0x000647E8 File Offset: 0x000629E8
		public static int scrollViewChildPosition(RectTransform target, ScrollRect scrollRect)
		{
			float num = Math.Abs(target.transform.localPosition.y);
			float height = scrollRect.gameObject.GetComponent<RectTransform>().rect.height;
			float y = scrollRect.content.anchoredPosition.y;
			if (num < y)
			{
				return -1;
			}
			if (num > y + height)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x0600135E RID: 4958 RVA: 0x00064844 File Offset: 0x00062A44
		public static void SnapHorizontalScrollViewTo(RectTransform target, ScrollRect scrollRect)
		{
			Canvas.ForceUpdateCanvases();
			int num = UIHelpers.scrollViewChildHorizontalPosition(target, scrollRect);
			if (num > 0)
			{
				Vector2 v = new Vector2(0f - (target.localPosition.x - scrollRect.viewport.rect.width + target.rect.width / 2f), 0f);
				scrollRect.content.localPosition = v;
				return;
			}
			if (num < 0)
			{
				Vector2 v2 = new Vector2(0f - (target.localPosition.x - target.rect.width / 2f), 0f);
				scrollRect.content.localPosition = v2;
			}
		}

		// Token: 0x0600135F RID: 4959 RVA: 0x00064904 File Offset: 0x00062B04
		public static int scrollViewChildHorizontalPosition(RectTransform target, ScrollRect scrollRect)
		{
			float num = Math.Abs(target.transform.localPosition.x);
			float width = scrollRect.gameObject.GetComponent<RectTransform>().rect.width;
			float num2 = Math.Abs(scrollRect.content.anchoredPosition.x);
			if (num < num2)
			{
				return -1;
			}
			if (num > num2 + width)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06001360 RID: 4960 RVA: 0x00064965 File Offset: 0x00062B65
		public static void DrawScreenRect(Rect rect, Color color)
		{
			GUI.color = color;
			GUI.DrawTexture(rect, UIHelpers.DefaultTexture);
			GUI.color = Color.white;
		}

		// Token: 0x06001361 RID: 4961 RVA: 0x00064984 File Offset: 0x00062B84
		public static void DrawScreenRectBorder(Rect rect, float thickness, Color color)
		{
			UIHelpers.DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
			UIHelpers.DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
			UIHelpers.DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
			UIHelpers.DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
		}

		// Token: 0x06001362 RID: 4962 RVA: 0x00064A1C File Offset: 0x00062C1C
		public static Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
		{
			screenPosition1.y = (float)Screen.height - screenPosition1.y;
			screenPosition2.y = (float)Screen.height - screenPosition2.y;
			Vector3 vector = Vector3.Min(screenPosition1, screenPosition2);
			Vector3 vector2 = Vector3.Max(screenPosition1, screenPosition2);
			return Rect.MinMaxRect(vector.x, vector.y, vector2.x, vector2.y);
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06001363 RID: 4963 RVA: 0x00064A7E File Offset: 0x00062C7E
		public static Texture2D DefaultTexture
		{
			get
			{
				if (UIHelpers._defaultTexture == null)
				{
					UIHelpers._defaultTexture = new Texture2D(1, 1);
					UIHelpers._defaultTexture.SetPixel(0, 0, Color.white);
					UIHelpers._defaultTexture.Apply();
				}
				return UIHelpers._defaultTexture;
			}
		}

		// Token: 0x06001364 RID: 4964 RVA: 0x00064ABC File Offset: 0x00062CBC
		public static void SetTextFont(Text text, FontType fontType = FontType.Basic)
		{
			if (text != null)
			{
				if (fontType == FontType.Basic)
				{
					text.font = (Font)Resources.Load("Fonts/" + UISettings.BasicFontName, typeof(Font));
					return;
				}
				if (fontType == FontType.Options)
				{
					text.font = (Font)Resources.Load("Fonts/" + UISettings.OptionsFontName, typeof(Font));
				}
			}
		}

		// Token: 0x06001365 RID: 4965 RVA: 0x00064B2C File Offset: 0x00062D2C
		public static void UpdateEquipmentButtonVisuals(EquipmentButtonItem equipmentButtonItem, bool? interactible = null)
		{
			Text componentInChildren = equipmentButtonItem.button.transform.Find("BottomText").GetComponentInChildren<Text>();
			if (componentInChildren != null)
			{
				componentInChildren.gameObject.SetActive(false);
			}
			if (equipmentButtonItem.equipmentTypeItem != null)
			{
				if (equipmentButtonItem.equipmentTypeItem.IsDisabled())
				{
					componentInChildren.gameObject.SetActive(true);
					componentInChildren.text = "X";
					if (interactible == null)
					{
						equipmentButtonItem.button.interactable = false;
					}
					else
					{
						UIHelpers.SetButtonColor(equipmentButtonItem.button, ButtonState.Basic, UISettings._basicDisabledColor, null);
					}
					componentInChildren.color = UISettings.BasicTextNotReadyColor;
				}
				else if (equipmentButtonItem.equipmentTypeItem != null)
				{
					componentInChildren.gameObject.SetActive(true);
					componentInChildren.text = LocalizationHelpers.LocalizedText("txt_points_short", new object[]
					{
						equipmentButtonItem.equipmentTypeItem.equipmentPoints
					});
					if (interactible == null)
					{
						equipmentButtonItem.button.interactable = true;
					}
					else
					{
						UIHelpers.SetButtonColor(equipmentButtonItem.button, ButtonState.Basic, null, null);
					}
					componentInChildren.color = UISettings.BasicTextColor;
				}
				if (interactible != null)
				{
					equipmentButtonItem.button.interactable = interactible.Value;
				}
			}
		}

		// Token: 0x06001366 RID: 4966 RVA: 0x00064C58 File Offset: 0x00062E58
		public static Sprite LoadSpriteFromResources(string path, string spriteName)
		{
			Sprite result = null;
			foreach (Sprite sprite in Resources.LoadAll<Sprite>(path))
			{
				if (sprite.name == spriteName)
				{
					result = sprite;
					break;
				}
			}
			return result;
		}

		// Token: 0x06001367 RID: 4967 RVA: 0x00064C94 File Offset: 0x00062E94
		public static Color DarkenColorByValue(Color color, float value)
		{
			float num = 0f;
			float h;
			float s;
			Color.RGBToHSV(color, out h, out s, out num);
			num -= value;
			return Color.HSVToRGB(h, s, num);
		}

		// Token: 0x06001368 RID: 4968 RVA: 0x00064CBF File Offset: 0x00062EBF
		public static Color SetColorAlpha(Color color, float value)
		{
			color.a = value;
			return color;
		}

		// Token: 0x06001369 RID: 4969 RVA: 0x00064CCC File Offset: 0x00062ECC
		public static void SetUpNavitagionForSelectable(Selectable navigationItem, Selectable target)
		{
			if (navigationItem == null)
			{
				return;
			}
			Navigation navigation = navigationItem.navigation;
			navigation.selectOnUp = target;
			navigationItem.navigation = navigation;
		}

		// Token: 0x0600136A RID: 4970 RVA: 0x00064CFC File Offset: 0x00062EFC
		public static void SetDownNavitagionForSelectable(Selectable navigationItem, Selectable target)
		{
			if (navigationItem == null)
			{
				return;
			}
			Navigation navigation = navigationItem.navigation;
			navigation.selectOnDown = target;
			navigationItem.navigation = navigation;
		}

		// Token: 0x04000E57 RID: 3671
		private static Texture2D _defaultTexture;
	}
}
