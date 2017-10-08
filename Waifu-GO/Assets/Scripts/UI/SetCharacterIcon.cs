using UnityEngine;
using UnityEngine.UI;
using WaifuGO.GameManager;

namespace WaifuGO.UI
{
    public class SetCharacterIcon : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Image>().sprite = PlayerSettings.selectedCharacter.icon;
        }
    }
}
