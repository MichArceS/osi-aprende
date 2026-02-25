using UnityEngine;
using System.Collections;

public class WaitAnimation : MonoBehaviour
{
    [Header("Configuración")]
    public float tiempoDeEspera = 2.0f;
    public GameObject ventanaSalir;

    void Start()
    {
        ventanaSalir.SetActive(false);
        StartCoroutine(EsperarYEcutar());
    }

    IEnumerator EsperarYEcutar()
    {
        yield return new WaitForSeconds(tiempoDeEspera);

        MiLogicaFinal();
    }

    void MiLogicaFinal()
    {
        ventanaSalir.SetActive(true);
        Destroy(GameProgressManager.instance);
    }
}
