using System;
using Newtonsoft.Json;
using ProtoBuf;

namespace MoveClasses
{
	// Token: 0x020002AB RID: 683
	[ProtoContract]
	[Serializable]
	public class JointMove
	{
		// Token: 0x17000226 RID: 550
		// (get) Token: 0x060013F5 RID: 5109 RVA: 0x0006576C File Offset: 0x0006396C
		// (set) Token: 0x060013F6 RID: 5110 RVA: 0x00065774 File Offset: 0x00063974
		[ProtoMember(1)]
		[JsonProperty("j")]
		public JointType joint { get; set; }

		// Token: 0x17000227 RID: 551
		// (set) Token: 0x060013F7 RID: 5111 RVA: 0x0006577D File Offset: 0x0006397D
		[JsonProperty("joint")]
		private JointType jointAlt
		{
			set
			{
				this.joint = value;
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x060013F8 RID: 5112 RVA: 0x00065786 File Offset: 0x00063986
		// (set) Token: 0x060013F9 RID: 5113 RVA: 0x0006578E File Offset: 0x0006398E
		[ProtoMember(2)]
		[JsonProperty("hs")]
		public HandState? handState { get; set; }

		// Token: 0x17000229 RID: 553
		// (set) Token: 0x060013FA RID: 5114 RVA: 0x00065797 File Offset: 0x00063997
		[JsonProperty("handState")]
		private HandState? handStateAlt
		{
			set
			{
				this.handState = value;
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x060013FB RID: 5115 RVA: 0x000657A0 File Offset: 0x000639A0
		// (set) Token: 0x060013FC RID: 5116 RVA: 0x000657E1 File Offset: 0x000639E1
		[ProtoMember(3)]
		[JsonProperty("tr")]
		public NullableVector3 targetRotation
		{
			get
			{
				if (this._targetRotation == null)
				{
					this._targetRotation = new NullableVector3(null, null, null);
				}
				return this._targetRotation;
			}
			set
			{
				this._targetRotation = value;
			}
		}

		// Token: 0x1700022B RID: 555
		// (set) Token: 0x060013FD RID: 5117 RVA: 0x000657EA File Offset: 0x000639EA
		[JsonProperty("targetRotation")]
		private NullableVector3 targetRotationAlt
		{
			set
			{
				this.targetRotation = value;
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x060013FE RID: 5118 RVA: 0x000657F3 File Offset: 0x000639F3
		// (set) Token: 0x060013FF RID: 5119 RVA: 0x000657FB File Offset: 0x000639FB
		[ProtoMember(4)]
		[JsonProperty("et")]
		public double executionTime { get; set; }

		// Token: 0x1700022D RID: 557
		// (set) Token: 0x06001400 RID: 5120 RVA: 0x00065804 File Offset: 0x00063A04
		[JsonProperty("executionTime")]
		private double executionTimeAlt
		{
			set
			{
				this.executionTime = value;
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06001401 RID: 5121 RVA: 0x0006580D File Offset: 0x00063A0D
		// (set) Token: 0x06001402 RID: 5122 RVA: 0x00065815 File Offset: 0x00063A15
		[JsonIgnore]
		public bool notSaved { get; set; }

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06001403 RID: 5123 RVA: 0x0006581E File Offset: 0x00063A1E
		// (set) Token: 0x06001404 RID: 5124 RVA: 0x00065826 File Offset: 0x00063A26
		[JsonIgnore]
		public bool lastMoveForJoint { get; set; }

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06001405 RID: 5125 RVA: 0x0006582F File Offset: 0x00063A2F
		// (set) Token: 0x06001406 RID: 5126 RVA: 0x00065837 File Offset: 0x00063A37
		[JsonIgnore]
		public JointMove NextMove
		{
			get
			{
				return this._nextMove;
			}
			set
			{
				this._nextMove = value;
			}
		}

		// Token: 0x04000ECA RID: 3786
		private NullableVector3 _targetRotation;

		// Token: 0x04000ECE RID: 3790
		[JsonIgnore]
		[NonSerialized]
		private JointMove _nextMove;

		// Token: 0x04000ECF RID: 3791
		[JsonIgnore]
		[NonSerialized]
		public int layer;

		// Token: 0x04000ED0 RID: 3792
		[JsonIgnore]
		[NonSerialized]
		public bool temp;

		// Token: 0x04000ED1 RID: 3793
		[JsonIgnore]
		[NonSerialized]
		public bool tempGenerated;

		// Token: 0x04000ED2 RID: 3794
		[JsonIgnore]
		[NonSerialized]
		public bool inPreviewList;
	}
}
