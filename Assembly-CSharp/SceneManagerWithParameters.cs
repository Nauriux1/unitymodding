using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020000DC RID: 220
public static class SceneManagerWithParameters
{
	// Token: 0x17000140 RID: 320
	// (get) Token: 0x060007A8 RID: 1960 RVA: 0x00025F7B File Offset: 0x0002417B
	// (set) Token: 0x060007A9 RID: 1961 RVA: 0x00025F94 File Offset: 0x00024194
	private static string PreviousScene
	{
		get
		{
			if (!string.IsNullOrEmpty(SceneManagerWithParameters._previousScene))
			{
				return SceneManagerWithParameters._previousScene;
			}
			return "MainMenu";
		}
		set
		{
			SceneManagerWithParameters._previousScene = value;
		}
	}

	// Token: 0x060007AA RID: 1962 RVA: 0x00025F9C File Offset: 0x0002419C
	public static void LoadScene(string sceneName, Dictionary<string, object> newParameters = null, bool forceShowLoadingBar = false, bool manuallyHideLoadingBar = false)
	{
		SceneManagerWithParameters.PreviousScene = SceneManagerWithParameters.currentScene;
		SceneManagerWithParameters.currentScene = sceneName;
		SceneManagerWithParameters.parameters = newParameters;
		SceneManagerWithParameters.DoSceneLoad(forceShowLoadingBar, manuallyHideLoadingBar);
	}

	// Token: 0x060007AB RID: 1963 RVA: 0x00025FBB File Offset: 0x000241BB
	public static Dictionary<string, object> GetParameters()
	{
		return SceneManagerWithParameters.parameters;
	}

	// Token: 0x060007AC RID: 1964 RVA: 0x00025FC2 File Offset: 0x000241C2
	public static object GetParameter(string name)
	{
		if (SceneManagerWithParameters.parameters != null && SceneManagerWithParameters.parameters.ContainsKey(name))
		{
			return SceneManagerWithParameters.parameters[name];
		}
		return null;
	}

	// Token: 0x060007AD RID: 1965 RVA: 0x00025FE5 File Offset: 0x000241E5
	public static void ReloadScene()
	{
		SceneManagerWithParameters.DoSceneLoad(false, false);
	}

	// Token: 0x060007AE RID: 1966 RVA: 0x00025FEE File Offset: 0x000241EE
	public static void LoadPreviousScene()
	{
		SceneManagerWithParameters.LoadScene(SceneManagerWithParameters.PreviousScene, null, false, false);
	}

	// Token: 0x060007AF RID: 1967 RVA: 0x00026000 File Offset: 0x00024200
	private static void DoSceneLoad(bool forceShowLoadingBar = false, bool manuallyHideLoadingBar = false)
	{
		if (SceneManagerWithParameters.loadingGameScene)
		{
			return;
		}
		SceneManagerWithParameters.loadingGameScene = true;
		if (SceneManagerWithParameters.parameters != null && SceneManagerWithParameters.parameters.ContainsKey("DoLocalMapInit"))
		{
			SceneManager.sceneLoaded += SceneManagerWithParameters.DoLocalMapInit;
		}
		else if (SceneManagerWithParameters.parameters != null && SceneManagerWithParameters.parameters.ContainsKey("DoReplayInit"))
		{
			SceneManager.sceneLoaded += SceneManagerWithParameters.DoReplayInit;
		}
		SceneManagerWithParameters.sceneLoadOperation = SceneManager.LoadSceneAsync(SceneManagerWithParameters.currentScene);
		if (GeneralManager.singleton != null)
		{
			GeneralManager.singleton.ShowLoadingBarForOperation(SceneManagerWithParameters.sceneLoadOperation, SceneManagerWithParameters.currentScene, false, forceShowLoadingBar, manuallyHideLoadingBar);
		}
	}

	// Token: 0x060007B0 RID: 1968 RVA: 0x000260A4 File Offset: 0x000242A4
	private static void DoLocalMapInit(Scene scene, LoadSceneMode mode)
	{
		SceneManager.sceneLoaded -= SceneManagerWithParameters.DoLocalMapInit;
		GameMaster gameMaster = new GameObject("GameMaster").AddComponent<GameMaster>();
		if (scene.name.ToLower().Contains("test"))
		{
			gameMaster.testScene = true;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Managers/PlayersManager", typeof(GameObject))) as GameObject;
		GameObject gameObject2 = GameObject.Find("Managers");
		if (gameObject2 != null)
		{
			gameMaster.transform.SetParent(gameObject2.transform);
			gameObject.transform.SetParent(gameObject2.transform);
		}
		UnityEngine.Object.Instantiate(Resources.Load("UI/GameMenu", typeof(GameObject)));
	}

	// Token: 0x060007B1 RID: 1969 RVA: 0x00026160 File Offset: 0x00024360
	private static void DoReplayInit(Scene scene, LoadSceneMode mode)
	{
		SceneManager.sceneLoaded -= SceneManagerWithParameters.DoReplayInit;
		UnityEngine.Object.Instantiate(Resources.Load("UI/GameMenu", typeof(GameObject)));
		if (ReplayManager.singleton != null)
		{
			ReplayManager.singleton.StartReplay();
		}
	}

	// Token: 0x060007B2 RID: 1970 RVA: 0x000261B0 File Offset: 0x000243B0
	public static bool IsValidScene(string sceneName)
	{
		for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
		{
			if (Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)) == sceneName)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0400051D RID: 1309
	private static Dictionary<string, object> parameters;

	// Token: 0x0400051E RID: 1310
	public static string currentScene;

	// Token: 0x0400051F RID: 1311
	private static string _previousScene;

	// Token: 0x04000520 RID: 1312
	public static AsyncOperation sceneLoadOperation;

	// Token: 0x04000521 RID: 1313
	public static bool loadingGameScene;
}
