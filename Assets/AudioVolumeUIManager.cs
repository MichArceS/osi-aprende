using UnityEngine;
using UnityEngine.UI;

public class AudioVolumeUIManager : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider voiceSlider;

    private void Start()
    {
        musicSlider.maxValue = 0.5f;
        sfxSlider.maxValue = 0.5f;
        voiceSlider.maxValue = 0.5f;
        // Link the Sliders to the AudioManager
        musicSlider.value = AudioManager.instance.musicVolume;
        sfxSlider.value = AudioManager.instance.sfxVolume;
        voiceSlider.value = AudioManager.instance.voiceVolume;

        // Add event listeners to update the AudioManager when Slider values change
        musicSlider.onValueChanged.AddListener(AudioManager.instance.SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(AudioManager.instance.SetSFXVolume);
        voiceSlider.onValueChanged.AddListener(AudioManager.instance.SetVoiceVolume);
    }
}