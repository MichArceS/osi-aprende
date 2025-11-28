using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using TMPro;


public class JsonIntroController : MonoBehaviour
{
    public GameObject confirmPanel;
    public TMP_InputField schoolField;
    public TMP_InputField scenaryField;
    private string mainInfoString = "mainInfo.data";
    private string appInfoPath;
    private int count;

    private void Awake()
    {
        appInfoPath = Application.persistentDataPath + "/" + mainInfoString;
        if (File.Exists(appInfoPath))
        {
            FileStream f = File.Open(appInfoPath, FileMode.Open);
            BinaryFormatter b = new BinaryFormatter();
            Tuple<string, string> schoolFields = (Tuple<string, string>)b.Deserialize(f);
            f.Close();
            schoolField.text = schoolFields.Item1;
            scenaryField.text = schoolFields.Item2;
        }
    }

    public void countMIDI()
    {
        this.count++;
        if (count >= 10)
        {
            count = 0;
            confirmPanel.gameObject.SetActive(true);
        }
    }

    public void acceptConfiguration()
    {
        confirmPanel.gameObject.SetActive(false);
        GameStateManager.Instance.SaveGlobalSettings(new GlobalSettings(schoolField.text, scenaryField.text));
        FileStream fs = new FileStream(appInfoPath, FileMode.Create);
        BinaryFormatter b = new BinaryFormatter();
        Tuple<string, string> schoolFields = new Tuple<string, string>(schoolField.text, scenaryField.text);
        b.Serialize(fs, schoolFields);
        fs.Close();
    }

    public void cancelConfiguration()
    {
        count = 0;
        confirmPanel.gameObject.SetActive(false);
    }

    void Start()
    {
        SessionManager.Instance.fecha_inicio_saludo = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
