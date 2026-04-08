using UnityEngine;
using UnityEngine.SceneManagement;

namespace JO
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance;

        public PlayerManager player;
        PlayerControls playerControls;

        [Header("Movement Input")]
        [SerializeField] Vector2 movementInput;
        public float verticalInput;
        public float horizontalInput;
        public float moveAmount;

        [Header("Camera Input")]
        [SerializeField] public Vector2 cameraInput;
        public float cameraVerticalInput;
        public float cameraHorizontalInput;

        [Header("Lock On Input")]
        [SerializeField] bool lockOnInput = false;
        [SerializeField] bool lockOnRightInput = false;
        [SerializeField] bool lockOnLeftInput = false;
        private Coroutine lockOnCoroutine;

        [Header("Player Action Input")]
        [SerializeField] bool dodgeInput = false;
        [SerializeField] bool sprintInput = false;
        [SerializeField] bool jumpInput = false;
        [SerializeField] bool lightAttackInput = false;
        [SerializeField] bool heavyAttackInput = false;
        [SerializeField] bool healingInput = false;
        [SerializeField] public bool interactInput = false;
        [SerializeField] bool TwoHandInput = false;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }  
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            SceneManager.activeSceneChanged += OnSceneChange;

            instance.enabled = false;
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            if (newScene.buildIndex == WorldSaveManager.instance.GetWorldIdSceneIndex())
            {
                instance.enabled = true;
                lightAttackInput = false;
            }
            else
            {
                instance.enabled = false;
            }
        }
        
        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();

                playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
                playerControls.PlayerCamera.CameraControls.performed += i => cameraInput = i.ReadValue<Vector2>();
                playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;
                playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
                playerControls.PlayerActions.LightAttack.performed += i => lightAttackInput = true;
                playerControls.PlayerActions.HeavyAttack.performed += i => heavyAttackInput = true;
                playerControls.PlayerActions.Healing.performed += i => healingInput = true;
                playerControls.PlayerActions.Interact.performed += i => interactInput = true;
                playerControls.PlayerActions.TwoHand.performed += i => TwoHandInput = true;

                playerControls.PlayerActions.LockOn.performed += i => lockOnInput = true;
                playerControls.PlayerActions.SeekLeftLockOnTarget.performed += i => lockOnLeftInput = true;
                playerControls.PlayerActions.SeekRightLockOnTarget.performed += i => lockOnRightInput = true;

                playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
                playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;
            }

            playerControls.Enable();
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChange;
        }

        private void OnApplicationFocus(bool focus)
        {
            if (enabled)
            {
                if(focus)
                {
                    playerControls.Enable();
                }
                else
                {  
                    playerControls.Disable();
                }
            }
        }

        private void Update()
        {
            HandleAllInputs();
        }

        private void HandleAllInputs()
        {
            HandleTwoHandInput();
            HandleInteractInput();
            HandleLockOnInput();
            HandleLockOnSwitchTargetInput();
            HandleMovementInput();
            HandleCameraMovementInput();
            HandleDodgeInput();
            HandleSprintInput();
            HandleJumpInput();
            HandleAttackInput();
            HandleHealingInput();
        }

        private void HandleTwoHandInput()
        {
            if (player.isPerforminAction)
                return;

            if (TwoHandInput && player.TH_Equiped)
            {
                TwoHandInput = false;
                player.TH_Equiped = false;
            }

            if (TwoHandInput && !player.TH_Equiped)
            {
                TwoHandInput = false;
                player.TH_Equiped = true;
            }
        }

        private void HandleInteractInput()
        {
            if (interactInput)
            {
                interactInput = false;
            }
        }

        private void HandleLockOnInput()
        {
            if (player.isLockedOn)
            {
                if (player.playerAttackManager.currentTarget == null)
                    return;
            }

            if (lockOnInput && player.isLockedOn)
            {
                lockOnInput = false;
                PlayerCamera.instance.ClearLockOnTargets();
                player.isLockedOn = false;
                return;
            }

            if (lockOnInput && !player.isLockedOn)
            {
                lockOnInput = false;

                PlayerCamera.instance.HandleLocatingLockOnTargets();

                if (PlayerCamera.instance.nearestLockOnTarget != null)
                {
                    player.playerAttackManager.SetTarget(PlayerCamera.instance.nearestLockOnTarget);
                    player.isLockedOn = true;
                }
            }
        }

        private void HandleLockOnSwitchTargetInput()
        {
            if (lockOnLeftInput)
            {
                lockOnLeftInput = false;

                if (player.isLockedOn)
                {
                    PlayerCamera.instance.HandleLocatingLockOnTargets();

                    if (PlayerCamera.instance.leftLockOnTarget != null)
                    {
                        player.playerAttackManager.SetTarget(PlayerCamera.instance.leftLockOnTarget);
                    }
                }
            }

            if (lockOnRightInput)
            {
                lockOnRightInput = false;

                if (player.isLockedOn)
                {
                    PlayerCamera.instance.HandleLocatingLockOnTargets();

                    if (PlayerCamera.instance.rightLockOnTarget != null)
                    {
                        player.playerAttackManager.SetTarget(PlayerCamera.instance.rightLockOnTarget);
                    }
                }
            }
        }

        private void HandleMovementInput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;

            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput)+ Mathf.Abs(horizontalInput));

            if (moveAmount <= 0.5 && moveAmount > 0)
            {
                moveAmount = 0.5f;
            }
            else if (moveAmount > 0.5 && moveAmount <= 1)
            {
                moveAmount = 1;
            }

            if (player == null)
                return;

            // if we are not locked on, only use the move amount
            if (!player.isLockedOn || player.isSprinting)
            {
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.isSprinting);
            }
            // if we are locked on target pass the horizontal movement as well
            else
            {
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontalInput, verticalInput, player.isSprinting);
            }
        }

        private void HandleCameraMovementInput()
        {
            cameraVerticalInput = cameraInput.y;
            cameraHorizontalInput = cameraInput.x;
        }

        private void HandleDodgeInput()
        {
            if (dodgeInput)
            {
                dodgeInput = false;

                player.playerLocomotionManager.AttemptToPerformDodge();
            }
        }

        private void HandleSprintInput()
        {
            if (sprintInput)
            {
                player.playerLocomotionManager.HandleSprinting();
            }
            else
            {
                player.isSprinting = false;
            }
        }

        private void HandleJumpInput()
        {
            if (jumpInput)
            {
                jumpInput = false;
                player.playerLocomotionManager.AttemptToPerformJump();
            }            
        }

        private void HandleAttackInput()
        {
            if(lightAttackInput)
            {
                lightAttackInput = false;
                if (player.canDoCombo)
                {
                    player.comboFlag = true;
                    player.playerAttackManager.HandleLightCombo();
                    player.comboFlag = false;
                }
                else
                {
                    if (player.canDoCombo)
                        return;
                    player.playerAttackManager.HandleLightAttack();
                }
            }
            
            if(heavyAttackInput)
            {
                heavyAttackInput = false;
                if (player.canDoCombo)
                {
                    player.comboFlag = true;
                    player.playerAttackManager.HandleHeavyCombo();
                    player.comboFlag = false;
                }
                else
                {
                    if (player.canDoCombo)
                        return;
                    player.playerAttackManager.HandleHeavyAttack();
                }
            }           
        }

        private void HandleHealingInput()
        {
            if (healingInput)
            {
                healingInput = false;
                if (player.canDrinkNext)
                {
                    if (player.isDrinking)
                        return;
                    player.drinkFlag = true;
                    player.playerAttackManager.HandleHealCombo();
                    player.drinkFlag = false;
                    
                }
                else
                {
                    player.playerAttackManager.AttemptToHeal();
                }
            }
        }
    }
}
