using System;
using System.Collections.Generic;
using MoveClasses;
using UnityEngine;

namespace Utils
{
	// Token: 0x02000275 RID: 629
	internal class GameSettingsHelper
	{
		// Token: 0x0600123D RID: 4669 RVA: 0x0005F1C8 File Offset: 0x0005D3C8
		public static bool CheckCanPlayerReady(MoveSet moveset, List<EquippedEquipment> equippedEquipments)
		{
			return GameSettingsHelper.CheckPlayerUsesAllowedMoveset(moveset) && GameSettingsHelper.CheckCanPlayerReadyByEquipmentPoints(equippedEquipments) && GameSettingsHelper.CheckPlayerUsesDefaultEquipmentOrEditingAllowed(moveset, equippedEquipments) && GameSettingsHelper.CheckPlayerUsesAllowedEquipment(equippedEquipments);
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x0005F1F4 File Offset: 0x0005D3F4
		public static bool CheckPlayerUsesAllowedMoveset(MoveSet moveset)
		{
			if (moveset != null && IGameSettingsManager.singleton != null)
			{
				if (IGameSettingsManager.singleton.AllowedMovesetTypes == AllowedMovesetTypes.Basic)
				{
					if (!moveset.defaultMoveset && !moveset.communityMoveset)
					{
						Debug.Log("Player is not using basic moveset");
						return false;
					}
				}
				else if (IGameSettingsManager.singleton.AllowedMovesetTypes == AllowedMovesetTypes.Community && !moveset.defaultMoveset)
				{
					Debug.Log("Player is not using basic or community moveset");
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600123F RID: 4671 RVA: 0x0005F256 File Offset: 0x0005D456
		public static bool CheckPlayerUsesDefaultEquipmentOrEditingAllowed(MoveSet moveset, List<EquippedEquipment> equippedEquipments)
		{
			return moveset == null || IGameSettingsManager.singleton == null || IGameSettingsManager.singleton.AllowEquipmentEdit || GameSettingsHelper.CheckPlayerUsesDefaultEquipment(moveset, equippedEquipments);
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x0005F278 File Offset: 0x0005D478
		public static bool CheckPlayerUsesDefaultEquipment(MoveSet moveset, List<EquippedEquipment> equippedEquipments)
		{
			if (moveset != null)
			{
				if (equippedEquipments.Count != moveset.defaultEquipment.Count)
				{
					Debug.Log("Player is not using default equipment");
					return false;
				}
				for (int i = 0; i < equippedEquipments.Count; i++)
				{
					bool flag = false;
					EquippedEquipment equippedEquipment = equippedEquipments[i];
					for (int j = 0; j < moveset.defaultEquipment.Count; j++)
					{
						if (equippedEquipment.Equals(moveset.defaultEquipment[j]))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						Debug.Log("Player is not using default equipment");
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x0005F304 File Offset: 0x0005D504
		public static bool CheckPlayerUsesAllowedEquipment(List<EquippedEquipment> equippedEquipments)
		{
			if (GameSettingsManagerMultiplayer.singleton != null && GameSettingsManagerMultiplayer.singleton.DisabledEquipmentTypes.Count > 0)
			{
				for (int i = 0; i < equippedEquipments.Count; i++)
				{
					EquippedEquipment equippedEquipment = equippedEquipments[i];
					for (int j = 0; j < GameSettingsManagerMultiplayer.singleton.DisabledEquipmentTypes.Count; j++)
					{
						if (GameSettingsManagerMultiplayer.singleton.DisabledEquipmentTypes[j] == equippedEquipment.equipment.equipmentType)
						{
							Debug.Log("Player is using equipment that is disabled");
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x0005F390 File Offset: 0x0005D590
		public static bool CheckCanPlayerReadyByEquipmentPoints(List<EquippedEquipment> equippedEquipments)
		{
			if (IGameSettingsManager.singleton != null)
			{
				int equipmentPoints = IGameSettingsManager.singleton.EquipmentPoints;
				if (equipmentPoints > 0)
				{
					int num = GameSettingsHelper.CountEquippedEquipmentPoints(equippedEquipments);
					if (equipmentPoints < num)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06001243 RID: 4675 RVA: 0x0005F3C4 File Offset: 0x0005D5C4
		public static int CountEquippedEquipmentPoints(List<EquippedEquipment> equippedEquipments)
		{
			int num = 0;
			if (equippedEquipments != null)
			{
				foreach (EquippedEquipment equippedEquipment in equippedEquipments)
				{
					num += equippedEquipment.equipment.equipmentPoints;
				}
			}
			return num;
		}

		// Token: 0x06001244 RID: 4676 RVA: 0x0005F420 File Offset: 0x0005D620
		public static string GetTextForTimeScaleValue(float timeScale)
		{
			string result = "1.00x";
			if (timeScale > 0f)
			{
				if (Generic.FloatEquals(timeScale, 0.25f))
				{
					result = "0.25x";
				}
				else if (Generic.FloatEquals(timeScale, 0.5f))
				{
					result = "0.50x";
				}
				else if (Generic.FloatEquals(timeScale, 0.75f))
				{
					result = "0.75x";
				}
			}
			return result;
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x0005F47C File Offset: 0x0005D67C
		public static List<EquippedEquipment> FilterDisabledEquipmentFromList(List<EquippedEquipment> equippedEquipments)
		{
			if (GameSettingsManagerMultiplayer.singleton != null && GameSettingsManagerMultiplayer.singleton.DisabledEquipmentTypes.Count > 0)
			{
				for (int i = equippedEquipments.Count - 1; i > -1; i--)
				{
					EquippedEquipment equippedEquipment = equippedEquipments[i];
					for (int j = 0; j < GameSettingsManagerMultiplayer.singleton.DisabledEquipmentTypes.Count; j++)
					{
						if (GameSettingsManagerMultiplayer.singleton.DisabledEquipmentTypes[j] == equippedEquipment.equipment.equipmentType)
						{
							equippedEquipments.RemoveAt(i);
							break;
						}
					}
				}
			}
			return equippedEquipments;
		}
	}
}
