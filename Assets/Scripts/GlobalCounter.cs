using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalCounter
{
    private static int aciertosTotales = 0;
    private static int noAciertosTotales = 0;

    private static AudioSource audioSourceCorrecto;
    private static AudioSource audioSourceIncorrecto;

    private static AudioClip[] vocesPositivas;  // Audios para el tercer acierto
    private static AudioClip[] vocesAnimo;      // Audios para el tercer error

    // ✅ Evento para notificar cambios de aciertos
    public static event System.Action OnAciertosChanged;

    public static void InitializeAudioSourceCorrecto(AudioSource source)
    {
        audioSourceCorrecto = source;
    }

    public static void InitializeAudioSourceIncorrecto(AudioSource source)
    {
        audioSourceIncorrecto = source;
    }

    public static void SetVocesPositivas(AudioClip[] clips)
    {
        vocesPositivas = clips;
    }

    public static void SetVocesAnimo(AudioClip[] clips)
    {
        vocesAnimo = clips;
    }

    public static void IncrementarAciertos()
    {
        aciertosTotales++;
        Debug.Log("Aciertos totales: " + aciertosTotales);

        if (aciertosTotales % 3 != 0)
        {
            if (audioSourceCorrecto != null)
            {
                audioSourceCorrecto.Play();
            }
        }
        else
        {
            if (vocesPositivas != null && vocesPositivas.Length > 0 && audioSourceCorrecto != null)
            {
                AudioClip clip = vocesPositivas[Random.Range(0, vocesPositivas.Length)];
                audioSourceCorrecto.PlayOneShot(clip);
            }
        }

        // 🔔 Llamamos al evento
        OnAciertosChanged?.Invoke();
    }

    public static void IncrementarNoAciertos()
    {
        noAciertosTotales++;
        Debug.Log("Errores totales: " + noAciertosTotales);

        if (noAciertosTotales % 3 != 0)
        {
            if (audioSourceIncorrecto != null)
            {
                audioSourceIncorrecto.Play();
            }
        }
        else
        {
            if (vocesAnimo != null && vocesAnimo.Length > 0 && audioSourceIncorrecto != null)
            {
                AudioClip clip = vocesAnimo[Random.Range(0, vocesAnimo.Length)];
                audioSourceIncorrecto.PlayOneShot(clip);
            }
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

    public static AudioSource GetAudioSourceCorrecto()
    {
        return audioSourceCorrecto;
    }

    public static AudioSource GetAudioSourceIncorrecto()
    {
        return audioSourceIncorrecto;
    }

    public static void ResetCounters()
    {
        aciertosTotales = 0;
        noAciertosTotales = 0;
        Debug.Log("Contadores reiniciados.");

        // 🔔 Llamamos al evento cuando se reinicia
        OnAciertosChanged?.Invoke();
    }

    public static void IncrementarAciertosCartas()
    {
        IncrementarAciertos();
    }
}
