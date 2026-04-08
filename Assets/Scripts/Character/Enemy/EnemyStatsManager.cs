using UnityEngine;

namespace JO
{
    public class EnemyStatsManager : CharacterStatsManager
    {
        EnemyManager enemy;

        public EnemyHealthBar enemyHealthBar;

        public int enemyLevel = 1;
        public int strengthLevel = 10;
        public int maxHealth;
        public int currentHealth;
        public int baseSufferingReward = 10;

        protected override void Awake()
        {
            base.Awake();

            enemy = GetComponent<EnemyManager>();
        }

        protected override void Update()
        {
            base.Update();
        }

        public int SetMaxHealthFromHealthLevel(int healthLevel)
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamage(int damage, Vector3 damageSource)
        {
            if (enemy.isDead) 
                return;

            currentHealth = currentHealth - damage;

            enemyHealthBar.SetCurrentHealth(currentHealth);

            if (currentHealth <= 0)
            {
                enemy.isDead = true;
                enemy.enemyAnimatorManager.EnemyTargetAttackAnimation("OH_Death_01", true, true);
                CalculateSufferingReward();

            }
            else
            {
                PlayDamageAnimation(damageSource);
            }
        }

        public void PlayDamageAnimation(Vector3 damageSource)
        {
            // Oblicz kierunek uderzenia
            Vector3 direction = damageSource - transform.position;
            direction.y = 0;
            direction.Normalize();

            // Oblicz k¹t miêdzy przodem gracza a kierunkiem uderzenia
            float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);

            // Wybierz odpowiedni¹ animacjê w zale¿noœci od k¹ta
            if (angle >= -45 && angle <= 45)
            {
                enemy.enemyAnimatorManager.EnemyTargetAttackAnimation("Hit_Reaction_Medium_F_01", true, true);
            }
            else if (angle > 45 && angle <= 135)
            {
                enemy.enemyAnimatorManager.EnemyTargetAttackAnimation("Hit_Reaction_Medium_R_01", true, true);
            }
            else if (angle < -45 && angle >= -135)
            {
                enemy.enemyAnimatorManager.EnemyTargetAttackAnimation("Hit_Reaction_Medium_L_01", true, true);
            }
            else
            {
                enemy.enemyAnimatorManager.EnemyTargetAttackAnimation("Hit_Reaction_Medium_B_01", true, true);
            }
        }

        private void CalculateSufferingReward()
        {
            int sufferingReward = baseSufferingReward + (enemyLevel * 3);

            PlayerManager player = FindFirstObjectByType<PlayerManager>();
            player.playerStatsManager.suffering += sufferingReward;

            player.playerStatsManager.UpdateSufferingText(player.playerStatsManager.suffering.ToString());
        }
    }
}
