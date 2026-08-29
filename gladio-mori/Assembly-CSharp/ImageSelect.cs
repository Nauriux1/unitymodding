using System;
using System.IO;
using SimpleFileBrowser;
using UnityEngine;
using UnityEngine.UI;
using Utils;

// Token: 0x020001D0 RID: 464
public class ImageSelect : MonoBehaviour
{
	// Token: 0x14000004 RID: 4
	// (add) Token: 0x06000DC9 RID: 3529 RVA: 0x00045E2C File Offset: 0x0004402C
	// (remove) Token: 0x06000DCA RID: 3530 RVA: 0x00045E64 File Offset: 0x00044064
	public event EventHandler ValueChangedEvent;

	// Token: 0x06000DCB RID: 3531 RVA: 0x00045E9C File Offset: 0x0004409C
	private void Start()
	{
		if (this.selectButton != null)
		{
			this.selectButton.onClick.AddListener(delegate()
			{
				this.SelectFile();
			});
		}
		if (this.saveLayoutButton != null)
		{
			this.saveLayoutButton.onClick.AddListener(delegate()
			{
				this.SelectSaveFolder();
			});
		}
		if (this.clearButton != null)
		{
			this.clearButton.onClick.AddListener(delegate()
			{
				this.ClearFile();
			});
		}
		if (this.communityTexturesButton != null)
		{
			this.communityTexturesButton.onClick.AddListener(delegate()
			{
				this.OpenCommunityTextureDialog();
			});
		}
	}

	// Token: 0x06000DCC RID: 3532 RVA: 0x00045F51 File Offset: 0x00044151
	public void Setup(string defaultPath)
	{
		this.path = defaultPath;
		this.RefreshShownValue();
	}

	// Token: 0x06000DCD RID: 3533 RVA: 0x00045F60 File Offset: 0x00044160
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

	// Token: 0x06000DCE RID: 3534 RVA: 0x00046004 File Offset: 0x00044204
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

	// Token: 0x06000DCF RID: 3535 RVA: 0x0004606A File Offset: 0x0004426A
	public void ClearFile()
	{
		this.valueChanged = true;
		this.path = "";
		if (this.texture != null)
		{
			UnityEngine.Object.Destroy(this.texture);
		}
		this.RefreshShownValue();
		this.InvokeValueChanged();
	}

	// Token: 0x06000DD0 RID: 3536 RVA: 0x000460A3 File Offset: 0x000442A3
	public void FileSelected(string newPath)
	{
		Debug.Log("Selected file: " + newPath);
		this.valueChanged = true;
		this.path = newPath;
		this.RefreshShownValue();
		this.InvokeValueChanged();
	}

	// Token: 0x06000DD1 RID: 3537 RVA: 0x000460CF File Offset: 0x000442CF
	private void InvokeValueChanged()
	{
		if (this.ValueChangedEvent != null)
		{
			this.ValueChangedEvent(this, EventArgs.Empty);
		}
	}

	// Token: 0x06000DD2 RID: 3538 RVA: 0x000460EC File Offset: 0x000442EC
	public void SaveFolderSelected(string newPath)
	{
		if (Generic.SaveTexture2DToFileAsPNG(Path.Combine(newPath, this.layoutFileName), LocalizationHelpers.LocalizedTexture2D("PlayerTextureUVLayout")))
		{
			GeneralManager.CreateAlertDialog(LocalizationHelpers.LocalizedText("txt_saved", Array.Empty<object>()), 1f, false);
			return;
		}
		GeneralManager.CreateAlertDialog(LocalizationHelpers.LocalizedText("txt_save_failed", Array.Empty<object>()), 1f, false);
	}

	// Token: 0x06000DD3 RID: 3539 RVA: 0x0004614D File Offset: 0x0004434D
	public void OpenCommunityTextureDialog()
	{
		UnityEngine.Object.Instantiate<GameObject>(this.communityTexturesDialogPrefab).GetComponent<CommunityTextureSelectDialog>().textureSelected += this.CommunityTextureSelected;
	}

	// Token: 0x06000DD4 RID: 3540 RVA: 0x00046170 File Offset: 0x00044370
	private void CommunityTextureSelected(object sender, string path)
	{
		this.FileSelected(path);
	}

	// Token: 0x06000DD5 RID: 3541 RVA: 0x0004617C File Offset: 0x0004437C
	public void RefreshShownValue()
	{
		this.texture = Generic.GetImageFromPath(this.path);
		if (this.texture != null)
		{
			this.previewImage.texture = this.texture;
			this.previewImage.gameObject.SetActive(true);
			return;
		}
		this.previewImage.texture = null;
		this.previewImage.gameObject.SetActive(false);
	}

	// Token: 0x040009EE RID: 2542
	public RawImage previewImage;

	// Token: 0x040009EF RID: 2543
	public Button selectButton;

	// Token: 0x040009F0 RID: 2544
	public Button clearButton;

	// Token: 0x040009F1 RID: 2545
	public Button saveLayoutButton;

	// Token: 0x040009F2 RID: 2546
	public Button communityTexturesButton;

	// Token: 0x040009F3 RID: 2547
	public GameObject communityTexturesDialogPrefab;

	// Token: 0x040009F4 RID: 2548
	public string path = "";

	// Token: 0x040009F5 RID: 2549
	public bool valueChanged;

	// Token: 0x040009F6 RID: 2550
	public string layoutFileName = "";

	// Token: 0x040009F8 RID: 2552
	public Texture2D texture;
}
