using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalCounterInitializer : MonoBehaviour
{
    public AudioSource audioSourceCorrecto;
    public AudioSource audioSourceIncorrecto;

    public AudioClip[] audioClipsForThirdCorrectAnswer;
    public AudioClip[] audioClipsForThirdIncorrectAnswer;

    void Start()
    {
        if (audioSourceCorrecto != null)
        {
            GlobalCounter.InitializeAudioSourceCorrecto(audioSourceCorrecto);
        }
        else
        {
            Debug.LogWarning("No se ha asignado el AudioSource correcto en GlobalCounterInitializer.");
        }

        if (audioSourceIncorrecto != null)
        {
            GlobalCounter.InitializeAudioSourceIncorrecto(audioSourceIncorrecto);
        }
        else
        {
            Debug.LogWarning("No se ha asignado el AudioSource incorrecto en GlobalCounterInitializer.");
        }

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
