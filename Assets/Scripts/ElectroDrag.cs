using UnityEngine;
using UnityEngine.EventSystems;

public class ElectroDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;

    private Vector3 initialPos;
    private LineRenderer currentLine;

    public JuegoElectrodomesticos manager;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        initialPos = rectTransform.position;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

        currentLine = Instantiate(manager.prefabLinea, manager.transform).GetComponent<LineRenderer>();
        currentLine.positionCount = 2;
        currentLine.SetPosition(0, initialPos);
        currentLine.SetPosition(1, initialPos);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position += (Vector3)(eventData.delta / canvas.scaleFactor);
        currentLine.SetPosition(1, rectTransform.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        GameObject target = eventData.pointerCurrentRaycast.gameObject;

        if (target != null && target.GetComponent<ObjetoDrop>() != null)
        {
            if (manager.EsRelacionCorrecta(gameObject, target))
            {
                currentLine.SetPosition(1, target.transform.position);
                manager.RegistrarLineaCorrecta(currentLine);
                rectTransform.position = initialPos;
                enabled = false;
                return;
            }
        }

        Destroy(currentLine.gameObject);
        rectTransform.position = initialPos;
    }
}
