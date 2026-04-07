using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Chapter6
{
    public class WateringController : MonoBehaviour
    {
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Tumble = Animator.StringToHash("Tumble");
        private static readonly int Drag = Animator.StringToHash("Drag");
        private static readonly int Water = Animator.StringToHash("Water");

        [Tooltip("The time in seconds that you have to water the plants")]
        [SerializeField] private int wateringTime = 10;
        [SerializeField] private ChapterSixManager manager;
        [Header("Plants")] [SerializeField]
        private List<PlantController> plants;
        
        private float _progress;
        private Animator _wateringCanAnimator;
        private bool isWatering;

        private RectTransform _rectTransform;
        private Vector3 defaultPosition;
        
        private void Start()
        {
            _wateringCanAnimator = GetComponent<Animator>();
            _rectTransform = GetComponent<RectTransform>();
            defaultPosition = _rectTransform.anchoredPosition;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Square")) return;
            
            isWatering = true;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Square")) return;
            
            _progress += Time.deltaTime;
            WaterAnimation();
            
            if (!(_progress >= wateringTime)) return;
            
            HandleWateringCompletion();
        }

        private void HandleWateringCompletion()
        {
            _rectTransform.anchoredPosition = defaultPosition;
            manager.Next();
            GetComponent<DragAndDrop>().enabled = false;
            GetComponent<Image>().raycastTarget = false;
            foreach (var plant in plants)
            {
                plant.GrowAnimation();
            }
            IdleAnimation();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Square")) return;
            
            isWatering = false;
        }

        public void TumbleAnimation()
        {
            _wateringCanAnimator.SetTrigger(Tumble);
        }
        
        public void WaterAnimation()
        {
            _wateringCanAnimator.SetTrigger(Water);
        }

        public void IdleAnimation()
        {
            _wateringCanAnimator.SetTrigger(Idle);
        }

        public void DragAnimation()
        {
            if (isWatering) return;
            _wateringCanAnimator.SetTrigger(Drag);
        }
    }
}
