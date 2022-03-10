using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundInterface : MonoBehaviour
{
    [SerializeField] private Text musicPlay;
    [SerializeField] private Text soundPlay;
    [SerializeField] private Slider musicVolume;
    [SerializeField] private Slider soundVolume;
    private bool _musicMute, _soundMute;
    public void OnSoundValue(float value)
    {
        Managers.Audio.soundVolume = value;
    }

    public void OnMusicValue(float value)
    {
        Managers.Audio.musicVolume = value;
    }

    public void PlayMusic()
    {
        Managers.Audio.PlayLevelMusic();
    }
    
    public void OnMusic()
    {
        if (_musicMute)
        {
            Managers.Audio.musicMute = _musicMute;
            musicPlay.text = "Включить музыку";
        }
        else
        {
            Managers.Audio.musicMute = _musicMute;
            musicPlay.text = "Выключить музыку";
        }
        _musicMute = !_musicMute;
    }

    public void OnSound()
    {
        if (_soundMute)
        {
            Managers.Audio.soundMute = _soundMute;
            soundPlay.text = "Включить звук";
        }
        else
        {
            Managers.Audio.soundMute = _soundMute;
            soundPlay.text = "Выключить звук";
        }
        _soundMute = !_soundMute;
    }

    public void UpdateSetting()
    {
        _soundMute = Managers.Audio.soundMute;
        _musicMute = Managers.Audio.musicMute;
        OnMusic();
        OnSound();
        soundVolume.value = Managers.Audio.soundVolume;
        musicVolume.value = Managers.Audio.musicVolume;
        //Debug.Log("Load setting audio: " + _soundMute + "/" + _musicMute + "/" + soundVolume.value + "/" + musicVolume.value);
    }
}
