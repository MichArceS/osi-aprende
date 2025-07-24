using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalCounterInitializer : MonoBehaviour
{
    public AudioClip goodSFX;
    public AudioClip badSFX;

    public AudioClip[] audioClipsForThirdCorrectAnswer;
    public AudioClip[] audioClipsForThirdIncorrectAnswer;

    void Start()
    {
        GlobalCounter.InitializeClips(goodSFX, badSFX);

        if (audioClipsForThirdCorrectAnswer != null && audioClipsForThirdCorrectAnswer.Length > 0)
        {
            GlobalCounter.SetVocesPositivas(audioClipsForThirdCorrectAnswer);
        }
        else
        {
            Debug.LogWarning("No se han asignado voces positivas en GlobalCounterInitializer.");
        }

        if (audioClipsForThirdIncorrectAnswer != null && audioClipsForThirdIncorrectAnswer.Length > 0)
        {
            GlobalCounter.SetVocesAnimo(audioClipsForThirdIncorrectAnswer);
        }
        else
        {
            Debug.LogWarning("No se han asignado voces de ánimo en GlobalCounterInitializer.");
        }
    }
}
