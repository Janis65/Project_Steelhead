using UnityEngine;

namespace JO
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        PlayerManager player;

        public float verticalMovement;
        public float horizontalMovement;
        public float moveAmount;

        private float staminaDamage = 1;

        [Header("Movement Settings")]
        private Vector3 moveDirection;
        private Vector3 targetRotationDirection;
        [SerializeField] float jumpHeight = 0.2f;
        [SerializeField] float walkingSpeed = 1;
        [SerializeField] float runningSpeed = 3;
        [SerializeField] float sprintingSpeed = 6;
        [SerializeField] float rotationSpeed = 15;

        [Header("Dodge")]
        private Vector3 rollDirection;

        [Header("Jump")]
        [SerializeField] float jumpForwardSpeed = 4;
        [SerializeField] float freeFallSpeed = 2;
        private Vector3 jumpDirection;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        private void Start()
        {

        }

        protected override void Update()
        {
            base.Update();
        }

        protected virtual void LateUpdate()
        {
            
        }

        private void FixedUpdate()
        {
            if (player.isSprinting)
                player.playerStatsManager.TakeStaminaDamage(staminaDamage);
        }

        public void HandleAllMovement()
        {
            HandleGroundedMovement();
            HandleRotation();
            HandleJumpingMovement();
            HandleFreeFallMovement();
        }

        private void GetMovementValues()
        {
            verticalMovement = PlayerInputManager.instance.verticalInput;
            horizontalMovement = PlayerInputManager.instance.horizontalInput;
        }

        private void HandleGroundedMovement()
        {
            if (!player.canMove)
                return;

            GetMovementValues();
            
            moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
            moveDirection = moveDirection + PlayerCamera.instance.transform.right * horizontalMovement;
            moveDirection.Normalize();
            moveDirection.y = 0;

            if (player.isSprinting)
            {
                player.characterController.Move(moveDirection * sprintingSpeed * Time.deltaTime);
            }
            else
            {
                if (PlayerInputManager.instance.moveAmount > 0.5f)
                {
                    // Move at running speed
                    player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
                }
                else if (PlayerInputManager.instance.moveAmount <= 0.5f)
                {
                    // Move at walking speed
                    player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
                }
            }
        }

        private void HandleJumpingMovement()
        {
            if (player.isJumping)
            {
                player.characterController.Move(jumpDirection * jumpForwardSpeed * Time.deltaTime);
            }
        }

        private void HandleFreeFallMovement()
        {
            if (!player.isGrounded)
            {
                Vector3 freeFallDirection;

                freeFallDirection = PlayerCamera.instance.transform.forward * PlayerInputManager.instance.verticalInput;
                freeFallDirection = freeFallDirection + PlayerCamera.instance.transform.right * PlayerInputManager.instance.horizontalInput;
                freeFallDirection.y = 0;

                player.characterController.Move(freeFallDirection * freeFallSpeed * Time.deltaTime);
            }
        }

        public void HandleSprinting()
        {
            if (player.outOfStamina)
                return;

            if (player.isPerforminAction)
            {
                player.isSprinting = false;
            }

            if (PlayerInputManager.instance.moveAmount > 0.5f)
            {
                player.isSprinting = true;
            }
            else
            {
                player.isSprinting = false;
            }
        }

        private void HandleRotation()
        {
            if (player.isDead)
                return;

            if (!player.canRotate)
                return;

            if (player.isLockedOn)
            {
                if (player.isSprinting || player.isRolling)
                {
                    Vector3 targetDirection = Vector3.zero;
                    targetDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
                    targetDirection += PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
                    targetDirection.Normalize();
                    targetDirection.y = 0;

                    if (targetDirection == Vector3.zero)
                        targetDirection = transform.forward;

                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    Quaternion finalRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    transform.rotation = finalRotation;
                }
                else
                {
                    if (player.playerAttackManager.currentTarget == null)
                        return;

                    Vector3 targetDirection;
                    targetDirection = player.playerAttackManager.currentTarget.transform.position - transform.position;
                    targetDirection.y = 0;
                    targetDirection.Normalize();

                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    Quaternion finalRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    transform.rotation = finalRotation;
                }
            }
            else
            {
                targetRotationDirection = Vector3.zero;
                targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
                targetRotationDirection = targetRotationDirection + PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
                targetRotationDirection.Normalize();
                targetRotationDirection.y = 0;

                if (targetRotationDirection == Vector3.zero)
                {
                    targetRotationDirection = transform.forward;
                }

                Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
                transform.rotation = targetRotation;
            }
        }

        public void AttemptToPerformDodge()
        {
            if (player.isPerforminAction)
                return;

            if (PlayerInputManager.instance.moveAmount > 0 && player.isGrounded && !player.isPerforminAction && !player.outOfStamina)
            {
                // Perform a roll
                rollDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
                rollDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;
                rollDirection.y = 0;
                rollDirection.Normalize();

                Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
                player.transform.rotation = playerRotation;

                player.playerAnimatorManager.PlayerTargetActionAnimation("Roll_Forward_01", true, true);

                player.canRegenStamina = false;
                player.playerStatsManager.TakeStaminaDamage(staminaDamage * 20);
            
            }
            else if (player.isGrounded && !player.isPerforminAction && !player.outOfStamina)
            {
                // Perform a backstep
                player.playerAnimatorManager.PlayerTargetActionAnimation("Back_Step_01", true, true);

                player.canRegenStamina = false;
                player.playerStatsManager.TakeStaminaDamage(staminaDamage * 10);
            }
        }

        public void AttemptToPerformJump()
        {
            if (player.isPerforminAction)
                return;

            if (player.isJumping)
                return;

            if (!player.isGrounded)
                return;

            if (player.isSprinting)
            {
                player.playerAnimatorManager.PlayerTargetActionAnimation("Main_Jump_01", false);

                player.isPerforminAction = true;
                player.isJumping = true;

                jumpDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
                //jumpDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;
                jumpDirection.y = 0;

                player.playerStatsManager.TakeStaminaDamage(staminaDamage * 20);
            }
        }

        public void ApplyJumpingVelocity()
        {
            yVelocity.y = Mathf.Sqrt(jumpHeight * -0.5f * gravityForce);
        }
    }
}
