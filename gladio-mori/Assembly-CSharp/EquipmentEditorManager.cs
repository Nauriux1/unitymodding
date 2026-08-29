using System;
using UnityEngine;

// Token: 0x02000076 RID: 118
public class EquipmentEditorManager : MonoBehaviour
{
	// Token: 0x06000336 RID: 822 RVA: 0x0001106C File Offset: 0x0000F26C
	private void Awake()
	{
		this.equipmentPanel.lobbyPlayer = new LobbyPlayer();
		this.equipmentPanel.playerHealth = this.playerHealth;
	}

	// Token: 0x06000337 RID: 823 RVA: 0x0000777A File Offset: 0x0000597A
	private void Update()
	{
	}

	// Token: 0x0400023D RID: 573
	public PlayerHealth playerHealth;

	// Token: 0x0400023E RID: 574
	public EquipmentPanel equipmentPanel;
}
