using UnityEngine;

namespace JO
{
    public class CharacterStatsManager : MonoBehaviour
    {
        CharacterManager character;
        Animator animator;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
            animator = GetComponent<Animator>();
        }

        protected virtual void Update()
        {

        }
    }
}