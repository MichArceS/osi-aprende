using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneController : MonoBehaviour
{
    public AudioClip music;
    public AudioClip initVoice;
    public AudioClip buttonEffect;

    void Start()
    {
        AudioController.Instance.PlayMusic(music);
        AudioController.Instance.PlayVoice(initVoice);
    }

    public void PlayButtonEffect()
    {
        AudioController.Instance.PlaySfx(buttonEffect);
    }

    public void HideAudioManage(bool b)
    {
        AudioController.Instance.HidePanel(b);
    }
}
