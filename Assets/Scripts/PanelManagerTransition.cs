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

    public string[] scenes; // Nombres de las escenas asociadas a cada botón
    public float waitTimeBeforePanel2Transition = 1f;
    public float waitTimeBeforePanel3Transition = 0.5f;
    public float transitionDuration = 1f;

    private AudioSource activeAudioSource;

    private void Start()
    {
        // Inicialmente desactivar todos los paneles
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

        // Seleccionar el audio correspondiente
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

        StartCoroutine(HandlePanelTransition(buttonIndex - 1)); // Pasar el índice de escena
    }

    private IEnumerator HandlePanelTransition(int sceneIndex)
    {
        // Activar todos los paneles
        panel2.SetActive(true);
        panel3.SetActive(true);
        panel4.SetActive(true);

        // Configurar CanvasGroup de Paneles
        CanvasGroup canvasGroup2 = panel2.GetComponent<CanvasGroup>();
        CanvasGroup canvasGroup3 = panel3.GetComponent<CanvasGroup>();
        CanvasGroup canvasGroup4 = panel4.GetComponent<CanvasGroup>();

        // Inicializar alfa de Paneles
        SetCanvasGroupAlpha(canvasGroup2, 1f); // Panel 2 visible
        SetCanvasGroupAlpha(canvasGroup3, 0f); // Panel 3 invisible
        SetCanvasGroupAlpha(canvasGroup4, 0f); // Panel 4 invisible

        // Reproducir el audio correspondiente después de la espera
        if (activeAudioSource != null)
        {
            activeAudioSource.Play();
        }

        // Esperar a que el Panel 2 esté completamente visible
        yield return new WaitForSeconds(waitTimeBeforePanel2Transition);

        // Transición entre Panel 2 y Panel 3
        yield return StartCoroutine(FadeOutAndIn(canvasGroup2, canvasGroup3, transitionDuration, 1f));

        // Esperar después de que el Panel 3 esté completamente visible
        yield return new WaitForSeconds(waitTimeBeforePanel3Transition);

        // Transición entre Panel 3 y Panel 4
        yield return StartCoroutine(FadeOutAndIn(canvasGroup3, canvasGroup4, transitionDuration, 1f));

        // Mostrar Panel 4 completamente
        SetCanvasGroupAlpha(canvasGroup4, 1f);

        // Esperar 0.5 segundos antes de cargar la nueva escena
        yield return new WaitForSeconds(0.5f);

        // Cargar la escena correspondiente
        SceneManager.LoadScene(scenes[sceneIndex]);
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
