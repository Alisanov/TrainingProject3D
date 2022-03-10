using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour, IGameManager
{
    [SerializeField] private AudioSource soundSource;
    [SerializeField] private AudioSource music1Source;
    [SerializeField] private AudioSource music2Source;

    private AudioSource _activeMusic;
    private AudioSource _inactiveMusic;

    public float crossFadeRate = 3f;
    private bool _crossFading;

    [SerializeField] private string introBGMusic;
    [SerializeField] private string levelBGMusic;
    private float _musicVolume;
    private float _soundVolume;

    private const string parmSoundVolume = "soundVolume";
    private const string parmMusicVolume = "musicVolume";
    private const string parmSoundMute = "soundMute";
    private const string parmMusicMute = "musicMute";

    private NetworkService _network;
    public float musicVolume
    {
        get
        {
            return _musicVolume;
        }
        set
        {
            _musicVolume = value;
            if (music1Source != null && !_crossFading)
            {
                music1Source.volume = _musicVolume;
                music2Source.volume = _musicVolume;
            }
        }
    }
    public bool musicMute
    {
        get
        {
            if (music1Source != null)
                return music1Source.mute;
            return false;
        }
        set
        {
            if (music1Source != null)
            {
                music1Source.mute = value;
                music2Source.mute = value;
            }
        }

    }
    public float soundVolume
    {
        get { return AudioListener.volume; }
        set {  AudioListener.volume = value; _soundVolume = value; }
    }
    public bool soundMute
    {
        get { return AudioListener.pause; }
        set { AudioListener.pause = value; }
    }
    public ManagerStatus status { get; private set; }

    public void Startup(NetworkService service)
    {
        Debug.Log("Audio manager service");

        music1Source.ignoreListenerVolume = true;
        music1Source.ignoreListenerPause = true;
        music2Source.ignoreListenerVolume = true;
        music2Source.ignoreListenerPause = true;
        float soundVolume = 1f, musicVolume = 1f;
        if(PlayerPrefs.HasKey(parmSoundVolume))
            soundVolume = PlayerPrefs.GetFloat(parmSoundVolume);
        if(PlayerPrefs.HasKey(parmMusicVolume))
            musicVolume = PlayerPrefs.GetFloat(parmMusicVolume);
        bool soundMute = true, musicMute = true;
        if (PlayerPrefs.HasKey(parmSoundMute))
            soundMute = PlayerPrefs.GetInt(parmSoundMute) == 1; 
        if (PlayerPrefs.HasKey(parmMusicMute))
            musicMute = PlayerPrefs.GetInt(parmMusicMute) == 1;

        UpdateDate(soundVolume, musicVolume, soundMute, musicMute);

        _activeMusic = music1Source;
        _inactiveMusic = music2Source;
        _network = service;

        status = ManagerStatus.Started;
    }

    public void PlaySound(AudioClip clip)
    {
        soundSource.PlayOneShot(clip);
    }

    public void PlayIntroMusic()
    {
        PlayMusic(Resources.Load("Music/" + introBGMusic) as AudioClip);
    }

    public void PlayLevelMusic()
    {
        PlayMusic(Resources.Load("Music/" + levelBGMusic) as AudioClip);
    }    

    private void PlayMusic(AudioClip clip)
    {
        if (_crossFading) { return; }
        StartCoroutine(CrossFadeMusic(clip));
    }
    private IEnumerator CrossFadeMusic(AudioClip clip)
    {
        _crossFading = true;

        _inactiveMusic.clip = clip;
        _inactiveMusic.volume = 0;
        _inactiveMusic.Play();

        float scaleRate = crossFadeRate * _musicVolume;

        while(_activeMusic.volume > 0)
        {
            _activeMusic.volume -= scaleRate * Time.deltaTime;
            _inactiveMusic.volume += scaleRate * Time.deltaTime;

            yield return null;
        }

        AudioSource temp = _activeMusic;

        _activeMusic = _inactiveMusic;
        _activeMusic.volume = _musicVolume = musicVolume;

        _inactiveMusic = temp;
        _inactiveMusic.Stop();

        _crossFading = false;
    }

    public void StopMusic()
    {
        music1Source.Stop();
        _inactiveMusic.Stop();
    }

    public void UpdateDate(float soundVolume, float musicVolume, bool soundMute, bool musicMute)
    {
        this.soundVolume = soundVolume;
        this.musicVolume = musicVolume;
        this.soundMute = soundMute;
        this.musicMute = musicMute;
    }

    public void SaveData()
    {
        PlayerPrefs.SetFloat(parmSoundVolume, _soundVolume);
        PlayerPrefs.SetFloat(parmMusicVolume, musicVolume);
        PlayerPrefs.SetInt(parmSoundMute, soundMute ? 1 : 0);
        PlayerPrefs.SetInt(parmMusicMute, musicMute ? 1 : 0);
        //Debug.Log("Save setting audio: " + soundMute + "/" + musicMute + "/" + _soundVolume + "/" + musicVolume);
    }
}
