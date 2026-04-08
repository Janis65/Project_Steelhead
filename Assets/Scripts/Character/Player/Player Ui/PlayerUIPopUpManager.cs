using UnityEngine;
using TMPro;
using System.Collections;

namespace JO
{
    public class PlayerUIPopUpManager : MonoBehaviour
    {

        [Header("YOU DIED Pop Up")]
        [SerializeField] GameObject youDiedPopUpGameObject;
        [SerializeField] TextMeshProUGUI youDiedPopUpBackgroundText;
        [SerializeField] TextMeshProUGUI youDiedPopUpText;
        [SerializeField] CanvasGroup youDiedPopUpCanvasGroup;
        // Allows us to set the alpha to fade over time

        [Header("INTERACT Pop Up")]
        [SerializeField] GameObject interactPopUpGameObject;
        [SerializeField] TextMeshProUGUI interactPopUpText;
        [SerializeField] CanvasGroup interactPopUpCanvasGroup;

        #region Interact Pop Up

        public void SendYouInteractPopUp()
        {
            interactPopUpGameObject.SetActive(true);
        }

        public void CloseInteractPopUp()
        {
            interactPopUpGameObject.SetActive(false);
        }

        public void UpdateInteractionText(string newText)
        {
            if (interactPopUpText != null)
            {
                interactPopUpText.text = newText;
            }
            else
            {
                Debug.LogError("InteractionText nie zosta³ przypisany w Inspektorze!");
            }
        }

        #endregion

        #region You Died Pop Up

        public void SendYouDiedPopUp()
        {
            // ACTIVATE POST PROCESSING EFFECTS
            youDiedPopUpGameObject.SetActive(true);
            youDiedPopUpBackgroundText.characterSpacing = 0;
            StartCoroutine(StretchPopUpTextOverTime(youDiedPopUpBackgroundText, 8, 15));
            StartCoroutine(FadeInPopUpOverTime(youDiedPopUpCanvasGroup, 5));
            StartCoroutine(WaitThenFadeInPopUpOverTime(youDiedPopUpCanvasGroup, 2, 5));
        }

        #endregion

        private IEnumerator StretchPopUpTextOverTime(TextMeshProUGUI text, float duration, float stretchAmount)
        {
            if (duration > 0f)
            {
                text.characterSpacing = 0; // RESETS OUR CHARACTER SPACING
                float timer = 0;

                yield return null;

                while (timer < duration)
                {
                    timer = timer + Time.deltaTime;
                    text.characterSpacing = Mathf.Lerp(text.characterSpacing, stretchAmount, duration * (Time.deltaTime / 20));
                    yield return null;
                }
            }
        }

        private IEnumerator FadeInPopUpOverTime(CanvasGroup canvas, float duration)
        {
            if (duration > 0)
            {
                canvas.alpha = 0;
                float timer = 0;

                yield return null;

                while (timer < duration)
                {
                    timer = timer + Time.deltaTime;
                    canvas.alpha = Mathf.Lerp(canvas.alpha, 1, duration * Time.deltaTime);
                    yield return null;
                }
            }

            canvas.alpha = 1;

            yield return null;
        }

        private IEnumerator WaitThenFadeInPopUpOverTime(CanvasGroup canvas, float duration, float delay)
        {
            if (duration > 0)
            {
                while (delay > 0)
                {
                    delay = delay - Time.deltaTime;
                    yield return null;
                }

                canvas.alpha = 1;
                float timer = 0;

                yield return null;

                while (timer < duration)
                {
                    timer = timer + Time.deltaTime;
                    canvas.alpha = Mathf.Lerp(canvas.alpha, 0, duration * Time.deltaTime);
                    yield return null;
                }
            }

            canvas.alpha = 0;

            yield return null;
        }
    }
}
