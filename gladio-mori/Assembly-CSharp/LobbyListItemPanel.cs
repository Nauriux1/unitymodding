using System;
using BasicUI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

// Token: 0x02000214 RID: 532
public class LobbyListItemPanel : Selectable, IPointerClickHandler, IEventSystemHandler, ISubmitHandler, ISelectHandler
{
	// Token: 0x06001029 RID: 4137 RVA: 0x00053DEF File Offset: 0x00051FEF
	public void OnPointerClick(PointerEventData eventData)
	{
		if (MultiplayerMenuManager.singleton != null)
		{
			MultiplayerMenuManager.singleton.SetSelectedLobbyItem(this);
			if (eventData.clickCount >= 2)
			{
				MultiplayerMenuManager.singleton.JoinGame();
			}
		}
	}

	// Token: 0x0600102A RID: 4138 RVA: 0x00053E1C File Offset: 0x0005201C
	public void OnSubmit(BaseEventData eventData)
	{
		if (MultiplayerMenuManager.singleton != null)
		{
			if (MultiplayerMenuManager.singleton.selectedLobby == this)
			{
				MultiplayerMenuManager.singleton.JoinGame();
				return;
			}
			MultiplayerMenuManager.singleton.SetSelectedLobbyItem(this);
		}
	}

	// Token: 0x0600102B RID: 4139 RVA: 0x00053E54 File Offset: 0x00052054
	public void SetLobbyItem(MultiplayerLobbyItem newLobbyItem)
	{
		this.rectTransform = base.gameObject.GetComponent<RectTransform>();
		this.lobbyItem = newLobbyItem;
		if (this.lobbyItem != null)
		{
			this.nameText.text = this.lobbyItem.name;
			this.gameTypeText.text = this.lobbyItem.gameType.GetDescription();
			this.pointsText.text = this.lobbyItem.points.ToString();
			this.statusText.text = this.lobbyItem.lobbyStatus.GetDescription();
			this.timeScaleText.text = this.lobbyItem.lobbyTimeScaleString;
			this.capacityText.text = string.Format("{0}/{1}", this.lobbyItem.currentPlayers, this.lobbyItem.maxPlayers);
			this.pingText.text = this.lobbyItem.ping.ToString();
			this.staminaToggle.SetIsOnWithoutNotify(newLobbyItem.stamina);
			this.dismembermentToggle.SetIsOnWithoutNotify(newLobbyItem.dismemberment);
		}
	}

	// Token: 0x0600102C RID: 4140 RVA: 0x00053F83 File Offset: 0x00052183
	public void SetSelected(bool selected = false)
	{
		if (selected)
		{
			UIHelpers.SetBackgroundColor(base.gameObject, UIHelpers.GetColorForButtonState(ButtonState.Selected, false));
			return;
		}
		UIHelpers.SetBackgroundColor(base.gameObject, UISettings.BasicSubPanelColor);
	}

	// Token: 0x0600102D RID: 4141 RVA: 0x00053FAC File Offset: 0x000521AC
	void ISelectHandler.OnSelect(BaseEventData eventData)
	{
		base.OnSelect(eventData);
		if (this.rectTransform == null || MultiplayerMenuManager.singleton == null || MultiplayerMenuManager.singleton.lobbyListScrollRect == null)
		{
			return;
		}
		UIHelpers.SnapScrollViewTo(this.rectTransform, MultiplayerMenuManager.singleton.lobbyListScrollRect);
	}

	// Token: 0x04000B8E RID: 2958
	public MultiplayerLobbyItem lobbyItem;

	// Token: 0x04000B8F RID: 2959
	public Text nameText;

	// Token: 0x04000B90 RID: 2960
	public Text gameTypeText;

	// Token: 0x04000B91 RID: 2961
	public Text pointsText;

	// Token: 0x04000B92 RID: 2962
	public Text statusText;

	// Token: 0x04000B93 RID: 2963
	public Text timeScaleText;

	// Token: 0x04000B94 RID: 2964
	public Text capacityText;

	// Token: 0x04000B95 RID: 2965
	public Text pingText;

	// Token: 0x04000B96 RID: 2966
	public Toggle staminaToggle;

	// Token: 0x04000B97 RID: 2967
	public Toggle dismembermentToggle;

	// Token: 0x04000B98 RID: 2968
	public RectTransform rectTransform;
}
