using System;
using System.ComponentModel;
using UnityEngine;

// Token: 0x02000244 RID: 580
public static class EnumExtensions
{
	// Token: 0x060010E8 RID: 4328 RVA: 0x00057534 File Offset: 0x00055734
	public static string GetDescription(this Enum enumValue)
	{
		try
		{
			DescriptionAttribute[] array = (DescriptionAttribute[])enumValue.GetType().GetField(enumValue.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
			if (array != null && array.Length != 0)
			{
				return array[0].Description;
			}
			return enumValue.ToString();
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
		return "[[" + enumValue.ToString() + "]]";
	}
}
