using System;
using System.Collections;
using DG.Tweening;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using EventHandler = Utilities.EventHandler;

namespace Dialogue
{
    public class DialogueUI : MonoBehaviour
    {
        public Image figure;
        public TextMeshProUGUI avatarName;
        public TextMeshProUGUI dialogueText;
        public GameObject dialogueBox;
        public GameObject continueSign;
        public GameObject playerHUD;
        private PlayerAttribute _playerAttr;

        private void Awake()
        {
            continueSign.SetActive(false);
            _playerAttr = GameObject.FindWithTag("Player").GetComponent<PlayerAttribute>();
        }

        private void OnEnable()
        {
            EventHandler.OnShowDialoguePiece += ShowDialogueEvent;
        }

        private void OnDisable()
        {
            EventHandler.OnShowDialoguePiece -= ShowDialogueEvent;
        }

        private void ShowDialogueEvent(DialoguePiece piece)
        {
            if (_playerAttr.isInCombat) return;
            StartCoroutine(ShowDialogue(piece));
        }

        private IEnumerator ShowDialogue(DialoguePiece piece)
        {
            if (piece != null)
            {
                piece.isDone = false;
                EventHandler.AllowInputControl(false);
                EventHandler.ChangeFOV(FovType.Narrative);
                playerHUD.SetActive(false);

                dialogueBox.SetActive(true);
                continueSign.SetActive(false);

                dialogueText.text = string.Empty;

                avatarName.gameObject.SetActive(piece.name != string.Empty);
                figure.gameObject.SetActive(piece.figureSprite != null);
                if (piece.name != string.Empty) avatarName.text = piece.name;
                yield return dialogueText.DOText(piece.dialogueText, 1f).WaitForCompletion();

                piece.isDone = true;
                
               continueSign.SetActive(piece.isDone); 
            }
            else
            {
               dialogueBox.SetActive(false);
               EventHandler.AllowInputControl(true);
               EventHandler.ChangeFOV(FovType.Normal);
               playerHUD.SetActive(true);
            }
        }
    }
}
