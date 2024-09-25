using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioEscenaPorIndice : MonoBehaviour
{
    public int indiceEscena = 0;

    public void CambiarEscenaPorIndice()
    {
        SceneManager.LoadSceneAsync(indiceEscena);
    }
}
