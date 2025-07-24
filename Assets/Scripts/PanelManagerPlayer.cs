using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PanelManagerPlayer : MonoBehaviour
{
    public GameObject[] panels;
    public AudioClip[] audioClips;
    public GameObject[] panelsRecompensas;
    public GameObject additionalPanel;
    public float delayBeforeNextPanel = 1f;
    public float rewardPanelDuration = 3f;
    public string sceneOnBackButton;

    private List<int> availablePanelIndices;
    private List<int> availableAudios;
    private Stack<int> panelHistory;
    private int currentPanelIndex = -1;
    private int[] aciertosPorPanel;
    private bool allPanelsCompleted = false;
    private AudioSource currentAudioSource;

    public AudioClip buttonEffect;
    public AudioClip music;

    void Start()
    {
        AudioController.Instance.PlayMusic(music);
        aciertosPorPanel = new int[panels.Length];
        for (int i = 0; i < aciertosPorPanel.Length; i++)
        {
            aciertosPorPanel[i] = 4;
        }
        InitializePanelIndices();
        panelHistory = new Stack<int>();
        ShowNextPanel();
        GlobalCounter.OnAciertosChanged += HandleAciertosChanged;
    }

    public void PlayButtonClickAudio()
    {
        AudioController.Instance.PlaySfx(buttonEffect);
    }

    public void RepeatAudio()
    {
        PlayButtonClickAudio();
        AudioController.Instance.ReplayVoice();
    }

    void OnDestroy()
    {
        GlobalCounter.OnAciertosChanged -= HandleAciertosChanged;
    }

    private void InitializePanelIndices()
    {
        availablePanelIndices = new List<int>();
        availableAudios = new List<int>();
        for (int i = 0; i < panels.Length; i++)
        {
            availablePanelIndices.Add(i);
            availableAudios.Add(i);
        }
        allPanelsCompleted = false;
    }

    public void SetCurrentAudioSource(AudioSource audioSource)
    {
        currentAudioSource = audioSource;
    }

    private IEnumerator FindAudioSourceInPanel(int panelIndex)
    {
        yield return new WaitForSeconds(0.1f);

        AudioController.Instance.PlayVoice(audioClips[panelIndex]);
    }

    private void ShowNextPanel()
    {
        if (availablePanelIndices.Count == 0)
        {
            if (!allPanelsCompleted)
            {
                allPanelsCompleted = true;
                if (additionalPanel != null)
                {
                    additionalPanel.SetActive(true);
                }
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
        currentPanelIndex = panelIndex;

        availablePanelIndices.RemoveAt(randomIndex);
        availableAudios.RemoveAt(randomIndex);
        
        panelHistory.Push(panelIndex);
        GlobalCounter.ResetCounters();
        StartCoroutine(FindAudioSourceInPanel(currentPanelIndex));
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
        ShowRewardPanelAndNext();
    }

    private void HandleAciertosChanged()
    {
        if (GlobalCounter.ObtenerAciertosTotales() >= aciertosPorPanel[currentPanelIndex])
        {
            ShowNextPanelAfterDelay(delayBeforeNextPanel);
        }
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
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("instruccion_juego1");
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
        panelsRecompensas[panelIndex].SetActive(true);
        yield return new WaitForSeconds(rewardPanelDuration);
        panelsRecompensas[panelIndex].SetActive(false);
        ShowNextPanel();
    }

    public void HideAudioManage(bool b)
    {
        AudioController.Instance.HidePanel(b);
    }
}