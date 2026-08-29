using System;
using System.ComponentModel;
using UnityEngine.Localization.Settings;
using Utils;

// Token: 0x02000243 RID: 579
public class LocalizedDescriptionAttribute : DescriptionAttribute
{
	// Token: 0x060010E6 RID: 4326 RVA: 0x000574D4 File Offset: 0x000556D4
	public LocalizedDescriptionAttribute(string resourceKey)
	{
		this._resourceKey = resourceKey;
	}

	// Token: 0x170001AF RID: 431
	// (get) Token: 0x060010E7 RID: 4327 RVA: 0x000574E4 File Offset: 0x000556E4
	public override string Description
	{
		get
		{
			string localizedString = LocalizationSettings.StringDatabase.GetLocalizedString(SettingsHelper.localizationTableName, this._resourceKey, null, FallbackBehavior.UseProjectSettings, Array.Empty<object>());
			if (!string.IsNullOrEmpty(localizedString))
			{
				return localizedString;
			}
			return string.Format("[[{0}]]", this._resourceKey);
		}
	}

	// Token: 0x04000C93 RID: 3219
	private readonly string _resourceKey;
}
