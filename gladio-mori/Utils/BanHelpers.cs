using System;
using Mirror;

namespace Utils
{
	// Token: 0x02000274 RID: 628
	public class BanHelpers
	{
		// Token: 0x0600123A RID: 4666 RVA: 0x0005F150 File Offset: 0x0005D350
		public static bool IsBanned(NetworkConnectionToClient conn, GladioMoriServerType serverType)
		{
			for (int i = 0; i < SettingsHelper.banList.banItems.Count; i++)
			{
				BanItem banItem = SettingsHelper.banList.banItems[i];
				if (banItem.address == conn.address && banItem.type == serverType)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x0005F1A7 File Offset: 0x0005D3A7
		public static void AddConnectionToBanList(string address, GladioMoriServerType serverType, string name = "")
		{
			SettingsHelper.AddItemToBanListAndSave(new BanItem
			{
				address = address,
				type = serverType,
				name = name
			});
		}
	}
}
