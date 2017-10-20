using UnityEngine.UI;
using UnityEngine;
using WaifuGO.GameManager;

namespace WaifuGo.UI
{
    public class MapSpritesHolder : MonoBehaviour
    {
        public Scenery[] sceneries;

        private Image background;

        private void Awake()
        {
            background = GetComponent<Image>();

            background.sprite = GetSpriteByName(WaifuManager.currentLocation);
        }

        private Sprite GetSpriteByName(string name)
        {
            foreach(Scenery s in sceneries)
            {
                if(s.name == name)
                    return s.sprite;
            }

            throw new System.Exception("The scenery choosen does not exist: " + name);
        }
    }

    [System.Serializable]
    public struct Scenery
    {
        public string name;
        public Sprite sprite;
    }
}
