using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityButton : MonoBehaviour
{
    public SceneManager sceneManager;
    public AudioClip audioClip;

    public void OnButtonPlay(string sceneName)
    {
        AudioController.Instance.PlayVoice(audioClip);
        sceneManager.AudioAndLoadScene(sceneName);
    }
}
