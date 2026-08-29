using System;
using System.Collections.Generic;
using System.IO;
using SimpleFileBrowser;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

// Token: 0x02000139 RID: 313
public class TextureSelectCanvas : MonoBehaviour
{
	// Token: 0x060009B8 RID: 2488 RVA: 0x0002E0BF File Offset: 0x0002C2BF
	private void Awake()
	{
		this.InitializeTextureCanvas();
	}

	// Token: 0x060009B9 RID: 2489 RVA: 0x0002E0C7 File Offset: 0x0002C2C7
	public void InitializeTextureCanvas()
	{
		if (TextureSelectCanvas.singleton != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		TextureSelectCanvas.singleton = this;
		this.LoadPreviewImageGenerationsScene();
		Debug.Log("Texture select canvas has been setup");
	}

	// Token: 0x060009BA RID: 2490 RVA: 0x0002E0F8 File Offset: 0x0002C2F8
	public void InitPreviews()
	{
		this.InitButtons();
		this.DisplayTextureOptions();
		this.InitPreviewPlayer();
	}

	// Token: 0x060009BB RID: 2491 RVA: 0x0002E10C File Offset: 0x0002C30C
	private void InitButtons()
	{
		if (this.backButton != null)
		{
			this.backButton.onClick.AddListener(delegate()
			{
				this.BackButtonClicked();
			});
		}
		this.editTextureButton.onClick.AddListener(delegate()
		{
			this.EditTextureButtonClicked();
		});
		this.copyTextureButton.onClick.AddListener(delegate()
		{
			this.CopyTextureButtonClicked();
		});
		this.selectTextureButton.onClick.AddListener(delegate()
		{
			this.SelectTextureButtonClicked();
		});
		this.newTextureButton.onClick.AddListener(delegate()
		{
			this.NewTextureButtonClicked();
		});
		this.deleteTextureButton.onClick.AddListener(delegate()
		{
			this.DeleteTextureButtonClicked();
		});
		if (this.saveLayoutButton != null)
		{
			this.saveLayoutButton.onClick.AddListener(delegate()
			{
				this.SelectSaveFolder();
			});
		}
		if (this.importButton != null)
		{
			this.importButton.onClick.AddListener(delegate()
			{
				this.SelectFile();
			});
		}
		this.UpdateButtons();
	}

	// Token: 0x060009BC RID: 2492 RVA: 0x0002E22C File Offset: 0x0002C42C
	private void SelectTextureButtonClicked()
	{
		if (this.selectedCustomTextureButtonItem != null && this.selectedCustomTextureButtonItem.customTextureItem != null)
		{
			if (SettingsHelper.SaveCustomPlayerTexture(this.selectedCustomTextureButtonItem.customTextureItem.path))
			{
				GeneralManager.CreateAlertDialog(LocalizationHelpers.LocalizedText("txt_saved", Array.Empty<object>()), 1f, false);
				SettingsHelper.LoadAllSettings();
				return;
			}
			GeneralManager.CreateConfirmDialog(LocalizationHelpers.LocalizedText("txt_save_failed", Array.Empty<object>()), null, true);
		}
	}

	// Token: 0x060009BD RID: 2493 RVA: 0x0002E2A4 File Offset: 0x0002C4A4
	private void EditTextureButtonClicked()
	{
		if (this.selectedCustomTextureButtonItem != null && this.selectedCustomTextureButtonItem.customTextureItem != null)
		{
			CustomTextureItem customTextureItem = this.selectedCustomTextureButtonItem.customTextureItem.CreateDeepClone();
			if (customTextureItem.type != CustomTextureType.Default)
			{
				customTextureItem.TurnIntoCopy();
			}
			SceneManagerWithParameters.LoadScene("PaintEditor", new Dictionary<string, object>
			{
				{
					"CustomTextureItem",
					customTextureItem
				}
			}, false, false);
		}
	}

	// Token: 0x060009BE RID: 2494 RVA: 0x0002E308 File Offset: 0x0002C508
	private void CopyTextureButtonClicked()
	{
		if (this.selectedCustomTextureButtonItem != null && this.selectedCustomTextureButtonItem.customTextureItem != null)
		{
			try
			{
				string text = this.selectedCustomTextureButtonItem.customTextureItem.textureName;
				if (text.Length <= 50)
				{
					text += LocalizationHelpers.LocalizedText("txt_append_to_copied_name", Array.Empty<object>());
				}
				Generic.CopyFileToLocation(SettingsHelper.GetSavedTextureSavePath(), this.selectedCustomTextureButtonItem.customTextureItem.path, text);
				this.DisplayTextureOptions();
				GeneralManager.CreateAlertDialog(LocalizationHelpers.LocalizedText("txt_created", Array.Empty<object>()), 1f, false);
			}
			catch (Exception message)
			{
				Debug.LogError(message);
				GeneralManager.CreateConfirmDialog(LocalizationHelpers.LocalizedText("txt_error", Array.Empty<object>()), null, true);
			}
		}
	}

	// Token: 0x060009BF RID: 2495 RVA: 0x0002E3D4 File Offset: 0x0002C5D4
	private void NewTextureButtonClicked()
	{
		SceneManagerWithParameters.LoadScene("PaintEditor", new Dictionary<string, object>
		{
			{
				"CustomTextureItem",
				TextureHelpers.CreateNewCustomTexture()
			}
		}, false, false);
	}

	// Token: 0x060009C0 RID: 2496 RVA: 0x0002E3F8 File Offset: 0x0002C5F8
	private void DeleteTextureButtonClicked()
	{
		if (this.selectedCustomTextureButtonItem != null && this.selectedCustomTextureButtonItem.customTextureItem != null && this.selectedCustomTextureButtonItem.customTextureItem.type == CustomTextureType.Default)
		{
			BasicConfirmDialog basicConfirmDialog = GeneralManager.CreateConfirmDialog(LocalizationHelpers.LocalizedText("confirm_txt_delete", new object[]
			{
				this.selectedCustomTextureButtonItem.customTextureItem.textureName
			}), null, false);
			basicConfirmDialog.okButton.onClick.AddListener(new UnityAction(this.DeleteTexture));
			basicConfirmDialog.cancelButton.Select();
		}
	}

	// Token: 0x060009C1 RID: 2497 RVA: 0x0002E484 File Offset: 0x0002C684
	private void DeleteTexture()
	{
		if (this.selectedCustomTextureButtonItem != null && this.selectedCustomTextureButtonItem.customTextureItem != null && this.selectedCustomTextureButtonItem.customTextureItem.type == CustomTextureType.Default)
		{
			TextureHelpers.DeleteCustomTextureItem(this.selectedCustomTextureButtonItem.customTextureItem);
			UnityEngine.Object.Destroy(this.selectedCustomTextureButtonItem.gameObject);
			this.SelectCustomTexture(null);
		}
	}

	// Token: 0x060009C2 RID: 2498 RVA: 0x0002E4E5 File Offset: 0x0002C6E5
	public void BackButtonClicked()
	{
		if (GeneralManager.AllowBackNavigation(null))
		{
			SceneManager.LoadScene("MainMenu");
		}
	}

	// Token: 0x060009C3 RID: 2499 RVA: 0x0002E4FC File Offset: 0x0002C6FC
	public CustomTextureButtonItem CreateButtonForTexture(CustomTextureItem item)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.buttonPrefab, this.textureSelectPanelItemsHolder.transform);
		CustomTextureButtonItem buttonItem = gameObject.GetComponent<CustomTextureButtonItem>();
		buttonItem.SetCustomTextureItem(item);
		buttonItem.button.onClick.AddListener(delegate()
		{
			this.SelectCustomTexture(buttonItem);
		});
		InputMoveScrollViewOnSelect inputMoveScrollViewOnSelect = gameObject.AddComponent<InputMoveScrollViewOnSelect>();
		inputMoveScrollViewOnSelect.horizontal = true;
		inputMoveScrollViewOnSelect.scrollRect = this.textureSelectScrollRect;
		gameObject.SetActive(false);
		return buttonItem;
	}

	// Token: 0x060009C4 RID: 2500 RVA: 0x0002E58C File Offset: 0x0002C78C
	public void SelectCustomTexture(CustomTextureButtonItem buttonItem)
	{
		if (this.selectedCustomTextureButtonItem != null)
		{
			this.selectedCustomTextureButtonItem.SetStatus(false);
		}
		this.selectedCustomTextureButtonItem = buttonItem;
		if (this.selectedCustomTextureButtonItem != null)
		{
			this.selectedCustomTextureButtonItem.SetStatus(true);
		}
		this.UpdateButtons();
		this.UpdatePreviewTexture();
	}

	// Token: 0x060009C5 RID: 2501 RVA: 0x0002E5E0 File Offset: 0x0002C7E0
	public void DisplayTextureOptions()
	{
		foreach (object obj in this.textureSelectPanelItemsHolder.transform)
		{
			UnityEngine.Object.Destroy(((Transform)obj).gameObject);
		}
		this.textureSelectButtons = new List<CustomTextureButtonItem>();
		this.textureFightItems = new List<FightItem>();
		foreach (CustomTextureItem customTextureItem in TextureHelpers.GetCustomTextureItems())
		{
			this.textureFightItems.Add(customTextureItem.CreateFightItem());
			this.textureSelectButtons.Add(this.CreateButtonForTexture(customTextureItem));
		}
		this.GenerateTextureImages();
	}

	// Token: 0x060009C6 RID: 2502 RVA: 0x0002E6BC File Offset: 0x0002C8BC
	private void UpdateButtons()
	{
		this.deleteTextureButton.interactable = false;
		this.editTextureButton.interactable = false;
		this.copyTextureButton.interactable = false;
		if (this.selectedCustomTextureButtonItem != null && this.selectedCustomTextureButtonItem.customTextureItem.type != CustomTextureType.None)
		{
			this.editTextureButton.interactable = true;
			this.copyTextureButton.interactable = true;
			if (this.selectedCustomTextureButtonItem.customTextureItem.type == CustomTextureType.Default)
			{
				this.deleteTextureButton.interactable = true;
			}
		}
		this.deleteTextureButton.GetComponent<BasicButton>().CheckDisableColor();
		this.editTextureButton.GetComponent<BasicButton>().CheckDisableColor();
		this.copyTextureButton.GetComponent<BasicButton>().CheckDisableColor();
	}

	// Token: 0x060009C7 RID: 2503 RVA: 0x0002E774 File Offset: 0x0002C974
	public void InitPreviewPlayer()
	{
		if (this.previewPlayerHealth == null)
		{
			return;
		}
		this.previewPlayerHealth.OnlyAnimation();
		this.previewPlayerHealth.InitMaterial();
		this.FetchDefaultTexture();
		this.UpdatePreviewTexture();
	}

	// Token: 0x060009C8 RID: 2504 RVA: 0x0002E7A7 File Offset: 0x0002C9A7
	public void FetchDefaultTexture()
	{
		this.defaultTexture2D = SettingsHelper.customPlayerTexture;
	}

	// Token: 0x060009C9 RID: 2505 RVA: 0x0002E7B4 File Offset: 0x0002C9B4
	public void UpdatePreviewTexture()
	{
		if (this.previewPlayerHealth == null)
		{
			return;
		}
		Texture2D texture2D = this.defaultTexture2D;
		if (this.selectedCustomTextureButtonItem != null)
		{
			texture2D = this.selectedCustomTextureButtonItem.customTextureItem.texture2D;
		}
		this.previewPlayerHealth.SetPlayerTexture(texture2D);
	}

	// Token: 0x060009CA RID: 2506 RVA: 0x0002E804 File Offset: 0x0002CA04
	public void SelectSaveFolder()
	{
		FileBrowser.SetFilters(false);
		FileBrowser.ShowLoadDialog(delegate(string[] paths)
		{
			this.SaveFolderSelected(paths[0]);
		}, delegate
		{
			Debug.Log("Canceled");
		}, FileBrowser.PickMode.Folders, false, null, null, LocalizationHelpers.LocalizedText("txt_select_save_location_for_layout", Array.Empty<object>()), LocalizationHelpers.LocalizedText("btn_save", Array.Empty<object>()));
	}

	// Token: 0x060009CB RID: 2507 RVA: 0x0002E86C File Offset: 0x0002CA6C
	public void SaveFolderSelected(string newPath)
	{
		if (Generic.SaveTexture2DToFileAsPNG(Path.Combine(newPath, this.layoutFileName), LocalizationHelpers.LocalizedTexture2D("PlayerTextureUVLayout")))
		{
			GeneralManager.CreateAlertDialog(LocalizationHelpers.LocalizedText("txt_saved", Array.Empty<object>()), 1f, false);
			return;
		}
		GeneralManager.CreateAlertDialog(LocalizationHelpers.LocalizedText("txt_save_failed", Array.Empty<object>()), 1f, false);
	}

	// Token: 0x060009CC RID: 2508 RVA: 0x0002E8D0 File Offset: 0x0002CAD0
	public void SelectFile()
	{
		FileBrowser.SetFilters(false, new FileBrowser.Filter[]
		{
			new FileBrowser.Filter(LocalizationHelpers.LocalizedText("txt_images", Array.Empty<object>()), new string[]
			{
				".jpg",
				".jpeg",
				".png"
			})
		});
		FileBrowser.ShowLoadDialog(delegate(string[] paths)
		{
			this.FileSelected(paths[0]);
		}, delegate
		{
			Debug.Log("Canceled");
		}, FileBrowser.PickMode.Files, false, null, null, LocalizationHelpers.LocalizedText("option_custom_player_texture", Array.Empty<object>()), LocalizationHelpers.LocalizedText("btn_select", Array.Empty<object>()));
	}

	// Token: 0x060009CD RID: 2509 RVA: 0x0002E974 File Offset: 0x0002CB74
	public void FileSelected(string newPath)
	{
		Debug.Log("Selected file: " + newPath);
		try
		{
			Generic.CopyFileToLocation(SettingsHelper.GetSavedTextureSavePath(), newPath, null);
			GeneralManager.CreateAlertDialog(LocalizationHelpers.LocalizedText("txt_imported", Array.Empty<object>()), 1f, false);
			this.DisplayTextureOptions();
		}
		catch (Exception message)
		{
			Debug.Log(message);
			GeneralManager.CreateAlertDialog(LocalizationHelpers.LocalizedText("txt_import_failed", Array.Empty<object>()), 1f, false);
		}
	}

	// Token: 0x060009CE RID: 2510 RVA: 0x0002E9F4 File Offset: 0x0002CBF4
	public void LoadPreviewImageGenerationsScene()
	{
		TextureSelectCanvas.sceneLoadOperation = SceneManager.LoadSceneAsync("PreviewImageGenerationScene", LoadSceneMode.Additive);
	}

	// Token: 0x060009CF RID: 2511 RVA: 0x0002EA06 File Offset: 0x0002CC06
	public void RegisterPreviewImageGenerationManager(PreviewImageGenerationManager newPreviewImageGenerationManager)
	{
		this.previewImageGenerationManager = newPreviewImageGenerationManager;
		this.InitPreviews();
	}

	// Token: 0x060009D0 RID: 2512 RVA: 0x0002EA15 File Offset: 0x0002CC15
	public void GenerateTextureImages()
	{
		this.imagesGenerated = false;
		this.previewImageGenerationManager.GenerateImagesForFightItems(this.textureFightItems, PreviewImageGenerationMode.TexturePreview);
	}

	// Token: 0x060009D1 RID: 2513 RVA: 0x0002EA30 File Offset: 0x0002CC30
	public void GenerateIndividualImage()
	{
		this.imagesGenerated = false;
	}

	// Token: 0x060009D2 RID: 2514 RVA: 0x0002EA39 File Offset: 0x0002CC39
	public void ImagesHaveBeenGenerated(PreviewImageGenerationMode mode)
	{
		if (mode == PreviewImageGenerationMode.TexturePreview)
		{
			this.UpdateButtonImagesImages();
		}
		if (GeneralManager.singleton != null)
		{
			GeneralManager.singleton.RemoveLoadingScreen();
		}
		this.imagesGenerated = true;
	}

	// Token: 0x060009D3 RID: 2515 RVA: 0x0002EA64 File Offset: 0x0002CC64
	public void UpdateButtonImagesImages()
	{
		for (int i = 0; i < this.textureSelectButtons.Count; i++)
		{
			this.textureSelectButtons[i].UpdatePreviewImage();
		}
	}

	// Token: 0x040006CC RID: 1740
	public Button backButton;

	// Token: 0x040006CD RID: 1741
	public Button deleteTextureButton;

	// Token: 0x040006CE RID: 1742
	public Button editTextureButton;

	// Token: 0x040006CF RID: 1743
	public Button newTextureButton;

	// Token: 0x040006D0 RID: 1744
	public Button selectTextureButton;

	// Token: 0x040006D1 RID: 1745
	public Button copyTextureButton;

	// Token: 0x040006D2 RID: 1746
	public Button importButton;

	// Token: 0x040006D3 RID: 1747
	public Button saveLayoutButton;

	// Token: 0x040006D4 RID: 1748
	public static TextureSelectCanvas singleton;

	// Token: 0x040006D5 RID: 1749
	public GameObject buttonPrefab;

	// Token: 0x040006D6 RID: 1750
	public GameObject textureSelectPanelItemsHolder;

	// Token: 0x040006D7 RID: 1751
	public ScrollRect textureSelectScrollRect;

	// Token: 0x040006D8 RID: 1752
	private CustomTextureButtonItem selectedCustomTextureButtonItem;

	// Token: 0x040006D9 RID: 1753
	public List<CustomTextureButtonItem> textureSelectButtons;

	// Token: 0x040006DA RID: 1754
	public PlayerHealth previewPlayerHealth;

	// Token: 0x040006DB RID: 1755
	private Texture2D defaultTexture2D;

	// Token: 0x040006DC RID: 1756
	private string layoutFileName = "GladioMoriPlayerTextureUV.png";

	// Token: 0x040006DD RID: 1757
	public List<FightItem> textureFightItems = new List<FightItem>();

	// Token: 0x040006DE RID: 1758
	private PreviewImageGenerationManager previewImageGenerationManager;

	// Token: 0x040006DF RID: 1759
	public static AsyncOperation sceneLoadOperation;

	// Token: 0x040006E0 RID: 1760
	public bool imagesGenerated;
}
