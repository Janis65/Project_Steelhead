using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JO
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera instance;
        public PlayerManager player;
        public Camera cameraObject;
        [SerializeField] Transform cameraPivotTransform;

        [Header("Camera Settings")]
        private float cameraSmoothSpeed = 1; // Ta wartoœæ zmienia jak szybko kamera pod¹¿a za celem, im wiêksza wartoœæ tym d³u¿ej jej to zajmie
        [SerializeField] float leftAndRightRotationSpeed = 220;
        [SerializeField] float upAndDownRotationSpeed = 220;
        [SerializeField] float minimumPivot = -30;
        [SerializeField] float maximumPivot = 60;
        [SerializeField] float cameraCollisionRadius = 0.2f;
        [SerializeField] LayerMask collideWithLayers;

        [Header("Lock On")]
        [SerializeField] float lockOnRadius = 20;
        [SerializeField] float minimumViewableAngle = -50;
        [SerializeField] float maximumViewableAngle = 50;
        [SerializeField] float lockOnTargetFollowSpeed = 0.2f;
        private Coroutine cameraLockOnHeightCoroutine;
        private List<EnemyManager> availableTargets = new List<EnemyManager>();
        public EnemyManager nearestLockOnTarget;
        public EnemyManager leftLockOnTarget;
        public EnemyManager rightLockOnTarget;

        [Header("Camera Values")]
        private Vector3 cameraVelocity;
        private Vector3 cameraObjectPosition;
        [SerializeField] float leftAndRightLookAngle;
        [SerializeField] float upAndDownLookAngle;
        private float cameraZPosition;
        private float targetCameraZPosition;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            cameraZPosition = cameraObject.transform.localPosition.z;        
        }

        public void HandleAllCameraAction()
        {
            if (player != null)
            {
                HandleFollowPlayer();

                HandleRotation();

                HandleCollisions();
            }
        }

        private void HandleFollowPlayer()
        {
            Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
            transform.position = targetCameraPosition;
        }

        private void HandleRotation()
        {
            if (player.isLockedOn)
            {
                // Rotates this game object
                Vector3 rotationDirection = player.playerAttackManager.currentTarget.lockOnTransform.position - transform.position;
                rotationDirection.Normalize();
                rotationDirection.y = 0;

                Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lockOnTargetFollowSpeed);

                // Rotates pivot of this game object
                rotationDirection = player.playerAttackManager.currentTarget.lockOnTransform.position - cameraPivotTransform.position;
                rotationDirection.Normalize();

                targetRotation = Quaternion.LookRotation(rotationDirection);
                cameraPivotTransform.transform.rotation = Quaternion.Slerp(cameraPivotTransform.rotation, targetRotation, lockOnTargetFollowSpeed);

                // Zapamiêtaj aktualny k¹t obrotu
                leftAndRightLookAngle = transform.eulerAngles.y;
                upAndDownLookAngle = cameraPivotTransform.localEulerAngles.x;
            }
            else
            {
                // Rotate left and right based on right stick horizontal movement
                leftAndRightLookAngle += (PlayerInputManager.instance.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;
                // Rotate up and down based on right stick vertical movement
                upAndDownLookAngle -= (PlayerInputManager.instance.cameraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;
                // Clamp vertical look angle
                upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

                Vector3 cameraRotation = Vector3.zero;
                Quaternion targetRotation;

                // Rotate this gameobject left and right
                cameraRotation.y = leftAndRightLookAngle;
                targetRotation = Quaternion.Euler(cameraRotation);
                transform.rotation = targetRotation;

                // Rotate this gameobject up and down
                cameraRotation = Vector3.zero;
                cameraRotation.x = upAndDownLookAngle;
                targetRotation = Quaternion.Euler(cameraRotation);
                cameraPivotTransform.localRotation = targetRotation;
            }
        }

        private void HandleCollisions()
        {
            targetCameraZPosition = cameraZPosition;
            RaycastHit hit;
            Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
            direction.Normalize();

            // Cheking if there is object in front of the camera
            if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), collideWithLayers))
            {
                float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
            }

            if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
            {
                targetCameraZPosition = -cameraCollisionRadius;
            }

            cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);
            cameraObject.transform.localPosition = cameraObjectPosition;
        }

        public void HandleLocatingLockOnTargets()
        {
            float shortestDistance = Mathf.Infinity;
            float shortestDistanceOfRightTarget = Mathf.Infinity;
            float shortestDistanceOfLeftTarget = -Mathf.Infinity;

            Collider[] colliders = Physics.OverlapSphere(player.transform.position, lockOnRadius, WorldUtilityManager.Instance.GetCharacterLayers());

            for (int i = 0; i < colliders.Length; i++)
            {
                EnemyManager lockOnTarget = colliders[i].GetComponent<EnemyManager>();

                if (lockOnTarget != null)
                {
                    // CHECK IF THEY ARE WITHIN OUR FIELD OF VIEW
                    Vector3 lockOnTargetsDirection = lockOnTarget.transform.position - player.transform.position;
                    float distanceFromTarget = Vector3.Distance(player.transform.position, lockOnTarget.transform.position);
                    float viewableAngle = Vector3.Angle(lockOnTargetsDirection, cameraObject.transform.forward);

                    // IF TARGET IS DEAD, CHECK THE NEXT POTENTIAL TARGET
                    if (lockOnTarget.isDead)
                        continue;

                    // IF TARGET IS US, CHECK THE NEXT POTENTIAL TARGET
                    if (lockOnTarget.transform.root == player.transform.root)
                        continue;

                    if (viewableAngle > minimumViewableAngle && viewableAngle < maximumViewableAngle)
                    {
                        RaycastHit hit;

                        // ADD CHECK FOR ENVIROMENT LAYERS ONLY
                        if (Physics.Linecast(player.lockOnTransform.position, lockOnTarget.lockOnTransform.position, out hit, WorldUtilityManager.Instance.GetEnviroLayers()))
                        {
                            continue;
                        }
                        else
                        {
                            availableTargets.Add(lockOnTarget);
                        }
                    }
                }
            }

            for (int k = 0; k < availableTargets.Count; k++)
            {
                if (availableTargets[k] != null)
                {
                    float distanceFromTarget = Vector3.Distance(player.transform.position, availableTargets[k].transform.position);

                    if (distanceFromTarget < shortestDistance)
                    {
                        shortestDistance = distanceFromTarget;
                        nearestLockOnTarget = availableTargets[k];
                    }

                    if (player.isLockedOn)
                    {
                        Vector3 relativeEnemyPosition = player.transform.InverseTransformPoint(availableTargets[k].transform.position);

                        var distanceFromLeftTarget = relativeEnemyPosition.x;
                        var distanceFromRightTarget = relativeEnemyPosition.x;

                        if (availableTargets[k] == player.playerAttackManager.currentTarget)
                            continue;

                        if (relativeEnemyPosition.x <= 0.00 && distanceFromLeftTarget > shortestDistanceOfLeftTarget)
                        {
                            shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                            leftLockOnTarget = availableTargets[k];
                        }
                        else if (relativeEnemyPosition.x >= 0.00 && distanceFromRightTarget < shortestDistanceOfRightTarget)
                        {
                            shortestDistanceOfRightTarget = distanceFromRightTarget;
                            rightLockOnTarget = availableTargets[k];
                        }
                    }
                }
                else
                {
                    player.isLockedOn = false;
                }
            }
        }

        public void ClearLockOnTargets()
        {
            nearestLockOnTarget = null;
            leftLockOnTarget = null;
            rightLockOnTarget = null;
            availableTargets.Clear();
        }

    }
}
