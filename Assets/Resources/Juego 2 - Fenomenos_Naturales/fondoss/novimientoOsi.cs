using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class novimientoOsi : MonoBehaviour
{
    private Rigidbody2D rb2D;

    [Header("Movimiento")]
    private float movimientoHorizontal;
    private float movimientoVertical;

    [SerializeField] private float velocidadDeMovimiento;
    [SerializeField] private Vector2 direccion;

    [Header("Animacion")]
    private Animator animator;
    private bool movimientoIniciado = false;

    [Header("UI")]
    [SerializeField] private List<Button> botones;

    [Header("Personaje Nuevo")]
    [SerializeField] private GameObject nuevoPersonaje;

    [Header("Segundo Personaje")]
    [SerializeField] private GameObject segundoPersonaje;

    [Header("Objetos a Desactivar")]
    [SerializeField] private GameObject objeto1;
    [SerializeField] private GameObject objeto2;
    [SerializeField] private GameObject objeto3;

    [Header("Objetos a Activar")]
    [SerializeField] private GameObject objeto0;

    // Contador para el número de clics en los botones
    private int contador = 0;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Desactivar todos los botones al iniciar
        foreach (Button boton in botones)
        {
            if (boton != null)
            {
                boton.interactable = false;
            }
        }

        if (botones.Count > 0)
        {
            botones[0].onClick.AddListener(OnBotonClickeado);
        }

        if (botones.Count > 1)
        {
            botones[1].onClick.AddListener(OnSegundoBotonClickeado);
        }
    }

    private void Update()
    {
        movimientoHorizontal = Input.GetAxisRaw("Horizontal");
        movimientoVertical = Input.GetAxisRaw("Vertical");

        // Iniciar movimiento solo si hay entrada
        if (!movimientoIniciado && (movimientoHorizontal != 0 || movimientoVertical != 0))
        {
            movimientoIniciado = true;
            animator.SetBool("IsMoving", true);
        }

        animator.SetFloat("MovimientoX", movimientoHorizontal);
        animator.SetFloat("MovimientoY", movimientoVertical);

        if (movimientoHorizontal != 0 || movimientoVertical != 0)
        {
            animator.SetFloat("UltimoX", movimientoHorizontal);
            animator.SetFloat("UltimoY", movimientoVertical);
        }

        direccion = new Vector2(movimientoHorizontal, movimientoVertical).normalized;
    }

    private void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + direccion * velocidadDeMovimiento * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("ZonaDeEncuentro"))
        {
            Debug.Log("¡Llegaste al lugar correcto, continúa!");

            if (botones.Count > 0 && botones[0] != null)
            {
                botones[0].interactable = true;
            }
        }

        if (other.gameObject.CompareTag("ZonaMochila"))
        {
            Debug.Log("¡Llegaste al lugar correcto, Recoge la mochila!");

            if (contador == 1)
            {
                if (botones.Count > 1 && botones[1] != null)
                {
                    botones[1].interactable = true;
                }
            }
            else
            {
                Debug.Log("¡Primero realiza la primera accion debajo de la mesa!");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("ZonaDeEncuentro"))
        {
            if (botones.Count > 0 && botones[0] != null)
            {
                botones[0].interactable = false; 
            }
        }

        if (other.gameObject.CompareTag("ZonaMochila"))
        {
            if (botones.Count > 1 && botones[1] != null)
            {
                botones[1].interactable = false;
            }
        }
    }

    private void OnBotonClickeado()
    {
        if (contador == 0)
        {
            Debug.Log("¡A salvo!");

            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.FadeOutAndBack(gameObject, DeactivateNewCharacter);
            }

            if (nuevoPersonaje != null)
            {
                nuevoPersonaje.SetActive(true);
            }

            contador++;
        }
    }

    private void OnSegundoBotonClickeado()
    {
        if (contador == 1)
        {
            Debug.Log("¡Activando segundo personaje y desactivando objetos!");

            if (objeto1 != null)
            {
                objeto1.SetActive(false);
            }

            if (objeto2 != null)
            {
                objeto2.SetActive(false);
            }
            if (objeto3 != null)
            {
                objeto3.SetActive(false);
            }
            if (objeto0 != null)
            {
                objeto0.SetActive(true);
            }

            if (segundoPersonaje != null)
            {
                segundoPersonaje.SetActive(true);
            }
            contador++;
        }
        
    }

    private void DeactivateNewCharacter()
    {
        if (nuevoPersonaje != null)
        {
            nuevoPersonaje.SetActive(false);
        }

        if (botones.Count > 0 && botones[0] != null)
        {
            botones[0].interactable = false;
        }
    }
}
