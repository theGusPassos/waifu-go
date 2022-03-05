using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace WaifuGO.GameManager
{
    public class CharacterSelection : MonoBehaviour
    {
        public GameObject playerSettings;

        public Character kouhai;
        public Character senpai;

        public void SelectCharacter(int selected)
        {
            Instantiate(playerSettings);

            if (selected == 0)
            {
                PlayerSettings.selectedCharacter = kouhai;
            }
            else if (selected == 1)
            {
                PlayerSettings.selectedCharacter = senpai;
            }

            SceneManager.LoadScene("WorldMap");
        }
    }

    [System.Serializable]
    public struct Character
    {
        public Sprite photo;
        public Sprite icon;
        public string name;
        public string description;
    }
}
