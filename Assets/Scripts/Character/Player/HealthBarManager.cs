using UnityEngine;
using UnityEngine.UI;

namespace JO
{
    public class UI_StatBar : MonoBehaviour
    {
        public Slider slider;
        public RectTransform barTransform; // RectTransform odpowiadaj¹cy za wizualizacjê paska

        [SerializeField] private float widthPerHealthUnit = 2f; // Dodatkowa szerokoœæ na jednostkê HP

        public void SetCurrentHealth(int currentHealth)
        {
            slider.value = currentHealth;
        }

        public virtual void SetStat(int newValue)
        {
            slider.value = newValue;
        }

        public virtual void SetMaxHealth(int maxHealth)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;

            // Aktualizacja szerokoœci paska
            UpdateBarWidth(maxHealth);
        }

        private void UpdateBarWidth(int maxHealth)
        {
            if (barTransform != null)
            {
                float newWidth = maxHealth * widthPerHealthUnit;
                barTransform.sizeDelta = new Vector2(newWidth, barTransform.sizeDelta.y);
            }
        }
    }
}