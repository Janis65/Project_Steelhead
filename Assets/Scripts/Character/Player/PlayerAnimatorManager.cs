using JetBrains.Annotations;
using UnityEngine;

namespace JO
{
    public class PlayerAnimatorManager : CharacterAnimationManager
    {
        PlayerManager player;
        CampManager camp;
        CampUIManager campUI;
        PlayerUIPopUpManager playerUIPopUpManager;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
            camp = FindFirstObjectByType<CampManager>();
            campUI = FindFirstObjectByType<CampUIManager>();
            playerUIPopUpManager = GetComponent<PlayerUIPopUpManager>();
        }
        private void OnAnimatorMove()
        {
            if (player.applyRootMotion)
            {
                Vector3 velocity = player.animator.deltaPosition;
                player.characterController.Move(velocity);
                player.transform.rotation *= player.animator.deltaRotation;
            }
        }

        public void EnableCombo()
        {
            player.animator.SetBool("canDoCombo", true);
        }

        public void DisableCombo()
        {
            player.animator.SetBool("canDoCombo", false);
        }

        public void EnableDrinkNext()
        {
            player.animator.SetBool("canDrinkNext", true);
        }

        public void DisableDrinkNext()
        {
            player.animator.SetBool("canDrinkNext", false);
        }

        public void EnableIsDrinking()
        {
            player.isDrinking = true;
        }

        public void DisableIsDrinking()
        {
            player.isDrinking = false;
        }

        public void EnableRollInvulnerability()
        {
            player.isRolling = true;
        }

        public void DisableRollInvulnerability()
        {
            player.isRolling = false;
        }

        public void Rest()
        {
            PlayerUIPopUpManager uiPopUpManager = FindFirstObjectByType<PlayerUIPopUpManager>();
            //PlayerInputManager input = FindFirstObjectByType<PlayerInputManager>();

            player.playerStatsManager.currentHealth = player.playerStatsManager.maxHealth;
            player.playerStatsManager.healthbar.SetCurrentHealth(player.playerStatsManager.currentHealth);

            player.playerStatsManager.currentStamina = player.playerStatsManager.maxStamina;
            player.playerStatsManager.staminabar.SetCurrentStamina(player.playerStatsManager.currentStamina);

            player.playerAttackManager.currentUses = player.playerAttackManager.maxUses;

            uiPopUpManager.CloseInteractPopUp();
            campUI.CampMenuGameObject.SetActive(true);
            campUI.isMenuOpen = true;
        }
    }
}
