using System;
using System.Collections.Generic;
using MoveClasses;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// Token: 0x0200018E RID: 398
public class TestModPlayer : MonoBehaviour
{
	// Token: 0x06000C74 RID: 3188 RVA: 0x0003C9D7 File Offset: 0x0003ABD7
	private void Start()
	{
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 120;
		this.PopulateWithFullArmor();
	}

	// Token: 0x06000C75 RID: 3189 RVA: 0x0003C9EC File Offset: 0x0003ABEC
	private void PopulateWithFullArmor()
	{
		foreach (EquipmentTypeItem equipmentTypeItem in EquipmentTypeItem.GetEquipmentTypeItems())
		{
			if (!equipmentTypeItem.equipmentPositions.Contains(EquipmentPosition.HandLeft) && !equipmentTypeItem.equipmentPositions.Contains(EquipmentPosition.Helmet))
			{
				foreach (EquipmentPosition positionInt in equipmentTypeItem.equipmentPositions)
				{
					this.equippedEquipment.Add(new EquippedEquipment
					{
						equipmentTypeInt = (int)equipmentTypeItem.equipmentType,
						positionInt = (int)positionInt
					});
				}
			}
		}
	}

	// Token: 0x06000C76 RID: 3190 RVA: 0x0003CAB4 File Offset: 0x0003ACB4
	private void SetEquipment()
	{
		if (this.playerHealth != null && this.equippedEquipment.Count > 0)
		{
			this.playerHealth.SetEquipment(this.equippedEquipment, false);
		}
	}

	// Token: 0x06000C77 RID: 3191 RVA: 0x0003CAE4 File Offset: 0x0003ACE4
	private void SetTexture()
	{
		if (this.playerHealth != null && this.testTexture != null)
		{
			this.playerHealth.SetPlayerTexture(this.testTexture);
		}
	}

	// Token: 0x06000C78 RID: 3192 RVA: 0x0003CB14 File Offset: 0x0003AD14
	private void Update()
	{
		if (Keyboard.current.vKey.wasPressedThisFrame)
		{
			Time.timeScale = 0.1f;
		}
		if (Keyboard.current.pKey.wasPressedThisFrame)
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}

	// Token: 0x040008DC RID: 2268
	public PlayerHealth playerHealth;

	// Token: 0x040008DD RID: 2269
	public List<EquippedEquipment> equippedEquipment;

	// Token: 0x040008DE RID: 2270
	public Texture2D testTexture;
}
