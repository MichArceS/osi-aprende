using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class GetDataPlayers : MonoBehaviour
{
    public GameObject prefabJugador;
    public Transform contenedorJugadores;
    public string nombreEscenaACargar;


    private PlayerData playerData;
    public string playerName;
    public string selectedAvatarName;

    // Start is called before the first frame update
    void Start()
    {
        SessionManager.Instance.fecha_inicio_nombre = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        LoadPlayersData();
    }

    private void LoadPlayersData()
    {
        string playersDataPath = Path.Combine(Application.persistentDataPath, "players.json");

        if (File.Exists(playersDataPath))
        {
            string jsonData = File.ReadAllText(playersDataPath);
            playerData = JsonUtility.FromJson<PlayerData>(jsonData);

            if (playerData == null || playerData.players == null || playerData.players.Count == 0)
            {
                Debug.LogWarning("El archivo players.json está vacío o no contiene datos.");
            }
            else
            {
                //Debug.Log("Los datos de los jugadores se están cargando correctamente.");

                foreach (Transform child in contenedorJugadores)
                {
                    Destroy(child.gameObject);
                }

                // Iterar sobre cada jugador en playerData.players
                foreach (var jugador in playerData.players)
                {
                    // Instanciar el prefab del jugador
                    GameObject nuevoJugador = Instantiate(prefabJugador, contenedorJugadores);

                    // Obtener referencia al componente Image donde se mostrará el avatar
                    Image imgAvatar = nuevoJugador.transform.Find("Image").GetComponent<Image>();

                    // Obtener referencia al componente TextMeshPro para el nombre del jugador
                    TMP_Text nombreText = nuevoJugador.GetComponentInChildren<TMP_Text>();
                    Button boton = nuevoJugador.GetComponent<Button>();

                    // Asignar los datos del jugador a los componentes de TextMeshPro e Image
                    nombreText.text = jugador.playerName; // Asignar nombre del jugador
                    imgAvatar.sprite = Resources.Load<Sprite>("Avatares/" + jugador.selectedAvatarName);

                    playerName = nombreText.text;
                    selectedAvatarName = imgAvatar.sprite.name;

                    // Asignar el listener al botón para guardar el nombre en PlayerPrefs y cargar la escena
                    boton.onClick.AddListener(() =>
                    {
                        SavePlayerName(jugador.playerCode);
                        UnityEngine.SceneManagement.SceneManager.LoadScene(nombreEscenaACargar);
                    });
                }
            }
        }
        else
        {
            Debug.LogWarning("No se encontró el archivo players.json en la ruta: " + playersDataPath);
        }
    }

    private void SavePlayerName(string playerCode)
    {
        // Guardar el nombre del jugador en PlayerPrefs
        PlayerPrefs.SetString("PlayerCodeContinue", playerCode);
        SessionManager.Instance.fecha_fin_nombre = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        SessionManager.Instance.SetPlayerInfo(selectedAvatarName, playerName, GameStateManager.Instance.gameTitle);
        Debug.Log("Codigo del jugador en continuar: " + playerCode);
    }
}
