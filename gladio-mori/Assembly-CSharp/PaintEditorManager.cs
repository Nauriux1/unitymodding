using System;
using System.Collections.Generic;
using System.IO;
using PaintIn3D;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

// Token: 0x02000132 RID: 306
public class PaintEditorManager : MonoBehaviour
{
	// Token: 0x0600097D RID: 2429 RVA: 0x0002D45D File Offset: 0x0002B65D
	private void Awake()
	{
		this.InitializePaintEditorManager();
	}

	// Token: 0x0600097E RID: 2430 RVA: 0x0002D465 File Offset: 0x0002B665
	private void Start()
	{
		this.InitTools();
	}

	// Token: 0x0600097F RID: 2431 RVA: 0x0002D470 File Offset: 0x0002B670
	private void InitializePaintEditorManager()
	{
		if (PaintEditorManager.singleton != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		this.userControls = SettingsHelper.GetUserControls();
		this.userControls.Generic.Enable();
		this.userControls.MoveEditorMap.Enable();
		this.LoadSelectedTexture();
		this.SetupGameObjectAsPaintable();
		PaintEditorManager.singleton = this;
		Debug.Log("Stamina manager has been setup");
	}

	// Token: 0x06000980 RID: 2432 RVA: 0x0002D4E3 File Offset: 0x0002B6E3
	public void LoadSelectedTexture()
	{
		PaintEditorManager.customTextureItem = (CustomTextureItem)SceneManagerWithParameters.GetParameter("CustomTextureItem");
		if (PaintEditorManager.customTextureItem == null)
		{
			PaintEditorManager.customTextureItem = TextureHelpers.CreateNewCustomTexture();
		}
	}

	// Token: 0x06000981 RID: 2433 RVA: 0x0002D50A File Offset: 0x0002B70A
	private void OnDestroy()
	{
		PaintEditorManager.customTextureItem = null;
	}

	// Token: 0x06000982 RID: 2434 RVA: 0x0002D514 File Offset: 0x0002B714
	private void Update()
	{
		if (this.userControls.Generic.Back.WasPerformedThisFrame())
		{
			this.BackButtonPress();
		}
		this.UpdateToolTempDisable();
		if (!this.CurrentlyFocusedOnTextField)
		{
			if (this.userControls.MoveEditorMap.Save.WasPressedThisFrame())
			{
				this.SaveButtonPress();
			}
			if (this.userControls.MoveEditorMap.Redo.WasPressedThisFrame())
			{
				this.Redo();
				return;
			}
			if (this.userControls.MoveEditorMap.Undo.WasPressedThisFrame())
			{
				this.Undo();
			}
		}
	}

	// Token: 0x17000155 RID: 341
	// (get) Token: 0x06000983 RID: 2435 RVA: 0x0002D5B0 File Offset: 0x0002B7B0
	public bool CurrentlyFocusedOnTextField
	{
		get
		{
			return EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.GetComponent<InputField>() != null;
		}
	}

	// Token: 0x06000984 RID: 2436 RVA: 0x0002D5EB File Offset: 0x0002B7EB
	public void BackButtonPress()
	{
		if (GeneralManager.AllowBackNavigation(null))
		{
			this.LeaveConfirm();
		}
	}

	// Token: 0x06000985 RID: 2437 RVA: 0x0002D5FC File Offset: 0x0002B7FC
	private void LeaveConfirm()
	{
		BasicConfirmDialog basicConfirmDialog = GeneralManager.CreateConfirmDialog(LocalizationHelpers.LocalizedText("confirm_txt_leave", Array.Empty<object>()), null, false);
		if (basicConfirmDialog != null)
		{
			basicConfirmDialog.okButton.onClick.AddListener(new UnityAction(this.LeaveMoveEditor));
		}
	}

	// Token: 0x06000986 RID: 2438 RVA: 0x0002D645 File Offset: 0x0002B845
	private void LeaveMoveEditor()
	{
		SceneManagerWithParameters.LoadScene("TextureSelect", null, true, true);
	}

	// Token: 0x06000987 RID: 2439 RVA: 0x0002D654 File Offset: 0x0002B854
	public void SaveButtonPress()
	{
		Debug.Log("save texture");
		this.SaveTexture(false);
	}

	// Token: 0x06000988 RID: 2440 RVA: 0x0002D668 File Offset: 0x0002B868
	public void SaveTexture(bool force = false)
	{
		if (!force && TextureHelpers.TextureExists(PaintEditorManager.customTextureItem))
		{
			this.ConfirmOverwriteSave();
			return;
		}
		try
		{
			Texture2D readableCopy = this.paintableRenderer.gameObject.GetComponent<CwPaintableMeshTexture>().GetReadableCopy(false);
			PaintEditorManager.customTextureItem.texture2D = readableCopy;
			PaintEditorManager.customTextureItem.fileName = TextureHelpers.SaveCustomTextureItemImage(PaintEditorManager.customTextureItem);
			UnityEngine.Object.Destroy(readableCopy);
			GeneralManager.CreateAlertDialog(LocalizationHelpers.LocalizedText("txt_saved", Array.Empty<object>()), 1f, false);
		}
		catch (Exception message)
		{
			Debug.LogError(message);
			GeneralManager.CreateConfirmDialog(LocalizationHelpers.LocalizedText("txt_save_failed", Array.Empty<object>()), null, true);
		}
	}

	// Token: 0x06000989 RID: 2441 RVA: 0x0002D714 File Offset: 0x0002B914
	private void ConfirmOverwriteSave()
	{
		BasicConfirmDialog basicConfirmDialog = GeneralManager.CreateConfirmDialog(LocalizationHelpers.LocalizedText("confirm_title_texture_save_overwrite", Array.Empty<object>()), null, false);
		basicConfirmDialog.okButton.onClick.AddListener(delegate()
		{
			this.SaveTexture(true);
		});
		basicConfirmDialog.cancelButton.Select();
	}

	// Token: 0x0600098A RID: 2442 RVA: 0x0002D754 File Offset: 0x0002B954
	public void SetupGameObjectAsPaintable()
	{
		this.paintEditorItems = PaintHelpers.SetupChildrenForPainting(this.basePaintableGameObject.transform.GetChild(0).gameObject);
		for (int i = 0; i < this.paintEditorItems.Count; i++)
		{
			PaintEditorItem paintEditorItem = this.paintEditorItems[i];
			if (paintEditorItem.mainItem)
			{
				this.mainPaintEditorItem = paintEditorItem;
				break;
			}
		}
		this.SetPlayerTexture();
		this.paintEditorHideTool.InitHideTool(this.paintEditorItems);
	}

	// Token: 0x0600098B RID: 2443 RVA: 0x0002D7CD File Offset: 0x0002B9CD
	public void SetPlayerTexture()
	{
		if (this.mainPaintEditorItem != null && PaintEditorManager.customTextureItem.texture2D != null)
		{
			this.mainPaintEditorItem.renderer.sharedMaterial.SetTexture("_MainTexture", PaintEditorManager.customTextureItem.texture2D);
		}
	}

	// Token: 0x0600098C RID: 2444 RVA: 0x0002D80D File Offset: 0x0002BA0D
	public void Undo()
	{
		if (this.mainPaintEditorItem == null || this.mainPaintEditorItem.cwMainPaintableMeshTexture == null)
		{
			return;
		}
		this.mainPaintEditorItem.cwMainPaintableMeshTexture.Undo();
	}

	// Token: 0x0600098D RID: 2445 RVA: 0x0002D83B File Offset: 0x0002BA3B
	public void Redo()
	{
		if (this.mainPaintEditorItem == null || this.mainPaintEditorItem.cwMainPaintableMeshTexture == null)
		{
			return;
		}
		this.mainPaintEditorItem.cwMainPaintableMeshTexture.Redo();
	}

	// Token: 0x0600098E RID: 2446 RVA: 0x0002D869 File Offset: 0x0002BA69
	public static void SetColor(Color color)
	{
		if (PaintEditorManager.singleton != null)
		{
			PaintEditorManager.singleton.SetToolColor(color);
		}
	}

	// Token: 0x0600098F RID: 2447 RVA: 0x0002D884 File Offset: 0x0002BA84
	public void SetToolColor(Color color)
	{
		foreach (PaintToolItem paintToolItem in this.paintToolItems)
		{
			paintToolItem.SetColor(color);
		}
	}

	// Token: 0x06000990 RID: 2448 RVA: 0x0002D8D8 File Offset: 0x0002BAD8
	public void InitTools()
	{
		this.textureNameInputField.text = PaintEditorManager.customTextureItem.textureName;
		this.textureNameInputField.onEndEdit.AddListener(delegate(string <p0>)
		{
			this.OnNameChanged(this.textureNameInputField.text);
		});
		using (List<PaintToolItem>.Enumerator enumerator = this.paintToolItems.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PaintToolItem tool = enumerator.Current;
				tool.Initialize();
				tool.toolButton.onClick.AddListener(delegate()
				{
					this.SetSelectedTool(tool);
				});
				tool.DeactivateTool();
			}
		}
		this.SetSelectedTool(this.paintToolItems[0]);
	}

	// Token: 0x06000991 RID: 2449 RVA: 0x0002D9B0 File Offset: 0x0002BBB0
	public void SetSelectedTool(PaintToolItem item)
	{
		if (this.currentTool != null)
		{
			this.currentTool.DeactivateTool();
		}
		this.currentTool = item;
		if (this.currentTool != null)
		{
			this.currentTool.ActivateTool();
		}
	}

	// Token: 0x06000992 RID: 2450 RVA: 0x0002D9EC File Offset: 0x0002BBEC
	public void UpdateToolTempDisable()
	{
		if (this.userControls == null || this.currentTool == null)
		{
			return;
		}
		if (this.userControls.Generic.Modifier.IsPressed())
		{
			this.currentTool.TempDisable(true);
			return;
		}
		this.currentTool.TempDisable(false);
	}

	// Token: 0x06000993 RID: 2451 RVA: 0x0002DA44 File Offset: 0x0002BC44
	public void OnNameChanged(string newText)
	{
		if (newText.IndexOfAny(Path.GetInvalidFileNameChars()) < 0 && newText.Length <= 50)
		{
			PaintEditorManager.customTextureItem.textureName = newText;
			PaintEditorManager.customTextureItem.FilterNameForProfanity();
		}
		this.textureNameInputField.SetTextWithoutNotify(PaintEditorManager.customTextureItem.textureName);
	}

	// Token: 0x040006AF RID: 1711
	public static PaintEditorManager singleton;

	// Token: 0x040006B0 RID: 1712
	public UserControls userControls;

	// Token: 0x040006B1 RID: 1713
	public GameObject basePaintableGameObject;

	// Token: 0x040006B2 RID: 1714
	public Renderer paintableRenderer;

	// Token: 0x040006B3 RID: 1715
	public static CustomTextureItem customTextureItem;

	// Token: 0x040006B4 RID: 1716
	public InputField textureNameInputField;

	// Token: 0x040006B5 RID: 1717
	public PaintEditorHideTool paintEditorHideTool;

	// Token: 0x040006B6 RID: 1718
	private PaintEditorItem mainPaintEditorItem;

	// Token: 0x040006B7 RID: 1719
	private List<PaintEditorItem> paintEditorItems;

	// Token: 0x040006B8 RID: 1720
	public List<PaintToolItem> paintToolItems = new List<PaintToolItem>();

	// Token: 0x040006B9 RID: 1721
	private PaintToolItem currentTool;
}
