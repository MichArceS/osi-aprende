using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Text;
using System.IO;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    private List<string> localJsonData = new List<string>();

    // URL de Producción
    private const string SERVER_URL = "http://midiapi.espol.edu.ec";
    private const string API_URL = SERVER_URL + "/api/v1/entrance/AlmacenarDatosController";

    // Si quieres usar desarrollo, comenta las de arriba y descomenta estas
    // private const string SERVER_URL = "http://test.midiapi.espol.edu.ec";
    // private const string API_URL = SERVER_URL + "/api/v1/entrance/AlmacenarDatosController";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (localJsonData.Count > 0 && Application.internetReachability != NetworkReachability.NotReachable)
        {
            foreach (var json in new List<string>(localJsonData))
            {
                SendDataToServer(json);
            }
        }
    }

    public void AddJsonToList(string json)
    {
        localJsonData.Add(json);
    }

    private void SendDataToServer(string json)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_URL);
        request.Method = "POST";
        request.ContentType = "application/json";

        byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
        request.ContentLength = jsonBytes.Length;

        using (Stream dataStream = request.GetRequestStream())
        {
            dataStream.Write(jsonBytes, 0, jsonBytes.Length);
        }

        try
        {
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    localJsonData.Remove(json);
                }
            }
        }
        catch (WebException e)
        {
            Debug.LogError("Error enviando datos: " + e.Message);
        }
    }

    // Metodo público para que puedas mandar datos manualmente cuando quieras
    public void SendData()
    {
        if (localJsonData.Count > 0)
        {
            foreach (var json in new List<string>(localJsonData))
            {
                SendDataToServer(json);
            }
        }
    }
}
