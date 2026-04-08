using UnityEngine;
using UnityEngine.AI;

namespace JO
{
    public class EnemyManager : CharacterManager
    {
        [HideInInspector] public EnemyLocomotionManager enemyLocomotionManager;
        [HideInInspector] public EnemyAnimatorManager enemyAnimatorManager;
        [HideInInspector] public EnemyStatsManager enemyStats;
        [HideInInspector] public EnemyInventoryManager enemyInventoryManager;
        [HideInInspector] public EnemyEquipmentManager enemyEquipmentManager;

        public State currentState;
        public PlayerManager currentTarget;
        public NavMeshAgent navmeshAgent;
        public Rigidbody enemyRigidBody;

        [Header("AI Settings")]
        public float rotationSpeed = 50;
        public float maximumAttackRange = 2;
        public float detectionRadius = 30;
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;
        public float currentRecoveryTime = 0;

        protected override void Awake()
        {
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            enemyStats = GetComponent<EnemyStatsManager>();
            enemyInventoryManager = GetComponent<EnemyInventoryManager>();
            enemyEquipmentManager = GetComponent<EnemyEquipmentManager>();

            navmeshAgent = GetComponentInChildren<NavMeshAgent>();
            navmeshAgent.enabled = false;
            enemyRigidBody = GetComponent<Rigidbody>();
            enemyRigidBody.isKinematic = false;
        }

        protected override void Update()
        {
            base.Update();

            HandleRecoveryTimer();

            if (navmeshAgent.isOnNavMesh)
            {
                NavMeshHit hit;

                if (navmeshAgent.hasPath && NavMesh.SamplePosition(navmeshAgent.transform.position, out hit, 1.0f, NavMesh.AllAreas))
                {
                    Vector3 adjustedPosition = transform.position;
                    adjustedPosition.y = hit.position.y + 0.01f;
                    transform.position = adjustedPosition;
                }
            }
        }

        private void FixedUpdate()
        {
            if (isDead) 
                return;
            
            HandleStateMachine();
        }

        private void HandleStateMachine()
        {
            if (currentState != null)
            {
                State nextState = currentState.Tick(this, enemyStats, enemyAnimatorManager);

                if (nextState != null)
                {
                    SwitchToNextState(nextState);
                }
            }
        }

        private void SwitchToNextState(State state)
        {
            currentState = state;
        }

        private void HandleRecoveryTimer()
        {
            if (currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if (isPerforminAction)
            {
                if (currentRecoveryTime <= 0)
                {
                    isPerforminAction = false;
                }
            }
        }
    }
}
