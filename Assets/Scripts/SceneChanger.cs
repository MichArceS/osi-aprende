using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public float timeShort;
    public string nameScene;

    void Start()
    {
        StartCoroutine(ChangeSceneAfterDelay(timeShort));
    }

    private IEnumerator ChangeSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(nameScene);
    }
    public void ChangeSceneAfterClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(nameScene);
    }
}
