using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020001D5 RID: 469
public class LoadSceneOnClick : MonoBehaviour
{
	// Token: 0x06000DF5 RID: 3573 RVA: 0x000465B8 File Offset: 0x000447B8
	public void LoadByIndex(int sceneIndex)
	{
		string scenePathByBuildIndex = SceneUtility.GetScenePathByBuildIndex(sceneIndex);
		int num = scenePathByBuildIndex.LastIndexOf('/');
		string text = scenePathByBuildIndex.Substring(num + 1);
		int length = text.LastIndexOf('.');
		SceneManagerWithParameters.LoadScene(text.Substring(0, length), null, false, false);
	}

	// Token: 0x06000DF6 RID: 3574 RVA: 0x000465F4 File Offset: 0x000447F4
	public void LoadByName(string scene)
	{
		SceneManagerWithParameters.LoadScene(scene, null, this.forceShowLoadingBar, this.manuallyHideLoadingBar);
	}

	// Token: 0x04000A04 RID: 2564
	public bool forceShowLoadingBar;

	// Token: 0x04000A05 RID: 2565
	public bool manuallyHideLoadingBar;
}
