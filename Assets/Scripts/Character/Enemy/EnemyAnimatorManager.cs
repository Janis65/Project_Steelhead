using Synty.AnimationBaseLocomotion.Samples;
using UnityEngine;

namespace JO
{
    public class EnemyAnimatorManager : CharacterAnimationManager
    {
        EnemyManager enemyManager;
        EnemyEquipmentManager enemyEquipmentManager;
        public Animator animator;

        protected override void Awake()
        {
            base.Awake();

            animator = GetComponent<Animator>();
            enemyManager = GetComponentInParent<EnemyManager>();
            enemyEquipmentManager = GetComponentInParent<EnemyEquipmentManager>();
        }

        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            enemyManager.enemyRigidBody.linearDamping = 0;
            Vector3 deltaPosition = animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            enemyManager.enemyRigidBody.linearVelocity = velocity;
        }

        public virtual void EnemyTargetAttackAnimation(string targetAnimation, bool isPerformingAction, bool applyRootMotin = true, bool canRotate = false, bool canMove = false)
        {
            enemyManager.applyRootMotion = applyRootMotin;
            enemyManager.animator.CrossFade(targetAnimation, 0.2f);
            // Can be used to stop character from performing actions
            enemyManager.isPerforminAction = isPerformingAction;
            enemyManager.canRotate = canRotate;
            enemyManager.canMove = canMove;
        }


        public void OpenRightDamageCollider()
        {
            enemyEquipmentManager.rightHandDamageCollider.EnableDamageCollider();
        }

        public void CloseRightDamageCollider()
        {
            enemyEquipmentManager.rightHandDamageCollider.DisableDamageCollider();
        }
        public void EnableCombo()
        {
            enemyManager.animator.SetBool("canDoCombo", true);
        }

        public void DisableCombo()
        {
            enemyManager.animator.SetBool("canDoCombo", false);
        }
    }
}
