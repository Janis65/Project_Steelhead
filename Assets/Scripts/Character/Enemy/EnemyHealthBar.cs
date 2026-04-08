using UnityEngine;
using UnityEngine.UI;

namespace JO
{
    public class EnemyHealthBar : MonoBehaviour
    {
        Slider slider;
        float timeUntilBarIsHidden = 0;

        private void Awake()
        {
            slider = GetComponentInChildren<Slider>();
        }

        public void SetCurrentHealth(int currentHealth)
        {
            slider.value = currentHealth;
            timeUntilBarIsHidden = 2;
        }

        public virtual void SetStat(int newValue)
        {
            slider.value = newValue;
        }

        public virtual void SetMaxHealth(int maxHealth)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
        }

        private void Update()
        {
            timeUntilBarIsHidden = timeUntilBarIsHidden - Time.deltaTime;

            if (slider != null)
            {
                if (timeUntilBarIsHidden <= 0)
                {
                    timeUntilBarIsHidden = 0;
                    slider.gameObject.SetActive(false);
                }
                else
                {
                    if (!slider.gameObject.activeInHierarchy)
                    {
                        slider.gameObject.SetActive(true);
                    }
                }
    
                if (slider.value <= 0)
                {
                    slider.gameObject.SetActive(false);
                    //Destroy(slider.gameObject);
                }
            }
        }
    }
}
