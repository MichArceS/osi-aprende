using UnityEngine;

public class CheckMarkManager : MonoBehaviour
{
    public GameObject[] checks;

    void Start()
    {
        // Activar checks guardados en PlayerPrefs
        for (int i = 0; i < checks.Length; i++)
        {
            if (PlayerPrefs.GetInt("Check_" + i, 0) == 1)
            {
                checks[i].SetActive(true);
            }
            else
            {
                checks[i].SetActive(false); // ← para evitar que quede marcado si reinicias
            }
        }
    }

    public void ActivarCheck(int index)
    {
        if (index >= 0 && index < checks.Length)
        {
            checks[index].SetActive(true);
            PlayerPrefs.SetInt("Check_" + index, 1); // Guardar para que persista
            PlayerPrefs.Save();
        }
    }

    // Si quieres resetear (por ejemplo al salir del juego)
    public void ResetearChecks()
    {
        for (int i = 0; i < checks.Length; i++)
        {
            PlayerPrefs.SetInt("Check_" + i, 0);
            checks[i].SetActive(false);
        }

        PlayerPrefs.Save();
    }
}
