using UnityEngine;
using UnityEngine.EventSystems;

namespace Chapter6
{
    public class PlantController : MonoBehaviour
    {
        private static readonly int Plant = Animator.StringToHash("Plant");
        private static readonly int Grow = Animator.StringToHash("Grow");
    
        private Animator _plantAnimator;

        private void Start()
        {
            _plantAnimator = GetComponent<Animator>();
        }

        public void PlantAnimation()
        {
            _plantAnimator.SetTrigger(Plant);
        }

        public void GrowAnimation()
        {
            _plantAnimator.SetTrigger(Grow);
        }

        public void PositionCorrection(PointerEventData eventData)
        {
            var finalPosition = eventData.pointerCurrentRaycast.gameObject.GetComponent<RectTransform>().position;
            var rectTransform = GetComponent<RectTransform>();
            
            rectTransform.position = finalPosition;
        }
    }
}
