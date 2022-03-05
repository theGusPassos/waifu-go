using UnityEngine;
using WaifuGO.DialogueSystem;

namespace WaifuGO.GameManager
{
    public class WaifusImageHolder : MonoBehaviour
    {
        private static DialogueDisplay dialogueDisplay;

        [Tooltip("Tsundere, Kuudere, Yandere, Trap, Goddess")]
        public WaifuMood[] waifus;

        private void Awake()
        {
            if (dialogueDisplay == null)
                dialogueDisplay = GameObject.Find("Dialogue Canvas").GetComponent<DialogueDisplay>();

            dialogueDisplay.SetWaifuMoodSprites(GetWaifuByName(WaifuManager.currentWaifu));
        }

        private WaifuMood GetWaifuByName(string waifusName)
        {
            if("tsundere".Equals(waifusName))
                return waifus[0];
            if("kuudere".Equals(waifusName))
                return waifus[1];
            if("yandere".Equals(waifusName))
                return waifus[2];
            if("trap".Equals(waifusName))
                return waifus[4];
            if("goddess".Equals(waifusName))
                return waifus[3];

            throw new System.Exception("The waifus name is wrong");
        }
    }
}
