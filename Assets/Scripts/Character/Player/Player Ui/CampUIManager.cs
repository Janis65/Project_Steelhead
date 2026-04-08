using UnityEngine;
using TMPro;
using UnityEditor;

namespace JO
{
    public class CampUIManager : MonoBehaviour
    {
        CampManager camp;
        PlayerManager player;

        [Header("UI References")]
        [SerializeField] public GameObject CampMenuGameObject;
        [SerializeField] public GameObject CampLevelUpMenuGameObject;
        [SerializeField] public GameObject PotionUpgradeMenuGameObject;

        [Header("Potion")]
        [SerializeField] TextMeshProUGUI potionUpgradeText;

        #region Level Up Menu Objects 

        [Header("Current Level")]
        [SerializeField] TextMeshProUGUI currentLevelText;
        [SerializeField] TextMeshProUGUI currentHealthLevelText;
        [SerializeField] TextMeshProUGUI currentEnduranceLevelText;
        [SerializeField] TextMeshProUGUI currentStrengthLevelText;

        [Header("Temporary Level")]
        [SerializeField] TextMeshProUGUI temporaryLevelText;
        [SerializeField] TextMeshProUGUI temporaryHealthLevelText;
        [SerializeField] TextMeshProUGUI temporaryEnduranceLevelText;
        [SerializeField] TextMeshProUGUI temporaryStrengthLevelText;

        [Header("Suffering")]
        [SerializeField] TextMeshProUGUI currentSufferingText;
        [SerializeField] TextMeshProUGUI finalSufferingText;
        [SerializeField] TextMeshProUGUI requiredSufferingText;
        #endregion

        int temporaryLevel = 1;
        int temporaryHealthLevel = 10;
        int temporaryEnduranceLevel = 10;
        int temporaryStrengthLevel = 10;

        int levelCost = 60;
        int finalSuffering = 0;

        private void Awake()
        {
            camp = FindFirstObjectByType<CampManager>();
            player = FindFirstObjectByType<PlayerManager>();
        }

        public bool isMenuOpen = false;

        void Update()
        {
            if (isMenuOpen)
            {
                PlayerInputManager input = FindFirstObjectByType<PlayerInputManager>();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                input.cameraInput = Vector2.zero;
            }
            else
            {
                PlayerInputManager input = FindFirstObjectByType<PlayerInputManager>();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        private void CalculateLevelUpCost()
        {
            finalSuffering = finalSuffering - levelCost;
            UpdateFinalSufferingText(finalSuffering.ToString());
            levelCost = levelCost + ((temporaryLevel - 1) * 9);
            UpdateRequiredSufferingText(levelCost.ToString());
        }

        private void CalculateLevelDownCost()
        {
            levelCost = levelCost - (temporaryLevel * 9);
            UpdateRequiredSufferingText(levelCost.ToString());
            finalSuffering = finalSuffering + levelCost;
            UpdateFinalSufferingText(finalSuffering.ToString());
        }

        #region Camp Menu

        public void OpenLevelUpMenuButton()
        {
            CampLevelUpMenuGameObject.SetActive(true);
            CampMenuGameObject.SetActive(false);
            UpdateCurrentSufferingText(player.playerStatsManager.suffering.ToString());
            finalSuffering = player.playerStatsManager.suffering;
            UpdateFinalSufferingText(finalSuffering.ToString());
            UpdateRequiredSufferingText(levelCost.ToString());
        }

        public void IncresePotionUsesButton()
        {
            if (player.playerAttackManager.usesUpgrade <= 0)
            {
                CampMenuGameObject.SetActive(false);
                UpdatePotionUpgradeText("No items to upgrade");
                PotionUpgradeMenuGameObject.SetActive(true);
            }
            else
            {
                CampMenuGameObject.SetActive(false);
                UpdatePotionUpgradeText("Potion uses has been incresed");
                PotionUpgradeMenuGameObject.SetActive(true);
                player.playerAttackManager.maxUses += player.playerAttackManager.usesUpgrade;
                player.playerAttackManager.usesUpgrade = 0;
                player.playerAttackManager.currentUses = player.playerAttackManager.maxUses;
            }
        }

        public void IncresePotionHealingButton()
        {
            if (player.playerAttackManager.healingUpgrade <= 0)
            {
                CampMenuGameObject.SetActive(false);
                UpdatePotionUpgradeText("No items to upgrade");
                PotionUpgradeMenuGameObject.SetActive(true);
            }
            else
            {
                CampMenuGameObject.SetActive(false);
                UpdatePotionUpgradeText("Potion healing has been incresed");
                PotionUpgradeMenuGameObject.SetActive(true);
                player.playerAttackManager.healing += player.playerAttackManager.healingUpgrade * 10;
                player.playerAttackManager.healingUpgrade = 0;
            }
        }

        public void LeaveCampButton()
        {
            PlayerUIPopUpManager uiPopUpManager = FindFirstObjectByType<PlayerUIPopUpManager>();

            player.playerAnimatorManager.PlayerTargetActionAnimation("Bonfire_End", true, true);
            CampMenuGameObject.SetActive(false);
            uiPopUpManager.UpdateInteractionText("PRESS F: SIT");
            uiPopUpManager.SendYouInteractPopUp();
            isMenuOpen = false;
        }

        #endregion

        #region Potion Upgrade Menu

        public void ConfirmPotionUpgradeMenuButton()
        {
            PotionUpgradeMenuGameObject.SetActive(false);
            CampMenuGameObject.SetActive(true);
        }

        public void UpdatePotionUpgradeText(string newText)
        {
            potionUpgradeText.text = newText;
        }

        #endregion

        #region Level Up Menu

        #region Text Updates

        public void UpdateCurrentLevelText(string newText)
        {
            currentLevelText.text = newText;
        }

        public void UpdateCurrentHealthLevelText(string newText)
        {
            currentHealthLevelText.text = newText;
        }

        public void UpdateCurrentEnduranceLevelText(string newText)
        {
            currentEnduranceLevelText.text = newText;
        }

        public void UpdateCurrentStrengthLevelText(string newText)
        {
            currentStrengthLevelText.text = newText;
        }

        public void UpdateTemporaryLevelText(string newText)
        {
            temporaryLevelText.text = newText;
        }

        public void UpdateTemporaryHealthLevelText(string newText)
        {
            temporaryHealthLevelText.text = newText;
        }

        public void UpdateTemporaryEnduranceLevelText(string newText)
        {
            temporaryEnduranceLevelText.text = newText;
        }

        public void UpdateTemporaryStrengthLevelText(string newText)
        {
            temporaryStrengthLevelText.text = newText;
        }

        public void UpdateCurrentSufferingText(string newText)
        {
            currentSufferingText.text = newText;
        }

        public void UpdateFinalSufferingText(string newText)
        {
            finalSufferingText.text = newText;
        }

        public void UpdateRequiredSufferingText(string newText)
        {
            requiredSufferingText.text = newText;
        }

        #endregion

        #region Buttons

        public void AddHealthLevelButton()
        {
            if (finalSuffering < levelCost)
                return;

            temporaryHealthLevel += 1;
            temporaryLevel += 1;
            UpdateTemporaryLevelText(temporaryLevel.ToString());
            UpdateTemporaryHealthLevelText(temporaryHealthLevel.ToString());
            CalculateLevelUpCost();
        }

        public void SubtractHealthLevelButton()
        {
            if (player.playerStatsManager.healthLevel == temporaryHealthLevel)
                return;

            temporaryHealthLevel -= 1;
            temporaryLevel -= 1;
            UpdateTemporaryLevelText(temporaryLevel.ToString());
            UpdateTemporaryHealthLevelText(temporaryHealthLevel.ToString());
            CalculateLevelDownCost();
        }

        public void AddEnduranceLevelButton()
        {
            if (finalSuffering < levelCost)
                return;

            temporaryEnduranceLevel += 1;
            temporaryLevel += 1;
            UpdateTemporaryLevelText(temporaryLevel.ToString());
            UpdateTemporaryEnduranceLevelText(temporaryEnduranceLevel.ToString());
            CalculateLevelUpCost();
        }

        public void SubtractEnduranceLevelButton()
        {
            if (player.playerStatsManager.enduranceLevel == temporaryEnduranceLevel)
                return;

            temporaryEnduranceLevel -= 1;
            temporaryLevel -= 1;
            UpdateTemporaryLevelText(temporaryLevel.ToString());
            UpdateTemporaryEnduranceLevelText(temporaryEnduranceLevel.ToString());
            CalculateLevelDownCost();
        }

        public void AddStrengthLevelButton()
        {
            if (finalSuffering < levelCost)
                return;

            temporaryStrengthLevel += 1;
            temporaryLevel += 1;
            UpdateTemporaryLevelText(temporaryLevel.ToString());
            UpdateTemporaryStrengthLevelText(temporaryStrengthLevel.ToString());
            CalculateLevelUpCost();
        }

        public void SubtractStrengthLevelButton()
        {
            if (player.playerStatsManager.strengthLevel == temporaryStrengthLevel)
                return;

            temporaryStrengthLevel -= 1;
            temporaryLevel -= 1;
            UpdateTemporaryLevelText(temporaryLevel.ToString());
            UpdateTemporaryStrengthLevelText(temporaryStrengthLevel.ToString());
            CalculateLevelDownCost();
        }

        public void ConfirmLevelUpButton()
        {
            player.playerStatsManager.playerLevel = temporaryLevel;
            UpdateCurrentLevelText(player.playerStatsManager.playerLevel.ToString());

            player.playerStatsManager.healthLevel = temporaryHealthLevel;
            UpdateCurrentHealthLevelText(player.playerStatsManager.healthLevel.ToString());

            player.playerStatsManager.enduranceLevel = temporaryEnduranceLevel;
            UpdateCurrentEnduranceLevelText(player.playerStatsManager.enduranceLevel.ToString());

            player.playerStatsManager.strengthLevel = temporaryStrengthLevel;
            UpdateCurrentStrengthLevelText(player.playerStatsManager.strengthLevel.ToString());

            player.playerStatsManager.SetMaxHealthFromHealthLevel();
            player.playerStatsManager.currentHealth = player.playerStatsManager.maxHealth;
            player.playerStatsManager.healthbar.SetMaxHealth(player.playerStatsManager.maxHealth);

            player.playerStatsManager.SetMaxStaminaFromEnduranceLevel();
            player.playerStatsManager.currentStamina = player.playerStatsManager.maxStamina;
            player.playerStatsManager.staminabar.SetMaxStamina(player.playerStatsManager.maxStamina);

            player.playerStatsManager.suffering = finalSuffering;
            player.playerStatsManager.UpdateSufferingText(player.playerStatsManager.suffering.ToString());
        }

        public void CloseLevelUpMenuButton()
        {
            CampLevelUpMenuGameObject.SetActive(false);
            CampMenuGameObject.SetActive(true);
        }

        #endregion

        #endregion
    }
}
