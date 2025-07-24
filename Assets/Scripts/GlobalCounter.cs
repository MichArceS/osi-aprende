using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalCounter
{
    private static int aciertosTotales = 0;
    private static int noAciertosTotales = 0;

    private static AudioClip goodSFX;
    private static AudioClip badSFX;

    private static AudioClip[] vocesPositivas;  // Audios para el tercer acierto
    private static AudioClip[] vocesAnimo;      // Audios para el tercer error

    // ✅ Evento para notificar cambios de aciertos
    public static event System.Action OnAciertosChanged;

    public static void InitializeClips(AudioClip goodAudioClip, AudioClip badAudioClip)
    {
        goodSFX = goodAudioClip;
        badSFX = badAudioClip;
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
            AudioController.Instance.PlaySfx(goodSFX);
        }
        else
        {
            if (vocesPositivas != null && vocesPositivas.Length > 0)
            {
                AudioClip clip = vocesPositivas[Random.Range(0, vocesPositivas.Length)];
                AudioController.Instance.PlayVoice(clip);
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
            AudioController.Instance.PlaySfx(badSFX);
        }
        else
        {
            if (vocesAnimo != null && vocesAnimo.Length > 0)
            {
                AudioClip clip = vocesAnimo[Random.Range(0, vocesAnimo.Length)];
                AudioController.Instance.PlayVoice(clip);
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
