using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WaifuGO.GameManager;
using WaifuGO.Sound;

namespace WaifuGO.DialogueSystem
{
    /// <summary>
    /// Shows the options according to the player's choice
    /// </summary>
    public class DialogueDisplay : MonoBehaviour
    {
        private DialogueLoader          loadDialogue;

        public Image                    waifuImage;
        private WaifuMood               waifuMoodSprites;

        public GameObject[]             buttonsObj = new GameObject[3];
        public Text                     waifuLine;

        // text inside the button
        private Text[]                  options = new Text[3];
        private Button[]                buttons = new Button[3];

        private DialogueNode[]          currentDialogueTree;
        private int                     currentNode;

        public GameObject               restartButton;

        private void Awake()
        {
            loadDialogue = new DialogueLoader();    

            for(int i = 0; i < buttonsObj.Length; i++)
            {
                options[i]  = buttonsObj[i].GetComponentInChildren<Text>();
                buttons[i]  = buttonsObj[i].GetComponent<Button>();
            }

            LoadFirstLine(WaifuManager.currentWaifu);
        }

        public void SetButtonsEnabled(bool enabled)
        {
            foreach(GameObject g in buttonsObj)
            {
                g.SetActive(enabled);
            }
        }

        /// <summary>
        /// Called when the waifu appears
        /// </summary>
        public void LoadFirstLine(string waifu)
        {
            if (waifu == "goddess")
            {
                waifuImage.GetComponent<RectTransform>().sizeDelta = new Vector2(1167, 1373);
            }
            else if (waifu == "trap")
            {
                waifuImage.GetComponent<RectTransform>().sizeDelta = new Vector2(957, 1305);
            }
            else
            {
                waifuImage.GetComponent<RectTransform>().sizeDelta = new Vector2(800, 1280);
            }

            currentDialogueTree = loadDialogue.GetDialogueTree(waifu);
            currentNode = 0;
            SetButtonsEnabled(true);

            SetOptions(currentNode);
            SetWaifuLine(currentNode);
        }

        /// <summary>
        /// Sets the next conversation node and text for each option
        /// </summary>
        /// <param name="node"></param>
        public void SetOptions(int node)
        {
            int i = 0;
            foreach (Option option in currentDialogueTree[node].options)
            {
                options[i].text = option.optionLine;
                buttons[i].onClick.RemoveAllListeners();
                buttons[i].onClick.AddListener( () => ChooseOption(option.nextNode) );              

                i++;
            }
        }

        public void SetWaifuMoodSprites(WaifuMood sprites)
        {
            waifuMoodSprites = sprites;
        }

        public void SetWaifuLine(int node)
        {
            waifuLine.text = currentDialogueTree[node].girlSpeak.line;
            waifuImage.sprite = GetSpriteByMood(currentDialogueTree[node].girlSpeak.reaction);
        }
        
        private Sprite GetSpriteByMood(string mood)
        {
            if ("neutral".Equals(mood))
                return waifuMoodSprites.neutral;
            if ("shy".Equals(mood))
                return waifuMoodSprites.shy;
            if ("madshy".Equals(mood))
                return waifuMoodSprites.madShy;
            if ("bored".Equals(mood))
                return waifuMoodSprites.bored;

            throw new Exception("The mood you typed in the xml doesn't exist: " + mood);
        }

        /// <summary>
        /// Choose an option passing the next conversation node
        /// </summary>
        /// <param name="nextNode"></param>
        public void ChooseOption(int nextNode)
        {
            EventSystem.current.SetSelectedGameObject(null);

            currentNode = nextNode;
            
            if (currentDialogueTree[currentNode].options.Length > 0)
            {
                SetOptions(currentNode);
                SetWaifuLine(currentNode);
            }
            else
            {
                // the last waifu line has no options to choose from
                SetWaifuLine(currentNode);
                EndConversation();
            }
        }

        // TODO: end the conversation wisely
        public void EndConversation()
        {
            restartButton.SetActive(true);

            SetButtonsEnabled(false);
            //conversationFinisher.NotifyConversationEnd(
            //    currentDialogueTree[currentNode].girlSpeak.reaction
            //    );
        }

        public void ReturnToMap()
        {
            SoundManager.instance.StartMusic();
            SceneManager.LoadScene("WorldMap");
        }
    }

    [Serializable]
    public struct WaifuMood
    {
        public Sprite neutral;
        public Sprite bored;
        public Sprite happy;
        public Sprite shy;
        public Sprite madShy;
        public Sprite mad;
        public Sprite inLove;
    }
}
