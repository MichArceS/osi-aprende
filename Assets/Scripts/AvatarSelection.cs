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

    private Button selectedButton;   
    private string selectedAvatarName;  

    void Start()
    {
        // Desactivar el botón
        readyButton.gameObject.SetActive(false);
        // Asignar función de manejo de clic a cada botón de avatar
        for (int i = 0; i < avatarButtons.Length; i++)
        {
            int index = i;
            avatarButtons[i].onClick.AddListener(() => SelectAvatar(index));
        }

        readyButton.onClick.AddListener(SaveAndLoadNextScene);

    }

    void SelectAvatar(int index)
    {
    
        if (selectedButton != null)
        {
            // Aquí podrías quitar cualquier efecto de resaltado visual del botón previamente seleccionado si lo hubiera
        }

        // Obtener el nombre del avatar seleccionado (nombre del sprite asociado al botón de avatar)
        selectedAvatarName = avatarButtons[index].GetComponent<Image>().sprite.name;

        // Guardar referencia al nuevo botón seleccionado
        selectedButton = avatarButtons[index];
        readyButton.gameObject.SetActive(true);

        //Debug.Log("Avatar seleccionado: " + selectedAvatarName);
    }

    void SaveAndLoadNextScene()
    {
        if (selectedButton != null)
        {
            // Guardar el nombre del avatar seleccionado en PlayerPrefs
            PlayerPrefs.SetString("SelectedAvatarImageName", selectedAvatarName);

            // Confirmación de que se ha guardado correctamente (opcional)
            Debug.Log("Avatar seleccionado y guardado: " + selectedAvatarName);

            // Cargar la siguiente escena
            SceneManager.LoadSceneAsync(nextSceneName);
        }
        else
        {
            Debug.LogWarning("Ningún avatar seleccionado.");
        }
    }
}
