using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Drag_Drop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Vector3 initialPosition;
    private Vector2 initialSizeDelta;
    private bool isDropped = false;

    public string tagNameObjet;       // Etiqueta del área donde se debe soltar
    public Transform dragLayer;       // Asignar en inspector: GameObject DragLayer (hijo directo de Canvas)
    public ScrollRect scrollRect;     // Asignar en el Inspector: el ScrollRect que contiene este objeto
    public string nombreEscenaRecompensa; // Nombre de la escena de recompensa

    private Transform originalParent;
    private float savedScrollPosition;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        originalParent = transform.parent;

        StartCoroutine(CaptureInitialPositionAfterDelay(0.5f));
    }

    private IEnumerator CaptureInitialPositionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        initialPosition = rectTransform.anchoredPosition;
        initialSizeDelta = rectTransform.sizeDelta;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (scrollRect != null)
            savedScrollPosition = scrollRect.verticalNormalizedPosition;

        transform.SetParent(dragLayer);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.8f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        GameObject target = eventData.pointerCurrentRaycast.gameObject;

        if (target != null && target.CompareTag(tagNameObjet))
        {
            RectTransform dropZone = target.GetComponent<RectTransform>();

            transform.SetParent(dropZone);
            rectTransform.anchorMin = dropZone.anchorMin;
            rectTransform.anchorMax = dropZone.anchorMax;
            rectTransform.pivot = dropZone.pivot;
            rectTransform.localPosition = Vector3.zero;
            rectTransform.localScale = dropZone.localScale;
            rectTransform.sizeDelta = dropZone.sizeDelta;

            isDropped = true;
            GlobalCounter.IncrementarAciertos();

            // Verifica si se alcanzaron los 3 aciertos
            if (GlobalCounter.ObtenerAciertosTotales() >= 3)
            {
                SceneManager.LoadScene(nombreEscenaRecompensa);
            }
        }
        else
        {
            transform.SetParent(originalParent, false);
            rectTransform.anchoredPosition = initialPosition;
            rectTransform.sizeDelta = initialSizeDelta;

            if (scrollRect != null)
                scrollRect.verticalNormalizedPosition = savedScrollPosition;

            GlobalCounter.IncrementarNoAciertos();
        }
    }

    private void Update()
    {
        if (isDropped)
        {
            canvasGroup.blocksRaycasts = false;
            enabled = false;
        }
    }

    public void ResetState()
    {
        transform.SetParent(originalParent, false);
        rectTransform.anchoredPosition = initialPosition;
        rectTransform.sizeDelta = initialSizeDelta;
        isDropped = false;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        enabled = true;
    }
}
