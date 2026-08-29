using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.Rendering.Universal;
using Utils;

// Token: 0x02000241 RID: 577
public static class LocalizationHelpers
{
	// Token: 0x060010D8 RID: 4312 RVA: 0x0005707F File Offset: 0x0005527F
	public static string LocalizedText(string key, [Nullable(new byte[]
	{
		0,
		2
	})] params object[] args)
	{
		return LocalizationSettings.StringDatabase.GetLocalizedString(SettingsHelper.localizationTableName, key, null, FallbackBehavior.UseProjectSettings, args);
	}

	// Token: 0x060010D9 RID: 4313 RVA: 0x0005709E File Offset: 0x0005529E
	public static string LocalizedText(string key, Locale locale, [Nullable(new byte[]
	{
		0,
		2
	})] params object[] args)
	{
		return LocalizationSettings.StringDatabase.GetLocalizedString(SettingsHelper.localizationTableName, key, locale, FallbackBehavior.UseProjectSettings, args);
	}

	// Token: 0x060010DA RID: 4314 RVA: 0x000570C0 File Offset: 0x000552C0
	public static List<string> LocalizedTextForAllLanguages(string key, [Nullable(new byte[]
	{
		0,
		2
	})] params object[] args)
	{
		List<string> list = new List<string>();
		foreach (Locale locale in LocalizationSettings.AvailableLocales.Locales)
		{
			string text = LocalizationHelpers.LocalizedText(key, locale, args);
			if (!string.IsNullOrWhiteSpace(text))
			{
				list.Add(text);
			}
		}
		return list;
	}

	// Token: 0x060010DB RID: 4315 RVA: 0x00057130 File Offset: 0x00055330
	public static string LocalizedTextWithCheck(string key, [Nullable(new byte[]
	{
		0,
		2
	})] params object[] args)
	{
		try
		{
			if (!string.IsNullOrEmpty(key))
			{
				if (key.Contains("{"))
				{
					string text = new string(key.SkipWhile((char c) => c != '{').Skip(1).TakeWhile((char c) => c != '}').ToArray<char>()).Trim();
					key = new string(key.TakeWhile((char c) => c != '{').ToArray<char>()).Trim();
					args = new object[]
					{
						text
					};
				}
				StringTable table = LocalizationSettings.StringDatabase.GetTable(SettingsHelper.localizationTableName, null);
				if (table != null && table.GetEntry(key) != null)
				{
					return LocalizationSettings.StringDatabase.GetLocalizedString(SettingsHelper.localizationTableName, key, null, FallbackBehavior.UseProjectSettings, args);
				}
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
		return key;
	}

	// Token: 0x060010DC RID: 4316 RVA: 0x0005726C File Offset: 0x0005546C
	public static string GetLocalizedTextForWindowMode(FullScreenMode fullScreenMode)
	{
		string result = "";
		switch (fullScreenMode)
		{
		case FullScreenMode.ExclusiveFullScreen:
			result = LocalizationHelpers.LocalizedText("option_windowmode_exclusivefullscreen", Array.Empty<object>());
			break;
		case FullScreenMode.FullScreenWindow:
			result = LocalizationHelpers.LocalizedText("option_windowmode_fullscreenwindow", Array.Empty<object>());
			break;
		case FullScreenMode.MaximizedWindow:
			result = LocalizationHelpers.LocalizedText("option_windowmode_maximizedwindow", Array.Empty<object>());
			break;
		case FullScreenMode.Windowed:
			result = LocalizationHelpers.LocalizedText("option_windowmode_windowed", Array.Empty<object>());
			break;
		}
		return result;
	}

	// Token: 0x060010DD RID: 4317 RVA: 0x000572E0 File Offset: 0x000554E0
	public static string GetLocalizedTextForAntiAliasing(AntialiasingMode antialiasingMode)
	{
		string result = "";
		switch (antialiasingMode)
		{
		case AntialiasingMode.None:
			result = LocalizationHelpers.LocalizedText("txt_none", Array.Empty<object>());
			break;
		case AntialiasingMode.FastApproximateAntialiasing:
			result = LocalizationHelpers.LocalizedText("option_antialiasing_fast_approximate_antialiasing", Array.Empty<object>());
			break;
		case AntialiasingMode.SubpixelMorphologicalAntiAliasing:
			result = LocalizationHelpers.LocalizedText("option_antialiasing_subpixel_morphological_antialiasing", Array.Empty<object>());
			break;
		}
		return result;
	}

	// Token: 0x060010DE RID: 4318 RVA: 0x0005733C File Offset: 0x0005553C
	public static string GetLocalizedTextForInputAction(string inputName, bool localizeDefaultMove = false)
	{
		string result;
		if (inputName.ToLower().Contains("action"))
		{
			string text = "Action";
			string text2 = "";
			if (inputName.ToLower().Contains("directional"))
			{
				text = "Directional_Action";
				localizeDefaultMove = false;
			}
			string text3 = inputName.Replace(text, "");
			if (localizeDefaultMove)
			{
				text2 = LocalizationHelpers.DefaultMoveTextForAction(Convert.ToInt32(text3));
			}
			result = LocalizationHelpers.LocalizedText(text, new object[]
			{
				text3,
				text2
			});
		}
		else
		{
			result = LocalizationHelpers.LocalizedText(inputName, Array.Empty<object>());
		}
		return result;
	}

	// Token: 0x060010DF RID: 4319 RVA: 0x000573C8 File Offset: 0x000555C8
	public static string DefaultMoveTextForAction(int actionNumber)
	{
		string text = "";
		object[] args = null;
		switch (actionNumber)
		{
		case 1:
			text = "moveset_action_attack_left";
			break;
		case 2:
			text = "moveset_action_stab";
			break;
		case 3:
			text = "moveset_action_attack_right";
			break;
		case 4:
			text = "moveset_action_attack_special";
			args = new object[]
			{
				"1"
			};
			break;
		case 5:
			text = "moveset_action_attack_high";
			break;
		case 6:
			text = "moveset_action_attack_special";
			args = new object[]
			{
				"2"
			};
			break;
		case 8:
			text = "moveset_action_attack_low";
			break;
		case 10:
			text = "moveset_stance_block";
			break;
		}
		string result = "";
		if (!string.IsNullOrEmpty(text))
		{
			string text2 = LocalizationHelpers.LocalizedText(text, args);
			result = LocalizationHelpers.LocalizedText("txt_default_action", new object[]
			{
				text2
			});
		}
		return result;
	}

	// Token: 0x060010E0 RID: 4320 RVA: 0x00057497 File Offset: 0x00055697
	public static Texture2D LocalizedTexture2D(string key)
	{
		return LocalizationSettings.AssetDatabase.GetLocalizedAsset<Texture2D>(SettingsHelper.localizationTableNameAssets, key, null);
	}
}
