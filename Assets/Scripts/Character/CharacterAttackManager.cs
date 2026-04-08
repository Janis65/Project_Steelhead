using UnityEngine;

namespace JO
{
    public class CharacterAttackManager : MonoBehaviour
    {

        CharacterManager character;

        [Header("Attack Target")]
        public CharacterManager currentTarget;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        protected virtual void Update()
        {
            
        }

        public virtual void SetTarget(CharacterManager newTarget)
        {
            if (newTarget != null)
            {
                currentTarget = newTarget;
            }
            else
            {
                currentTarget = null;
            }
        }
    }
}
