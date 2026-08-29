using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ProtoBuf;

namespace MoveClasses
{
	// Token: 0x020002A9 RID: 681
	[ProtoContract]
	[Serializable]
	public class Stance
	{
		// Token: 0x060013C7 RID: 5063 RVA: 0x00065430 File Offset: 0x00063630
		public Stance()
		{
			this.CreateNewGuid();
		}

		// Token: 0x060013C8 RID: 5064 RVA: 0x0006544C File Offset: 0x0006364C
		public void CreateNewGuid()
		{
			this.guid = Guid.NewGuid().ToString();
		}

		// Token: 0x060013C9 RID: 5065 RVA: 0x0006519C File Offset: 0x0006339C
		public string GetJsonCopyString()
		{
			return JsonConvert.SerializeObject(this);
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x060013CA RID: 5066 RVA: 0x00065472 File Offset: 0x00063672
		// (set) Token: 0x060013CB RID: 5067 RVA: 0x0006547A File Offset: 0x0006367A
		[ProtoMember(1)]
		public string guid { get; set; }

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x060013CC RID: 5068 RVA: 0x00065483 File Offset: 0x00063683
		// (set) Token: 0x060013CD RID: 5069 RVA: 0x0006548B File Offset: 0x0006368B
		[ProtoMember(2)]
		public string name { get; set; }

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x060013CE RID: 5070 RVA: 0x00065494 File Offset: 0x00063694
		// (set) Token: 0x060013CF RID: 5071 RVA: 0x000654B0 File Offset: 0x000636B0
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

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x060013D0 RID: 5072 RVA: 0x000654B9 File Offset: 0x000636B9
		// (set) Token: 0x060013D1 RID: 5073 RVA: 0x000654C1 File Offset: 0x000636C1
		[ProtoMember(3)]
		public bool isDefault { get; set; }

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x060013D2 RID: 5074 RVA: 0x000654CA File Offset: 0x000636CA
		// (set) Token: 0x060013D3 RID: 5075 RVA: 0x000654D2 File Offset: 0x000636D2
		[ProtoMember(4)]
		[JsonProperty(Required = Required.Default)]
		public List<Move> moveList { get; set; } = new List<Move>();

		// Token: 0x060013D4 RID: 5076 RVA: 0x000654DB File Offset: 0x000636DB
		public void FilterNameForProfanity()
		{
			if (GeneralManager.singleton != null && !string.IsNullOrEmpty(this.name))
			{
				this.name = GeneralManager.singleton.FilterBadWords(this.name, true);
			}
		}

		// Token: 0x04000EBA RID: 3770
		[JsonIgnore]
		private string _unlocalizedName;
	}
}
