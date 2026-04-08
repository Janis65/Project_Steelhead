using UnityEngine;
using TMPro;

namespace JO
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        [HideInInspector] PlayerManager player;
        [HideInInspector] public PlayerUIPopUpManager playerUIPopUpManager;

        [Header("Player Levels")]
        public int playerLevel = 1;
        public int healthLevel = 10;
        public int enduranceLevel = 10;
        public int strengthLevel = 10;

        [Header("Health")]
        public int maxHealth;
        public int currentHealth;

        [Header("Stamina")]
        public float maxStamina;
        public float currentStamina;

        [Header("Player Bars")]
        public UI_StatBar healthbar;
        public StaminaBarManager staminabar;

        [Header("Suffering")] // zasób do ulepszania postaci (dusze z ds)
        public int suffering = 0;
        [SerializeField] TextMeshProUGUI sufferingText;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
            playerUIPopUpManager = GetComponent<PlayerUIPopUpManager>();
        }

        protected override void Update()
        {
            base.Update();
        }

        private void FixedUpdate()
        {
            if (currentStamina > 0)
            {
                player.outOfStamina = false;
            }

            // Regeneracja staminy
            if (!player.canRegenStamina)
            {
                ResetStaminaRegenCooldown();
            }
            else
            {
                staminaTimer += Time.deltaTime;
                if (staminaTimer >= staminaRegenCooldown)
                {
                    TakeStaminaDamage(staminaDamage);
                }
            }
        }

        void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthbar.SetMaxHealth(maxHealth);

            maxStamina = SetMaxStaminaFromEnduranceLevel();
            currentStamina = maxStamina;
            staminabar.SetMaxStamina(maxStamina);

            if (playerUIPopUpManager == null)
            {
                playerUIPopUpManager = FindFirstObjectByType<PlayerUIPopUpManager>();
                if (playerUIPopUpManager == null)
                {
                    Debug.LogError("PlayerUIPopUpManager could not be found in the scene!");
                }
            }
        }

        public void UpdateSufferingText(string newText)
        {
            sufferingText.text = newText;
        }

        public int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public float SetMaxStaminaFromEnduranceLevel()
        {
            maxStamina = enduranceLevel * 10;
            return maxStamina;
        }

        #region Taking Damage

        public void TakeDamage(int damage, Vector3 damageSource)
        {
            if (player.isDead)
                return;

            if (player.isRolling)
            {
                Debug.Log("Damage evaded");
                return;
            }

            currentHealth = currentHealth - damage;

            healthbar.SetCurrentHealth(currentHealth);

            if (currentHealth <= 0)
            {
                player.isDead = true;
                player.playerAnimatorManager.PlayerTargetActionAnimation("OH_Death_01", true, true);
                playerUIPopUpManager.SendYouDiedPopUp();
            }
            else
            {
                PlayDamageAnimation(damageSource);
            }
        }

        public void PlayDamageAnimation(Vector3 damageSource)
        {
            Vector3 direction = damageSource - transform.position;
            direction.y = 0;
            direction.Normalize();

            float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);

            if (angle >= -45 && angle <= 45)
            {
                player.playerAnimatorManager.PlayerTargetActionAnimation("Hit_Reaction_Medium_F_01", true, true);
            }
            else if (angle > 45 && angle <= 135)
            {
                player.playerAnimatorManager.PlayerTargetActionAnimation("Hit_Reaction_Medium_R_01", true, true);
            }
            else if (angle < -45 && angle >= -135)
            {
                player.playerAnimatorManager.PlayerTargetActionAnimation("Hit_Reaction_Medium_L_01", true, true);
            }
            else
            {
                player.playerAnimatorManager.PlayerTargetActionAnimation("Hit_Reaction_Medium_B_01", true, true);
            }
        }

        public void Heal(int healing)
        {
            currentHealth = currentHealth + healing;
            player.playerAttackManager.currentUses = player.playerAttackManager.currentUses - 1;

            healthbar.SetCurrentHealth(currentHealth);

            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }

        #endregion

        #region Taking Stamina Damage

        [HideInInspector] public float staminaRegenCooldown = 1f; // czas bezczynnoœci w sekundach
        [HideInInspector] private float staminaTimer = 0f;

        [HideInInspector] float staminaDamage = -0.5f; // wartoœæ regeneracji staminy

        public void TakeStaminaDamage(float staminaDamage)
        {
            currentStamina -= staminaDamage;

            if (currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }

            if (currentStamina < 0)
            {
                currentStamina = 0;
            }

            if (currentStamina <= 0)
            {
                player.outOfStamina = true;
                player.isSprinting = false;
            }

            staminabar.SetCurrentStamina(currentStamina);
        }

        private void ResetStaminaRegenCooldown()
        {
            staminaTimer = 0f;
        }

        #endregion
    }
}
