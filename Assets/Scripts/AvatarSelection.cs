using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AvatarSelection : MonoBehaviour
{
    public Button[] avatarButtons;  
    public Button readyButton;      
    public string nextSceneName;
    public AudioSource audioSource;

    private Button selectedButton;   
    private string selectedAvatarName;  

    void Start()
    {
        readyButton.gameObject.SetActive(false);
        for (int i = 0; i < avatarButtons.Length; i++)
        {
            int index = i;
            avatarButtons[i].onClick.AddListener(() => SelectAvatar(index));
        }
        readyButton.onClick.AddListener(SaveAndLoadNextScene);
    }

    void SelectAvatar(int index)
    {
        selectedAvatarName = avatarButtons[index].GetComponent<Image>().sprite.name;

        selectedButton = avatarButtons[index];
        readyButton.gameObject.SetActive(true);
    }

    void SaveAndLoadNextScene()
    {
        if (selectedButton != null)
        {
            PlayerPrefs.SetString("SelectedAvatarImageName", selectedAvatarName);
            StartCoroutine(PlayAudioAndLoadScene(nextSceneName));
        }
        else
        {
            Debug.LogWarning("Ning�n avatar seleccionado.");
        }
    }

    IEnumerator PlayAudioAndLoadScene(string sceneToLoad)
    {
        yield return new WaitForSeconds(0.1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
    }
}
