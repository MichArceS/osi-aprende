using UnityEngine;

public static class GlobalCounter
{
    private static int aciertosTotales = 0;
    private static int noAciertosTotales = 0;

    private static AudioSource audioSource;
    private static AudioSource audioSourceIncorrect;

    private static AudioClip[] audioClipsForThirdCorrectAnswer;
    private static AudioClip[] audioClipsForThirdIncorrectAnswer;

    // Evento que se dispara cuando cambian los aciertos
    public static event System.Action OnAciertosChanged;

    // Método para inicializar el AudioSource
    public static void InitializeAudioSource(AudioSource source)
    {
        audioSource = source;
    }

    // Método para inicializar el AudioSource
    public static void InitializeAudioSourceIncorrect(AudioSource sourceIn)
    {
        audioSourceIncorrect = sourceIn;
    }

    // Método para establecer los AudioClips
    public static void SetAudioClips(AudioClip[] clips)
    {
        audioClipsForThirdCorrectAnswer = clips;
    }

    // Método para establecer los AudioClips
    public static void SetAudioClipsIncorrect(AudioClip[] clips)
    {
        audioClipsForThirdIncorrectAnswer = clips;
    }

    public static void IncrementarAciertos()
    {
        aciertosTotales++;
        Debug.Log("Aciertos Totales: " + aciertosTotales);

        // Reproduce el audio si el número total de aciertos es menor a dos
        if (aciertosTotales < 3)
        {
            if (audioSource != null)
            {
                audioSource.Play();
            }
        }

        // Reproduce un audio aleatorio cuando se alcanza el tercer acierto
        if (aciertosTotales >= 3 && audioClipsForThirdCorrectAnswer != null && audioClipsForThirdCorrectAnswer.Length > 0)
        {
            AudioClip clipToPlay = audioClipsForThirdCorrectAnswer[Random.Range(0, audioClipsForThirdCorrectAnswer.Length)];
            audioSource.PlayOneShot(clipToPlay);
        }

        //Debug.Log("Disparando OnAciertosChanged.");

        OnAciertosChanged?.Invoke();
    }

    public static void IncrementarNoAciertos()
    {
        noAciertosTotales++;
        Debug.Log("No Aciertos Totales: " + noAciertosTotales);

        // Reproduce el audio si el número total de aciertos es menor a dos
        if (noAciertosTotales < 3)
        {
            if (audioSourceIncorrect != null)
            {
                audioSourceIncorrect.Play();
            }
        }

        // Reproduce un audio aleatorio 
        if (noAciertosTotales >= 3 && audioClipsForThirdIncorrectAnswer != null && audioClipsForThirdIncorrectAnswer.Length > 0)
        {
            AudioClip clipToPlay = audioClipsForThirdIncorrectAnswer[Random.Range(0, audioClipsForThirdIncorrectAnswer.Length)];
            audioSource.PlayOneShot(clipToPlay);

        }
    }

    public static int ObtenerAciertosTotales()
    {
        return aciertosTotales;
    }

    public static int ObtenerNoAciertosTotales()
    {
        return noAciertosTotales;
    }

    // Método para reiniciar los contadores
    public static void ResetCounters()
    {
        aciertosTotales = 0;
        noAciertosTotales = 0;
        Debug.Log("Contadores reiniciados.");
    }

}
