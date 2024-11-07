using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelManagerTransition : MonoBehaviour
{
    public GameObject panel2;
    public GameObject panel3;
    public GameObject panel4;
    public AudioSource audioButton1;
    public AudioSource audioButton2;
    public AudioSource audioButton3;
    public AudioSource audioInstrucion;

    public Animator animator1;
    public Animator animator2;
    public Animator animator3;

    public string[] scenes;
    public float waitTimeBeforePanel2Transition = 1f;
    public float waitTimeBeforePanel3Transition = 0.5f;
    public float transitionDuration = 1f;

    private AudioSource activeAudioSource;

    private void Start()
    {
        panel2.SetActive(false);
        panel3.SetActive(false);
        panel4.SetActive(false);
    }

    public void OnButtonClick(int buttonIndex)
    {
        if (buttonIndex < 1 || buttonIndex > scenes.Length)
        {
            Debug.LogError("Botón inválido");
            return;
        }

        StartCoroutine(HandleButtonClick(buttonIndex));
    }

    private IEnumerator HandleButtonClick(int buttonIndex)
    {
        yield return new WaitForSeconds(1.5f);

        // Establece el audio activo
        switch (buttonIndex)
        {
            case 1:
                activeAudioSource = audioButton1;
                break;
            case 2:
                activeAudioSource = audioButton2;
                break;
            case 3:
                activeAudioSource = audioButton3;
                break;
        }

        // Reproduce el audio
        if (activeAudioSource != null)
        {
            activeAudioSource.Play();
        }

        // Inicia la transición de panel
        StartCoroutine(HandlePanelTransition(buttonIndex - 1));
    }

    private IEnumerator HandlePanelTransition(int sceneIndex)
    {
        panel2.SetActive(true);
        panel3.SetActive(true);
        panel4.SetActive(true);

        CanvasGroup canvasGroup2 = panel2.GetComponent<CanvasGroup>();
        CanvasGroup canvasGroup3 = panel3.GetComponent<CanvasGroup>();
        CanvasGroup canvasGroup4 = panel4.GetComponent<CanvasGroup>();

        SetCanvasGroupAlpha(canvasGroup2, 1f);
        SetCanvasGroupAlpha(canvasGroup3, 0f);
        SetCanvasGroupAlpha(canvasGroup4, 0f);

        yield return new WaitForSeconds(waitTimeBeforePanel2Transition);
        Debug.Log("Esperando transición de Panel 2 a Panel 3.");
        yield return StartCoroutine(FadeOutAndIn(canvasGroup2, canvasGroup3, transitionDuration, 1f));
        Debug.Log("Transición de Panel 2 a Panel 3 completada.");

        yield return new WaitForSeconds(waitTimeBeforePanel3Transition);
        Debug.Log("Esperando transición de Panel 3 a Panel 4.");
        yield return StartCoroutine(FadeOutAndIn(canvasGroup3, canvasGroup4, transitionDuration, 1f));
        Debug.Log("Transición de Panel 3 a Panel 4 completada.");

        

        SetCanvasGroupAlpha(canvasGroup4, 1f);

        // Reproduce el audio
        if (audioInstrucion != null)
        {
            audioInstrucion.Play();
        }
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(PlayAnimations());

        SceneManager.LoadScene(scenes[sceneIndex]);
    }

    private IEnumerator PlayAnimations()
    {
        animator1.SetTrigger("PlayAnimation1");
        yield return new WaitForSeconds(0.5f);

        animator2.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        animator2.gameObject.SetActive(false);

        animator3.SetTrigger("PlayAnimation3");
        yield return new WaitForSeconds(2.5f);
    }

    private IEnumerator FadeOutAndIn(CanvasGroup fadeOutGroup, CanvasGroup fadeInGroup, float duration, float initialFadeInAlpha)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            fadeOutGroup.alpha = Mathf.Clamp01(1 - (elapsedTime / duration));
            fadeInGroup.alpha = Mathf.Clamp01(Mathf.Lerp(initialFadeInAlpha, 1f, elapsedTime / duration));
            yield return null;
        }

        fadeOutGroup.alpha = 0f;
        fadeInGroup.alpha = 1f;
    }

    private void SetCanvasGroupAlpha(CanvasGroup canvasGroup, float alpha)
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = alpha;
        }
    }
}
