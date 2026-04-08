using System.Threading.Tasks;
using UnityEngine;

namespace JO
{
    public class ResetActionFlad : StateMachineBehaviour
    {
        CharacterManager character;
        EnemyManager enemy;
        DamageCollider damageCollider;
        PlayerManager player;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (character == null)
            {
                character = animator.GetComponent<CharacterManager>();
                damageCollider = animator.GetComponentInChildren<DamageCollider>();
                player = animator.GetComponent<PlayerManager>();
            }

            // Jeœli nadal nie znaleziono, spróbuj pobraæ EnemyManager
            if (character == null)
            {
                character = animator.GetComponentInParent<EnemyManager>();
                damageCollider = animator.GetComponentInChildren<DamageCollider>();
            }


            if (character != null)
            {
                character.isPerforminAction = false;
                character.isAttacking = false;
                character.applyRootMotion = false;
                character.canRotate = true;
                character.canMove = true;
                character.isJumping = false;
                character.canRegenStamina = true;
                character.canDoCombo = false;
                character.canDrinkNext = false;
                character.isDrinking = false;
                character.isRolling = false;
                damageCollider.DisableDamageCollider();
                player.playerAttackManager.HidePotion();
                player.playerAttackManager.ShowRightWeapon();
            }
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }
}
