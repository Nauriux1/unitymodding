using System;
using System.Collections.Generic;
using Mirror;

namespace MoveClasses
{
	// Token: 0x020002B5 RID: 693
	[Serializable]
	public class EquipmentTypeItem
	{
		// Token: 0x06001439 RID: 5177 RVA: 0x00065CC0 File Offset: 0x00063EC0
		public bool IsDisabled()
		{
			if (NetworkClient.active && GameSettingsManagerMultiplayer.singleton != null && GameSettingsManagerMultiplayer.singleton.DisabledEquipmentTypes != null)
			{
				for (int i = 0; i < GameSettingsManagerMultiplayer.singleton.DisabledEquipmentTypes.Count; i++)
				{
					if (GameSettingsManagerMultiplayer.singleton.DisabledEquipmentTypes[i] == this.equipmentType)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x0600143A RID: 5178 RVA: 0x00065D22 File Offset: 0x00063F22
		// (set) Token: 0x0600143B RID: 5179 RVA: 0x00065D2A File Offset: 0x00063F2A
		public EquipmentType equipmentType { get; set; }

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x0600143C RID: 5180 RVA: 0x00065D33 File Offset: 0x00063F33
		// (set) Token: 0x0600143D RID: 5181 RVA: 0x00065D3B File Offset: 0x00063F3B
		public List<EquipmentPosition> equipmentPositions { get; set; }

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x0600143E RID: 5182 RVA: 0x00065D44 File Offset: 0x00063F44
		// (set) Token: 0x0600143F RID: 5183 RVA: 0x00065D4C File Offset: 0x00063F4C
		public int equipmentPoints { get; set; } = 5;

		// Token: 0x06001440 RID: 5184 RVA: 0x00065D58 File Offset: 0x00063F58
		public static List<EquipmentTypeItem> GetEquipmentTypeItems()
		{
			return new List<EquipmentTypeItem>
			{
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.Dagger,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.HandLeft,
						EquipmentPosition.HandRight
					},
					equipmentPoints = 3
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.Katar,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.HandLeft,
						EquipmentPosition.HandRight
					},
					equipmentPoints = 4
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.Sica,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.HandLeft,
						EquipmentPosition.HandRight
					},
					equipmentPoints = 4
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.Gladius,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.HandLeft,
						EquipmentPosition.HandRight
					},
					equipmentPoints = 6
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.ShortSword,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.HandLeft,
						EquipmentPosition.HandRight
					},
					equipmentPoints = 9
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.Kilij,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.HandLeft,
						EquipmentPosition.HandRight
					},
					equipmentPoints = 9
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.HookSword,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.HandLeft,
						EquipmentPosition.HandRight
					},
					equipmentPoints = 10
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.Rapier,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.HandLeft,
						EquipmentPosition.HandRight
					},
					equipmentPoints = 14
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.Katana,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.HandLeft,
						EquipmentPosition.HandRight
					},
					equipmentPoints = 13
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.Longsword,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.HandLeft,
						EquipmentPosition.HandRight
					},
					equipmentPoints = 15
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.Falx,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.HandLeft,
						EquipmentPosition.HandRight
					},
					equipmentPoints = 13
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.Claymore,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.HandLeft,
						EquipmentPosition.HandRight
					},
					equipmentPoints = 22
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.Zweihander,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.HandLeft,
						EquipmentPosition.HandRight
					},
					equipmentPoints = 29
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.BeardedAxe,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.HandLeft,
						EquipmentPosition.HandRight
					},
					equipmentPoints = 4
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.Podao,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.HandLeft,
						EquipmentPosition.HandRight
					},
					equipmentPoints = 13
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.Bardiche,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.HandLeft,
						EquipmentPosition.HandRight
					},
					equipmentPoints = 13
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.DaneAxe,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.HandLeft,
						EquipmentPosition.HandRight
					},
					equipmentPoints = 13
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.Twinblade,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.HandLeft,
						EquipmentPosition.HandRight
					},
					equipmentPoints = 12
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.ShortSpear,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.HandLeft,
						EquipmentPosition.HandRight
					},
					equipmentPoints = 13
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.Naginata,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.HandLeft,
						EquipmentPosition.HandRight
					},
					equipmentPoints = 19
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.Spear,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.HandLeft,
						EquipmentPosition.HandRight
					},
					equipmentPoints = 24
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.Halberd,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.HandLeft,
						EquipmentPosition.HandRight
					},
					equipmentPoints = 30
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.Mace,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.HandLeft,
						EquipmentPosition.HandRight
					},
					equipmentPoints = 7
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.Kanabo,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.HandLeft,
						EquipmentPosition.HandRight
					},
					equipmentPoints = 15
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.JoStaff,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.HandLeft,
						EquipmentPosition.HandRight
					},
					equipmentPoints = 8
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.BoStaff,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.HandLeft,
						EquipmentPosition.HandRight
					},
					equipmentPoints = 12
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.GouRang,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.HandLeft,
						EquipmentPosition.HandRight
					},
					equipmentPoints = 7
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.Parma,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.HandLeft,
						EquipmentPosition.HandRight
					},
					equipmentPoints = 8
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.Shield,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.HandLeft,
						EquipmentPosition.HandRight
					},
					equipmentPoints = 15
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.Scutum,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.HandLeft,
						EquipmentPosition.HandRight
					},
					equipmentPoints = 19
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.NasalSpangenhelm,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.Helmet
					},
					equipmentPoints = 8
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.KettleHelmet,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.Helmet
					},
					equipmentPoints = 10
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.GreatHelmet,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.Helmet
					},
					equipmentPoints = 13
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.Cuisses,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.ThighLeft,
						EquipmentPosition.ThighRight
					},
					equipmentPoints = 4
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.Greaves,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.LegLeft,
						EquipmentPosition.LegRight
					},
					equipmentPoints = 3
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.Vambrace,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.ArmLeft,
						EquipmentPosition.ArmRight
					},
					equipmentPoints = 3
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.Rerebrace,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.BicepLeft,
						EquipmentPosition.BicepRight
					},
					equipmentPoints = 4
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.Cuirass,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.Chest
					},
					equipmentPoints = 7
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.Plackart,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.Stomach
					},
					equipmentPoints = 6
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.Faulds,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.Hip
					},
					equipmentPoints = 4
				},
				new EquipmentTypeItem
				{
					equipmentType = EquipmentType.SteelSlab,
					equipmentPositions = new List<EquipmentPosition>
					{
						EquipmentPosition.HandLeft,
						EquipmentPosition.HandRight
					},
					equipmentPoints = 100
				}
			};
		}
	}
}
