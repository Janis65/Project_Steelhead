using System.Globalization;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace JO
{
    public class CharacterManager : MonoBehaviour
    {
        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Animator animator;
        [HideInInspector] public CharacterAttackManager characterAttackManager;
        [HideInInspector] public CharacterStatsManager characterStatsManager;
        [HideInInspector] public CharacterLocomotionManager characterLocomotionManager;

        public Transform lockOnTransform;

        [Header("Flags")]
        public bool isPerforminAction = false;
        public bool isAttacking = false;
        public bool isSprinting = false;
        public bool isJumping = false;
        public bool isRolling = false;
        public bool isGrounded = false;
        public bool applyRootMotion = false;
        public bool canRotate = true;
        public bool canMove = true;
        public bool isDead = false;
        public bool outOfStamina = false;
        public bool canRegenStamina = true;
        public bool canDoCombo;
        public bool comboFlag;
        public bool canDrinkNext;
        public bool drinkFlag;
        public bool isDrinking = false;
        public bool isLockedOn = false;
        public bool TH_Equiped = false;

        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);

            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            characterAttackManager = GetComponent<CharacterAttackManager>();
            characterStatsManager = GetComponent<CharacterStatsManager>();
            characterLocomotionManager = GetComponent<CharacterLocomotionManager>();
        }

        protected virtual void Update()
        {
            animator.SetBool("isGrounded", isGrounded);
            canDoCombo = animator.GetBool("canDoCombo");
            canDrinkNext = animator.GetBool("canDrinkNext");

            if (TH_Equiped)
            {
                animator.SetBool("TH_Equiped", true);
            }
            else
            {
                animator.SetBool("TH_Equiped", false);
            }
        }

        protected virtual void LateUpdate()
        {

        }

        protected virtual void IgnoreMyOwnColliders()
        {

        }
    }
}
