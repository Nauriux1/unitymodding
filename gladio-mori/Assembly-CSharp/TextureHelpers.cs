using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Utils;

// Token: 0x02000137 RID: 311
public static class TextureHelpers
{
	// Token: 0x060009AF RID: 2479 RVA: 0x0002DD8F File Offset: 0x0002BF8F
	public static CustomTextureItem CreateNewCustomTexture()
	{
		return new CustomTextureItem
		{
			type = CustomTextureType.Default,
			textureName = LocalizationHelpers.LocalizedText("txt_new_texture", Array.Empty<object>())
		};
	}

	// Token: 0x060009B0 RID: 2480 RVA: 0x0002DDB4 File Offset: 0x0002BFB4
	public static List<CustomTextureItem> GetCustomTextureItems()
	{
		List<CustomTextureItem> list = new List<CustomTextureItem>();
		list.Add(new CustomTextureItem
		{
			fileName = null,
			path = null,
			texture2D = null,
			type = CustomTextureType.None,
			textureName = LocalizationHelpers.LocalizedText("txt_none", Array.Empty<object>())
		});
		list.AddRange(TextureHelpers.GetCustomTextureItemsFromPath(SettingsHelper.GetSavedTextureSavePath(), CustomTextureType.Default));
		list.AddRange(TextureHelpers.GetCustomTextureItemsFromPath(SettingsHelper.GetCommunityTexturesFolder(), CustomTextureType.CommunityTexture));
		return list;
	}

	// Token: 0x060009B1 RID: 2481 RVA: 0x0002DE24 File Offset: 0x0002C024
	public static List<CustomTextureItem> GetCustomTextureItemsFromPath(string path, CustomTextureType customTextureType)
	{
		List<CustomTextureItem> list = new List<CustomTextureItem>();
		try
		{
			foreach (FileInfo fileInfo in new DirectoryInfo(path).GetFiles())
			{
				try
				{
					if (fileInfo.Extension.ToLower() == ".jpg" || fileInfo.Extension.ToLower() == ".jpeg" || fileInfo.Extension.ToLower() == ".png")
					{
						string[] array = Path.GetFileNameWithoutExtension(fileInfo.Name).Split("-", 2, StringSplitOptions.None);
						CustomTextureItem customTextureItem = new CustomTextureItem
						{
							texture2D = Generic.GetImageFromPath(fileInfo.FullName),
							path = fileInfo.FullName,
							fileName = Path.GetFileName(fileInfo.FullName),
							textureName = array[0].Trim(),
							textureCredits = ((array.Length > 1) ? array[1].Trim() : ""),
							type = customTextureType
						};
						if (customTextureItem.texture2D != null)
						{
							list.Add(customTextureItem);
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
		return (from x in list
		orderby x.textureName
		select x).ToList<CustomTextureItem>();
	}

	// Token: 0x060009B2 RID: 2482 RVA: 0x0002DFAC File Offset: 0x0002C1AC
	public static void DeleteCustomTextureItem(CustomTextureItem customTextureItem)
	{
		if (customTextureItem == null || customTextureItem.type != CustomTextureType.Default)
		{
			return;
		}
		Generic.DeleteFile(customTextureItem.path);
	}

	// Token: 0x060009B3 RID: 2483 RVA: 0x0002DFC8 File Offset: 0x0002C1C8
	public static string SaveCustomTextureItemImage(CustomTextureItem customTextureItem)
	{
		string fileName = customTextureItem.fileName;
		string path = customTextureItem.path;
		string text = SettingsHelper.GetSavedTextureSavePath() + customTextureItem.GetFileNameFromTextureName();
		byte[] bytes = Generic.Texture2DToJpgEncodedByteArray(customTextureItem.texture2D);
		string path2 = Generic.CreateBackupForFile(text);
		string path3 = null;
		if (text != path)
		{
			path3 = Generic.CreateBackupForFile(path);
		}
		if (!string.IsNullOrEmpty(fileName) && fileName != Path.GetFileName(text))
		{
			Generic.DeleteFile(path);
		}
		if (File.Exists(text))
		{
			Generic.DeleteFile(text);
		}
		File.WriteAllBytes(text, bytes);
		if (File.Exists(text))
		{
			Generic.DeleteFile(path2);
			Generic.DeleteFile(path3);
			return Path.GetFileName(text);
		}
		throw new Exception("Save failed. Destination file was not created for " + text);
	}

	// Token: 0x060009B4 RID: 2484 RVA: 0x0002E07C File Offset: 0x0002C27C
	public static bool TextureExists(CustomTextureItem customTextureItem)
	{
		return customTextureItem.GetFileNameFromTextureName() != customTextureItem.fileName && File.Exists(Path.Combine(SettingsHelper.GetSavedTextureSavePath(), customTextureItem.GetFileNameFromTextureName()));
	}
}
