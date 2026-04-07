using System;
using System.Collections;
using UnityEngine;

namespace Chapter6
{
    public class ChapterSixOsiController : MonoBehaviour
    {
        public static readonly int Idle = Animator.StringToHash("Idle");
        public static readonly int Walk = Animator.StringToHash("Walk");
        public static readonly int StomachAche = Animator.StringToHash("Stomach");
        public static readonly int Sneeze = Animator.StringToHash("Sneeze");
        public static readonly int Nervous = Animator.StringToHash("Nervous");
        public static readonly int Back = Animator.StringToHash("Back");
        public static readonly int Heal = Animator.StringToHash("Heal");
        public static readonly int Jump = Animator.StringToHash("Jump");
        public static readonly int Talk = Animator.StringToHash("Talk");

        [SerializeField] private float healingTime = 3f;
        
        private Animator _osiAnimator;
        
        // [SerializeField] private GameObject osi;

        private void Start()
        {
            _osiAnimator = GetComponent<Animator>();
        }

        public void ChangeAnimation(int animationHash)
        {
            _osiAnimator.SetTrigger(animationHash);
        }

        public IEnumerator WaitForHeal(Action onComplete)
        {
            yield return new WaitForSeconds(healingTime);
            onComplete?.Invoke();
        }
    }
}
