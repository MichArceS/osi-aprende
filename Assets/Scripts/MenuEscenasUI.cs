using UnityEngine;

public class MenuEscenasUI : MonoBehaviour
{
    public GameObject checkBanio;
    public GameObject checkCocina;
    public GameObject checkComedor;
    public GameObject checkDormitorio;
    public GameObject checkSala;
    public GameObject checkPatio;

    void Start()
    {
        if (GameProgressManager.instance != null)
        {
            if (GameProgressManager.instance.EstaEscenaCompletada("Escena_Banio"))
                checkBanio.SetActive(true);
            if (GameProgressManager.instance.EstaEscenaCompletada("Escena_Cocina"))
                checkCocina.SetActive(true);
            if (GameProgressManager.instance.EstaEscenaCompletada("Escena_Comedor"))
                checkComedor.SetActive(true);
            if (GameProgressManager.instance.EstaEscenaCompletada("Escena_Dormitorio"))
                checkDormitorio.SetActive(true);
            if (GameProgressManager.instance.EstaEscenaCompletada("Escena_Sala"))
                checkSala.SetActive(true);
            if (GameProgressManager.instance.EstaEscenaCompletada("Escena_Patio"))
                checkPatio.SetActive(true);
        }
    }
}
