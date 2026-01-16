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

    public string tagNameObjet;
    public Transform dragLayer;
    public ScrollRect scrollRect;

    public int totalAciertosEscena = 3;

    private Transform originalParent;
    private float savedScrollPosition;

    // Audio para acierto y error
    public AudioClip audioAcierto;
    public AudioClip audioError;

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

            canvasGroup.blocksRaycasts = false;
            enabled = false;

            // Sonido de acierto
            AudioController.Instance.PlayVoice(audioAcierto);

            // Notifica al GameProgressManager
            if (GameProgressManager.instance != null)
            {
                GameProgressManager.instance.RegistrarAciertoEscena(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, totalAciertosEscena);
            }
        }
        else
        {
            transform.SetParent(originalParent, false);
            rectTransform.anchoredPosition = initialPosition;
            rectTransform.sizeDelta = initialSizeDelta;

            if (scrollRect != null)
                scrollRect.verticalNormalizedPosition = savedScrollPosition;

            // Sonido de error
            AudioController.Instance.PlayVoice(audioError);
        }
    }
}
