using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderVolume : MonoBehaviour
{
    [SerializeField] private string volumeName;
    [SerializeField] Slider volumeSlider;
    
    private void Awake()
    {
        volumeSlider.value = PlayerPrefs.GetFloat(volumeName);
    }

    private void Update()
    {
        UpdateVolume();
    }
    private void UpdateVolume()
    {
        float volumeValue = volumeSlider.value;
        volumeValue = volumeSlider.value;
        if(volumeName == "soundVolume"){
            SoundManager.instance.SetSoundVolume(volumeValue);}
        else{
            SoundManager.instance.SetMusicVolume(volumeValue);
        }
         PlayerPrefs.SetFloat(volumeName, volumeValue);
    }


}
