using UnityEngine;

namespace JO
{
    public class CampManager : MonoBehaviour
    {
        [Header("References")]
        [HideInInspector] private PlayerUIPopUpManager playerUIPopUpManager;
        [HideInInspector] private PlayerInputManager input;
        [HideInInspector] private PlayerManager player;
        [HideInInspector] private PlayerAnimatorManager playerAnimatorManager;

        [SerializeField] public Transform campRespawnPoint;
        [SerializeField] GameObject ActivCampGameObject;
        [SerializeField] GameObject UnactiveCampGameObject;

        public bool canInteract = false;
        public bool campActive = false;
        private float activeCampTimer = 1;

        public void Awake()
        {
            playerUIPopUpManager = GetComponent<PlayerUIPopUpManager>();
            player = FindFirstObjectByType<PlayerManager>();
            input = FindFirstObjectByType<PlayerInputManager>();
            playerAnimatorManager = FindFirstObjectByType<PlayerAnimatorManager>();
        }

        public void Update()
        {
            if(canInteract && input.interactInput)
            {
                CampInteract();
            }

            
            if(campActive)
            {
                activeCampTimer -= Time.deltaTime;

                if (activeCampTimer <= 0)
                {
                    ActivCampGameObject.SetActive(true);
                    UnactiveCampGameObject.SetActive(false);
                }
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerUIPopUpManager uiPopUpManager = FindFirstObjectByType<PlayerUIPopUpManager>();

                if (!campActive)
                {
                    uiPopUpManager.UpdateInteractionText("PRESS F: IGNITE");
                }
                else
                {
                    uiPopUpManager.UpdateInteractionText("PRESS F: SIT");
                }
                uiPopUpManager.SendYouInteractPopUp();

                canInteract = true;
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerUIPopUpManager uiPopUpManager = FindFirstObjectByType<PlayerUIPopUpManager>();
                uiPopUpManager.CloseInteractPopUp();

                canInteract = false;
            }
        }

        public void CampInteract()
        {
            PlayerUIPopUpManager uiPopUpManager = FindFirstObjectByType<PlayerUIPopUpManager>();

            if (!campActive)
            {
                if (player.isPerforminAction)
                    return;

                player.playerAttackManager.HideRightWeapon();
                player.playerAnimatorManager.PlayerTargetActionAnimation("Bonfire_Ignite", true, true);
                campActive = true;
                uiPopUpManager.UpdateInteractionText("PRESS F: SIT");
            }
            else if (campActive)
            {
                if (player.isPerforminAction)
                    return;

                player.playerAttackManager.HideRightWeapon();
                player.playerAnimatorManager.PlayerTargetActionAnimation("Bonfire_Start", true, true);
                player.respawnPoint = campRespawnPoint;

                foreach (EnemySpawnerManager spawner in EnemySpawnerManager.AllSpawners)
                {
                    spawner.DestroyEnemy();
                    spawner.SpawnEnemy();
                }
            }
        }

    }
}
