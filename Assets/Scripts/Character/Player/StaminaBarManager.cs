using UnityEngine;
using UnityEngine.UI;

namespace JO
{
    public class StaminaBarManager : MonoBehaviour
    {
        public Slider slider;
        public RectTransform barTransform; // RectTransform odpowiadaj¹cy za wizualizacjê paska

        [SerializeField] private float widthPerHealthUnit = 2f; // Dodatkowa szerokoœæ na jednostkê HP

        public void SetCurrentStamina(float currentStamina)
        {
            slider.value = currentStamina;
        }

        public virtual void SetStat(int newValue)
        {
            slider.value = newValue;
        }

        public virtual void SetMaxStamina(float maxStamina)
        {
            slider.maxValue = maxStamina;
            slider.value = maxStamina;

            // Aktualizacja szerokoœci paska
            UpdateBarWidth(maxStamina);
        }

        private void UpdateBarWidth(float maxStamina)
        {
            if (barTransform != null)
            {
                float newWidth = maxStamina * widthPerHealthUnit;
                barTransform.sizeDelta = new Vector2(newWidth, barTransform.sizeDelta.y);
            }
        }
    }
}
