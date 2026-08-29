using System;
using TMPro;
using UnityEngine;
using Utils;

// Token: 0x02000085 RID: 133
public class MainMenuManager : MonoBehaviour
{
	// Token: 0x06000448 RID: 1096 RVA: 0x00014F74 File Offset: 0x00013174
	private void Awake()
	{
		Debug.Log("Main menu");
		if (!SettingsHelper.initiated)
		{
			if (this.logger != null)
			{
				this.logger.GetComponent<CustomLog>().InitializeCustomLogger();
			}
			Debug.Log("Initialize settings");
			SettingsHelper.LoadAllSettings();
		}
		else
		{
			GeneralManager.CleanUp();
		}
		if (SettingsHelper.GetFirstLoad())
		{
			this.LoadTutorial();
		}
		this.UpdateVersionText();
	}

	// Token: 0x06000449 RID: 1097 RVA: 0x00014FD9 File Offset: 0x000131D9
	private void LoadTutorial()
	{
		SceneManagerWithParameters.LoadScene("Tutorial", null, false, false);
	}

	// Token: 0x0600044A RID: 1098 RVA: 0x00014FE8 File Offset: 0x000131E8
	private void UpdateVersionText()
	{
		this.versionTextMesh.SetText("v" + Application.version, true);
	}

	// Token: 0x040002BF RID: 703
	public GameObject replayButton;

	// Token: 0x040002C0 RID: 704
	public GameObject wishlistButton;

	// Token: 0x040002C1 RID: 705
	public GameObject logger;

	// Token: 0x040002C2 RID: 706
	public TextMeshProUGUI versionTextMesh;
}
