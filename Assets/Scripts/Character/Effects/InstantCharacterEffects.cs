using UnityEngine;

namespace JO
{
    public class InstantCharacterEffects : ScriptableObject
    {
        [Header("Effect ID")]
        public int instantEffectID;

        public virtual void ProcessEffect(CharacterManager character)
        {

        }
    }
}
