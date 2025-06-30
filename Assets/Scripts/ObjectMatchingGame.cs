using UnityEngine;

public class ObjectMatchingGame : MonoBehaviour
{
    private LineRenderer previewLine;
    private Transform firstCard;
    private Transform secondCard;

    private ObjectMatchForm firstForm;
    private ObjectMatchForm secondForm;

    private bool isSelecting = false;

    // Audios para acierto y error
    public AudioClip aciertoClip;
    public AudioClip errorClip;
    private AudioSource audioSource;

    void Start()
    {
        previewLine = GetComponent<LineRenderer>();
        previewLine.positionCount = 2;
        previewLine.material = new Material(Shader.Find("Sprites/Default"));
        previewLine.startColor = Color.red;
        previewLine.endColor = Color.red;
        previewLine.startWidth = 0.15f;
        previewLine.endWidth = 0.15f;
        previewLine.useWorldSpace = true;
        previewLine.enabled = false;

        // Crear AudioSource si no existe
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null)
            {
                ObjectMatchForm form = hit.collider.GetComponent<ObjectMatchForm>();

                if (form != null)
                {
                    if (form.transform == firstCard)
                    {
                        Debug.LogWarning("⚠️ No puedes seleccionar la misma carta dos veces.");
                        return;
                    }

                    if (!isSelecting)
                    {
                        firstCard = form.transform;
                        firstForm = form;
                        isSelecting = true;

                        previewLine.enabled = true;
                        previewLine.SetPosition(0, firstCard.position);
                        previewLine.SetPosition(1, firstCard.position);
                    }
                    else
                    {
                        secondCard = form.transform;
                        secondForm = form;

                        if (firstForm != null && secondForm != null && secondCard != null)
                        {
                            previewLine.SetPosition(1, secondCard.position);

                            if (firstForm.Get_ID() == secondForm.Get_ID())
                            {
                                Debug.Log("✅ ¡Emparejadas!");
                                CrearLineaPermanente(firstCard.position, secondCard.position);
                                ReproducirSonido(aciertoClip);
                                // Aquí puedes desactivar las cartas si quieres
                            }
                            else
                            {
                                Debug.Log("❌ No coinciden");
                                ReproducirSonido(errorClip);
                                StartCoroutine(BorrarLineaConRetraso());
                            }
                        }

                        isSelecting = false;
                        firstCard = null;
                        secondCard = null;
                        firstForm = null;
                        secondForm = null;
                    }
                }
            }
        }

        if (isSelecting && firstCard != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            previewLine.SetPosition(1, mousePos);
        }
    }

    private System.Collections.IEnumerator BorrarLineaConRetraso()
    {
        yield return new WaitForSeconds(0.5f);
        previewLine.enabled = false;
    }

    private void CrearLineaPermanente(Vector3 start, Vector3 end)
    {
        GameObject newLine = new GameObject("LineaEmparejada");
        LineRenderer lr = newLine.AddComponent<LineRenderer>();

        lr.positionCount = 2;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = Color.green;
        lr.endColor = Color.green;
        lr.startWidth = 0.15f;
        lr.endWidth = 0.15f;
        lr.useWorldSpace = true;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

    private void ReproducirSonido(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
