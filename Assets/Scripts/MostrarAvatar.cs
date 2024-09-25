using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MostrarAvatar : MonoBehaviour
{
    public Image imagenAvatar;

    void Start()
    {
        // Recuperar el nombre de la imagen del avatar seleccionado
        string avatarImageName = PlayerPrefs.GetString("SelectedAvatarImageName", "");

        if (!string.IsNullOrEmpty(avatarImageName))
        {
            // Cargar la imagen del avatar usando su nombre
            Sprite avatarSprite = Resources.Load<Sprite>("Avatares/" + avatarImageName); // Suponiendo que las im�genes de los avatares est�n en la carpeta Resources/Avatares
            if (avatarSprite != null)
            {
                imagenAvatar.sprite = avatarSprite;
            }
            else
            {
                Debug.LogWarning("No se encontr� la imagen del avatar: " + avatarImageName);
            }
        }
        else
        {
            Debug.LogWarning("No se ha seleccionado ning�n avatar.");
        }
    }
}