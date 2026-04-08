using UnityEngine;

namespace JO
{
    public class WayPointPath1 : MonoBehaviour
    {
        public Transform GetWayPoint(int wayPointIndex)
        {
            return transform.GetChild(wayPointIndex);
        }

        public int GetNextWayPointIndex(int currentWayPointIndex)
        {
            int nextWayPointIndex = currentWayPointIndex + 1;

            if (nextWayPointIndex == transform.childCount)
            {
                nextWayPointIndex = 0;
            }
            return nextWayPointIndex;
        }
    }
}
