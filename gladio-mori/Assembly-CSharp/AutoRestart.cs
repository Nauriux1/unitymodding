using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200017D RID: 381
public class AutoRestart : MonoBehaviour
{
	// Token: 0x06000C27 RID: 3111 RVA: 0x0003A160 File Offset: 0x00038360
	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06000C28 RID: 3112 RVA: 0x0003A16D File Offset: 0x0003836D
	private IEnumerator WaitAndRestart()
	{
		for (;;)
		{
			yield return new WaitForSeconds(this.waitTime);
			if (this.restart && GameMenu.singleton != null && SceneManager.GetActiveScene().name.Contains("map_"))
			{
				GameMenu.singleton.RestartGame();
			}
			else if (this.restart && SceneManager.GetActiveScene().name.ToLower().Contains("lobbymultiplayer"))
			{
				PlayerCanvasController playerCanvasController = UnityEngine.Object.FindObjectOfType<PlayerCanvasController>();
				if (playerCanvasController != null)
				{
					playerCanvasController.readyButton.onClick.Invoke();
				}
			}
		}
		yield break;
	}

	// Token: 0x04000896 RID: 2198
	public bool restart = true;

	// Token: 0x04000897 RID: 2199
	public float waitTime = 5f;
}
