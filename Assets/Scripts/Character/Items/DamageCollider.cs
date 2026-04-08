using UnityEngine;

namespace JO
{
    public class DamageCollider : MonoBehaviour
    {
        Collider damageCollider;

        public int damage = 10;

        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
        }

        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider collision)
        {
            CharacterManager characterManager = GetComponentInParent<CharacterManager>();

            PlayerManager player = GetComponentInParent<PlayerManager>();

            EnemyManager enemy = GetComponentInParent<EnemyManager>();

            if (collision.tag == "Player" && player == null)
            {
                PlayerStatsManager playerStats = collision.GetComponent<PlayerStatsManager>();

                EnemyStatsManager enemyStats = FindObjectOfType<EnemyStatsManager>();

                if (playerStats != null)
                {
                    if (characterManager.TH_Equiped)
                    {
                        playerStats.TakeDamage(damage + enemyStats.strengthLevel * 2, enemyStats.transform.position);
                    }
                    else
                    {
                        playerStats.TakeDamage(damage + enemyStats.strengthLevel, enemyStats.transform.position);
                    }
                }
            }

            if (collision.tag == "Enemy" && enemy == null)
            {
                EnemyStatsManager enemyStats = collision.GetComponent<EnemyStatsManager>();

                PlayerStatsManager playerStats = FindObjectOfType<PlayerStatsManager>();

                if (enemyStats != null)
                {
                    if (characterManager.TH_Equiped)
                    {
                        enemyStats.TakeDamage(damage + playerStats.strengthLevel * 2, playerStats.transform.position);
                    }
                    else
                    {
                        enemyStats.TakeDamage(damage + playerStats.strengthLevel, playerStats.transform.position);
                    }
                }
            }
        }
    }
}
