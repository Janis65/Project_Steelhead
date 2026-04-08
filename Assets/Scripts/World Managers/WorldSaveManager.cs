using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JO
{
    public class WorldSaveManager : MonoBehaviour
    {
        public static WorldSaveManager instance;

        [SerializeField] int WorldSceneIndex = 1;

        private void Awake()
        {
            // There can only be one instance of this script at one time, if another exists, destroy it
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
        public IEnumerator LoadNewGame()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(WorldSceneIndex);

            yield return null;
        }
        public int GetWorldIdSceneIndex()
        {
            return WorldSceneIndex;
        }
    }
}
