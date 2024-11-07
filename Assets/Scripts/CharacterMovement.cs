using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterMovement : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 targetPosition;
    private bool isMoving;
    private bool firstClick = true;
    private Animator animator;
    private Rigidbody2D rb;

    public List<Vector2> waypoints;
    private int currentWaypointIndex = 0;

    public Vector2 DestinationCorrecto;
    public Vector2 DestinationInorrecto;
    public Vector2 DestinationEmpezando;
    public Vector2 DestinationIncorrectoSegundo;
    public Vector2 DestinationIncorrectoTercer;
    public Vector2 DestinationEmpezarTercer;
    public List<Button> botones;
    private int clickCount = 0;

    public GameObject newCharacter; 
    private Animator newCharacterAnimator;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clickSound;
    public AudioClip[] randomSounds;
    public AudioSource audioWalkCorrect1;
    public AudioSource audioWalkCorrect2;
    public AudioSource audioWalkInCorrect1;
    public AudioSource audioWalkInCorrect2;
    public AudioSource audioWalkInCorrect3;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        isMoving = false;
        animator.SetBool("isWalking", false);

        newCharacterAnimator = newCharacter.GetComponent<Animator>();
        newCharacter.SetActive(false);

        foreach (Button boton in botones)
        {
            if (boton != null)
            {
                boton.interactable = false;
            }
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
                    Debug.Log("Por ahí no puede caminar.");
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


    void HandleClick(Collider2D hit)
    {
        if (hit.CompareTag("caminoCorrecto"))
        {
            targetPosition = DestinationCorrecto;
            Debug.Log("Dirigiéndose a camino correcto");
            isMoving = true;

            if (botones.Count > 1 && botones[0] != null)
            {
                botones[0].interactable = true;
            }
            PlayTheAudioCorrect(audioWalkCorrect1);
        }
        else if (hit.CompareTag("caminoIncorrecto"))
        {
            targetPosition = DestinationInorrecto;
            Debug.Log("Dirigiéndose a camino incorrecto");
            isMoving = true;
            PlayTheAudioCorrect(audioWalkInCorrect1);
        }
        else if (hit.CompareTag("empezandoCaminar"))
        {
            targetPosition = DestinationEmpezando;
            Debug.Log("Empiece a elegir el camino correcto");
            isMoving = true;
        }
        else if (hit.CompareTag("segundoCaminoCorrecto"))
        {
            if (waypoints.Count > 0)
            {
                currentWaypointIndex = 0;
                targetPosition = waypoints[currentWaypointIndex];
                Debug.Log("Dirigiéndose a lugar a salvo");
                isMoving = true;
            }
            if (botones.Count > 1 && botones[1] != null)
            {
                botones[1].interactable = true;
            }
            PlayTheAudioCorrect(audioWalkCorrect1);
        }
        else if (hit.CompareTag("segundoCaminoIncorrecto"))
        {
            targetPosition = DestinationIncorrectoSegundo;
            Debug.Log("Está pensando pasar por el puente");
            isMoving = true;
            PlayTheAudioCorrect(audioWalkInCorrect2);
        }
        else if (hit.CompareTag("Salir"))
        {
            StartCoroutine(HandleCharacterSwitch());

            if (botones.Count > 1 && botones[2] != null)
            {
                botones[2].interactable = true;
            }
            PlayTheAudioCorrect(audioWalkCorrect2);
        }
        else if (hit.CompareTag("tercerCaminoIncorrecto"))
        {
            targetPosition = DestinationIncorrectoTercer;
            Debug.Log("Camino Incorrecto");
            isMoving = true;
            PlayTheAudioCorrect(audioWalkInCorrect3);
        }
        else
        {
            Debug.Log("Lugar incorrecto");
            return;
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
                targetPosition = DestinationEmpezando;
                Debug.Log("Regresando al punto de inicio...");
            }
            else if (targetPosition == DestinationIncorrectoSegundo)
            {
                targetPosition = DestinationCorrecto;
                Debug.Log("Regresando al punto de inicio...");
            }
            else if (targetPosition == DestinationIncorrectoTercer)
            {
                targetPosition = DestinationEmpezarTercer;
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

    private IEnumerator HandleCharacterSwitch()
    {
        // Fade out the current character
        SpriteRenderer currentRenderer = GetComponent<SpriteRenderer>();
        currentRenderer.color = new Color(currentRenderer.color.r, currentRenderer.color.g, currentRenderer.color.b, 0f);

        // Activate the new character
        newCharacter.SetActive(true);
       

        // Wait for the animation to finish
        yield return new WaitForSeconds(newCharacterAnimator.GetCurrentAnimatorStateInfo(0).length);

        if (GameManagerOsiPersonaje.Instance != null)
        {
            GameManagerOsiPersonaje.Instance.IncrementScore();
        }

        Debug.Log("Terminado");

        SceneManager.LoadSceneAsync("recompensas_juego2");
    }

    private void ShowDestinationMessage(Vector2 position)
    {
        if (position == DestinationCorrecto)
        {
            Debug.Log("Cruzaste sin problemas avanza al siguiente");

            if (GameManagerOsiPersonaje.Instance != null)
            {
                GameManagerOsiPersonaje.Instance.IncrementScore();
            }
        }
        else if (currentWaypointIndex == waypoints.Count - 1)
        {
            Debug.Log("Avanzaste muy bien");

            if (GameManagerOsiPersonaje.Instance != null)
            {
                GameManagerOsiPersonaje.Instance.IncrementScore();
            }
        }
    }

    void UpdateDirection(Vector2 newPosition)
    {
        Vector2 direction = (newPosition - rb.position).normalized;
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
    }
}
