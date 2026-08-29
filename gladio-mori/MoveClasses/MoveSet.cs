using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json;
using ProtoBuf;
using Utils;

namespace MoveClasses
{
	// Token: 0x020002A7 RID: 679
	[ProtoContract]
	[Serializable]
	public class MoveSet
	{
		// Token: 0x060013AB RID: 5035 RVA: 0x00065108 File Offset: 0x00063308
		public MoveSet()
		{
			this.CreateNewGuid();
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x0006513C File Offset: 0x0006333C
		public void CreateNewGuid()
		{
			this.guid = Guid.NewGuid().ToString();
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x00065162 File Offset: 0x00063362
		public void TurnIntoCopy()
		{
			this.name += LocalizationHelpers.LocalizedText("txt_append_to_copied_name", Array.Empty<object>());
			this.CreateNewGuid();
		}

		// Token: 0x060013AE RID: 5038 RVA: 0x0006518A File Offset: 0x0006338A
		public string GetFileNameFromName()
		{
			return this.name + ".json";
		}

		// Token: 0x060013AF RID: 5039 RVA: 0x0006519C File Offset: 0x0006339C
		public string GetJsonString()
		{
			return JsonConvert.SerializeObject(this);
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x060013B0 RID: 5040 RVA: 0x000651A4 File Offset: 0x000633A4
		// (set) Token: 0x060013B1 RID: 5041 RVA: 0x000651AC File Offset: 0x000633AC
		[ProtoMember(1)]
		public string guid { get; set; }

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x060013B2 RID: 5042 RVA: 0x000651B5 File Offset: 0x000633B5
		// (set) Token: 0x060013B3 RID: 5043 RVA: 0x000651BD File Offset: 0x000633BD
		[ProtoMember(2)]
		public string name { get; set; }

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x060013B4 RID: 5044 RVA: 0x000651C6 File Offset: 0x000633C6
		// (set) Token: 0x060013B5 RID: 5045 RVA: 0x000651CE File Offset: 0x000633CE
		public string creator { get; set; }

		// Token: 0x060013B6 RID: 5046 RVA: 0x000651D7 File Offset: 0x000633D7
		public string GetCreatorName()
		{
			if (!string.IsNullOrWhiteSpace(this.creator))
			{
				return this.creator;
			}
			return "unknown";
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x060013B7 RID: 5047 RVA: 0x000651F2 File Offset: 0x000633F2
		// (set) Token: 0x060013B8 RID: 5048 RVA: 0x0006520E File Offset: 0x0006340E
		[JsonIgnore]
		public string unlocalizedName
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(this._unlocalizedName))
				{
					return this._unlocalizedName;
				}
				return this.name;
			}
			set
			{
				this._unlocalizedName = value;
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x060013B9 RID: 5049 RVA: 0x00065217 File Offset: 0x00063417
		// (set) Token: 0x060013BA RID: 5050 RVA: 0x0006521F File Offset: 0x0006341F
		[ProtoMember(3)]
		[JsonProperty(Required = Required.Default)]
		public List<Stance> stanceList { get; set; } = new List<Stance>();

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x060013BB RID: 5051 RVA: 0x00065228 File Offset: 0x00063428
		// (set) Token: 0x060013BC RID: 5052 RVA: 0x00065230 File Offset: 0x00063430
		public List<EquippedEquipment> defaultEquipment { get; set; } = new List<EquippedEquipment>();

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x060013BD RID: 5053 RVA: 0x00065239 File Offset: 0x00063439
		// (set) Token: 0x060013BE RID: 5054 RVA: 0x00065241 File Offset: 0x00063441
		[JsonIgnore]
		[ProtoMember(4)]
		public string fileName { get; set; }

		// Token: 0x060013BF RID: 5055 RVA: 0x0006524A File Offset: 0x0006344A
		public bool Equals(MoveSet compareMoveSet)
		{
			if (!string.IsNullOrEmpty(compareMoveSet.fileName) || !string.IsNullOrEmpty(this.fileName))
			{
				return this.fileName == compareMoveSet.fileName;
			}
			return this.name == compareMoveSet.name;
		}

		// Token: 0x060013C0 RID: 5056 RVA: 0x00065289 File Offset: 0x00063489
		public void FilterNameForProfanity()
		{
			if (GeneralManager.singleton != null && !string.IsNullOrEmpty(this.name))
			{
				this.name = GeneralManager.singleton.FilterBadWords(this.name, true);
			}
		}

		// Token: 0x060013C1 RID: 5057 RVA: 0x000652BC File Offset: 0x000634BC
		public void FilterMoveSetForProfanity()
		{
			this.FilterNameForProfanity();
			if (this.stanceList != null)
			{
				foreach (Stance stance in this.stanceList)
				{
					stance.FilterNameForProfanity();
					if (stance.moveList != null)
					{
						foreach (Move move in stance.moveList)
						{
							move.FilterNameForProfanity();
						}
					}
				}
			}
		}

		// Token: 0x060013C2 RID: 5058 RVA: 0x00065364 File Offset: 0x00063564
		public Stance GetDefaultStance()
		{
			Stance stance = null;
			if (this.stanceList != null)
			{
				stance = (from x in this.stanceList
				where x.isDefault
				select x).FirstOrDefault<Stance>();
				if (stance == null)
				{
					stance = this.stanceList.FirstOrDefault<Stance>();
				}
			}
			return stance;
		}

		// Token: 0x060013C3 RID: 5059 RVA: 0x000653BC File Offset: 0x000635BC
		public MoveSet GetClone()
		{
			MoveSet moveSet = this.SerializeToString_PB<MoveSet>().DeserializeFromString_PB<MoveSet>();
			if (this.defaultEquipment != null)
			{
				moveSet.defaultEquipment = JsonConvert.DeserializeObject<List<EquippedEquipment>>(JsonConvert.SerializeObject(this.defaultEquipment));
			}
			moveSet.defaultMoveset = this.defaultMoveset;
			moveSet.communityMoveset = this.communityMoveset;
			moveSet.gameType = this.gameType;
			moveSet.stamina = this.stamina;
			return moveSet;
		}

		// Token: 0x04000EAB RID: 3755
		[DefaultValue(true)]
		public bool stamina = true;

		// Token: 0x04000EAC RID: 3756
		[DefaultValue(GameTypes.Creative)]
		public GameTypes gameType = GameTypes.Creative;

		// Token: 0x04000EAE RID: 3758
		[JsonIgnore]
		private string _unlocalizedName;

		// Token: 0x04000EAF RID: 3759
		[JsonIgnore]
		public bool defaultMoveset;

		// Token: 0x04000EB0 RID: 3760
		[JsonIgnore]
		public bool communityMoveset;

		// Token: 0x04000EB1 RID: 3761
		public bool notUsableByAI;

		// Token: 0x04000EB5 RID: 3765
		[JsonIgnore]
		public bool loaded;
	}
}
