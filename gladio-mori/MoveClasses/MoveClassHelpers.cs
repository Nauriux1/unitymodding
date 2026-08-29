using System;
using System.Collections.Generic;

namespace MoveClasses
{
	// Token: 0x0200029A RID: 666
	public static class MoveClassHelpers
	{
		// Token: 0x06001373 RID: 4979 RVA: 0x00064E0C File Offset: 0x0006300C
		public static string GetAxesForJointType(JointType type)
		{
			string result = "xyz";
			if (type <= JointType.ELBOW_LEFT)
			{
				if (type != JointType.ELBOW_RIGHT)
				{
					if (type == JointType.ELBOW_LEFT)
					{
						result = "y";
					}
				}
				else
				{
					result = "y";
				}
			}
			else if (type != JointType.KNEE_RIGHT)
			{
				if (type == JointType.KNEE_LEFT)
				{
					result = "x";
				}
			}
			else
			{
				result = "x";
			}
			return result;
		}

		// Token: 0x06001374 RID: 4980 RVA: 0x00064E58 File Offset: 0x00063058
		public static List<EquippedEquipment> CloneEquipmentList(List<EquippedEquipment> equippedEquipmentList)
		{
			List<EquippedEquipment> list = new List<EquippedEquipment>();
			if (equippedEquipmentList != null)
			{
				for (int i = 0; i < equippedEquipmentList.Count; i++)
				{
					EquippedEquipment equippedEquipment = equippedEquipmentList[i];
					EquippedEquipment item = new EquippedEquipment
					{
						positionInt = equippedEquipment.positionInt,
						equipmentStartHoldPosition = equippedEquipment.equipmentStartHoldPosition,
						equipmentStartHoldTypeInt = equippedEquipment.equipmentStartHoldTypeInt,
						equipmentTypeInt = equippedEquipment.equipmentTypeInt
					};
					list.Add(item);
				}
			}
			return list;
		}
	}
}
