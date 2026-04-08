using UnityEngine;

namespace JO
{
    public class CharacterAnimationManager : MonoBehaviour
    {
        CharacterManager character;

        float vertical;
        float horizontal;
        protected virtual void Awake() 
        {
            character = GetComponent<CharacterManager>();
        }
        public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement, bool isSprinting)
        {
            float horizontalAmount = horizontalMovement;
            float verticalAmount = verticalMovement;

            if (horizontalMovement > 0 && horizontalMovement <= 0.5f)
            {
                horizontalAmount = 0.5f;
            }
            else if (horizontalMovement > 0.5f && horizontalMovement <= 1)
            {
                horizontalAmount = 1;
            }
            else if (horizontalMovement < 0 && horizontalMovement >= -0.5f)
            {
                horizontalAmount = -0.5f;
            }
            else if (horizontalMovement < -0.5f && horizontalMovement >= -1)
            {
                horizontalAmount = -1;
            }
            else
            {
                horizontalAmount = 0;
            }

            if (verticalMovement > 0 && verticalMovement <= 0.5f)
            {
                verticalAmount = 0.5f;
            }
            else if (verticalMovement > 0.5f && verticalMovement <= 1)
            {
                verticalAmount = 1;
            }
            else if (verticalMovement < 0 && verticalMovement >= -0.5f)
            {
                verticalAmount = -0.5f;
            }
            else if (verticalMovement < -0.5f && verticalMovement >= -1)
            {
                verticalAmount = -1;
            }
            else
            {
                verticalAmount = 0;
            }


            if (isSprinting)
            {
                verticalAmount = 2;
            }

            character.animator.SetFloat("Horizontal", horizontalAmount, 0.1f, Time.deltaTime);
            character.animator.SetFloat("Vertical", verticalAmount, 0.1f, Time.deltaTime);
        }

        public virtual void PlayerTargetActionAnimation(string targetAnimation, bool isPerformingAction, bool applyRootMotin = true, bool canRotate = false, bool canMove = false)
        {
            character.applyRootMotion = applyRootMotin;
            character.animator.CrossFade(targetAnimation, 0.2f);
            // Can be used to stop character from performing actions
            character.isPerforminAction = isPerformingAction;
            character.canRotate = canRotate;
            character.canMove = canMove;
        }

        public virtual void PlayerTargetAttackAnimation(string targetAnimation, bool isPerformingAction, bool applyRootMotin = true, bool canRotate = true, bool canMove = false)
        {
            character.applyRootMotion = applyRootMotin;
            character.animator.CrossFade(targetAnimation, 0.2f);
            // Can be used to stop character from performing actions
            character.isPerforminAction = isPerformingAction;
            character.canRotate = canRotate;
            character.canMove = canMove;
        }
    }
}
