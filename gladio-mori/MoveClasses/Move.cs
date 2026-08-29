using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ProtoBuf;
using Utils;

namespace MoveClasses
{
	// Token: 0x020002AA RID: 682
	[ProtoContract]
	[Serializable]
	public class Move
	{
		// Token: 0x060013D5 RID: 5077 RVA: 0x0006550E File Offset: 0x0006370E
		public Move()
		{
			this.CreateNewGuid();
		}

		// Token: 0x060013D6 RID: 5078 RVA: 0x00065528 File Offset: 0x00063728
		public void CreateNewGuid()
		{
			this.guid = Guid.NewGuid().ToString();
		}

		// Token: 0x060013D7 RID: 5079 RVA: 0x0006519C File Offset: 0x0006339C
		public string GetJsonCopyString()
		{
			return JsonConvert.SerializeObject(this);
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x060013D8 RID: 5080 RVA: 0x0006554E File Offset: 0x0006374E
		// (set) Token: 0x060013D9 RID: 5081 RVA: 0x00065556 File Offset: 0x00063756
		[ProtoMember(1)]
		public string guid { get; set; }

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x060013DA RID: 5082 RVA: 0x0006555F File Offset: 0x0006375F
		// (set) Token: 0x060013DB RID: 5083 RVA: 0x00065567 File Offset: 0x00063767
		[ProtoMember(2)]
		public string name { get; set; }

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x060013DC RID: 5084 RVA: 0x00065570 File Offset: 0x00063770
		// (set) Token: 0x060013DD RID: 5085 RVA: 0x0006558C File Offset: 0x0006378C
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

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x060013DE RID: 5086 RVA: 0x00065595 File Offset: 0x00063795
		// (set) Token: 0x060013DF RID: 5087 RVA: 0x0006559D File Offset: 0x0006379D
		[ProtoMember(3)]
		public int layer { get; set; }

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x060013E0 RID: 5088 RVA: 0x000655A6 File Offset: 0x000637A6
		// (set) Token: 0x060013E1 RID: 5089 RVA: 0x000655AE File Offset: 0x000637AE
		[ProtoMember(4)]
		[JsonProperty(Required = Required.Default)]
		public List<JointMove> jointMoveList { get; set; } = new List<JointMove>();

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x060013E2 RID: 5090 RVA: 0x000655B7 File Offset: 0x000637B7
		// (set) Token: 0x060013E3 RID: 5091 RVA: 0x000655BF File Offset: 0x000637BF
		[ProtoMember(5)]
		public string playerInput { get; set; }

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x060013E4 RID: 5092 RVA: 0x000655C8 File Offset: 0x000637C8
		// (set) Token: 0x060013E5 RID: 5093 RVA: 0x000655D0 File Offset: 0x000637D0
		[ProtoMember(6)]
		public inputType inputType { get; set; }

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x060013E6 RID: 5094 RVA: 0x000655D9 File Offset: 0x000637D9
		// (set) Token: 0x060013E7 RID: 5095 RVA: 0x000655E1 File Offset: 0x000637E1
		[ProtoMember(7)]
		public float duration { get; set; }

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x060013E8 RID: 5096 RVA: 0x000655EA File Offset: 0x000637EA
		// (set) Token: 0x060013E9 RID: 5097 RVA: 0x000655F2 File Offset: 0x000637F2
		[ProtoMember(8)]
		public string stanceGuid { get; set; }

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x060013EA RID: 5098 RVA: 0x000655FB File Offset: 0x000637FB
		// (set) Token: 0x060013EB RID: 5099 RVA: 0x00065603 File Offset: 0x00063803
		[ProtoMember(9)]
		public bool stanceChange { get; set; }

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x060013EC RID: 5100 RVA: 0x0006560C File Offset: 0x0006380C
		// (set) Token: 0x060013ED RID: 5101 RVA: 0x00065614 File Offset: 0x00063814
		[ProtoMember(10)]
		public stanceChangeType stanceChangeType { get; set; }

		// Token: 0x060013EE RID: 5102 RVA: 0x0006561D File Offset: 0x0006381D
		public void SortSingleMoves()
		{
			if (this.jointMoveList != null)
			{
				Move.JointMovesQuickSort(this.jointMoveList);
			}
		}

		// Token: 0x060013EF RID: 5103 RVA: 0x00065634 File Offset: 0x00063834
		public static void exchangeRunningMoves(List<JointMove> data, int m, int n)
		{
			JointMove value = data[m];
			data[m] = data[n];
			data[n] = value;
		}

		// Token: 0x060013F0 RID: 5104 RVA: 0x00065660 File Offset: 0x00063860
		public static void JointMovesQuickSort(List<JointMove> data, int l, int r)
		{
			int num = l;
			int num2 = r;
			JointMove jointMove = data[(l + r) / 2];
			for (;;)
			{
				if (Move.CompareJointMoves(jointMove, data[num]) <= 0)
				{
					while (Move.CompareJointMoves(data[num2], jointMove) > 0)
					{
						num2--;
					}
					if (num <= num2)
					{
						Move.exchangeRunningMoves(data, num, num2);
						num++;
						num2--;
					}
					if (num > num2)
					{
						break;
					}
				}
				else
				{
					num++;
				}
			}
			if (l < num2)
			{
				Move.JointMovesQuickSort(data, l, num2);
			}
			if (num < r)
			{
				Move.JointMovesQuickSort(data, num, r);
			}
		}

		// Token: 0x060013F1 RID: 5105 RVA: 0x000656DC File Offset: 0x000638DC
		private static int CompareJointMoves(JointMove x, JointMove y)
		{
			int num = Generic.CompareDouble(x.executionTime, y.executionTime);
			if (num != 0)
			{
				return num;
			}
			return x.joint.CompareTo(y.joint);
		}

		// Token: 0x060013F2 RID: 5106 RVA: 0x0006571F File Offset: 0x0006391F
		public static void JointMovesQuickSort(List<JointMove> data)
		{
			if (data.Count > 0)
			{
				Move.JointMovesQuickSort(data, 0, data.Count - 1);
			}
		}

		// Token: 0x060013F3 RID: 5107 RVA: 0x00065739 File Offset: 0x00063939
		public void FilterNameForProfanity()
		{
			if (GeneralManager.singleton != null && !string.IsNullOrEmpty(this.name))
			{
				this.name = GeneralManager.singleton.FilterBadWords(this.name, true);
			}
		}

		// Token: 0x04000EBF RID: 3775
		[JsonIgnore]
		private string _unlocalizedName;
	}
}
