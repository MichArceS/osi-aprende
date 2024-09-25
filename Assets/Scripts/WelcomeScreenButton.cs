using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WelcomeScreenButton : MonoBehaviour
{
    public string pantallaAvatar;
    public string pantallaNuevoJuego;

    public void OnButtonClick()
    {
        // Verificar si es la primera vez que juega
        if (PlayerPrefs.GetInt("FirstTime", 1) == 1 && !(PlayerPrefs.HasKey("SelectedAvatarImageName") && PlayerPrefs.HasKey("playerName")))
        {
            Debug.Log("Es la primera vez que juega.");
            SceneManager.LoadSceneAsync(pantallaAvatar);
        }
        else
        {
            Debug.Log("No es la primera vez que juega.");

            SceneManager.LoadSceneAsync(pantallaNuevoJuego);

            // Limpiar el valor de "FirstTime" si ya no es la primera vez
            //PlayerPrefs.DeleteKey("FirstTime");
        }
    }

}
