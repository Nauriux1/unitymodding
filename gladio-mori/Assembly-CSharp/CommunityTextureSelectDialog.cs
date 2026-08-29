using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Utils;

// Token: 0x02000222 RID: 546
public class CommunityTextureSelectDialog : MonoBehaviour, IDialog
{
	// Token: 0x14000008 RID: 8
	// (add) Token: 0x06001097 RID: 4247 RVA: 0x00055C54 File Offset: 0x00053E54
	// (remove) Token: 0x06001098 RID: 4248 RVA: 0x00055C8C File Offset: 0x00053E8C
	public event EventHandler<string> textureSelected;

	// Token: 0x06001099 RID: 4249 RVA: 0x00055CC1 File Offset: 0x00053EC1
	private void Start()
	{
		this.OnCreated();
		this.closeButton.onClick.AddListener(delegate()
		{
			this.Close();
		});
		this.RecalculateDialogSize();
		this.DisplayTextureOptions();
	}

	// Token: 0x0600109A RID: 4250 RVA: 0x00055CF1 File Offset: 0x00053EF1
	public void OnCreated()
	{
		GeneralManager.DialogCreated(this);
	}

	// Token: 0x0600109B RID: 4251 RVA: 0x00055CF9 File Offset: 0x00053EF9
	private void OnDestroy()
	{
		GeneralManager.DialogDestroyed(this);
	}

	// Token: 0x0600109C RID: 4252 RVA: 0x00055D04 File Offset: 0x00053F04
	private void RecalculateDialogSize()
	{
		float num = this.itemWidth + this.paddingSize * 1f;
		float num2 = this.paddingSize * 4f + this.scrollBarWidth;
		int num3 = (int)Math.Floor((double)(((float)Screen.width - num2 - this.dialogPadding * 2f) / num));
		if (num3 == 0)
		{
			num3 = 1;
		}
		if (num3 > 5)
		{
			num3 = 5;
		}
		this.dialog.sizeDelta = new Vector2((float)num3 * num + num2, (float)(Screen.height - 200));
	}

	// Token: 0x0600109D RID: 4253 RVA: 0x00055D88 File Offset: 0x00053F88
	public void DisplayTextureOptions()
	{
		foreach (object obj in this.textureSelectPanelItemsHolder.transform)
		{
			UnityEngine.Object.Destroy(((Transform)obj).gameObject);
		}
		this.textureSelectButtons = new List<CommunityTextureItem>();
		foreach (CommunityTextureItem item in CommunityTextureSelectDialog.GetCommunityTextureItems())
		{
			this.createButtonForTexture(item);
		}
	}

	// Token: 0x0600109E RID: 4254 RVA: 0x00055E38 File Offset: 0x00054038
	public static List<CommunityTextureItem> GetCommunityTextureItems()
	{
		List<CommunityTextureItem> list = new List<CommunityTextureItem>();
		try
		{
			foreach (FileInfo fileInfo in new DirectoryInfo(SettingsHelper.GetCommunityTexturesFolder()).GetFiles())
			{
				try
				{
					if (fileInfo.Extension.ToLower() == ".jpg" || fileInfo.Extension.ToLower() == ".jpeg" || fileInfo.Extension.ToLower() == ".png")
					{
						string[] array = Path.GetFileNameWithoutExtension(fileInfo.Name).Split("-", 2, StringSplitOptions.None);
						CommunityTextureItem communityTextureItem = new CommunityTextureItem
						{
							texture2D = Generic.GetImageFromPath(fileInfo.FullName),
							path = fileInfo.FullName,
							textureName = array[0].Trim(),
							textureCredits = ((array.Length > 1) ? array[1].Trim() : "")
						};
						if (communityTextureItem.texture2D != null)
						{
							list.Add(communityTextureItem);
						}
					}
				}
				catch (Exception message)
				{
					Debug.Log(message);
				}
			}
		}
		catch (Exception message2)
		{
			Debug.Log(message2);
		}
		return list;
	}

	// Token: 0x0600109F RID: 4255 RVA: 0x00055F6C File Offset: 0x0005416C
	public CommunityTextureButtonItem createButtonForTexture(CommunityTextureItem item)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.buttonPrefab);
		CommunityTextureButtonItem buttonItem = gameObject.GetComponent<CommunityTextureButtonItem>();
		gameObject.transform.parent = this.textureSelectPanelItemsHolder.transform;
		buttonItem.SetCommunityTextureItem(item);
		buttonItem.button.onClick.AddListener(delegate()
		{
			this.SelectCommunityTexture(buttonItem.communityTextureItem);
		});
		return buttonItem;
	}

	// Token: 0x060010A0 RID: 4256 RVA: 0x00055FE7 File Offset: 0x000541E7
	public void SelectCommunityTexture(CommunityTextureItem item)
	{
		if (!string.IsNullOrWhiteSpace(item.path))
		{
			this.textureSelected(this, item.path);
		}
		this.Close();
	}

	// Token: 0x060010A1 RID: 4257 RVA: 0x000117D2 File Offset: 0x0000F9D2
	public void Close()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04000BFD RID: 3069
	public Button closeButton;

	// Token: 0x04000BFE RID: 3070
	public GameObject textureSelectPanelScrollView;

	// Token: 0x04000BFF RID: 3071
	public GameObject textureSelectPanelItemsHolder;

	// Token: 0x04000C00 RID: 3072
	public ScrollRect textureSelectScrollRect;

	// Token: 0x04000C01 RID: 3073
	public RectTransform dialog;

	// Token: 0x04000C02 RID: 3074
	public GameObject buttonPrefab;

	// Token: 0x04000C03 RID: 3075
	public List<CommunityTextureItem> textureSelectButtons;

	// Token: 0x04000C05 RID: 3077
	private float itemWidth = 260f;

	// Token: 0x04000C06 RID: 3078
	private float paddingSize = 2f;

	// Token: 0x04000C07 RID: 3079
	private float scrollBarWidth = 20f;

	// Token: 0x04000C08 RID: 3080
	private float dialogPadding = 100f;
}
