using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    
    public static SoundManager instance {  get; private set; }
    public static event Action<float> OnSoundVolumeChanged;
    private AudioSource source; //soundsource
    private AudioSource musicSource;
    

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        musicSource = transform.GetChild(0).GetComponent<AudioSource>();  
        
        source = GetComponent<AudioSource>();
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //destroy duplicate
        else if(instance != null && instance != this)
            Destroy(gameObject);

        ChangeMusicVolume(0);
        ChangeSoundVolume(0);
    }

    public void PlaySound(AudioClip _sound)
    {
        source.PlayOneShot(_sound);
    }

    public void ChangeSoundVolume(float _change)
    {
        float currentVolume = PlayerPrefs.GetFloat("soundVolume",0.5f); //load last saved sound volume from player prefs
        currentVolume += _change;

        if (currentVolume > 1)
            currentVolume = 0;
        else if (currentVolume < 0)
            currentVolume = 1;

        //assign final volume               
        source.volume = currentVolume;

        //save final value to player prefs
        PlayerPrefs.SetFloat("soundVolume", source.volume);
        OnSoundVolumeChanged?.Invoke(source.volume);
    }
    public void SetSoundVolume(float _change)
    {

        source.volume = Mathf.Clamp01(_change);

    // Değeri kaydet
        PlayerPrefs.SetFloat("soundVolume", source.volume);

    // Event'i tetikle
        OnSoundVolumeChanged?.Invoke(source.volume);
        
        
    }
    public void ChangeMusicVolume(float _change)
    {
        //base value
        float baseVolume = 0.3f;

        float currentVolume = PlayerPrefs.GetFloat("musicVolume",0.15f);
        currentVolume += _change;

        if (currentVolume > 1)
            currentVolume = 0;
        else if (currentVolume < 0)
            currentVolume = 1;
        //assign final volume  
        float finalVolume = currentVolume * baseVolume;
        musicSource.volume = finalVolume;

        //save final value to player prefs
        PlayerPrefs.SetFloat("musicVolume", currentVolume);
    }

    public void SetMusicVolume(float _change)
    {
        //base value
        float baseVolume = 0.3f;

        musicSource.volume = Mathf.Clamp01(_change) * baseVolume;

    // Değeri kaydet
        PlayerPrefs.SetFloat("musicVolume", _change);
    }




}
