using UnityEngine;
using System.Collections.Generic;

public class JuegoElectrodomesticos : MonoBehaviour
{
    [System.Serializable]
    public class ParCorrecto
    {
        public string nombreElectro;
        public string nombreObjeto;
    }

    public GameObject prefabLinea;
    public List<ParCorrecto> paresPosibles;

    public Dictionary<GameObject, string> electroAsignado = new Dictionary<GameObject, string>();
    public Dictionary<GameObject, string> objetoAsignado = new Dictionary<GameObject, string>();

    private List<GameObject> lineasCorrectas = new List<GameObject>();

    public bool EsRelacionCorrecta(GameObject electro, GameObject objeto)
    {
        string nE = electroAsignado[electro];
        string nO = objetoAsignado[objeto];

        foreach (var par in paresPosibles)
        {
            if (par.nombreElectro == nE && par.nombreObjeto == nO)
                return true;
        }
        return false;
    }

    public void RegistrarLineaCorrecta(LineRenderer linea)
    {
        lineasCorrectas.Add(linea.gameObject);
    }
}
