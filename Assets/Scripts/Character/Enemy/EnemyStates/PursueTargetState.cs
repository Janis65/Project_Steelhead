using UnityEngine;
using UnityEngine.AI;

namespace JO
{
    public class PursueTargetState : State
    {
        public CombatStanceState combatStanceState;
        public AttackState attackState;
        public IdleState idleState;

        public float walkSpeed = 1;

        public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            if (enemyManager.isPerforminAction)
            {
                enemyAnimatorManager.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return this;
            }

            if (enemyManager.currentTarget == null)
            {
                enemyAnimatorManager.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return idleState;
            }

            else if (enemyManager.currentTarget.isDead)
            {
                enemyAnimatorManager.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                enemyManager.currentTarget = null;
                return idleState;
            }

            Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

            if (distanceFromTarget > enemyManager.maximumAttackRange)
            {
                enemyManager.animator.SetFloat("Vertical", walkSpeed, 0.1f, Time.deltaTime);
            }

            HandleRotateTowardsTarget(enemyManager);

            enemyManager.navmeshAgent.transform.localPosition = Vector3.zero;
            enemyManager.navmeshAgent.transform.localRotation = Quaternion.identity;

            if (distanceFromTarget <= enemyManager.maximumAttackRange)
            {
                return attackState;
            }
            else
            {
                return this;
            }
        }

        public void HandleRotateTowardsTarget(EnemyManager enemyManager)
        {
            if (enemyManager.isPerforminAction)
            {
                Vector3 direction = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
            else
            {

                Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navmeshAgent.desiredVelocity);
                Vector3 targetVelocity = enemyManager.enemyRigidBody.linearVelocity;

                enemyManager.navmeshAgent.enabled = true;
                enemyManager.navmeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
                enemyManager.enemyRigidBody.linearVelocity = targetVelocity;
                enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navmeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
                
            }
        }
    }
}
