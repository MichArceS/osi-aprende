using UnityEngine;

public class AudioSceneCaller : MonoBehaviour
{
    void Start()
    {
        if (HabitacionAudioManager.instance != null)
        {
            HabitacionAudioManager.instance.ReproducirAudioUnaVez();
        }
    }
}
