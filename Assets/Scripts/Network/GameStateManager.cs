using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Net.Http;
using System.Net.Http.Headers;

public class GameStateManager : UnitySingleton<GameStateManager>
{
    private string fechaVersion = "25.1";
    private static readonly HttpClient client = new HttpClient();

    public bool netService;
    public string gameTitle = "Osi Aprende";

    // Entorno de Produccion
    private const string SERVER_URL = "http://midiapi.espol.edu.ec";
    private const string API_URL = SERVER_URL + "/api/v1/entrance/AlmacenarDatosController";

    // Entorno de desarrollo
    // private const string SERVER_URL = "http://localhost:1337";
    // private const string API_URL = SERVER_URL + "/api/v1/entrance/AlmacenarDatosController";
    
    private List<string> jsonList = new List<string>();
    private bool IsConnectedToServer = false;

    private string settingsFileName = "/globalSettings.dat";
    private string gameDataFileName = "/gamesave.dat";

    private GlobalSettings globalSettings;

    // Use this for initialization
    IEnumerator Start()
    {
        Debug.Log(Application.persistentDataPath);

        GameObject.DontDestroyOnLoad(this.gameObject);
        netService = true;

        if (netService)
        {
            Debug.Log(API_URL);
            if (CheckNet()) yield return StartCoroutine(CheckServer());
            Debug.Log(Application.persistentDataPath);
            ReadLocalFile();
            StartCoroutine(SyncJsonData());
        }

        LoadGlobalSettings();
    }

    private void LoadGlobalSettings()
    {
        if (File.Exists(Application.persistentDataPath + settingsFileName))
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + settingsFileName, FileMode.Open);
                GlobalSettings data = (GlobalSettings)bf.Deserialize(file);
                file.Close();

                this.globalSettings = new GlobalSettings(data.school, data.room);
            }
            catch (Exception)
            {
                Debug.Log("Configuraciones: ERROR al leer el archivo");
                SaveGlobalSettings(new GlobalSettings());
                throw;
            }
        }
        else
        {
            Debug.Log("Configuraciones: No existe archivo");
            Debug.Log("Configuraciones: Creando Archivo");
            SaveGlobalSettings(new GlobalSettings());
        }
    }


    public void SaveGlobalSettings(GlobalSettings settings)
    {
        this.globalSettings = settings;

        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + settingsFileName);

            bf.Serialize(file, settings);
            file.Close();
            Debug.Log("Configuraciones: Guardado con éxito");
        }
        catch (Exception)
        {
            Debug.Log("Configuraciones: ERROR al guardar el archivo");
            throw;
        }

    }

    public GlobalSettings GetGlobalSettings()
    {
        return globalSettings;
    }

    public int getCurrentSceneIndex()
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
    }

    public string getCurrentSceneName()
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }

    public void PrintJsonList()
    {
        foreach (string jsonItem in jsonList)
        {
            print("JSONList[" + jsonList.IndexOf(jsonItem) + "] = " + jsonItem);
        }
    }


    public bool CheckNet()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            return true;
        }
        Debug.Log("Error. Check internet connection!");
        return false;
    }

    IEnumerator CheckServer()
    {
        WWW www = new WWW(SERVER_URL);
        Debug.Log("CONNECTING TO SERVER...");
        yield return StartCoroutine("StartUpload", www);

        if (www.error != null)
        {
            Debug.Log("THIS IS AN ERROR: " + www.error);
            IsConnectedToServer = false;
        }
        else
        {
            Debug.Log("CONNECTED TO SERVER");
            IsConnectedToServer = true;
        }
    }

    public void PingFinished(Ping p)
    {
        print(p);
    }

    public void AddJsonToList(string json)
    {
        ReadLocalFile();
        jsonList.Add(json);
        StartCoroutine(SyncJsonData());
    }

    public IEnumerator SyncJsonData()
    {
        if (netService)
        {

            PrintJsonList();
            yield return CheckServer();

            if (CheckNet() && IsConnectedToServer)
            {

                Debug.Log("READY TO UPLOAD");
                List<string> sendedJsonItems = new List<string>();

                foreach (string jsonItem in jsonList)
                {

                    Dictionary<string, string> postHeader = new Dictionary<string, string>
                    {
                        { "Content-Type", "application/json" }
                    };
                    postHeader.Add("version", fechaVersion);
                    byte[] body = Encoding.UTF8.GetBytes(jsonItem);
                    WWW www = new WWW(API_URL, body, postHeader);
                    Debug.Log("CONNECTING TO SERVER...");
                    yield return StartCoroutine("StartUpload", www);

                    if (www.error != null)
                    {
                        Debug.Log("THIS IS AN ERROR: " + www.error);
                        IsConnectedToServer = false;

                    }
                    else
                    {
                        Debug.Log("Data Submitted");
                        sendedJsonItems.Add(jsonItem);
                    }
                }

                print(sendedJsonItems.Count + " FILE(S) UPLOADED");

                foreach (string sended in sendedJsonItems)
                {
                    jsonList.Remove(sended);
                }

                sendedJsonItems.Clear();

            }
            else
            {
                Debug.Log("CONNECTION NOT ESTABLISHED");
            }

            print(jsonList.Count + " JSON(S) NOT UPLOADED");


            SaveLocal();

        }
    }


    IEnumerator StartUpload(WWW www)
    {
        yield return www;

    }


    public void ReadLocalFile()
    {
        if (File.Exists(Application.persistentDataPath + gameDataFileName))
        {
            Debug.Log("SAVE DATA FOUND");
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + gameDataFileName, FileMode.Open);
                jsonList = (List<string>)bf.Deserialize(file);
                file.Close();
            }
            catch (System.Exception)
            {
                Debug.Log("ERROR READING FILE");
                throw;
            }

        }
        else
        {
            Debug.Log("READING - SAVE DATA NOT FOUND");
        }

    }

    public void SaveLocal()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + gameDataFileName);
            bf.Serialize(file, jsonList);
            file.Close();

            Debug.Log("Saved Locally");
        }
        catch (System.Exception)
        {
            Debug.Log("ERROR SAVING FILE");
            throw;
        }

    }

    public void DeleteSaveFile()
    {
        if (File.Exists(Application.persistentDataPath + gameDataFileName))
        {
            Debug.Log("SAVE DATA FOUND");
            try
            {
                File.Delete(Application.persistentDataPath + gameDataFileName);
                Debug.Log("SAVE DATA DELETED");
            }
            catch (System.Exception)
            {
                Debug.Log("ERROR DELETING DATA");
                throw;
            }
        }
        else
        {
            Debug.Log("DELETING - SAVE DATA NOT FOUND");
        }

    }
}

