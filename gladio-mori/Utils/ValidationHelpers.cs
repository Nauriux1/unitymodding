using System;
using UnityEngine;

namespace Utils
{
	// Token: 0x02000299 RID: 665
	public class ValidationHelpers
	{
		// Token: 0x0600136D RID: 4973 RVA: 0x00064D29 File Offset: 0x00062F29
		public static string ValidatePlayerNameLength(string name)
		{
			return ValidationHelpers.ValidateStringLength(name, 64);
		}

		// Token: 0x0600136E RID: 4974 RVA: 0x00064D33 File Offset: 0x00062F33
		public static string ValidateStringLength(string text, int maxLength)
		{
			if (string.IsNullOrEmpty(text))
			{
				return "";
			}
			if (text.Length <= maxLength)
			{
				return text;
			}
			return text.Substring(0, maxLength);
		}

		// Token: 0x0600136F RID: 4975 RVA: 0x00064D56 File Offset: 0x00062F56
		public static float ValidateFloatInput(float value)
		{
			return Mathf.Clamp(value, -1f, 1f);
		}

		// Token: 0x06001370 RID: 4976 RVA: 0x00064D68 File Offset: 0x00062F68
		public static bool ValidateTexture(Texture2D texture, byte[] textureBytes)
		{
			return ((textureBytes == null || textureBytes.Length == 0) && texture == null) || (texture != null && textureBytes != null && textureBytes.Length != 0 && textureBytes.Length < SettingsHelper.customPlayerTextureMaxBytes && texture.width <= SettingsHelper.customPlayerTextureMaxWidthHeight && texture.height <= SettingsHelper.customPlayerTextureMaxWidthHeight);
		}

		// Token: 0x06001371 RID: 4977 RVA: 0x00064DBC File Offset: 0x00062FBC
		public static bool ValidateEquipmentStartHoldPosition(float startHoldPos, Handle handle)
		{
			float num = 0.02f;
			float num2 = handle.StartHoldPositionLimit(false) + num;
			float num3 = handle.StartHoldPositionLimit(true) - num;
			if ((Generic.FloatEquals(startHoldPos, num3) || num3 < startHoldPos) && (Generic.FloatEquals(startHoldPos, num2) || num2 > startHoldPos))
			{
				return true;
			}
			Debug.Log("Weapon start hold position is invalid");
			return false;
		}
	}
}
