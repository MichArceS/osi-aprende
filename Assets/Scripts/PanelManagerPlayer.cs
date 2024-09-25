using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PanelManagerPlayer : MonoBehaviour
{
    public GameObject[] panels;
    public GameObject finalPanel;
    public GameObject additionalPanel;
    public float delayBeforeNextPanel = 1f;
    public float delayBeforeAdditionalPanel = 2f;
    public string sceneOnBackButton;

    private List<int> availablePanelIndices;
    private Stack<int> panelHistory;
    private int currentPanelIndex = -1;
    private int[] aciertosPorPanel;
    private bool allPanelsCompleted = false;
    private AudioSource currentAudioSource;

    void Start()
    {
        // Inicializa el array de aciertos necesarios para cada panel
        aciertosPorPanel = new int[panels.Length];
        // Establece el número de aciertos necesarios para cada panel a 4
        for (int i = 0; i < aciertosPorPanel.Length; i++)
        {
            aciertosPorPanel[i] = 4;
        }

        InitializePanelIndices();
        panelHistory = new Stack<int>();
        ShowNextPanel();

        //Suscribiendo el evento OnAciertosChanged
        GlobalCounter.OnAciertosChanged += HandleAciertosChanged;
    }

    public void RepeatAudio()
    {
        if (currentAudioSource != null)
        {
            currentAudioSource.Stop();
            currentAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("No se encontró AudioSource actual para repetir.");
        }
    }

    void OnDestroy()
    {
        // Desuscripción del evento
        GlobalCounter.OnAciertosChanged -= HandleAciertosChanged;
    }

    // Inicializa la lista de índices de paneles disponibles
    private void InitializePanelIndices()
    {
        availablePanelIndices = new List<int>();

        for (int i = 0; i < panels.Length; i++)
        {
            availablePanelIndices.Add(i);
        }

        allPanelsCompleted = false; 
    }

    //Manejar audio 
    public void SetCurrentAudioSource(AudioSource audioSource)
    {
        currentAudioSource = audioSource;
    }

    private IEnumerator FindAudioSourceInPanel(int panelIndex)
    {
        yield return new WaitForSeconds(0.1f);

        AudioSource audioSource = panels[panelIndex].GetComponentInChildren<AudioSource>();

        if (audioSource != null)
        {
            SetCurrentAudioSource(audioSource);
            audioSource.Play();
        }
        else
        {
            Debug.Log($"No se encontró AudioSource en el panel {panelIndex}.");
        }
    }

    // Muestra el siguiente panel aleatorio sin repeticiones
    private void ShowNextPanel()
    {
        if (availablePanelIndices.Count == 0)
        {
            // Si todos los paneles han sido activados
            if (!allPanelsCompleted)
            {
                allPanelsCompleted = true;
                StartCoroutine(ActivateFinalPanelAndShowAdditionalPanel());

            }
            return;
        }

        // Desactiva todos los paneles
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }

        // Selecciona un índice aleatorio de los disponibles
        int randomIndex = Random.Range(0, availablePanelIndices.Count);
        int panelIndex = availablePanelIndices[randomIndex];

        // Activa el panel seleccionado
        panels[panelIndex].SetActive(true);

        // Actualiza el índice actual y elimina el índice seleccionado de la lista
        currentPanelIndex = panelIndex;
        availablePanelIndices.RemoveAt(randomIndex);

        panelHistory.Push(panelIndex);
        // Reinicia los contadores al cambiar de panel
        GlobalCounter.ResetCounters();
        StartCoroutine(FindAudioSourceInPanel(currentPanelIndex));
    }

    // Coroutine para activar el panel final y luego activar otro panel después de un retraso
    private IEnumerator ActivateFinalPanelAndShowAdditionalPanel()
    {
        finalPanel.SetActive(true); // Activa el panel final
        yield return new WaitForSeconds(delayBeforeAdditionalPanel); // Espera el tiempo especificado

        // Activa el panel adicional
        if (additionalPanel != null)
        {
            additionalPanel.SetActive(true);
        }
    }

    public void ShowPreviousPanel()
    {
        if (panelHistory.Count > 1)
        {
            panelHistory.Pop();
            int previousPanelIndex = panelHistory.Peek();

            ShowPanel(previousPanelIndex);

            ResetPanelObjects();
        }
        else
        {
            SceneManager.LoadSceneAsync(sceneOnBackButton);
        }
    }

    // Cambia al siguiente panel después de un retraso
    public void ShowNextPanelAfterDelay(float delay)
    {
        StartCoroutine(ChangePanelAfterDelay(delay));
    }

    // Coroutine para esperar un tiempo antes de cambiar al siguiente panel
    private IEnumerator ChangePanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowNextPanel();
    }

    private void HandleAciertosChanged()
    {
        Debug.Log("HandleAciertosChanged llamado. Aciertos Totales: " + GlobalCounter.ObtenerAciertosTotales());
        if (GlobalCounter.ObtenerAciertosTotales() >= aciertosPorPanel[currentPanelIndex])
        {
            Debug.Log("Número de aciertos alcanzado. Cambiando al siguiente panel.");
            ShowNextPanelAfterDelay(delayBeforeNextPanel);
        }
    }

    // Muestra el panel en el índice proporcionado y oculta los demás
    private void ShowPanel(int index)
    {
        if (index < 0 || index >= panels.Length)
        {
            Debug.LogWarning("Índice de panel fuera de rango.");
            return;
        }

        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == index);
        }

        currentPanelIndex = index;

        // Reinicia los contadores al cambiar de panel
        GlobalCounter.ResetCounters();

        StartCoroutine(FindAudioSourceInPanel(currentPanelIndex));
    }

    private void ResetPanelObjects()
    {
        // Asegúrate de que currentPanelIndex es válido
        if (currentPanelIndex < 0 || currentPanelIndex >= panels.Length)
        {
            Debug.LogWarning("Índice de panel fuera de rango.");
            return;
        }

        // Obtén el panel actual
        GameObject currentPanel = panels[currentPanelIndex];
        //Debug.Log($"ResetPanelObjects: Activando panel {currentPanelIndex}");

        // Busca el hijo llamado 'almario'
        Transform almarioTransform = currentPanel.transform.Find("Almario");
        if (almarioTransform == null)
        {
            Debug.LogWarning("No se encontró el hijo 'almario' en el panel.");
            return;
        }

        //Debug.Log("Encontrado el objeto 'almario'.");

        // Itera sobre todos los hijos del objeto 'almario'
        foreach (Transform child in almarioTransform)
        {
            // Verifica si el hijo tiene el tag "Draggable"
            if (child.CompareTag("Draggable"))
            {
                //Debug.Log($"Encontrado objeto arrastrable: {child.name}");
                // Obtén el componente ControllerDrag_and_Drop
                var draggable = child.GetComponent<ControllerDrag_and_Drop>();
                if (draggable != null)
                {
                    //Debug.Log($"Reiniciando objeto: {child.name}");
                    draggable.ResetState(); 
                }
                else
                {
                    Debug.LogWarning($"No se encontró el componente ControllerDrag_and_Drop en el objeto: {child.name}");
                }
            }
            
        }
    }

    // Reinicia la escena actual
    public void RestartScene()
    {
        // Obtiene el nombre de la escena actual
        string currentSceneName = SceneManager.GetActiveScene().name;
        // Vuelve a cargar la escena actual
        SceneManager.LoadScene(currentSceneName);
    }

    // Carga una nueva escena
    public void LoadNewScene()
    {
        if (!string.IsNullOrEmpty(sceneOnBackButton))
        {
            // Carga la nueva escena especificada en el campo 'sceneToLoadOnExit'
            SceneManager.LoadScene(sceneOnBackButton);
        }
    }
}