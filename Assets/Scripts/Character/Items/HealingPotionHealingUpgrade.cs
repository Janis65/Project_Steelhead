using UnityEngine;

namespace JO
{
    public class HealingPotionHealingUpgrade : MonoBehaviour
    {
        [SerializeField] private GameObject potionObject;
        private PlayerManager player;
        private PlayerInputManager input;

        [SerializeField] private bool canInteract = false;

        private void Awake()
        {
            player = FindFirstObjectByType<PlayerManager>();
            input = FindFirstObjectByType<PlayerInputManager>();
        }

        private void Update()
        {
            if (canInteract && input.interactInput)
            {
                PotionInteract();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerUIPopUpManager uiPopUpManager = FindFirstObjectByType<PlayerUIPopUpManager>();

                uiPopUpManager.UpdateInteractionText("PRESS F: PICK UP POTION HEALING UPGRADE");
                uiPopUpManager.SendYouInteractPopUp();

                canInteract = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerUIPopUpManager uiPopUpManager = FindFirstObjectByType<PlayerUIPopUpManager>();
                uiPopUpManager.CloseInteractPopUp();

                canInteract = false;
            }
        }

        private void PotionInteract()
        {
            player.playerAttackManager.HideRightWeapon();
            player.playerAnimatorManager.PlayerTargetActionAnimation("Item_SoulHarvest", true, true);
            PlayerUIPopUpManager uiPopUpManager = FindFirstObjectByType<PlayerUIPopUpManager>();
            uiPopUpManager.CloseInteractPopUp();
            player.playerAttackManager.healingUpgrade += 1;
            Destroy(potionObject);
        }
    }
}
