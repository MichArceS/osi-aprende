using UnityEngine;
using UnityEngine.UI;
using System;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource voiceSource;

    [Header("Volume Sliders (UI)")]
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider voiceVolumeSlider;

    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";
    private const string VOICE_VOLUME_KEY = "VoiceVolume";

    public static event Action<float> OnMusicVolumeChanged;
    public static event Action<float> OnSfxVolumeChanged;
    public static event Action<float> OnVoiceVolumeChanged;

    public GameObject background;
    public GameObject panel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        //SetAudioUI();
        //InitializeVolumeSettings();
        //AddSliderListeners();
    }

    public void SetAudioUI()
    {
        background = GameObject.FindGameObjectWithTag("BackgroundAudio");
        panel = GameObject.FindGameObjectWithTag("PanelAudio");
        musicVolumeSlider = GameObject.FindGameObjectWithTag("SliderMusic").GetComponent<Slider>();
        sfxVolumeSlider = GameObject.FindGameObjectWithTag("SliderSFX").GetComponent<Slider>();
        voiceVolumeSlider = GameObject.FindGameObjectWithTag("SliderVoice").GetComponent<Slider>();

        background.SetActive(false);
        panel.SetActive(false);
    }

    public void InitializeVolumeSettings()
    {
        musicSource.volume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 0.75f);
        sfxSource.volume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 0.75f);
        voiceSource.volume = PlayerPrefs.GetFloat(VOICE_VOLUME_KEY, 0.75f);

        if (musicVolumeSlider != null) musicVolumeSlider.value = musicSource.volume;
        if (sfxVolumeSlider != null) sfxVolumeSlider.value = sfxSource.volume;
        if (voiceVolumeSlider != null) voiceVolumeSlider.value = voiceSource.volume;
    }

    public void AddSliderListeners()
    {
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        }
        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.onValueChanged.AddListener(SetSfxVolume);
        }
        if (voiceVolumeSlider != null)
        {
            voiceVolumeSlider.onValueChanged.AddListener(SetVoiceVolume);
        }
    }


    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
        OnMusicVolumeChanged?.Invoke(volume);
    }

    public void SetSfxVolume(float volume)
    {
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);
        OnSfxVolumeChanged?.Invoke(volume);
    }

    public void SetVoiceVolume(float volume)
    {
        voiceSource.volume = volume;
        PlayerPrefs.SetFloat(VOICE_VOLUME_KEY, volume);
        OnVoiceVolumeChanged?.Invoke(volume);
    }


    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (musicSource.clip == clip && musicSource.isPlaying && musicSource.loop == loop) return;
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void PlaySfx(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayVoice(AudioClip clip)
    {
        voiceSource.clip = clip;
        voiceSource.Play();
    }

    public AudioSource GetSFXSource()
    {
        return sfxSource;
    }

    public AudioSource GetVoiceSource()
    {
        return voiceSource;
    }

    public AudioClip GetVoice()
    {
        return voiceSource.clip;
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void StopVoice()
    {
        voiceSource.Stop();
    }

    public void ReplayVoice()
    {
        voiceSource.Stop();
        voiceSource.Play();
    }

    public void StopAllAudio()
    {
        musicSource.Stop();
        sfxSource.Stop();
        voiceSource.Stop();
    }

    public void HidePanel(bool b)
    {
        panel.SetActive(b);
        background.SetActive(b);
    }
}
