using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioClip audioClip1;
    public AudioClip audioClip2;
    public AudioClip audioClip3;
    public AudioClip audioClip4;
    public AudioClip audioClip5;
    public AudioClip audioClip6;
    public float fadeDuration = 0.3f;

    private AudioSource audioSource1;
    private AudioSource audioSource2;
    private AudioSource audioSource3;
    private AudioSource audioSource4;
    private AudioSource audioSource5;
    private AudioSource audioSource6;
    private string currentClipName;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        AudioSource[] audioSources = GetComponents<AudioSource>();

        if (audioSources.Length < 6)
        {
            return;
        }

        audioSource1 = audioSources[0];
        audioSource2 = audioSources[1];
        audioSource3 = audioSources[2];
        audioSource4 = audioSources[3];
        audioSource5 = audioSources[4];
        audioSource6 = audioSources[5];

        audioSource1.loop = true;
        audioSource2.loop = true;
        audioSource3.loop = true;
        audioSource4.loop = true;
        audioSource5.loop = true;
        audioSource6.loop = true;

        audioSource1.volume = 0f;
        audioSource2.volume = 0f;
        audioSource3.volume = 0f;
        audioSource4.volume = 0f;
        audioSource5.volume = 0f;
        audioSource6.volume = 0f;

        audioSource1.clip = audioClip1;
        audioSource2.clip = audioClip2;
        audioSource3.clip = audioClip3;
        audioSource4.clip = audioClip4;
        audioSource5.clip = audioClip5;
        audioSource6.clip = audioClip6;

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

    public void SwitchAudio(string clipName)
    {
        if (currentClipName == clipName)
        {
            return;
        }

        StartCoroutine(FadeAudio(clipName));
    }

    private IEnumerator FadeAudio(string clipName)
    {
        AudioSource currentAudio = GetAudioSourceByName(currentClipName);
        AudioSource nextAudio = GetAudioSourceByName(clipName);

        // Fade out
        float currentVolume = currentAudio.volume;
        float targetVolume = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            currentAudio.volume = Mathf.Lerp(currentVolume, targetVolume, elapsedTime / fadeDuration);
            yield return null;
        }
        currentAudio.volume = targetVolume;
        currentAudio.Stop();

        // Prepara el nuevo audio
        nextAudio.volume = 0.2f;
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

        currentClipName = clipName;
    }

    private AudioSource GetAudioSourceByName(string clipName)
    {
        switch (clipName)
        {
            case "audioClip1": return audioSource1;
            case "audioClip2": return audioSource2;
            case "audioClip3": return audioSource3;
            case "audioClip4": return audioSource4;
            case "audioClip5": return audioSource5;
            case "audioClip6": return audioSource6;
            default: return null;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Lista de escenas permitidas para la reproducci�n de audio
        string[] allowedScenes = {
            "Pantalla_Inicio",
            "Pantalla_Nuevo_Juego",
            "Pantalla_Avatars",
            "Pantalla_Nombre",
            "Pantalla_eleccion_avatars_creates",
            "Pantalla_Actividades",
            "Pantalla_select_clima",
            "Pantalla_select_fenomenos",
            "Pantalla_select_lugares",
            "historia_clima",
            "juegos_clima",
            "historia_fenomenos_naturales",
            "juegos_fenomenos_naturales",
            "historia_lugares_publicos",
            "terremoto_juego",
            "terremoto_juego2",
            "inundacion_juego",
            "inundacion_juego2",
            "erupcion_volcanica",
            "erupcion_Volcanica2",
            "recompensas_juego2",
            "instruccion_juego1",
            "recompensa1_jego2",
            "recompensa3_juego2"

        };

        if (System.Array.Exists(allowedScenes, s => s == scene.name))
        {
            switch (scene.name)
            {
                case "Pantalla_Inicio":
                case "Pantalla_Nuevo_Juego":
                    SwitchAudio("audioClip1");
                    break;
                case "Pantalla_Avatars":
                case "Pantalla_Nombre":
                case "Pantalla_eleccion_avatars_creates":
                case "Pantalla_Actividades":
                case "Pantalla_select_clima":
                case "Pantalla_select_fenomenos":
                case "Pantalla_select_lugares":
                case "juegos_fenomenos_naturales":
                case "instruccion_juego1":
                    SwitchAudio("audioClip2");
                    break;
                case "historia_fenomenos_naturales":
                case "juegos_clima":
                    SwitchAudio("audioClip3");
                    break;
                case "terremoto_juego":
                case "terremoto_juego2":
                case "inundacion_juego":
                case "inundacion_juego2":
                case "erupcion_volcanica":
                case "erupcion_Volcanica2":
                case "recompensas_juego2":
                case "recompensa1_jego2":
                case "recompensa3_jego2":
                    SwitchAudio("audioClip4");
                    break;
                case "historia_clima":
                    SwitchAudio("audioClip5");
                    break;
                case "historia_lugares_publicos":
                    SwitchAudio("audioClip6");
                    break;
                default:
                    SwitchAudio("audioClip2");
                    break;
            }
        }
        else
        {
            audioSource1.Stop();
            audioSource2.Stop();
            audioSource3.Stop();
            audioSource4.Stop();
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
