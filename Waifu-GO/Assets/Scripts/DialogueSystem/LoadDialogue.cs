///
/// @author GusPassos
/// 

namespace WaifuGO.DialogueSystem
{
    /// <summary>
    /// Reads a .xml file and parse its content into a DialogueNode struct
    /// </summary>
    public class LoadDialogue
    {   
        
    }
    
    /// <summary>
    /// A dialogue node that defines the current state of the conversation
    /// </summary>
    public struct DialogueNode
    {
        public string       girlLine;
        public Option[]     options;
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