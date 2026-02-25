using UnityEngine;
using UnityEngine.EventSystems;

public class CardItem : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("Configuración")]
    public int cardID;
    public bool isSource;
    public bool isMatched = false;

    private MatchGameManager gameManager;
    private RectTransform rectTransform;

    void Start()
    {
        gameManager = FindFirstObjectByType<MatchGameManager>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isSource && !isMatched)
        {
            gameManager.StartDrawing(this);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isSource && !isMatched)
        {
            gameManager.UpdateDrawing(Input.mousePosition);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isSource && !isMatched)
        {
            GameObject hitObject = eventData.pointerCurrentRaycast.gameObject;
            CardItem targetCard = null;

            if (hitObject != null)
            {
                targetCard = hitObject.GetComponent<CardItem>();
                if (targetCard == null) targetCard = hitObject.GetComponentInParent<CardItem>();
            }
            gameManager.FinishDrawing(this, targetCard);
        }
    }

    public Vector3 GetAnchorPosition()
    {
        return rectTransform.position;
    }
}