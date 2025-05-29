using System;
using UnityEngine;
using UnityEngine.Events;

namespace Dialogue
{
    [Serializable]
    public class DialoguePiece
    {
        public UnityEvent beforeTalkEvent;
        public Sprite figureSprite;
        public string name;
        [TextArea]
        public string dialogueText;

        // public bool hasToPause;
        [HideInInspector]public bool isDone;
        public UnityEvent afterTalkEvent;
    }
}
