using System;
using UnityEngine.UI;

namespace UnityUIExtensionMethods
{
	// Token: 0x0200026A RID: 618
	public static class MyExtensions
	{
		// Token: 0x060011E7 RID: 4583 RVA: 0x0005BAB5 File Offset: 0x00059CB5
		public static string GetStringValue(this Dropdown.OptionData optionData)
		{
			if (optionData.GetType() == typeof(OptionDataWithValue))
			{
				return ((OptionDataWithValue)optionData).stringValue;
			}
			return "";
		}

		// Token: 0x060011E8 RID: 4584 RVA: 0x0005BADF File Offset: 0x00059CDF
		public static float GetFloatValue(this Dropdown.OptionData optionData)
		{
			if (optionData.GetType() == typeof(OptionDataWithValue))
			{
				return ((OptionDataWithValue)optionData).floatValue;
			}
			return 0f;
		}
	}
}
