using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class PlayerRegistration : MonoBehaviour
{
    public TMP_InputField playerNameInput;
    public Button saveButton;
    public string nextSceneName;

    private PlayerData playerData;

    private void Start()
    {
        //desactivar el boton
        saveButton.gameObject.SetActive(false);
        // Cargar datos de jugadores al iniciar
        LoadPlayersData();

        // Añadir listener al input field para verificar cambios en el texto
        playerNameInput.onValueChanged.AddListener(OnPlayerNameInputChanged);

        //Llamar a la funcion reiniciar Todo
        //reiniciarTodo();

        saveButton.onClick.AddListener(SavePlayerData);
    }

    private void OnPlayerNameInputChanged(string inputText)
    {
        // Mostrar u ocultar el botón en función de la longitud del texto
        if (inputText.Length >= 3)
        {
            saveButton.gameObject.SetActive(true);
        }
        else
        {
            saveButton.gameObject.SetActive(false);
        }
    }

    private void SavePlayerData()
    {
        string playerName = playerNameInput.text.Trim();
        string selectedAvatarName = PlayerPrefs.GetString("SelectedAvatarImageName", "");

        // Verificar si playerData está inicializado
        if (playerData == null)
        {
            Debug.LogError("Algo paso.");
            return;
        }

        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogWarning("Por favor ingresar un nombre antes de seguir");
            return;
        }

        // Verificar si el jugador ya existe en la lista
        if (IsPlayerDuplicate(playerName, selectedAvatarName))
        {
            Debug.LogWarning("El jugador con el mismo nombre y avatar ya existe.");
            return;
        }

        //crear el jugador
        PlayerInfo newPlayer = new PlayerInfo(playerName, selectedAvatarName);

        // cargar informacion
        playerData.players.Add(newPlayer);

        SavePlayersData();

        // Actualizar interfaz de usuario
        //infoText.text = "Jugador registrado exitosamente: " + playerName;
        playerNameInput.text = "";
        PlayerPrefs.SetString("playerName", playerName);
        // Cargar la siguiente escena
        SceneManager.LoadSceneAsync(nextSceneName);
        //modifaca la seccion
        PlayerPrefs.SetInt("FirstTime", 0);
        PlayerPrefs.Save();
    }

    private void SavePlayersData()
    {
        string playersDataPath = Path.Combine(Application.persistentDataPath, "players.json");
        string jsonData = JsonUtility.ToJson(playerData);
        File.WriteAllText(playersDataPath, jsonData);

        Debug.Log("url: " + playersDataPath);
    }

    private bool IsPlayerDuplicate(string playerName, string selectedAvatarName)
    {
        foreach (PlayerInfo player in playerData.players)
        {
            if (player.playerName == playerName && player.selectedAvatarName == selectedAvatarName)
            {
                return true;
            }
        }
        return false;
    }

    private void LoadPlayersData()
    {
        string playersDataPath = Path.Combine(Application.persistentDataPath, "players.json");

        if (File.Exists(playersDataPath))
        {
            string jsonData = File.ReadAllText(playersDataPath);
            playerData = JsonUtility.FromJson<PlayerData>(jsonData);
        }
        else
        {
            playerData = new PlayerData();
        }
    }

    private void reiniciarTodo()
    {

        if (PlayerPrefs.HasKey("playerName"))
        {
            PlayerPrefs.DeleteKey("playerName");
        }
        if (PlayerPrefs.HasKey("SelectedAvatarImageName"))
        {
            PlayerPrefs.DeleteKey("SelectedAvatarImageName");
        }
        if (PlayerPrefs.HasKey("PlayerCodeContinue"))
        {
            PlayerPrefs.DeleteKey("PlayerCodeContinue");
        }
        // Construir la ruta completa al archivo players.json en el directorio persistente de datos de la aplicación
        string playersDataPath = Path.Combine(Application.persistentDataPath, "players.json");

        // Verificar si el archivo existe antes de intentar eliminarlo
        if (File.Exists(playersDataPath))
        {
            // Eliminar el archivo
            File.Delete(playersDataPath);
            Debug.Log("Archivo players.json eliminado correctamente.");
        }
        else
        {
            Debug.Log("El archivo players.json no existe en la ruta especificada.");
        }
        PlayerPrefs.SetInt("FirstTime", 1);
        PlayerPrefs.Save();
        PlayerInfo.ResetNextId();
        Debug.Log("Reiniciado_Todo");
    }
}
