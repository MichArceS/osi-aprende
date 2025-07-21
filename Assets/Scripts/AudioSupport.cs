using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSupport : MonoBehaviour
{
    void Start()
    {
        AudioController.Instance.SetAudioUI();
        AudioController.Instance.InitializeVolumeSettings();
        AudioController.Instance.AddSliderListeners();
    }
}
