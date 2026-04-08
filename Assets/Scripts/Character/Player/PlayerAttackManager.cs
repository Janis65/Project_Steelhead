using UnityEngine;

namespace JO
{
    public class PlayerAttackManager : CharacterAttackManager
    {
        PlayerManager player;

        public string lastAttack;
        private float staminaDamage = 1;

        // Healing Potion
        [SerializeField] GameObject HealingPotion;
        [SerializeField] GameObject EmptyPotion;

        [Header("Healing Potion Stats")]
        public int healing = 40;
        public int maxUses = 5;
        public int currentUses;
        public int healingUpgrade = 0;
        public int usesUpgrade = 0;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        protected override void Update()
        {
            base.Update();
        }

        private void Start()
        {
            currentUses = maxUses;  // Ustaw u¿ycia mikstury na liczbê maksymalnych u¿yæ
        }

        #region Attacks

        public void HandleLightAttack()
        {
            if (player.isPerforminAction)
                return;

            if (player.outOfStamina)
                return;

            player.playerStatsManager.TakeStaminaDamage(staminaDamage * 35);

            if (player.isGrounded && player.TH_Equiped)
            {
                player.isAttacking = true;
                player.canRegenStamina = false;
                player.playerAnimatorManager.PlayerTargetAttackAnimation("TH_Light_Attack_01", true, true);
                lastAttack = "TH_Light_Attack_01";
            }
            else if (player.isGrounded)
            {
                player.isAttacking = true;
                player.canRegenStamina = false;
                player.playerAnimatorManager.PlayerTargetAttackAnimation("OH_Light_Attack_01", true, true);
                lastAttack = "OH_Light_Attack_01";
            }
        }

        public void HandleLightCombo()
        {
            if (player.comboFlag)
            {
                if (player.outOfStamina)
                    return;

                player.animator.SetBool("canDoCombo", false);

                if (player.TH_Equiped)
                {
                    player.playerStatsManager.TakeStaminaDamage(staminaDamage * 35);

                    if (lastAttack == "TH_Light_Attack_01")
                    {
                        player.isAttacking = true;
                        player.canRegenStamina = false;
                        player.playerAnimatorManager.PlayerTargetAttackAnimation("TH_Light_Attack_02", true, true);
                        lastAttack = "TH_Light_Attack_02";
                    }
                    else if (lastAttack == "TH_Light_Attack_02")
                    {
                        player.isAttacking = true;
                        player.canRegenStamina = false;
                        player.playerAnimatorManager.PlayerTargetAttackAnimation("TH_Light_Attack_03", true, true);
                        lastAttack = "TH_Light_Attack_03";
                    }
                    else if (lastAttack == "TH_Heavy_Attack_01")
                    {
                        player.isAttacking = true;
                        player.canRegenStamina = false;
                        player.playerAnimatorManager.PlayerTargetAttackAnimation("TH_Light_Attack_02", true, true);
                        lastAttack = "TH_Light_Attack_02";
                    }
                }
                else
                {
                    player.playerStatsManager.TakeStaminaDamage(staminaDamage * 30);

                    if (lastAttack == "OH_Light_Attack_01")
                    {
                        player.isAttacking = true;
                        player.canRegenStamina = false;
                        player.playerAnimatorManager.PlayerTargetAttackAnimation("OH_Light_Attack_02", true, true);
                        lastAttack = "OH_Light_Attack_02";
                    }
                    else if (lastAttack == "OH_Light_Attack_02")
                    {
                        player.isAttacking = true;
                        player.canRegenStamina = false;
                        player.playerAnimatorManager.PlayerTargetAttackAnimation("OH_Light_Attack_01", true, true);
                        lastAttack = "OH_Light_Attack_01";
                    }
                    else if (lastAttack == "OH_Heavy_Attack_01")
                    {
                        player.isAttacking = true;
                        player.canRegenStamina = false;
                        player.playerAnimatorManager.PlayerTargetAttackAnimation("OH_Light_Attack_02", true, true);
                        lastAttack = "OH_Light_Attack_02";
                    }
                    else if (lastAttack == "OH_Heavy_Attack_02")
                    {
                        player.isAttacking = true;
                        player.canRegenStamina = false;
                        player.playerAnimatorManager.PlayerTargetAttackAnimation("OH_Light_Attack_01", true, true);
                        lastAttack = "OH_Light_Attack_01";
                    }
                }
            }
        }

        public void HandleHeavyAttack()
        {
            if (player.isPerforminAction)
                return;

            if (player.outOfStamina)
                return;

            player.playerStatsManager.TakeStaminaDamage(staminaDamage * 50);

            if (player.isGrounded && player.TH_Equiped)
            {
                player.isAttacking = true;
                player.canRegenStamina = false;
                player.playerAnimatorManager.PlayerTargetAttackAnimation("TH_Heavy_Attack_01", true, true);
                lastAttack = "TH_Heavy_Attack_01";
            }
            else if (player.isGrounded)
            {
                player.isAttacking = true;
                player.canRegenStamina = false;
                player.playerAnimatorManager.PlayerTargetAttackAnimation("OH_Heavy_Attack_01", true, true);
                lastAttack = "OH_Heavy_Attack_01";
            }
        }

        public void HandleHeavyCombo()
        {
            if (player.comboFlag)
            {
                if (player.outOfStamina)
                    return;

                player.animator.SetBool("canDoCombo", false);

                player.playerStatsManager.TakeStaminaDamage(staminaDamage * 50);

                if (player.TH_Equiped)
                {
                    if (lastAttack == "TH_Light_Attack_01")
                    {
                        player.isAttacking = true;
                        player.canRegenStamina = false;
                        player.playerAnimatorManager.PlayerTargetAttackAnimation("TH_Heavy_Attack_02", true, true);
                        lastAttack = "TH_Heavy_Attack_02";
                    }
                    else if (lastAttack == "TH_Light_Attack_02")
                    {
                        player.isAttacking = true;
                        player.canRegenStamina = false;
                        player.playerAnimatorManager.PlayerTargetAttackAnimation("TH_Heavy_Attack_01", true, true);
                        lastAttack = "TH_Heavy_Attack_01";
                    }
                    else if (lastAttack == "TH_Heavy_Attack_01")
                    {
                        player.isAttacking = true;
                        player.canRegenStamina = false;
                        player.playerAnimatorManager.PlayerTargetAttackAnimation("TH_Heavy_Attack_02", true, true);
                        lastAttack = "TH_Heavy_Attack_02";
                    }
                }
                else
                {
                    if (lastAttack == "OH_Light_Attack_01")
                    {
                        player.isAttacking = true;
                        player.canRegenStamina = false;
                        player.playerAnimatorManager.PlayerTargetAttackAnimation("OH_Heavy_Attack_02", true, true);
                        lastAttack = "OH_Heavy_Attack_02";
                    }
                    else if (lastAttack == "OH_Light_Attack_02")
                    {
                        player.isAttacking = true;
                        player.canRegenStamina = false;
                        player.playerAnimatorManager.PlayerTargetAttackAnimation("OH_Heavy_Attack_01", true, true);
                        lastAttack = "OH_Heavy_Attack_01";
                    }
                    else if (lastAttack == "OH_Heavy_Attack_01")
                    {
                        player.isAttacking = true;
                        player.canRegenStamina = false;
                        player.playerAnimatorManager.PlayerTargetAttackAnimation("OH_Heavy_Attack_02", true, true);
                        lastAttack = "OH_Heavy_Attack_02";
                    }
                    else if (lastAttack == "OH_Heavy_Attack_02")
                    {
                        player.isAttacking = true;
                        player.canRegenStamina = false;
                        player.playerAnimatorManager.PlayerTargetAttackAnimation("OH_Heavy_Attack_01", true, true);
                        lastAttack = "OH_Heavy_Attack_01";
                    }
                }
            }
        }

        #endregion

        #region Healing

        public void AttemptToHeal()
        {
            if (player.isPerforminAction)
                return;

            if (currentUses > 0)
            {
                HideRightWeapon();
                HealingPotion.SetActive(true);
                player.playerAnimatorManager.PlayerTargetActionAnimation("Potion_Drink", true, true);
            }

            if (currentUses <= 0)
            {
                HideRightWeapon();
                EmptyPotion.SetActive(true);
                player.playerAnimatorManager.PlayerTargetActionAnimation("Potion_Empty", true, true);
            }
        }

        public void HandleHealCombo()
        {
            if (player.drinkFlag)
            {
                if (currentUses <= 0)
                {
                    HideRightWeapon();
                    EmptyPotion.SetActive(true);
                    player.playerAnimatorManager.PlayerTargetActionAnimation("Potion_Empty", true, true);
                }

                if (currentUses > 0)
                {
                    HideRightWeapon();
                    HealingPotion.SetActive(true);
                    player.playerAnimatorManager.PlayerTargetActionAnimation("Potion_Drink_Next", true, true);
                }
            }
        }

        public void HealPlayer()
        {
            player.playerStatsManager.Heal(healing);
        }

        #endregion

        #region Hide Objects During Actions

        public void HidePotion()
        {
            HealingPotion.SetActive(false);
            EmptyPotion.SetActive(false);
        }

        public void HideRightWeapon()
        {
            if (player.playerEquipmentManager.rightHandWeaponModel != null)
                player.playerEquipmentManager.rightHandWeaponModel.SetActive(false);
        }

        public void ShowRightWeapon()
        {
            player.playerEquipmentManager.rightHandWeaponModel.SetActive(true);
        }

        #endregion
    }
}
