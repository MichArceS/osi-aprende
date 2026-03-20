using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Chapter6
{
    public class ChapterSixManager : MonoBehaviour
    {
        private static readonly int Walk = Animator.StringToHash("Walk");
        private static readonly int Act1Anim = Animator.StringToHash("Act1");
        private static readonly int Solve1Anim = Animator.StringToHash("Solve1");

        [Header("References")]
        [Tooltip("The GameObject for Osi")]
        [SerializeField] private GameObject osi;
        [SerializeField] private GameObject screenInput;
        [Tooltip("The parent GameObject containing Osi and his cart")]
        [SerializeField] private GameObject player;
        [Tooltip("The parent GameObject that contains Background and Foreground")]
        [SerializeField] private GameObject environment;
        [Header("Movement")]
        [Tooltip("The time in seconds that Osi will take to move from one position to another")]
        [SerializeField] private float transitionTime = 3;
        [Header("Plants")]
        [SerializeField] private GameObject act1Plant;
        [SerializeField] private GameObject act1CartPlant;
        [SerializeField] private GameObject act2Plant;
        [SerializeField] private GameObject act2CartPlant;
        [SerializeField] private GameObject act3Plant;
        [SerializeField] private GameObject act3CartPlant;
        [SerializeField] private GameObject act4Plant;
        [SerializeField] private GameObject act4CartPlant;
        
        private int _progress;

        // Components
        private Animator _osiAnimator;
        private RectTransform _playerRectTransform;
        private RectTransform _environmentRectTransform;
        private Button _screenInputButton;
        private Image _screenInputImage;
        
        // Osi Movement
        private static Vector3 PlayerPosition { get; } = new Vector3(-190, -188, 0);
        private static Vector3 PlayerOffsetPosition { get; } = new Vector3(0, -188, 0);
        private static Vector3 EnvironmentStartPosition { get; } = new Vector3(6022, 0, 0);
        private static Vector3 EnvironmentAct1Position { get; } = new Vector3(3950, 0, 0);
        private static Vector3 EnvironmentAct2Position { get; } = new Vector3(1845, 0, 0);
        private static Vector3 EnvironmentAct3Position { get; } = new Vector3(-60, 0, 0);
        private static Vector3 EnvironmentAct4Position { get; } = new Vector3(-1982, 0, 0);
        
        // Gameplay
        private bool _canInteract;

        private void Start()
        {
            _progress = 0;
            _osiAnimator = osi.GetComponent<Animator>();
            _playerRectTransform = player.GetComponent<RectTransform>();
            _environmentRectTransform = environment.GetComponent<RectTransform>();
            _screenInputButton = screenInput.GetComponent<Button>();
            _screenInputImage = screenInput.GetComponent<Image>();
            _canInteract = true;
        }

        public void Next()
        {
            if (!_canInteract) return;
            
            _progress += 1;

            switch (_progress)
            {
                case 1:
                    Act1();
                    break;
                case 2:
                    Solve1();
                    break;
                case 3:
                    Act2();
                    break;
                case 4:
                    Solve2();
                    break;
                case 5:
                    Act3();
                    break;
                case 6:
                    Solve3();
                    break;
                case 7:
                    Act4();
                    break;
                case 8:
                    Solve4();
                    break;
            }
        }

        private void Act1()
        {
            SwitchInputScreen(false);
            StartCoroutine(MovementRoutine(EnvironmentStartPosition, EnvironmentAct1Position, () => 
            {
                _osiAnimator.SetTrigger(Act1Anim);
                _canInteract = true;
            }));
        }

        private void Solve1()
        {
            SwitchInputScreen(true);
            _osiAnimator.SetTrigger(Solve1Anim);
            MovePlantToCart(act1Plant, act1CartPlant);
        }

        private void Act2()
        {
            SwitchInputScreen(false);
            StartCoroutine(MovementRoutine(EnvironmentAct1Position, EnvironmentAct2Position, () => 
            {
                _osiAnimator.SetTrigger(Act1Anim);
                _canInteract = true;
            }));
        }

        private void Solve2()
        {
            SwitchInputScreen(true);
            _osiAnimator.SetTrigger(Solve1Anim);
            MovePlantToCart(act2Plant, act2CartPlant);
        }

        private void Act3()
        {
            SwitchInputScreen(false);
            StartCoroutine(MovementRoutine(EnvironmentAct2Position, EnvironmentAct3Position, () => 
            {
                _osiAnimator.SetTrigger(Act1Anim);
                _canInteract = true;
            }));
        }

        private void Solve3()
        {
            SwitchInputScreen(true);
            _osiAnimator.SetTrigger(Solve1Anim);
            MovePlantToCart(act3Plant, act3CartPlant);
        }

        private void Act4()
        {
            SwitchInputScreen(false);
            StartCoroutine(MovementRoutine(EnvironmentAct3Position, EnvironmentAct4Position, () => 
            {
                _osiAnimator.SetTrigger(Act1Anim);
                _canInteract = true;
            }));
        }

        private void Solve4()
        {
            SwitchInputScreen(true);
            _osiAnimator.SetTrigger(Solve1Anim);
            MovePlantToCart(act4Plant, act4CartPlant);
        }

        private void SwitchInputScreen(bool state)
        {
            _screenInputImage.raycastTarget = state;
            _screenInputButton.interactable = state;
        }

        private void MovePlantToCart(GameObject plant, GameObject cartPlant)
        {
            plant.SetActive(false);
            cartPlant.SetActive(true);
        }

        private IEnumerator MovementRoutine(Vector3 envStart, Vector3 envEnd, Action onComplete = null)
        {
            var elapsedTime = 0f;
            _osiAnimator.SetTrigger(Walk);
            _canInteract = false;

            // Ensure the player is where it should to start
            _environmentRectTransform.anchoredPosition3D = envStart;
            _playerRectTransform.anchoredPosition3D = PlayerPosition;

            // Moves the player to the new position (for this we move the environment and slightly osi on screen)
            while (elapsedTime < transitionTime)
            {
                elapsedTime += Time.deltaTime;
                
                var t = elapsedTime / transitionTime;
                var easedT = Mathf.SmoothStep(0f, 1f, t);

                _environmentRectTransform.anchoredPosition3D = Vector3.Lerp(envStart, envEnd, easedT);

                var playerT = Mathf.Sin(t * Mathf.PI);
                _playerRectTransform.anchoredPosition3D = Vector3.Lerp(PlayerPosition, PlayerOffsetPosition, playerT);

                yield return null;
            }

            // Ensure that the player is where it had to go
            _environmentRectTransform.anchoredPosition3D = envEnd;
            _playerRectTransform.anchoredPosition3D = PlayerPosition;
            
            // Invoke actions after coroutine completion
            onComplete?.Invoke();
        }
    }
}
