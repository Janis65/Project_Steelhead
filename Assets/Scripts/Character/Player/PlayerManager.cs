using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.UIElements;

namespace JO
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerAttackManager playerAttackManager;
        [HideInInspector] public PlayerInventoryManager playerInventoryManager;
        [HideInInspector] public PlayerEquipmentManager playerEquipmentManager;
        [HideInInspector] public PlayerStatsManager playerStatsManager;
        [HideInInspector] public PlayerUIManager playerUIManager;

        [Header("Respawn")]
        public GameObject playerGameObject;
        private float deathRespawnTimer = 7f;
        public Transform respawnPoint; // Punkt respawnu


        protected override void Awake()
        {
            base.Awake();

            //Do more stuff, only for the player
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAttackManager = GetComponent<PlayerAttackManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerUIManager = GetComponent<PlayerUIManager>();

        }

        protected override void Update()
        {
            base.Update();

            // Handle movement
            playerLocomotionManager.HandleAllMovement();

            if (!isLockedOn)
            {
                playerAttackManager.currentTarget = null;
            }

            if (isDead)
            {
                deathRespawnTimer -= Time.deltaTime;
                if (deathRespawnTimer <= 0)
                {
                    foreach (EnemySpawnerManager spawner in EnemySpawnerManager.AllSpawners)
                    {
                        spawner.DestroyEnemy();
                        spawner.SpawnEnemy();
                    }

                    RespawnPlayer();
                }
            }
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
            PlayerCamera.instance.player = this;
            PlayerInputManager.instance.player = this;
            PlayerCamera.instance.HandleAllCameraAction();

        }

        public void RespawnPlayer()
        {
            Debug.Log("Respawn Attempt");
            playerAttackManager.HideRightWeapon();
            playerAnimatorManager.PlayerTargetActionAnimation("Empty", true, true);
            playerGameObject.SetActive(false);

            // Przeniesienie gracza do punktu respawnu
            playerGameObject.transform.position = respawnPoint.transform.position; 
            playerGameObject.transform.rotation = respawnPoint.transform.rotation;
            playerGameObject.SetActive(true);

            playerAnimatorManager.PlayerTargetActionAnimation("Bonfire_Idle", true, true);
            playerAnimatorManager.PlayerTargetActionAnimation("Bonfire_End", true, true);

            playerStatsManager.currentHealth = playerStatsManager.maxHealth;
            playerStatsManager.healthbar.SetCurrentHealth(playerStatsManager.currentHealth);

            playerStatsManager.currentStamina = playerStatsManager.maxStamina;
            playerStatsManager.staminabar.SetCurrentStamina(playerStatsManager.currentStamina);

            playerStatsManager.suffering = playerStatsManager.suffering / 2;
            playerStatsManager.UpdateSufferingText(playerStatsManager.suffering.ToString());

            playerAttackManager.currentUses = playerAttackManager.maxUses;
            isDead = false;
            deathRespawnTimer = 7f;
            isLockedOn = false;
        }
    }
}
