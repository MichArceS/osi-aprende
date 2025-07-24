using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public void OnChangeScene(string sceneToLoad)
    {
        StartCoroutine(LoadScene(sceneToLoad));
    }

    IEnumerator LoadScene(string sceneToLoad)
    {
        yield return new WaitForSeconds(0.1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
    }

    public void AudioAndLoadScene(string sceneToLoad)
    {
        StartCoroutine(PlayAudioAndLoadScene(sceneToLoad));
    }

    IEnumerator PlayAudioAndLoadScene(string sceneToLoad)
    {
        yield return new WaitForSeconds(AudioController.Instance.GetVoice().length + 0.1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
    }
}
