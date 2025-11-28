using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class PanelManagerPlayerLugares : MonoBehaviour
{
    public GameObject[] panels;
    public AudioClip[] audioClipPanels;

    public GameObject[] panelsContent;

    public GameObject[] panelsRecompensas;
    public AudioClip audioClipRecompensa;

    public GameObject additionalPanel;
    public float delayBeforeNextPanel = 5f;
    public float rewardPanelDuration = 3f;
    public string sceneOnBackButton;

    public AudioClip buttonClickAudio;

    private List<int> availablePanelIndices;
    private Stack<int> panelHistory;
    private int currentPanelIndex = -1;
    private int[] aciertosPorPanel;
    private bool allPanelsCompleted = false;
    
    public AudioClip[] audiosLugares;
    public AudioClip correctAudio;
    public AudioClip correctSFX;
    public AudioClip incorrectSFX;

    public RuntimeAnimatorController correctAnimator;
    public RuntimeAnimatorController wrongAnimator;

    public Sprite[] optionImages;

    public LevelMetaData levelData;
    private bool isPlayingAudio = false;

    void Start()
    {
        levelData = new LevelMetaData(SessionManager.Instance.nombre_jugador, "Nivel Lugares Publicos", "Nivel Lugares Publicos Descripcion", "Lugares Publicos", "Lugares Publicos Historia", "Lugares Publicos Descripcion");
        aciertosPorPanel = new int[panels.Length];
        for (int i = 0; i < aciertosPorPanel.Length; i++)
        {
            aciertosPorPanel[i] = 1;
        }
        InitializePanelIndices();
        panelHistory = new Stack<int>();
        ShowNextPanel();
    }

    public void PlayButtonClickAudio()
    {
        AudioController.Instance.PlaySfx(buttonClickAudio);
    }

    public void RepeatAudio()
    {
        PlayButtonClickAudio();
        AudioController.Instance.ReplayVoice();
    }

    private void InitializePanelIndices()
    {
        availablePanelIndices = new List<int>();
        for (int i = 0; i < panels.Length; i++)
        {
            availablePanelIndices.Add(i);
        }
        allPanelsCompleted = false;
    }

    private IEnumerator FindAudioSourceInPanel(int panelIndex)
    {
        yield return new WaitForSeconds(0.1f);
        AudioClip audioClip = audioClipPanels[panelIndex];
        AudioController.Instance.PlayVoice(audioClip);
    }

    public void PlayCurrentAudio()
    {
        PlayButtonClickAudio();
        AudioController.Instance.ReplayVoice();
    }


    private void ShowNextPanel()
    {
        if (availablePanelIndices.Count == 0)
        {
            if (!allPanelsCompleted)
            {
                allPanelsCompleted = true;
                Debug.Log("ENDING");
                Debug.Log(panelsRecompensas[0]);
                StartCoroutine(ShowRewardPanel(0));
            }
            return;
        }

        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }

        int randomIndex = Random.Range(0, availablePanelIndices.Count);
        int panelIndex = availablePanelIndices[randomIndex];
        panels[panelIndex].SetActive(true);
        int correctAnswer = panelIndex; // Assuming the correct answer is based on the panel index
        Sprite correctImage = optionImages[correctAnswer];
        AudioClip audioCorrect = audiosLugares[correctAnswer];
        Transform itemsParent = panelsContent[panelIndex].transform;
        if (itemsParent != null)
        {
            List<Transform> items = new List<Transform>();
            foreach (Transform item in itemsParent)
            {
                items.Add(item);
            }

            // Load all images into each item image component
            for (int i = 0; i < items.Count; i++)
            {
                Image itemImage = items[i].GetComponent<Image>();
                if (itemImage != null)
                {
                    itemImage.sprite = optionImages[i];
                }
            }

            // Shuffle the items
            for (int i = items.Count - 1; i > 0; i--)
            {
                int randIndex = Random.Range(0, i + 1);
                Transform temp = items[i];
                items[i] = items[randIndex];
                items[randIndex] = temp;
            }

            // Assign the shuffled items back to the parent
            for (int i = 0; i < items.Count; i++)
            {
                items[i].SetSiblingIndex(i);
            }

            // Assign interaction events
            for (int i = 0; i < items.Count; i++)
            {
                Image itemImage = items[i].GetComponent<Image>();
                if (itemImage != null)
                {
                    Button itemButton = items[i].GetComponent<Button>();
                    if (itemButton != null)
                    {
                        if (itemImage.sprite == correctImage)
                        {
                            Animator childAnimator = itemButton.GetComponentInChildren<Animator>();
                            if (childAnimator != null)
                            {
                                childAnimator.runtimeAnimatorController = correctAnimator;
                                Debug.Log("Correct Animator: " + correctAnimator);
                                itemButton.onClick.AddListener(() => { childAnimator.Play("ConfettiCorrect"); HandleCorrectAnswer(); });
                            }
                        }
                        else
                        {
                            Animator childAnimator = itemButton.GetComponentInChildren<Animator>();
                            if (childAnimator != null)
                            {
                                childAnimator.runtimeAnimatorController = wrongAnimator;
                                Debug.Log("Correct Animator: " + correctAnimator);
                                itemButton.onClick.AddListener(() => { childAnimator.Play("ConfettiWrong"); HandleWrongAnswer(); });
                            }
                        }
                    }
                }
            }
        }
        correctAudio = audioCorrect;
        currentPanelIndex = panelIndex;
        panelHistory.Push(panelIndex);
        GlobalCounter.ResetCounters();
        StartCoroutine(FindAudioSourceInPanel(currentPanelIndex));
        availablePanelIndices.RemoveAt(randomIndex);
    }

    private IEnumerator PlayCorrectAudio()
    {
        if (isPlayingAudio) yield break;
        isPlayingAudio = true;

        if (correctAudio != null && correctAudio != null)
        {
            AudioController.Instance.PlayVoice(correctAudio);
            Debug.Log("Playing correct audio", correctAudio);
            yield return new WaitForSeconds(correctAudio.length);
        }
        else
        {
            Debug.LogWarning("No correct audio found");
        }

        yield return new WaitForSeconds(1.5f);
        isPlayingAudio = false;
    }

    private void HandleCorrectAnswer()
    {
        AudioController.Instance.PlaySfx(correctSFX);
        Animator panelAnimator = panels[currentPanelIndex].GetComponent<Animator>();
        if (panelAnimator != null)
        {
            panelAnimator.SetTrigger("CorrectAnswer");
        }
        Debug.Log("Correct Answer");
        StartCoroutine(PlayCorrectAudio());
        StartCoroutine(waitTenSeconds());
        GlobalCounter.IncrementarAciertos();
        ShowNextPanelAfterDelay(delayBeforeNextPanel);
    }

    private IEnumerator waitTenSeconds()
    {
        yield return new WaitForSeconds(10);
    }

    private void HandleWrongAnswer()
    {
        AudioController.Instance.PlaySfx(incorrectSFX);
        GlobalCounter.IncrementarNoAciertos();
    }

    public void ShowPreviousPanel()
    {
        PlayButtonClickAudio();
        if (panelHistory.Count > 1)
        {
            panelHistory.Pop();
            int previousPanelIndex = panelHistory.Peek();
            ShowPanel(previousPanelIndex);
            ResetPanelObjects();
        }
        else
        {
            PlayButtonClickAudio();
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneOnBackButton);
        }
    }

    public void ShowNextPanelAfterDelay(float delay)
    {
        StartCoroutine(ChangePanelAfterDelay(delay));
    }

    private IEnumerator ChangePanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowNextPanel();
    }

    private void ShowPanel(int index)
    {
        if (index < 0 || index >= panels.Length)
        {
            return;
        }

        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == index);
        }

        currentPanelIndex = index;
        GlobalCounter.ResetCounters();
        StartCoroutine(FindAudioSourceInPanel(currentPanelIndex));
    }

    private void ResetPanelObjects()
    {
        if (currentPanelIndex < 0 || currentPanelIndex >= panels.Length)
        {
            return;
        }

        GameObject currentPanel = panels[currentPanelIndex];
        Transform almarioTransform = currentPanel.transform.Find("Almario");
        if (almarioTransform == null)
        {
            return;
        }

        foreach (Transform child in almarioTransform)
        {
            if (child.CompareTag("Draggable"))
            {
                var draggable = child.GetComponent<ControllerDrag_and_Drop>();
                if (draggable != null)
                {
                    draggable.ResetState();
                }
            }
        }
    }

    public void RestartScene()
    {
        PlayButtonClickAudio();
        //string currentSceneName = SceneManager.GetActiveScene().name;
        //SceneManager.LoadScene(currentSceneName);
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("lugares_publicos_juego");
    }
    public void BackFirstScene()
    {
        PlayButtonClickAudio();
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Pantalla_Actividades");
    }

    public void LoadNewScene()
    {
        if (!string.IsNullOrEmpty(sceneOnBackButton))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneOnBackButton);
        }
    }

    private void ShowRewardPanelAndNext()
    {
        if (currentPanelIndex < panelsRecompensas.Length)
        {
            StartCoroutine(ShowRewardPanel(currentPanelIndex));
        }
        else
        {
            if (additionalPanel != null)
            {
                additionalPanel.SetActive(true);
            }
        }
    }

    private IEnumerator ShowRewardPanel(int panelIndex)
    {
        panelsRecompensas[0].SetActive(true);
        AudioController.Instance.PlayVoice(audioClipRecompensa);
        EndLevel("completado");
        yield return new WaitUntil(() => !AudioController.Instance.GetVoiceSource().isPlaying);
        yield return new WaitForSeconds(rewardPanelDuration);

        if (additionalPanel != null)
        {
            additionalPanel.SetActive(true);
        }
    }

    public void EndLevel(string status)
    {
        levelData.estado = status;
        levelData.fecha_fin = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        levelData.tiempo_juego = (System.Math.Round(Time.timeSinceLevelLoad)).ToString();
        //levelData.puntaje = contP.puntaje.ToString();
        levelData.correctas = GlobalCounter.ObtenerAciertosTotales().ToString();
        levelData.incorrectas = GlobalCounter.ObtenerNoAciertosTotales().ToString();
        GameStateManager.Instance.AddJsonToList(JsonUtility.ToJson(levelData));
        //GameStateManager.Instance.LoadScene("ActivityHub");
    }
}