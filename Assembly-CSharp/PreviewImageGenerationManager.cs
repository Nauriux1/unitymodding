using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

// Token: 0x02000167 RID: 359
public class PreviewImageGenerationManager : MonoBehaviour
{
	// Token: 0x06000B8B RID: 2955 RVA: 0x000378A4 File Offset: 0x00035AA4
	private int GetImageHeight()
	{
		if (this.currentMode == PreviewImageGenerationMode.TexturePreview)
		{
			return this.imageSizeTexturePreview;
		}
		if (this.currentMode == PreviewImageGenerationMode.Difficulty)
		{
			return this.imageHeightDifficulty;
		}
		return this.imageHeight;
	}

	// Token: 0x06000B8C RID: 2956 RVA: 0x000378CB File Offset: 0x00035ACB
	private int GetImageWidth()
	{
		if (this.currentMode == PreviewImageGenerationMode.TexturePreview)
		{
			return this.imageSizeTexturePreview;
		}
		return this.imageWidth;
	}

	// Token: 0x06000B8D RID: 2957 RVA: 0x000378E4 File Offset: 0x00035AE4
	private void Start()
	{
		this.renderTexture = new RenderTexture(this.GetImageWidth(), this.GetImageHeight(), 0);
		this.renderTexture.filterMode = FilterMode.Bilinear;
		this.renderTexture.antiAliasing = 8;
		this.renderTexture.Create();
		this.currentCamera.targetTexture = this.renderTexture;
		RenderPipelineManager.endCameraRendering += this.OnPostRenderCallback;
		this.initiated = true;
		if (SingleplayerManager.singleton != null)
		{
			SingleplayerManager.singleton.RegisterPreviewImageGenerationManager(this);
		}
		if (TextureSelectCanvas.singleton != null)
		{
			TextureSelectCanvas.singleton.RegisterPreviewImageGenerationManager(this);
		}
	}

	// Token: 0x06000B8E RID: 2958 RVA: 0x00037986 File Offset: 0x00035B86
	private void EnableTestItems()
	{
		this.testCamera.gameObject.SetActive(true);
		this.testImage.gameObject.SetActive(true);
		this.testCanvas.gameObject.SetActive(true);
	}

	// Token: 0x06000B8F RID: 2959 RVA: 0x000379BB File Offset: 0x00035BBB
	public void CreateNewDestinationTexture()
	{
		this.destinationTexture = new Texture2D(this.GetImageWidth(), this.GetImageHeight(), TextureFormat.RGBA32, false);
	}

	// Token: 0x06000B90 RID: 2960 RVA: 0x000379D8 File Offset: 0x00035BD8
	public void SetPreviewImageGenerationMode(PreviewImageGenerationMode newMode)
	{
		this.currentMode = newMode;
		int num = this.GetImageWidth();
		int num2 = this.GetImageHeight();
		if (this.renderTexture != null && (num != this.renderTexture.width || num2 != this.renderTexture.height))
		{
			this.renderTexture.Release();
			this.renderTexture.width = num;
			this.renderTexture.height = num2;
		}
	}

	// Token: 0x06000B91 RID: 2961 RVA: 0x00037A48 File Offset: 0x00035C48
	public void GenerateImagesForFightItems(List<FightItem> fightItems, PreviewImageGenerationMode mode)
	{
		this.SetPreviewImageGenerationMode(mode);
		this.allPreviewImageItems.Clear();
		foreach (FightItem fightItem in fightItems)
		{
			PreviewImageItem previewImageItem = new PreviewImageItem();
			previewImageItem.fightItem = fightItem;
			foreach (FightOpponent fightOpponent in fightItem.fightOpponents)
			{
				PreviewImageCharacter previewImageCharacter = new PreviewImageCharacter();
				if (SingleplayerManager.singleton != null)
				{
					previewImageCharacter.moveSet = SingleplayerManager.GetFightOpponentMoveset(fightOpponent);
					previewImageCharacter.equippedEquipment = SingleplayerManager.GetFightOpponentEquipment(fightOpponent);
				}
				previewImageCharacter.customTexture = fightOpponent.customTexture;
				previewImageCharacter.ai = true;
				previewImageItem.previewImageCharacters.Add(previewImageCharacter);
			}
			this.allPreviewImageItems.Add(previewImageItem);
		}
		this.StartTakingImages();
	}

	// Token: 0x06000B92 RID: 2962 RVA: 0x00037B58 File Offset: 0x00035D58
	public void GenerateImagesForLobbyPlayer(LobbyPlayer lobbyPlayer)
	{
		this.SetPreviewImageGenerationMode(PreviewImageGenerationMode.Player);
		this.allPreviewImageItems.Clear();
		PreviewImageItem previewImageItem = new PreviewImageItem();
		PreviewImageCharacter previewImageCharacter = new PreviewImageCharacter();
		previewImageCharacter.moveSet = lobbyPlayer.GetMoveSet();
		previewImageCharacter.equippedEquipment = lobbyPlayer.GetSelectedEquipment();
		previewImageCharacter.ai = false;
		previewImageCharacter.customTexture = SettingsHelper.GetCustomPlayerTexture();
		previewImageItem.previewImageCharacters.Add(previewImageCharacter);
		this.allPreviewImageItems.Add(previewImageItem);
		this.StartTakingImages();
	}

	// Token: 0x06000B93 RID: 2963 RVA: 0x00037BCB File Offset: 0x00035DCB
	private void StartTakingImages()
	{
		this.currentFightItemIndex = -1;
		this.TakeNextImage();
	}

	// Token: 0x06000B94 RID: 2964 RVA: 0x00037BDC File Offset: 0x00035DDC
	private void TakeNextImage()
	{
		this.currentFightItemIndex++;
		if (this.allPreviewImageItems.Count > this.currentFightItemIndex)
		{
			this.GenerateImageForItem(this.allPreviewImageItems[this.currentFightItemIndex]);
			return;
		}
		this.DisableCamera();
		if (SingleplayerManager.singleton != null)
		{
			SingleplayerManager.singleton.ImagesHaveBeenGenerated(this.currentMode);
		}
		if (TextureSelectCanvas.singleton != null)
		{
			TextureSelectCanvas.singleton.ImagesHaveBeenGenerated(this.currentMode);
		}
	}

	// Token: 0x06000B95 RID: 2965 RVA: 0x00037C62 File Offset: 0x00035E62
	public void GenerateImageForItem(PreviewImageItem newPreviewImageItem)
	{
		this.CreateNewDestinationTexture();
		this.previewImageItem = newPreviewImageItem;
		this.CreatePlayerCharactersForFightItem();
	}

	// Token: 0x06000B96 RID: 2966 RVA: 0x00037C78 File Offset: 0x00035E78
	public void CreatePlayerCharactersForFightItem()
	{
		foreach (PlayerHealth playerHealth in this.playerHealths)
		{
			playerHealth.transform.position = new Vector3(playerHealth.transform.position.x, -100f, playerHealth.transform.position.z);
		}
		int num = 0;
		foreach (PreviewImageCharacter previewImageCharacter in this.previewImageItem.previewImageCharacters)
		{
			PlayerHealth playerHealth2 = null;
			if (num < this.playerHealths.Count)
			{
				playerHealth2 = this.playerHealths[num];
			}
			if (playerHealth2 == null && this.playerCharacterPrefab != null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.playerCharacterPrefab, new Vector3(this.currentCamera.transform.position.x, this.currentCamera.transform.position.y - 1f, this.currentCamera.transform.position.z + 4f), this.currentCamera.transform.rotation);
				SceneManager.MoveGameObjectToScene(gameObject, base.gameObject.scene);
				PlayerHealth component = gameObject.GetComponent<PlayerHealth>();
				float y = gameObject.transform.position.y;
				gameObject.transform.position = new Vector3(gameObject.transform.position.x + component.playerAnimator.transform.localPosition.x, y, gameObject.transform.position.z);
				gameObject.transform.Rotate(new Vector3(0f, 180f, 0f));
				component.OnlyAnimation();
				this.playerHealths.Add(component);
				playerHealth2 = component;
			}
			if (playerHealth2 != null)
			{
				playerHealth2.ai = previewImageCharacter.ai;
				playerHealth2.LoadLocalCustomPlayerTexture();
				playerHealth2.playerAnimator.SetMoveSet(previewImageCharacter.moveSet, true, true);
				playerHealth2.SetEquipment(previewImageCharacter.equippedEquipment, false);
				if (previewImageCharacter.customTexture != null)
				{
					playerHealth2.SetPlayerTexture(previewImageCharacter.customTexture);
				}
				else
				{
					playerHealth2.SetPlayerTexture(null);
				}
				this.SetPlayerPosition(playerHealth2, this.previewImageItem.previewImageCharacters.Count, num);
			}
			num++;
		}
		this.TakeImage();
	}

	// Token: 0x06000B97 RID: 2967 RVA: 0x00037F44 File Offset: 0x00036144
	public void SetPlayerPosition(PlayerHealth playerHealth, int totalAmount, int index)
	{
		Vector3 position = new Vector3(this.currentCamera.transform.position.x + playerHealth.playerAnimator.transform.localPosition.x, this.currentCamera.transform.position.y - 1.5f, this.currentCamera.transform.position.z + 3.5f);
		int num = (int)Math.Ceiling((double)((float)index / 2f));
		float num2 = (float)num;
		float num3 = (float)num;
		if (index % 2 == 0)
		{
			num3 *= -1f;
		}
		position.x += num3;
		position.z += num2;
		if (this.currentMode == PreviewImageGenerationMode.TexturePreview)
		{
			position.y += 0.5f;
		}
		playerHealth.transform.position = position;
	}

	// Token: 0x06000B98 RID: 2968 RVA: 0x00038024 File Offset: 0x00036224
	public void TakeImage()
	{
		this.isTakingImage = true;
		this.EnableCamera();
	}

	// Token: 0x06000B99 RID: 2969 RVA: 0x00038033 File Offset: 0x00036233
	private void OnDestroy()
	{
		if (this.initiated)
		{
			this.renderTexture.Release();
			UnityEngine.Object.Destroy(this.renderTexture);
			RenderPipelineManager.endCameraRendering -= this.OnPostRenderCallback;
		}
	}

	// Token: 0x06000B9A RID: 2970 RVA: 0x00038064 File Offset: 0x00036264
	private void OnPostRenderCallback(ScriptableRenderContext context, Camera cam)
	{
		if (this.isTakingImage && cam == this.currentCamera)
		{
			Rect source = new Rect(0f, 0f, (float)this.GetImageWidth(), (float)this.GetImageHeight());
			int destX = 0;
			int destY = 0;
			bool recalculateMipMaps = false;
			this.destinationTexture.ReadPixels(source, destX, destY, recalculateMipMaps);
			this.destinationTexture.Apply();
			if (this.allPreviewImageItems[this.currentFightItemIndex].fightItem != null)
			{
				this.allPreviewImageItems[this.currentFightItemIndex].fightItem.previewImage = this.destinationTexture;
			}
			else if (SingleplayerManager.singleton != null)
			{
				SingleplayerManager.singleton.playerImage = this.destinationTexture;
			}
			this.isTakingImage = false;
			this.TakeNextImage();
		}
	}

	// Token: 0x06000B9B RID: 2971 RVA: 0x00038137 File Offset: 0x00036337
	private void EnableCamera()
	{
		this.currentCamera.enabled = true;
	}

	// Token: 0x06000B9C RID: 2972 RVA: 0x00038145 File Offset: 0x00036345
	private void DisableCamera()
	{
		this.currentCamera.enabled = false;
	}

	// Token: 0x06000B9D RID: 2973 RVA: 0x00038153 File Offset: 0x00036353
	private void UpdateTestImage()
	{
		if (this.testImage != null)
		{
			this.testImage.texture = this.renderTexture;
		}
	}

	// Token: 0x04000813 RID: 2067
	public Camera testCamera;

	// Token: 0x04000814 RID: 2068
	public RawImage testImage;

	// Token: 0x04000815 RID: 2069
	public Canvas testCanvas;

	// Token: 0x04000816 RID: 2070
	public FightItem testFightItem;

	// Token: 0x04000817 RID: 2071
	public Camera currentCamera;

	// Token: 0x04000818 RID: 2072
	public RenderTexture renderTexture;

	// Token: 0x04000819 RID: 2073
	private bool initiated;

	// Token: 0x0400081A RID: 2074
	public Texture2D destinationTexture;

	// Token: 0x0400081B RID: 2075
	private int imageWidth = 300;

	// Token: 0x0400081C RID: 2076
	private int imageHeight = 200;

	// Token: 0x0400081D RID: 2077
	private int imageHeightDifficulty = 400;

	// Token: 0x0400081E RID: 2078
	private int imageSizeTexturePreview = 256;

	// Token: 0x0400081F RID: 2079
	public PreviewImageGenerationMode currentMode;

	// Token: 0x04000820 RID: 2080
	public GameObject playerCharacterPrefab;

	// Token: 0x04000821 RID: 2081
	private List<PreviewImageItem> allPreviewImageItems = new List<PreviewImageItem>();

	// Token: 0x04000822 RID: 2082
	private int currentFightItemIndex = -1;

	// Token: 0x04000823 RID: 2083
	public PreviewImageItem previewImageItem;

	// Token: 0x04000824 RID: 2084
	private List<PlayerHealth> playerHealths = new List<PlayerHealth>();

	// Token: 0x04000825 RID: 2085
	private bool isTakingImage;
}
