using UnityEngine;

namespace JO
{
    [CreateAssetMenu(menuName = "A.I/Enemy Actions/ Attack Actions")]
    public class EnemyAttackActionsManager : EnemyActionsManager
    {
        public int attackScore = 3;
        public float recoveryTime = 1;

        public float maximumAttackAngle = 35;
        public float minimumAttackAngle = -35;

        public float maximumDistanceNeededToAttack = 3;
        public float minimumDistanceNeededToAttack = 0;
    }
}
