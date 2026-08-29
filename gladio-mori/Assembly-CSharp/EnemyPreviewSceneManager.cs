using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;
using Utils;

// Token: 0x02000163 RID: 355
public class EnemyPreviewSceneManager : MonoBehaviour
{
	// Token: 0x1700016E RID: 366
	// (get) Token: 0x06000B5C RID: 2908 RVA: 0x00036C7C File Offset: 0x00034E7C
	public float fightPreviewItemHeight
	{
		get
		{
			if (this._fightPreviewItemHeight == null)
			{
				this._fightPreviewItemHeight = new float?(this.fightPreviewItemPrefab.GetComponent<RectTransform>().sizeDelta.y);
			}
			return this._fightPreviewItemHeight.Value;
		}
	}

	// Token: 0x1700016F RID: 367
	// (get) Token: 0x06000B5D RID: 2909 RVA: 0x00036CB6 File Offset: 0x00034EB6
	private float fightItemTotalHeight
	{
		get
		{
			return this.fightPreviewItemHeight + this.fightItemMargin;
		}
	}

	// Token: 0x06000B5E RID: 2910 RVA: 0x00036CC8 File Offset: 0x00034EC8
	private void Start()
	{
		this.youWinText.gameObject.SetActive(false);
		this.SetWinWeaponPositions();
		this.PopulateUI();
		this.DoTowerAnimation();
		this.buttonsPanel.gameObject.SetActive(false);
		this.gameOverButtonsPanel.gameObject.SetActive(false);
		this.startButton.onClick.AddListener(delegate()
		{
			this.StartFight();
		});
		this.abandonRunButton.onClick.AddListener(delegate()
		{
			this.AbandonRun();
		});
		this.backButton.onClick.AddListener(delegate()
		{
			this.NavigateBack();
		});
		this.gameOverContinueButton.onClick.AddListener(delegate()
		{
			this.NavigateBack();
		});
		this.userControls = SettingsHelper.GetUserControls();
		this.userControls.Generic.Enable();
		InputSystem.onAnyButtonPress.Call(new Action<InputControl>(this.SkipTowerAnimation));
	}

	// Token: 0x06000B5F RID: 2911 RVA: 0x00036DC0 File Offset: 0x00034FC0
	private void Update()
	{
		if (this.userControls != null && this.userControls.Generic.Back.WasPerformedThisFrame())
		{
			this.NavigateBack();
		}
	}

	// Token: 0x06000B60 RID: 2912 RVA: 0x00036DF5 File Offset: 0x00034FF5
	private void NavigateBack()
	{
		if (GeneralManager.AllowBackNavigation(null) && this.TowerAnimationIsDone())
		{
			if (SingleplayerManager.singleton.PlayerWonTheCampaign)
			{
				SingleplayerManager.singleton.DoAbandonRun();
				return;
			}
			SceneManagerWithParameters.LoadScene("MainMenu", null, false, false);
		}
	}

	// Token: 0x06000B61 RID: 2913 RVA: 0x00036E2B File Offset: 0x0003502B
	public bool TowerAnimationIsDone()
	{
		return this.runningTweenKillFrame != Time.frameCount && this.runningTween != null && !this.runningTween.IsActive();
	}

	// Token: 0x06000B62 RID: 2914 RVA: 0x00036E52 File Offset: 0x00035052
	private void PopulateUI()
	{
		this.PopulatePlayerSide();
		this.PopulateEnemySide();
	}

	// Token: 0x06000B63 RID: 2915 RVA: 0x00036E60 File Offset: 0x00035060
	private void PopulatePlayerSide()
	{
		UnityEngine.Object.Instantiate<GameObject>(this.fightPreviewItemPrefab, this.playerPreviewPanel).GetComponent<EnemyPreviewItem>().SetPlayerInfo();
	}

	// Token: 0x06000B64 RID: 2916 RVA: 0x00036E80 File Offset: 0x00035080
	private void PopulateEnemySide()
	{
		int num = 0;
		foreach (FightItem fightItem in SingleplayerManager.singleton.fightItems)
		{
			EnemyPreviewItem component = UnityEngine.Object.Instantiate<GameObject>(this.fightPreviewItemPrefab, this.enemyPreviewPanel).GetComponent<EnemyPreviewItem>();
			component.SetFightItem(fightItem);
			component.rectTransform.localPosition = new Vector3(0f, this.fightItemTotalHeight * (float)num);
			this.enemyPreviewItems.Add(component);
			num++;
		}
	}

	// Token: 0x06000B65 RID: 2917 RVA: 0x00036F20 File Offset: 0x00035120
	private void DoTowerAnimation()
	{
		Vector3 newLocalPosition = default(Vector3);
		float animationDuration = this.animationTimePerEnemy;
		if (SingleplayerManager.singleton.singleplayerRun.fightIndex == 0)
		{
			animationDuration = (float)this.enemyPreviewItems.Count - 1f * this.animationTimePerEnemy;
			this.enemyPreviewPanel.localPosition = new Vector3(0f, -this.fightItemTotalHeight * (float)(this.enemyPreviewItems.Count - 1));
		}
		else if (SingleplayerManager.singleton.PlayerWonTheCampaign)
		{
			this.versusText.gameObject.SetActive(false);
			this.enemyPreviewPanel.localPosition = new Vector3(0f, -this.fightItemTotalHeight * (float)(SingleplayerManager.singleton.singleplayerRun.fightIndex - 1));
			float num = (float)(Screen.height / 2) + this.fightItemTotalHeight;
			animationDuration = num / this.fightItemTotalHeight * this.animationTimePerEnemy;
			newLocalPosition = new Vector3(0f, this.enemyPreviewPanel.localPosition.y - num);
			base.StartCoroutine(this.AnimateGameWon(animationDuration));
		}
		else
		{
			this.enemyPreviewPanel.localPosition = new Vector3(0f, -this.fightItemTotalHeight * (float)(SingleplayerManager.singleton.singleplayerRun.fightIndex - 1));
			newLocalPosition = new Vector3(0f, -this.fightItemTotalHeight * (float)SingleplayerManager.singleton.singleplayerRun.fightIndex);
		}
		base.StartCoroutine(this.AnimateEnemyPreviewList(newLocalPosition, animationDuration));
	}

	// Token: 0x06000B66 RID: 2918 RVA: 0x00037096 File Offset: 0x00035296
	private IEnumerator AnimateEnemyPreviewList(Vector3 newLocalPosition, float animationDuration)
	{
		this.runningTween = this.enemyPreviewPanel.DOLocalMove(newLocalPosition, animationDuration, false);
		this.runningTween.Pause<TweenerCore<Vector3, Vector3, VectorOptions>>();
		this.runningTween.OnKill(new TweenCallback(this.TowerAnimationCompleted));
		yield return new WaitForSeconds(1f);
		this.runningTween.Play<TweenerCore<Vector3, Vector3, VectorOptions>>();
		yield break;
	}

	// Token: 0x06000B67 RID: 2919 RVA: 0x000370B4 File Offset: 0x000352B4
	private void SkipTowerAnimation(InputControl inputControl)
	{
		if (this.runningTween != null && this.runningTween.IsActive())
		{
			this.runningTween.Kill(true);
			this.runningTweenKillFrame = Time.frameCount;
		}
		if (this.gameWonTween != null && this.gameWonTween.IsActive())
		{
			this.gameWonTween.Kill(true);
		}
	}

	// Token: 0x06000B68 RID: 2920 RVA: 0x00037110 File Offset: 0x00035310
	private void TowerAnimationCompleted()
	{
		if (SingleplayerManager.singleton.PlayerWonTheCampaign)
		{
			this.gameOverButtonsPanel.gameObject.SetActive(true);
			this.gameOverContinueButton.Select();
			return;
		}
		this.buttonsPanel.gameObject.SetActive(true);
		this.startButton.Select();
	}

	// Token: 0x06000B69 RID: 2921 RVA: 0x00037162 File Offset: 0x00035362
	private void StartFight()
	{
		if (this.TowerAnimationIsDone())
		{
			this.buttonsPanel.gameObject.SetActive(false);
			base.StartCoroutine(this.StartNextFightTimer());
		}
	}

	// Token: 0x06000B6A RID: 2922 RVA: 0x0003718A File Offset: 0x0003538A
	private void AbandonRun()
	{
		SingleplayerManager.singleton.AbandonRun();
	}

	// Token: 0x06000B6B RID: 2923 RVA: 0x00037196 File Offset: 0x00035396
	private IEnumerator StartNextFightTimer()
	{
		if (MusicManager.singleton != null)
		{
			MusicManager.singleton.EndSong();
		}
		yield return new WaitForSeconds(1f);
		SingleplayerManager.singleton.StartNextMatch();
		yield break;
	}

	// Token: 0x06000B6C RID: 2924 RVA: 0x0003719E File Offset: 0x0003539E
	private void OnDestroy()
	{
		this.DisposeUserControls();
	}

	// Token: 0x06000B6D RID: 2925 RVA: 0x000371A6 File Offset: 0x000353A6
	public void DisposeUserControls()
	{
		if (this.userControls != null)
		{
			this.userControls.Disable();
			this.userControls.Dispose();
		}
	}

	// Token: 0x06000B6E RID: 2926 RVA: 0x000371C6 File Offset: 0x000353C6
	private IEnumerator AnimateGameWon(float animationDuration)
	{
		this.gameWonTween = this.playerPreviewPanel.DOLocalMove(new Vector3(0f, 0f, 0f), animationDuration, false);
		this.gameWonTween.Pause<TweenerCore<Vector3, Vector3, VectorOptions>>();
		this.gameWonTween.OnKill(new TweenCallback(this.GameWonTweenCompleted));
		yield return new WaitForSeconds(1f);
		this.gameWonTween.Play<TweenerCore<Vector3, Vector3, VectorOptions>>();
		yield break;
	}

	// Token: 0x06000B6F RID: 2927 RVA: 0x000371DC File Offset: 0x000353DC
	private void GameWonTweenCompleted()
	{
		this.youWinText.gameObject.SetActive(true);
		this.AnimateWinWeapons();
	}

	// Token: 0x06000B70 RID: 2928 RVA: 0x000371F8 File Offset: 0x000353F8
	private void SetWinWeaponPositions()
	{
		this.weapon1Position = this.weapon1.localPosition;
		this.weapon2Position = this.weapon2.localPosition;
		this.weapon3Position = this.weapon3.localPosition;
		this.weapon4Position = this.weapon4.localPosition;
		this.weapon1.localPosition = new Vector3(-((float)(Screen.width / 2) + this.weapon1.sizeDelta.x), this.weapon1.localPosition.y, this.weapon1.localPosition.z);
		this.weapon2.localPosition = new Vector3((float)(Screen.width / 2) + this.weapon2.sizeDelta.x, this.weapon2.localPosition.y, this.weapon2.localPosition.z);
		this.weapon3.localPosition = new Vector3(-((float)(Screen.width / 2) + this.weapon3.sizeDelta.x), this.weapon3.localPosition.y, this.weapon3.localPosition.z);
		this.weapon4.localPosition = new Vector3((float)(Screen.width / 2) + this.weapon4.sizeDelta.x, this.weapon4.localPosition.y, this.weapon4.localPosition.z);
	}

	// Token: 0x06000B71 RID: 2929 RVA: 0x00037370 File Offset: 0x00035570
	private void AnimateWinWeapons()
	{
		this.PlayWeaponAudioSources();
		this.weaponTween = this.weapon1.DOLocalMove(this.weapon1Position, this.winWeaponAnimationDuration, false);
		this.weapon1.DORotate(new Vector3(this.weapon1.eulerAngles.x, this.weapon1.eulerAngles.y, this.weapon1.eulerAngles.z - 1080f), this.winWeaponAnimationDuration, RotateMode.FastBeyond360);
		this.weaponTween.OnKill(new TweenCallback(this.StopWeaponAudioSources));
		this.weapon2.DOLocalMove(this.weapon2Position, this.winWeaponAnimationDuration, false);
		this.weapon2.DORotate(new Vector3(this.weapon2.eulerAngles.x, this.weapon2.eulerAngles.y, this.weapon2.eulerAngles.z + 1080f), this.winWeaponAnimationDuration, RotateMode.FastBeyond360);
		this.weapon3.DOLocalMove(this.weapon3Position, this.winWeaponAnimationDuration, false);
		this.weapon3.DORotate(new Vector3(this.weapon3.eulerAngles.x, this.weapon3.eulerAngles.y, this.weapon3.eulerAngles.z - 1080f), this.winWeaponAnimationDuration, RotateMode.FastBeyond360);
		this.weapon4.DOLocalMove(this.weapon4Position, this.winWeaponAnimationDuration, false);
		this.weapon4.DORotate(new Vector3(this.weapon4.eulerAngles.x, this.weapon4.eulerAngles.y, this.weapon4.eulerAngles.z + 1080f), this.winWeaponAnimationDuration, RotateMode.FastBeyond360);
	}

	// Token: 0x06000B72 RID: 2930 RVA: 0x0003753C File Offset: 0x0003573C
	private void PlayWeaponAudioSources()
	{
		int num = 0;
		foreach (AudioSource audioSource in this.weaponAudioSources)
		{
			audioSource.time = 0f;
			if (num == 1)
			{
				audioSource.time = 0.02f;
			}
			audioSource.Play();
			audioSource.transform.DOLocalMove(new Vector3(0f, audioSource.transform.localPosition.y, audioSource.transform.localPosition.z), this.winWeaponAnimationDuration, false);
			num++;
		}
	}

	// Token: 0x06000B73 RID: 2931 RVA: 0x000375EC File Offset: 0x000357EC
	private void StopWeaponAudioSources()
	{
		foreach (AudioSource audioSource in this.weaponAudioSources)
		{
			audioSource.Stop();
		}
		this.shwingAudio.Play();
		this.shwingAudio2.PlayDelayed(0.02f);
	}

	// Token: 0x040007E8 RID: 2024
	public Transform playerPreviewPanel;

	// Token: 0x040007E9 RID: 2025
	public Transform enemyPreviewPanel;

	// Token: 0x040007EA RID: 2026
	public GameObject fightPreviewItemPrefab;

	// Token: 0x040007EB RID: 2027
	private float? _fightPreviewItemHeight;

	// Token: 0x040007EC RID: 2028
	public Transform buttonsPanel;

	// Token: 0x040007ED RID: 2029
	public Transform gameOverButtonsPanel;

	// Token: 0x040007EE RID: 2030
	public Button backButton;

	// Token: 0x040007EF RID: 2031
	public Button startButton;

	// Token: 0x040007F0 RID: 2032
	public Button abandonRunButton;

	// Token: 0x040007F1 RID: 2033
	public Button gameOverContinueButton;

	// Token: 0x040007F2 RID: 2034
	public Text versusText;

	// Token: 0x040007F3 RID: 2035
	public Text youWinText;

	// Token: 0x040007F4 RID: 2036
	public UserControls userControls;

	// Token: 0x040007F5 RID: 2037
	private float fightItemMargin = 10f;

	// Token: 0x040007F6 RID: 2038
	public List<EnemyPreviewItem> enemyPreviewItems = new List<EnemyPreviewItem>();

	// Token: 0x040007F7 RID: 2039
	private float animationTimePerEnemy = 1f;

	// Token: 0x040007F8 RID: 2040
	private TweenerCore<Vector3, Vector3, VectorOptions> runningTween;

	// Token: 0x040007F9 RID: 2041
	private int runningTweenKillFrame;

	// Token: 0x040007FA RID: 2042
	private TweenerCore<Vector3, Vector3, VectorOptions> gameWonTween;

	// Token: 0x040007FB RID: 2043
	public RectTransform weapon1;

	// Token: 0x040007FC RID: 2044
	public RectTransform weapon2;

	// Token: 0x040007FD RID: 2045
	public RectTransform weapon3;

	// Token: 0x040007FE RID: 2046
	public RectTransform weapon4;

	// Token: 0x040007FF RID: 2047
	private Vector3 weapon1Position;

	// Token: 0x04000800 RID: 2048
	private Vector3 weapon2Position;

	// Token: 0x04000801 RID: 2049
	private Vector3 weapon3Position;

	// Token: 0x04000802 RID: 2050
	private Vector3 weapon4Position;

	// Token: 0x04000803 RID: 2051
	private float winWeaponAnimationDuration = 1f;

	// Token: 0x04000804 RID: 2052
	public List<AudioSource> weaponAudioSources = new List<AudioSource>();

	// Token: 0x04000805 RID: 2053
	public AudioSource shwingAudio;

	// Token: 0x04000806 RID: 2054
	public AudioSource shwingAudio2;

	// Token: 0x04000807 RID: 2055
	private TweenerCore<Vector3, Vector3, VectorOptions> weaponTween;
}
