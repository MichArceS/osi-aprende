using UnityEngine;

public class HabitacionAudioManager : MonoBehaviour
{
    public static HabitacionAudioManager instance;
    public AudioClip audioClip;    // Asigna aquí el audio que quieres reproducir
    private AudioSource audioSource;

    private bool audioYaReproducido = false;

    void Awake()
    {
        // Singleton para que solo exista una instancia persistente
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ReproducirAudioUnaVez()
    {
        if (!audioYaReproducido && audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
            audioYaReproducido = true;
        }
    }
}
