using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Dialogue
{
    [Serializable]
    public class DialogueList
    {
        public UnityEvent onFinishEvent;
        public List<DialoguePiece> dialogueList;
    }
}