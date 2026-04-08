using UnityEngine;

namespace JO
{
    public class WeaponItem : Item
    {
        [Header("Weapon Model")]
        public GameObject weaponModel;

        [Header("Weapon Requirements")]
        public int strenghtREQ = 0;
        public int dexREQ = 0;
        public int intREQ = 0;
        public int faithREQ = 0;

        [Header("Weapon Base Damage")]
        public int physicalDamage = 0;
        public int magicDamage = 0;

        [Header("Weapon Base Poise Damage")]
        public float poiseDamage = 0;

        /*
        [Header("Stamina Costs")]
        public int baseStaminaCost = 20;
        */
    }
}
