using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager instance;

    public Dictionary<string, bool> escenasCompletadas = new Dictionary<string, bool>();
    private Dictionary<string, int> aciertosPorEscena = new Dictionary<string, int>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            escenasCompletadas.Add("Escena_Banio", false);
            escenasCompletadas.Add("Escena_Cocina", false);
            escenasCompletadas.Add("Escena_Comedor", false);
            escenasCompletadas.Add("Escena_Dormitorio", false);
            escenasCompletadas.Add("Escena_Sala", false);
            escenasCompletadas.Add("Escena_Patio", false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegistrarAciertoEscena(string nombreEscena, int totalAciertosRequeridos)
    {
        if (!aciertosPorEscena.ContainsKey(nombreEscena))
            aciertosPorEscena[nombreEscena] = 0;

        aciertosPorEscena[nombreEscena]++;
        Debug.Log($"Aciertos en {nombreEscena}: {aciertosPorEscena[nombreEscena]}/{totalAciertosRequeridos}");

        if (aciertosPorEscena[nombreEscena] >= totalAciertosRequeridos)
        {
            MarcarEscenaCompletada(nombreEscena);
        }
    }

    public void MarcarEscenaCompletada(string nombreEscena)
    {
        if (escenasCompletadas.ContainsKey(nombreEscena))
        {
            escenasCompletadas[nombreEscena] = true;
            Debug.Log("Escena completada: " + nombreEscena);

            if (TodasEscenasCompletadas())
            {
                Debug.Log("¡Todas las escenas completadas! Cargando recompensa...");
                SceneManager.LoadScene("Escena_Recompensa_Casa");
            }
            else
            {
                SceneManager.LoadScene("juegos_Casa");
            }
        }
        else
        {
            Debug.LogWarning("El nombre de la escena no está registrado: " + nombreEscena);
        }
    }

    public bool EstaEscenaCompletada(string nombreEscena)
    {
        return escenasCompletadas.ContainsKey(nombreEscena) && escenasCompletadas[nombreEscena];
    }

    private bool TodasEscenasCompletadas()
    {
        foreach (bool completada in escenasCompletadas.Values)
        {
            if (!completada) return false;
        }
        return true;
    }
}
