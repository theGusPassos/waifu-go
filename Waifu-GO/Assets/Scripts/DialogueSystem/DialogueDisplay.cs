using UnityEngine;
using UnityEngine.UI;

namespace WaifuGO.DialogueSystem
{
    /// <summary>
    /// Shows the options according to the player's choice
    /// </summary>
    public class DialogueDisplay : MonoBehaviour
    {
        private DialogueLoader          loadDialogue;
        private ConversationFinisher    conversationFinisher;

        public GameObject[]             buttonsObj = new GameObject[3];
        public Text                     waifuLine;

        // text inside the button
        private Text[]                  options = new Text[3];
        private Button[]                buttons = new Button[3];

        private DialogueNode[]          currentDialogueTree;
        private int                     currentNode;

        private void Awake()
        {
            loadDialogue = new DialogueLoader();    
            conversationFinisher = GetComponent<ConversationFinisher>();

            for(int i = 0; i < buttonsObj.Length; i++)
            {
                options[i]  = buttonsObj[i].GetComponentInChildren<Text>();
                buttons[i]  = buttonsObj[i].GetComponent<Button>();
            }

            //fortest
            LoadFirstLine("tsundere");
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

        public void SetWaifuLine(int node)
        {
            waifuLine.text = currentDialogueTree[node].girlSpeak.line;
        }

        /// <summary>
        /// Choose an option passing the next conversation node
        /// </summary>
        /// <param name="nextNode"></param>
        public void ChooseOption(int nextNode)
        {
            Debug.Log("Option chosen: " + nextNode);

            currentNode = nextNode;
            
            if (currentDialogueTree[currentNode].options.Length > 0)
            {
                SetOptions(currentNode);
                SetWaifuLine(currentNode);
            }
            else
            {
                // the last waifu line has no options to choose from
                EndConversation();
            }
        }

        // TODO: end the conversation wisely
        public void EndConversation()
        {
            Debug.Log("ending conversation");

            SetButtonsEnabled(false);
            conversationFinisher.NotifyConversationEnd(
                currentDialogueTree[currentNode].girlSpeak.reaction
                );
        }
    }
}
