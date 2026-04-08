using UnityEngine;

namespace JO
{
    public class PlayerUIManager : MonoBehaviour
    {
        public static PlayerUIManager instance;

        public PlayerUIPopUpManager playerUIPopUpManager;


        private void Awake()
        {
            if (instance == null)
            { 
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            playerUIPopUpManager = GetComponent<PlayerUIPopUpManager>();
        }

    }
}
