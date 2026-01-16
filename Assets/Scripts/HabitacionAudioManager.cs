using UnityEngine;

public class HabitacionAudioManager : MonoBehaviour
{
    public static HabitacionAudioManager instance;

    public AudioClip audioClip1;
    public AudioClip audioClip2;

    private bool audioYaReproducido = false;

    void Awake()
    {
        // Singleton para que solo exista una instancia persistente
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ReproducirPrimerAudio()
    {
        AudioController.Instance.PlayVoice(audioClip1);
    }

    public void ReproducirAudioUnaVez()
    {
        AudioController.Instance.PlayVoice(audioClip2);
        audioYaReproducido = true;
    }
}
