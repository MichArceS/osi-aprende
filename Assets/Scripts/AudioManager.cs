using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public AudioClip audioClip1; 
    public AudioClip audioClip2; 
    public float fadeDuration = 0.3f; 

    private AudioSource audioSource1;
    private AudioSource audioSource2;
    private string currentClipName;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        
        AudioSource[] audioSources = GetComponents<AudioSource>();

        if (audioSources.Length < 2)
        {
            Debug.LogError("Se requieren exactamente dos componentes AudioSource en el GameObject.");
            return;
        }

        audioSource1 = audioSources[0];
        audioSource2 = audioSources[1];

        audioSource1.loop = true;
        audioSource2.loop = true;

        audioSource1.volume = 0f;
        audioSource2.volume = 0f;

        audioSource1.clip = audioClip1;
        audioSource2.clip = audioClip2;

        
        audioSource1.Play();
        StartCoroutine(FadeInAudio(audioSource1, 0.5f));
        currentClipName = "audioClip1";
    }

    private IEnumerator FadeInAudio(AudioSource audioSource, float targetVolume)
    {
        float startVolume = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / fadeDuration);
            yield return null;
        }
        audioSource.volume = targetVolume;
    }

    public void SwitchAudio(bool useFirstAudio)
    {
        if ((useFirstAudio && currentClipName == "audioClip1") ||
            (!useFirstAudio && currentClipName == "audioClip2"))
        {
            return; 
        }

        StartCoroutine(FadeAudio(useFirstAudio));
    }

    private IEnumerator FadeAudio(bool useFirstAudio)
    {
        AudioSource currentAudio = useFirstAudio ? audioSource2 : audioSource1;
        AudioSource nextAudio = useFirstAudio ? audioSource1 : audioSource2;

        // Fade out
        float currentVolume = currentAudio.volume;
        float targetVolume = 0.3f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            currentAudio.volume = Mathf.Lerp(currentVolume, targetVolume, elapsedTime / fadeDuration);
            yield return null;
        }
        currentAudio.volume = targetVolume;
        currentAudio.Stop();

        // Prepare next audio
        nextAudio.clip = useFirstAudio ? audioClip1 : audioClip2;
        nextAudio.volume = 0f;
        nextAudio.Play();

        // Fade in
        elapsedTime = 0f;
        targetVolume = 0.5f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            nextAudio.volume = Mathf.Lerp(0f, targetVolume, elapsedTime / fadeDuration);
            yield return null;
        }
        nextAudio.volume = targetVolume;

        currentClipName = useFirstAudio ? "audioClip1" : "audioClip2";
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Lista de escenas permitidas para la reproducción de audio
        string[] allowedScenes = {
        "Pantalla_Inicio",
        "Pantalla_Nuevo_Juego",
        "Pantalla_Avatars",
        "Pantalla_Nombre",
        "Pantalla_eleccion_avatars_creates",
        "Pantalla_Actividades"
        };

        if (System.Array.Exists(allowedScenes, s => s == scene.name))
        {
            if (scene.name == "Pantalla_Inicio" || scene.name == "Pantalla_Nuevo_Juego")
            {
                SwitchAudio(true);
            }
            else
            {
                SwitchAudio(false);
            }
        }
        else
        {
            
            audioSource1.Stop();
            audioSource2.Stop();
        }

    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
