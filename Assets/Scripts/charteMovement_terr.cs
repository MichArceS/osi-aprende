using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class charteMovement_terr : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 targetPosition;
    private bool isMoving;
    private bool firstClick = true;
    private Animator animator;
    private Rigidbody2D rb;

    public List<Vector2> waypoints;
    private int currentWaypointIndex = 0;
    private int clickCount = 0;

    public Vector2 DestinationInorrecto;
    public Vector2 DestinationEmpezando;
    public Button boton;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clickSound;
    public AudioClip[] randomSounds;
    public AudioSource audioSalir;
    public AudioSource audioCamIncorrecto;


    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        isMoving = false;
        animator.SetBool("isWalking", false);

        if (boton != null)
        {
            boton.interactable = false;
        }
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && firstClick)
        {
            animator.SetBool("isMoving", true);
            firstClick = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit)
            {
                // Detectar si se hace clic en los cuadrados
                if (hit.collider.CompareTag("Square"))
                {
                    PlayToAudioClicIncorrect();
                    //Debug.Log("Por ahí no puede caminar.");
                    return;
                }

                HandleClick(hit.collider);
            }
        }

        if (isMoving)
        {
            MoveCharacter();
        }
    }

    void HandleClick(Collider2D hit)
    {
        if (hit.CompareTag("caminoCorrecto"))
        {
            if (waypoints.Count > 0)
            {
                currentWaypointIndex = 0;
                targetPosition = waypoints[currentWaypointIndex];
                Debug.Log("Dirigiéndose a lugar de encuentro");
                isMoving = true;
            }
            boton.interactable = true; 
        }
        else if (hit.CompareTag("caminoIncorrecto"))
        {
            targetPosition = DestinationInorrecto;
            Debug.Log("Dirigiéndose a camino incorrecto");
            isMoving = true;
        }
        else if (hit.CompareTag("empezandoCaminar"))
        {
            targetPosition = DestinationEmpezando;
            Debug.Log("Empiece a elegir el camino correcto");
            isMoving = true;
        }
        else
        {
            Debug.Log("Lugar incorrecto");
            return;
        }
    }

    private void PlayTheAudioCorrect(AudioSource audioSource)
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    private void PlayToAudioClicIncorrect()
    {
        clickCount++;

        if (clickCount <= 2)
        {
            PlayAudio(clickSound);
        }
        else if (clickCount > 2)
        {
            PlayRandomSound();
        }

    }
    private void PlayAudio(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    private void PlayRandomSound()
    {
        if (audioSource != null && randomSounds.Length > 0)
        {
            AudioClip randomClip = randomSounds[Random.Range(0, randomSounds.Length)];
            PlayAudio(randomClip);
        }
    }

    void MoveCharacter()
    {
        Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPosition, speed * Time.deltaTime);
        rb.MovePosition(newPosition);

        UpdateDirection(newPosition);

        // Comprobar si se ha alcanzado el targetPosition
        if (Vector2.Distance(rb.position, targetPosition) < 0.1f)
        {
            if (targetPosition == DestinationInorrecto)
            {
                PlayTheAudioCorrect(audioCamIncorrecto);
                targetPosition = DestinationEmpezando;
                Debug.Log("Regresando al punto de inicio...");
            }
            else if (currentWaypointIndex < waypoints.Count - 1 && targetPosition == waypoints[currentWaypointIndex])
            {
                currentWaypointIndex++;
                targetPosition = waypoints[currentWaypointIndex];
            }
            else
            {
                // Si hemos llegado al último waypoint
                isMoving = false;
                animator.SetBool("isWalking", false);
                ShowDestinationMessage(targetPosition);
            }
        }
        else
        {
            animator.SetBool("isWalking", true);
        }
    }

    private void ShowDestinationMessage(Vector2 position)
    {
        if (currentWaypointIndex == waypoints.Count - 1)
        {
            Debug.Log("Llegaste al encuentro muy bien");

            PlayTheAudioCorrect(audioSalir);

            if (GameManagerOsiPersonaje.Instance != null)
            {
                GameManagerOsiPersonaje.Instance.IncrementScore();
            }

            StartCoroutine(LoadSceneWithDelay("recompensa1_jego2", 3f));
        }
    }

    private IEnumerator LoadSceneWithDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadSceneAsync(sceneName);
    }

    void UpdateDirection(Vector2 newPosition)
    {
        Vector2 direction = (newPosition - rb.position).normalized;
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
    }
}