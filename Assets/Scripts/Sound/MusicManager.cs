using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    [SerializeField]
    private MusicLibrary musicLibrary;
    [SerializeField]
    private AudioSource musicSource;

    /* 
    *  Create Singleton intance of the Music Manager
    *  This is so there are no more than ONE Music Manager existing at a time
    *  This is a singleton that nees to be ATTACHED to a GameObject
    *  Music manager can be accessed via: MusicManager.Intance
    */
    public static MusicManager Instance { get; private set;}
    void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
    }

    public void PlayMusic(string trackName, float fadeDuration = 0.5f) {
        musicSource.mute = false;
        StartCoroutine(AnimateMusicCrossfade(musicLibrary.GetClipFromName(trackName), fadeDuration));
    }

    public void PlayMusicNoFade(string trackName) {
        musicSource.mute = false;
        HandlePlayMusicNoFade(musicLibrary.GetClipFromName(trackName)); 
    }

    private void HandlePlayMusicNoFade(AudioClip nextTrack) {
        musicSource.clip = nextTrack;
        musicSource.Play();
    }
 
    IEnumerator AnimateMusicCrossfade(AudioClip nextTrack, float fadeDuration = 0.5f) {
        float percent = 0;
        while (percent < 1) {
            percent += Time.deltaTime * 1 / fadeDuration;
            musicSource.volume = Mathf.Lerp(1f, 0, percent);
            yield return null;
        }
 
        musicSource.clip = nextTrack;
        musicSource.Play();
 
        percent = 0;
        while (percent < 1) {
            percent += Time.deltaTime * 1 / fadeDuration;
            musicSource.volume = Mathf.Lerp(0, 1f, percent);
            yield return null;
        }
    }

    public void PauseMusic() {
        musicSource.mute = true;
    }

    public void DisableMusic() {
        gameObject.SetActive(false);
    }

    public void EnableMusic() {
        gameObject.SetActive(true);
    }
}