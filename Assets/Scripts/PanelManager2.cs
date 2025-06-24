using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PanelManager2 : MonoBehaviour
{
    public GameObject[] panels;
    public string sceneOnBackButton;
    public string sceneOnNextButton;
    public AudioSource audioClic;
    public AudioSource backgroundAudio;
    public Button backButton;
    public Button nextButton; 

    private int currentPanelIndex = 0;
    private Animator panelAnimator;
    private AudioSource currentAudioSource;

    void Start()
    {
        panelAnimator = GetComponent<Animator>();
        ShowPanel(currentPanelIndex);
        StartCoroutine(FindAudioSourceInPanel(currentPanelIndex));

        AdjustBackButtonPosition();
    }

    private void PlayAudioClic()
    {
        if (audioClic != null)
        {
            audioClic.Play();
        }
    }

    public void RepeatAudio()
    {
        PlayAudioClic();

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

    public void SetCurrentAudioSource(AudioSource audioSource)
    {
        currentAudioSource = audioSource;
    }

    public void ShowNextPanel()
    {
        PlayAudioClic();

        if (currentPanelIndex < panels.Length - 1)
        {
            StartCoroutine(TransitionToPanel(currentPanelIndex + 1, true));
        }
        else
        {
            SceneManager.LoadSceneAsync(sceneOnNextButton);
        }
    }

    public void ShowPreviousPanel()
    {
        PlayAudioClic();

        if (currentPanelIndex > 0)
        {
            StartCoroutine(TransitionToPanel(currentPanelIndex - 1, false));
        }
        else
        {
            SceneManager.LoadSceneAsync(sceneOnBackButton);
        }
    }

    private IEnumerator TransitionToPanel(int nextIndex, bool isNext)
    {
        if (panelAnimator != null)
        {
            ShowPanel(nextIndex);
            panels[currentPanelIndex].SetActive(false);

            panelAnimator.SetTrigger("FadeIn");
            float fadeInDuration = GetAnimatorStateDuration("FadeIn");
            yield return new WaitForSeconds(fadeInDuration);

            yield return StartCoroutine(FindAudioSourceInPanel(nextIndex));
        }
        else
        {
            panels[currentPanelIndex].SetActive(false);
            yield return StartCoroutine(FindAudioSourceInPanel(nextIndex));
        }

        currentPanelIndex = nextIndex;
        AdjustBackButtonPosition();
    }

    private void ShowPanel(int index)
    {
        panels[index].SetActive(true);
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
    }

    private float GetAnimatorStateDuration(string stateName)
    {
        if (panelAnimator == null)
            return 0f;

        AnimatorStateInfo stateInfo = panelAnimator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName(stateName))
        {
            return stateInfo.length;
        }

        return 1f;
    }

    private void AdjustBackButtonPosition()
    {
        if (backButton != null)
        {
            backButton.gameObject.SetActive(true);

            RectTransform backRect = backButton.GetComponent<RectTransform>();

            if (currentPanelIndex == 0)
            {
                // Posición arriba a la izquierda
                backRect.anchoredPosition = new Vector2(135f, -135f);
            }
            else if (nextButton != null)
            {
                // Alineado con botón siguiente pero a la izquierda
                RectTransform nextRect = nextButton.GetComponent<RectTransform>();
                Vector2 nextPos = nextRect.anchoredPosition;

                backRect.anchoredPosition = new Vector2(157f, -921f);
            }
            else
            {
                // Fallback si no hay botón siguiente
                backRect.anchoredPosition = new Vector2(135f, 135f);
            }
        }
    }
}
