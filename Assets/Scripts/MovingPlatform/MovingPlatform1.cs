using UnityEngine;

namespace JO
{
    public class MovingPlatform1 : MonoBehaviour
    {
        [SerializeField] private WayPointPath1 wayPointPath;

        [SerializeField] private float speed;

        private int targetWayPointIndex;

        private Transform previousWayPoint;
        private Transform targetWayPoint;

        private float timeToWayPoint;
        private float elapsedTime;

        void Start()
        {
            TargetNextWayPoint();
        }

        void FixedUpdate()
        {
            elapsedTime += Time.deltaTime;

            float elapsedPercentage = elapsedTime / timeToWayPoint;

            transform.position= Vector3.Lerp(previousWayPoint.position, targetWayPoint.position, elapsedPercentage);

            if (elapsedPercentage >= 1)
            {
                TargetNextWayPoint();
            }
        }

        private void  TargetNextWayPoint()
        {
            previousWayPoint = wayPointPath.GetWayPoint(targetWayPointIndex);
            targetWayPointIndex = wayPointPath.GetNextWayPointIndex(targetWayPointIndex);
            targetWayPoint = wayPointPath.GetWayPoint(targetWayPointIndex);

            elapsedTime = 0;

            float distanceToWayPoint = Vector3.Distance(previousWayPoint.position, targetWayPoint.position);
            timeToWayPoint = distanceToWayPoint / speed;
        }

        private void OnTriggerEnter(Collider other)
        {
            other.transform.SetParent(transform);
        }

        private void OnTriggerExit(Collider other)
        {
            other.transform.SetParent(null);
        }
    }
}
