using UnityEngine;

public class RecompensaAudioManager : MonoBehaviour
{
    public AudioClip[] audiosRecompensa;  // Asigna los 3 audios desde el inspector
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (audiosRecompensa.Length > 0)
        {
            // Elegir un audio aleatorio
            int index = Random.Range(0, audiosRecompensa.Length);
            AudioClip clipElegido = audiosRecompensa[index];

            audioSource.clip = clipElegido;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No hay audios asignados en el RecompensaAudioManager.");
        }
    }
}
