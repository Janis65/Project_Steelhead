using UnityEngine;

namespace JO
{
    public class AttackState : State
    {
        public CombatStanceState combatStanceState;
        public IdleState idleState;

        public EnemyAttackActionsManager[] enemyAttacks;
        public EnemyAttackActionsManager currentAttack;

        public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

            if (enemyManager.isPerforminAction)
                return combatStanceState;

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

            if (currentAttack != null)
            {
                if (distanceFromTarget < currentAttack.minimumDistanceNeededToAttack)
                {
                    return this;
                }
                else if (distanceFromTarget < currentAttack.maximumDistanceNeededToAttack)
                {
                    if (viewableAngle <= currentAttack.maximumAttackAngle && viewableAngle >= currentAttack.minimumAttackAngle)
                    {
                        if (enemyManager.currentRecoveryTime <= 0 && enemyManager.isPerforminAction == false)
                        {
                            enemyManager.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                            enemyManager.animator.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);

                            enemyAnimatorManager.EnemyTargetAttackAnimation(currentAttack.actionAnimation, true);
                            enemyManager.isPerforminAction = true;
                            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
                            currentAttack = null;
                            return combatStanceState;
                        }
                    }
                }
            }
            else
            {
                GetNewAttack(enemyManager);
            }

            return combatStanceState;
        }

        private void GetNewAttack(EnemyManager enemyManager)
        {
            Vector3 targetsDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            float viewableAngle = Vector3.Angle(targetsDirection, enemyManager.transform.forward);
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

            int maxScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackActionsManager enemyAttackAction = enemyAttacks[i];

                if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        maxScore += enemyAttackAction.attackScore;
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackActionsManager enemyAttackAction = enemyAttacks[i];

                if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        if (currentAttack != null)
                            return;

                        temporaryScore += enemyAttackAction.attackScore;

                        if (temporaryScore > randomValue)
                        {
                            currentAttack = enemyAttackAction;
                        }
                    }
                }
            }
        }
    }
}
