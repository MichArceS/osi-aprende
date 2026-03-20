using UnityEngine;

public class CreditsScroller : MonoBehaviour
{
    [Header("Configuración de Scroll")]
    [Tooltip("Qué tan rápido se moverá la imagen.")]
    public float velocidad = 50f;
    
    [Tooltip("El punto en el eje Y donde la imagen debe detenerse y reiniciarse.")]
    public float posicionYFinal = 1500f; 
    
    [Tooltip("Dirección del movimiento. Por lo general los créditos suben (0, 1).")]
    public Vector2 direccion = Vector2.up; 

    private RectTransform rectTransform;
    private Vector2 posicionInicial;

    void Start()
    {
        // Obtenemos el componente RectTransform de la imagen UI
        rectTransform = GetComponent<RectTransform>();
        
        // Guardamos la posición original para poder regresar a ella
        posicionInicial = rectTransform.anchoredPosition;
    }

    void Update()
    {
        // Movemos la imagen suavemente en base a la velocidad y el tiempo
        rectTransform.anchoredPosition += direccion * velocidad * Time.deltaTime;

        // Comprobamos si la imagen ya llegó a su destino (asumiendo que se mueve hacia arriba)
        if (direccion == Vector2.up && rectTransform.anchoredPosition.y >= posicionYFinal)
        {
            ReiniciarCreditos();
        }
        // Condición alternativa por si decides que la imagen se mueva hacia abajo
        else if (direccion == Vector2.down && rectTransform.anchoredPosition.y <= posicionYFinal)
        {
            ReiniciarCreditos();
        }
    }

    public void ReiniciarCreditos()
    {
        // Devuelve la imagen a su punto de origen
        rectTransform.anchoredPosition = posicionInicial;
    }
}