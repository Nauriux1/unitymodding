using System;
using System.Linq;
using Newtonsoft.Json;
using ProtoBuf;
using Utils;

namespace MoveClasses
{
	// Token: 0x020002AF RID: 687
	[ProtoContract]
	[Serializable]
	public class EquippedEquipment
	{
		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06001422 RID: 5154 RVA: 0x00065B62 File Offset: 0x00063D62
		// (set) Token: 0x06001423 RID: 5155 RVA: 0x00065B6A File Offset: 0x00063D6A
		[JsonIgnore]
		public EquipmentPosition position
		{
			get
			{
				return (EquipmentPosition)this.positionInt;
			}
			set
			{
				this.positionInt = (int)value;
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06001424 RID: 5156 RVA: 0x00065B74 File Offset: 0x00063D74
		// (set) Token: 0x06001425 RID: 5157 RVA: 0x00065BC8 File Offset: 0x00063DC8
		[JsonIgnore]
		public EquipmentTypeItem equipment
		{
			get
			{
				if (this.equipmentTypeItem != null)
				{
					return this.equipmentTypeItem;
				}
				if (this.equipmentTypeInt == -1)
				{
					return null;
				}
				EquipmentType type = (EquipmentType)this.equipmentTypeInt;
				return (from x in EquipmentTypeItem.GetEquipmentTypeItems()
				where x.equipmentType == type
				select x).FirstOrDefault<EquipmentTypeItem>();
			}
			set
			{
				this.equipmentTypeItem = value;
				if (value != null)
				{
					this.equipmentTypeInt = (int)value.equipmentType;
					return;
				}
				this.equipmentTypeInt = -1;
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06001426 RID: 5158 RVA: 0x00065BE8 File Offset: 0x00063DE8
		// (set) Token: 0x06001427 RID: 5159 RVA: 0x00065BF0 File Offset: 0x00063DF0
		[JsonIgnore]
		public EquipmentStartHandleRotation equipmentStartHoldType
		{
			get
			{
				return (EquipmentStartHandleRotation)this.equipmentStartHoldTypeInt;
			}
			set
			{
				this.equipmentStartHoldTypeInt = (int)value;
			}
		}

		// Token: 0x06001428 RID: 5160 RVA: 0x00065BFC File Offset: 0x00063DFC
		public bool Equals(EquippedEquipment compareEquippedEquipment)
		{
			return compareEquippedEquipment.equipmentTypeInt == this.equipmentTypeInt && compareEquippedEquipment.positionInt == this.positionInt && Generic.FloatEquals(compareEquippedEquipment.equipmentStartHoldPosition, this.equipmentStartHoldPosition) && compareEquippedEquipment.equipmentStartHoldTypeInt == this.equipmentStartHoldTypeInt;
		}

		// Token: 0x04000EDD RID: 3805
		[ProtoMember(1)]
		public int positionInt;

		// Token: 0x04000EDE RID: 3806
		[ProtoMember(2)]
		public int equipmentTypeInt;

		// Token: 0x04000EDF RID: 3807
		[ProtoMember(3)]
		public int equipmentStartHoldTypeInt;

		// Token: 0x04000EE0 RID: 3808
		[ProtoMember(4)]
		public float equipmentStartHoldPosition;

		// Token: 0x04000EE1 RID: 3809
		[JsonIgnore]
		[NonSerialized]
		private EquipmentTypeItem equipmentTypeItem;
	}
}
