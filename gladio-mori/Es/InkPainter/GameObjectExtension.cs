using System;
using System.Collections.Generic;
using UnityEngine;

namespace Es.InkPainter
{
	// Token: 0x020002F8 RID: 760
	public static class GameObjectExtension
	{
		// Token: 0x06001718 RID: 5912 RVA: 0x00074EE4 File Offset: 0x000730E4
		public static InkCanvas AddInkCanvas(this GameObject gameObject, List<InkCanvas.PaintSet> paintDatas)
		{
			if (paintDatas == null || paintDatas.Count == 0)
			{
				Debug.LogError("Parameter is null or empty.");
				return null;
			}
			bool activeSelf = gameObject.activeSelf;
			gameObject.SetActive(false);
			InkCanvas inkCanvas = gameObject.AddComponent<InkCanvas>();
			if (inkCanvas == null)
			{
				Debug.LogError("Could not attach InkCanvas to GameObject.");
				return null;
			}
			inkCanvas.OnCanvasAttached += delegate(InkCanvas canvas)
			{
				canvas.PaintDatas = paintDatas;
			};
			gameObject.SetActive(activeSelf);
			return inkCanvas;
		}

		// Token: 0x06001719 RID: 5913 RVA: 0x00074F63 File Offset: 0x00073163
		public static InkCanvas AddInkCanvas(this GameObject gameObject, InkCanvas.PaintSet paintData)
		{
			if (paintData == null)
			{
				Debug.LogError("Parameter is null or empty.");
				return null;
			}
			return gameObject.AddInkCanvas(new List<InkCanvas.PaintSet>
			{
				paintData
			});
		}
	}
}
