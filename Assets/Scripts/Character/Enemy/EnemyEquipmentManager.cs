using UnityEngine;

namespace JO
{
    public class EnemyEquipmentManager : CharacterEquipmentManager
    {
        EnemyManager enemy;
        Animator animator;

        public WeaponModelInstantiationSlot rightHandSlot;
        public WeaponModelInstantiationSlot leftHandSlot;

        public GameObject rightHandWeaponModel;
        public GameObject leftHandWeaponModel;

        public DamageCollider rightHandDamageCollider;
        public DamageCollider leftHandDamageCollider;

        protected override void Awake()
        {
            base.Awake();

            enemy = GetComponent<EnemyManager>();
            animator = GetComponentInChildren<Animator>();

            InitializeWeaponSlots();
        }

        protected override void Start()
        {
            base.Start();

            LoadRightWeapon();
            //LoadWeaponsOnBothHands();
        }

        private void InitializeWeaponSlots()
        {
            WeaponModelInstantiationSlot[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();

            foreach (var weaponSlot in weaponSlots)
            {
                if (weaponSlot.weaponSlot == WeaponModelSlot.RightHand)
                {
                    rightHandSlot = weaponSlot;
                }
                else if (weaponSlot.weaponSlot == WeaponModelSlot.LeftHand)
                {
                    leftHandSlot = weaponSlot;
                }
            }
        }

        private void LoadWeaponsOnBothHands()
        {
            LoadRightWeapon();
            LoadLeftWeapon();
        }

        public void LoadRightWeapon()
        {

            if (enemy.enemyInventoryManager.currentRightHandWeapon != null)
            {
                rightHandWeaponModel = Instantiate(enemy.enemyInventoryManager.currentRightHandWeapon.weaponModel);
                rightHandSlot.LoadWeapon(rightHandWeaponModel);
                LoadRightWeaponDamageCollider();
            }
        }

        public void LoadLeftWeapon()
        {
            if (enemy.enemyInventoryManager.currentLeftHandWeapon != null)
            {
                leftHandWeaponModel = Instantiate(enemy.enemyInventoryManager.currentLeftHandWeapon.weaponModel);
                leftHandSlot.LoadWeapon(leftHandWeaponModel);
                LoadLeftWeaponDamageCollider();
            }
        }

        private void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        private void LoadLeftWeaponDamageCollider()
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }


        public void OpenLeftDamageCollider()
        {
            leftHandDamageCollider.EnableDamageCollider();
        }

        public void CloseLeftDamageCollider()
        {
            leftHandDamageCollider.DisableDamageCollider();
        }
    }
}
