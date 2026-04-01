using UnityEngine;
using UnityEngine.EventSystems;

namespace Chapter1
{
    public class ClothesController : MonoBehaviour
    {
        public void OnDragSuccessful(PointerEventData eventData)
        {
            var rectTransform = eventData.pointerDrag.gameObject.GetComponent<RectTransform>();
            var canvasGroup = eventData.pointerDrag.gameObject.GetComponent<CanvasGroup>();
            var droppableRect = eventData.pointerCurrentRaycast.gameObject.GetComponent<RectTransform>();

            rectTransform.position = droppableRect.position;
            rectTransform.sizeDelta = droppableRect.sizeDelta;

            canvasGroup.blocksRaycasts = false;
            enabled = false;

            if (eventData.pointerCurrentRaycast.gameObject.CompareTag("DroppableMedias"))
            {
                // Desactiva el objeto con la etiqueta
                var objetoDesactivar = GameObject.FindGameObjectWithTag("DroppableMedias");

                if (objetoDesactivar != null)
                {
                    objetoDesactivar.SetActive(false);
                }
            }

            // Incrementa el contador global de aciertos
            GlobalCounter.IncrementarAciertos();
        }

        public void OnDragCancelled(PointerEventData eventData)
        {
            GlobalCounter.IncrementarNoAciertos();
        }
    }
}
