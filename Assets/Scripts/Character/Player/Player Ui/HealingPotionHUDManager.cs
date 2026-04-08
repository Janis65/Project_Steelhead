using UnityEngine;
using TMPro;

namespace JO
{
    public class HealingPotionHUDManager : MonoBehaviour
    {
        [SerializeField] public GameObject HealingPotionImageGameObject;
        [SerializeField] public GameObject EmptyPotionImageGameObject;

        [SerializeField] TextMeshProUGUI currentPotionUsesText;

        PlayerAttackManager player;

        private void Awake()
        {
            player = FindFirstObjectByType<PlayerAttackManager>();
        }

        private void Update()
        {
            UpdateCurrentPotionUsesText(player.currentUses.ToString());

            if (player.currentUses <= 0)
            {
                HealingPotionImageGameObject.SetActive(false);
                EmptyPotionImageGameObject.SetActive(true);
            }
            else
            {
                HealingPotionImageGameObject.SetActive(true);
                EmptyPotionImageGameObject.SetActive(false);
            }
        }

        public void UpdateCurrentPotionUsesText(string newText)
        {
            currentPotionUsesText.text = newText;
        }
    }
}
