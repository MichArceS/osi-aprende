using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Chapter6
{
    public class ChapterSixManager : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("The GameObject for Osi")]
        [SerializeField] private ChapterSixOsiController osi;
        [Tooltip("The GameObject for Screen Touch detection")]
        [SerializeField] private GameObject screenInput;
        [Tooltip("The parent GameObject containing Osi and his cart")]
        [SerializeField] private GameObject player;
        [Tooltip("The parent GameObject that contains Background and Foreground")]
        [SerializeField] private GameObject environment;
        [SerializeField] private GameObject butterflies;
        [SerializeField] private GameObject cart;
        
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
        [SerializeField] private GameObject wateringCan;
        [Tooltip("The groups containing the plants of each act")]
        [SerializeField] private List<GameObject> plantGroups;
        
        [Header("Sounds")]
        [SerializeField] private AudioClip levelMusic;
        [SerializeField] private AudioClip startVoice;
        [SerializeField] private AudioClip act1Voice;
        [SerializeField] private AudioClip act2Voice;
        [SerializeField] private AudioClip act3Voice;
        [SerializeField] private AudioClip act4Voice;
        [SerializeField] private AudioClip act5Voice;
        [SerializeField] private AudioClip act6Voice;
        
        private int _progress;

        // Components
        private RectTransform _playerRectTransform;
        private RectTransform _environmentRectTransform;
        private Button _screenInputButton;
        private Image _screenInputImage;
        
        // Environment Movement
        private static Vector3 PlayerPosition { get; } = new Vector3(-190, -188, 0);
        private static Vector3 PlayerOffsetPosition { get; } = new Vector3(0, -188, 0);
        private static Vector3 PlayerFinalPosition { get; } = new Vector3(235, -188, 0);
        private static Vector3 EnvironmentStartPosition { get; } = new Vector3(6022, 0, 0);
        private static Vector3 EnvironmentAct1Position { get; } = new Vector3(3950, 0, 0);
        private static Vector3 EnvironmentAct2Position { get; } = new Vector3(1845, 0, 0);
        private static Vector3 EnvironmentAct3Position { get; } = new Vector3(-60, 0, 0);
        private static Vector3 EnvironmentAct4Position { get; } = new Vector3(-1982, 0, 0);
        private static Vector3 EnvironmentAct5Position { get; } = new Vector3(-3995, 0, 0);
        private static Vector3 EnvironmentAct6Position { get; } = new Vector3(-5970, 0, 0);
        
        // Gameplay
        private bool _canInteract;

        private void Start()
        {
            _progress = 0;
            _playerRectTransform = player.GetComponent<RectTransform>();
            _environmentRectTransform = environment.GetComponent<RectTransform>();
            _screenInputButton = screenInput.GetComponent<Button>();
            _screenInputImage = screenInput.GetComponent<Image>();
            SwitchInteraction(true);
            
            GlobalCounter.ResetCounters();
            
            AudioController.Instance.PlayMusic(levelMusic);
            AudioController.Instance.PlayVoice(startVoice);
        }

        public void Next()
        {
            if (!_canInteract) return;
            
            _progress += 1;

            switch (_progress)
            {
                case 1: // Act 1 - Stomach ache
                    SwitchInputScreen(false);
                    StartCoroutine(MovementRoutine(EnvironmentStartPosition, EnvironmentAct1Position, () => 
                    {
                        osi.ChangeAnimation(ChapterSixOsiController.StomachAche);
                        EndMovement();
                    }));
                    break;
                case 2: // Act 1 - Heal
                    HandleHealing();
                    MovePlantToCart(act1Plant, act1CartPlant);
                    break;
                case 3: // Act 2 - Sneeze
                    SwitchInputScreen(false);
                    StartCoroutine(MovementRoutine(EnvironmentAct1Position, EnvironmentAct2Position, () => 
                    {
                        osi.ChangeAnimation(ChapterSixOsiController.Sneeze);
                        EndMovement();
                    }));
                    break;
                case 4: // Act 2 - Heal
                    HandleHealing();
                    MovePlantToCart(act2Plant, act2CartPlant);
                    break;
                case 5: // Act 3 - Nervous
                    SwitchInputScreen(false);
                    StartCoroutine(MovementRoutine(EnvironmentAct2Position, EnvironmentAct3Position, () => 
                    {
                        osi.ChangeAnimation(ChapterSixOsiController.Nervous);
                        EndMovement();
                    }));
                    break;
                case 6: // Act 3 - Heal
                    HandleHealing();
                    MovePlantToCart(act3Plant, act3CartPlant);
                    break;
                case 7: // Act 4 - Back pain
                    SwitchInputScreen(false);
                    StartCoroutine(MovementRoutine(EnvironmentAct3Position, EnvironmentAct4Position, () => 
                    {
                        osi.ChangeAnimation(ChapterSixOsiController.Back);
                        EndMovement();
                    }));
                    break;
                case 8: // Act 4 - Heal
                    HandleHealing();
                    MovePlantToCart(act4Plant, act4CartPlant);
                    break;
                case 9: // Break
                    SwitchInputScreen(false);
                    StartCoroutine(MovementRoutine(EnvironmentAct4Position, EnvironmentAct5Position, () => 
                    {
                        osi.ChangeAnimation(ChapterSixOsiController.Idle);
                        EndMovement();
                        SwitchInputScreen(true);
                    }));
                    break;
                case 10: // Act 5 - Garden
                    SwitchInputScreen(false);
                    StartCoroutine(MovementRoutine(EnvironmentAct5Position, EnvironmentAct6Position, () => 
                    {
                        osi.ChangeAnimation(ChapterSixOsiController.Idle);
                        EndMovement();
                        act1CartPlant.GetComponent<DragAndDrop>().enabled = true;
                        act2CartPlant.GetComponent<DragAndDrop>().enabled = true;
                        act3CartPlant.GetComponent<DragAndDrop>().enabled = true;
                        act4CartPlant.GetComponent<DragAndDrop>().enabled = true;
                        act1CartPlant.GetComponent<Image>().raycastTarget = true;
                        act2CartPlant.GetComponent<Image>().raycastTarget = true;
                        act3CartPlant.GetComponent<Image>().raycastTarget = true;
                        act4CartPlant.GetComponent<Image>().raycastTarget = true;
                        cart.GetComponent<Image>().raycastTarget = false;
                    }));
                    break;
                case 14: // Act 6 - Watering
                    wateringCan.GetComponent<DragAndDrop>().enabled = true;
                    wateringCan.GetComponent<WateringController>().TumbleAnimation();
                    break;
                case 15: // End
                    SwitchInputScreen(false);
                    // Animacion de plantas
                    osi.ChangeAnimation(ChapterSixOsiController.Jump);
                    butterflies.SetActive(true);
                    break;
            }
        }

        private void SwitchInteraction(bool interactable)
        {
            _canInteract = interactable;
            var plantsDragAndDrops = plantGroups.Select(sector => sector.GetComponentsInChildren<DragAndDrop>())
                .SelectMany(plants => plants);
            foreach (var dragAndDrop in plantsDragAndDrops)
            {
                dragAndDrop.enabled = interactable;
            }
        }

        private void HandleHealing()
        {
            SwitchInteraction(false);
            SwitchInputScreen(true);
            StartCoroutine(osi.WaitForHeal(() =>
            {
                SwitchInteraction(true);
            }));
            osi.ChangeAnimation(ChapterSixOsiController.Heal);
        }

        private void EndMovement()
        {
            SwitchInteraction(true);
            ReplayVoice();
        }

        public void Success()
        {
            Next();
            GlobalCounter.IncrementarAciertos();
        }

        public void Failure()
        {
            GlobalCounter.IncrementarNoAciertos();
        }

        public void ReplayVoice()
        {
            switch (_progress)
            {
                case 0:
                    if (startVoice == null) return;
                    AudioController.Instance.PlayVoice(startVoice);
                    break;
                case 1:
                case 2:
                    if (act1Voice == null) return;
                    AudioController.Instance.PlayVoice(act1Voice);
                    break;
                case 3:
                case 4:
                    if (act2Voice == null) return;
                    AudioController.Instance.PlayVoice(act2Voice);
                    break;
                case 5:
                case 6:
                    if (act3Voice == null) return;
                    AudioController.Instance.PlayVoice(act3Voice);
                    break;
                case 7:
                case 8:
                    if (act4Voice == null) return;
                    AudioController.Instance.PlayVoice(act4Voice);
                    break;
                case 9:
                    if (act5Voice == null) return;
                    AudioController.Instance.PlayVoice(act5Voice);
                    break;
                case 10:
                case 11:
                case 12:
                case 13:
                    if (act6Voice == null) return;
                    AudioController.Instance.PlayVoice(act6Voice);
                    break;
            }
        }

        private void SwitchInputScreen(bool state)
        {
            _screenInputImage.raycastTarget = state;
            _screenInputButton.interactable = state;
        }

        private static void MovePlantToCart(GameObject plant, GameObject cartPlant)
        {
            plant.SetActive(false);
            cartPlant.SetActive(true);
        }
    
        public void PlaySfx(AudioClip audioClip)
        {
            if (audioClip == null) return;
            AudioController.Instance.PlaySfx(audioClip);
        }
    
        public void PlayVoice(AudioClip audioClip)
        {
            if (audioClip == null) return;
            AudioController.Instance.PlayVoice(audioClip);
        }

        private IEnumerator MovementRoutine(Vector3 envStart, Vector3 envEnd, Action onComplete = null)
        {
            var elapsedTime = 0f;
            
            osi.ChangeAnimation(ChapterSixOsiController.Walk);
            SwitchInteraction(false);
            
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

                if (_progress < 10)
                {
                    var playerT = Mathf.Sin(t * Mathf.PI);
                    _playerRectTransform.anchoredPosition3D = Vector3.Lerp(PlayerPosition, PlayerOffsetPosition, playerT);
                }
                else
                {
                    _playerRectTransform.anchoredPosition3D = Vector3.Lerp(PlayerPosition, PlayerFinalPosition, easedT);
                }

                yield return null;
            }

            // Ensure that the player is where it had to go
            _environmentRectTransform.anchoredPosition3D = envEnd;
            _playerRectTransform.anchoredPosition3D = _progress < 10 ? PlayerPosition : PlayerFinalPosition;
            
            // Invoke actions after coroutine completion
            onComplete?.Invoke();
        }
    }
}
