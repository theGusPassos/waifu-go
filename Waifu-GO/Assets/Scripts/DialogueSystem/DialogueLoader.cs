using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

namespace WaifuGO.DialogueSystem
{
    /// <summary>
    /// Reads a .xml file and parse its content into a DialogueNode struct
    /// </summary>
    public class DialogueLoader
    {
        private XmlDocument xmlDoc;

        public DialogueLoader()
        {
            xmlDoc = new XmlDocument();
            xmlDoc.Load(Application.dataPath + Path.DirectorySeparatorChar +
                "Database" + Path.DirectorySeparatorChar + "waifus-dialogue-tree.xml");

            //// for test
            //GetDialogueTree("tsundere");
            //UnitTest();
        }

        /* XML hierarchy
         * 
         * script waifu
         *      dialogue
         *          dialogue-id
         *          girl-speak
         *              text
         *              reaction
         *          option
         *              text
         *              next-node
         */
        public DialogueNode[] GetDialogueTree(string waifuName)
        {
            List<DialogueNode> loadedDialogue = new List<DialogueNode>();
            
            XmlNode waifuScriptNode = xmlDoc.GetElementById(waifuName);

            Debug.Log(waifuName);

            // one dialogue node at time
            foreach(XmlNode dialogueNode in waifuScriptNode.SelectNodes("dialogue"))
            {
                loadedDialogue.Add(LoadDialogueInNode(dialogueNode));
            }

            return loadedDialogue.ToArray();
        }

        public DialogueNode LoadDialogueInNode (XmlNode node)
        {
            DialogueNode dialogueNode = new DialogueNode();

            dialogueNode.id = int.Parse(node.SelectSingleNode("dialogue-id").InnerText);

            dialogueNode.girlSpeak = GetGirlSpeakInNode(
                node.SelectSingleNode("girl-speak")
                );

            dialogueNode.options = GetOptionsInNode(node);

            return dialogueNode;
        }

        private GirlSpeak GetGirlSpeakInNode(XmlNode node)
        {
            GirlSpeak girlSpeak = new GirlSpeak
            {
                line        = node.SelectSingleNode("text").InnerText,
                reaction    = node.SelectSingleNode("reaction").InnerText
            };

            return girlSpeak;
        }

        private Option[] GetOptionsInNode(XmlNode node)
        {
            List<Option> options = new List<Option>();

            foreach (XmlNode optionNode in node.SelectNodes("option"))
            {
                Option option = new Option();

                option.nextNode = int.Parse(optionNode.SelectSingleNode("next-node").InnerText);
                option.optionLine = optionNode.SelectSingleNode("text").InnerText;

                options.Add(option);
            }

            return options.ToArray();
        }

        /// <summary>
        /// Creates a .txt with every dialogue for each waifu
        /// </summary>
        private void UnitTest()
        {
            // TODO: store waifus names in other place
            string[] waifus = {"tsundere"};

            string fileName = Application.dataPath + Path.DirectorySeparatorChar +
                "Database" + Path.DirectorySeparatorChar + "unit_test.txt";

            var file = File.CreateText(fileName);

            foreach(string waifu in waifus)
            {
                DialogueNode[] dialogueTree = GetDialogueTree(waifu);

                foreach (DialogueNode node in dialogueTree)
                {
                    file.WriteLine("\nWaifu: ");
                    file.WriteLine("\tDialogue ID: " + node.id);

                    file.WriteLine("\tGirl-Speak");
                    file.WriteLine("\t\tLine: " + node.girlSpeak.line);
                    file.WriteLine("\t\tReaction: " + node.girlSpeak.reaction);

                    file.WriteLine("\tOptions");
                    foreach (Option option in node.options)
                    {
                        file.WriteLine("\t\tOption Line: " + option.optionLine);
                        file.WriteLine("\t\tNext Node: " + option.nextNode + "\n");
                    }
                }
            }

            file.Close();
        }
    }
    
    /// <summary>
    /// A dialogue node that defines the current state of the conversation
    /// </summary>
    public struct DialogueNode
    {
        public int          id;
        public GirlSpeak    girlSpeak;
        public Option[]     options;
    }

    public struct GirlSpeak
    {
        public string   line;
        public string   reaction;
    }

    /// <summary>
    /// A dialogue option that takes to another dialogue node
    /// </summary>
    public struct Option
    {
        // may have some other vars that defines if this was a good choice or not
        public string   optionLine;
        public int      nextNode;
    }
}