using System;

namespace Dialogue
{
    [Serializable]
    public class DialogueEventData
    {
        public string id;
        public bool isAppeared;
        public int dialogueIndex;
    }
}