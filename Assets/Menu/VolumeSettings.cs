using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider Mainslider;
    [SerializeField] private Slider Musiclider;
    [SerializeField] private Slider SFXslider;

    private List<AudioSource> musicSources = new List<AudioSource>();
    private List<AudioSource> sfxSources = new List<AudioSource>();

    void Start()
    {
        if (audioMixer == null)
        {
            Debug.LogError("AudioMixer not assigned in VolumeSettings script.");
            return;
        }

        // Automatycznie wykryj AudioSource
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource source in allAudioSources)
        {
            if (source.outputAudioMixerGroup != null && source.outputAudioMixerGroup.name == "Music")
            {
                musicSources.Add(source);
            }
            else if (source.outputAudioMixerGroup != null && source.outputAudioMixerGroup.name == "SFX")
            {
                sfxSources.Add(source);
            }
        }

        // Za³aduj zapisane ustawienia g³oœnoœci
        if (PlayerPrefs.HasKey("mainvolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMainVolume();
            SetMusicVolume();
            SetSFXVolume();
        }

        // Dodaj listener do suwaków
        Mainslider.onValueChanged.AddListener(delegate { SetMainVolume(); });
        Musiclider.onValueChanged.AddListener(delegate { SetMusicVolume(); });
        SFXslider.onValueChanged.AddListener(delegate { SetSFXVolume(); });
    }

    public void SetMainVolume()
    {
        if (audioMixer == null) return;
        float volume = Mainslider.value;
        audioMixer.SetFloat("MainVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("mainvolume", volume);
    }

    public void SetMusicVolume()
    {
        if (audioMixer == null) return;
        float volume = Musiclider.value;
        foreach (AudioSource source in musicSources)
        {
            source.volume = volume;
        }
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicvolume", volume);
    }

    public void SetSFXVolume()
    {
        if (audioMixer == null) return;
        float volume = SFXslider.value;
        foreach (AudioSource source in sfxSources)
        {
            source.volume = volume;
        }
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXvolume", volume);
    }

    private void LoadVolume()
    {
        if (audioMixer == null) return;
        Mainslider.value = PlayerPrefs.GetFloat("mainvolume");
        Musiclider.value = PlayerPrefs.GetFloat("musicvolume");
        SFXslider.value = PlayerPrefs.GetFloat("SFXvolume");
        SetMainVolume();
        SetMusicVolume();
        SetSFXVolume();
    }
}