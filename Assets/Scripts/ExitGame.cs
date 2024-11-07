using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitGame : MonoBehaviour
{
    public Button exitButton;
    public AudioSource audioSource;

    void Start()
    {
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }
    }

    void OnExitButtonClicked()
    {
        // Reproduce el sonido
        if (audioSource != null)
        {
            audioSource.Play();
            // Espera a que termine el audio para salir
            Invoke("QuitGame", audioSource.clip.length);
        }
    }

    void QuitGame()
    {
        // Para editor
        #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}
