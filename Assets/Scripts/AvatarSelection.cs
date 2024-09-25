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
        // Desactivar el bot�n
        readyButton.gameObject.SetActive(false);
        // Asignar funci�n de manejo de clic a cada bot�n de avatar
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
            // Aqu� podr�as quitar cualquier efecto de resaltado visual del bot�n previamente seleccionado si lo hubiera
        }

        // Obtener el nombre del avatar seleccionado (nombre del sprite asociado al bot�n de avatar)
        selectedAvatarName = avatarButtons[index].GetComponent<Image>().sprite.name;

        // Guardar referencia al nuevo bot�n seleccionado
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

            // Confirmaci�n de que se ha guardado correctamente (opcional)
            Debug.Log("Avatar seleccionado y guardado: " + selectedAvatarName);

            // Cargar la siguiente escena
            SceneManager.LoadSceneAsync(nextSceneName);
        }
        else
        {
            Debug.LogWarning("Ning�n avatar seleccionado.");
        }
    }
}
