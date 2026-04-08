using UnityEngine;

namespace JO
{
    public class DamagePlayer : MonoBehaviour
    {
        public int damage = 25;
        private void OnTriggerEnter(Collider other)
        {
            PlayerStatsManager playerStats = other.GetComponent<PlayerStatsManager>();

            if (playerStats != null) 
            {
                playerStats.TakeDamage(damage, transform.position);
            }
        }
    }
}
