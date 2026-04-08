using Unity.VisualScripting;
using UnityEngine;

namespace JO
{
    public abstract class State : MonoBehaviour
    {
        public abstract State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStats, EnemyAnimatorManager enemyAnimatorManager);
    }
}
