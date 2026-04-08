using UnityEngine;

namespace JO
{
    public class IdleState : State
    {
        public float detectionRadius = 2;

        public PursueTargetState pursueTargetState;

        public LayerMask detectionLayer;

        public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            #region Enemy Target Detection

            Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);

            Collider[] detection = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);

            enemyAnimatorManager.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);

            // FOV Detection
            for (int i = 0; i < colliders.Length; i++)
            {
                PlayerManager playerManager = colliders[i].transform.GetComponent<PlayerManager>();

                if (playerManager != null)
                {
                    Vector3 targetDirection = playerManager.transform.position - enemyManager.transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

                    if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle && !playerManager.isDead)
                    {
                        enemyManager.currentTarget = playerManager;
                    }
                }
            }

            // Surounding detection
            for (int i = 0; i < detection.Length; i++)
            {
                PlayerManager playerManager = colliders[i].transform.GetComponent<PlayerManager>();

                if (playerManager != null)
                {
                    if (!playerManager.isDead)
                    enemyManager.currentTarget = playerManager;
                }
            }
            #endregion

            #region Handle Switch To Next State
            if (enemyManager.currentTarget != null)
            {
                return pursueTargetState;
            }
            else
            {
                return this;
            }
            #endregion
        }
    }
}
