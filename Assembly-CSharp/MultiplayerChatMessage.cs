using System;
using BasicUI;
using TMPro;
using UnityEngine;

// Token: 0x02000125 RID: 293
public class MultiplayerChatMessage : MonoBehaviour
{
	// Token: 0x06000923 RID: 2339 RVA: 0x0002C3B4 File Offset: 0x0002A5B4
	public void SetMessage(string message, string playerName, string playerNameColor = "")
	{
		if (string.IsNullOrEmpty(playerNameColor))
		{
			playerNameColor = UISettings._basicTextColor;
		}
		this.messageText.SetText(string.Concat(new string[]
		{
			"<color=",
			playerNameColor,
			"><noparse>",
			playerName,
			"</noparse></color>: <noparse>",
			message,
			"</noparse>"
		}), true);
	}

	// Token: 0x04000662 RID: 1634
	public TMP_Text messageText;
}
