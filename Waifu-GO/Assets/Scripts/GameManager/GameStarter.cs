using UnityEngine;
using UnityEngine.SceneManagement;
using WaifuGO.DataPersistance;

namespace WaifuGO.GameManager
{
    public class GameStarter : MonoBehaviour
    {
        public string chooseCharSceneName;
        public string mapSceneName;

        public void StartGame()
        {
            if (ProgressLoader.HasProgressToLoad())
            {
                // loads the game progress
            }
            else
            {
                SceneManager.LoadScene(chooseCharSceneName);
            }
        }
    }
}