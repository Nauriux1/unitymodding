using System;
using System.Collections.Generic;
using BasicUI;
using MoveClasses;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Utils;

// Token: 0x02000059 RID: 89
public class LobbyPlayer : IRoomPlayer
{
	// Token: 0x1700009D RID: 157
	// (get) Token: 0x06000274 RID: 628 RVA: 0x0000C5B5 File Offset: 0x0000A7B5
	// (set) Token: 0x06000275 RID: 629 RVA: 0x0000C5BD File Offset: 0x0000A7BD
	public MoveSet selectedMoveSet { get; set; }

	// Token: 0x1700009E RID: 158
	// (get) Token: 0x06000276 RID: 630 RVA: 0x0000C5C6 File Offset: 0x0000A7C6
	// (set) Token: 0x06000277 RID: 631 RVA: 0x0000C5CE File Offset: 0x0000A7CE
	public List<EquippedEquipment> selectedEquipment { get; set; } = new List<EquippedEquipment>();

	// Token: 0x1700009F RID: 159
	// (get) Token: 0x06000278 RID: 632 RVA: 0x0000C5D7 File Offset: 0x0000A7D7
	// (set) Token: 0x06000279 RID: 633 RVA: 0x0000C5DF File Offset: 0x0000A7DF
	public bool readyToBegin { get; set; }

	// Token: 0x170000A0 RID: 160
	// (get) Token: 0x0600027A RID: 634 RVA: 0x0000C5E8 File Offset: 0x0000A7E8
	// (set) Token: 0x0600027B RID: 635 RVA: 0x0000C5F0 File Offset: 0x0000A7F0
	public bool playerExists { get; set; }

	// Token: 0x170000A1 RID: 161
	// (get) Token: 0x0600027C RID: 636 RVA: 0x0000C5F9 File Offset: 0x0000A7F9
	// (set) Token: 0x0600027D RID: 637 RVA: 0x0000C601 File Offset: 0x0000A801
	public bool ai { get; set; }

	// Token: 0x170000A2 RID: 162
	// (get) Token: 0x0600027E RID: 638 RVA: 0x0000C60A File Offset: 0x0000A80A
	// (set) Token: 0x0600027F RID: 639 RVA: 0x0000C612 File Offset: 0x0000A812
	public string playerName { get; set; }

	// Token: 0x170000A3 RID: 163
	// (get) Token: 0x06000280 RID: 640 RVA: 0x0000C61B File Offset: 0x0000A81B
	// (set) Token: 0x06000281 RID: 641 RVA: 0x0000C623 File Offset: 0x0000A823
	public InputDevice device { get; set; }

	// Token: 0x170000A4 RID: 164
	// (get) Token: 0x06000282 RID: 642 RVA: 0x0000C62C File Offset: 0x0000A82C
	// (set) Token: 0x06000283 RID: 643 RVA: 0x0000C634 File Offset: 0x0000A834
	public Camera camera { get; set; }

	// Token: 0x170000A5 RID: 165
	// (get) Token: 0x06000284 RID: 644 RVA: 0x0000C63D File Offset: 0x0000A83D
	public bool playerReadyState
	{
		get
		{
			return this.readyToBegin;
		}
	}

	// Token: 0x170000A6 RID: 166
	// (get) Token: 0x06000285 RID: 645 RVA: 0x0000C645 File Offset: 0x0000A845
	// (set) Token: 0x06000286 RID: 646 RVA: 0x0000C64D File Offset: 0x0000A84D
	public PlayerHealth playerHealth { get; set; }

	// Token: 0x170000A7 RID: 167
	// (get) Token: 0x06000287 RID: 647 RVA: 0x0000C656 File Offset: 0x0000A856
	// (set) Token: 0x06000288 RID: 648 RVA: 0x0000C65E File Offset: 0x0000A85E
	public PlayerCanvasController playerCanvasContoller { get; set; }

	// Token: 0x06000289 RID: 649 RVA: 0x0000C667 File Offset: 0x0000A867
	public Camera GetCamera()
	{
		return this.camera;
	}

	// Token: 0x0600028A RID: 650 RVA: 0x0000C66F File Offset: 0x0000A86F
	public void GoBack()
	{
		SceneManagerWithParameters.LoadScene("MainMenu", null, false, false);
	}

	// Token: 0x0600028B RID: 651 RVA: 0x0000C67E File Offset: 0x0000A87E
	public void SetEquipment(List<EquippedEquipment> newEquipment)
	{
		this.selectedEquipment = newEquipment;
	}

	// Token: 0x0600028C RID: 652 RVA: 0x0000C688 File Offset: 0x0000A888
	public void SetMoveSet(MoveSet newMoveSet)
	{
		if (SceneManager.GetActiveScene().name != "LobbyMoveEditor" && IGameSettingsManager.singleton != null && IGameSettingsManager.singleton.GameType == GameTypes.Classic && newMoveSet != null && (!newMoveSet.defaultMoveset || newMoveSet.communityMoveset))
		{
			newMoveSet = MoveSetHelpers.ConvertMoveSetToClassic(newMoveSet);
		}
		this.selectedMoveSet = newMoveSet;
		this.selectedEquipment = MoveClassHelpers.CloneEquipmentList(newMoveSet.defaultEquipment);
		if (this.playerHealth != null)
		{
			this.playerHealth.playerAnimator.SetMoveSet(this.selectedMoveSet, true, false);
			this.playerHealth.SetEquipment(this.selectedEquipment, false);
		}
	}

	// Token: 0x0600028D RID: 653 RVA: 0x0000C72C File Offset: 0x0000A92C
	public MoveSet GetMoveSet()
	{
		return this.selectedMoveSet;
	}

	// Token: 0x0600028E RID: 654 RVA: 0x0000C734 File Offset: 0x0000A934
	public void SetReady()
	{
		if (this.readyToBegin)
		{
			this.readyToBegin = false;
			return;
		}
		if (!GameSettingsHelper.CheckCanPlayerReadyByEquipmentPoints(this.selectedEquipment))
		{
			if (this.playerCanvasContoller != null)
			{
				int equipmentPoints = IGameSettingsManager.singleton.EquipmentPoints;
				string text = string.Format("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGBA(UISettings.BasicButtonNotReadyColor), GameSettingsHelper.CountEquippedEquipmentPoints(this.selectedEquipment));
				this.playerCanvasContoller.DisplayInfoMessage(LocalizationHelpers.LocalizedText("txt_max_equipment_points_alert", new object[]
				{
					equipmentPoints,
					text
				}));
			}
			return;
		}
		this.readyToBegin = true;
	}

	// Token: 0x0600028F RID: 655 RVA: 0x0000777A File Offset: 0x0000597A
	public void SetSpectator(bool value)
	{
	}

	// Token: 0x06000290 RID: 656 RVA: 0x0000C7CC File Offset: 0x0000A9CC
	public bool GetSpectator()
	{
		return false;
	}

	// Token: 0x06000291 RID: 657 RVA: 0x0000C7CF File Offset: 0x0000A9CF
	public List<EquippedEquipment> GetSelectedEquipment()
	{
		return this.selectedEquipment;
	}

	// Token: 0x06000292 RID: 658 RVA: 0x0000C7D7 File Offset: 0x0000A9D7
	public bool ApplyTempPlayerValues()
	{
		return true;
	}

	// Token: 0x06000293 RID: 659 RVA: 0x0000777A File Offset: 0x0000597A
	public void UpdatePreviewVisuals()
	{
	}

	// Token: 0x06000294 RID: 660 RVA: 0x0000777A File Offset: 0x0000597A
	public void SetEquipmentStartingHold(EquippedEquipment equippedEquipment)
	{
	}

	// Token: 0x04000180 RID: 384
	public CustomAiObject customAiObject;
}
