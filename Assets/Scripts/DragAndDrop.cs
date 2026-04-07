using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

[Serializable]
public class DropEvent : UnityEvent<PointerEventData> { }

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform _rectTransform;
    private Canvas _canvas;
    private CanvasGroup _canvasGroup;
    private Vector3 _initialPosition;
    private Vector2 _initialSizeDelta;
    
    [Tooltip("The GameObject that represents the final destination of the dragging (Can be multiple destinations)")]
    [SerializeField] private List<GameObject> destination;
    [SerializeField] private float initialDelay = 0.5f;
    
    [Header("Events")]
    public DropEvent onDropSuccess;
    public DropEvent onDropFailed;
    public DropEvent onDragStart;
    public DropEvent onDragStay;
    public DropEvent onDragEnd;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        _canvasGroup = GetComponent<CanvasGroup>();

        StartCoroutine(CaptureInitialPositionAfterDelay(initialDelay));
    }

    private IEnumerator CaptureInitialPositionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        _initialPosition = _rectTransform.anchoredPosition;
        _initialSizeDelta = _rectTransform.sizeDelta;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        onDragStart?.Invoke(eventData);
        if (_canvasGroup == null) return;
        
        // Prevent additional interactions from being detected while dragging.
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.alpha = 0.8f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        onDragStay?.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Return interactions with the object to normal
        if (_canvasGroup != null)
        {
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = 1f;
        }

        if (HasDestination() && IsDestination(eventData.pointerCurrentRaycast.gameObject))
        {
            onDropSuccess?.Invoke(eventData);
        }
        else
        {
            _rectTransform.anchoredPosition = _initialPosition;
            onDropFailed?.Invoke(eventData);
        }
        
        onDragEnd?.Invoke(eventData);
    }

    private bool HasDestination()
    {
        return  destination is { Count: > 0 };
    }

    private bool IsDestination(GameObject raycastObject)
    {
        return destination.Any(option => option.GetInstanceID() == raycastObject.GetInstanceID());
    }

    public void ResetState()
    {
        _rectTransform.anchoredPosition = _initialPosition;
        _rectTransform.sizeDelta = _initialSizeDelta;
        if (_canvasGroup != null)
        {
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = 1f;
        }
        enabled = true;
    }
}
