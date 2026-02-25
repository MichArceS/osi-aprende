using UnityEngine;

public class RecompensaAudioManager : MonoBehaviour
{
    public AudioClip[] audiosRecompensa;

    void Start()
    {
        if (audiosRecompensa.Length > 0)
        {
            int index = Random.Range(0, audiosRecompensa.Length);
            AudioClip clipElegido = audiosRecompensa[index];

            AudioController.Instance.PlayVoice(clipElegido);
        }
        else
        {
            Debug.LogWarning("No hay audios asignados en el RecompensaAudioManager.");
        }
    }
}
