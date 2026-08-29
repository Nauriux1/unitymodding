using System;
using System.Collections.Generic;
using PaintCore;
using PaintIn3D;
using UnityEngine;

namespace Utils
{
	// Token: 0x02000271 RID: 625
	public class PaintHelpers
	{
		// Token: 0x06001236 RID: 4662 RVA: 0x0005EEF4 File Offset: 0x0005D0F4
		public static List<PaintEditorItem> SetupChildrenForPainting(GameObject gameObject)
		{
			List<PaintEditorItem> list = new List<PaintEditorItem>();
			PaintEditorItem paintEditorItem = PaintHelpers.SetupMainGameObjectForPainting(gameObject);
			if (paintEditorItem != null)
			{
				paintEditorItem.renderer.material = new Material(paintEditorItem.renderer.material);
				list.Add(paintEditorItem);
				foreach (object obj in gameObject.transform)
				{
					Transform transform = (Transform)obj;
					if (!(transform == paintEditorItem.gameObject.transform))
					{
						PaintEditorItem paintEditorItem2 = PaintHelpers.SetupChildGameObjectForPainting(transform.gameObject, paintEditorItem);
						if (paintEditorItem2 != null)
						{
							list.Add(paintEditorItem2);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x0005EFAC File Offset: 0x0005D1AC
		public static PaintEditorItem SetupMainGameObjectForPainting(GameObject holderGameObject)
		{
			GameObject gameObject = holderGameObject.transform.Find("MeshHip").gameObject;
			MeshFilter component = gameObject.GetComponent<MeshFilter>();
			MeshRenderer component2 = gameObject.GetComponent<MeshRenderer>();
			if (component == null || component2 == null)
			{
				return null;
			}
			MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
			meshCollider.sharedMesh = component.sharedMesh;
			CwPaintableMesh cwPaintableMesh = gameObject.AddComponent<CwPaintableMesh>();
			cwPaintableMesh.UseMesh = CwMeshModel.UseMeshType.AutoSeamFix;
			CwPaintableMeshTexture cwPaintableMeshTexture = gameObject.AddComponent<CwPaintableMeshTexture>();
			cwPaintableMeshTexture.Slot = new CwSlot(0, "_MainTexture");
			cwPaintableMeshTexture.Width = 1024;
			cwPaintableMeshTexture.Height = 1024;
			cwPaintableMeshTexture.UndoRedo = CwPaintableTexture.UndoRedoType.LocalCommandCopy;
			return new PaintEditorItem
			{
				meshFilter = component,
				renderer = component2,
				cwMainPaintableMesh = cwPaintableMesh,
				cwMainPaintableMeshTexture = cwPaintableMeshTexture,
				cwLocalPaintableMesh = cwPaintableMesh,
				gameObject = gameObject,
				mainItem = true,
				meshCollider = meshCollider
			};
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x0005F090 File Offset: 0x0005D290
		public static PaintEditorItem SetupChildGameObjectForPainting(GameObject gameObject, PaintEditorItem mainPaintEditorItem)
		{
			MeshFilter component = gameObject.GetComponent<MeshFilter>();
			MeshRenderer component2 = gameObject.GetComponent<MeshRenderer>();
			if (component == null || component2 == null)
			{
				return null;
			}
			mainPaintEditorItem.cwMainPaintableMesh.OtherRenderers.Add(component2);
			component2.sharedMaterial = mainPaintEditorItem.renderer.sharedMaterial;
			MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
			meshCollider.sharedMesh = component.sharedMesh;
			CwPaintableMesh cwPaintableMesh = gameObject.AddComponent<CwPaintableMesh>();
			cwPaintableMesh.UseMesh = CwMeshModel.UseMeshType.AutoSeamFix;
			cwPaintableMesh.Register(mainPaintEditorItem.cwMainPaintableMeshTexture);
			return new PaintEditorItem
			{
				meshFilter = component,
				renderer = component2,
				cwMainPaintableMesh = mainPaintEditorItem.cwMainPaintableMesh,
				cwMainPaintableMeshTexture = mainPaintEditorItem.cwMainPaintableMeshTexture,
				cwLocalPaintableMesh = cwPaintableMesh,
				gameObject = gameObject,
				meshCollider = meshCollider
			};
		}
	}
}
