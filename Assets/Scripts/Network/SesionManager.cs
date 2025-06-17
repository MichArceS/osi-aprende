using UnityEngine;
using System.Net;
using System.Text;
using System.IO;

public class SessionManager : MonoBehaviour
{
    public static SessionManager Instance;

    private string playerName;
    private string playerAvatar;

    // URL de Producción
    private const string SERVER_URL = "http://midiapi.espol.edu.ec";
    private const string API_URL = SERVER_URL + "/api/v1/entrance/AlmacenarDatosController";

    // URL de Desarrollo
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

    public void SetPlayerInfo(string avatar, string name)
    {
        playerAvatar = avatar;
        playerName = name;

        SendSessionData();
    }

    private void SendSessionData()
    {
        SessionData session = new SessionData()
        {
            Avatar = playerAvatar,
            Nombre = playerName
        };

        string json = JsonUtility.ToJson(session);

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
                    Debug.Log("Sesión enviada exitosamente.");
                }
            }
        }
        catch (WebException e)
        {
            Debug.LogError("Error enviando sesión: " + e.Message);
        }
    }
}

[System.Serializable]
public class SessionData
{
    public string Avatar;
    public string Nombre;
}
