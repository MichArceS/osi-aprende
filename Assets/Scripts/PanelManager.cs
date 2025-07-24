using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PanelManager : MonoBehaviour
{
    public GameObject[] panels;
    public AudioClip[] audioClips;

    public string sceneOnBackButton;
    public string sceneOnNextButton;

    public Button backButton;

    private int currentPanelIndex = 0;
    private Animator panelAnimator;

    public AudioClip buttonEffect;
    public AudioClip music;

    void Start()
    {
        AudioController.Instance.PlayMusic(music);
        panelAnimator = GetComponent<Animator>();
        ShowPanel(currentPanelIndex);
        StartCoroutine(FindAudioSourceInPanel(currentPanelIndex));

        AdjustBackButtonPosition();
    }
    public void PlayAudioClic()
    {
        AudioController.Instance.PlaySfx(buttonEffect);
    }

    public void RepeatAudio()
    {
        PlayAudioClic();
        AudioController.Instance.ReplayVoice();
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
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneOnNextButton);
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
            AdjustBackButtonPosition(true);
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneOnBackButton);
        }
    }

    private IEnumerator TransitionToPanel(int nextIndex, bool isNext)
    {
        AudioController.Instance.StopVoice();
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
            Debug.LogWarning("Animator es null durante la transici�n.");
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

        AudioController.Instance.PlayVoice(audioClips[panelIndex]);
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

    private void AdjustBackButtonPosition(bool isInitialSetup = false)
    {
        if (backButton != null)
        {

            if (currentPanelIndex == 0 && !isInitialSetup)
            {
                backButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(135, 945);
            }
            else
            {

                backButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(135, 135);
            }
        }
    }

    public void HideAudioManage(bool b)
    {
        AudioController.Instance.HidePanel(b);
    }
}
