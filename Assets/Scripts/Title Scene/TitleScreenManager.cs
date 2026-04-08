using System.Collections;
using UnityEngine;

namespace JO
{
    public class TitleScreenManager : MonoBehaviour
    {
        public GameObject MainMenu;
        public GameObject ControllsWindow;

        public void StartNewGame()
        {
            StartCoroutine(WorldSaveManager.instance.LoadNewGame());
        }

        public void ControllsMenu()
        {
            MainMenu.SetActive(false);
            ControllsWindow.SetActive(true);
        }

        public void CloseControllsMenu()
        {
            MainMenu.SetActive(true);
            ControllsWindow.SetActive(false);
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
