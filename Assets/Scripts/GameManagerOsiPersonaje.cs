using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerOsiPersonaje : MonoBehaviour
{
    public static GameManagerOsiPersonaje Instance { get; private set; }

    public int score; // Contador que se mantendr� entre escenas

    private void Awake()
    {
        // Si ya existe otra instancia de GameManagerOsiPersonaje, destr�yela
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantiene este objeto entre escenas
        }
    }

    public void IncrementScore()
    {
        score++;
        Debug.Log("Puntuaci�n actual: " + score);
    }
}
