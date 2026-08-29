using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Utils;

// Token: 0x0200009A RID: 154
public class MusicManager : MonoBehaviour
{
	// Token: 0x06000558 RID: 1368 RVA: 0x00019417 File Offset: 0x00017617
	private void Awake()
	{
		this.InitializeMusicManager();
	}

	// Token: 0x06000559 RID: 1369 RVA: 0x00019420 File Offset: 0x00017620
	public void InitializeMusicManager()
	{
		if (MusicManager.singleton != null)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		MusicManager.singleton = this;
		this.audioSource.loop = true;
		this.SetMusicVolume();
		this.SetMasterVolume();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		Debug.Log("Music manager has been setup");
		this.SetSong("default");
	}

	// Token: 0x0600055A RID: 1370 RVA: 0x0001947F File Offset: 0x0001767F
	public void SetMusicVolume()
	{
		this.audioMixer.SetFloat("MusicVolume", SettingsHelper.GetMusicVolume());
	}

	// Token: 0x0600055B RID: 1371 RVA: 0x00019498 File Offset: 0x00017698
	public void SetMasterVolume()
	{
		float masterVolume = SettingsHelper.GetMasterVolume();
		this.audioMixer.SetFloat("MasterVolume", masterVolume);
	}

	// Token: 0x0600055C RID: 1372 RVA: 0x000194BD File Offset: 0x000176BD
	public void SetEffectsVolume()
	{
		this.audioMixer.SetFloat("EffectsVolume", SettingsHelper.GetEffectsVolume());
	}

	// Token: 0x0600055D RID: 1373 RVA: 0x000194D5 File Offset: 0x000176D5
	public void SetVoiceChatVolume()
	{
		this.audioMixer.SetFloat("VoiceChatVolume", SettingsHelper.GetVoiceChatVolume());
	}

	// Token: 0x0600055E RID: 1374 RVA: 0x000194F0 File Offset: 0x000176F0
	public void SetSong(string sceneName = "default")
	{
		SongSound songSound = (from x in this.musicList
		where x.sceneName == sceneName
		select x).FirstOrDefault<SongSound>();
		if (songSound == null)
		{
			songSound = this.musicList.FirstOrDefault<SongSound>();
		}
		if (songSound != null)
		{
			this.PlaySong(songSound);
		}
	}

	// Token: 0x0600055F RID: 1375 RVA: 0x00019540 File Offset: 0x00017740
	public void PlaySong(SongSound song)
	{
		if (this.audioSource != null && song.sceneName != this.currentSceneName)
		{
			this.audioSource.Stop();
			this.startAudioSource.Stop();
			if (song.StartAudioClip != null)
			{
				this.startAudioSource.clip = song.StartAudioClip;
				this.startAudioSource.Play();
				double time = AudioSettings.dspTime + (double)song.StartAudioClip.length;
				this.audioSource.clip = song.AudioClip;
				this.audioSource.PlayScheduled(time);
			}
			else
			{
				this.audioSource.clip = song.AudioClip;
				this.audioSource.Play();
			}
			this.currentSong = song;
			this.currentSceneName = song.sceneName;
		}
	}

	// Token: 0x06000560 RID: 1376 RVA: 0x00019618 File Offset: 0x00017818
	public void EndSong()
	{
		if (this.currentSong != null && this.currentSong.EndAudioClip != null)
		{
			this.audioSource.Stop();
			this.startAudioSource.Stop();
			this.endAudioSource.clip = this.currentSong.EndAudioClip;
			this.endAudioSource.Play();
		}
	}

	// Token: 0x06000561 RID: 1377 RVA: 0x00019677 File Offset: 0x00017877
	private void OnEnable()
	{
		SceneManager.sceneLoaded += this.OnSceneLoadedMusic;
	}

	// Token: 0x06000562 RID: 1378 RVA: 0x0001968A File Offset: 0x0001788A
	private void OnDisable()
	{
		SceneManager.sceneLoaded -= this.OnSceneLoadedMusic;
	}

	// Token: 0x06000563 RID: 1379 RVA: 0x0001969D File Offset: 0x0001789D
	private void OnSceneLoadedMusic(Scene scene, LoadSceneMode mode)
	{
		this.SetSong(scene.name);
	}

	// Token: 0x04000334 RID: 820
	public AudioSource audioSource;

	// Token: 0x04000335 RID: 821
	public AudioSource startAudioSource;

	// Token: 0x04000336 RID: 822
	public AudioSource endAudioSource;

	// Token: 0x04000337 RID: 823
	public List<SongSound> musicList;

	// Token: 0x04000338 RID: 824
	public static MusicManager singleton;

	// Token: 0x04000339 RID: 825
	public AudioMixer audioMixer;

	// Token: 0x0400033A RID: 826
	private string currentSceneName = "";

	// Token: 0x0400033B RID: 827
	private SongSound currentSong;
}
